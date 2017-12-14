using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace HueLights
{
    public class HueItem
    {
        public string Id;
        public string Name;
    }

    public class HueBulb : HueItem
    {
		public bool On;
		public uint Bri;
		public uint Hue;
		public uint Sat;
	    public uint Ct;
		public string Alert;
		public string Effect;
		public string ColorMode;
        public bool Reachable;
		public string Type;
		public string Model;
		public string Manufacturer;
		public string Uid;
        public string SwVer;

        public HueBulb(string id, bool on, uint bri, string type, string name, string model, string manufacturer, string uid, string swver, bool reachable)
        {
            Id = id;
            On = on;
            Bri = bri;
            Type = type;
            Name = name;
            Model = model;
            Manufacturer = manufacturer;
            Uid = uid;
            SwVer = swver;
	        Reachable = reachable;
        }

        public HueBulb(string id, bool on, uint bri, uint hue, uint sat, uint ct, string type, string name, string model, string manufacturer, string uid, string swver, string colormode, bool reachable)
        {
            Id = id;
            On = on;
            Bri = bri;
            Hue = hue;
            Sat = sat;
	        Ct = ct;
            Type = type;
            Name = name;
            Model = model;
	        ColorMode = colormode;
            Manufacturer = manufacturer;
            Uid = uid;
            SwVer = swver;
	        Reachable = reachable;
        }
    }

    public class HueGroup : HueItem
    {
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

        public HueGroup(string id, string roomname, string roomtype, bool on, uint bri, string load, string[] loads, string groupclass)
        {
            Id = id;
            Name = roomname;
            RoomType = roomtype;
            On = on;
            Bri = bri;
            AssignedLoad = load;
            Loads = loads;
	        GroupClass = groupclass;
        }
    }

    public class HueScene : HueItem
    {
        public string[] Loads = new string[20];
        public string Group;

        public HueScene(string id, string name, string[] loads)
        {
            Id = id;
            Name = name;
            Loads = loads;
        }
    }

	public class HueSensor : HueItem
	{
		public string Type;
		public string Uid;
		public bool Daylight;
		public string LastUpdated;
		public bool Presence;
		public ushort Temp;
		public string Alert;
		public bool Reachable;
		public ushort Battery;

		public HueSensor(string id, string uid, string name, string type)
		{
			Id = id;
			Uid = uid;
			Name = name;
			Type = type;
		}
	}
}