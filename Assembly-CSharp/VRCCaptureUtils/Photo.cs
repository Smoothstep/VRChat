using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009ED RID: 2541
	[RequireComponent(typeof(VRC_Pickup))]
	public class Photo : VRCPunBehaviour
	{
		// Token: 0x06004D49 RID: 19785 RVA: 0x0019E5E0 File Offset: 0x0019C9E0
		public override IEnumerator Start()
		{
			yield return base.Start();
			this.overlayRend = base.GetComponent<Renderer>();
			yield break;
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0019E5FB File Offset: 0x0019C9FB
		public void SetCameraAndUser(PolaroidCamera cam, VRC.Player player)
		{
			if (base.isMine)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "SetCameraRPC", new object[]
				{
					cam.gameObject,
					VRC.Network.GetInstigatorID(player)
				});
			}
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x0019E638 File Offset: 0x0019CA38
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.All
		})]
		private void SetCameraRPC(GameObject camObj, int newOwnerID, VRC.Player instigator)
		{
			if (base.Owner != instigator)
			{
				return;
			}
			this.cam = camObj.GetComponent<PolaroidCamera>();
			VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(newOwnerID);
			if (playerByInstigatorID != null)
			{
				this.attribution.text = "Taken by: " + playerByInstigatorID.name;
			}
			if (this.cam.screenShot != null)
			{
				this.photoRend.material.SetTexture("_MainTex", this.cam.screenShot);
				this.photoRend.material.SetTexture("_EmissionMap", this.cam.screenShot);
			}
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x0019E6E7 File Offset: 0x0019CAE7
		private void Update()
		{
			this.timer += Time.deltaTime;
			if (this.timer >= this.lifetime)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x0400353B RID: 13627
		public float lifetime = 60f;

		// Token: 0x0400353C RID: 13628
		public Renderer photoRend;

		// Token: 0x0400353D RID: 13629
		public Renderer overlayRend;

		// Token: 0x0400353E RID: 13630
		public Text attribution;

		// Token: 0x0400353F RID: 13631
		private float timer;

		// Token: 0x04003540 RID: 13632
		private PolaroidCamera cam;

		// Token: 0x04003541 RID: 13633
		private bool isHeld;
	}
}
