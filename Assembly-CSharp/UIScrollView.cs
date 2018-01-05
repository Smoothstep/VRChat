using System;
using UnityEngine;

// Token: 0x020005CC RID: 1484
[ExecuteInEditMode]
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Interaction/Scroll View")]
public class UIScrollView : MonoBehaviour
{
	// Token: 0x17000768 RID: 1896
	// (get) Token: 0x0600315E RID: 12638 RVA: 0x000F346C File Offset: 0x000F186C
	public UIPanel panel
	{
		get
		{
			return this.mPanel;
		}
	}

	// Token: 0x17000769 RID: 1897
	// (get) Token: 0x0600315F RID: 12639 RVA: 0x000F3474 File Offset: 0x000F1874
	public bool isDragging
	{
		get
		{
			return this.mPressed && this.mDragStarted;
		}
	}

	// Token: 0x1700076A RID: 1898
	// (get) Token: 0x06003160 RID: 12640 RVA: 0x000F348A File Offset: 0x000F188A
	public virtual Bounds bounds
	{
		get
		{
			if (!this.mCalculatedBounds)
			{
				this.mCalculatedBounds = true;
				this.mTrans = base.transform;
				this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
			}
			return this.mBounds;
		}
	}

	// Token: 0x1700076B RID: 1899
	// (get) Token: 0x06003161 RID: 12641 RVA: 0x000F34C7 File Offset: 0x000F18C7
	public bool canMoveHorizontally
	{
		get
		{
			return this.movement == UIScrollView.Movement.Horizontal || this.movement == UIScrollView.Movement.Unrestricted || (this.movement == UIScrollView.Movement.Custom && this.customMovement.x != 0f);
		}
	}

	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x06003162 RID: 12642 RVA: 0x000F3508 File Offset: 0x000F1908
	public bool canMoveVertically
	{
		get
		{
			return this.movement == UIScrollView.Movement.Vertical || this.movement == UIScrollView.Movement.Unrestricted || (this.movement == UIScrollView.Movement.Custom && this.customMovement.y != 0f);
		}
	}

	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x06003163 RID: 12643 RVA: 0x000F3554 File Offset: 0x000F1954
	public virtual bool shouldMoveHorizontally
	{
		get
		{
			float num = this.bounds.size.x;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.x * 2f;
			}
			return Mathf.RoundToInt(num - this.mPanel.width) > 0;
		}
	}

	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x06003164 RID: 12644 RVA: 0x000F35BC File Offset: 0x000F19BC
	public virtual bool shouldMoveVertically
	{
		get
		{
			float num = this.bounds.size.y;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.y * 2f;
			}
			return Mathf.RoundToInt(num - this.mPanel.height) > 0;
		}
	}

	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x06003165 RID: 12645 RVA: 0x000F3624 File Offset: 0x000F1A24
	protected virtual bool shouldMove
	{
		get
		{
			if (!this.disableDragIfFits)
			{
				return true;
			}
			if (this.mPanel == null)
			{
				this.mPanel = base.GetComponent<UIPanel>();
			}
			Vector4 finalClipRegion = this.mPanel.finalClipRegion;
			Bounds bounds = this.bounds;
			float num = (finalClipRegion.z != 0f) ? (finalClipRegion.z * 0.5f) : ((float)Screen.width);
			float num2 = (finalClipRegion.w != 0f) ? (finalClipRegion.w * 0.5f) : ((float)Screen.height);
			if (this.canMoveHorizontally)
			{
				if (bounds.min.x < finalClipRegion.x - num)
				{
					return true;
				}
				if (bounds.max.x > finalClipRegion.x + num)
				{
					return true;
				}
			}
			if (this.canMoveVertically)
			{
				if (bounds.min.y < finalClipRegion.y - num2)
				{
					return true;
				}
				if (bounds.max.y > finalClipRegion.y + num2)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000770 RID: 1904
	// (get) Token: 0x06003166 RID: 12646 RVA: 0x000F3759 File Offset: 0x000F1B59
	// (set) Token: 0x06003167 RID: 12647 RVA: 0x000F3761 File Offset: 0x000F1B61
	public Vector3 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
			this.mShouldMove = true;
		}
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x000F3774 File Offset: 0x000F1B74
	private void Awake()
	{
		this.mTrans = base.transform;
		this.mPanel = base.GetComponent<UIPanel>();
		if (this.mPanel.clipping == UIDrawCall.Clipping.None)
		{
			this.mPanel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
		}
		if (this.movement != UIScrollView.Movement.Custom && this.scale.sqrMagnitude > 0.001f)
		{
			if (this.scale.x == 1f && this.scale.y == 0f)
			{
				this.movement = UIScrollView.Movement.Horizontal;
			}
			else if (this.scale.x == 0f && this.scale.y == 1f)
			{
				this.movement = UIScrollView.Movement.Vertical;
			}
			else if (this.scale.x == 1f && this.scale.y == 1f)
			{
				this.movement = UIScrollView.Movement.Unrestricted;
			}
			else
			{
				this.movement = UIScrollView.Movement.Custom;
				this.customMovement.x = this.scale.x;
				this.customMovement.y = this.scale.y;
			}
			this.scale = Vector3.zero;
		}
		if (this.contentPivot == UIWidget.Pivot.TopLeft && this.relativePositionOnReset != Vector2.zero)
		{
			this.contentPivot = NGUIMath.GetPivot(new Vector2(this.relativePositionOnReset.x, 1f - this.relativePositionOnReset.y));
			this.relativePositionOnReset = Vector2.zero;
		}
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x000F3910 File Offset: 0x000F1D10
	private void OnEnable()
	{
		UIScrollView.list.Add(this);
		if (Application.isPlaying)
		{
			if (this.horizontalScrollBar != null)
			{
				EventDelegate.Add(this.horizontalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
				this.horizontalScrollBar.alpha = ((this.showScrollBars != UIScrollView.ShowCondition.Always && !this.shouldMoveHorizontally) ? 0f : 1f);
			}
			if (this.verticalScrollBar != null)
			{
				EventDelegate.Add(this.verticalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
				this.verticalScrollBar.alpha = ((this.showScrollBars != UIScrollView.ShowCondition.Always && !this.shouldMoveVertically) ? 0f : 1f);
			}
		}
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x000F39EE File Offset: 0x000F1DEE
	private void OnDisable()
	{
		UIScrollView.list.Remove(this);
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x000F39FC File Offset: 0x000F1DFC
	public bool RestrictWithinBounds(bool instant)
	{
		return this.RestrictWithinBounds(instant, true, true);
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x000F3A08 File Offset: 0x000F1E08
	public bool RestrictWithinBounds(bool instant, bool horizontal, bool vertical)
	{
		Bounds bounds = this.bounds;
		Vector3 vector = this.mPanel.CalculateConstrainOffset(bounds.min, bounds.max);
		if (!horizontal)
		{
			vector.x = 0f;
		}
		if (!vertical)
		{
			vector.y = 0f;
		}
		if (vector.sqrMagnitude > 0.1f)
		{
			if (!instant && this.dragEffect == UIScrollView.DragEffect.MomentumAndSpring)
			{
				Vector3 pos = this.mTrans.localPosition + vector;
				pos.x = Mathf.Round(pos.x);
				pos.y = Mathf.Round(pos.y);
				SpringPanel.Begin(this.mPanel.gameObject, pos, 13f).strength = 8f;
			}
			else
			{
				this.MoveRelative(vector);
				if (Mathf.Abs(vector.x) > 0.01f)
				{
					this.mMomentum.x = 0f;
				}
				if (Mathf.Abs(vector.y) > 0.01f)
				{
					this.mMomentum.y = 0f;
				}
				if (Mathf.Abs(vector.z) > 0.01f)
				{
					this.mMomentum.z = 0f;
				}
				this.mScroll = 0f;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x000F3B6C File Offset: 0x000F1F6C
	public void DisableSpring()
	{
		SpringPanel component = base.GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x000F3B93 File Offset: 0x000F1F93
	public void UpdateScrollbars()
	{
		this.UpdateScrollbars(true);
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x000F3B9C File Offset: 0x000F1F9C
	public virtual void UpdateScrollbars(bool recalculateBounds)
	{
		if (this.mPanel == null)
		{
			return;
		}
		if (this.horizontalScrollBar != null || this.verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				this.mCalculatedBounds = false;
				this.mShouldMove = this.shouldMove;
			}
			Bounds bounds = this.bounds;
			Vector2 vector = bounds.min;
			Vector2 vector2 = bounds.max;
			if (this.horizontalScrollBar != null && vector2.x > vector.x)
			{
				Vector4 finalClipRegion = this.mPanel.finalClipRegion;
				int num = Mathf.RoundToInt(finalClipRegion.z);
				if ((num & 1) != 0)
				{
					num--;
				}
				float num2 = (float)num * 0.5f;
				num2 = Mathf.Round(num2);
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num2 -= this.mPanel.clipSoftness.x;
				}
				float contentSize = vector2.x - vector.x;
				float viewSize = num2 * 2f;
				float num3 = vector.x;
				float num4 = vector2.x;
				float num5 = finalClipRegion.x - num2;
				float num6 = finalClipRegion.x + num2;
				num3 = num5 - num3;
				num4 -= num6;
				this.UpdateScrollbars(this.horizontalScrollBar, num3, num4, contentSize, viewSize, false);
			}
			if (this.verticalScrollBar != null && vector2.y > vector.y)
			{
				Vector4 finalClipRegion2 = this.mPanel.finalClipRegion;
				int num7 = Mathf.RoundToInt(finalClipRegion2.w);
				if ((num7 & 1) != 0)
				{
					num7--;
				}
				float num8 = (float)num7 * 0.5f;
				num8 = Mathf.Round(num8);
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num8 -= this.mPanel.clipSoftness.y;
				}
				float contentSize2 = vector2.y - vector.y;
				float viewSize2 = num8 * 2f;
				float num9 = vector.y;
				float num10 = vector2.y;
				float num11 = finalClipRegion2.y - num8;
				float num12 = finalClipRegion2.y + num8;
				num9 = num11 - num9;
				num10 -= num12;
				this.UpdateScrollbars(this.verticalScrollBar, num9, num10, contentSize2, viewSize2, true);
			}
		}
		else if (recalculateBounds)
		{
			this.mCalculatedBounds = false;
		}
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x000F3E10 File Offset: 0x000F2210
	protected void UpdateScrollbars(UIProgressBar slider, float contentMin, float contentMax, float contentSize, float viewSize, bool inverted)
	{
		if (slider == null)
		{
			return;
		}
		this.mIgnoreCallbacks = true;
		float num;
		if (viewSize < contentSize)
		{
			contentMin = Mathf.Clamp01(contentMin / contentSize);
			contentMax = Mathf.Clamp01(contentMax / contentSize);
			num = contentMin + contentMax;
			slider.value = ((!inverted) ? ((num <= 0.001f) ? 1f : (contentMin / num)) : ((num <= 0.001f) ? 0f : (1f - contentMin / num)));
		}
		else
		{
			contentMin = Mathf.Clamp01(-contentMin / contentSize);
			contentMax = Mathf.Clamp01(-contentMax / contentSize);
			num = contentMin + contentMax;
			slider.value = ((!inverted) ? ((num <= 0.001f) ? 1f : (contentMin / num)) : ((num <= 0.001f) ? 0f : (1f - contentMin / num)));
			if (contentSize > 0f)
			{
				contentMin = Mathf.Clamp01(contentMin / contentSize);
				contentMax = Mathf.Clamp01(contentMax / contentSize);
				num = contentMin + contentMax;
			}
		}
		UIScrollBar uiscrollBar = slider as UIScrollBar;
		if (uiscrollBar != null)
		{
			uiscrollBar.barSize = 1f - num;
		}
		this.mIgnoreCallbacks = false;
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x000F3F54 File Offset: 0x000F2354
	public virtual void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		if (this.mPanel == null)
		{
			this.mPanel = base.GetComponent<UIPanel>();
		}
		this.DisableSpring();
		Bounds bounds = this.bounds;
		if (bounds.min.x == bounds.max.x || bounds.min.y == bounds.max.y)
		{
			return;
		}
		Vector4 finalClipRegion = this.mPanel.finalClipRegion;
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		float num3 = bounds.min.x + num;
		float num4 = bounds.max.x - num;
		float num5 = bounds.min.y + num2;
		float num6 = bounds.max.y - num2;
		if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			num3 -= this.mPanel.clipSoftness.x;
			num4 += this.mPanel.clipSoftness.x;
			num5 -= this.mPanel.clipSoftness.y;
			num6 += this.mPanel.clipSoftness.y;
		}
		float num7 = Mathf.Lerp(num3, num4, x);
		float num8 = Mathf.Lerp(num6, num5, y);
		if (!updateScrollbars)
		{
			Vector3 localPosition = this.mTrans.localPosition;
			if (this.canMoveHorizontally)
			{
				localPosition.x += finalClipRegion.x - num7;
			}
			if (this.canMoveVertically)
			{
				localPosition.y += finalClipRegion.y - num8;
			}
			this.mTrans.localPosition = localPosition;
		}
		if (this.canMoveHorizontally)
		{
			finalClipRegion.x = num7;
		}
		if (this.canMoveVertically)
		{
			finalClipRegion.y = num8;
		}
		Vector4 baseClipRegion = this.mPanel.baseClipRegion;
		this.mPanel.clipOffset = new Vector2(finalClipRegion.x - baseClipRegion.x, finalClipRegion.y - baseClipRegion.y);
		if (updateScrollbars)
		{
			this.UpdateScrollbars(this.mDragID == -10);
		}
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x000F41BE File Offset: 0x000F25BE
	public void InvalidateBounds()
	{
		this.mCalculatedBounds = false;
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x000F41C8 File Offset: 0x000F25C8
	[ContextMenu("Reset Clipping Position")]
	public void ResetPosition()
	{
		if (NGUITools.GetActive(this))
		{
			this.mCalculatedBounds = false;
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
			this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, false);
			this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, true);
		}
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x000F422C File Offset: 0x000F262C
	public void UpdatePosition()
	{
		if (!this.mIgnoreCallbacks && (this.horizontalScrollBar != null || this.verticalScrollBar != null))
		{
			this.mIgnoreCallbacks = true;
			this.mCalculatedBounds = false;
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
			float x = (!(this.horizontalScrollBar != null)) ? pivotOffset.x : this.horizontalScrollBar.value;
			float y = (!(this.verticalScrollBar != null)) ? (1f - pivotOffset.y) : this.verticalScrollBar.value;
			this.SetDragAmount(x, y, false);
			this.UpdateScrollbars(true);
			this.mIgnoreCallbacks = false;
		}
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x000F42F0 File Offset: 0x000F26F0
	public void OnScrollBar()
	{
		if (!this.mIgnoreCallbacks)
		{
			this.mIgnoreCallbacks = true;
			float x = (!(this.horizontalScrollBar != null)) ? 0f : this.horizontalScrollBar.value;
			float y = (!(this.verticalScrollBar != null)) ? 0f : this.verticalScrollBar.value;
			this.SetDragAmount(x, y, false);
			this.mIgnoreCallbacks = false;
		}
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x000F4370 File Offset: 0x000F2770
	public virtual void MoveRelative(Vector3 relative)
	{
		this.mTrans.localPosition += relative;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= relative.x;
		clipOffset.y -= relative.y;
		this.mPanel.clipOffset = clipOffset;
		this.UpdateScrollbars(false);
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x000F43E0 File Offset: 0x000F27E0
	public void MoveAbsolute(Vector3 absolute)
	{
		Vector3 a = this.mTrans.InverseTransformPoint(absolute);
		Vector3 b = this.mTrans.InverseTransformPoint(Vector3.zero);
		this.MoveRelative(a - b);
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x000F4418 File Offset: 0x000F2818
	public void Press(bool pressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		if (this.smoothDragStart && pressed)
		{
			this.mDragStarted = false;
			this.mDragStartOffset = Vector2.zero;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (!pressed && this.mDragID == UICamera.currentTouchID)
			{
				this.mDragID = -10;
			}
			this.mCalculatedBounds = false;
			this.mShouldMove = this.shouldMove;
			if (!this.mShouldMove)
			{
				return;
			}
			this.mPressed = pressed;
			if (pressed)
			{
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
				this.DisableSpring();
				this.mLastPos = UICamera.lastWorldPosition;
				this.mPlane = new Plane(this.mTrans.rotation * Vector3.back, this.mLastPos);
				Vector2 clipOffset = this.mPanel.clipOffset;
				clipOffset.x = Mathf.Round(clipOffset.x);
				clipOffset.y = Mathf.Round(clipOffset.y);
				this.mPanel.clipOffset = clipOffset;
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.x = Mathf.Round(localPosition.x);
				localPosition.y = Mathf.Round(localPosition.y);
				this.mTrans.localPosition = localPosition;
				if (!this.smoothDragStart)
				{
					this.mDragStarted = true;
					this.mDragStartOffset = Vector2.zero;
					if (this.onDragStarted != null)
					{
						this.onDragStarted();
					}
				}
			}
			else
			{
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					this.RestrictWithinBounds(this.dragEffect == UIScrollView.DragEffect.None, this.canMoveHorizontally, this.canMoveVertically);
				}
				if (this.mDragStarted && this.onDragFinished != null)
				{
					this.onDragFinished();
				}
				if (!this.mShouldMove && this.onStoppedMoving != null)
				{
					this.onStoppedMoving();
				}
			}
		}
	}

	// Token: 0x06003179 RID: 12665 RVA: 0x000F4634 File Offset: 0x000F2A34
	public void Drag()
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mShouldMove)
		{
			if (this.mDragID == -10)
			{
				this.mDragID = UICamera.currentTouchID;
			}
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			if (this.smoothDragStart && !this.mDragStarted)
			{
				this.mDragStarted = true;
				this.mDragStartOffset = UICamera.currentTouch.totalDelta;
				if (this.onDragStarted != null)
				{
					this.onDragStarted();
				}
			}
			Ray ray = (!this.smoothDragStart) ? UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos - this.mDragStartOffset);
			float distance = 0f;
			if (this.mPlane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (vector.x != 0f || vector.y != 0f || vector.z != 0f)
				{
					vector = this.mTrans.InverseTransformDirection(vector);
					if (this.movement == UIScrollView.Movement.Horizontal)
					{
						vector.y = 0f;
						vector.z = 0f;
					}
					else if (this.movement == UIScrollView.Movement.Vertical)
					{
						vector.x = 0f;
						vector.z = 0f;
					}
					else if (this.movement == UIScrollView.Movement.Unrestricted)
					{
						vector.z = 0f;
					}
					else
					{
						vector.Scale(this.customMovement);
					}
					vector = this.mTrans.TransformDirection(vector);
				}
				if (this.dragEffect == UIScrollView.DragEffect.None)
				{
					this.mMomentum = Vector3.zero;
				}
				else
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				if (!this.iOSDragEmulation || this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.MoveAbsolute(vector);
				}
				else if (this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max).magnitude > 1f)
				{
					this.MoveAbsolute(vector * 0.5f);
					this.mMomentum *= 0.5f;
				}
				else
				{
					this.MoveAbsolute(vector);
				}
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.RestrictWithinBounds(true, this.canMoveHorizontally, this.canMoveVertically);
				}
			}
		}
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x000F4950 File Offset: 0x000F2D50
	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.scrollWheelFactor != 0f)
		{
			this.DisableSpring();
			this.mShouldMove |= this.shouldMove;
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x000F49D8 File Offset: 0x000F2DD8
	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		if (this.showScrollBars != UIScrollView.ShowCondition.Always && (this.verticalScrollBar || this.horizontalScrollBar))
		{
			bool flag = false;
			bool flag2 = false;
			if (this.showScrollBars != UIScrollView.ShowCondition.WhenDragging || this.mDragID != -10 || this.mMomentum.magnitude > 0.01f)
			{
				flag = this.shouldMoveVertically;
				flag2 = this.shouldMoveHorizontally;
			}
			if (this.verticalScrollBar)
			{
				float num = this.verticalScrollBar.alpha;
				num += ((!flag) ? (-deltaTime * 3f) : (deltaTime * 6f));
				num = Mathf.Clamp01(num);
				if (this.verticalScrollBar.alpha != num)
				{
					this.verticalScrollBar.alpha = num;
				}
			}
			if (this.horizontalScrollBar)
			{
				float num2 = this.horizontalScrollBar.alpha;
				num2 += ((!flag2) ? (-deltaTime * 3f) : (deltaTime * 6f));
				num2 = Mathf.Clamp01(num2);
				if (this.horizontalScrollBar.alpha != num2)
				{
					this.horizontalScrollBar.alpha = num2;
				}
			}
		}
		if (!this.mShouldMove)
		{
			return;
		}
		if (!this.mPressed)
		{
			if (this.mMomentum.magnitude > 0.0001f || this.mScroll != 0f)
			{
				if (this.movement == UIScrollView.Movement.Horizontal)
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, 0f, 0f));
				}
				else if (this.movement == UIScrollView.Movement.Vertical)
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(0f, this.mScroll * 0.05f, 0f));
				}
				else if (this.movement == UIScrollView.Movement.Unrestricted)
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, this.mScroll * 0.05f, 0f));
				}
				else
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * this.customMovement.x * 0.05f, this.mScroll * this.customMovement.y * 0.05f, 0f));
				}
				this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
				Vector3 absolute = NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
				this.MoveAbsolute(absolute);
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					if (NGUITools.GetActive(this.centerOnChild))
					{
						if (this.centerOnChild.nextPageThreshold != 0f)
						{
							this.mMomentum = Vector3.zero;
							this.mScroll = 0f;
						}
						else
						{
							this.centerOnChild.Recenter();
						}
					}
					else
					{
						this.RestrictWithinBounds(false, this.canMoveHorizontally, this.canMoveVertically);
					}
				}
				if (this.onMomentumMove != null)
				{
					this.onMomentumMove();
				}
			}
			else
			{
				this.mScroll = 0f;
				this.mMomentum = Vector3.zero;
				SpringPanel component = base.GetComponent<SpringPanel>();
				if (component != null && component.enabled)
				{
					return;
				}
				this.mShouldMove = false;
				if (this.onStoppedMoving != null)
				{
					this.onStoppedMoving();
				}
			}
		}
		else
		{
			this.mScroll = 0f;
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
		}
	}

	// Token: 0x04001BDA RID: 7130
	public static BetterList<UIScrollView> list = new BetterList<UIScrollView>();

	// Token: 0x04001BDB RID: 7131
	public UIScrollView.Movement movement;

	// Token: 0x04001BDC RID: 7132
	public UIScrollView.DragEffect dragEffect = UIScrollView.DragEffect.MomentumAndSpring;

	// Token: 0x04001BDD RID: 7133
	public bool restrictWithinPanel = true;

	// Token: 0x04001BDE RID: 7134
	public bool disableDragIfFits;

	// Token: 0x04001BDF RID: 7135
	public bool smoothDragStart = true;

	// Token: 0x04001BE0 RID: 7136
	public bool iOSDragEmulation = true;

	// Token: 0x04001BE1 RID: 7137
	public float scrollWheelFactor = 0.25f;

	// Token: 0x04001BE2 RID: 7138
	public float momentumAmount = 35f;

	// Token: 0x04001BE3 RID: 7139
	public UIProgressBar horizontalScrollBar;

	// Token: 0x04001BE4 RID: 7140
	public UIProgressBar verticalScrollBar;

	// Token: 0x04001BE5 RID: 7141
	public UIScrollView.ShowCondition showScrollBars = UIScrollView.ShowCondition.OnlyIfNeeded;

	// Token: 0x04001BE6 RID: 7142
	public Vector2 customMovement = new Vector2(1f, 0f);

	// Token: 0x04001BE7 RID: 7143
	public UIWidget.Pivot contentPivot;

	// Token: 0x04001BE8 RID: 7144
	public UIScrollView.OnDragNotification onDragStarted;

	// Token: 0x04001BE9 RID: 7145
	public UIScrollView.OnDragNotification onDragFinished;

	// Token: 0x04001BEA RID: 7146
	public UIScrollView.OnDragNotification onMomentumMove;

	// Token: 0x04001BEB RID: 7147
	public UIScrollView.OnDragNotification onStoppedMoving;

	// Token: 0x04001BEC RID: 7148
	[HideInInspector]
	[SerializeField]
	private Vector3 scale = new Vector3(1f, 0f, 0f);

	// Token: 0x04001BED RID: 7149
	[SerializeField]
	[HideInInspector]
	private Vector2 relativePositionOnReset = Vector2.zero;

	// Token: 0x04001BEE RID: 7150
	protected Transform mTrans;

	// Token: 0x04001BEF RID: 7151
	protected UIPanel mPanel;

	// Token: 0x04001BF0 RID: 7152
	protected Plane mPlane;

	// Token: 0x04001BF1 RID: 7153
	protected Vector3 mLastPos;

	// Token: 0x04001BF2 RID: 7154
	protected bool mPressed;

	// Token: 0x04001BF3 RID: 7155
	protected Vector3 mMomentum = Vector3.zero;

	// Token: 0x04001BF4 RID: 7156
	protected float mScroll;

	// Token: 0x04001BF5 RID: 7157
	protected Bounds mBounds;

	// Token: 0x04001BF6 RID: 7158
	protected bool mCalculatedBounds;

	// Token: 0x04001BF7 RID: 7159
	protected bool mShouldMove;

	// Token: 0x04001BF8 RID: 7160
	protected bool mIgnoreCallbacks;

	// Token: 0x04001BF9 RID: 7161
	protected int mDragID = -10;

	// Token: 0x04001BFA RID: 7162
	protected Vector2 mDragStartOffset = Vector2.zero;

	// Token: 0x04001BFB RID: 7163
	protected bool mDragStarted;

	// Token: 0x04001BFC RID: 7164
	[HideInInspector]
	public UICenterOnChild centerOnChild;

	// Token: 0x020005CD RID: 1485
	public enum Movement
	{
		// Token: 0x04001BFE RID: 7166
		Horizontal,
		// Token: 0x04001BFF RID: 7167
		Vertical,
		// Token: 0x04001C00 RID: 7168
		Unrestricted,
		// Token: 0x04001C01 RID: 7169
		Custom
	}

	// Token: 0x020005CE RID: 1486
	public enum DragEffect
	{
		// Token: 0x04001C03 RID: 7171
		None,
		// Token: 0x04001C04 RID: 7172
		Momentum,
		// Token: 0x04001C05 RID: 7173
		MomentumAndSpring
	}

	// Token: 0x020005CF RID: 1487
	public enum ShowCondition
	{
		// Token: 0x04001C07 RID: 7175
		Always,
		// Token: 0x04001C08 RID: 7176
		OnlyIfNeeded,
		// Token: 0x04001C09 RID: 7177
		WhenDragging
	}

	// Token: 0x020005D0 RID: 1488
	// (Invoke) Token: 0x0600317E RID: 12670
	public delegate void OnDragNotification();
}
