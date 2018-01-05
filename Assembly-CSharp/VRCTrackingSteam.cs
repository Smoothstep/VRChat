using System;
using System.Collections;
using UnityEngine;
using Valve.VR;

// Token: 0x02000B25 RID: 2853
public class VRCTrackingSteam : VRCTracking
{
	// Token: 0x17000C9A RID: 3226
	// (get) Token: 0x060056E1 RID: 22241 RVA: 0x001DE680 File Offset: 0x001DCA80
	private SteamVR_Camera steamCamera
	{
		get
		{
			if (this._steamCamera == null)
			{
				this._steamCamera = base.GetComponentInChildren<SteamVR_Camera>();
			}
			return this._steamCamera;
		}
	}

	// Token: 0x17000C9B RID: 3227
	// (get) Token: 0x060056E2 RID: 22242 RVA: 0x001DE6A5 File Offset: 0x001DCAA5
	// (set) Token: 0x060056E3 RID: 22243 RVA: 0x001DE6B1 File Offset: 0x001DCAB1
	public override bool SeatedPlay
	{
		get
		{
			return VRCVrCamera.GetInstance().GetSitMode();
		}
		set
		{
			VRCVrCamera.GetInstance().SetSitMode(value, true);
		}
	}

	// Token: 0x060056E4 RID: 22244 RVA: 0x001DE6C0 File Offset: 0x001DCAC0
	public override bool IsInVRMode()
	{
		bool result = true;
		if (!SteamVR.active || (SteamVR.instance == null && SteamVR.instance.hmd == null))
		{
			result = false;
		}
		return result;
	}

	// Token: 0x060056E5 RID: 22245 RVA: 0x001DE6F8 File Offset: 0x001DCAF8
	public override void ResetHMDOrientation()
	{
		if (SteamVR.active && SteamVR.instance != null && SteamVR.instance.hmd != null)
		{
			SteamVR.instance.hmd.ResetSeatedZeroPose();
		}
		VRCVrCamera instance = VRCVrCamera.GetInstance();
		instance.SetSitMode(instance.GetSitMode(), false);
	}

	// Token: 0x060056E6 RID: 22246 RVA: 0x001DE74C File Offset: 0x001DCB4C
	public override void ToggleSeatedPlay(bool recalibrate = true)
	{
		VRCVrCamera instance = VRCVrCamera.GetInstance();
		instance.SetSitMode(!instance.GetSitMode(), recalibrate);
	}

	// Token: 0x060056E7 RID: 22247 RVA: 0x001DE770 File Offset: 0x001DCB70
	public override float GetEyeHeight()
	{
		float result = VRCTracking.DefaultEyeHeight;
		Transform trackedTransform = this.GetTrackedTransform(VRCTracking.ID.Hmd);
		if (trackedTransform.position.y != 0f)
		{
			result = trackedTransform.position.y - base.transform.position.y;
		}
		return result;
	}

	// Token: 0x060056E8 RID: 22248 RVA: 0x001DE7C8 File Offset: 0x001DCBC8
	public override float GetArmLength()
	{
		float result = VRCTracking.DefaultArmLength;
		Transform trackedTransform = this.GetTrackedTransform(VRCTracking.ID.Hmd);
		Transform trackedTransform2 = this.GetTrackedTransform(VRCTracking.ID.HandTracker_LeftWrist);
		Transform trackedTransform3 = this.GetTrackedTransform(VRCTracking.ID.HandTracker_RightWrist);
		float a = 0f;
		float b = 0f;
		if (trackedTransform2 != null)
		{
			a = (trackedTransform2.position - trackedTransform.position).magnitude;
		}
		if (trackedTransform3 != null)
		{
			b = (trackedTransform3.position - trackedTransform.position).magnitude;
		}
		float num = Mathf.Max(a, b);
		if (num >= 0.3f)
		{
			result = num;
		}
		return result;
	}

	// Token: 0x060056E9 RID: 22249 RVA: 0x001DE86C File Offset: 0x001DCC6C
	private void Awake()
	{
		this.controllerManager = base.GetComponentInChildren<SteamVR_ControllerManager>();
		this.currentHapticLoop = new Coroutine[16];
	}

