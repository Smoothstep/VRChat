using System;

namespace PhysSound
{
	// Token: 0x020007C1 RID: 1985
	public class PhysSoundTypeList
	{
		// Token: 0x06004008 RID: 16392 RVA: 0x001420E6 File Offset: 0x001404E6
		public static string GetKey(int index)
		{
			return (index < PhysSoundTypeList.PhysSoundTypes.Length && index >= 0) ? PhysSoundTypeList.PhysSoundTypes[index] : string.Empty;
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0014210D File Offset: 0x0014050D
		public static bool HasKey(int index)
		{
			return index < PhysSoundTypeList.PhysSoundTypes.Length && index >= 0;
		}

		// Token: 0x04002863 RID: 10339
		public static string[] PhysSoundTypes = new string[]
		{
			"Hard",
			"Soft",
			"Other"
		};
	}
}
