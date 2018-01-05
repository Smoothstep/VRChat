using System;

// Token: 0x02000A67 RID: 2663
public static class SingleExtensions
{
	// Token: 0x06005099 RID: 20633 RVA: 0x001B8B4D File Offset: 0x001B6F4D
	public static bool IsBad(this float val)
	{
		return float.IsInfinity(val) || float.IsNaN(val) || float.IsNegativeInfinity(val) || float.IsPositiveInfinity(val);
	}
}
