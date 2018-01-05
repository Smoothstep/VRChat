using System;
using UnityEngine;

// Token: 0x02000840 RID: 2112
internal class ButtonTrigger : MonoBehaviour
{
	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x06004190 RID: 16784 RVA: 0x0014B49E File Offset: 0x0014989E
	// (set) Token: 0x06004191 RID: 16785 RVA: 0x0014B4A6 File Offset: 0x001498A6
	public int ID { get; set; }

	// Token: 0x06004192 RID: 16786 RVA: 0x0014B4B0 File Offset: 0x001498B0
	private void Update()
	{
		bool flag = this.hover;
		RaycastHit raycastHit;
		if (base.GetComponent<Collider>().Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, float.PositiveInfinity))
		{
			this.hover = true;
		}
		else
		{
			this.hover = false;
		}
		if (Input.GetMouseButtonDown(0) && this.hover)
		{
			PrimitivesDemo.Instance.OnButtonHit(this.ID);
		}
		else if (this.hover != flag)
		{
			PrimitivesDemo.Instance.OnButtonHover(this.ID, this.hover);
		}
	}

	// Token: 0x04002A87 RID: 10887
	private bool hover;
}
