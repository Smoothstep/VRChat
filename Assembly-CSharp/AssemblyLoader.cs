using System;
using System.Reflection;

// Token: 0x02000B51 RID: 2897
[Serializable]
public class AssemblyLoader
{
	// Token: 0x060058C4 RID: 22724 RVA: 0x001EBEEC File Offset: 0x001EA2EC
	public void Load()
	{
		AppDomain.CurrentDomain.ResourceResolve += ((object sender, ResolveEventArgs args) => Assembly.Load(this.DataBytes));
		Assembly.Load(this.DataBytes);
	}

	// Token: 0x04003F8A RID: 16266
	public byte[] DataBytes;
}
