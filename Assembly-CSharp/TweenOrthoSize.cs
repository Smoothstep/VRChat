using System;
using UnityEngine;

// Token: 0x0200061F RID: 1567
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/Tween/Tween Orthographic Size")]
public class TweenOrthoSize : UITweener
{
	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x06003470 RID: 13424 RVA: 0x00109876 File Offset: 0x00107C76
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

	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x06003471 RID: 13425 RVA: 0x0010989B File Offset: 0x00107C9B
	// (set) Token: 0x06003472 RID: 13426 RVA: 0x001098A3 File Offset: 0x00107CA3
	[Obsolete("Use 'value' instead")]
	public float orthoSize
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

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x06003473 RID: 13427 RVA: 0x001098AC File Offset: 0x00107CAC
	// (set) Token: 0x06003474 RID: 13428 RVA: 0x001098B9 File Offset: 0x00107CB9
	public float value
	{
		get
		{
			return this.cachedCamera.orthographicSize;
		}
		set
		{
			this.cachedCamera.orthographicSize = value;
		}
	}

	// Token: 0x06003475 RID: 13429 RVA: 0x001098C7 File Offset: 0x00107CC7
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06003476 RID: 13430 RVA: 0x001098E8 File Offset: 0x00107CE8
	public static TweenOrthoSize Begin(GameObject go, float duration, float to)
	{
		TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration);
		tweenOrthoSize.from = tweenOrthoSize.value;
		tweenOrthoSize.to = to;
		if (duration <= 0f)
		{
			tweenOrthoSize.Sample(1f, true);
			tweenOrthoSize.enabled = false;
		}
		return tweenOrthoSize;
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x0010992F File Offset: 0x00107D2F
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06003478 RID: 13432 RVA: 0x0010993D File Offset: 0x00107D3D
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x04001DE1 RID: 7649
	public float from = 1f;

	// Token: 0x04001DE2 RID: 7650
	public float to = 1f;

	// Token: 0x04001DE3 RID: 7651
	private Camera mCam;
}
