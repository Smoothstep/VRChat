using System;
using UnityEngine;
using VRC.Core;

// Token: 0x020008D2 RID: 2258
public class SU_LaserShot : MonoBehaviour
{
	// Token: 0x17000A87 RID: 2695
	// (get) Token: 0x060044CA RID: 17610 RVA: 0x00170317 File Offset: 0x0016E717
	// (set) Token: 0x060044CB RID: 17611 RVA: 0x0017031F File Offset: 0x0016E71F
	public Transform firedBy { get; set; }

	// Token: 0x060044CC RID: 17612 RVA: 0x00170328 File Offset: 0x0016E728
	private void Start()
	{
		this._newPos = base.transform.position;
		this._oldPos = this._newPos;
		this._velocity = this.velocity * base.transform.forward;
		UnityEngine.Object.Destroy(base.gameObject, this.life);
	}

	// Token: 0x060044CD RID: 17613 RVA: 0x00170380 File Offset: 0x0016E780
	private void Update()
	{
		this._newPos += base.transform.forward * this._velocity.magnitude * Time.deltaTime;
		Vector3 direction = this._newPos - this._oldPos;
		float magnitude = direction.magnitude;
		RaycastHit raycastHit;
		if (magnitude > 0f && Physics.Raycast(this._oldPos, direction, out raycastHit, magnitude) && raycastHit.transform != this.firedBy && !raycastHit.collider.isTrigger)
		{
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, raycastHit.normal);
			AssetManagement.Instantiate(this.impactEffect, raycastHit.point, rot);
			if (UnityEngine.Random.Range(0, 20) < 2)
			{
				AssetManagement.Instantiate(this.explosionEffect, raycastHit.transform.position, rot);
				UnityEngine.Object.Destroy(raycastHit.transform.gameObject);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this._oldPos = base.transform.position;
		base.transform.position = this._newPos;
	}

	// Token: 0x04002EC3 RID: 11971
	public float life = 2f;

	// Token: 0x04002EC4 RID: 11972
	public float velocity = 1000f;

	// Token: 0x04002EC5 RID: 11973
	public Transform impactEffect;

	// Token: 0x04002EC6 RID: 11974
	public Transform explosionEffect;

	// Token: 0x04002EC8 RID: 11976
	private Vector3 _velocity;

	// Token: 0x04002EC9 RID: 11977
	private Vector3 _newPos;

	// Token: 0x04002ECA RID: 11978
	private Vector3 _oldPos;
}
