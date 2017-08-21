
namespace HueLights
{
    public class HueGroup
    {
        public string RoomName;
        public ushort RoomID;
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

        public HueGroup(ushort roomid, string roomname, string roomtype, bool on, uint bri, string alert, string load, string[] loads)
        {
            this.RoomID = roomid;
            this.RoomName = roomname;
            this.RoomType = roomtype;
            this.On = on;
            this.Bri = bri;
            this.Alert = alert;
            this.AssignedLoad = load;
            this.loads = loads;
        }
    }
}