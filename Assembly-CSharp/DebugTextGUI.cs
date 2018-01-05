using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A76 RID: 2678
public class DebugTextGUI : VRGUI
{
	// Token: 0x060050CB RID: 20683 RVA: 0x001B9EA3 File Offset: 0x001B82A3
	public static void DebugPrint(string format, params object[] values)
	{
		if (DebugTextGUI.Instance != null)
		{
			DebugTextGUI.Instance.AddText(string.Format(format, values));
		}
	}

	// Token: 0x060050CC RID: 20684 RVA: 0x001B9EC8 File Offset: 0x001B82C8
	private void Awake()
	{
		if (DebugTextGUI.Instance == null)
		{
			DebugTextGUI.Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
		this.acceptMouse = false;
		if (this.textStyle == null)
		{
			this.textStyle = new GUIStyle(GUI.skin.label);
			this.textStyle.fontSize = Mathf.CeilToInt((float)this.textStyle.fontSize * 0.5f);
		}
	}

	// Token: 0x060050CD RID: 20685 RVA: 0x001B9F3F File Offset: 0x001B833F
	private void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.Menu))
		{
			this.visible = !this.visible;
		}
		this.FlipDebugTextBuffer(Time.deltaTime);
	}

	// Token: 0x060050CE RID: 20686 RVA: 0x001B9F76 File Offset: 0x001B8376
	public override void OnVRGUI()
	{
		if (this.visible)
		{
			GUILayout.BeginArea(base.MakeCenterRect(), GUI.skin.box);
			this.DrawDebugTextWindow();
			GUILayout.EndArea();
		}
	}

	// Token: 0x060050CF RID: 20687 RVA: 0x001B9FA3 File Offset: 0x001B83A3
	private void AddText(string text)
	{
		this.newDebugText.Add(text);
	}

	// Token: 0x060050D0 RID: 20688 RVA: 0x001B9FB4 File Offset: 0x001B83B4
	private void FlipDebugTextBuffer(float delta)
	{
		object obj = this.textForGUI;
		lock (obj)
		{
			foreach (DebugTextGUI.TextInfo textInfo in this.textForGUI)
			{
				textInfo.timeRemaining -= delta;
			}
			this.textForGUI.RemoveAll((DebugTextGUI.TextInfo text) => text.timeRemaining < 0f);
			foreach (string text2 in this.newDebugText)
			{
				DebugTextGUI.TextInfo textInfo2 = (this.textForGUI.Count != 0) ? this.textForGUI[this.textForGUI.Count - 1] : null;
				if (textInfo2 != null && textInfo2.text == text2)
				{
					textInfo2.timeRemaining = this.secondsVisible;
					textInfo2.count++;
				}
				else
				{
					this.textForGUI.Add(new DebugTextGUI.TextInfo
					{
						timeRemaining = this.secondsVisible,
						count = 1,
						text = text2
					});
				}
			}
			this.newDebugText.Clear();
		}
	}

	// Token: 0x060050D1 RID: 20689 RVA: 0x001BA174 File Offset: 0x001B8574
	private void DrawDebugTextWindow()
	{
		this.debugScrollPosition = GUILayout.BeginScrollView(this.debugScrollPosition, new GUILayoutOption[0]);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		object obj = this.textForGUI;
		lock (obj)
		{
			foreach (DebugTextGUI.TextInfo textInfo in this.textForGUI)
			{
				GUILayout.Label(string.Concat(new object[]
				{
					"[",
					textInfo.count,
					"] ",
					textInfo.text
				}), new GUILayoutOption[0]);
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	// Token: 0x04003953 RID: 14675
	public bool visible;

	// Token: 0x04003954 RID: 14676
	public float secondsVisible = 5f;

	// Token: 0x04003955 RID: 14677
	private Vector2 debugScrollPosition = default(Vector2);

	// Token: 0x04003956 RID: 14678
	private List<string> newDebugText = new List<string>();

	// Token: 0x04003957 RID: 14679
	private List<DebugTextGUI.TextInfo> textForGUI = new List<DebugTextGUI.TextInfo>();

	// Token: 0x04003958 RID: 14680
	public static DebugTextGUI Instance;

	// Token: 0x04003959 RID: 14681
	public GUIStyle textStyle;

	// Token: 0x02000A77 RID: 2679
	private class TextInfo
	{
		// Token: 0x0400395B RID: 14683
		public string text;

		// Token: 0x0400395C RID: 14684
		public float timeRemaining;

		// Token: 0x0400395D RID: 14685
		public int count;
	}
}
