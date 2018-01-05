using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000590 RID: 1424
[AddComponentMenu("NGUI/Examples/Play Idle Animations")]
public class PlayIdleAnimations : MonoBehaviour
{
	// Token: 0x06002FDD RID: 12253 RVA: 0x000EA2A8 File Offset: 0x000E86A8
	private void Start()
	{
		this.mAnim = base.GetComponentInChildren<Animation>();
		if (this.mAnim == null)
		{
			Debug.LogWarning(NGUITools.GetHierarchy(base.gameObject) + " has no Animation component");
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AnimationState animationState = (AnimationState)obj;
					if (animationState.clip.name == "idle")
					{
						animationState.layer = 0;
						this.mIdle = animationState.clip;
						this.mAnim.Play(this.mIdle.name);
					}
					else if (animationState.clip.name.StartsWith("idle"))
					{
						animationState.layer = 1;
						this.mBreaks.Add(animationState.clip);
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
			if (this.mBreaks.Count == 0)
			{
				UnityEngine.Object.Destroy(this);
			}
		}
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x000EA3DC File Offset: 0x000E87DC
	private void Update()
	{
		if (this.mNextBreak < Time.time)
		{
			if (this.mBreaks.Count == 1)
			{
				AnimationClip animationClip = this.mBreaks[0];
				this.mNextBreak = Time.time + animationClip.length + UnityEngine.Random.Range(5f, 15f);
				this.mAnim.CrossFade(animationClip.name);
			}
			else
			{
				int num = UnityEngine.Random.Range(0, this.mBreaks.Count - 1);
				if (this.mLastIndex == num)
				{
					num++;
					if (num >= this.mBreaks.Count)
					{
						num = 0;
					}
				}
				this.mLastIndex = num;
				AnimationClip animationClip2 = this.mBreaks[num];
				this.mNextBreak = Time.time + animationClip2.length + UnityEngine.Random.Range(2f, 8f);
				this.mAnim.CrossFade(animationClip2.name);
			}
		}
	}

	// Token: 0x04001A46 RID: 6726
	private Animation mAnim;

	// Token: 0x04001A47 RID: 6727
	private AnimationClip mIdle;

	// Token: 0x04001A48 RID: 6728
	private List<AnimationClip> mBreaks = new List<AnimationClip>();

	// Token: 0x04001A49 RID: 6729
	private float mNextBreak;

	// Token: 0x04001A4A RID: 6730
	private int mLastIndex;
}
