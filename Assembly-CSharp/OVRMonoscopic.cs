using System;
using UnityEngine;

// Token: 0x020006E0 RID: 1760
public class OVRMonoscopic : MonoBehaviour
{
	// Token: 0x06003A10 RID: 14864 RVA: 0x00125440 File Offset: 0x00123840
	private void Update()
	{
		if (OVRInput.GetDown(this.toggleButton, OVRInput.Controller.Active))
		{
			this.monoscopic = !this.monoscopic;
			OVRManager.instance.monoscopic = this.monoscopic;
		}
	}

	// Token: 0x040022EE RID: 8942
	public OVRInput.RawButton toggleButton = OVRInput.RawButton.B;

	// Token: 0x040022EF RID: 8943
	private bool monoscopic;
}
