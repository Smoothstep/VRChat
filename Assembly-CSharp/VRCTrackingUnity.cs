using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x02000B2A RID: 2858
public class VRCTrackingUnity : VRCTracking
{
	// Token: 0x17000C9F RID: 3231
	// (get) Token: 0x0600570F RID: 22287 RVA: 0x001DFE35 File Offset: 0x001DE235
	// (set) Token: 0x06005710 RID: 22288 RVA: 0x001DFE41 File Offset: 0x001DE241
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

	// Token: 0x06005711 RID: 22289 RVA: 0x001DFE50 File Offset: 0x001DE250
	public override bool IsInVRMode()
	{
		return HMDManager.IsHmdDetected();
	}

	// Token: 0x06005712 RID: 22290 RVA: 0x001DFE58 File Offset: 0x001DE258
	public override void ResetHMDOrientation()
	{
		InputTracking.Recenter();
		VRCVrCamera instance = VRCVrCamera.GetInstance();
		instance.SetSitMode(instance.GetSitMode(), false);
	}

	// Token: 0x06005713 RID: 22291 RVA: 0x001DFE80 File Offset: 0x001DE280
	public override void ToggleSeatedPlay(bool recalibrate = true)
	{
		VRCVrCamera instance = VRCVrCamera.GetInstance();
		instance.SetSitMode(!instance.GetSitMode(), recalibrate);
	}

	// Token: 0x06005714 RID: 22292 RVA: 0x001DFEA4 File Offset: 0x001DE2A4
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

	// Token: 0x06005715 RID: 22293 RVA: 0x001DFEFC File Offset: 0x001DE2FC
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

	// Token: 0x06005716 RID: 22294 RVA: 0x001DFFA0 File Offset: 0x001DE3A0
	private void Awake()
	{
		this.currentHapticLoop = new Coroutine[2];
	}

