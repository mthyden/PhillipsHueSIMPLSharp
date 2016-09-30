using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace HueLights
{
    public class HueGroup
    {
        public string RoomName;
        public string RoomType;
        public bool On;
        public uint Bri;
        public uint Hue;
        public uint Sat;
        public string Alert;
        public string AssignedLoad;
        public uint ScenesNum;
        public string[] loads = new string[20];
        public string[] SceneName = new string[20];
        public string[] SceneID = new string[20];

        public HueGroup(string roomname, string roomtype, bool on, uint bri, string alert, string load)
        {
            this.RoomName = roomname;
            this.RoomType = roomtype;
            this.On = on;
            this.Bri = bri;
            this.Alert = alert;
            this.AssignedLoad = load;
        }
    }
}