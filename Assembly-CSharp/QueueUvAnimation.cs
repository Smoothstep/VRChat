using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A4 RID: 2212
public class QueueUvAnimation : MonoBehaviour
{
	// Token: 0x060043D4 RID: 17364 RVA: 0x0016650E File Offset: 0x0016490E
	private void Start()
	{
		this.deltaTime = 1f / this.Fps;
		this.InitDefaultTex(this.RowsFadeIn, this.ColumnsFadeIn);
	}

	// Token: 0x060043D5 RID: 17365 RVA: 0x00166534 File Offset: 0x00164934
	private void InitDefaultTex(int rows, int colums)
	{
		this.count = rows * colums;
		this.index += colums - 1;
		Vector2 value = new Vector2(1f / (float)colums, 1f / (float)rows);
		base.GetComponent<Renderer>().material.SetTextureScale("_MainTex", value);
		if (this.IsBump)
		{
			base.GetComponent<Renderer>().material.SetTextureScale("_BumpMap", value);
		}
	}

	// Token: 0x060043D6 RID: 17366 RVA: 0x001665A8 File Offset: 0x001649A8
	private void OnBecameVisible()
	{
		this.isVisible = true;
		base.StartCoroutine(this.UpdateTiling());
	}

	// Token: 0x060043D7 RID: 17367 RVA: 0x001665BE File Offset: 0x001649BE
	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	// Token: 0x060043D8 RID: 17368 RVA: 0x001665C8 File Offset: 0x001649C8
	private IEnumerator UpdateTiling()
	{
		while (this.isVisible && this.allCount != this.count)
		{
			this.allCount++;
			this.index++;
			if (this.index >= this.count)
			{
				this.index = 0;
			}
			Vector2 offset = this.isFadeHandle ? new Vector2((float)this.index / (float)this.ColumnsLoop - (float)(this.index / this.ColumnsLoop), 1f - (float)(this.index / this.ColumnsLoop) / (float)this.RowsLoop) : new Vector2((float)this.index / (float)this.ColumnsFadeIn - (float)(this.index / this.ColumnsFadeIn), 1f - (float)(this.index / this.ColumnsFadeIn) / (float)this.RowsFadeIn);
			if (!this.isFadeHandle)
			{
				base.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
				if (this.IsBump)
				{
					base.GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", offset);
				}
			}
			else
			{
				base.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
				if (this.IsBump)
				{
					base.GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", offset);
				}
			}
			if (this.allCount == this.count)
			{
				this.isFadeHandle = true;
				base.GetComponent<Renderer>().material = this.NextMaterial;
				this.InitDefaultTex(this.RowsLoop, this.ColumnsLoop);
			}
			yield return new WaitForSeconds(this.deltaTime);
		}
		yield break;
	}

	// Token: 0x04002CB7 RID: 11447
	public int RowsFadeIn = 4;

	// Token: 0x04002CB8 RID: 11448
	public int ColumnsFadeIn = 4;

	// Token: 0x04002CB9 RID: 11449
	public int RowsLoop = 4;

	// Token: 0x04002CBA RID: 11450
	public int ColumnsLoop = 4;

	// Token: 0x04002CBB RID: 11451
	public float Fps = 20f;

	// Token: 0x04002CBC RID: 11452
	public bool IsBump;

	// Token: 0x04002CBD RID: 11453
	public Material NextMaterial;

	// Token: 0x04002CBE RID: 11454
	private int index;

	// Token: 0x04002CBF RID: 11455
	private int count;

	// Token: 0x04002CC0 RID: 11456
	private int allCount;

	// Token: 0x04002CC1 RID: 11457
	private float deltaTime;

	// Token: 0x04002CC2 RID: 11458
	private bool isVisible;

	// Token: 0x04002CC3 RID: 11459
	private bool isFadeHandle;
}
