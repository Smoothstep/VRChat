using System;
using System.Collections;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000B38 RID: 2872
public class VRCMotionState : VRCPunBehaviour
{
	// Token: 0x17000CAE RID: 3246
	// (get) Token: 0x060057D3 RID: 22483 RVA: 0x001E65C2 File Offset: 0x001E49C2
	public bool isGrounded
	{
		get
		{
			return this._isGrounded;
		}
	}

	// Token: 0x17000CAF RID: 3247
	// (get) Token: 0x060057D4 RID: 22484 RVA: 0x001E65CA File Offset: 0x001E49CA
	// (set) Token: 0x060057D5 RID: 22485 RVA: 0x001E65D2 File Offset: 0x001E49D2
	public bool isLocomoting
	{
		get
		{
			return this._locomoting;
		}
		set
		{
			this._locomoting = value;
		}
	}

	// Token: 0x17000CB0 RID: 3248
	// (get) Token: 0x060057D6 RID: 22486 RVA: 0x001E65DB File Offset: 0x001E49DB
	public Vector3 walkingVelocity
	{
		get
		{
			return Quaternion.Inverse(base.transform.rotation) * this.playerVelocity;
		}
	}

	// Token: 0x17000CB1 RID: 3249
	// (get) Token: 0x060057D7 RID: 22487 RVA: 0x001E65F8 File Offset: 0x001E49F8
	public Vector3 inertialVelocity
	{
		get
		{
			return this.platformVelocity + this.playerVelocity;
		}
	}

	// Token: 0x17000CB2 RID: 3250
	// (get) Token: 0x060057D8 RID: 22488 RVA: 0x001E660B File Offset: 0x001E4A0B
	// (set) Token: 0x060057D9 RID: 22489 RVA: 0x001E6613 File Offset: 0x001E4A13
	public Vector3 PlayerVelocity
	{
		get
		{
			return this.playerVelocity;
		}
		set
		{
			this.playerVelocity = value;
		}
	}

	// Token: 0x17000CB3 RID: 3251
	// (get) Token: 0x060057DA RID: 22490 RVA: 0x001E661C File Offset: 0x001E4A1C
	// (set) Token: 0x060057DB RID: 22491 RVA: 0x001E6624 File Offset: 0x001E4A24
	public bool IsImmobilized
	{
		get
		{
			return this.immobilized;
		}
		set
		{
			if (base.isMine && this.immobilized != value)
			{
				this.immobilized = value;
			}
		}
	}

	// Token: 0x17000CB4 RID: 3252
	// (get) Token: 0x060057DC RID: 22492 RVA: 0x001E6644 File Offset: 0x001E4A44
	// (set) Token: 0x060057DD RID: 22493 RVA: 0x001E664C File Offset: 0x001E4A4C
	public bool IsSeated
	{
		get
		{
			return this.seated;
		}
		set
		{
			if (base.isMine && this.seated != value)
			{
				this.seated = value;
			}
		}
	}

	// Token: 0x17000CB5 RID: 3253
	// (get) Token: 0x060057DE RID: 22494 RVA: 0x001E666C File Offset: 0x001E4A6C
	// (set) Token: 0x060057DF RID: 22495 RVA: 0x001E6674 File Offset: 0x001E4A74
	public bool InVehicle
	{
		get
		{
			return this.inVehicle;
		}
		set
		{
			if (base.isMine && this.inVehicle != value)
			{
				this.inVehicle = value;
			}
		}
	}

	// Token: 0x17000CB6 RID: 3254
	// (get) Token: 0x060057E0 RID: 22496 RVA: 0x001E6694 File Offset: 0x001E4A94
	// (set) Token: 0x060057E1 RID: 22497 RVA: 0x001E669C File Offset: 0x001E4A9C
	public float StandingHeight
	{
		get
		{
			return this.standHeight;
		}
		set
		{
			if (!Mathf.Approximately(this.standHeight, value))
			{
				this.standHeight = value;
			}
		}
	}

	// Token: 0x060057E2 RID: 22498 RVA: 0x001E66B8 File Offset: 0x001E4AB8
	public override IEnumerator Start()
	{
		yield return base.Start();
		base.ObserveThis();
		this.characterController = base.GetComponent<CharacterController>();
		if (VRCPlayer.Instance != base.GetComponent<VRCPlayer>())
		{
			UnityEngine.Object.Destroy(this.characterController);
			this.characterController = null;
		}
		if (this.characterController != null)
		{
			this.slopeLimitAngleCos = Mathf.Cos(this.characterController.slopeLimit * 0.0174532924f);
		}
		for (int i = 0; i < 32; i++)
		{
			if (string.IsNullOrEmpty(LayerMask.LayerToName(i)))
			{
				this.groundedLayers.value = (this.groundedLayers.value & ~(1 << i));
			}
		}
		yield break;
	}

