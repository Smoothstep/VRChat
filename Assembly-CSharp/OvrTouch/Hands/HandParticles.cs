using System;
using UnityEngine;

namespace OvrTouch.Hands
{
	// Token: 0x0200071D RID: 1821
	[RequireComponent(typeof(ParticleSystem))]
	public class HandParticles : MonoBehaviour
	{
		// Token: 0x06003B67 RID: 15207 RVA: 0x0012B114 File Offset: 0x00129514
		public void SetHand(Hand hand)
		{
			this.m_hand = hand;
			this.m_rigidbody.transform.position = this.m_hand.transform.position;
			this.m_rigidbody.transform.rotation = this.m_hand.transform.rotation;
			this.m_fixedJoint.connectedBody = this.m_hand.Rigidbody;
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x0012B180 File Offset: 0x00129580
		private void Awake()
		{
			this.m_particleSystem = base.GetComponent<ParticleSystem>();
			SphereCollider sphereCollider = base.gameObject.AddComponent<SphereCollider>();
			sphereCollider.radius = 0.01f;
			sphereCollider.isTrigger = true;
			this.m_rigidbody = base.gameObject.AddComponent<Rigidbody>();
			this.m_fixedJoint = base.gameObject.AddComponent<FixedJoint>();
			if (this.m_hand != null)
			{
				this.SetHand(this.m_hand);
			}
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x0012B1F8 File Offset: 0x001295F8
		private void Update()
		{
			if (this.m_hand != null)
			{
				float emissionRate = this.m_hand.LinearVelocity.magnitude * this.m_emissionRateVelocityScale;
				this.m_particleSystem.emissionRate = emissionRate;
			}
		}

		// Token: 0x04002424 RID: 9252
		[SerializeField]
		private Hand m_hand;

		// Token: 0x04002425 RID: 9253
		[SerializeField]
		private float m_emissionRateVelocityScale = 25f;

		// Token: 0x04002426 RID: 9254
		private ParticleSystem m_particleSystem;

		// Token: 0x04002427 RID: 9255
		private Rigidbody m_rigidbody;

		// Token: 0x04002428 RID: 9256
		private FixedJoint m_fixedJoint;
	}
}
