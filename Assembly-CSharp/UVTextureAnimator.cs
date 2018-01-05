using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A6 RID: 2214
internal class UVTextureAnimator : MonoBehaviour
{
	// Token: 0x060043DF RID: 17375 RVA: 0x00166B3E File Offset: 0x00164F3E
	private void Start()
	{
		this.InitMaterial();
		this.InitDefaultVariables();
		this.isInizialised = true;
		this.isVisible = true;
		base.StartCoroutine(this.UpdateCorutine());
	}

	// Token: 0x060043E0 RID: 17376 RVA: 0x00166B67 File Offset: 0x00164F67
	public void SetInstanceMaterial(Material mat, Vector2 offsetMat)
	{
		this.instanceMaterial = mat;
		this.InitDefaultVariables();
	}

	// Token: 0x060043E1 RID: 17377 RVA: 0x00166B78 File Offset: 0x00164F78
	private void InitDefaultVariables()
	{
		this.allCount = 0;
		this.deltaFps = 1f / this.Fps;
		this.count = this.Rows * this.Columns;
		this.index = this.Columns - 1;
		Vector2 value = new Vector2((float)this.index / (float)this.Columns - (float)(this.index / this.Columns), 1f - (float)(this.index / this.Columns) / (float)this.Rows);
		this.OffsetMat = (this.IsRandomOffsetForInctance ? UnityEngine.Random.Range(0, this.count) : (this.OffsetMat - this.OffsetMat / this.count * this.count));
		Vector2 value2 = (!(this.SelfTiling == Vector2.zero)) ? this.SelfTiling : new Vector2(1f / (float)this.Columns, 1f / (float)this.Rows);
		if (this.AnimatedMaterialsNotInstance.Length > 0)
		{
			foreach (Material material in this.AnimatedMaterialsNotInstance)
			{
				material.SetTextureScale("_MainTex", value2);
				material.SetTextureOffset("_MainTex", Vector2.zero);
				if (this.IsBump)
				{
					material.SetTextureScale("_BumpMap", value2);
					material.SetTextureOffset("_BumpMap", Vector2.zero);
				}
				if (this.IsHeight)
				{
					material.SetTextureScale("_HeightMap", value2);
					material.SetTextureOffset("_HeightMap", Vector2.zero);
				}
				if (this.IsCutOut)
				{
					material.SetTextureScale("_CutOut", value2);
					material.SetTextureOffset("_CutOut", Vector2.zero);
				}
			}
		}
		else if (this.instanceMaterial != null)
		{
			this.instanceMaterial.SetTextureScale("_MainTex", value2);
			this.instanceMaterial.SetTextureOffset("_MainTex", value);
			if (this.IsBump)
			{
				this.instanceMaterial.SetTextureScale("_BumpMap", value2);
				this.instanceMaterial.SetTextureOffset("_BumpMap", value);
			}
			if (this.IsBump)
			{
				this.instanceMaterial.SetTextureScale("_HeightMap", value2);
				this.instanceMaterial.SetTextureOffset("_HeightMap", value);
			}
			if (this.IsCutOut)
			{
				this.instanceMaterial.SetTextureScale("_CutOut", value2);
				this.instanceMaterial.SetTextureOffset("_CutOut", value);
			}
		}
		else if (this.currentRenderer != null)
		{
			this.currentRenderer.material.SetTextureScale("_MainTex", value2);
			this.currentRenderer.material.SetTextureOffset("_MainTex", value);
			if (this.IsBump)
			{
				this.currentRenderer.material.SetTextureScale("_BumpMap", value2);
				this.currentRenderer.material.SetTextureOffset("_BumpMap", value);
			}
			if (this.IsHeight)
			{
				this.currentRenderer.material.SetTextureScale("_HeightMap", value2);
				this.currentRenderer.material.SetTextureOffset("_HeightMap", value);
			}
			if (this.IsCutOut)
			{
				this.currentRenderer.material.SetTextureScale("_CutOut", value2);
				this.currentRenderer.material.SetTextureOffset("_CutOut", value);
			}
		}
	}

