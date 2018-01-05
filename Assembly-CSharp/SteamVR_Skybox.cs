using System;
using UnityEngine;
using Valve.VR;

// Token: 0x02000C0B RID: 3083
public class SteamVR_Skybox : MonoBehaviour
{
	// Token: 0x06005F87 RID: 24455 RVA: 0x0021981C File Offset: 0x00217C1C
	public void SetTextureByIndex(int i, Texture t)
	{
		switch (i)
		{
		case 0:
			this.front = t;
			break;
		case 1:
			this.back = t;
			break;
		case 2:
			this.left = t;
			break;
		case 3:
			this.right = t;
			break;
		case 4:
			this.top = t;
			break;
		case 5:
			this.bottom = t;
			break;
		}
	}

	// Token: 0x06005F88 RID: 24456 RVA: 0x00219894 File Offset: 0x00217C94
	public Texture GetTextureByIndex(int i)
	{
		switch (i)
		{
		case 0:
			return this.front;
		case 1:
			return this.back;
		case 2:
			return this.left;
		case 3:
			return this.right;
		case 4:
			return this.top;
		case 5:
			return this.bottom;
		default:
			return null;
		}
	}

	// Token: 0x06005F89 RID: 24457 RVA: 0x002198F0 File Offset: 0x00217CF0
	public static void SetOverride(Texture front = null, Texture back = null, Texture left = null, Texture right = null, Texture top = null, Texture bottom = null)
	{
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			Texture[] array = new Texture[]
			{
				front,
				back,
				left,
				right,
				top,
				bottom
			};
			Texture_t[] array2 = new Texture_t[6];
			for (int i = 0; i < 6; i++)
			{
				array2[i].handle = ((!(array[i] != null)) ? IntPtr.Zero : array[i].GetNativeTexturePtr());
				array2[i].eType = SteamVR.instance.textureType;
				array2[i].eColorSpace = EColorSpace.Auto;
			}
			EVRCompositorError evrcompositorError = compositor.SetSkyboxOverride(array2);
			if (evrcompositorError != EVRCompositorError.None)
			{
				Debug.LogError("Failed to set skybox override with error: " + evrcompositorError);
				if (evrcompositorError == EVRCompositorError.TextureIsOnWrongDevice)
				{
					Debug.Log("Set your graphics driver to use the same video card as the headset is plugged into for Unity.");
				}
				else if (evrcompositorError == EVRCompositorError.TextureUsesUnsupportedFormat)
				{
					Debug.Log("Ensure skybox textures are not compressed and have no mipmaps.");
				}
			}
		}
	}

	// Token: 0x06005F8A RID: 24458 RVA: 0x002199E4 File Offset: 0x00217DE4
	public static void ClearOverride()
	{
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			compositor.ClearSkyboxOverride();
		}
	}

	// Token: 0x06005F8B RID: 24459 RVA: 0x00219A03 File Offset: 0x00217E03
	private void OnEnable()
	{
		SteamVR_Skybox.SetOverride(this.front, this.back, this.left, this.right, this.top, this.bottom);
	}

	// Token: 0x06005F8C RID: 24460 RVA: 0x00219A2E File Offset: 0x00217E2E
	private void OnDisable()
	{
		SteamVR_Skybox.ClearOverride();
	}

	// Token: 0x0400453D RID: 17725
	public Texture front;

	// Token: 0x0400453E RID: 17726
	public Texture back;

	// Token: 0x0400453F RID: 17727
	public Texture left;

	// Token: 0x04004540 RID: 17728
	public Texture right;

	// Token: 0x04004541 RID: 17729
	public Texture top;

	// Token: 0x04004542 RID: 17730
	public Texture bottom;

	// Token: 0x04004543 RID: 17731
	public SteamVR_Skybox.CellSize StereoCellSize = SteamVR_Skybox.CellSize.x32;

	// Token: 0x04004544 RID: 17732
	public float StereoIpdMm = 64f;

	// Token: 0x02000C0C RID: 3084
	public enum CellSize
	{
		// Token: 0x04004546 RID: 17734
		x1024,
		// Token: 0x04004547 RID: 17735
		x64,
		// Token: 0x04004548 RID: 17736
		x32,
		// Token: 0x04004549 RID: 17737
		x16,
		// Token: 0x0400454A RID: 17738
		x8
	}
}
