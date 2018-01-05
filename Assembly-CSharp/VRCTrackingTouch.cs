using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR;

// Token: 0x02000B28 RID: 2856
public class VRCTrackingTouch : VRCTracking
{
	// Token: 0x17000C9D RID: 3229
	// (get) Token: 0x060056FC RID: 22268 RVA: 0x001DF60F File Offset: 0x001DDA0F
	// (set) Token: 0x060056FD RID: 22269 RVA: 0x001DF61B File Offset: 0x001DDA1B
	public override bool SeatedPlay
	{
		get
		{
			return VRCVrCamera.GetInstance().GetSitMode();
		}
		set
		{
			VRCVrCamera.GetInstance().SetSitMode(value, false);
		}
	}

	// Token: 0x060056FE RID: 22270 RVA: 0x001DF62A File Offset: 0x001DDA2A
	public override bool IsInVRMode()
	{
		return HMDManager.IsHmdDetected();
	}

	// Token: 0x060056FF RID: 22271 RVA: 0x001DF634 File Offset: 0x001DDA34
	public override void ResetHMDOrientation()
	{
		InputTracking.Recenter();
		VRCVrCamera instance = VRCVrCamera.GetInstance();
		instance.SetSitMode(instance.GetSitMode(), false);
	}

	// Token: 0x06005700 RID: 22272 RVA: 0x001DF65C File Offset: 0x001DDA5C
	public override void ToggleSeatedPlay(bool recalibrate = true)
	{
		VRCVrCamera instance = VRCVrCamera.GetInstance();
		instance.SetSitMode(!instance.GetSitMode(), recalibrate);
	}

	// Token: 0x06005701 RID: 22273 RVA: 0x001DF680 File Offset: 0x001DDA80
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

	// Token: 0x06005702 RID: 22274 RVA: 0x001DF6D8 File Offset: 0x001DDAD8
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

	// Token: 0x06005703 RID: 22275 RVA: 0x001DF77C File Offset: 0x001DDB7C
	private void Awake()
	{
		this.currentHapticLoop = new Coroutine[2];
		this.ovrManager = base.GetComponentInChildren<OVRManager>();
		this.ovrCameraRig = base.GetComponentInChildren<OVRCameraRig>();
		this.RefreshTrackerState();
	}

