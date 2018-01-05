using System;
using UnityEngine;

namespace Tacticsoft
{
	// Token: 0x020008E1 RID: 2273
	public class TableViewCell : MonoBehaviour
	{
		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x0600452C RID: 17708 RVA: 0x00170C6F File Offset: 0x0016F06F
		public virtual string reuseIdentifier
		{
			get
			{
				return base.GetType().Name;
			}
		}
	}
}
