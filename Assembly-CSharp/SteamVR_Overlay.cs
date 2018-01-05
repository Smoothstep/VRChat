using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

// Token: 0x02000C03 RID: 3075
public class SteamVR_Overlay : MonoBehaviour
{
	// Token: 0x17000D78 RID: 3448
	// (get) Token: 0x06005F3C RID: 24380 RVA: 0x00216BDE File Offset: 0x00214FDE
	// (set) Token: 0x06005F3D RID: 24381 RVA: 0x00216BE5 File Offset: 0x00214FE5
	public static SteamVR_Overlay instance { get; private set; }

	// Token: 0x17000D79 RID: 3449
	// (get) Token: 0x06005F3E RID: 24382 RVA: 0x00216BED File Offset: 0x00214FED
	public static string key
	{
		get
		{
			return "unity:" + Application.companyName + "." + Application.productName;
		}
	}

	// Token: 0x06005F3F RID: 24383 RVA: 0x00216C08 File Offset: 0x00215008
	private void OnEnable()
	{
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay != null)
		{
			EVROverlayError evroverlayError = overlay.CreateOverlay(SteamVR_Overlay.key, base.gameObject.name, ref this.handle);
			if (evroverlayError != EVROverlayError.None)
			{
				Debug.Log(overlay.GetOverlayErrorNameFromEnum(evroverlayError));
				base.enabled = false;
				return;
			}
		}
		SteamVR_Overlay.instance = this;
	}

	// Token: 0x06005F40 RID: 24384 RVA: 0x00216C60 File Offset: 0x00215060
	private void OnDisable()
	{
		if (this.handle != 0UL)
		{
			CVROverlay overlay = OpenVR.Overlay;
			if (overlay != null)
			{
				overlay.DestroyOverlay(this.handle);
			}
			this.handle = 0UL;
		}
		SteamVR_Overlay.instance = null;
	}

	// Token: 0x06005F41 RID: 24385 RVA: 0x00216CA4 File Offset: 0x002150A4
	public void UpdateOverlay()
	{
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay == null)
		{
			return;
		}
		if (this.texture != null)
		{
			EVROverlayError evroverlayError = overlay.ShowOverlay(this.handle);
			if ((evroverlayError == EVROverlayError.InvalidHandle || evroverlayError == EVROverlayError.UnknownOverlay) && overlay.FindOverlay(SteamVR_Overlay.key, ref this.handle) != EVROverlayError.None)
			{
				return;
			}
			Texture_t texture_t = default(Texture_t);
			texture_t.handle = this.texture.GetNativeTexturePtr();
			texture_t.eType = SteamVR.instance.textureType;
			texture_t.eColorSpace = EColorSpace.Auto;
			overlay.SetOverlayTexture(this.handle, ref texture_t);
			overlay.SetOverlayAlpha(this.handle, this.alpha);
			overlay.SetOverlayWidthInMeters(this.handle, this.scale);
			overlay.SetOverlayAutoCurveDistanceRangeInMeters(this.handle, this.curvedRange.x, this.curvedRange.y);
			VRTextureBounds_t vrtextureBounds_t = default(VRTextureBounds_t);
			vrtextureBounds_t.uMin = this.uvOffset.x * this.uvOffset.z;
			vrtextureBounds_t.vMin = (1f + this.uvOffset.y) * this.uvOffset.w;
			vrtextureBounds_t.uMax = (1f + this.uvOffset.x) * this.uvOffset.z;
			vrtextureBounds_t.vMax = this.uvOffset.y * this.uvOffset.w;
			overlay.SetOverlayTextureBounds(this.handle, ref vrtextureBounds_t);
			HmdVector2_t hmdVector2_t = default(HmdVector2_t);
			hmdVector2_t.v0 = this.mouseScale.x;
			hmdVector2_t.v1 = this.mouseScale.y;
			overlay.SetOverlayMouseScale(this.handle, ref hmdVector2_t);
			SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
			if (steamVR_Camera != null && steamVR_Camera.origin != null)
			{
				SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(steamVR_Camera.origin, base.transform);
				rigidTransform.pos.x = rigidTransform.pos.x / steamVR_Camera.origin.localScale.x;
				rigidTransform.pos.y = rigidTransform.pos.y / steamVR_Camera.origin.localScale.y;
				rigidTransform.pos.z = rigidTransform.pos.z / steamVR_Camera.origin.localScale.z;
				rigidTransform.pos.z = rigidTransform.pos.z + this.distance;
				HmdMatrix34_t hmdMatrix34_t = rigidTransform.ToHmdMatrix34();
				overlay.SetOverlayTransformAbsolute(this.handle, SteamVR_Render.instance.trackingSpace, ref hmdMatrix34_t);
			}
			overlay.SetOverlayInputMethod(this.handle, this.inputMethod);
			if (this.curved || this.antialias)
			{
				this.highquality = true;
			}
			if (this.highquality)
			{
				overlay.SetHighQualityOverlay(this.handle);
				overlay.SetOverlayFlag(this.handle, VROverlayFlags.Curved, this.curved);
				overlay.SetOverlayFlag(this.handle, VROverlayFlags.RGSS4X, this.antialias);
			}
			else if (overlay.GetHighQualityOverlay() == this.handle)
			{
				overlay.SetHighQualityOverlay(0UL);
			}
		}
		else
		{
			overlay.HideOverlay(this.handle);
		}
	}

	// Token: 0x06005F42 RID: 24386 RVA: 0x00216FF4 File Offset: 0x002153F4
	public bool PollNextEvent(ref VREvent_t pEvent)
	{
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay == null)
		{
			return false;
		}
		uint uncbVREvent = (uint)Marshal.SizeOf(typeof(VREvent_t));
		return overlay.PollNextOverlayEvent(this.handle, ref pEvent, uncbVREvent);
	}

	// Token: 0x06005F43 RID: 24387 RVA: 0x00217030 File Offset: 0x00215430
	public bool ComputeIntersection(Vector3 source, Vector3 direction, ref SteamVR_Overlay.IntersectionResults results)
	{
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay == null)
		{
			return false;
		}
		VROverlayIntersectionParams_t vroverlayIntersectionParams_t = default(VROverlayIntersectionParams_t);
		vroverlayIntersectionParams_t.eOrigin = SteamVR_Render.instance.trackingSpace;
		vroverlayIntersectionParams_t.vSource.v0 = source.x;
		vroverlayIntersectionParams_t.vSource.v1 = source.y;
		vroverlayIntersectionParams_t.vSource.v2 = -source.z;
		vroverlayIntersectionParams_t.vDirection.v0 = direction.x;
		vroverlayIntersectionParams_t.vDirection.v1 = direction.y;
		vroverlayIntersectionParams_t.vDirection.v2 = -direction.z;
		VROverlayIntersectionResults_t vroverlayIntersectionResults_t = default(VROverlayIntersectionResults_t);
		if (!overlay.ComputeOverlayIntersection(this.handle, ref vroverlayIntersectionParams_t, ref vroverlayIntersectionResults_t))
		{
			return false;
		}
		results.point = new Vector3(vroverlayIntersectionResults_t.vPoint.v0, vroverlayIntersectionResults_t.vPoint.v1, -vroverlayIntersectionResults_t.vPoint.v2);
		results.normal = new Vector3(vroverlayIntersectionResults_t.vNormal.v0, vroverlayIntersectionResults_t.vNormal.v1, -vroverlayIntersectionResults_t.vNormal.v2);
		results.UVs = new Vector2(vroverlayIntersectionResults_t.vUVs.v0, vroverlayIntersectionResults_t.vUVs.v1);
		results.distance = vroverlayIntersectionResults_t.fDistance;
		return true;
	}

	// Token: 0x040044FB RID: 17659
	public Texture texture;

	// Token: 0x040044FC RID: 17660
	public bool curved = true;

	// Token: 0x040044FD RID: 17661
	public bool antialias = true;

	// Token: 0x040044FE RID: 17662
	public bool highquality = true;

	// Token: 0x040044FF RID: 17663
	[Tooltip("Size of overlay view.")]
	public float scale = 3f;

	// Token: 0x04004500 RID: 17664
	[Tooltip("Distance from surface.")]
	public float distance = 1.25f;

	// Token: 0x04004501 RID: 17665
	[Tooltip("Opacity")]
	[Range(0f, 1f)]
	public float alpha = 1f;

	// Token: 0x04004502 RID: 17666
	public Vector4 uvOffset = new Vector4(0f, 0f, 1f, 1f);

	// Token: 0x04004503 RID: 17667
	public Vector2 mouseScale = new Vector2(1f, 1f);

	// Token: 0x04004504 RID: 17668
	public Vector2 curvedRange = new Vector2(1f, 2f);

	// Token: 0x04004505 RID: 17669
	public VROverlayInputMethod inputMethod;

	// Token: 0x04004507 RID: 17671
	private ulong handle;

	// Token: 0x02000C04 RID: 3076
	public struct IntersectionResults
	{
		// Token: 0x04004508 RID: 17672
		public Vector3 point;

		// Token: 0x04004509 RID: 17673
		public Vector3 normal;

		// Token: 0x0400450A RID: 17674
		public Vector2 UVs;

		// Token: 0x0400450B RID: 17675
		public float distance;
	}
}
