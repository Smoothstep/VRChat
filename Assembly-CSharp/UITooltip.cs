using System;
using UnityEngine;

// Token: 0x0200065F RID: 1631
[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	// Token: 0x170008A5 RID: 2213
	// (get) Token: 0x060036FD RID: 14077 RVA: 0x00119A3F File Offset: 0x00117E3F
	public static bool isVisible
	{
		get
		{
			return UITooltip.mInstance != null && UITooltip.mInstance.mTarget == 1f;
		}
	}

	// Token: 0x060036FE RID: 14078 RVA: 0x00119A65 File Offset: 0x00117E65
	private void Awake()
	{
		UITooltip.mInstance = this;
	}

	// Token: 0x060036FF RID: 14079 RVA: 0x00119A6D File Offset: 0x00117E6D
	private void OnDestroy()
	{
		UITooltip.mInstance = null;
	}

	// Token: 0x06003700 RID: 14080 RVA: 0x00119A78 File Offset: 0x00117E78
	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.mPos = this.mTrans.localPosition;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.SetAlpha(0f);
	}

	// Token: 0x06003701 RID: 14081 RVA: 0x00119AE0 File Offset: 0x00117EE0
	protected virtual void Update()
	{
		if (this.mHover != UICamera.hoveredObject)
		{
			this.mHover = null;
			this.mTarget = 0f;
		}
		if (this.mCurrent != this.mTarget)
		{
			this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, RealTime.deltaTime * this.appearSpeed);
			if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
			{
				this.mCurrent = this.mTarget;
			}
			this.SetAlpha(this.mCurrent * this.mCurrent);
			if (this.scalingTransitions)
			{
				Vector3 b = this.mSize * 0.25f;
				b.y = -b.y;
				Vector3 localScale = Vector3.one * (1.5f - this.mCurrent * 0.5f);
				Vector3 localPosition = Vector3.Lerp(this.mPos - b, this.mPos, this.mCurrent);
				this.mTrans.localPosition = localPosition;
				this.mTrans.localScale = localScale;
			}
		}
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x00119C04 File Offset: 0x00118004
	protected virtual void SetAlpha(float val)
	{
		int i = 0;
		int num = this.mWidgets.Length;
		while (i < num)
		{
			UIWidget uiwidget = this.mWidgets[i];
			Color color = uiwidget.color;
			color.a = val;
			uiwidget.color = color;
			i++;
		}
	}

	// Token: 0x06003703 RID: 14083 RVA: 0x00119C4C File Offset: 0x0011804C
	protected virtual void SetText(string tooltipText)
	{
		if (this.text != null && !string.IsNullOrEmpty(tooltipText))
		{
			this.mTarget = 1f;
			this.mHover = UICamera.hoveredObject;
			this.text.text = tooltipText;
			this.mPos = Input.mousePosition;
			Transform transform = this.text.transform;
			Vector3 localPosition = transform.localPosition;
			Vector3 localScale = transform.localScale;
			this.mSize = this.text.printedSize;
			this.mSize.x = this.mSize.x * localScale.x;
			this.mSize.y = this.mSize.y * localScale.y;
			if (this.background != null)
			{
				Vector4 border = this.background.border;
				this.mSize.x = this.mSize.x + (border.x + border.z + (localPosition.x - border.x) * 2f);
				this.mSize.y = this.mSize.y + (border.y + border.w + (-localPosition.y - border.y) * 2f);
				this.background.width = Mathf.RoundToInt(this.mSize.x);
				this.background.height = Mathf.RoundToInt(this.mSize.y);
			}
			if (this.uiCamera != null)
			{
				this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.width);
				this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.height);
				float num = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
				float num2 = (float)Screen.height * 0.5f / num;
				Vector2 vector = new Vector2(num2 * this.mSize.x / (float)Screen.width, num2 * this.mSize.y / (float)Screen.height);
				this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector.x);
				this.mPos.y = Mathf.Max(this.mPos.y, vector.y);
				this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
				this.mPos = this.mTrans.localPosition;
				this.mPos.x = Mathf.Round(this.mPos.x);
				this.mPos.y = Mathf.Round(this.mPos.y);
				this.mTrans.localPosition = this.mPos;
			}
			else
			{
				if (this.mPos.x + this.mSize.x > (float)Screen.width)
				{
					this.mPos.x = (float)Screen.width - this.mSize.x;
				}
				if (this.mPos.y - this.mSize.y < 0f)
				{
					this.mPos.y = this.mSize.y;
				}
				this.mPos.x = this.mPos.x - (float)Screen.width * 0.5f;
				this.mPos.y = this.mPos.y - (float)Screen.height * 0.5f;
			}
		}
		else
		{
			this.mHover = null;
			this.mTarget = 0f;
		}
	}

	// Token: 0x06003704 RID: 14084 RVA: 0x0011A010 File Offset: 0x00118410
	[Obsolete("Use UITooltip.Show instead")]
	public static void ShowText(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x0011A02D File Offset: 0x0011842D
	public static void Show(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	// Token: 0x06003706 RID: 14086 RVA: 0x0011A04A File Offset: 0x0011844A
	public static void Hide()
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.mHover = null;
			UITooltip.mInstance.mTarget = 0f;
		}
	}

	// Token: 0x04001FE0 RID: 8160
	protected static UITooltip mInstance;

	// Token: 0x04001FE1 RID: 8161
	public Camera uiCamera;

	// Token: 0x04001FE2 RID: 8162
	public UILabel text;

	// Token: 0x04001FE3 RID: 8163
	public UISprite background;

	// Token: 0x04001FE4 RID: 8164
	public float appearSpeed = 10f;

	// Token: 0x04001FE5 RID: 8165
	public bool scalingTransitions = true;

	// Token: 0x04001FE6 RID: 8166
	protected GameObject mHover;

	// Token: 0x04001FE7 RID: 8167
	protected Transform mTrans;

	// Token: 0x04001FE8 RID: 8168
	protected float mTarget;

	// Token: 0x04001FE9 RID: 8169
	protected float mCurrent;

	// Token: 0x04001FEA RID: 8170
	protected Vector3 mPos;

	// Token: 0x04001FEB RID: 8171
	protected Vector3 mSize = Vector3.zero;

	// Token: 0x04001FEC RID: 8172
	protected UIWidget[] mWidgets;
}
