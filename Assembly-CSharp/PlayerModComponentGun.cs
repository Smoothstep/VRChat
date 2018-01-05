using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000AC7 RID: 2759
public class PlayerModComponentGun : VRCPunBehaviour, IPlayerModComponent
{
	// Token: 0x060053D8 RID: 21464 RVA: 0x001CEC0F File Offset: 0x001CD00F
	public override void Awake()
	{
		base.Awake();
		this._headLook = base.GetComponentInChildren<Dk2HeadLook>();
		this._animationController = base.GetComponentInChildren<VRC_AnimationController>();
		this._animControllerManager = base.GetComponentInChildren<AnimatorControllerManager>();
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x001CEC3B File Offset: 0x001CD03B
	private void OnDestroy()
	{
		this.ResetModdedVariables();
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x001CEC44 File Offset: 0x001CD044
	public void SetProperties(List<VRCPlayerModProperty> properties)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (VRCPlayerModProperty vrcplayerModProperty in properties)
		{
			dictionary[vrcplayerModProperty.name] = vrcplayerModProperty.value();
		}
		this.gunPrefab = (GameObject)Tools.GetOrDefaultFromDictionary(dictionary, "GunPrefab", null);
		this.gunAnimations = (RuntimeAnimatorController)Tools.GetOrDefaultFromDictionary(dictionary, "GunAnimations", null);
		VRC_AvatarDescriptor componentInChildren = base.GetComponentInChildren<VRC_AvatarDescriptor>();
		if (componentInChildren == null)
		{
			return;
		}
		this._avatar = componentInChildren.gameObject;
		Animator component = this._avatar.GetComponent<Animator>();
		if (component.isHuman)
		{
			if (this._animator == null)
			{
				this._animator = component;
				if (this.gunAnimations != null)
				{
					this._animControllerManager.Push(this.gunAnimations);
				}
			}
			else if (this._animator != component)
			{
				this._animator = component;
			}
			this.rightHand = this._animator.GetBoneTransform(HumanBodyBones.RightHand);
			if (this.gunPrefab != null && this.rightHand != null)
			{
				this.gunInstance = (GameObject)AssetManagement.Instantiate(this.gunPrefab);
				this.gunStats = this.gunInstance.GetComponent<VRC_GunStats>();
				this.gunControls = this.gunInstance.GetComponent<VRC_PropController>();
				this.gunControls.controllingPlayer = base.GetComponent<VRC_PlayerApi>();
				this.ClipContents = ((this.gunStats.clipSize == 0) ? -1 : this.gunStats.clipSize);
				this.gunInstance.transform.parent = this.rightHand;
				this.gunInstance.transform.localPosition = Vector3.zero;
				this.gunInstance.transform.localRotation = Quaternion.Inverse(this._animationController.GetTPoseRotation(HumanBodyBones.RightHand)) * this.gunInstance.transform.rotation;
			}
		}
	}

	// Token: 0x060053DB RID: 21467 RVA: 0x001CEE74 File Offset: 0x001CD274
	private void ResetModdedVariables()
	{
		this._animator = null;
		if (this.gunAnimations != null)
		{
			this._animControllerManager.Pop();
		}
		this.gunStats = null;
		UnityEngine.Object.Destroy(this.gunInstance);
		this.gunInstance = null;
	}

	// Token: 0x060053DC RID: 21468 RVA: 0x001CEEB4 File Offset: 0x001CD2B4
	private void Update()
	{
		if (this._animator)
		{
			Vector3 eulerAngles = this._headLook.HeadRot.eulerAngles;
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			if (eulerAngles.y > 180f)
			{
				eulerAngles.y -= 360f;
			}
			this._animator.SetFloat("AimLeftRight", eulerAngles.y);
			this._animator.SetFloat("AimDownUp", -eulerAngles.x);
		}
		float num = 1f;
		bool flag = false;
		if (this.gunStats.reloadAudio != null && this.gunStats.reloadAudio.isPlaying)
		{
			flag = true;
			num = 0f;
			this._animator.SetBool("Reload", false);
		}
		if (!(this.gunStats.leftHandContact != null) || num > 0f)
		{
		}
		if (base.isMine)
		{
			if (this.gunControls.Inputs[0].value)
			{
				this.ReloadHoldTime += Time.deltaTime;
				if (this.ReloadHoldTime > 0.5f)
				{
					VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Others, base.gameObject, "ShowReload", new object[0]);
					this._animator.SetBool("Reload", true);
					this.ClipContents = ((this.gunStats.clipSize == 0) ? -1 : this.gunStats.clipSize);
					this.gunStats.reloadAudio.Play();
					flag = true;
				}
			}
			else
			{
				this.ReloadHoldTime = 0f;
			}
			this.timeSinceShot += Time.deltaTime;
			if (this.timeSinceShot > 1f / this.gunStats.rateOfFire)
			{
				bool flag2 = false;
				if (!flag)
				{
					if (this.gunStats.fullAuto && this.gunControls.Inputs[1].value)
					{
						flag2 = true;
					}
					else if (this.gunControls.Inputs[1].GetKeyDown())
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					this.FireWeapon();
				}
				else
				{
					this.gunStats.muzzleEffect.SetActive(false);
					this._animator.SetBool("Shooting", false);
				}
			}
		}
		else
		{
			this.RemoteMuzzleFlashTime -= Time.deltaTime;
			if (this.RemoteMuzzleFlashTime <= 0f)
			{
				if (this.gunStats.muzzleEffect != null)
				{
					this.gunStats.muzzleEffect.SetActive(false);
				}
				this._animator.SetBool("Shooting", false);
			}
		}
	}

