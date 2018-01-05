using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Rendering;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B31 RID: 2865
public class VRCAvatarManager : VRCPunBehaviour
{
	// Token: 0x17000CA9 RID: 3241
	// (get) Token: 0x06005762 RID: 22370 RVA: 0x001E1A8C File Offset: 0x001DFE8C
	// (set) Token: 0x06005763 RID: 22371 RVA: 0x001E1A94 File Offset: 0x001DFE94
	public bool fullyCulled { get; private set; }

	// Token: 0x17000CAA RID: 3242
	// (get) Token: 0x06005764 RID: 22372 RVA: 0x001E1A9D File Offset: 0x001DFE9D
	public bool IsHuman
	{
		get
		{
			return !this._nonHuman;
		}
	}

	// Token: 0x17000CAB RID: 3243
	// (get) Token: 0x06005765 RID: 22373 RVA: 0x001E1AA8 File Offset: 0x001DFEA8
	// (set) Token: 0x06005766 RID: 22374 RVA: 0x001E1AB0 File Offset: 0x001DFEB0
	public bool WasMeasured { get; private set; }

	// Token: 0x17000CAC RID: 3244
	// (get) Token: 0x06005767 RID: 22375 RVA: 0x001E1AB9 File Offset: 0x001DFEB9
	public ApiAvatar CurrentAvatar
	{
		get
		{
			return this.lastAvatar;
		}
	}

	// Token: 0x06005768 RID: 22376 RVA: 0x001E1AC1 File Offset: 0x001DFEC1
	public override void Awake()
	{
		base.Awake();
		if (AssetManagement.SceneHasLightProbes)
		{
			this.useLightProbes = LightProbeUsage.BlendProbes;
		}
		else
		{
			this.useLightProbes = LightProbeUsage.Off;
		}
		this._vrcPlayer = base.GetComponentInParent<VRCPlayer>();
		this._avatarPedestal = base.GetComponent<AvatarPedestal>();
	}

	// Token: 0x06005769 RID: 22377 RVA: 0x001E1B00 File Offset: 0x001DFF00
	public override IEnumerator Start()
	{
		yield return base.Start();
		if (this.proxyObject != null)
		{
			this.proxyObject.GetComponent<MeshRenderer>().material.renderQueue = 3997;
		}
		yield break;
	}

