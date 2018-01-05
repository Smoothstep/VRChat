using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VRC;
using VRC.Core;

// Token: 0x02000A45 RID: 2629
public class PoseRecorder : VRCPunBehaviour
{
	// Token: 0x17000BC2 RID: 3010
	// (get) Token: 0x06004F4A RID: 20298 RVA: 0x001AC618 File Offset: 0x001AAA18
	private PoseRecorder.PoseEvent LastEvent
	{
		get
		{
			int count = this.eventHistory.Count;
			if (count > 0)
			{
				return this.eventHistory[count - 1];
			}
			return null;
		}
	}

	// Token: 0x06004F4B RID: 20299 RVA: 0x001AC648 File Offset: 0x001AAA48
	public override IEnumerator Start()
	{
		yield return base.Start();
		base.ObserveThis();
		this.ragDoll = base.gameObject.GetComponentInSelfOrParent<RagdollController>();
		if (this.ragDoll == null)
		{
			this.ragDoll = base.gameObject.GetComponentInChildren<RagdollController>();
		}
		yield break;
	}

	// Token: 0x06004F4C RID: 20300 RVA: 0x001AC664 File Offset: 0x001AAA64
	public void Initialize(Animator targetAnimator, Transform offsetTransform)
	{
		this.animator = targetAnimator;
		this.offset = offsetTransform;
		this.handgest = base.GetComponentInChildren<HandGestureController>();
		this.ik = base.GetComponentInChildren<IkController>();
		PoseRecorder.poseContents = 0;
		if (targetAnimator != null)
		{
			this.bones[0] = targetAnimator.GetBoneTransform(HumanBodyBones.Hips);
			this.bones[1] = targetAnimator.GetBoneTransform(HumanBodyBones.Spine);
			this.bones[2] = targetAnimator.GetBoneTransform(HumanBodyBones.Chest);
			this.bones[3] = targetAnimator.GetBoneTransform(HumanBodyBones.Neck);
			this.bones[4] = targetAnimator.GetBoneTransform(HumanBodyBones.Head);
			this.bones[5] = targetAnimator.GetBoneTransform(HumanBodyBones.LeftShoulder);
			this.bones[6] = targetAnimator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
			this.bones[7] = targetAnimator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
			this.bones[8] = targetAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
			this.bones[9] = targetAnimator.GetBoneTransform(HumanBodyBones.RightShoulder);
			this.bones[10] = targetAnimator.GetBoneTransform(HumanBodyBones.RightUpperArm);
			this.bones[11] = targetAnimator.GetBoneTransform(HumanBodyBones.RightLowerArm);
			this.bones[12] = targetAnimator.GetBoneTransform(HumanBodyBones.RightHand);
			this.bones[13] = targetAnimator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
			this.bones[14] = targetAnimator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
			this.bones[15] = targetAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
			this.bones[16] = targetAnimator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
			this.bones[17] = targetAnimator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
			this.bones[18] = targetAnimator.GetBoneTransform(HumanBodyBones.RightFoot);
		}
	}

	// Token: 0x06004F4D RID: 20301 RVA: 0x001AC7DA File Offset: 0x001AABDA
	private void SerializeVector(PhotonStream stream, Vector3 vect)
	{
		Serialization.SerializeVector(stream, vect);
	}

	// Token: 0x06004F4E RID: 20302 RVA: 0x001AC7E3 File Offset: 0x001AABE3
	private Vector3 DeserializeVector(PhotonStream stream)
	{
		return Serialization.DeserializeVector(stream);
	}

	// Token: 0x06004F4F RID: 20303 RVA: 0x001AC7EB File Offset: 0x001AABEB
	private void SerializeQuaternion(PhotonStream stream, Quaternion quat)
	{
		Serialization.SerializeQuaternion(stream, quat);
	}

