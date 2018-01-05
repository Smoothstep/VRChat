using System;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x02000B0F RID: 2831
public class VRCVrCameraUnity : VRCVrCamera
{
	// Token: 0x060055C9 RID: 21961 RVA: 0x001D8E47 File Offset: 0x001D7247
	public override void Awake()
	{
		base.Awake();
		this.aimTransform = this.CameraStereo.transform;
		if (this.cameraLiftTransform == null)
		{
			this.cameraLiftTransform = base.gameObject.transform;
		}
	}

	// Token: 0x060055CA RID: 21962 RVA: 0x001D8E84 File Offset: 0x001D7284
	private void Start()
	{
		this._vrDetected = true;
		this._seatedPlay = false;
		if (!HMDManager.IsHmdDetected())
		{
			this._seatedSpace = true;
			this._seatedPlay = true;
			this._vrDetected = false;
		}
		if (this._seatedSpace)
		{
			OVRManager.instance.trackingOriginType = OVRManager.TrackingOrigin.EyeLevel;
			this.cameraLiftTransform.localPosition = new Vector3(0f, VRCTracking.DefaultEyeHeight, 0f);
		}
		else
		{
			OVRManager.instance.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;
			this.cameraLiftTransform.localPosition = Vector3.zero;
		}
		NeckMouseRotator componentInChildren = base.GetComponentInChildren<NeckMouseRotator>();
		componentInChildren.enabled = !this._vrDetected;
		if (this._seatedPlay && this._vrDetected)
		{
			InputTracking.Recenter();
		}
		base.StartCoroutine(base.RegisterOnEnteredWorldCallback());
	}

	// Token: 0x060055CB RID: 21963 RVA: 0x001D8F51 File Offset: 0x001D7351
	private void OnDestroy()
	{
		base.UnregisterOnEnteredWorldCallback();
	}

	// Token: 0x060055CC RID: 21964 RVA: 0x001D8F59 File Offset: 0x001D7359
	private void Update()
	{
		base.UpdateSitMode();
	}

	// Token: 0x060055CD RID: 21965 RVA: 0x001D8F61 File Offset: 0x001D7361
	public override bool ShouldDefaultToSeatedPlayMode()
	{
		return !this._vrDetected || (VRDevice.model.Contains("Oculus Rift") && !VRCInputManager.IsUsingHandController()) || VRCTrackingManager.GetSeatedPlayMode();
	}

