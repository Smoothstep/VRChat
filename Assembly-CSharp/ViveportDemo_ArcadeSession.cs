using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Viveport;
using Viveport.Arcade;
using Viveport.Core;

// Token: 0x02000975 RID: 2421
public class ViveportDemo_ArcadeSession : MonoBehaviour
{
	// Token: 0x0600496B RID: 18795 RVA: 0x001895BC File Offset: 0x001879BC
	private void Start()
	{
		if (ViveportDemo_ArcadeSession.f__mg0 == null)
		{
			ViveportDemo_ArcadeSession.f__mg0 = new StatusCallback(ViveportDemo_ArcadeSession.InitStatusHandler);
		}
		Api.Init(ViveportDemo_ArcadeSession.f__mg0, ViveportDemo_ArcadeSession.VIVEPORT_ARCADE_APP_TEST_ID);
		Viveport.Core.Logger.Log("Version: " + Api.Version());
		this.mListener = new ViveportDemo_ArcadeSession.Result();
	}

	// Token: 0x0600496C RID: 18796 RVA: 0x00189610 File Offset: 0x00187A10
	private void Update()
	{
	}

	// Token: 0x0600496D RID: 18797 RVA: 0x00189612 File Offset: 0x00187A12
	private static void InitStatusHandler(int nResult)
	{
		Viveport.Core.Logger.Log("InitStatusHandler: " + nResult);
	}

	// Token: 0x0600496E RID: 18798 RVA: 0x0018962C File Offset: 0x00187A2C
	private void OnGUI()
	{
		if (GUI.Button(new Rect((float)this.nXStart, (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "Session IsReady"))
		{
			Viveport.Core.Logger.Log("Session IsReady");
			Session.IsReady(this.mListener);
		}
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "Session Start"))
		{
			Viveport.Core.Logger.Log("Session Start");
			Session.Start(this.mListener);
		}
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + 2 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Session Stop"))
		{
			Viveport.Core.Logger.Log("Session Stop");
			Session.Stop(this.mListener);
		}
	}

	// Token: 0x040031D1 RID: 12753
	private int nWidth = 120;

	// Token: 0x040031D2 RID: 12754
	private int nHeight = 40;

	// Token: 0x040031D3 RID: 12755
	private int nXStart = 10;

	// Token: 0x040031D4 RID: 12756
	private int nYStart = 35;

	// Token: 0x040031D5 RID: 12757
	private static string VIVEPORT_ARCADE_APP_TEST_ID = "469fbcbb-bfde-40b5-a7d4-381249d387cd";

	// Token: 0x040031D6 RID: 12758
	private ViveportDemo_ArcadeSession.Result mListener;

	// Token: 0x040031D7 RID: 12759
	[CompilerGenerated]
	private static StatusCallback f__mg0;

	// Token: 0x02000976 RID: 2422
	private class Result : Session.SessionListener
	{
		// Token: 0x06004971 RID: 18801 RVA: 0x00189742 File Offset: 0x00187B42
		public override void OnSuccess(string pchAppID)
		{
			Viveport.Core.Logger.Log("[Session OnSuccess] pchAppID=" + pchAppID);
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x00189754 File Offset: 0x00187B54
		public override void OnStartSuccess(string pchAppID, string pchGuid)
		{
			Viveport.Core.Logger.Log("[Session OnStartSuccess] pchAppID=" + pchAppID + ",pchGuid=" + pchGuid);
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x0018976C File Offset: 0x00187B6C
		public override void OnStopSuccess(string pchAppID, string pchGuid)
		{
			Viveport.Core.Logger.Log("[Session OnStopSuccess] pchAppID=" + pchAppID + ",pchGuid=" + pchGuid);
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x00189784 File Offset: 0x00187B84
		public override void OnFailure(int nCode, string pchMessage)
		{
			Viveport.Core.Logger.Log(string.Concat(new object[]
			{
				"[Session OnFailed] nCode=",
				nCode,
				",pchMessage=",
				pchMessage
			}));
			Time.timeScale = 0f;
		}
	}
}
