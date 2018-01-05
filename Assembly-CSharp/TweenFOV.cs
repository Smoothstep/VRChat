using System;
using UnityEngine;

// Token: 0x0200061D RID: 1565
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/Tween/Tween Field of View")]
public class TweenFOV : UITweener
{
	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x06003458 RID: 13400 RVA: 0x001095F5 File Offset: 0x001079F5
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x06003459 RID: 13401 RVA: 0x0010961A File Offset: 0x00107A1A
	// (set) Token: 0x0600345A RID: 13402 RVA: 0x00109622 File Offset: 0x00107A22
	[Obsolete("Use 'value' instead")]
	public float fov
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x0600345B RID: 13403 RVA: 0x0010962B File Offset: 0x00107A2B
	// (set) Token: 0x0600345C RID: 13404 RVA: 0x00109638 File Offset: 0x00107A38
	public float value
	{
		get
		{
			return this.cachedCamera.fieldOfView;
		}
		set
		{
			this.cachedCamera.fieldOfView = value;
		}
	}

	// Token: 0x0600345D RID: 13405 RVA: 0x00109646 File Offset: 0x00107A46
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x0600345E RID: 13406 RVA: 0x00109668 File Offset: 0x00107A68
	public static TweenFOV Begin(GameObject go, float duration, float to)
	{
		TweenFOV tweenFOV = UITweener.Begin<TweenFOV>(go, duration);
		tweenFOV.from = tweenFOV.value;
		tweenFOV.to = to;
		if (duration <= 0f)
		{
			tweenFOV.Sample(1f, true);
			tweenFOV.enabled = false;
		}
		return tweenFOV;
	}

	// Token: 0x0600345F RID: 13407 RVA: 0x001096AF File Offset: 0x00107AAF
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x001096BD File Offset: 0x00107ABD
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06003461 RID: 13409 RVA: 0x001096CB File Offset: 0x00107ACB
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06003462 RID: 13410 RVA: 0x001096D9 File Offset: 0x00107AD9
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001DD9 RID: 7641
	public float from = 45f;

	// Token: 0x04001DDA RID: 7642
	public float to = 45f;

	// Token: 0x04001DDB RID: 7643
	private Camera mCam;
}
