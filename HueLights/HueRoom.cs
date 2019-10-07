using System;
using System.Collections.Generic;
using System.Linq;
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
	    private HueGroup _room;

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
			    foreach (KeyValuePair<string, HueGroup> entry in HueBridge.HueGroups)
			    {
				    if (entry.Value.Name == GroupName)
				    {
						RoomId = Convert.ToUInt16(entry.Key);
						_foundroom = true;
					    _room = entry.Value;
						break;
				    }
			    }

			    if (_foundroom)
			    {
					RoomClass = _room.GroupClass;
					for (int i = 0; i < _room.ScenesNum; i++)
					{
						SceneName[i+1] = _room.Scenes[i].Name;
						SceneId[i + 1] = _room.Scenes[i].SceneId;
					}
					SceneNum = (ushort)_room.ScenesNum;
					//CrestronConsole.PrintLine("scenenum: {0}",SceneNum);
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
						_room.On = (bool)_json["action"]["on"];
						GroupIsOn = (ushort)(_room.On ? 1 : 0);
					}
					if (_json["action"].SelectToken("bri") != null)
					{
						_room.Bri = (ushort)_json["action"]["bri"];
						RoomBri = (ushort)(_room.Bri);
					}		
					if (_json["action"].SelectToken("colormode") != null)
					{
						_supportsColor = true;
					}					
					if (_supportsColor)
					{
						if (_json["action"].SelectToken("hue") != null)
						{
							_room.Hue = (uint)_json["action"]["hue"];
							RoomHue = (ushort)(_room.Hue);
						}
						if (_json["action"].SelectToken("sat") != null)
						{
							_room.Sat = (uint)_json["action"]["sat"];
							RoomSat = (ushort)(_room.Sat);
						}
						if (_json["action"].SelectToken("ct") != null)
						{
							_room.Ct = (uint)_json["action"]["ct"];
							RoomCt = (ushort)(_room.Ct);
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
                            _room.On = (bool)myaction[tokenreturn];
                            GroupIsOn = (ushort)(_room.On ? 1 : 0);
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
                                    _room.Bri = (uint)jData[0]["success"][nodeVal];
                                    RoomBri = (ushort)_room.Bri;
                                    TriggerRoomBriUpdate();
                                    break;
                                }
                            case "hue":
                                {
									_room.Hue = (uint)jData[0]["success"][nodeVal];
									RoomHue = (ushort)_room.Hue;
                                    TriggerRoomHueUpdate();
                                    break;
                                }
                            case "sat":
                                {
									_room.Sat = (uint)jData[0]["success"][nodeVal];
									RoomSat = (ushort)_room.Sat;
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