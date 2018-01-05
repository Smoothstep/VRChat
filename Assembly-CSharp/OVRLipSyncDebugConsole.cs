using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020006E9 RID: 1769
public class OVRLipSyncDebugConsole : MonoBehaviour
{
	// Token: 0x17000921 RID: 2337
	// (get) Token: 0x06003A42 RID: 14914 RVA: 0x00126B84 File Offset: 0x00124F84
	public static OVRLipSyncDebugConsole instance
	{
		get
		{
			if (OVRLipSyncDebugConsole.s_Instance == null)
			{
				OVRLipSyncDebugConsole.s_Instance = (UnityEngine.Object.FindObjectOfType(typeof(OVRLipSyncDebugConsole)) as OVRLipSyncDebugConsole);
				if (OVRLipSyncDebugConsole.s_Instance == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<OVRLipSyncDebugConsole>();
					gameObject.name = "OVRLipSyncDebugConsole";
					OVRLipSyncDebugConsole.s_Instance = (UnityEngine.Object.FindObjectOfType(typeof(OVRLipSyncDebugConsole)) as OVRLipSyncDebugConsole);
				}
			}
			return OVRLipSyncDebugConsole.s_Instance;
		}
	}

	// Token: 0x06003A43 RID: 14915 RVA: 0x00126C00 File Offset: 0x00125000
	private void Awake()
	{
		OVRLipSyncDebugConsole.s_Instance = this;
		this.Init();
	}

	// Token: 0x06003A44 RID: 14916 RVA: 0x00126C10 File Offset: 0x00125010
	private void Update()
	{
		if (this.clearTimeoutOn)
		{
			this.clearTimeout -= Time.deltaTime;
			if (this.clearTimeout < 0f)
			{
				OVRLipSyncDebugConsole.Clear();
				this.clearTimeout = 0f;
				this.clearTimeoutOn = false;
			}
		}
	}

	// Token: 0x06003A45 RID: 14917 RVA: 0x00126C61 File Offset: 0x00125061
	public void Init()
	{
		if (this.textMsg == null)
		{
			Debug.LogWarning("DebugConsole Init WARNING::UI text not set. Will not be able to display anything.");
		}
		OVRLipSyncDebugConsole.Clear();
	}

	// Token: 0x06003A46 RID: 14918 RVA: 0x00126C83 File Offset: 0x00125083
	public static void Log(string message)
	{
		OVRLipSyncDebugConsole.instance.AddMessage(message, Color.white);
	}

	// Token: 0x06003A47 RID: 14919 RVA: 0x00126C95 File Offset: 0x00125095
	public static void Log(string message, Color color)
	{
		OVRLipSyncDebugConsole.instance.AddMessage(message, color);
	}

	// Token: 0x06003A48 RID: 14920 RVA: 0x00126CA3 File Offset: 0x001250A3
	public static void Clear()
	{
		OVRLipSyncDebugConsole.instance.ClearMessages();
	}

	// Token: 0x06003A49 RID: 14921 RVA: 0x00126CAF File Offset: 0x001250AF
	public static void ClearTimeout(float timeToClear)
	{
		OVRLipSyncDebugConsole.instance.SetClearTimeout(timeToClear);
	}

	// Token: 0x06003A4A RID: 14922 RVA: 0x00126CBC File Offset: 0x001250BC
	public void AddMessage(string message, Color color)
	{
		this.messages.Add(message);
		if (this.textMsg != null)
		{
			this.textMsg.color = color;
		}
		this.Display();
	}

	// Token: 0x06003A4B RID: 14923 RVA: 0x00126CEE File Offset: 0x001250EE
	public void ClearMessages()
	{
		this.messages.Clear();
		this.Display();
	}

	// Token: 0x06003A4C RID: 14924 RVA: 0x00126D01 File Offset: 0x00125101
	public void SetClearTimeout(float timeout)
	{
		this.clearTimeout = timeout;
		this.clearTimeoutOn = true;
	}

	// Token: 0x06003A4D RID: 14925 RVA: 0x00126D14 File Offset: 0x00125114
	private void Prune()
	{
		if (this.messages.Count > this.maxMessages)
		{
			int count;
			if (this.messages.Count <= 0)
			{
				count = 0;
			}
			else
			{
				count = this.messages.Count - this.maxMessages;
			}
			this.messages.RemoveRange(0, count);
		}
	}

	// Token: 0x06003A4E RID: 14926 RVA: 0x00126D70 File Offset: 0x00125170
	private void Display()
	{
		if (this.messages.Count > this.maxMessages)
		{
			this.Prune();
		}
		if (this.textMsg != null)
		{
			this.textMsg.text = string.Empty;
			for (int i = 0; i < this.messages.Count; i++)
			{
				Text text = this.textMsg;
				text.text += (string)this.messages[i];
				Text text2 = this.textMsg;
				text2.text += '\n';
			}
		}
	}

	// Token: 0x04002322 RID: 8994
	public ArrayList messages = new ArrayList();

	// Token: 0x04002323 RID: 8995
	public int maxMessages = 15;

	// Token: 0x04002324 RID: 8996
	public Text textMsg;

	// Token: 0x04002325 RID: 8997
	private static OVRLipSyncDebugConsole s_Instance;

	// Token: 0x04002326 RID: 8998
	private bool clearTimeoutOn;

	// Token: 0x04002327 RID: 8999
	private float clearTimeout;
}
