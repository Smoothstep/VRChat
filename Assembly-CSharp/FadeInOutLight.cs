using System;
using UnityEngine;

// Token: 0x0200089B RID: 2203
public class FadeInOutLight : MonoBehaviour
{
	// Token: 0x0600439A RID: 17306 RVA: 0x00164EC0 File Offset: 0x001632C0
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

	// Token: 0x0600439B RID: 17307 RVA: 0x00164F0C File Offset: 0x0016330C
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += this.prefabSettings_CollisionEnter;
		}
		this.goLight = base.GetComponent<Light>();
		this.startIntensity = this.goLight.intensity;
		this.isStartDelay = (this.StartDelay > 0.001f);
		this.isIn = (this.FadeInSpeed > 0.001f);
		this.isOut = (this.FadeOutSpeed > 0.001f);
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	// Token: 0x0600439C RID: 17308 RVA: 0x00164FB0 File Offset: 0x001633B0
	private void InitDefaultVariables()
	{
		this.fadeInComplited = false;
		this.fadeOutComplited = false;
		this.allComplited = false;
		this.canStartFadeOut = false;
		this.isCollisionEnter = false;
		this.oldIntensity = 0f;
		this.currentIntensity = 0f;
		this.canStart = false;
		this.goLight.intensity = ((!this.isIn) ? this.startIntensity : 0f);
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
			this.oldIntensity = this.startIntensity;
		}
	}

	// Token: 0x0600439D RID: 17309 RVA: 0x0016507E File Offset: 0x0016347E
	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		if (!this.isIn && this.FadeOutAfterCollision)
		{
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
	}

	// Token: 0x0600439E RID: 17310 RVA: 0x001650AE File Offset: 0x001634AE
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x0600439F RID: 17311 RVA: 0x001650C1 File Offset: 0x001634C1
	private void SetupStartDelay()
	{
		this.canStart = true;
	}

	// Token: 0x060043A0 RID: 17312 RVA: 0x001650CA File Offset: 0x001634CA
	private void SetupFadeOutDelay()
	{
		this.canStartFadeOut = true;
	}

	// Token: 0x060043A1 RID: 17313 RVA: 0x001650D4 File Offset: 0x001634D4
	private void Update()
	{
		if (!this.canStart)
		{
			return;
		}
		if (this.effectSettings != null && this.UseHideStatus && this.allComplited && this.effectSettings.IsVisible)
		{
			this.allComplited = false;
			this.fadeInComplited = false;
			this.fadeOutComplited = false;
			this.InitDefaultVariables();
		}
		if (this.isIn && !this.fadeInComplited)
		{
			if (this.effectSettings == null)
			{
				this.FadeIn();
			}
			else if ((this.UseHideStatus && this.effectSettings.IsVisible) || !this.UseHideStatus)
			{
				this.FadeIn();
			}
		}
		if (!this.isOut || this.fadeOutComplited || !this.canStartFadeOut)
		{
			return;
		}
		if (this.effectSettings == null || (!this.UseHideStatus && !this.FadeOutAfterCollision))
		{
			this.FadeOut();
		}
		else if ((this.UseHideStatus && !this.effectSettings.IsVisible) || (this.FadeOutAfterCollision && this.isCollisionEnter))
		{
			this.FadeOut();
		}
	}

	// Token: 0x060043A2 RID: 17314 RVA: 0x00165228 File Offset: 0x00163628
	private void FadeIn()
	{
		this.currentIntensity = this.oldIntensity + Time.deltaTime / this.FadeInSpeed * this.startIntensity;
		if (this.currentIntensity >= this.startIntensity)
		{
			this.fadeInComplited = true;
			if (!this.isOut)
			{
				this.allComplited = true;
			}
			this.currentIntensity = this.startIntensity;
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
		this.goLight.intensity = this.currentIntensity;
		this.oldIntensity = this.currentIntensity;
	}

	// Token: 0x060043A3 RID: 17315 RVA: 0x001652BC File Offset: 0x001636BC
	private void FadeOut()
	{
		this.currentIntensity = this.oldIntensity - Time.deltaTime / this.FadeOutSpeed * this.startIntensity;
		if (this.currentIntensity <= 0f)
		{
			this.currentIntensity = 0f;
			this.fadeOutComplited = true;
			this.allComplited = true;
		}
		this.goLight.intensity = this.currentIntensity;
		this.oldIntensity = this.currentIntensity;
	}

	// Token: 0x04002C5A RID: 11354
	public float StartDelay;

	// Token: 0x04002C5B RID: 11355
	public float FadeInSpeed;

	// Token: 0x04002C5C RID: 11356
	public float FadeOutDelay;

	// Token: 0x04002C5D RID: 11357
	public float FadeOutSpeed;

	// Token: 0x04002C5E RID: 11358
	public bool FadeOutAfterCollision;

	// Token: 0x04002C5F RID: 11359
	public bool UseHideStatus;

	// Token: 0x04002C60 RID: 11360
	private Light goLight;

	// Token: 0x04002C61 RID: 11361
	private float oldIntensity;

	// Token: 0x04002C62 RID: 11362
	private float currentIntensity;

	// Token: 0x04002C63 RID: 11363
	private float startIntensity;

	// Token: 0x04002C64 RID: 11364
	private bool canStart;

	// Token: 0x04002C65 RID: 11365
	private bool canStartFadeOut;

	// Token: 0x04002C66 RID: 11366
	private bool fadeInComplited;

	// Token: 0x04002C67 RID: 11367
	private bool fadeOutComplited;

	// Token: 0x04002C68 RID: 11368
	private bool isCollisionEnter;

	// Token: 0x04002C69 RID: 11369
	private bool allComplited;

	// Token: 0x04002C6A RID: 11370
	private bool isStartDelay;

	// Token: 0x04002C6B RID: 11371
	private bool isIn;

	// Token: 0x04002C6C RID: 11372
	private bool isOut;

	// Token: 0x04002C6D RID: 11373
	private EffectSettings effectSettings;

	// Token: 0x04002C6E RID: 11374
	private bool isInitialized;
}
