using System;
using UnityEngine;
using Valve.VR;

// Token: 0x02000B0E RID: 2830
public class VRCVrCameraSteam : VRCVrCamera
{
	// Token: 0x060055B2 RID: 21938 RVA: 0x001D894C File Offset: 0x001D6D4C
	public override void Awake()
	{
		base.Awake();
		this.mouseGaze = base.GetComponentInChildren<NeckMouseRotator>();
		this.neckTransform = this.mouseGaze.transform;
		this.gameView = base.GetComponentInChildren<SteamVR_GameView>();
		this.eyeTransform = this.neckTransform.GetChild(0);
		this.postCamera = this.gameView.GetComponent<Camera>();
		SteamVR_Camera componentInChildren = base.GetComponentInChildren<SteamVR_Camera>();
		this.eyeCamera = componentInChildren.GetComponent<Camera>();
	}

	// Token: 0x060055B3 RID: 21939 RVA: 0x001D89C0 File Offset: 0x001D6DC0
	private void Start()
	{
		this._vrDetected = true;
		if (!SteamVR.active || (SteamVR.instance == null && SteamVR.instance.hmd == null))
		{
			this._seatedSpace = true;
			this._seatedPlay = true;
			this._vrDetected = false;
		}
		if (this._seatedSpace)
		{
			this.steamVrRenderer.trackingSpace = ETrackingUniverseOrigin.TrackingUniverseSeated;
			this.cameraLiftTransform.localPosition = new Vector3(0f, VRCTracking.DefaultEyeHeight, 0f);
		}
		else
		{
			this.steamVrRenderer.trackingSpace = ETrackingUniverseOrigin.TrackingUniverseStanding;
			this.cameraLiftTransform.localPosition = Vector3.zero;
		}
		this.mouseGaze.enabled = !this._vrDetected;
		base.StartCoroutine(base.RegisterOnEnteredWorldCallback());
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x001D8A84 File Offset: 0x001D6E84
	private void OnDestroy()
	{
		base.UnregisterOnEnteredWorldCallback();
	}

	// Token: 0x060055B5 RID: 21941 RVA: 0x001D8A8C File Offset: 0x001D6E8C
	public override bool ShouldDefaultToSeatedPlayMode()
	{
		return !this._vrDetected || !SteamVR.active || (SteamVR.instance == null && SteamVR.instance.hmd == null) || (SteamVR.instance.hmd_TrackingSystemName == "oculus" && !VRCInputManager.IsUsingHandController()) || VRCTrackingManager.GetSeatedPlayMode();
	}

	// Token: 0x060055B6 RID: 21942 RVA: 0x001D8AF3 File Offset: 0x001D6EF3
	public override bool GetSitMode()
	{
		return this._seatedPlay;
	}

	// Token: 0x060055B7 RID: 21943 RVA: 0x001D8AFC File Offset: 0x001D6EFC
	public override bool SetSitMode(bool sit, bool calibrate = false)
	{
		if (this._seatedSpace)
		{
			this.cameraLiftTransform.localPosition = new Vector3(0f, VRCTrackingManager.GetPlayerSeatedSpaceAdjustment(), 0f);
			return true;
		}
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
		return sit;
	}

	// Token: 0x060055B8 RID: 21944 RVA: 0x001D8B94 File Offset: 0x001D6F94
	public override float GetLiftAmount()
	{
		return this.cameraLiftTransform.localPosition.y;
	}

	// Token: 0x060055B9 RID: 21945 RVA: 0x001D8BB4 File Offset: 0x001D6FB4
	public override void SetCalibrationMode()
	{
		this.cameraLiftTransform.localPosition = Vector3.zero;
		this.cameraLiftTransform.localScale = Vector3.one;
	}

	// Token: 0x060055BA RID: 21946 RVA: 0x001D8BD6 File Offset: 0x001D6FD6
	public override bool IsTrackingPosition()
	{
		return true;
	}

	// Token: 0x060055BB RID: 21947 RVA: 0x001D8BDC File Offset: 0x001D6FDC
	public override Ray GetWorldLookRay()
	{
		Transform transform = this.eyeTransform;
		return new Ray(transform.position, transform.forward);
	}

	// Token: 0x060055BC RID: 21948 RVA: 0x001D8C01 File Offset: 0x001D7001
	public override Vector3 GetLocalCameraPos()
	{
		return base.transform.parent.InverseTransformPoint(this.eyeTransform.position);
	}

	// Token: 0x060055BD RID: 21949 RVA: 0x001D8C1E File Offset: 0x001D701E
	public override Vector3 GetWorldCameraPos()
	{
		return this.eyeTransform.position;
	}

	// Token: 0x060055BE RID: 21950 RVA: 0x001D8C2B File Offset: 0x001D702B
	public override Quaternion GetLocalCameraRot()
	{
		return Quaternion.Inverse(base.transform.parent.rotation) * this.eyeTransform.rotation;
	}

	// Token: 0x060055BF RID: 21951 RVA: 0x001D8C52 File Offset: 0x001D7052
	public override Quaternion GetWorldCameraRot()
	{
		return this.eyeTransform.rotation;
	}

	// Token: 0x060055C0 RID: 21952 RVA: 0x001D8C5F File Offset: 0x001D705F
	public override Vector3 GetLocalCameraNeckPos()
	{
		return base.transform.parent.InverseTransformPoint(this.eyeTransform.position) - this.GetLocalCameraRot() * this.neckToEyeOffset;
	}

	// Token: 0x060055C1 RID: 21953 RVA: 0x001D8C92 File Offset: 0x001D7092
	public override Vector3 GetWorldCameraNeckPos()
	{
		return base.transform.parent.TransformPoint(this.GetLocalCameraNeckPos());
	}

	// Token: 0x060055C2 RID: 21954 RVA: 0x001D8CAA File Offset: 0x001D70AA
	public override void SetMode(VRCVrCamera.CameraMode mode)
	{
	}

	// Token: 0x060055C3 RID: 21955 RVA: 0x001D8CAC File Offset: 0x001D70AC
	public override void InitializeCameras(CameraSelector selector)
	{
		this.eyeCamera.gameObject.SetActive(false);
		this.eyeCamera.cullingMask = selector.cameraLayers;
		this.eyeCamera.farClipPlane = selector.DefaultDrawDistance;
		this.eyeCamera.clearFlags = selector.clearFlags;
		this.eyeCamera.depth = (float)selector.depth;
		this.eyeCamera.backgroundColor = selector.clearColor;
		this.eyeCamera.renderingPath = selector.renderingPath;
		this.postCamera.depth = (float)(selector.depth + 1);
		this.screenCamera = this.eyeCamera;
	}

	// Token: 0x060055C4 RID: 21956 RVA: 0x001D8D56 File Offset: 0x001D7156
	public override void SetNeckToEyeOffset(Vector3 avatarEyeToNeckVector)
	{
		this.neckToEyeOffset = avatarEyeToNeckVector / VRCTrackingManager.GetTrackingScale();
	}

	// Token: 0x060055C5 RID: 21957 RVA: 0x001D8D69 File Offset: 0x001D7169
	private void Update()
	{
		if (!this.eyeCamera.gameObject.activeSelf)
		{
			this.eyeCamera.gameObject.SetActive(true);
		}
		base.UpdateSitMode();
	}

	// Token: 0x060055C6 RID: 21958 RVA: 0x001D8D98 File Offset: 0x001D7198
	public override Ray ScreenPointToRay(Vector3 point)
	{
		int width = Screen.width;
		int height = Screen.height;
		if (this.screenCamera.targetTexture != null)
		{
			width = this.screenCamera.targetTexture.width;
			height = this.screenCamera.targetTexture.height;
		}
		point.x = point.x * (float)width / (float)Screen.width;
		point.y = point.y * (float)height / (float)Screen.height;
		return this.screenCamera.ScreenPointToRay(point);
	}

	// Token: 0x060055C7 RID: 21959 RVA: 0x001D8E27 File Offset: 0x001D7227
	public override void SetBlind(bool blind)
	{
		this.eyeCamera.enabled = !blind;
	}

	// Token: 0x04003C7D RID: 15485
	public Transform cameraLiftTransform;

	// Token: 0x04003C7E RID: 15486
	public SteamVR_Render steamVrRenderer;

	// Token: 0x04003C7F RID: 15487
	private SteamVR_GameView gameView;

	// Token: 0x04003C80 RID: 15488
	private Transform neckTransform;

	// Token: 0x04003C81 RID: 15489
	private Transform eyeTransform;

	// Token: 0x04003C82 RID: 15490
	private Camera eyeCamera;

	// Token: 0x04003C83 RID: 15491
	private Camera postCamera;

	// Token: 0x04003C84 RID: 15492
	private NeckMouseRotator mouseGaze;

	// Token: 0x04003C85 RID: 15493
	private Vector3 neckToEyeOffset;

	// Token: 0x04003C86 RID: 15494
	private bool _vrDetected;

	// Token: 0x04003C87 RID: 15495
	private bool _seatedSpace;

	// Token: 0x04003C88 RID: 15496
	private bool _seatedPlay;

	// Token: 0x04003C89 RID: 15497
	private bool _seatedPlayCalibrated;
}
