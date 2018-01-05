using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x02000933 RID: 2355
	[RequireComponent(typeof(RectTransform))]
	public class Draggable : MonoBehaviour
	{
		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x060046D9 RID: 18137 RVA: 0x001802F6 File Offset: 0x0017E6F6
		// (set) Token: 0x060046DA RID: 18138 RVA: 0x00180300 File Offset: 0x0017E700
		public GameObject Handle
		{
			get
			{
				return this.handle;
			}
			set
			{
				if (this.handle)
				{
					UnityEngine.Object.Destroy(this.handleScript);
				}
				this.handle = value;
				this.handleScript = this.handle.AddComponent<DraggableHandle>();
				this.handleScript.Drag(base.gameObject.GetComponent<RectTransform>());
			}
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x00180356 File Offset: 0x0017E756
		private void Start()
		{
			this.Handle = ((!(this.Handle == null)) ? this.handle : base.gameObject);
		}

		// Token: 0x0400305C RID: 12380
		[SerializeField]
		private GameObject handle;

		// Token: 0x0400305D RID: 12381
		private DraggableHandle handleScript;
	}
}
