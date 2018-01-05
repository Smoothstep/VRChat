using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200082C RID: 2092
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x06004134 RID: 16692 RVA: 0x00142382 File Offset: 0x00140782
		public virtual DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.None;
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06004135 RID: 16693
		public abstract bool active { get; }

		// Token: 0x06004136 RID: 16694 RVA: 0x00142385 File Offset: 0x00140785
		public virtual void OnEnable()
		{
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x00142387 File Offset: 0x00140787
		public virtual void OnDisable()
		{
		}

		// Token: 0x06004138 RID: 16696
		public abstract PostProcessingModel GetModel();

		// Token: 0x04002A4B RID: 10827
		public PostProcessingContext context;
	}
}
