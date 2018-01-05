using System;
using UnityEngine;

// Token: 0x020008A1 RID: 2209
public class FixShaderQueue : MonoBehaviour
{
	// Token: 0x060043CA RID: 17354 RVA: 0x001663C8 File Offset: 0x001647C8
	private void Start()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().sharedMaterial.renderQueue += this.AddQueue;
		}
		else
		{
			base.Invoke("SetProjectorQueue", 0.1f);
		}
	}

	// Token: 0x060043CB RID: 17355 RVA: 0x00166418 File Offset: 0x00164818
	private void SetProjectorQueue()
	{
		base.GetComponent<Projector>().material.renderQueue += this.AddQueue;
	}

	// Token: 0x060043CC RID: 17356 RVA: 0x00166437 File Offset: 0x00164837
	private void Update()
	{
	}

	// Token: 0x04002CB4 RID: 11444
	public int AddQueue = 1;
}
