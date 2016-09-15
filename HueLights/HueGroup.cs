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
        public string Alert;
        public string AssignedLoad;
        public string[] SceneName = new string[5];
        public string[] SceneID = new string[5];

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