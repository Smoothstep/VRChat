using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200076A RID: 1898
public class PhotonPingManager
{
	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x06003DF8 RID: 15864 RVA: 0x00138574 File Offset: 0x00136974
	public Region BestRegion
	{
		get
		{
			Region result = null;
			int num = int.MaxValue;
			foreach (Region region in PhotonNetwork.networkingPeer.AvailableRegions)
			{
				UnityEngine.Debug.Log("BestRegion checks region: " + region);
				if (region.Ping != 0 && region.Ping < num)
				{
					num = region.Ping;
					result = region;
				}
			}
			return result;
		}
	}

	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x00138608 File Offset: 0x00136A08
	public bool Done
	{
		get
		{
			return this.PingsRunning == 0;
		}
	}

	// Token: 0x06003DFA RID: 15866 RVA: 0x00138614 File Offset: 0x00136A14
	public IEnumerator PingSocket(Region region)
	{
		region.Ping = PhotonPingManager.Attempts * PhotonPingManager.MaxMilliseconsPerPing;
		this.PingsRunning++;
		PhotonPing ping;
		if (PhotonHandler.PingImplementation == typeof(PingNativeDynamic))
		{
			UnityEngine.Debug.Log("Using constructor for new PingNativeDynamic()");
			ping = new PingNativeDynamic();
		}
		else if (PhotonHandler.PingImplementation == typeof(PingMono))
		{
			ping = new PingMono();
		}
		else
		{
			ping = (PhotonPing)Activator.CreateInstance(PhotonHandler.PingImplementation);
		}
		float rttSum = 0f;
		int replyCount = 0;
		string regionAddress = region.HostAndPort;
		int indexOfColon = regionAddress.LastIndexOf(':');
		if (indexOfColon > 1)
		{
			regionAddress = regionAddress.Substring(0, indexOfColon);
		}
		regionAddress = PhotonPingManager.ResolveHost(regionAddress);
		for (int i = 0; i < PhotonPingManager.Attempts; i++)
		{
			bool overtime = false;
			Stopwatch sw = new Stopwatch();
			sw.Start();
			try
			{
				ping.StartPing(regionAddress);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.Log("catched: " + arg);
				this.PingsRunning--;
				break;
			}
			while (!ping.Done())
			{
				if (sw.ElapsedMilliseconds >= (long)PhotonPingManager.MaxMilliseconsPerPing)
				{
					overtime = true;
					break;
				}
				yield return 0;
			}
			int rtt = (int)sw.ElapsedMilliseconds;
			if (!PhotonPingManager.IgnoreInitialAttempt || i != 0)
			{
				if (ping.Successful && !overtime)
				{
					rttSum += (float)rtt;
					replyCount++;
					region.Ping = (int)(rttSum / (float)replyCount);
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
		this.PingsRunning--;
		yield return null;
		yield break;
	}

	// Token: 0x06003DFB RID: 15867 RVA: 0x00138638 File Offset: 0x00136A38
	public static string ResolveHost(string hostName)
	{
		string text = string.Empty;
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
			if (hostAddresses.Length == 1)
			{
				return hostAddresses[0].ToString();
			}
			foreach (IPAddress ipaddress in hostAddresses)
			{
				if (ipaddress != null)
				{
					if (ipaddress.ToString().Contains(":"))
					{
						return ipaddress.ToString();
					}
					if (string.IsNullOrEmpty(text))
					{
						text = hostAddresses.ToString();
					}
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Exception caught! " + ex.Source + " Message: " + ex.Message);
		}
		return text;
	}

	// Token: 0x040026A1 RID: 9889
	public bool UseNative;

	// Token: 0x040026A2 RID: 9890
	public static int Attempts = 5;

	// Token: 0x040026A3 RID: 9891
	public static bool IgnoreInitialAttempt = true;

	// Token: 0x040026A4 RID: 9892
	public static int MaxMilliseconsPerPing = 800;

	// Token: 0x040026A5 RID: 9893
	private int PingsRunning;
}
