using System;
using UnityEngine;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009E8 RID: 2536
	public class Enableable : MonoBehaviour
	{
		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004D24 RID: 19748 RVA: 0x0019D9E6 File Offset: 0x0019BDE6
		// (set) Token: 0x06004D25 RID: 19749 RVA: 0x0019D9EE File Offset: 0x0019BDEE
		[SerializeField]
		public virtual bool isEnabled
		{
			get
			{
				return this.mEnabled;
			}
			set
			{
				this.mEnabled = value;
			}
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x0019D9F7 File Offset: 0x0019BDF7
		public void Enable()
		{
			this.isEnabled = true;
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x0019DA00 File Offset: 0x0019BE00
		public void Disable()
		{
			this.isEnabled = false;
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x0019DA09 File Offset: 0x0019BE09
		public void ToggleEnable()
		{
			this.isEnabled = !this.isEnabled;
		}

		// Token: 0x04003510 RID: 13584
		protected VRC_EventHandler eventHandler;

		// Token: 0x04003511 RID: 13585
		[HideInInspector]
		[SerializeField]
		protected bool mEnabled = true;
	}
}
