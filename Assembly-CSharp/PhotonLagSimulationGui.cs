using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200075E RID: 1886
public class PhotonLagSimulationGui : MonoBehaviour
{
	// Token: 0x17000990 RID: 2448
	// (get) Token: 0x06003D04 RID: 15620 RVA: 0x00134701 File Offset: 0x00132B01
	// (set) Token: 0x06003D05 RID: 15621 RVA: 0x00134709 File Offset: 0x00132B09
	public PhotonPeer Peer { get; set; }

	// Token: 0x06003D06 RID: 15622 RVA: 0x00134712 File Offset: 0x00132B12
	public void Start()
	{
		this.Peer = PhotonNetwork.networkingPeer;
	}

	// Token: 0x06003D07 RID: 15623 RVA: 0x00134720 File Offset: 0x00132B20
	public void OnGUI()
	{
		if (!this.Visible)
		{
			return;
		}
		if (this.Peer == null)
		{
			this.WindowRect = GUILayout.Window(this.WindowId, this.WindowRect, new GUI.WindowFunction(this.NetSimHasNoPeerWindow), "Netw. Sim.", new GUILayoutOption[0]);
		}
		else
		{
			this.WindowRect = GUILayout.Window(this.WindowId, this.WindowRect, new GUI.WindowFunction(this.NetSimWindow), "Netw. Sim.", new GUILayoutOption[0]);
		}
	}

	// Token: 0x06003D08 RID: 15624 RVA: 0x001347A5 File Offset: 0x00132BA5
	private void NetSimHasNoPeerWindow(int windowId)
	{
		GUILayout.Label("No peer to communicate with. ", new GUILayoutOption[0]);
	}

	// Token: 0x06003D09 RID: 15625 RVA: 0x001347B8 File Offset: 0x00132BB8
	private void NetSimWindow(int windowId)
	{
		GUILayout.Label(string.Format("Rtt:{0,4} +/-{1,3}", this.Peer.RoundTripTime, this.Peer.RoundTripTimeVariance), new GUILayoutOption[0]);
		bool isSimulationEnabled = this.Peer.IsSimulationEnabled;
		bool flag = GUILayout.Toggle(isSimulationEnabled, "Simulate", new GUILayoutOption[0]);
		if (flag != isSimulationEnabled)
		{
			this.Peer.IsSimulationEnabled = flag;
		}
		float num = (float)this.Peer.NetworkSimulationSettings.IncomingLag;
		GUILayout.Label("Lag " + num, new GUILayoutOption[0]);
		num = GUILayout.HorizontalSlider(num, 0f, 500f, new GUILayoutOption[0]);
		this.Peer.NetworkSimulationSettings.IncomingLag = (int)num;
		this.Peer.NetworkSimulationSettings.OutgoingLag = (int)num;
		float num2 = (float)this.Peer.NetworkSimulationSettings.IncomingJitter;
		GUILayout.Label("Jit " + num2, new GUILayoutOption[0]);
		num2 = GUILayout.HorizontalSlider(num2, 0f, 100f, new GUILayoutOption[0]);
		this.Peer.NetworkSimulationSettings.IncomingJitter = (int)num2;
		this.Peer.NetworkSimulationSettings.OutgoingJitter = (int)num2;
		float num3 = (float)this.Peer.NetworkSimulationSettings.IncomingLossPercentage;
		GUILayout.Label("Loss " + num3, new GUILayoutOption[0]);
		num3 = GUILayout.HorizontalSlider(num3, 0f, 10f, new GUILayoutOption[0]);
		this.Peer.NetworkSimulationSettings.IncomingLossPercentage = (int)num3;
		this.Peer.NetworkSimulationSettings.OutgoingLossPercentage = (int)num3;
		if (GUI.changed)
		{
			this.WindowRect.height = 100f;
		}
		GUI.DragWindow();
	}

	// Token: 0x04002636 RID: 9782
	public Rect WindowRect = new Rect(0f, 100f, 120f, 100f);

	// Token: 0x04002637 RID: 9783
	public int WindowId = 101;

	// Token: 0x04002638 RID: 9784
	public bool Visible = true;
}
