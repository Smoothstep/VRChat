using System;
using System.Collections.Generic;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x020007AB RID: 1963
	public class ChatClient : IPhotonPeerListener
	{
		// Token: 0x06003F64 RID: 16228 RVA: 0x0013ED94 File Offset: 0x0013D194
		public ChatClient(IChatClientListener listener, ConnectionProtocol protocol = ConnectionProtocol.Udp)
		{
			this.listener = listener;
			this.State = ChatState.Uninitialized;
			this.chatPeer = new ChatPeer(this, protocol);
			this.PublicChannels = new Dictionary<string, ChatChannel>();
			this.PrivateChannels = new Dictionary<string, ChatChannel>();
			this.PublicChannelsUnsubscribing = new HashSet<string>();
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06003F65 RID: 16229 RVA: 0x0013EDF6 File Offset: 0x0013D1F6
		// (set) Token: 0x06003F66 RID: 16230 RVA: 0x0013EDFE File Offset: 0x0013D1FE
		public string NameServerAddress { get; private set; }

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06003F67 RID: 16231 RVA: 0x0013EE07 File Offset: 0x0013D207
		// (set) Token: 0x06003F68 RID: 16232 RVA: 0x0013EE0F File Offset: 0x0013D20F
		public string FrontendAddress { get; private set; }

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06003F69 RID: 16233 RVA: 0x0013EE18 File Offset: 0x0013D218
		// (set) Token: 0x06003F6A RID: 16234 RVA: 0x0013EE20 File Offset: 0x0013D220
		public string ChatRegion
		{
			get
			{
				return this.chatRegion;
			}
			set
			{
				this.chatRegion = value;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x0013EE29 File Offset: 0x0013D229
		// (set) Token: 0x06003F6C RID: 16236 RVA: 0x0013EE31 File Offset: 0x0013D231
		public ChatState State { get; private set; }

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06003F6D RID: 16237 RVA: 0x0013EE3A File Offset: 0x0013D23A
		// (set) Token: 0x06003F6E RID: 16238 RVA: 0x0013EE42 File Offset: 0x0013D242
		public ChatDisconnectCause DisconnectedCause { get; private set; }

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06003F6F RID: 16239 RVA: 0x0013EE4B File Offset: 0x0013D24B
		public bool CanChat
		{
			get
			{
				return this.State == ChatState.ConnectedToFrontEnd && this.HasPeer;
			}
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x0013EE62 File Offset: 0x0013D262
		public bool CanChatInChannel(string channelName)
		{
			return this.CanChat && this.PublicChannels.ContainsKey(channelName) && !this.PublicChannelsUnsubscribing.Contains(channelName);
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06003F71 RID: 16241 RVA: 0x0013EE92 File Offset: 0x0013D292
		private bool HasPeer
		{
			get
			{
				return this.chatPeer != null;
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06003F72 RID: 16242 RVA: 0x0013EEA0 File Offset: 0x0013D2A0
		// (set) Token: 0x06003F73 RID: 16243 RVA: 0x0013EEA8 File Offset: 0x0013D2A8
		public string AppVersion { get; private set; }

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06003F74 RID: 16244 RVA: 0x0013EEB1 File Offset: 0x0013D2B1
		// (set) Token: 0x06003F75 RID: 16245 RVA: 0x0013EEB9 File Offset: 0x0013D2B9
		public string AppId { get; private set; }

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06003F76 RID: 16246 RVA: 0x0013EEC2 File Offset: 0x0013D2C2
		// (set) Token: 0x06003F77 RID: 16247 RVA: 0x0013EECA File Offset: 0x0013D2CA
		public AuthenticationValues AuthValues { get; set; }

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06003F78 RID: 16248 RVA: 0x0013EED3 File Offset: 0x0013D2D3
		// (set) Token: 0x06003F79 RID: 16249 RVA: 0x0013EEF1 File Offset: 0x0013D2F1
		public string UserId
		{
			get
			{
				return (this.AuthValues == null) ? null : this.AuthValues.UserId;
			}
			private set
			{
				if (this.AuthValues == null)
				{
					this.AuthValues = new AuthenticationValues();
				}
				this.AuthValues.UserId = value;
			}
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x0013EF18 File Offset: 0x0013D318
		public bool Connect(string appId, string appVersion, AuthenticationValues authValues)
		{
			this.chatPeer.TimePingInterval = 3000;
			this.DisconnectedCause = ChatDisconnectCause.None;
			if (authValues == null)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Connect failed: no authentication values specified");
				}
				return false;
			}
			this.AuthValues = authValues;
			if (this.AuthValues.UserId == null || this.AuthValues.UserId == string.Empty)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Connect failed: no UserId specified in authentication values");
				}
				return false;
			}
			this.AppId = appId;
			this.AppVersion = appVersion;
			this.didAuthenticate = false;
			this.msDeltaForServiceCalls = 100;
			this.chatPeer.QuickResendAttempts = 2;
			this.chatPeer.SentCountAllowance = 7;
			this.PublicChannels.Clear();
			this.PrivateChannels.Clear();
			this.PublicChannelsUnsubscribing.Clear();
			this.NameServerAddress = this.chatPeer.NameServerAddress;
			bool flag = this.chatPeer.Connect();
			if (flag)
			{
				this.State = ChatState.ConnectingToNameServer;
			}
			return flag;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x0013F038 File Offset: 0x0013D438
		public void Service()
		{
			if (this.HasPeer && (Environment.TickCount - this.msTimestampOfLastServiceCall > this.msDeltaForServiceCalls || this.msTimestampOfLastServiceCall == 0))
			{
				this.msTimestampOfLastServiceCall = Environment.TickCount;
				this.chatPeer.Service();
			}
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x0013F088 File Offset: 0x0013D488
		public void Disconnect()
		{
			if (this.HasPeer && this.chatPeer.PeerState != PeerStateValue.Disconnected)
			{
				this.chatPeer.Disconnect();
			}
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x0013F0B0 File Offset: 0x0013D4B0
		public void StopThread()
		{
			if (this.HasPeer)
			{
				this.chatPeer.StopThread();
			}
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x0013F0C8 File Offset: 0x0013D4C8
		public bool Subscribe(string[] channels)
		{
			return this.Subscribe(channels, 0);
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x0013F0D4 File Offset: 0x0013D4D4
		public bool Subscribe(string[] channels, int messagesFromHistory)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Subscribe called while not connected to front end server.");
				}
				return false;
			}
			if (channels == null || channels.Length == 0)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "Subscribe can't be called for empty or null channels-list.");
				}
				return false;
			}
			return this.SendChannelOperation(channels, 0, messagesFromHistory);
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x0013F144 File Offset: 0x0013D544
		public bool Unsubscribe(string[] channels)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Unsubscribe called while not connected to front end server.");
				}
				return false;
			}
			if (channels == null || channels.Length == 0)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "Unsubscribe can't be called for empty or null channels-list.");
				}
				return false;
			}
			foreach (string item in channels)
			{
				this.PublicChannelsUnsubscribing.Add(item);
			}
			return this.SendChannelOperation(channels, 1, 0);
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0013F1D8 File Offset: 0x0013D5D8
		public bool PublishMessage(string channelName, object message, bool forwardAsWebhook = false)
		{
			return this.publishMessage(channelName, message, true, forwardAsWebhook);
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x0013F1E4 File Offset: 0x0013D5E4
		internal bool PublishMessageUnreliable(string channelName, object message, bool forwardAsWebhook = false)
		{
			return this.publishMessage(channelName, message, false, forwardAsWebhook);
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x0013F1F0 File Offset: 0x0013D5F0
		private bool publishMessage(string channelName, object message, bool reliable, bool forwardAsWebhook = false)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "PublishMessage called while not connected to front end server.");
				}
				return false;
			}
			if (string.IsNullOrEmpty(channelName) || message == null)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "PublishMessage parameters must be non-null and not empty.");
				}
				return false;
			}
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>
			{
				{
					1,
					channelName
				},
				{
					3,
					message
				}
			};
			if (forwardAsWebhook)
			{
				dictionary.Add(21, 1);
			}
			return this.chatPeer.OpCustom(2, dictionary, reliable);
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x0013F292 File Offset: 0x0013D692
		public bool SendPrivateMessage(string target, object message, bool forwardAsWebhook = false)
		{
			return this.SendPrivateMessage(target, message, false, forwardAsWebhook);
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x0013F29E File Offset: 0x0013D69E
		public bool SendPrivateMessage(string target, object message, bool encrypt, bool forwardAsWebhook)
		{
			return this.sendPrivateMessage(target, message, encrypt, true, forwardAsWebhook);
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x0013F2AC File Offset: 0x0013D6AC
		internal bool SendPrivateMessageUnreliable(string target, object message, bool encrypt, bool forwardAsWebhook = false)
		{
			return this.sendPrivateMessage(target, message, encrypt, false, forwardAsWebhook);
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x0013F2BC File Offset: 0x0013D6BC
		private bool sendPrivateMessage(string target, object message, bool encrypt, bool reliable, bool forwardAsWebhook = false)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "SendPrivateMessage called while not connected to front end server.");
				}
				return false;
			}
			if (string.IsNullOrEmpty(target) || message == null)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "SendPrivateMessage parameters must be non-null and not empty.");
				}
				return false;
			}
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>
			{
				{
					225,
					target
				},
				{
					3,
					message
				}
			};
			if (forwardAsWebhook)
			{
				dictionary.Add(21, 1);
			}
			return this.chatPeer.OpCustom(3, dictionary, reliable, 0, encrypt);
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0013F368 File Offset: 0x0013D768
		private bool SetOnlineStatus(int status, object message, bool skipMessage)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "SetOnlineStatus called while not connected to front end server.");
				}
				return false;
			}
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>
			{
				{
					10,
					status
				}
			};
			if (skipMessage)
			{
				dictionary[12] = true;
			}
			else
			{
				dictionary[3] = message;
			}
			return this.chatPeer.OpCustom(5, dictionary, true);
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x0013F3E4 File Offset: 0x0013D7E4
		public bool SetOnlineStatus(int status)
		{
			return this.SetOnlineStatus(status, null, true);
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x0013F3EF File Offset: 0x0013D7EF
		public bool SetOnlineStatus(int status, object message)
		{
			return this.SetOnlineStatus(status, message, false);
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x0013F3FC File Offset: 0x0013D7FC
		public bool AddFriends(string[] friends)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "AddFriends called while not connected to front end server.");
				}
				return false;
			}
			if (friends == null || friends.Length == 0)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "AddFriends can't be called for empty or null list.");
				}
				return false;
			}
			if (friends.Length > 1024)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, string.Concat(new object[]
					{
						"AddFriends max list size exceeded: ",
						friends.Length,
						" > ",
						1024
					}));
				}
				return false;
			}
			Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>
			{
				{
					11,
					friends
				}
			};
			return this.chatPeer.OpCustom(6, customOpParameters, true);
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x0013F4DC File Offset: 0x0013D8DC
		public bool RemoveFriends(string[] friends)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "RemoveFriends called while not connected to front end server.");
				}
				return false;
			}
			if (friends == null || friends.Length == 0)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "RemoveFriends can't be called for empty or null list.");
				}
				return false;
			}
			if (friends.Length > 1024)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, string.Concat(new object[]
					{
						"RemoveFriends max list size exceeded: ",
						friends.Length,
						" > ",
						1024
					}));
				}
				return false;
			}
			Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>
			{
				{
					11,
					friends
				}
			};
			return this.chatPeer.OpCustom(7, customOpParameters, true);
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x0013F5B9 File Offset: 0x0013D9B9
		public string GetPrivateChannelNameByUser(string userName)
		{
			return string.Format("{0}:{1}", this.UserId, userName);
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x0013F5CC File Offset: 0x0013D9CC
		public bool TryGetChannel(string channelName, bool isPrivate, out ChatChannel channel)
		{
			if (!isPrivate)
			{
				return this.PublicChannels.TryGetValue(channelName, out channel);
			}
			return this.PrivateChannels.TryGetValue(channelName, out channel);
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x0013F5F0 File Offset: 0x0013D9F0
		public bool TryGetChannel(string channelName, out ChatChannel channel)
		{
			bool flag = this.PublicChannels.TryGetValue(channelName, out channel);
			return flag || this.PrivateChannels.TryGetValue(channelName, out channel);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x0013F624 File Offset: 0x0013DA24
		public void SendAcksOnly()
		{
			if (this.chatPeer != null)
			{
				this.chatPeer.SendAcksOnly();
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06003F92 RID: 16274 RVA: 0x0013F64B File Offset: 0x0013DA4B
		// (set) Token: 0x06003F91 RID: 16273 RVA: 0x0013F63D File Offset: 0x0013DA3D
		public DebugLevel DebugOut
		{
			get
			{
				return this.chatPeer.DebugOut;
			}
			set
			{
				this.chatPeer.DebugOut = value;
			}
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x0013F658 File Offset: 0x0013DA58
		void IPhotonPeerListener.DebugReturn(DebugLevel level, string message)
		{
			this.listener.DebugReturn(level, message);
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x0013F668 File Offset: 0x0013DA68
		void IPhotonPeerListener.OnEvent(EventData eventData)
		{
			switch (eventData.Code)
			{
			case 0:
				this.HandleChatMessagesEvent(eventData);
				break;
			case 2:
				this.HandlePrivateMessageEvent(eventData);
				break;
			case 4:
				this.HandleStatusUpdate(eventData);
				break;
			case 5:
				this.HandleSubscribeEvent(eventData);
				break;
			case 6:
				this.HandleUnsubscribeEvent(eventData);
				break;
			}
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x0013F6E0 File Offset: 0x0013DAE0
		void IPhotonPeerListener.OnOperationResponse(OperationResponse operationResponse)
		{
			byte operationCode = operationResponse.OperationCode;
			switch (operationCode)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				break;
			default:
				if (operationCode == 230)
				{
					this.HandleAuthResponse(operationResponse);
					return;
				}
				break;
			}
			if (operationResponse.ReturnCode != 0 && this.DebugOut >= DebugLevel.ERROR)
			{
				if (operationResponse.ReturnCode == -2)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Chat Operation {0} unknown on server. Check your AppId and make sure it's for a Chat application.", operationResponse.OperationCode));
				}
				else
				{
					this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Chat Operation {0} failed (Code: {1}). Debug Message: {2}", operationResponse.OperationCode, operationResponse.ReturnCode, operationResponse.DebugMessage));
				}
			}
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x0013F7A8 File Offset: 0x0013DBA8
		void IPhotonPeerListener.OnStatusChanged(StatusCode statusCode)
		{
			if (statusCode != StatusCode.Connect)
			{
				if (statusCode != StatusCode.Disconnect)
				{
					if (statusCode != StatusCode.EncryptionEstablished)
					{
						if (statusCode == StatusCode.EncryptionFailedToEstablish)
						{
							this.State = ChatState.Disconnecting;
							this.chatPeer.Disconnect();
						}
					}
					else if (!this.didAuthenticate)
					{
						this.didAuthenticate = this.chatPeer.AuthenticateOnNameServer(this.AppId, this.AppVersion, this.chatRegion, this.AuthValues);
						if (!this.didAuthenticate && this.DebugOut >= DebugLevel.ERROR)
						{
							((IPhotonPeerListener)this).DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected. State: " + this.State);
						}
					}
				}
				else if (this.State == ChatState.Authenticated)
				{
					this.ConnectToFrontEnd();
				}
				else
				{
					this.State = ChatState.Disconnected;
					this.listener.OnChatStateChange(ChatState.Disconnected);
					this.listener.OnDisconnected();
				}
			}
			else
			{
				if (!this.chatPeer.IsProtocolSecure)
				{
					this.chatPeer.EstablishEncryption();
				}
				else if (!this.didAuthenticate)
				{
					this.didAuthenticate = this.chatPeer.AuthenticateOnNameServer(this.AppId, this.AppVersion, this.chatRegion, this.AuthValues);
					if (!this.didAuthenticate && this.DebugOut >= DebugLevel.ERROR)
					{
						((IPhotonPeerListener)this).DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected. State: " + this.State);
					}
				}
				if (this.State == ChatState.ConnectingToNameServer)
				{
					this.State = ChatState.ConnectedToNameServer;
					this.listener.OnChatStateChange(this.State);
				}
				else if (this.State == ChatState.ConnectingToFrontEnd)
				{
					this.AuthenticateOnFrontEnd();
				}
			}
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x0013F970 File Offset: 0x0013DD70
		private bool SendChannelOperation(string[] channels, byte operation, int historyLength)
		{
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>
			{
				{
					0,
					channels
				}
			};
			if (historyLength != 0)
			{
				dictionary.Add(14, historyLength);
			}
			return this.chatPeer.OpCustom(operation, dictionary, true);
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x0013F9B0 File Offset: 0x0013DDB0
		private void HandlePrivateMessageEvent(EventData eventData)
		{
			object message = eventData.Parameters[3];
			string text = (string)eventData.Parameters[5];
			string privateChannelNameByUser;
			if (this.UserId != null && this.UserId.Equals(text))
			{
				string userName = (string)eventData.Parameters[225];
				privateChannelNameByUser = this.GetPrivateChannelNameByUser(userName);
			}
			else
			{
				privateChannelNameByUser = this.GetPrivateChannelNameByUser(text);
			}
			ChatChannel chatChannel;
			if (!this.PrivateChannels.TryGetValue(privateChannelNameByUser, out chatChannel))
			{
				chatChannel = new ChatChannel(privateChannelNameByUser);
				chatChannel.IsPrivate = true;
				chatChannel.MessageLimit = this.MessageLimit;
				this.PrivateChannels.Add(chatChannel.Name, chatChannel);
			}
			chatChannel.Add(text, message);
			this.listener.OnPrivateMessage(text, message, privateChannelNameByUser);
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0013FA80 File Offset: 0x0013DE80
		private void HandleChatMessagesEvent(EventData eventData)
		{
			object[] messages = (object[])eventData.Parameters[2];
			string[] senders = (string[])eventData.Parameters[4];
			string text = (string)eventData.Parameters[1];
			ChatChannel chatChannel;
			if (!this.PublicChannels.TryGetValue(text, out chatChannel))
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "Channel " + text + " for incoming message event not found.");
				}
				return;
			}
			chatChannel.Add(senders, messages);
			this.listener.OnGetMessages(text, senders, messages);
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0013FB18 File Offset: 0x0013DF18
		private void HandleSubscribeEvent(EventData eventData)
		{
			string[] array = (string[])eventData.Parameters[0];
			bool[] array2 = (bool[])eventData.Parameters[15];
			for (int i = 0; i < array.Length; i++)
			{
				if (array2[i])
				{
					string text = array[i];
					if (!this.PublicChannels.ContainsKey(text))
					{
						ChatChannel chatChannel = new ChatChannel(text);
						chatChannel.MessageLimit = this.MessageLimit;
						this.PublicChannels.Add(chatChannel.Name, chatChannel);
					}
				}
			}
			this.listener.OnSubscribed(array, array2);
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0013FBB4 File Offset: 0x0013DFB4
		private void HandleUnsubscribeEvent(EventData eventData)
		{
            string[] channels = (string[])eventData[(byte)0];
            for (int index = 0; index < channels.Length; ++index)
            {
                string key = channels[index];
                this.PublicChannels.Remove(key);
                this.PublicChannelsUnsubscribing.Remove(key);
            }
            this.listener.OnUnsubscribed(channels);
        }

		// Token: 0x06003F9C RID: 16284 RVA: 0x0013FC0C File Offset: 0x0013E00C
		private void HandleAuthResponse(OperationResponse operationResponse)
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				this.listener.DebugReturn(DebugLevel.INFO, operationResponse.ToStringFull() + " on: " + this.chatPeer.NameServerAddress);
			}
			if (operationResponse.ReturnCode == 0)
			{
				if (this.State == ChatState.ConnectedToNameServer)
				{
					this.State = ChatState.Authenticated;
					this.listener.OnChatStateChange(this.State);
					if (operationResponse.Parameters.ContainsKey(221))
					{
						if (this.AuthValues == null)
						{
							this.AuthValues = new AuthenticationValues();
						}
						this.AuthValues.Token = (operationResponse[221] as string);
						this.FrontendAddress = (string)operationResponse[230];
						this.chatPeer.Disconnect();
					}
					else if (this.DebugOut >= DebugLevel.ERROR)
					{
						this.listener.DebugReturn(DebugLevel.ERROR, "No secret in authentication response.");
					}
				}
				else if (this.State == ChatState.ConnectingToFrontEnd)
				{
					this.msDeltaForServiceCalls *= 4;
					this.State = ChatState.ConnectedToFrontEnd;
					this.listener.OnChatStateChange(this.State);
					this.listener.OnConnected();
				}
			}
			else
			{
				short returnCode = operationResponse.ReturnCode;
				switch (returnCode)
				{
				case 32755:
					this.DisconnectedCause = ChatDisconnectCause.CustomAuthenticationFailed;
					break;
				case 32756:
					this.DisconnectedCause = ChatDisconnectCause.InvalidRegion;
					break;
				case 32757:
					this.DisconnectedCause = ChatDisconnectCause.MaxCcuReached;
					break;
				default:
					if (returnCode != -3)
					{
						if (returnCode == 32767)
						{
							this.DisconnectedCause = ChatDisconnectCause.InvalidAuthentication;
						}
					}
					else
					{
						this.DisconnectedCause = ChatDisconnectCause.OperationNotAllowedInCurrentState;
					}
					break;
				}
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Authentication request error: " + operationResponse.ReturnCode + ". Disconnecting.");
				}
				this.State = ChatState.Disconnecting;
				this.chatPeer.Disconnect();
			}
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x0013FE08 File Offset: 0x0013E208
		private void HandleStatusUpdate(EventData eventData)
		{
			string user = (string)eventData.Parameters[5];
			int status = (int)eventData.Parameters[10];
			object message = null;
			bool flag = eventData.Parameters.ContainsKey(3);
			if (flag)
			{
				message = eventData.Parameters[3];
			}
			this.listener.OnStatusUpdate(user, status, flag, message);
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x0013FE6C File Offset: 0x0013E26C
		private void ConnectToFrontEnd()
		{
			this.State = ChatState.ConnectingToFrontEnd;
			if (this.DebugOut >= DebugLevel.INFO)
			{
				this.listener.DebugReturn(DebugLevel.INFO, "Connecting to frontend " + this.FrontendAddress);
			}
			this.chatPeer.Connect(this.FrontendAddress, "chat");
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x0013FEC0 File Offset: 0x0013E2C0
		private bool AuthenticateOnFrontEnd()
		{
			if (this.AuthValues == null)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Can't authenticate on front end server. Authentication Values are not set");
				}
				return false;
			}
			if (this.AuthValues.Token == null || this.AuthValues.Token == string.Empty)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Can't authenticate on front end server. Secret is not set");
				}
				return false;
			}
			Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>
			{
				{
					221,
					this.AuthValues.Token
				}
			};
			return this.chatPeer.OpCustom(230, customOpParameters, true);
		}

		// Token: 0x040027B4 RID: 10164
		private const int FriendRequestListMax = 1024;

		// Token: 0x040027B7 RID: 10167
		private string chatRegion = "EU";

		// Token: 0x040027BD RID: 10173
		public int MessageLimit;

		// Token: 0x040027BE RID: 10174
		public readonly Dictionary<string, ChatChannel> PublicChannels;

		// Token: 0x040027BF RID: 10175
		public readonly Dictionary<string, ChatChannel> PrivateChannels;

		// Token: 0x040027C0 RID: 10176
		private readonly HashSet<string> PublicChannelsUnsubscribing;

		// Token: 0x040027C1 RID: 10177
		private readonly IChatClientListener listener;

		// Token: 0x040027C2 RID: 10178
		public ChatPeer chatPeer;

		// Token: 0x040027C3 RID: 10179
		private bool didAuthenticate;

		// Token: 0x040027C4 RID: 10180
		private int msDeltaForServiceCalls = 50;

		// Token: 0x040027C5 RID: 10181
		private int msTimestampOfLastServiceCall;

		// Token: 0x040027C6 RID: 10182
		private const string ChatAppName = "chat";
	}
}