	// Token: 0x060057E3 RID: 22499 RVA: 0x001E66D3 File Offset: 0x001E4AD3
	public void AddMotionOffset(Vector3 motion)
	{
		if (this.characterController != null)
		{
			this.characterController.Move(motion);
		}
	}

	// Token: 0x060057E4 RID: 22500 RVA: 0x001E66F4 File Offset: 0x001E4AF4
	private void Update()
	{
		if (this.jumpResetTimer > 0f)
		{
			this.jumpResetTimer -= Time.deltaTime;
			if (this.jumpResetTimer <= 0f)
			{
				this.jump = VRCMotionState.JumpState.None;
			}
		}
		switch (this.jump)
		{
		case VRCMotionState.JumpState.Pressed:
			if (this._isGrounded)
			{
				this.playerVelocity.y = this.playerVelocity.y + this.jumpPower * 10.0f;
				this.jump = VRCMotionState.JumpState.ForceApplied;
			}
			break;
		case VRCMotionState.JumpState.ForceApplied:
			if (!this._isGrounded)
			{
				this.jump = VRCMotionState.JumpState.Airborne;
			}
			break;
		case VRCMotionState.JumpState.Airborne:
			if (this._isGrounded)
			{
				this.jump = VRCMotionState.JumpState.None;
			}
			break;
        }
    }

	// Token: 0x060057E5 RID: 22501 RVA: 0x001E67BE File Offset: 0x001E4BBE
	public void Jump(float power)
	{
		this.jumpResetTimer = 0.2f;
        this.jumpPower = power * 10.0f;
		this.jump = VRCMotionState.JumpState.Pressed;
	}

	// Token: 0x060057E6 RID: 22502 RVA: 0x001E67D9 File Offset: 0x001E4BD9
	public void Stop()
	{
		this.playerVelocity = Vector3.zero;
	}

	// Token: 0x060057E7 RID: 22503 RVA: 0x001E67E8 File Offset: 0x001E4BE8
	public void Reset()
	{
		this.platformVelocity = Vector3.zero;
		this.playerVelocity = Vector3.zero;
		this.platformRotation = 0f;
		this.jump = VRCMotionState.JumpState.None;
		this.jumpResetTimer = 0f;
		this._isGrounded = true;
		this._isNearGround = true;
		this.platformCollider = null;
		this.platformNormal = Vector3.zero;
		this.platformRelativePos = Vector3.zero;
		this.platformWorldPos = Vector3.zero;
		this.platformRelativeRot = 0f;
		this.platformWorldRot = 0f;
	}

	// Token: 0x060057E8 RID: 22504 RVA: 0x001E6874 File Offset: 0x001E4C74
	public void SetLocomotion(Vector3 velo, float dt)
	{
		this.platformVelocity = Vector3.zero;
		if (!this._isGrounded || this.jump == VRCMotionState.JumpState.ForceApplied)
		{
			if (Mathf.Sign(this.playerVelocity.x) == Mathf.Sign(velo.x) && Mathf.Abs(this.playerVelocity.x) > Mathf.Abs(velo.x))
			{
				velo.x = 0f;
			}
			if (Mathf.Sign(this.playerVelocity.z) == Mathf.Sign(velo.z) && Mathf.Abs(this.playerVelocity.z) > Mathf.Abs(velo.z))
			{
				velo.z = 0f;
			}
			this.playerVelocity += velo * this.AirControlPct;
			this.platformRotation = 0f;
		}
		else if (this.platformCollider != null)
		{
			Vector3 a = this.platformCollider.transform.TransformPoint(this.platformRelativePos);
			Vector3 a2 = a - this.platformWorldPos;
			this.platformRotation = this.platformCollider.transform.rotation.eulerAngles.y + this.platformRelativeRot - this.platformWorldRot;
			Vector3 planeNormal = this.platformNormal;
			if (planeNormal.y <= this.slopeLimitAngleCos)
			{
				Vector3 axis = Vector3.Cross(Vector3.up, this.platformNormal);
				axis.Normalize();
				planeNormal = Quaternion.AngleAxis(this.characterController.slopeLimit, axis) * Vector3.up;
			}
			this.playerVelocity = ((!this._isNearGround) ? velo : Vector3.ProjectOnPlane(velo, planeNormal));
			this.platformVelocity = a2 / dt;
		}
		else
		{
			this.platformRotation = 0f;
			this.playerVelocity = velo;
			this.platformVelocity = Vector3.zero;
		}
		this.playerVelocity += Physics.gravity * dt;
		this.platformCollider = null;
		this._isGrounded = false;
		this._isNearGround = false;
		this.Bounced = false;
		Vector3 position = base.transform.position;
		if (this.platformVelocity != Vector3.zero)
		{
			base.transform.position += this.platformVelocity * dt;
		}
		base.transform.rotation = Quaternion.Euler(0f, this.platformRotation, 0f) * base.transform.rotation;
		Vector3 vector = this.playerVelocity * dt;
		vector.y = -0.01f;
		float magnitude = vector.magnitude;
		int num = Mathf.CeilToInt(magnitude / 0.2f);
		if (this.characterController != null)
		{
			for (int i = 0; i < num; i++)
			{
				this.characterController.Move(this.playerVelocity * dt / (float)num);
			}
		}
		if (!this.Bounced)
		{
			Vector3 vector2 = (base.transform.position - position) / dt;
			this.GroundCheck();
			if (!vector2.AlmostEquals(this.platformVelocity + this.playerVelocity, 0.001f))
			{
				Vector3 vector3 = vector2 - this.platformVelocity;
				Vector2 target = new Vector2(vector3.x, vector3.z);
				Vector2 second = new Vector2(this.playerVelocity.x, this.playerVelocity.z);
				if (!target.AlmostEquals(second, 0.0001f))
				{
					this.playerVelocity.x = vector2.x - this.platformVelocity.x;
					this.playerVelocity.z = vector2.z - this.platformVelocity.z;
				}
			}
		}
		if (this._isGrounded && this.jump != VRCMotionState.JumpState.ForceApplied)
		{
			this.playerVelocity.y = 0f;
		}
		else
		{
			this.platformCollider = null;
		}
	}

