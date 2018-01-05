using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x02000626 RID: 1574
public abstract class UITweener : MonoBehaviour
{
	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x060034BA RID: 13498 RVA: 0x00108808 File Offset: 0x00106C08
	public float amountPerDelta
	{
		get
		{
			if (this.mDuration != this.duration)
			{
				this.mDuration = this.duration;
				this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f) ? 1000f : (1f / this.duration)) * Mathf.Sign(this.mAmountPerDelta);
			}
			return this.mAmountPerDelta;
		}
	}

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x060034BB RID: 13499 RVA: 0x00108875 File Offset: 0x00106C75
	// (set) Token: 0x060034BC RID: 13500 RVA: 0x0010887D File Offset: 0x00106C7D
	public float tweenFactor
	{
		get
		{
			return this.mFactor;
		}
		set
		{
			this.mFactor = Mathf.Clamp01(value);
		}
	}

	// Token: 0x17000801 RID: 2049
	// (get) Token: 0x060034BD RID: 13501 RVA: 0x0010888B File Offset: 0x00106C8B
	public Direction direction
	{
		get
		{
			return (this.amountPerDelta >= 0f) ? Direction.Forward : Direction.Reverse;
		}
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x001088A4 File Offset: 0x00106CA4
	private void Reset()
	{
		if (!this.mStarted)
		{
			this.SetStartToCurrentValue();
			this.SetEndToCurrentValue();
		}
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x001088BD File Offset: 0x00106CBD
	protected virtual void Start()
	{
		this.Update();
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x001088C8 File Offset: 0x00106CC8
	private void Update()
	{
		float num = (!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime;
		float num2 = (!this.ignoreTimeScale) ? Time.time : RealTime.time;
		if (!this.mStarted)
		{
			this.mStarted = true;
			this.mStartTime = num2 + this.delay;
		}
		if (num2 < this.mStartTime)
		{
			return;
		}
		this.mFactor += this.amountPerDelta * num;
		if (this.style == UITweener.Style.Loop)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor -= Mathf.Floor(this.mFactor);
			}
		}
		else if (this.style == UITweener.Style.PingPong)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			else if (this.mFactor < 0f)
			{
				this.mFactor = -this.mFactor;
				this.mFactor -= Mathf.Floor(this.mFactor);
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
		}
		if (this.style == UITweener.Style.Once && (this.duration == 0f || this.mFactor > 1f || this.mFactor < 0f))
		{
			this.mFactor = Mathf.Clamp01(this.mFactor);
			this.Sample(this.mFactor, true);
			if (this.duration == 0f || (this.mFactor == 1f && this.mAmountPerDelta > 0f) || (this.mFactor == 0f && this.mAmountPerDelta < 0f))
			{
				base.enabled = false;
			}
			if (UITweener.current == null)
			{
				UITweener.current = this;
				if (this.onFinished != null)
				{
					this.mTemp = this.onFinished;
					this.onFinished = new List<EventDelegate>();
					EventDelegate.Execute(this.mTemp);
					for (int i = 0; i < this.mTemp.Count; i++)
					{
						EventDelegate eventDelegate = this.mTemp[i];
						if (eventDelegate != null && !eventDelegate.oneShot)
						{
							EventDelegate.Add(this.onFinished, eventDelegate, eventDelegate.oneShot);
						}
					}
					this.mTemp = null;
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				UITweener.current = null;
			}
		}
		else
		{
			this.Sample(this.mFactor, false);
		}
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x00108BA6 File Offset: 0x00106FA6
	public void SetOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x00108BB5 File Offset: 0x00106FB5
	public void SetOnFinished(EventDelegate del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x00108BC3 File Offset: 0x00106FC3
	public void AddOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x00108BD2 File Offset: 0x00106FD2
	public void AddOnFinished(EventDelegate del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x00108BE0 File Offset: 0x00106FE0
	public void RemoveOnFinished(EventDelegate del)
	{
		if (this.onFinished != null)
		{
			this.onFinished.Remove(del);
		}
		if (this.mTemp != null)
		{
			this.mTemp.Remove(del);
		}
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x00108C12 File Offset: 0x00107012
	private void OnDisable()
	{
		this.mStarted = false;
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x00108C1C File Offset: 0x0010701C
	public void Sample(float factor, bool isFinished)
	{
		float num = Mathf.Clamp01(factor);
		if (this.method == UITweener.Method.EaseIn)
		{
			num = 1f - Mathf.Sin(1.57079637f * (1f - num));
			if (this.steeperCurves)
			{
				num *= num;
			}
		}
		else if (this.method == UITweener.Method.EaseOut)
		{
			num = Mathf.Sin(1.57079637f * num);
			if (this.steeperCurves)
			{
				num = 1f - num;
				num = 1f - num * num;
			}
		}
		else if (this.method == UITweener.Method.EaseInOut)
		{
			num -= Mathf.Sin(num * 6.28318548f) / 6.28318548f;
			if (this.steeperCurves)
			{
				num = num * 2f - 1f;
				float num2 = Mathf.Sign(num);
				num = 1f - Mathf.Abs(num);
				num = 1f - num * num;
				num = num2 * num * 0.5f + 0.5f;
			}
		}
		else if (this.method == UITweener.Method.BounceIn)
		{
			num = this.BounceLogic(num);
		}
		else if (this.method == UITweener.Method.BounceOut)
		{
			num = 1f - this.BounceLogic(1f - num);
		}
		this.OnUpdate((this.animationCurve == null) ? num : this.animationCurve.Evaluate(num), isFinished);
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x00108D70 File Offset: 0x00107170
	private float BounceLogic(float val)
	{
		if (val < 0.363636f)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f)
		{
			val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
		}
		else if (val < 0.90909f)
		{
			val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
		}
		else
		{
			val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;
		}
		return val;
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x00108E07 File Offset: 0x00107207
	[Obsolete("Use PlayForward() instead")]
	public void Play()
	{
		this.Play(true);
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x00108E10 File Offset: 0x00107210
	public void PlayForward()
	{
		this.Play(true);
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x00108E19 File Offset: 0x00107219
	public void PlayReverse()
	{
		this.Play(false);
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x00108E22 File Offset: 0x00107222
	public void Play(bool forward)
	{
		this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		if (!forward)
		{
			this.mAmountPerDelta = -this.mAmountPerDelta;
		}
		base.enabled = true;
		this.Update();
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x00108E55 File Offset: 0x00107255
	public void ResetToBeginning()
	{
		this.mStarted = false;
		this.mFactor = ((this.amountPerDelta >= 0f) ? 0f : 1f);
		this.Sample(this.mFactor, false);
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x00108E90 File Offset: 0x00107290
	public void Toggle()
	{
		if (this.mFactor > 0f)
		{
			this.mAmountPerDelta = -this.amountPerDelta;
		}
		else
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		}
		base.enabled = true;
	}

	// Token: 0x060034CF RID: 13519
	protected abstract void OnUpdate(float factor, bool isFinished);

	// Token: 0x060034D0 RID: 13520 RVA: 0x00108ECC File Offset: 0x001072CC
	public static T Begin<T>(GameObject go, float duration) where T : UITweener
	{
		T t = go.GetComponent<T>();
		if (t != null && t.tweenGroup != 0)
		{
			t = (T)((object)null);
			T[] components = go.GetComponents<T>();
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				t = components[i];
				if (t != null && t.tweenGroup == 0)
				{
					break;
				}
				t = (T)((object)null);
				i++;
			}
		}
		if (t == null)
		{
			t = go.AddComponent<T>();
			if (t == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Unable to add ",
					typeof(T),
					" to ",
					NGUITools.GetHierarchy(go)
				}), go);
				return (T)((object)null);
			}
		}
		t.mStarted = false;
		t.duration = duration;
		t.mFactor = 0f;
		t.mAmountPerDelta = Mathf.Abs(t.amountPerDelta);
		t.style = UITweener.Style.Once;
		t.animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});
		t.eventReceiver = null;
		t.callWhenFinished = null;
		t.enabled = true;
		return t;
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x00109099 File Offset: 0x00107499
	public virtual void SetStartToCurrentValue()
	{
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x0010909B File Offset: 0x0010749B
	public virtual void SetEndToCurrentValue()
	{
	}

	// Token: 0x04001E00 RID: 7680
	public static UITweener current;

	// Token: 0x04001E01 RID: 7681
	[HideInInspector]
	public UITweener.Method method;

	// Token: 0x04001E02 RID: 7682
	[HideInInspector]
	public UITweener.Style style;

	// Token: 0x04001E03 RID: 7683
	[HideInInspector]
	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x04001E04 RID: 7684
	[HideInInspector]
	public bool ignoreTimeScale = true;

	// Token: 0x04001E05 RID: 7685
	[HideInInspector]
	public float delay;

	// Token: 0x04001E06 RID: 7686
	[HideInInspector]
	public float duration = 1f;

	// Token: 0x04001E07 RID: 7687
	[HideInInspector]
	public bool steeperCurves;

	// Token: 0x04001E08 RID: 7688
	[HideInInspector]
	public int tweenGroup;

	// Token: 0x04001E09 RID: 7689
	[HideInInspector]
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04001E0A RID: 7690
	[HideInInspector]
	public GameObject eventReceiver;

	// Token: 0x04001E0B RID: 7691
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x04001E0C RID: 7692
	private bool mStarted;

	// Token: 0x04001E0D RID: 7693
	private float mStartTime;

	// Token: 0x04001E0E RID: 7694
	private float mDuration;

	// Token: 0x04001E0F RID: 7695
	private float mAmountPerDelta = 1000f;

	// Token: 0x04001E10 RID: 7696
	private float mFactor;

	// Token: 0x04001E11 RID: 7697
	private List<EventDelegate> mTemp;

	// Token: 0x02000627 RID: 1575
	public enum Method
	{
		// Token: 0x04001E13 RID: 7699
		Linear,
		// Token: 0x04001E14 RID: 7700
		EaseIn,
		// Token: 0x04001E15 RID: 7701
		EaseOut,
		// Token: 0x04001E16 RID: 7702
		EaseInOut,
		// Token: 0x04001E17 RID: 7703
		BounceIn,
		// Token: 0x04001E18 RID: 7704
		BounceOut
	}

	// Token: 0x02000628 RID: 1576
	public enum Style
	{
		// Token: 0x04001E1A RID: 7706
		Once,
		// Token: 0x04001E1B RID: 7707
		Loop,
		// Token: 0x04001E1C RID: 7708
		PingPong
	}
}
