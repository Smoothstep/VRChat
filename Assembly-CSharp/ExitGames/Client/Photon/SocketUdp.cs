using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace ExitGames.Client.Photon
{
	// Token: 0x02000771 RID: 1905
	internal class SocketUdp : IPhotonSocket, IDisposable
	{
		// Token: 0x06003E4D RID: 15949 RVA: 0x00139633 File Offset: 0x00137A33
		public SocketUdp(PeerBase npeer) : base(npeer)
		{
			if (base.ReportDebugOfLevel(DebugLevel.ALL))
			{
				base.Listener.DebugReturn(DebugLevel.ALL, "CSharpSocket: UDP, Unity3d.");
			}
			base.Protocol = ConnectionProtocol.Udp;
			this.PollReceive = false;
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x00139674 File Offset: 0x00137A74
		public void Dispose()
		{
			base.State = PhotonSocketState.Disconnecting;
			if (this.sock != null)
			{
				try
				{
					if (this.sock.Connected)
					{
						this.sock.Close();
					}
				}
				catch (Exception arg)
				{
					base.EnqueueDebugReturn(DebugLevel.INFO, "Exception in Dispose(): " + arg);
				}
			}
			this.sock = null;
			base.State = PhotonSocketState.Disconnected;
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x001396EC File Offset: 0x00137AEC
		public override bool Connect()
		{
			object obj = this.syncer;
			bool result;
			lock (obj)
			{
				if (!base.Connect())
				{
					result = false;
				}
				else
				{
					base.State = PhotonSocketState.Connecting;
					new Thread(new ThreadStart(this.DnsAndConnect))
					{
						Name = "photon dns thread",
						IsBackground = true
					}.Start();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x0013976C File Offset: 0x00137B6C
		public override bool Disconnect()
		{
			if (base.ReportDebugOfLevel(DebugLevel.INFO))
			{
				base.EnqueueDebugReturn(DebugLevel.INFO, "CSharpSocket.Disconnect()");
			}
			base.State = PhotonSocketState.Disconnecting;
			object obj = this.syncer;
			lock (obj)
			{
				if (this.sock != null)
				{
					try
					{
						this.sock.Close();
					}
					catch (Exception arg)
					{
						base.EnqueueDebugReturn(DebugLevel.INFO, "Exception in Disconnect(): " + arg);
					}
					this.sock = null;
				}
			}
			base.State = PhotonSocketState.Disconnected;
			return true;
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x00139810 File Offset: 0x00137C10
		public override PhotonSocketError Send(byte[] data, int length)
		{
			object obj = this.syncer;
			lock (obj)
			{
				if (this.sock == null || !this.sock.Connected)
				{
					return PhotonSocketError.Skipped;
				}
				try
				{
					this.sock.Send(data, 0, length, SocketFlags.None);
				}
				catch (Exception ex)
				{
					if (base.ReportDebugOfLevel(DebugLevel.ERROR))
					{
						base.EnqueueDebugReturn(DebugLevel.ERROR, "Cannot send to: " + base.ServerAddress + ". " + ex.Message);
					}
					return PhotonSocketError.Exception;
				}
			}
			return PhotonSocketError.Success;
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x001398C4 File Offset: 0x00137CC4
		public override PhotonSocketError Receive(out byte[] data)
		{
			data = null;
			return PhotonSocketError.NoData;
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x001398CC File Offset: 0x00137CCC
		internal void DnsAndConnect()
		{
			IPAddress ipaddress = null;
			try
			{
				object obj = this.syncer;
				lock (obj)
				{
					ipaddress = IPhotonSocket.GetIpAddress(base.ServerAddress);
					if (ipaddress == null)
					{
						throw new ArgumentException("Invalid IPAddress. Address: " + base.ServerAddress);
					}
					if (ipaddress.AddressFamily != AddressFamily.InterNetwork && ipaddress.AddressFamily != AddressFamily.InterNetworkV6)
					{
						throw new ArgumentException(string.Concat(new object[]
						{
							"AddressFamily '",
							ipaddress.AddressFamily,
							"' not supported. Address: ",
							base.ServerAddress
						}));
					}
					this.sock = new Socket(ipaddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
					this.sock.Connect(ipaddress, base.ServerPort);
					base.AddressResolvedAsIpv6 = (ipaddress.AddressFamily == AddressFamily.InterNetworkV6);
					base.State = PhotonSocketState.Connected;
					this.peerBase.OnConnect();
				}
			}
			catch (SecurityException ex)
			{
				if (base.ReportDebugOfLevel(DebugLevel.ERROR))
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat(new string[]
					{
						"Connect() to '",
						base.ServerAddress,
						"' (",
						(ipaddress != null) ? ipaddress.AddressFamily.ToString() : string.Empty,
						") failed: ",
						ex.ToString()
					}));
				}
				base.HandleException(StatusCode.SecurityExceptionOnConnect);
				return;
			}
			catch (Exception ex2)
			{
				if (base.ReportDebugOfLevel(DebugLevel.ERROR))
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, string.Concat(new string[]
					{
						"Connect() to '",
						base.ServerAddress,
						"' (",
						(ipaddress != null) ? ipaddress.AddressFamily.ToString() : string.Empty,
						") failed: ",
						ex2.ToString()
					}));
				}
				base.HandleException(StatusCode.ExceptionOnConnect);
				return;
			}
			new Thread(new ThreadStart(this.ReceiveLoop))
			{
				Name = "photon receive thread",
				IsBackground = true
			}.Start();
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x00139B20 File Offset: 0x00137F20
		public void ReceiveLoop()
		{
			byte[] array = new byte[base.MTU];
			while (base.State == PhotonSocketState.Connected)
			{
				try
				{
					int length = this.sock.Receive(array);
					base.HandleReceivedDatagram(array, length, true);
				}
				catch (Exception ex)
				{
					if (base.State != PhotonSocketState.Disconnecting && base.State != PhotonSocketState.Disconnected)
					{
						if (base.ReportDebugOfLevel(DebugLevel.ERROR))
						{
							base.EnqueueDebugReturn(DebugLevel.ERROR, string.Concat(new object[]
							{
								"Receive issue. State: ",
								base.State,
								". Server: '",
								base.ServerAddress,
								"' Exception: ",
								ex
							}));
						}
						base.HandleException(StatusCode.ExceptionOnReceive);
					}
				}
			}
			this.Disconnect();
		}

		// Token: 0x040026CF RID: 9935
		private Socket sock;

		// Token: 0x040026D0 RID: 9936
		private readonly object syncer = new object();
	}
}
