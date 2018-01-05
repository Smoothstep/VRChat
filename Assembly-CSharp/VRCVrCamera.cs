using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B0A RID: 2826
public class VRCVrCamera : MonoBehaviour
{
	// Token: 0x0600558E RID: 21902 RVA: 0x001D8459 File Offset: 0x001D6859
	public static VRCVrCamera GetInstance()
	{
		return VRCVrCamera.instance;
	}

	// Token: 0x0600558F RID: 21903 RVA: 0x001D8460 File Offset: 0x001D6860
	public virtual void Awake()
	{
		if (VRCVrCamera.instance != null)
		{
			Debug.LogError("Two VRCVrCamera objects created");
		}
		VRCVrCamera.instance = this;
	}

	// Token: 0x06005590 RID: 21904 RVA: 0x001D8482 File Offset: 0x001D6882
	public virtual bool IsTrackingPosition()
	{
		return false;
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x001D8485 File Offset: 0x001D6885
	public virtual Ray GetWorldLookRay()
	{
		return new Ray(base.transform.position, base.transform.forward);
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x001D84A2 File Offset: 0x001D68A2
	public virtual Vector3 GetLocalCameraPos()
	{
		return Vector3.zero;
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x001D84A9 File Offset: 0x001D68A9
	public virtual Vector3 GetLocalCameraNeckPos()
	{
		return Vector3.zero;
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x001D84B0 File Offset: 0x001D68B0
	public virtual Quaternion GetLocalCameraRot()
	{
		return Quaternion.identity;
	}

	// Token: 0x06005595 RID: 21909 RVA: 0x001D84B7 File Offset: 0x001D68B7
	public virtual Vector3 GetWorldCameraPos()
	{
		return base.transform.parent.TransformPoint(this.GetLocalCameraPos());
	}

	// Token: 0x06005596 RID: 21910 RVA: 0x001D84CF File Offset: 0x001D68CF
	public virtual Vector3 GetWorldCameraNeckPos()
	{
		return base.transform.parent.TransformPoint(this.GetLocalCameraNeckPos());
	}

	// Token: 0x06005597 RID: 21911 RVA: 0x001D84E7 File Offset: 0x001D68E7
	public virtual Quaternion GetWorldCameraRot()
	{
		return base.transform.parent.rotation * this.GetLocalCameraRot();
	}

	// Token: 0x06005598 RID: 21912 RVA: 0x001D8504 File Offset: 0x001D6904
	public virtual void SetMode(VRCVrCamera.CameraMode mode)
	{
	}

	// Token: 0x06005599 RID: 21913 RVA: 0x001D8506 File Offset: 0x001D6906
	public virtual bool GetSitMode()
	{
		return true;
	}

	// Token: 0x0600559A RID: 21914 RVA: 0x001D8509 File Offset: 0x001D6909
	public virtual bool SetSitMode(bool seated, bool calibrate = false)
	{
		return true;
	}

	// Token: 0x0600559B RID: 21915 RVA: 0x001D850C File Offset: 0x001D690C
	public virtual float GetLiftAmount()
	{
		return 0f;
	}

	// Token: 0x0600559C RID: 21916 RVA: 0x001D8513 File Offset: 0x001D6913
	public virtual void SetCalibrationMode()
	{
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x001D8515 File Offset: 0x001D6915
	public virtual void SetNeckToEyeOffset(Vector3 avatarEyeToNeckVector)
	{
	}

	// Token: 0x0600559E RID: 21918 RVA: 0x001D8518 File Offset: 0x001D6918
	public virtual void InitializeCameras(CameraSelector selector)
	{
		Camera[] componentsInChildren = base.GetComponentsInChildren<Camera>();
		foreach (Camera camera in componentsInChildren)
		{
			camera.cullingMask = selector.cameraLayers;
			camera.farClipPlane = selector.DefaultDrawDistance;
			camera.clearFlags = selector.clearFlags;
			camera.depth = (float)selector.depth;
			camera.backgroundColor = selector.clearColor;
			camera.renderingPath = selector.renderingPath;
		}
		this.screenCamera = componentsInChildren[0];
		this.SetStereoContentPresent();
	}

	// Token: 0x0600559F RID: 21919 RVA: 0x001D85A4 File Offset: 0x001D69A4
	public virtual Ray ScreenPointToRay(Vector3 point)
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

	// Token: 0x060055A0 RID: 21920 RVA: 0x001D8634 File Offset: 0x001D6A34
	public void SetExclusiveLights(Light[] lights)
	{
		this.exclusiveLights = lights;
		foreach (Light light in this.exclusiveLights)
		{
			light.enabled = false;
		}
	}

	// Token: 0x060055A1 RID: 21921 RVA: 0x001D8670 File Offset: 0x001D6A70
	private void OnPreCull()
	{
		foreach (Light light in this.exclusiveLights)
		{
			light.enabled = true;
		}
	}

	// Token: 0x060055A2 RID: 21922 RVA: 0x001D86A4 File Offset: 0x001D6AA4
	private void OnPostRender()
	{
		foreach (Light light in this.exclusiveLights)
		{
			light.enabled = false;
		}
	}

	// Token: 0x060055A3 RID: 21923 RVA: 0x001D86D7 File Offset: 0x001D6AD7
	public virtual void SetStereoContentPresent()
	{
		Debug.LogError("Camera must override stereo mode for stereo render support.");
	}

	// Token: 0x060055A4 RID: 21924 RVA: 0x001D86E3 File Offset: 0x001D6AE3
	public virtual void SetBlind(bool blind)
	{
		Debug.LogError("Camera must override setblind.");
	}

	// Token: 0x060055A5 RID: 21925 RVA: 0x001D86EF File Offset: 0x001D6AEF
	public virtual bool ShouldDefaultToSeatedPlayMode()
	{
		return false;
	}

	// Token: 0x060055A6 RID: 21926 RVA: 0x001D86F4 File Offset: 0x001D6AF4
	protected IEnumerator RegisterOnEnteredWorldCallback()
	{
		yield return new WaitUntil(() => VRCFlowManager.Instance != null);
		VRCFlowManager.Instance.onEnteredWorld += this.InitializedSeatedPlayMode;
		yield break;
	}

	// Token: 0x060055A7 RID: 21927 RVA: 0x001D870F File Offset: 0x001D6B0F
	protected void UnregisterOnEnteredWorldCallback()
	{
		if (VRCFlowManager.Instance != null)
		{
			VRCFlowManager.Instance.onEnteredWorld -= this.InitializedSeatedPlayMode;
		}
	}

	// Token: 0x060055A8 RID: 21928 RVA: 0x001D8737 File Offset: 0x001D6B37
	private void InitializedSeatedPlayMode()
	{
		if (this._seatedPlayModeInitialized)
		{
			return;
		}
		this._seatedPlayModeInitialized = true;
		if (this.ShouldDefaultToSeatedPlayMode())
		{
			this.SetSitMode(true, false);
		}
	}

	// Token: 0x060055A9 RID: 21929 RVA: 0x001D8760 File Offset: 0x001D6B60
	protected void UpdateSitMode()
	{
		if (this.GetSitMode() && HMDManager.IsHmdDetected())
		{
			float num = VRCTrackingManager.GetAvatarEyeHeight() / VRCTrackingManager.GetTrackingScale();
			float y = this.GetLocalCameraPos().y;
			if (y - num > 0.3048f)
			{
				if (this._playerStandUpStartTime < 0f)
				{
					this._playerStandUpStartTime = Time.realtimeSinceStartup;
				}
				if (Time.realtimeSinceStartup - this._playerStandUpStartTime > 2f)
				{
					VRCTrackingManager.SetSeatedPlayMode(false);
					QuickMenu.Instance.RefreshSitButton();
					this._playerStandUpStartTime = -1f;
				}
			}
			else if (this._playerStandUpStartTime >= 0f)
			{
				this._playerStandUpStartTime = -1f;
			}
		}
		else if (this._playerStandUpStartTime >= 0f)
		{
			this._playerStandUpStartTime = -1f;
		}
	}

	// Token: 0x04003C72 RID: 15474
	private static VRCVrCamera instance;

	// Token: 0x04003C73 RID: 15475
	public static bool IsStereoRequired;

	// Token: 0x04003C74 RID: 15476
	public Camera screenCamera;

	// Token: 0x04003C75 RID: 15477
	private Light[] exclusiveLights;

	// Token: 0x04003C76 RID: 15478
	private float _playerStandUpStartTime = -1f;

	// Token: 0x04003C77 RID: 15479
	private const float STAND_UP_TIME_THRESHOLD = 2f;

	// Token: 0x04003C78 RID: 15480
	private const float STAND_UP_DISTANCE_THRESHOLD = 0.3048f;

	// Token: 0x04003C79 RID: 15481
	private bool _seatedPlayModeInitialized;

	// Token: 0x02000B0B RID: 2827
	public enum CameraMode
	{
		// Token: 0x04003C7B RID: 15483
		Avatar,
		// Token: 0x04003C7C RID: 15484
		Ui
	}
}
