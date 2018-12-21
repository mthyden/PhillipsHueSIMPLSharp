using System;
using System.Linq;
using Crestron.SimplSharp;                          	// For Basic SIMPL# Classes
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharpPro;                       	// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using HueLights;
using Newtonsoft.Json.Linq;
using Crestron.SimplSharp.CrestronDataStore;

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
		private FileStream _bulbStream;
		private StreamReader _bulbReader;

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
				CrestronEnvironment.ProgramStatusEventHandler +=
					new ProgramStatusEventHandler(ControlSystem_ControllerProgramEventHandler);
				CrestronEnvironment.EthernetEventHandler += new EthernetEventHandler(ControlSystem_ControllerEthernetEventHandler);

				#region json cmds
				CrestronConsole.AddNewConsoleCommand(ReadBulbs, "bulbs", "Tests the Hue request",
					ConsoleAccessLevelEnum.AccessAdministrator);
				CrestronConsole.AddNewConsoleCommand(ReadGroups, "groups", "Tests the Hue request",
					ConsoleAccessLevelEnum.AccessAdministrator);
				CrestronConsole.AddNewConsoleCommand(ReadScenes, "scenes", "Tests the Hue request",
					ConsoleAccessLevelEnum.AccessAdministrator);
				CrestronConsole.AddNewConsoleCommand(ReadSensors, "sensors", "Tests the Hue request",
					ConsoleAccessLevelEnum.AccessAdministrator);
				#endregion

				#region datastore cmds
				CrestronConsole.AddNewConsoleCommand(DsInit, "dsinit", "creates datastore",
					ConsoleAccessLevelEnum.AccessAdministrator);
				CrestronConsole.AddNewConsoleCommand(DsStore, "dsstore", "stores datastore",
	ConsoleAccessLevelEnum.AccessAdministrator);
				CrestronConsole.AddNewConsoleCommand(DsRead, "dsread", "reads datastore",
	ConsoleAccessLevelEnum.AccessAdministrator);
				CrestronConsole.AddNewConsoleCommand(DsDel, "dsdel", "deletes datastore",
	ConsoleAccessLevelEnum.AccessAdministrator);
				#endregion

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
				_hue = new HueProc();
			}
			catch (Exception e)
			{
				ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
			}
		}

		#region datastore

		void DsInit(string s)
		{
			try
			{
				CrestronDataStoreStatic.InitCrestronDataStore();
				CrestronDataStoreStatic.GlobalAccess = CrestronDataStore.CSDAFLAGS.OWNERREADWRITE;
			}
			catch (Exception e)
			{
				CrestronConsole.PrintLine("Exception is {0}", e);
			}			
		}

		void DsStore(string s)
		{
			if (CrestronDataStoreStatic.SetLocalStringValue("apikey", s) != CrestronDataStore.CDS_ERROR.CDS_SUCCESS)
				CrestronConsole.PrintLine("error storing apikey");
			CrestronConsole.PrintLine("Bridge registration complete");
		}

		void DsRead(string s)
		{
			string temp;
			if (CrestronDataStoreStatic.GetLocalStringValue("apikey", out temp) == CrestronDataStore.CDS_ERROR.CDS_SUCCESS &&
			    temp.Length > 30)
			{
				CrestronConsole.PrintLine("local key: {0}", temp);
			}
			else
			{
				CrestronConsole.PrintLine("No local API key stored");
			}
		}

		void DsDel(string s)
		{
			if(CrestronDataStoreStatic.SetLocalStringValue("apikey", null) != CrestronDataStore.CDS_ERROR.CDS_SUCCESS)
				CrestronConsole.PrintLine("Error removing API key");
		}

		#endregion 

		#region json data

		public void ReadBulbs(string s)
		{
			try
			{
				_bulbStream = new FileStream(@"\NVRAM\lights.txt", FileMode.Open, FileAccess.ReadWrite);
				_bulbReader = new StreamReader(_bulbStream);
				string jsonbulbs = _bulbReader.ReadToEnd();
				_bulbStream.Close();
				_bulbReader.Close();
				_hue.ProcBulbs(jsonbulbs);
			}
			catch (Exception e)
			{
				CrestronConsole.PrintLine(e.ToString());
			}
		}

		public void ReadGroups(string s)
		{
			try
			{
				_roomStream = new FileStream(@"\NVRAM\groups.txt", FileMode.Open, FileAccess.Read);
				_roomReader = new StreamReader(_roomStream);
				string jsonrooms = _roomReader.ReadToEnd();
				_roomStream.Close();
				_roomReader.Close();
				_hue.ProcRooms(jsonrooms);
			}
			catch (Exception e)
			{
				CrestronConsole.PrintLine(e.ToString());
			}
		}

		public void ReadScenes(string s)
		{
			try
			{
				_scenesStream = new FileStream(@"\NVRAM\Scenes.txt", FileMode.Open, FileAccess.Read);
				_scenesReader = new StreamReader(_scenesStream);
				string jsonscenes = _scenesReader.ReadToEnd();
				_scenesStream.Close();
				_scenesReader.Close();
				_hue.ProcScenes(jsonscenes);
			}
			catch (Exception e)
			{
				CrestronConsole.PrintLine(e.ToString());
			}
		}

		public void ReadSensors(string s)
		{
			try
			{
				_sensorStream = new FileStream(@"\NVRAM\sensors.txt", FileMode.Open, FileAccess.Read);
				_sensorReader = new StreamReader(_sensorStream);
				string jsonsensors = _sensorReader.ReadToEnd();
				_sensorStream.Close();
				_sensorReader.Close();
				_hue.ProcSensors(jsonsensors);
			}
			catch (Exception e)
			{
				CrestronConsole.PrintLine(e.ToString());
			}
		}

		#endregion 

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