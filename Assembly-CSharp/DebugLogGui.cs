using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VRC;

// Token: 0x02000A72 RID: 2674
public class DebugLogGui : VRGUI
{
	// Token: 0x060050BE RID: 20670 RVA: 0x001B9890 File Offset: 0x001B7C90
	protected override void OnEnable()
	{
		base.OnEnable();
		VRCLog.MessageLogged += this.HandleLog;
	}

	// Token: 0x060050BF RID: 20671 RVA: 0x001B98A9 File Offset: 0x001B7CA9
	protected override void OnDisable()
	{
		base.OnDisable();
		VRCLog.MessageLogged -= this.HandleLog;
	}

	// Token: 0x060050C0 RID: 20672 RVA: 0x001B98C4 File Offset: 0x001B7CC4
	private void Awake()
	{
		if (DebugLogGui.Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			DebugLogGui.Instance = this;
		}
		if (this.textStyle == null)
		{
			this.textStyle = new GUIStyle(GUI.skin.label);
			this.textStyle.fontSize = Mathf.CeilToInt((float)this.textStyle.fontSize * 0.5f);
		}
	}

	// Token: 0x060050C1 RID: 20673 RVA: 0x001B9934 File Offset: 0x001B7D34
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.Menu))
		{
			this.visible = !this.visible;
		}
		this.acceptMouse = this.visible;
	}

	// Token: 0x060050C2 RID: 20674 RVA: 0x001B996C File Offset: 0x001B7D6C
	public override void OnVRGUI()
	{
		if (this.visible)
		{
			this.windowRect = base.MakeCenterRect();
			GUILayout.BeginArea(this.windowRect, GUI.skin.box);
			this.DrawDebugLogWindow();
			GUILayout.EndArea();
		}
	}

	// Token: 0x060050C3 RID: 20675 RVA: 0x001B99A8 File Offset: 0x001B7DA8
	private void DrawDebugLogWindow()
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		this.expand = GUILayout.Toggle(this.expand, "Expand", this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		this.autoScroll = GUILayout.Toggle(this.autoScroll, "Scroll", this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		this.showLog = GUILayout.Toggle(this.showLog, "Log", this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		this.showWarning = GUILayout.Toggle(this.showWarning, "Warning", this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		this.showError = GUILayout.Toggle(this.showError, "Error", this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("FPS: " + Mathf.CeilToInt(1f / Time.smoothDeltaTime * Time.timeScale).ToString(), this.textStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginArea(new Rect(10f, 40f, this.windowRect.width - 20f, this.windowRect.height - 50f));
		this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, new GUILayoutOption[0]);
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		for (int i = this.debugLog.Count - 1; i >= 0; i--)
		{
			DebugLogGui.DisplayInfo displayInfo = this.FormatEntry(this.debugLog[i]);
			if (displayInfo != null)
			{
				stringBuilder.Append(displayInfo.text);
				num += displayInfo.lineCount;
			}
			if (num >= this.LabelLines)
			{
				num = 0;
				GUILayout.Label(stringBuilder.ToString(), this.textStyle, new GUILayoutOption[0]);
				stringBuilder = new StringBuilder();
			}
		}
		if (num > 0)
		{
			GUILayout.Label(stringBuilder.ToString(), this.textStyle, new GUILayoutOption[0]);
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	// Token: 0x060050C4 RID: 20676 RVA: 0x001B9BCC File Offset: 0x001B7FCC
	private DebugLogGui.DisplayInfo FormatEntry(DebugLogGui.LogEntry entry)
	{
		string text = this.LogColor;
		LogType type = entry.type;
		if (type != LogType.Log)
		{
			if (type != LogType.Warning)
			{
				if (!this.showError)
				{
					return null;
				}
				text = this.ErrorColor;
			}
			else
			{
				if (!this.showWarning)
				{
					return null;
				}
				text = this.WarningColor;
			}
		}
		else
		{
			if (!this.showLog)
			{
				return null;
			}
			text = this.LogColor;
		}
		string[] array = entry.text.Split(new char[]
		{
			'\n'
		});
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		foreach (string text2 in array)
		{
			string value;
			if (num == 0)
			{
				value = string.Format("<color={0}>{1}{2}</color>: {3}", new object[]
				{
					text,
					(entry.count <= 1) ? string.Empty : ("(" + entry.count.ToString() + ") "),
					entry.type.ToString().ToUpper(),
					text2
				});
			}
			else
			{
				value = text2;
			}
			stringBuilder.AppendLine(value);
			num++;
			if (!this.expand)
			{
				break;
			}
		}
		return new DebugLogGui.DisplayInfo
		{
			text = stringBuilder.ToString(),
			lineCount = num
		};
	}

	// Token: 0x060050C5 RID: 20677 RVA: 0x001B9D44 File Offset: 0x001B8144
	private void HandleLog(string condition, string stackTrace, LogType type)
	{
		string text = condition + "\n" + stackTrace;
		DebugLogGui.LogEntry logEntry = (this.debugLog.Count <= 0) ? null : this.debugLog[0];
		if (logEntry != null && logEntry.type == type && logEntry.text == text)
		{
			logEntry.count++;
		}
		else
		{
			this.debugLog.Insert(0, new DebugLogGui.LogEntry
			{
				type = type,
				text = text,
				count = 1
			});
			int count = this.debugLog.Count;
			if (count > this.MaxLines)
			{
				this.debugLog.RemoveRange(this.MaxLines, count - this.MaxLines);
			}
			if (this.autoScroll)
			{
				this.scrollPosition.y = float.MaxValue;
			}
		}
	}

	// Token: 0x0400393D RID: 14653
	private string LogColor = "grey";

	// Token: 0x0400393E RID: 14654
	private string WarningColor = "yellow";

	// Token: 0x0400393F RID: 14655
	private string ErrorColor = "red";

	// Token: 0x04003940 RID: 14656
	private int LabelLines = 25;

	// Token: 0x04003941 RID: 14657
	private int MaxLines = 500;

	// Token: 0x04003942 RID: 14658
	public bool visible;

	// Token: 0x04003943 RID: 14659
	private Rect windowRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x04003944 RID: 14660
	private bool showLog = true;

	// Token: 0x04003945 RID: 14661
	private bool showWarning = true;

	// Token: 0x04003946 RID: 14662
	private bool showError = true;

	// Token: 0x04003947 RID: 14663
	private bool expand;

	// Token: 0x04003948 RID: 14664
	private bool autoScroll = true;

	// Token: 0x04003949 RID: 14665
	private Queue<int> fpsHistory;

	// Token: 0x0400394A RID: 14666
	private Vector2 scrollPosition = Vector2.zero;

	// Token: 0x0400394B RID: 14667
	public static DebugLogGui Instance;

	// Token: 0x0400394C RID: 14668
	private List<DebugLogGui.LogEntry> debugLog = new List<DebugLogGui.LogEntry>();

	// Token: 0x0400394D RID: 14669
	public GUIStyle textStyle;

	// Token: 0x02000A73 RID: 2675
	private class LogEntry
	{
		// Token: 0x0400394E RID: 14670
		public string text;

		// Token: 0x0400394F RID: 14671
		public LogType type;

		// Token: 0x04003950 RID: 14672
		public int count;
	}

	// Token: 0x02000A74 RID: 2676
	private class DisplayInfo
	{
		// Token: 0x04003951 RID: 14673
		public string text;

		// Token: 0x04003952 RID: 14674
		public int lineCount;
	}
}