	// Token: 0x060056EA RID: 22250 RVA: 0x001DE888 File Offset: 0x001DCC88
	private void Start()
	{
		bool flag = false;
		if (SteamVR.instance != null && SteamVR.instance.hmd_TrackingSystemName.Contains("oculus"))
		{
			flag = true;
		}
		GameObject gameObject = (!flag) ? this.ControllerUIPrefabVive : this.ControllerUIPrefabTouchLeft;
		GameObject gameObject2 = (!flag) ? this.ControllerUIPrefabVive : this.ControllerUIPrefabTouchRight;
		if (gameObject != null)
		{
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			if (gameObject3 != null)
			{
				gameObject3.transform.parent = ((!flag) ? this.controllerManager.left.transform : this.ControllerUITouchRootLeft);
				gameObject3.transform.localPosition = Vector3.zero;
				gameObject3.transform.localRotation = Quaternion.identity;
				gameObject3.transform.localScale = Vector3.one;
			}
		}
		if (gameObject2 != null)
		{
			GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
			if (gameObject4 != null)
			{
				gameObject4.transform.parent = ((!flag) ? this.controllerManager.right.transform : this.ControllerUITouchRootRight);
				gameObject4.transform.localPosition = Vector3.zero;
				gameObject4.transform.localRotation = Quaternion.identity;
				gameObject4.transform.localScale = Vector3.one;
			}
		}
		this._leftControllerUI = this.controllerManager.left.GetComponentInChildren<ControllerUI>();
		this._rightControllerUI = this.controllerManager.right.GetComponentInChildren<ControllerUI>();
		this._leftControllerUI.EnableVisibility(false);
		this._rightControllerUI.EnableVisibility(false);
	}

	// Token: 0x060056EB RID: 22251 RVA: 0x001DEA2C File Offset: 0x001DCE2C
	private void Update()
	{
		if (!this.IsInVRMode())
		{
			Time.fixedDeltaTime = Time.timeScale * VRCApplication.OriginalFixedDelta;
		}
		bool flag = VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Vive || VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Oculus;
		if (flag)
		{
			this._areControllersAsleep = false;
			this._usingHandControllers = true;
		}
		else if (!this._areControllersAsleep)
		{
			if (this._usingHandControllers)
			{
				this._usingHandControllers = false;
				this._controllerSleepTimeoutStart = Time.unscaledTime;
			}
			if (Time.unscaledTime - this._controllerSleepTimeoutStart > 1f)
			{
				this._areControllersAsleep = true;
				VRCTrackingManager.OnHandControllerAsleep();
			}
		}
	}

	// Token: 0x060056EC RID: 22252 RVA: 0x001DEAD0 File Offset: 0x001DCED0
	private IEnumerator HapticLoop(VRCTrackingSteam.HapticWave wave)
	{
		float timeElapsed = 0f;
		float cycleWait = 0.005f;
		float pulseLengthUSec = wave.strength * 3998f + 1f;
		float pulseLength = pulseLengthUSec / 1000000f;
		float period = 0.5f / wave.frequency;
		if (period < pulseLength + cycleWait)
		{
			period = pulseLength + cycleWait;
		}
		else
		{
			cycleWait = period - pulseLength;
		}
		while (timeElapsed < wave.duration * 0.5f)
		{
			float cycleStart = Time.unscaledTime;
			float cycleTime = 0f;
			SteamVR_Controller.Input((int)wave.controllerIndex).TriggerHapticPulse((ushort)Mathf.RoundToInt(pulseLengthUSec), EVRButtonId.k_EButton_Axis0);
			while (cycleTime < period)
			{
				yield return null;
				float measuredDelta = Time.unscaledTime - cycleStart;
				cycleTime += measuredDelta;
				timeElapsed += measuredDelta;
				cycleStart = Time.unscaledTime;
			}
		}
		this.currentHapticLoop[(int)((UIntPtr)wave.controllerIndex)] = null;
		yield return null;
		yield break;
	}

