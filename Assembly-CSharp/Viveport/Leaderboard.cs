using System;

namespace Viveport
{
	// Token: 0x0200097E RID: 2430
	public class Leaderboard
	{
		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x0600499C RID: 18844 RVA: 0x0018A374 File Offset: 0x00188774
		// (set) Token: 0x0600499D RID: 18845 RVA: 0x0018A37C File Offset: 0x0018877C
		public int Rank { get; set; }

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x0600499E RID: 18846 RVA: 0x0018A385 File Offset: 0x00188785
		// (set) Token: 0x0600499F RID: 18847 RVA: 0x0018A38D File Offset: 0x0018878D
		public int Score { get; set; }

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x060049A0 RID: 18848 RVA: 0x0018A396 File Offset: 0x00188796
		// (set) Token: 0x060049A1 RID: 18849 RVA: 0x0018A39E File Offset: 0x0018879E
		public string UserName { get; set; }
	}
}
