using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000610 RID: 1552
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Widget")]
public class UIWidget : UIRect
{
	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x060033DA RID: 13274 RVA: 0x001021F3 File Offset: 0x001005F3
	// (set) Token: 0x060033DB RID: 13275 RVA: 0x001021FC File Offset: 0x001005FC
	public UIDrawCall.OnRenderCallback onRender
	{
		get
		{
			return this.mOnRender;
		}
		set
		{
			if (this.mOnRender != value)
			{
				if (this.drawCall != null && this.drawCall.onRender != null && this.mOnRender != null)
				{
					UIDrawCall uidrawCall = this.drawCall;
					uidrawCall.onRender = (UIDrawCall.OnRenderCallback)Delegate.Remove(uidrawCall.onRender, this.mOnRender);
				}
				this.mOnRender = value;
				if (this.drawCall != null)
				{
					UIDrawCall uidrawCall2 = this.drawCall;
					uidrawCall2.onRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(uidrawCall2.onRender, value);
				}
			}
		}
	}

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x060033DC RID: 13276 RVA: 0x0010229B File Offset: 0x0010069B
	// (set) Token: 0x060033DD RID: 13277 RVA: 0x001022A3 File Offset: 0x001006A3
	public Vector4 drawRegion
	{
		get
		{
			return this.mDrawRegion;
		}
		set
		{
			if (this.mDrawRegion != value)
			{
				this.mDrawRegion = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x060033DE RID: 13278 RVA: 0x001022D4 File Offset: 0x001006D4
	public Vector2 pivotOffset
	{
		get
		{
			return NGUIMath.GetPivotOffset(this.pivot);
		}
	}

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x060033DF RID: 13279 RVA: 0x001022E1 File Offset: 0x001006E1
	// (set) Token: 0x060033E0 RID: 13280 RVA: 0x001022EC File Offset: 0x001006EC
	public int width
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			int minWidth = this.minWidth;
			if (value < minWidth)
			{
				value = minWidth;
			}
			if (this.mWidth != value && this.keepAspectRatio != UIWidget.AspectRatioSource.BasedOnHeight)
			{
				if (this.isAnchoredHorizontally)
				{
					if (this.leftAnchor.target != null && this.rightAnchor.target != null)
					{
						if (this.mPivot == UIWidget.Pivot.BottomLeft || this.mPivot == UIWidget.Pivot.Left || this.mPivot == UIWidget.Pivot.TopLeft)
						{
							NGUIMath.AdjustWidget(this, 0f, 0f, (float)(value - this.mWidth), 0f);
						}
						else if (this.mPivot == UIWidget.Pivot.BottomRight || this.mPivot == UIWidget.Pivot.Right || this.mPivot == UIWidget.Pivot.TopRight)
						{
							NGUIMath.AdjustWidget(this, (float)(this.mWidth - value), 0f, 0f, 0f);
						}
						else
						{
							int num = value - this.mWidth;
							num -= (num & 1);
							if (num != 0)
							{
								NGUIMath.AdjustWidget(this, (float)(-(float)num) * 0.5f, 0f, (float)num * 0.5f, 0f);
							}
						}
					}
					else if (this.leftAnchor.target != null)
					{
						NGUIMath.AdjustWidget(this, 0f, 0f, (float)(value - this.mWidth), 0f);
					}
					else
					{
						NGUIMath.AdjustWidget(this, (float)(this.mWidth - value), 0f, 0f, 0f);
					}
				}
				else
				{
					this.SetDimensions(value, this.mHeight);
				}
			}
		}
	}

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x060033E1 RID: 13281 RVA: 0x0010248A File Offset: 0x0010088A
	// (set) Token: 0x060033E2 RID: 13282 RVA: 0x00102494 File Offset: 0x00100894
	public int height
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			int minHeight = this.minHeight;
			if (value < minHeight)
			{
				value = minHeight;
			}
			if (this.mHeight != value && this.keepAspectRatio != UIWidget.AspectRatioSource.BasedOnWidth)
			{
				if (this.isAnchoredVertically)
				{
					if (this.bottomAnchor.target != null && this.topAnchor.target != null)
					{
						if (this.mPivot == UIWidget.Pivot.BottomLeft || this.mPivot == UIWidget.Pivot.Bottom || this.mPivot == UIWidget.Pivot.BottomRight)
						{
							NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float)(value - this.mHeight));
						}
						else if (this.mPivot == UIWidget.Pivot.TopLeft || this.mPivot == UIWidget.Pivot.Top || this.mPivot == UIWidget.Pivot.TopRight)
						{
							NGUIMath.AdjustWidget(this, 0f, (float)(this.mHeight - value), 0f, 0f);
						}
						else
						{
							int num = value - this.mHeight;
							num -= (num & 1);
							if (num != 0)
							{
								NGUIMath.AdjustWidget(this, 0f, (float)(-(float)num) * 0.5f, 0f, (float)num * 0.5f);
							}
						}
					}
					else if (this.bottomAnchor.target != null)
					{
						NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float)(value - this.mHeight));
					}
					else
					{
						NGUIMath.AdjustWidget(this, 0f, (float)(this.mHeight - value), 0f, 0f);
					}
				}
				else
				{
					this.SetDimensions(this.mWidth, value);
				}
			}
		}
	}

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x060033E3 RID: 13283 RVA: 0x00102632 File Offset: 0x00100A32
	// (set) Token: 0x060033E4 RID: 13284 RVA: 0x0010263C File Offset: 0x00100A3C
	public Color color
	{
		get
		{
			return this.mColor;
		}
		set
		{
			if (this.mColor != value)
			{
				bool includeChildren = this.mColor.a != value.a;
				this.mColor = value;
				this.Invalidate(includeChildren);
			}
		}
	}

	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x060033E5 RID: 13285 RVA: 0x00102680 File Offset: 0x00100A80
	// (set) Token: 0x060033E6 RID: 13286 RVA: 0x0010268D File Offset: 0x00100A8D
	public override float alpha
	{
		get
		{
			return this.mColor.a;
		}
		set
		{
			if (this.mColor.a != value)
			{
				this.mColor.a = value;
				this.Invalidate(true);
			}
		}
	}

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x060033E7 RID: 13287 RVA: 0x001026B3 File Offset: 0x00100AB3
	public bool isVisible
	{
		get
		{
			return this.mIsVisibleByPanel && this.mIsVisibleByAlpha && this.mIsInFront && this.finalAlpha > 0.001f && NGUITools.GetActive(this);
		}
	}

	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x060033E8 RID: 13288 RVA: 0x001026EF File Offset: 0x00100AEF
	public bool hasVertices
	{
		get
		{
			return this.geometry != null && this.geometry.hasVertices;
		}
	}

	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x060033E9 RID: 13289 RVA: 0x0010270A File Offset: 0x00100B0A
	// (set) Token: 0x060033EA RID: 13290 RVA: 0x00102712 File Offset: 0x00100B12
	public UIWidget.Pivot rawPivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				this.mPivot = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x060033EB RID: 13291 RVA: 0x0010273E File Offset: 0x00100B3E
	// (set) Token: 0x060033EC RID: 13292 RVA: 0x00102748 File Offset: 0x00100B48
	public UIWidget.Pivot pivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				Vector3 vector = this.worldCorners[0];
				this.mPivot = value;
				this.mChanged = true;
				Vector3 vector2 = this.worldCorners[0];
				Transform cachedTransform = base.cachedTransform;
				Vector3 vector3 = cachedTransform.position;
				float z = cachedTransform.localPosition.z;
				vector3.x += vector.x - vector2.x;
				vector3.y += vector.y - vector2.y;
				base.cachedTransform.position = vector3;
				vector3 = base.cachedTransform.localPosition;
				vector3.x = Mathf.Round(vector3.x);
				vector3.y = Mathf.Round(vector3.y);
				vector3.z = z;
				base.cachedTransform.localPosition = vector3;
			}
		}
	}

	// Token: 0x170007D3 RID: 2003
	// (get) Token: 0x060033ED RID: 13293 RVA: 0x0010283F File Offset: 0x00100C3F
	// (set) Token: 0x060033EE RID: 13294 RVA: 0x00102848 File Offset: 0x00100C48
	public int depth
	{
		get
		{
			return this.mDepth;
		}
		set
		{
			if (this.mDepth != value)
			{
				if (this.panel != null)
				{
					this.panel.RemoveWidget(this);
				}
				this.mDepth = value;
				if (this.panel != null)
				{
					this.panel.AddWidget(this);
					if (!Application.isPlaying)
					{
						this.panel.SortWidgets();
						this.panel.RebuildAllDrawCalls();
					}
				}
			}
		}
	}

	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x060033EF RID: 13295 RVA: 0x001028C4 File Offset: 0x00100CC4
	public int raycastDepth
	{
		get
		{
			if (this.panel == null)
			{
				this.CreatePanel();
			}
			return (!(this.panel != null)) ? this.mDepth : (this.mDepth + this.panel.depth * 1000);
		}
	}

	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x060033F0 RID: 13296 RVA: 0x00102920 File Offset: 0x00100D20
	public override Vector3[] localCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float x = num + (float)this.mWidth;
			float y = num2 + (float)this.mHeight;
			this.mCorners[0] = new Vector3(num, num2);
			this.mCorners[1] = new Vector3(num, y);
			this.mCorners[2] = new Vector3(x, y);
			this.mCorners[3] = new Vector3(x, num2);
			return this.mCorners;
		}
	}

	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x060033F1 RID: 13297 RVA: 0x001029D4 File Offset: 0x00100DD4
	public virtual Vector2 localSize
	{
		get
		{
			Vector3[] localCorners = this.localCorners;
			return localCorners[2] - localCorners[0];
		}
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x060033F2 RID: 13298 RVA: 0x00102A0C File Offset: 0x00100E0C
	public Vector3 localCenter
	{
		get
		{
			Vector3[] localCorners = this.localCorners;
			return Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
		}
	}

	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x060033F3 RID: 13299 RVA: 0x00102A44 File Offset: 0x00100E44
	public override Vector3[] worldCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float x = num + (float)this.mWidth;
			float y = num2 + (float)this.mHeight;
			Transform cachedTransform = base.cachedTransform;
			this.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
			this.mCorners[1] = cachedTransform.TransformPoint(num, y, 0f);
			this.mCorners[2] = cachedTransform.TransformPoint(x, y, 0f);
			this.mCorners[3] = cachedTransform.TransformPoint(x, num2, 0f);
			return this.mCorners;
		}
	}

	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x060033F4 RID: 13300 RVA: 0x00102B1B File Offset: 0x00100F1B
	public Vector3 worldCenter
	{
		get
		{
			return base.cachedTransform.TransformPoint(this.localCenter);
		}
	}

	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x060033F5 RID: 13301 RVA: 0x00102B30 File Offset: 0x00100F30
	public virtual Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			return new Vector4((this.mDrawRegion.x != 0f) ? Mathf.Lerp(num, num3, this.mDrawRegion.x) : num, (this.mDrawRegion.y != 0f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.y) : num2, (this.mDrawRegion.z != 1f) ? Mathf.Lerp(num, num3, this.mDrawRegion.z) : num3, (this.mDrawRegion.w != 1f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.w) : num4);
		}
	}

	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x060033F6 RID: 13302 RVA: 0x00102C37 File Offset: 0x00101037
	// (set) Token: 0x060033F7 RID: 13303 RVA: 0x00102C3A File Offset: 0x0010103A
	public virtual Material material
	{
		get
		{
			return null;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no material setter");
		}
	}

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x060033F8 RID: 13304 RVA: 0x00102C54 File Offset: 0x00101054
	// (set) Token: 0x060033F9 RID: 13305 RVA: 0x00102C80 File Offset: 0x00101080
	public virtual Texture mainTexture
	{
		get
		{
			Material material = this.material;
			return (!(material != null)) ? null : material.mainTexture;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no mainTexture setter");
		}
	}

	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x060033FA RID: 13306 RVA: 0x00102C98 File Offset: 0x00101098
	// (set) Token: 0x060033FB RID: 13307 RVA: 0x00102CC4 File Offset: 0x001010C4
	public virtual Shader shader
	{
		get
		{
			Material material = this.material;
			return (!(material != null)) ? null : material.shader;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no shader setter");
		}
	}

	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x060033FC RID: 13308 RVA: 0x00102CDB File Offset: 0x001010DB
	[Obsolete("There is no relative scale anymore. Widgets now have width and height instead")]
	public Vector2 relativeSize
	{
		get
		{
			return Vector2.one;
		}
	}

	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x060033FD RID: 13309 RVA: 0x00102CE4 File Offset: 0x001010E4
	public bool hasBoxCollider
	{
		get
		{
			BoxCollider x = base.GetComponent<Collider>() as BoxCollider;
			return x != null || base.GetComponent<BoxCollider2D>() != null;
		}
	}

	// Token: 0x060033FE RID: 13310 RVA: 0x00102D18 File Offset: 0x00101118
	public void SetDimensions(int w, int h)
	{
		if (this.mWidth != w || this.mHeight != h)
		{
			this.mWidth = w;
			this.mHeight = h;
			if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnWidth)
			{
				this.mHeight = Mathf.RoundToInt((float)this.mWidth / this.aspectRatio);
			}
			else if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnHeight)
			{
				this.mWidth = Mathf.RoundToInt((float)this.mHeight * this.aspectRatio);
			}
			else if (this.keepAspectRatio == UIWidget.AspectRatioSource.Free)
			{
				this.aspectRatio = (float)this.mWidth / (float)this.mHeight;
			}
			this.mMoved = true;
			if (this.autoResizeBoxCollider)
			{
				this.ResizeCollider();
			}
			this.MarkAsChanged();
		}
	}

	// Token: 0x060033FF RID: 13311 RVA: 0x00102DE0 File Offset: 0x001011E0
	public override Vector3[] GetSides(Transform relativeTo)
	{
		Vector2 pivotOffset = this.pivotOffset;
		float num = -pivotOffset.x * (float)this.mWidth;
		float num2 = -pivotOffset.y * (float)this.mHeight;
		float num3 = num + (float)this.mWidth;
		float num4 = num2 + (float)this.mHeight;
		float x = (num + num3) * 0.5f;
		float y = (num2 + num4) * 0.5f;
		Transform cachedTransform = base.cachedTransform;
		this.mCorners[0] = cachedTransform.TransformPoint(num, y, 0f);
		this.mCorners[1] = cachedTransform.TransformPoint(x, num4, 0f);
		this.mCorners[2] = cachedTransform.TransformPoint(num3, y, 0f);
		this.mCorners[3] = cachedTransform.TransformPoint(x, num2, 0f);
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				this.mCorners[i] = relativeTo.InverseTransformPoint(this.mCorners[i]);
			}
		}
		return this.mCorners;
	}

	// Token: 0x06003400 RID: 13312 RVA: 0x00102F1D File Offset: 0x0010131D
	public override float CalculateFinalAlpha(int frameID)
	{
		if (this.mAlphaFrameID != frameID)
		{
			this.mAlphaFrameID = frameID;
			this.UpdateFinalAlpha(frameID);
		}
		return this.finalAlpha;
	}

	// Token: 0x06003401 RID: 13313 RVA: 0x00102F40 File Offset: 0x00101340
	protected void UpdateFinalAlpha(int frameID)
	{
		if (!this.mIsVisibleByAlpha || !this.mIsInFront)
		{
			this.finalAlpha = 0f;
		}
		else
		{
			UIRect parent = base.parent;
			this.finalAlpha = ((!(base.parent != null)) ? this.mColor.a : (parent.CalculateFinalAlpha(frameID) * this.mColor.a));
		}
	}

	// Token: 0x06003402 RID: 13314 RVA: 0x00102FB4 File Offset: 0x001013B4
	public override void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		this.mAlphaFrameID = -1;
		if (this.panel != null)
		{
			bool visibleByPanel = (!this.hideIfOffScreen && !this.panel.hasCumulativeClipping) || this.panel.IsVisible(this);
			this.UpdateVisibility(this.CalculateCumulativeAlpha(Time.frameCount) > 0.001f, visibleByPanel);
			this.UpdateFinalAlpha(Time.frameCount);
			if (includeChildren)
			{
				base.Invalidate(true);
			}
		}
	}

	// Token: 0x06003403 RID: 13315 RVA: 0x00103040 File Offset: 0x00101440
	public float CalculateCumulativeAlpha(int frameID)
	{
		UIRect parent = base.parent;
		return (!(parent != null)) ? this.mColor.a : (parent.CalculateFinalAlpha(frameID) * this.mColor.a);
	}

	// Token: 0x06003404 RID: 13316 RVA: 0x00103084 File Offset: 0x00101484
	public override void SetRect(float x, float y, float width, float height)
	{
		Vector2 pivotOffset = this.pivotOffset;
		float num = Mathf.Lerp(x, x + width, pivotOffset.x);
		float num2 = Mathf.Lerp(y, y + height, pivotOffset.y);
		int num3 = Mathf.FloorToInt(width + 0.5f);
		int num4 = Mathf.FloorToInt(height + 0.5f);
		if (pivotOffset.x == 0.5f)
		{
			num3 = num3 >> 1 << 1;
		}
		if (pivotOffset.y == 0.5f)
		{
			num4 = num4 >> 1 << 1;
		}
		Transform transform = base.cachedTransform;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = Mathf.Floor(num + 0.5f);
		localPosition.y = Mathf.Floor(num2 + 0.5f);
		if (num3 < this.minWidth)
		{
			num3 = this.minWidth;
		}
		if (num4 < this.minHeight)
		{
			num4 = this.minHeight;
		}
		transform.localPosition = localPosition;
		this.width = num3;
		this.height = num4;
		if (base.isAnchored)
		{
			transform = transform.parent;
			if (this.leftAnchor.target)
			{
				this.leftAnchor.SetHorizontal(transform, x);
			}
			if (this.rightAnchor.target)
			{
				this.rightAnchor.SetHorizontal(transform, x + width);
			}
			if (this.bottomAnchor.target)
			{
				this.bottomAnchor.SetVertical(transform, y);
			}
			if (this.topAnchor.target)
			{
				this.topAnchor.SetVertical(transform, y + height);
			}
		}
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x00103223 File Offset: 0x00101623
	public void ResizeCollider()
	{
		if (NGUITools.GetActive(this))
		{
			NGUITools.UpdateWidgetCollider(base.gameObject);
		}
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x0010323C File Offset: 0x0010163C
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int FullCompareFunc(UIWidget left, UIWidget right)
	{
		int num = UIPanel.CompareFunc(left.panel, right.panel);
		return (num != 0) ? num : UIWidget.PanelCompareFunc(left, right);
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x00103270 File Offset: 0x00101670
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int PanelCompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		Material material = left.material;
		Material material2 = right.material;
		if (material == material2)
		{
			return 0;
		}
		if (material != null)
		{
			return -1;
		}
		if (material2 != null)
		{
			return 1;
		}
		return (material.GetInstanceID() >= material2.GetInstanceID()) ? 1 : -1;
	}

	// Token: 0x06003408 RID: 13320 RVA: 0x001032F3 File Offset: 0x001016F3
	public Bounds CalculateBounds()
	{
		return this.CalculateBounds(null);
	}

	// Token: 0x06003409 RID: 13321 RVA: 0x001032FC File Offset: 0x001016FC
	public Bounds CalculateBounds(Transform relativeParent)
	{
		if (relativeParent == null)
		{
			Vector3[] localCorners = this.localCorners;
			Bounds result = new Bounds(localCorners[0], Vector3.zero);
			for (int i = 1; i < 4; i++)
			{
				result.Encapsulate(localCorners[i]);
			}
			return result;
		}
		Matrix4x4 worldToLocalMatrix = relativeParent.worldToLocalMatrix;
		Vector3[] worldCorners = this.worldCorners;
		Bounds result2 = new Bounds(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[0]), Vector3.zero);
		for (int j = 1; j < 4; j++)
		{
			result2.Encapsulate(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[j]));
		}
		return result2;
	}

	// Token: 0x0600340A RID: 13322 RVA: 0x001033C0 File Offset: 0x001017C0
	public void SetDirty()
	{
		if (this.drawCall != null)
		{
			this.drawCall.isDirty = true;
		}
		else if (this.isVisible && this.hasVertices)
		{
			this.CreatePanel();
		}
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x0010340C File Offset: 0x0010180C
	public void RemoveFromPanel()
	{
		if (this.panel != null)
		{
			this.panel.RemoveWidget(this);
			this.panel = null;
		}
		this.drawCall = null;
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x0010343C File Offset: 0x0010183C
	public virtual void MarkAsChanged()
	{
		if (NGUITools.GetActive(this))
		{
			this.mChanged = true;
			if (this.panel != null && base.enabled && NGUITools.GetActive(base.gameObject) && !this.mPlayMode)
			{
				this.SetDirty();
				this.CheckLayer();
			}
		}
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x001034A0 File Offset: 0x001018A0
	public UIPanel CreatePanel()
	{
		if (this.mStarted && this.panel == null && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.panel = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
			if (this.panel != null)
			{
				this.mParentFound = false;
				this.panel.AddWidget(this);
				this.CheckLayer();
				this.Invalidate(true);
			}
		}
		return this.panel;
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x00103538 File Offset: 0x00101938
	public void CheckLayer()
	{
		if (this.panel != null && this.panel.gameObject.layer != base.gameObject.layer)
		{
			UnityEngine.Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = this.panel.gameObject.layer;
		}
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x0010359C File Offset: 0x0010199C
	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		if (this.panel != null)
		{
			UIPanel y = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
			if (this.panel != y)
			{
				this.RemoveFromPanel();
				this.CreatePanel();
			}
		}
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x001035F6 File Offset: 0x001019F6
	protected virtual void Awake()
	{
		this.mGo = base.gameObject;
		this.mPlayMode = Application.isPlaying;
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x00103610 File Offset: 0x00101A10
	protected override void OnInit()
	{
		base.OnInit();
		this.RemoveFromPanel();
		this.mMoved = true;
		if (this.mWidth == 100 && this.mHeight == 100 && base.cachedTransform.localScale.magnitude > 8f)
		{
			this.UpgradeFrom265();
			base.cachedTransform.localScale = Vector3.one;
		}
		base.Update();
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x00103684 File Offset: 0x00101A84
	protected virtual void UpgradeFrom265()
	{
		Vector3 localScale = base.cachedTransform.localScale;
		this.mWidth = Mathf.Abs(Mathf.RoundToInt(localScale.x));
		this.mHeight = Mathf.Abs(Mathf.RoundToInt(localScale.y));
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x001036D7 File Offset: 0x00101AD7
	protected override void OnStart()
	{
		this.CreatePanel();
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x001036E0 File Offset: 0x00101AE0
	protected override void OnAnchor()
	{
		Transform cachedTransform = base.cachedTransform;
		Transform parent = cachedTransform.parent;
		Vector3 localPosition = cachedTransform.localPosition;
		Vector2 pivotOffset = this.pivotOffset;
		float num;
		float num2;
		float num3;
		float num4;
		if (this.leftAnchor.target == this.bottomAnchor.target && this.leftAnchor.target == this.rightAnchor.target && this.leftAnchor.target == this.topAnchor.target)
		{
			Vector3[] sides = this.leftAnchor.GetSides(parent);
			if (sides != null)
			{
				num = NGUIMath.Lerp(sides[0].x, sides[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				num2 = NGUIMath.Lerp(sides[0].x, sides[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				num3 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				num4 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
				this.mIsInFront = true;
			}
			else
			{
				Vector3 localPos = base.GetLocalPos(this.leftAnchor, parent);
				num = localPos.x + (float)this.leftAnchor.absolute;
				num3 = localPos.y + (float)this.bottomAnchor.absolute;
				num2 = localPos.x + (float)this.rightAnchor.absolute;
				num4 = localPos.y + (float)this.topAnchor.absolute;
				this.mIsInFront = (!this.hideIfOffScreen || localPos.z >= 0f);
			}
		}
		else
		{
			this.mIsInFront = true;
			if (this.leftAnchor.target)
			{
				Vector3[] sides2 = this.leftAnchor.GetSides(parent);
				if (sides2 != null)
				{
					num = NGUIMath.Lerp(sides2[0].x, sides2[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				}
				else
				{
					num = base.GetLocalPos(this.leftAnchor, parent).x + (float)this.leftAnchor.absolute;
				}
			}
			else
			{
				num = localPosition.x - pivotOffset.x * (float)this.mWidth;
			}
			if (this.rightAnchor.target)
			{
				Vector3[] sides3 = this.rightAnchor.GetSides(parent);
				if (sides3 != null)
				{
					num2 = NGUIMath.Lerp(sides3[0].x, sides3[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				}
				else
				{
					num2 = base.GetLocalPos(this.rightAnchor, parent).x + (float)this.rightAnchor.absolute;
				}
			}
			else
			{
				num2 = localPosition.x - pivotOffset.x * (float)this.mWidth + (float)this.mWidth;
			}
			if (this.bottomAnchor.target)
			{
				Vector3[] sides4 = this.bottomAnchor.GetSides(parent);
				if (sides4 != null)
				{
					num3 = NGUIMath.Lerp(sides4[3].y, sides4[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				}
				else
				{
					num3 = base.GetLocalPos(this.bottomAnchor, parent).y + (float)this.bottomAnchor.absolute;
				}
			}
			else
			{
				num3 = localPosition.y - pivotOffset.y * (float)this.mHeight;
			}
			if (this.topAnchor.target)
			{
				Vector3[] sides5 = this.topAnchor.GetSides(parent);
				if (sides5 != null)
				{
					num4 = NGUIMath.Lerp(sides5[3].y, sides5[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
				}
				else
				{
					num4 = base.GetLocalPos(this.topAnchor, parent).y + (float)this.topAnchor.absolute;
				}
			}
			else
			{
				num4 = localPosition.y - pivotOffset.y * (float)this.mHeight + (float)this.mHeight;
			}
		}
		Vector3 vector = new Vector3(Mathf.Lerp(num, num2, pivotOffset.x), Mathf.Lerp(num3, num4, pivotOffset.y), localPosition.z);
		int num5 = Mathf.FloorToInt(num2 - num + 0.5f);
		int num6 = Mathf.FloorToInt(num4 - num3 + 0.5f);
		if (this.keepAspectRatio != UIWidget.AspectRatioSource.Free && this.aspectRatio != 0f)
		{
			if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnHeight)
			{
				num5 = Mathf.RoundToInt((float)num6 * this.aspectRatio);
			}
			else
			{
				num6 = Mathf.RoundToInt((float)num5 / this.aspectRatio);
			}
		}
		if (num5 < this.minWidth)
		{
			num5 = this.minWidth;
		}
		if (num6 < this.minHeight)
		{
			num6 = this.minHeight;
		}
		if (Vector3.SqrMagnitude(localPosition - vector) > 0.001f)
		{
			base.cachedTransform.localPosition = vector;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
		}
		if (this.mWidth != num5 || this.mHeight != num6)
		{
			this.mWidth = num5;
			this.mHeight = num6;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
			if (this.autoResizeBoxCollider)
			{
				this.ResizeCollider();
			}
		}
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x00103D04 File Offset: 0x00102104
	protected override void OnUpdate()
	{
		if (this.panel == null)
		{
			this.CreatePanel();
		}
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x00103D1E File Offset: 0x0010211E
	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			this.MarkAsChanged();
		}
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x00103D2C File Offset: 0x0010212C
	protected override void OnDisable()
	{
		this.RemoveFromPanel();
		base.OnDisable();
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x00103D3A File Offset: 0x0010213A
	private void OnDestroy()
	{
		this.RemoveFromPanel();
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x00103D42 File Offset: 0x00102142
	public bool UpdateVisibility(bool visibleByAlpha, bool visibleByPanel)
	{
		if (this.mIsVisibleByAlpha != visibleByAlpha || this.mIsVisibleByPanel != visibleByPanel)
		{
			this.mChanged = true;
			this.mIsVisibleByAlpha = visibleByAlpha;
			this.mIsVisibleByPanel = visibleByPanel;
			return true;
		}
		return false;
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x00103D74 File Offset: 0x00102174
	public bool UpdateTransform(int frame)
	{
		if (!this.mMoved && !this.panel.widgetsAreStatic && base.cachedTransform.hasChanged)
		{
			this.mTrans.hasChanged = false;
			this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
			this.mMatrixFrame = frame;
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float x = num + (float)this.mWidth;
			float y = num2 + (float)this.mHeight;
			Transform cachedTransform = base.cachedTransform;
			Vector3 vector = cachedTransform.TransformPoint(num, num2, 0f);
			Vector3 vector2 = cachedTransform.TransformPoint(x, y, 0f);
			vector = this.panel.worldToLocal.MultiplyPoint3x4(vector);
			vector2 = this.panel.worldToLocal.MultiplyPoint3x4(vector2);
			if (Vector3.SqrMagnitude(this.mOldV0 - vector) > 1E-06f || Vector3.SqrMagnitude(this.mOldV1 - vector2) > 1E-06f)
			{
				this.mMoved = true;
				this.mOldV0 = vector;
				this.mOldV1 = vector2;
			}
		}
		if (this.mMoved && this.onChange != null)
		{
			this.onChange();
		}
		return this.mMoved || this.mChanged;
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x00103EF4 File Offset: 0x001022F4
	public bool UpdateGeometry(int frame)
	{
		float num = this.CalculateFinalAlpha(frame);
		if (this.mIsVisibleByAlpha && this.mLastAlpha != num)
		{
			this.mChanged = true;
		}
		this.mLastAlpha = num;
		if (this.mChanged)
		{
			this.mChanged = false;
			if (this.mIsVisibleByAlpha && num > 0.001f && this.shader != null)
			{
				bool hasVertices = this.geometry.hasVertices;
				if (this.fillGeometry)
				{
					this.geometry.Clear();
					this.OnFill(this.geometry.verts, this.geometry.uvs, this.geometry.cols);
				}
				if (this.geometry.hasVertices)
				{
					if (this.mMatrixFrame != frame)
					{
						this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
						this.mMatrixFrame = frame;
					}
					this.geometry.ApplyTransform(this.mLocalToPanel);
					this.mMoved = false;
					return true;
				}
				return hasVertices;
			}
			else if (this.geometry.hasVertices)
			{
				if (this.fillGeometry)
				{
					this.geometry.Clear();
				}
				this.mMoved = false;
				return true;
			}
		}
		else if (this.mMoved && this.geometry.hasVertices)
		{
			if (this.mMatrixFrame != frame)
			{
				this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
				this.mMatrixFrame = frame;
			}
			this.geometry.ApplyTransform(this.mLocalToPanel);
			this.mMoved = false;
			return true;
		}
		this.mMoved = false;
		return false;
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x001040B5 File Offset: 0x001024B5
	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		this.geometry.WriteToBuffers(v, u, c, n, t);
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x001040CC File Offset: 0x001024CC
	public virtual void MakePixelPerfect()
	{
		Vector3 localPosition = base.cachedTransform.localPosition;
		localPosition.z = Mathf.Round(localPosition.z);
		localPosition.x = Mathf.Round(localPosition.x);
		localPosition.y = Mathf.Round(localPosition.y);
		base.cachedTransform.localPosition = localPosition;
		Vector3 localScale = base.cachedTransform.localScale;
		base.cachedTransform.localScale = new Vector3(Mathf.Sign(localScale.x), Mathf.Sign(localScale.y), 1f);
	}

	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x0600341E RID: 13342 RVA: 0x00104163 File Offset: 0x00102563
	public virtual int minWidth
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x0600341F RID: 13343 RVA: 0x00104166 File Offset: 0x00102566
	public virtual int minHeight
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x06003420 RID: 13344 RVA: 0x00104169 File Offset: 0x00102569
	// (set) Token: 0x06003421 RID: 13345 RVA: 0x00104170 File Offset: 0x00102570
	public virtual Vector4 border
	{
		get
		{
			return Vector4.zero;
		}
		set
		{
		}
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x00104172 File Offset: 0x00102572
	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
	}

	// Token: 0x04001D8C RID: 7564
	[HideInInspector]
	[SerializeField]
	protected Color mColor = Color.white;

	// Token: 0x04001D8D RID: 7565
	[HideInInspector]
	[SerializeField]
	protected UIWidget.Pivot mPivot = UIWidget.Pivot.Center;

	// Token: 0x04001D8E RID: 7566
	[HideInInspector]
	[SerializeField]
	protected int mWidth = 100;

	// Token: 0x04001D8F RID: 7567
	[HideInInspector]
	[SerializeField]
	protected int mHeight = 100;

	// Token: 0x04001D90 RID: 7568
	[HideInInspector]
	[SerializeField]
	protected int mDepth;

	// Token: 0x04001D91 RID: 7569
	public UIWidget.OnDimensionsChanged onChange;

	// Token: 0x04001D92 RID: 7570
	public UIWidget.OnPostFillCallback onPostFill;

	// Token: 0x04001D93 RID: 7571
	public UIDrawCall.OnRenderCallback mOnRender;

	// Token: 0x04001D94 RID: 7572
	public bool autoResizeBoxCollider;

	// Token: 0x04001D95 RID: 7573
	public bool hideIfOffScreen;

	// Token: 0x04001D96 RID: 7574
	public UIWidget.AspectRatioSource keepAspectRatio;

	// Token: 0x04001D97 RID: 7575
	public float aspectRatio = 1f;

	// Token: 0x04001D98 RID: 7576
	public UIWidget.HitCheck hitCheck;

	// Token: 0x04001D99 RID: 7577
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x04001D9A RID: 7578
	[NonSerialized]
	public UIGeometry geometry = new UIGeometry();

	// Token: 0x04001D9B RID: 7579
	[NonSerialized]
	public bool fillGeometry = true;

	// Token: 0x04001D9C RID: 7580
	[NonSerialized]
	protected bool mPlayMode = true;

	// Token: 0x04001D9D RID: 7581
	[NonSerialized]
	protected Vector4 mDrawRegion = new Vector4(0f, 0f, 1f, 1f);

	// Token: 0x04001D9E RID: 7582
	[NonSerialized]
	private Matrix4x4 mLocalToPanel;

	// Token: 0x04001D9F RID: 7583
	[NonSerialized]
	private bool mIsVisibleByAlpha = true;

	// Token: 0x04001DA0 RID: 7584
	[NonSerialized]
	private bool mIsVisibleByPanel = true;

	// Token: 0x04001DA1 RID: 7585
	[NonSerialized]
	private bool mIsInFront = true;

	// Token: 0x04001DA2 RID: 7586
	[NonSerialized]
	private float mLastAlpha;

	// Token: 0x04001DA3 RID: 7587
	[NonSerialized]
	private bool mMoved;

	// Token: 0x04001DA4 RID: 7588
	[NonSerialized]
	public UIDrawCall drawCall;

	// Token: 0x04001DA5 RID: 7589
	[NonSerialized]
	protected Vector3[] mCorners = new Vector3[4];

	// Token: 0x04001DA6 RID: 7590
	[NonSerialized]
	private int mAlphaFrameID = -1;

	// Token: 0x04001DA7 RID: 7591
	private int mMatrixFrame = -1;

	// Token: 0x04001DA8 RID: 7592
	private Vector3 mOldV0;

	// Token: 0x04001DA9 RID: 7593
	private Vector3 mOldV1;

	// Token: 0x02000611 RID: 1553
	public enum Pivot
	{
		// Token: 0x04001DAB RID: 7595
		TopLeft,
		// Token: 0x04001DAC RID: 7596
		Top,
		// Token: 0x04001DAD RID: 7597
		TopRight,
		// Token: 0x04001DAE RID: 7598
		Left,
		// Token: 0x04001DAF RID: 7599
		Center,
		// Token: 0x04001DB0 RID: 7600
		Right,
		// Token: 0x04001DB1 RID: 7601
		BottomLeft,
		// Token: 0x04001DB2 RID: 7602
		Bottom,
		// Token: 0x04001DB3 RID: 7603
		BottomRight
	}

	// Token: 0x02000612 RID: 1554
	// (Invoke) Token: 0x06003424 RID: 13348
	public delegate void OnDimensionsChanged();

	// Token: 0x02000613 RID: 1555
	// (Invoke) Token: 0x06003428 RID: 13352
	public delegate void OnPostFillCallback(UIWidget widget, int bufferOffset, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols);

	// Token: 0x02000614 RID: 1556
	public enum AspectRatioSource
	{
		// Token: 0x04001DB5 RID: 7605
		Free,
		// Token: 0x04001DB6 RID: 7606
		BasedOnWidth,
		// Token: 0x04001DB7 RID: 7607
		BasedOnHeight
	}

	// Token: 0x02000615 RID: 1557
	// (Invoke) Token: 0x0600342C RID: 13356
	public delegate bool HitCheck(Vector3 worldPos);
}
