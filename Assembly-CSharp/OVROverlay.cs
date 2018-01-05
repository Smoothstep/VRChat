using System;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x0200069C RID: 1692
public class OVROverlay : MonoBehaviour
{
	// Token: 0x060038ED RID: 14573 RVA: 0x001223FC File Offset: 0x001207FC
	public void OverrideOverlayTextureInfo(Texture srcTexture, IntPtr nativePtr, VRNode node)
	{
		int num = (node != VRNode.RightEye) ? 0 : 1;
		this.textures[num] = srcTexture;
		this.cachedTextures[num] = srcTexture;
		this.texNativePtrs[num] = nativePtr;
	}

	// Token: 0x060038EE RID: 14574 RVA: 0x00122438 File Offset: 0x00120838
	private void Awake()
	{
		Debug.Log("Overlay Awake");
		this.rend = base.GetComponent<Renderer>();
		for (int i = 0; i < 2; i++)
		{
			if (this.rend != null && this.textures[i] == null)
			{
				this.textures[i] = this.rend.material.mainTexture;
			}
			if (this.textures[i] != null)
			{
				this.cachedTextures[i] = this.textures[i];
				this.texNativePtrs[i] = this.textures[i].GetNativeTexturePtr();
			}
		}
	}

	// Token: 0x060038EF RID: 14575 RVA: 0x001224E8 File Offset: 0x001208E8
	private void OnEnable()
	{
		if (!OVRManager.isHmdPresent)
		{
			base.enabled = false;
			return;
		}
		this.OnDisable();
		for (int i = 0; i < 15; i++)
		{
			if (OVROverlay.instances[i] == null || OVROverlay.instances[i] == this)
			{
				this.layerIndex = i;
				OVROverlay.instances[i] = this;
				break;
			}
		}
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x00122558 File Offset: 0x00120958
	private void OnDisable()
	{
		if (this.layerIndex != -1)
		{
			OVRPlugin.SetOverlayQuad(true, false, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, OVRPose.identity.ToPosef(), Vector3.one.ToVector3f(), this.layerIndex, OVRPlugin.OverlayShape.Quad);
			OVROverlay.instances[this.layerIndex] = null;
		}
		this.layerIndex = -1;
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x001225BC File Offset: 0x001209BC
	private void OnRenderObject()
	{
		if (!Camera.current.CompareTag("MainCamera") || Camera.current.cameraType != CameraType.Game || this.layerIndex == -1 || this.currentOverlayType == OVROverlay.OverlayType.None)
		{
			return;
		}
		if (this.currentOverlayShape == OVROverlay.OverlayShape.Cubemap || this.currentOverlayShape == OVROverlay.OverlayShape.Cylinder)
		{
			Debug.LogWarning("Overlay shape " + this.currentOverlayShape + " is not supported on current platform");
		}
		for (int i = 0; i < 2; i++)
		{
			if (i < this.textures.Length)
			{
				if (this.textures[i] != this.cachedTextures[i])
				{
					this.cachedTextures[i] = this.textures[i];
					if (this.cachedTextures[i] != null)
					{
						this.texNativePtrs[i] = this.cachedTextures[i].GetNativeTexturePtr();
					}
				}
				if (this.currentOverlayShape == OVROverlay.OverlayShape.Cubemap && this.textures[i] != null && this.textures[i].GetType() != typeof(Cubemap))
				{
					Debug.LogError("Need Cubemap texture for cube map overlay");
					return;
				}
			}
		}
		if (this.cachedTextures[0] == null || this.texNativePtrs[0] == IntPtr.Zero)
		{
			return;
		}
		bool onTop = this.currentOverlayType == OVROverlay.OverlayType.Overlay;
		bool flag = false;
		Transform transform = base.transform;
		while (transform != null && !flag)
		{
			flag |= (transform == Camera.current.transform);
			transform = transform.parent;
		}
		OVRPose ovrpose = (!flag) ? base.transform.ToTrackingSpacePose() : base.transform.ToHeadSpacePose();
		Vector3 lossyScale = base.transform.lossyScale;
		for (int j = 0; j < 3; j++)
		{
			int index;
			lossyScale[index = j] = lossyScale[index] / Camera.current.transform.lossyScale[j];
		}
		if (this.currentOverlayShape == OVROverlay.OverlayShape.Cylinder)
		{
			float num = lossyScale.x / lossyScale.z / 3.14159274f * 180f;
			if (num > 180f)
			{
				Debug.LogError("Cylinder overlay's arc angle has to be below 180 degree, current arc angle is " + num + " degree.");
				return;
			}
		}
		bool flag2 = OVRPlugin.SetOverlayQuad(onTop, flag, this.texNativePtrs[0], this.texNativePtrs[1], IntPtr.Zero, ovrpose.flipZ().ToPosef(), lossyScale.ToVector3f(), this.layerIndex, (OVRPlugin.OverlayShape)this.currentOverlayShape);
		if (this.rend)
		{
			this.rend.enabled = !flag2;
		}
	}

	// Token: 0x040021BB RID: 8635
	private const int maxInstances = 15;

	// Token: 0x040021BC RID: 8636
	private static OVROverlay[] instances = new OVROverlay[15];

	// Token: 0x040021BD RID: 8637
	public OVROverlay.OverlayType currentOverlayType = OVROverlay.OverlayType.Overlay;

	// Token: 0x040021BE RID: 8638
	public OVROverlay.OverlayShape currentOverlayShape;

	// Token: 0x040021BF RID: 8639
	public Texture[] textures = new Texture[2];

	// Token: 0x040021C0 RID: 8640
	private Texture[] cachedTextures = new Texture[2];

	// Token: 0x040021C1 RID: 8641
	private IntPtr[] texNativePtrs = new IntPtr[]
	{
		IntPtr.Zero,
		IntPtr.Zero
	};

	// Token: 0x040021C2 RID: 8642
	private int layerIndex = -1;

	// Token: 0x040021C3 RID: 8643
	private Renderer rend;

	// Token: 0x0200069D RID: 1693
	public enum OverlayShape
	{
		// Token: 0x040021C5 RID: 8645
		Quad,
		// Token: 0x040021C6 RID: 8646
		Cylinder,
		// Token: 0x040021C7 RID: 8647
		Cubemap
	}

	// Token: 0x0200069E RID: 1694
	public enum OverlayType
	{
		// Token: 0x040021C9 RID: 8649
		None,
		// Token: 0x040021CA RID: 8650
		Underlay,
		// Token: 0x040021CB RID: 8651
		Overlay,
		// Token: 0x040021CC RID: 8652
		OverlayShowLod
	}
}
