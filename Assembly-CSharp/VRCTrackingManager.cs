using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.Core;

// Token: 0x02000B24 RID: 2852
public class VRCTrackingManager : MonoBehaviour
{
	// Token: 0x0600569A RID: 22170 RVA: 0x001DCCD1 File Offset: 0x001DB0D1
	public static bool IsInitialized()
	{
		return VRCTrackingManager.instance != null;
	}

	// Token: 0x0600569B RID: 22171 RVA: 0x001DCCDE File Offset: 0x001DB0DE
	public static bool IsPlayerNearTracking()
	{
		return VRCTrackingManager.instance.playerNearTracking;
	}

	// Token: 0x0600569C RID: 22172 RVA: 0x001DCCEA File Offset: 0x001DB0EA
	public static void SetPlayerNearTracking(bool near)
	{
		VRCTrackingManager.instance.playerNearTracking = near;
	}

	// Token: 0x17000C99 RID: 3225
	// (get) Token: 0x0600569D RID: 22173 RVA: 0x001DCCF8 File Offset: 0x001DB0F8
	// (set) Token: 0x0600569E RID: 22174 RVA: 0x001DCD2A File Offset: 0x001DB12A
	private static bool InSeatedPlay
	{
		get
		{
			return !VRCTrackingManager.IsInVRMode() || PlayerPrefs.GetInt("SeatedPlayEnabled", 0) != 0;
		}
		set
		{
			if (VRCTrackingManager.IsInVRMode())
			{
				PlayerPrefs.SetInt("SeatedPlayEnabled", (!value) ? 0 : 1);
				PlayerPrefs.Save();
			}
		}
	}

	// Token: 0x0600569F RID: 22175 RVA: 0x001DCD54 File Offset: 0x001DB154
	private void Start()
	{
		VRCTrackingManager.instance = this;
		GameObject asset = this.TestTracking;
		if (this.ForceTestTracking)
		{
			asset = this.TestTracking;
		}
		else
		{
			asset = this.SteamVrTracking;
		}
		GameObject gameObject = AssetManagement.Instantiate(asset) as GameObject;
		this.activeTrackers.Add(gameObject.GetComponent<VRCTracking>());
		gameObject.transform.SetParent(base.transform);
		GameObject gameObject2 = AssetManagement.Instantiate(this.HandProxyTracking) as GameObject;
		this.activeTrackers.Add(gameObject2.GetComponent<VRCTracking>());
		gameObject2.transform.SetParent(base.transform);
		VRCTrackingManager.playerEyeHeight = VRCTracking.DefaultEyeHeight;
		VRCTrackingManager.playerArmLength = VRCTracking.DefaultArmLength;
		VRCTrackingManager._avatarViewPoint = new Vector3(0f, VRCTrackingManager.playerEyeHeight, 0f);
	}

	// Token: 0x060056A0 RID: 22176 RVA: 0x001DCE1C File Offset: 0x001DB21C
	private void LateUpdate()
	{
		if (VRCPlayer.Instance != null)
		{
			if (this.cameraMount == null)
			{
				this.cameraMount = VRCPlayer.Instance.transform.Find("CameraMount");
			}
			if (this.avatarMgr == null)
			{
				this.avatarMgr = VRCPlayer.Instance.transform.Find("ForwardDirection").GetComponent<VRCAvatarManager>();
			}
			if (this.animatorControllerMgr == null)
			{
				this.animatorControllerMgr = VRCPlayer.Instance.transform.Find("AnimationController").GetComponent<AnimatorControllerManager>();
				this.ik = this.animatorControllerMgr.GetComponent<VRC_AnimationController>().HeadAndHandsIkController.GetComponent<IkController>();
			}
		}
		else
		{
			this.cameraMount = null;
		}
	}

