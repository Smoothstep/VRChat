using System;
using UnityEngine;

// Token: 0x020006E3 RID: 1763
public class OVRResetOrientation : MonoBehaviour
{
	// Token: 0x06003A29 RID: 14889 RVA: 0x00126371 File Offset: 0x00124771
	private void Update()
	{
		if (OVRInput.GetDown(this.resetButton, OVRInput.Controller.Active))
		{
			OVRManager.display.RecenterPose();
		}
	}

	// Token: 0x04002310 RID: 8976
	public OVRInput.RawButton resetButton = OVRInput.RawButton.Y;
}
