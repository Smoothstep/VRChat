using System;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000AA2 RID: 2722
public class InteractivePlayer : MonoBehaviour
{
	// Token: 0x060051CD RID: 20941 RVA: 0x001C0680 File Offset: 0x001BEA80
	private void Awake()
	{
		this.inUseLeft = VRCInputManager.FindInput("UseLeft");
		this.inUseRight = VRCInputManager.FindInput("UseRight");
		this.inGrabLeft = VRCInputManager.FindInput("GrabLeft");
		this.inGrabRight = VRCInputManager.FindInput("GrabRight");
	}

	// Token: 0x060051CE RID: 20942 RVA: 0x001C06D0 File Offset: 0x001BEAD0
	private void Update()
	{
		if (this.inUseRight.down || this.inGrabRight.down)
		{
			VRC_Interactable[] interactable = VRCUiCursorManager.GetInteractable(VRCUiCursor.CursorHandedness.Right);
			if (interactable != null)
			{
				foreach (VRC_Interactable vrc_Interactable in interactable)
				{
					vrc_Interactable.Interact();
				}
			}
		}
		if (this.inUseLeft.down || this.inGrabLeft.down)
		{
			VRC_Interactable[] interactable2 = VRCUiCursorManager.GetInteractable(VRCUiCursor.CursorHandedness.Left);
			if (interactable2 != null)
			{
				foreach (VRC_Interactable vrc_Interactable2 in interactable2)
				{
					vrc_Interactable2.Interact();
				}
			}
		}
	}

	// Token: 0x04003A14 RID: 14868
	private VRCInput inUseLeft;

	// Token: 0x04003A15 RID: 14869
	private VRCInput inUseRight;

	// Token: 0x04003A16 RID: 14870
	private VRCInput inGrabLeft;

	// Token: 0x04003A17 RID: 14871
	private VRCInput inGrabRight;
}
