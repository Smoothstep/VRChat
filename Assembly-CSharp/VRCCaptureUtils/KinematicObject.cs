using System;
using UnityEngine;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009E9 RID: 2537
	public class KinematicObject : MonoBehaviour
	{
		// Token: 0x06004D2A RID: 19754 RVA: 0x0019DA22 File Offset: 0x0019BE22
		private void Start()
		{
			this.objSync = base.GetComponent<VRC_ObjectSync>();
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x0019DA30 File Offset: 0x0019BE30
		private void OnNetworkReady()
		{
			if (Networking.IsMaster && this.setKinematic)
			{
				this.objSync.isKinematic = true;
			}
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x0019DA53 File Offset: 0x0019BE53
		public void OnUse()
		{
			this.useGravity = !this.objSync.useGravity;
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x0019DA69 File Offset: 0x0019BE69
		public void OnDrop()
		{
			if (this.setKinematic)
			{
				this.objSync.isKinematic = true;
			}
			this.objSync.useGravity = this.useGravity;
		}

		// Token: 0x06004D2E RID: 19758 RVA: 0x0019DA93 File Offset: 0x0019BE93
		public void OnPickup()
		{
			if (this.setKinematic)
			{
				this.objSync.isKinematic = false;
			}
		}

		// Token: 0x04003512 RID: 13586
		public bool setKinematic;

		// Token: 0x04003513 RID: 13587
		private VRC_ObjectSync objSync;

		// Token: 0x04003514 RID: 13588
		private bool useGravity;
	}
}
