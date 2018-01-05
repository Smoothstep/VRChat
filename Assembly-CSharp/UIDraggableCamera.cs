using System;
using UnityEngine;

// Token: 0x020005B1 RID: 1457
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/Interaction/Draggable Camera")]
public class UIDraggableCamera : MonoBehaviour
{
	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x06003086 RID: 12422 RVA: 0x000EE0BE File Offset: 0x000EC4BE
	// (set) Token: 0x06003087 RID: 12423 RVA: 0x000EE0C6 File Offset: 0x000EC4C6
	public Vector2 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
		}
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x000EE0D0 File Offset: 0x000EC4D0
	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		this.mTrans = base.transform;
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		if (this.rootForBounds == null)
		{
			Debug.LogError(NGUITools.GetHierarchy(base.gameObject) + " needs the 'Root For Bounds' parameter to be set", this);
			base.enabled = false;
		}
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x000EE13C File Offset: 0x000EC53C
	private Vector3 CalculateConstrainOffset()
	{
		if (this.rootForBounds == null || this.rootForBounds.childCount == 0)
		{
			return Vector3.zero;
		}
		Vector3 vector = new Vector3(this.mCam.rect.xMin * (float)Screen.width, this.mCam.rect.yMin * (float)Screen.height, 0f);
		Vector3 vector2 = new Vector3(this.mCam.rect.xMax * (float)Screen.width, this.mCam.rect.yMax * (float)Screen.height, 0f);
		vector = this.mCam.ScreenToWorldPoint(vector);
		vector2 = this.mCam.ScreenToWorldPoint(vector2);
		Vector2 minRect = new Vector2(this.mBounds.min.x, this.mBounds.min.y);
		Vector2 maxRect = new Vector2(this.mBounds.max.x, this.mBounds.max.y);
		return NGUIMath.ConstrainRect(minRect, maxRect, vector, vector2);
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x000EE284 File Offset: 0x000EC684
	public bool ConstrainToBounds(bool immediate)
	{
		if (this.mTrans != null && this.rootForBounds != null)
		{
			Vector3 b = this.CalculateConstrainOffset();
			if (b.sqrMagnitude > 0f)
			{
				if (immediate)
				{
					this.mTrans.position -= b;
				}
				else
				{
					SpringPosition springPosition = SpringPosition.Begin(base.gameObject, this.mTrans.position - b, 13f);
					springPosition.ignoreTimeScale = true;
					springPosition.worldSpace = true;
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x000EE320 File Offset: 0x000EC720
	public void Press(bool isPressed)
	{
		if (isPressed)
		{
			this.mDragStarted = false;
		}
		if (this.rootForBounds != null)
		{
			this.mPressed = isPressed;
			if (isPressed)
			{
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				this.mMomentum = Vector2.zero;
				this.mScroll = 0f;
				SpringPosition component = base.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else if (this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
			{
				this.ConstrainToBounds(false);
			}
		}
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x000EE3B4 File Offset: 0x000EC7B4
	public void Drag(Vector2 delta)
	{
		if (this.smoothDragStart && !this.mDragStarted)
		{
			this.mDragStarted = true;
			return;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		if (this.mRoot != null)
		{
			delta *= this.mRoot.pixelSizeAdjustment;
		}
		Vector3 vector = Vector2.Scale(delta, -this.scale);
		this.mTrans.localPosition += vector;
		this.mMomentum = Vector2.Lerp(this.mMomentum, this.mMomentum + new Vector2(vector.x, vector.y) * (0.01f * this.momentumAmount), 0.67f);
		if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.ConstrainToBounds(true))
		{
			this.mMomentum = Vector2.zero;
			this.mScroll = 0f;
		}
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x000EE4A0 File Offset: 0x000EC8A0
	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x000EE500 File Offset: 0x000EC900
	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		if (this.mPressed)
		{
			SpringPosition component = base.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
			this.mScroll = 0f;
		}
		else
		{
			this.mMomentum += this.scale * (this.mScroll * 20f);
			this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
			if (this.mMomentum.magnitude > 0.01f)
			{
                Vector2 v = NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);

                this.mTrans.localPosition += new Vector3(v.x, v.y);
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				if (!this.ConstrainToBounds(this.dragEffect == UIDragObject.DragEffect.None))
				{
					SpringPosition component2 = base.GetComponent<SpringPosition>();
					if (component2 != null)
					{
						component2.enabled = false;
					}
				}
				return;
			}
			this.mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
	}

	// Token: 0x04001AFC RID: 6908
	public Transform rootForBounds;

	// Token: 0x04001AFD RID: 6909
	public Vector2 scale = Vector2.one;

	// Token: 0x04001AFE RID: 6910
	public float scrollWheelFactor;

	// Token: 0x04001AFF RID: 6911
	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	// Token: 0x04001B00 RID: 6912
	public bool smoothDragStart = true;

	// Token: 0x04001B01 RID: 6913
	public float momentumAmount = 35f;

	// Token: 0x04001B02 RID: 6914
	private Camera mCam;

	// Token: 0x04001B03 RID: 6915
	private Transform mTrans;

	// Token: 0x04001B04 RID: 6916
	private bool mPressed;

	// Token: 0x04001B05 RID: 6917
	private Vector2 mMomentum = Vector2.zero;

	// Token: 0x04001B06 RID: 6918
	private Bounds mBounds;

	// Token: 0x04001B07 RID: 6919
	private float mScroll;

	// Token: 0x04001B08 RID: 6920
	private UIRoot mRoot;

	// Token: 0x04001B09 RID: 6921
	private bool mDragStarted;
}
