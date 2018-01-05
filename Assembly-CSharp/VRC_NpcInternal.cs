using System;
using RealisticEyeMovements;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B70 RID: 2928
public class VRC_NpcInternal : MonoBehaviour
{
	// Token: 0x14000074 RID: 116
	// (add) Token: 0x06005A7D RID: 23165 RVA: 0x001F8310 File Offset: 0x001F6710
	// (remove) Token: 0x06005A7E RID: 23166 RVA: 0x001F8348 File Offset: 0x001F6748
	public event VRC_NpcInternal.OnAvatarIsReady onAvatarIsReady;

	// Token: 0x06005A7F RID: 23167 RVA: 0x001F8380 File Offset: 0x001F6780
	private void Awake()
	{
		this.animationController = base.GetComponentInChildren<VRC_AnimationController>();
		this.animControlManager = base.GetComponentInChildren<AnimatorControllerManager>();
		this.namePlateColor = this.defaultNamePlateColor;
		this.namePlateEnable = true;
		this.avatarSwitcher = base.GetComponentInChildren<VRCAvatarManager>();
		VRCAvatarManager vrcavatarManager = this.avatarSwitcher;
		vrcavatarManager.OnAvatarCreated = (VRCAvatarManager.AvatarCreationCallback)Delegate.Combine(vrcavatarManager.OnAvatarCreated, new VRCAvatarManager.AvatarCreationCallback(this.AvatarIsReady));
		this.cameraMount = base.transform.Find("CameraMount");
		this.speakSrc = this.cameraMount.GetComponentInChildren<AudioSource>();
		this.lipSync = this.cameraMount.GetComponentInChildren<OVRLipSyncContext>();
		this.emojiGen = this.cameraMount.GetComponentInChildren<EmojiGenerator>();
		base.gameObject.AddComponent<VRC_NpcApi>();
	}

