using System;
using UnityEngine;

// Token: 0x02000619 RID: 1561
[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : MonoBehaviour
{
	// Token: 0x06003439 RID: 13369 RVA: 0x001084DE File Offset: 0x001068DE
	private void Start()
	{
		this.mTrans = base.transform;
		if (this.updateScrollView)
		{
			this.mSv = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
	}

	// Token: 0x0600343A RID: 13370 RVA: 0x00108508 File Offset: 0x00106908
	private void Update()
	{
		float deltaTime = (!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime;
		if (this.worldSpace)
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.position).sqrMagnitude * 0.001f;
			}
			this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.position).sqrMagnitude)
			{
				this.mTrans.position = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		else
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.localPosition).sqrMagnitude * 1E-05f;
			}
			this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.localPosition).sqrMagnitude)
			{
				this.mTrans.localPosition = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		if (this.mSv != null)
		{
			this.mSv.UpdateScrollbars(true);
		}
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x001086B0 File Offset: 0x00106AB0
	private void NotifyListeners()
	{
		SpringPosition.current = this;
		if (this.onFinished != null)
		{
			this.onFinished();
		}
		if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
		{
			this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
		}
		SpringPosition.current = null;
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x00108714 File Offset: 0x00106B14
	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition springPosition = go.GetComponent<SpringPosition>();
		if (springPosition == null)
		{
			springPosition = go.AddComponent<SpringPosition>();
		}
		springPosition.target = pos;
		springPosition.strength = strength;
		springPosition.onFinished = null;
		if (!springPosition.enabled)
		{
			springPosition.mThreshold = 0f;
			springPosition.enabled = true;
		}
		return springPosition;
	}

	// Token: 0x04001DC0 RID: 7616
	public static SpringPosition current;

	// Token: 0x04001DC1 RID: 7617
	public Vector3 target = Vector3.zero;

	// Token: 0x04001DC2 RID: 7618
	public float strength = 10f;

	// Token: 0x04001DC3 RID: 7619
	public bool worldSpace;

	// Token: 0x04001DC4 RID: 7620
	public bool ignoreTimeScale;

	// Token: 0x04001DC5 RID: 7621
	public bool updateScrollView;

	// Token: 0x04001DC6 RID: 7622
	public SpringPosition.OnFinished onFinished;

	// Token: 0x04001DC7 RID: 7623
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x04001DC8 RID: 7624
	[SerializeField]
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x04001DC9 RID: 7625
	private Transform mTrans;

	// Token: 0x04001DCA RID: 7626
	private float mThreshold;

	// Token: 0x04001DCB RID: 7627
	private UIScrollView mSv;

	// Token: 0x0200061A RID: 1562
	// (Invoke) Token: 0x0600343E RID: 13374
	public delegate void OnFinished();
}
