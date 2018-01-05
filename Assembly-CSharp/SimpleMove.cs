using System;
using UnityEngine;

// Token: 0x02000AF2 RID: 2802
[RequireComponent(typeof(CharacterController))]
public class SimpleMove : MonoBehaviour
{
	// Token: 0x060054D3 RID: 21715 RVA: 0x001D41B2 File Offset: 0x001D25B2
	private void Awake()
	{
		this.characterController = base.GetComponent<CharacterController>();
	}

	// Token: 0x060054D4 RID: 21716 RVA: 0x001D41C0 File Offset: 0x001D25C0
	private void Update()
	{
		Vector3 speed = (base.transform.forward * Input.GetAxis("Vertical") + base.transform.right * Input.GetAxis("Horizontal")) * this.moveSpeed * 10.0f;
		this.characterController.SimpleMove(speed);
	}

	// Token: 0x04003BDE RID: 15326
	public float moveSpeed = 15f;

	// Token: 0x04003BDF RID: 15327
	public float rotationSpeed = 30f;

	// Token: 0x04003BE0 RID: 15328
	private Vector3 targetAngles;

	// Token: 0x04003BE1 RID: 15329
	private Vector3 followAngles;

	// Token: 0x04003BE2 RID: 15330
	private Vector3 followVelocity;

	// Token: 0x04003BE3 RID: 15331
	public Vector2 rotationRange = new Vector3(70f, 70f);

	// Token: 0x04003BE4 RID: 15332
	public float dampingTime = 0.2f;

	// Token: 0x04003BE5 RID: 15333
	private Vector3 inputVector;

	// Token: 0x04003BE6 RID: 15334
	private Vector3 headRotation;

	// Token: 0x04003BE7 RID: 15335
	public CharacterController characterController;
}
