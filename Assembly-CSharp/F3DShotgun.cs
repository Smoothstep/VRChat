using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class F3DShotgun : MonoBehaviour
{
	// Token: 0x06002804 RID: 10244 RVA: 0x000D04D4 File Offset: 0x000CE8D4
	private void OnParticleCollision(GameObject other)
	{
		int num = base.GetComponent<ParticleSystem>().GetCollisionEvents(other, this.collisionEvents);
		for (int i = 0; i < num; i++)
		{
			F3DAudioController.instance.ShotGunHit(this.collisionEvents[i].intersection);
			if (other.GetComponent<Rigidbody>())
			{
				Vector3 intersection = this.collisionEvents[i].intersection;
				Vector3 force = this.collisionEvents[i].velocity.normalized * 50f;
				other.GetComponent<Rigidbody>().AddForceAtPosition(force, intersection);
			}
		}
		this.collisionEvents.Clear();
	}

	// Token: 0x04001647 RID: 5703
	private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
}
