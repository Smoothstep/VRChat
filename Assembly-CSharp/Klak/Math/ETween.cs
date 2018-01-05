using System;
using UnityEngine;

namespace Klak.Math
{
	// Token: 0x02000522 RID: 1314
	internal static class ETween
	{
		// Token: 0x06002E2D RID: 11821 RVA: 0x000E22AC File Offset: 0x000E06AC
		public static float Step(float current, float target, float omega)
		{
			float t = Mathf.Exp(-omega * Time.deltaTime);
			return Mathf.Lerp(target, current, t);
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x000E22D0 File Offset: 0x000E06D0
		public static float StepAngle(float current, float target, float omega)
		{
			float num = Mathf.Exp(-omega * Time.deltaTime);
			float num2 = Mathf.DeltaAngle(current, target);
			return target - num2 * num;
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x000E22F8 File Offset: 0x000E06F8
		public static Vector3 Step(Vector3 current, Vector3 target, float omega)
		{
			float t = Mathf.Exp(-omega * Time.deltaTime);
			return Vector3.Lerp(target, current, t);
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000E231C File Offset: 0x000E071C
		public static Quaternion Step(Quaternion current, Quaternion target, float omega)
		{
			if (current == target)
			{
				return target;
			}
			float t = Mathf.Exp(-omega * Time.deltaTime);
			return Quaternion.Lerp(target, current, t);
		}
	}
}
