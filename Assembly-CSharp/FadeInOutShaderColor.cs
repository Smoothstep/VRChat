using System;
using UnityEngine;

// Token: 0x0200089E RID: 2206
public class FadeInOutShaderColor : MonoBehaviour
{
	// Token: 0x060043B0 RID: 17328 RVA: 0x00165854 File Offset: 0x00163C54
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

	// Token: 0x060043B1 RID: 17329 RVA: 0x0016589D File Offset: 0x00163C9D
	public void UpdateMaterial(Material instanceMaterial)
	{
		this.mat = instanceMaterial;
		this.InitMaterial();
	}

	// Token: 0x060043B2 RID: 17330 RVA: 0x001658AC File Offset: 0x00163CAC
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += this.prefabSettings_CollisionEnter;
		}
		this.InitMaterial();
	}

	// Token: 0x060043B3 RID: 17331 RVA: 0x001658E8 File Offset: 0x00163CE8
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
		this.oldColor = this.mat.GetColor(this.ShaderColorName);
		this.isStartDelay = (this.StartDelay > 0.001f);
		this.isIn = (this.FadeInSpeed > 0.001f);
		this.isOut = (this.FadeOutSpeed > 0.001f);
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	// Token: 0x060043B4 RID: 17332 RVA: 0x00165A24 File Offset: 0x00163E24
	private void InitDefaultVariables()
	{
		this.fadeInComplited = false;
		this.fadeOutComplited = false;
		this.canStartFadeOut = false;
		this.isCollisionEnter = false;
		this.oldAlpha = 0f;
		this.alpha = 0f;
		this.canStart = false;
		this.currentColor = this.oldColor;
		if (this.isIn)
		{
			this.currentColor.a = 0f;
		}
		this.mat.SetColor(this.ShaderColorName, this.currentColor);
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
			this.oldAlpha = this.oldColor.a;
		}
	}

	// Token: 0x060043B5 RID: 17333 RVA: 0x00165B08 File Offset: 0x00163F08
	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		if (!this.isIn && this.FadeOutAfterCollision)
		{
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
	}

	// Token: 0x060043B6 RID: 17334 RVA: 0x00165B38 File Offset: 0x00163F38
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x060043B7 RID: 17335 RVA: 0x00165B4B File Offset: 0x00163F4B
	private void SetupStartDelay()
	{
		this.canStart = true;
	}

	// Token: 0x060043B8 RID: 17336 RVA: 0x00165B54 File Offset: 0x00163F54
	private void SetupFadeOutDelay()
	{
		this.canStartFadeOut = true;
	}

	// Token: 0x060043B9 RID: 17337 RVA: 0x00165B60 File Offset: 0x00163F60
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

	// Token: 0x060043BA RID: 17338 RVA: 0x00165D04 File Offset: 0x00164104
	private void FadeIn()
	{
		this.alpha = this.oldAlpha + Time.deltaTime / this.FadeInSpeed;
		if (this.alpha >= this.oldColor.a)
		{
			this.fadeInComplited = true;
			this.alpha = this.oldColor.a;
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
		this.currentColor.a = this.alpha;
		this.mat.SetColor(this.ShaderColorName, this.currentColor);
		this.oldAlpha = this.alpha;
	}

	// Token: 0x060043BB RID: 17339 RVA: 0x00165DA0 File Offset: 0x001641A0
	private void FadeOut()
	{
		this.alpha = this.oldAlpha - Time.deltaTime / this.FadeOutSpeed;
		if (this.alpha <= 0f)
		{
			this.alpha = 0f;
			this.fadeOutComplited = true;
		}
		this.currentColor.a = this.alpha;
		this.mat.SetColor(this.ShaderColorName, this.currentColor);
		this.oldAlpha = this.alpha;
	}

	// Token: 0x04002C7E RID: 11390
	public string ShaderColorName = "_Color";

	// Token: 0x04002C7F RID: 11391
	public float StartDelay;

	// Token: 0x04002C80 RID: 11392
	public float FadeInSpeed;

	// Token: 0x04002C81 RID: 11393
	public float FadeOutDelay;

	// Token: 0x04002C82 RID: 11394
	public float FadeOutSpeed;

	// Token: 0x04002C83 RID: 11395
	public bool UseSharedMaterial;

	// Token: 0x04002C84 RID: 11396
	public bool FadeOutAfterCollision;

	// Token: 0x04002C85 RID: 11397
	public bool UseHideStatus;

	// Token: 0x04002C86 RID: 11398
	private Material mat;

	// Token: 0x04002C87 RID: 11399
	private Color oldColor;

	// Token: 0x04002C88 RID: 11400
	private Color currentColor;

	// Token: 0x04002C89 RID: 11401
	private float oldAlpha;

	// Token: 0x04002C8A RID: 11402
	private float alpha;

	// Token: 0x04002C8B RID: 11403
	private bool canStart;

	// Token: 0x04002C8C RID: 11404
	private bool canStartFadeOut;

	// Token: 0x04002C8D RID: 11405
	private bool fadeInComplited;

	// Token: 0x04002C8E RID: 11406
	private bool fadeOutComplited;

	// Token: 0x04002C8F RID: 11407
	private bool isCollisionEnter;

	// Token: 0x04002C90 RID: 11408
	private bool isStartDelay;

	// Token: 0x04002C91 RID: 11409
	private bool isIn;

	// Token: 0x04002C92 RID: 11410
	private bool isOut;

	// Token: 0x04002C93 RID: 11411
	private EffectSettings effectSettings;

	// Token: 0x04002C94 RID: 11412
	private bool isInitialized;
}
