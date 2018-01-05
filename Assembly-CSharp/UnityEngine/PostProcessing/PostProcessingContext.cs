using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000830 RID: 2096
	public class PostProcessingContext
	{
		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06004145 RID: 16709 RVA: 0x001499BE File Offset: 0x00147DBE
		// (set) Token: 0x06004146 RID: 16710 RVA: 0x001499C6 File Offset: 0x00147DC6
		public bool interrupted { get; private set; }

		// Token: 0x06004147 RID: 16711 RVA: 0x001499CF File Offset: 0x00147DCF
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x001499D8 File Offset: 0x00147DD8
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004149 RID: 16713 RVA: 0x001499FE File Offset: 0x00147DFE
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == RenderingPath.DeferredShading;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x0600414A RID: 16714 RVA: 0x00149A0E File Offset: 0x00147E0E
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x0600414B RID: 16715 RVA: 0x00149A1B File Offset: 0x00147E1B
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x0600414C RID: 16716 RVA: 0x00149A28 File Offset: 0x00147E28
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x0600414D RID: 16717 RVA: 0x00149A35 File Offset: 0x00147E35
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x04002A4D RID: 10829
		public PostProcessingProfile profile;

		// Token: 0x04002A4E RID: 10830
		public Camera camera;

		// Token: 0x04002A4F RID: 10831
		public MaterialFactory materialFactory;

		// Token: 0x04002A50 RID: 10832
		public RenderTextureFactory renderTextureFactory;
	}
}
