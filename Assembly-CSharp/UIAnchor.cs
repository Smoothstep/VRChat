using System;
using UnityEngine;

// Token: 0x0200062B RID: 1579
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Anchor")]
public class UIAnchor : MonoBehaviour
{
	// Token: 0x060034EE RID: 13550 RVA: 0x0010AFBE File Offset: 0x001093BE
	private void Awake()
	{
		this.mTrans = base.transform;
		this.mAnim = base.GetComponent<Animation>();
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x060034EF RID: 13551 RVA: 0x0010AFF8 File Offset: 0x001093F8
	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x060034F0 RID: 13552 RVA: 0x0010B01A File Offset: 0x0010941A
	private void ScreenSizeChanged()
	{
		if (this.mStarted && this.runOnlyOnce)
		{
			this.Update();
		}
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x0010B038 File Offset: 0x00109438
	private void Start()
	{
		if (this.container == null && this.widgetContainer != null)
		{
			this.container = this.widgetContainer.gameObject;
			this.widgetContainer = null;
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.Update();
		this.mStarted = true;
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x0010B0C4 File Offset: 0x001094C4
	private void Update()
	{
		if (this.mAnim != null && this.mAnim.enabled && this.mAnim.isPlaying)
		{
			return;
		}
		bool flag = false;
		UIWidget uiwidget = (!(this.container == null)) ? this.container.GetComponent<UIWidget>() : null;
		UIPanel uipanel = (!(this.container == null) || !(uiwidget == null)) ? this.container.GetComponent<UIPanel>() : null;
		if (uiwidget != null)
		{
			Bounds bounds = uiwidget.CalculateBounds(this.container.transform.parent);
			this.mRect.x = bounds.min.x;
			this.mRect.y = bounds.min.y;
			this.mRect.width = bounds.size.x;
			this.mRect.height = bounds.size.y;
		}
		else if (uipanel != null)
		{
			if (uipanel.clipping == UIDrawCall.Clipping.None)
			{
				float num = (!(this.mRoot != null)) ? 0.5f : ((float)this.mRoot.activeHeight / (float)Screen.height * 0.5f);
				this.mRect.xMin = (float)(-(float)Screen.width) * num;
				this.mRect.yMin = (float)(-(float)Screen.height) * num;
				this.mRect.xMax = -this.mRect.xMin;
				this.mRect.yMax = -this.mRect.yMin;
			}
			else
			{
				Vector4 finalClipRegion = uipanel.finalClipRegion;
				this.mRect.x = finalClipRegion.x - finalClipRegion.z * 0.5f;
				this.mRect.y = finalClipRegion.y - finalClipRegion.w * 0.5f;
				this.mRect.width = finalClipRegion.z;
				this.mRect.height = finalClipRegion.w;
			}
		}
		else if (this.container != null)
		{
			Transform parent = this.container.transform.parent;
			Bounds bounds2 = (!(parent != null)) ? NGUIMath.CalculateRelativeWidgetBounds(this.container.transform) : NGUIMath.CalculateRelativeWidgetBounds(parent, this.container.transform);
			this.mRect.x = bounds2.min.x;
			this.mRect.y = bounds2.min.y;
			this.mRect.width = bounds2.size.x;
			this.mRect.height = bounds2.size.y;
		}
		else
		{
			if (!(this.uiCamera != null))
			{
				return;
			}
			flag = true;
			this.mRect = this.uiCamera.pixelRect;
		}
		float x = (this.mRect.xMin + this.mRect.xMax) * 0.5f;
		float y = (this.mRect.yMin + this.mRect.yMax) * 0.5f;
		Vector3 vector = new Vector3(x, y, 0f);
		if (this.side != UIAnchor.Side.Center)
		{
			if (this.side == UIAnchor.Side.Right || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.BottomRight)
			{
				vector.x = this.mRect.xMax;
			}
			else if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Bottom)
			{
				vector.x = x;
			}
			else
			{
				vector.x = this.mRect.xMin;
			}
			if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.TopLeft)
			{
				vector.y = this.mRect.yMax;
			}
			else if (this.side == UIAnchor.Side.Left || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Right)
			{
				vector.y = y;
			}
			else
			{
				vector.y = this.mRect.yMin;
			}
		}
		float width = this.mRect.width;
		float height = this.mRect.height;
		vector.x += this.pixelOffset.x + this.relativeOffset.x * width;
		vector.y += this.pixelOffset.y + this.relativeOffset.y * height;
		if (flag)
		{
			if (this.uiCamera.orthographic)
			{
				vector.x = Mathf.Round(vector.x);
				vector.y = Mathf.Round(vector.y);
			}
			vector.z = this.uiCamera.WorldToScreenPoint(this.mTrans.position).z;
			vector = this.uiCamera.ScreenToWorldPoint(vector);
		}
		else
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			if (uipanel != null)
			{
				vector = uipanel.cachedTransform.TransformPoint(vector);
			}
			else if (this.container != null)
			{
				Transform parent2 = this.container.transform.parent;
				if (parent2 != null)
				{
					vector = parent2.TransformPoint(vector);
				}
			}
			vector.z = this.mTrans.position.z;
		}
		if (this.mTrans.position != vector)
		{
			this.mTrans.position = vector;
		}
		if (this.runOnlyOnce && Application.isPlaying)
		{
			base.enabled = false;
		}
	}

	// Token: 0x04001E2D RID: 7725
	public Camera uiCamera;

	// Token: 0x04001E2E RID: 7726
	public GameObject container;

	// Token: 0x04001E2F RID: 7727
	public UIAnchor.Side side = UIAnchor.Side.Center;

	// Token: 0x04001E30 RID: 7728
	public bool runOnlyOnce = true;

	// Token: 0x04001E31 RID: 7729
	public Vector2 relativeOffset = Vector2.zero;

	// Token: 0x04001E32 RID: 7730
	public Vector2 pixelOffset = Vector2.zero;

	// Token: 0x04001E33 RID: 7731
	[HideInInspector]
	[SerializeField]
	private UIWidget widgetContainer;

	// Token: 0x04001E34 RID: 7732
	private Transform mTrans;

	// Token: 0x04001E35 RID: 7733
	private Animation mAnim;

	// Token: 0x04001E36 RID: 7734
	private Rect mRect = default(Rect);

	// Token: 0x04001E37 RID: 7735
	private UIRoot mRoot;

	// Token: 0x04001E38 RID: 7736
	private bool mStarted;

	// Token: 0x0200062C RID: 1580
	public enum Side
	{
		// Token: 0x04001E3A RID: 7738
		BottomLeft,
		// Token: 0x04001E3B RID: 7739
		Left,
		// Token: 0x04001E3C RID: 7740
		TopLeft,
		// Token: 0x04001E3D RID: 7741
		Top,
		// Token: 0x04001E3E RID: 7742
		TopRight,
		// Token: 0x04001E3F RID: 7743
		Right,
		// Token: 0x04001E40 RID: 7744
		BottomRight,
		// Token: 0x04001E41 RID: 7745
		Bottom,
		// Token: 0x04001E42 RID: 7746
		Center
	}
}
