using System;
using System.Collections;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000B5F RID: 2911
public class SyncAnimation : VRCPunBehaviour
{
	// Token: 0x06005951 RID: 22865 RVA: 0x001F036C File Offset: 0x001EE76C
	public override void Awake()
	{
		base.Awake();
		this.sync = base.GetComponent<VRC_SyncAnimation>();
		float animationStartPosition = this.sync.AnimationStartPosition;
		this.targetAnimation = base.GetComponent<Animation>();
		if (this.targetAnimation)
		{
			IEnumerator enumerator = this.targetAnimation.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AnimationState animationState = (AnimationState)obj;
					animationState.normalizedTime += animationStartPosition;
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
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x06005952 RID: 22866 RVA: 0x001F0420 File Offset: 0x001EE820
	public override IEnumerator Start()
	{
		yield return base.Start();
		base.ObserveThis();
		yield break;
	}

	// Token: 0x06005953 RID: 22867 RVA: 0x001F043C File Offset: 0x001EE83C
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.targetAnimation != null)
			{
				this.SendAnimation(stream);
			}
			if (this.animator != null)
			{
				this.SendAnimator(stream);
			}
		}
		else
		{
			if (this.targetAnimation != null)
			{
				this.ReceiveAnimation(stream);
			}
			if (this.animator != null)
			{
				this.ReceiveAnimator(stream);
			}
		}
	}

	// Token: 0x06005954 RID: 22868 RVA: 0x001F04BC File Offset: 0x001EE8BC
	private void SendAnimation(PhotonStream stream)
	{
		stream.SendNext(this.targetAnimation.isPlaying);
		if (this.targetAnimation.isPlaying)
		{
			IEnumerator enumerator = this.targetAnimation.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AnimationState animationState = (AnimationState)obj;
					stream.SendNext((short)Mathf.FloatToHalf(animationState.weight));
					stream.SendNext((short)Mathf.FloatToHalf(animationState.time));
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
	}

	// Token: 0x06005955 RID: 22869 RVA: 0x001F0570 File Offset: 0x001EE970
	private void ReceiveAnimation(PhotonStream stream)
	{
		bool flag = (bool)stream.ReceiveNext();
		if (flag != this.targetAnimation.isPlaying)
		{
			if (flag)
			{
				this.targetAnimation.Play();
			}
			else
			{
				this.targetAnimation.Stop();
			}
		}
		if (this.targetAnimation.isPlaying)
		{
			IEnumerator enumerator = this.targetAnimation.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AnimationState animationState = (AnimationState)obj;
					float num = Mathf.HalfToFloat((ushort)((short)stream.ReceiveNext()));
					float num2 = Mathf.HalfToFloat((ushort)((short)stream.ReceiveNext()));
					float num3 = Mathf.Abs(num2 - animationState.time);
					if (num3 > 0.5f)
					{
						animationState.time = Mathf.MoveTowards(animationState.time, num2, 1f);
					}
					if (num != animationState.weight)
					{
						animationState.weight = num;
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
	}

	// Token: 0x06005956 RID: 22870 RVA: 0x001F068C File Offset: 0x001EEA8C
	private void SendAnimator(PhotonStream stream)
	{
		int layerCount = this.animator.layerCount;
		stream.SendNext(layerCount);
		for (int i = 0; i < layerCount; i++)
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(i);
			float layerWeight = this.animator.GetLayerWeight(i);
			int fullPathHash = currentAnimatorStateInfo.fullPathHash;
			stream.SendNext(layerWeight);
			stream.SendNext(fullPathHash);
			stream.SendNext(currentAnimatorStateInfo.normalizedTime - (float)Math.Truncate((double)currentAnimatorStateInfo.normalizedTime));
		}
		int parameterCount = this.animator.parameterCount;
		stream.SendNext(parameterCount);
		for (int j = 0; j < parameterCount; j++)
		{
			AnimatorControllerParameter parameter = this.animator.GetParameter(j);
			AnimatorControllerParameterType type = parameter.type;
			switch (type)
			{
			case AnimatorControllerParameterType.Float:
			{
				float @float = this.animator.GetFloat(parameter.nameHash);
				stream.SendNext(@float);
				break;
			}
			default:
				if (type == AnimatorControllerParameterType.Trigger)
				{
					bool @bool = this.animator.GetBool(parameter.nameHash);
					stream.SendNext(@bool);
				}
				break;
			case AnimatorControllerParameterType.Int:
			{
				int integer = this.animator.GetInteger(parameter.nameHash);
				stream.SendNext(integer);
				break;
			}
			case AnimatorControllerParameterType.Bool:
			{
				bool bool2 = this.animator.GetBool(parameter.nameHash);
				stream.SendNext(bool2);
				break;
			}
			}
		}
	}

	// Token: 0x06005957 RID: 22871 RVA: 0x001F082C File Offset: 0x001EEC2C
	private void ReceiveAnimator(PhotonStream stream)
	{
		int num = (int)stream.ReceiveNext();
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)stream.ReceiveNext();
			int num3 = (int)stream.ReceiveNext();
			float num4 = (float)stream.ReceiveNext();
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(i);
			float num5 = (float)VRC.Network.SimulationDelay(base.Owner);
			float num6 = currentAnimatorStateInfo.normalizedTime - (float)Math.Truncate((double)currentAnimatorStateInfo.normalizedTime);
			float num7 = num4 * currentAnimatorStateInfo.length;
			float num8 = num6 * currentAnimatorStateInfo.length;
			if (this.animator.GetLayerWeight(i) != num2)
			{
				this.animator.SetLayerWeight(i, num2);
			}
			if (currentAnimatorStateInfo.fullPathHash != num3)
			{
				Debug.LogFormat("{0} crossfading {1} to {2}", new object[]
				{
					base.name,
					currentAnimatorStateInfo.fullPathHash,
					num3
				});
				this.animator.CrossFade((num3 != currentAnimatorStateInfo.fullPathHash) ? num3 : 0, num5, i, num4 + (float)VRC.Network.SendInterval);
			}
			if (Mathf.Abs(num8 - num7) > num5 + 0.5f)
			{
				Debug.LogFormat("{0} moving time from {1} to {2}, was off by {3}", new object[]
				{
					base.name,
					num8,
					num7,
					Mathf.Abs(num8 - num7)
				});
				this.animator.Play(num3, i, (num7 + (float)VRC.Network.SendInterval) / currentAnimatorStateInfo.length);
			}
		}
		int num9 = (int)stream.ReceiveNext();
		for (int j = 0; j < num9; j++)
		{
			AnimatorControllerParameter parameter = this.animator.GetParameter(j);
			AnimatorControllerParameterType type = parameter.type;
			switch (type)
			{
			case AnimatorControllerParameterType.Float:
			{
				float value = (float)stream.ReceiveNext();
				this.animator.SetFloat(parameter.nameHash, value);
				break;
			}
			default:
				if (type == AnimatorControllerParameterType.Trigger)
				{
					bool flag = (bool)stream.ReceiveNext();
					if (flag)
					{
						this.animator.SetTrigger(j);
					}
					else
					{
						this.animator.ResetTrigger(j);
					}
				}
				break;
			case AnimatorControllerParameterType.Int:
			{
				int value2 = (int)stream.ReceiveNext();
				this.animator.SetInteger(parameter.nameHash, value2);
				break;
			}
			case AnimatorControllerParameterType.Bool:
			{
				bool value3 = (bool)stream.ReceiveNext();
				this.animator.SetBool(parameter.nameHash, value3);
				break;
			}
			}
		}
	}

	// Token: 0x04003FE9 RID: 16361
	private Animator animator;

	// Token: 0x04003FEA RID: 16362
	private Animation targetAnimation;

	// Token: 0x04003FEB RID: 16363
	private VRC_SyncAnimation sync;
}
