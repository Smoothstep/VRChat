using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C8B RID: 3211
[RequireComponent(typeof(Image))]
public class VRCUiShadowPlate : MonoBehaviour
{
	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x060063BC RID: 25532 RVA: 0x00236F13 File Offset: 0x00235313
	// (set) Token: 0x060063BD RID: 25533 RVA: 0x00236F20 File Offset: 0x00235320
	public string text
	{
		get
		{
			return this.mainText.text;
		}
		set
		{
			this.mainText.text = value;
			this.dropShadow.text = value;
		}
	}

	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x060063BE RID: 25534 RVA: 0x00236F3A File Offset: 0x0023533A
	public Image image
	{
		get
		{
			return base.gameObject.GetComponent<Image>();
		}
	}

	// Token: 0x060063BF RID: 25535 RVA: 0x00236F47 File Offset: 0x00235347
	private void Start()
	{
		if (this.mainText == null || this.dropShadow == null)
		{
			Debug.LogError("UI Element " + base.name + " has a null Text field.");
		}
	}

	// Token: 0x04004910 RID: 18704
	public Text mainText;

	// Token: 0x04004911 RID: 18705
	public Text dropShadow;
}
