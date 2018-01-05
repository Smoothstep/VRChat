using System;
using UnityEngine;

// Token: 0x020006DF RID: 1759
public class OVRModeParms : MonoBehaviour
{
	// Token: 0x06003A0C RID: 14860 RVA: 0x001253CF File Offset: 0x001237CF
	private void Start()
	{
		if (!OVRManager.isHmdPresent)
		{
			base.enabled = false;
			return;
		}
		base.InvokeRepeating("TestPowerStateMode", 10f, 10f);
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x001253F8 File Offset: 0x001237F8
	private void Update()
	{
		if (OVRInput.GetDown(this.resetButton, OVRInput.Controller.Active))
		{
			OVRPlugin.cpuLevel = 0;
			OVRPlugin.gpuLevel = 1;
		}
	}

	// Token: 0x06003A0E RID: 14862 RVA: 0x0012541B File Offset: 0x0012381B
	private void TestPowerStateMode()
	{
		if (OVRPlugin.powerSaving)
		{
			Debug.Log("POWER SAVE MODE ACTIVATED");
		}
	}

	// Token: 0x040022ED RID: 8941
	public OVRInput.RawButton resetButton = OVRInput.RawButton.X;
}
