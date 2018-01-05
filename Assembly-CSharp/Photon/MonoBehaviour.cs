using System;
using UnityEngine;

namespace Photon
{
	// Token: 0x02000756 RID: 1878
	public class MonoBehaviour : UnityEngine.MonoBehaviour
	{
		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06003CAC RID: 15532 RVA: 0x000BF6C4 File Offset: 0x000BDAC4
		public PhotonView photonView
		{
			get
			{
				if (this.pvCache == null)
				{
					this.pvCache = PhotonView.Get(this);
				}
				return this.pvCache;
			}
		}

		// Token: 0x04002612 RID: 9746
		private PhotonView pvCache;
	}
}
