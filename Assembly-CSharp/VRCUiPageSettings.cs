using System;
using UnityEngine.UI;

// Token: 0x02000C7A RID: 3194
public class VRCUiPageSettings : VRCUiPage
{
	// Token: 0x06006326 RID: 25382 RVA: 0x0023425E File Offset: 0x0023265E
	public override void Awake()
	{
		base.Awake();
		this.versionText.text = "version " + VRCApplication.clientVersion;
	}

	// Token: 0x040048A1 RID: 18593
	public Text versionText;
}