	// Token: 0x060057E9 RID: 22505 RVA: 0x001E6CAC File Offset: 0x001E50AC
	private void TestHitAsPlatform(Vector3 hitNormal, Collider hitCollider, Transform hitTransform)
	{
		if (hitNormal.y > 0f)
		{
			this._isGrounded = true;
			this.platformCollider = hitCollider;
			this.platformNormal = hitNormal;
			this.platformWorldPos = base.transform.position;
			this.platformRelativePos = hitTransform.InverseTransformPoint(this.platformWorldPos);
			this.platformWorldRot = base.transform.rotation.eulerAngles.y;
			this.platformRelativeRot = this.platformWorldRot - hitTransform.rotation.eulerAngles.y;
		}
	}

	// Token: 0x060057EA RID: 22506 RVA: 0x001E6D48 File Offset: 0x001E5148
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.collider.material != null && hit.collider.material.bounciness != 0f)
		{
			Vector3 b = Vector3.zero;
			if (hit.rigidbody != null)
			{
				b = hit.rigidbody.GetPointVelocity(hit.point);
			}
			Vector3 vector = this.playerVelocity - b;
			if (Vector3.Dot(vector, hit.normal) < -3f)
			{
				this._isGrounded = false;
				this.playerVelocity = Vector3.Reflect(vector, hit.normal) * (hit.collider.material.bounciness + 0.01f);
				this.Bounced = true;
				this.platformCollider = null;
				return;
			}
		}
		this.TestHitAsPlatform(hit.normal, hit.collider, hit.transform);
	}

	// Token: 0x060057EB RID: 22507 RVA: 0x001E6E2C File Offset: 0x001E522C
	private void GroundCheck()
	{
		if (this.characterController == null)
		{
			return;
		}
		if (!this._isGrounded)
		{
			Ray ray = new Ray(base.transform.position + Vector3.up, Vector3.down);
			RaycastHit hitInfo;
			bool flag = Physics.Raycast(ray, out hitInfo, 1.2f, this.groundedLayers, QueryTriggerInteraction.Ignore);
			if (flag && Vector3.Distance(hitInfo.point, ray.origin) < 1.03f && this.CanGroundOn(hitInfo))
			{
				this.TestHitAsPlatform(hitInfo.normal, hitInfo.collider, hitInfo.transform);
			}
		}
		if (!this._isGrounded)
		{
			float radius = this.characterController.radius;
			Ray ray2 = new Ray(base.transform.position + Vector3.up * radius, Vector3.down);
			RaycastHit hitInfo2;
			bool flag2 = Physics.SphereCast(ray2, radius, out hitInfo2, 0.2f, this.groundedLayers, QueryTriggerInteraction.Ignore);
			if (flag2 && Vector3.Distance(hitInfo2.point, ray2.origin) < radius + 0.03f && this.CanGroundOn(hitInfo2))
			{
				this.TestHitAsPlatform(hitInfo2.normal, hitInfo2.collider, hitInfo2.transform);
			}
		}
		Ray ray3 = new Ray(base.transform.position + Vector3.up, Vector3.down);
		RaycastHit hitInfo3;
		bool flag3 = Physics.Raycast(ray3, out hitInfo3, 2f, this.groundedLayers, QueryTriggerInteraction.Ignore);
		if (flag3 && Vector3.Distance(hitInfo3.point, ray3.origin) < 1.1f && this.CanGroundOn(hitInfo3))
		{
			this._isNearGround = true;
		}
	}

	// Token: 0x060057EC RID: 22508 RVA: 0x001E6FFC File Offset: 0x001E53FC
	private bool CanGroundOn(RaycastHit hitInfo)
	{
		VRC_Pickup componentInSelfOrParent = hitInfo.collider.gameObject.GetComponentInSelfOrParent<VRC_Pickup>();
		return !(componentInSelfOrParent != null) || !(componentInSelfOrParent.currentlyHeldBy != null);
	}

	// Token: 0x060057ED RID: 22509 RVA: 0x001E703C File Offset: 0x001E543C
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			byte b = (byte)(((!this.immobilized) ? 0 : 1) | ((!this._locomoting) ? 0 : 2) | ((!this.seated) ? 0 : 4) | ((!this.inVehicle) ? 0 : 8));
			stream.SendNext(b);
			stream.SendNext((short)Mathf.FloatToHalf(this.standHeight));
		}
		else
		{
			byte b2 = (byte)stream.ReceiveNext();
			this.immobilized = ((b2 & 1) > 0);
			this._locomoting = ((b2 & 2) > 0);
			this.seated = ((b2 & 4) > 0);
			this.inVehicle = ((b2 & 8) > 0);
			short num = (short)stream.ReceiveNext();
			this.standHeight = Mathf.HalfToFloat((ushort)num);
		}
	}

	// Token: 0x060057EE RID: 22510 RVA: 0x001E711E File Offset: 0x001E551E
	private void DebugLogHelper(object message)
	{
	}

	// Token: 0x04003EF0 RID: 16112
	public LayerMask groundedLayers;

	// Token: 0x04003EF1 RID: 16113
	public float AirControlPct = 0.05f;

	// Token: 0x04003EF2 RID: 16114
	private CharacterController characterController;

	// Token: 0x04003EF3 RID: 16115
	private Vector3 platformVelocity = Vector3.zero;

	// Token: 0x04003EF4 RID: 16116
	private Vector3 playerVelocity = Vector3.zero;

	// Token: 0x04003EF5 RID: 16117
	private float platformRotation;

	// Token: 0x04003EF6 RID: 16118
	private bool _isGrounded = true;

	// Token: 0x04003EF7 RID: 16119
	private bool _isNearGround = true;

	// Token: 0x04003EF8 RID: 16120
	private Collider platformCollider;

	// Token: 0x04003EF9 RID: 16121
	private Vector3 platformNormal = Vector3.zero;

	// Token: 0x04003EFA RID: 16122
	private Vector3 platformRelativePos = Vector3.zero;

	// Token: 0x04003EFB RID: 16123
	private Vector3 platformWorldPos = Vector3.zero;

	// Token: 0x04003EFC RID: 16124
	private float platformRelativeRot;

	// Token: 0x04003EFD RID: 16125
	private float platformWorldRot;

	// Token: 0x04003EFE RID: 16126
	private float slopeLimitAngleCos = Mathf.Cos(0.7853982f);

	// Token: 0x04003EFF RID: 16127
	private bool _locomoting;

	// Token: 0x04003F00 RID: 16128
	private bool immobilized;

	// Token: 0x04003F01 RID: 16129
	private bool inVehicle;

	// Token: 0x04003F02 RID: 16130
	private bool seated;

	// Token: 0x04003F03 RID: 16131
	private float standHeight = 1f;

	// Token: 0x04003F04 RID: 16132
	private float jumpResetTimer;

	// Token: 0x04003F05 RID: 16133
	private VRCMotionState.JumpState jump;

	// Token: 0x04003F06 RID: 16134
	private float jumpPower;

	// Token: 0x04003F07 RID: 16135
	private bool Bounced;

	// Token: 0x02000B39 RID: 2873
	private enum JumpState
	{
		// Token: 0x04003F09 RID: 16137
		None,
		// Token: 0x04003F0A RID: 16138
		Pressed,
		// Token: 0x04003F0B RID: 16139
		ForceApplied,
		// Token: 0x04003F0C RID: 16140
		Airborne
	}
}