	// Token: 0x17000C9C RID: 3228
	// (get) Token: 0x060056ED RID: 22253 RVA: 0x001DEAF4 File Offset: 0x001DCEF4
	public override bool calibrated
	{
		get
		{
			if (this.CanSupportHipAndFeetTracking())
			{
				return this.leftWrist.gameObject.activeInHierarchy && this.rightWrist.gameObject.activeInHierarchy && this.leftFoot.gameObject.activeInHierarchy && this.rightFoot.gameObject.activeInHierarchy && this.hip.gameObject.activeInHierarchy;
			}
			if (this.CanSupportHipTracking())
			{
				return this.leftWrist.gameObject.activeInHierarchy && this.rightWrist.gameObject.activeInHierarchy && this.hip.gameObject.activeInHierarchy;
			}
			return this.leftWrist.gameObject.activeInHierarchy && this.rightWrist.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x060056EE RID: 22254 RVA: 0x001DEBE8 File Offset: 0x001DCFE8
	private Transform ActivateClosestTracker(Animator anim, HumanBodyBones bone)
	{
		if (bone != HumanBodyBones.Hips && bone != HumanBodyBones.LeftFoot && bone != HumanBodyBones.RightFoot)
		{
			Debug.LogError("SteamTracking: Tried to track unsupported body part:" + bone.ToString());
			return null;
		}
		Vector3 position = anim.GetBoneTransform(bone).position;
		if (this.controllerManager != null)
		{
			int num = 0;
			Vector3 a = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			for (int i = 2; i < 5; i++)
			{
				Vector3 position2 = this.controllerManager.objects[i].transform.position;
				if ((a - position).magnitude > (position2 - position).magnitude)
				{
					num = i;
					a = position2;
				}
			}
			if (num > 0)
			{
				Transform transform = this.controllerManager.objects[num].transform;
				Transform transform2 = transform.Find("offset");
				Vector3 zero = Vector3.zero;
				transform2.position = anim.GetBoneTransform(bone).position;
				transform2.rotation = anim.GetBoneTransform(bone).rotation;
				return transform2;
			}
		}
		return null;
	}

	// Token: 0x060056EF RID: 22255 RVA: 0x001DED0D File Offset: 0x001DD10D
	public override bool IsCalibratedForAvatar(string avatarId)
	{
		return this.calibratedAvatarId != null && avatarId == this.calibratedAvatarId;
	}

	// Token: 0x060056F0 RID: 22256 RVA: 0x001DED30 File Offset: 0x001DD130
	public override void PerformCalibration(Animator avatarAnim, bool includeHip, bool includeFeet)
	{
		if (avatarAnim == null)
		{
			return;
		}
		if (!avatarAnim.isHuman)
		{
			return;
		}
		if (includeHip)
		{
			this.hip = this.ActivateClosestTracker(avatarAnim, HumanBodyBones.Hips);
		}
		if (includeFeet)
		{
			this.leftFoot = this.ActivateClosestTracker(avatarAnim, HumanBodyBones.LeftFoot);
			this.rightFoot = this.ActivateClosestTracker(avatarAnim, HumanBodyBones.RightFoot);
		}
		if (avatarAnim.transform.parent != null)
		{
			VRCAvatarManager component = avatarAnim.transform.parent.GetComponent<VRCAvatarManager>();
			if (component != null)
			{
				this.calibratedAvatarId = component.CurrentAvatar.id;
			}
		}
	}

	// Token: 0x060056F1 RID: 22257 RVA: 0x001DEDD0 File Offset: 0x001DD1D0
	public override Transform GetTrackedTransform(VRCTracking.ID id)
	{
		Transform transform = null;
		switch (id)
		{
		case VRCTracking.ID.Hmd:
			transform = this.steamCamera.transform;
			break;
		case VRCTracking.ID.HandTracker_LeftWrist:
			transform = this.leftWrist;
			break;
		case VRCTracking.ID.HandTracker_RightWrist:
			transform = this.rightWrist;
			break;
		case VRCTracking.ID.HandTracker_LeftPointer:
			transform = this.leftPointer;
			break;
		case VRCTracking.ID.HandTracker_RightPointer:
			transform = this.rightPointer;
			break;
		case VRCTracking.ID.HandTracker_LeftGun:
			transform = this.leftGun;
			break;
		case VRCTracking.ID.HandTracker_RightGun:
			transform = this.rightGun;
			break;
		case VRCTracking.ID.HandTracker_LeftGrip:
			transform = this.leftGrip;
			break;
		case VRCTracking.ID.HandTracker_RightGrip:
			transform = this.rightGrip;
			break;
		case VRCTracking.ID.HandTracker_LeftPalm:
			transform = this.leftPalm;
			break;
		case VRCTracking.ID.HandTracker_RightPalm:
			transform = this.rightPalm;
			break;
		case VRCTracking.ID.FootTracker_LeftFoot:
			transform = this.leftFoot;
			break;
		case VRCTracking.ID.FootTracker_RightFoot:
			transform = this.rightFoot;
			break;
		case VRCTracking.ID.BodyTracker_Hip:
			transform = this.hip;
			break;
		}
		if (!transform.gameObject.activeInHierarchy || (id != VRCTracking.ID.Hmd && this._areControllersAsleep))
		{
			transform = null;
		}
		return transform;
	}

	// Token: 0x060056F2 RID: 22258 RVA: 0x001DEEF4 File Offset: 0x001DD2F4
	public override void GenerateHapticEvent(VRCTracking.ID id, float duration, float amplitude, float frequency)
	{
		CVRSystem system = OpenVR.System;
		if (system == null)
		{
			return;
		}
		ETrackedControllerRole unDeviceType = ETrackedControllerRole.Invalid;
		if (id != VRCTracking.ID.HandTracker_LeftWrist)
		{
			if (id == VRCTracking.ID.HandTracker_RightWrist)
			{
				unDeviceType = ETrackedControllerRole.RightHand;
			}
		}
		else
		{
			unDeviceType = ETrackedControllerRole.LeftHand;
		}
		uint trackedDeviceIndexForControllerRole = system.GetTrackedDeviceIndexForControllerRole(unDeviceType);
		if (trackedDeviceIndexForControllerRole < 0u || trackedDeviceIndexForControllerRole >= 16u)
		{
			return;
		}
		VRCTrackingSteam.HapticWave hapticWave = default(VRCTrackingSteam.HapticWave);
		hapticWave.controllerIndex = trackedDeviceIndexForControllerRole;
		hapticWave.duration = duration;
		hapticWave.strength = Mathf.Clamp01(amplitude);
		hapticWave.frequency = frequency;
		if (this.currentHapticLoop[(int)((UIntPtr)hapticWave.controllerIndex)] != null)
		{
			base.StopCoroutine(this.currentHapticLoop[(int)((UIntPtr)hapticWave.controllerIndex)]);
		}
		this.currentHapticLoop[(int)((UIntPtr)hapticWave.controllerIndex)] = base.StartCoroutine("HapticLoop", hapticWave);
	}

	// Token: 0x060056F3 RID: 22259 RVA: 0x001DEFC0 File Offset: 0x001DD3C0
	public override bool IsTracked(VRCTracking.ID id)
	{
		bool flag = false;
		switch (id)
		{
		case VRCTracking.ID.Hmd:
			flag = true;
			break;
		case VRCTracking.ID.HandTracker_LeftWrist:
			flag = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightWrist:
			flag = this.rightWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_LeftPointer:
			flag = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightPointer:
			flag = this.rightWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_LeftGun:
			flag = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightGun:
			flag = this.rightWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_LeftGrip:
			flag = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightGrip:
			flag = this.rightWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_LeftPalm:
			flag = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightPalm:
			flag = this.rightWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.FootTracker_LeftFoot:
			flag = (!(this.leftFoot == null) && this.leftFoot.gameObject.activeInHierarchy);
			break;
		case VRCTracking.ID.FootTracker_RightFoot:
			flag = (!(this.rightFoot == null) && this.rightFoot.gameObject.activeInHierarchy);
			break;
		case VRCTracking.ID.BodyTracker_Hip:
			flag = (!(this.hip == null) && this.hip.gameObject.activeInHierarchy);
			break;
		}
		return (id == VRCTracking.ID.Hmd || !this._areControllersAsleep) && flag;
	}

	// Token: 0x060056F4 RID: 22260 RVA: 0x001DF190 File Offset: 0x001DD590
	public override bool CanSupportHipTracking()
	{
		return this.controllerManager != null && this.controllerManager.objects.Length >= 3 && this.controllerManager.objects[2].activeInHierarchy;
	}

	// Token: 0x060056F5 RID: 22261 RVA: 0x001DF1D0 File Offset: 0x001DD5D0
	public override bool CanSupportHipAndFeetTracking()
	{
		return this.controllerManager != null && this.controllerManager.objects.Length >= 5 && this.controllerManager.objects[2].activeInHierarchy && this.controllerManager.objects[3].activeInHierarchy && this.controllerManager.objects[4].activeInHierarchy;
	}

	// Token: 0x060056F6 RID: 22262 RVA: 0x001DF24C File Offset: 0x001DD64C
	public override void SetControllerVisibility(bool flag)
	{
		if (this.controllerManager != null)
		{
			GameObject gameObject = this.controllerManager.left.transform.Find("Model").gameObject;
			GameObject gameObject2 = this.controllerManager.right.transform.Find("Model").gameObject;
			if (gameObject != null)
			{
				gameObject.SetActive(flag);
			}
			if (gameObject2 != null)
			{
				gameObject2.SetActive(flag);
			}
			for (int i = 0; i < this.controllerManager.objects.Length; i++)
			{
				GameObject gameObject3 = this.controllerManager.objects[i].transform.Find("Model").gameObject;
				if (gameObject3 != null && (gameObject3 != gameObject || gameObject3 != gameObject2))
				{
					gameObject3.SetActive(flag);
				}
			}
		}
	}

	// Token: 0x060056F7 RID: 22263 RVA: 0x001DF33B File Offset: 0x001DD73B
	public override ControllerUI GetControllerUI(ControllerHand hand)
	{
		if (hand == ControllerHand.None)
		{
			return null;
		}
		return (hand != ControllerHand.Left) ? this._rightControllerUI : this._leftControllerUI;
	}

	// Token: 0x04003DC0 RID: 15808
	public Transform leftWrist;

	// Token: 0x04003DC1 RID: 15809
	public Transform rightWrist;

	// Token: 0x04003DC2 RID: 15810
	public Transform leftPointer;

	// Token: 0x04003DC3 RID: 15811
	public Transform rightPointer;

	// Token: 0x04003DC4 RID: 15812
	public Transform leftGun;

	// Token: 0x04003DC5 RID: 15813
	public Transform rightGun;

	// Token: 0x04003DC6 RID: 15814
	public Transform leftGrip;

	// Token: 0x04003DC7 RID: 15815
	public Transform rightGrip;

	// Token: 0x04003DC8 RID: 15816
	public Transform leftPalm;

	// Token: 0x04003DC9 RID: 15817
	public Transform rightPalm;

	// Token: 0x04003DCA RID: 15818
	public Transform leftFoot;

	// Token: 0x04003DCB RID: 15819
	public Transform rightFoot;

	// Token: 0x04003DCC RID: 15820
	public Transform hip;

	// Token: 0x04003DCD RID: 15821
	public GameObject ControllerUIPrefabVive;

	// Token: 0x04003DCE RID: 15822
	public GameObject ControllerUIPrefabTouchLeft;

	// Token: 0x04003DCF RID: 15823
	public GameObject ControllerUIPrefabTouchRight;

	// Token: 0x04003DD0 RID: 15824
	public Transform ControllerUITouchRootLeft;

	// Token: 0x04003DD1 RID: 15825
	public Transform ControllerUITouchRootRight;

	// Token: 0x04003DD2 RID: 15826
	private string calibratedAvatarId;

	// Token: 0x04003DD3 RID: 15827
	private SteamVR_Camera _steamCamera;

	// Token: 0x04003DD4 RID: 15828
	private SteamVR_ControllerManager controllerManager;

	// Token: 0x04003DD5 RID: 15829
	private ControllerUI _leftControllerUI;

	// Token: 0x04003DD6 RID: 15830
	private ControllerUI _rightControllerUI;

	// Token: 0x04003DD7 RID: 15831
	private const float MIN_ARM_LENGTH = 0.3f;

	// Token: 0x04003DD8 RID: 15832
	private const uint OPENVR_MAX_TRACKED_DEVICES = 16u;

	// Token: 0x04003DD9 RID: 15833
	private Coroutine[] currentHapticLoop;

	// Token: 0x04003DDA RID: 15834
	private bool _usingHandControllers;

	// Token: 0x04003DDB RID: 15835
	private bool _areControllersAsleep;

	// Token: 0x04003DDC RID: 15836
	private float _controllerSleepTimeoutStart;

	// Token: 0x04003DDD RID: 15837
	private const float CONTROLLER_SLEEP_TIMEOUT = 1f;

	// Token: 0x02000B26 RID: 2854
	private struct HapticWave
	{
		// Token: 0x04003DDE RID: 15838
		public uint controllerIndex;

		// Token: 0x04003DDF RID: 15839
		public float duration;

		// Token: 0x04003DE0 RID: 15840
		public float strength;

		// Token: 0x04003DE1 RID: 15841
		public float frequency;
	}
}
