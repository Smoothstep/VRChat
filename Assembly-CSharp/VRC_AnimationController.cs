using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000A50 RID: 2640
public class VRC_AnimationController : VRCPunBehaviour
{
	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x06004FEC RID: 20460 RVA: 0x001B45AE File Offset: 0x001B29AE
	// (set) Token: 0x06004FED RID: 20461 RVA: 0x001B45B6 File Offset: 0x001B29B6
	public bool isCulled { get; private set; }

	// Token: 0x06004FEE RID: 20462 RVA: 0x001B45C0 File Offset: 0x001B29C0
	public override void Awake()
	{
		base.Awake();
		this._player = base.GetComponentInParent<VRCPlayer>();
		if (this._cameraMount == null)
		{
			Debug.LogError("VRC_AnimationController: camera mount was not connected!");
		}
		this.jawController = base.GetComponent<JawController>();
		this.lipsyncController = base.GetComponent<LipSyncController>();
		if (this._player != null)
		{
			this.muteController = this._player.GetComponentInChildren<MuteController>();
		}
		this.lookController = base.GetComponent<EyeLookController>();
		this.cameraMountController = base.GetComponent<CameraMountController>();
		this._kinectController = this.Kinect.GetComponent<KinectAvatarController>();
		this._ragdollController = base.GetComponent<RagdollController>();
		this._avatarAnimation = base.GetComponentInChildren<AvatarAnimation>();
		this._meatHook = base.gameObject.AddComponent<MeatHook>();
		this._handGestureController = base.GetComponent<HandGestureController>();
		this._emotePlayer = base.GetComponent<EmotePlayer>();
		this.UpdateActiveElements();
	}

	// Token: 0x06004FEF RID: 20463 RVA: 0x001B46A4 File Offset: 0x001B2AA4
	public override IEnumerator Start()
	{
		yield return base.Start();
		this._meatHook.Initialize(base.isMine, this.avatarRoot, this._cameraMount);
		yield break;
	}

	// Token: 0x06004FF0 RID: 20464 RVA: 0x001B46C0 File Offset: 0x001B2AC0
	private void UpdateActiveElements()
	{
		if (!base.isMine)
		{
			return;
		}
		if (this.RunKinect != this.Kinect.activeSelf)
		{
			this.Kinect.SetActive(this.RunKinect);
			if (this._animator != null)
			{
				if (this.RunKinect)
				{
					this.Kinect.GetComponent<KinectAvatarController>().Initialize(this._animator, base.isMine);
				}
				this._animator.enabled = !this.RunKinect;
			}
		}
	}

	// Token: 0x06004FF1 RID: 20465 RVA: 0x001B474C File Offset: 0x001B2B4C
	public void Detach()
	{
		this._animator = null;
		this.jawController.Detach();
		if (this._meatHook != null)
		{
			this._meatHook.Detach();
		}
		if (this.lipsyncController.enabled)
		{
			this.lipsyncController.DisableLipSyncComponents();
			this.lipsyncController.enabled = false;
		}
		if (this.lookController != null && this.lookController.enabled)
		{
			this.lookController.UnInitialize();
			this.lookController.enabled = false;
		}
		if (this.usingVRIK)
		{
			this._vrcVrIkController.Uninitialize();
		}
		if (this.usingFBBIK)
		{
			this._vrcFbbIkController.Uninitialize();
		}
	}

	// Token: 0x06004FF2 RID: 20466 RVA: 0x001B4814 File Offset: 0x001B2C14
	private void ResetInUseComponents()
	{
		if (this.lipsyncController != null && this.lipsyncController.enabled)
		{
			this.lipsyncController.DisableLipSyncComponents();
			this.lipsyncController.enabled = false;
		}
		if (this.lookController != null && this.lookController.enabled)
		{
			this.lookController.UnInitialize();
			this.lookController.enabled = false;
		}
		if (this._ikController != null)
		{
			this._ikController.ClearIkType();
			this._vrcVrIkController = null;
			this._vrcFbbIkController = null;
			this.usingVRIK = false;
			this.usingFBBIK = false;
		}
		if (this.usingVRIK && this._vrcVrIkController != null)
		{
			this._vrcVrIkController.Uninitialize();
		}
		if (this.usingFBBIK && this._vrcFbbIkController != null)
		{
			this._vrcFbbIkController.Uninitialize();
		}
	}

