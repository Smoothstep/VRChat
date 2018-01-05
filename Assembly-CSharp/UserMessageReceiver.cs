using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B09 RID: 2825
public class UserMessageReceiver : MonoBehaviour
{
	// Token: 0x0600558B RID: 21899 RVA: 0x001D83E6 File Offset: 0x001D67E6
	private void Start()
	{
		if (this.uText != null)
		{
			this.uText.text = UserMessage.message;
		}
	}

	// Token: 0x0600558C RID: 21900 RVA: 0x001D8409 File Offset: 0x001D6809
	private void Update()
	{
		if (this.uText != null && this.uText.text != UserMessage.message)
		{
			this.uText.text = UserMessage.message;
		}
	}

	// Token: 0x04003C71 RID: 15473
	public Text uText;
}
