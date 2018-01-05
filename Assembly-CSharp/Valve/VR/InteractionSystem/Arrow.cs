using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BCF RID: 3023
	public class Arrow : MonoBehaviour
	{
		// Token: 0x06005D9A RID: 23962 RVA: 0x0020B54E File Offset: 0x0020994E
		private void Start()
		{
			Physics.IgnoreCollision(this.shaftRB.GetComponent<Collider>(), Player.instance.headCollider);
		}

		// Token: 0x06005D9B RID: 23963 RVA: 0x0020B56C File Offset: 0x0020996C
		private void FixedUpdate()
		{
			if (this.released && this.inFlight)
			{
				this.prevPosition = base.transform.position;
				this.prevRotation = base.transform.rotation;
				this.prevVelocity = base.GetComponent<Rigidbody>().velocity;
				this.prevHeadPosition = this.arrowHeadRB.transform.position;
				this.travelledFrames++;
			}
		}

		// Token: 0x06005D9C RID: 23964 RVA: 0x0020B5E8 File Offset: 0x002099E8
		public void ArrowReleased(float inputVelocity)
		{
			this.inFlight = true;
			this.released = true;
			this.airReleaseSound.Play();
			if (this.glintParticle != null)
			{
				this.glintParticle.Play();
			}
			if (base.gameObject.GetComponentInChildren<FireSource>().isBurning)
			{
				this.fireReleaseSound.Play();
			}
			RaycastHit[] array = Physics.SphereCastAll(base.transform.position, 0.01f, base.transform.forward, 0.8f, -5, QueryTriggerInteraction.Ignore);
			foreach (RaycastHit raycastHit in array)
			{
				if (raycastHit.collider.gameObject != base.gameObject && raycastHit.collider.gameObject != this.arrowHeadRB.gameObject && raycastHit.collider != Player.instance.headCollider)
				{
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
			}
			this.travelledFrames = 0;
			this.prevPosition = base.transform.position;
			this.prevRotation = base.transform.rotation;
			this.prevHeadPosition = this.arrowHeadRB.transform.position;
			this.prevVelocity = base.GetComponent<Rigidbody>().velocity;
			UnityEngine.Object.Destroy(base.gameObject, 30f);
		}

		// Token: 0x06005D9D RID: 23965 RVA: 0x0020B75C File Offset: 0x00209B5C
		private void OnCollisionEnter(Collision collision)
		{
			if (this.inFlight)
			{
				Rigidbody component = base.GetComponent<Rigidbody>();
				float sqrMagnitude = component.velocity.sqrMagnitude;
				bool flag = this.targetPhysMaterial != null && collision.collider.sharedMaterial == this.targetPhysMaterial && sqrMagnitude > 0.2f;
				bool flag2 = collision.collider.gameObject.GetComponent<Balloon>() != null;
				if (this.travelledFrames < 2 && !flag)
				{
					base.transform.position = this.prevPosition - this.prevVelocity * Time.deltaTime;
					base.transform.rotation = this.prevRotation;
					Vector3 a = Vector3.Reflect(this.arrowHeadRB.velocity, collision.contacts[0].normal);
					this.arrowHeadRB.velocity = a * 0.25f;
					this.shaftRB.velocity = a * 0.25f;
					this.travelledFrames = 0;
					return;
				}
				if (this.glintParticle != null)
				{
					this.glintParticle.Stop(true);
				}
				if (sqrMagnitude > 0.1f)
				{
					this.hitGroundSound.Play();
				}
				FireSource componentInChildren = base.gameObject.GetComponentInChildren<FireSource>();
				FireSource componentInParent = collision.collider.GetComponentInParent<FireSource>();
				if (componentInChildren != null && componentInChildren.isBurning && componentInParent != null)
				{
					if (!this.hasSpreadFire)
					{
						collision.collider.gameObject.SendMessageUpwards("FireExposure", base.gameObject, SendMessageOptions.DontRequireReceiver);
						this.hasSpreadFire = true;
					}
				}
				else if (sqrMagnitude > 0.1f || flag2)
				{
					collision.collider.gameObject.SendMessageUpwards("ApplyDamage", SendMessageOptions.DontRequireReceiver);
					base.gameObject.SendMessage("HasAppliedDamage", SendMessageOptions.DontRequireReceiver);
				}
				if (flag2)
				{
					base.transform.position = this.prevPosition;
					base.transform.rotation = this.prevRotation;
					this.arrowHeadRB.velocity = this.prevVelocity;
					Physics.IgnoreCollision(this.arrowHeadRB.GetComponent<Collider>(), collision.collider);
					Physics.IgnoreCollision(this.shaftRB.GetComponent<Collider>(), collision.collider);
				}
				if (flag)
				{
					this.StickInTarget(collision, this.travelledFrames < 2);
				}
				if (Player.instance && collision.collider == Player.instance.headCollider)
				{
					Player.instance.PlayerShotSelf();
				}
			}
		}

		// Token: 0x06005D9E RID: 23966 RVA: 0x0020BA0C File Offset: 0x00209E0C
		private void StickInTarget(Collision collision, bool bSkipRayCast)
		{
			Vector3 direction = this.prevRotation * Vector3.forward;
			if (!bSkipRayCast)
			{
				RaycastHit[] array = Physics.RaycastAll(this.prevHeadPosition - this.prevVelocity * Time.deltaTime, direction, this.prevVelocity.magnitude * Time.deltaTime * 2f);
				bool flag = false;
				foreach (RaycastHit raycastHit in array)
				{
					if (raycastHit.collider == collision.collider)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			UnityEngine.Object.Destroy(this.glintParticle);
			this.inFlight = false;
			this.shaftRB.velocity = Vector3.zero;
			this.shaftRB.angularVelocity = Vector3.zero;
			this.shaftRB.isKinematic = true;
			this.shaftRB.useGravity = false;
			this.shaftRB.transform.GetComponent<BoxCollider>().enabled = false;
			this.arrowHeadRB.velocity = Vector3.zero;
			this.arrowHeadRB.angularVelocity = Vector3.zero;
			this.arrowHeadRB.isKinematic = true;
			this.arrowHeadRB.useGravity = false;
			this.arrowHeadRB.transform.GetComponent<BoxCollider>().enabled = false;
			this.hitTargetSound.Play();
			this.scaleParentObject = new GameObject("Arrow Scale Parent");
			Transform transform = collision.collider.transform;
			ExplosionWobble component = collision.collider.gameObject.GetComponent<ExplosionWobble>();
			if (!component && transform.parent)
			{
				transform = transform.parent;
			}
			this.scaleParentObject.transform.parent = transform;
			base.transform.parent = this.scaleParentObject.transform;
			base.transform.rotation = this.prevRotation;
			base.transform.position = this.prevPosition;
			base.transform.position = collision.contacts[0].point - base.transform.forward * (0.75f - (Util.RemapNumberClamped(this.prevVelocity.magnitude, 0f, 10f, 0f, 0.1f) + UnityEngine.Random.Range(0f, 0.05f)));
		}

		// Token: 0x06005D9F RID: 23967 RVA: 0x0020BC75 File Offset: 0x0020A075
		private void OnDestroy()
		{
			if (this.scaleParentObject != null)
			{
				UnityEngine.Object.Destroy(this.scaleParentObject);
			}
		}

		// Token: 0x0400430B RID: 17163
		public ParticleSystem glintParticle;

		// Token: 0x0400430C RID: 17164
		public Rigidbody arrowHeadRB;

		// Token: 0x0400430D RID: 17165
		public Rigidbody shaftRB;

		// Token: 0x0400430E RID: 17166
		public PhysicMaterial targetPhysMaterial;

		// Token: 0x0400430F RID: 17167
		private Vector3 prevPosition;

		// Token: 0x04004310 RID: 17168
		private Quaternion prevRotation;

		// Token: 0x04004311 RID: 17169
		private Vector3 prevVelocity;

		// Token: 0x04004312 RID: 17170
		private Vector3 prevHeadPosition;

		// Token: 0x04004313 RID: 17171
		public SoundPlayOneshot fireReleaseSound;

		// Token: 0x04004314 RID: 17172
		public SoundPlayOneshot airReleaseSound;

		// Token: 0x04004315 RID: 17173
		public SoundPlayOneshot hitTargetSound;

		// Token: 0x04004316 RID: 17174
		public PlaySound hitGroundSound;

		// Token: 0x04004317 RID: 17175
		private bool inFlight;

		// Token: 0x04004318 RID: 17176
		private bool released;

		// Token: 0x04004319 RID: 17177
		private bool hasSpreadFire;

		// Token: 0x0400431A RID: 17178
		private int travelledFrames;

		// Token: 0x0400431B RID: 17179
		private GameObject scaleParentObject;
	}
}
