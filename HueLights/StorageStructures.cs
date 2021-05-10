using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace HueLights
{
	public class InfoEventArgs : EventArgs
	{
		public String InfoType { get; set; }
		public String JsonData { get; set; }
	}

	public class HueEventArgs : EventArgs
	{
		public string Response;
	}

	public class HueInstanceEventArgs : EventArgs
	{
		public HueEventId HueEventId;
	}

	public enum HueEventId
	{
		On = 1,
		Bri = 2,
		Hue = 3,
		Sat = 4,
		Ct = 5,
		Alert = 6,
		Scene = 7
	}

	public enum PayloadType
	{
		RoomOnOff,
		BulbOnOff,
		Lvl,
		XY,
		Scene
	}

	public class Payload
	{
		private string _cmdtype, _settype;
		public string OnOff { get; set; }
		public ushort Lvl { get; set; }
		public string LvlType { get; set; }
		public string Effect { get; set; }
		public ushort Xval { get; set; }
		public ushort Yval { get; set; }
		public string Scene { get; set; }
		public string SetType
		{
			get { return _settype; }
			set
			{
				_settype = value;
				if (value == "lights")
					_cmdtype = "state";
				else if (value == "groups")
					_cmdtype = "action";
			}
		}
		public string CmdType
		{
			get { return _cmdtype; }

		}
	}
}