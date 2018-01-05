using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200082D RID: 2093
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x0600413A RID: 16698 RVA: 0x00142391 File Offset: 0x00140791
		// (set) Token: 0x0600413B RID: 16699 RVA: 0x00142399 File Offset: 0x00140799
		public T model { get; internal set; }

		// Token: 0x0600413C RID: 16700 RVA: 0x001423A2 File Offset: 0x001407A2
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x001423B2 File Offset: 0x001407B2
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}
