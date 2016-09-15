using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace HueLights
{
    public class HueBulb
    {
        public bool On;
        public uint Bri;
        public uint Hue;
        public uint Sat;
        public String Alert;
        public String Effect;
        public String ColorMode;
        public bool Reachable;
        public String Type;
        public String Name;
        public String Model;
        public String Manufacturer;
        public String Uid;
        public String SwVer;

        public HueBulb(bool on, uint bri, string alert, bool reachable, string type, string name, string model, string manufacturer, string uid, string swver)
        {
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

        public HueBulb(bool on, uint bri, uint hue, uint sat, string alert, bool reachable, string type, string name, string model, string manufacturer, string uid, string swver)
        {
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
}