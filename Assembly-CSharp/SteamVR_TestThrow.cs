using System;
using UnityEngine;

// Token: 0x02000B8E RID: 2958
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SteamVR_TestThrow : MonoBehaviour
{
	// Token: 0x06005C00 RID: 23552 RVA: 0x00201FDA File Offset: 0x002003DA
	private void Awake()
	{
		this.trackedObj = base.GetComponent<SteamVR_TrackedObject>();
	}

	// Token: 0x06005C01 RID: 23553 RVA: 0x00201FE8 File Offset: 0x002003E8
	private void FixedUpdate()
	{
		SteamVR_Controller.Device device = SteamVR_Controller.Input((int)this.trackedObj.index);
		if (this.joint == null && device.GetTouchDown(8589934592UL))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			gameObject.transform.position = this.attachPoint.transform.position;
			this.joint = gameObject.AddComponent<FixedJoint>();
			this.joint.connectedBody = this.attachPoint;
		}
		else if (this.joint != null && device.GetTouchUp(8589934592UL))
		{
			GameObject gameObject2 = this.joint.gameObject;
			Rigidbody component = gameObject2.GetComponent<Rigidbody>();
			UnityEngine.Object.DestroyImmediate(this.joint);
			this.joint = null;
			UnityEngine.Object.Destroy(gameObject2, 15f);
			Transform transform = (!this.trackedObj.origin) ? this.trackedObj.transform.parent : this.trackedObj.origin;
			if (transform != null)
			{
				component.velocity = transform.TransformVector(device.velocity);
				component.angularVelocity = transform.TransformVector(device.angularVelocity);
			}
			else
			{
				component.velocity = device.velocity;
				component.angularVelocity = device.angularVelocity;
			}
			component.maxAngularVelocity = component.angularVelocity.magnitude;
		}
	}

	// Token: 0x04004195 RID: 16789
	public GameObject prefab;

	// Token: 0x04004196 RID: 16790
	public Rigidbody attachPoint;

	// Token: 0x04004197 RID: 16791
	private SteamVR_TrackedObject trackedObj;

	// Token: 0x04004198 RID: 16792
	private FixedJoint joint;
}
