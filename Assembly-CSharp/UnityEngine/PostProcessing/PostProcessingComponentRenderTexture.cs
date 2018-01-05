using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200082F RID: 2095
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06004143 RID: 16707 RVA: 0x0014299B File Offset: 0x00140D9B
		public virtual void Prepare(Material material)
		{
		}
	}
}
