using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json.Linq;
using Crestron.SimplSharp.CrestronDataStore;

namespace HueLights
{
    public static class HueBridge
    {
        public static event EventHandler<InfoEventArgs> InfoReceived;  //event handler indicating data received 
	    //public static event EventHandler<HueEventArgs> CmdReceived;

	    public static bool LocalKey;
        public static bool Authorized;
        public static bool Populated;
        public static string BridgeIp;
        public static string BridgeApi;
	    private static string _url;
	    private static string _cmd;
	    private static string _response;

		public static Dictionary<string, HueBulb> HueBulbs = new Dictionary<string, HueBulb>();
        public static Dictionary<string, HueGroup> HueGroups = new Dictionary<string, HueGroup>();
		public static Dictionary<string, HueSensor> HueSensors = new Dictionary<string, HueSensor>();
		//public static List<IHueItem> HueBulbs = new List<IHueItem>();
        
        /// <summary>
        /// registers with bridge, authorizes a user based on API key from the pairing
        /// </summary>
        public static void Register()
        {
            CrestronConsole.PrintLine("registering with bridge...");
            try
            {
				_url = string.Format("http://{0}/api", BridgeIp);
	            _cmd = "{\"devicetype\":\"my_hue_app#crestron\"}";
				_response = HttpConnect.Instance.RequestInfo(_url, _cmd, Crestron.SimplSharp.Net.Http.RequestType.Post);
				if (_response.Contains("link button not pressed"))
                {
                    Authorized = false;
                    CrestronConsole.PrintLine("Registration incomplete press button and retry...");
                }
				else if (_response.Contains("username"))
                {
                    Authorized = true;
					var data = JArray.Parse(_response);
                    BridgeApi = (String)data[0]["success"]["username"];
                    //CrestronConsole.PrintLine("API key is {0}",BridgeApi);
                    if (CrestronDataStoreStatic.SetLocalStringValue("apikey", BridgeApi) != CrestronDataStore.CDS_ERROR.CDS_SUCCESS)
                        CrestronConsole.PrintLine("error storing apikey");
                    CrestronConsole.PrintLine("Bridge registration complete");
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        /// <summary>
        /// sets up datastore for storing the bridge API key
        /// </summary>
        public static void SetupDataStore()
        {
            try
            {
                var err = CrestronDataStoreStatic.InitCrestronDataStore();
				CrestronConsole.PrintLine("DataStore status: {0}", err);
                CrestronDataStoreStatic.GlobalAccess = CrestronDataStore.CSDAFLAGS.OWNERREADWRITE;
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
        }

        /// <summary>
        /// pulls API key from datastore
        /// </summary>
        /// <returns></returns>
        public static string GetDataStore()
        {
            string temp;
	        if (CrestronDataStoreStatic.GetLocalStringValue("apikey", out temp) == CrestronDataStore.CDS_ERROR.CDS_SUCCESS && temp.Length > 30)
	        {
				//CrestronConsole.PrintLine("local key: {0}",temp);
		        LocalKey = true;
	        }
	        else
	        {
				CrestronConsole.PrintLine("No local API key stored");
				LocalKey = false;
	        }

            return temp;
        }

		/// <summary>
		/// resets the datastore
		/// </summary>
        public static void ResetDataStore()
		{
			if (CrestronDataStoreStatic.clearLocal("apikey") != CrestronDataStore.CDS_ERROR.CDS_SUCCESS)
				CrestronConsole.PrintLine("Error removing API key");
		}

        /// <summary>
        /// gets the IP of the local bridge, currently one bridge is supported
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            try
            {
				_url = "https://discovery.meethue.com";
	            _response = HttpsConnect.Instance.Request(_url, null);
                /*
                 [{"id":"001788fffe2ad33b","internalipaddress":"172.22.131.242"}]
                 */
				JArray BridgeArray = JArray.Parse(_response);
                BridgeIp = (String)BridgeArray[0].SelectToken("internalipaddress");
                //BridgeApi = "U8FEH-CRuHFGxXe59pitg6UeyqGKWnMsqHef8oMt";
                CrestronConsole.PrintLine("Get IP of Bridge complete...");
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
            }
            return BridgeIp;
        }

	    public static void RegisterEvents()
	    {
			HttpConnect.Instance.ResponseUpdated += Instance_ResponseUpdated;
	    }

        /// <summary>
        /// generic request for lights, groups, scenes returns a JSON string to be parsed
        /// </summary>
        /// <param name="infotype"></param>
        /// <returns></returns>
        public static void GetBridgeInfo(string infotype)
        {
            try
            {
				_url = string.Format("http://{0}/api/{1}/{2}", BridgeIp, BridgeApi, infotype);
				string json = HttpConnect.Instance.RequestInfo(_url, null, RequestType.Get);
				OnInfoReceived(infotype, json);
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception: {0}",e);
            }
        }

		/// <summary>
		/// determines command type, formats string and sends to HTTPConnect instance
		/// </summary>
		/// <param name="payloadtype"></param>
		/// <param name="payload"></param>
		/// <param name="setid"></param>
		/// <returns></returns>
        public static void SetCmd(PayloadType payloadtype, Payload payload, ushort setid)
        {
				_url = string.Format("http://{0}/api/{1}/{2}/{3}/{4}", BridgeIp, BridgeApi, payload.SetType, setid, payload.CmdType);
				switch (payloadtype)
				{
					case PayloadType.RoomOnOff:
						{
							_cmd = String.Format("{0}\"on\":{1},\"effect\":\"{2}\"{3}", '{', payload.OnOff, payload.Effect, '}');
							break;
						}
					case PayloadType.BulbOnOff:
						{
							_cmd = String.Format("{0}\"on\":{1}{2}", '{', payload.OnOff, '}');
							break;
						}
					case PayloadType.Lvl:
						{
							_cmd = "{\"" + payload.LvlType + "\":" + payload.Lvl.ToString() + "}";
							break;
						}
					case PayloadType.XY:
						{
							decimal x = (decimal)payload.Xval / 100;
							decimal y = (decimal)payload.Yval / 100;
							_cmd = "{\"xy\":[" + x + "," + y + "]}";
							break;
						}
					case PayloadType.Scene:
						{
							_cmd = String.Format("{0}\"scene\":\"{1}\"{2}", '{', payload.Scene, '}');
							break;
						}
				}
				HttpConnect.Instance.SetCmd(_url, _cmd, payload.SetType, payloadtype);
        }

		static void Instance_ResponseUpdated(object sender, HueEventArgs e)
		{
			var setType = "";
			var id = "";
			var cmd = "";
			var json = JArray.Parse(e.Response);
			foreach (var jobj in json)
			{
				if (jobj.SelectToken("success") != null)
				{
					var jtok = jobj["success"].First;
					var key = ((JProperty)jtok).Name;
					var value = ((JProperty)jtok).Value;
					var values = key.Split('/');
					setType = values[1];
					id = values[2];
					cmd = values[4];
					ProcessCmdResponse(setType, id, cmd, value);
				}
			}
		}

	    private static void ProcessCmdResponse(string type, string id, string cmd, JToken value)
	    {
		    switch (type)
		    {
				case "groups":
			    {
				    switch (cmd)
				    {
						case "on":
					    {
						    HueGroups[id].On = (bool)value;
						    break;
					    }
						case "bri":
						{
							HueGroups[id].Bri = (ushort)value;
							break;
						}
						case "hue":
						{
							HueGroups[id].Hue = (uint)value;
							break;
						}
						case "sat":
						{
							HueGroups[id].Sat = (uint)value;
							break;
						}
						case "ct":
						{
							HueGroups[id].Ct = (uint)value;
							break;
						}
						case "effect":
					    {
							///todo add effect property
						    break;
					    }
				    }
				    break;
			    }
				case "lights":
			    {
					switch (cmd)
					{
						case "on":
							{
								HueBulbs[id].On = (bool)value;
								break;
							}
						case "bri":
							{
								HueBulbs[id].Bri = (ushort)value;
								break;
							}
						case "hue":
							{
								HueBulbs[id].Hue = (uint)value;
								break;
							}
						case "sat":
							{
								HueBulbs[id].Sat = (uint)value;
								break;
							}
						case "ct":
							{
								HueBulbs[id].Ct = (uint)value;
								break;
							}
					}
				    break;
			    }
		    }
	    }

		/// <summary>
		/// raises the event for received data
		/// </summary>
		/// <param name="infotype"></param>
		/// <param name="jsondata"></param>
        static void OnInfoReceived(string infotype, string jsondata)
        {
            if(infotype != null)
                InfoReceived(null, new InfoEventArgs(){InfoType = infotype, JsonData = jsondata}); //event delegate
        }
    }
}