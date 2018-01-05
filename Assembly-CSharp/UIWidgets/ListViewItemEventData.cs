using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x02000950 RID: 2384
	public class ListViewItemEventData : BaseEventData
	{
		// Token: 0x06004832 RID: 18482 RVA: 0x00183831 File Offset: 0x00181C31
		public ListViewItemEventData(EventSystem eventSystem) : base(eventSystem)
		{
		}

		// Token: 0x0400311C RID: 12572
		public GameObject NewSelectedObject;
	}
}