	// Token: 0x06005A80 RID: 23168 RVA: 0x001F8440 File Offset: 0x001F6840
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Local,
		VRC_EventHandler.VrcTargetType.All
	})]
	public void LoadAvatar(string npcName, string blueprintId, float scale = 1f)
	{
		this.playerName = npcName;
		if (this.nameTag != null)
		{
			this.nameTag.text = this.playerName;
		}
		if (this.animationController != null)
		{
			this.animationController.Detach();
		}
		if (!string.IsNullOrEmpty(blueprintId))
		{
			ApiAvatar.Fetch(blueprintId, delegate(ApiAvatar bp)
			{
				this.apiAvatar = bp;
				this.avatarSwitcher.SwitchAvatar(this.apiAvatar, scale, null);
			}, delegate(string message)
			{
				this.avatarSwitcher.SwitchToErrorAvatar(1f);
			});
		}
	}

	// Token: 0x06005A81 RID: 23169 RVA: 0x001F84D0 File Offset: 0x001F68D0
	private void SetupHead()
	{
		if (this.avatarAnimator != null && this.avatarAnimator.isHuman)
		{
			Transform boneTransform = this.avatarAnimator.GetBoneTransform(HumanBodyBones.Head);
			Transform transform = this.speakSrc.transform;
			Transform transform2 = this.emojiGen.transform;
			if (this.cameraMount != null && transform.parent != boneTransform)
			{
				transform.parent = boneTransform;
				transform2.parent = boneTransform;
			}
			EyeAndHeadAnimator component = this.avatarAnimator.GetComponent<EyeAndHeadAnimator>();
			if (component != null && component.enabled)
			{
				component.headWeight = 1f;
			}
		}
	}

	// Token: 0x06005A82 RID: 23170 RVA: 0x001F8584 File Offset: 0x001F6984
	private void AvatarIsReady(GameObject avatar, VRC_AvatarDescriptor Ad, bool loaded)
	{
		this.avatarAnimator = avatar.GetComponent<Animator>();
		if (this.avatarAnimator != null)
		{
			this.animationController.Reset(this.avatarAnimator, Ad);
			this.animControlManager.Initialize(this.avatarAnimator, Ad, false);
			this.SetupHead();
			this.lastPosition = base.transform.position;
			this.locoWeight = 0f;
		}
		if (this.onAvatarIsReady != null)
		{
			this.onAvatarIsReady();
		}
	}

	// Token: 0x06005A83 RID: 23171 RVA: 0x001F860C File Offset: 0x001F6A0C
	private void SetAvatarPose(RuntimeAnimatorController rac)
	{
		Animator componentInChildren = base.GetComponentInChildren<Animator>();
		VRC_AvatarDescriptor componentInChildren2 = base.GetComponentInChildren<VRC_AvatarDescriptor>();
		if (rac != null)
		{
			componentInChildren.runtimeAnimatorController = rac;
		}
		else if (componentInChildren2 == null)
		{
			componentInChildren.runtimeAnimatorController = this.avatarSwitcher.animatorControllerMale;
		}
		else if (componentInChildren2.Animations == VRC_AvatarDescriptor.AnimationSet.Female)
		{
			componentInChildren.runtimeAnimatorController = this.avatarSwitcher.animatorControllerFemale;
		}
		else
		{
			componentInChildren.runtimeAnimatorController = this.avatarSwitcher.animatorControllerMale;
		}
		int layerIndex = componentInChildren.GetLayerIndex("Idle Layer");
		int layerIndex2 = componentInChildren.GetLayerIndex("Locomotion Layer");
		int layerIndex3 = componentInChildren.GetLayerIndex("HandLeft");
		int layerIndex4 = componentInChildren.GetLayerIndex("HandRight");
		if (layerIndex >= 0)
		{
			componentInChildren.SetLayerWeight(layerIndex, 1f);
		}
		if (layerIndex2 >= 0)
		{
			componentInChildren.SetLayerWeight(layerIndex2, 0f);
		}
		if (layerIndex3 >= 0)
		{
			componentInChildren.SetLayerWeight(layerIndex3, 0f);
		}
		if (layerIndex4 >= 0)
		{
			componentInChildren.SetLayerWeight(layerIndex4, 0f);
		}
		componentInChildren.SetFloat("HeightScale", 1f);
		componentInChildren.SetFloat("HeightScaleNOMOVE", 1f);
		VRIK component = componentInChildren.GetComponent<VRIK>();
		if (component != null)
		{
			component.enabled = false;
		}
		global::LimbIK component2 = componentInChildren.GetComponent<global::LimbIK>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
	}

	// Token: 0x06005A84 RID: 23172 RVA: 0x001F8770 File Offset: 0x001F6B70
	private float GetRMSLevel(AudioSource src)
	{
		src.GetOutputData(this.samples, 0);
		float num = 0f;
		for (int i = 0; i < this.samples.Length; i++)
		{
			num += this.samples[i] * this.samples[i];
		}
		return Mathf.Sqrt(num / (float)this.samples.Length);
	}

	// Token: 0x06005A85 RID: 23173 RVA: 0x001F87D0 File Offset: 0x001F6BD0
	private void Update()
	{
		this.isSpeaking = (this.GetRMSLevel(this.speakSrc) > 0f);
		if (VRC_NpcInternal.NamePlatesVisible && this.namePlateEnable)
		{
			this.namePlate.gameObject.SetActive(true);
			this.vipBadge.gameObject.SetActive(false);
			this.statusBadge.gameObject.SetActive(false);
			this.friendSprite.gameObject.SetActive(false);
			this.speakingSprite.enabled = false;
			this.mutedSprite.enabled = false;
			this.speakingMutedSprite.enabled = false;
			this.listeningSprite.enabled = false;
			this.listeningMutedSprite.enabled = false;
			Color color;
			if (this.canSpeak)
			{
				this.nameTag.color = Color.white;
				color = this.namePlateColor;
				if (this.lipSync != null && this.lipSync.enabled)
				{
					this.lipSync.audioMute = false;
				}
			}
			else
			{
				this.nameTag.color = this.mutedNamePlateColor;
				color = this.mutedNamePlateColor;
				if (this.lipSync != null && this.lipSync.enabled)
				{
					this.lipSync.audioMute = true;
				}
			}
			if (this.isSpeaking)
			{
				this.namePlate.sprite = this.namePlateTalkSprite;
				this.vipBadge.sprite = this.namePlateTalkSprite;
				this.statusBadge.sprite = this.namePlateTalkSprite;
				if (this.canSpeak)
				{
					this.speakingSprite.enabled = true;
					this.speakingSprite.color = Color.white;
				}
				else
				{
					this.speakingMutedSprite.enabled = true;
					this.speakingMutedSprite.color = this.mutedNamePlateColor;
				}
			}
			else
			{
				this.namePlate.sprite = this.namePlateSilentSprite;
				this.vipBadge.sprite = this.namePlateSilentSprite;
				this.statusBadge.sprite = this.namePlateSilentSprite;
				if (!this.canSpeak)
				{
					this.mutedSprite.enabled = true;
					this.mutedSprite.color = this.mutedNamePlateColor;
				}
			}
			if (!this.canHear)
			{
				this.listeningMutedSprite.enabled = true;
				this.listeningMutedSprite.color = ((!this.canSpeak) ? this.mutedNamePlateColor : Color.white);
			}
			this.namePlate.color = color;
			this.vipBadge.color = color;
			this.statusBadge.color = color;
		}
		else
		{
			this.namePlate.gameObject.SetActive(false);
			this.vipBadge.gameObject.SetActive(false);
			this.statusBadge.gameObject.SetActive(false);
			this.speakingSprite.enabled = false;
			this.mutedSprite.enabled = false;
			this.speakingMutedSprite.enabled = false;
			this.listeningSprite.enabled = false;
			this.listeningMutedSprite.enabled = false;
			this.friendSprite.gameObject.SetActive(false);
		}
		if (this.avatarAnimator != null)
		{
			if (this.selectedAct != this.currentAct)
			{
				this.avatarAnimator.SetInteger(this.actParam, this.selectedAct);
				this.currentAct = this.selectedAct;
			}
			AnimatorTransitionInfo animatorTransitionInfo = this.avatarAnimator.GetAnimatorTransitionInfo(this.actLayer);
			if (!this.isLoopingAct && animatorTransitionInfo.IsUserName("EmoteExit"))
			{
				this.selectedAct = 0;
			}
		}
		if (this.avatarAnimator != null)
		{
			Vector3 vector = this.lastPosition - base.transform.position;
			float num = vector.magnitude / Time.deltaTime;
			float num2;
			if (num > 0.1f)
			{
				num2 = 1f;
				Vector3 vector2 = num * this.avatarAnimator.transform.parent.InverseTransformDirection(vector.normalized);
				vector2.y = 0f;
				this.avatarAnimator.SetFloat("MovementX", vector2.x);
				this.avatarAnimator.SetFloat("MovementZ", -vector2.z);
			}
			else
			{
				num2 = 0f;
				if (Mathf.Approximately(num2, 0f))
				{
					num2 = 0f;
				}
				this.avatarAnimator.SetFloat("MovementX", 0f);
				this.avatarAnimator.SetFloat("MovementZ", 0f);
			}
			if (this.locoWeight != num2)
			{
				this.avatarAnimator.SetLayerWeight(1, num2);
				this.locoWeight = num2;
			}
		}
		this.lastPosition = base.transform.position;
	}

	// Token: 0x06005A86 RID: 23174 RVA: 0x001F8CA0 File Offset: 0x001F70A0
	public void InitializeApi(VRC_NpcApi api)
	{
	}

	// Token: 0x06005A87 RID: 23175 RVA: 0x001F8CA2 File Offset: 0x001F70A2
	public void ActThis(int emoteNum, bool loop)
	{
		if (this.avatarAnimator != null)
		{
			this.selectedAct = emoteNum;
			if (emoteNum == 0)
			{
				this.isLoopingAct = false;
			}
			else
			{
				this.isLoopingAct = loop;
			}
		}
	}

	// Token: 0x06005A88 RID: 23176 RVA: 0x001F8CD8 File Offset: 0x001F70D8
	public void SayThis(AudioClip clip, float volume)
	{
		if (this.speakSrc != null)
		{
			if (this.speakSrc.isPlaying)
			{
				this.speakSrc.Stop();
			}
			this.speakSrc.PlayOneShot(clip, volume);
			if (this.lipSync != null)
			{
				this.lipSync.enabled = false;
				this.lipSync.enabled = true;
			}
		}
	}

	// Token: 0x06005A89 RID: 23177 RVA: 0x001F8D47 File Offset: 0x001F7147
	public void SetNamePlate(bool visible, string nameString, string vipString)
	{
		this.namePlateEnable = visible;
		this.nameTag.text = nameString;
		this.vipTag.text = vipString;
	}

	// Token: 0x06005A8A RID: 23178 RVA: 0x001F8D68 File Offset: 0x001F7168
	public void SetSocialStatus(bool friend, bool vip, bool blocked)
	{
	}

	// Token: 0x06005A8B RID: 23179 RVA: 0x001F8D6A File Offset: 0x001F716A
	public void SetMuteStatus(bool speak, bool hear)
	{
		this.canSpeak = speak;
		this.canHear = hear;
	}

	// Token: 0x0400403F RID: 16447
	private static bool NamePlatesVisible = true;

	// Token: 0x04004040 RID: 16448
	public Image namePlate;

	// Token: 0x04004041 RID: 16449
	public Image vipBadge;

	// Token: 0x04004042 RID: 16450
	public Image statusBadge;

	// Token: 0x04004043 RID: 16451
	public Color defaultNamePlateColor = Color.green;

	// Token: 0x04004044 RID: 16452
	public Color vipNamePlateColor = Color.yellow;

	// Token: 0x04004045 RID: 16453
	public Color mutedNamePlateColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);

	// Token: 0x04004046 RID: 16454
	public Sprite namePlateSilentSprite;

	// Token: 0x04004047 RID: 16455
	public Sprite namePlateTalkSprite;

	// Token: 0x04004048 RID: 16456
	public Image speakingSprite;

	// Token: 0x04004049 RID: 16457
	public Image speakingMutedSprite;

	// Token: 0x0400404A RID: 16458
	public Image mutedSprite;

	// Token: 0x0400404B RID: 16459
	public Image listeningSprite;

	// Token: 0x0400404C RID: 16460
	public Image listeningMutedSprite;

	// Token: 0x0400404D RID: 16461
	public Image friendSprite;

	// Token: 0x0400404E RID: 16462
	public Text nameTag;

	// Token: 0x0400404F RID: 16463
	public Text vipTag;

	// Token: 0x04004050 RID: 16464
	public Text statusTag;

	// Token: 0x04004051 RID: 16465
	private Color namePlateColor;

	// Token: 0x04004052 RID: 16466
	private bool namePlateEnable;

	// Token: 0x04004053 RID: 16467
	private Transform cameraMount;

	// Token: 0x04004054 RID: 16468
	private ApiAvatar apiAvatar;

	// Token: 0x04004055 RID: 16469
	private VRCAvatarManager avatarSwitcher;

	// Token: 0x04004056 RID: 16470
	private Animator avatarAnimator;

	// Token: 0x04004057 RID: 16471
	private VRC_AnimationController animationController;

	// Token: 0x04004058 RID: 16472
	private AnimatorControllerManager animControlManager;

	// Token: 0x0400405A RID: 16474
	private EmojiGenerator emojiGen;

	// Token: 0x0400405B RID: 16475
	private AudioSource speakSrc;

	// Token: 0x0400405C RID: 16476
	private OVRLipSyncContext lipSync;

	// Token: 0x0400405D RID: 16477
	private bool isSpeaking;

	// Token: 0x0400405E RID: 16478
	private bool canSpeak = true;

	// Token: 0x0400405F RID: 16479
	private bool canHear = true;

	// Token: 0x04004060 RID: 16480
	private int currentAct;

	// Token: 0x04004061 RID: 16481
	private int selectedAct;

	// Token: 0x04004062 RID: 16482
	private bool isLoopingAct;

	// Token: 0x04004063 RID: 16483
	private int actLayer;

	// Token: 0x04004064 RID: 16484
	private string actParam = "Emote";

	// Token: 0x04004065 RID: 16485
	private string playerName;

	// Token: 0x04004066 RID: 16486
	private Vector3 lastPosition;

	// Token: 0x04004067 RID: 16487
	private float locoWeight;

	// Token: 0x04004068 RID: 16488
	private float[] samples = new float[16];

	// Token: 0x02000B71 RID: 2929
	// (Invoke) Token: 0x06005A8E RID: 23182
	public delegate void OnAvatarIsReady();
}
