using System;
using UnityEngine;

// Token: 0x02000624 RID: 1572
[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("NGUI/Tween/Tween Volume")]
public class TweenVolume : UITweener
{
	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x060034A4 RID: 13476 RVA: 0x0010A090 File Offset: 0x00108490
	public AudioSource audioSource
	{
		get
		{
			if (this.mSource == null)
			{
				this.mSource = base.GetComponent<AudioSource>();
				if (this.mSource == null)
				{
					this.mSource = base.GetComponent<AudioSource>();
					if (this.mSource == null)
					{
						Debug.LogError("TweenVolume needs an AudioSource to work with", this);
						base.enabled = false;
					}
				}
			}
			return this.mSource;
		}
	}

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x060034A5 RID: 13477 RVA: 0x0010A100 File Offset: 0x00108500
	// (set) Token: 0x060034A6 RID: 13478 RVA: 0x0010A108 File Offset: 0x00108508
	[Obsolete("Use 'value' instead")]
	public float volume
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

	// Token: 0x170007FB RID: 2043
	// (get) Token: 0x060034A7 RID: 13479 RVA: 0x0010A111 File Offset: 0x00108511
	// (set) Token: 0x060034A8 RID: 13480 RVA: 0x0010A139 File Offset: 0x00108539
	public float value
	{
		get
		{
			return (!(this.audioSource != null)) ? 0f : this.mSource.volume;
		}
		set
		{
			if (this.audioSource != null)
			{
				this.mSource.volume = value;
			}
		}
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x0010A158 File Offset: 0x00108558
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		this.mSource.enabled = (this.mSource.volume > 0.01f);
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x0010A194 File Offset: 0x00108594
	public static TweenVolume Begin(GameObject go, float duration, float targetVolume)
	{
		TweenVolume tweenVolume = UITweener.Begin<TweenVolume>(go, duration);
		tweenVolume.from = tweenVolume.value;
		tweenVolume.to = targetVolume;
		return tweenVolume;
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x0010A1BD File Offset: 0x001085BD
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x0010A1CB File Offset: 0x001085CB
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x04001DF8 RID: 7672
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x04001DF9 RID: 7673
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x04001DFA RID: 7674
	private AudioSource mSource;
}
