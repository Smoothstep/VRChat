using System;

namespace ExitGames.Client.DemoParticle
{
	// Token: 0x020007A6 RID: 1958
	public class TimeKeeper
	{
		// Token: 0x06003F45 RID: 16197 RVA: 0x0013E9A5 File Offset: 0x0013CDA5
		public TimeKeeper(int interval)
		{
			this.IsEnabled = true;
			this.Interval = interval;
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x06003F46 RID: 16198 RVA: 0x0013E9C6 File Offset: 0x0013CDC6
		// (set) Token: 0x06003F47 RID: 16199 RVA: 0x0013E9CE File Offset: 0x0013CDCE
		public int Interval { get; set; }

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06003F48 RID: 16200 RVA: 0x0013E9D7 File Offset: 0x0013CDD7
		// (set) Token: 0x06003F49 RID: 16201 RVA: 0x0013E9DF File Offset: 0x0013CDDF
		public bool IsEnabled { get; set; }

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06003F4A RID: 16202 RVA: 0x0013E9E8 File Offset: 0x0013CDE8
		// (set) Token: 0x06003F4B RID: 16203 RVA: 0x0013EA1A File Offset: 0x0013CE1A
		public bool ShouldExecute
		{
			get
			{
				return this.IsEnabled && (this.shouldExecute || Environment.TickCount - this.lastExecutionTime > this.Interval);
			}
			set
			{
				this.shouldExecute = value;
			}
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x0013EA23 File Offset: 0x0013CE23
		public void Reset()
		{
			this.shouldExecute = false;
			this.lastExecutionTime = Environment.TickCount;
		}

		// Token: 0x040027A0 RID: 10144
		private int lastExecutionTime = Environment.TickCount;

		// Token: 0x040027A1 RID: 10145
		private bool shouldExecute;
	}
}
