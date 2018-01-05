using System;

namespace VRC
{
	// Token: 0x02000B4D RID: 2893
	public interface IEvent
	{
		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x060058A0 RID: 22688
		double Time { get; }

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x060058A1 RID: 22689
		bool Store { get; }

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x060058A2 RID: 22690
		// (set) Token: 0x060058A3 RID: 22691
		int Instigator { get; set; }
	}
}
