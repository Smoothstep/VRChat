using System;
using Photon;
using UnityEngine;

// Token: 0x0200078C RID: 1932
[RequireComponent(typeof(PhotonView))]
public class MoveByKeys : Photon.MonoBehaviour
{
	// Token: 0x06003EC2 RID: 16066 RVA: 0x0013C52B File Offset: 0x0013A92B
	public void Start()
	{
		this.isSprite = (base.GetComponent<SpriteRenderer>() != null);
		this.body2d = base.GetComponent<Rigidbody2D>();
		this.body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06003EC3 RID: 16067 RVA: 0x0013C558 File Offset: 0x0013A958
	public void FixedUpdate()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		if (Input.GetAxisRaw("Horizontal") < -0.1f || Input.GetAxisRaw("Horizontal") > 0.1f)
		{
			base.transform.position += Vector3.right * (this.Speed * Time.deltaTime) * Input.GetAxisRaw("Horizontal");
		}
		if (this.jumpingTime <= 0f)
		{
			if ((this.body != null || this.body2d != null) && Input.GetKey(KeyCode.Space))
			{
				this.jumpingTime = this.JumpTimeout;
				Vector2 vector = Vector2.up * this.JumpForce;
				if (this.body2d != null)
				{
					this.body2d.AddForce(vector);
				}
				else if (this.body != null)
				{
					this.body.AddForce(vector);
				}
			}
		}
		else
		{
			this.jumpingTime -= Time.deltaTime;
		}
		if (!this.isSprite && (Input.GetAxisRaw("Vertical") < -0.1f || Input.GetAxisRaw("Vertical") > 0.1f))
		{
			base.transform.position += Vector3.forward * (this.Speed * Time.deltaTime) * Input.GetAxisRaw("Vertical");
		}
	}

	// Token: 0x0400275F RID: 10079
	public float Speed = 10f;

	// Token: 0x04002760 RID: 10080
	public float JumpForce = 200f;

	// Token: 0x04002761 RID: 10081
	public float JumpTimeout = 0.5f;

	// Token: 0x04002762 RID: 10082
	private bool isSprite;

	// Token: 0x04002763 RID: 10083
	private float jumpingTime;

	// Token: 0x04002764 RID: 10084
	private Rigidbody body;

	// Token: 0x04002765 RID: 10085
	private Rigidbody2D body2d;
}
