using System;
using Crestron.SimplSharp;
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
        public ushort BulbOnline;
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
                if (HueBridge.Populated == true)
                {
                    BulbOnline = 0;
                    foreach (var huebulb in HueBridge.HueBulbs)
                    {
                        if (huebulb.Name == BulbName)
                        {
                            BulbId = Convert.ToUInt16(huebulb.Id);
                            BulbOnline = 1;
                            TriggerBulbOnlineUpdate();
                            BulbPopulate();
                        }
                    }
                }

            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        private void BulbPopulate()
        {
            BulbName = (String)HueBridge.HueBulbs[BulbId - 1].Name;
            BulbIsOn = (ushort)(HueBridge.HueBulbs[BulbId - 1].On ? 1 : 0);
            BulbType = (String)HueBridge.HueBulbs[BulbId - 1].Type;
            BulbBri = (ushort)HueBridge.HueBulbs[BulbId - 1].Bri;
            Reachable = (ushort)(HueBridge.HueBulbs[BulbId - 1].Reachable ? 1 : 0);
            if (HueBridge.HueBulbs[BulbId].Type.Contains("Color"))
            {
                BulbHue = (ushort)(HueBridge.HueBulbs[BulbId - 1].Hue);
                BulbSat = (ushort)(HueBridge.HueBulbs[BulbId - 1].Sat);
            }
        }

        public void LightsAction(string lvltype, string val, string effect)
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    Payload payload = new Payload() { SetType = "lights", LvlType = lvltype, OnOff = val, Effect = effect };
                    string json = HueBridge.SetCmd(PayloadType.OnOff, payload, BulbId);
                    JArray JReturn = JArray.Parse(json);
                    string tokenreturn = "/lights/" + BulbId + "/state/on";
                    foreach (var Jobj in JReturn)
                    {
                        var myaction = Jobj["success"];
                        string whodidwhat = myaction.ToString();
                        if (whodidwhat.Contains(tokenreturn))
                        {
                            HueBridge.HueBulbs[BulbId - 1].On = (bool)myaction[tokenreturn];
                            BulbIsOn = (ushort)(HueBridge.HueBulbs[BulbId - 1].On ? 1 : 0);
                            TriggerBulbOnOffUpdate();
                        }
                    }
                }
                else
                {
                    CrestronConsole.PrintLine("Error with Bulb Action {0}", BulbId);
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
                    Payload payload = new Payload() { SetType = "lights", Lvl = val, LvlType = lvltype };
                    string json = HueBridge.SetCmd(PayloadType.Lvl, payload, BulbId);
                    if (json.Contains("success"))
                    {
                        JArray JData = JArray.Parse(json);
                        string NodeVal = "/" + payload.SetType + "/" + BulbId + "/state/" + lvltype;
                        HueBridge.HueBulbs[BulbId - 1].Bri = (uint)JData[0]["success"][NodeVal];
                        switch (lvltype)
                        {
                            case "bri":
                                {
                                    BulbBri = (ushort)HueBridge.HueBulbs[BulbId - 1].Bri;
                                    TriggerBulbBriUpdate();
                                    break;
                                }
                            case "hue":
                                {
                                    BulbHue = (ushort)HueBridge.HueBulbs[BulbId - 1].Hue;
                                    TriggerBulbHueUpdate();
                                    break;
                                }
                            case "sat":
                                {
                                    BulbSat = (ushort)HueBridge.HueBulbs[BulbId - 1].Sat;
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
