using System;
using UnityEngine;

// Token: 0x02000659 RID: 1625
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Stretch")]
public class UIStretch : MonoBehaviour
{
	// Token: 0x060036D4 RID: 14036 RVA: 0x001182E8 File Offset: 0x001166E8
	private void Awake()
	{
		this.mAnim = base.GetComponent<Animation>();
		this.mRect = default(Rect);
		this.mTrans = base.transform;
		this.mWidget = base.GetComponent<UIWidget>();
		this.mSprite = base.GetComponent<UISprite>();
		this.mPanel = base.GetComponent<UIPanel>();
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x00118360 File Offset: 0x00116760
	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x060036D6 RID: 14038 RVA: 0x00118382 File Offset: 0x00116782
	private void ScreenSizeChanged()
	{
		if (this.mStarted && this.runOnlyOnce)
		{
			this.Update();
		}
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x001183A0 File Offset: 0x001167A0
	private void Start()
	{
		if (this.container == null && this.widgetContainer != null)
		{
			this.container = this.widgetContainer.gameObject;
			this.widgetContainer = null;
		}
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		this.Update();
		this.mStarted = true;
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x0011842C File Offset: 0x0011682C
	private void Update()
	{
		if (this.mAnim != null && this.mAnim.isPlaying)
		{
			return;
		}
		if (this.style != UIStretch.Style.None)
		{
			UIWidget uiwidget = (!(this.container == null)) ? this.container.GetComponent<UIWidget>() : null;
			UIPanel uipanel = (!(this.container == null) || !(uiwidget == null)) ? this.container.GetComponent<UIPanel>() : null;
			float num = 1f;
			if (uiwidget != null)
			{
				Bounds bounds = uiwidget.CalculateBounds(base.transform.parent);
				this.mRect.x = bounds.min.x;
				this.mRect.y = bounds.min.y;
				this.mRect.width = bounds.size.x;
				this.mRect.height = bounds.size.y;
			}
			else if (uipanel != null)
			{
				if (uipanel.clipping == UIDrawCall.Clipping.None)
				{
					float num2 = (!(this.mRoot != null)) ? 0.5f : ((float)this.mRoot.activeHeight / (float)Screen.height * 0.5f);
					this.mRect.xMin = (float)(-(float)Screen.width) * num2;
					this.mRect.yMin = (float)(-(float)Screen.height) * num2;
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
				Transform parent = base.transform.parent;
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
				this.mRect = this.uiCamera.pixelRect;
				if (this.mRoot != null)
				{
					num = this.mRoot.pixelSizeAdjustment;
				}
			}
			float num3 = this.mRect.width;
			float num4 = this.mRect.height;
			if (num != 1f && num4 > 1f)
			{
				float num5 = (float)this.mRoot.activeHeight / num4;
				num3 *= num5;
				num4 *= num5;
			}
			Vector3 vector = (!(this.mWidget != null)) ? this.mTrans.localScale : new Vector3((float)this.mWidget.width, (float)this.mWidget.height);
			if (this.style == UIStretch.Style.BasedOnHeight)
			{
				vector.x = this.relativeSize.x * num4;
				vector.y = this.relativeSize.y * num4;
			}
			else if (this.style == UIStretch.Style.FillKeepingRatio)
			{
				float num6 = num3 / num4;
				float num7 = this.initialSize.x / this.initialSize.y;
				if (num7 < num6)
				{
					float num8 = num3 / this.initialSize.x;
					vector.x = num3;
					vector.y = this.initialSize.y * num8;
				}
				else
				{
					float num9 = num4 / this.initialSize.y;
					vector.x = this.initialSize.x * num9;
					vector.y = num4;
				}
			}
			else if (this.style == UIStretch.Style.FitInternalKeepingRatio)
			{
				float num10 = num3 / num4;
				float num11 = this.initialSize.x / this.initialSize.y;
				if (num11 > num10)
				{
					float num12 = num3 / this.initialSize.x;
					vector.x = num3;
					vector.y = this.initialSize.y * num12;
				}
				else
				{
					float num13 = num4 / this.initialSize.y;
					vector.x = this.initialSize.x * num13;
					vector.y = num4;
				}
			}
			else
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector.x = this.relativeSize.x * num3;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector.y = this.relativeSize.y * num4;
				}
			}
			if (this.mSprite != null)
			{
				float num14 = (!(this.mSprite.atlas != null)) ? 1f : this.mSprite.atlas.pixelSize;
				vector.x -= this.borderPadding.x * num14;
				vector.y -= this.borderPadding.y * num14;
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mSprite.width = Mathf.RoundToInt(vector.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mSprite.height = Mathf.RoundToInt(vector.y);
				}
				vector = Vector3.one;
			}
			else if (this.mWidget != null)
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					this.mWidget.width = Mathf.RoundToInt(vector.x - this.borderPadding.x);
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					this.mWidget.height = Mathf.RoundToInt(vector.y - this.borderPadding.y);
				}
				vector = Vector3.one;
			}
			else if (this.mPanel != null)
			{
				Vector4 baseClipRegion = this.mPanel.baseClipRegion;
				if (this.style != UIStretch.Style.Vertical)
				{
					baseClipRegion.z = vector.x - this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					baseClipRegion.w = vector.y - this.borderPadding.y;
				}
				this.mPanel.baseClipRegion = baseClipRegion;
				vector = Vector3.one;
			}
			else
			{
				if (this.style != UIStretch.Style.Vertical)
				{
					vector.x -= this.borderPadding.x;
				}
				if (this.style != UIStretch.Style.Horizontal)
				{
					vector.y -= this.borderPadding.y;
				}
			}
			if (this.mTrans.localScale != vector)
			{
				this.mTrans.localScale = vector;
			}
			if (this.runOnlyOnce && Application.isPlaying)
			{
				base.enabled = false;
			}
		}
	}

