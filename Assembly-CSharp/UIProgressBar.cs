using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005C6 RID: 1478
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/NGUI Progress Bar")]
public class UIProgressBar : UIWidgetContainer
{
	// Token: 0x1700075C RID: 1884
	// (get) Token: 0x06003131 RID: 12593 RVA: 0x000F1BEA File Offset: 0x000EFFEA
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x06003132 RID: 12594 RVA: 0x000F1C0F File Offset: 0x000F000F
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			return this.mCam;
		}
	}

	// Token: 0x1700075E RID: 1886
	// (get) Token: 0x06003133 RID: 12595 RVA: 0x000F1C3E File Offset: 0x000F003E
	// (set) Token: 0x06003134 RID: 12596 RVA: 0x000F1C46 File Offset: 0x000F0046
	public UIWidget foregroundWidget
	{
		get
		{
			return this.mFG;
		}
		set
		{
			if (this.mFG != value)
			{
				this.mFG = value;
				this.mIsDirty = true;
			}
		}
	}

	// Token: 0x1700075F RID: 1887
	// (get) Token: 0x06003135 RID: 12597 RVA: 0x000F1C67 File Offset: 0x000F0067
	// (set) Token: 0x06003136 RID: 12598 RVA: 0x000F1C6F File Offset: 0x000F006F
	public UIWidget backgroundWidget
	{
		get
		{
			return this.mBG;
		}
		set
		{
			if (this.mBG != value)
			{
				this.mBG = value;
				this.mIsDirty = true;
			}
		}
	}

	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x06003137 RID: 12599 RVA: 0x000F1C90 File Offset: 0x000F0090
	// (set) Token: 0x06003138 RID: 12600 RVA: 0x000F1C98 File Offset: 0x000F0098
	public UIProgressBar.FillDirection fillDirection
	{
		get
		{
			return this.mFill;
		}
		set
		{
			if (this.mFill != value)
			{
				this.mFill = value;
				this.ForceUpdate();
			}
		}
	}

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x06003139 RID: 12601 RVA: 0x000F1CB3 File Offset: 0x000F00B3
	// (set) Token: 0x0600313A RID: 12602 RVA: 0x000F1CE8 File Offset: 0x000F00E8
	public float value
	{
		get
		{
			if (this.numberOfSteps > 1)
			{
				return Mathf.Round(this.mValue * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
			}
			return this.mValue;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mValue != num)
			{
				float value2 = this.value;
				this.mValue = num;
				if (value2 != this.value)
				{
					this.ForceUpdate();
					if (UIProgressBar.current == null && NGUITools.GetActive(this) && EventDelegate.IsValid(this.onChange))
					{
						UIProgressBar.current = this;
						EventDelegate.Execute(this.onChange);
						UIProgressBar.current = null;
					}
				}
			}
		}
	}

	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x0600313B RID: 12603 RVA: 0x000F1D6C File Offset: 0x000F016C
	// (set) Token: 0x0600313C RID: 12604 RVA: 0x000F1DB8 File Offset: 0x000F01B8
	public float alpha
	{
		get
		{
			if (this.mFG != null)
			{
				return this.mFG.alpha;
			}
			if (this.mBG != null)
			{
				return this.mBG.alpha;
			}
			return 1f;
		}
		set
		{
			if (this.mFG != null)
			{
				this.mFG.alpha = value;
				if (this.mFG.GetComponent<Collider>() != null)
				{
					this.mFG.GetComponent<Collider>().enabled = (this.mFG.alpha > 0.001f);
				}
				else if (this.mFG.GetComponent<Collider2D>() != null)
				{
					this.mFG.GetComponent<Collider2D>().enabled = (this.mFG.alpha > 0.001f);
				}
			}
			if (this.mBG != null)
			{
				this.mBG.alpha = value;
				if (this.mBG.GetComponent<Collider>() != null)
				{
					this.mBG.GetComponent<Collider>().enabled = (this.mBG.alpha > 0.001f);
				}
				else if (this.mBG.GetComponent<Collider2D>() != null)
				{
					this.mBG.GetComponent<Collider2D>().enabled = (this.mBG.alpha > 0.001f);
				}
			}
			if (this.thumb != null)
			{
				UIWidget component = this.thumb.GetComponent<UIWidget>();
				if (component != null)
				{
					component.alpha = value;
					if (component.GetComponent<Collider>() != null)
					{
						component.GetComponent<Collider>().enabled = (component.alpha > 0.001f);
					}
					else if (component.GetComponent<Collider2D>() != null)
					{
						component.GetComponent<Collider2D>().enabled = (component.alpha > 0.001f);
					}
				}
			}
		}
	}

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x0600313D RID: 12605 RVA: 0x000F1F70 File Offset: 0x000F0370
	protected bool isHorizontal
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.LeftToRight || this.mFill == UIProgressBar.FillDirection.RightToLeft;
		}
	}

	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x0600313E RID: 12606 RVA: 0x000F1F89 File Offset: 0x000F0389
	protected bool isInverted
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.RightToLeft || this.mFill == UIProgressBar.FillDirection.TopToBottom;
		}
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x000F1FA4 File Offset: 0x000F03A4
	protected void Start()
	{
		this.Upgrade();
		if (Application.isPlaying)
		{
			if (this.mBG != null)
			{
				this.mBG.autoResizeBoxCollider = true;
			}
			this.OnStart();
			if (UIProgressBar.current == null && this.onChange != null)
			{
				UIProgressBar.current = this;
				EventDelegate.Execute(this.onChange);
				UIProgressBar.current = null;
			}
		}
		this.ForceUpdate();
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x000F201C File Offset: 0x000F041C
	protected virtual void Upgrade()
	{
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x000F201E File Offset: 0x000F041E
	protected virtual void OnStart()
	{
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x000F2020 File Offset: 0x000F0420
	protected void Update()
	{
		if (this.mIsDirty)
		{
			this.ForceUpdate();
		}
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x000F2034 File Offset: 0x000F0434
	protected void OnValidate()
	{
		if (NGUITools.GetActive(this))
		{
			this.Upgrade();
			this.mIsDirty = true;
			float num = Mathf.Clamp01(this.mValue);
			if (this.mValue != num)
			{
				this.mValue = num;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 20)
			{
				this.numberOfSteps = 20;
			}
			this.ForceUpdate();
		}
		else
		{
			float num2 = Mathf.Clamp01(this.mValue);
			if (this.mValue != num2)
			{
				this.mValue = num2;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 20)
			{
				this.numberOfSteps = 20;
			}
		}
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x000F20FC File Offset: 0x000F04FC
	protected float ScreenToValue(Vector2 screenPos)
	{
		Transform cachedTransform = this.cachedTransform;
		Plane plane = new Plane(cachedTransform.rotation * Vector3.back, cachedTransform.position);
		Ray ray = this.cachedCamera.ScreenPointToRay(screenPos);
		float distance;
		if (!plane.Raycast(ray, out distance))
		{
			return this.value;
		}
		return this.LocalToValue(cachedTransform.InverseTransformPoint(ray.GetPoint(distance)));
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x000F2170 File Offset: 0x000F0570
	protected virtual float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return this.value;
		}
		Vector3[] localCorners = this.mFG.localCorners;
		Vector3 vector = localCorners[2] - localCorners[0];
		if (this.isHorizontal)
		{
			float num = (localPos.x - localCorners[0].x) / vector.x;
			return (!this.isInverted) ? num : (1f - num);
		}
		float num2 = (localPos.y - localCorners[0].y) / vector.y;
		return (!this.isInverted) ? num2 : (1f - num2);
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x000F2238 File Offset: 0x000F0638
	public virtual void ForceUpdate()
	{
		this.mIsDirty = false;
		if (this.mFG != null)
		{
			UIBasicSprite uibasicSprite = this.mFG as UIBasicSprite;
			if (this.isHorizontal)
			{
				if (uibasicSprite != null && uibasicSprite.type == UIBasicSprite.Type.Filled)
				{
					if (uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
					{
						uibasicSprite.fillDirection = UIBasicSprite.FillDirection.Horizontal;
						uibasicSprite.invert = this.isInverted;
					}
					uibasicSprite.fillAmount = this.value;
				}
				else
				{
					this.mFG.drawRegion = ((!this.isInverted) ? new Vector4(0f, 0f, this.value, 1f) : new Vector4(1f - this.value, 0f, 1f, 1f));
					this.mFG.enabled = (this.value > 0.001f);
				}
			}
			else if (uibasicSprite != null && uibasicSprite.type == UIBasicSprite.Type.Filled)
			{
				if (uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
				{
					uibasicSprite.fillDirection = UIBasicSprite.FillDirection.Vertical;
					uibasicSprite.invert = this.isInverted;
				}
				uibasicSprite.fillAmount = this.value;
			}
			else
			{
				this.mFG.drawRegion = ((!this.isInverted) ? new Vector4(0f, 0f, 1f, this.value) : new Vector4(0f, 1f - this.value, 1f, 1f));
				this.mFG.enabled = (this.value > 0.001f);
			}
		}
		if (this.thumb != null && (this.mFG != null || this.mBG != null))
		{
			Vector3[] array = (!(this.mFG != null)) ? this.mBG.localCorners : this.mFG.localCorners;
			Vector4 vector = (!(this.mFG != null)) ? this.mBG.border : this.mFG.border;
			Vector3[] array2 = array;
			int num = 0;
			array2[num].x = array2[num].x + vector.x;
			Vector3[] array3 = array;
			int num2 = 1;
			array3[num2].x = array3[num2].x + vector.x;
			Vector3[] array4 = array;
			int num3 = 2;
			array4[num3].x = array4[num3].x - vector.z;
			Vector3[] array5 = array;
			int num4 = 3;
			array5[num4].x = array5[num4].x - vector.z;
			Vector3[] array6 = array;
			int num5 = 0;
			array6[num5].y = array6[num5].y + vector.y;
			Vector3[] array7 = array;
			int num6 = 1;
			array7[num6].y = array7[num6].y - vector.w;
			Vector3[] array8 = array;
			int num7 = 2;
			array8[num7].y = array8[num7].y - vector.w;
			Vector3[] array9 = array;
			int num8 = 3;
			array9[num8].y = array9[num8].y + vector.y;
			Transform transform = (!(this.mFG != null)) ? this.mBG.cachedTransform : this.mFG.cachedTransform;
			for (int i = 0; i < 4; i++)
			{
				array[i] = transform.TransformPoint(array[i]);
			}
			if (this.isHorizontal)
			{
				Vector3 a = Vector3.Lerp(array[0], array[1], 0.5f);
				Vector3 b = Vector3.Lerp(array[2], array[3], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(a, b, (!this.isInverted) ? this.value : (1f - this.value)));
			}
			else
			{
				Vector3 a2 = Vector3.Lerp(array[0], array[3], 0.5f);
				Vector3 b2 = Vector3.Lerp(array[1], array[2], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(a2, b2, (!this.isInverted) ? this.value : (1f - this.value)));
			}
		}
	}

	// Token: 0x06003147 RID: 12615 RVA: 0x000F26BC File Offset: 0x000F0ABC
	protected void SetThumbPosition(Vector3 worldPos)
	{
		Transform parent = this.thumb.parent;
		if (parent != null)
		{
			worldPos = parent.InverseTransformPoint(worldPos);
			worldPos.x = Mathf.Round(worldPos.x);
			worldPos.y = Mathf.Round(worldPos.y);
			worldPos.z = 0f;
			if (Vector3.Distance(this.thumb.localPosition, worldPos) > 0.001f)
			{
				this.thumb.localPosition = worldPos;
			}
		}
		else if (Vector3.Distance(this.thumb.position, worldPos) > 1E-05f)
		{
			this.thumb.position = worldPos;
		}
	}

	// Token: 0x04001BBD RID: 7101
	public static UIProgressBar current;

	// Token: 0x04001BBE RID: 7102
	public UIProgressBar.OnDragFinished onDragFinished;

	// Token: 0x04001BBF RID: 7103
	public Transform thumb;

	// Token: 0x04001BC0 RID: 7104
	[HideInInspector]
	[SerializeField]
	protected UIWidget mBG;

	// Token: 0x04001BC1 RID: 7105
	[HideInInspector]
	[SerializeField]
	protected UIWidget mFG;

	// Token: 0x04001BC2 RID: 7106
	[HideInInspector]
	[SerializeField]
	protected float mValue = 1f;

	// Token: 0x04001BC3 RID: 7107
	[HideInInspector]
	[SerializeField]
	protected UIProgressBar.FillDirection mFill;

	// Token: 0x04001BC4 RID: 7108
	protected Transform mTrans;

	// Token: 0x04001BC5 RID: 7109
	protected bool mIsDirty;

	// Token: 0x04001BC6 RID: 7110
	protected Camera mCam;

	// Token: 0x04001BC7 RID: 7111
	protected float mOffset;

	// Token: 0x04001BC8 RID: 7112
	public int numberOfSteps;

	// Token: 0x04001BC9 RID: 7113
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x020005C7 RID: 1479
	public enum FillDirection
	{
		// Token: 0x04001BCB RID: 7115
		LeftToRight,
		// Token: 0x04001BCC RID: 7116
		RightToLeft,
		// Token: 0x04001BCD RID: 7117
		BottomToTop,
		// Token: 0x04001BCE RID: 7118
		TopToBottom
	}

	// Token: 0x020005C8 RID: 1480
	// (Invoke) Token: 0x06003149 RID: 12617
	public delegate void OnDragFinished();
}
