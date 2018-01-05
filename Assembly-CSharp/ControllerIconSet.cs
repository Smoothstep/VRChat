using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B01 RID: 2817
public class ControllerIconSet : MonoBehaviour
{
	// Token: 0x06005524 RID: 21796 RVA: 0x001D581B File Offset: 0x001D3C1B
	private void Awake()
	{
		this.Bind();
	}

	// Token: 0x06005525 RID: 21797 RVA: 0x001D5824 File Offset: 0x001D3C24
	private void Bind()
	{
		this.Icons[ControllerActionUI.None] = this.DefaultIcon;
		this.Icons[ControllerActionUI.Use] = this.Use;
		this.Icons[ControllerActionUI.Drop] = this.Drop;
		this.Icons[ControllerActionUI.ReleaseObject] = this.ReleaseObject;
		this.Icons[ControllerActionUI.Move] = this.Move;
		this.Icons[ControllerActionUI.UIMenu] = this.UIMenu;
		this.Icons[ControllerActionUI.UISelect] = this.UISelect;
	}

	// Token: 0x06005526 RID: 21798 RVA: 0x001D58AF File Offset: 0x001D3CAF
	public Material GetIcon(ControllerActionUI action)
	{
		if (this.Icons.ContainsKey(action))
		{
			return this.Icons[action];
		}
		return this.DefaultIcon;
	}

	// Token: 0x04003C1B RID: 15387
	public Material DefaultIcon;

	// Token: 0x04003C1C RID: 15388
	public Material Use;

	// Token: 0x04003C1D RID: 15389
	public Material Drop;

	// Token: 0x04003C1E RID: 15390
	public Material ReleaseObject;

	// Token: 0x04003C1F RID: 15391
	public Material Move;

	// Token: 0x04003C20 RID: 15392
	public Material UIMenu;

	// Token: 0x04003C21 RID: 15393
	public Material UISelect;

	// Token: 0x04003C22 RID: 15394
	private Dictionary<ControllerActionUI, Material> Icons = new Dictionary<ControllerActionUI, Material>();
}
