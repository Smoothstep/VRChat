using System;
using UnityEngine;

// Token: 0x020006D2 RID: 1746
public class OVRProfile : UnityEngine.Object
{
	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x060039CA RID: 14794 RVA: 0x001238E3 File Offset: 0x00121CE3
	[Obsolete]
	public string id
	{
		get
		{
			return "000abc123def";
		}
	}

	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x060039CB RID: 14795 RVA: 0x001238EA File Offset: 0x00121CEA
	[Obsolete]
	public string userName
	{
		get
		{
			return "Oculus User";
		}
	}

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x060039CC RID: 14796 RVA: 0x001238F1 File Offset: 0x00121CF1
	[Obsolete]
	public string locale
	{
		get
		{
			return "en_US";
		}
	}

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x060039CD RID: 14797 RVA: 0x001238F8 File Offset: 0x00121CF8
	public float ipd
	{
		get
		{
			return Vector3.Distance(OVRPlugin.GetNodePose(OVRPlugin.Node.EyeLeft, false).ToOVRPose().position, OVRPlugin.GetNodePose(OVRPlugin.Node.EyeRight, false).ToOVRPose().position);
		}
	}

	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x060039CE RID: 14798 RVA: 0x00123932 File Offset: 0x00121D32
	public float eyeHeight
	{
		get
		{
			return OVRPlugin.eyeHeight;
		}
	}

	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x060039CF RID: 14799 RVA: 0x00123939 File Offset: 0x00121D39
	public float eyeDepth
	{
		get
		{
			return OVRPlugin.eyeDepth;
		}
	}

	// Token: 0x1700091B RID: 2331
	// (get) Token: 0x060039D0 RID: 14800 RVA: 0x00123940 File Offset: 0x00121D40
	public float neckHeight
	{
		get
		{
			return this.eyeHeight - 0.075f;
		}
	}

	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x060039D1 RID: 14801 RVA: 0x0012394E File Offset: 0x00121D4E
	[Obsolete]
	public OVRProfile.State state
	{
		get
		{
			return OVRProfile.State.READY;
		}
	}

	// Token: 0x020006D3 RID: 1747
	[Obsolete]
	public enum State
	{
		// Token: 0x040022A3 RID: 8867
		NOT_TRIGGERED,
		// Token: 0x040022A4 RID: 8868
		LOADING,
		// Token: 0x040022A5 RID: 8869
		READY,
		// Token: 0x040022A6 RID: 8870
		ERROR
	}
}
