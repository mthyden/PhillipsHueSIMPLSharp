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
	    public ushort SensorNum; //number of sensors
        public ushort HueOnline;
        public String[] GrpName;
        public String[] BlbName;

        //^^^^^ Signals for SIMPL+ ^^^^^^^^

        public event EventHandler InitComplete;

        //public DelegateValueUpdate ValueUpdate {get; set;}

        /// <summary>
        /// Default constructor
        /// </summary>
        public HueProc()
        {
            try
            {
                HueOnline = 0;
                HueBridge.InfoReceived += OnInfoReceived;
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
                if (HueBridge.LocalKey == true)
                {
                    HueBridge.BridgeApi = tempapi;
                    HueBridge.Authorized = true;
                    Authorized = (ushort)(HueBridge.Authorized ? 1 : 0);
                }
                else
                {
                    HueBridge.Register();
                    Authorized = (ushort)(HueBridge.Authorized ? 1 : 0);
                }
                APIKey = HueBridge.BridgeApi;
				getData();
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
                IPAddress = HueBridge.GetIp();
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
                if (HueBridge.Authorized)
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
                    CrestronConsole.PrintLine("no groups data found");
                }
                
            }
            if (e.InfoType == "scenes")
            {
                if (e.JsonData != null)
                    ProcScenes(e.JsonData);
                else
                {
                    CrestronConsole.PrintLine("no scenes data found");
                }    
            }
	        if (e.InfoType == "sensors")
	        {
				if (e.JsonData != null)
					ProcSensors(e.JsonData);
				else
				{
					CrestronConsole.PrintLine("no sensor data found");
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
        public void ProcBulbs(string jsondata)
        {
            try
            {
				JObject json = JObject.Parse(jsondata);
				HueBridge.HueBulbs.Clear();
				foreach (var bulb in json)
				{
					string id = bulb.Key;
					uint hue = 0;
					uint sat = 0;
					uint ct = 0;
					string colormode;
					bool on = (bool)json[id]["state"]["on"];
					uint bri = (uint)json[id]["state"]["bri"];
					string type = (string)json[id]["type"];
					string name = (string)json[id]["name"];
					string model = (string)json[id]["modelid"];
					string manufacturer = (string)json[id]["manufacturername"];
					string uid = (string)json[id]["uniqueid"];
					string swver = (string)json[id]["swversion"];
					if (json[id]["state"].SelectToken("colormode") != null)
					{
						colormode = (string)json[id]["state"]["colormode"];
						if (json[id]["state"].SelectToken("hue") != null)
						{
							hue = (uint)json[id]["state"]["hue"];
						}
						if (json[id]["state"].SelectToken("sat") != null)
						{
							sat = (uint)json[id]["state"]["sat"];
						}
						if (json[id]["state"].SelectToken("ct") != null)
						{
							ct = (uint)json[id]["state"]["ct"];
						}
						HueBridge.HueBulbs.Add(new HueBulb(id, on, bri, hue, sat, ct, type, name, model, manufacturer, uid, swver, colormode));
					}
					else
					{
						HueBridge.HueBulbs.Add(new HueBulb(id, on, bri, type, name, model, manufacturer, uid, swver));
					}
				}
                    BulbNum = (ushort)HueBridge.HueBulbs.Count;
                    CrestronConsole.PrintLine("{0} Bulbs discovered", BulbNum);
                    HueBridge.GetBridgeInfo("groups");
                }
            catch (Exception e)
            {
				CrestronConsole.PrintLine("Error getting bulbs: {0}", e);
            }
        }

        /// <summary>
        /// Pulls all the groups/rooms from the bridge
        /// </summary>
        public void ProcRooms(string jsondata)
        {
            try
            {
				JObject json = JObject.Parse(jsondata);
                    HueBridge.HueGroups.Clear();
					foreach (var group in json)
                    {
                        string load;
	                    uint bri;
                        JArray loadList;
                        string[] loads;
	                    string roomclass;
	                    string type;
                        string id = group.Key;
						if ((string)json[group.Key]["type"] == "Room")
	                    {
							string name = (string)json[group.Key]["name"];
							roomclass = (string)json[group.Key]["class"];
							type = (string)json[group.Key]["type"];
							if (json[group.Key]["lights"].HasValues)
							{
								load = (string)json[group.Key]["lights"][0];
								loadList = (JArray)json[group.Key]["lights"];
								loads = loadList.ToObject<string[]>();
								bri = (uint)json[group.Key]["action"]["bri"];
							}
							else
							{
								load = "0";
								loads = null;
								bri = 0;
								CrestronConsole.PrintLine("No lights in {0}", name);
							}

							bool on = (bool)json[group.Key]["action"]["on"];
							HueBridge.HueGroups.Add(new HueGroup(id, name, type, on, bri, load, loads, roomclass));
	                    }
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
        public void ProcScenes(string jsondata)
        {
            try
            {
				JObject json = JObject.Parse(jsondata);
                //HueBridge.HueScenes.Clear();
                string id = "";
                string name = "";
                string load = "";
				foreach (var scene in json)
                {
                    id = scene.Key;
					name = (string)json[id]["name"];
					if (json[id]["lights"].HasValues)
                    {
						load = (string)json[id]["lights"][0];
                    }
                    else
                    {
                        load = "";
                        //CrestronConsole.PrintLine("load is null");
                    }
                    for (int x = 0; x < (HueBridge.HueGroups.Count); x++)
                    {
                        if (HueBridge.HueGroups[x].Loads != null && load != "")
                        {
                            if (HueBridge.HueGroups[x].Loads.Contains(load))
                            {
                                //CrestronConsole.PrintLine("found room: {0}, with load: {1}", HueBridge.HueGroups[x].Name, load);
                                for (int y = 1; y < 20; y++)
                                {
                                    if (HueBridge.HueGroups[x].SceneName[y] == null)
                                    {
                                        //CrestronConsole.PrintLine("SceneName: {0}, with D: {1}", name, id);
                                        HueBridge.HueGroups[x].SceneName[y] = name;
                                        HueBridge.HueGroups[x].SceneID[y] = id;
                                        break;
                                    }
                                }
                            }
                        }
                    } 
                }
				CrestronConsole.PrintLine("{0} Scenes discovered", json.Count);
                HueOnline = 1;
                HueBridge.Populated = true;
                GrpName = new String[50];
                BlbName = new String[50];
                
                foreach (var huegroup in HueBridge.HueGroups)
                {
                    GrpName[Convert.ToUInt16(huegroup.Id)] = huegroup.Name;
                }
                foreach (var huebulb in HueBridge.HueBulbs)
                {
                    BlbName[Convert.ToUInt16(huebulb.Id)] = huebulb.Name;
                }
				//HueBridge.GetBridgeInfo("sensors");
				OnInitComplete();

            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting scenes: {0}",e);
            }
        }

	    public void ProcSensors(string jsondata)
	    {
		    try
		    {
				string id;
				string type;
				string name;
				string daylight;
				bool? presence;
				uint temp;
				ushort battery;
				bool reachable;
				string lastupdated;
				string alert;
				string uid;

				HueBridge.HueSensors.Clear();
				JObject json = JObject.Parse(jsondata);
				foreach (var sensor in json)
				{
					id = sensor.Key;
					type = (string)json[id]["type"];
					if (type == "ZLLPresence")
					{
						name = (string)json[id]["name"];
						uid = (string)json[id]["uid"];
						//battery = (ushort)json[id]["config"]["battery"];
						//reachable = (bool)json[id]["config"]["reachable"];
						if (json[id]["state"].SelectToken("presence") != null)
						{
							//CrestronConsole.PrintLine("presence not null");
							//presence = false;
						}
						else
						{
							//CrestronConsole.PrintLine("presence is null");
							//presence = (bool)json[id]["state"]["presence"];
						}
							
						lastupdated = (string)json[id]["state"]["lastupdated"];
						HueBridge.HueSensors.Add(new HueSensor(id, uid, name, type));
					}
				}
				SensorNum = (ushort)HueBridge.HueSensors.Count;
				CrestronConsole.PrintLine("{0} Sensors discovered", SensorNum);
		    }
		    catch (Exception e)
		    {

				CrestronConsole.PrintLine("Error getting sensors: {0}", e);
		    }

	    }
    }
}