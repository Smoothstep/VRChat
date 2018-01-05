using System;
using UnityEngine;

// Token: 0x02000A5F RID: 2655
public class CopyTextFrom : MonoBehaviour
{
	// Token: 0x0600505B RID: 20571 RVA: 0x001B7E3C File Offset: 0x001B623C
	private void Start()
	{
		this.textMesh = base.GetComponent<TextMesh>();
		this.textMesh.text = this.from.text;
	}

	// Token: 0x0600505C RID: 20572 RVA: 0x001B7E60 File Offset: 0x001B6260
	private void Update()
	{
		if (this.textMesh.text != this.from.text)
		{
			this.textMesh.text = this.from.text;
		}
	}

	// Token: 0x04003924 RID: 14628
	public TextMesh from;

	// Token: 0x04003925 RID: 14629
	private TextMesh textMesh;
}
