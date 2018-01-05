using System;

// Token: 0x02000ADE RID: 2782
public interface IRenderCallbackListener
{
	// Token: 0x06005464 RID: 21604
	void OnWillRenderObject();

	// Token: 0x06005465 RID: 21605
	void OnPreCull();

	// Token: 0x06005466 RID: 21606
	void OnPreRender();

	// Token: 0x06005467 RID: 21607
	void OnPostRender();
}
