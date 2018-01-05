using System;
using UnityEngine;

// Token: 0x0200089D RID: 2205
public class FadeInOutScale : MonoBehaviour
{
	// Token: 0x060043A9 RID: 17321 RVA: 0x001654C0 File Offset: 0x001638C0
	private void Start()
	{
		this.t = base.transform;
		this.oldScale = this.t.localScale;
		this.isInitialized = true;
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += this.prefabSettings_CollisionEnter;
		}
	}

	// Token: 0x060043AA RID: 17322 RVA: 0x00165528 File Offset: 0x00163928
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	// Token: 0x060043AB RID: 17323 RVA: 0x00165574 File Offset: 0x00163974
	public void InitDefaultVariables()
	{
		if (this.FadeInOutStatus == FadeInOutStatus.OutAfterCollision)
		{
			this.t.localScale = this.oldScale;
			this.canUpdate = false;
		}
		else
		{
			this.t.localScale = Vector3.zero;
			this.canUpdate = true;
		}
		this.updateTime = true;
		this.time = 0f;
		this.oldSin = 0f;
		this.isCollisionEnter = false;
	}

	// Token: 0x060043AC RID: 17324 RVA: 0x001655E5 File Offset: 0x001639E5
	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		this.canUpdate = true;
	}

	// Token: 0x060043AD RID: 17325 RVA: 0x001655F5 File Offset: 0x001639F5
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x060043AE RID: 17326 RVA: 0x00165608 File Offset: 0x00163A08
	private void Update()
	{
		if (!this.canUpdate)
		{
			return;
		}
		if (this.updateTime)
		{
			this.time = Time.time;
			this.updateTime = false;
		}
		float num = Mathf.Sin((Time.time - this.time) / this.Speed);
		float num2;
		if (this.oldSin > num)
		{
			this.canUpdate = false;
			num2 = this.MaxScale;
		}
		else
		{
			num2 = num * this.MaxScale;
		}
		if (this.FadeInOutStatus == FadeInOutStatus.In)
		{
			if (num2 < this.MaxScale)
			{
				this.t.localScale = new Vector3(this.oldScale.x * num2, this.oldScale.y * num2, this.oldScale.z * num2);
			}
			else
			{
				this.t.localScale = new Vector3(this.MaxScale, this.MaxScale, this.MaxScale);
			}
		}
		if (this.FadeInOutStatus == FadeInOutStatus.Out)
		{
			if (num2 > 0f)
			{
				this.t.localScale = new Vector3(this.MaxScale * this.oldScale.x - this.oldScale.x * num2, this.MaxScale * this.oldScale.y - this.oldScale.y * num2, this.MaxScale * this.oldScale.z - this.oldScale.z * num2);
			}
			else
			{
				this.t.localScale = Vector3.zero;
			}
		}
		if (this.FadeInOutStatus == FadeInOutStatus.OutAfterCollision && this.isCollisionEnter)
		{
			if (num2 > 0f)
			{
				this.t.localScale = new Vector3(this.MaxScale * this.oldScale.x - this.oldScale.x * num2, this.MaxScale * this.oldScale.y - this.oldScale.y * num2, this.MaxScale * this.oldScale.z - this.oldScale.z * num2);
			}
			else
			{
				this.t.localScale = Vector3.zero;
			}
		}
		this.oldSin = num;
	}

	// Token: 0x04002C72 RID: 11378
	public FadeInOutStatus FadeInOutStatus;

	// Token: 0x04002C73 RID: 11379
	public float Speed = 1f;

	// Token: 0x04002C74 RID: 11380
	public float MaxScale = 2f;

	// Token: 0x04002C75 RID: 11381
	private Vector3 oldScale;

	// Token: 0x04002C76 RID: 11382
	private float time;

	// Token: 0x04002C77 RID: 11383
	private float oldSin;

	// Token: 0x04002C78 RID: 11384
	private bool updateTime = true;

	// Token: 0x04002C79 RID: 11385
	private bool canUpdate = true;

	// Token: 0x04002C7A RID: 11386
	private Transform t;

	// Token: 0x04002C7B RID: 11387
	private EffectSettings effectSettings;

	// Token: 0x04002C7C RID: 11388
	private bool isInitialized;

	// Token: 0x04002C7D RID: 11389
	private bool isCollisionEnter;
}
