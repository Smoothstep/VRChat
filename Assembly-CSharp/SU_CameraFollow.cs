using System;
using UnityEngine;

// Token: 0x020008CD RID: 2253
public class SU_CameraFollow : MonoBehaviour
{
	// Token: 0x060044BF RID: 17599 RVA: 0x0017001F File Offset: 0x0016E41F
	private void Start()
	{
		this._cacheTransform = base.transform;
	}

	// Token: 0x060044C0 RID: 17600 RVA: 0x0017002D File Offset: 0x0016E42D
	private void FixedUpdate()
	{
		if (this.updateMode == SU_CameraFollow.UpdateMode.FIXED_UPDATE)
		{
			this.DoCamera();
		}
	}

	// Token: 0x060044C1 RID: 17601 RVA: 0x00170040 File Offset: 0x0016E440
	private void Update()
	{
		if (this.updateMode == SU_CameraFollow.UpdateMode.UPDATE)
		{
			this.DoCamera();
		}
	}

	// Token: 0x060044C2 RID: 17602 RVA: 0x00170054 File Offset: 0x0016E454
	private void LateUpdate()
	{
		if (this.updateMode == SU_CameraFollow.UpdateMode.LATE_UPDATE)
		{
			this.DoCamera();
		}
	}

	// Token: 0x060044C3 RID: 17603 RVA: 0x00170068 File Offset: 0x0016E468
	private void DoCamera()
	{
		if (this.target == null)
		{
			return;
		}
		SU_CameraFollow.FollowMode followMode = this.followMode;
		if (followMode != SU_CameraFollow.FollowMode.SPECTATOR)
		{
			if (followMode == SU_CameraFollow.FollowMode.CHASE)
			{
				if (!Input.GetKey(this.freezeKey))
				{
					Quaternion b = this.target.rotation;
					this._cacheTransform.rotation = Quaternion.Lerp(this._cacheTransform.rotation, b, Time.deltaTime * this.lookAtDamping);
					this._cacheTransform.position = Vector3.Lerp(this._cacheTransform.position, this.target.position - this.target.forward * this.distance + this.target.up * this.chaseHeight, Time.deltaTime * this.followDamping * 10f);
				}
			}
		}
		else
		{
			Quaternion b = Quaternion.LookRotation(this.target.position - this._cacheTransform.position);
			this._cacheTransform.rotation = Quaternion.Lerp(this._cacheTransform.rotation, b, Time.deltaTime * this.lookAtDamping);
			if (!Input.GetKey(this.freezeKey) && Vector3.Distance(this._cacheTransform.position, this.target.position) > this.distance)
			{
				this._cacheTransform.position = Vector3.Lerp(this._cacheTransform.position, this.target.position, Time.deltaTime * this.followDamping);
			}
		}
	}

	// Token: 0x04002EB1 RID: 11953
	public SU_CameraFollow.UpdateMode updateMode;

	// Token: 0x04002EB2 RID: 11954
	public SU_CameraFollow.FollowMode followMode = SU_CameraFollow.FollowMode.SPECTATOR;

	// Token: 0x04002EB3 RID: 11955
	public Transform target;

	// Token: 0x04002EB4 RID: 11956
	public float distance = 60f;

	// Token: 0x04002EB5 RID: 11957
	public float chaseHeight = 15f;

	// Token: 0x04002EB6 RID: 11958
	public float followDamping = 0.3f;

	// Token: 0x04002EB7 RID: 11959
	public float lookAtDamping = 4f;

	// Token: 0x04002EB8 RID: 11960
	public KeyCode freezeKey;

	// Token: 0x04002EB9 RID: 11961
	private Transform _cacheTransform;

	// Token: 0x020008CE RID: 2254
	public enum UpdateMode
	{
		// Token: 0x04002EBB RID: 11963
		FIXED_UPDATE,
		// Token: 0x04002EBC RID: 11964
		UPDATE,
		// Token: 0x04002EBD RID: 11965
		LATE_UPDATE
	}

	// Token: 0x020008CF RID: 2255
	public enum FollowMode
	{
		// Token: 0x04002EBF RID: 11967
		CHASE,
		// Token: 0x04002EC0 RID: 11968
		SPECTATOR
	}
}