	// Token: 0x06004FF3 RID: 20467 RVA: 0x001B4917 File Offset: 0x001B2D17
	public void SetupHandGestures(Animator animator, bool local)
	{
		this._handGestureController.Initialize(animator, base.isMine);
	}

	// Token: 0x06004FF4 RID: 20468 RVA: 0x001B492C File Offset: 0x001B2D2C
	public IkController.IkType ResetIKSystem(Animator animator, bool forceIkType, IkController.IkType forceToType)
	{
		if (animator.transform.parent != null)
		{
			this._avatarSwitcher = animator.transform.parent.GetComponent<VRCAvatarManager>();
		}
		if (this._avatarSwitcher == null)
		{
			forceIkType = true;
			forceToType = IkController.IkType.None;
		}
		else if (this._avatarSwitcher.currentAvatarKind != VRCAvatarManager.AvatarKind.Custom)
		{
			forceIkType = true;
			forceToType = IkController.IkType.Limb;
		}
		this.vrik = animator.GetComponent<VRIK>();
		this.fbbik = animator.GetComponent<FullBodyBipedIK>();
		this.limbik = animator.GetComponent<global::LimbIK>();
		this.usingFBBIK = false;
		this.usingVRIK = false;
		if (this._player != null && this.HeadAndHandsIkController != null)
		{
			this._vrcVrIkController = this.HeadAndHandsIkController.GetComponent<VRCVrIkController>();
			this._vrcFbbIkController = this.HeadAndHandsIkController.GetComponent<VRCFbbIkController>();
			if (this.limbik != null && this.vrik != null && this.fbbik != null && this._vrcVrIkController != null && this._vrcFbbIkController != null)
			{
				if ((forceIkType && forceToType == IkController.IkType.SixPoint) || (!forceIkType && base.isMine && VRCTrackingManager.CanSupportHipAndFeetTracking()))
				{
					this._vrcVrIkController.enabled = false;
					this._vrcFbbIkController.enabled = true;
					if (this._vrcFbbIkController.Initialize(this, animator, this._player, base.isMine))
					{
						this.usingFBBIK = true;
						this.fbbik.enabled = true;
						this.usingVRIK = false;
						this.vrik.enabled = false;
						this.limbik.enabled = false;
						Debug.Log("IK[" + this._player.name + "]: 6 Point IK");
						return IkController.IkType.SixPoint;
					}
				}
				if ((forceIkType && forceToType == IkController.IkType.ThreeOrFourPoint) || (!forceIkType && !this.usingFBBIK && this._vrcVrIkController != null))
				{
					this._vrcVrIkController.enabled = true;
					this._vrcFbbIkController.enabled = false;
					if (this._vrcVrIkController.Initialize(this, animator, this._player, base.isMine))
					{
						this.usingVRIK = true;
						this.vrik.enabled = true;
						this.usingFBBIK = false;
						this.fbbik.enabled = false;
						this.limbik.enabled = false;
						Debug.Log("IK[" + this._player.name + "]: 3/4 Point IK");
						return IkController.IkType.ThreeOrFourPoint;
					}
				}
				if ((forceIkType && forceToType == IkController.IkType.Limb) || (!forceIkType && !this.usingFBBIK && !this.usingVRIK))
				{
					this.limbik.enabled = true;
					this._vrcVrIkController.enabled = false;
					this._vrcVrIkController = null;
					this._vrcFbbIkController.enabled = false;
					this._vrcFbbIkController = null;
					this.usingVRIK = false;
					this.vrik.enabled = false;
					this.usingFBBIK = false;
					this.fbbik.enabled = false;
					Debug.Log("IK[" + this._player.name + "]: Limb IK");
					return IkController.IkType.Limb;
				}
			}
		}
		this._vrcVrIkController = null;
		this._vrcFbbIkController = null;
		this._ikController = null;
		this.usingVRIK = false;
		this.vrik.enabled = false;
		this.usingFBBIK = false;
		this.fbbik.enabled = false;
		this.limbik.enabled = false;
		Debug.Log("IK Mode: None");
		return IkController.IkType.None;
	}

