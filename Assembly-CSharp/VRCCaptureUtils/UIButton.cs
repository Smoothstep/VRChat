using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VRCCaptureUtils
{
	// Token: 0x020009F9 RID: 2553
	public class UIButton : MonoBehaviour
	{
		// Token: 0x06004DAA RID: 19882 RVA: 0x001A0EC3 File Offset: 0x0019F2C3
		private void Start()
		{
			this.button.onClick.AddListener(new UnityAction(this.ButtonClicked));
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x001A0EE1 File Offset: 0x0019F2E1
		private void ButtonClicked()
		{
			this.playerViewer.UIButtonPressed(this.buttonType);
		}

		// Token: 0x040035B4 RID: 13748
		public Button button;

		// Token: 0x040035B5 RID: 13749
		public PlayerCamViewer playerViewer;

		// Token: 0x040035B6 RID: 13750
		public PlayerCamViewer.buttons buttonType;
	}
}
