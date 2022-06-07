using System;
using System.Collections.Generic;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharp.Net.Https;
using RequestType = Crestron.SimplSharp.Net.Http.RequestType;

namespace HueLights
{
	public sealed class HttpConnect	//Singleton for HTTP client
	{
		public event EventHandler<HueEventArgs> ResponseUpdated;
		public event EventHandler<HueEventArgs> RequestUpdated;

		private static HttpConnect _Instance;
		private static readonly object _Lock = new object();
		private readonly HttpClient _client;
		private readonly HttpClientRequest _request;
		private PayloadType _payloadType;
		private string _setType;
		private HueRequestId _id;

		public static HttpConnect Instance
		{
			get
			{
				lock (_Lock)
				{
					if (_Instance == null)
					{
						_Instance = new HttpConnect();
					}
				}
				return _Instance;
			}
		}

		private HttpConnect()
		{
			_client = new HttpClient();
			_client.KeepAlive = false;
			_client.Accept = "application/json";
			_request = new HttpClientRequest();
		}

		public string RequestInfo(string url, string cmd, RequestType req)
		{
				_request.Url.Parse(url);
				_request.RequestType = req;
				if (cmd == null)
				{
					_request.ContentString = "";
				}
				else
				{
					_request.ContentString = cmd;
				}
				var response =_client.Dispatch(_request);
				response.Encoding = Encoding.UTF8;
				String jsontext = response.ContentString;
			return jsontext;
		}

		public void RequestAllInfo(string url, HueRequestId id)
		{
			_request.Url.Parse(url);
			_request.RequestType = RequestType.Get;
			_id = id;
			_client.DispatchAsync(_request, OnRequestResponse);
		}

		public void SetCmd(string url, string cmd, string settype, PayloadType payloadtype)
		{
			_payloadType = payloadtype;
			_setType = settype;
			_request.Url.Parse(url);
			_request.RequestType = RequestType.Put;
			if (cmd == null)
			{
				_request.ContentString = "";
			}
			else
			{
				_request.ContentString = cmd;
			}
			_client.DispatchAsync(_request, OnTransferResponse);

		}

		private void OnRequestResponse(HttpClientResponse response, HTTP_CALLBACK_ERROR error)
		{
			if (response.Code == 200)
			{
				response.Encoding = Encoding.UTF8;
				OnRequestUpdated(response.ContentString, _id);
			}	
		}

		private void OnTransferResponse(HttpClientResponse response, HTTP_CALLBACK_ERROR error)
		{
			if (response.Code == 200)
			{
				response.Encoding = Encoding.UTF8;
				OnResponseUpdated(response.ContentString, _setType, _payloadType);
			}	
		}

		public void OnResponseUpdated(string response, string setType, PayloadType payloadType)
		{
			if(ResponseUpdated != null)
			ResponseUpdated(null, new HueEventArgs() { Response = response });
		}

		public void OnRequestUpdated(string response, HueRequestId id)
		{
			if(RequestUpdated != null)
			RequestUpdated(null, new HueEventArgs() {Response = response, Id = id});
		}
	}

	public sealed class HttpsConnect	//Singleton for HTTPS client
	{
		private static HttpsConnect _Instance;
		private static readonly object _Lock = new object();
		private readonly HttpsClient _client;
		private readonly HttpsClientRequest _request;
		private HttpsClientResponse _response;

		public static HttpsConnect Instance
		{
			get
			{
				lock (_Lock)
				{
					if (_Instance == null)
					{
						_Instance = new HttpsConnect();
					}
				}
				return _Instance;
			}
		}

		private HttpsConnect()
		{
			_client = new HttpsClient();
			_client.KeepAlive = false;
			_client.HostVerification = false;
			_client.PeerVerification = false;
			_client.Accept = "application/json";
			_request = new HttpsClientRequest();
		}

		public string Request(string url, string cmd)
		{
			_request.Url.Parse(url);
			if (cmd != null)
				_request.ContentString = cmd;
			_response = _client.Dispatch(_request);
			String jsontext = _response.ContentString;
			return jsontext;
		}
	}
}