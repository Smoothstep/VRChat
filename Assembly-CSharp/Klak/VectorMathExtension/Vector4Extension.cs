using System;
using UnityEngine;

namespace Klak.VectorMathExtension
{
	// Token: 0x0200051A RID: 1306
	internal static class Vector4Extension
	{
		// Token: 0x06002DFB RID: 11771 RVA: 0x000E1818 File Offset: 0x000DFC18
		public static Quaternion ToQuaternion(this Vector4 v)
		{
			return new Quaternion(v.x, v.y, v.z, v.w);
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000E183B File Offset: 0x000DFC3B
		public static Quaternion ToNormalizedQuaternion(this Vector4 v)
		{
			v = Vector4.Normalize(v);
			return new Quaternion(v.x, v.y, v.z, v.w);
		}
	}
}