	// Token: 0x060043E2 RID: 17378 RVA: 0x00166EE8 File Offset: 0x001652E8
	private void InitMaterial()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			this.currentRenderer = base.GetComponent<Renderer>();
		}
		else
		{
			Projector component = base.GetComponent<Projector>();
			if (component != null)
			{
				if (!component.material.name.EndsWith("(Instance)"))
				{
					component.material = new Material(component.material)
					{
						name = component.material.name + " (Instance)"
					};
				}
				this.instanceMaterial = component.material;
			}
		}
	}

	// Token: 0x060043E3 RID: 17379 RVA: 0x00166F7E File Offset: 0x0016537E
	private void OnEnable()
	{
		if (!this.isInizialised)
		{
			return;
		}
		this.InitDefaultVariables();
		this.isVisible = true;
		if (!this.isCorutineStarted)
		{
			base.StartCoroutine(this.UpdateCorutine());
		}
	}

	// Token: 0x060043E4 RID: 17380 RVA: 0x00166FB1 File Offset: 0x001653B1
	private void OnDisable()
	{
		this.isCorutineStarted = false;
		this.isVisible = false;
		base.StopAllCoroutines();
	}

	// Token: 0x060043E5 RID: 17381 RVA: 0x00166FC7 File Offset: 0x001653C7
	private void OnBecameVisible()
	{
		this.isVisible = true;
		if (!this.isCorutineStarted)
		{
			base.StartCoroutine(this.UpdateCorutine());
		}
	}

	// Token: 0x060043E6 RID: 17382 RVA: 0x00166FE8 File Offset: 0x001653E8
	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	// Token: 0x060043E7 RID: 17383 RVA: 0x00166FF4 File Offset: 0x001653F4
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

	// Token: 0x060043E8 RID: 17384 RVA: 0x00167010 File Offset: 0x00165410
	private void UpdateCorutineFrame()
	{
		if (this.currentRenderer == null && this.instanceMaterial == null && this.AnimatedMaterialsNotInstance.Length == 0)
		{
			return;
		}
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
		if (this.AnimatedMaterialsNotInstance.Length > 0)
		{
			for (int i = 0; i < this.AnimatedMaterialsNotInstance.Length; i++)
			{
				int num = i * this.OffsetMat + this.index + this.OffsetMat;
				num -= num / this.count * this.count;
				Vector2 value = new Vector2((float)num / (float)this.Columns - (float)(num / this.Columns), 1f - (float)(num / this.Columns) / (float)this.Rows);
				this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_MainTex", value);
				if (this.IsBump)
				{
					this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_BumpMap", value);
				}
				if (this.IsHeight)
				{
					this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_HeightMap", value);
				}
				if (this.IsCutOut)
				{
					this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_CutOut", value);
				}
			}
		}
		else
		{
			Vector2 value2;
			if (this.IsRandomOffsetForInctance)
			{
				int num2 = this.index + this.OffsetMat;
				value2 = new Vector2((float)num2 / (float)this.Columns - (float)(num2 / this.Columns), 1f - (float)(num2 / this.Columns) / (float)this.Rows);
			}
			else
			{
				value2 = new Vector2((float)this.index / (float)this.Columns - (float)(this.index / this.Columns), 1f - (float)(this.index / this.Columns) / (float)this.Rows);
			}
			if (this.instanceMaterial != null)
			{
				this.instanceMaterial.SetTextureOffset("_MainTex", value2);
				if (this.IsBump)
				{
					this.instanceMaterial.SetTextureOffset("_BumpMap", value2);
				}
				if (this.IsHeight)
				{
					this.instanceMaterial.SetTextureOffset("_HeightMap", value2);
				}
				if (this.IsCutOut)
				{
					this.instanceMaterial.SetTextureOffset("_CutOut", value2);
				}
			}
			else if (this.currentRenderer != null)
			{
				this.currentRenderer.material.SetTextureOffset("_MainTex", value2);
				if (this.IsBump)
				{
					this.currentRenderer.material.SetTextureOffset("_BumpMap", value2);
				}
				if (this.IsHeight)
				{
					this.currentRenderer.material.SetTextureOffset("_HeightMap", value2);
				}
				if (this.IsCutOut)
				{
					this.currentRenderer.material.SetTextureOffset("_CutOut", value2);
				}
			}
		}
	}

	// Token: 0x04002CCE RID: 11470
	public Material[] AnimatedMaterialsNotInstance;

	// Token: 0x04002CCF RID: 11471
	public int Rows = 4;

	// Token: 0x04002CD0 RID: 11472
	public int Columns = 4;

	// Token: 0x04002CD1 RID: 11473
	public float Fps = 20f;

	// Token: 0x04002CD2 RID: 11474
	public int OffsetMat;

	// Token: 0x04002CD3 RID: 11475
	public Vector2 SelfTiling = default(Vector2);

	// Token: 0x04002CD4 RID: 11476
	public bool IsLoop = true;

	// Token: 0x04002CD5 RID: 11477
	public bool IsReverse;

	// Token: 0x04002CD6 RID: 11478
	public bool IsRandomOffsetForInctance;

	// Token: 0x04002CD7 RID: 11479
	public bool IsBump;

	// Token: 0x04002CD8 RID: 11480
	public bool IsHeight;

	// Token: 0x04002CD9 RID: 11481
	public bool IsCutOut;

	// Token: 0x04002CDA RID: 11482
	private bool isInizialised;

	// Token: 0x04002CDB RID: 11483
	private int index;

	// Token: 0x04002CDC RID: 11484
	private int count;

	// Token: 0x04002CDD RID: 11485
	private int allCount;

	// Token: 0x04002CDE RID: 11486
	private float deltaFps;

	// Token: 0x04002CDF RID: 11487
	private bool isVisible;

	// Token: 0x04002CE0 RID: 11488
	private bool isCorutineStarted;

	// Token: 0x04002CE1 RID: 11489
	private Renderer currentRenderer;

	// Token: 0x04002CE2 RID: 11490
	private Material instanceMaterial;
}
