using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000653 RID: 1619
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Root")]
public class UIRoot : MonoBehaviour
{
	// Token: 0x17000883 RID: 2179
	// (get) Token: 0x06003699 RID: 13977 RVA: 0x00116FB2 File Offset: 0x001153B2
	public UIRoot.Constraint constraint
	{
		get
		{
			if (this.fitWidth)
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.Fit;
				}
				return UIRoot.Constraint.FitWidth;
			}
			else
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.FitHeight;
				}
				return UIRoot.Constraint.Fill;
			}
		}
	}

	// Token: 0x17000884 RID: 2180
	// (get) Token: 0x0600369A RID: 13978 RVA: 0x00116FDC File Offset: 0x001153DC
	public UIRoot.Scaling activeScaling
	{
		get
		{
			UIRoot.Scaling scaling = this.scalingStyle;
			if (scaling == UIRoot.Scaling.ConstrainedOnMobiles)
			{
				return UIRoot.Scaling.Flexible;
			}
			return scaling;
		}
	}

	// Token: 0x17000885 RID: 2181
	// (get) Token: 0x0600369B RID: 13979 RVA: 0x00116FFC File Offset: 0x001153FC
	public int activeHeight
	{
		get
		{
			if (this.activeScaling == UIRoot.Scaling.Flexible)
			{
				Vector2 screenSize = NGUITools.screenSize;
				float num = screenSize.x / screenSize.y;
				if (screenSize.y < (float)this.minimumHeight)
				{
					screenSize.y = (float)this.minimumHeight;
					screenSize.x = screenSize.y * num;
				}
				else if (screenSize.y > (float)this.maximumHeight)
				{
					screenSize.y = (float)this.maximumHeight;
					screenSize.x = screenSize.y * num;
				}
				int num2 = Mathf.RoundToInt((!this.shrinkPortraitUI || screenSize.y <= screenSize.x) ? screenSize.y : (screenSize.y / num));
				return (!this.adjustByDPI) ? num2 : NGUIMath.AdjustByDPI((float)num2);
			}
			UIRoot.Constraint constraint = this.constraint;
			if (constraint == UIRoot.Constraint.FitHeight)
			{
				return this.manualHeight;
			}
			Vector2 screenSize2 = NGUITools.screenSize;
			float num3 = screenSize2.x / screenSize2.y;
			float num4 = (float)this.manualWidth / (float)this.manualHeight;
			if (constraint == UIRoot.Constraint.FitWidth)
			{
				return Mathf.RoundToInt((float)this.manualWidth / num3);
			}
			if (constraint == UIRoot.Constraint.Fit)
			{
				return (num4 <= num3) ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / num3);
			}
			if (constraint != UIRoot.Constraint.Fill)
			{
				return this.manualHeight;
			}
			return (num4 >= num3) ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / num3);
		}
	}

	// Token: 0x17000886 RID: 2182
	// (get) Token: 0x0600369C RID: 13980 RVA: 0x001171A0 File Offset: 0x001155A0
	public float pixelSizeAdjustment
	{
		get
		{
			int num = Mathf.RoundToInt(NGUITools.screenSize.y);
			return (num != -1) ? this.GetPixelSizeAdjustment(num) : 1f;
		}
	}

	// Token: 0x0600369D RID: 13981 RVA: 0x001171D8 File Offset: 0x001155D8
	public static float GetPixelSizeAdjustment(GameObject go)
	{
		UIRoot uiroot = NGUITools.FindInParents<UIRoot>(go);
		return (!(uiroot != null)) ? 1f : uiroot.pixelSizeAdjustment;
	}

	// Token: 0x0600369E RID: 13982 RVA: 0x00117208 File Offset: 0x00115608
	public float GetPixelSizeAdjustment(int height)
	{
		height = Mathf.Max(2, height);
		if (this.activeScaling == UIRoot.Scaling.Constrained)
		{
			return (float)this.activeHeight / (float)height;
		}
		if (height < this.minimumHeight)
		{
			return (float)this.minimumHeight / (float)height;
		}
		if (height > this.maximumHeight)
		{
			return (float)this.maximumHeight / (float)height;
		}
		return 1f;
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x00117268 File Offset: 0x00115668
	protected virtual void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060036A0 RID: 13984 RVA: 0x00117276 File Offset: 0x00115676
	protected virtual void OnEnable()
	{
		UIRoot.list.Add(this);
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x00117283 File Offset: 0x00115683
	protected virtual void OnDisable()
	{
		UIRoot.list.Remove(this);
	}

	// Token: 0x060036A2 RID: 13986 RVA: 0x00117294 File Offset: 0x00115694
	protected virtual void Start()
	{
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
			}
		}
		else
		{
			this.Update();
		}
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x001172F4 File Offset: 0x001156F4
	private void Update()
	{
		if (this.mTrans != null)
		{
			float num = (float)this.activeHeight;
			if (num > 0f)
			{
				float num2 = 2f / num;
				Vector3 localScale = this.mTrans.localScale;
				if (Mathf.Abs(localScale.x - num2) > 1.401298E-45f || Mathf.Abs(localScale.y - num2) > 1.401298E-45f || Mathf.Abs(localScale.z - num2) > 1.401298E-45f)
				{
					this.mTrans.localScale = new Vector3(num2, num2, num2);
				}
			}
		}
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x00117394 File Offset: 0x00115794
	public static void Broadcast(string funcName)
	{
		int i = 0;
		int count = UIRoot.list.Count;
		while (i < count)
		{
			UIRoot uiroot = UIRoot.list[i];
			if (uiroot != null)
			{
				uiroot.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
			}
			i++;
		}
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x001173E0 File Offset: 0x001157E0
	public static void Broadcast(string funcName, object param)
	{
		if (param == null)
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
		}
		else
		{
			int i = 0;
			int count = UIRoot.list.Count;
			while (i < count)
			{
				UIRoot uiroot = UIRoot.list[i];
				if (uiroot != null)
				{
					uiroot.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
				}
				i++;
			}
		}
	}

	// Token: 0x04001F83 RID: 8067
	public static List<UIRoot> list = new List<UIRoot>();

	// Token: 0x04001F84 RID: 8068
	public UIRoot.Scaling scalingStyle;

	// Token: 0x04001F85 RID: 8069
	public int manualWidth = 1280;

	// Token: 0x04001F86 RID: 8070
	public int manualHeight = 720;

	// Token: 0x04001F87 RID: 8071
	public int minimumHeight = 320;

	// Token: 0x04001F88 RID: 8072
	public int maximumHeight = 1536;

	// Token: 0x04001F89 RID: 8073
	public bool fitWidth;

	// Token: 0x04001F8A RID: 8074
	public bool fitHeight = true;

	// Token: 0x04001F8B RID: 8075
	public bool adjustByDPI;

	// Token: 0x04001F8C RID: 8076
	public bool shrinkPortraitUI;

	// Token: 0x04001F8D RID: 8077
	private Transform mTrans;

	// Token: 0x02000654 RID: 1620
	public enum Scaling
	{
		// Token: 0x04001F8F RID: 8079
		Flexible,
		// Token: 0x04001F90 RID: 8080
		Constrained,
		// Token: 0x04001F91 RID: 8081
		ConstrainedOnMobiles
	}

	// Token: 0x02000655 RID: 1621
	public enum Constraint
	{
		// Token: 0x04001F93 RID: 8083
		Fit,
		// Token: 0x04001F94 RID: 8084
		Fill,
		// Token: 0x04001F95 RID: 8085
		FitWidth,
		// Token: 0x04001F96 RID: 8086
		FitHeight
	}
}
