using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AC4 RID: 2756
internal class LoadingAvatarTextureAnimation : MonoBehaviour
{
	// Token: 0x060053B2 RID: 21426 RVA: 0x001CDE0D File Offset: 0x001CC20D
	private void Awake()
	{
		this.animatedMaterial = base.GetComponent<SkinnedMeshRenderer>().materials[this.matIndex];
		this.InitDefaultVariables();
		this.isInizialised = true;
	}

	// Token: 0x060053B3 RID: 21427 RVA: 0x001CDE34 File Offset: 0x001CC234
	private void InitDefaultVariables()
	{
		this.allCount = 0;
		this.deltaFps = 1f / this.Fps;
		this.count = this.Rows * this.Columns;
		this.index = this.Columns - 1;
		Vector2 value = new Vector2((float)this.index / (float)this.Columns - (float)(this.index / this.Columns), 1f - (float)(this.index / this.Columns) / (float)this.Rows);
		this.OffsetMat = (this.IsRandomOffsetForInctance ? UnityEngine.Random.Range(0, this.count) : (this.OffsetMat - this.OffsetMat / this.count * this.count));
		Vector2 value2 = (!(this.SelfTiling == Vector2.zero)) ? this.SelfTiling : new Vector2(1f / (float)this.Columns, 1f / (float)this.Rows);
		if (this.animatedMaterial != null)
		{
			this.animatedMaterial.SetTextureScale("_MainTex", value2);
			this.animatedMaterial.SetTextureOffset("_MainTex", value);
		}
	}

	// Token: 0x060053B4 RID: 21428 RVA: 0x001CDF6C File Offset: 0x001CC36C
	private void OnEnable()
	{
		if (this.isInizialised)
		{
			this.InitDefaultVariables();
		}
		this.isVisible = true;
		if (!this.isCorutineStarted)
		{
			base.StartCoroutine(this.UpdateCorutine());
		}
	}

	// Token: 0x060053B5 RID: 21429 RVA: 0x001CDF9E File Offset: 0x001CC39E
	private void OnDisable()
	{
		this.isCorutineStarted = false;
		this.isVisible = false;
		base.StopAllCoroutines();
	}

	// Token: 0x060053B6 RID: 21430 RVA: 0x001CDFB4 File Offset: 0x001CC3B4
	private void OnBecameVisible()
	{
		this.isVisible = true;
		if (!this.isCorutineStarted)
		{
			base.StartCoroutine(this.UpdateCorutine());
		}
	}

	// Token: 0x060053B7 RID: 21431 RVA: 0x001CDFD5 File Offset: 0x001CC3D5
	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	// Token: 0x060053B8 RID: 21432 RVA: 0x001CDFE0 File Offset: 0x001CC3E0
	private IEnumerator UpdateCorutine()
	{
		this.isCorutineStarted = true;
		while (this.isVisible && (this.IsLoop || this.allCount != this.count))
		{
			this.UpdateCorutineFrame();
			if (!this.IsLoop && this.allCount == this.count)
			{
				break;
			}
			yield return new WaitForSeconds(this.deltaFps);
		}
		this.isCorutineStarted = false;
		yield break;
	}

	// Token: 0x060053B9 RID: 21433 RVA: 0x001CDFFC File Offset: 0x001CC3FC
	private void UpdateCorutineFrame()
	{
		this.allCount++;
		if (this.IsReverse)
		{
			this.index--;
		}
		else
		{
			this.index++;
		}
		if (this.index >= this.count)
		{
			this.index = 0;
		}
		Vector2 value;
		if (this.IsRandomOffsetForInctance)
		{
			int num = this.index + this.OffsetMat;
			value = new Vector2((float)num / (float)this.Columns - (float)(num / this.Columns), 1f - (float)(num / this.Columns) / (float)this.Rows);
		}
		else
		{
			value = new Vector2((float)this.index / (float)this.Columns - (float)(this.index / this.Columns), 1f - (float)(this.index / this.Columns) / (float)this.Rows);
		}
		if (this.animatedMaterial != null)
		{
			this.animatedMaterial.SetTextureOffset("_MainTex", value);
		}
	}

	// Token: 0x04003B04 RID: 15108
	private Material animatedMaterial;

	// Token: 0x04003B05 RID: 15109
	public int matIndex = 1;

	// Token: 0x04003B06 RID: 15110
	public int Rows = 4;

	// Token: 0x04003B07 RID: 15111
	public int Columns = 4;

	// Token: 0x04003B08 RID: 15112
	public float Fps = 20f;

	// Token: 0x04003B09 RID: 15113
	public int OffsetMat;

	// Token: 0x04003B0A RID: 15114
	public Vector2 SelfTiling = default(Vector2);

	// Token: 0x04003B0B RID: 15115
	public bool IsLoop = true;

	// Token: 0x04003B0C RID: 15116
	public bool IsReverse;

	// Token: 0x04003B0D RID: 15117
	public bool IsRandomOffsetForInctance;

	// Token: 0x04003B0E RID: 15118
	private bool isInizialised;

	// Token: 0x04003B0F RID: 15119
	private int index;

	// Token: 0x04003B10 RID: 15120
	private int count;

	// Token: 0x04003B11 RID: 15121
	private int allCount;

	// Token: 0x04003B12 RID: 15122
	private float deltaFps;

	// Token: 0x04003B13 RID: 15123
	private bool isVisible;

	// Token: 0x04003B14 RID: 15124
	private bool isCorutineStarted;
}