	// Token: 0x04001FB2 RID: 8114
	public Camera uiCamera;

	// Token: 0x04001FB3 RID: 8115
	public GameObject container;

	// Token: 0x04001FB4 RID: 8116
	public UIStretch.Style style;

	// Token: 0x04001FB5 RID: 8117
	public bool runOnlyOnce = true;

	// Token: 0x04001FB6 RID: 8118
	public Vector2 relativeSize = Vector2.one;

	// Token: 0x04001FB7 RID: 8119
	public Vector2 initialSize = Vector2.one;

	// Token: 0x04001FB8 RID: 8120
	public Vector2 borderPadding = Vector2.zero;

	// Token: 0x04001FB9 RID: 8121
	[HideInInspector]
	[SerializeField]
	private UIWidget widgetContainer;

	// Token: 0x04001FBA RID: 8122
	private Transform mTrans;

	// Token: 0x04001FBB RID: 8123
	private UIWidget mWidget;

	// Token: 0x04001FBC RID: 8124
	private UISprite mSprite;

	// Token: 0x04001FBD RID: 8125
	private UIPanel mPanel;

	// Token: 0x04001FBE RID: 8126
	private UIRoot mRoot;

	// Token: 0x04001FBF RID: 8127
	private Animation mAnim;

	// Token: 0x04001FC0 RID: 8128
	private Rect mRect;

	// Token: 0x04001FC1 RID: 8129
	private bool mStarted;

	// Token: 0x0200065A RID: 1626
	public enum Style
	{
		// Token: 0x04001FC3 RID: 8131
		None,
		// Token: 0x04001FC4 RID: 8132
		Horizontal,
		// Token: 0x04001FC5 RID: 8133
		Vertical,
		// Token: 0x04001FC6 RID: 8134
		Both,
		// Token: 0x04001FC7 RID: 8135
		BasedOnHeight,
		// Token: 0x04001FC8 RID: 8136
		FillKeepingRatio,
		// Token: 0x04001FC9 RID: 8137
		FitInternalKeepingRatio
	}
}
