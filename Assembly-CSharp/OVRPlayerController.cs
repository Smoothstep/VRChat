using System;
using UnityEngine;

// Token: 0x020006E1 RID: 1761
[RequireComponent(typeof(CharacterController))]
public class OVRPlayerController : MonoBehaviour
{
	// Token: 0x06003A12 RID: 14866 RVA: 0x00125524 File Offset: 0x00123924
	private void Start()
	{
		Vector3 localPosition = this.CameraRig.transform.localPosition;
		localPosition.z = OVRManager.profile.eyeDepth;
		this.CameraRig.transform.localPosition = localPosition;
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x00125564 File Offset: 0x00123964
	private void Awake()
	{
		this.Controller = base.gameObject.GetComponent<CharacterController>();
		if (this.Controller == null)
		{
			Debug.LogWarning("OVRPlayerController: No CharacterController attached.");
		}
		OVRCameraRig[] componentsInChildren = base.gameObject.GetComponentsInChildren<OVRCameraRig>();
		if (componentsInChildren.Length == 0)
		{
			Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
		}
		else if (componentsInChildren.Length > 1)
		{
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
		}
		else
		{
			this.CameraRig = componentsInChildren[0];
		}
		this.InitialYRotation = base.transform.rotation.eulerAngles.y;
	}

	// Token: 0x06003A14 RID: 14868 RVA: 0x00125602 File Offset: 0x00123A02
	private void OnEnable()
	{
		OVRManager.display.RecenteredPose += this.ResetOrientation;
		if (this.CameraRig != null)
		{
			this.CameraRig.UpdatedAnchors += this.UpdateTransform;
		}
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x00125642 File Offset: 0x00123A42
	private void OnDisable()
	{
		OVRManager.display.RecenteredPose -= this.ResetOrientation;
		if (this.CameraRig != null)
		{
			this.CameraRig.UpdatedAnchors -= this.UpdateTransform;
		}
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x00125684 File Offset: 0x00123A84
	protected virtual void Update()
	{
		if (this.useProfileData)
		{
			OVRPose? initialPose = this.InitialPose;
			if (initialPose == null)
			{
				this.InitialPose = new OVRPose?(new OVRPose
				{
					position = this.CameraRig.transform.localPosition,
					orientation = this.CameraRig.transform.localRotation
				});
			}
			Vector3 localPosition = this.CameraRig.transform.localPosition;
			if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.EyeLevel)
			{
				localPosition.y = OVRManager.profile.eyeHeight - 0.5f * this.Controller.height + this.Controller.center.y;
			}
			else if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.FloorLevel)
			{
				localPosition.y = -(0.5f * this.Controller.height) + this.Controller.center.y;
			}
			this.CameraRig.transform.localPosition = localPosition;
		}
		else
		{
			OVRPose? initialPose2 = this.InitialPose;
			if (initialPose2 != null)
			{
				this.CameraRig.transform.localPosition = this.InitialPose.Value.position;
				this.CameraRig.transform.localRotation = this.InitialPose.Value.orientation;
				this.InitialPose = null;
			}
		}
		this.UpdateMovement();
		Vector3 vector = Vector3.zero;
		float num = 1f + this.Damping * this.SimulationRate * Time.deltaTime;
		this.MoveThrottle.x = this.MoveThrottle.x / num;
		this.MoveThrottle.y = ((this.MoveThrottle.y <= 0f) ? this.MoveThrottle.y : (this.MoveThrottle.y / num));
		this.MoveThrottle.z = this.MoveThrottle.z / num;
		vector += this.MoveThrottle * this.SimulationRate * Time.deltaTime;
		if (this.Controller.isGrounded && this.FallSpeed <= 0f)
		{
			this.FallSpeed = Physics.gravity.y * (this.GravityModifier * 0.002f);
		}
		else
		{
			this.FallSpeed += Physics.gravity.y * (this.GravityModifier * 0.002f) * this.SimulationRate * Time.deltaTime;
		}
		vector.y += this.FallSpeed * this.SimulationRate * Time.deltaTime;
		if (this.Controller.isGrounded && this.MoveThrottle.y <= base.transform.lossyScale.y * 0.001f)
		{
			float stepOffset = this.Controller.stepOffset;
			Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
			float d = Mathf.Max(stepOffset, vector2.magnitude);
			vector -= d * Vector3.up;
		}
		Vector3 vector3 = Vector3.Scale(this.Controller.transform.localPosition + vector, new Vector3(1f, 0f, 1f));
		this.Controller.Move(vector);
		Vector3 vector4 = Vector3.Scale(this.Controller.transform.localPosition, new Vector3(1f, 0f, 1f));
		if (vector3 != vector4)
		{
			this.MoveThrottle += (vector4 - vector3) / (this.SimulationRate * Time.deltaTime);
		}
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x00125A8C File Offset: 0x00123E8C
	public virtual void UpdateMovement()
	{
		if (this.HaltUpdateMovement)
		{
			return;
		}
		bool flag = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
		bool flag2 = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
		bool flag3 = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
		bool flag4 = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
		bool flag5 = false;
		if (OVRInput.Get(OVRInput.Button.DpadUp, OVRInput.Controller.Active))
		{
			flag = true;
			flag5 = true;
		}
		if (OVRInput.Get(OVRInput.Button.DpadDown, OVRInput.Controller.Active))
		{
			flag4 = true;
			flag5 = true;
		}
		this.MoveScale = 1f;
		if ((flag && flag2) || (flag && flag3) || (flag4 && flag2) || (flag4 && flag3))
		{
			this.MoveScale = 0.707106769f;
		}
		if (!this.Controller.isGrounded)
		{
			//this.MoveScale = 0f;
		}
		this.MoveScale *= this.SimulationRate * Time.deltaTime * 10.0f;
		float num = this.Acceleration * 0.1f * this.MoveScale * this.MoveScaleMultiplier;
		if (flag5 || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			num *= 2f;
		}
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		eulerAngles.z = (eulerAngles.x = 0f);
		Quaternion rotation = Quaternion.Euler(eulerAngles);
		if (flag)
		{
			this.MoveThrottle += rotation * (base.transform.lossyScale.z * num * Vector3.forward);
		}
		if (flag4)
		{
			this.MoveThrottle += rotation * (base.transform.lossyScale.z * num * this.BackAndSideDampen * Vector3.back);
		}
		if (flag2)
		{
			this.MoveThrottle += rotation * (base.transform.lossyScale.x * num * this.BackAndSideDampen * Vector3.left);
		}
		if (flag3)
		{
			this.MoveThrottle += rotation * (base.transform.lossyScale.x * num * this.BackAndSideDampen * Vector3.right);
		}
		Vector3 eulerAngles2 = base.transform.rotation.eulerAngles;
		bool flag6 = OVRInput.Get(OVRInput.Button.PrimaryShoulder, OVRInput.Controller.Active);
		if (flag6 && !this.prevHatLeft)
		{
			eulerAngles2.y -= this.RotationRatchet;
		}
		this.prevHatLeft = flag6;
		bool flag7 = OVRInput.Get(OVRInput.Button.SecondaryShoulder, OVRInput.Controller.Active);
		if (flag7 && !this.prevHatRight)
		{
			eulerAngles2.y += this.RotationRatchet;
		}
		this.prevHatRight = flag7;
		if (Input.GetKeyDown(KeyCode.Q))
		{
			eulerAngles2.y -= this.RotationRatchet;
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			eulerAngles2.y += this.RotationRatchet;
		}
		float num2 = this.SimulationRate * Time.deltaTime * this.RotationAmount * this.RotationScaleMultiplier;
		if (!this.SkipMouseRotation)
		{
			eulerAngles2.y += Input.GetAxis("Mouse X") * num2 * 3.25f;
		}
		num = this.Acceleration * 0.1f * this.MoveScale * this.MoveScaleMultiplier * 10.0f;
		num *= 1f + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Active);
		Vector2 vector = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active);
		if (vector.y > 0f)
		{
			this.MoveThrottle += rotation * (vector.y * base.transform.lossyScale.z * num * Vector3.forward);
		}
		if (vector.y < 0f)
		{
			this.MoveThrottle += rotation * (Mathf.Abs(vector.y) * base.transform.lossyScale.z * num * this.BackAndSideDampen * Vector3.back);
		}
		if (vector.x < 0f)
		{
			this.MoveThrottle += rotation * (Mathf.Abs(vector.x) * base.transform.lossyScale.x * num * this.BackAndSideDampen * Vector3.left);
		}
		if (vector.x > 0f)
		{
			this.MoveThrottle += rotation * (vector.x * base.transform.lossyScale.x * num * this.BackAndSideDampen * Vector3.right);
		}
		Vector2 vector2 = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Active);
		eulerAngles2.y += vector2.x * num2;
		base.transform.rotation = Quaternion.Euler(eulerAngles2);
	}

	// Token: 0x06003A18 RID: 14872 RVA: 0x00126044 File Offset: 0x00124444
	public void UpdateTransform(OVRCameraRig rig)
	{
		Transform trackingSpace = this.CameraRig.trackingSpace;
		Transform centerEyeAnchor = this.CameraRig.centerEyeAnchor;
		if (this.HmdRotatesY)
		{
			Vector3 position = trackingSpace.position;
			Quaternion rotation = trackingSpace.rotation;
			base.transform.rotation = Quaternion.Euler(0f, centerEyeAnchor.rotation.eulerAngles.y, 0f);
			trackingSpace.position = position;
			trackingSpace.rotation = rotation;
		}
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x001260C4 File Offset: 0x001244C4
	public bool Jump()
	{
		if (!this.Controller.isGrounded)
		{
			return false;
		}
		this.MoveThrottle += new Vector3(0f, base.transform.lossyScale.y * this.JumpForce, 0f);
		return true;
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x0012611E File Offset: 0x0012451E
	public void Stop()
	{
		this.Controller.Move(Vector3.zero);
		this.MoveThrottle = Vector3.zero;
		this.FallSpeed = 0f;
	}

	// Token: 0x06003A1B RID: 14875 RVA: 0x00126147 File Offset: 0x00124547
	public void GetMoveScaleMultiplier(ref float moveScaleMultiplier)
	{
		moveScaleMultiplier = this.MoveScaleMultiplier;
	}

	// Token: 0x06003A1C RID: 14876 RVA: 0x00126151 File Offset: 0x00124551
	public void SetMoveScaleMultiplier(float moveScaleMultiplier)
	{
		this.MoveScaleMultiplier = moveScaleMultiplier;
	}

	// Token: 0x06003A1D RID: 14877 RVA: 0x0012615A File Offset: 0x0012455A
	public void GetRotationScaleMultiplier(ref float rotationScaleMultiplier)
	{
		rotationScaleMultiplier = this.RotationScaleMultiplier;
	}

	// Token: 0x06003A1E RID: 14878 RVA: 0x00126164 File Offset: 0x00124564
	public void SetRotationScaleMultiplier(float rotationScaleMultiplier)
	{
		this.RotationScaleMultiplier = rotationScaleMultiplier;
	}

	// Token: 0x06003A1F RID: 14879 RVA: 0x0012616D File Offset: 0x0012456D
	public void GetSkipMouseRotation(ref bool skipMouseRotation)
	{
		skipMouseRotation = this.SkipMouseRotation;
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x00126177 File Offset: 0x00124577
	public void SetSkipMouseRotation(bool skipMouseRotation)
	{
		this.SkipMouseRotation = skipMouseRotation;
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x00126180 File Offset: 0x00124580
	public void GetHaltUpdateMovement(ref bool haltUpdateMovement)
	{
		haltUpdateMovement = this.HaltUpdateMovement;
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x0012618A File Offset: 0x0012458A
	public void SetHaltUpdateMovement(bool haltUpdateMovement)
	{
		this.HaltUpdateMovement = haltUpdateMovement;
	}

	// Token: 0x06003A23 RID: 14883 RVA: 0x00126194 File Offset: 0x00124594
	public void ResetOrientation()
	{
		if (this.HmdResetsY)
		{
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			eulerAngles.y = this.InitialYRotation;
			base.transform.rotation = Quaternion.Euler(eulerAngles);
		}
	}

	// Token: 0x040022F0 RID: 8944
	public float Acceleration = 0.1f;

	// Token: 0x040022F1 RID: 8945
	public float Damping = 0.3f;

	// Token: 0x040022F2 RID: 8946
	public float BackAndSideDampen = 0.5f;

	// Token: 0x040022F3 RID: 8947
	public float JumpForce = 0.3f;

	// Token: 0x040022F4 RID: 8948
	public float RotationAmount = 1.5f;

	// Token: 0x040022F5 RID: 8949
	public float RotationRatchet = 45f;

	// Token: 0x040022F6 RID: 8950
	public bool HmdResetsY = true;

	// Token: 0x040022F7 RID: 8951
	public bool HmdRotatesY = true;

	// Token: 0x040022F8 RID: 8952
	public float GravityModifier = 0.379f;

	// Token: 0x040022F9 RID: 8953
	public bool useProfileData = true;

	// Token: 0x040022FA RID: 8954
	protected CharacterController Controller;

	// Token: 0x040022FB RID: 8955
	protected OVRCameraRig CameraRig;

	// Token: 0x040022FC RID: 8956
	private float MoveScale = 1f;

	// Token: 0x040022FD RID: 8957
	private Vector3 MoveThrottle = Vector3.zero;

	// Token: 0x040022FE RID: 8958
	private float FallSpeed;

	// Token: 0x040022FF RID: 8959
	private OVRPose? InitialPose;

	// Token: 0x04002300 RID: 8960
	private float InitialYRotation;

	// Token: 0x04002301 RID: 8961
	private float MoveScaleMultiplier = 1f;

	// Token: 0x04002302 RID: 8962
	private float RotationScaleMultiplier = 1f;

	// Token: 0x04002303 RID: 8963
	private bool SkipMouseRotation;

	// Token: 0x04002304 RID: 8964
	private bool HaltUpdateMovement;

	// Token: 0x04002305 RID: 8965
	private bool prevHatLeft;

	// Token: 0x04002306 RID: 8966
	private bool prevHatRight;

	// Token: 0x04002307 RID: 8967
	private float SimulationRate = 60f;
}
