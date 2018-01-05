using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class VRMouseLook : MonoBehaviour
{
	// Token: 0x0600277D RID: 10109 RVA: 0x000CC5E4 File Offset: 0x000CA9E4
	private void Awake()
	{
		Camera componentInChildren = base.gameObject.GetComponentInChildren<Camera>();
		this.vrCameraTransform = componentInChildren.transform;
		this.rotationTransform = new GameObject("VR Mouse Look (Rotation)").GetComponent<Transform>();
		this.forwardTransform = new GameObject("VR Mouse Look (Forward)").GetComponent<Transform>();
		this.rotationTransform.SetParent(base.transform.parent, false);
		this.forwardTransform.SetParent(this.rotationTransform, false);
		base.transform.SetParent(this.forwardTransform, false);
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x000CC670 File Offset: 0x000CAA70
	private void Update()
	{
		bool flag = false;
		bool flag2 = false;
		if (Input.GetMouseButton(0))
		{
			flag2 = true;
			if (this.enableYaw)
			{
				this.mousePosition.x = this.mousePosition.x + Input.GetAxis("Mouse X") * 5f;
				if (this.mousePosition.x <= -180f)
				{
					this.mousePosition.x = this.mousePosition.x + 360f;
				}
				else if (this.mousePosition.x > 180f)
				{
					this.mousePosition.x = this.mousePosition.x - 360f;
				}
			}
			this.mousePosition.y = this.mousePosition.y - Input.GetAxis("Mouse Y") * 2.4f;
			this.mousePosition.y = Mathf.Clamp(this.mousePosition.y, -85f, 85f);
		}
		else if (Input.GetMouseButton(1))
		{
			flag = true;
			this.mousePosition.z = this.mousePosition.z + Input.GetAxis("Mouse X") * 5f;
			this.mousePosition.z = Mathf.Clamp(this.mousePosition.z, -85f, 85f);
		}
		if (!flag && this.autoRecenterRoll)
		{
			this.mousePosition.z = Mathf.Lerp(this.mousePosition.z, 0f, Time.deltaTime / (Time.deltaTime + 0.1f));
		}
		if (!flag2 && this.autoRecenterPitch)
		{
			this.mousePosition.y = Mathf.Lerp(this.mousePosition.y, 0f, Time.deltaTime / (Time.deltaTime + 0.1f));
		}
		this.forwardTransform.localRotation = Quaternion.Inverse(Quaternion.Euler(0f, this.vrCameraTransform.localRotation.eulerAngles.y, 0f));
		this.rotationTransform.localRotation = Quaternion.Euler(0f, this.vrCameraTransform.localRotation.eulerAngles.y, 0f) * Quaternion.Euler(this.mousePosition.y, this.mousePosition.x, this.mousePosition.z);
	}

	// Token: 0x04001561 RID: 5473
	[SerializeField]
	private bool enableYaw = true;

	// Token: 0x04001562 RID: 5474
	[SerializeField]
	private bool autoRecenterPitch = true;

	// Token: 0x04001563 RID: 5475
	[SerializeField]
	private bool autoRecenterRoll = true;

	// Token: 0x04001564 RID: 5476
	private Transform vrCameraTransform;

	// Token: 0x04001565 RID: 5477
	private Transform rotationTransform;

	// Token: 0x04001566 RID: 5478
	private Transform forwardTransform;

	// Token: 0x04001567 RID: 5479
	private Vector3 mousePosition = Vector3.zero;
}
