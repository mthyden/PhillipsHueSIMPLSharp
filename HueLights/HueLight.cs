using System;
using System.Collections.Generic;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json.Linq;

namespace HueLights
{
    public class HueLight
    {
        public ushort BulbId;
        public String BulbName;
        public ushort BulbIsOn;
        public String BulbType;
        public ushort BulbBri;
        public ushort BulbHue;
        public ushort BulbSat;
	    public ushort BulbCt;
        public ushort BulbOnline;
        public ushort Reachable;
	    public ushort BulbColor;

        //^^^^^ Signals for SIMPL+ ^^^^^^^^

	    private bool _foundBulb;
	    private string _url;
	    private JObject _json;
	    private string _jsontext;
	    private bool _supportsColor;
	    private HueBulb _bulb;

        public event EventHandler BulbBriUpdate;
        public event EventHandler BulbHueUpdate;
        public event EventHandler BulbSatUpdate;
	    public event EventHandler BulbCtUpdate;
        public event EventHandler BulbOnlineUpdate;
	    public event EventHandler BulbUpdate;
        public event EventHandler BulbOnOffUpdate;

        public HueLight()
        {

        }

	    public void BulbInit()
	    {
		    BulbOnline = 0;
		    BulbIsOn = 0;
		    BulbType = "";
		    BulbBri = 0;
		    BulbHue = 0;
		    BulbCt = 0;
		    Reachable = 0;
		    BulbColor = 0;
			if (HueBridge.Populated == true)
			{
				BulbOnline = 0;
				foreach (KeyValuePair<string, HueBulb> entry in HueBridge.HueBulbs)
				{
					if (entry.Value.Name == BulbName)
					{
						BulbId = Convert.ToUInt16(entry.Key);
						_foundBulb = true;
						_bulb = entry.Value;
						break;
					}
				}
				if (_foundBulb == true)
				{
					BulbName = (String)_bulb.Name;
					BulbIsOn = (ushort)(_bulb.On ? 1 : 0);
					BulbType = (String)_bulb.Type;
					Reachable = (ushort)(_bulb.Reachable ? 1 : 0);
					BulbBri = (ushort)_bulb.Bri;
					if (_bulb.Type.Contains("Color"))
					{
						BulbHue = (ushort)(_bulb.Hue);
						BulbSat = (ushort)(_bulb.Sat);
					}
					BulbOnline = 1;
					CrestronConsole.PrintLine("Get {0} is complete", BulbName);
					_bulb.HueUpdated += _bulbUpdated;
					_bulb.Online = true;
					TriggerBulbOnlineUpdate();
				}
				else
				{
					CrestronConsole.PrintLine("Bulb not found: {0}", BulbName);
				}
			}
	    }

        public void GetBulb()
        {
	        try
	        {
		        if (_foundBulb == true)
		        {
			        _url = string.Format("http://{0}/api/{1}/{2}/{3}", HueBridge.BridgeIp, HueBridge.BridgeApi, "lights", BulbId);
					//CrestronConsole.PrintLine("url: {0}", _url);
			        _jsontext = HttpConnect.Instance.RequestInfo(_url, null, RequestType.Get);
			        _json = JObject.Parse(_jsontext);

			        if (_json["state"].SelectToken("reachable") != null)
			        {
						_bulb.Reachable = (bool)_json["state"]["reachable"];
						Reachable = (ushort)(_bulb.Reachable ? 1 : 0);
			        }			
			        if (_json["state"].SelectToken("on") != null)
			        {
						_bulb.On = (bool)_json["state"]["on"];
						BulbIsOn = (ushort)(_bulb.On ? 1 : 0);
			        }	
			        if (_json["state"].SelectToken("bri") != null)
			        {
						_bulb.Bri = (ushort)_json["state"]["bri"];
						BulbBri = (ushort)(_bulb.Bri);
			        }
					if (_json["state"].SelectToken("colormode") != null)
					{
						_bulb.ColorMode = (string)_json["state"]["colormode"];
						_supportsColor = true;
					}
					if (_json["state"].SelectToken("ct") != null)
					{
						_bulb.Ct = (ushort)_json["state"]["ct"];
						BulbCt = (ushort)(_bulb.Ct);
					}
			        if (_supportsColor)
			        {
				        if (_json["state"].SelectToken("hue") != null)
				        {
							_bulb.Hue = (uint)_json["state"]["hue"];
							BulbHue = (ushort)(_bulb.Hue); 
				        }
				        if (_json["state"].SelectToken("sat") != null)
				        {
							_bulb.Sat = (uint)_json["state"]["sat"];
							BulbSat = (ushort)(_bulb.Sat);
				        }	
			        }
					BulbColor = (ushort)(_supportsColor ? 1 : 0);
		        }
		        else
		        {
			        CrestronConsole.PrintLine("Error getting bulb data: {0}", BulbName);
		        }
		        TriggerBulbUpdate();
	        }
	        catch (Exception e)
	        {
		        CrestronConsole.PrintLine("Exception is {0}", e);
	        }
        }

        public void LightsAction(string lvltype, string val, string effect)
        {
            try
            {
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    Payload payload = new Payload() { SetType = "lights", LvlType = lvltype, OnOff = val, Effect = effect };
                    HueBridge.SetCmd(PayloadType.BulbOnOff, payload, BulbId);
                }
                else
                {
                    CrestronConsole.PrintLine("Error with Bulb Action {0}", BulbName);
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void LightsVal(string lvltype, ushort val)
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    var payload = new Payload() { SetType = "lights", Lvl = val, LvlType = lvltype };
                    HueBridge.SetCmd(PayloadType.Lvl, payload, BulbId);
                }
                else
                {
                    CrestronConsole.PrintLine("Bridge not authorized");
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

		void _bulbUpdated(object sender, HueInstanceEventArgs e)
		{
			switch (e.HueEventId)
			{
				case HueEventId.Bri:
					{
						BulbBri = (ushort)_bulb.Bri;
						TriggerBulbBriUpdate();
						break;
					}
				case HueEventId.Hue:
					{
						BulbHue = (ushort)_bulb.Hue;
						TriggerBulbHueUpdate();
						break;
					}
				case HueEventId.Sat:
					{
						BulbSat = (ushort)_bulb.Sat;
						TriggerBulbSatUpdate();
						break;
					}
				case HueEventId.Ct:
					{
						BulbCt = (ushort)_bulb.Ct;
						TriggerBulbCtUpdate();
						break;
					}
				case HueEventId.On:
					{
						BulbIsOn = (ushort)(_bulb.On ? 1 : 0);
						TriggerBulbOnOffUpdate();
						break;
					}
			}
		}

        public void TriggerBulbBriUpdate()
        {
            BulbBriUpdate(this, new EventArgs());
        }

        public void TriggerBulbHueUpdate()
        {
            BulbHueUpdate(this, new EventArgs());
        }

        public void TriggerBulbSatUpdate()
        {
            BulbSatUpdate(this, new EventArgs());
        }

		public void TriggerBulbCtUpdate()
		{
			BulbCtUpdate(this, new EventArgs());
		}

        public void TriggerBulbOnOffUpdate()
        {
            BulbOnOffUpdate(this, new EventArgs());
        }

	    public void TriggerBulbUpdate()
	    {
		    BulbUpdate(this, new EventArgs());
	    }

        public void TriggerBulbOnlineUpdate()
        {
            BulbOnlineUpdate(this, new EventArgs());
        }
    }
}