	// Token: 0x060055CE RID: 21966 RVA: 0x001D8F93 File Offset: 0x001D7393
	public override bool GetSitMode()
	{
		return this._seatedPlay;
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x001D8F9C File Offset: 0x001D739C
	public override bool SetSitMode(bool sit, bool calibrate = false)
	{
		if (this._seatedSpace)
		{
			this.cameraLiftTransform.localPosition = new Vector3(0f, VRCTrackingManager.GetPlayerSeatedSpaceAdjustment(), 0f);
			return true;
		}
		bool flag = sit != this._seatedPlay;
		this._seatedPlay = sit;
		float y;
		if (this._seatedPlay)
		{
			bool recalibrate = calibrate;
			if (!this._seatedPlayCalibrated)
			{
				recalibrate = true;
				this._seatedPlayCalibrated = true;
			}
			y = VRCTrackingManager.GetPlayerSeatedPlayAdjustment(recalibrate);
		}
		else
		{
			y = VRCTrackingManager.GetPlayerHeightAdjustment();
		}
		this.cameraLiftTransform.localPosition = new Vector3(0f, y, 0f);
		if (flag)
		{
			InputTracking.Recenter();
		}
		return sit;
	}

	// Token: 0x060055D0 RID: 21968 RVA: 0x001D904C File Offset: 0x001D744C
	public override float GetLiftAmount()
	{
		return this.cameraLiftTransform.localPosition.y;
	}

	// Token: 0x060055D1 RID: 21969 RVA: 0x001D906C File Offset: 0x001D746C
	public override void SetCalibrationMode()
	{
		this.cameraLiftTransform.localPosition = Vector3.zero;
		this.cameraLiftTransform.localScale = Vector3.one;
	}

	// Token: 0x060055D2 RID: 21970 RVA: 0x001D908E File Offset: 0x001D748E
	public override bool IsTrackingPosition()
	{
		return VRDevice.model.Contains("Oculus Rift");
	}

	// Token: 0x060055D3 RID: 21971 RVA: 0x001D90A7 File Offset: 0x001D74A7
	public override Ray GetWorldLookRay()
	{
		return new Ray(this.aimTransform.position, this.aimTransform.forward);
	}

	// Token: 0x060055D4 RID: 21972 RVA: 0x001D90C4 File Offset: 0x001D74C4
	public override Vector3 GetLocalCameraPos()
	{
		if (HMDManager.IsHmdDetected())
		{
			return base.transform.localPosition + InputTracking.GetLocalPosition(VRNode.CenterEye);
		}
		return base.transform.parent.InverseTransformPoint(this.CameraStereo.transform.position);
	}

	// Token: 0x060055D5 RID: 21973 RVA: 0x001D9112 File Offset: 0x001D7512
	public override Vector3 GetWorldCameraPos()
	{
		return this.CameraStereo.transform.position;
	}

	// Token: 0x060055D6 RID: 21974 RVA: 0x001D9124 File Offset: 0x001D7524
	public override Quaternion GetLocalCameraRot()
	{
		if (HMDManager.IsHmdDetected())
		{
			return InputTracking.GetLocalRotation(VRNode.CenterEye);
		}
		return Quaternion.Inverse(base.transform.parent.rotation) * this.CameraStereo.transform.rotation;
	}

	// Token: 0x060055D7 RID: 21975 RVA: 0x001D9164 File Offset: 0x001D7564
	public override Vector3 GetLocalCameraNeckPos()
	{
		if (HMDManager.IsHmdDetected())
		{
			return InputTracking.GetLocalPosition(VRNode.CenterEye) - InputTracking.GetLocalRotation(VRNode.CenterEye) * this.neckToEyeOffset;
		}
		return base.transform.parent.InverseTransformPoint(base.transform.position);
	}

	// Token: 0x060055D8 RID: 21976 RVA: 0x001D91B4 File Offset: 0x001D75B4
	public override void SetMode(VRCVrCamera.CameraMode mode)
	{
		NeckMouseRotator componentInParent = base.GetComponentInParent<NeckMouseRotator>();
		if (mode != VRCVrCamera.CameraMode.Avatar)
		{
			if (mode == VRCVrCamera.CameraMode.Ui)
			{
				componentInParent.enabled = false;
			}
		}
		else if (HMDManager.IsHmdDetected())
		{
			Debug.Log("HMD detecting. Disabling vertical mouise");
			componentInParent.enabled = false;
		}
		else
		{
			componentInParent.enabled = true;
		}
	}

	// Token: 0x060055D9 RID: 21977 RVA: 0x001D9214 File Offset: 0x001D7614
	public override void InitializeCameras(CameraSelector selector)
	{
		Camera[] componentsInChildren = base.GetComponentsInChildren<Camera>();
		foreach (Camera camera in componentsInChildren)
		{
			camera.gameObject.SetActive(false);
			camera.cullingMask = selector.cameraLayers;
			camera.farClipPlane = selector.DefaultDrawDistance;
			camera.clearFlags = selector.clearFlags;
			camera.depth = (float)selector.depth;
			camera.backgroundColor = selector.clearColor;
			camera.renderingPath = selector.renderingPath;
		}
		this.screenCamera = this.CameraStereo.GetComponent<Camera>();
	}

	// Token: 0x060055DA RID: 21978 RVA: 0x001D92B0 File Offset: 0x001D76B0
	public override void SetNeckToEyeOffset(Vector3 avatarEyeToNeckVector)
	{
		this.neckToEyeOffset = avatarEyeToNeckVector / VRCTrackingManager.GetTrackingScale();
		this.CameraStereo.transform.localPosition = this.neckToEyeOffset;
		this.CameraLeft.transform.localPosition = this.neckToEyeOffset;
		this.CameraRight.transform.localPosition = this.neckToEyeOffset;
	}

	// Token: 0x060055DB RID: 21979 RVA: 0x001D9310 File Offset: 0x001D7710
	public override void SetStereoContentPresent()
	{
		if (VRCVrCamera.IsStereoRequired)
		{
			int num = LayerMask.NameToLayer("StereoLeft");
			int num2 = LayerMask.NameToLayer("StereoRight");
			this.CameraStereo.SetActive(false);
			this.CameraLeft.SetActive(true);
			if ((this.CameraLeft.GetComponent<Camera>().cullingMask & 1 << num2) != 0)
			{
				this.CameraLeft.GetComponent<Camera>().cullingMask -= 1 << num2;
			}
			this.CameraRight.SetActive(true);
			if ((this.CameraRight.GetComponent<Camera>().cullingMask & 1 << num) != 0)
			{
				this.CameraRight.GetComponent<Camera>().cullingMask -= 1 << num;
			}
			this.aimTransform = this.CameraLeft.transform;
		}
		else
		{
			this.CameraStereo.SetActive(true);
			this.CameraLeft.SetActive(false);
			this.CameraRight.SetActive(false);
			this.aimTransform = this.CameraStereo.transform;
		}
	}

	// Token: 0x060055DC RID: 21980 RVA: 0x001D941E File Offset: 0x001D781E
	public override void SetBlind(bool blind)
	{
		this.CameraStereo.GetComponent<Camera>().enabled = !blind;
		this.CameraLeft.GetComponent<Camera>().enabled = !blind;
		this.CameraRight.GetComponent<Camera>().enabled = !blind;
	}

	// Token: 0x04003C8A RID: 15498
	public GameObject CameraStereo;

	// Token: 0x04003C8B RID: 15499
	public GameObject CameraLeft;

	// Token: 0x04003C8C RID: 15500
	public GameObject CameraRight;

	// Token: 0x04003C8D RID: 15501
	private Transform aimTransform;

	// Token: 0x04003C8E RID: 15502
	private Vector3 neckToEyeOffset;

	// Token: 0x04003C8F RID: 15503
	public Transform cameraLiftTransform;

	// Token: 0x04003C90 RID: 15504
	private bool _vrDetected;

	// Token: 0x04003C91 RID: 15505
	private bool _seatedSpace;

	// Token: 0x04003C92 RID: 15506
	private bool _seatedPlay = true;

	// Token: 0x04003C93 RID: 15507
	private bool _seatedPlayCalibrated;
}
