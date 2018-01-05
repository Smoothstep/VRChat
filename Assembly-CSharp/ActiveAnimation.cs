using System;
using System.Collections;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x020005DE RID: 1502
[AddComponentMenu("NGUI/Internal/Active Animation")]
public class ActiveAnimation : MonoBehaviour
{
	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x060031C1 RID: 12737 RVA: 0x000F6624 File Offset: 0x000F4A24
	private float playbackTime
	{
		get
		{
			return Mathf.Clamp01(this.mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		}
	}

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x060031C2 RID: 12738 RVA: 0x000F664C File Offset: 0x000F4A4C
	public bool isPlaying
	{
		get
		{
			if (!(this.mAnim == null))
			{
				IEnumerator enumerator = this.mAnim.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						AnimationState animationState = (AnimationState)obj;
						if (this.mAnim.IsPlaying(animationState.name))
						{
							if (this.mLastDirection == Direction.Forward)
							{
								if (animationState.time < animationState.length)
								{
									return true;
								}
							}
							else
							{
								if (this.mLastDirection != Direction.Reverse)
								{
									return true;
								}
								if (animationState.time > 0f)
								{
									return true;
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				return false;
			}
			if (this.mAnimator != null)
			{
				if (this.mLastDirection == Direction.Reverse)
				{
					if (this.playbackTime == 0f)
					{
						return false;
					}
				}
				else if (this.playbackTime == 1f)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}

	// Token: 0x060031C3 RID: 12739 RVA: 0x000F677C File Offset: 0x000F4B7C
	public void Finish()
	{
		if (this.mAnim != null)
		{
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AnimationState animationState = (AnimationState)obj;
					if (this.mLastDirection == Direction.Forward)
					{
						animationState.time = animationState.length;
					}
					else if (this.mLastDirection == Direction.Reverse)
					{
						animationState.time = 0f;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.mAnim.Sample();
		}
		else if (this.mAnimator != null)
		{
			this.mAnimator.Play(this.mClip, 0, (this.mLastDirection != Direction.Forward) ? 0f : 1f);
		}
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x000F6870 File Offset: 0x000F4C70
	public void Reset()
	{
		if (this.mAnim != null)
		{
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AnimationState animationState = (AnimationState)obj;
					if (this.mLastDirection == Direction.Reverse)
					{
						animationState.time = animationState.length;
					}
					else if (this.mLastDirection == Direction.Forward)
					{
						animationState.time = 0f;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
		else if (this.mAnimator != null)
		{
			this.mAnimator.Play(this.mClip, 0, (this.mLastDirection != Direction.Reverse) ? 0f : 1f);
		}
	}

	// Token: 0x060031C5 RID: 12741 RVA: 0x000F6958 File Offset: 0x000F4D58
	private void Start()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x060031C6 RID: 12742 RVA: 0x000F698C File Offset: 0x000F4D8C
	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		if (deltaTime == 0f)
		{
			return;
		}
		if (this.mAnimator != null)
		{
			this.mAnimator.Update((this.mLastDirection != Direction.Reverse) ? deltaTime : (-deltaTime));
			if (this.isPlaying)
			{
				return;
			}
			this.mAnimator.enabled = false;
			base.enabled = false;
		}
		else
		{
			if (!(this.mAnim != null))
			{
				base.enabled = false;
				return;
			}
			bool flag = false;
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AnimationState animationState = (AnimationState)obj;
					if (this.mAnim.IsPlaying(animationState.name))
					{
						float num = animationState.speed * deltaTime;
						animationState.time += num;
						if (num < 0f)
						{
							if (animationState.time > 0f)
							{
								flag = true;
							}
							else
							{
								animationState.time = 0f;
							}
						}
						else if (animationState.time < animationState.length)
						{
							flag = true;
						}
						else
						{
							animationState.time = animationState.length;
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.mAnim.Sample();
			if (flag)
			{
				return;
			}
			base.enabled = false;
		}
		if (this.mNotify)
		{
			this.mNotify = false;
			if (ActiveAnimation.current == null)
			{
				ActiveAnimation.current = this;
				EventDelegate.Execute(this.onFinished);
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
				}
				ActiveAnimation.current = null;
			}
			if (this.mDisableDirection != Direction.Toggle && this.mLastDirection == this.mDisableDirection)
			{
				NGUITools.SetActive(base.gameObject, false);
			}
		}
	}

	// Token: 0x060031C7 RID: 12743 RVA: 0x000F6BAC File Offset: 0x000F4FAC
	private void Play(string clipName, Direction playDirection)
	{
		if (playDirection == Direction.Toggle)
		{
			playDirection = ((this.mLastDirection == Direction.Forward) ? Direction.Reverse : Direction.Forward);
		}
		if (this.mAnim != null)
		{
			base.enabled = true;
			this.mAnim.enabled = false;
			bool flag = string.IsNullOrEmpty(clipName);
			if (flag)
			{
				if (!this.mAnim.isPlaying)
				{
					this.mAnim.Play();
				}
			}
			else if (!this.mAnim.IsPlaying(clipName))
			{
				this.mAnim.Play(clipName);
			}
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AnimationState animationState = (AnimationState)obj;
					if (string.IsNullOrEmpty(clipName) || animationState.name == clipName)
					{
						float num = Mathf.Abs(animationState.speed);
						animationState.speed = num * (float)playDirection;
						if (playDirection == Direction.Reverse && animationState.time == 0f)
						{
							animationState.time = animationState.length;
						}
						else if (playDirection == Direction.Forward && animationState.time == animationState.length)
						{
							animationState.time = 0f;
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.mLastDirection = playDirection;
			this.mNotify = true;
			this.mAnim.Sample();
		}
		else if (this.mAnimator != null)
		{
			if (base.enabled && this.isPlaying && this.mClip == clipName)
			{
				this.mLastDirection = playDirection;
				return;
			}
			base.enabled = true;
			this.mNotify = true;
			this.mLastDirection = playDirection;
			this.mClip = clipName;
			this.mAnimator.Play(this.mClip, 0, (playDirection != Direction.Forward) ? 1f : 0f);
		}
	}

	// Token: 0x060031C8 RID: 12744 RVA: 0x000F6DB8 File Offset: 0x000F51B8
	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (!NGUITools.GetActive(anim.gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
			UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				componentsInChildren[i].Refresh();
				i++;
			}
		}
		ActiveAnimation activeAnimation = anim.GetComponent<ActiveAnimation>();
		if (activeAnimation == null)
		{
			activeAnimation = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mAnim = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.onFinished.Clear();
		activeAnimation.Play(clipName, playDirection);
		if (activeAnimation.mAnim != null)
		{
			activeAnimation.mAnim.Sample();
		}
		else if (activeAnimation.mAnimator != null)
		{
			activeAnimation.mAnimator.Update(0f);
		}
		return activeAnimation;
	}

	// Token: 0x060031C9 RID: 12745 RVA: 0x000F6E98 File Offset: 0x000F5298
	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	// Token: 0x060031CA RID: 12746 RVA: 0x000F6EA4 File Offset: 0x000F52A4
	public static ActiveAnimation Play(Animation anim, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x000F6EB0 File Offset: 0x000F52B0
	public static ActiveAnimation Play(Animator anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (enableBeforePlay != EnableCondition.IgnoreDisabledState && !NGUITools.GetActive(anim.gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
			UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				componentsInChildren[i].Refresh();
				i++;
			}
		}
		ActiveAnimation activeAnimation = anim.GetComponent<ActiveAnimation>();
		if (activeAnimation == null)
		{
			activeAnimation = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mAnimator = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.onFinished.Clear();
		activeAnimation.Play(clipName, playDirection);
		if (activeAnimation.mAnim != null)
		{
			activeAnimation.mAnim.Sample();
		}
		else if (activeAnimation.mAnimator != null)
		{
			activeAnimation.mAnimator.Update(0f);
		}
		return activeAnimation;
	}

	// Token: 0x04001C52 RID: 7250
	public static ActiveAnimation current;

	// Token: 0x04001C53 RID: 7251
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04001C54 RID: 7252
	[HideInInspector]
	public GameObject eventReceiver;

	// Token: 0x04001C55 RID: 7253
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x04001C56 RID: 7254
	private Animation mAnim;

	// Token: 0x04001C57 RID: 7255
	private Direction mLastDirection;

	// Token: 0x04001C58 RID: 7256
	private Direction mDisableDirection;

	// Token: 0x04001C59 RID: 7257
	private bool mNotify;

	// Token: 0x04001C5A RID: 7258
	private Animator mAnimator;

	// Token: 0x04001C5B RID: 7259
	private string mClip = string.Empty;
}
