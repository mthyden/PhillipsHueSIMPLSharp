using System;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace HueLights
{
    public class HueRoom
    {
        public ushort RoomId;
        public ushort GroupIsOn;
        public String GroupName;
        public ushort RoomBri;
        public ushort RoomHue;
        public ushort RoomSat;
        public ushort RoomXVal;
        public ushort RoomYVal;
        public ushort SceneNum;
        public ushort RoomOnline;
        public string[] SceneName = new string[20];
        public string[] SceneId = new string[20];

        //^^^^^ Signals for SIMPL+ ^^^^^^^^

        public event EventHandler RoomBriUpdate;

        public event EventHandler RoomHueUpdate;

        public event EventHandler RoomSatUpdate;

        public event EventHandler RoomOnOffUpdate;

        public event EventHandler RoomOnlineUpdate;

        public HueRoom()
        {

        }

        public void GetRoom()
        {
            try
            {
                if (HueBridge.Populated == true)
                {
                    RoomOnline = 0;
                    foreach (var huegroup in HueBridge.HueGroups)
                    {
                        if (huegroup.Name == GroupName)
                        {
                            RoomId = Convert.ToUInt16(huegroup.Id);
                            RoomOnline = 1;
                            TriggerRoomOnlineUpdate();
                            RoomPopulate();
                            break;
                        }
                    }
                    if (RoomOnline == 0)
                    {
                        CrestronConsole.PrintLine("Room not found");   
                    }
                }
                else
                {
                    RoomOnline = 0;
                    CrestronConsole.PrintLine("Error getting {0} data", GroupName);
                    TriggerRoomOnlineUpdate();
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        private void RoomPopulate()
        {
            GroupIsOn = (ushort)(HueBridge.HueGroups[RoomId - 1].On ? 1 : 0);
            RoomBri = (ushort)HueBridge.HueGroups[RoomId - 1].Bri;
            RoomHue = (ushort)HueBridge.HueGroups[RoomId - 1].Hue;
            RoomSat = (ushort)HueBridge.HueGroups[RoomId - 1].Sat;
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
            CrestronConsole.PrintLine("Get {0} is complete", GroupName);
        }

        /// <summary>
        /// Sets a group to be on/off/scene select
        /// </summary>
        /// <param name="action">"true", "false", "scene"</param>
        public void GroupAction(string lvltype, string val, string effect)
        {
            try
            {
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    Payload payload = new Payload(){SetType = "groups", LvlType = lvltype, OnOff = val, Effect = effect};
                    string json = HueBridge.SetCmd(PayloadType.OnOff, payload, RoomId);
                    //String json = HueBridge.SetOnOff("groups", RoomId, actioncmd, "action", effect);
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
                    Payload payload = new Payload() { SetType = "groups", Lvl = val, LvlType = lvltype };
                    string json = HueBridge.SetCmd(PayloadType.Lvl, payload, RoomId);
                    if (json.Contains("success"))
                    {
                        JArray JData = JArray.Parse(json);
                        string NodeVal = "/" + payload.SetType + "/" + RoomId + payload.CmdType + lvltype;
                        switch (lvltype)
                        {
                            case "bri":
                                {
                                    HueBridge.HueGroups[RoomId - 1].Bri = (uint)JData[0]["success"][NodeVal];
                                    RoomBri = (ushort)HueBridge.HueGroups[RoomId - 1].Bri;
                                    TriggerRoomBriUpdate();
                                    break;
                                }
                            case "hue":
                                {
                                    HueBridge.HueGroups[RoomId - 1].Hue = (uint)JData[0]["success"][NodeVal];
                                    RoomHue = (ushort)HueBridge.HueGroups[RoomId - 1].Hue;
                                    TriggerRoomHueUpdate();
                                    break;
                                }
                            case "sat":
                                {
                                    HueBridge.HueGroups[RoomId - 1].Sat = (uint)JData[0]["success"][NodeVal];
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
                    Payload payload = new Payload(){SetType = "groups", Scene = this.SceneId[i]};
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
                    Payload payload = new Payload() { SetType = "groups", Xval = xval, Yval = yval };
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

        public void TriggerRoomOnlineUpdate()
        {
            RoomOnlineUpdate(this, new EventArgs());
        }
    }
}