	// Token: 0x06004FF5 RID: 20469 RVA: 0x001B4CCC File Offset: 0x001B30CC
	public void Reset(Animator animator, VRC_AvatarDescriptor ad)
	{
		this.ResetInUseComponents();
		this._animator = animator;
		if (ad == null)
		{
			ad = base.transform.parent.GetComponentInChildren<VRC_AvatarDescriptor>();
		}
		GameObject gameObject = ad.gameObject;
		base.GetComponent<PoseRecorder>().Initialize(this._animator, this.avatarRoot);
		if (this.Kinect != null && this.RunKinect)
		{
			this.Kinect.GetComponent<KinectAvatarController>().Initialize(animator, base.isMine);
		}
		if (this._avatarAnimation != null)
		{
			this._avatarAnimation.Initialize(animator, base.isMine);
		}
		if (animator != null && animator.isHuman)
		{
			if (this._player != null && this._emotePlayer != null)
			{
				this._emotePlayer.Initialize(this._animator, base.isMine);
			}
			IkController.IkType ikType = this.ResetIKSystem(animator, false, IkController.IkType.None);
			this._ikController = this.HeadAndHandsIkController.GetComponent<IkController>();
			if (this._ikController != null)
			{
				this._ikController.Initialize(ikType, animator, this._player, base.isMine);
			}
			if (ad != null)
			{
				switch (ad.lipSync)
				{
				case VRC_AvatarDescriptor.LipSyncStyle.Default:
				case VRC_AvatarDescriptor.LipSyncStyle.JawFlapBone:
				case VRC_AvatarDescriptor.LipSyncStyle.JawFlapBlendShape:
					if (this.jawController != null)
					{
						this.lipsyncController.DisableLipSyncComponents();
						this.lipsyncController.enabled = false;
						this.jawController.MouthOpenBlendShapeName = ad.MouthOpenBlendShapeName;
						this.jawController.enabled = true;
						this.jawController.Initialize(gameObject, animator, base.isMine);
					}
					break;
				case VRC_AvatarDescriptor.LipSyncStyle.VisemeBlendShape:
					if (ad.VisemeSkinnedMesh != null)
					{
						if (this.jawController != null)
						{
							this.jawController.enabled = false;
						}
						this.lipsyncController.enabled = true;
						this.lipsyncController.InitializeBlendShapes(ad.VisemeBlendShapes, gameObject, animator, ad.VisemeSkinnedMesh, base.isMine);
					}
					break;
				}
			}
			if (this.lookController != null)
			{
				if (this.lookController.Initialize(animator, this.usingVRIK))
				{
					this.lookController.enabled = true;
				}
				else
				{
					this.lookController.enabled = false;
				}
			}
			if (this._ragdollController != null)
			{
				this._ragdollController.Initialize(animator, base.isMine);
			}
			if (this._handGestureController != null && !this.usingVRIK)
			{
				this.SetupHandGestures(animator, base.isMine);
			}
		}
		if (this._player != null && animator != null && base.isMine)
		{
			this._meatHook.Attach(animator);
		}
		this.cameraMountController.Initialize(ad, animator, base.isMine);
		if (base.isMine)
		{
			if (this.muteController != null)
			{
				this.muteController.LocalMute();
			}
			else if (this.muteController != null)
			{
				this.muteController.Reset();
			}
		}
	}

	// Token: 0x06004FF6 RID: 20470 RVA: 0x001B5017 File Offset: 0x001B3417
	public void GenerateLocalHandCollision()
	{
		if (this._handGestureController != null)
		{
			this._handGestureController.GenerateLocalHandCollision();
		}
	}

