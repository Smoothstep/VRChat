using System;
using UnityEngine;

// Token: 0x0200062A RID: 1578
public class UI2DSpriteAnimation : MonoBehaviour
{
	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x060034E4 RID: 13540 RVA: 0x0010ACA4 File Offset: 0x001090A4
	public bool isPlaying
	{
		get
		{
			return base.enabled;
		}
	}

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x060034E5 RID: 13541 RVA: 0x0010ACAC File Offset: 0x001090AC
	// (set) Token: 0x060034E6 RID: 13542 RVA: 0x0010ACB4 File Offset: 0x001090B4
	public int framesPerSecond
	{
		get
		{
			return this.framerate;
		}
		set
		{
			this.framerate = value;
		}
	}

	// Token: 0x060034E7 RID: 13543 RVA: 0x0010ACC0 File Offset: 0x001090C0
	public void Play()
	{
		if (this.frames != null && this.frames.Length > 0)
		{
			if (!base.enabled && !this.loop)
			{
				int num = (this.framerate <= 0) ? (this.mIndex - 1) : (this.mIndex + 1);
				if (num < 0 || num >= this.frames.Length)
				{
					this.mIndex = ((this.framerate >= 0) ? 0 : (this.frames.Length - 1));
				}
			}
			base.enabled = true;
			this.UpdateSprite();
		}
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x0010AD62 File Offset: 0x00109162
	public void Pause()
	{
		base.enabled = false;
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x0010AD6B File Offset: 0x0010916B
	public void ResetToBeginning()
	{
		this.mIndex = ((this.framerate >= 0) ? 0 : (this.frames.Length - 1));
		this.UpdateSprite();
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x0010AD95 File Offset: 0x00109195
	private void Start()
	{
		this.Play();
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x0010ADA0 File Offset: 0x001091A0
	private void Update()
	{
		if (this.frames == null || this.frames.Length == 0)
		{
			base.enabled = false;
		}
		else if (this.framerate != 0)
		{
			float num = (!this.ignoreTimeScale) ? Time.time : RealTime.time;
			if (this.mUpdate < num)
			{
				this.mUpdate = num;
				int num2 = (this.framerate <= 0) ? (this.mIndex - 1) : (this.mIndex + 1);
				if (!this.loop && (num2 < 0 || num2 >= this.frames.Length))
				{
					base.enabled = false;
					return;
				}
				this.mIndex = NGUIMath.RepeatIndex(num2, this.frames.Length);
				this.UpdateSprite();
			}
		}
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x0010AE70 File Offset: 0x00109270
	private void UpdateSprite()
	{
		if (this.mUnitySprite == null && this.mNguiSprite == null)
		{
			this.mUnitySprite = base.GetComponent<SpriteRenderer>();
			this.mNguiSprite = base.GetComponent<UI2DSprite>();
			if (this.mUnitySprite == null && this.mNguiSprite == null)
			{
				base.enabled = false;
				return;
			}
		}
		float num = (!this.ignoreTimeScale) ? Time.time : RealTime.time;
		if (this.framerate != 0)
		{
			this.mUpdate = num + Mathf.Abs(1f / (float)this.framerate);
		}
		if (this.mUnitySprite != null)
		{
			this.mUnitySprite.sprite = this.frames[this.mIndex];
		}
		else if (this.mNguiSprite != null)
		{
			this.mNguiSprite.nextSprite = this.frames[this.mIndex];
		}
	}

	// Token: 0x04001E25 RID: 7717
	[SerializeField]
	protected int framerate = 20;

	// Token: 0x04001E26 RID: 7718
	public bool ignoreTimeScale = true;

	// Token: 0x04001E27 RID: 7719
	public bool loop = true;

	// Token: 0x04001E28 RID: 7720
	public Sprite[] frames;

	// Token: 0x04001E29 RID: 7721
	private SpriteRenderer mUnitySprite;

	// Token: 0x04001E2A RID: 7722
	private UI2DSprite mNguiSprite;

	// Token: 0x04001E2B RID: 7723
	private int mIndex;

	// Token: 0x04001E2C RID: 7724
	private float mUpdate;
}
