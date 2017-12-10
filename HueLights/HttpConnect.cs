using System;
using System.Collections.Generic;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharp.Net.Https;

namespace HueLights
{
	public sealed class HttpConnect	//Singleton for HTTP client
	{
		private static HttpConnect _Instance;
		private static readonly object _Lock = new object();
		private readonly HttpClient _client;
		private readonly HttpClientRequest _request;
		private HttpClientResponse _response;

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

		public string Request(string url, string cmd, Crestron.SimplSharp.Net.Http.RequestType reqtype)
		{
			_request.Url.Parse(url);
			_request.RequestType = reqtype;
			if (cmd == null)
			{
				_request.ContentString = "";
			}
			else
			{
				_request.ContentString = cmd;
			}
			_response = _client.Dispatch(_request);
			String jsontext = _response.ContentString;
			return jsontext;
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