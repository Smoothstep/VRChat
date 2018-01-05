using System;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR;

// Token: 0x0200046A RID: 1130
public class CameraControl : MonoBehaviour
{
	// Token: 0x0600275E RID: 10078 RVA: 0x000CAF2D File Offset: 0x000C932D
	private void Awake()
	{
		this.Initialization();
	}

	// Token: 0x0600275F RID: 10079 RVA: 0x000CAF38 File Offset: 0x000C9338
	public void Initialization()
	{
		this.defaultCamera.SetActive(false);
		this.oculusCameraRig.SetActive(false);
		this.steamVRCameraRig.SetActive(false);
		if ("Oculus".Equals(VRSettings.loadedDeviceName))
		{
			this.oculusCameraRig.SetActive(true);
			InputTracking.Recenter();
		}
		else if ("OpenVR".Equals(VRSettings.loadedDeviceName))
		{
			this.steamVRCameraRig.SetActive(true);
			OpenVR.System.ResetSeatedZeroPose();
		}
		else
		{
			this.defaultCamera.SetActive(true);
		}
	}

	// Token: 0x0400151D RID: 5405
	private const string OCULUS_DEVICE_NAME = "Oculus";

	// Token: 0x0400151E RID: 5406
	private const string STEAMVR_DEVICE_NAME = "OpenVR";

	// Token: 0x0400151F RID: 5407
	[SerializeField]
	private GameObject defaultCamera;

	// Token: 0x04001520 RID: 5408
	[SerializeField]
	private GameObject oculusCameraRig;

	// Token: 0x04001521 RID: 5409
	[SerializeField]
	private GameObject steamVRCameraRig;
}
