using System;
using System.Collections.Generic;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Https;
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json.Linq;
using Crestron.SimplSharp.CrestronDataStore;

namespace HueLights
{
    public class InfoEventArgs : EventArgs
    {
        public String InfoType { get; set; }
        public String JsonData { get; set; }
    }

    public enum PayloadType
    {
        OnOff,
        Lvl,
        XY,
        Scene
    }

    public class Payload
    {
        private string _settype, _cmdtype;
        public string OnOff { get; set; }
        public ushort Lvl { get; set; }
        public string LvlType { get; set; }
        public string Effect { get; set; }
        public ushort Xval { get; set; }
        public ushort Yval { get; set; }
        public string Scene { get; set; }
        public string SetType
        {
            get { return this._settype; }
            set
            {
                this._settype = value;
                if (value == "lights")
                    _cmdtype = "state";
                else if (value == "groups")
                    _cmdtype = "action";
            }
        }
        public string CmdType
        {
            get { return this._cmdtype; }

        }
    }

    public static class HueBridge
    {
        public static event EventHandler<InfoEventArgs> InfoReceived;  //event handler indicating data received 

        public static bool Authorized;
        public static bool Populated;
        public static string BridgeIp;
        public static string BridgeApi;

        public static List<HueBulb> HueBulbs = new List<HueBulb>();
        public static List<HueGroup> HueGroups = new List<HueGroup>();
        public static List<HueScene> HueScenes = new List<HueScene>();
        
