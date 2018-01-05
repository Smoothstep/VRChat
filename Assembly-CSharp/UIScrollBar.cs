using System;
using UnityEngine;

// Token: 0x020005CA RID: 1482
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/NGUI Scroll Bar")]
public class UIScrollBar : UISlider
{
	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x06003155 RID: 12629 RVA: 0x000F2F33 File Offset: 0x000F1333
	// (set) Token: 0x06003156 RID: 12630 RVA: 0x000F2F3B File Offset: 0x000F133B
	[Obsolete("Use 'value' instead")]
	public float scrollValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x06003157 RID: 12631 RVA: 0x000F2F44 File Offset: 0x000F1344
	// (set) Token: 0x06003158 RID: 12632 RVA: 0x000F2F4C File Offset: 0x000F134C
	public float barSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mSize != num)
			{
				this.mSize = num;
				this.mIsDirty = true;
				if (NGUITools.GetActive(this))
				{
					if (UIProgressBar.current == null && this.onChange != null)
					{
						UIProgressBar.current = this;
						EventDelegate.Execute(this.onChange);
						UIProgressBar.current = null;
					}
					this.ForceUpdate();
				}
			}
		}
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x000F2FC0 File Offset: 0x000F13C0
	protected override void Upgrade()
	{
		if (this.mDir != UIScrollBar.Direction.Upgraded)
		{
			this.mValue = this.mScroll;
			if (this.mDir == UIScrollBar.Direction.Horizontal)
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			else
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.TopToBottom : UIProgressBar.FillDirection.BottomToTop);
			}
			this.mDir = UIScrollBar.Direction.Upgraded;
		}
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x000F302C File Offset: 0x000F142C
	protected override void OnStart()
	{
		base.OnStart();
		if (this.mFG != null && this.mFG.gameObject != base.gameObject)
		{
			if (!(this.mFG.GetComponent<Collider>() != null) && !(this.mFG.GetComponent<Collider2D>() != null))
			{
				return;
			}
			UIEventListener uieventListener = UIEventListener.Get(this.mFG.gameObject);
			UIEventListener uieventListener2 = uieventListener;
			uieventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onPress, new UIEventListener.BoolDelegate(base.OnPressForeground));
			UIEventListener uieventListener3 = uieventListener;
			uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener3.onDrag, new UIEventListener.VectorDelegate(base.OnDragForeground));
			this.mFG.autoResizeBoxCollider = true;
		}
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x000F3100 File Offset: 0x000F1500
	protected override float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return base.LocalToValue(localPos);
		}
		float num = Mathf.Clamp01(this.mSize) * 0.5f;
		float num2 = num;
		float num3 = 1f - num;
		Vector3[] localCorners = this.mFG.localCorners;
		if (base.isHorizontal)
		{
			num2 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, num2);
			num3 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, num3);
			float num4 = num3 - num2;
			if (num4 == 0f)
			{
				return base.value;
			}
			return (!base.isInverted) ? ((localPos.x - num2) / num4) : ((num3 - localPos.x) / num4);
		}
		else
		{
			num2 = Mathf.Lerp(localCorners[0].y, localCorners[1].y, num2);
			num3 = Mathf.Lerp(localCorners[3].y, localCorners[2].y, num3);
			float num5 = num3 - num2;
			if (num5 == 0f)
			{
				return base.value;
			}
			return (!base.isInverted) ? ((localPos.y - num2) / num5) : ((num3 - localPos.y) / num5);
		}
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x000F3258 File Offset: 0x000F1658
	public override void ForceUpdate()
	{
		if (this.mFG != null)
		{
			this.mIsDirty = false;
			float num = Mathf.Clamp01(this.mSize) * 0.5f;
			float num2 = Mathf.Lerp(num, 1f - num, base.value);
			float num3 = num2 - num;
			float num4 = num2 + num;
			if (base.isHorizontal)
			{
				this.mFG.drawRegion = ((!base.isInverted) ? new Vector4(num3, 0f, num4, 1f) : new Vector4(1f - num4, 0f, 1f - num3, 1f));
			}
			else
			{
				this.mFG.drawRegion = ((!base.isInverted) ? new Vector4(0f, num3, 1f, num4) : new Vector4(0f, 1f - num4, 1f, 1f - num3));
			}
			if (this.thumb != null)
			{
				Vector4 drawingDimensions = this.mFG.drawingDimensions;
				Vector3 position = new Vector3(Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, 0.5f), Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, 0.5f));
				base.SetThumbPosition(this.mFG.cachedTransform.TransformPoint(position));
			}
		}
		else
		{
			base.ForceUpdate();
		}
	}

	// Token: 0x04001BD3 RID: 7123
	[HideInInspector]
	[SerializeField]
	protected float mSize = 1f;

	// Token: 0x04001BD4 RID: 7124
	[HideInInspector]
	[SerializeField]
	private float mScroll;

	// Token: 0x04001BD5 RID: 7125
	[HideInInspector]
	[SerializeField]
	private UIScrollBar.Direction mDir = UIScrollBar.Direction.Upgraded;

	// Token: 0x020005CB RID: 1483
	private enum Direction
	{
		// Token: 0x04001BD7 RID: 7127
		Horizontal,
		// Token: 0x04001BD8 RID: 7128
		Vertical,
		// Token: 0x04001BD9 RID: 7129
		Upgraded
	}
}
