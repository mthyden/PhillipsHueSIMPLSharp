using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Https;
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Crestron.SimplSharp.CrestronDataStore;

namespace HueLights
{
    public static class HueBridge
    {
        public static ushort Authorized;
        public static string BridgeIp;
        public static string BridgeApi;
        
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
                    Authorized = 0;
                    CrestronConsole.PrintLine("Registration incomplete press button and retry...");
                }
                else if (jsontext.Contains("username"))
                {
                    Authorized = 1;
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

        public static string GetDataStore()
        {
            string temp;
            if (CrestronDataStoreStatic.GetLocalStringValue("apikey", out temp) != CrestronDataStore.CDS_ERROR.CDS_SUCCESS)
                CrestronConsole.PrintLine("error getting apikey");
            return temp;
        }

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

        public static string GetBridgeInfo(string infotype)
        {
            var getLights = new HttpClient();
            getLights.KeepAlive = false;
            getLights.Accept = "application/json";
            HttpClientRequest bridgeRequest = new HttpClientRequest();
            string url = string.Format("http://{0}/api/{1}/{2}", HueBridge.BridgeIp, HueBridge.BridgeApi, infotype);
            bridgeRequest.Url.Parse(url);
            HttpClientResponse lResponse = getLights.Dispatch(bridgeRequest);
            String jsontext = lResponse.ContentString;
            return jsontext;
        }

        public static string SetLights(string settype, ushort setid, string value, string cmdtype)
        {
            var setLights = new HttpClient();
            setLights.KeepAlive = false;
            setLights.Accept = "application/json";
            HttpClientRequest lightRequest = new HttpClientRequest();
            string url = string.Format("http://{0}/api/{1}/{2}/{3}/{4}", HueBridge.BridgeIp, HueBridge.BridgeApi, settype, setid, cmdtype);
            lightRequest.RequestType = Crestron.SimplSharp.Net.Http.RequestType.Put;
            lightRequest.Url.Parse(url);
            String payload = String.Format("{0}\"on\":{1}{2}",'{', value, '}');
            lightRequest.ContentString = payload;
            HttpClientResponse lResponse = setLights.Dispatch(lightRequest);
            String jsontext = lResponse.ContentString;
            return jsontext;
        }

        public static string SetScene(string groupid, ushort sceneid, string payload)
        {
            var setLights = new HttpClient();
            setLights.KeepAlive = false;
            setLights.Accept = "application/json";
            HttpClientRequest lightRequest = new HttpClientRequest();
            string url = string.Format("http://{0}/api/{1}/groups/{2}/action", HueBridge.BridgeIp, HueBridge.BridgeApi, groupid);
            lightRequest.RequestType = Crestron.SimplSharp.Net.Http.RequestType.Put;
            lightRequest.Url.Parse(url);
            lightRequest.ContentString = payload;
            HttpClientResponse lResponse = setLights.Dispatch(lightRequest);
            String jsontext = lResponse.ContentString;
            return jsontext;
        }

        public static string SetBri(string settype, string lvltype, ushort setid, ushort value)
        {
            var setLights = new HttpClient();
            setLights.KeepAlive = false;
            setLights.Accept = "application/json";
            HttpClientRequest lightRequest = new HttpClientRequest();
            string url = string.Format("http://{0}/api/{1}/lights/{2}/state", HueBridge.BridgeIp, HueBridge.BridgeApi, setid);
            lightRequest.RequestType = Crestron.SimplSharp.Net.Http.RequestType.Put;
            lightRequest.Url.Parse(url);
            string tempVal = "{\"" + lvltype + "\":" + value.ToString() + "}";
            lightRequest.ContentString = tempVal;
            HttpClientResponse lResponse = setLights.Dispatch(lightRequest);
            String jsontext = lResponse.ContentString;
            return jsontext;
        }
    }
}