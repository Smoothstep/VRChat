using System;
using UnityEngine;

// Token: 0x02000AAA RID: 2730
public class NeckMouseRotator : MonoBehaviour
{
	// Token: 0x06005261 RID: 21089 RVA: 0x001C4704 File Offset: 0x001C2B04
	private void Start()
	{
		this.inLookHorizontal = VRCInputManager.FindInput("LookHorizontal");
		this.inLookVertical = VRCInputManager.FindInput("LookVertical");
		this.originalPosition = base.transform.localPosition;
		this.originalRotation = base.transform.localRotation;
		this.originalEulers = this.originalRotation.eulerAngles;
		if (this.originalEulers.y > 180f)
		{
			this.originalEulers.y = this.originalEulers.y - 360f;
		}
		if (this.originalEulers.x > 180f)
		{
			this.originalEulers.x = this.originalEulers.x - 360f;
		}
		if (this.originalEulers.y < -180f)
		{
			this.originalEulers.y = this.originalEulers.y + 360f;
		}
		if (this.originalEulers.x < -180f)
		{
			this.originalEulers.x = this.originalEulers.x + 360f;
		}
	}

	// Token: 0x06005262 RID: 21090 RVA: 0x001C4814 File Offset: 0x001C2C14
	private void FixedUpdate()
	{
		if (this.IsFirstPhysicsStepThisFrame)
		{
			this.UpdateInput();
		}
		this.IsFirstPhysicsStepThisFrame = false;
	}

	// Token: 0x06005263 RID: 21091 RVA: 0x001C482E File Offset: 0x001C2C2E
	private void Update()
	{
		this.IsFirstPhysicsStepThisFrame = true;
	}

	// Token: 0x06005264 RID: 21092 RVA: 0x001C4838 File Offset: 0x001C2C38
	private void UpdateInput()
	{
		if (VRCInputManager.comfortTurning)
		{
			return;
		}
		base.transform.localRotation = this.originalRotation;
		if (this.relative)
		{
			float num = CrossPlatformInput.GetAxis("Mouse X");
			float num2 = CrossPlatformInput.GetAxis("Mouse Y");
			num += this.inLookHorizontal.axis * this.controllerRotationSpeed;
			num2 -= this.inLookVertical.axis * this.controllerRotationSpeed;
			if (this.targetAngles.y > 180f)
			{
				this.targetAngles.y = this.targetAngles.y - 360f;
				this.followAngles.y = this.followAngles.y - 360f;
			}
			if (this.targetAngles.x > 180f)
			{
				this.targetAngles.x = this.targetAngles.x - 360f;
				this.followAngles.x = this.followAngles.x - 360f;
			}
			if (this.targetAngles.y < -180f)
			{
				this.targetAngles.y = this.targetAngles.y + 360f;
				this.followAngles.y = this.followAngles.y + 360f;
			}
			if (this.targetAngles.x < -180f)
			{
				this.targetAngles.x = this.targetAngles.x + 360f;
				this.followAngles.x = this.followAngles.x + 360f;
			}
			this.targetAngles.y = this.targetAngles.y + num * this.rotationSpeed;
			this.targetAngles.x = this.targetAngles.x + num2 * this.rotationSpeed;
			this.targetAngles.y = Mathf.Clamp(this.targetAngles.y, -this.rotationRange.y * 0.5f, this.rotationRange.y * 0.5f);
			this.targetAngles.x = Mathf.Clamp(this.targetAngles.x, this.rotationRange.x, this.rotationRange.z);
		}
		else
		{
			float num = Input.mousePosition.x;
			float num2 = Input.mousePosition.y;
			this.targetAngles.y = Mathf.Lerp(-this.rotationRange.y * 0.5f, this.rotationRange.y * 0.5f, num / (float)Screen.width);
			this.targetAngles.x = Mathf.Lerp(this.rotationRange.x, this.rotationRange.z, num2 / (float)Screen.height);
		}
		this.followAngles = Vector3.SmoothDamp(this.followAngles, this.targetAngles, ref this.followVelocity, this.dampingTime);
		base.transform.localRotation = this.originalRotation * Quaternion.Euler(-this.followAngles.x, this.followAngles.y, 0f);
		float num3 = Mathf.Clamp(this.followAngles.x - this.originalEulers.x, -60f, 60f);
		Vector3 localPosition = this.originalPosition;
		if (num3 < 0f)
		{
			localPosition.y += this.neckLookDownDist * num3 / 60f;
		}
		if (num3 > 0f)
		{
			localPosition.y -= this.neckLookUpDist * num3 / 60f;
		}
		base.transform.localPosition = localPosition;
	}

	// Token: 0x04003A3D RID: 14909
	public float neckLookUpDist;

	// Token: 0x04003A3E RID: 14910
	public float neckLookDownDist;

	// Token: 0x04003A3F RID: 14911
	public Vector3 rotationRange = new Vector3(-70f, 70f, 70f);

	// Token: 0x04003A40 RID: 14912
	public float rotationSpeed = 10f;

	// Token: 0x04003A41 RID: 14913
	public float dampingTime = 0.2f;

	// Token: 0x04003A42 RID: 14914
	public bool autoZeroVerticalOnMobile = true;

	// Token: 0x04003A43 RID: 14915
	public bool autoZeroHorizontalOnMobile;

	// Token: 0x04003A44 RID: 14916
	public bool relative = true;

	// Token: 0x04003A45 RID: 14917
	private float controllerRotationSpeed = 0.1f;

	// Token: 0x04003A46 RID: 14918
	private Vector3 targetAngles;

	// Token: 0x04003A47 RID: 14919
	private Vector3 followAngles;

	// Token: 0x04003A48 RID: 14920
	private Vector3 followVelocity;

	// Token: 0x04003A49 RID: 14921
	private Vector3 originalPosition;

	// Token: 0x04003A4A RID: 14922
	private Quaternion originalRotation;

	// Token: 0x04003A4B RID: 14923
	private Vector3 originalEulers;

	// Token: 0x04003A4C RID: 14924
	private VRCInput inLookHorizontal;

	// Token: 0x04003A4D RID: 14925
	private VRCInput inLookVertical;

	// Token: 0x04003A4E RID: 14926
	private bool IsFirstPhysicsStepThisFrame = true;
}
