using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Newtonsoft.Json;
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
        public string[] SceneName = new string[20];
        public string[] SceneID = new string[20];

        //^^^^^ Signals for SIMPL+ ^^^^^^^^

        public HueRoom()
        {

        }

        public void GetRoom()
        {
            try
            {
                GroupName = HueBridge.HueGroups[RoomID - 1].RoomName;
                GroupIsOn = (ushort)(HueBridge.HueGroups[RoomID - 1].On ? 1 : 0);
                RoomBri = (ushort)HueBridge.HueGroups[RoomID - 1].Bri;
                RoomHue = (ushort)HueBridge.HueGroups[RoomID - 1].Hue;
                RoomSat = (ushort)HueBridge.HueGroups[RoomID - 1].Sat;
                for (int i = 1; i <= 20; i++)
                {
                    SceneName[i] = HueBridge.HueGroups[RoomID - 1].SceneName[i];
                    SceneID[i] = HueBridge.HueGroups[RoomID - 1].SceneID[i];
                }
                CrestronConsole.PrintLine("Get Room{0} is complete", RoomID);
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void GroupOn()
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    String json = HueBridge.SetOnOff("groups", RoomID, "true", "action");
                    if (json.Contains("success"))
                    {
                        HueBridge.HueGroups[RoomID - 1].On = true;
                        GroupIsOn = 1;
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

        public void GroupOff()
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    String json = HueBridge.SetOnOff("groups", RoomID, "false", "action");
                    if (json.Contains("success"))
                    {
                        HueBridge.HueGroups[RoomID - 1].On = false;
                        GroupIsOn = 0;
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
                if (HueBridge.Authorized == true)
                {
                    String payload = String.Format("{0}\"scene\":\"{1}\"{2}", '{', SceneID[i], '}');
                    String json = HueBridge.SetScene(RoomID, payload);
                    if (json.Contains("success"))
                    {
                        CrestronConsole.PrintLine("Successfully changed scenes");
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

        public void LightsVal(string settype, string lvltype, ushort val)
        {
            try
            {
                if (HueBridge.Authorized == true)
                {

                    String json = HueBridge.SetLvl(settype, lvltype, RoomID, val, "action");
                    if (json.Contains("success"))
                    {
                        JArray JData = JArray.Parse(json);
                        string NodeVal = "/" + settype + "/" + RoomID + "/action/" + lvltype;
                        switch (lvltype)
                        {
                            case "bri":
                                {
                                    HueBridge.HueGroups[RoomID - 1].Bri = (uint)JData[0]["success"][NodeVal];
                                    RoomBri = (ushort)HueBridge.HueBulbs[RoomID - 1].Bri;
                                    break;
                                }
                            case "hue":
                                {
                                    HueBridge.HueGroups[RoomID - 1].Hue = (uint)JData[0]["success"][NodeVal];
                                    RoomHue = (ushort)HueBridge.HueBulbs[RoomID - 1].Hue;
                                    break;
                                }
                            case "sat":
                                {
                                    HueBridge.HueGroups[RoomID - 1].Sat = (uint)JData[0]["success"][NodeVal];
                                    RoomSat = (ushort)HueBridge.HueBulbs[RoomID - 1].Sat;
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
    }
}