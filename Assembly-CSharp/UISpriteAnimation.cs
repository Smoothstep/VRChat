using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000657 RID: 1623
[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation")]
public class UISpriteAnimation : MonoBehaviour
{
	// Token: 0x17000892 RID: 2194
	// (get) Token: 0x060036BD RID: 14013 RVA: 0x00117E67 File Offset: 0x00116267
	public int frames
	{
		get
		{
			return this.mSpriteNames.Count;
		}
	}

	// Token: 0x17000893 RID: 2195
	// (get) Token: 0x060036BE RID: 14014 RVA: 0x00117E74 File Offset: 0x00116274
	// (set) Token: 0x060036BF RID: 14015 RVA: 0x00117E7C File Offset: 0x0011627C
	public int framesPerSecond
	{
		get
		{
			return this.mFPS;
		}
		set
		{
			this.mFPS = value;
		}
	}

	// Token: 0x17000894 RID: 2196
	// (get) Token: 0x060036C0 RID: 14016 RVA: 0x00117E85 File Offset: 0x00116285
	// (set) Token: 0x060036C1 RID: 14017 RVA: 0x00117E8D File Offset: 0x0011628D
	public string namePrefix
	{
		get
		{
			return this.mPrefix;
		}
		set
		{
			if (this.mPrefix != value)
			{
				this.mPrefix = value;
				this.RebuildSpriteList();
			}
		}
	}

	// Token: 0x17000895 RID: 2197
	// (get) Token: 0x060036C2 RID: 14018 RVA: 0x00117EAD File Offset: 0x001162AD
	// (set) Token: 0x060036C3 RID: 14019 RVA: 0x00117EB5 File Offset: 0x001162B5
	public bool loop
	{
		get
		{
			return this.mLoop;
		}
		set
		{
			this.mLoop = value;
		}
	}

	// Token: 0x17000896 RID: 2198
	// (get) Token: 0x060036C4 RID: 14020 RVA: 0x00117EBE File Offset: 0x001162BE
	public bool isPlaying
	{
		get
		{
			return this.mActive;
		}
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x00117EC6 File Offset: 0x001162C6
	protected virtual void Start()
	{
		this.RebuildSpriteList();
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x00117ED0 File Offset: 0x001162D0
	protected virtual void Update()
	{
		if (this.mActive && this.mSpriteNames.Count > 1 && Application.isPlaying && this.mFPS > 0)
		{
			this.mDelta += RealTime.deltaTime;
			float num = 1f / (float)this.mFPS;
			if (num < this.mDelta)
			{
				this.mDelta = ((num <= 0f) ? 0f : (this.mDelta - num));
				if (++this.mIndex >= this.mSpriteNames.Count)
				{
					this.mIndex = 0;
					this.mActive = this.mLoop;
				}
				if (this.mActive)
				{
					this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
					if (this.mSnap)
					{
						this.mSprite.MakePixelPerfect();
					}
				}
			}
		}
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x00117FD0 File Offset: 0x001163D0
	public void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
			int i = 0;
			int count = spriteList.Count;
			while (i < count)
			{
				UISpriteData uispriteData = spriteList[i];
				if (string.IsNullOrEmpty(this.mPrefix) || uispriteData.name.StartsWith(this.mPrefix))
				{
					this.mSpriteNames.Add(uispriteData.name);
				}
				i++;
			}
			this.mSpriteNames.Sort();
		}
	}

	// Token: 0x060036C8 RID: 14024 RVA: 0x001180A0 File Offset: 0x001164A0
	public void Play()
	{
		this.mActive = true;
	}

	// Token: 0x060036C9 RID: 14025 RVA: 0x001180A9 File Offset: 0x001164A9
	public void Pause()
	{
		this.mActive = false;
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x001180B4 File Offset: 0x001164B4
	public void ResetToBeginning()
	{
		this.mActive = true;
		this.mIndex = 0;
		if (this.mSprite != null && this.mSpriteNames.Count > 0)
		{
			this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
			if (this.mSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	// Token: 0x04001F9C RID: 8092
	[HideInInspector]
	[SerializeField]
	protected int mFPS = 30;

	// Token: 0x04001F9D RID: 8093
	[HideInInspector]
	[SerializeField]
	protected string mPrefix = string.Empty;

	// Token: 0x04001F9E RID: 8094
	[HideInInspector]
	[SerializeField]
	protected bool mLoop = true;

	// Token: 0x04001F9F RID: 8095
	[HideInInspector]
	[SerializeField]
	protected bool mSnap = true;

	// Token: 0x04001FA0 RID: 8096
	protected UISprite mSprite;

	// Token: 0x04001FA1 RID: 8097
	protected float mDelta;

	// Token: 0x04001FA2 RID: 8098
	protected int mIndex;

	// Token: 0x04001FA3 RID: 8099
	protected bool mActive = true;

	// Token: 0x04001FA4 RID: 8100
	protected List<string> mSpriteNames = new List<string>();
}
