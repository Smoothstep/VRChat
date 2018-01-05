using System;
using UnityEngine;

// Token: 0x020005FC RID: 1532
public abstract class UIBasicSprite : UIWidget
{
	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x06003333 RID: 13107 RVA: 0x001041DA File Offset: 0x001025DA
	// (set) Token: 0x06003334 RID: 13108 RVA: 0x001041E2 File Offset: 0x001025E2
	public virtual UIBasicSprite.Type type
	{
		get
		{
			return this.mType;
		}
		set
		{
			if (this.mType != value)
			{
				this.mType = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x06003335 RID: 13109 RVA: 0x001041FD File Offset: 0x001025FD
	// (set) Token: 0x06003336 RID: 13110 RVA: 0x00104205 File Offset: 0x00102605
	public UIBasicSprite.Flip flip
	{
		get
		{
			return this.mFlip;
		}
		set
		{
			if (this.mFlip != value)
			{
				this.mFlip = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x06003337 RID: 13111 RVA: 0x00104220 File Offset: 0x00102620
	// (set) Token: 0x06003338 RID: 13112 RVA: 0x00104228 File Offset: 0x00102628
	public UIBasicSprite.FillDirection fillDirection
	{
		get
		{
			return this.mFillDirection;
		}
		set
		{
			if (this.mFillDirection != value)
			{
				this.mFillDirection = value;
				this.mChanged = true;
			}
		}
	}

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x06003339 RID: 13113 RVA: 0x00104244 File Offset: 0x00102644
	// (set) Token: 0x0600333A RID: 13114 RVA: 0x0010424C File Offset: 0x0010264C
	public float fillAmount
	{
		get
		{
			return this.mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mFillAmount != num)
			{
				this.mFillAmount = num;
				this.mChanged = true;
			}
		}
	}

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x0600333B RID: 13115 RVA: 0x0010427C File Offset: 0x0010267C
	public override int minWidth
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.x + vector.z);
				return Mathf.Max(base.minWidth, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minWidth;
		}
	}

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x0600333C RID: 13116 RVA: 0x001042EC File Offset: 0x001026EC
	public override int minHeight
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.y + vector.w);
				return Mathf.Max(base.minHeight, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minHeight;
		}
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x0600333D RID: 13117 RVA: 0x0010435C File Offset: 0x0010275C
	// (set) Token: 0x0600333E RID: 13118 RVA: 0x00104364 File Offset: 0x00102764
	public bool invert
	{
		get
		{
			return this.mInvert;
		}
		set
		{
			if (this.mInvert != value)
			{
				this.mInvert = value;
				this.mChanged = true;
			}
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x0600333F RID: 13119 RVA: 0x00104380 File Offset: 0x00102780
	public bool hasBorder
	{
		get
		{
			Vector4 border = this.border;
			return border.x != 0f || border.y != 0f || border.z != 0f || border.w != 0f;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x06003340 RID: 13120 RVA: 0x001043DB File Offset: 0x001027DB
	public virtual bool premultipliedAlpha
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x06003341 RID: 13121 RVA: 0x001043DE File Offset: 0x001027DE
	public virtual float pixelSize
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x06003342 RID: 13122 RVA: 0x001043E8 File Offset: 0x001027E8
	private Vector4 drawingUVs
	{
		get
		{
			switch (this.mFlip)
			{
			case UIBasicSprite.Flip.Horizontally:
				return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMin, this.mOuterUV.xMin, this.mOuterUV.yMax);
			case UIBasicSprite.Flip.Vertically:
				return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMax, this.mOuterUV.xMax, this.mOuterUV.yMin);
			case UIBasicSprite.Flip.Both:
				return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMax, this.mOuterUV.xMin, this.mOuterUV.yMin);
			default:
				return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMin, this.mOuterUV.xMax, this.mOuterUV.yMax);
			}
		}
	}

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x06003343 RID: 13123 RVA: 0x001044DC File Offset: 0x001028DC
	private Color32 drawingColor
	{
		get
		{
			Color c = base.color;
			c.a = this.finalAlpha;
			if (this.premultipliedAlpha)
			{
				c = NGUITools.ApplyPMA(c);
			}
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				c.r = Mathf.Pow(c.r, 2.2f);
				c.g = Mathf.Pow(c.g, 2.2f);
				c.b = Mathf.Pow(c.b, 2.2f);
			}
			return c;
		}
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x00104568 File Offset: 0x00102968
	protected void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, Rect outer, Rect inner)
	{
		this.mOuterUV = outer;
		this.mInnerUV = inner;
		switch (this.type)
		{
		case UIBasicSprite.Type.Simple:
			this.SimpleFill(verts, uvs, cols);
			break;
		case UIBasicSprite.Type.Sliced:
			this.SlicedFill(verts, uvs, cols);
			break;
		case UIBasicSprite.Type.Tiled:
			this.TiledFill(verts, uvs, cols);
			break;
		case UIBasicSprite.Type.Filled:
			this.FilledFill(verts, uvs, cols);
			break;
		case UIBasicSprite.Type.Advanced:
			this.AdvancedFill(verts, uvs, cols);
			break;
		}
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x001045F4 File Offset: 0x001029F4
	private void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 drawingUVs = this.drawingUVs;
		Color32 drawingColor = this.drawingColor;
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
		uvs.Add(new Vector2(drawingUVs.x, drawingUVs.y));
		uvs.Add(new Vector2(drawingUVs.x, drawingUVs.w));
		uvs.Add(new Vector2(drawingUVs.z, drawingUVs.w));
		uvs.Add(new Vector2(drawingUVs.z, drawingUVs.y));
		cols.Add(drawingColor);
		cols.Add(drawingColor);
		cols.Add(drawingColor);
		cols.Add(drawingColor);
	}

	// Token: 0x06003346 RID: 13126 RVA: 0x001046FC File Offset: 0x00102AFC
	private void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 vector = this.border * this.pixelSize;
		if (vector.x == 0f && vector.y == 0f && vector.z == 0f && vector.w == 0f)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Color32 drawingColor = this.drawingColor;
		Vector4 drawingDimensions = this.drawingDimensions;
		UIBasicSprite.mTempPos[0].x = drawingDimensions.x;
		UIBasicSprite.mTempPos[0].y = drawingDimensions.y;
		UIBasicSprite.mTempPos[3].x = drawingDimensions.z;
		UIBasicSprite.mTempPos[3].y = drawingDimensions.w;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.z;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.x;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.x;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.z;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.w;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.y;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.y;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.w;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UIBasicSprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num2 = j + 1;
					verts.Add(new Vector3(UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[j].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num2].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[num2].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[j].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num2].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[num2].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y));
					cols.Add(drawingColor);
					cols.Add(drawingColor);
					cols.Add(drawingColor);
					cols.Add(drawingColor);
				}
			}
		}
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x00104CD0 File Offset: 0x001030D0
	private void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector2 a = new Vector2(this.mInnerUV.width * (float)mainTexture.width, this.mInnerUV.height * (float)mainTexture.height);
		a *= this.pixelSize;
		if (mainTexture == null || a.x < 2f || a.y < 2f)
		{
			return;
		}
		Color32 drawingColor = this.drawingColor;
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 vector;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			vector.x = this.mInnerUV.xMax;
			vector.z = this.mInnerUV.xMin;
		}
		else
		{
			vector.x = this.mInnerUV.xMin;
			vector.z = this.mInnerUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			vector.y = this.mInnerUV.yMax;
			vector.w = this.mInnerUV.yMin;
		}
		else
		{
			vector.y = this.mInnerUV.yMin;
			vector.w = this.mInnerUV.yMax;
		}
		float num = drawingDimensions.x;
		float num2 = drawingDimensions.y;
		float x = vector.x;
		float y = vector.y;
		while (num2 < drawingDimensions.w)
		{
			num = drawingDimensions.x;
			float num3 = num2 + a.y;
			float y2 = vector.w;
			if (num3 > drawingDimensions.w)
			{
				y2 = Mathf.Lerp(vector.y, vector.w, (drawingDimensions.w - num2) / a.y);
				num3 = drawingDimensions.w;
			}
			while (num < drawingDimensions.z)
			{
				float num4 = num + a.x;
				float x2 = vector.z;
				if (num4 > drawingDimensions.z)
				{
					x2 = Mathf.Lerp(vector.x, vector.z, (drawingDimensions.z - num) / a.x);
					num4 = drawingDimensions.z;
				}
				verts.Add(new Vector3(num, num2));
				verts.Add(new Vector3(num, num3));
				verts.Add(new Vector3(num4, num3));
				verts.Add(new Vector3(num4, num2));
				uvs.Add(new Vector2(x, y));
				uvs.Add(new Vector2(x, y2));
				uvs.Add(new Vector2(x2, y2));
				uvs.Add(new Vector2(x2, y));
				cols.Add(drawingColor);
				cols.Add(drawingColor);
				cols.Add(drawingColor);
				cols.Add(drawingColor);
				num += a.x;
			}
			num2 += a.y;
		}
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x00104FD8 File Offset: 0x001033D8
	private void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mFillAmount < 0.001f)
		{
			return;
		}
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 drawingUVs = this.drawingUVs;
		Color32 drawingColor = this.drawingColor;
		if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal || this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
		{
			if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal)
			{
				float num = (drawingUVs.z - drawingUVs.x) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					drawingUVs.x = drawingUVs.z - num;
				}
				else
				{
					drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					drawingUVs.z = drawingUVs.x + num;
				}
			}
			else if (this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
			{
				float num2 = (drawingUVs.w - drawingUVs.y) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					drawingUVs.y = drawingUVs.w - num2;
				}
				else
				{
					drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					drawingUVs.w = drawingUVs.y + num2;
				}
			}
		}
		UIBasicSprite.mTempPos[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
		UIBasicSprite.mTempPos[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
		UIBasicSprite.mTempPos[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
		UIBasicSprite.mTempPos[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
		UIBasicSprite.mTempUVs[0] = new Vector2(drawingUVs.x, drawingUVs.y);
		UIBasicSprite.mTempUVs[1] = new Vector2(drawingUVs.x, drawingUVs.w);
		UIBasicSprite.mTempUVs[2] = new Vector2(drawingUVs.z, drawingUVs.w);
		UIBasicSprite.mTempUVs[3] = new Vector2(drawingUVs.z, drawingUVs.y);
		if (this.mFillAmount < 1f)
		{
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial90)
			{
				if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, this.mFillAmount, this.mInvert, 0))
				{
					for (int i = 0; i < 4; i++)
					{
						verts.Add(UIBasicSprite.mTempPos[i]);
						uvs.Add(UIBasicSprite.mTempUVs[i]);
						cols.Add(drawingColor);
					}
				}
				return;
			}
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial180)
			{
				for (int j = 0; j < 2; j++)
				{
					float t = 0f;
					float t2 = 1f;
					float t3;
					float t4;
					if (j == 0)
					{
						t3 = 0f;
						t4 = 0.5f;
					}
					else
					{
						t3 = 0.5f;
						t4 = 1f;
					}
					UIBasicSprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t3);
					UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
					UIBasicSprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t4);
					UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
					UIBasicSprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t);
					UIBasicSprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t2);
					UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
					UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
					UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t3);
					UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
					UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t4);
					UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
					UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t);
					UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t2);
					UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
					UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
					float value = this.mInvert ? (this.mFillAmount * 2f - (float)(1 - j)) : (this.fillAmount * 2f - (float)j);
					if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(value), !this.mInvert, NGUIMath.RepeatIndex(j + 3, 4)))
					{
						for (int k = 0; k < 4; k++)
						{
							verts.Add(UIBasicSprite.mTempPos[k]);
							uvs.Add(UIBasicSprite.mTempUVs[k]);
							cols.Add(drawingColor);
						}
					}
				}
				return;
			}
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial360)
			{
				for (int l = 0; l < 4; l++)
				{
					float t5;
					float t6;
					if (l < 2)
					{
						t5 = 0f;
						t6 = 0.5f;
					}
					else
					{
						t5 = 0.5f;
						t6 = 1f;
					}
					float t7;
					float t8;
					if (l == 0 || l == 3)
					{
						t7 = 0f;
						t8 = 0.5f;
					}
					else
					{
						t7 = 0.5f;
						t8 = 1f;
					}
					UIBasicSprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t5);
					UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
					UIBasicSprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t6);
					UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
					UIBasicSprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t7);
					UIBasicSprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t8);
					UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
					UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
					UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t5);
					UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
					UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t6);
					UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
					UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t7);
					UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t8);
					UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
					UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
					float value2 = (!this.mInvert) ? (this.mFillAmount * 4f - (float)(3 - NGUIMath.RepeatIndex(l + 2, 4))) : (this.mFillAmount * 4f - (float)NGUIMath.RepeatIndex(l + 2, 4));
					if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(value2), this.mInvert, NGUIMath.RepeatIndex(l + 2, 4)))
					{
						for (int m = 0; m < 4; m++)
						{
							verts.Add(UIBasicSprite.mTempPos[m]);
							uvs.Add(UIBasicSprite.mTempUVs[m]);
							cols.Add(drawingColor);
						}
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add(UIBasicSprite.mTempPos[n]);
			uvs.Add(UIBasicSprite.mTempUVs[n]);
			cols.Add(drawingColor);
		}
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x001059F0 File Offset: 0x00103DF0
	private void AdvancedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector4 vector = this.border * this.pixelSize;
		if (vector.x == 0f && vector.y == 0f && vector.z == 0f && vector.w == 0f)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Color32 drawingColor = this.drawingColor;
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector2 a = new Vector2(this.mInnerUV.width * (float)mainTexture.width, this.mInnerUV.height * (float)mainTexture.height);
		a *= this.pixelSize;
		if (a.x < 1f)
		{
			a.x = 1f;
		}
		if (a.y < 1f)
		{
			a.y = 1f;
		}
		UIBasicSprite.mTempPos[0].x = drawingDimensions.x;
		UIBasicSprite.mTempPos[0].y = drawingDimensions.y;
		UIBasicSprite.mTempPos[3].x = drawingDimensions.z;
		UIBasicSprite.mTempPos[3].y = drawingDimensions.w;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.z;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.x;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.x;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.z;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.w;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.y;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.y;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.w;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UIBasicSprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num2 = j + 1;
					if (i == 1 && j == 1)
					{
						if (this.centerType == UIBasicSprite.AdvancedType.Tiled)
						{
							float x = UIBasicSprite.mTempPos[i].x;
							float x2 = UIBasicSprite.mTempPos[num].x;
							float y = UIBasicSprite.mTempPos[j].y;
							float y2 = UIBasicSprite.mTempPos[num2].y;
							float x3 = UIBasicSprite.mTempUVs[i].x;
							float y3 = UIBasicSprite.mTempUVs[j].y;
							for (float num3 = y; num3 < y2; num3 += a.y)
							{
								float num4 = x;
								float num5 = UIBasicSprite.mTempUVs[num2].y;
								float num6 = num3 + a.y;
								if (num6 > y2)
								{
									num5 = Mathf.Lerp(y3, num5, (y2 - num3) / a.y);
									num6 = y2;
								}
								while (num4 < x2)
								{
									float num7 = num4 + a.x;
									float num8 = UIBasicSprite.mTempUVs[num].x;
									if (num7 > x2)
									{
										num8 = Mathf.Lerp(x3, num8, (x2 - num4) / a.x);
										num7 = x2;
									}
									UIBasicSprite.Fill(verts, uvs, cols, num4, num7, num3, num6, x3, num8, y3, num5, drawingColor);
									num4 += a.x;
								}
							}
						}
						else if (this.centerType == UIBasicSprite.AdvancedType.Sliced)
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
						}
					}
					else if (i == 1)
					{
						if ((j == 0 && this.bottomType == UIBasicSprite.AdvancedType.Tiled) || (j == 2 && this.topType == UIBasicSprite.AdvancedType.Tiled))
						{
							float x4 = UIBasicSprite.mTempPos[i].x;
							float x5 = UIBasicSprite.mTempPos[num].x;
							float y4 = UIBasicSprite.mTempPos[j].y;
							float y5 = UIBasicSprite.mTempPos[num2].y;
							float x6 = UIBasicSprite.mTempUVs[i].x;
							float y6 = UIBasicSprite.mTempUVs[j].y;
							float y7 = UIBasicSprite.mTempUVs[num2].y;
							for (float num9 = x4; num9 < x5; num9 += a.x)
							{
								float num10 = num9 + a.x;
								float num11 = UIBasicSprite.mTempUVs[num].x;
								if (num10 > x5)
								{
									num11 = Mathf.Lerp(x6, num11, (x5 - num9) / a.x);
									num10 = x5;
								}
								UIBasicSprite.Fill(verts, uvs, cols, num9, num10, y4, y5, x6, num11, y6, y7, drawingColor);
							}
						}
						else if ((j == 0 && this.bottomType == UIBasicSprite.AdvancedType.Sliced) || (j == 2 && this.topType == UIBasicSprite.AdvancedType.Sliced))
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
						}
					}
					else if (j == 1)
					{
						if ((i == 0 && this.leftType == UIBasicSprite.AdvancedType.Tiled) || (i == 2 && this.rightType == UIBasicSprite.AdvancedType.Tiled))
						{
							float x7 = UIBasicSprite.mTempPos[i].x;
							float x8 = UIBasicSprite.mTempPos[num].x;
							float y8 = UIBasicSprite.mTempPos[j].y;
							float y9 = UIBasicSprite.mTempPos[num2].y;
							float x9 = UIBasicSprite.mTempUVs[i].x;
							float x10 = UIBasicSprite.mTempUVs[num].x;
							float y10 = UIBasicSprite.mTempUVs[j].y;
							for (float num12 = y8; num12 < y9; num12 += a.y)
							{
								float num13 = UIBasicSprite.mTempUVs[num2].y;
								float num14 = num12 + a.y;
								if (num14 > y9)
								{
									num13 = Mathf.Lerp(y10, num13, (y9 - num12) / a.y);
									num14 = y9;
								}
								UIBasicSprite.Fill(verts, uvs, cols, x7, x8, num12, num14, x9, x10, y10, num13, drawingColor);
							}
						}
						else if ((i == 0 && this.leftType == UIBasicSprite.AdvancedType.Sliced) || (i == 2 && this.rightType == UIBasicSprite.AdvancedType.Sliced))
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
						}
					}
					else
					{
						UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
					}
				}
			}
		}
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x0010657C File Offset: 0x0010497C
	private static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if ((corner & 1) == 1)
		{
			invert = !invert;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float num = Mathf.Clamp01(fill);
		if (invert)
		{
			num = 1f - num;
		}
		num *= 1.57079637f;
		float cos = Mathf.Cos(num);
		float sin = Mathf.Sin(num);
		UIBasicSprite.RadialCut(xy, cos, sin, invert, corner);
		UIBasicSprite.RadialCut(uv, cos, sin, invert, corner);
		return true;
	}

	// Token: 0x0600334B RID: 13131 RVA: 0x001065FC File Offset: 0x001049FC
	private static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
	{
		int num = NGUIMath.RepeatIndex(corner + 1, 4);
		int num2 = NGUIMath.RepeatIndex(corner + 2, 4);
		int num3 = NGUIMath.RepeatIndex(corner + 3, 4);
		if ((corner & 1) == 1)
		{
			if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num2].x = xy[num].x;
				}
			}
			else if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num2].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num3].y = xy[num2].y;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (!invert)
			{
				xy[num3].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
			else
			{
				xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
		}
		else
		{
			if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num2].y = xy[num].y;
				}
			}
			else if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num2].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num3].x = xy[num2].x;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (invert)
			{
				xy[num3].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
			else
			{
				xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
		}
	}

	// Token: 0x0600334C RID: 13132 RVA: 0x00106898 File Offset: 0x00104C98
	private static void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, float v0x, float v1x, float v0y, float v1y, float u0x, float u1x, float u0y, float u1y, Color col)
	{
		verts.Add(new Vector3(v0x, v0y));
		verts.Add(new Vector3(v0x, v1y));
		verts.Add(new Vector3(v1x, v1y));
		verts.Add(new Vector3(v1x, v0y));
		uvs.Add(new Vector2(u0x, u0y));
		uvs.Add(new Vector2(u0x, u1y));
		uvs.Add(new Vector2(u1x, u1y));
		uvs.Add(new Vector2(u1x, u0y));
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
	}

	// Token: 0x04001D08 RID: 7432
	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite.Type mType;

	// Token: 0x04001D09 RID: 7433
	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite.FillDirection mFillDirection = UIBasicSprite.FillDirection.Radial360;

	// Token: 0x04001D0A RID: 7434
	[Range(0f, 1f)]
	[HideInInspector]
	[SerializeField]
	protected float mFillAmount = 1f;

	// Token: 0x04001D0B RID: 7435
	[HideInInspector]
	[SerializeField]
	protected bool mInvert;

	// Token: 0x04001D0C RID: 7436
	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite.Flip mFlip;

	// Token: 0x04001D0D RID: 7437
	[NonSerialized]
	private Rect mInnerUV = default(Rect);

	// Token: 0x04001D0E RID: 7438
	[NonSerialized]
	private Rect mOuterUV = default(Rect);

	// Token: 0x04001D0F RID: 7439
	public UIBasicSprite.AdvancedType centerType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x04001D10 RID: 7440
	public UIBasicSprite.AdvancedType leftType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x04001D11 RID: 7441
	public UIBasicSprite.AdvancedType rightType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x04001D12 RID: 7442
	public UIBasicSprite.AdvancedType bottomType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x04001D13 RID: 7443
	public UIBasicSprite.AdvancedType topType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x04001D14 RID: 7444
	protected static Vector2[] mTempPos = new Vector2[4];

	// Token: 0x04001D15 RID: 7445
	protected static Vector2[] mTempUVs = new Vector2[4];

	// Token: 0x020005FD RID: 1533
	public enum Type
	{
		// Token: 0x04001D17 RID: 7447
		Simple,
		// Token: 0x04001D18 RID: 7448
		Sliced,
		// Token: 0x04001D19 RID: 7449
		Tiled,
		// Token: 0x04001D1A RID: 7450
		Filled,
		// Token: 0x04001D1B RID: 7451
		Advanced
	}

	// Token: 0x020005FE RID: 1534
	public enum FillDirection
	{
		// Token: 0x04001D1D RID: 7453
		Horizontal,
		// Token: 0x04001D1E RID: 7454
		Vertical,
		// Token: 0x04001D1F RID: 7455
		Radial90,
		// Token: 0x04001D20 RID: 7456
		Radial180,
		// Token: 0x04001D21 RID: 7457
		Radial360
	}

	// Token: 0x020005FF RID: 1535
	public enum AdvancedType
	{
		// Token: 0x04001D23 RID: 7459
		Invisible,
		// Token: 0x04001D24 RID: 7460
		Sliced,
		// Token: 0x04001D25 RID: 7461
		Tiled
	}

	// Token: 0x02000600 RID: 1536
	public enum Flip
	{
		// Token: 0x04001D27 RID: 7463
		Nothing,
		// Token: 0x04001D28 RID: 7464
		Horizontally,
		// Token: 0x04001D29 RID: 7465
		Vertically,
		// Token: 0x04001D2A RID: 7466
		Both
	}
}
