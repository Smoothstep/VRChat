using System;
using UnityEngine;

// Token: 0x020005AD RID: 1453
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIDragObject : MonoBehaviour
{
	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x0600306D RID: 12397 RVA: 0x000ED474 File Offset: 0x000EB874
	// (set) Token: 0x0600306E RID: 12398 RVA: 0x000ED47C File Offset: 0x000EB87C
	public Vector3 dragMovement
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
		}
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x000ED488 File Offset: 0x000EB888
	private void OnEnable()
	{
		if (this.scrollWheelFactor != 0f)
		{
			this.scrollMomentum = this.scale * this.scrollWheelFactor;
			this.scrollWheelFactor = 0f;
		}
		if (this.contentRect == null && this.target != null && Application.isPlaying)
		{
			UIWidget component = this.target.GetComponent<UIWidget>();
			if (component != null)
			{
				this.contentRect = component;
			}
		}
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x000ED512 File Offset: 0x000EB912
	private void OnDisable()
	{
		this.mStarted = false;
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x000ED51C File Offset: 0x000EB91C
	private void FindPanel()
	{
		this.panelRegion = ((!(this.target != null)) ? null : UIPanel.Find(this.target.transform.parent));
		if (this.panelRegion == null)
		{
			this.restrictWithinPanel = false;
		}
	}

	// Token: 0x06003072 RID: 12402 RVA: 0x000ED574 File Offset: 0x000EB974
	private void UpdateBounds()
	{
		if (this.contentRect)
		{
			Transform cachedTransform = this.panelRegion.cachedTransform;
			Matrix4x4 worldToLocalMatrix = cachedTransform.worldToLocalMatrix;
			Vector3[] worldCorners = this.contentRect.worldCorners;
			for (int i = 0; i < 4; i++)
			{
				worldCorners[i] = worldToLocalMatrix.MultiplyPoint3x4(worldCorners[i]);
			}
			this.mBounds = new Bounds(worldCorners[0], Vector3.zero);
			for (int j = 1; j < 4; j++)
			{
				this.mBounds.Encapsulate(worldCorners[j]);
			}
		}
		else
		{
			this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.panelRegion.cachedTransform, this.target);
		}
	}

	// Token: 0x06003073 RID: 12403 RVA: 0x000ED64C File Offset: 0x000EBA4C
	private void OnPress(bool pressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			if (pressed)
			{
				if (!this.mPressed)
				{
					this.mTouchID = UICamera.currentTouchID;
					this.mPressed = true;
					this.mStarted = false;
					this.CancelMovement();
					if (this.restrictWithinPanel && this.panelRegion == null)
					{
						this.FindPanel();
					}
					if (this.restrictWithinPanel)
					{
						this.UpdateBounds();
					}
					this.CancelSpring();
					Transform transform = UICamera.currentCamera.transform;
					this.mPlane = new Plane(((!(this.panelRegion != null)) ? transform.rotation : this.panelRegion.cachedTransform.rotation) * Vector3.back, UICamera.lastWorldPosition);
				}
			}
			else if (this.mPressed && this.mTouchID == UICamera.currentTouchID)
			{
				this.mPressed = false;
				if (this.restrictWithinPanel && this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, false))
				{
					this.CancelMovement();
				}
			}
		}
	}

	// Token: 0x06003074 RID: 12404 RVA: 0x000ED7A0 File Offset: 0x000EBBA0
	private void OnDrag(Vector2 delta)
	{
		if (this.mPressed && this.mTouchID == UICamera.currentTouchID && base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float distance = 0f;
			if (this.mPlane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (!this.mStarted)
				{
					this.mStarted = true;
					vector = Vector3.zero;
				}
				if (vector.x != 0f || vector.y != 0f)
				{
					vector = this.target.InverseTransformDirection(vector);
					vector.Scale(this.scale);
					vector = this.target.TransformDirection(vector);
				}
				if (this.dragEffect != UIDragObject.DragEffect.None)
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				Vector3 localPosition = this.target.localPosition;
				this.Move(vector);
				if (this.restrictWithinPanel)
				{
					this.mBounds.center = this.mBounds.center + (this.target.localPosition - localPosition);
					if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
					{
						this.CancelMovement();
					}
				}
			}
		}
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x000ED96C File Offset: 0x000EBD6C
	private void Move(Vector3 worldDelta)
	{
		if (this.panelRegion != null)
		{
			this.mTargetPos += worldDelta;
			this.target.position = this.mTargetPos;
			Vector3 localPosition = this.target.localPosition;
			localPosition.x = Mathf.Round(localPosition.x);
			localPosition.y = Mathf.Round(localPosition.y);
			this.target.localPosition = localPosition;
			UIScrollView component = this.panelRegion.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
		else
		{
			this.target.position += worldDelta;
		}
	}

	// Token: 0x06003076 RID: 12406 RVA: 0x000EDA28 File Offset: 0x000EBE28
	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		this.mMomentum -= this.mScroll;
		this.mScroll = NGUIMath.SpringLerp(this.mScroll, Vector3.zero, 20f, deltaTime);
		if (this.mMomentum.magnitude < 0.0001f)
		{
			return;
		}
		if (!this.mPressed)
		{
			if (this.panelRegion == null)
			{
				this.FindPanel();
			}
			this.Move(NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime));
			if (this.restrictWithinPanel && this.panelRegion != null)
			{
				this.UpdateBounds();
				if (this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
				{
					this.CancelMovement();
				}
				else
				{
					this.CancelSpring();
				}
			}
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
			if (this.mMomentum.magnitude < 0.0001f)
			{
				this.CancelMovement();
			}
		}
		else
		{
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
		}
	}

	// Token: 0x06003077 RID: 12407 RVA: 0x000EDB6C File Offset: 0x000EBF6C
	public void CancelMovement()
	{
		if (this.target != null)
		{
			Vector3 localPosition = this.target.localPosition;
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			this.target.localPosition = localPosition;
		}
		this.mTargetPos = ((!(this.target != null)) ? Vector3.zero : this.target.position);
		this.mMomentum = Vector3.zero;
		this.mScroll = Vector3.zero;
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x000EDC20 File Offset: 0x000EC020
	public void CancelSpring()
	{
		SpringPosition component = this.target.GetComponent<SpringPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x000EDC4C File Offset: 0x000EC04C
	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.mScroll -= this.scrollMomentum * (delta * 0.05f);
		}
	}

	// Token: 0x04001AD4 RID: 6868
	public Transform target;

	// Token: 0x04001AD5 RID: 6869
	public UIPanel panelRegion;

	// Token: 0x04001AD6 RID: 6870
	public Vector3 scrollMomentum = Vector3.zero;

	// Token: 0x04001AD7 RID: 6871
	public bool restrictWithinPanel;

	// Token: 0x04001AD8 RID: 6872
	public UIRect contentRect;

	// Token: 0x04001AD9 RID: 6873
	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	// Token: 0x04001ADA RID: 6874
	public float momentumAmount = 35f;

	// Token: 0x04001ADB RID: 6875
	[SerializeField]
	protected Vector3 scale = new Vector3(1f, 1f, 0f);

	// Token: 0x04001ADC RID: 6876
	[SerializeField]
	[HideInInspector]
	private float scrollWheelFactor;

	// Token: 0x04001ADD RID: 6877
	private Plane mPlane;

	// Token: 0x04001ADE RID: 6878
	private Vector3 mTargetPos;

	// Token: 0x04001ADF RID: 6879
	private Vector3 mLastPos;

	// Token: 0x04001AE0 RID: 6880
	private Vector3 mMomentum = Vector3.zero;

	// Token: 0x04001AE1 RID: 6881
	private Vector3 mScroll = Vector3.zero;

	// Token: 0x04001AE2 RID: 6882
	private Bounds mBounds;

	// Token: 0x04001AE3 RID: 6883
	private int mTouchID;

	// Token: 0x04001AE4 RID: 6884
	private bool mStarted;

	// Token: 0x04001AE5 RID: 6885
	private bool mPressed;

	// Token: 0x020005AE RID: 1454
	public enum DragEffect
	{
		// Token: 0x04001AE7 RID: 6887
		None,
		// Token: 0x04001AE8 RID: 6888
		Momentum,
		// Token: 0x04001AE9 RID: 6889
		MomentumAndSpring
	}
}
