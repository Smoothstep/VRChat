using System;
using System.Net.Sockets;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000769 RID: 1897
public class PingMonoEditor : PhotonPing
{
	// Token: 0x06003DF4 RID: 15860 RVA: 0x001383A0 File Offset: 0x001367A0
	public override bool StartPing(string ip)
	{
		base.Init();
		try
		{
			if (ip.Contains("."))
			{
				this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			}
			else
			{
				this.sock = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
			}
			this.sock.ReceiveTimeout = 5000;
			this.sock.Connect(ip, 5055);
			this.PingBytes[this.PingBytes.Length - 1] = this.PingId;
			this.sock.Send(this.PingBytes);
			this.PingBytes[this.PingBytes.Length - 1] = this.PingId;
            this.PingBytes[this.PingBytes.Length - 1]--;

        }
		catch (Exception value)
		{
			this.sock = null;
			Console.WriteLine(value);
		}
		return false;
	}

	// Token: 0x06003DF5 RID: 15861 RVA: 0x00138474 File Offset: 0x00136874
	public override bool Done()
	{
		if (this.GotResult || this.sock == null)
		{
			return true;
		}
		if (this.sock.Available <= 0)
		{
			return false;
		}
		int num = this.sock.Receive(this.PingBytes, SocketFlags.None);
		if (this.PingBytes[this.PingBytes.Length - 1] != this.PingId || num != this.PingLength)
		{
			Debug.Log("ReplyMatch is false! ");
		}
		this.Successful = (num == this.PingBytes.Length && this.PingBytes[this.PingBytes.Length - 1] == this.PingId);
		this.GotResult = true;
		return true;
	}

	// Token: 0x06003DF6 RID: 15862 RVA: 0x00138530 File Offset: 0x00136930
	public override void Dispose()
	{
		try
		{
			this.sock.Close();
		}
		catch
		{
		}
		this.sock = null;
	}

	// Token: 0x040026A0 RID: 9888
	private Socket sock;
}
