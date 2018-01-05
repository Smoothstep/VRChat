using System;
using UnityEngine.SocialPlatforms;

namespace Tacticsoft
{
	// Token: 0x020008E0 RID: 2272
	internal static class RangeExtensions
	{
		// Token: 0x06004529 RID: 17705 RVA: 0x00171E32 File Offset: 0x00170232
		public static int Last(this Range range)
		{
			if (range.count == 0)
			{
				throw new InvalidOperationException("Empty range has no to()");
			}
			return range.from + range.count - 1;
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x00171E5C File Offset: 0x0017025C
		public static bool Contains(this Range range, int num)
		{
			return num >= range.from && num < range.from + range.count;
		}
	}
}
