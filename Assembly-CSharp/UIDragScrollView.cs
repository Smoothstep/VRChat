using System;
using UnityEngine;

// Token: 0x020005B0 RID: 1456
[AddComponentMenu("NGUI/Interaction/Drag Scroll View")]
public class UIDragScrollView : MonoBehaviour
{
	// Token: 0x0600307F RID: 12415 RVA: 0x000EDEA0 File Offset: 0x000EC2A0
	private void OnEnable()
	{
		this.mTrans = base.transform;
		if (this.scrollView == null && this.draggablePanel != null)
		{
			this.scrollView = this.draggablePanel;
			this.draggablePanel = null;
		}
		if (this.mStarted && (this.mAutoFind || this.mScroll == null))
		{
			this.FindScrollView();
		}
	}

	// Token: 0x06003080 RID: 12416 RVA: 0x000EDF1B File Offset: 0x000EC31B
	private void Start()
	{
		this.mStarted = true;
		this.FindScrollView();
	}

	// Token: 0x06003081 RID: 12417 RVA: 0x000EDF2C File Offset: 0x000EC32C
	private void FindScrollView()
	{
		UIScrollView y = NGUITools.FindInParents<UIScrollView>(this.mTrans);
		if (this.scrollView == null)
		{
			this.scrollView = y;
			this.mAutoFind = true;
		}
		else if (this.scrollView == y)
		{
			this.mAutoFind = true;
		}
		this.mScroll = this.scrollView;
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x000EDF90 File Offset: 0x000EC390
	private void OnPress(bool pressed)
	{
		if (this.mAutoFind && this.mScroll != this.scrollView)
		{
			this.mScroll = this.scrollView;
			this.mAutoFind = false;
		}
		if (this.scrollView && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.scrollView.Press(pressed);
			if (!pressed && this.mAutoFind)
			{
				this.scrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
				this.mScroll = this.scrollView;
			}
		}
	}

	// Token: 0x06003083 RID: 12419 RVA: 0x000EE036 File Offset: 0x000EC436
	private void OnDrag(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Drag();
		}
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x000EE05E File Offset: 0x000EC45E
	private void OnScroll(float delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Scroll(delta);
		}
	}

	// Token: 0x04001AF6 RID: 6902
	public UIScrollView scrollView;

	// Token: 0x04001AF7 RID: 6903
	[HideInInspector]
	[SerializeField]
	private UIScrollView draggablePanel;

	// Token: 0x04001AF8 RID: 6904
	private Transform mTrans;

	// Token: 0x04001AF9 RID: 6905
	private UIScrollView mScroll;

	// Token: 0x04001AFA RID: 6906
	private bool mAutoFind;

	// Token: 0x04001AFB RID: 6907
	private bool mStarted;
}
