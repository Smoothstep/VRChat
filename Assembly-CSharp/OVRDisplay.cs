using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x02000673 RID: 1651
public class OVRDisplay
{
	// Token: 0x060037DE RID: 14302 RVA: 0x0011CDB7 File Offset: 0x0011B1B7
	public OVRDisplay()
	{
		this.UpdateTextures();
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x0011CDD1 File Offset: 0x0011B1D1
	public void Update()
	{
		this.UpdateTextures();
	}

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x060037E0 RID: 14304 RVA: 0x0011CDDC File Offset: 0x0011B1DC
	// (remove) Token: 0x060037E1 RID: 14305 RVA: 0x0011CE14 File Offset: 0x0011B214
	public event Action RecenteredPose;

	// Token: 0x060037E2 RID: 14306 RVA: 0x0011CE4A File Offset: 0x0011B24A
	public void RecenterPose()
	{
		InputTracking.Recenter();
		if (this.RecenteredPose != null)
		{
			this.RecenteredPose();
		}
	}

	// Token: 0x170008C8 RID: 2248
	// (get) Token: 0x060037E3 RID: 14307 RVA: 0x0011CE68 File Offset: 0x0011B268
	public Vector3 acceleration
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return Vector3.zero;
			}
			return OVRPlugin.GetEyeAcceleration(OVRPlugin.Eye.None).ToOVRPose().position;
		}
	}

	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x060037E4 RID: 14308 RVA: 0x0011CE98 File Offset: 0x0011B298
	public Quaternion angularAcceleration
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return Quaternion.identity;
			}
			return OVRPlugin.GetEyeAcceleration(OVRPlugin.Eye.None).ToOVRPose().orientation;
		}
	}

	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x060037E5 RID: 14309 RVA: 0x0011CEC8 File Offset: 0x0011B2C8
	public Vector3 velocity
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return Vector3.zero;
			}
			return OVRPlugin.GetEyeVelocity(OVRPlugin.Eye.None).ToOVRPose().position;
		}
	}

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x060037E6 RID: 14310 RVA: 0x0011CEF8 File Offset: 0x0011B2F8
	public Quaternion angularVelocity
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return Quaternion.identity;
			}
			return OVRPlugin.GetEyeVelocity(OVRPlugin.Eye.None).ToOVRPose().orientation;
		}
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x0011CF28 File Offset: 0x0011B328
	public OVRDisplay.EyeRenderDesc GetEyeRenderDesc(VRNode eye)
	{
		return this.eyeDescs[(int)eye];
	}

	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x060037E8 RID: 14312 RVA: 0x0011CF3C File Offset: 0x0011B33C
	public OVRDisplay.LatencyData latency
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return default(OVRDisplay.LatencyData);
			}
			string latency = OVRPlugin.latency;
			Regex regex = new Regex("Render: ([0-9]+[.][0-9]+)ms, TimeWarp: ([0-9]+[.][0-9]+)ms, PostPresent: ([0-9]+[.][0-9]+)ms", RegexOptions.None);
			OVRDisplay.LatencyData result = default(OVRDisplay.LatencyData);
			Match match = regex.Match(latency);
			if (match.Success)
			{
				result.render = float.Parse(match.Groups[1].Value);
				result.timeWarp = float.Parse(match.Groups[2].Value);
				result.postPresent = float.Parse(match.Groups[3].Value);
			}
			return result;
		}
	}

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x060037E9 RID: 14313 RVA: 0x0011CFE8 File Offset: 0x0011B3E8
	public int recommendedMSAALevel
	{
		get
		{
			int num = OVRPlugin.recommendedMSAALevel;
			if (num == 1)
			{
				num = 0;
			}
			return num;
		}
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x0011D005 File Offset: 0x0011B405
	private void UpdateTextures()
	{
		this.ConfigureEyeDesc(VRNode.LeftEye);
		this.ConfigureEyeDesc(VRNode.RightEye);
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x0011D018 File Offset: 0x0011B418
	private void ConfigureEyeDesc(VRNode eye)
	{
		if (!OVRManager.isHmdPresent)
		{
			return;
		}
		OVRPlugin.Sizei eyeTextureSize = OVRPlugin.GetEyeTextureSize((OVRPlugin.Eye)eye);
		OVRPlugin.Frustumf eyeFrustum = OVRPlugin.GetEyeFrustum((OVRPlugin.Eye)eye);
		this.eyeDescs[(int)eye] = new OVRDisplay.EyeRenderDesc
		{
			resolution = new Vector2((float)eyeTextureSize.w, (float)eyeTextureSize.h),
			fov = 57.29578f * new Vector2(eyeFrustum.fovX, eyeFrustum.fovY)
		};
	}

	// Token: 0x04002062 RID: 8290
	private bool needsConfigureTexture;

	// Token: 0x04002063 RID: 8291
	private OVRDisplay.EyeRenderDesc[] eyeDescs = new OVRDisplay.EyeRenderDesc[2];

	// Token: 0x02000674 RID: 1652
	public struct EyeRenderDesc
	{
		// Token: 0x04002065 RID: 8293
		public Vector2 resolution;

		// Token: 0x04002066 RID: 8294
		public Vector2 fov;
	}

	// Token: 0x02000675 RID: 1653
	public struct LatencyData
	{
		// Token: 0x04002067 RID: 8295
		public float render;

		// Token: 0x04002068 RID: 8296
		public float timeWarp;

		// Token: 0x04002069 RID: 8297
		public float postPresent;

		// Token: 0x0400206A RID: 8298
		public float renderError;

		// Token: 0x0400206B RID: 8299
		public float timeWarpError;
	}
}
