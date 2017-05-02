using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Crestron.SimplSharp.Net.Http;

namespace HueLights
{
    public class HueProc
    {
        public delegate ushort DelegateValueUpdate();

        public String IPSet;
        public String IPAddress;    //IP Address for a Hue Bridge
        public ushort Authorized;   //Reports if the API key has been acquired
        public String APIKey;       //API Key used to control Hue devices (stored in CrestronDataStore
        public ushort BulbNum; // number of bulbs
        public ushort GroupNum; // number of rooms

        //^^^^^ Signals for SIMPL+ ^^^^^^^^

        public DelegateValueUpdate ValueUpdate {get; set;}

        /// <summary>
        /// Default constructor
        /// </summary>
        public HueProc()
        {

        }

        /// <summary>
        /// Registers the app to the bridge, if API key already exists in datastore will pull
        /// </summary>
        public void Register()
        {
            try
            {
                HueBridge.SetupDataStore();
                string tempapi;
                tempapi = HueBridge.GetDataStore();
                //CrestronConsole.PrintLine("api is {0}", tempapi);
                if (tempapi != null)
                {
                    HueBridge.BridgeApi = tempapi;
                    HueBridge.Authorized = true;
                    Authorized = (ushort)(HueBridge.Authorized ? 1 : 0);
                }
                else
                {
                    HueBridge.register();
                    Authorized = (ushort)(HueBridge.Authorized ? 1 : 0);
                }
                APIKey = HueBridge.BridgeApi;
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        /// <summary>
        /// gets bridge IP from the broker server www.meethue.com/api/nupnp
        /// </summary>
        public void getIP()
        {
                IPAddress = HueBridge.getIP();
                HueBridge.Authorized = false;
                Authorized = (ushort)(HueBridge.Authorized ? 1 : 0);
        }

        /// <summary>
        /// Sets the IP if one exists from SIMPL
        /// </summary>
        /// <param name="str"></param>
        public void setIP(string str)
        {
            HueBridge.BridgeIp = str;
        }

        /// <summary>
        /// Pulls all the bulbs and their current state from the bridge
        /// </summary>
        public void getBulbs()
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    string json = HueBridge.GetBridgeInfo("lights");
                    JObject JData = JObject.Parse(json);
                    HueBridge.HueBulbs.Clear();
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
                            HueBridge.HueBulbs.Add(new HueBulb(on, bri, hue, sat, alert, reachable, type, name, model, manufacturer, uid, swver));
                        }
                        else
                        {
                            HueBridge.HueBulbs.Add(new HueBulb(on, bri, alert, reachable, type, name, model, manufacturer, uid, swver));
                        }
                    }
                    BulbNum = (ushort)HueBridge.HueBulbs.Count;
                    CrestronConsole.PrintLine("Get Bulbs is complete...");
                }
                else
                {
                    CrestronConsole.PrintLine("Bridge not authorized");
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting bulbs {0}", e);
            }
        }

        /// <summary>
        /// Pulls all the groups/rooms from the bridge
        /// </summary>
        public void getRooms()
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    string json = HueBridge.GetBridgeInfo("groups");
                    JObject JData = JObject.Parse(json);
                    HueBridge.HueGroups.Clear();
                    for (int i = 1; i <= JData.Count; i++)
                    {
                        string name = (string)JData[i.ToString()]["name"];
                        string load = (string)JData[i.ToString()]["lights"][0];
                        JArray LoadList = (JArray)JData[i.ToString()]["lights"];
                        string[] loads = LoadList.ToObject<string[]>();
                        string type = (string)JData[i.ToString()]["type"];
                        bool on = (bool)JData[i.ToString()]["action"]["on"];
                        uint bri = (uint)JData[i.ToString()]["action"]["bri"];
                        string alert = (string)JData[i.ToString()]["action"]["alert"];
                        HueBridge.HueGroups.Add(new HueGroup(name, type, on, bri, alert, load, loads));
                        Array.Clear(HueBridge.HueGroups[i - 1].SceneName, 0, 20);
                        Array.Clear(HueBridge.HueGroups[i - 1].SceneID, 0, 20);
                    }
                    GroupNum = (ushort)HueBridge.HueGroups.Count;
                    CrestronConsole.PrintLine("Get Rooms is complete...");
                }
                else
                {
                    CrestronConsole.PrintLine("Bridge not authorized");
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting rooms: {0}", e);
            }
        }

        /// <summary>
        /// Pulls all the scenes from the bridge and assigns them to their appropriate room based on the assigned bulbs 
        /// </summary>
        public void getScenes()
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    string json = HueBridge.GetBridgeInfo("scenes");
                    JObject JData = JObject.Parse(json);
                    string JText = JData.ToString();
                    HueBridge.HueScenes.Clear();
                    for (int i = 1; i <= JData.Count; i++)
                    {
                        int pos1 = JText.IndexOf("\"");
                        string ID = JText.Substring(pos1 + 1, 15);
                        pos1 = JText.IndexOf("name\"");
                        int pos2 = JText.IndexOf(",", pos1);
                        string name = JText.Substring(pos1 + 8, ((pos2 - 1) - (pos1 + 8)));
                        pos2 = JText.IndexOf("lights");
                        pos1 = JText.IndexOf("[", pos2);
                        pos1 = JText.IndexOf("\"", pos1)+1;
                        pos2 = JText.IndexOf("\"", pos1);
                        string load = JText.Substring(pos1, (pos2 - pos1));
                        HueBridge.HueScenes.Add(new HueScene(ID, name, load));
                        int pos3 = JText.IndexOf("lastupdated");
                        int pos4 = JText.IndexOf("version", pos3);
                        int pos5 = JText.IndexOf("}", pos4);
                        JText = JText.Remove(0, pos5);
                        for (int x = 0; x < (HueBridge.HueGroups.Count); x++)
                        {
                            if(HueBridge.HueGroups[x].loads.Contains(load))               
                            {
                                for (int y = 1; y < 20; y++)
                                {
                                    if (HueBridge.HueGroups[x].SceneName[y] == null)
                                    {
                                        HueBridge.HueGroups[x].SceneName[y] = name;
                                        HueBridge.HueGroups[x].SceneID[y] = ID;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    CrestronConsole.PrintLine("Get Scenes is complete...");
                }
                else
                {
                    CrestronConsole.PrintLine("Bridge not authorized");
                }

            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting scenes: {0}",e);
            }
        }
    }
}