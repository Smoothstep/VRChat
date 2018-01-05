using System;
using UnityEngine;
using VRC.Core;

// Token: 0x02000A5B RID: 2651
public class CameraSelector : MonoBehaviour
{
	// Token: 0x0600503D RID: 20541 RVA: 0x001B7914 File Offset: 0x001B5D14
	private void Awake()
	{
		VRCPlayer componentInParent = base.GetComponentInParent<VRCPlayer>();
		if (componentInParent != null && VRCPlayer.Instance != componentInParent)
		{
			base.enabled = false;
			return;
		}
		this.SwitchHMD();
	}

	// Token: 0x0600503E RID: 20542 RVA: 0x001B7952 File Offset: 0x001B5D52
	private void Update()
	{
		if (this.current != HMDManager.GetHmdType())
		{
			this.SwitchHMD();
		}
	}

	// Token: 0x0600503F RID: 20543 RVA: 0x001B796C File Offset: 0x001B5D6C
	public void SwitchHMD()
	{
		if (this.cameraInstance != null)
		{
			UnityEngine.Object.Destroy(this.cameraInstance);
		}
		this.current = HMDManager.GetHmdType();
		this.cameraInstance = HMDManager.InstantiateCameraPrefab();
		if (this.cameraInstance != null)
		{
			this.cameraInstance.transform.parent = base.transform;
			this.cameraInstance.transform.localPosition = Vector3.zero;
			this.cameraInstance.transform.localRotation = Quaternion.identity;
			this.cameraInstance.transform.localScale = Vector3.one;
			this.vrCamera = this.cameraInstance.GetComponentInChildren<VRCVrCamera>();
			this.vrCamera.SetMode(this.vrCameraMode);
			this.vrCamera.InitializeCameras(this);
			this.vrCamera.SetExclusiveLights(this.exclusiveLights);
			if (this.disableAudio)
			{
				AudioListener componentInChildren = this.cameraInstance.GetComponentInChildren<AudioListener>();
				componentInChildren.enabled = false;
			}
			if (this.hudPrefab != null)
			{
				GameObject gameObject = AssetManagement.Instantiate(this.hudPrefab) as GameObject;
				gameObject.transform.parent = this.cameraInstance.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
			}
		}
	}

	// Token: 0x04003907 RID: 14599
	private GameObject cameraInstance;

	// Token: 0x04003908 RID: 14600
	public VRCVrCamera vrCamera;

	// Token: 0x04003909 RID: 14601
	public VRCVrCamera.CameraMode vrCameraMode;

	// Token: 0x0400390A RID: 14602
	public LayerMask cameraLayers = -1;

	// Token: 0x0400390B RID: 14603
	public GameObject hudPrefab;

	// Token: 0x0400390C RID: 14604
	public float DefaultDrawDistance = 1000f;

	// Token: 0x0400390D RID: 14605
	public CameraClearFlags clearFlags = CameraClearFlags.Skybox;

	// Token: 0x0400390E RID: 14606
	public Color clearColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0400390F RID: 14607
	public int depth = 1;

	// Token: 0x04003910 RID: 14608
	public RenderingPath renderingPath = RenderingPath.UsePlayerSettings;

	// Token: 0x04003911 RID: 14609
	public bool disableAudio;

	// Token: 0x04003912 RID: 14610
	public Light[] exclusiveLights;

	// Token: 0x04003913 RID: 14611
	private HMDManager.HMDType current = HMDManager.HMDType.Uninitialized;
}
