using System;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace HueLights
{
    public class HueRoom
    {
        public ushort RoomID;
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
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    RoomOnline = 0;
                    foreach (var huegroup in HueBridge.HueGroups)
                    {
                        if (huegroup.RoomName == GroupName)
                        {
                            RoomID = huegroup.RoomID;
                            RoomOnline = 1;
                            TriggerRoomOnlineUpdate();
                            RoomPopulate();
                            break;
                        }
                    }
                }
                else
                {
                    RoomOnline = 0;
                    CrestronConsole.PrintLine("Bridge not authorized");
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
            GroupIsOn = (ushort)(HueBridge.HueGroups[RoomID - 1].On ? 1 : 0);
            RoomBri = (ushort)HueBridge.HueGroups[RoomID - 1].Bri;
            RoomHue = (ushort)HueBridge.HueGroups[RoomID - 1].Hue;
            RoomSat = (ushort)HueBridge.HueGroups[RoomID - 1].Sat;
            for (int i = 1; i <= 20; i++)
            {
                if (HueBridge.HueGroups[RoomID - 1].SceneName[i] != null)
                {
                    SceneName[i] = HueBridge.HueGroups[RoomID - 1].SceneName[i];
                    SceneId[i] = HueBridge.HueGroups[RoomID - 1].SceneID[i];
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
        public void GroupAction(string actiontype, string actioncmd, string effect)
        {
            try
            {
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    String json = HueBridge.SetOnOff("groups", RoomID, actioncmd, "action", effect);
                    JArray JReturn = JArray.Parse(json);
                    string tokenreturn = "/groups/" + RoomID + "/action/" + actiontype;
                    foreach (var Jobj in JReturn)
                    {
                        var myaction = Jobj["success"];
                        string whodidwhat = myaction.ToString();
                        if (whodidwhat.Contains(tokenreturn))
                        {
                            HueBridge.HueGroups[RoomID - 1].On = (bool)myaction[tokenreturn];
                            GroupIsOn = (ushort)(HueBridge.HueGroups[RoomID - 1].On ? 1 : 0);
                            TriggerRoomOnOffUpdate();
                        }
                    }

                    /*
                    for (int i = 0; i < JReturn.Count; i++)
                    {
                        if (json.Contains("success"))
                        {
                            var JData = JReturn[i].SelectToken("success");
                            //string tokenreturn = "/groups/" + RoomID + "/action/" + actiontype;
                            string tokenreturn = "/groups/" + RoomID + "/action/on";
                            if (JData.Contains(tokenreturn))
                            {

                                break;
                            }
                        }
                    }*/
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
                    String payload = String.Format("{0}\"scene\":\"{1}\"{2}", '{', SceneId[i], '}');
                    String json = HueBridge.SetScene(RoomID, payload);
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

        /// <summary>
        /// test
        /// </summary>
        /// <param name="settype"></param>
        /// <param name="lvltype"></param>
        /// <param name="val"></param>
        public void LightsVal(string settype, string lvltype, ushort val)
        {
            try
            {
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    String cmdval = "{\"" + lvltype + "\":" + val.ToString() + "}";
                    String json = HueBridge.SetLvl(settype, RoomID, "action", cmdval);
                    if (json.Contains("success"))
                    {
                        JArray JData = JArray.Parse(json);
                        string NodeVal = "/" + settype + "/" + RoomID + "/action/" + lvltype;
                        switch (lvltype)
                        {
                            case "bri":
                                {
                                    HueBridge.HueGroups[RoomID - 1].Bri = (uint)JData[0]["success"][NodeVal];
                                    RoomBri = (ushort)HueBridge.HueGroups[RoomID - 1].Bri;
                                    TriggerRoomBriUpdate();
                                    break;
                                }
                            case "hue":
                                {
                                    HueBridge.HueGroups[RoomID - 1].Hue = (uint)JData[0]["success"][NodeVal];
                                    RoomHue = (ushort)HueBridge.HueGroups[RoomID - 1].Hue;
                                    TriggerRoomHueUpdate();
                                    break;
                                }
                            case "sat":
                                {
                                    HueBridge.HueGroups[RoomID - 1].Sat = (uint)JData[0]["success"][NodeVal];
                                    RoomSat = (ushort)HueBridge.HueGroups[RoomID - 1].Sat;
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

        public void XYVal(string settype, ushort xval, ushort yval)
        {
            try
            {
                if (HueBridge.Authorized == true && HueBridge.Populated == true)
                {
                    decimal x = (decimal)xval / 100;
                    decimal y = (decimal)yval / 100;
                    String cmdval = "{\"xy\":[" + x.ToString() + "," + y.ToString() + "]}";
                    String json = HueBridge.SetLvl(settype, RoomID, "action", cmdval);
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }
    }
}