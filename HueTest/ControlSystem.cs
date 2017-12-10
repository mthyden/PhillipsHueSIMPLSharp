using System;
using System.Linq;
using Crestron.SimplSharp;                          	// For Basic SIMPL# Classes
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharpPro;                       	// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using HueLights;
using Newtonsoft.Json.Linq;

namespace HueTest
{
    public class ControlSystem : CrestronControlSystem
    {
        private FileStream _roomStream;
        private StreamReader _roomReader;
        private FileStream _scenesStream;
        private StreamReader _scenesReader;
		private FileStream _sensorStream;
	    private StreamReader _sensorReader;

        public String[] GrpName;
        public String[] BlbName;

        private HueProc _hue;

        /// <summary>
        /// ControlSystem Constructor. Starting point for the SIMPL#Pro program.
        /// Use the constructor to:
        /// * Initialize the maximum number of threads (max = 400)
        /// * Register devices
        /// * Register event handlers
        /// * Add Console Commands
        /// 
        /// Please be aware that the constructor needs to exit quickly; if it doesn't
        /// exit in time, the SIMPL#Pro program will exit.
        /// 
        /// You cannot send / receive data in the constructor
        /// </summary>
        public ControlSystem()
            : base()
        {
            try
            {
                Thread.MaxNumberOfUserThreads = 20;

                //Subscribe to the controller events (System, Program, and Ethernet)
                CrestronEnvironment.SystemEventHandler += new SystemEventHandler(ControlSystem_ControllerSystemEventHandler);
                CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(ControlSystem_ControllerProgramEventHandler);
                CrestronEnvironment.EthernetEventHandler += new EthernetEventHandler(ControlSystem_ControllerEthernetEventHandler);
                CrestronConsole.AddNewConsoleCommand(ConfigureHue, "testhue", "Tests the Hue request",
                    ConsoleAccessLevelEnum.AccessAdministrator);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);
            }
        }

        /// <summary>
        /// InitializeSystem - this method gets called after the constructor 
        /// has finished. 
        /// 
        /// Use InitializeSystem to:
        /// * Start threads
        /// * Configure ports, such as serial and verisports
        /// * Start and initialize socket connections
        /// Send initial device configurations
        /// 
        /// Please be aware that InitializeSystem needs to exit quickly also; 
        /// if it doesn't exit in time, the SIMPL#Pro program will exit.
        /// </summary>
        public override void InitializeSystem()
        {
            try
            {

            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
            }
        }

        public void ConfigureHue(string s)
        {
			//ProcBulbs();
            //ProcGroups();
	        //ProcScenes();
	        ProcSensors();
        }

