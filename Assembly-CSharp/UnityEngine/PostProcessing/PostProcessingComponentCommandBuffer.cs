using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200082E RID: 2094
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x0600413F RID: 16703
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x06004140 RID: 16704
		public abstract string GetName();

		// Token: 0x06004141 RID: 16705
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
