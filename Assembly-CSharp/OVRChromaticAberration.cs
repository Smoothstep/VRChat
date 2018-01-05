using System;
using UnityEngine;

// Token: 0x020006DB RID: 1755
public class OVRChromaticAberration : MonoBehaviour
{
	// Token: 0x060039EC RID: 14828 RVA: 0x00123EB0 File Offset: 0x001222B0
	private void Start()
	{
		OVRManager.instance.chromatic = this.chromatic;
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x00123EC2 File Offset: 0x001222C2
	private void Update()
	{
		if (OVRInput.GetDown(this.toggleButton, OVRInput.Controller.Active))
		{
			this.chromatic = !this.chromatic;
			OVRManager.instance.chromatic = this.chromatic;
		}
	}

	// Token: 0x040022BE RID: 8894
	public OVRInput.RawButton toggleButton = OVRInput.RawButton.X;

	// Token: 0x040022BF RID: 8895
	private bool chromatic;
}
