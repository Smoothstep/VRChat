using System;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x020006E2 RID: 1762
public class OVRRTOverlayConnector : MonoBehaviour
{
	// Token: 0x06003A25 RID: 14885 RVA: 0x00126208 File Offset: 0x00124608
	private void ConstructRenderTextureChain()
	{
		for (int i = 0; i < 3; i++)
		{
			this.overlayRTChain[i] = new RenderTexture(this.srcRT.width, this.srcRT.height, 1, this.srcRT.format, RenderTextureReadWrite.sRGB);
			this.overlayRTChain[i].antiAliasing = 1;
			this.overlayRTChain[i].depth = 0;
			this.overlayRTChain[i].wrapMode = TextureWrapMode.Clamp;
			this.overlayRTChain[i].hideFlags = HideFlags.HideAndDontSave;
			this.overlayRTChain[i].Create();
			this.overlayTexturePtrs[i] = this.overlayRTChain[i].GetNativeTexturePtr();
		}
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x001262B8 File Offset: 0x001246B8
	private void Start()
	{
		this.ownerCamera = base.GetComponent<Camera>();
		this.srcRT = this.ownerCamera.targetTexture;
		this.ConstructRenderTextureChain();
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x001262E0 File Offset: 0x001246E0
	private void OnPostRender()
	{
		if (this.srcRT)
		{
			Graphics.Blit(this.srcRT, this.overlayRTChain[this.overlayRTIndex]);
			OVROverlay component = this.ovrOverlayObj.GetComponent<OVROverlay>();
			component.OverrideOverlayTextureInfo(this.overlayRTChain[this.overlayRTIndex], this.overlayTexturePtrs[this.overlayRTIndex], VRNode.LeftEye);
			this.overlayRTIndex++;
			this.overlayRTIndex %= 3;
		}
	}

	// Token: 0x04002308 RID: 8968
	public int alphaBorderSizePixels = 3;

	// Token: 0x04002309 RID: 8969
	private const int overlayRTChainSize = 3;

	// Token: 0x0400230A RID: 8970
	private int overlayRTIndex;

	// Token: 0x0400230B RID: 8971
	private IntPtr[] overlayTexturePtrs = new IntPtr[3];

	// Token: 0x0400230C RID: 8972
	private RenderTexture[] overlayRTChain = new RenderTexture[3];

	// Token: 0x0400230D RID: 8973
	public GameObject ovrOverlayObj;

	// Token: 0x0400230E RID: 8974
	private RenderTexture srcRT;

	// Token: 0x0400230F RID: 8975
	private Camera ownerCamera;
}