	// Token: 0x06004FF7 RID: 20471 RVA: 0x001B5038 File Offset: 0x001B3438
	private void Update()
	{
		this.UpdateActiveElements();
		if (this._avatarSwitcher != null && this._ikController != null)
		{
			this.isCulled = this._avatarSwitcher.fullyCulled;
		}
		else
		{
			this.isCulled = false;
		}
		if (this._ikController != null)
		{
			this._ikController.SetCull(this.isCulled);
		}
	}

	// Token: 0x06004FF8 RID: 20472 RVA: 0x001B50AC File Offset: 0x001B34AC
	private void LateUpdate()
	{
		if (this.RunKinect)
		{
			this._kinectController.Apply();
			this.cameraMountController.manualHeadTracking = this._kinectController.CurrentHeadOffset;
			this.cameraMountController.Apply();
		}
		if (this.shouldMeatHook && base.isMine && this._meatHook.enabled && this._avatarSwitcher != null && this._avatarSwitcher.WasMeasured)
		{
			if (this.usingVRIK || this.usingFBBIK)
			{
				this._meatHook.Apply(true);
			}
			else
			{
				this._meatHook.Apply(false);
			}
		}
	}

	// Token: 0x06004FF9 RID: 20473 RVA: 0x001B516A File Offset: 0x001B356A
	public bool HasTPoseBone(HumanBodyBones bone)
	{
		return this.tPoseRotations.ContainsKey(bone);
	}

	// Token: 0x06004FFA RID: 20474 RVA: 0x001B5178 File Offset: 0x001B3578
	public Quaternion GetTPoseRotation(HumanBodyBones bone)
	{
		return this.tPoseRotations[bone];
	}

	// Token: 0x06004FFB RID: 20475 RVA: 0x001B5186 File Offset: 0x001B3586
	public Vector3 GetTPosePosition(HumanBodyBones bone)
	{
		return this.tPosePositions[bone];
	}

	// Token: 0x06004FFC RID: 20476 RVA: 0x001B5194 File Offset: 0x001B3594
	public bool GetTPoseRotation(HumanBodyBones bone, out Quaternion rotation)
	{
		if (!this.tPoseRotations.ContainsKey(bone))
		{
			rotation = Quaternion.identity;
			return false;
		}
		rotation = this.tPoseRotations[bone];
		return true;
	}

	// Token: 0x06004FFD RID: 20477 RVA: 0x001B51C7 File Offset: 0x001B35C7
	public bool GetTPosePosition(HumanBodyBones bone, out Vector3 position)
	{
		if (!this.tPosePositions.ContainsKey(bone))
		{
			position = Vector3.zero;
			return false;
		}
		position = this.tPosePositions[bone];
		return true;
	}

	// Token: 0x06004FFE RID: 20478 RVA: 0x001B51FC File Offset: 0x001B35FC
	private void RecordTPoseBone(Animator targetAnimator, Quaternion worldToLocal, HumanBodyBones bone)
	{
		Transform boneTransform = targetAnimator.GetBoneTransform(bone);
		if (boneTransform != null)
		{
			this.tPoseRotations.Add(bone, worldToLocal * boneTransform.rotation);
			this.tPosePositions.Add(bone, base.transform.InverseTransformPoint(boneTransform.position));
		}
	}

