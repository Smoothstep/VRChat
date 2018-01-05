using System;

namespace PhysSound
{
	// Token: 0x020007C0 RID: 1984
	public class PhysSoundComposition
	{
		// Token: 0x06004003 RID: 16387 RVA: 0x0014208D File Offset: 0x0014048D
		public PhysSoundComposition(int key)
		{
			this.KeyIndex = key;
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x0014209C File Offset: 0x0014049C
		public void Reset()
		{
			this.Value = 0f;
			this.Count = 0;
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x001420B0 File Offset: 0x001404B0
		public void Add(float val)
		{
			this.Value += val;
			this.Count++;
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x001420CE File Offset: 0x001404CE
		public float GetAverage()
		{
			return this.Value / (float)this.Count;
		}

		// Token: 0x04002860 RID: 10336
		public int KeyIndex;

		// Token: 0x04002861 RID: 10337
		public float Value;

		// Token: 0x04002862 RID: 10338
		public int Count;
	}
}