	// Token: 0x06005717 RID: 22295 RVA: 0x001DFFB0 File Offset: 0x001DE3B0
	private void Start()
	{
		if (this.ControllerUIPrefabLeft != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ControllerUIPrefabLeft);
			if (gameObject != null)
			{
				gameObject.transform.parent = this.leftHandAnchor;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				this._leftControllerUI = gameObject.GetComponentInChildren<ControllerUI>();
				this._leftControllerUI.EnableVisibility(false);
			}
		}
		if (this.ControllerUIPrefabRight != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.ControllerUIPrefabRight);
			if (gameObject2 != null)
			{
				gameObject2.transform.parent = this.rightHandAnchor;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = Vector3.one;
				this._rightControllerUI = gameObject2.GetComponentInChildren<ControllerUI>();
				this._rightControllerUI.EnableVisibility(false);
			}
		}
	}

	// Token: 0x06005718 RID: 22296 RVA: 0x001E00C1 File Offset: 0x001DE4C1
	private void Update()
	{
		Time.fixedDeltaTime = Time.timeScale * VRCApplication.OriginalFixedDelta;
	}

	// Token: 0x17000CA0 RID: 3232
	// (get) Token: 0x06005719 RID: 22297 RVA: 0x001E00D3 File Offset: 0x001DE4D3
	public override bool calibrated
	{
		get
		{
			return this.leftWrist.gameObject.activeInHierarchy && this.rightWrist.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x0600571A RID: 22298 RVA: 0x001E0100 File Offset: 0x001DE500
	public override Transform GetTrackedTransform(VRCTracking.ID id)
	{
		Transform transform = null;
		switch (id)
		{
		case VRCTracking.ID.Hmd:
			transform = this.headTransform;
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
		try
		{
			if (id != VRCTracking.ID.Hmd && !transform.gameObject.activeInHierarchy)
			{
				transform = null;
			}
		}
		catch (UnassignedReferenceException)
		{
			transform = null;
		}
		return transform;
	}

	// Token: 0x0600571B RID: 22299 RVA: 0x001E0200 File Offset: 0x001DE600
	public override bool IsTracked(VRCTracking.ID id)
	{
		return this.GetTrackedTransform(id) != null;
	}

	// Token: 0x0600571C RID: 22300 RVA: 0x001E0218 File Offset: 0x001DE618
	public override void GenerateHapticEvent(VRCTracking.ID id, float duration, float amplitude, float frequency)
	{
		amplitude *= 2f;
		if (OVRManager.instance == null || !OVRManager.instance.enabled)
		{
			return;
		}
		if (duration <= 0f)
		{
			return;
		}
		int num = -1;
		switch (id)
		{
		case VRCTracking.ID.HandTracker_LeftWrist:
		case VRCTracking.ID.HandTracker_LeftPointer:
		case VRCTracking.ID.HandTracker_LeftGun:
		case VRCTracking.ID.HandTracker_LeftGrip:
		case VRCTracking.ID.HandTracker_LeftPalm:
			num = 0;
			break;
		case VRCTracking.ID.HandTracker_RightWrist:
		case VRCTracking.ID.HandTracker_RightPointer:
		case VRCTracking.ID.HandTracker_RightGun:
		case VRCTracking.ID.HandTracker_RightGrip:
		case VRCTracking.ID.HandTracker_RightPalm:
			num = 1;
			break;
		}
		if (num < 0)
		{
			return;
		}
		VRCTrackingUnity.HapticWave hapticWave = default(VRCTrackingUnity.HapticWave);
		hapticWave.controllerIndex = num;
		hapticWave.duration = duration;
		hapticWave.strength = Mathf.Clamp01(amplitude);
		hapticWave.frequency = frequency;
		if (this.currentHapticLoop[hapticWave.controllerIndex] != null)
		{
			base.StopCoroutine(this.currentHapticLoop[hapticWave.controllerIndex]);
		}
		this.currentHapticLoop[hapticWave.controllerIndex] = base.StartCoroutine("HapticLoop", hapticWave);
	}

	// Token: 0x0600571D RID: 22301 RVA: 0x001E031C File Offset: 0x001DE71C
	private IEnumerator HapticLoop(VRCTrackingUnity.HapticWave wave)
	{
		float timeElapsed = 0f;
		while (timeElapsed < wave.duration)
		{
			OVRInput.SetControllerVibration(wave.frequency, wave.strength, (wave.controllerIndex != 0) ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch);
			timeElapsed += Time.unscaledTime;
			yield return null;
		}
		OVRInput.SetControllerVibration(0f, 0f, (wave.controllerIndex != 0) ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch);
		this.currentHapticLoop[wave.controllerIndex] = null;
		yield break;
	}

	// Token: 0x0600571E RID: 22302 RVA: 0x001E033E File Offset: 0x001DE73E
	public override void SetControllerVisibility(bool flag)
	{
		if (this.LeftControllerRenderModel != null)
		{
			this.LeftControllerRenderModel.SetActive(flag);
		}
		if (this.RightControllerRenderModel != null)
		{
			this.RightControllerRenderModel.SetActive(flag);
		}
	}

	// Token: 0x0600571F RID: 22303 RVA: 0x001E037A File Offset: 0x001DE77A
	public override ControllerUI GetControllerUI(ControllerHand hand)
	{
		return (hand != ControllerHand.Left) ? this._rightControllerUI : this._leftControllerUI;
	}

	// Token: 0x04003E04 RID: 15876
	public Transform headTransform;

	// Token: 0x04003E05 RID: 15877
	public Transform leftHandAnchor;

	// Token: 0x04003E06 RID: 15878
	public Transform rightHandAnchor;

	// Token: 0x04003E07 RID: 15879
	public Transform leftWrist;

	// Token: 0x04003E08 RID: 15880
	public Transform rightWrist;

	// Token: 0x04003E09 RID: 15881
	public Transform leftPointer;

	// Token: 0x04003E0A RID: 15882
	public Transform rightPointer;

	// Token: 0x04003E0B RID: 15883
	public Transform leftGun;

	// Token: 0x04003E0C RID: 15884
	public Transform rightGun;

	// Token: 0x04003E0D RID: 15885
	public Transform leftGrip;

	// Token: 0x04003E0E RID: 15886
	public Transform rightGrip;

	// Token: 0x04003E0F RID: 15887
	public Transform leftPalm;

	// Token: 0x04003E10 RID: 15888
	public Transform rightPalm;

	// Token: 0x04003E11 RID: 15889
	public GameObject ControllerUIPrefabLeft;

	// Token: 0x04003E12 RID: 15890
	public GameObject ControllerUIPrefabRight;

	// Token: 0x04003E13 RID: 15891
	public GameObject LeftControllerRenderModel;

	// Token: 0x04003E14 RID: 15892
	public GameObject RightControllerRenderModel;

	// Token: 0x04003E15 RID: 15893
	private ControllerUI _leftControllerUI;

	// Token: 0x04003E16 RID: 15894
	private ControllerUI _rightControllerUI;

	// Token: 0x04003E17 RID: 15895
	private const float MIN_ARM_LENGTH = 0.3f;

	// Token: 0x04003E18 RID: 15896
	private const int HapticDevices = 2;

	// Token: 0x04003E19 RID: 15897
	private Coroutine[] currentHapticLoop;

	// Token: 0x02000B2B RID: 2859
	private struct HapticWave
	{
		// Token: 0x04003E1A RID: 15898
		public int controllerIndex;

		// Token: 0x04003E1B RID: 15899
		public float duration;

		// Token: 0x04003E1C RID: 15900
		public float strength;

		// Token: 0x04003E1D RID: 15901
		public float frequency;
	}
}
