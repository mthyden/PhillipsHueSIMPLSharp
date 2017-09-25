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
        public String Alert;
        public String Effect;
        public String ColorMode;
        public bool Reachable;
        public String Type;
        public String Model;
        public String Manufacturer;
        public String Uid;
        public String SwVer;

        public HueBulb(string id, bool on, uint bri, string alert, bool reachable, string type, string name, string model, string manufacturer, string uid, string swver)
        {
            this.Id = id;
            this.On = on;
            this.Bri = bri;
            this.Alert = alert;
            this.Reachable = reachable;
            this.Type = type;
            this.Name = name;
            this.Model = model;
            this.Manufacturer = manufacturer;
            this.Uid = uid;
            this.SwVer = swver;
        }

        public HueBulb(string id, bool on, uint bri, uint hue, uint sat, string alert, bool reachable, string type, string name, string model, string manufacturer, string uid, string swver)
        {
            this.Id = id;
            this.On = on;
            this.Bri = bri;
            this.Hue = hue;
            this.Sat = sat;
            this.Alert = alert;
            this.Reachable = reachable;
            this.Type = type;
            this.Name = name;
            this.Model = model;
            this.Manufacturer = manufacturer;
            this.Uid = uid;
            this.SwVer = swver;
        }
    }

    public class HueGroup : HueItem
    {
        public string RoomType;
        public string Alert;
        public string AssignedLoad;
        public uint ScenesNum;
        public string[] Loads = new string[20];
        public string[] SceneName = new string[20];
        public string[] SceneID = new string[20];

        public HueGroup(string id, string roomname, string roomtype, bool on, uint bri, string alert, string load, string[] loads)
        {
            this.Id = id;
            this.Name = roomname;
            this.RoomType = roomtype;
            this.On = on;
            this.Bri = bri;
            this.Alert = alert;
            this.AssignedLoad = load;
            this.Loads = loads;
        }
    }

    public class HueScene : HueItem
    {
        public string[] Loads = new string[20];
        public string Group;

        public HueScene(string id, string name, string[] loads)
        {
            this.Id = id;
            this.Name = name;
            this.Loads = loads;
        }
    }

}