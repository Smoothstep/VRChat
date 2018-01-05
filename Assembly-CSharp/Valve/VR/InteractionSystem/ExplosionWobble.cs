using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BD7 RID: 3031
	public class ExplosionWobble : MonoBehaviour
	{
		// Token: 0x06005DC5 RID: 24005 RVA: 0x0020CFC0 File Offset: 0x0020B3C0
		public void ExplosionEvent(Vector3 explosionPos)
		{
			Rigidbody component = base.GetComponent<Rigidbody>();
			if (component)
			{
				component.AddExplosionForce(2000f, explosionPos, 10f);
			}
		}
	}
}
