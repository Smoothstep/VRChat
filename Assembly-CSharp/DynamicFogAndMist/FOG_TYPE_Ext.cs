using System;

namespace DynamicFogAndMist
{
	// Token: 0x020009AB RID: 2475
	internal static class FOG_TYPE_Ext
	{
		// Token: 0x06004AA6 RID: 19110 RVA: 0x0018C502 File Offset: 0x0018A902
		public static bool isPlus(this FOG_TYPE fogType)
		{
			return fogType == FOG_TYPE.DesktopFogPlusWithSkyHaze || fogType == FOG_TYPE.MobileFogSimple || fogType == FOG_TYPE.MobileFogBasic || fogType == FOG_TYPE.MobileFogOrthogonal || fogType == FOG_TYPE.DesktopFogPlusOrthogonal;
		}
	}
}
