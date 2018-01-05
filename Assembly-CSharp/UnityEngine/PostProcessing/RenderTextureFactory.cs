using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000836 RID: 2102
	public sealed class RenderTextureFactory : IDisposable
	{
		// Token: 0x06004162 RID: 16738 RVA: 0x0014A0DB File Offset: 0x001484DB
		public RenderTextureFactory()
		{
			this.m_TemporaryRTs = new HashSet<RenderTexture>();
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x0014A0F0 File Offset: 0x001484F0
		public RenderTexture Get(RenderTexture baseRenderTexture)
		{
			return this.Get(baseRenderTexture.width, baseRenderTexture.height, baseRenderTexture.depth, baseRenderTexture.format, (!baseRenderTexture.sRGB) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB, baseRenderTexture.filterMode, baseRenderTexture.wrapMode, "FactoryTempTexture");
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x0014A140 File Offset: 0x00148540
		public RenderTexture Get(int width, int height, int depthBuffer = 0, RenderTextureFormat format = RenderTextureFormat.ARGBHalf, RenderTextureReadWrite rw = RenderTextureReadWrite.Default, FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, string name = "FactoryTempTexture")
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format);
			temporary.filterMode = filterMode;
			temporary.wrapMode = wrapMode;
			temporary.name = name;
			this.m_TemporaryRTs.Add(temporary);
			return temporary;
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x0014A180 File Offset: 0x00148580
		public void Release(RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (!this.m_TemporaryRTs.Contains(rt))
			{
				throw new ArgumentException(string.Format("Attempting to remove a RenderTexture that was not allocated: {0}", rt));
			}
			this.m_TemporaryRTs.Remove(rt);
			RenderTexture.ReleaseTemporary(rt);
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x0014A1D0 File Offset: 0x001485D0
		public void ReleaseAll()
		{
			foreach (RenderTexture temp in this.m_TemporaryRTs)
			{
				RenderTexture.ReleaseTemporary(temp);
			}
			this.m_TemporaryRTs.Clear();
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x0014A211 File Offset: 0x00148611
		public void Dispose()
		{
			this.ReleaseAll();
		}

		// Token: 0x04002A6A RID: 10858
		private HashSet<RenderTexture> m_TemporaryRTs;
	}
}
