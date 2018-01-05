using System;
using UnityEngine;

namespace RealisticEyeMovements
{
	// Token: 0x020008B6 RID: 2230
	public class DestroyNotifier : MonoBehaviour
	{
		// Token: 0x1400004B RID: 75
		// (add) Token: 0x0600442E RID: 17454 RVA: 0x00169F7C File Offset: 0x0016837C
		// (remove) Token: 0x0600442F RID: 17455 RVA: 0x00169FB4 File Offset: 0x001683B4
		public event Action<DestroyNotifier> OnDestroyedEvent;

		// Token: 0x06004430 RID: 17456 RVA: 0x00169FEA File Offset: 0x001683EA
		private void OnDestroyed()
		{
			if (this.OnDestroyedEvent != null)
			{
				this.OnDestroyedEvent(this);
			}
		}
	}
}
