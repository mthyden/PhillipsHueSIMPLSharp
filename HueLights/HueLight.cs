using System;
using System.Text;
using Crestron.SimplSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Crestron.SimplSharp.Net.Http;
using System.Collections.Generic;

namespace HueLights
{
    public class HueLight
    {

        public delegate ushort DelegateValueUpdate();

        public String IPAddress;    //IP Address for a Hue Bridge
        public ushort Authorized;   //Reports if the API key has been acquired
        public String APIKey;       //API Key used to control Hue devices (stored in CrestronDataStore

        public String[] BulbName = new String[50];
        public ushort[] BulbIsOn = new ushort[50];
        public String[] BulbType = new String[50];
        public ushort[] BulbBri = new ushort[50];
        public ushort[] BulbHue = new ushort[50];
        public ushort[] BulbSat = new ushort[50];
        public ushort[] Reachable = new ushort[50];
        public ushort BulbNum;
        
        public String[] GroupName = new String[25]; //Room names
        public ushort[] GroupVal = new ushort[25]; //ability to adjust brightness for a room
        public ushort[] GroupIsOn = new ushort[25]; //reports if a room is on
        public ushort GroupNum; //

        public List<HueBulb> HueBulbs = new List<HueBulb>();
        public List<HueGroup> HueGroups = new List<HueGroup>();
        public List<HueScene> HueScenes = new List<HueScene>();

        public DelegateValueUpdate ValueUpdate {get; set;}

        public HueLight()
        {

        }

