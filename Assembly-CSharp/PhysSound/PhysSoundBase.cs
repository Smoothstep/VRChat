using System;
using UnityEngine;

namespace PhysSound
{
	// Token: 0x020007B8 RID: 1976
	public abstract class PhysSoundBase : MonoBehaviour
	{
		// Token: 0x06003FC7 RID: 16327
		public abstract PhysSoundMaterial GetPhysSoundMaterial(Vector3 contactPoint);
	}
}
