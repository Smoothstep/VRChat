using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008E9 RID: 2281
public class ChatController : MonoBehaviour
{
	// Token: 0x0600453E RID: 17726 RVA: 0x001733CA File Offset: 0x001717CA
	private void OnEnable()
	{
		this.TMP_ChatInput.onSubmit.AddListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x0600453F RID: 17727 RVA: 0x001733E8 File Offset: 0x001717E8
	private void OnDisable()
	{
		this.TMP_ChatInput.onSubmit.RemoveListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x06004540 RID: 17728 RVA: 0x00173408 File Offset: 0x00171808
	private void AddToChatOutput(string newText)
	{
		this.TMP_ChatInput.text = string.Empty;
		DateTime now = DateTime.Now;
		TMP_Text tmp_ChatOutput = this.TMP_ChatOutput;
		string text = tmp_ChatOutput.text;
		tmp_ChatOutput.text = string.Concat(new string[]
		{
			text,
			"[<#FFFF80>",
			now.Hour.ToString("d2"),
			":",
			now.Minute.ToString("d2"),
			":",
			now.Second.ToString("d2"),
			"</color>] ",
			newText,
			"\n"
		});
		this.TMP_ChatInput.ActivateInputField();
		this.ChatScrollbar.value = 0f;
	}

	// Token: 0x04002F44 RID: 12100
	public TMP_InputField TMP_ChatInput;

	// Token: 0x04002F45 RID: 12101
	public TMP_Text TMP_ChatOutput;

	// Token: 0x04002F46 RID: 12102
	public Scrollbar ChatScrollbar;
}
