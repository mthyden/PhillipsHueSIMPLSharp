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
		public event EventHandler<HueInstanceEventArgs> HueUpdated;
		public string Name { get; set; }
		public bool Reachable { get; set; }
		public string Type { get; set; }
		public string Model { get; set; }
		public string Manufacturer { get; set; }
		public string Uid { get; set; }
		public string SwVer { get; set; }
		public string ColorMode { get; set; }
		public string Alert { get; set; }
		public string Effect { get; set; }
		public bool Online;
		public bool On
		{
			get { return _on; }
			set
			{
				_on = value;
				if (Online)
					OnHueItemUpdated(HueEventId.On);
			}
		}
		public uint Bri
		{
			get { return _bri; }
			set
			{
				_bri = value;
				if (Online)
					OnHueItemUpdated(HueEventId.Bri);
			}
		}
		public uint Hue
		{
			get
			{
				return _hue;
			}
			set
			{
				_hue = value;
				if (Online)
					OnHueItemUpdated(HueEventId.Hue);
			}
		}
		public uint Ct
		{
			get
			{
				return _ct;
			}
			set
			{
				_ct = value;
				if (Online)
					OnHueItemUpdated(HueEventId.Ct);
			}
		}
		public uint Sat
		{
			get
			{
				return _sat;
			}
			set
			{
				_sat = value;
				if (Online)
					OnHueItemUpdated(HueEventId.Sat);
			}
		}

		private bool _on;
		private uint _bri;
		private uint _hue;
		private uint _sat;
		private uint _ct;

        public HueBulb()
        {

        }

		public void OnHueItemUpdated(HueEventId id)
		{
			if (id != 0)
				HueUpdated(null, new HueInstanceEventArgs() { HueEventId = id });
		}
    }

    public class HueGroup
    {
	    public event EventHandler<HueInstanceEventArgs> HueUpdated;
		public string Name;
	    public bool Online;
		public bool On {
			get { return _on; }
			set
			{
				_on = value;
				if(Online)
					OnHueItemUpdated(HueEventId.On);
			}
		}
	    public uint Bri
	    {
		    get { return _bri; }
		    set
		    {
			    _bri = value;
				if (Online)
					OnHueItemUpdated(HueEventId.Bri);
		    }
	    }
	    public uint Hue
	    {
		    get
		    {
			    return _hue;
		    }
		    set
		    {
				_hue = value;
				if (Online)
					OnHueItemUpdated(HueEventId.Hue);
		    }
	    }
		public uint Ct
		{
			get
			{
				return _ct;
			}
			set
			{
				_ct = value;
				if (Online)
					OnHueItemUpdated(HueEventId.Ct);
			}
		}
		public uint Sat
		{
			get
			{
				return _sat;
			}
			set
			{
				_sat = value;
				if (Online)
					OnHueItemUpdated(HueEventId.Sat);
			}
		}
        public string RoomType;
        public string Alert { get; set; }
	    public string ColorMode;
	    public string GroupClass;
        public string AssignedLoad;
        public uint ScenesNum;
        public string[] Loads = new string[20];
	    public List<HueScene> Scenes;

	    private bool _on;
	    private uint _bri;
	    private uint _hue;
	    private uint _sat;
	    private uint _ct;

        public HueGroup(string roomname, string roomtype, bool on, uint bri, string load, string[] loads, string groupclass)
        {
            Name = roomname;
            RoomType = roomtype;
            On = on;
            Bri = bri;
            AssignedLoad = load;
            Loads = loads;
	        GroupClass = groupclass;
			Scenes = new List<HueScene>();
        }

		public void OnHueItemUpdated(HueEventId id)
	    {
			if(id != 0)
		    HueUpdated(null, new HueInstanceEventArgs() {HueEventId = id});
	    }
    }

    public class HueScene
    {
		public string Name;
        public string Group;
	    public string SceneId;
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