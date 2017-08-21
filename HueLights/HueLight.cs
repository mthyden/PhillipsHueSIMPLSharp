using System;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace HueLights
{
    public class HueLight
    {
        public ushort BulbID;
        public String BulbName;
        public ushort BulbIsOn;
        public String BulbType;
        public ushort BulbBri;
        public ushort BulbHue;
        public ushort BulbSat;
        public ushort Reachable;

        //^^^^^ Signals for SIMPL+ ^^^^^^^^

        public event EventHandler BulbBriUpdate;

        public event EventHandler BulbHueUpdate;

        public event EventHandler BulbSatUpdate;

        public event EventHandler BulbOnlineUpdate;

        public event EventHandler BulbOnOffUpdate;

        public HueLight()
        {

        }

        public void GetBulb()
        {
            try
            {
                BulbName = (String)HueBridge.HueBulbs[BulbID - 1].Name;
                BulbIsOn = (ushort)(HueBridge.HueBulbs[BulbID - 1].On ? 1 : 0);
                BulbType = (String)HueBridge.HueBulbs[BulbID - 1].Type;
                BulbBri = (ushort)HueBridge.HueBulbs[BulbID - 1].Bri;
                Reachable = (ushort)(HueBridge.HueBulbs[BulbID - 1].Reachable ? 1 : 0);
                if (HueBridge.HueBulbs[BulbID].Type.Contains("Color"))
                {
                    BulbHue = (ushort)(HueBridge.HueBulbs[BulbID - 1].Hue);
                    BulbSat = (ushort)(HueBridge.HueBulbs[BulbID - 1].Sat);
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
                    String cmdval = "{\"" + lvltype + "\":" + val.ToString() + "}";
                    String json = HueBridge.SetLvl(settype, BulbID, "state", cmdval);
                    if (json.Contains("success"))
                    {
                        JArray JData = JArray.Parse(json);
                        string NodeVal = "/" + settype + "/" + BulbID + "/state/" + lvltype;
                        HueBridge.HueBulbs[BulbID - 1].Bri = (uint)JData[0]["success"][NodeVal];
                        switch (lvltype)
                        {
                            case "bri":
                                {
                                    BulbBri = (ushort)HueBridge.HueBulbs[BulbID - 1].Bri;
                                    TriggerBulbBriUpdate();
                                    break;
                                }
                            case "hue":
                                {
                                    BulbHue = (ushort)HueBridge.HueBulbs[BulbID - 1].Hue;
                                    TriggerBulbHueUpdate();
                                    break;
                                }
                            case "sat":
                                {
                                    BulbSat = (ushort)HueBridge.HueBulbs[BulbID - 1].Sat;
                                    TriggerBulbSatUpdate();
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

        public void LightsAction(string actiontype, string actioncmd, string effect)
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    String json = HueBridge.SetOnOff("lights", BulbID, "true", "state", "none");
                    if (json.Contains("success"))
                    {
                        HueBridge.HueBulbs[BulbID - 1].On = true;
                        BulbIsOn = 1;
                        TriggerBulbOnOffUpdate();
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

        public void TriggerBulbOnOffUpdate()
        {
            BulbOnOffUpdate(this, new EventArgs());
        }

        public void TriggerBulbOnlineUpdate()
        {
            BulbOnlineUpdate(this, new EventArgs());
        }
    }
}