        /// <summary>
        /// registers with bridge, authorizes a user based on API key from the pairing
        /// </summary>
        public static void register()
        {
            CrestronConsole.PrintLine("registering with bridge...");
            try
            {
                var registerBridge = new HttpClient();
                registerBridge.KeepAlive = false;
                registerBridge.Accept = "application/json";
                HttpClientRequest bridgeRequest = new HttpClientRequest();
                string url = string.Format("http://{0}/api", BridgeIp);
                bridgeRequest.RequestType = Crestron.SimplSharp.Net.Http.RequestType.Post;
                bridgeRequest.Url.Parse(url);
                bridgeRequest.ContentString = "{\"devicetype\":\"my_hue_app#crestron\"}";
                HttpClientResponse lResponse = registerBridge.Dispatch(bridgeRequest);
                var jsontext = lResponse.ContentString;
                CrestronConsole.PrintLine("response is {0}", jsontext);
                if (jsontext.Contains("link button not pressed"))
                {
                    Authorized = false;
                    CrestronConsole.PrintLine("Registration incomplete press button and retry...");
                }
                else if (jsontext.Contains("username"))
                {
                    Authorized = true;
                    JArray data = JArray.Parse(jsontext);
                    BridgeApi = (String)data[0]["success"]["username"];
                    CrestronConsole.PrintLine("API key is {0}",BridgeApi);
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
                CrestronDataStoreStatic.InitCrestronDataStore();
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
            if (CrestronDataStoreStatic.GetLocalStringValue("apikey", out temp) != CrestronDataStore.CDS_ERROR.CDS_SUCCESS)
                CrestronConsole.PrintLine("error getting apikey");
            return temp;
        }

        public static void ResetDataStore()
        {
           if(CrestronDataStoreStatic.SetLocalStringValue("apikey", null) != CrestronDataStore.CDS_ERROR.CDS_SUCCESS)
               CrestronConsole.PrintLine("Error removing API key");
        }

        /// <summary>
        /// gets the IP of the local bridge, currently one bridge is supported
        /// </summary>
        /// <returns></returns>
        public static string getIP()
        {
            try
            {
                var getBridge = new HttpsClient();
                getBridge.KeepAlive = false;
                getBridge.Accept = "application/json";
                getBridge.HostVerification = false;
                getBridge.PeerVerification = false;
                HttpsClientRequest bridgeRequest = new HttpsClientRequest();
                bridgeRequest.Url.Parse("https://www.meethue.com/api/nupnp");
                HttpsClientResponse lResponse = getBridge.Dispatch(bridgeRequest);
                String jsontext = lResponse.ContentString;
                /*
                 [{"id":"001788fffe2ad33b","internalipaddress":"172.22.131.242"}]
                 */
                JArray BridgeArray = JArray.Parse(jsontext);
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

        /// <summary>
        /// generic request for lights, groups, scenes returns a JSON string to be parsed
        /// </summary>
        /// <param name="infotype"></param>
        /// <returns></returns>
        public static void GetBridgeInfo(string infotype)
        {
            try
            {
                var getLights = new HttpClient();
                getLights.KeepAlive = false;
                getLights.Accept = "application/json";
                HttpClientRequest bridgeRequest = new HttpClientRequest();
                string url = string.Format("http://{0}/api/{1}/{2}", HueBridge.BridgeIp, HueBridge.BridgeApi, infotype);
                bridgeRequest.Url.Parse(url);
                HttpClientResponse lResponse = getLights.Dispatch(bridgeRequest);
                String jsontext = lResponse.ContentString;
                OnInfoReceived(infotype, jsontext);
                //CrestronConsole.PrintLine("getting bridge info: {0}", infotype);
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception: {0}",e);
            }
        }

        public static string SetCmd(PayloadType payloadtype, Payload payload, ushort setid)
        {
            string url = "";
            url = string.Format("http://{0}/api/{1}/{2}/{3}/{4}", HueBridge.BridgeIp, HueBridge.BridgeApi, payload.SetType, setid, payload.CmdType);
            string cmdval = "";
            switch (payloadtype)
            {
                case PayloadType.OnOff:
                {
                    cmdval = String.Format("{0}\"on\":{1},\"effect\":\"{2}\"{3}", '{', payload.OnOff, payload.Effect, '}');
                    break;
                }
                case PayloadType.Lvl:
                {
                    cmdval = "{\"" + payload.LvlType + "\":" + payload.Lvl.ToString() + "}";
                    break;
                }
                case PayloadType.XY:
                {
                    decimal x = (decimal)payload.Xval / 100;
                    decimal y = (decimal)payload.Yval / 100;
                    cmdval = "{\"xy\":[" + x.ToString() + "," + y.ToString() + "]}";
                    break;
                }
                case PayloadType.Scene:
                {
                    cmdval = String.Format("{0}\"scene\":\"{1}\"{2}", '{', payload.Scene, '}');
                    //CrestronConsole.PrintLine("cmdval: {0}", cmdval);
                    break;
                }
            }
            var setLights = new HttpClient();
            setLights.KeepAlive = false;
            setLights.Accept = "application/json";
            HttpClientRequest lightRequest = new HttpClientRequest();
            lightRequest.RequestType = Crestron.SimplSharp.Net.Http.RequestType.Put;
            lightRequest.Url.Parse(url);
            lightRequest.ContentString = cmdval;
            HttpClientResponse lResponse = setLights.Dispatch(lightRequest);
            String jsontext = lResponse.ContentString;
            return jsontext;

        }

        static void OnInfoReceived(String infotype, string jsondata)
        {
            if(infotype != null)
                InfoReceived(null, new InfoEventArgs(){InfoType = infotype, JsonData = jsondata});
        }
    }
}

/*
        /// <summary>
        /// generic on/off method for individual bulbs or groups
        /// </summary>
        /// <param name="settype">"lights" or "group"</param>
        /// <param name="setid">ID of group or light</param>
        /// <param name="value">body argument, ie "on" or "off"</param>
        /// <param name="cmdtype">command type</param>
        /// <returns></returns>
        public static string SetOnOff(string settype, ushort setid, string value, string cmdtype, string effect)
        {
            try
            {
                var setLights = new HttpClient();
                setLights.KeepAlive = false;
                setLights.Accept = "application/json";
                HttpClientRequest lightRequest = new HttpClientRequest();
                string url = string.Format("http://{0}/api/{1}/{2}/{3}/{4}", HueBridge.BridgeIp, HueBridge.BridgeApi, settype, setid, cmdtype);
                lightRequest.RequestType = Crestron.SimplSharp.Net.Http.RequestType.Put;
                lightRequest.Url.Parse(url);
                String payload = String.Format("{0}\"on\":{1},\"effect\":\"{2}\"{3}", '{', value, effect, '}');
                lightRequest.ContentString = payload;
                HttpClientResponse lResponse = setLights.Dispatch(lightRequest);
                String jsontext = lResponse.ContentString;
                return jsontext;
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Exception is {0}", e);
                return e.ToString();
            }
        }

        /// <summary>
        /// sets scene method
        /// </summary>
        /// <param name="setid"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string SetScene(ushort setid, string payload)
        {
                var setLights = new HttpClient();
                setLights.KeepAlive = false;
                setLights.Accept = "application/json";
                HttpClientRequest lightRequest = new HttpClientRequest();
                string url = string.Format("http://{0}/api/{1}/groups/{2}/action", HueBridge.BridgeIp, HueBridge.BridgeApi, setid);
                lightRequest.RequestType = Crestron.SimplSharp.Net.Http.RequestType.Put;
                lightRequest.Url.Parse(url);
                lightRequest.ContentString = payload;
                HttpClientResponse lResponse = setLights.Dispatch(lightRequest);
                String jsontext = lResponse.ContentString;
                return jsontext;
        }

        public static string SetLvl(string settype, ushort setid, string cmdtype, string cmdval )
        {
            var setLights = new HttpClient();
            setLights.KeepAlive = false;
            setLights.Accept = "application/json";
            HttpClientRequest lightRequest = new HttpClientRequest();
            string url = string.Format("http://{0}/api/{1}/{2}/{3}/{4}", HueBridge.BridgeIp, HueBridge.BridgeApi, settype, setid, cmdtype);
            lightRequest.RequestType = Crestron.SimplSharp.Net.Http.RequestType.Put;
            lightRequest.Url.Parse(url);
            lightRequest.ContentString = cmdval;
            HttpClientResponse lResponse = setLights.Dispatch(lightRequest);
            String jsontext = lResponse.ContentString;
            return jsontext;
        }
*/