	// Token: 0x06004FFF RID: 20479 RVA: 0x001B5254 File Offset: 0x001B3654
	public void RecordTPose(Animator targetAnimator)
	{
		Quaternion worldToLocal = Quaternion.Inverse(base.transform.rotation);
		this.tPoseRotations.Clear();
		this.tPosePositions.Clear();
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.Hips);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.Spine);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.Chest);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.Neck);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.Head);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftShoulder);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftUpperArm);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftLowerArm);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftHand);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftThumbProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftThumbIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftThumbDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftIndexProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftIndexIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftIndexDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftMiddleProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftMiddleIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftMiddleDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftRingProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftRingIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftRingDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftLittleProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftLittleIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftLittleDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightShoulder);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightUpperArm);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightLowerArm);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightHand);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightThumbProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightThumbIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightThumbDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightIndexProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightIndexIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightIndexDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightMiddleProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightMiddleIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightMiddleDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightRingProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightRingIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightRingDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightLittleProximal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightLittleIntermediate);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightLittleDistal);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftUpperLeg);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftLowerLeg);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.LeftFoot);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightUpperLeg);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightLowerLeg);
		this.RecordTPoseBone(targetAnimator, worldToLocal, HumanBodyBones.RightFoot);
	}

	// Token: 0x040038B4 RID: 14516
	public bool RunKinect;

	// Token: 0x040038B5 RID: 14517
	public GameObject Kinect;

	// Token: 0x040038B6 RID: 14518
	private KinectAvatarController _kinectController;

	// Token: 0x040038B7 RID: 14519
	public GameObject HeadAndHandsIkController;

	// Token: 0x040038B8 RID: 14520
	private VRCPlayer _player;

	// Token: 0x040038B9 RID: 14521
	private Animator _animator;

	// Token: 0x040038BA RID: 14522
	private AvatarAnimation _avatarAnimation;

	// Token: 0x040038BB RID: 14523
	private VRCAvatarManager _avatarSwitcher;

	// Token: 0x040038BC RID: 14524
	private JawController jawController;

	// Token: 0x040038BD RID: 14525
	private LipSyncController lipsyncController;

	// Token: 0x040038BE RID: 14526
	private MuteController muteController;

	// Token: 0x040038BF RID: 14527
	private EyeLookController lookController;

	// Token: 0x040038C0 RID: 14528
	private CameraMountController cameraMountController;

	// Token: 0x040038C1 RID: 14529
	private RagdollController _ragdollController;

	// Token: 0x040038C2 RID: 14530
	private VRCVrIkController _vrcVrIkController;

	// Token: 0x040038C3 RID: 14531
	private VRCFbbIkController _vrcFbbIkController;

	// Token: 0x040038C4 RID: 14532
	private IkController _ikController;

	// Token: 0x040038C5 RID: 14533
	private EmotePlayer _emotePlayer;

	// Token: 0x040038C6 RID: 14534
	public Transform avatarRoot;

	// Token: 0x040038C7 RID: 14535
	public Transform _cameraMount;

	// Token: 0x040038C8 RID: 14536
	public bool shouldMeatHook = true;

	// Token: 0x040038C9 RID: 14537
	private MeatHook _meatHook;

	// Token: 0x040038CA RID: 14538
	private HandGestureController _handGestureController;

	// Token: 0x040038CB RID: 14539
	private bool usingVRIK;

	// Token: 0x040038CC RID: 14540
	private bool usingFBBIK;

	// Token: 0x040038CD RID: 14541
	private global::LimbIK limbik;

	// Token: 0x040038CE RID: 14542
	private VRIK vrik;

	// Token: 0x040038CF RID: 14543
	private FullBodyBipedIK fbbik;

	// Token: 0x040038D0 RID: 14544
	private VRCMotionState motionState;

	// Token: 0x040038D2 RID: 14546
	private Dictionary<HumanBodyBones, Quaternion> tPoseRotations = new Dictionary<HumanBodyBones, Quaternion>();

	// Token: 0x040038D3 RID: 14547
	private Dictionary<HumanBodyBones, Vector3> tPosePositions = new Dictionary<HumanBodyBones, Vector3>();

	// Token: 0x040038D4 RID: 14548
	public bool ragDolled;

	// Token: 0x040038D5 RID: 14549
	public bool oughtToVRIK;

	// Token: 0x040038D6 RID: 14550
	public bool oughtToLimbIK;
}
