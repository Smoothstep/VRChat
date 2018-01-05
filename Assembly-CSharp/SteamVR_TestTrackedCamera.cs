using System;
using UnityEngine;
using Valve.VR;

// Token: 0x02000B8F RID: 2959
public class SteamVR_TestTrackedCamera : MonoBehaviour
{
	// Token: 0x06005C03 RID: 23555 RVA: 0x0020217C File Offset: 0x0020057C
	private void OnEnable()
	{
		SteamVR_TrackedCamera.VideoStreamTexture videoStreamTexture = SteamVR_TrackedCamera.Source(this.undistorted, 0);
		videoStreamTexture.Acquire();
		if (!videoStreamTexture.hasCamera)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06005C04 RID: 23556 RVA: 0x002021B0 File Offset: 0x002005B0
	private void OnDisable()
	{
		this.material.mainTexture = null;
		SteamVR_TrackedCamera.VideoStreamTexture videoStreamTexture = SteamVR_TrackedCamera.Source(this.undistorted, 0);
		videoStreamTexture.Release();
	}

	// Token: 0x06005C05 RID: 23557 RVA: 0x002021E0 File Offset: 0x002005E0
	private void Update()
	{
		SteamVR_TrackedCamera.VideoStreamTexture videoStreamTexture = SteamVR_TrackedCamera.Source(this.undistorted, 0);
		Texture2D texture = videoStreamTexture.texture;
		if (texture == null)
		{
			return;
		}
		this.material.mainTexture = texture;
		float num = (float)texture.width / (float)texture.height;
		if (this.cropped)
		{
			VRTextureBounds_t frameBounds = videoStreamTexture.frameBounds;
			this.material.mainTextureOffset = new Vector2(frameBounds.uMin, frameBounds.vMin);
			float num2 = frameBounds.uMax - frameBounds.uMin;
			float num3 = frameBounds.vMax - frameBounds.vMin;
			this.material.mainTextureScale = new Vector2(num2, num3);
			num *= Mathf.Abs(num2 / num3);
		}
		else
		{
			this.material.mainTextureOffset = Vector2.zero;
			this.material.mainTextureScale = new Vector2(1f, -1f);
		}
		this.target.localScale = new Vector3(1f, 1f / num, 1f);
		if (videoStreamTexture.hasTracking)
		{
			SteamVR_Utils.RigidTransform transform = videoStreamTexture.transform;
			this.target.localPosition = transform.pos;
			this.target.localRotation = transform.rot;
		}
	}

	// Token: 0x04004199 RID: 16793
	public Material material;

	// Token: 0x0400419A RID: 16794
	public Transform target;

	// Token: 0x0400419B RID: 16795
	public bool undistorted = true;

	// Token: 0x0400419C RID: 16796
	public bool cropped = true;
}
