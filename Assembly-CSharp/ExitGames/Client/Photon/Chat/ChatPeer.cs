using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x020007B0 RID: 1968
	public class ChatPeer : PhotonPeer
	{
		// Token: 0x06003FA3 RID: 16291 RVA: 0x0013FF88 File Offset: 0x0013E388
		public ChatPeer(IPhotonPeerListener listener, ConnectionProtocol protocol) : base(listener, protocol)
		{
			this.ConfigUnitySockets();
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06003FA4 RID: 16292 RVA: 0x0013FF98 File Offset: 0x0013E398
		public string NameServerAddress
		{
			get
			{
				return this.GetNameServerAddress();
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06003FA5 RID: 16293 RVA: 0x0013FFA0 File Offset: 0x0013E3A0
		internal virtual bool IsProtocolSecure
		{
			get
			{
				return base.UsedProtocol == ConnectionProtocol.WebSocketSecure;
			}
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0013FFAC File Offset: 0x0013E3AC
		[Conditional("UNITY")]
		private void ConfigUnitySockets()
		{
			Type type = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp", false);
			if (type == null)
			{
				type = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp-firstpass", false);
			}
			if (type != null)
			{
				this.SocketImplementationConfig[ConnectionProtocol.WebSocket] = type;
				this.SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = type;
			}
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0013FFF8 File Offset: 0x0013E3F8
		private string GetNameServerAddress()
		{
			int num = 0;
			ChatPeer.ProtocolToNameServerPort.TryGetValue(base.TransportProtocol, out num);
			switch (base.TransportProtocol)
			{
			case ConnectionProtocol.Udp:
			case ConnectionProtocol.Tcp:
				return string.Format("{0}:{1}", "ns.exitgames.com", num);
			case ConnectionProtocol.WebSocket:
				return string.Format("ws://{0}:{1}", "ns.exitgames.com", num);
			case ConnectionProtocol.WebSocketSecure:
				return string.Format("wss://{0}:{1}", "ns.exitgames.com", num);
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0014008B File Offset: 0x0013E48B
		public bool Connect()
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "Connecting to nameserver " + this.NameServerAddress);
			}
			return this.Connect(this.NameServerAddress, "NameServer");
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x001400C8 File Offset: 0x0013E4C8
		public bool AuthenticateOnNameServer(string appId, string appVersion, string region, AuthenticationValues authValues)
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
			}
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
			dictionary[220] = appVersion;
			dictionary[224] = appId;
			dictionary[210] = region;
			if (authValues != null)
			{
				if (!string.IsNullOrEmpty(authValues.UserId))
				{
					dictionary[225] = authValues.UserId;
				}
				if (authValues != null && authValues.AuthType != CustomAuthenticationType.None)
				{
					dictionary[217] = (byte)authValues.AuthType;
					if (!string.IsNullOrEmpty(authValues.Token))
					{
						dictionary[221] = authValues.Token;
					}
					else
					{
						if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
						{
							dictionary[216] = authValues.AuthGetParameters;
						}
						if (authValues.AuthPostData != null)
						{
							dictionary[214] = authValues.AuthPostData;
						}
					}
				}
			}
			return this.OpCustom(230, dictionary, true, 0, base.IsEncryptionAvailable);
		}

		// Token: 0x040027F4 RID: 10228
		public const string NameServerHost = "ns.exitgames.com";

		// Token: 0x040027F5 RID: 10229
		public const string NameServerHttp = "http://ns.exitgamescloud.com:80/photon/n";

		// Token: 0x040027F6 RID: 10230
		private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>
		{
			{
				ConnectionProtocol.Udp,
				5058
			},
			{
				ConnectionProtocol.Tcp,
				4533
			},
			{
				ConnectionProtocol.WebSocket,
				9093
			},
			{
				ConnectionProtocol.WebSocketSecure,
				19093
			}
		};
	}
}
