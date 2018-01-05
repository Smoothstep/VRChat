using System;
using System.Collections.Generic;

namespace UIWidgets
{
	// Token: 0x0200096F RID: 2415
	public static class ForEachExtensions
	{
		// Token: 0x06004949 RID: 18761 RVA: 0x00187978 File Offset: 0x00185D78
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
		{
			int num = 0;
			foreach (T arg in enumerable)
			{
				handler(arg, num);
				num++;
			}
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x001879D4 File Offset: 0x00185DD4
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> handler)
		{
			foreach (T obj in enumerable)
			{
				handler(obj);
			}
		}
	}
}
