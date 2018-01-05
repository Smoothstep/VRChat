using System;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class F3DTurret : MonoBehaviour
{
	// Token: 0x06002810 RID: 10256 RVA: 0x000D080F File Offset: 0x000CEC0F
	private Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
	{
		return vector - Vector3.Dot(vector, planeNormal) * planeNormal;
	}

	// Token: 0x06002811 RID: 10257 RVA: 0x000D0824 File Offset: 0x000CEC24
	private float SignedVectorAngle(Vector3 referenceVector, Vector3 otherVector, Vector3 normal)
	{
		Vector3 lhs = Vector3.Cross(normal, referenceVector);
		float num = Vector3.Angle(referenceVector, otherVector);
		return num * Mathf.Sign(Vector3.Dot(lhs, otherVector));
	}

	// Token: 0x06002812 RID: 10258 RVA: 0x000D0854 File Offset: 0x000CEC54
	private void Track()
	{
		if (this.hub != null && this.barrel != null)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out this.hitInfo, 500f))
			{
				Vector3 vector = this.ProjectVectorOnPlane(this.hub.up, this.hitInfo.point - this.hub.position);
				Quaternion b = Quaternion.LookRotation(vector);
				this.hubAngle = this.SignedVectorAngle(base.transform.forward, vector, Vector3.up);
				if (this.hubAngle <= -60f)
				{
					b = Quaternion.LookRotation(Quaternion.Euler(0f, -60f, 0f) * base.transform.forward);
				}
				else if (this.hubAngle >= 60f)
				{
					b = Quaternion.LookRotation(Quaternion.Euler(0f, 60f, 0f) * base.transform.forward);
				}
				this.hub.rotation = Quaternion.Slerp(this.hub.rotation, b, Time.deltaTime * 5f);
				Vector3 vector2 = this.ProjectVectorOnPlane(this.hub.right, this.hitInfo.point - this.barrel.position);
				Quaternion b2 = Quaternion.LookRotation(vector2);
				this.barrelAngle = this.SignedVectorAngle(this.hub.forward, vector2, this.hub.right);
				if (this.barrelAngle < -30f)
				{
					b2 = Quaternion.LookRotation(Quaternion.AngleAxis(-30f, this.hub.right) * this.hub.forward);
				}
				else if (this.barrelAngle > 15f)
				{
					b2 = Quaternion.LookRotation(Quaternion.AngleAxis(15f, this.hub.right) * this.hub.forward);
				}
				this.barrel.rotation = Quaternion.Slerp(this.barrel.rotation, b2, Time.deltaTime * 5f);
			}
		}
	}

	// Token: 0x06002813 RID: 10259 RVA: 0x000D0A98 File Offset: 0x000CEE98
	private void Update()
	{
		this.Track();
		if (!this.isFiring && Input.GetKeyDown(KeyCode.Mouse0))
		{
			this.isFiring = true;
			F3DFXController.instance.Fire();
		}
		if (this.isFiring && Input.GetKeyUp(KeyCode.Mouse0))
		{
			this.isFiring = false;
			F3DFXController.instance.Stop();
		}
	}

	// Token: 0x04001653 RID: 5715
	public Transform hub;

	// Token: 0x04001654 RID: 5716
	public Transform barrel;

	// Token: 0x04001655 RID: 5717
	private RaycastHit hitInfo;

	// Token: 0x04001656 RID: 5718
	private bool isFiring;

	// Token: 0x04001657 RID: 5719
	private float hubAngle;

	// Token: 0x04001658 RID: 5720
	private float barrelAngle;
}
