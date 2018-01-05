using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200064F RID: 1615
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Panel")]
public class UIPanel : UIRect
{
	// Token: 0x1700086B RID: 2155
	// (get) Token: 0x06003645 RID: 13893 RVA: 0x00114584 File Offset: 0x00112984
	public static int nextUnusedDepth
	{
		get
		{
			int num = int.MinValue;
			int i = 0;
			int count = UIPanel.list.Count;
			while (i < count)
			{
				num = Mathf.Max(num, UIPanel.list[i].depth);
				i++;
			}
			return (num != int.MinValue) ? (num + 1) : 0;
		}
	}

	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x06003646 RID: 13894 RVA: 0x001145DF File Offset: 0x001129DF
	public override bool canBeAnchored
	{
		get
		{
			return this.mClipping != UIDrawCall.Clipping.None;
		}
	}

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x06003647 RID: 13895 RVA: 0x001145ED File Offset: 0x001129ED
	// (set) Token: 0x06003648 RID: 13896 RVA: 0x001145F8 File Offset: 0x001129F8
	public override float alpha
	{
		get
		{
			return this.mAlpha;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mAlpha != num)
			{
				this.mAlphaFrameID = -1;
				this.mResized = true;
				this.mAlpha = num;
				this.SetDirty();
			}
		}
	}

	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x06003649 RID: 13897 RVA: 0x00114633 File Offset: 0x00112A33
	// (set) Token: 0x0600364A RID: 13898 RVA: 0x0011463B File Offset: 0x00112A3B
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
				this.mDepth = value;
				List<UIPanel> list = UIPanel.list;
				if (UIPanel.f__mg0 == null)
				{
					UIPanel.f__mg0 = new Comparison<UIPanel>(UIPanel.CompareFunc);
				}
				list.Sort(UIPanel.f__mg0);
			}
		}
	}

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x0600364B RID: 13899 RVA: 0x00114677 File Offset: 0x00112A77
	// (set) Token: 0x0600364C RID: 13900 RVA: 0x0011467F File Offset: 0x00112A7F
	public int sortingOrder
	{
		get
		{
			return this.mSortingOrder;
		}
		set
		{
			if (this.mSortingOrder != value)
			{
				this.mSortingOrder = value;
				this.UpdateDrawCalls();
			}
		}
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x0011469C File Offset: 0x00112A9C
	public static int CompareFunc(UIPanel a, UIPanel b)
	{
		if (!(a != b) || !(a != null) || !(b != null))
		{
			return 0;
		}
		if (a.mDepth < b.mDepth)
		{
			return -1;
		}
		if (a.mDepth > b.mDepth)
		{
			return 1;
		}
		return (a.GetInstanceID() >= b.GetInstanceID()) ? 1 : -1;
	}

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x0600364E RID: 13902 RVA: 0x00114710 File Offset: 0x00112B10
	public float width
	{
		get
		{
			return this.GetViewSize().x;
		}
	}

	// Token: 0x17000871 RID: 2161
	// (get) Token: 0x0600364F RID: 13903 RVA: 0x0011472C File Offset: 0x00112B2C
	public float height
	{
		get
		{
			return this.GetViewSize().y;
		}
	}

	// Token: 0x17000872 RID: 2162
	// (get) Token: 0x06003650 RID: 13904 RVA: 0x00114747 File Offset: 0x00112B47
	public bool halfPixelOffset
	{
		get
		{
			return this.mHalfPixelOffset;
		}
	}

	// Token: 0x17000873 RID: 2163
	// (get) Token: 0x06003651 RID: 13905 RVA: 0x0011474F File Offset: 0x00112B4F
	public bool usedForUI
	{
		get
		{
			return base.anchorCamera != null && this.mCam.orthographic;
		}
	}

	// Token: 0x17000874 RID: 2164
	// (get) Token: 0x06003652 RID: 13906 RVA: 0x00114770 File Offset: 0x00112B70
	public Vector3 drawCallOffset
	{
		get
		{
			if (this.mHalfPixelOffset && base.anchorCamera != null && this.mCam.orthographic)
			{
				float num = 1f / this.GetWindowSize().y / this.mCam.orthographicSize;
				return new Vector3(-num, num);
			}
			return Vector3.zero;
		}
	}

	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x06003653 RID: 13907 RVA: 0x001147D8 File Offset: 0x00112BD8
	// (set) Token: 0x06003654 RID: 13908 RVA: 0x001147E0 File Offset: 0x00112BE0
	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mResized = true;
				this.mClipping = value;
				this.mMatrixFrame = -1;
			}
		}
	}

	// Token: 0x17000876 RID: 2166
	// (get) Token: 0x06003655 RID: 13909 RVA: 0x00114803 File Offset: 0x00112C03
	public UIPanel parentPanel
	{
		get
		{
			return this.mParentPanel;
		}
	}

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x06003656 RID: 13910 RVA: 0x0011480C File Offset: 0x00112C0C
	public int clipCount
	{
		get
		{
			int num = 0;
			UIPanel uipanel = this;
			while (uipanel != null)
			{
				if (uipanel.mClipping == UIDrawCall.Clipping.SoftClip || uipanel.mClipping == UIDrawCall.Clipping.TextureMask)
				{
					num++;
				}
				uipanel = uipanel.mParentPanel;
			}
			return num;
		}
	}

	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x06003657 RID: 13911 RVA: 0x00114852 File Offset: 0x00112C52
	public bool hasClipping
	{
		get
		{
			return this.mClipping == UIDrawCall.Clipping.SoftClip || this.mClipping == UIDrawCall.Clipping.TextureMask;
		}
	}

	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x06003658 RID: 13912 RVA: 0x0011486C File Offset: 0x00112C6C
	public bool hasCumulativeClipping
	{
		get
		{
			return this.clipCount != 0;
		}
	}

	// Token: 0x1700087A RID: 2170
	// (get) Token: 0x06003659 RID: 13913 RVA: 0x0011487A File Offset: 0x00112C7A
	[Obsolete("Use 'hasClipping' or 'hasCumulativeClipping' instead")]
	public bool clipsChildren
	{
		get
		{
			return this.hasCumulativeClipping;
		}
	}

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x0600365A RID: 13914 RVA: 0x00114882 File Offset: 0x00112C82
	// (set) Token: 0x0600365B RID: 13915 RVA: 0x0011488C File Offset: 0x00112C8C
	public Vector2 clipOffset
	{
		get
		{
			return this.mClipOffset;
		}
		set
		{
			if (Mathf.Abs(this.mClipOffset.x - value.x) > 0.001f || Mathf.Abs(this.mClipOffset.y - value.y) > 0.001f)
			{
				this.mClipOffset = value;
				this.InvalidateClipping();
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x00114904 File Offset: 0x00112D04
	private void InvalidateClipping()
	{
		this.mResized = true;
		this.mMatrixFrame = -1;
		this.mCullTime = ((this.mCullTime != 0f) ? (RealTime.time + 0.15f) : 0.001f);
		int i = 0;
		int count = UIPanel.list.Count;
		while (i < count)
		{
			UIPanel uipanel = UIPanel.list[i];
			if (uipanel != this && uipanel.parentPanel == this)
			{
				uipanel.InvalidateClipping();
			}
			i++;
		}
	}

	// Token: 0x1700087C RID: 2172
	// (get) Token: 0x0600365D RID: 13917 RVA: 0x00114996 File Offset: 0x00112D96
	// (set) Token: 0x0600365E RID: 13918 RVA: 0x0011499E File Offset: 0x00112D9E
	public Texture2D clipTexture
	{
		get
		{
			return this.mClipTexture;
		}
		set
		{
			if (this.mClipTexture != value)
			{
				this.mClipTexture = value;
			}
		}
	}

	// Token: 0x1700087D RID: 2173
	// (get) Token: 0x0600365F RID: 13919 RVA: 0x001149B8 File Offset: 0x00112DB8
	// (set) Token: 0x06003660 RID: 13920 RVA: 0x001149C0 File Offset: 0x00112DC0
	[Obsolete("Use 'finalClipRegion' or 'baseClipRegion' instead")]
	public Vector4 clipRange
	{
		get
		{
			return this.baseClipRegion;
		}
		set
		{
			this.baseClipRegion = value;
		}
	}

	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x06003661 RID: 13921 RVA: 0x001149C9 File Offset: 0x00112DC9
	// (set) Token: 0x06003662 RID: 13922 RVA: 0x001149D4 File Offset: 0x00112DD4
	public Vector4 baseClipRegion
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			if (Mathf.Abs(this.mClipRange.x - value.x) > 0.001f || Mathf.Abs(this.mClipRange.y - value.y) > 0.001f || Mathf.Abs(this.mClipRange.z - value.z) > 0.001f || Mathf.Abs(this.mClipRange.w - value.w) > 0.001f)
			{
				this.mResized = true;
				this.mCullTime = ((this.mCullTime != 0f) ? (RealTime.time + 0.15f) : 0.001f);
				this.mClipRange = value;
				this.mMatrixFrame = -1;
				UIScrollView component = base.GetComponent<UIScrollView>();
				if (component != null)
				{
					component.UpdatePosition();
				}
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x06003663 RID: 13923 RVA: 0x00114ADC File Offset: 0x00112EDC
	public Vector4 finalClipRegion
	{
		get
		{
			Vector2 viewSize = this.GetViewSize();
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				return new Vector4(this.mClipRange.x + this.mClipOffset.x, this.mClipRange.y + this.mClipOffset.y, viewSize.x, viewSize.y);
			}
			return new Vector4(0f, 0f, viewSize.x, viewSize.y);
		}
	}

	// Token: 0x17000880 RID: 2176
	// (get) Token: 0x06003664 RID: 13924 RVA: 0x00114B5A File Offset: 0x00112F5A
	// (set) Token: 0x06003665 RID: 13925 RVA: 0x00114B62 File Offset: 0x00112F62
	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoftness;
		}
		set
		{
			if (this.mClipSoftness != value)
			{
				this.mClipSoftness = value;
			}
		}
	}

	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x06003666 RID: 13926 RVA: 0x00114B7C File Offset: 0x00112F7C
	public override Vector3[] localCorners
	{
		get
		{
			if (this.mClipping == UIDrawCall.Clipping.None)
			{
				Vector3[] worldCorners = this.worldCorners;
				Transform cachedTransform = base.cachedTransform;
				for (int i = 0; i < 4; i++)
				{
					worldCorners[i] = cachedTransform.InverseTransformPoint(worldCorners[i]);
				}
				return worldCorners;
			}
			float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
			float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
			float x = num + this.mClipRange.z;
			float y = num2 + this.mClipRange.w;
			UIPanel.mCorners[0] = new Vector3(num, num2);
			UIPanel.mCorners[1] = new Vector3(num, y);
			UIPanel.mCorners[2] = new Vector3(x, y);
			UIPanel.mCorners[3] = new Vector3(x, num2);
			return UIPanel.mCorners;
		}
	}

	// Token: 0x17000882 RID: 2178
	// (get) Token: 0x06003667 RID: 13927 RVA: 0x00114CB0 File Offset: 0x001130B0
	public override Vector3[] worldCorners
	{
		get
		{
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
				float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
				float x = num + this.mClipRange.z;
				float y = num2 + this.mClipRange.w;
				Transform cachedTransform = base.cachedTransform;
				UIPanel.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
				UIPanel.mCorners[1] = cachedTransform.TransformPoint(num, y, 0f);
				UIPanel.mCorners[2] = cachedTransform.TransformPoint(x, y, 0f);
				UIPanel.mCorners[3] = cachedTransform.TransformPoint(x, num2, 0f);
			}
			else
			{
				if (base.anchorCamera != null)
				{
					return this.mCam.GetWorldCorners(base.cameraRayDistance);
				}
				Vector2 viewSize = this.GetViewSize();
				float num3 = -0.5f * viewSize.x;
				float num4 = -0.5f * viewSize.y;
				float x2 = num3 + viewSize.x;
				float y2 = num4 + viewSize.y;
				UIPanel.mCorners[0] = new Vector3(num3, num4);
				UIPanel.mCorners[1] = new Vector3(num3, y2);
				UIPanel.mCorners[2] = new Vector3(x2, y2);
				UIPanel.mCorners[3] = new Vector3(x2, num4);
				if ((this.anchorOffset && this.mCam == null) || this.mCam.transform.parent != base.cachedTransform)
				{
					Vector3 position = base.cachedTransform.position;
					for (int i = 0; i < 4; i++)
					{
						UIPanel.mCorners[i] += position;
					}
				}
			}
			return UIPanel.mCorners;
		}
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x00114F08 File Offset: 0x00113308
	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
			float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
			float num3 = num + this.mClipRange.z;
			float num4 = num2 + this.mClipRange.w;
			float x = (num + num3) * 0.5f;
			float y = (num2 + num4) * 0.5f;
			Transform cachedTransform = base.cachedTransform;
			UIRect.mSides[0] = cachedTransform.TransformPoint(num, y, 0f);
			UIRect.mSides[1] = cachedTransform.TransformPoint(x, num4, 0f);
			UIRect.mSides[2] = cachedTransform.TransformPoint(num3, y, 0f);
			UIRect.mSides[3] = cachedTransform.TransformPoint(x, num2, 0f);
			if (relativeTo != null)
			{
				for (int i = 0; i < 4; i++)
				{
					UIRect.mSides[i] = relativeTo.InverseTransformPoint(UIRect.mSides[i]);
				}
			}
			return UIRect.mSides;
		}
		if (base.anchorCamera != null && this.anchorOffset)
		{
			Vector3[] sides = this.mCam.GetSides(base.cameraRayDistance);
			Vector3 position = base.cachedTransform.position;
			for (int j = 0; j < 4; j++)
			{
				sides[j] += position;
			}
			if (relativeTo != null)
			{
				for (int k = 0; k < 4; k++)
				{
					sides[k] = relativeTo.InverseTransformPoint(sides[k]);
				}
			}
			return sides;
		}
		return base.GetSides(relativeTo);
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x00115135 File Offset: 0x00113535
	public override void Invalidate(bool includeChildren)
	{
		this.mAlphaFrameID = -1;
		base.Invalidate(includeChildren);
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x00115148 File Offset: 0x00113548
	public override float CalculateFinalAlpha(int frameID)
	{
		if (this.mAlphaFrameID != frameID)
		{
			this.mAlphaFrameID = frameID;
			UIRect parent = base.parent;
			this.finalAlpha = ((!(base.parent != null)) ? this.mAlpha : (parent.CalculateFinalAlpha(frameID) * this.mAlpha));
		}
		return this.finalAlpha;
	}

	// Token: 0x0600366B RID: 13931 RVA: 0x001151A8 File Offset: 0x001135A8
	public override void SetRect(float x, float y, float width, float height)
	{
		int num = Mathf.FloorToInt(width + 0.5f);
		int num2 = Mathf.FloorToInt(height + 0.5f);
		num = num >> 1 << 1;
		num2 = num2 >> 1 << 1;
		Transform transform = base.cachedTransform;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = Mathf.Floor(x + 0.5f);
		localPosition.y = Mathf.Floor(y + 0.5f);
		if (num < 2)
		{
			num = 2;
		}
		if (num2 < 2)
		{
			num2 = 2;
		}
		this.baseClipRegion = new Vector4(localPosition.x, localPosition.y, (float)num, (float)num2);
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

	// Token: 0x0600366C RID: 13932 RVA: 0x001152E0 File Offset: 0x001136E0
	public bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this.UpdateTransformMatrix();
		a = this.worldToLocal.MultiplyPoint3x4(a);
		b = this.worldToLocal.MultiplyPoint3x4(b);
		c = this.worldToLocal.MultiplyPoint3x4(c);
		d = this.worldToLocal.MultiplyPoint3x4(d);
		UIPanel.mTemp[0] = a.x;
		UIPanel.mTemp[1] = b.x;
		UIPanel.mTemp[2] = c.x;
		UIPanel.mTemp[3] = d.x;
		float num = Mathf.Min(UIPanel.mTemp);
		float num2 = Mathf.Max(UIPanel.mTemp);
		UIPanel.mTemp[0] = a.y;
		UIPanel.mTemp[1] = b.y;
		UIPanel.mTemp[2] = c.y;
		UIPanel.mTemp[3] = d.y;
		float num3 = Mathf.Min(UIPanel.mTemp);
		float num4 = Mathf.Max(UIPanel.mTemp);
		return num2 >= this.mMin.x && num4 >= this.mMin.y && num <= this.mMax.x && num3 <= this.mMax.y;
	}

	// Token: 0x0600366D RID: 13933 RVA: 0x00115418 File Offset: 0x00113818
	public bool IsVisible(Vector3 worldPos)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None || this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip)
		{
			return true;
		}
		this.UpdateTransformMatrix();
		Vector3 vector = this.worldToLocal.MultiplyPoint3x4(worldPos);
		return vector.x >= this.mMin.x && vector.y >= this.mMin.y && vector.x <= this.mMax.x && vector.y <= this.mMax.y;
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x001154C8 File Offset: 0x001138C8
	public bool IsVisible(UIWidget w)
	{
		UIPanel uipanel = this;
		Vector3[] array = null;
		while (uipanel != null)
		{
			if ((uipanel.mClipping == UIDrawCall.Clipping.None || uipanel.mClipping == UIDrawCall.Clipping.ConstrainButDontClip) && !w.hideIfOffScreen)
			{
				uipanel = uipanel.mParentPanel;
			}
			else
			{
				if (array == null)
				{
					array = w.worldCorners;
				}
				if (!uipanel.IsVisible(array[0], array[1], array[2], array[3]))
				{
					return false;
				}
				uipanel = uipanel.mParentPanel;
			}
		}
		return true;
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x0011556C File Offset: 0x0011396C
	public bool Affects(UIWidget w)
	{
		if (w == null)
		{
			return false;
		}
		UIPanel panel = w.panel;
		if (panel == null)
		{
			return false;
		}
		UIPanel uipanel = this;
		while (uipanel != null)
		{
			if (uipanel == panel)
			{
				return true;
			}
			if (!uipanel.hasCumulativeClipping)
			{
				return false;
			}
			uipanel = uipanel.mParentPanel;
		}
		return false;
	}

	// Token: 0x06003670 RID: 13936 RVA: 0x001155D2 File Offset: 0x001139D2
	[ContextMenu("Force Refresh")]
	public void RebuildAllDrawCalls()
	{
		this.mRebuild = true;
	}

	// Token: 0x06003671 RID: 13937 RVA: 0x001155DC File Offset: 0x001139DC
	public void SetDirty()
	{
		int i = 0;
		int count = this.drawCalls.Count;
		while (i < count)
		{
			this.drawCalls[i].isDirty = true;
			i++;
		}
		this.Invalidate(true);
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x00115620 File Offset: 0x00113A20
	private void Awake()
	{
		this.mGo = base.gameObject;
		this.mTrans = base.transform;
		this.mHalfPixelOffset = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor);
		if (this.mHalfPixelOffset)
		{
			this.mHalfPixelOffset = (SystemInfo.graphicsShaderLevel < 40 && SystemInfo.graphicsDeviceVersion.Contains("Direct3D"));
		}
	}

	// Token: 0x06003673 RID: 13939 RVA: 0x00115690 File Offset: 0x00113A90
	private void FindParent()
	{
		Transform parent = base.cachedTransform.parent;
		this.mParentPanel = ((!(parent != null)) ? null : NGUITools.FindInParents<UIPanel>(parent.gameObject));
	}

	// Token: 0x06003674 RID: 13940 RVA: 0x001156CC File Offset: 0x00113ACC
	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		this.FindParent();
	}

	// Token: 0x06003675 RID: 13941 RVA: 0x001156DA File Offset: 0x00113ADA
	protected override void OnStart()
	{
		this.mLayer = this.mGo.layer;
	}

	// Token: 0x06003676 RID: 13942 RVA: 0x001156ED File Offset: 0x00113AED
	protected override void OnEnable()
	{
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		this.OnStart();
		base.OnEnable();
		this.mMatrixFrame = -1;
	}

	// Token: 0x06003677 RID: 13943 RVA: 0x00115718 File Offset: 0x00113B18
	protected override void OnInit()
	{
		base.OnInit();
		this.FindParent();
		if (base.GetComponent<Rigidbody>() == null && this.mParentPanel == null)
		{
			UICamera uicamera = (!(base.anchorCamera != null)) ? null : this.mCam.GetComponent<UICamera>();
			if (uicamera != null && (uicamera.eventType == UICamera.EventType.UI_3D || uicamera.eventType == UICamera.EventType.World_3D))
			{
				Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
				rigidbody.isKinematic = true;
				rigidbody.useGravity = false;
			}
		}
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		UIPanel.list.Add(this);
		List<UIPanel> list = UIPanel.list;
		if (UIPanel.f__mg1 == null)
		{
			UIPanel.f__mg1 = new Comparison<UIPanel>(UIPanel.CompareFunc);
		}
		list.Sort(UIPanel.f__mg1);
	}

	// Token: 0x06003678 RID: 13944 RVA: 0x001157FC File Offset: 0x00113BFC
	protected override void OnDisable()
	{
		int i = 0;
		int count = this.drawCalls.Count;
		while (i < count)
		{
			UIDrawCall uidrawCall = this.drawCalls[i];
			if (uidrawCall != null)
			{
				UIDrawCall.Destroy(uidrawCall);
			}
			i++;
		}
		this.drawCalls.Clear();
		UIPanel.list.Remove(this);
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		if (UIPanel.list.Count == 0)
		{
			UIDrawCall.ReleaseAll();
			UIPanel.mUpdateFrame = -1;
		}
		base.OnDisable();
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x0011588C File Offset: 0x00113C8C
	private void UpdateTransformMatrix()
	{
		int frameCount = Time.frameCount;
		if (this.mMatrixFrame != frameCount)
		{
			this.mMatrixFrame = frameCount;
			this.worldToLocal = base.cachedTransform.worldToLocalMatrix;
			Vector2 vector = this.GetViewSize() * 0.5f;
			float num = this.mClipOffset.x + this.mClipRange.x;
			float num2 = this.mClipOffset.y + this.mClipRange.y;
			this.mMin.x = num - vector.x;
			this.mMin.y = num2 - vector.y;
			this.mMax.x = num + vector.x;
			this.mMax.y = num2 + vector.y;
		}
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x00115954 File Offset: 0x00113D54
	protected override void OnAnchor()
	{
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return;
		}
		Transform cachedTransform = base.cachedTransform;
		Transform parent = cachedTransform.parent;
		Vector2 viewSize = this.GetViewSize();
		Vector2 vector = cachedTransform.localPosition;
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
			}
			else
			{
				Vector2 vector2 = base.GetLocalPos(this.leftAnchor, parent);
				num = vector2.x + (float)this.leftAnchor.absolute;
				num3 = vector2.y + (float)this.bottomAnchor.absolute;
				num2 = vector2.x + (float)this.rightAnchor.absolute;
				num4 = vector2.y + (float)this.topAnchor.absolute;
			}
		}
		else
		{
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
				num = this.mClipRange.x - 0.5f * viewSize.x;
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
				num2 = this.mClipRange.x + 0.5f * viewSize.x;
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
				num3 = this.mClipRange.y - 0.5f * viewSize.y;
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
				num4 = this.mClipRange.y + 0.5f * viewSize.y;
			}
		}
		num -= vector.x + this.mClipOffset.x;
		num2 -= vector.x + this.mClipOffset.x;
		num3 -= vector.y + this.mClipOffset.y;
		num4 -= vector.y + this.mClipOffset.y;
		float x = Mathf.Lerp(num, num2, 0.5f);
		float y = Mathf.Lerp(num3, num4, 0.5f);
		float num5 = num2 - num;
		float num6 = num4 - num3;
		float num7 = Mathf.Max(2f, this.mClipSoftness.x);
		float num8 = Mathf.Max(2f, this.mClipSoftness.y);
		if (num5 < num7)
		{
			num5 = num7;
		}
		if (num6 < num8)
		{
			num6 = num8;
		}
		this.baseClipRegion = new Vector4(x, y, num5, num6);
	}

	// Token: 0x0600367B RID: 13947 RVA: 0x00115EFC File Offset: 0x001142FC
	private void LateUpdate()
	{
		if (UIPanel.mUpdateFrame != Time.frameCount)
		{
			UIPanel.mUpdateFrame = Time.frameCount;
			int i = 0;
			int count = UIPanel.list.Count;
			while (i < count)
			{
				UIPanel.list[i].UpdateSelf();
				i++;
			}
			int num = 3000;
			int j = 0;
			int count2 = UIPanel.list.Count;
			while (j < count2)
			{
				UIPanel uipanel = UIPanel.list[j];
				if (uipanel.renderQueue == UIPanel.RenderQueue.Automatic)
				{
					uipanel.startingRenderQueue = num;
					uipanel.UpdateDrawCalls();
					num += uipanel.drawCalls.Count;
				}
				else if (uipanel.renderQueue == UIPanel.RenderQueue.StartAt)
				{
					uipanel.UpdateDrawCalls();
					if (uipanel.drawCalls.Count != 0)
					{
						num = Mathf.Max(num, uipanel.startingRenderQueue + uipanel.drawCalls.Count);
					}
				}
				else
				{
					uipanel.UpdateDrawCalls();
					if (uipanel.drawCalls.Count != 0)
					{
						num = Mathf.Max(num, uipanel.startingRenderQueue + 1);
					}
				}
				j++;
			}
		}
	}

	// Token: 0x0600367C RID: 13948 RVA: 0x00116020 File Offset: 0x00114420
	private void UpdateSelf()
	{
		this.mUpdateTime = RealTime.time;
		this.UpdateTransformMatrix();
		this.UpdateLayers();
		this.UpdateWidgets();
		if (this.mRebuild)
		{
			this.mRebuild = false;
			this.FillAllDrawCalls();
		}
		else
		{
			int i = 0;
			while (i < this.drawCalls.Count)
			{
				UIDrawCall uidrawCall = this.drawCalls[i];
				if (uidrawCall.isDirty && !this.FillDrawCall(uidrawCall))
				{
					UIDrawCall.Destroy(uidrawCall);
					this.drawCalls.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
		if (this.mUpdateScroll)
		{
			this.mUpdateScroll = false;
			UIScrollView component = base.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars();
			}
		}
	}

	// Token: 0x0600367D RID: 13949 RVA: 0x001160E9 File Offset: 0x001144E9
	public void SortWidgets()
	{
		this.mSortWidgets = false;
		List<UIWidget> list = this.widgets;
		if (UIPanel.f__mg2 == null)
		{
			UIPanel.f__mg2 = new Comparison<UIWidget>(UIWidget.PanelCompareFunc);
		}
		list.Sort(UIPanel.f__mg2);
	}

	// Token: 0x0600367E RID: 13950 RVA: 0x0011611C File Offset: 0x0011451C
	private void FillAllDrawCalls()
	{
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall.Destroy(this.drawCalls[i]);
		}
		this.drawCalls.Clear();
		Material material = null;
		Texture texture = null;
		Shader shader = null;
		UIDrawCall uidrawCall = null;
		int num = 0;
		if (this.mSortWidgets)
		{
			this.SortWidgets();
		}
		for (int j = 0; j < this.widgets.Count; j++)
		{
			UIWidget uiwidget = this.widgets[j];
			if (uiwidget.isVisible && uiwidget.hasVertices)
			{
				Material material2 = uiwidget.material;
				Texture mainTexture = uiwidget.mainTexture;
				Shader shader2 = uiwidget.shader;
				if (material != material2 || texture != mainTexture || shader != shader2)
				{
					if (uidrawCall != null && uidrawCall.verts.size != 0)
					{
						this.drawCalls.Add(uidrawCall);
						uidrawCall.UpdateGeometry(num);
						uidrawCall.onRender = this.mOnRender;
						this.mOnRender = null;
						num = 0;
						uidrawCall = null;
					}
					material = material2;
					texture = mainTexture;
					shader = shader2;
				}
				if (material != null || shader != null || texture != null)
				{
					if (uidrawCall == null)
					{
						uidrawCall = UIDrawCall.Create(this, material, texture, shader);
						uidrawCall.depthStart = uiwidget.depth;
						uidrawCall.depthEnd = uidrawCall.depthStart;
						uidrawCall.panel = this;
					}
					else
					{
						int depth = uiwidget.depth;
						if (depth < uidrawCall.depthStart)
						{
							uidrawCall.depthStart = depth;
						}
						if (depth > uidrawCall.depthEnd)
						{
							uidrawCall.depthEnd = depth;
						}
					}
					uiwidget.drawCall = uidrawCall;
					num++;
					if (this.generateNormals)
					{
						uiwidget.WriteToBuffers(uidrawCall.verts, uidrawCall.uvs, uidrawCall.cols, uidrawCall.norms, uidrawCall.tans);
					}
					else
					{
						uiwidget.WriteToBuffers(uidrawCall.verts, uidrawCall.uvs, uidrawCall.cols, null, null);
					}
					if (uiwidget.mOnRender != null)
					{
						if (this.mOnRender == null)
						{
							this.mOnRender = uiwidget.mOnRender;
						}
						else
						{
							this.mOnRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(this.mOnRender, uiwidget.mOnRender);
						}
					}
				}
			}
			else
			{
				uiwidget.drawCall = null;
			}
		}
		if (uidrawCall != null && uidrawCall.verts.size != 0)
		{
			this.drawCalls.Add(uidrawCall);
			uidrawCall.UpdateGeometry(num);
			uidrawCall.onRender = this.mOnRender;
			this.mOnRender = null;
		}
	}

	// Token: 0x0600367F RID: 13951 RVA: 0x001163FC File Offset: 0x001147FC
	private bool FillDrawCall(UIDrawCall dc)
	{
		if (dc != null)
		{
			dc.isDirty = false;
			int num = 0;
			int i = 0;
			while (i < this.widgets.Count)
			{
				UIWidget uiwidget = this.widgets[i];
				if (uiwidget == null)
				{
					this.widgets.RemoveAt(i);
				}
				else
				{
					if (uiwidget.drawCall == dc)
					{
						if (uiwidget.isVisible && uiwidget.hasVertices)
						{
							num++;
							if (this.generateNormals)
							{
								uiwidget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, dc.norms, dc.tans);
							}
							else
							{
								uiwidget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, null, null);
							}
							if (uiwidget.mOnRender != null)
							{
								if (this.mOnRender == null)
								{
									this.mOnRender = uiwidget.mOnRender;
								}
								else
								{
									this.mOnRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(this.mOnRender, uiwidget.mOnRender);
								}
							}
						}
						else
						{
							uiwidget.drawCall = null;
						}
					}
					i++;
				}
			}
			if (dc.verts.size != 0)
			{
				dc.UpdateGeometry(num);
				dc.onRender = this.mOnRender;
				this.mOnRender = null;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x0011655C File Offset: 0x0011495C
	private void UpdateDrawCalls()
	{
		Transform cachedTransform = base.cachedTransform;
		bool usedForUI = this.usedForUI;
		if (this.clipping != UIDrawCall.Clipping.None)
		{
			this.drawCallClipRange = this.finalClipRegion;
			this.drawCallClipRange.z = this.drawCallClipRange.z * 0.5f;
			this.drawCallClipRange.w = this.drawCallClipRange.w * 0.5f;
		}
		else
		{
			this.drawCallClipRange = Vector4.zero;
		}
		if (this.drawCallClipRange.z == 0f)
		{
			this.drawCallClipRange.z = (float)Screen.width * 0.5f;
		}
		if (this.drawCallClipRange.w == 0f)
		{
			this.drawCallClipRange.w = (float)Screen.height * 0.5f;
		}
		if (this.halfPixelOffset)
		{
			this.drawCallClipRange.x = this.drawCallClipRange.x - 0.5f;
			this.drawCallClipRange.y = this.drawCallClipRange.y + 0.5f;
		}
		Vector3 vector;
		if (usedForUI)
		{
			Transform parent = base.cachedTransform.parent;
			vector = base.cachedTransform.localPosition;
			if (this.clipping != UIDrawCall.Clipping.None)
			{
				vector.x = (float)Mathf.RoundToInt(vector.x);
				vector.y = (float)Mathf.RoundToInt(vector.y);
			}
			if (parent != null)
			{
				vector = parent.TransformPoint(vector);
			}
			vector += this.drawCallOffset;
		}
		else
		{
			vector = cachedTransform.position;
		}
		Quaternion rotation = cachedTransform.rotation;
		Vector3 lossyScale = cachedTransform.lossyScale;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall uidrawCall = this.drawCalls[i];
			Transform cachedTransform2 = uidrawCall.cachedTransform;
			cachedTransform2.position = vector;
			cachedTransform2.rotation = rotation;
			cachedTransform2.localScale = lossyScale;
			uidrawCall.renderQueue = ((this.renderQueue != UIPanel.RenderQueue.Explicit) ? (this.startingRenderQueue + i) : this.startingRenderQueue);
			uidrawCall.alwaysOnScreen = (this.alwaysOnScreen && (this.mClipping == UIDrawCall.Clipping.None || this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip));
			uidrawCall.sortingOrder = this.mSortingOrder;
			uidrawCall.clipTexture = this.mClipTexture;
		}
	}

	// Token: 0x06003681 RID: 13953 RVA: 0x001167AC File Offset: 0x00114BAC
	private void UpdateLayers()
	{
		if (this.mLayer != base.cachedGameObject.layer)
		{
			this.mLayer = this.mGo.layer;
			NGUITools.SetChildLayer(base.cachedTransform, this.mLayer);
			base.ResetAnchors();
			for (int i = 0; i < this.drawCalls.Count; i++)
			{
				this.drawCalls[i].gameObject.layer = this.mLayer;
			}
		}
	}

	// Token: 0x06003682 RID: 13954 RVA: 0x00116830 File Offset: 0x00114C30
	private void UpdateWidgets()
	{
		bool flag = !this.cullWhileDragging && this.mCullTime > this.mUpdateTime;
		bool flag2 = false;
		if (this.mForced != flag)
		{
			this.mForced = flag;
			this.mResized = true;
		}
		bool hasCumulativeClipping = this.hasCumulativeClipping;
		int i = 0;
		int count = this.widgets.Count;
		while (i < count)
		{
			UIWidget uiwidget = this.widgets[i];
			if (uiwidget.panel == this && uiwidget.enabled)
			{
				int frameCount = Time.frameCount;
				if (uiwidget.UpdateTransform(frameCount) || this.mResized)
				{
					bool visibleByAlpha = flag || uiwidget.CalculateCumulativeAlpha(frameCount) > 0.001f;
					uiwidget.UpdateVisibility(visibleByAlpha, flag || (!hasCumulativeClipping && !uiwidget.hideIfOffScreen) || this.IsVisible(uiwidget));
				}
				if (uiwidget.UpdateGeometry(frameCount))
				{
					flag2 = true;
					if (!this.mRebuild)
					{
						if (uiwidget.drawCall != null)
						{
							uiwidget.drawCall.isDirty = true;
						}
						else
						{
							this.FindDrawCall(uiwidget);
						}
					}
				}
			}
			i++;
		}
		if (flag2 && this.onGeometryUpdated != null)
		{
			this.onGeometryUpdated();
		}
		this.mResized = false;
	}

	// Token: 0x06003683 RID: 13955 RVA: 0x001169A8 File Offset: 0x00114DA8
	public UIDrawCall FindDrawCall(UIWidget w)
	{
		Material material = w.material;
		Texture mainTexture = w.mainTexture;
		int depth = w.depth;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall uidrawCall = this.drawCalls[i];
			int num = (i != 0) ? (this.drawCalls[i - 1].depthEnd + 1) : int.MinValue;
			int num2 = (i + 1 != this.drawCalls.Count) ? (this.drawCalls[i + 1].depthStart - 1) : int.MaxValue;
			if (num <= depth && num2 >= depth)
			{
				if (uidrawCall.baseMaterial == material && uidrawCall.mainTexture == mainTexture)
				{
					if (w.isVisible)
					{
						w.drawCall = uidrawCall;
						if (w.hasVertices)
						{
							uidrawCall.isDirty = true;
						}
						return uidrawCall;
					}
				}
				else
				{
					this.mRebuild = true;
				}
				return null;
			}
		}
		this.mRebuild = true;
		return null;
	}

	// Token: 0x06003684 RID: 13956 RVA: 0x00116AC4 File Offset: 0x00114EC4
	public void AddWidget(UIWidget w)
	{
		this.mUpdateScroll = true;
		if (this.widgets.Count == 0)
		{
			this.widgets.Add(w);
		}
		else if (this.mSortWidgets)
		{
			this.widgets.Add(w);
			this.SortWidgets();
		}
		else if (UIWidget.PanelCompareFunc(w, this.widgets[0]) == -1)
		{
			this.widgets.Insert(0, w);
		}
		else
		{
			int i = this.widgets.Count;
			while (i > 0)
			{
				if (UIWidget.PanelCompareFunc(w, this.widgets[--i]) != -1)
				{
					this.widgets.Insert(i + 1, w);
					break;
				}
			}
		}
		this.FindDrawCall(w);
	}

	// Token: 0x06003685 RID: 13957 RVA: 0x00116B9C File Offset: 0x00114F9C
	public void RemoveWidget(UIWidget w)
	{
		if (this.widgets.Remove(w) && w.drawCall != null)
		{
			int depth = w.depth;
			if (depth == w.drawCall.depthStart || depth == w.drawCall.depthEnd)
			{
				this.mRebuild = true;
			}
			w.drawCall.isDirty = true;
			w.drawCall = null;
		}
	}

	// Token: 0x06003686 RID: 13958 RVA: 0x00116C0E File Offset: 0x0011500E
	public void Refresh()
	{
		this.mRebuild = true;
		UIPanel.mUpdateFrame = -1;
		if (UIPanel.list.Count > 0)
		{
			UIPanel.list[0].LateUpdate();
		}
	}

	// Token: 0x06003687 RID: 13959 RVA: 0x00116C40 File Offset: 0x00115040
	public virtual Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		Vector4 finalClipRegion = this.finalClipRegion;
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		Vector2 minRect = new Vector2(min.x, min.y);
		Vector2 maxRect = new Vector2(max.x, max.y);
		Vector2 minArea = new Vector2(finalClipRegion.x - num, finalClipRegion.y - num2);
		Vector2 maxArea = new Vector2(finalClipRegion.x + num, finalClipRegion.y + num2);
		if (this.softBorderPadding && this.clipping == UIDrawCall.Clipping.SoftClip)
		{
			minArea.x += this.mClipSoftness.x;
			minArea.y += this.mClipSoftness.y;
			maxArea.x -= this.mClipSoftness.x;
			maxArea.y -= this.mClipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}

	// Token: 0x06003688 RID: 13960 RVA: 0x00116D58 File Offset: 0x00115158
	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 vector = targetBounds.min;
		Vector3 vector2 = targetBounds.max;
		float num = 1f;
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				num = root.pixelSizeAdjustment;
			}
		}
		if (num != 1f)
		{
			vector /= num;
			vector2 /= num;
		}
		Vector3 b = this.CalculateConstrainOffset(vector, vector2) * num;
		if (b.sqrMagnitude > 0f)
		{
			if (immediate)
			{
				target.localPosition += b;
				targetBounds.center += b;
				SpringPosition component = target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + b, 13f);
				springPosition.ignoreTimeScale = true;
				springPosition.worldSpace = false;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06003689 RID: 13961 RVA: 0x00116E64 File Offset: 0x00115264
	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.cachedTransform, target);
		return this.ConstrainTargetToBounds(target, ref bounds, immediate);
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x00116E88 File Offset: 0x00115288
	public static UIPanel Find(Transform trans)
	{
		return UIPanel.Find(trans, false, -1);
	}

	// Token: 0x0600368B RID: 13963 RVA: 0x00116E92 File Offset: 0x00115292
	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		return UIPanel.Find(trans, createIfMissing, -1);
	}

	// Token: 0x0600368C RID: 13964 RVA: 0x00116E9C File Offset: 0x0011529C
	public static UIPanel Find(Transform trans, bool createIfMissing, int layer)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(trans);
		if (uipanel != null)
		{
			return uipanel;
		}
		return (!createIfMissing) ? null : NGUITools.CreateUI(trans, false, layer);
	}

	// Token: 0x0600368D RID: 13965 RVA: 0x00116ED4 File Offset: 0x001152D4
	private Vector2 GetWindowSize()
	{
		UIRoot root = base.root;
		Vector2 vector = NGUITools.screenSize;
		if (root != null)
		{
			vector *= root.GetPixelSizeAdjustment(Mathf.RoundToInt(vector.y));
		}
		return vector;
	}

	// Token: 0x0600368E RID: 13966 RVA: 0x00116F14 File Offset: 0x00115314
	public Vector2 GetViewSize()
	{
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			return new Vector2(this.mClipRange.z, this.mClipRange.w);
		}
		return NGUITools.screenSize;
	}

	// Token: 0x04001F52 RID: 8018
	public static List<UIPanel> list = new List<UIPanel>();

	// Token: 0x04001F53 RID: 8019
	public UIPanel.OnGeometryUpdated onGeometryUpdated;

	// Token: 0x04001F54 RID: 8020
	public bool showInPanelTool = true;

	// Token: 0x04001F55 RID: 8021
	public bool generateNormals;

	// Token: 0x04001F56 RID: 8022
	public bool widgetsAreStatic;

	// Token: 0x04001F57 RID: 8023
	public bool cullWhileDragging = true;

	// Token: 0x04001F58 RID: 8024
	public bool alwaysOnScreen;

	// Token: 0x04001F59 RID: 8025
	public bool anchorOffset;

	// Token: 0x04001F5A RID: 8026
	public bool softBorderPadding = true;

	// Token: 0x04001F5B RID: 8027
	public UIPanel.RenderQueue renderQueue;

	// Token: 0x04001F5C RID: 8028
	public int startingRenderQueue = 3000;

	// Token: 0x04001F5D RID: 8029
	[NonSerialized]
	public List<UIWidget> widgets = new List<UIWidget>();

	// Token: 0x04001F5E RID: 8030
	[NonSerialized]
	public List<UIDrawCall> drawCalls = new List<UIDrawCall>();

	// Token: 0x04001F5F RID: 8031
	[NonSerialized]
	public Matrix4x4 worldToLocal = Matrix4x4.identity;

	// Token: 0x04001F60 RID: 8032
	[NonSerialized]
	public Vector4 drawCallClipRange = new Vector4(0f, 0f, 1f, 1f);

	// Token: 0x04001F61 RID: 8033
	public UIPanel.OnClippingMoved onClipMove;

	// Token: 0x04001F62 RID: 8034
	[HideInInspector]
	[SerializeField]
	private Texture2D mClipTexture;

	// Token: 0x04001F63 RID: 8035
	[HideInInspector]
	[SerializeField]
	private float mAlpha = 1f;

	// Token: 0x04001F64 RID: 8036
	[HideInInspector]
	[SerializeField]
	private UIDrawCall.Clipping mClipping;

	// Token: 0x04001F65 RID: 8037
	[HideInInspector]
	[SerializeField]
	private Vector4 mClipRange = new Vector4(0f, 0f, 300f, 200f);

	// Token: 0x04001F66 RID: 8038
	[HideInInspector]
	[SerializeField]
	private Vector2 mClipSoftness = new Vector2(4f, 4f);

	// Token: 0x04001F67 RID: 8039
	[HideInInspector]
	[SerializeField]
	private int mDepth;

	// Token: 0x04001F68 RID: 8040
	[HideInInspector]
	[SerializeField]
	private int mSortingOrder;

	// Token: 0x04001F69 RID: 8041
	private bool mRebuild;

	// Token: 0x04001F6A RID: 8042
	private bool mResized;

	// Token: 0x04001F6B RID: 8043
	[SerializeField]
	private Vector2 mClipOffset = Vector2.zero;

	// Token: 0x04001F6C RID: 8044
	private float mCullTime;

	// Token: 0x04001F6D RID: 8045
	private float mUpdateTime;

	// Token: 0x04001F6E RID: 8046
	private int mMatrixFrame = -1;

	// Token: 0x04001F6F RID: 8047
	private int mAlphaFrameID;

	// Token: 0x04001F70 RID: 8048
	private int mLayer = -1;

	// Token: 0x04001F71 RID: 8049
	private static float[] mTemp = new float[4];

	// Token: 0x04001F72 RID: 8050
	private Vector2 mMin = Vector2.zero;

	// Token: 0x04001F73 RID: 8051
	private Vector2 mMax = Vector2.zero;

	// Token: 0x04001F74 RID: 8052
	private bool mHalfPixelOffset;

	// Token: 0x04001F75 RID: 8053
	private bool mSortWidgets;

	// Token: 0x04001F76 RID: 8054
	private bool mUpdateScroll;

	// Token: 0x04001F77 RID: 8055
	private UIPanel mParentPanel;

	// Token: 0x04001F78 RID: 8056
	private static Vector3[] mCorners = new Vector3[4];

	// Token: 0x04001F79 RID: 8057
	private static int mUpdateFrame = -1;

	// Token: 0x04001F7A RID: 8058
	private UIDrawCall.OnRenderCallback mOnRender;

	// Token: 0x04001F7B RID: 8059
	private bool mForced;

	// Token: 0x04001F7C RID: 8060
	[CompilerGenerated]
	private static Comparison<UIPanel> f__mg0;

	// Token: 0x04001F7D RID: 8061
	[CompilerGenerated]
	private static Comparison<UIPanel> f__mg1;

	// Token: 0x04001F7E RID: 8062
	[CompilerGenerated]
	private static Comparison<UIWidget> f__mg2;

	// Token: 0x02000650 RID: 1616
	public enum RenderQueue
	{
		// Token: 0x04001F80 RID: 8064
		Automatic,
		// Token: 0x04001F81 RID: 8065
		StartAt,
		// Token: 0x04001F82 RID: 8066
		Explicit
	}

	// Token: 0x02000651 RID: 1617
	// (Invoke) Token: 0x06003691 RID: 13969
	public delegate void OnGeometryUpdated();

	// Token: 0x02000652 RID: 1618
	// (Invoke) Token: 0x06003695 RID: 13973
	public delegate void OnClippingMoved(UIPanel panel);
}
