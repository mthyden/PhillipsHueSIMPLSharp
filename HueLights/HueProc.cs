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
        }

        /// <summary>
        /// Sets the IP if one exists from SIMPL
        /// </summary>
        /// <param name="str"></param>
        public void setIP(string str)
        {
            HueBridge.BridgeIp = str;
        }

        public void getData()
        {
            var bvalid = getBulbs();
            var gvalid = getRooms();
            var svalid = getScenes();
            if (bvalid == 1 && gvalid == 1 && svalid == 1)
                HueBridge.Populated = true;
            else
                HueBridge.Populated = false;
        }

        /// <summary>
        /// Pulls all the bulbs and their current state from the bridge
        /// </summary>
        ushort getBulbs()
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    string json = HueBridge.GetBridgeInfo("lights");
                    JObject JData = JObject.Parse(json);
                    HueBridge.HueBulbs.Clear();
                    foreach( var bulb in JData)
                    {
                        string id = bulb.Key;
                        bool on = (bool)JData[id]["state"]["on"];
                        uint bri = (uint)JData[id]["state"]["bri"];
                        string alert = (string)JData[id]["state"]["alert"];
                        bool reachable = (bool)JData[id]["state"]["reachable"];
                        string type = (string)JData[id]["type"];
                        string name = (string)JData[id]["name"];
                        string model = (string)JData[id]["modelid"];
                        string manufacturer = (string)JData[id]["manufacturername"];
                        string uid = (string)JData[id]["uniqueid"];
                        string swver = (string)JData[id]["swversion"];
                        if (type.Contains("color") || type.Contains("Color"))
                        {
                            uint hue = (uint)JData[id]["state"]["hue"];
                            uint sat = (uint)JData[id]["state"]["sat"];
                            HueBridge.HueBulbs.Add(new HueBulb(id, on, bri, hue, sat, alert, reachable, type, name, model, manufacturer, uid, swver));
                        }
                        else
                        {
                            HueBridge.HueBulbs.Add(new HueBulb(id, on, bri, alert, reachable, type, name, model, manufacturer, uid, swver));
                        }
                    }
                    BulbNum = (ushort)HueBridge.HueBulbs.Count;
                    CrestronConsole.PrintLine("{0} Bulbs discovered", BulbNum);
                    return 1;
                }
                else
                {
                    CrestronConsole.PrintLine("Bridge not authorized");
                    return 0;
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting bulbs {0}", e);
                return 0;
            }
        }

        /// <summary>
        /// Pulls all the groups/rooms from the bridge
        /// </summary>
        ushort getRooms()
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    string json = HueBridge.GetBridgeInfo("groups");
                    JObject JData = JObject.Parse(json);
                    HueBridge.HueGroups.Clear();
                    foreach (var group in JData)
                    {
                        string id = group.Key;
                        string name = (string)JData[group.Key]["name"];
                        string load = (string)JData[group.Key]["lights"][0];
                        JArray LoadList = (JArray)JData[group.Key]["lights"];
                        string[] loads = LoadList.ToObject<string[]>();
                        string type = (string)JData[group.Key]["type"];
                        bool on = (bool)JData[group.Key]["action"]["on"];
                        uint bri = (uint)JData[group.Key]["action"]["bri"];
                        string alert = (string)JData[group.Key]["action"]["alert"];
                        HueBridge.HueGroups.Add(new HueGroup(name, type, on, bri, alert, load, loads));
                    }
                    for (int i = 0; i < JData.Count; i++)
                    {
                        Array.Clear(HueBridge.HueGroups[i].SceneName, 0, 20);
                        Array.Clear(HueBridge.HueGroups[i].SceneID, 0, 20);
                    }

                        GroupNum = (ushort)HueBridge.HueGroups.Count;
                    CrestronConsole.PrintLine("{0} Rooms discovered", GroupNum);
                    return 1;
                }
                else
                {
                    CrestronConsole.PrintLine("Bridge not authorized");
                    return 0;
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting rooms: {0}", e);
                return 0;
            }
        }

        /// <summary>
        /// Pulls all the scenes from the bridge and assigns them to their appropriate room based on the assigned bulbs 
        /// </summary>
        ushort getScenes()
        {
            try
            {
                if (HueBridge.Authorized == true)
                {
                    string json = HueBridge.GetBridgeInfo("scenes");
                    JObject JData = JObject.Parse(json);
                    HueBridge.HueScenes.Clear();
                    foreach (var scene in JData)
                    {
                        string id = scene.Key;
                        string name = (string)JData[id]["name"];
                        string load = (string)JData[id]["lights"][0];
                        for (int x = 0; x < (HueBridge.HueGroups.Count); x++)
                        {
                            if (HueBridge.HueGroups[x].loads.Contains(load))
                            {
                                for (int y = 1; y < 20; y++)
                                {
                                    if (HueBridge.HueGroups[x].SceneName[y] == null)
                                    {
                                        HueBridge.HueGroups[x].SceneName[y] = name;
                                        HueBridge.HueGroups[x].SceneID[y] = id;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    CrestronConsole.PrintLine("{0} Scenes discovered", JData.Count);
                    return 1;
                }
                else
                {
                    CrestronConsole.PrintLine("Bridge not authorized");
                    return 0;
                }

            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting scenes: {0}",e);
                return 0;
            }
        }
    }
}