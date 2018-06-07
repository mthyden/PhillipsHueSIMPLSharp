using System;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace HueLights
{
    public class HueRoom
    {
        public ushort RoomId;
        public ushort GroupIsOn;
        public string GroupName;
	    public string RoomClass;
        public ushort RoomBri;
        public ushort RoomHue;
        public ushort RoomSat;
	    public ushort RoomCt;
        public ushort RoomXVal;
        public ushort RoomYVal;
        public ushort SceneNum;
        public ushort RoomOnline;
        public string[] SceneName = new string[20];
        public string[] SceneId = new string[20];

        //^^^^^ Signals for SIMPL+ ^^^^^^^^

	    private bool _foundroom;
	    private string _url;
	    private JObject _json;
	    private string _jsontext;
	    private bool _supportsColor;

        public event EventHandler RoomBriUpdate;

        public event EventHandler RoomHueUpdate;

        public event EventHandler RoomSatUpdate;

        public event EventHandler RoomOnOffUpdate;

        public event EventHandler RoomUpdate;

	    public event EventHandler RoomOnlineUpdate;

        public HueRoom()
        {

        }

	    public void RoomInit()
	    {
			RoomOnline = 0;
		    RoomId = 0;
		    GroupIsOn = 0;
		    RoomClass = "";
		    RoomBri = 0;
		    RoomHue = 0;
		    RoomSat = 0;
		    RoomCt = 0;
		    RoomXVal = 0;
		    RoomYVal = 0;
		    SceneNum = 0;
		    if (HueBridge.Populated == true)
		    {
			    _foundroom = false;
			    foreach (var huegroup in HueBridge.HueGroups)
			    {
				    if (huegroup.Name == GroupName)
				    {
					    RoomId = Convert.ToUInt16(huegroup.Id);
					    _foundroom = true;
					    break;
				    }	
				}
			    if (_foundroom)
			    {
					RoomClass = HueBridge.HueGroups[RoomId - 1].GroupClass;
					for (int i = 1; i <= 20; i++)
					{
						if (HueBridge.HueGroups[RoomId - 1].SceneName[i] != null)
						{
							SceneName[i] = HueBridge.HueGroups[RoomId - 1].SceneName[i];
							SceneId[i] = HueBridge.HueGroups[RoomId - 1].SceneID[i];
						}
						else
						{
							int x;
							x = i - 1;
							SceneNum = (ushort)x;
							break;
						}
					}
					RoomOnline = 1;
					CrestronConsole.PrintLine("Get {0} is complete", GroupName);
					TriggerRoomOnlineUpdate(); 
			    }
			    else
			    {
					CrestronConsole.PrintLine("Room not found: {0}", GroupName);
			    }
		    }
	    }

	    public void GetRoom()
        {
            try
            {
				if (_foundroom == true)
				{
					_url = string.Format("http://{0}/api/{1}/{2}/{3}", HueBridge.BridgeIp, HueBridge.BridgeApi, "groups", RoomId);
					_jsontext = HttpConnect.Instance.Request(_url, null, Crestron.SimplSharp.Net.Http.RequestType.Get);
					_json = JObject.Parse(_jsontext);

					if (_json["action"].SelectToken("on") != null)
					{
						HueBridge.HueGroups[RoomId - 1].On = (bool)_json["action"]["on"];
						GroupIsOn = (ushort)(HueBridge.HueGroups[RoomId - 1].On ? 1 : 0);
					}
					if (_json["action"].SelectToken("bri") != null)
					{
						HueBridge.HueGroups[RoomId - 1].Bri = (ushort)_json["action"]["bri"];
						RoomBri = (ushort)(HueBridge.HueGroups[RoomId - 1].Bri);
					}		
					if (_json["action"].SelectToken("colormode") != null)
					{
						_supportsColor = true;
					}					
					if (_supportsColor)
					{
						if (_json["action"].SelectToken("hue") != null)
						{
							HueBridge.HueGroups[RoomId - 1].Hue = (uint)_json["action"]["hue"];
							RoomHue = (ushort)(HueBridge.HueGroups[RoomId - 1].Hue);
						}
						if (_json["action"].SelectToken("sat") != null)
						{
							HueBridge.HueGroups[RoomId - 1].Sat = (uint)_json["action"]["sat"];
							RoomSat = (ushort)(HueBridge.HueGroups[RoomId - 1].Sat);
						}
						if (_json["action"].SelectToken("ct") != null)
						{
							HueBridge.HueGroups[RoomId - 1].Ct = (uint)_json["action"]["ct"];
							RoomCt = (ushort)(HueBridge.HueGroups[RoomId - 1].Ct);
						}
					}
				}
				else
				{
					CrestronConsole.PrintLine("Error getting Room data: {0}", GroupName);
				}
				TriggerRoomUpdate();
            }
            catch (Exception e)
            {
				CrestronConsole.PrintLine("Error getting Room data: {0}", e);
            }
        }

	    /// <summary>
	    /// Sets a group to be on/off/scene select
	    /// </summary>
	    /// <param name="action">"true", "false", "scene"</param>
	    /// <param name="lvltype"></param>
	    /// <param name="val"></param>
	    /// <param name="effect"></param>
	    public void GroupAction(string lvltype, string val, string effect)
        {
            try
            {
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    Payload payload = new Payload(){SetType = "groups", LvlType = lvltype, OnOff = val, Effect = effect};
                    string json = HueBridge.SetCmd(PayloadType.RoomOnOff, payload, RoomId);
                    JArray JReturn = JArray.Parse(json);
                    string tokenreturn = "/groups/" + RoomId + "/action/on";
                    foreach (var Jobj in JReturn)
                    {
                        var myaction = Jobj["success"];
                        string whodidwhat = myaction.ToString();
                        if (whodidwhat.Contains(tokenreturn))
                        {
                            HueBridge.HueGroups[RoomId - 1].On = (bool)myaction[tokenreturn];
                            GroupIsOn = (ushort)(HueBridge.HueGroups[RoomId - 1].On ? 1 : 0);
                            TriggerRoomOnOffUpdate();
                        }
                    }
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

        /// <summary>
        /// test
        /// </summary>
        /// <param name="settype"></param>
        /// <param name="lvltype"></param>
        /// <param name="val"></param>
        public void LightsVal(string lvltype, ushort val)
        {
            try
            {
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    var payload = new Payload() { SetType = "groups", Lvl = val, LvlType = lvltype };
                    var json = HueBridge.SetCmd(PayloadType.Lvl, payload, RoomId);
					
                    if (json.Contains("success"))
                    {
                        var jData = JArray.Parse(json);
                        var nodeVal = "/" + payload.SetType + "/" + RoomId + "/" + payload.CmdType + "/" + lvltype;
                        switch (lvltype)
                        {
                            case "bri":
                                {
                                    HueBridge.HueGroups[RoomId - 1].Bri = (uint)jData[0]["success"][nodeVal];
                                    RoomBri = (ushort)HueBridge.HueGroups[RoomId - 1].Bri;
                                    TriggerRoomBriUpdate();
                                    break;
                                }
                            case "hue":
                                {
                                    HueBridge.HueGroups[RoomId - 1].Hue = (uint)jData[0]["success"][nodeVal];
                                    RoomHue = (ushort)HueBridge.HueGroups[RoomId - 1].Hue;
                                    TriggerRoomHueUpdate();
                                    break;
                                }
                            case "sat":
                                {
                                    HueBridge.HueGroups[RoomId - 1].Sat = (uint)jData[0]["success"][nodeVal];
                                    RoomSat = (ushort)HueBridge.HueGroups[RoomId - 1].Sat;
                                    TriggerRoomSatUpdate();
                                    break;
                                }
                            default:
                                break;
                        }
                    }
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

        public void RecallScene(ushort i)
        {
            try
            {
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    var payload = new Payload(){SetType = "groups", Scene = this.SceneId[i]};
                    string json = HueBridge.SetCmd(PayloadType.Scene, payload, RoomId);
                    //CrestronConsole.PrintLine("json response: {0}",json);
                    if (json.Contains("success"))
                    {
                        CrestronConsole.PrintLine("Scene changed");
                    }
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

        public void XYVal(ushort xval, ushort yval)
        {
            try
            {
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    var payload = new Payload() { SetType = "groups", Xval = xval, Yval = yval };
                    string json = HueBridge.SetCmd(PayloadType.XY, payload, RoomId);
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void TriggerRoomBriUpdate()
        {
            RoomBriUpdate(this, new EventArgs());
        }

        public void TriggerRoomHueUpdate()
        {
            RoomHueUpdate(this, new EventArgs());
        }

        public void TriggerRoomSatUpdate()
        {
            RoomSatUpdate(this, new EventArgs());
        }

        public void TriggerRoomOnOffUpdate()
        {
            RoomOnOffUpdate(this, new EventArgs());
        }

        public void TriggerRoomUpdate()
        {
            RoomUpdate(this, new EventArgs());
        }

	    public void TriggerRoomOnlineUpdate()
	    {
		    RoomOnlineUpdate(this, new EventArgs());
	    }
    }
}