using System;
using UnityEngine;

namespace VolumetricFogAndMist
{
	// Token: 0x020009B1 RID: 2481
	public class FogAreaCullingManager : MonoBehaviour
	{
		// Token: 0x06004AD7 RID: 19159 RVA: 0x0018EBCC File Offset: 0x0018CFCC
		private void OnEnable()
		{
			if (this.fog == null)
			{
				this.fog = base.GetComponent<VolumetricFog>();
				if (this.fog == null)
				{
					this.fog = base.gameObject.AddComponent<VolumetricFog>();
				}
			}
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x0018EC18 File Offset: 0x0018D018
		private void OnBecameVisible()
		{
			if (this.fog != null)
			{
				this.fog.enabled = true;
			}
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x0018EC37 File Offset: 0x0018D037
		private void OnBecameInvisible()
		{
			if (this.fog != null)
			{
				this.fog.enabled = false;
			}
		}

		// Token: 0x040032BA RID: 12986
		public VolumetricFog fog;
	}
}
