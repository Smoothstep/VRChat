using System;

// Token: 0x02000B3C RID: 2876
public class VRCUser : Storage
{
	// Token: 0x17000CC9 RID: 3273
	// (get) Token: 0x0600584B RID: 22603 RVA: 0x001E98B5 File Offset: 0x001E7CB5
	public static bool isInitialized
	{
		get
		{
			return VRCUser.mInitialized;
		}
	}

	// Token: 0x0600584C RID: 22604 RVA: 0x001E98BC File Offset: 0x001E7CBC
	private void Awake()
	{
		VRCUser.mInitialized = true;
		if (VRCUser.onInitialized != null)
		{
			VRCUser.onInitialized();
		}
	}

	// Token: 0x04003F50 RID: 16208
	private static bool mInitialized;

	// Token: 0x04003F51 RID: 16209
	public static VRCUser.OnInitialized onInitialized;

	// Token: 0x02000B3D RID: 2877
	// (Invoke) Token: 0x0600584F RID: 22607
	public delegate void OnInitialized();
}
