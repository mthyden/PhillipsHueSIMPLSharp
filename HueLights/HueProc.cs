using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace HueLights
{
    public class HueProc
    {
        public String IPSet;
        public String IPAddress;    //IP Address for a Hue Bridge
        public ushort Authorized;   //Reports if the API key has been acquired
	    public String ApiKeyIn;
        public String ApiKeyOut;       //API Key used to control Hue devices (stored in CrestronDataStore
        public ushort BulbNum; // number of bulbs
        public ushort GroupNum; // number of rooms
	    public ushort SensorNum; //number of sensors
        public ushort HueOnline;
        public String[] GrpName;
        public String[] BlbName;

        //^^^^^ Signals for SIMPL+ ^^^^^^^^

        public event EventHandler InitComplete;

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
                ApiKeyOut = HueBridge.BridgeApi;
				HueBridge.HueBulbs.Clear();
				HueBridge.HueGroups.Clear();
				HueBridge.HueSensors.Clear();
				//CrestronConsole.PrintLine("Registering Bridge with DataStore API key");
				getData();
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception in registration {0}", e);
            }
        }

	    public void RegisterWithKey()
	    {
		    HueBridge.BridgeApi = ApiKeyIn;
			ApiKeyOut = HueBridge.BridgeApi;
			HueBridge.Authorized = true;
			Authorized = (ushort)(HueBridge.Authorized ? 1 : 0);
			HueBridge.HueBulbs.Clear();
			HueBridge.HueGroups.Clear();
			HueBridge.HueSensors.Clear();
			getData();
			//CrestronConsole.PrintLine("Registering Bridge with manual API key");
	    }

		/// <summary>
		/// reset the api key from the datastore
		/// </summary>
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
				//CrestronConsole.PrintLine("Bridge IP is being get");
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
			IPAddress = HueBridge.GetIp();
			//CrestronConsole.PrintLine("Bridge IP is being set");
        }

		/// <summary>
		/// Gets bridge data starting with lights
		/// </summary>
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

		/// <summary>
		/// EventHandler for data being received from Bridge
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
        public void OnInfoReceived(object source, HueEventArgs e)
        {
            if (e.Id == HueRequestId.Lights)
            {
                if(e.Response != null)
					ProcBulbs(e.Response);
                else
                {
                    CrestronConsole.PrintLine("no lights data found");
                }
            }
			if (e.Id == HueRequestId.Rooms)
            {
				if (e.Response != null)
					ProcRooms(e.Response);
                else
                {
                    CrestronConsole.PrintLine("no rooms data found");
                }
                
            }
			if (e.Id == HueRequestId.Scenes)
            {
				if (e.Response != null)
					ProcScenes(e.Response);
                else
                {
                    CrestronConsole.PrintLine("no scenes data found");
                }    
            }
			if (e.Id == HueRequestId.Sensors)
	        {
				if (e.Response != null)
					ProcSensors(e.Response);
				else
				{
					CrestronConsole.PrintLine("no sensor data found");
				} 		        
	        }
        }

		/// <summary>
		/// Event Initialization complete delegate
		/// </summary>
        public void OnInitComplete()
        {
			HueBridge.RegisterEvents();
            InitComplete(this, new EventArgs()); //delegate method
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
				uint hue;
				uint sat;
				uint ct;
				uint bri;
				string colormode;
	            string type;
	            string name;
	            string manufacturer;
	            string uid;
	            string swver;
	            bool reachable;
	            string model;
	            bool on;
				foreach (var bulb in json)
				{
					string id = bulb.Key;
					type = "";
					name = "";
					manufacturer = "";
					uid = "";
					swver = "";
					reachable = false;
					model = "";
					on = false;
					if (json[id]["state"].SelectToken("on") != null)
					on = (bool)json[id]["state"]["on"];
					if (json[id]["state"].SelectToken("reachable") != null)
						reachable = (bool)json[id]["state"]["reachable"];
					if (json[id].SelectToken("type") != null)
					type = (string)json[id]["type"];
					if (json[id].SelectToken("name") != null)
					name = (string)json[id]["name"];
					if (json[id].SelectToken("modelid") != null)
					model = (string)json[id]["modelid"];
					if (json[id].SelectToken("manufacturername") != null)
					manufacturer = (string)json[id]["manufacturername"];
					if (json[id].SelectToken("uniqueid") != null)
					uid = (string)json[id]["uniqueid"];
					if (json[id].SelectToken("swversion") != null)
					swver = (string)json[id]["swversion"];
					HueBridge.HueBulbs.Add(id, new HueBulb() { On = on, Type = type, Name = name, Model = model, Manufacturer = manufacturer, Uid = uid, SwVer = swver, Reachable = reachable});
					if (json[id]["state"].SelectToken("bri") != null)
					{
						bri = (uint)json[id]["state"]["bri"];
						HueBridge.HueBulbs[id].Bri = bri;
					}	
					if (json[id]["state"].SelectToken("colormode") != null)
					{						
						colormode = (string)json[id]["state"]["colormode"];
						HueBridge.HueBulbs[id].ColorMode = colormode;
						if (json[id]["state"].SelectToken("hue") != null)
						{
							hue = (uint)json[id]["state"]["hue"];
							HueBridge.HueBulbs[id].Hue = hue;
						}
						if (json[id]["state"].SelectToken("sat") != null)
						{
							sat = (uint)json[id]["state"]["sat"];
							HueBridge.HueBulbs[id].Sat = sat;
						}
							
						if (json[id]["state"].SelectToken("ct") != null)
						{
							ct = (uint)json[id]["state"]["ct"];
							HueBridge.HueBulbs[id].Ct = ct;
						}
					}
				}
				BulbNum = (ushort)HueBridge.HueBulbs.Count;
                CrestronConsole.PrintLine("{0} Bulbs discovered", BulbNum);
				if(Authorized == 1)
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
							string name = (string)json[id]["name"];
							roomclass = (string)json[id]["class"];
							type = (string)json[id]["type"];
		                    bri = 0;
							if (json[id]["lights"].HasValues)
							{
								load = (string)json[id]["lights"][0];
								loadList = (JArray)json[id]["lights"];
								loads = loadList.ToObject<string[]>();
								if (json[id]["action"].SelectToken("bri") != null)
								bri = (uint)json[id]["action"]["bri"];
							}
							else
							{
								load = "0";
								loads = null;
								CrestronConsole.PrintLine("No lights in {0}", name);
							}

							bool on = (bool)json[group.Key]["action"]["on"];
							HueBridge.HueGroups.Add(id, new HueGroup(name, type, on, bri, load, loads, roomclass));
							
	                    }
                    GroupNum = (ushort)HueBridge.HueGroups.Count;
                    CrestronConsole.PrintLine("{0} Rooms discovered", GroupNum);
					if (Authorized == 1)
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
                string id = "";
                string name = "";
	            string group = "";
                string load = "";
				foreach (var scene in json)
                {
                    id = scene.Key;
					name = (string)json[id]["name"];
	                if (json[id].SelectToken("group") != null)
	                {
						group = (string)json[id]["group"];
						if (HueBridge.HueGroups.ContainsKey(group)) { HueBridge.HueGroups[group].Scenes.Add(new HueScene() { Group = group, Name = name, SceneId = id }); }
	                }
	                else
	                {
						CrestronConsole.PrintLine("SceneName: {0} is invalid, recreate from the Hue app",name);
	                }	
                }
				
                HueOnline = 1;
                HueBridge.Populated = true;
                GrpName = new String[50];
                BlbName = new String[50];

	            int i = 1;
				foreach (KeyValuePair<string, HueGroup> entry in HueBridge.HueGroups)
				{
					HueBridge.HueGroups[entry.Key].ScenesNum = (ushort)entry.Value.Scenes.Count;
					CrestronConsole.PrintLine("{0} Scenes discovered in the {1} group", entry.Value.Scenes.Count, entry.Value.Name);
					GrpName[i] = entry.Value.Name;
					i++;
				}
				i = 1;

				foreach (KeyValuePair<string, HueBulb> entry in HueBridge.HueBulbs)
				{
					BlbName[i] = entry.Value.Name;
					i++;
				}
				//HueBridge.GetBridgeInfo("sensors");
				OnInitComplete();

            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting scenes: {0}",e);
            }
        }

		/// <summary>
		/// Pulls all the sensors from the bridge, currently extracts only the presence sensor
		/// </summary>
		/// <param name="jsondata"></param>
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
						HueBridge.HueSensors.Add(id, new HueSensor(uid, name, type));
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