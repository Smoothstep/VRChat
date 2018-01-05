using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C65 RID: 3173
public class VRCUiButton : MonoBehaviour
{
	// Token: 0x0600628C RID: 25228 RVA: 0x00232739 File Offset: 0x00230B39
	private void Awake()
	{
		this.b = base.GetComponent<Button>();
	}

	// Token: 0x0600628D RID: 25229 RVA: 0x00232748 File Offset: 0x00230B48
	public void SetButtonText(string s)
	{
		if (this.b != null)
		{
			Text componentInChildren = this.b.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				componentInChildren.text = s;
			}
		}
	}

	// Token: 0x040047FD RID: 18429
	public string Name;

	// Token: 0x040047FE RID: 18430
	private Button b;
}
