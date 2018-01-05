using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005A5 RID: 1445
[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x06003045 RID: 12357 RVA: 0x000ECC13 File Offset: 0x000EB013
	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x000ECC1B File Offset: 0x000EB01B
	private void Start()
	{
		this.Recenter();
	}

	// Token: 0x06003047 RID: 12359 RVA: 0x000ECC23 File Offset: 0x000EB023
	private void OnEnable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = this;
			this.Recenter();
		}
	}

	// Token: 0x06003048 RID: 12360 RVA: 0x000ECC47 File Offset: 0x000EB047
	private void OnDisable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = null;
		}
	}

	// Token: 0x06003049 RID: 12361 RVA: 0x000ECC65 File Offset: 0x000EB065
	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	// Token: 0x0600304A RID: 12362 RVA: 0x000ECC78 File Offset: 0x000EB078
	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

	// Token: 0x0600304B RID: 12363 RVA: 0x000ECC8C File Offset: 0x000EB08C
	[ContextMenu("Execute")]
	public void Recenter()
	{
		if (this.mScrollView == null)
		{
			this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
			if (this.mScrollView == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					base.GetType(),
					" requires ",
					typeof(UIScrollView),
					" on a parent object in order to work"
				}), this);
				base.enabled = false;
				return;
			}
			if (this.mScrollView)
			{
				this.mScrollView.centerOnChild = this;
				UIScrollView uiscrollView = this.mScrollView;
				uiscrollView.onDragFinished = (UIScrollView.OnDragNotification)Delegate.Combine(uiscrollView.onDragFinished, new UIScrollView.OnDragNotification(this.OnDragFinished));
			}
			if (this.mScrollView.horizontalScrollBar != null)
			{
				UIProgressBar horizontalScrollBar = this.mScrollView.horizontalScrollBar;
				horizontalScrollBar.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(horizontalScrollBar.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
			}
			if (this.mScrollView.verticalScrollBar != null)
			{
				UIProgressBar verticalScrollBar = this.mScrollView.verticalScrollBar;
				verticalScrollBar.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(verticalScrollBar.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
			}
		}
		if (this.mScrollView.panel == null)
		{
			return;
		}
		Transform transform = base.transform;
		if (transform.childCount == 0)
		{
			return;
		}
		Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
		Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Vector3 vector2 = this.mScrollView.currentMomentum * this.mScrollView.momentumAmount;
		Vector3 a = NGUIMath.SpringDampen(ref vector2, 9f, 2f);
		Vector3 b = vector - a * 0.01f;
		float num = float.MaxValue;
		Transform target = null;
		int num2 = 0;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			Transform child = transform.GetChild(i);
			if (child.gameObject.activeInHierarchy)
			{
				float num3 = Vector3.SqrMagnitude(child.position - b);
				if (num3 < num)
				{
					num = num3;
					target = child;
					num2 = i;
				}
			}
			i++;
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null && this.mCenteredObject.transform == transform.GetChild(num2))
		{
			Vector2 totalDelta = UICamera.currentTouch.totalDelta;
			UIScrollView.Movement movement = this.mScrollView.movement;
			float num4;
			if (movement != UIScrollView.Movement.Horizontal)
			{
				if (movement != UIScrollView.Movement.Vertical)
				{
					num4 = totalDelta.magnitude;
				}
				else
				{
					num4 = totalDelta.y;
				}
			}
			else
			{
				num4 = totalDelta.x;
			}
			if (Mathf.Abs(num4) > this.nextPageThreshold)
			{
				UIGrid component = base.GetComponent<UIGrid>();
				if (component != null && component.sorting != UIGrid.Sorting.None)
				{
					List<Transform> childList = component.GetChildList();
					if (num4 > this.nextPageThreshold)
					{
						if (num2 > 0)
						{
							target = childList[num2 - 1];
						}
						else
						{
							target = childList[0];
						}
					}
					else if (num4 < -this.nextPageThreshold)
					{
						if (num2 < childList.Count - 1)
						{
							target = childList[num2 + 1];
						}
						else
						{
							target = childList[childList.Count - 1];
						}
					}
				}
				else
				{
					Debug.LogWarning("Next Page Threshold requires a sorted UIGrid in order to work properly", this);
				}
			}
		}
		this.CenterOn(target, vector);
	}

	// Token: 0x0600304C RID: 12364 RVA: 0x000ED064 File Offset: 0x000EB464
	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && this.mScrollView != null && this.mScrollView.panel != null)
		{
			Transform cachedTransform = this.mScrollView.panel.cachedTransform;
			this.mCenteredObject = target.gameObject;
			Vector3 a = cachedTransform.InverseTransformPoint(target.position);
			Vector3 b = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 b2 = a - b;
			if (!this.mScrollView.canMoveHorizontally)
			{
				b2.x = 0f;
			}
			if (!this.mScrollView.canMoveVertically)
			{
				b2.y = 0f;
			}
			b2.z = 0f;
			SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, cachedTransform.localPosition - b2, this.springStrength).onFinished = this.onFinished;
		}
		else
		{
			this.mCenteredObject = null;
		}
		if (this.onCenter != null)
		{
			this.onCenter(this.mCenteredObject);
		}
	}

	// Token: 0x0600304D RID: 12365 RVA: 0x000ED17C File Offset: 0x000EB57C
	public void CenterOn(Transform target)
	{
		if (this.mScrollView != null && this.mScrollView.panel != null)
		{
			Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
			Vector3 panelCenter = (worldCorners[2] + worldCorners[0]) * 0.5f;
			this.CenterOn(target, panelCenter);
		}
	}

	// Token: 0x04001AB6 RID: 6838
	public float springStrength = 8f;

	// Token: 0x04001AB7 RID: 6839
	public float nextPageThreshold;

	// Token: 0x04001AB8 RID: 6840
	public SpringPanel.OnFinished onFinished;

	// Token: 0x04001AB9 RID: 6841
	public UICenterOnChild.OnCenterCallback onCenter;

	// Token: 0x04001ABA RID: 6842
	private UIScrollView mScrollView;

	// Token: 0x04001ABB RID: 6843
	private GameObject mCenteredObject;

	// Token: 0x020005A6 RID: 1446
	// (Invoke) Token: 0x0600304F RID: 12367
	public delegate void OnCenterCallback(GameObject centeredObject);
}
