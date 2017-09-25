using System;
using System.Linq;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

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
        public ushort HueOnline;

        //^^^^^ Signals for SIMPL+ ^^^^^^^^

        public event EventHandler InitComplete;

        public DelegateValueUpdate ValueUpdate {get; set;}

        /// <summary>
        /// Default constructor
        /// </summary>
        public HueProc()
        {
            try
            {
                HueOnline = 0;
                HueBridge.InfoReceived += this.OnInfoReceived;
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception: {0}", e);
            }

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

        public void ResetAPI()
        {
            HueBridge.ResetDataStore();
        }

        /// <summary>
        /// gets bridge IP from the broker server www.meethue.com/api/nupnp
        /// </summary>
        public void getIP()
        {
            try
            {
                IPAddress = HueBridge.getIP();
            }
            catch (Exception e)
            {
                
                CrestronConsole.PrintLine("Exception: {0}",e);
            }     
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
            try
            {
                if (HueBridge.Authorized == true)
                {
                    HueBridge.GetBridgeInfo("lights");
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

        public void OnInfoReceived(object source, InfoEventArgs e)
        {
            if (e.InfoType == "lights")
            {
                if(e.JsonData != null)
                ProcBulbs(e.JsonData);
                else
                {
                    CrestronConsole.PrintLine("no bulb data found");
                }
            }
            if (e.InfoType == "groups")
            {
                if (e.JsonData != null)
                    ProcRooms(e.JsonData);
                else
                {
                    CrestronConsole.PrintLine("no bulb data found");
                }
                
            }
            if (e.InfoType == "scenes")
            {
                if (e.JsonData != null)
                    ProcScenes(e.JsonData);
                else
                {
                    CrestronConsole.PrintLine("no bulb data found");
                }
                
            }
        }

        public void OnInitComplete()
        {
            InitComplete(this, new EventArgs());
        }

        /// <summary>
        /// Pulls all the bulbs and their current state from the bridge
        /// </summary>
        public void ProcBulbs(String jsondata)
        {
            try
            {
                    JObject jData = JObject.Parse(jsondata);
                    HueBridge.HueBulbs.Clear();
                    foreach( var bulb in jData)
                    {
                        string id = bulb.Key;
                        bool on = (bool)jData[id]["state"]["on"];
                        uint bri = (uint)jData[id]["state"]["bri"];
                        string alert = (string)jData[id]["state"]["alert"];
                        bool reachable = (bool)jData[id]["state"]["reachable"];
                        string type = (string)jData[id]["type"];
                        string name = (string)jData[id]["name"];
                        string model = (string)jData[id]["modelid"];
                        string manufacturer = (string)jData[id]["manufacturername"];
                        string uid = (string)jData[id]["uniqueid"];
                        string swver = (string)jData[id]["swversion"];
                        if (type.Contains("color") || type.Contains("Color"))
                        {
                            uint hue = (uint)jData[id]["state"]["hue"];
                            uint sat = (uint)jData[id]["state"]["sat"];
                            HueBridge.HueBulbs.Add(new HueBulb(id, on, bri, hue, sat, alert, reachable, type, name, model, manufacturer, uid, swver));
                        }
                        else
                        {
                            HueBridge.HueBulbs.Add(new HueBulb(id, on, bri, alert, reachable, type, name, model, manufacturer, uid, swver));
                        }
                    }
                    BulbNum = (ushort)HueBridge.HueBulbs.Count;
                    CrestronConsole.PrintLine("{0} Bulbs discovered", BulbNum);
                    HueBridge.GetBridgeInfo("groups");
                }
            catch (Exception e)
            {
                
            }
        }

        /// <summary>
        /// Pulls all the groups/rooms from the bridge
        /// </summary>
        public void ProcRooms(String jsondata)
        {
            try
            {
                    JObject jData = JObject.Parse(jsondata);
                    HueBridge.HueGroups.Clear();
                    foreach (var group in jData)
                    {
                        string load;
                        JArray LoadList;
                        string[] loads;
                        string id = group.Key;
                        string name = (string)jData[group.Key]["name"];
                        if (jData[group.Key]["lights"].HasValues)
                        {
                            load = (string)jData[group.Key]["lights"][0];
                            LoadList = (JArray)jData[group.Key]["lights"];
                            loads = LoadList.ToObject<string[]>();
                        }
                        else
                        {
                            load = "0";
                            loads = null;
                        }
 
                        string type = (string)jData[group.Key]["type"];
                        bool on = (bool)jData[group.Key]["action"]["on"];
                        uint bri = (uint)jData[group.Key]["action"]["bri"];
                        string alert = (string)jData[group.Key]["action"]["alert"];
                        HueBridge.HueGroups.Add(new HueGroup(id, name, type, on, bri, alert, load, loads));
                    }
                    for (int i = 0; i < jData.Count; i++)
                    {
                        Array.Clear(HueBridge.HueGroups[i].SceneName, 0, 20);
                        Array.Clear(HueBridge.HueGroups[i].SceneID, 0, 20);
                    }

                        GroupNum = (ushort)HueBridge.HueGroups.Count;
                    CrestronConsole.PrintLine("{0} Rooms discovered", GroupNum);
                    HueBridge.GetBridgeInfo("scenes");
                }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting rooms: {0}", e);
            }
        }

        /// <summary>
        /// Pulls all the scenes from the bridge and assigns them to their appropriate room based on the assigned bulbs 
        /// </summary>
        public void ProcScenes(String jsondata)
        {
            try
            {
                JObject jData = JObject.Parse(jsondata);
                HueBridge.HueScenes.Clear();
                string id = "";
                string name = "";
                string load = "";
                foreach (var scene in jData)
                {
                    id = scene.Key;
                    name = (string)jData[id]["name"];
                    if (jData[id]["lights"].HasValues)
                    {
                        load = (string)jData[id]["lights"][0];
                    }
                    else
                    {
                        load = "";
                        CrestronConsole.PrintLine("load is null");
                    }
                    for (int x = 0; x < (HueBridge.HueGroups.Count); x++)
                    {
                        if (HueBridge.HueGroups[x].Loads != null && load != "")
                        {
                            if (HueBridge.HueGroups[x].Loads.Contains(load))
                            {
                                CrestronConsole.PrintLine("found room: {0}, with load: {1}", HueBridge.HueGroups[x].Name, load);
                                for (int y = 1; y < 20; y++)
                                {
                                    if (HueBridge.HueGroups[x].SceneName[y] == null)
                                    {
                                        CrestronConsole.PrintLine("SceneName: {0}, with D: {1}", name, id);
                                        HueBridge.HueGroups[x].SceneName[y] = name;
                                        HueBridge.HueGroups[x].SceneID[y] = id;
                                        break;
                                    }
                                }
                            }
                        }
                    } 
                }
                CrestronConsole.PrintLine("{0} Scenes discovered", jData.Count);
                HueOnline = 1;
                HueBridge.Populated = true;
                OnInitComplete();
                }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting scenes: {0}",e);
            }
        }
    }
}