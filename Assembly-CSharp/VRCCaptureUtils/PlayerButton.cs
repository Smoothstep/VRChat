using System;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009EE RID: 2542
	public class PlayerButton : MonoBehaviour
	{
		// Token: 0x06004D4F RID: 19791 RVA: 0x0019E7CA File Offset: 0x0019CBCA
		public void ButtonClicked()
		{
			if (this.playerViewer.isMine)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, this.playerViewer.gameObject, "SetNewPlayer", new object[]
				{
					this.playerId
				});
			}
		}

		// Token: 0x04003542 RID: 13634
		public int playerId;

		// Token: 0x04003543 RID: 13635
		public Text playerNameText;

		// Token: 0x04003544 RID: 13636
		public Button button;

		// Token: 0x04003545 RID: 13637
		public PlayerCamViewer playerViewer;
	}
}
