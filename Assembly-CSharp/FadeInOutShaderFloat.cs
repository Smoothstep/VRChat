using System;
using UnityEngine;

// Token: 0x0200089F RID: 2207
public class FadeInOutShaderFloat : MonoBehaviour
{
	// Token: 0x060043BD RID: 17341 RVA: 0x00165E3C File Offset: 0x0016423C
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

	// Token: 0x060043BE RID: 17342 RVA: 0x00165E85 File Offset: 0x00164285
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += this.prefabSettings_CollisionEnter;
		}
		this.InitMaterial();
	}

	// Token: 0x060043BF RID: 17343 RVA: 0x00165EC1 File Offset: 0x001642C1
	public void UpdateMaterial(Material instanceMaterial)
	{
		this.mat = instanceMaterial;
		this.InitMaterial();
	}

	// Token: 0x060043C0 RID: 17344 RVA: 0x00165ED0 File Offset: 0x001642D0
	private void InitMaterial()
	{
		if (this.isInitialized)
		{
			return;
		}
		if (base.GetComponent<Renderer>() != null)
		{
			this.mat = base.GetComponent<Renderer>().material;
		}
		else
		{
			LineRenderer component = base.GetComponent<LineRenderer>();
			if (component != null)
			{
				this.mat = component.material;
			}
			else
			{
				Projector component2 = base.GetComponent<Projector>();
				if (component2 != null)
				{
					if (!component2.material.name.EndsWith("(Instance)"))
					{
						component2.material = new Material(component2.material)
						{
							name = component2.material.name + " (Instance)"
						};
					}
					this.mat = component2.material;
				}
			}
		}
		if (this.mat == null)
		{
			return;
		}
		this.isStartDelay = (this.StartDelay > 0.001f);
		this.isIn = (this.FadeInSpeed > 0.001f);
		this.isOut = (this.FadeOutSpeed > 0.001f);
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	// Token: 0x060043C1 RID: 17345 RVA: 0x00165FF4 File Offset: 0x001643F4
	private void InitDefaultVariables()
	{
		this.fadeInComplited = false;
		this.fadeOutComplited = false;
		this.canStartFadeOut = false;
		this.canStart = false;
		this.isCollisionEnter = false;
		this.oldFloat = 0f;
		this.currentFloat = this.MaxFloat;
		if (this.isIn)
		{
			this.currentFloat = 0f;
		}
		this.mat.SetFloat(this.PropertyName, this.currentFloat);
		if (this.isStartDelay)
		{
			base.Invoke("SetupStartDelay", this.StartDelay);
		}
		else
		{
			this.canStart = true;
		}
		if (!this.isIn)
		{
			if (!this.FadeOutAfterCollision)
			{
				base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
			}
			this.oldFloat = this.MaxFloat;
		}
	}

	// Token: 0x060043C2 RID: 17346 RVA: 0x001660C3 File Offset: 0x001644C3
	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		if (!this.isIn && this.FadeOutAfterCollision)
		{
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
	}

	// Token: 0x060043C3 RID: 17347 RVA: 0x001660F3 File Offset: 0x001644F3
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x060043C4 RID: 17348 RVA: 0x00166106 File Offset: 0x00164506
	private void SetupStartDelay()
	{
		this.canStart = true;
	}

	// Token: 0x060043C5 RID: 17349 RVA: 0x0016610F File Offset: 0x0016450F
	private void SetupFadeOutDelay()
	{
		this.canStartFadeOut = true;
	}

	// Token: 0x060043C6 RID: 17350 RVA: 0x00166118 File Offset: 0x00164518
	private void Update()
	{
		if (!this.canStart)
		{
			return;
		}
		if (this.effectSettings != null && this.UseHideStatus)
		{
			if (!this.effectSettings.IsVisible && this.fadeInComplited)
			{
				this.fadeInComplited = false;
			}
			if (this.effectSettings.IsVisible && this.fadeOutComplited)
			{
				this.fadeOutComplited = false;
			}
		}
		if (this.UseHideStatus)
		{
			if (this.isIn && this.effectSettings != null && this.effectSettings.IsVisible && !this.fadeInComplited)
			{
				this.FadeIn();
			}
			if (this.isOut && this.effectSettings != null && !this.effectSettings.IsVisible && !this.fadeOutComplited)
			{
				this.FadeOut();
			}
		}
		else if (!this.FadeOutAfterCollision)
		{
			if (this.isIn && !this.fadeInComplited)
			{
				this.FadeIn();
			}
			if (this.isOut && this.canStartFadeOut && !this.fadeOutComplited)
			{
				this.FadeOut();
			}
		}
		else
		{
			if (this.isIn && !this.fadeInComplited)
			{
				this.FadeIn();
			}
			if (this.isOut && this.isCollisionEnter && this.canStartFadeOut && !this.fadeOutComplited)
			{
				this.FadeOut();
			}
		}
	}

	// Token: 0x060043C7 RID: 17351 RVA: 0x001662BC File Offset: 0x001646BC
	private void FadeIn()
	{
		this.currentFloat = this.oldFloat + Time.deltaTime / this.FadeInSpeed * this.MaxFloat;
		if (this.currentFloat >= this.MaxFloat)
		{
			this.fadeInComplited = true;
			this.currentFloat = this.MaxFloat;
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
		this.mat.SetFloat(this.PropertyName, this.currentFloat);
		this.oldFloat = this.currentFloat;
	}

	// Token: 0x060043C8 RID: 17352 RVA: 0x00166344 File Offset: 0x00164744
	private void FadeOut()
	{
		this.currentFloat = this.oldFloat - Time.deltaTime / this.FadeOutSpeed * this.MaxFloat;
		if (this.currentFloat <= 0f)
		{
			this.currentFloat = 0f;
			this.fadeOutComplited = true;
		}
		this.mat.SetFloat(this.PropertyName, this.currentFloat);
		this.oldFloat = this.currentFloat;
	}

	// Token: 0x04002C95 RID: 11413
	public string PropertyName = "_CutOut";

	// Token: 0x04002C96 RID: 11414
	public float MaxFloat = 1f;

	// Token: 0x04002C97 RID: 11415
	public float StartDelay;

	// Token: 0x04002C98 RID: 11416
	public float FadeInSpeed;

	// Token: 0x04002C99 RID: 11417
	public float FadeOutDelay;

	// Token: 0x04002C9A RID: 11418
	public float FadeOutSpeed;

	// Token: 0x04002C9B RID: 11419
	public bool FadeOutAfterCollision;

	// Token: 0x04002C9C RID: 11420
	public bool UseHideStatus;

	// Token: 0x04002C9D RID: 11421
	private Material OwnMaterial;

	// Token: 0x04002C9E RID: 11422
	private Material mat;

	// Token: 0x04002C9F RID: 11423
	private float oldFloat;

	// Token: 0x04002CA0 RID: 11424
	private float currentFloat;

	// Token: 0x04002CA1 RID: 11425
	private bool canStart;

	// Token: 0x04002CA2 RID: 11426
	private bool canStartFadeOut;

	// Token: 0x04002CA3 RID: 11427
	private bool fadeInComplited;

	// Token: 0x04002CA4 RID: 11428
	private bool fadeOutComplited;

	// Token: 0x04002CA5 RID: 11429
	private bool previousFrameVisibleStatus;

	// Token: 0x04002CA6 RID: 11430
	private bool isCollisionEnter;

	// Token: 0x04002CA7 RID: 11431
	private bool isStartDelay;

	// Token: 0x04002CA8 RID: 11432
	private bool isIn;

	// Token: 0x04002CA9 RID: 11433
	private bool isOut;

	// Token: 0x04002CAA RID: 11434
	private EffectSettings effectSettings;

	// Token: 0x04002CAB RID: 11435
	private bool isInitialized;
}