        public void ProcGroups()
        {
            try
            {
                _roomStream = new FileStream(@"\NVRAM\Groups.txt", FileMode.Open, FileAccess.Read);
                _roomReader = new StreamReader(_roomStream);
                string jsonrooms = _roomReader.ReadToEnd();
                _roomStream.Close();
                _roomReader.Close();
                RoomsTest(jsonrooms);
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.ToString());
            }
        }

		public void ProcScenes()
		{
			try
			{
				_scenesStream = new FileStream(@"\NVRAM\Scenes.txt", FileMode.Open, FileAccess.Read);
				_scenesReader = new StreamReader(_scenesStream);
				string jsonscenes = _scenesReader.ReadToEnd();
				_scenesStream.Close();
				_scenesReader.Close();
				ScenesTest(jsonscenes);
			}
			catch (Exception e)
			{
				CrestronConsole.PrintLine(e.ToString());
			}
		}

		public void ProcSensors()
		{
			try
			{
				_sensorStream = new FileStream(@"\NVRAM\sensors.txt", FileMode.Open, FileAccess.Read);
				_sensorReader = new StreamReader(_sensorStream);
				string jsonsensors = _sensorReader.ReadToEnd();
				_sensorStream.Close();
				_sensorReader.Close();
				RoomsTest(jsonsensors);
			}
			catch (Exception e)
			{
				CrestronConsole.PrintLine(e.ToString());
			}
		}


        public void RoomsTest(string jsondata)
        {
            try
            {
                JObject jData = JObject.Parse(jsondata);
                HueBridge.HueGroups.Clear();
                foreach (var group in jData)
                {
                    string load;
                    JArray loadList;
	                uint bri;
                    string[] loads;
                    string id = group.Key;
                    string name = (string)jData[group.Key]["name"];
                    if (jData[group.Key]["lights"].HasValues)
                    {
                        load = (string)jData[group.Key]["lights"][0];
                        loadList = (JArray)jData[group.Key]["lights"];
                        loads = loadList.ToObject<string[]>();
						bri = (uint)jData[group.Key]["action"]["bri"];
                    }
                    else
                    {
                        load = "0";
                        loads = null;
	                    bri = 0;
                    }

                    string type = (string)jData[group.Key]["type"];
					string roomclass = (string)jData[group.Key]["class"];
                    bool on = (bool)jData[group.Key]["action"]["on"];

                    string alert = (string)jData[group.Key]["action"]["alert"];
                    HueBridge.HueGroups.Add(new HueGroup(id, name, type, on, bri, load, loads, roomclass));
                }

                var GroupNum = (ushort)HueBridge.HueGroups.Count;
                CrestronConsole.PrintLine("{0} Rooms discovered", GroupNum);
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting rooms: {0}", e);
            }
        }

        public void ScenesTest(string jsondata)
        {
            try
            {
                JObject jData = JObject.Parse(jsondata);
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
                CrestronConsole.PrintLine("{0} Scenes discovered", jData.Count);
                //HueOnline = 1;
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
                
                for (int i = 0 ; i < 50; i++)
                {
                    CrestronConsole.PrintLine("GrpName: {0}",GrpName[i]);
                    CrestronConsole.PrintLine("BlbName: {0}", BlbName[i]);
                }
                //OnInitComplete();
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error getting scenes: {0}", e);
            }
        }

	    public void SensorTest(string jsondata)
	    {
		    string id;
		    string type;
		    string name;
		    string daylight;
		    bool presence;
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
			    type = (string) json[id]["type"];
			    if (type == "ZLLPresence")
			    {
				    name = (string) json[id]["name"];
				    uid = (string) json[id]["uid"];
				    battery = (ushort) json[id]["config"]["battery"];
				    reachable = (bool) json[id]["config"]["reachable"];
				    presence = (bool) json[id]["state"]["presence"];
				    lastupdated = (string) json[id]["state"]["lastupdated"];
				    HueBridge.HueSensors.Add(new HueSensor(id, uid, name, type));
			    }
		    }
			/*
		    HueBridge.HueSensors[MotionId - 1].Daylight = (bool)json[MotionId]["state"]["on"];
			HueBridge.HueSensors[MotionId - 1].Presence = (bool)json[MotionId]["state"]["on"];
			HueBridge.HueSensors[MotionId - 1].Temp = (ushort)json[MotionId]["state"]["on"];
			HueBridge.HueSensors[MotionId - 1].Reachable = (bool)json[MotionId]["state"]["on"];
			MotionDaylight = (ushort)(HueBridge.HueSensors[MotionId - 1].Daylight ? 1 : 0);
			MotionPresence = (ushort)(HueBridge.HueSensors[MotionId - 1].Presence ? 1 : 0);
			MotionTemp = (ushort)(HueBridge.HueSensors[MotionId - 1].Temp);
			Reachable = (ushort)(HueBridge.HueSensors[MotionId - 1].Reachable ? 1 : 0);*/
	    }

        /// <summary>
        /// Event Handler for Ethernet events: Link Up and Link Down. 
        /// Use these events to close / re-open sockets, etc. 
        /// </summary>
        /// <param name="ethernetEventArgs">This parameter holds the values 
        /// such as whether it's a Link Up or Link Down event. It will also indicate 
        /// wich Ethernet adapter this event belongs to.
        /// </param>
        void ControlSystem_ControllerEthernetEventHandler(EthernetEventArgs ethernetEventArgs)
        {
            switch (ethernetEventArgs.EthernetEventType)
            {//Determine the event type Link Up or Link Down
                case (eEthernetEventType.LinkDown):
                    //Next need to determine which adapter the event is for. 
                    //LAN is the adapter is the port connected to external networks.
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {
                        //
                    }
                    break;
                case (eEthernetEventType.LinkUp):
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {

                    }
                    break;
            }
        }

        /// <summary>
        /// Event Handler for Programmatic events: Stop, Pause, Resume.
        /// Use this event to clean up when a program is stopping, pausing, and resuming.
        /// This event only applies to this SIMPL#Pro program, it doesn't receive events
        /// for other programs stopping
        /// </summary>
        /// <param name="programStatusEventType"></param>
        void ControlSystem_ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
        {
            switch (programStatusEventType)
            {
                case (eProgramStatusEventType.Paused):
                    //The program has been paused.  Pause all user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Resumed):
                    //The program has been resumed. Resume all the user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Stopping):
                    //The program has been stopped.
                    //Close all threads. 
                    //Shutdown all Client/Servers in the system.
                    //General cleanup.
                    //Unsubscribe to all System Monitor events
                    break;
            }

        }

        /// <summary>
        /// Event Handler for system events, Disk Inserted/Ejected, and Reboot
        /// Use this event to clean up when someone types in reboot, or when your SD /USB
        /// removable media is ejected / re-inserted.
        /// </summary>
        /// <param name="systemEventType"></param>
        void ControlSystem_ControllerSystemEventHandler(eSystemEventType systemEventType)
        {
            switch (systemEventType)
            {
                case (eSystemEventType.DiskInserted):
                    //Removable media was detected on the system
                    break;
                case (eSystemEventType.DiskRemoved):
                    //Removable media was detached from the system
                    break;
                case (eSystemEventType.Rebooting):
                    //The system is rebooting. 
                    //Very limited time to preform clean up and save any settings to disk.
                    break;
            }

        }
    }
}