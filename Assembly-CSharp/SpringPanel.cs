using System;
using UnityEngine;

// Token: 0x020005FA RID: 1530
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Internal/Spring Panel")]
public class SpringPanel : MonoBehaviour
{
	// Token: 0x0600332A RID: 13098 RVA: 0x00101223 File Offset: 0x000FF623
	private void Start()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDrag = base.GetComponent<UIScrollView>();
		this.mTrans = base.transform;
	}

	// Token: 0x0600332B RID: 13099 RVA: 0x00101249 File Offset: 0x000FF649
	private void Update()
	{
		this.AdvanceTowardsPosition();
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x00101254 File Offset: 0x000FF654
	protected virtual void AdvanceTowardsPosition()
	{
		float deltaTime = RealTime.deltaTime;
		bool flag = false;
		Vector3 localPosition = this.mTrans.localPosition;
		Vector3 vector = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
		if ((vector - this.target).sqrMagnitude < 0.01f)
		{
			vector = this.target;
			base.enabled = false;
			flag = true;
		}
		this.mTrans.localPosition = vector;
		Vector3 vector2 = vector - localPosition;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= vector2.x;
		clipOffset.y -= vector2.y;
		this.mPanel.clipOffset = clipOffset;
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(false);
		}
		if (flag && this.onFinished != null)
		{
			SpringPanel.current = this;
			this.onFinished();
			SpringPanel.current = null;
		}
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x00101360 File Offset: 0x000FF760
	public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanel springPanel = go.GetComponent<SpringPanel>();
		if (springPanel == null)
		{
			springPanel = go.AddComponent<SpringPanel>();
		}
		springPanel.target = pos;
		springPanel.strength = strength;
		springPanel.onFinished = null;
		springPanel.enabled = true;
		return springPanel;
	}

	// Token: 0x04001D01 RID: 7425
	public static SpringPanel current;

	// Token: 0x04001D02 RID: 7426
	public Vector3 target = Vector3.zero;

	// Token: 0x04001D03 RID: 7427
	public float strength = 10f;

	// Token: 0x04001D04 RID: 7428
	public SpringPanel.OnFinished onFinished;

	// Token: 0x04001D05 RID: 7429
	private UIPanel mPanel;

	// Token: 0x04001D06 RID: 7430
	private Transform mTrans;

	// Token: 0x04001D07 RID: 7431
	private UIScrollView mDrag;

	// Token: 0x020005FB RID: 1531
	// (Invoke) Token: 0x0600332F RID: 13103
	public delegate void OnFinished();
}