	// Token: 0x06004F50 RID: 20304 RVA: 0x001AC7F4 File Offset: 0x001AABF4
	private Quaternion DeserializeQuaternion(PhotonStream stream)
	{
		return Serialization.DeserializeQuaternion(stream);
	}

	// Token: 0x06004F51 RID: 20305 RVA: 0x001AC7FC File Offset: 0x001AABFC
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		PoseRecorder.PoseEvent poseEvent = new PoseRecorder.PoseEvent();
		if (stream.isWriting)
		{
			if (this.ragDoll != null && this.ragDoll.IsRagDolled)
			{
				PoseRecorder.poseContents = 0;
			}
			poseEvent.poseContents = PoseRecorder.poseContents;
			PoseRecorder.poseContents = 0;
			stream.SendNext((short)poseEvent.poseContents);
			if (poseEvent.Contains(PoseRecorder.PoseContents.RootOffset))
			{
				poseEvent.rootOffset = this.offset.position;
				this.SerializeVector(stream, poseEvent.rootOffset);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.BaseSkeleton))
			{
				for (int i = 0; i < this.bones.Length; i++)
				{
					poseEvent.boneRotations[i] = ((!(this.bones[i] == null)) ? this.bones[i].rotation : Quaternion.identity);
					this.SerializeQuaternion(stream, poseEvent.boneRotations[i]);
				}
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HandGesture))
			{
				int num = 255;
				int num2 = 255;
				if (this.handgest != null)
				{
					this.handgest.GetRemoteHandGestures(out num, out num2);
				}
				stream.SendNext((byte)num);
				stream.SendNext((byte)num2);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HandEffectorPositionOrientationL))
			{
				if (this.ik != null)
				{
					this.ik.GetHandEffectorLeft(out poseEvent.leftHandPosition, out poseEvent.leftHandRotation);
				}
				this.SerializeVector(stream, poseEvent.leftHandPosition);
				this.SerializeQuaternion(stream, poseEvent.leftHandRotation);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HandEffectorPositionOrientationR))
			{
				if (this.ik != null)
				{
					this.ik.GetHandEffectorRight(out poseEvent.rightHandPosition, out poseEvent.rightHandRotation);
				}
				this.SerializeVector(stream, poseEvent.rightHandPosition);
				this.SerializeQuaternion(stream, poseEvent.rightHandRotation);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HeadEffectorPositionOrientation))
			{
				if (this.ik != null)
				{
					this.ik.GetHeadEffector(out poseEvent.headPosition, out poseEvent.headRotation);
				}
				this.SerializeVector(stream, poseEvent.headPosition);
				this.SerializeQuaternion(stream, poseEvent.headRotation);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HipEffectorPositionOrientation))
			{
				if (this.ik != null)
				{
					this.ik.GetHipEffector(out poseEvent.hipPosition, out poseEvent.hipRotation);
				}
				this.SerializeVector(stream, poseEvent.hipPosition);
				this.SerializeQuaternion(stream, poseEvent.hipRotation);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.FootEffectorPositionOrientationL))
			{
				if (this.ik != null)
				{
					this.ik.GetFootEffectorLeft(out poseEvent.leftFootPosition, out poseEvent.leftFootRotation);
				}
				this.SerializeVector(stream, poseEvent.leftFootPosition);
				this.SerializeQuaternion(stream, poseEvent.leftFootRotation);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.FootEffectorPositionOrientationR))
			{
				if (this.ik != null)
				{
					this.ik.GetFootEffectorRight(out poseEvent.rightFootPosition, out poseEvent.rightFootRotation);
				}
				this.SerializeVector(stream, poseEvent.rightFootPosition);
				this.SerializeQuaternion(stream, poseEvent.rightFootRotation);
			}
		}
		else
		{
			poseEvent.poseContents = (int)((short)stream.ReceiveNext());
			if (poseEvent.Contains(PoseRecorder.PoseContents.RootOffset))
			{
				poseEvent.rootOffset = this.DeserializeVector(stream);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.BaseSkeleton))
			{
				for (int j = 0; j < this.bones.Length; j++)
				{
					poseEvent.boneRotations[j] = this.DeserializeQuaternion(stream);
				}
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HandGesture))
			{
				poseEvent.handGestureLeft = (byte)stream.ReceiveNext();
				poseEvent.handGestureRight = (byte)stream.ReceiveNext();
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HandEffectorPositionOrientationL))
			{
				poseEvent.leftHandPosition = this.DeserializeVector(stream);
				poseEvent.leftHandRotation = this.DeserializeQuaternion(stream);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HandEffectorPositionOrientationR))
			{
				poseEvent.rightHandPosition = this.DeserializeVector(stream);
				poseEvent.rightHandRotation = this.DeserializeQuaternion(stream);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HeadEffectorPositionOrientation))
			{
				poseEvent.headPosition = this.DeserializeVector(stream);
				poseEvent.headRotation = this.DeserializeQuaternion(stream);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HipEffectorPositionOrientation))
			{
				poseEvent.hipPosition = this.DeserializeVector(stream);
				poseEvent.hipRotation = this.DeserializeQuaternion(stream);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.FootEffectorPositionOrientationL))
			{
				poseEvent.leftFootPosition = this.DeserializeVector(stream);
				poseEvent.leftFootRotation = this.DeserializeQuaternion(stream);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.FootEffectorPositionOrientationR))
			{
				poseEvent.rightFootPosition = this.DeserializeVector(stream);
				poseEvent.rightFootRotation = this.DeserializeQuaternion(stream);
			}
		}
		poseEvent.timeReceived = (double)Time.time;
		TweenFunctions.RecordValue<PoseRecorder.PoseEvent>(this.eventHistory, poseEvent);
	}

	// Token: 0x17000BC3 RID: 3011
	// (get) Token: 0x06004F52 RID: 20306 RVA: 0x001ACCDD File Offset: 0x001AB0DD
	private double ExpectedInterval
	{
		get
		{
			return VRC.Network.SendInterval + (double)Time.smoothDeltaTime;
		}
	}

	// Token: 0x06004F53 RID: 20307 RVA: 0x001ACCEC File Offset: 0x001AB0EC
	private static PoseRecorder.PoseEvent LinearTweenFunction(IList<PoseRecorder.PoseEvent> l, int idx, double delta)
	{
		PoseRecorder.PoseEvent poseEvent = l[idx];
		PoseRecorder.PoseEvent poseEvent2 = l[TweenFunctions.FindNextIndex<PoseRecorder.PoseEvent>(l, idx)];
		float t = (float)delta;
		PoseRecorder.PoseEvent poseEvent3 = new PoseRecorder.PoseEvent
		{
			poseContents = (poseEvent.poseContents & poseEvent2.poseContents),
			timeReceived = (poseEvent2.timeReceived - poseEvent.timeReceived) * delta + poseEvent.timeReceived
		};
		if (poseEvent2.Contains(PoseRecorder.PoseContents.HandGesture))
		{
			poseEvent3.handGestureLeft = poseEvent2.handGestureLeft;
			poseEvent3.handGestureRight = poseEvent2.handGestureRight;
			poseEvent3.poseContents |= 16;
		}
		if (poseEvent3.Contains(PoseRecorder.PoseContents.Nothing))
		{
			return poseEvent3;
		}
		if (poseEvent3.Contains(PoseRecorder.PoseContents.RootOffset))
		{
			PoseRecorder.MaybeSetPosition(poseEvent.rootOffset, Vector3.Lerp(poseEvent.rootOffset, poseEvent2.rootOffset, t), ref poseEvent3.rootOffset);
		}
		if (poseEvent3.Contains(PoseRecorder.PoseContents.BaseSkeleton))
		{
			poseEvent3.boneRotations = new Quaternion[19];
			for (int i = 0; i < 19; i++)
			{
				poseEvent3.boneRotations[i] = Quaternion.Slerp(poseEvent.boneRotations[i], poseEvent2.boneRotations[i], t).Normalize();
			}
		}
		if (poseEvent3.Contains(PoseRecorder.PoseContents.HandEffectorPositionOrientationL))
		{
			PoseRecorder.MaybeSetPosition(poseEvent.leftHandPosition, Vector3.Lerp(poseEvent.leftHandPosition, poseEvent2.leftHandPosition, t), ref poseEvent3.leftHandPosition);
			poseEvent3.leftHandRotation = Quaternion.Slerp(poseEvent.leftHandRotation, poseEvent2.leftHandRotation, t).Normalize();
		}
		if (poseEvent3.Contains(PoseRecorder.PoseContents.HandEffectorPositionOrientationR))
		{
			PoseRecorder.MaybeSetPosition(poseEvent.rightHandPosition, Vector3.Lerp(poseEvent.rightHandPosition, poseEvent2.rightHandPosition, t), ref poseEvent3.rightHandPosition);
			poseEvent3.rightHandRotation = Quaternion.Slerp(poseEvent.rightHandRotation, poseEvent2.rightHandRotation, t).Normalize();
		}
		if (poseEvent3.Contains(PoseRecorder.PoseContents.HeadEffectorPositionOrientation))
		{
			PoseRecorder.MaybeSetPosition(poseEvent.headPosition, Vector3.Lerp(poseEvent.headPosition, poseEvent2.headPosition, t), ref poseEvent3.headPosition);
			poseEvent3.headRotation = Quaternion.Slerp(poseEvent.headRotation, poseEvent2.headRotation, t).Normalize();
		}
		if (poseEvent3.Contains(PoseRecorder.PoseContents.HipEffectorPositionOrientation))
		{
			PoseRecorder.MaybeSetPosition(poseEvent.hipPosition, Vector3.Lerp(poseEvent.hipPosition, poseEvent2.hipPosition, t), ref poseEvent3.hipPosition);
			poseEvent3.hipRotation = Quaternion.Slerp(poseEvent.hipRotation, poseEvent2.hipRotation, t).Normalize();
		}
		if (poseEvent3.Contains(PoseRecorder.PoseContents.FootEffectorPositionOrientationL))
		{
			PoseRecorder.MaybeSetPosition(poseEvent.leftFootPosition, Vector3.Lerp(poseEvent.leftFootPosition, poseEvent2.leftFootPosition, t), ref poseEvent3.leftFootPosition);
			poseEvent3.leftFootRotation = Quaternion.Slerp(poseEvent.leftFootRotation, poseEvent2.leftFootRotation, t).Normalize();
		}
		if (poseEvent3.Contains(PoseRecorder.PoseContents.FootEffectorPositionOrientationR))
		{
			PoseRecorder.MaybeSetPosition(poseEvent.rightFootPosition, Vector3.Lerp(poseEvent.rightFootPosition, poseEvent2.rightFootPosition, t), ref poseEvent3.rightFootPosition);
			poseEvent3.rightFootRotation = Quaternion.Slerp(poseEvent.rightFootRotation, poseEvent2.rightFootRotation, t).Normalize();
		}
		return poseEvent3;
	}

	// Token: 0x06004F54 RID: 20308 RVA: 0x001AD002 File Offset: 0x001AB402
	private static void MaybeSetPosition(Vector3 current, Vector3 desired, ref Vector3 target)
	{
		if (!desired.IsBad())
		{
			target = desired;
		}
	}

	// Token: 0x06004F55 RID: 20309 RVA: 0x001AD016 File Offset: 0x001AB416
	private static void MaybeSetRotation(Quaternion current, Quaternion desired, ref Quaternion target)
	{
		if (!desired.IsBad())
		{
			target = desired;
		}
	}

	// Token: 0x06004F56 RID: 20310 RVA: 0x001AD02C File Offset: 0x001AB42C
	private void MaybeSetEffector(PoseRecorder.GetEffector getter, PoseRecorder.SetEffector setter, Vector3 desiredPos, Quaternion desiredRot)
	{
		Vector3 vector;
		Quaternion quaternion;
		getter(out vector, out quaternion);
		Vector3 pos = vector;
		Quaternion rot = quaternion;
		if (!desiredPos.IsBad())
		{
			pos = ((!this.UseSecondaryTween) ? desiredPos : Vector3.MoveTowards(vector, desiredPos, this.MaxPositionDelta));
		}
		if (!desiredRot.IsBad())
		{
			rot = ((!this.UseSecondaryTween) ? desiredRot : Quaternion.RotateTowards(quaternion, desiredRot, this.MaxRotationDelta).Normalize());
		}
		setter(pos, rot);
	}

	// Token: 0x06004F57 RID: 20311 RVA: 0x001AD0AC File Offset: 0x001AB4AC
	public void DoAdjustment(float now)
	{
		if (base.isMine || this.animator == null)
		{
			return;
		}
		IList<PoseRecorder.PoseEvent> eventList = this.eventHistory;
		if (PoseRecorder.f__mg0 == null)
		{
			PoseRecorder.f__mg0 = new TweenFunctions.TweenFunction<PoseRecorder.PoseEvent>(PoseRecorder.LinearTweenFunction);
		}
		PoseRecorder.PoseEvent poseEvent = TweenFunctions.Tween<PoseRecorder.PoseEvent>(eventList, PoseRecorder.f__mg0, (double)now - VRC.Network.SimulationDelay(base.Owner), this.ExpectedInterval, -1);
		if (poseEvent == null || poseEvent.Contains(PoseRecorder.PoseContents.Nothing))
		{
			return;
		}
		if (poseEvent.Contains(PoseRecorder.PoseContents.BaseSkeleton))
		{
			this.animator.enabled = false;
			for (int i = 0; i < 19; i++)
			{
				Quaternion localRotation = this.bones[i].localRotation;
				PoseRecorder.MaybeSetRotation(this.bones[i].localRotation, poseEvent.boneRotations[i], ref localRotation);
				this.bones[i].localRotation = localRotation;
			}
		}
		if (this.ik != null)
		{
			if (poseEvent.Contains(PoseRecorder.PoseContents.HandEffectorPositionOrientationL))
			{
				this.MaybeSetEffector(new PoseRecorder.GetEffector(this.ik.GetHandEffectorLeft), new PoseRecorder.SetEffector(this.ik.SetRemoteHandEffectorLeft), poseEvent.leftHandPosition, poseEvent.leftHandRotation);
			}
			else
			{
				this.ik.ClearRemoteHandEffectorLeft();
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HandEffectorPositionOrientationR))
			{
				this.MaybeSetEffector(new PoseRecorder.GetEffector(this.ik.GetHandEffectorRight), new PoseRecorder.SetEffector(this.ik.SetRemoteHandEffectorRight), poseEvent.rightHandPosition, poseEvent.rightHandRotation);
			}
			else
			{
				this.ik.ClearRemoteHandEffectorRight();
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HeadEffectorPositionOrientation))
			{
				this.MaybeSetEffector(new PoseRecorder.GetEffector(this.ik.GetHeadEffector), new PoseRecorder.SetEffector(this.ik.SetRemoteHeadEffector), poseEvent.headPosition, poseEvent.headRotation);
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.HipEffectorPositionOrientation))
			{
				this.MaybeSetEffector(new PoseRecorder.GetEffector(this.ik.GetHipEffector), new PoseRecorder.SetEffector(this.ik.SetRemoteHipEffector), poseEvent.hipPosition, poseEvent.hipRotation);
			}
			else
			{
				this.ik.ClearRemoteHipEffector();
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.FootEffectorPositionOrientationL))
			{
				this.MaybeSetEffector(new PoseRecorder.GetEffector(this.ik.GetFootEffectorLeft), new PoseRecorder.SetEffector(this.ik.SetRemoteFootEffectorLeft), poseEvent.leftFootPosition, poseEvent.leftFootRotation);
			}
			else
			{
				this.ik.ClearRemoteFootEffectorLeft();
			}
			if (poseEvent.Contains(PoseRecorder.PoseContents.FootEffectorPositionOrientationR))
			{
				this.MaybeSetEffector(new PoseRecorder.GetEffector(this.ik.GetFootEffectorRight), new PoseRecorder.SetEffector(this.ik.SetRemoteFootEffectorRight), poseEvent.rightFootPosition, poseEvent.rightFootRotation);
			}
			else
			{
				this.ik.ClearRemoteFootEffectorRight();
			}
		}
		if (poseEvent.Contains(PoseRecorder.PoseContents.HandGesture))
		{
			if (this.handgest != null)
			{
				this.handgest.SetRemoteHandGestures((int)poseEvent.handGestureLeft, (int)poseEvent.handGestureRight);
			}
		}
		else if (this.handgest != null)
		{
			this.handgest.ClearRemoteHandGestures();
		}
		if (this.offset != null && poseEvent.Contains(PoseRecorder.PoseContents.RootOffset))
		{
			this.offset.localPosition = Vector3.zero;
		}
		if (this.animator != null && poseEvent.Contains(PoseRecorder.PoseContents.BaseSkeleton))
		{
			this.animator.enabled = true;
		}
		if (base.Owner.playerNet.WasRecentlyDiscontinuous)
		{
			this.ik.Reset(true);
		}
	}

	// Token: 0x040037C0 RID: 14272
	public static int poseContents;

	// Token: 0x040037C1 RID: 14273
	private const int FRAME_BUFFER_BONES = 19;

	// Token: 0x040037C2 RID: 14274
	private LimitedCapacityList<PoseRecorder.PoseEvent> eventHistory = new LimitedCapacityList<PoseRecorder.PoseEvent>();

	// Token: 0x040037C3 RID: 14275
	private Transform[] bones = new Transform[19];

	// Token: 0x040037C4 RID: 14276
	private HandGestureController handgest;

	// Token: 0x040037C5 RID: 14277
	private IkController ik;

	// Token: 0x040037C6 RID: 14278
	private Animator animator;

	// Token: 0x040037C7 RID: 14279
	private RagdollController ragDoll;

	// Token: 0x040037C8 RID: 14280
	private Transform offset;

	// Token: 0x040037C9 RID: 14281
	[Tooltip("If true, then MoveTowards/RotateTowards are used to set the value, using the max deltas as tolerances.")]
	public bool UseSecondaryTween;

	// Token: 0x040037CA RID: 14282
	[Range(0f, 1f)]
	public float MaxPositionDelta = 0.05f;

	// Token: 0x040037CB RID: 14283
	[Range(0f, 45f)]
	public float MaxRotationDelta = 1f;

	// Token: 0x040037CC RID: 14284
	[CompilerGenerated]
	private static TweenFunctions.TweenFunction<PoseRecorder.PoseEvent> f__mg0;

	// Token: 0x02000A46 RID: 2630
	public enum PoseContents
	{
		// Token: 0x040037CE RID: 14286
		Nothing,
		// Token: 0x040037CF RID: 14287
		RootOffset,
		// Token: 0x040037D0 RID: 14288
		BaseSkeleton,
		// Token: 0x040037D1 RID: 14289
		HandEffectorPositionOrientationL = 4,
		// Token: 0x040037D2 RID: 14290
		HandEffectorPositionOrientationR = 8,
		// Token: 0x040037D3 RID: 14291
		HandGesture = 16,
		// Token: 0x040037D4 RID: 14292
		HeadEffectorPositionOrientation = 32,
		// Token: 0x040037D5 RID: 14293
		HipEffectorPositionOrientation = 64,
		// Token: 0x040037D6 RID: 14294
		FootEffectorPositionOrientationL = 128,
		// Token: 0x040037D7 RID: 14295
		FootEffectorPositionOrientationR = 256
	}

	// Token: 0x02000A47 RID: 2631
	private class PoseEvent : TweenFunctions.ITimedValue
	{
		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06004F5B RID: 20315 RVA: 0x001AD51D File Offset: 0x001AB91D
		// (set) Token: 0x06004F5C RID: 20316 RVA: 0x001AD525 File Offset: 0x001AB925
		public double Time
		{
			get
			{
				return this.timeReceived;
			}
			set
			{
				this.timeReceived = value;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06004F5D RID: 20317 RVA: 0x001AD52E File Offset: 0x001AB92E
		public bool Skip
		{
			get
			{
				return this.Contains(PoseRecorder.PoseContents.Nothing);
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06004F5E RID: 20318 RVA: 0x001AD537 File Offset: 0x001AB937
		public bool Discontinuity
		{
			get
			{
				return this.Contains(PoseRecorder.PoseContents.Nothing);
			}
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x001AD540 File Offset: 0x001AB940
		public bool Contains(PoseRecorder.PoseContents content)
		{
			return (this.poseContents & (int)content) != 0;
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x001AD550 File Offset: 0x001AB950
		public void Remove(PoseRecorder.PoseContents content)
		{
			this.poseContents &= (int)(~(int)content);
		}

		// Token: 0x040037D8 RID: 14296
		public int poseContents;

		// Token: 0x040037D9 RID: 14297
		public Vector3 rootOffset = Vector3.zero;

		// Token: 0x040037DA RID: 14298
		public Quaternion[] boneRotations = new Quaternion[19];

		// Token: 0x040037DB RID: 14299
		public byte handGestureLeft = byte.MaxValue;

		// Token: 0x040037DC RID: 14300
		public byte handGestureRight = byte.MaxValue;

		// Token: 0x040037DD RID: 14301
		public Vector3 leftHandPosition = Vector3.zero;

		// Token: 0x040037DE RID: 14302
		public Vector3 rightHandPosition = Vector3.zero;

		// Token: 0x040037DF RID: 14303
		public Vector3 headPosition = Vector3.zero;

		// Token: 0x040037E0 RID: 14304
		public Quaternion leftHandRotation = Quaternion.identity;

		// Token: 0x040037E1 RID: 14305
		public Quaternion rightHandRotation = Quaternion.identity;

		// Token: 0x040037E2 RID: 14306
		public Quaternion headRotation = Quaternion.identity;

		// Token: 0x040037E3 RID: 14307
		public Vector3 leftFootPosition = Vector3.zero;

		// Token: 0x040037E4 RID: 14308
		public Vector3 rightFootPosition = Vector3.zero;

		// Token: 0x040037E5 RID: 14309
		public Vector3 hipPosition = Vector3.zero;

		// Token: 0x040037E6 RID: 14310
		public Quaternion leftFootRotation = Quaternion.identity;

		// Token: 0x040037E7 RID: 14311
		public Quaternion rightFootRotation = Quaternion.identity;

		// Token: 0x040037E8 RID: 14312
		public Quaternion hipRotation = Quaternion.identity;

		// Token: 0x040037E9 RID: 14313
		public double timeReceived;
	}

	// Token: 0x02000A48 RID: 2632
	// (Invoke) Token: 0x06004F62 RID: 20322
	private delegate void GetEffector(out Vector3 pos, out Quaternion rot);

	// Token: 0x02000A49 RID: 2633
	// (Invoke) Token: 0x06004F66 RID: 20326
	private delegate void SetEffector(Vector3 pos, Quaternion rot);
}
