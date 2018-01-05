using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000A6D RID: 2669
public class DebugDrawHMD : VRGUI
{
	// Token: 0x060050B2 RID: 20658 RVA: 0x001B938C File Offset: 0x001B778C
	private void Start()
	{
		if (DebugDrawHMD.Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			DebugDrawHMD.Instance = this;
		}
		this.acceptMouse = false;
		this.cursorSize = 0;
		if (this.textStyle == null)
		{
			this.textStyle = new GUIStyle(GUI.skin.label);
			this.textStyle.fontSize = Mathf.CeilToInt((float)this.textStyle.fontSize * 0.5f);
		}
	}

	// Token: 0x060050B3 RID: 20659 RVA: 0x001B940C File Offset: 0x001B780C
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKey(KeyCode.Menu))
		{
			this.visible = !this.visible;
		}
		bool flag = true;
		foreach (DebugDrawHMD.DebugMessage debugMessage in this.messages)
		{
			if (debugMessage.Timeout >= 0f && Time.unscaledTime - debugMessage.TimePosted >= debugMessage.Timeout)
			{
				debugMessage.Message = string.Empty;
			}
			flag = (flag && string.IsNullOrEmpty(debugMessage.Message));
		}
		if (flag)
		{
			this.messages.Clear();
		}
	}

	// Token: 0x060050B4 RID: 20660 RVA: 0x001B94E8 File Offset: 0x001B78E8
	public override void OnVRGUI()
	{
		if (!this.visible)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(this.MakeStaticDebugText());
		foreach (DebugDrawHMD.DebugMessage debugMessage in this.messages)
		{
			stringBuilder.AppendLine(debugMessage.Message);
		}
		GUI.Box(base.MakeCenterRect(), stringBuilder.ToString(), this.textStyle);
	}

	// Token: 0x060050B5 RID: 20661 RVA: 0x001B9580 File Offset: 0x001B7980
	private string MakeStaticDebugText()
	{
		return string.Format("S:{1} V:{2} W:{3} FPS:{0}", new object[]
		{
			Mathf.CeilToInt(1f / Time.smoothDeltaTime),
			VRCApplicationSetup.Instance.GetGameServerVersion(),
			VRCApplicationSetup.Instance.appVersion,
			(!(VRC_SceneDescriptor.Instance != null)) ? "(not loaded)" : VRC_SceneDescriptor.Instance.unityVersion
		});
	}

	// Token: 0x060050B6 RID: 20662 RVA: 0x001B95F8 File Offset: 0x001B79F8
	public void SetText(string message, int index = 0, float timeout = -1f)
	{
		while (this.messages.Count <= index)
		{
			this.messages.Add(new DebugDrawHMD.DebugMessage());
		}
		DebugDrawHMD.DebugMessage debugMessage = new DebugDrawHMD.DebugMessage();
		debugMessage.Message = message;
		debugMessage.Timeout = timeout;
		debugMessage.TimePosted = Time.unscaledTime;
		this.messages[index] = debugMessage;
	}

	// Token: 0x0400392F RID: 14639
	public static DebugDrawHMD Instance;

	// Token: 0x04003930 RID: 14640
	public bool visible;

	// Token: 0x04003931 RID: 14641
	private List<DebugDrawHMD.DebugMessage> messages = new List<DebugDrawHMD.DebugMessage>();

	// Token: 0x04003932 RID: 14642
	public GUIStyle textStyle;

	// Token: 0x02000A6E RID: 2670
	private class DebugMessage
	{
		// Token: 0x04003933 RID: 14643
		public string Message = string.Empty;

		// Token: 0x04003934 RID: 14644
		public float Timeout = -1f;

		// Token: 0x04003935 RID: 14645
		public float TimePosted;
	}
}
