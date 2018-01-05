using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C39 RID: 3129
public class SpriteSwapAnimation : MonoBehaviour
{
	// Token: 0x06006130 RID: 24880 RVA: 0x00224764 File Offset: 0x00222B64
	private void Awake()
	{
		this.currentFrameTime = 0f;
		this.framePeriod = 1f / (float)this.frameRate;
	}

	// Token: 0x06006131 RID: 24881 RVA: 0x00224784 File Offset: 0x00222B84
	private void OnEnabled()
	{
		this.currentFrameTime = 0f;
		this.currentFrame = 0;
	}

	// Token: 0x06006132 RID: 24882 RVA: 0x00224798 File Offset: 0x00222B98
	private void Update()
	{
		if (!base.gameObject.activeInHierarchy || !this.image.enabled || this.sprites.Length == 0)
		{
			return;
		}
		this.currentFrameTime += Time.deltaTime;
		if (this.currentFrameTime > this.framePeriod)
		{
			this.currentFrame++;
			if (this.currentFrame >= this.sprites.Length)
			{
				this.currentFrame = 0;
			}
			this.image.sprite = this.sprites[this.currentFrame];
			this.currentFrameTime = 0f;
		}
	}

	// Token: 0x040046CE RID: 18126
	public Image image;

	// Token: 0x040046CF RID: 18127
	public Sprite[] sprites;

	// Token: 0x040046D0 RID: 18128
	public int frameRate = 4;

	// Token: 0x040046D1 RID: 18129
	private int currentFrame;

	// Token: 0x040046D2 RID: 18130
	private float currentFrameTime;

	// Token: 0x040046D3 RID: 18131
	private float framePeriod = 1f;
}
