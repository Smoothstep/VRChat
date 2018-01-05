using System;
using UIWidgets;
using UnityEngine;

namespace UIWidgetsSamples
{
	// Token: 0x02000915 RID: 2325
	[Serializable]
	public class ListViewVariableHeightItemDescription : IItemHeight
	{
		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x060045EB RID: 17899 RVA: 0x0017C699 File Offset: 0x0017AA99
		// (set) Token: 0x060045EC RID: 17900 RVA: 0x0017C6A1 File Offset: 0x0017AAA1
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x04003001 RID: 12289
		[SerializeField]
		public string Name;

		// Token: 0x04003002 RID: 12290
		[SerializeField]
		public string Text;

		// Token: 0x04003003 RID: 12291
		[SerializeField]
		private float height;
	}
}
