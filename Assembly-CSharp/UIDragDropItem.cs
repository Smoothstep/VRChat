using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005AA RID: 1450
[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
	// Token: 0x0600305C RID: 12380 RVA: 0x000E961D File Offset: 0x000E7A1D
	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mCollider = base.GetComponent<Collider>();
		this.mCollider2D = base.GetComponent<Collider2D>();
		this.mButton = base.GetComponent<UIButton>();
		this.mDragScrollView = base.GetComponent<UIDragScrollView>();
	}

	// Token: 0x0600305D RID: 12381 RVA: 0x000E965B File Offset: 0x000E7A5B
	protected void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			this.mDragStartTime = RealTime.time + this.pressAndHoldDelay;
			this.mPressed = true;
		}
		else
		{
			this.mPressed = false;
		}
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x000E9688 File Offset: 0x000E7A88
	protected virtual void Update()
	{
		if (this.restriction == UIDragDropItem.Restriction.PressAndHold && this.mPressed && !this.mDragging && this.mDragStartTime < RealTime.time)
		{
			this.StartDragging();
		}
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x000E96C4 File Offset: 0x000E7AC4
	protected void OnDragStart()
	{
		if (!base.enabled || this.mTouchID != -2147483648)
		{
			return;
		}
		if (this.restriction != UIDragDropItem.Restriction.None)
		{
			if (this.restriction == UIDragDropItem.Restriction.Horizontal)
			{
				Vector2 totalDelta = UICamera.currentTouch.totalDelta;
				if (Mathf.Abs(totalDelta.x) < Mathf.Abs(totalDelta.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.Vertical)
			{
				Vector2 totalDelta2 = UICamera.currentTouch.totalDelta;
				if (Mathf.Abs(totalDelta2.x) > Mathf.Abs(totalDelta2.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.PressAndHold)
			{
				return;
			}
		}
		this.StartDragging();
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x000E9780 File Offset: 0x000E7B80
	protected virtual void StartDragging()
	{
		if (!this.mDragging)
		{
			if (this.cloneOnDrag)
			{
				GameObject gameObject = NGUITools.AddChild(base.transform.parent.gameObject, base.gameObject);
				gameObject.transform.localPosition = base.transform.localPosition;
				gameObject.transform.localRotation = base.transform.localRotation;
				gameObject.transform.localScale = base.transform.localScale;
				UIButtonColor component = gameObject.GetComponent<UIButtonColor>();
				if (component != null)
				{
					component.defaultColor = base.GetComponent<UIButtonColor>().defaultColor;
				}
				UICamera.currentTouch.dragged = gameObject;
				UIDragDropItem component2 = gameObject.GetComponent<UIDragDropItem>();
				component2.mDragging = true;
				component2.Start();
				component2.OnDragDropStart();
			}
			else
			{
				this.mDragging = true;
				this.OnDragDropStart();
			}
		}
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x000E985C File Offset: 0x000E7C5C
	protected void OnDrag(Vector2 delta)
	{
		if (!this.mDragging || !base.enabled || this.mTouchID != UICamera.currentTouchID)
		{
			return;
		}
		this.OnDragDropMove(delta * this.mRoot.pixelSizeAdjustment);
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x000E989C File Offset: 0x000E7C9C
	protected void OnDragEnd()
	{
		if (!base.enabled || this.mTouchID != UICamera.currentTouchID)
		{
			return;
		}
		this.StopDragging(UICamera.hoveredObject);
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x000E98C5 File Offset: 0x000E7CC5
	public void StopDragging(GameObject go)
	{
		if (this.mDragging)
		{
			this.mDragging = false;
			this.OnDragDropRelease(go);
		}
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x000E98E0 File Offset: 0x000E7CE0
	protected virtual void OnDragDropStart()
	{
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = false;
		}
		if (this.mButton != null)
		{
			this.mButton.isEnabled = false;
		}
		else if (this.mCollider != null)
		{
			this.mCollider.enabled = false;
		}
		else if (this.mCollider2D != null)
		{
			this.mCollider2D.enabled = false;
		}
		this.mTouchID = UICamera.currentTouchID;
		this.mParent = this.mTrans.parent;
		this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
		this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
		this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
		if (UIDragDropRoot.root != null)
		{
			this.mTrans.parent = UIDragDropRoot.root;
		}
		Vector3 localPosition = this.mTrans.localPosition;
		localPosition.z = 0f;
		this.mTrans.localPosition = localPosition;
		TweenPosition component = base.GetComponent<TweenPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
		SpringPosition component2 = base.GetComponent<SpringPosition>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
		NGUITools.MarkParentAsChanged(base.gameObject);
		if (this.mTable != null)
		{
			this.mTable.repositionNow = true;
		}
		if (this.mGrid != null)
		{
			this.mGrid.repositionNow = true;
		}
	}

	// Token: 0x06003065 RID: 12389 RVA: 0x000E9A77 File Offset: 0x000E7E77
	protected virtual void OnDragDropMove(Vector2 delta)
	{
		this.mTrans.localPosition += new Vector3(delta.x, delta.y);
	}

	// Token: 0x06003066 RID: 12390 RVA: 0x000E9A98 File Offset: 0x000E7E98
	protected virtual void OnDragDropRelease(GameObject surface)
	{
		if (!this.cloneOnDrag)
		{
			this.mTouchID = int.MinValue;
			if (this.mButton != null)
			{
				this.mButton.isEnabled = true;
			}
			else if (this.mCollider != null)
			{
				this.mCollider.enabled = true;
			}
			else if (this.mCollider2D != null)
			{
				this.mCollider2D.enabled = true;
			}
			UIDragDropContainer uidragDropContainer = (!surface) ? null : NGUITools.FindInParents<UIDragDropContainer>(surface);
			if (uidragDropContainer != null)
			{
				this.mTrans.parent = ((!(uidragDropContainer.reparentTarget != null)) ? uidragDropContainer.transform : uidragDropContainer.reparentTarget);
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.z = 0f;
				this.mTrans.localPosition = localPosition;
			}
			else
			{
				this.mTrans.parent = this.mParent;
			}
			this.mParent = this.mTrans.parent;
			this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
			this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
			if (this.mDragScrollView != null)
			{
				base.StartCoroutine(this.EnableDragScrollView());
			}
			NGUITools.MarkParentAsChanged(base.gameObject);
			if (this.mTable != null)
			{
				this.mTable.repositionNow = true;
			}
			if (this.mGrid != null)
			{
				this.mGrid.repositionNow = true;
			}
			this.OnDragDropEnd();
		}
		else
		{
			NGUITools.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x000E9C53 File Offset: 0x000E8053
	protected virtual void OnDragDropEnd()
	{
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x000E9C58 File Offset: 0x000E8058
	protected IEnumerator EnableDragScrollView()
	{
		yield return new WaitForEndOfFrame();
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = true;
		}
		yield break;
	}

	// Token: 0x04001ABE RID: 6846
	public UIDragDropItem.Restriction restriction;

	// Token: 0x04001ABF RID: 6847
	public bool cloneOnDrag;

	// Token: 0x04001AC0 RID: 6848
	[HideInInspector]
	public float pressAndHoldDelay = 1f;

	// Token: 0x04001AC1 RID: 6849
	protected Transform mTrans;

	// Token: 0x04001AC2 RID: 6850
	protected Transform mParent;

	// Token: 0x04001AC3 RID: 6851
	protected Collider mCollider;

	// Token: 0x04001AC4 RID: 6852
	protected Collider2D mCollider2D;

	// Token: 0x04001AC5 RID: 6853
	protected UIButton mButton;

	// Token: 0x04001AC6 RID: 6854
	protected UIRoot mRoot;

	// Token: 0x04001AC7 RID: 6855
	protected UIGrid mGrid;

	// Token: 0x04001AC8 RID: 6856
	protected UITable mTable;

	// Token: 0x04001AC9 RID: 6857
	protected int mTouchID = int.MinValue;

	// Token: 0x04001ACA RID: 6858
	protected float mDragStartTime;

	// Token: 0x04001ACB RID: 6859
	protected UIDragScrollView mDragScrollView;

	// Token: 0x04001ACC RID: 6860
	protected bool mPressed;

	// Token: 0x04001ACD RID: 6861
	protected bool mDragging;

	// Token: 0x020005AB RID: 1451
	public enum Restriction
	{
		// Token: 0x04001ACF RID: 6863
		None,
		// Token: 0x04001AD0 RID: 6864
		Horizontal,
		// Token: 0x04001AD1 RID: 6865
		Vertical,
		// Token: 0x04001AD2 RID: 6866
		PressAndHold
	}
}
