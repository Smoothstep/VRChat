using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x02000921 RID: 2337
	[Serializable]
	public class AccordionItem
	{
		// Token: 0x0400301B RID: 12315
		public GameObject ToggleObject;

		// Token: 0x0400301C RID: 12316
		public GameObject ContentObject;

		// Token: 0x0400301D RID: 12317
		public bool Open;

		// Token: 0x0400301E RID: 12318
		[HideInInspector]
		public Coroutine CurrentCorutine;

		// Token: 0x0400301F RID: 12319
		[HideInInspector]
		public RectTransform ContentObjectRect;

		// Token: 0x04003020 RID: 12320
		[HideInInspector]
		public float ContentObjectHeight;
	}
}
