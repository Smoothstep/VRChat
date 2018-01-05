using System;
using UnityEngine;

// Token: 0x02000ADF RID: 2783
public class ForwardRenderCallback : MonoBehaviour
{
	// Token: 0x06005469 RID: 21609 RVA: 0x001D284D File Offset: 0x001D0C4D
	public void OnWillRenderObject()
	{
		if (this.Target != null)
		{
			this.Target.OnWillRenderObject();
		}
	}

	// Token: 0x0600546A RID: 21610 RVA: 0x001D2865 File Offset: 0x001D0C65
	public void OnPreCull()
	{
		if (this.Target != null)
		{
			this.Target.OnPreCull();
		}
	}

	// Token: 0x0600546B RID: 21611 RVA: 0x001D287D File Offset: 0x001D0C7D
	public void OnPreRender()
	{
		if (this.Target != null)
		{
			this.Target.OnPreRender();
		}
	}

	// Token: 0x0600546C RID: 21612 RVA: 0x001D2895 File Offset: 0x001D0C95
	public void OnPostRender()
	{
		if (this.Target != null)
		{
			this.Target.OnPostRender();
		}
	}

	// Token: 0x04003B84 RID: 15236
	public IRenderCallbackListener Target;
}