	// Token: 0x060056A1 RID: 22177 RVA: 0x001DCEEC File Offset: 0x001DB2EC
	public static void ResetHMDOrientation()
	{
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			vrctracking.ResetHMDOrientation();
		}
	}

	// Token: 0x060056A2 RID: 22178 RVA: 0x001DCF4C File Offset: 0x001DB34C
	public static void ToggleSeatedPlay()
	{
		VRCTrackingManager.InSeatedPlay = !VRCTrackingManager.InSeatedPlay;
		VRCTrackingManager.SetSeatedPlayMode(VRCTrackingManager.InSeatedPlay);
	}

	// Token: 0x060056A3 RID: 22179 RVA: 0x001DCF68 File Offset: 0x001DB368
	public static Quaternion GetMotionOrientation()
	{
		Quaternion quaternion = VRCTrackingManager.instance.transform.rotation;
		if (VRCInputManager.headLookWalk)
		{
			quaternion = VRCTrackingManager.instance.transform.rotation * VRCVrCamera.GetInstance().GetLocalCameraRot();
		}
		return Quaternion.Euler(0f, quaternion.eulerAngles.y, 0f);
	}

	// Token: 0x060056A4 RID: 22180 RVA: 0x001DCFCC File Offset: 0x001DB3CC
	public static Vector3 GetWorldTrackingMotion()
	{
		if (VRCTrackingManager.instance.cameraMount != null)
		{
			Vector3 position = VRCPlayer.Instance.transform.InverseTransformPoint(VRCVrCamera.GetInstance().GetWorldCameraNeckPos());
			position.y = 0f;
			Vector3 a = VRCPlayer.Instance.transform.TransformPoint(position);
			Vector3 position2 = VRCPlayer.Instance.transform.InverseTransformPoint(VRCTrackingManager.instance.cameraMount.position);
			position2.y = 0f;
			Vector3 b = VRCPlayer.Instance.transform.TransformPoint(position2);
			return a - b;
		}
		return Vector3.zero;
	}

	// Token: 0x060056A5 RID: 22181 RVA: 0x001DD074 File Offset: 0x001DB474
	public static Vector3 GetWorldTrackingPosition()
	{
		Vector3 worldCameraNeckPos = VRCVrCamera.GetInstance().GetWorldCameraNeckPos();
		if (VRCPlayer.Instance != null)
		{
			Vector3 position = VRCPlayer.Instance.transform.InverseTransformPoint(worldCameraNeckPos);
			position.y = 0f;
			return VRCPlayer.Instance.transform.TransformPoint(position);
		}
		return worldCameraNeckPos;
	}

	// Token: 0x060056A6 RID: 22182 RVA: 0x001DD0CD File Offset: 0x001DB4CD
	public static Vector3 GetTrackingWorldOrigin()
	{
		return VRCTrackingManager.instance.transform.position;
	}

	// Token: 0x060056A7 RID: 22183 RVA: 0x001DD0DE File Offset: 0x001DB4DE
	public static Transform GetTrackingTransform()
	{
		return VRCTrackingManager.instance.transform;
	}

	// Token: 0x060056A8 RID: 22184 RVA: 0x001DD0EA File Offset: 0x001DB4EA
	public static void SetTrackingWorldOrigin(Vector3 pos, Quaternion rot)
	{
		VRCTrackingManager.instance.transform.position = pos;
		VRCTrackingManager.instance.transform.rotation = rot;
	}

	// Token: 0x060056A9 RID: 22185 RVA: 0x001DD10C File Offset: 0x001DB50C
	public static bool IsInVRMode()
	{
		if (VRCTrackingManager.instance != null)
		{
			foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
			{
				if (vrctracking.IsInVRMode())
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x060056AA RID: 22186 RVA: 0x001DD18C File Offset: 0x001DB58C
	public static float GetTrackingScale()
	{
		return VRCTrackingManager.instance.transform.localScale.x;
	}

	// Token: 0x060056AB RID: 22187 RVA: 0x001DD1B0 File Offset: 0x001DB5B0
	public static float GetPlayerEyeHeight()
	{
		return VRCTrackingManager.playerEyeHeight;
	}

	// Token: 0x060056AC RID: 22188 RVA: 0x001DD1B7 File Offset: 0x001DB5B7
	public static float GetPlayerArmLength()
	{
		return VRCTrackingManager.playerArmLength;
	}

	// Token: 0x060056AD RID: 22189 RVA: 0x001DD1BE File Offset: 0x001DB5BE
	public static float GetPlayerHeightAdjustment()
	{
		return VRCTrackingManager.playerHeightAdjust * 0.985f;
	}

	// Token: 0x060056AE RID: 22190 RVA: 0x001DD1CC File Offset: 0x001DB5CC
	public static float GetPlayerSeatedSpaceAdjustment()
	{
		float x = VRCTrackingManager.instance.transform.localScale.x;
		return VRCTrackingManager.GetAvatarViewPoint().y * 0.985f / x;
	}

	// Token: 0x060056AF RID: 22191 RVA: 0x001DD208 File Offset: 0x001DB608
	public static float GetPlayerSeatedPlayAdjustment(bool recalibrate = false)
	{
		float x = VRCTrackingManager.instance.transform.localScale.x;
		float num = VRCTracking.DefaultSeatedEye;
		VRCVrCamera vrcvrCamera = VRCVrCamera.GetInstance();
		VRCPlayer vrcplayer = VRCPlayer.Instance;
		if (!VRCTrackingManager._usingStationViewPoint && recalibrate && vrcvrCamera != null && vrcplayer != null)
		{
			num = vrcplayer.transform.InverseTransformPoint(vrcvrCamera.GetWorldCameraPos()).y / x;
			num -= vrcvrCamera.GetLiftAmount();
			VRCTrackingManager.instance.seatedEyePosition = num;
		}
		else
		{
			num = VRCTrackingManager.instance.seatedEyePosition;
		}
		return VRCTrackingManager.GetAvatarViewPoint().y * 0.96f / x - num;
	}

	// Token: 0x060056B0 RID: 22192 RVA: 0x001DD2C4 File Offset: 0x001DB6C4
	public static void ChangedPlayerHeight(float height)
	{
		VRCTrackingManager.playerEyeHeight = height * 0.9391f;
		VRCTrackingManager.playerArmLength = VRCTrackingManager.playerEyeHeight * 0.4537f;
		if (VRCTrackingManager.instance == null || VRCTrackingManager.instance.avatarMgr == null)
		{
			return;
		}
		float avatarArmLength = VRCTrackingManager.instance.avatarMgr.GetAvatarArmLength();
		float num = avatarArmLength / VRCTrackingManager.playerArmLength;
		VRCTrackingManager.SetTrackingScale(new Vector3(num, num, num));
		VRCTrackingManager.OffsetCameraForHeight(num, VRCTrackingManager.playerEyeHeight, true);
	}

	// Token: 0x060056B1 RID: 22193 RVA: 0x001DD344 File Offset: 0x001DB744
	public static float GetPlayerShoulderHeight()
	{
		return VRCTrackingManager.playerEyeHeight * 0.8667f;
	}

	// Token: 0x060056B2 RID: 22194 RVA: 0x001DD354 File Offset: 0x001DB754
	public static void AdjustViewPositionToAvatar()
	{
		float playerHeight = VRCTrackingManager.GetPlayerHeight();
		VRCTrackingManager.ChangedPlayerHeight(playerHeight);
	}

	// Token: 0x060056B3 RID: 22195 RVA: 0x001DD36D File Offset: 0x001DB76D
	public static Vector3 GetAvatarViewPoint()
	{
		return (!VRCTrackingManager._usingStationViewPoint) ? VRCTrackingManager._avatarViewPoint : VRCTrackingManager._avatarStationViewPoint;
	}

	// Token: 0x060056B4 RID: 22196 RVA: 0x001DD388 File Offset: 0x001DB788
	public static void OffsetCameraForHeight(float scale, float eyeHeight, bool adjustHeight = true)
	{
		if (adjustHeight)
		{
			VRCTrackingManager.playerHeightAdjust = VRCTrackingManager.GetAvatarViewPoint().y / scale - eyeHeight;
		}
		bool sitMode = VRCVrCamera.GetInstance().GetSitMode();
		VRCVrCamera.GetInstance().SetSitMode(sitMode, false);
	}

	// Token: 0x060056B5 RID: 22197 RVA: 0x001DD3C9 File Offset: 0x001DB7C9
	public static void SetAvatarViewPoint(Vector3 pt, Vector3 headToViewPointOffset)
	{
		VRCTrackingManager._avatarViewPoint = pt;
	}

	// Token: 0x060056B6 RID: 22198 RVA: 0x001DD3D4 File Offset: 0x001DB7D4
	public static void UseAvatarStationViewPoint(bool isInStation)
	{
		if (VRCTrackingManager.instance.ik != null && VRCTrackingManager.instance.ik.currentIk == IkController.IkType.SixPoint && VRCTrackingManager.instance.ik.HasLowerBodyTracking)
		{
			return;
		}
		VRCTrackingManager.instance.StartCoroutine(VRCTrackingManager.UseAvatarStationViewPointCoroutine(isInStation));
	}

	// Token: 0x060056B7 RID: 22199 RVA: 0x001DD434 File Offset: 0x001DB834
	private static IEnumerator UseAvatarStationViewPointCoroutine(bool isInStation)
	{
		VRCPlayer player = VRCPlayer.Instance;
		bool alreadyInStation = VRCTrackingManager._usingStationViewPoint;
		if (isInStation)
		{
			VRCVrCamera vrcam = VRCVrCamera.GetInstance();
			float eyeHeight = VRCTrackingManager.GetAvatarEyeHeight();
			if (player != null)
			{
				if (vrcam != null)
				{
					eyeHeight = player.transform.InverseTransformPoint(VRCVrCamera.GetInstance().GetWorldCameraPos()).y;
				}
				if (VRCTrackingManager.instance.avatarMgr != null && VRCTrackingManager.instance.animatorControllerMgr != null && VRCTrackingManager.instance.animatorControllerMgr.avatarAnimator != null)
				{
					if (VRCTrackingManager.instance.ik != null)
					{
						VRCTrackingManager.instance.ik.SeatedChange(true);
						yield return null;
						VRCTrackingManager.instance.ik.SeatedChange(true);
						yield return null;
						VRCTrackingManager.instance.ik.SeatedChange(true);
						yield return null;
						VRCTrackingManager.instance.ik.SeatedChange(true);
						yield return null;
					}
					Transform headTransform = VRCTrackingManager.instance.animatorControllerMgr.avatarAnimator.GetBoneTransform(HumanBodyBones.Head);
					if (headTransform != null && player != null)
					{
						Transform transform = headTransform.Find("HmdPivot");
						VRCTrackingManager._avatarStationViewPoint = player.transform.InverseTransformPoint(transform.position);
					}
					if (VRCTrackingManager.instance.ik != null)
					{
						VRCTrackingManager.instance.ik.enabled = true;
					}
				}
			}
			VRCTrackingManager._usingStationViewPoint = true;
			if (VRCTrackingManager.instance.cameraMount != null)
			{
				VRCTrackingManager.instance.cameraMount.localPosition = new Vector3(0f, VRCTrackingManager._avatarStationViewPoint.y, 0f);
			}
			float scale = VRCTrackingManager.instance.transform.localScale.x;
			float lift = 0f;
			if (vrcam != null)
			{
				lift = vrcam.GetLiftAmount();
			}
			VRCTrackingManager.OffsetCameraForHeight(scale, eyeHeight / scale - lift, !alreadyInStation);
		}
		else
		{
			VRCTrackingManager._usingStationViewPoint = false;
			if (VRCTrackingManager.instance.cameraMount != null)
			{
				VRCTrackingManager.instance.cameraMount.localPosition = new Vector3(0f, VRCTrackingManager._avatarViewPoint.y, 0f);
			}
			float x = VRCTrackingManager.instance.transform.localScale.x;
			VRCTrackingManager.OffsetCameraForHeight(x, VRCTrackingManager.playerEyeHeight, true);
		}
		yield break;
	}

	// Token: 0x060056B8 RID: 22200 RVA: 0x001DD44F File Offset: 0x001DB84F
	public static void RefreshAvatarViewPoint()
	{
		VRCTrackingManager.UseAvatarStationViewPoint(VRCTrackingManager._usingStationViewPoint);
	}

	// Token: 0x060056B9 RID: 22201 RVA: 0x001DD45B File Offset: 0x001DB85B
	public static float GetAvatarEyeHeight()
	{
		if (VRCTrackingManager.instance.cameraMount == null)
		{
			return VRCTrackingManager.GetPlayerEyeHeight();
		}
		return VRCTrackingManager._avatarViewPoint.y;
	}

	// Token: 0x060056BA RID: 22202 RVA: 0x001DD484 File Offset: 0x001DB884
	public static float GetPlayerUprightAmount()
	{
		VRCPlayer vrcplayer = VRCPlayer.Instance;
		VRCVrCamera vrcvrCamera = VRCVrCamera.GetInstance();
		float value = 1f;
		float num = VRCTrackingManager.playerEyeHeight;
		if (vrcplayer.GetVRMode())
		{
			float num2 = VRCTrackingManager.playerEyeHeight;
			float num3 = VRCTrackingManager.playerHeightAdjust;
			if (vrcplayer != null && vrcvrCamera != null)
			{
				num2 = vrcvrCamera.GetLocalCameraPos().y - num3;
				num = VRCTrackingManager.playerEyeHeight;
			}
			value = num2 / num;
		}
		return Mathf.Clamp01(value);
	}

	// Token: 0x060056BB RID: 22203 RVA: 0x001DD500 File Offset: 0x001DB900
	public static Quaternion GetWorldTrackingRotation()
	{
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			Quaternion localTrackingRotation = vrctracking.GetLocalTrackingRotation();
			if (localTrackingRotation != Quaternion.identity)
			{
				return Quaternion.Inverse(VRCTrackingManager.instance.cameraMount.rotation) * (VRCTrackingManager.instance.transform.rotation * localTrackingRotation);
			}
		}
		return (!(VRCTrackingManager.instance.cameraMount == null)) ? (Quaternion.Inverse(VRCTrackingManager.instance.cameraMount.rotation) * VRCTrackingManager.instance.transform.rotation) : Quaternion.identity;
	}

	// Token: 0x060056BC RID: 22204 RVA: 0x001DD5F0 File Offset: 0x001DB9F0
	public static Quaternion GetWorldTrackingOrientation()
	{
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			Quaternion localTrackingRotation = vrctracking.GetLocalTrackingRotation();
			if (localTrackingRotation != Quaternion.identity)
			{
				return VRCTrackingManager.instance.transform.rotation * localTrackingRotation;
			}
		}
		return VRCTrackingManager.instance.transform.rotation;
	}

	// Token: 0x060056BD RID: 22205 RVA: 0x001DD68C File Offset: 0x001DBA8C
	public static AudioListener GetAudioListener()
	{
		if (VRCTrackingManager._audioListener == null)
		{
			VRCTrackingManager._audioListener = VRCTrackingManager.instance.GetComponentInChildren<AudioListener>();
		}
		return VRCTrackingManager._audioListener;
	}

	// Token: 0x060056BE RID: 22206 RVA: 0x001DD6B2 File Offset: 0x001DBAB2
	public static void ImmobilizePlayer(bool active)
	{
		VRCTrackingManager.instance.playerImmobilized = active;
		if (active)
		{
			VRCTrackingManager.instance.playerImmobilizedInitialize = true;
		}
	}

	// Token: 0x060056BF RID: 22207 RVA: 0x001DD6D0 File Offset: 0x001DBAD0
	public static bool IsPlayerImmobilized()
	{
		return VRCTrackingManager.instance.playerImmobilized;
	}

	// Token: 0x060056C0 RID: 22208 RVA: 0x001DD6DC File Offset: 0x001DBADC
	public static void ApplyPlayerMotion(Vector3 playerWorldMotion, Quaternion playerWorldRotation)
	{
		if (VRCPlayer.Instance == null || VRCUiManager.Instance.IsActive())
		{
			return;
		}
		if (!VRCTrackingManager.instance.playerImmobilized || VRCTrackingManager.instance.playerImmobilizedInitialize)
		{
			VRCTrackingManager.instance.transform.position += playerWorldMotion;
			Vector3 worldTrackingPosition = VRCTrackingManager.GetWorldTrackingPosition();
			VRCTrackingManager.instance.transform.rotation *= playerWorldRotation;
			Vector3 worldTrackingPosition2 = VRCTrackingManager.GetWorldTrackingPosition();
			VRCTrackingManager.instance.transform.position += worldTrackingPosition - worldTrackingPosition2;
		}
	}

	// Token: 0x060056C1 RID: 22209 RVA: 0x001DD78A File Offset: 0x001DBB8A
	public static void ResetTrackingToOrigin()
	{
		VRCTrackingManager.instance.transform.position = Vector3.zero;
		VRCTrackingManager.instance.transform.rotation = Quaternion.identity;
		VRCTrackingManager.SetTrackingScale(Vector3.one);
	}

	// Token: 0x060056C2 RID: 22210 RVA: 0x001DD7C0 File Offset: 0x001DBBC0
	public static Transform GetTrackedTransform(VRCTracking.ID id)
	{
		if (VRCTrackingManager.instance == null || VRCTrackingManager.instance.activeTrackers == null)
		{
			return null;
		}
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			Transform trackedTransform = vrctracking.GetTrackedTransform(id);
			if (trackedTransform != null)
			{
				return trackedTransform;
			}
		}
		return null;
	}

	// Token: 0x060056C3 RID: 22211 RVA: 0x001DD858 File Offset: 0x001DBC58
	public static bool CanSupportHipTracking()
	{
		if (VRCTrackingManager.instance == null || VRCTrackingManager.instance.activeTrackers == null)
		{
			return false;
		}
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			if (vrctracking.CanSupportHipTracking())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060056C4 RID: 22212 RVA: 0x001DD8E8 File Offset: 0x001DBCE8
	public static bool CanSupportHipAndFeetTracking()
	{
		if (VRCTrackingManager.instance == null || VRCTrackingManager.instance.activeTrackers == null)
		{
			return false;
		}
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			if (vrctracking.CanSupportHipAndFeetTracking())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060056C5 RID: 22213 RVA: 0x001DD978 File Offset: 0x001DBD78
	public static bool IsTracked(VRCTracking.ID id)
	{
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			if (vrctracking.IsTracked(id))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060056C6 RID: 22214 RVA: 0x001DD9E8 File Offset: 0x001DBDE8
	public static bool AreHandsTracked()
	{
		return VRCTrackingManager.IsTracked(VRCTracking.ID.HandTracker_LeftWrist) && VRCTrackingManager.IsTracked(VRCTracking.ID.HandTracker_RightWrist);
	}

	// Token: 0x060056C7 RID: 22215 RVA: 0x001DDA00 File Offset: 0x001DBE00
	public static void GenerateHapticEvent(VRCTracking.ID location, float duration, float amplitude, float frequency)
	{
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			Transform trackedTransform = vrctracking.GetTrackedTransform(location);
			if (trackedTransform != null)
			{
				vrctracking.GenerateHapticEvent(location, duration, amplitude, frequency);
			}
		}
	}

	// Token: 0x060056C8 RID: 22216 RVA: 0x001DDA78 File Offset: 0x001DBE78
	public static Camera[] GetCameras()
	{
		return VRCTrackingManager.instance.GetComponentsInChildren<Camera>();
	}

	// Token: 0x060056C9 RID: 22217 RVA: 0x001DDA84 File Offset: 0x001DBE84
	public static T GetTrackingComponent<T>()
	{
		return VRCTrackingManager.instance.GetComponentInChildren<T>();
	}

	// Token: 0x060056CA RID: 22218 RVA: 0x001DDA90 File Offset: 0x001DBE90
	public static float GetRayReach(Ray ray)
	{
		float num = Mathf.Max(ray.origin.y - VRCTrackingManager.instance.transform.position.y, 0f);
		float num2 = 1.25f * VRCTrackingManager.GetTrackingScale();
		if (ray.direction.y >= -0.05f)
		{
			return num2;
		}
		Vector2 vector = new Vector2(ray.direction.x, ray.direction.z);
		float magnitude = vector.magnitude;
		float y = ray.direction.y;
		float num3 = y / magnitude;
		float num4 = -num / num2;
		if (num3 < num4)
		{
			return num * 1.1f;
		}
		return num2 / magnitude;
	}

	// Token: 0x060056CB RID: 22219 RVA: 0x001DDB5C File Offset: 0x001DBF5C
	public static void SetControllerVisibility(bool flag)
	{
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			vrctracking.SetControllerVisibility(flag);
		}
	}

	// Token: 0x060056CC RID: 22220 RVA: 0x001DDBBC File Offset: 0x001DBFBC
	public static ControllerUI GetControllerUI(ControllerHand hand)
	{
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			ControllerUI controllerUI = vrctracking.GetControllerUI(hand);
			if (controllerUI != null)
			{
				return controllerUI;
			}
		}
		return null;
	}

	// Token: 0x060056CD RID: 22221 RVA: 0x001DDC34 File Offset: 0x001DC034
	public static bool SetSeatedPlayMode(bool seated)
	{
		if (seated && VRCTrackingManager.instance.ik != null && VRCTrackingManager.instance.ik.HasLowerBodyTracking)
		{
			return false;
		}
		if (VRCTrackingManager.IsInVRMode())
		{
			using (List<VRCTracking>.Enumerator enumerator = VRCTrackingManager.instance.activeTrackers.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					VRCTracking vrctracking = enumerator.Current;
					vrctracking.SeatedPlay = seated;
					if (VRCTrackingManager.InSeatedPlay != vrctracking.SeatedPlay)
					{
						VRCTrackingManager.InSeatedPlay = vrctracking.SeatedPlay;
					}
					return VRCTrackingManager.InSeatedPlay;
				}
			}
			return true;
		}
		return true;
	}

	// Token: 0x060056CE RID: 22222 RVA: 0x001DDCF8 File Offset: 0x001DC0F8
	public static bool GetSeatedPlayMode()
	{
		return VRCTrackingManager.InSeatedPlay;
	}

	// Token: 0x060056CF RID: 22223 RVA: 0x001DDD00 File Offset: 0x001DC100
	public static bool IsPointWithinHMDView(Vector3 pt)
	{
		Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.Hmd);
		if (trackedTransform == null)
		{
			return false;
		}
		Vector3 vector = pt - trackedTransform.position;
		return vector.magnitude >= 0.1f && Vector3.Dot(trackedTransform.forward, vector.normalized) > Mathf.Cos(0.9599311f);
	}

	// Token: 0x060056D0 RID: 22224 RVA: 0x001DDD60 File Offset: 0x001DC160
	private static IEnumerator WaitThenRestoreLevel()
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		Vector3 eulers = VRCTrackingManager.instance.transform.localRotation.eulerAngles;
		eulers.x = (eulers.z = 0f);
		VRCTrackingManager.instance.transform.localRotation = Quaternion.Euler(eulers);
		yield break;
	}

	// Token: 0x060056D1 RID: 22225 RVA: 0x001DDD74 File Offset: 0x001DC174
	public static void RestoreLevel()
	{
		VRCTrackingManager.instance.StartCoroutine(VRCTrackingManager.WaitThenRestoreLevel());
	}

	// Token: 0x060056D2 RID: 22226 RVA: 0x001DDD86 File Offset: 0x001DC186
	public static void SetTrackingForCalibration()
	{
		VRCTrackingManager.SetControllerVisibility(true);
		if (VRCVrCamera.GetInstance().GetSitMode())
		{
			Debug.LogError("ERROR: You are trying to calibrate in seated play mode!");
		}
		VRCVrCamera.GetInstance().SetSitMode(false, false);
	}

	// Token: 0x060056D3 RID: 22227 RVA: 0x001DDDB4 File Offset: 0x001DC1B4
	public static bool IsCalibratedForAvatar(string avatarId)
	{
		if (VRCTrackingManager.instance == null || VRCTrackingManager.instance.activeTrackers == null)
		{
			return false;
		}
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			if (vrctracking.IsCalibratedForAvatar(avatarId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060056D4 RID: 22228 RVA: 0x001DDE44 File Offset: 0x001DC244
	public static void PerformCalibration(Animator avatarAnim, bool useHip, bool useFeet)
	{
		if (VRCTrackingManager.instance == null || VRCTrackingManager.instance.activeTrackers == null)
		{
			return;
		}
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			vrctracking.PerformCalibration(avatarAnim, useHip, useFeet);
		}
	}

	// Token: 0x060056D5 RID: 22229 RVA: 0x001DDEC8 File Offset: 0x001DC2C8
	public static void RestoreTrackingAfterCalibration()
	{
		VRCVrCamera.GetInstance().SetSitMode(false, false);
		VRCTrackingManager.SetControllerVisibility(false);
	}

	// Token: 0x060056D6 RID: 22230 RVA: 0x001DDEDD File Offset: 0x001DC2DD
	private static void SetTrackingScale(Vector3 scale)
	{
		VRCTrackingManager.instance.transform.localScale = scale;
		VRCTrackingManager.OnTrackingScaleChanged();
	}

	// Token: 0x060056D7 RID: 22231 RVA: 0x001DDEF4 File Offset: 0x001DC2F4
	private static void OnTrackingScaleChanged()
	{
		VRCTrackingManager.RefreshAudioListenerScale();
	}

	// Token: 0x060056D8 RID: 22232 RVA: 0x001DDEFC File Offset: 0x001DC2FC
	private static void RefreshAudioListenerScale()
	{
		float x = VRCTrackingManager.instance.transform.localScale.x;
		AudioListener[] array = UnityEngine.Object.FindObjectsOfType<AudioListener>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i])
			{
				array[i].transform.localScale = new Vector3(1f / x, 1f / x, 1f / x);
			}
		}
	}

	// Token: 0x060056D9 RID: 22233 RVA: 0x001DDF70 File Offset: 0x001DC370
	public static float MeasurePlayerHeight()
	{
		Vector3 localScale = VRCTrackingManager.instance.transform.localScale;
		VRCTrackingManager.SetTrackingScale(Vector3.one);
		bool sitMode = VRCVrCamera.GetInstance().GetSitMode();
		VRCVrCamera.GetInstance().SetCalibrationMode();
		foreach (VRCTracking vrctracking in VRCTrackingManager.instance.activeTrackers)
		{
			if (vrctracking.GetEyeHeight() != 0f)
			{
				VRCTrackingManager.SetTrackingScale(localScale);
				VRCVrCamera.GetInstance().SetSitMode(sitMode, false);
				return vrctracking.GetEyeHeight() / 0.9391f;
			}
		}
		VRCVrCamera.GetInstance().SetSitMode(sitMode, false);
		VRCTrackingManager.SetTrackingScale(localScale);
		return 0f;
	}

	// Token: 0x060056DA RID: 22234 RVA: 0x001DE04C File Offset: 0x001DC44C
	public static float GetPlayerHeight()
	{
		float num;
		if (PlayerPrefs.HasKey("PlayerHeight"))
		{
			num = PlayerPrefs.GetFloat("PlayerHeight", VRCTracking.DefaultPlayerHeight);
			if (num < 0.9144f || num > 2.4384f)
			{
				num = VRCTracking.DefaultPlayerHeight;
				PlayerPrefs.SetFloat("PlayerHeight", num);
			}
		}
		else
		{
			num = VRCTracking.DefaultPlayerHeight;
		}
		return num;
	}

	// Token: 0x060056DB RID: 22235 RVA: 0x001DE0AB File Offset: 0x001DC4AB
	public static void SetPlayerHeight(float ht)
	{
		VRCTrackingManager.ChangedPlayerHeight(ht);
		PlayerPrefs.SetFloat("PlayerHeight", ht);
	}

	// Token: 0x060056DC RID: 22236 RVA: 0x001DE0C0 File Offset: 0x001DC4C0
	public static void IncreasePlayerHeight()
	{
		float num = VRCTrackingManager.GetPlayerHeight();
		num += 0.0254f;
		if (num <= 2.4384f)
		{
			VRCTrackingManager.SetPlayerHeight(num);
		}
	}

	// Token: 0x060056DD RID: 22237 RVA: 0x001DE0EC File Offset: 0x001DC4EC
	public static void DecreasePlayerHeight()
	{
		float num = VRCTrackingManager.GetPlayerHeight();
		num -= 0.0254f;
		if (num >= 0.9144f)
		{
			VRCTrackingManager.SetPlayerHeight(num);
		}
	}

	// Token: 0x060056DE RID: 22238 RVA: 0x001DE118 File Offset: 0x001DC518
	public static void OnHandControllerAsleep()
	{
		if (VRCPlayer.Instance != null)
		{
			List<VRCHandGrasper> hands = VRCPlayer.Instance.hands;
			foreach (VRCHandGrasper vrchandGrasper in hands)
			{
				vrchandGrasper.SetSelectedObject(null, null);
			}
		}
		LocomotionInputController.navigationCursorActive = false;
	}

	// Token: 0x04003D9B RID: 15771
	private static VRCTrackingManager instance;

	// Token: 0x04003D9C RID: 15772
	public const string K_PLAYER_HEIGHT = "PlayerHeight";

	// Token: 0x04003D9D RID: 15773
	public bool ForceTestTracking;

	// Token: 0x04003D9E RID: 15774
	private Transform cameraMount;

	// Token: 0x04003D9F RID: 15775
	private VRCAvatarManager avatarMgr;

	// Token: 0x04003DA0 RID: 15776
	private AnimatorControllerManager animatorControllerMgr;

	// Token: 0x04003DA1 RID: 15777
	private IkController ik;

	// Token: 0x04003DA2 RID: 15778
	public GameObject UnityVrTracking;

	// Token: 0x04003DA3 RID: 15779
	public GameObject OculusTracking;

	// Token: 0x04003DA4 RID: 15780
	public GameObject SteamVrTracking;

	// Token: 0x04003DA5 RID: 15781
	public GameObject CardboardTracking;

	// Token: 0x04003DA6 RID: 15782
	public GameObject HydraTracking;

	// Token: 0x04003DA7 RID: 15783
	public GameObject HandProxyTracking;

	// Token: 0x04003DA8 RID: 15784
	public GameObject TestTracking;

	// Token: 0x04003DA9 RID: 15785
	private List<VRCTracking> activeTrackers = new List<VRCTracking>();

	// Token: 0x04003DAA RID: 15786
	private static float playerEyeHeight;

	// Token: 0x04003DAB RID: 15787
	private static float playerArmLength;

	// Token: 0x04003DAC RID: 15788
	private static float playerHeightAdjust;

	// Token: 0x04003DAD RID: 15789
	private static Vector3 _avatarViewPoint;

	// Token: 0x04003DAE RID: 15790
	private static bool _usingStationViewPoint;

	// Token: 0x04003DAF RID: 15791
	private static Vector3 _avatarStationViewPoint;

	// Token: 0x04003DB0 RID: 15792
	private const float EyeHeightToHeightRatio = 0.9391f;

	// Token: 0x04003DB1 RID: 15793
	private const float ArmLengthToEyeHeightRatio = 0.4537f;

	// Token: 0x04003DB2 RID: 15794
	private const float ShoulderToEyeHeightRatio = 0.8667f;

	// Token: 0x04003DB3 RID: 15795
	private const float PlayerHeightMin = 0.9144f;

	// Token: 0x04003DB4 RID: 15796
	private const float PlayerHeightMax = 2.4384f;

	// Token: 0x04003DB5 RID: 15797
	private const float TPoseRelax = 0.985f;

	// Token: 0x04003DB6 RID: 15798
	private const float TPoseRelaxStation = 0.98f;

	// Token: 0x04003DB7 RID: 15799
	private const float TPoseRelaxSeatedPlay = 0.96f;

	// Token: 0x04003DB8 RID: 15800
	private bool playerImmobilized;

	// Token: 0x04003DB9 RID: 15801
	private bool playerImmobilizedInitialize;

	// Token: 0x04003DBA RID: 15802
	private bool playerNearTracking = true;

	// Token: 0x04003DBB RID: 15803
	private Vector3 savedPosition;

	// Token: 0x04003DBC RID: 15804
	private Quaternion savedRotation;

	// Token: 0x04003DBD RID: 15805
	private float seatedEyePosition;

	// Token: 0x04003DBE RID: 15806
	private static AudioListener _audioListener;

	// Token: 0x04003DBF RID: 15807
	private const string K_PREF_SEATEDPLAY = "SeatedPlayEnabled";
}