        public void Register()
        {
            try
            {
                HueBridge.SetupDataStore();
                string tempapi;
                tempapi = HueBridge.GetDataStore();
                CrestronConsole.PrintLine("api is {0}", tempapi);
                if (tempapi != null)
                {
                    HueBridge.BridgeApi = tempapi;
                    HueBridge.Authorized = 1;
                    Authorized = HueBridge.Authorized;
                }
                else
                {
                    HueBridge.register();
                    Authorized = HueBridge.Authorized;
                }
                APIKey = HueBridge.BridgeApi;
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void getIP()
        {
            IPAddress = HueBridge.getIP();
            HueBridge.Authorized = 0;
            Authorized = HueBridge.Authorized;
        }

        public void getBulbs()
        {
            try
            {
                string json = HueBridge.GetBridgeInfo("lights");
                JObject JData = JObject.Parse(json);
                HueBulbs.Clear();
                for (int i = 1; i <= JData.Count; i++)
                {
                    bool on = (bool)JData[i.ToString()]["state"]["on"];
                    uint bri = (uint)JData[i.ToString()]["state"]["bri"];
                    string alert = (string)JData[i.ToString()]["state"]["alert"];
                    bool reachable = (bool)JData[i.ToString()]["state"]["reachable"];
                    string type = (string)JData[i.ToString()]["type"];
                    string name = (string)JData[i.ToString()]["name"];
                    string model = (string)JData[i.ToString()]["modelid"];
                    string manufacturer = (string)JData[i.ToString()]["manufacturername"];
                    string uid = (string)JData[i.ToString()]["uniqueid"];
                    string swver = (string)JData[i.ToString()]["swversion"];
                    if (type.Contains("color"))
                    {
                        uint hue = (uint)JData[i.ToString()]["state"]["hue"];
                        uint sat = (uint)JData[i.ToString()]["state"]["sat"];
                        HueBulbs.Add(new HueBulb(on, bri, hue, sat, alert, reachable, type, name, model, manufacturer, uid, swver));
                        BulbHue[i] = (ushort)(HueBulbs[i - 1].Hue);
                        BulbSat[i] = (ushort)(HueBulbs[i - 1].Sat);

                    }
                    else
                    {
                        HueBulbs.Add(new HueBulb(on, bri, alert, reachable, type, name, model, manufacturer, uid, swver));
                    }
                    
                    
                    BulbName[i] = HueBulbs[i-1].Name;
                    BulbIsOn[i] = (ushort)(HueBulbs[i-1].On ? 1 : 0);
                    BulbType[i] = HueBulbs[i-1].Type;
                    BulbBri[i] = (ushort)HueBulbs[i-1].Bri;
                    Reachable[i] = (ushort)(HueBulbs[i - 1].Reachable ? 1 : 0);
                }
                BulbNum = (ushort)HueBulbs.Count;
                CrestronConsole.PrintLine("Get Bulbs is complete...");
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void getRooms()
        {
            try
            {
                string json = HueBridge.GetBridgeInfo("groups");
                JObject JData = JObject.Parse(json);
                HueGroups.Clear();
                for (int i = 1; i <= JData.Count; i++)
                {
                    string name = (string)JData[i.ToString()]["name"];
                    string load = (string)JData[i.ToString()]["lights"][0];
                    string type = (string)JData[i.ToString()]["type"];
                    bool on = (bool)JData[i.ToString()]["action"]["on"];
                    uint bri = (uint)JData[i.ToString()]["action"]["bri"];
                    string alert = (string)JData[i.ToString()]["action"]["alert"];
                    HueGroups.Add(new HueGroup(name, type, on, bri, alert, load));
                    GroupName[i] = HueGroups[i-1].RoomName;
                    GroupIsOn[i] = (ushort)(HueGroups[i-1].On ? 1 : 0);
                    GroupVal[i] = (ushort)HueGroups[i-1].Bri;
                    Array.Clear(HueGroups[i - 1].SceneName, 0, 5);
                    Array.Clear(HueGroups[i - 1].SceneID, 0, 5);
                }
                GroupNum = (ushort)HueGroups.Count;
                CrestronConsole.PrintLine("Get Rooms is complete...");
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void getScenes()
        {
            try
            {
                string json = HueBridge.GetBridgeInfo("scenes");
                JObject JData = JObject.Parse(json);
                string JText = JData.ToString();
                HueScenes.Clear();
                for (int i = 1; i <= JData.Count; i++)
                {
                    int pos1 = JText.IndexOf("\"");
                    string ID = JText.Substring(pos1 + 1, 15);
                    pos1 = JText.IndexOf("\"", 31);
                    int pos2 = JText.IndexOf(",", 31);
                    string name = JText.Substring(pos1 + 1, ((pos2 - 1) - (pos1 + 1)));
                    pos1 = JText.IndexOf("lights");
                    pos1 = pos1 + 17;
                    pos2 = JText.IndexOf("\"", pos1);
                    string load = JText.Substring(pos1, (pos2 - pos1));
                    HueScenes.Add(new HueScene(ID, name, load));
                    for (int x = 0; x < HueGroups.Count; x++)
                    {
                        CrestronConsole.PrintLine("x = {0}",x);
                        if (HueScenes[x].AssignedLoad == HueGroups[x].AssignedLoad)
                        {
                            for (int y = 0; y < 5; y++)
                            {
                                if (HueGroups[x].SceneName[y] == null)
                                {
                                    HueGroups[x].SceneName[y] = HueScenes[x-1].Name;
                                    HueGroups[x].SceneID[y] = HueScenes[x-1].ID;
                                    CrestronConsole.PrintLine("{0}, {1}",HueGroups[x].SceneID[y],HueGroups[x].SceneName[y]);
                                    break;
                                }
                            }
                            CrestronConsole.PrintLine("Load matched");
                        }
                    }
                    int pos3 = JText.IndexOf("lastupdated");
                    int pos4 = JText.IndexOf("version", pos3);
                    int pos5 = JText.IndexOf("}",pos4);
                    JText = JText.Remove(0, pos5);
                }

            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting scenes: {0}",e);
            }

        }

        public void LightsVal(ushort val, ushort lightnum, string settype, string lvltype)
        {
            try
            {
                String json = HueBridge.SetBri(settype, lvltype, lightnum, val);
                if (json.Contains("success"))
                {
                    JArray JData = JArray.Parse(json);
                    string NodeVal = "/" + settype + "/" + lightnum + "/state/" + lvltype;
                    HueBulbs[lightnum - 1].Bri = (uint)JData[0]["success"][NodeVal];
                    switch (lvltype)
                    {
                        case "bri":
                            {
                                BulbBri[lightnum] = (ushort)HueBulbs[lightnum - 1].Bri;
                                break;
                            }
                        case "hue":
                            {
                                BulbHue[lightnum] = (ushort)HueBulbs[lightnum - 1].Hue;
                                break;
                            }
                        case "sat":
                            {
                                BulbSat[lightnum] = (ushort)HueBulbs[lightnum - 1].Sat;
                                break;
                            }
                        default:
                            break;
                    }
                    
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void LightsOn(ushort i)
        {
            try
            {
                String json = HueBridge.SetLights("lights", i, "true", "state");
                if (json.Contains("success"))
                {
                    HueBulbs[i-1].On = true;
                    BulbIsOn[i] = 1;
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void LightsOff(ushort i)
        {
            try
            {
                String json = HueBridge.SetLights("lights", i, "false", "state");
                if (json.Contains("success"))
                {
                    HueBulbs[i-1].On = false;
                    BulbIsOn[i] = 0;
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void GroupOn(ushort i)
        {
            try
            {
                String json = HueBridge.SetLights("groups", i, "true", "action");
                if (json.Contains("success"))
                {
                    HueGroups[i-1].On = true;
                    GroupIsOn[i] = 1;
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        public void GroupOff(ushort i)
        {
            try
            {
                String json = HueBridge.SetLights("groups", i, "false", "action");
                if (json.Contains("success"))
                {
                    HueGroups[i-1].On = false;
                    GroupIsOn[i] = 0;
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }
/*
        public void Group1Scene(string groupid, ushort i)
        {
            try
            {
                String payload = String.Format("{0}\"scene\":\"{2}", HueGroups, '}');
                String json = HueBridge.SetScene(groupid, i, payload);
                if (json.Contains("success"))
                {

                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }
        */
    }
}
