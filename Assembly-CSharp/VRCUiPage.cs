using System;
using UnityEngine;

// Token: 0x02000C71 RID: 3185
[RequireComponent(typeof(CanvasGroup))]
public class VRCUiPage : MonoBehaviour
{
	// Token: 0x060062E7 RID: 25319 RVA: 0x0021D64C File Offset: 0x0021BA4C
	public virtual void Awake()
	{
		this.currentlyShown = false;
		this.screen = base.GetComponent<CanvasGroup>();
		this.screen.alpha = 0f;
		if (this.AudioLoop != null)
		{
			this.AudioLoopVolume = this.AudioLoop.volume;
		}
	}

	// Token: 0x060062E8 RID: 25320 RVA: 0x0021D69E File Offset: 0x0021BA9E
	public virtual void Start()
	{
	}

	// Token: 0x060062E9 RID: 25321 RVA: 0x0021D6A0 File Offset: 0x0021BAA0
	public virtual void SetShown(bool shown)
	{
		if (this.currentlyShown == shown)
		{
			return;
		}
		if (shown)
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			if (this.AudioShow != null)
			{
				this.AudioShow.Play();
			}
			if (this.AudioLoop != null)
			{
				this.AudioLoop.Play();
			}
		}
		else if (this.AudioHide != null)
		{
			this.AudioHide.Play();
		}
		this.currentlyShown = shown;
	}

	// Token: 0x060062EA RID: 25322 RVA: 0x0021D73C File Offset: 0x0021BB3C
	public bool GetShown()
	{
		return this.currentlyShown;
	}

	// Token: 0x060062EB RID: 25323 RVA: 0x0021D744 File Offset: 0x0021BB44
	public virtual void Update()
	{
		float target = (!this.currentlyShown) ? 0f : 1f;
		if (this.screen != null)
		{
			this.screen.alpha = Mathf.MoveTowards(this.screen.alpha, target, 2f * Time.deltaTime);
		}
		if (this.AudioLoop != null)
		{
			this.AudioLoop.volume = this.AudioLoopVolume * this.screen.alpha;
		}
		if ((this.screen == null || this.screen.alpha == 0f) && !this.currentlyShown)
		{
			base.gameObject.SetActive(false);
			if (this.AudioLoop != null)
			{
				this.AudioLoop.Stop();
			}
		}
	}

	// Token: 0x060062EC RID: 25324 RVA: 0x0021D82B File Offset: 0x0021BC2B
	public virtual void OnDisable()
	{
	}

	// Token: 0x060062ED RID: 25325 RVA: 0x0021D82D File Offset: 0x0021BC2D
	public virtual void OnEnable()
	{
	}

	// Token: 0x0400486F RID: 18543
	public string screenType;

	// Token: 0x04004870 RID: 18544
	public string displayName;

	// Token: 0x04004871 RID: 18545
	public AudioSource AudioShow;

	// Token: 0x04004872 RID: 18546
	public AudioSource AudioLoop;

	// Token: 0x04004873 RID: 18547
	public AudioSource AudioHide;

	// Token: 0x04004874 RID: 18548
	public Action onPageActivated;

	// Token: 0x04004875 RID: 18549
	public Action onPageDeactivated;

	// Token: 0x04004876 RID: 18550
	protected bool currentlyShown;

	// Token: 0x04004877 RID: 18551
	private float loopVolume;

	// Token: 0x04004878 RID: 18552
	protected CanvasGroup screen;

	// Token: 0x04004879 RID: 18553
	private float AudioLoopVolume = 1f;
}
