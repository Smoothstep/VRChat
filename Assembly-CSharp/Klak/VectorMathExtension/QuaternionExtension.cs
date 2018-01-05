using System;
using UnityEngine;

namespace Klak.VectorMathExtension
{
	// Token: 0x0200051B RID: 1307
	internal static class QuaternionExtension
	{
		// Token: 0x06002DFD RID: 11773 RVA: 0x000E1866 File Offset: 0x000DFC66
		public static Vector4 ToVector4(this Quaternion q)
		{
			return new Vector4(q.x, q.y, q.z, q.w);
		}
	}
}
