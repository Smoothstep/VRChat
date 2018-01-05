using System;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x02000B1D RID: 2845
public class VRCOVRTracker : MonoBehaviour
{
	// Token: 0x06005677 RID: 22135 RVA: 0x001DC647 File Offset: 0x001DAA47
	private void Start()
	{
		this.Update();
	}

	// Token: 0x06005678 RID: 22136 RVA: 0x001DC650 File Offset: 0x001DAA50
	private void Update()
	{
		if (OVRManager.instance != null)
		{
			OVRManager.instance.enabled = (VRSettings.enabled && this.IsInVRMode());
		}
		if (this.TrackerAnchor != null)
		{
			if (OVRManager.instance != null && OVRManager.instance.enabled)
			{
				if (!this.TrackerAnchor.gameObject.activeSelf)
				{
					this.TrackerAnchor.gameObject.SetActive(true);
				}
				OVRPose pose = OVRManager.tracker.GetPose(0);
				this.TrackerAnchor.localPosition = pose.position;
				this.TrackerAnchor.localRotation = pose.orientation;
			}
			else if (this.TrackerAnchor.gameObject.activeSelf)
			{
				this.TrackerAnchor.gameObject.SetActive(false);
			}
		}
		bool flag = OVRManager.instance != null && OVRManager.instance.enabled && VRCInputManager.IsPresent(VRCInputManager.InputMethod.Oculus);
		OVRInput.Controller controller = OVRInput.GetConnectedControllers() & OVRInput.GetActiveController();
		bool flag2 = VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Oculus;
		if (flag2)
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
		if (this.LeftHandAnchor != null)
		{
			if (flag && (controller & OVRInput.Controller.LTouch) != OVRInput.Controller.None && !this._areControllersAsleep)
			{
				if (!this.LeftHandAnchor.gameObject.activeSelf)
				{
					this.LeftHandAnchor.gameObject.SetActive(true);
				}
				this.LeftHandAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
				this.LeftHandAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
			}
			else if (this.LeftHandAnchor.gameObject.activeSelf)
			{
				this.LeftHandAnchor.gameObject.SetActive(false);
			}
		}
		if (this.RightHandAnchor != null)
		{
			if (flag && (controller & OVRInput.Controller.RTouch) != OVRInput.Controller.None && !this._areControllersAsleep)
			{
				if (!this.RightHandAnchor.gameObject.activeSelf)
				{
					this.RightHandAnchor.gameObject.SetActive(true);
				}
				this.RightHandAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
				this.RightHandAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
			}
			else if (this.RightHandAnchor.gameObject.activeSelf)
			{
				this.RightHandAnchor.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06005679 RID: 22137 RVA: 0x001DC916 File Offset: 0x001DAD16
	private bool IsInVRMode()
	{
		return HMDManager.IsHmdDetected();
	}

	// Token: 0x04003D61 RID: 15713
	public Transform LeftHandAnchor;

	// Token: 0x04003D62 RID: 15714
	public Transform RightHandAnchor;

	// Token: 0x04003D63 RID: 15715
	public Transform TrackerAnchor;

	// Token: 0x04003D64 RID: 15716
	private bool _usingHandControllers;

	// Token: 0x04003D65 RID: 15717
	private bool _areControllersAsleep;

	// Token: 0x04003D66 RID: 15718
	private float _controllerSleepTimeoutStart;

	// Token: 0x04003D67 RID: 15719
	private const float CONTROLLER_SLEEP_TIMEOUT = 1f;
}