	// Token: 0x0600576A RID: 22378 RVA: 0x001E1B1B File Offset: 0x001DFF1B
	protected override void OnNetworkReady()
	{
		base.OnNetworkReady();
		if (VRC.Network.IsOwner(base.gameObject))
		{
			this.profile.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600576B RID: 22379 RVA: 0x001E1B44 File Offset: 0x001DFF44
	private void hideProgressBar()
	{
		if (this.progressBar != null)
		{
			this.progressBar.Hide();
		}
	}

	// Token: 0x0600576C RID: 22380 RVA: 0x001E1B62 File Offset: 0x001DFF62
	private void showProgressBar()
	{
		if (this.progressBar != null && this.localAvatarVisibility != VRCAvatarManager.LocalAvatarVisibility.Invisible)
		{
			this.progressBar.Show();
		}
	}

	// Token: 0x0600576D RID: 22381 RVA: 0x001E1B8C File Offset: 0x001DFF8C
	public void SetLastApiAvatar(ApiAvatar a)
	{
		this.lastAvatar = a;
	}

	// Token: 0x0600576E RID: 22382 RVA: 0x001E1B98 File Offset: 0x001DFF98
	public bool SwitchAvatar(ApiAvatar apiAvatar, float scale = 1f, VRCAvatarManager.AvatarCreationCallback onAvatarLoaded = null)
	{
		if (apiAvatar == null)
		{
			return false;
		}
		VRC.Core.Logger.Log("Switching avatar to " + apiAvatar.assetUrl, DebugLevel.All);
		this.hideProgressBar();
		try
		{
			if (!this._atLeastOneAvatarWasLoaded || VRC.Network.IsOwner(base.gameObject))
			{
				this.SwitchToLoadingAvatar(scale);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		this.DownloadCustomAvatar(apiAvatar, scale, onAvatarLoaded);
		this._fromPrefab = false;
		return true;
	}

	// Token: 0x0600576F RID: 22383 RVA: 0x001E1C20 File Offset: 0x001E0020
	private void SwitchToPrefabAvatar(GameObject prefab, string name, float scale = 1f, Action onSuccess = null, Action onError = null)
	{
		this._fromPrefab = true;
		this.AttachAvatar(prefab, name, scale, onSuccess, onError);
	}

	// Token: 0x06005770 RID: 22384 RVA: 0x001E1C36 File Offset: 0x001E0036
	public void SwitchToLastAvatar(float scale = 1f)
	{
		if (this._fromPrefab)
		{
			this.currentAvatarKind = VRCAvatarManager.AvatarKind.Custom;
			this.SwitchAvatar(this.lastAvatar, scale, this.OnLastAvatarLoaded);
		}
	}

	// Token: 0x06005771 RID: 22385 RVA: 0x001E1C60 File Offset: 0x001E0060
	public void SwitchToErrorAvatar(float scale = 1f)
	{
		if (this.currentAvatarKind == VRCAvatarManager.AvatarKind.Error)
		{
			return;
		}
		this.SwitchToPrefabAvatar(this.errorAvatarPrefab, "error", scale, delegate
		{
		}, delegate
		{
			Debug.LogError("FATAL ERROR: Couldn't even switch to error avatar!");
		});
		this.currentAvatarKind = VRCAvatarManager.AvatarKind.Error;
	}

	// Token: 0x06005772 RID: 22386 RVA: 0x001E1CD0 File Offset: 0x001E00D0
	public void SwitchToLoadingAvatar(float scale = 1f)
	{
		if (this.currentAvatarKind == VRCAvatarManager.AvatarKind.Loading)
		{
			return;
		}
		this.currentAvatarKind = VRCAvatarManager.AvatarKind.Loading;
		this.SwitchToPrefabAvatar(this.tempAvatarPrefab, "loading", scale, delegate
		{
		}, delegate
		{
			Debug.LogError("Failed to switch to LOADING avatar!");
		});
	}

	// Token: 0x06005773 RID: 22387 RVA: 0x001E1D40 File Offset: 0x001E0140
	public void SwitchToBlockedAvatar(float scale = 1f)
	{
		if (this.currentAvatarKind == VRCAvatarManager.AvatarKind.Blocked)
		{
			return;
		}
		this.currentAvatarKind = VRCAvatarManager.AvatarKind.Blocked;
		this.SwitchToPrefabAvatar(this.blockedAvatarPrefab, "blocked", scale, delegate
		{
		}, delegate
		{
			Debug.LogError("Failed to switch to BLOCKED avatar!");
		});
	}

	// Token: 0x06005774 RID: 22388 RVA: 0x001E1DB0 File Offset: 0x001E01B0
	public void SwitchToInvisibleAvatar(float scale = 1f)
	{
		if (this.currentAvatarKind == VRCAvatarManager.AvatarKind.Invisible)
		{
			return;
		}
		this.currentAvatarKind = VRCAvatarManager.AvatarKind.Invisible;
		this.SwitchToPrefabAvatar(this.invisibleAvatarPrefab, "invisible", scale, delegate
		{
		}, delegate
		{
			Debug.LogError("Failed to switch to INVISIBLE avatar!");
		});
	}

	// Token: 0x06005775 RID: 22389 RVA: 0x001E1E20 File Offset: 0x001E0220
	public void SwitchToFallbackAvatar(float scale = 1f, VRCAvatarManager.AvatarCreationCallback onAvatarLoaded = null)
	{
		ApiAvatar.Fetch(this.fallbackAvatarId, delegate(ApiAvatar avatar)
		{
			this.SwitchAvatar(avatar, scale, onAvatarLoaded);
		}, delegate(string message)
		{
			this.SwitchToErrorAvatar(1f);
		});
	}

	// Token: 0x06005776 RID: 22390 RVA: 0x001E1E6C File Offset: 0x001E026C
	private void AttachAvatar(UnityEngine.Object Prefab, string Name, float Scale = 1f, Action onSuccess = null, Action onError = null)
	{
		if (Prefab == null)
		{
			return;
		}
		if (!this.CanAttachAvatars())
		{
			Debug.Log("Deferring avatar load: " + Name + " from object " + base.name);
			base.StartCoroutine(this.DeferWhile(() => !this.CanAttachAvatars(), delegate
			{
				this.AttachAvatar(Prefab, Name, Scale, onSuccess, onError);
			}));
			return;
		}
		if (Prefab == this.errorAvatarPrefab)
		{
			VRCLog.UploadMiniLog("Failed to load avatar!");
		}
		try
		{
			this.AttachAvatarInternal(Prefab, Name, Scale);
			if (onSuccess != null)
			{
				onSuccess();
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex, base.gameObject);
			Analytics.Send(ApiAnalyticEvent.EventType.error, "Avatar Loading Exception: " + Name + " - " + ex.ToString(), null, null);
			if (onError != null)
			{
				onError();
			}
		}
	}

	// Token: 0x06005777 RID: 22391 RVA: 0x001E1FC8 File Offset: 0x001E03C8
	private bool CanAttachAvatars()
	{
		return VRC.Network.SceneEventHandler != null;
	}

	// Token: 0x06005778 RID: 22392 RVA: 0x001E1FD8 File Offset: 0x001E03D8
	private IEnumerator DeferWhile(Func<bool> predicate, Action todo)
	{
		while (predicate())
		{
			yield return null;
		}
		todo();
		yield break;
	}

	// Token: 0x06005779 RID: 22393 RVA: 0x001E1FFC File Offset: 0x001E03FC
	private AnimatorOverrideController MergeAnims(string name, AnimatorOverrideController defaults, AnimatorOverrideController changes)
	{
		if (defaults != null && changes != null && this.animatorControllerTemplate != null)
		{
			AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(this.animatorControllerTemplate);
			for (int i = 0; i < this.animatorControllerTemplate.animationClips.Length; i++)
			{
				string name2 = this.animatorControllerTemplate.animationClips[i].name;
				AnimationClip animationClip = changes[name2];
				AnimationClip animationClip2 = defaults[name2];
				if (animationClip != null && name2 != animationClip.name)
				{
					animatorOverrideController[name2] = animationClip;
				}
				else if (animationClip2 != null)
				{
					animatorOverrideController[name2] = animationClip2;
				}
			}
			animatorOverrideController.name = name;
			return animatorOverrideController;
		}
		return defaults;
	}

	// Token: 0x0600577A RID: 22394 RVA: 0x001E20CC File Offset: 0x001E04CC
	public void MeasureHumanAvatar(Animator A)
	{
		this.headTransform = A.GetBoneTransform(HumanBodyBones.Head);
		Transform transform = base.transform;
		A.transform.localPosition = Vector3.zero;
		transform.localPosition = Vector3.zero;
		VRC_AnimationController vrc_AnimationController = null;
		if (this._vrcPlayer != null)
		{
			vrc_AnimationController = this._vrcPlayer.GetComponentInChildren<VRC_AnimationController>();
		}
		float a = 0f;
		float b = 0f;
		Transform boneTransform = A.GetBoneTransform(HumanBodyBones.LeftHand);
		Transform boneTransform2 = A.GetBoneTransform(HumanBodyBones.RightHand);
		if (boneTransform)
		{
			a = (this.headTransform.position - boneTransform.position).magnitude;
		}
		if (boneTransform2)
		{
			b = (this.headTransform.position - boneTransform2.position).magnitude;
		}
		float num = Mathf.Max(a, b);
		if (num > 0f)
		{
			this.currentAvatarArmLength = num;
		}
		else
		{
			this.currentAvatarArmLength = VRCTracking.DefaultArmLength;
		}
		Vector3 viewPosition = this.currentAvatarDescriptor.ViewPosition;
		this.currentAvatarEyeHeight = viewPosition.y;
		Vector3 headToViewPointOffset = viewPosition - transform.InverseTransformPoint(this.headTransform.position);
		headToViewPointOffset.z = -headToViewPointOffset.z;
		if (this.localPlayer)
		{
			VRCTrackingManager.SetAvatarViewPoint(viewPosition, headToViewPointOffset);
		}
		this.headScale = this.headTransform.localScale;
		GameObject gameObject = new GameObject("HmdPivot");
		gameObject.transform.position = transform.TransformPoint(new Vector3(viewPosition.x, viewPosition.y, viewPosition.z));
		gameObject.transform.SetParent(this.headTransform, true);
		gameObject.transform.rotation = transform.rotation;
		if (vrc_AnimationController != null)
		{
			this.maxHeadRadius = 0.5f * this.currentAvatarArmLength / VRCTracking.DefaultArmLength;
			IkController component = vrc_AnimationController.HeadAndHandsIkController.GetComponent<IkController>();
			component.InitHeadEffector(A, this.localPlayer);
		}
		this.WasMeasured = true;
		if (this.localPlayer)
		{
			VRCTrackingManager.AdjustViewPositionToAvatar();
			Vector3 neckToEyeOffset = base.transform.InverseTransformVector(gameObject.transform.position - this.headTransform.position);
			VRCVrCamera.GetInstance().SetNeckToEyeOffset(neckToEyeOffset);
		}
	}

	// Token: 0x0600577B RID: 22395 RVA: 0x001E2320 File Offset: 0x001E0720
	private void AttachAvatarInternal(UnityEngine.Object Prefab, string Name, float Scale = 1f)
	{
		this.WasMeasured = false;
		if (this == null || base.gameObject == null || base.GetComponent<Transform>() == null)
		{
			Debug.LogError("AttachAvatarInternal: VRCAvatarManager was destroyed, could not attach avatar: " + Name);
			return;
		}
		GameObject gameObject = AssetManagement.Instantiate<GameObject>(Prefab);
		if (gameObject == null || gameObject.GetComponent<Transform>() == null)
		{
			Debug.LogError("Could not instantiate avatar " + Name);
			return;
		}
		AvatarValidation.RemoveIllegalComponents(Name, gameObject, true);
		AvatarValidation.EnforceAudioSourceLimits(gameObject);
		if (!this.localPlayer)
		{
			foreach (Camera camera in gameObject.GetComponentsInChildren<Camera>())
			{
				Debug.LogWarning("Removing camera from " + camera.gameObject.name);
				UnityEngine.Object.Destroy(camera);
			}
		}
		int num = 2;
		VRC.Network.AssignNetworkIDsToObject(gameObject, false, VRC.Network.GetOwnerId(base.gameObject).Value, ref num);
		VRC.Network.IsObjectReady(gameObject);
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
		gameObject.transform.localScale *= Scale;
		VRC_AvatarDescriptor component = gameObject.GetComponent<VRC_AvatarDescriptor>();
		if (component == null)
		{
			Debug.LogError("Avatar Missing Descriptor");
			Analytics.Send(ApiAnalyticEvent.EventType.error, "Avatar Missing Descriptor: " + Name, null, null);
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		if (gameObject.GetComponent<Animator>() == null)
		{
			Debug.Log("Avatar Missing Animator");
			if (base.transform.parent != null)
			{
				Debug.Log("Avatar player object name=" + base.transform.parent.name);
			}
			Analytics.Send(ApiAnalyticEvent.EventType.error, "Avatar Missing Animator: " + Name, null, null);
		}
		if (this._avatarMirrorClone != null)
		{
			this.DestroyAvatarRenderClone();
		}
		if (this.currentAvatar != null)
		{
			VRC_EventHandler[] componentsInChildren2 = this.currentAvatar.GetComponentsInChildren<VRC_EventHandler>();
			foreach (VRC_EventHandler vrc_EventHandler in componentsInChildren2)
			{
				vrc_EventHandler.Unregister();
			}
			if (this.currentAvatar.GetComponent<Transform>() != null)
			{
				this.currentAvatar.GetComponent<Transform>().parent = null;
			}
			UnityEngine.Object.Destroy(this.currentAvatar);
		}
		this.currentAvatar = gameObject;
		this.currentAvatarDescriptor = component;
		AudioListener[] componentsInChildren3 = gameObject.GetComponentsInChildren<AudioListener>();
		foreach (AudioListener audioListener in componentsInChildren3)
		{
			audioListener.enabled = false;
		}
		if (!this.localPlayer)
		{
			Camera[] componentsInChildren4 = gameObject.GetComponentsInChildren<Camera>();
			foreach (Camera camera2 in componentsInChildren4)
			{
				camera2.enabled = false;
			}
		}
		AudioListener[] componentsInChildren5 = gameObject.GetComponentsInChildren<AudioListener>();
		foreach (AudioListener audioListener2 in componentsInChildren5)
		{
			audioListener2.enabled = false;
		}
		SkinnedMeshRenderer[] componentsInChildren6 = this.currentAvatar.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren6)
		{
			skinnedMeshRenderer.updateWhenOffscreen = true;
		}
		this.visibleElements = gameObject.GetComponentsInChildren<Renderer>(true);
		this.visibleElementEnable = new bool[this.visibleElements.Length];
		for (int num2 = 0; num2 < this.visibleElements.Length; num2++)
		{
			this.visibleElementEnable[num2] = this.visibleElements[num2].enabled;
		}
		List<IVRCCullable> list = new List<IVRCCullable>();
		foreach (DynamicBone item in gameObject.GetComponentsInChildren<DynamicBone>(true))
		{
			list.Add(item);
		}
		this.cullableElements = list.ToArray();
		this._laserSelectRegion = null;
		if (!this.localPlayer && this._vrcPlayer != null)
		{
			Transform transform = this._vrcPlayer.transform.Find("SelectRegion");
			if (transform != null)
			{
				this._laserSelectRegion = transform.gameObject.GetComponent<PlayerSelector>();
			}
		}
		Animator component2 = this.currentAvatar.GetComponent<Animator>();
		if (component2 != null && component2.avatar != null)
		{
			if (component2.isHuman)
			{
				this._nonHuman = false;
				this.currentAvatar.GetOrAddComponent<VRIK>();
				this.currentAvatar.GetOrAddComponent<FullBodyBipedIK>();
				this.currentAvatar.GetOrAddComponent<global::LimbIK>();
				component2.runtimeAnimatorController = null;
				if (component.Animations == VRC_AvatarDescriptor.AnimationSet.Female)
				{
					this._useFemaleAnims = true;
				}
				else
				{
					this._useFemaleAnims = false;
				}
				AnimatorOverrideController defaults = this.animatorControllerMale;
				if (this._useFemaleAnims)
				{
					defaults = this.animatorControllerFemale;
				}
				AnimatorOverrideController defaults2 = this.animatorControllerMaleSitting;
				if (this._useFemaleAnims)
				{
					defaults2 = this.animatorControllerFemaleSitting;
				}
				this.currentStandingAnims = this.MergeAnims("CustomStanding", defaults, component.CustomStandingAnims);
				this.currentSittingAnims = this.MergeAnims("CustomSitting", defaults2, component.CustomSittingAnims);
				component2.runtimeAnimatorController = this.currentStandingAnims;
				component2.updateMode = AnimatorUpdateMode.Normal;
				if (this.currentStandingAnims == null)
				{
					Debug.LogError(((!(this._vrcPlayer != null)) ? base.gameObject.name : this._vrcPlayer.name) + " has human avatar with broken standing animations.", this);
					this.currentStandingAnims = this.animatorControllerMale;
				}
				else if (this.currentSittingAnims == null)
				{
					Debug.LogError(((!(this._vrcPlayer != null)) ? base.gameObject.name : this._vrcPlayer.name) + " has human avatar with broken sitting animations.", this);
					this.currentSittingAnims = this.animatorControllerMaleSitting;
				}
			}
			else
			{
				this._nonHuman = true;
				if (this.localPlayer)
				{
					VRCTrackingManager.SetAvatarViewPoint(this.currentAvatarDescriptor.ViewPosition, Vector3.zero);
				}
				this.currentAvatarArmLength = VRCTracking.DefaultArmLength;
				this.currentAvatarEyeHeight = this.currentAvatarDescriptor.ViewPosition.y;
				if (this.localPlayer)
				{
					VRCTrackingManager.AdjustViewPositionToAvatar();
				}
			}
			if (this.localPlayer)
			{
				component2.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			}
			else
			{
				component2.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			}
			component2.applyRootMotion = false;
		}
		else
		{
			this._nonHuman = true;
			if (this.localPlayer)
			{
				VRCTrackingManager.SetAvatarViewPoint(this.currentAvatarDescriptor.ViewPosition, Vector3.zero);
			}
			this.currentAvatarArmLength = VRCTracking.DefaultArmLength;
			this.currentAvatarEyeHeight = this.currentAvatarDescriptor.ViewPosition.y;
			if (this.localPlayer)
			{
				VRCTrackingManager.AdjustViewPositionToAvatar();
			}
		}
		Transform transform2 = base.transform.Find("../CameraMount");
		if (transform2 != null && base.transform.parent != null)
		{
			transform2.position = base.transform.parent.TransformPoint(new Vector3(this.currentAvatarDescriptor.ViewPosition.x / base.transform.parent.localScale.x, this.currentAvatarDescriptor.ViewPosition.y / base.transform.parent.localScale.y, 0f));
		}
		MeshRenderer[] componentsInChildren8 = this.currentAvatar.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren8)
		{
			meshRenderer.lightProbeUsage = this.useLightProbes;
		}
		SkinnedMeshRenderer[] componentsInChildren9 = this.currentAvatar.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in componentsInChildren9)
		{
			skinnedMeshRenderer2.lightProbeUsage = this.useLightProbes;
			skinnedMeshRenderer2.updateWhenOffscreen = this.localPlayer;
		}
		Vector3 localPosition = new Vector3(0f, this.currentAvatarDescriptor.ViewPosition.y + 0.5f, 0f);
		if (this.profile != null)
		{
			this.profile.localPosition = localPosition;
		}
		if (this.old_profile != null)
		{
			this.old_profile.localPosition = localPosition;
		}
		if (this.localPlayer)
		{
			Tools.SetLayerRecursively(gameObject, LayerMask.NameToLayer("PlayerLocal"), LayerMask.NameToLayer("UiMenu"));
		}
		else
		{
			Tools.SetLayerRecursively(gameObject, LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("UiMenu"));
		}
		if (this.localPlayer)
		{
			this.InitAvatarRenderClone();
		}
		this._isAttached = true;
		this.localAvatarVisibility = VRCAvatarManager.LocalAvatarVisibility.Visible;
		this.RefreshLocalAvatarVisibility();
		if (this.OnAvatarCreated != null)
		{
			this.OnAvatarCreated(this.currentAvatar, this.currentAvatarDescriptor, false);
		}
	}

	// Token: 0x0600577C RID: 22396 RVA: 0x001E2C9A File Offset: 0x001E109A
	private void InitAvatarRenderClone()
	{
		this.DestroyAvatarRenderClone();
		if (this.currentAvatar != null)
		{
			this._avatarMirrorClone = this.CreateAvatarClone("_AvatarMirrorClone", AvatarCloneType.Mirror);
			this._avatarShadowClone = this.CreateAvatarClone("_AvatarShadowClone", AvatarCloneType.Shadow);
		}
	}

	// Token: 0x0600577D RID: 22397 RVA: 0x001E2CD8 File Offset: 0x001E10D8
	private GameObject CreateAvatarClone(string name, AvatarCloneType type)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.currentAvatar);
		AvatarValidation.RemoveIllegalComponents(name, gameObject, true);
		gameObject.name = name;
		gameObject.transform.parent = this.currentAvatar.transform.parent;
		gameObject.transform.position = this.currentAvatar.transform.position;
		gameObject.transform.rotation = this.currentAvatar.transform.rotation;
		AvatarClone avatarClone = gameObject.AddComponent<AvatarClone>();
		avatarClone.OriginalAvatar = this.currentAvatar;
		avatarClone.CloneType = type;
		avatarClone.StripComponents();
		Tools.SetLayerRecursively(gameObject, (type != AvatarCloneType.Mirror) ? LayerMask.NameToLayer("PlayerLocal") : LayerMask.NameToLayer("MirrorReflection"), -1);
		return gameObject;
	}

