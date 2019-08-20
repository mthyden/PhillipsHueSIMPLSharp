using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace HueLights
{
    public interface IHueItem
    {
        string Name { get; set; }
		bool Reachable { get; set; }
		string Type { get; set; }
		string Model { get; set; }
		string Manufacturer { get; set; }
		string Uid { get; set; }
		string SwVer { get; set; }
    }

    public class HueBulb : IHueItem
    {
		public string Name { get; set; }
		public bool Reachable { get; set; }
		public string Type { get; set; }
		public string Model { get; set; }
		public string Manufacturer { get; set; }
		public string Uid { get; set; }
		public string SwVer { get; set; }
		public bool On { get; set; }
		public uint Bri { get; set; }
		public string ColorMode { get; set; }
		public uint Hue { get; set; }
		public uint Sat { get; set; }
	    public uint Ct { get; set; }
		public string Alert { get; set; }
		public string Effect { get; set; }

        public HueBulb()
        {

        }
    }

    public class HueGroup
    {
		public string Name;
		public bool On;
		public uint Bri;
		public uint Hue;
		public uint Sat;
	    public uint Ct;
        public string RoomType;
        public string Alert;
	    public string ColorMode;
	    public string GroupClass;
        public string AssignedLoad;
        public uint ScenesNum;
        public string[] Loads = new string[20];
        public string[] SceneName = new string[20];
        public string[] SceneID = new string[20];

        public HueGroup(string roomname, string roomtype, bool on, uint bri, string load, string[] loads, string groupclass)
        {
            Name = roomname;
            RoomType = roomtype;
            On = on;
            Bri = bri;
            AssignedLoad = load;
            Loads = loads;
	        GroupClass = groupclass;
        }
    }

    public class HueScene
    {
		public string Name;
        public string[] Loads = new string[20];
        public string Group;

        public HueScene(string name, string[] loads)
        {
            Name = name;
            Loads = loads;
        }
    }

	public class HueSensor
	{
		public string Name;
		public string Type;
		public string Uid;
		public bool Daylight;
		public string LastUpdated;
		public bool Presence;
		public ushort Temp;
		public string Alert;
		public bool Reachable;
		public ushort Battery;

		public HueSensor(string uid, string name, string type)
		{
			Uid = uid;
			Name = name;
			Type = type;
		}
	}
}