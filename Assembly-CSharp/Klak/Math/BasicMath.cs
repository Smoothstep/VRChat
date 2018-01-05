using System;
using UnityEngine;

namespace Klak.Math
{
	// Token: 0x0200051C RID: 1308
	internal static class BasicMath
	{
		// Token: 0x06002DFE RID: 11774 RVA: 0x000E1889 File Offset: 0x000DFC89
		public static float Lerp(float a, float b, float mix)
		{
			return a * (1f - mix) + b * mix;
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000E1898 File Offset: 0x000DFC98
		public static Vector3 Lerp(Vector3 a, Vector3 b, float mix)
		{
			return a * (1f - mix) + b * mix;
		}
	}
}