	// Token: 0x06005704 RID: 22276 RVA: 0x001DF7A8 File Offset: 0x001DDBA8
	private void Start()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ControllerUIPrefab);
		gameObject.transform.parent = this.leftWrist.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		this._leftControllerUI = gameObject.GetComponentInChildren<ControllerUI>();
		this._leftControllerUI.EnableVisibility(false);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.ControllerUIPrefab);
		gameObject2.transform.parent = this.rightWrist.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = Vector3.one;
		this._rightControllerUI = gameObject2.GetComponentInChildren<ControllerUI>();
		this._rightControllerUI.EnableVisibility(false);
	}

	// Token: 0x06005705 RID: 22277 RVA: 0x001DF889 File Offset: 0x001DDC89
	private void Update()
	{
		Time.fixedDeltaTime = Time.timeScale * VRCApplication.OriginalFixedDelta;
		this.RefreshTrackerState();
	}

	// Token: 0x06005706 RID: 22278 RVA: 0x001DF8A4 File Offset: 0x001DDCA4
	private IEnumerator HapticLoop(VRCTrackingTouch.HapticWave wave)
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
			Debug.LogWarning("Touch Controller Not Fully Implemented");
		}
		this.currentHapticLoop[(int)((UIntPtr)wave.controllerIndex)] = null;
		yield return null;
		yield break;
	}

	// Token: 0x06005707 RID: 22279 RVA: 0x001DF8C8 File Offset: 0x001DDCC8
	private void RefreshTrackerState()
	{
		if (this.ovrManager != null)
		{
			this.ovrManager.enabled = (VRSettings.enabled && this.IsInVRMode());
		}
		if (this.ovrCameraRig != null)
		{
			this.ovrCameraRig.enabled = (VRSettings.enabled && this.IsInVRMode());
			if (this.ovrCameraRig.enabled)
			{
				OVRInput.Controller connectedControllers = OVRInput.GetConnectedControllers();
				if (this.ovrCameraRig.leftHandAnchor != null)
				{
					this.ovrCameraRig.leftHandAnchor.gameObject.SetActive((connectedControllers & OVRInput.Controller.LTouch) != OVRInput.Controller.None);
				}
				if (this.ovrCameraRig.rightHandAnchor != null)
				{
					this.ovrCameraRig.rightHandAnchor.gameObject.SetActive((connectedControllers & OVRInput.Controller.RTouch) != OVRInput.Controller.None);
				}
			}
		}
	}

	// Token: 0x17000C9E RID: 3230
	// (get) Token: 0x06005708 RID: 22280 RVA: 0x001DF9B1 File Offset: 0x001DDDB1
	public override bool calibrated
	{
		get
		{
			return this.leftWrist.gameObject.activeInHierarchy && this.rightWrist.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06005709 RID: 22281 RVA: 0x001DF9DC File Offset: 0x001DDDDC
	public override Transform GetTrackedTransform(VRCTracking.ID id)
	{
		Transform transform = null;
		switch (id)
		{
		case VRCTracking.ID.Hmd:
			transform = this.centerEye;
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
		}
		if (!transform.gameObject.activeInHierarchy)
		{
			transform = null;
		}
		return transform;
	}

	// Token: 0x0600570A RID: 22282 RVA: 0x001DFABC File Offset: 0x001DDEBC
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
		VRCTrackingTouch.HapticWave hapticWave = default(VRCTrackingTouch.HapticWave);
		hapticWave.controllerIndex = system.GetTrackedDeviceIndexForControllerRole(unDeviceType);
		hapticWave.duration = duration;
		hapticWave.strength = Mathf.Clamp01(amplitude);
		hapticWave.frequency = frequency;
		if (this.currentHapticLoop[(int)((UIntPtr)hapticWave.controllerIndex)] != null)
		{
			base.StopCoroutine(this.currentHapticLoop[(int)((UIntPtr)hapticWave.controllerIndex)]);
		}
		this.currentHapticLoop[(int)((UIntPtr)hapticWave.controllerIndex)] = base.StartCoroutine("HapticLoop", hapticWave);
	}

	// Token: 0x0600570B RID: 22283 RVA: 0x001DFB78 File Offset: 0x001DDF78
	public override bool IsTracked(VRCTracking.ID id)
	{
		bool result = false;
		switch (id)
		{
		case VRCTracking.ID.Hmd:
			result = true;
			break;
		case VRCTracking.ID.HandTracker_LeftWrist:
			result = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightWrist:
			result = this.rightWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_LeftPointer:
			result = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightPointer:
			result = this.rightWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_LeftGun:
			result = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightGun:
			result = this.rightWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_LeftGrip:
			result = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightGrip:
			result = this.rightWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_LeftPalm:
			result = this.leftWrist.gameObject.activeInHierarchy;
			break;
		case VRCTracking.ID.HandTracker_RightPalm:
			result = this.rightWrist.gameObject.activeInHierarchy;
			break;
		}
		return result;
	}

	// Token: 0x0600570C RID: 22284 RVA: 0x001DFCA2 File Offset: 0x001DE0A2
	public override void SetControllerVisibility(bool flag)
	{
		Debug.LogWarning("Touch Controller Not Fully Implemented");
	}

	// Token: 0x0600570D RID: 22285 RVA: 0x001DFCAE File Offset: 0x001DE0AE
	public override ControllerUI GetControllerUI(ControllerHand hand)
	{
		return (hand != ControllerHand.Left) ? this._rightControllerUI : this._leftControllerUI;
	}

	// Token: 0x04003DED RID: 15853
	public Transform centerEye;

	// Token: 0x04003DEE RID: 15854
	public Transform leftWrist;

	// Token: 0x04003DEF RID: 15855
	public Transform rightWrist;

	// Token: 0x04003DF0 RID: 15856
	public Transform leftPointer;

	// Token: 0x04003DF1 RID: 15857
	public Transform rightPointer;

	// Token: 0x04003DF2 RID: 15858
	public Transform leftGun;

	// Token: 0x04003DF3 RID: 15859
	public Transform rightGun;

	// Token: 0x04003DF4 RID: 15860
	public Transform leftGrip;

	// Token: 0x04003DF5 RID: 15861
	public Transform rightGrip;

	// Token: 0x04003DF6 RID: 15862
	public Transform leftPalm;

	// Token: 0x04003DF7 RID: 15863
	public Transform rightPalm;

	// Token: 0x04003DF8 RID: 15864
	public GameObject ControllerUIPrefab;

	// Token: 0x04003DF9 RID: 15865
	private ControllerUI _leftControllerUI;

	// Token: 0x04003DFA RID: 15866
	private ControllerUI _rightControllerUI;

	// Token: 0x04003DFB RID: 15867
	private const float MIN_ARM_LENGTH = 0.3f;

	// Token: 0x04003DFC RID: 15868
	private const int HapticDevices = 2;

	// Token: 0x04003DFD RID: 15869
	private Coroutine[] currentHapticLoop;

	// Token: 0x04003DFE RID: 15870
	private OVRManager ovrManager;

	// Token: 0x04003DFF RID: 15871
	private OVRCameraRig ovrCameraRig;

	// Token: 0x02000B29 RID: 2857
	private struct HapticWave
	{
		// Token: 0x04003E00 RID: 15872
		public uint controllerIndex;

		// Token: 0x04003E01 RID: 15873
		public float duration;

		// Token: 0x04003E02 RID: 15874
		public float strength;

		// Token: 0x04003E03 RID: 15875
		public float frequency;
	}
}
