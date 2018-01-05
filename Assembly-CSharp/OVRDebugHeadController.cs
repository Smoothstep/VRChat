using System;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x02000672 RID: 1650
public class OVRDebugHeadController : MonoBehaviour
{
	// Token: 0x060037DB RID: 14299 RVA: 0x0011CBA0 File Offset: 0x0011AFA0
	private void Awake()
	{
		OVRCameraRig[] componentsInChildren = base.gameObject.GetComponentsInChildren<OVRCameraRig>();
		if (componentsInChildren.Length == 0)
		{
			Debug.LogWarning("OVRCamParent: No OVRCameraRig attached.");
		}
		else if (componentsInChildren.Length > 1)
		{
			Debug.LogWarning("OVRCamParent: More then 1 OVRCameraRig attached.");
		}
		else
		{
			this.CameraRig = componentsInChildren[0];
		}
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x0011CBF1 File Offset: 0x0011AFF1
	private void Start()
	{
	}

	// Token: 0x060037DD RID: 14301 RVA: 0x0011CBF4 File Offset: 0x0011AFF4
	private void Update()
	{
		if (this.AllowMovement)
		{
			float y = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.Active).y;
			float x = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.Active).x;
			Vector3 a = this.CameraRig.centerEyeAnchor.rotation * Vector3.forward * y * Time.deltaTime * this.ForwardSpeed;
			Vector3 b = this.CameraRig.centerEyeAnchor.rotation * Vector3.right * x * Time.deltaTime * this.StrafeSpeed;
			base.transform.position += a + b;
		}
		if (!VRDevice.isPresent && (this.AllowYawLook || this.AllowPitchLook))
		{
			Quaternion quaternion = base.transform.rotation;
			if (this.AllowYawLook)
			{
				float x2 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.Active).x;
				float angle = x2 * Time.deltaTime * this.GamePad_YawDegreesPerSec;
				Quaternion lhs = Quaternion.AngleAxis(angle, Vector3.up);
				quaternion = lhs * quaternion;
			}
			if (this.AllowPitchLook)
			{
				float num = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.Active).y;
				if (Mathf.Abs(num) > 0.0001f)
				{
					if (this.InvertPitch)
					{
						num *= -1f;
					}
					float angle2 = num * Time.deltaTime * this.GamePad_PitchDegreesPerSec;
					Quaternion rhs = Quaternion.AngleAxis(angle2, Vector3.left);
					quaternion *= rhs;
				}
			}
			base.transform.rotation = quaternion;
		}
	}

	// Token: 0x04002059 RID: 8281
	[SerializeField]
	public bool AllowPitchLook;

	// Token: 0x0400205A RID: 8282
	[SerializeField]
	public bool AllowYawLook = true;

	// Token: 0x0400205B RID: 8283
	[SerializeField]
	public bool InvertPitch;

	// Token: 0x0400205C RID: 8284
	[SerializeField]
	public float GamePad_PitchDegreesPerSec = 90f;

	// Token: 0x0400205D RID: 8285
	[SerializeField]
	public float GamePad_YawDegreesPerSec = 90f;

	// Token: 0x0400205E RID: 8286
	[SerializeField]
	public bool AllowMovement;

	// Token: 0x0400205F RID: 8287
	[SerializeField]
	public float ForwardSpeed = 2f;

	// Token: 0x04002060 RID: 8288
	[SerializeField]
	public float StrafeSpeed = 2f;

	// Token: 0x04002061 RID: 8289
	protected OVRCameraRig CameraRig;
}
