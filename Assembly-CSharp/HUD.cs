using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A94 RID: 2708
public class HUD : VRGUI
{
	// Token: 0x06005181 RID: 20865 RVA: 0x001BECC1 File Offset: 0x001BD0C1
	private void Start()
	{
		Debug.LogError("FIX PREVIOUS LINE");
	}

	// Token: 0x06005182 RID: 20866 RVA: 0x001BECCD File Offset: 0x001BD0CD
	private void FixedUpdate()
	{
	}

	// Token: 0x06005183 RID: 20867 RVA: 0x001BECD0 File Offset: 0x001BD0D0
	public override void OnVRGUI()
	{
		GUILayout.BeginArea(new Rect((float)(Screen.width / 2) - this.averageMessageSize, 0f, (float)Screen.width, (float)Screen.height));
		for (int i = 0; i < this._hudUpdates.Count; i++)
		{
			GUILayout.Box(this._hudUpdates[i].message, this.guiStyle, new GUILayoutOption[0]);
		}
		GUILayout.EndArea();
	}

	// Token: 0x06005184 RID: 20868 RVA: 0x001BED4A File Offset: 0x001BD14A
	public void Log(string message)
	{
		this._hudUpdates.Add(new HUD.HudUpdate(message));
	}

	// Token: 0x040039DC RID: 14812
	public static HUD Instance;

	// Token: 0x040039DD RID: 14813
	public GUIStyle guiStyle;

	// Token: 0x040039DE RID: 14814
	private float averageMessageSize = 150f;

	// Token: 0x040039DF RID: 14815
	private List<HUD.HudUpdate> _hudUpdates = new List<HUD.HudUpdate>();

	// Token: 0x02000A95 RID: 2709
	private class HudUpdate
	{
		// Token: 0x06005185 RID: 20869 RVA: 0x001BED5D File Offset: 0x001BD15D
		public HudUpdate(string mes)
		{
			this.message = mes;
		}

		// Token: 0x040039E0 RID: 14816
		public string message = string.Empty;

		// Token: 0x040039E1 RID: 14817
		public float time = Time.time;
	}
}
