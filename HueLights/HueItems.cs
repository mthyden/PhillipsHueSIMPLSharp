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
        public bool On;
        public uint Bri;
        public uint Hue;
        public uint Sat;
    }

    public class HueBulb : HueItem
    {
		public string Alert;
		public string Effect;
		public string ColorMode;
        public bool Reachable;
		public string Type;
		public string Model;
		public string Manufacturer;
		public string Uid;
        public string SwVer;

        public HueBulb(string id, bool on, uint bri, string alert, bool reachable, string type, string name, string model, string manufacturer, string uid, string swver)
        {
            Id = id;
            On = on;
            Bri = bri;
            Alert = alert;
            Reachable = reachable;
            Type = type;
            Name = name;
            Model = model;
            Manufacturer = manufacturer;
            Uid = uid;
            SwVer = swver;
        }

        public HueBulb(string id, bool on, uint bri, uint hue, uint sat, string alert, bool reachable, string type, string name, string model, string manufacturer, string uid, string swver)
        {
            Id = id;
            On = on;
            Bri = bri;
            Hue = hue;
            Sat = sat;
            Alert = alert;
            Reachable = reachable;
            Type = type;
            Name = name;
            Model = model;
            Manufacturer = manufacturer;
            Uid = uid;
            SwVer = swver;
        }
    }

    public class HueGroup : HueItem
    {
        public string RoomType;
        public string Alert;
	    public string GroupClass;
        public string AssignedLoad;
        public uint ScenesNum;
        public string[] Loads = new string[20];
        public string[] SceneName = new string[20];
        public string[] SceneID = new string[20];

        public HueGroup(string id, string roomname, string roomtype, bool on, uint bri, string alert, string load, string[] loads, string groupclass)
        {
            Id = id;
            Name = roomname;
            RoomType = roomtype;
            On = on;
            Bri = bri;
            Alert = alert;
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

}