	// Token: 0x0600577E RID: 22398 RVA: 0x001E2D98 File Offset: 0x001E1198
	private void DestroyAvatarRenderClone()
	{
		if (this._avatarMirrorClone != null)
		{
			UnityEngine.Object.Destroy(this._avatarMirrorClone);
		}
		this._avatarMirrorClone = null;
		if (this._avatarShadowClone != null)
		{
			UnityEngine.Object.Destroy(this._avatarShadowClone);
		}
		this._avatarShadowClone = null;
	}

	// Token: 0x0600577F RID: 22399 RVA: 0x001E2DEC File Offset: 0x001E11EC
	private void DownloadCustomAvatar(ApiAvatar apiAvatar, float scale, VRCAvatarManager.AvatarCreationCallback onAvatarLoaded)
	{
		this.showProgressBar();
		this.currentScale = scale;
		Downloader.DownloadAssetBundle(apiAvatar, new OnDownloadProgressDelegate(this.OnDownloadProgress), delegate(string assetUrl, AssetBundleDownload download)
		{
			this.LoadAvatarFromAssetBundle(assetUrl, download.asset);
			this.lastAvatar = apiAvatar;
			try
			{
				this.currentAvatarKind = VRCAvatarManager.AvatarKind.Custom;
				if (onAvatarLoaded != null)
				{
					if (this.currentAvatar != null)
					{
						onAvatarLoaded(this.currentAvatar, this.currentAvatar.GetComponent<VRC_AvatarDescriptor>(), true);
					}
					else
					{
						onAvatarLoaded(null, null, false);
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this._atLeastOneAvatarWasLoaded = true;
		}, delegate(string url, string message, LoadErrorReason reason)
		{
			try
			{
				this.currentAvatarKind = VRCAvatarManager.AvatarKind.Custom;
				if (onAvatarLoaded != null)
				{
					if (this.currentAvatar != null)
					{
						onAvatarLoaded(this.currentAvatar, this.currentAvatar.GetComponent<VRC_AvatarDescriptor>(), false);
					}
					else
					{
						onAvatarLoaded(null, null, false);
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this.OnDownloadError(url, message, reason);
		}, AssetBundleDownloadManager.UnpackType.Async);
	}

	// Token: 0x06005780 RID: 22400 RVA: 0x001E2E54 File Offset: 0x001E1254
	private void OnDownloadProgress(HTTPRequest request, int downloaded, int length)
	{
		if (this.progressBar == null)
		{
			return;
		}
		float num = (float)downloaded / (float)length;
		this.progressBar.progressBarUI.value = num;
		num *= 100f;
		this.progressBar.percentLabel.text = num.ToString() + "%";
	}

	// Token: 0x06005781 RID: 22401 RVA: 0x001E2EBC File Offset: 0x001E12BC
	private void LoadAvatarFromAssetBundle(string url, UnityEngine.Object asset)
	{
		GameObject gameObject = asset as GameObject;
		VRC_AvatarDescriptor component = gameObject.GetComponent<VRC_AvatarDescriptor>();
		if (component != null)
		{
			this.LoadAvatarFromAsset(url, asset, this.currentScale);
		}
		else
		{
			this.SwitchToErrorAvatar(1f);
			Debug.LogWarning("Attempting to load a non-custom-avatar asset into an avatar asset. www.assetBundle.mainAsset.name = " + asset.name + " from url " + url);
		}
	}

	// Token: 0x06005782 RID: 22402 RVA: 0x001E2F1C File Offset: 0x001E131C
	private void LoadAvatarFromAsset(string url, UnityEngine.Object asset, float scale)
	{
		this.AttachAvatar(asset, url, scale, delegate
		{
			if (this.progressBar != null)
			{
				this.hideProgressBar();
			}
		}, delegate
		{
			this.SwitchToErrorAvatar(scale);
			if (this.progressBar != null)
			{
				this.hideProgressBar();
			}
		});
	}

	// Token: 0x06005783 RID: 22403 RVA: 0x001E2F63 File Offset: 0x001E1363
	private void OnDownloadError(string url, string message, LoadErrorReason reason)
	{
		this.SwitchToErrorAvatar(1f);
		Debug.Log("Error downloading avatar [" + url + "] - " + message);
	}

	// Token: 0x06005784 RID: 22404 RVA: 0x001E2F88 File Offset: 0x001E1388
	private void RefreshLocalAvatarVisibility()
	{
		bool flag = this._vrcPlayer != null && (this._vrcPlayer.isLocal || (!this._isInsidePersonalSpace && !this._vrcPlayer.isInvisible));
		if (this._vrcPlayer == null)
		{
			flag = true;
		}
		this.SetLocalAvatarVisibility((!flag) ? VRCAvatarManager.LocalAvatarVisibility.Invisible : VRCAvatarManager.LocalAvatarVisibility.Visible);
	}

	// Token: 0x06005785 RID: 22405 RVA: 0x001E3000 File Offset: 0x001E1400
	private void SetLocalAvatarVisibility(VRCAvatarManager.LocalAvatarVisibility vis)
	{
		if (this.localAvatarVisibility == VRCAvatarManager.LocalAvatarVisibility.Visible && vis == VRCAvatarManager.LocalAvatarVisibility.Invisible)
		{
			for (int i = 0; i < this.visibleElements.Length; i++)
			{
				this.visibleElementEnable[i] = this.visibleElements[i].enabled;
				this.visibleElements[i].enabled = false;
			}
			this.EnableSelectionCapsule(false);
			if (this.currentAvatarKind == VRCAvatarManager.AvatarKind.Loading)
			{
				this.hideProgressBar();
			}
		}
		else if (this.localAvatarVisibility == VRCAvatarManager.LocalAvatarVisibility.Invisible && vis == VRCAvatarManager.LocalAvatarVisibility.Visible)
		{
			for (int j = 0; j < this.visibleElements.Length; j++)
			{
				this.visibleElements[j].enabled = this.visibleElementEnable[j];
			}
			this.EnableSelectionCapsule(true);
			if (this.currentAvatarKind == VRCAvatarManager.AvatarKind.Loading)
			{
				this.showProgressBar();
			}
		}
		else if (this.proxyObject != null)
		{
			if (vis == VRCAvatarManager.LocalAvatarVisibility.Proxy)
			{
				this.proxyObject.SetActive(true);
			}
			else
			{
				this.proxyObject.SetActive(false);
			}
		}
		this.localAvatarVisibility = vis;
	}

	// Token: 0x06005786 RID: 22406 RVA: 0x001E3113 File Offset: 0x001E1513
	private void EnableSelectionCapsule(bool enable)
	{
		if (this._laserSelectRegion == null)
		{
			return;
		}
		this._laserSelectRegion.NotifyAvatarIsVisible(enable);
	}

	// Token: 0x06005787 RID: 22407 RVA: 0x001E3134 File Offset: 0x001E1534
	private void HeadChop()
	{
		if (this.DisableHeadChop)
		{
			return;
		}
		Vector3 position = this.headTransform.position;
		Vector3 worldCameraPos = VRCVrCamera.GetInstance().GetWorldCameraPos();
		if (Vector3.Distance(position, worldCameraPos) < this.maxHeadRadius)
		{
			this.headTransform.localScale = Vector3.zero;
		}
		else
		{
			this.headTransform.localScale = this.headScale;
		}
	}

	// Token: 0x06005788 RID: 22408 RVA: 0x001E319C File Offset: 0x001E159C
	public void RestoreHeadScale()
	{
		if (this.headTransform != null)
		{
			this.headTransform.localScale = this.headScale;
		}
	}

	// Token: 0x06005789 RID: 22409 RVA: 0x001E31C0 File Offset: 0x001E15C0
	private void Update()
	{
		if (!this._isAttached)
		{
			return;
		}
		this.RestoreHeadScale();
		bool flag = this.TotallyCulled();
		if (flag != this.fullyCulled)
		{
			foreach (IVRCCullable ivrccullable in this.cullableElements)
			{
				ivrccullable.CullFunction(flag);
			}
			this.fullyCulled = flag;
		}
	}

	// Token: 0x0600578A RID: 22410 RVA: 0x001E3220 File Offset: 0x001E1620
	private void LateUpdate()
	{
		if (!this._isAttached)
		{
			return;
		}
		if (this.localPlayer)
		{
			if (this.headTransform == null || !this.WasMeasured)
			{
				return;
			}
			this.HeadChop();
		}
		else
		{
			this.CheckSafetyBubble();
			this.RefreshLocalAvatarVisibility();
		}
	}

	// Token: 0x0600578B RID: 22411 RVA: 0x001E3278 File Offset: 0x001E1678
	public float GetAvatarArmLength()
	{
		if (this.IsHuman && !this.WasMeasured)
		{
			Debug.LogError("Asked for arm length before measured.");
		}
		return this.currentAvatarArmLength;
	}

	// Token: 0x0600578C RID: 22412 RVA: 0x001E32A0 File Offset: 0x001E16A0
	public float GetAvatarEyeHeight()
	{
		if (this.IsHuman && !this.WasMeasured)
		{
			Debug.LogError("Asked for eye height before measured.");
		}
		return this.currentAvatarEyeHeight;
	}

	// Token: 0x0600578D RID: 22413 RVA: 0x001E32C8 File Offset: 0x001E16C8
	public RuntimeAnimatorController GetSitAnimController()
	{
		if (this._nonHuman)
		{
			return null;
		}
		return this.currentSittingAnims;
	}

	// Token: 0x0600578E RID: 22414 RVA: 0x001E32DD File Offset: 0x001E16DD
	public RuntimeAnimatorController GetStandAnimController()
	{
		if (this._nonHuman)
		{
			return null;
		}
		return this.currentStandingAnims;
	}

	// Token: 0x0600578F RID: 22415 RVA: 0x001E32F2 File Offset: 0x001E16F2
	public bool IsFemale()
	{
		return this._useFemaleAnims;
	}

	// Token: 0x06005790 RID: 22416 RVA: 0x001E32FC File Offset: 0x001E16FC
	private void CheckSafetyBubble()
	{
		if (this.currentAvatar == null || this.currentAvatarDescriptor == null)
		{
			return;
		}
		if (this._avatarPedestal != null)
		{
			return;
		}
		if (this._vrcPlayer == null)
		{
			return;
		}
		if (!VRCInputManager.personalSpace)
		{
			this.SetIsInsidePersonalSpace(false);
			return;
		}
		if (APIUser.IsFriendsWith(this._vrcPlayer.player.userId))
		{
			this.SetIsInsidePersonalSpace(false);
			return;
		}
		float y = this.currentAvatarDescriptor.ViewPosition.y;
		float num = Mathf.Lerp(this.MinBubbleRadius, this.MaxBubbleRadius, Mathf.InverseLerp(2f, 4f, y));
		float capsuleHeight = y + num * 2f;
		Vector3 capsuleCenter = new Vector3(0f, y / 2f, 0f);
		Vector3 worldCameraPos = VRCVrCamera.GetInstance().GetWorldCameraPos();
		bool flag = false;
		PhysicsUtil.ClosestPointOnCapsule(base.transform.localToWorldMatrix, capsuleCenter, capsuleHeight, num, 1, worldCameraPos, ref flag);
		bool flag2 = false;
		if (flag)
		{
			this.SetIsInsidePersonalSpace(true);
			flag2 = true;
		}
		else
		{
			Animator component = this.currentAvatar.GetComponent<Animator>();
			if (component != null && component.isHuman)
			{
				Transform boneTransform = component.GetBoneTransform(HumanBodyBones.LeftHand);
				if (boneTransform != null && Vector3.SqrMagnitude(boneTransform.position - worldCameraPos) <= this.HandBubbleRadius * this.HandBubbleRadius)
				{
					this.SetIsInsidePersonalSpace(true);
					flag2 = true;
				}
				if (!flag2)
				{
					Transform boneTransform2 = component.GetBoneTransform(HumanBodyBones.RightHand);
					if (boneTransform2 != null && Vector3.SqrMagnitude(boneTransform2.position - worldCameraPos) <= this.HandBubbleRadius * this.HandBubbleRadius)
					{
						this.SetIsInsidePersonalSpace(true);
						flag2 = true;
					}
				}
			}
		}
		if (!flag2)
		{
			this.SetIsInsidePersonalSpace(false);
		}
	}

	// Token: 0x06005791 RID: 22417 RVA: 0x001E34E3 File Offset: 0x001E18E3
	private void SetIsInsidePersonalSpace(bool isInside)
	{
		if (isInside != this._isInsidePersonalSpace)
		{
			this._isInsidePersonalSpace = isInside;
			this.RefreshLocalAvatarVisibility();
		}
	}

	// Token: 0x06005792 RID: 22418 RVA: 0x001E3500 File Offset: 0x001E1900
	private bool TotallyCulled()
	{
		if (this.localPlayer)
		{
			return false;
		}
		foreach (Renderer renderer in this.visibleElements)
		{
			if (renderer.isVisible)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04003E50 RID: 15952
	public GameObject tempAvatarPrefab;

	// Token: 0x04003E51 RID: 15953
	public GameObject errorAvatarPrefab;

	// Token: 0x04003E52 RID: 15954
	public GameObject blockedAvatarPrefab;

	// Token: 0x04003E53 RID: 15955
	public GameObject invisibleAvatarPrefab;

	// Token: 0x04003E54 RID: 15956
	public string fallbackAvatarId = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11";

	// Token: 0x04003E55 RID: 15957
	private bool DisableHeadChop;

	// Token: 0x04003E56 RID: 15958
	private VRCAvatarManager.LocalAvatarVisibility localAvatarVisibility;

	// Token: 0x04003E57 RID: 15959
	public GameObject proxyObject;

	// Token: 0x04003E58 RID: 15960
	private Renderer[] visibleElements;

	// Token: 0x04003E59 RID: 15961
	private IVRCCullable[] cullableElements;

	// Token: 0x04003E5A RID: 15962
	private bool[] visibleElementEnable;

	// Token: 0x04003E5B RID: 15963
	private bool _isInsidePersonalSpace;

	// Token: 0x04003E5D RID: 15965
	private bool _atLeastOneAvatarWasLoaded;

	// Token: 0x04003E5E RID: 15966
	private VRCPlayer _vrcPlayer;

	// Token: 0x04003E5F RID: 15967
	private PlayerSelector _laserSelectRegion;

	// Token: 0x04003E60 RID: 15968
	private AvatarPedestal _avatarPedestal;

	// Token: 0x04003E61 RID: 15969
	public GameObject currentAvatar;

	// Token: 0x04003E62 RID: 15970
	public VRCAvatarManager.AvatarKind currentAvatarKind;

	// Token: 0x04003E63 RID: 15971
	public VRC_AvatarDescriptor currentAvatarDescriptor;

	// Token: 0x04003E64 RID: 15972
	public string currentAvatarId;

	// Token: 0x04003E65 RID: 15973
	public RuntimeAnimatorController animatorControllerTemplate;

	// Token: 0x04003E66 RID: 15974
	public AnimatorOverrideController animatorControllerMale;

	// Token: 0x04003E67 RID: 15975
	public AnimatorOverrideController animatorControllerFemale;

	// Token: 0x04003E68 RID: 15976
	public AnimatorOverrideController animatorControllerMaleSitting;

	// Token: 0x04003E69 RID: 15977
	public AnimatorOverrideController animatorControllerFemaleSitting;

	// Token: 0x04003E6A RID: 15978
	private const float kMinAvatarHeightForBubbleScale = 2f;

	// Token: 0x04003E6B RID: 15979
	private const float kMaxAvatarHeightForBubbleScale = 4f;

	// Token: 0x04003E6C RID: 15980
	public float MinBubbleRadius = 1.5f;

	// Token: 0x04003E6D RID: 15981
	public float MaxBubbleRadius = 3f;

	// Token: 0x04003E6E RID: 15982
	public float HandBubbleRadius = 0.5f;

	// Token: 0x04003E6F RID: 15983
	private GameObject _avatarMirrorClone;

	// Token: 0x04003E70 RID: 15984
	private GameObject _avatarShadowClone;

	// Token: 0x04003E71 RID: 15985
	private Vector3 headScale;

	// Token: 0x04003E72 RID: 15986
	private Transform headTransform;

	// Token: 0x04003E73 RID: 15987
	private Transform ikHeadBone;

	// Token: 0x04003E74 RID: 15988
	public Transform profile;

	// Token: 0x04003E75 RID: 15989
	public Transform old_profile;

	// Token: 0x04003E76 RID: 15990
	public string avatarUrl;

	// Token: 0x04003E77 RID: 15991
	private float currentScale = 1f;

	// Token: 0x04003E78 RID: 15992
	private float currentAvatarArmLength;

	// Token: 0x04003E79 RID: 15993
	private float currentAvatarEyeHeight;

	// Token: 0x04003E7A RID: 15994
	private float maxHeadRadius;

	// Token: 0x04003E7B RID: 15995
	private AnimatorOverrideController currentStandingAnims;

	// Token: 0x04003E7C RID: 15996
	private AnimatorOverrideController currentSittingAnims;

	// Token: 0x04003E7D RID: 15997
	private bool _isAttached;

	// Token: 0x04003E7E RID: 15998
	public ProgressBar progressBar;

	// Token: 0x04003E7F RID: 15999
	public bool propAvatar;

	// Token: 0x04003E80 RID: 16000
	public bool localPlayer;

	// Token: 0x04003E81 RID: 16001
	public VRCAvatarManager.AvatarCreationCallback OnAvatarCreated;

	// Token: 0x04003E82 RID: 16002
	private LightProbeUsage useLightProbes;

	// Token: 0x04003E83 RID: 16003
	private bool _useFemaleAnims;

	// Token: 0x04003E84 RID: 16004
	private bool _nonHuman;

	// Token: 0x04003E85 RID: 16005
	private bool _fromPrefab;

	// Token: 0x04003E86 RID: 16006
	private ApiAvatar lastAvatar;

	// Token: 0x04003E87 RID: 16007
	public VRCAvatarManager.AvatarCreationCallback OnLastAvatarLoaded;

	// Token: 0x02000B32 RID: 2866
	public enum LocalAvatarVisibility
	{
		// Token: 0x04003E92 RID: 16018
		Visible,
		// Token: 0x04003E93 RID: 16019
		Invisible,
		// Token: 0x04003E94 RID: 16020
		Proxy
	}

	// Token: 0x02000B33 RID: 2867
	public enum AvatarKind
	{
		// Token: 0x04003E96 RID: 16022
		Undefined,
		// Token: 0x04003E97 RID: 16023
		Loading,
		// Token: 0x04003E98 RID: 16024
		Error,
		// Token: 0x04003E99 RID: 16025
		Blocked,
		// Token: 0x04003E9A RID: 16026
		Invisible,
		// Token: 0x04003E9B RID: 16027
		Custom
	}

	// Token: 0x02000B34 RID: 2868
	// (Invoke) Token: 0x0600579D RID: 22429
	public delegate void AvatarCreationCallback(GameObject Avatar, VRC_AvatarDescriptor Ad, bool loaded);
}
