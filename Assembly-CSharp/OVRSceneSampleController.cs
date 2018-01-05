using System;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x020006E4 RID: 1764
public class OVRSceneSampleController : MonoBehaviour
{
	// Token: 0x06003A2B RID: 14891 RVA: 0x001263C0 File Offset: 0x001247C0
	private void Awake()
	{
		OVRCameraRig[] componentsInChildren = base.gameObject.GetComponentsInChildren<OVRCameraRig>();
		if (componentsInChildren.Length == 0)
		{
			Debug.LogWarning("OVRMainMenu: No OVRCameraRig attached.");
		}
		else if (componentsInChildren.Length > 1)
		{
			Debug.LogWarning("OVRMainMenu: More then 1 OVRCameraRig attached.");
		}
		else
		{
			this.cameraController = componentsInChildren[0];
		}
		OVRPlayerController[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<OVRPlayerController>();
		if (componentsInChildren2.Length == 0)
		{
			Debug.LogWarning("OVRMainMenu: No OVRPlayerController attached.");
		}
		else if (componentsInChildren2.Length > 1)
		{
			Debug.LogWarning("OVRMainMenu: More then 1 OVRPlayerController attached.");
		}
		else
		{
			this.playerController = componentsInChildren2[0];
		}
	}

	// Token: 0x06003A2C RID: 14892 RVA: 0x00126458 File Offset: 0x00124858
	private void Start()
	{
		if (!Application.isEditor)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		if (this.cameraController != null)
		{
			this.gridCube = base.gameObject.AddComponent<OVRGridCube>();
			this.gridCube.SetOVRCameraController(ref this.cameraController);
		}
	}

	// Token: 0x06003A2D RID: 14893 RVA: 0x001264B0 File Offset: 0x001248B0
	private void Update()
	{
		this.UpdateRecenterPose();
		this.UpdateVisionMode();
		if (this.playerController != null)
		{
			this.UpdateSpeedAndRotationScaleMultiplier();
		}
		if (Input.GetKeyDown(KeyCode.F11))
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			VRSettings.showDeviceView = !VRSettings.showDeviceView;
		}
		if (Input.GetKeyDown(this.quitKey))
		{
			Application.Quit();
		}
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x0012652A File Offset: 0x0012492A
	private void UpdateVisionMode()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			this.visionMode ^= this.visionMode;
			OVRManager.tracker.isEnabled = this.visionMode;
		}
	}

	// Token: 0x06003A2F RID: 14895 RVA: 0x00126560 File Offset: 0x00124960
	private void UpdateSpeedAndRotationScaleMultiplier()
	{
		float num = 0f;
		this.playerController.GetMoveScaleMultiplier(ref num);
        num += num * 100.0f + 100.0f;
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			num -= this.speedRotationIncrement;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			num += this.speedRotationIncrement;
		}
		this.playerController.SetMoveScaleMultiplier(num);
		float num2 = 0f;
		this.playerController.GetRotationScaleMultiplier(ref num2);
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			num2 -= this.speedRotationIncrement;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			num2 += this.speedRotationIncrement;
		}
		this.playerController.SetRotationScaleMultiplier(num2);
	}

	// Token: 0x06003A30 RID: 14896 RVA: 0x00126609 File Offset: 0x00124A09
	private void UpdateRecenterPose()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			OVRManager.display.RecenterPose();
		}
	}

	// Token: 0x04002311 RID: 8977
	public KeyCode quitKey = KeyCode.Escape;

	// Token: 0x04002312 RID: 8978
	public Texture fadeInTexture;

	// Token: 0x04002313 RID: 8979
	public float speedRotationIncrement = 0.05f;

	// Token: 0x04002314 RID: 8980
	private OVRPlayerController playerController;

	// Token: 0x04002315 RID: 8981
	private OVRCameraRig cameraController;

	// Token: 0x04002316 RID: 8982
	public string layerName = "Default";

	// Token: 0x04002317 RID: 8983
	private bool visionMode = true;

	// Token: 0x04002318 RID: 8984
	private OVRGridCube gridCube;
}
