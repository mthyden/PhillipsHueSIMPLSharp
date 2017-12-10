using System;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace HueLights
{
	public class HueMotion
	{
		public ushort MotionId;
		public string MotionName;
		public ushort MotionOnline;
		public ushort MotionDaylight;
		public ushort MotionPresence;
		public ushort MotionTemp;

		//^^^^^ Signals for SIMPL+ ^^^^^^^^

		private string _url;
		private string _jsontext;
		private JObject _json;
		private bool _foundsensor;

		public event EventHandler PresenceUpdate;


		public HueMotion()
		{

		}

		public void MotionInit()
		{
			_foundsensor = false;
			MotionOnline = 0;
			if (HueBridge.Populated == true)
			{
				foreach (var sensor in HueBridge.HueSensors)
				{
					if (sensor.Name == MotionName)
					{
						MotionId = Convert.ToUInt16(sensor.Id);
						_foundsensor = true;
						GetMotion();
						break;
					}
				}
				if (_foundsensor == false)
				{
					CrestronConsole.PrintLine("Sensor not found: {0}",MotionName);
				}
			}
		}

		public void GetMotion()
		{
			try
			{
				if (_foundsensor == true)
				{
					_url = string.Format("http://{0}/api/{1}/{2}/{3}", HueBridge.BridgeIp, HueBridge.BridgeApi, "sensors", MotionId);
					_jsontext = HttpConnect.Instance.Request(_url, null, Crestron.SimplSharp.Net.Http.RequestType.Get);
					_json = JObject.Parse(_jsontext);
					HueBridge.HueSensors[MotionId - 1].Presence = (bool)_json["state"]["presence"];
					HueBridge.HueSensors[MotionId - 1].Battery = (ushort)_json["config"]["battery"];
					HueBridge.HueSensors[MotionId - 1].Reachable = (bool)_json["config"]["reachable"];
					MotionPresence = (ushort)(HueBridge.HueSensors[MotionId - 1].Presence ? 1 : 0);
					MotionOnline = (ushort)(HueBridge.HueSensors[MotionId - 1].Reachable ? 1 : 0);
					TriggerPresenceUpdate();
				}
				else
				{
					CrestronConsole.PrintLine("Sensor not online: {0}", MotionName);
				}
			}
			catch (Exception e)
			{

				CrestronConsole.PrintLine("Error getting Sensor data: {0}", e);
			}

		}

		public void TriggerPresenceUpdate()
		{
			PresenceUpdate(this, new EventArgs());
		}
	}
}