	// Token: 0x060053DD RID: 21469 RVA: 0x001CF198 File Offset: 0x001CD598
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Others
	})]
	private void ShowWeaponFire(Vector3 pos, Vector3 dir, Vector3 normal, VRC.Player instigator)
	{
		if (base.Owner != instigator)
		{
			return;
		}
		if (this.gunStats.muzzleAudio != null)
		{
			this.gunStats.muzzleAudio.PlayOneShot(this.gunStats.fireAudio[UnityEngine.Random.Range(0, this.gunStats.fireAudio.Length)]);
		}
		this.GenerateBulletHitEffect(pos, dir, normal);
		this.RemoteMuzzleFlashTime = 1f / this.gunStats.rateOfFire;
		this.gunStats.muzzleEffect.SetActive(true);
	}

	// Token: 0x060053DE RID: 21470 RVA: 0x001CF22E File Offset: 0x001CD62E
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Others
	})]
	private void ShowReload(VRC.Player instigator)
	{
		if (base.Owner != instigator)
		{
			return;
		}
		this._animator.SetBool("Reload", true);
		this.gunStats.reloadAudio.Play();
	}

	// Token: 0x060053DF RID: 21471 RVA: 0x001CF264 File Offset: 0x001CD664
	private void GenerateBulletHitEffect(Vector3 pos, Vector3 dir, Vector3 normal)
	{
		Vector3 toDirection = Vector3.Reflect(dir, normal);
		Quaternion rot = Quaternion.FromToRotation(Vector3.forward, toDirection);
		GameObject obj = (GameObject)AssetManagement.Instantiate(this.gunStats.hitEffects[0], pos, rot);
		UnityEngine.Object.Destroy(obj, 2f);
	}

	// Token: 0x060053E0 RID: 21472 RVA: 0x001CF2AC File Offset: 0x001CD6AC
	private void FireWeapon()
	{
		this.timeSinceShot = 0f;
		if (this.ClipContents == 0)
		{
			this.gunStats.muzzleAudio.PlayOneShot(this.gunStats.EmptyClipFire);
			this.gunStats.muzzleEffect.SetActive(false);
			this._animator.SetBool("Shooting", false);
		}
		else
		{
			this.ClipContents--;
			this.gunStats.muzzleAudio.PlayOneShot(this.gunStats.fireAudio[UnityEngine.Random.Range(0, this.gunStats.fireAudio.Length)]);
			this.gunStats.muzzleEffect.SetActive(true);
			this._animator.SetBool("Shooting", true);
			Ray ray = new Ray(this.gunInstance.transform.position, this.gunInstance.transform.forward);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit))
			{
				this.GenerateBulletHitEffect(raycastHit.point, this.gunInstance.transform.forward, raycastHit.normal);
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.Others, base.gameObject, "ShowWeaponFire", new object[]
				{
					raycastHit.point,
					this.gunInstance.transform.forward,
					raycastHit.normal
				});
				PlayerModComponentHealth component = raycastHit.collider.GetComponent<PlayerModComponentHealth>();
				if (component != null)
				{
					component.RemoveHealth(this.gunStats.damage);
				}
				NpcMortality componentInParent = raycastHit.collider.GetComponentInParent<NpcMortality>();
				if (componentInParent != null)
				{
					componentInParent.ApplyDamage(this.gunStats.damage);
				}
			}
		}
	}

	// Token: 0x04003B1E RID: 15134
	private GameObject gunPrefab;

	// Token: 0x04003B1F RID: 15135
	private RuntimeAnimatorController gunAnimations;

	// Token: 0x04003B20 RID: 15136
	private VRC_PropController gunControls;

	// Token: 0x04003B21 RID: 15137
	private float timeSinceShot;

	// Token: 0x04003B22 RID: 15138
	private GameObject gunInstance;

	// Token: 0x04003B23 RID: 15139
	private VRC_GunStats gunStats;

	// Token: 0x04003B24 RID: 15140
	private Transform rightHand;

	// Token: 0x04003B25 RID: 15141
	private Animator _animator;

	// Token: 0x04003B26 RID: 15142
	private GameObject _avatar;

	// Token: 0x04003B27 RID: 15143
	private float RemoteMuzzleFlashTime;

	// Token: 0x04003B28 RID: 15144
	private float ReloadHoldTime;

	// Token: 0x04003B29 RID: 15145
	private int ClipContents;

	// Token: 0x04003B2A RID: 15146
	private Dk2HeadLook _headLook;

	// Token: 0x04003B2B RID: 15147
	private VRC_AnimationController _animationController;

	// Token: 0x04003B2C RID: 15148
	private AnimatorControllerManager _animControllerManager;
}
