using System;
using System.Runtime.InteropServices;

// Token: 0x020006A2 RID: 1698
internal static class OVRPlugin
{
	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x06003900 RID: 14592 RVA: 0x00122D48 File Offset: 0x00121148
	public static Version version
	{
		get
		{
			if (OVRPlugin._version == null)
			{
				try
				{
					string text = OVRPlugin.OVRP_1_1_0.ovrp_GetVersion();
					if (text != null)
					{
						text = text.Split(new char[]
						{
							'-'
						})[0];
						OVRPlugin._version = new Version(text);
					}
					else
					{
						OVRPlugin._version = new Version(0, 0, 0);
					}
				}
				catch
				{
					OVRPlugin._version = new Version(0, 0, 0);
				}
				if (OVRPlugin._version == OVRPlugin.OVRP_0_5_0.version)
				{
					OVRPlugin._version = OVRPlugin.OVRP_0_1_0.version;
				}
				if (OVRPlugin._version < OVRPlugin.OVRP_1_3_0.version)
				{
					throw new PlatformNotSupportedException(string.Concat(new object[]
					{
						"Oculus Utilities version ",
						OVRPlugin.wrapperVersion,
						" is too new for OVRPlugin version ",
						OVRPlugin._version.ToString(),
						". Update to the latest version of Unity."
					}));
				}
			}
			return OVRPlugin._version;
		}
	}

	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x06003901 RID: 14593 RVA: 0x00122E40 File Offset: 0x00121240
	public static Version nativeSDKVersion
	{
		get
		{
			if (OVRPlugin._nativeSDKVersion == null)
			{
				try
				{
					string text = string.Empty;
					if (OVRPlugin.version >= OVRPlugin.OVRP_1_1_0.version)
					{
						text = OVRPlugin.OVRP_1_1_0.ovrp_GetNativeSDKVersion();
					}
					else
					{
						text = "0.0.0";
					}
					if (text != null)
					{
						text = text.Split(new char[]
						{
							'-'
						})[0];
						OVRPlugin._nativeSDKVersion = new Version(text);
					}
					else
					{
						OVRPlugin._nativeSDKVersion = new Version(0, 0, 0);
					}
				}
				catch
				{
					OVRPlugin._nativeSDKVersion = new Version(0, 0, 0);
				}
			}
			return OVRPlugin._nativeSDKVersion;
		}
	}

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x06003902 RID: 14594 RVA: 0x00122EEC File Offset: 0x001212EC
	public static bool initialized
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetInitialized() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x06003903 RID: 14595 RVA: 0x00122EF6 File Offset: 0x001212F6
	// (set) Token: 0x06003904 RID: 14596 RVA: 0x00122F16 File Offset: 0x00121316
	public static bool chromatic
	{
		get
		{
			return !(OVRPlugin.version >= OVRPlugin.OVRP_1_7_0.version) || OVRPlugin.OVRP_1_7_0.ovrp_GetAppChromaticCorrection() == OVRPlugin.Bool.True;
		}
		set
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_7_0.version)
			{
				OVRPlugin.OVRP_1_7_0.ovrp_SetAppChromaticCorrection(OVRPlugin.ToBool(value));
			}
		}
	}

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x06003905 RID: 14597 RVA: 0x00122F38 File Offset: 0x00121338
	// (set) Token: 0x06003906 RID: 14598 RVA: 0x00122F42 File Offset: 0x00121342
	public static bool monoscopic
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppMonoscopic() == OVRPlugin.Bool.True;
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetAppMonoscopic(OVRPlugin.ToBool(value));
		}
	}

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x06003907 RID: 14599 RVA: 0x00122F50 File Offset: 0x00121350
	// (set) Token: 0x06003908 RID: 14600 RVA: 0x00122F5A File Offset: 0x0012135A
	public static bool rotation
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetTrackingOrientationEnabled() == OVRPlugin.Bool.True;
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetTrackingOrientationEnabled(OVRPlugin.ToBool(value));
		}
	}

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x06003909 RID: 14601 RVA: 0x00122F68 File Offset: 0x00121368
	// (set) Token: 0x0600390A RID: 14602 RVA: 0x00122F72 File Offset: 0x00121372
	public static bool position
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetTrackingPositionEnabled() == OVRPlugin.Bool.True;
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetTrackingPositionEnabled(OVRPlugin.ToBool(value));
		}
	}

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x0600390B RID: 14603 RVA: 0x00122F80 File Offset: 0x00121380
	// (set) Token: 0x0600390C RID: 14604 RVA: 0x00122FA0 File Offset: 0x001213A0
	public static bool useIPDInPositionTracking
	{
		get
		{
			return !(OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version) || OVRPlugin.OVRP_1_6_0.ovrp_GetTrackingIPDEnabled() == OVRPlugin.Bool.True;
		}
		set
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
			{
				OVRPlugin.OVRP_1_6_0.ovrp_SetTrackingIPDEnabled(OVRPlugin.ToBool(value));
			}
		}
	}

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x0600390D RID: 14605 RVA: 0x00122FC2 File Offset: 0x001213C2
	public static bool positionSupported
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetTrackingPositionSupported() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x0600390E RID: 14606 RVA: 0x00122FCC File Offset: 0x001213CC
	public static bool positionTracked
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetNodePositionTracked(OVRPlugin.Node.EyeCenter) == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x0600390F RID: 14607 RVA: 0x00122FD7 File Offset: 0x001213D7
	public static bool powerSaving
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemPowerSavingMode() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x06003910 RID: 14608 RVA: 0x00122FE1 File Offset: 0x001213E1
	public static bool hmdPresent
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetNodePresent(OVRPlugin.Node.EyeCenter) == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x06003911 RID: 14609 RVA: 0x00122FEC File Offset: 0x001213EC
	public static bool userPresent
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetUserPresent() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x06003912 RID: 14610 RVA: 0x00122FF6 File Offset: 0x001213F6
	public static bool headphonesPresent
	{
		get
		{
			return OVRPlugin.OVRP_1_3_0.ovrp_GetSystemHeadphonesPresent() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x06003913 RID: 14611 RVA: 0x00123000 File Offset: 0x00121400
	public static int recommendedMSAALevel
	{
		get
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
			{
				return OVRPlugin.OVRP_1_6_0.ovrp_GetSystemRecommendedMSAALevel();
			}
			return 2;
		}
	}

	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x06003914 RID: 14612 RVA: 0x0012301D File Offset: 0x0012141D
	public static OVRPlugin.SystemRegion systemRegion
	{
		get
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_5_0.version)
			{
				return OVRPlugin.OVRP_1_5_0.ovrp_GetSystemRegion();
			}
			return OVRPlugin.SystemRegion.Unspecified;
		}
	}

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x06003915 RID: 14613 RVA: 0x0012303C File Offset: 0x0012143C
	public static string audioOutId
	{
		get
		{
			try
			{
				IntPtr intPtr = OVRPlugin.OVRP_1_1_0.ovrp_GetAudioOutId();
				if (intPtr != IntPtr.Zero)
				{
					OVRPlugin.GUID guid = (OVRPlugin.GUID)Marshal.PtrToStructure(intPtr, typeof(OVRPlugin.GUID));
					Guid guid2 = new Guid(guid.a, guid.b, guid.c, guid.d0, guid.d1, guid.d2, guid.d3, guid.d4, guid.d5, guid.d6, guid.d7);
					if (guid2 != OVRPlugin._cachedAudioOutGuid)
					{
						OVRPlugin._cachedAudioOutGuid = guid2;
						OVRPlugin._cachedAudioOutString = OVRPlugin._cachedAudioOutGuid.ToString();
					}
					return OVRPlugin._cachedAudioOutString;
				}
			}
			catch
			{
			}
			return string.Empty;
		}
	}

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x06003916 RID: 14614 RVA: 0x00123124 File Offset: 0x00121524
	public static string audioInId
	{
		get
		{
			try
			{
				IntPtr intPtr = OVRPlugin.OVRP_1_1_0.ovrp_GetAudioInId();
				if (intPtr != IntPtr.Zero)
				{
					OVRPlugin.GUID guid = (OVRPlugin.GUID)Marshal.PtrToStructure(intPtr, typeof(OVRPlugin.GUID));
					Guid guid2 = new Guid(guid.a, guid.b, guid.c, guid.d0, guid.d1, guid.d2, guid.d3, guid.d4, guid.d5, guid.d6, guid.d7);
					if (guid2 != OVRPlugin._cachedAudioInGuid)
					{
						OVRPlugin._cachedAudioInGuid = guid2;
						OVRPlugin._cachedAudioInString = OVRPlugin._cachedAudioInGuid.ToString();
					}
					return OVRPlugin._cachedAudioInString;
				}
			}
			catch
			{
			}
			return string.Empty;
		}
	}

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x06003917 RID: 14615 RVA: 0x0012320C File Offset: 0x0012160C
	public static bool hasVrFocus
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppHasVrFocus() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x06003918 RID: 14616 RVA: 0x00123216 File Offset: 0x00121616
	public static bool shouldQuit
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppShouldQuit() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x06003919 RID: 14617 RVA: 0x00123220 File Offset: 0x00121620
	public static bool shouldRecenter
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppShouldRecenter() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x0600391A RID: 14618 RVA: 0x0012322A File Offset: 0x0012162A
	public static string productName
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemProductName();
		}
	}

	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x0600391B RID: 14619 RVA: 0x00123231 File Offset: 0x00121631
	public static string latency
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppLatencyTimings();
		}
	}

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x0600391C RID: 14620 RVA: 0x00123238 File Offset: 0x00121638
	// (set) Token: 0x0600391D RID: 14621 RVA: 0x0012323F File Offset: 0x0012163F
	public static float eyeDepth
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetUserEyeDepth();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetUserEyeDepth(value);
		}
	}

	// Token: 0x1700090A RID: 2314
	// (get) Token: 0x0600391E RID: 14622 RVA: 0x00123248 File Offset: 0x00121648
	// (set) Token: 0x0600391F RID: 14623 RVA: 0x0012324F File Offset: 0x0012164F
	public static float eyeHeight
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetUserEyeHeight();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetUserEyeHeight(value);
		}
	}

	// Token: 0x1700090B RID: 2315
	// (get) Token: 0x06003920 RID: 14624 RVA: 0x00123258 File Offset: 0x00121658
	public static float batteryLevel
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemBatteryLevel();
		}
	}

	// Token: 0x1700090C RID: 2316
	// (get) Token: 0x06003921 RID: 14625 RVA: 0x0012325F File Offset: 0x0012165F
	public static float batteryTemperature
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemBatteryTemperature();
		}
	}

	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x06003922 RID: 14626 RVA: 0x00123266 File Offset: 0x00121666
	// (set) Token: 0x06003923 RID: 14627 RVA: 0x0012326D File Offset: 0x0012166D
	public static int cpuLevel
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemCpuLevel();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetSystemCpuLevel(value);
		}
	}

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x06003924 RID: 14628 RVA: 0x00123276 File Offset: 0x00121676
	// (set) Token: 0x06003925 RID: 14629 RVA: 0x0012327D File Offset: 0x0012167D
	public static int gpuLevel
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemGpuLevel();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetSystemGpuLevel(value);
		}
	}

	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x06003926 RID: 14630 RVA: 0x00123286 File Offset: 0x00121686
	// (set) Token: 0x06003927 RID: 14631 RVA: 0x0012328D File Offset: 0x0012168D
	public static int vsyncCount
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemVSyncCount();
		}
		set
		{
			OVRPlugin.OVRP_1_2_0.ovrp_SetSystemVSyncCount(value);
		}
	}

	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x06003928 RID: 14632 RVA: 0x00123296 File Offset: 0x00121696
	public static float systemVolume
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemVolume();
		}
	}

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x06003929 RID: 14633 RVA: 0x0012329D File Offset: 0x0012169D
	// (set) Token: 0x0600392A RID: 14634 RVA: 0x001232A4 File Offset: 0x001216A4
	public static float ipd
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetUserIPD();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetUserIPD(value);
		}
	}

	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x0600392B RID: 14635 RVA: 0x001232AD File Offset: 0x001216AD
	// (set) Token: 0x0600392C RID: 14636 RVA: 0x001232B7 File Offset: 0x001216B7
	public static bool occlusionMesh
	{
		get
		{
			return OVRPlugin.OVRP_1_3_0.ovrp_GetEyeOcclusionMeshEnabled() == OVRPlugin.Bool.True;
		}
		set
		{
			OVRPlugin.OVRP_1_3_0.ovrp_SetEyeOcclusionMeshEnabled(OVRPlugin.ToBool(value));
		}
	}

	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x0600392D RID: 14637 RVA: 0x001232C5 File Offset: 0x001216C5
	public static OVRPlugin.BatteryStatus batteryStatus
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemBatteryStatus();
		}
	}

	// Token: 0x0600392E RID: 14638 RVA: 0x001232CC File Offset: 0x001216CC
	public static OVRPlugin.Posef GetEyeVelocity(OVRPlugin.Eye eyeId)
	{
		return OVRPlugin.GetNodeVelocity((OVRPlugin.Node)eyeId, false);
	}

	// Token: 0x0600392F RID: 14639 RVA: 0x001232D5 File Offset: 0x001216D5
	public static OVRPlugin.Posef GetEyeAcceleration(OVRPlugin.Eye eyeId)
	{
		return OVRPlugin.GetNodeAcceleration((OVRPlugin.Node)eyeId, false);
	}

	// Token: 0x06003930 RID: 14640 RVA: 0x001232DE File Offset: 0x001216DE
	public static OVRPlugin.Frustumf GetEyeFrustum(OVRPlugin.Eye eyeId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodeFrustum((OVRPlugin.Node)eyeId);
	}

	// Token: 0x06003931 RID: 14641 RVA: 0x001232E6 File Offset: 0x001216E6
	public static OVRPlugin.Sizei GetEyeTextureSize(OVRPlugin.Eye eyeId)
	{
		return OVRPlugin.OVRP_0_1_0.ovrp_GetEyeTextureSize(eyeId);
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x001232EE File Offset: 0x001216EE
	public static OVRPlugin.Posef GetTrackerPose(OVRPlugin.Tracker trackerId)
	{
		return OVRPlugin.GetNodePose((OVRPlugin.Node)(trackerId + 5), false);
	}

	// Token: 0x06003933 RID: 14643 RVA: 0x001232F9 File Offset: 0x001216F9
	public static OVRPlugin.Frustumf GetTrackerFrustum(OVRPlugin.Tracker trackerId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodeFrustum((OVRPlugin.Node)(trackerId + 5));
	}

	// Token: 0x06003934 RID: 14644 RVA: 0x00123303 File Offset: 0x00121703
	public static bool ShowUI(OVRPlugin.PlatformUI ui)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_ShowSystemUI(ui) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003935 RID: 14645 RVA: 0x00123310 File Offset: 0x00121710
	public static bool SetOverlayQuad(bool onTop, bool headLocked, IntPtr leftTexture, IntPtr rightTexture, IntPtr device, OVRPlugin.Posef pose, OVRPlugin.Vector3f scale, int layerIndex = 0, OVRPlugin.OverlayShape shape = OVRPlugin.OverlayShape.Quad)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
		{
			uint num = 0u;
			if (onTop)
			{
				num |= 1u;
			}
			if (headLocked)
			{
				num |= 2u;
			}
			return shape != OVRPlugin.OverlayShape.Cylinder && shape != OVRPlugin.OverlayShape.Cubemap && OVRPlugin.OVRP_1_6_0.ovrp_SetOverlayQuad3(num, leftTexture, rightTexture, device, pose, scale, layerIndex) == OVRPlugin.Bool.True;
		}
		return layerIndex == 0 && OVRPlugin.OVRP_0_1_1.ovrp_SetOverlayQuad2(OVRPlugin.ToBool(onTop), OVRPlugin.ToBool(headLocked), leftTexture, device, pose, scale) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003936 RID: 14646 RVA: 0x00123391 File Offset: 0x00121791
	public static bool UpdateNodePhysicsPoses(int frameIndex, double predictionSeconds)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_Update2(0, frameIndex, predictionSeconds) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003937 RID: 14647 RVA: 0x001233B4 File Offset: 0x001217B4
	public static OVRPlugin.Posef GetNodePose(OVRPlugin.Node nodeId, bool usePhysicsPose)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && usePhysicsPose)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetNodePose2(0, nodeId);
		}
		return OVRPlugin.OVRP_0_1_2.ovrp_GetNodePose(nodeId);
	}

	// Token: 0x06003938 RID: 14648 RVA: 0x001233DE File Offset: 0x001217DE
	public static OVRPlugin.Posef GetNodeVelocity(OVRPlugin.Node nodeId, bool usePhysicsPose)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && usePhysicsPose)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetNodeVelocity2(0, nodeId);
		}
		return OVRPlugin.OVRP_0_1_3.ovrp_GetNodeVelocity(nodeId);
	}

	// Token: 0x06003939 RID: 14649 RVA: 0x00123408 File Offset: 0x00121808
	public static OVRPlugin.Posef GetNodeAcceleration(OVRPlugin.Node nodeId, bool usePhysicsPose)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && usePhysicsPose)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetNodeAcceleration2(0, nodeId);
		}
		return OVRPlugin.OVRP_0_1_3.ovrp_GetNodeAcceleration(nodeId);
	}

	// Token: 0x0600393A RID: 14650 RVA: 0x00123432 File Offset: 0x00121832
	public static bool GetNodePresent(OVRPlugin.Node nodeId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodePresent(nodeId) == OVRPlugin.Bool.True;
	}

	// Token: 0x0600393B RID: 14651 RVA: 0x0012343D File Offset: 0x0012183D
	public static bool GetNodeOrientationTracked(OVRPlugin.Node nodeId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodeOrientationTracked(nodeId) == OVRPlugin.Bool.True;
	}

	// Token: 0x0600393C RID: 14652 RVA: 0x00123448 File Offset: 0x00121848
	public static bool GetNodePositionTracked(OVRPlugin.Node nodeId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodePositionTracked(nodeId) == OVRPlugin.Bool.True;
	}

	// Token: 0x0600393D RID: 14653 RVA: 0x00123453 File Offset: 0x00121853
	public static OVRPlugin.ControllerState GetControllerState(uint controllerMask)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetControllerState(controllerMask);
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x0012345B File Offset: 0x0012185B
	public static bool SetControllerVibration(uint controllerMask, float frequency, float amplitude)
	{
		return OVRPlugin.OVRP_0_1_2.ovrp_SetControllerVibration(controllerMask, frequency, amplitude) == OVRPlugin.Bool.True;
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x00123468 File Offset: 0x00121868
	public static OVRPlugin.HapticsDesc GetControllerHapticsDesc(uint controllerMask)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
		{
			return OVRPlugin.OVRP_1_6_0.ovrp_GetControllerHapticsDesc(controllerMask);
		}
		return default(OVRPlugin.HapticsDesc);
	}

	// Token: 0x06003940 RID: 14656 RVA: 0x0012349C File Offset: 0x0012189C
	public static OVRPlugin.HapticsState GetControllerHapticsState(uint controllerMask)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
		{
			return OVRPlugin.OVRP_1_6_0.ovrp_GetControllerHapticsState(controllerMask);
		}
		return default(OVRPlugin.HapticsState);
	}

	// Token: 0x06003941 RID: 14657 RVA: 0x001234CD File Offset: 0x001218CD
	public static bool SetControllerHaptics(uint controllerMask, OVRPlugin.HapticsBuffer hapticsBuffer)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version && OVRPlugin.OVRP_1_6_0.ovrp_SetControllerHaptics(controllerMask, hapticsBuffer) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003942 RID: 14658 RVA: 0x001234EF File Offset: 0x001218EF
	public static float GetEyeRecommendedResolutionScale()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
		{
			return OVRPlugin.OVRP_1_6_0.ovrp_GetEyeRecommendedResolutionScale();
		}
		return 1f;
	}

	// Token: 0x06003943 RID: 14659 RVA: 0x00123510 File Offset: 0x00121910
	public static float GetAppCpuStartToGpuEndTime()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
		{
			return OVRPlugin.OVRP_1_6_0.ovrp_GetAppCpuStartToGpuEndTime();
		}
		return 0f;
	}

	// Token: 0x06003944 RID: 14660 RVA: 0x00123531 File Offset: 0x00121931
	public static bool GetBoundaryConfigured()
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryConfigured() == OVRPlugin.Bool.True;
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x00123554 File Offset: 0x00121954
	public static OVRPlugin.BoundaryTestResult TestBoundaryNode(OVRPlugin.Node nodeId, OVRPlugin.BoundaryType boundaryType)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_TestBoundaryNode(nodeId, boundaryType);
		}
		return default(OVRPlugin.BoundaryTestResult);
	}

	// Token: 0x06003946 RID: 14662 RVA: 0x00123588 File Offset: 0x00121988
	public static OVRPlugin.BoundaryTestResult TestBoundaryPoint(OVRPlugin.Vector3f point, OVRPlugin.BoundaryType boundaryType)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_TestBoundaryPoint(point, boundaryType);
		}
		return default(OVRPlugin.BoundaryTestResult);
	}

	// Token: 0x06003947 RID: 14663 RVA: 0x001235BA File Offset: 0x001219BA
	public static bool SetBoundaryLookAndFeel(OVRPlugin.BoundaryLookAndFeel lookAndFeel)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_SetBoundaryLookAndFeel(lookAndFeel) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003948 RID: 14664 RVA: 0x001235DB File Offset: 0x001219DB
	public static bool ResetBoundaryLookAndFeel()
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_ResetBoundaryLookAndFeel() == OVRPlugin.Bool.True;
	}

	// Token: 0x06003949 RID: 14665 RVA: 0x001235FC File Offset: 0x001219FC
	public static OVRPlugin.BoundaryGeometry GetBoundaryGeometry(OVRPlugin.BoundaryType boundaryType)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryGeometry(boundaryType);
		}
		return default(OVRPlugin.BoundaryGeometry);
	}

	// Token: 0x0600394A RID: 14666 RVA: 0x0012362D File Offset: 0x00121A2D
	public static bool GetBoundaryGeometry2(OVRPlugin.BoundaryType boundaryType, IntPtr points, ref int pointsCount)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetBoundaryGeometry2(boundaryType, points, ref pointsCount) == OVRPlugin.Bool.True;
		}
		pointsCount = 0;
		return false;
	}

	// Token: 0x0600394B RID: 14667 RVA: 0x00123654 File Offset: 0x00121A54
	public static OVRPlugin.AppPerfStats GetAppPerfStats()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetAppPerfStats();
		}
		return default(OVRPlugin.AppPerfStats);
	}

	// Token: 0x0600394C RID: 14668 RVA: 0x00123684 File Offset: 0x00121A84
	public static bool ResetAppPerfStats()
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version && OVRPlugin.OVRP_1_9_0.ovrp_ResetAppPerfStats() == OVRPlugin.Bool.True;
	}

	// Token: 0x0600394D RID: 14669 RVA: 0x001236A4 File Offset: 0x00121AA4
	public static OVRPlugin.Vector3f GetBoundaryDimensions(OVRPlugin.BoundaryType boundaryType)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryDimensions(boundaryType);
		}
		return default(OVRPlugin.Vector3f);
	}

	// Token: 0x0600394E RID: 14670 RVA: 0x001236D5 File Offset: 0x00121AD5
	public static bool GetBoundaryVisible()
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryVisible() == OVRPlugin.Bool.True;
	}

	// Token: 0x0600394F RID: 14671 RVA: 0x001236F5 File Offset: 0x00121AF5
	public static bool SetBoundaryVisible(bool value)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_SetBoundaryVisible(OVRPlugin.ToBool(value)) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003950 RID: 14672 RVA: 0x0012371B File Offset: 0x00121B1B
	public static OVRPlugin.SystemHeadset GetSystemHeadsetType()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetSystemHeadsetType();
		}
		return OVRPlugin.SystemHeadset.None;
	}

	// Token: 0x06003951 RID: 14673 RVA: 0x00123738 File Offset: 0x00121B38
	public static OVRPlugin.Controller GetActiveController()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetActiveController();
		}
		return OVRPlugin.Controller.None;
	}

	// Token: 0x06003952 RID: 14674 RVA: 0x00123755 File Offset: 0x00121B55
	public static OVRPlugin.Controller GetConnectedControllers()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetConnectedControllers();
		}
		return OVRPlugin.Controller.None;
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x00123772 File Offset: 0x00121B72
	private static OVRPlugin.Bool ToBool(bool b)
	{
		return (!b) ? OVRPlugin.Bool.False : OVRPlugin.Bool.True;
	}

	// Token: 0x06003954 RID: 14676 RVA: 0x00123781 File Offset: 0x00121B81
	public static OVRPlugin.TrackingOrigin GetTrackingOriginType()
	{
		return OVRPlugin.OVRP_1_0_0.ovrp_GetTrackingOriginType();
	}

	// Token: 0x06003955 RID: 14677 RVA: 0x00123788 File Offset: 0x00121B88
	public static bool SetTrackingOriginType(OVRPlugin.TrackingOrigin originType)
	{
		return OVRPlugin.OVRP_1_0_0.ovrp_SetTrackingOriginType(originType) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003956 RID: 14678 RVA: 0x00123793 File Offset: 0x00121B93
	public static OVRPlugin.Posef GetTrackingCalibratedOrigin()
	{
		return OVRPlugin.OVRP_1_0_0.ovrp_GetTrackingCalibratedOrigin();
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x0012379A File Offset: 0x00121B9A
	public static bool SetTrackingCalibratedOrigin()
	{
		return OVRPlugin.OVRP_1_2_0.ovrpi_SetTrackingCalibratedOrigin() == OVRPlugin.Bool.True;
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x001237A4 File Offset: 0x00121BA4
	public static bool RecenterTrackingOrigin(OVRPlugin.RecenterFlags flags)
	{
		return OVRPlugin.OVRP_1_0_0.ovrp_RecenterTrackingOrigin((uint)flags) == OVRPlugin.Bool.True;
	}

	// Token: 0x17000914 RID: 2324
	// (set) Token: 0x06003959 RID: 14681 RVA: 0x001237AF File Offset: 0x00121BAF
	internal static bool ignoreVrFocus
	{
		set
		{
			OVRPlugin.OVRP_1_2_1.ovrp_SetAppIgnoreVrFocus(OVRPlugin.ToBool(value));
		}
	}

	// Token: 0x040021E6 RID: 8678
	public static readonly Version wrapperVersion = OVRPlugin.OVRP_1_9_0.version;

	// Token: 0x040021E7 RID: 8679
	private static Version _version;

	// Token: 0x040021E8 RID: 8680
	private static Version _nativeSDKVersion;

	// Token: 0x040021E9 RID: 8681
	private const int OverlayShapeFlagShift = 4;

	// Token: 0x040021EA RID: 8682
	public const int AppPerfFrameStatsMaxCount = 5;

	// Token: 0x040021EB RID: 8683
	private static Guid _cachedAudioOutGuid;

	// Token: 0x040021EC RID: 8684
	private static string _cachedAudioOutString;

	// Token: 0x040021ED RID: 8685
	private static Guid _cachedAudioInGuid;

	// Token: 0x040021EE RID: 8686
	private static string _cachedAudioInString;

	// Token: 0x040021EF RID: 8687
	private const string pluginName = "OVRPlugin";

	// Token: 0x020006A3 RID: 1699
	private struct GUID
	{
		// Token: 0x040021F0 RID: 8688
		public int a;

		// Token: 0x040021F1 RID: 8689
		public short b;

		// Token: 0x040021F2 RID: 8690
		public short c;

		// Token: 0x040021F3 RID: 8691
		public byte d0;

		// Token: 0x040021F4 RID: 8692
		public byte d1;

		// Token: 0x040021F5 RID: 8693
		public byte d2;

		// Token: 0x040021F6 RID: 8694
		public byte d3;

		// Token: 0x040021F7 RID: 8695
		public byte d4;

		// Token: 0x040021F8 RID: 8696
		public byte d5;

		// Token: 0x040021F9 RID: 8697
		public byte d6;

		// Token: 0x040021FA RID: 8698
		public byte d7;
	}

	// Token: 0x020006A4 RID: 1700
	public enum Bool
	{
		// Token: 0x040021FC RID: 8700
		False,
		// Token: 0x040021FD RID: 8701
		True
	}

	// Token: 0x020006A5 RID: 1701
	public enum Eye
	{
		// Token: 0x040021FF RID: 8703
		None = -1,
		// Token: 0x04002200 RID: 8704
		Left,
		// Token: 0x04002201 RID: 8705
		Right,
		// Token: 0x04002202 RID: 8706
		Count
	}

	// Token: 0x020006A6 RID: 1702
	public enum Tracker
	{
		// Token: 0x04002204 RID: 8708
		None = -1,
		// Token: 0x04002205 RID: 8709
		Zero,
		// Token: 0x04002206 RID: 8710
		One,
		// Token: 0x04002207 RID: 8711
		Count
	}

	// Token: 0x020006A7 RID: 1703
	public enum Node
	{
		// Token: 0x04002209 RID: 8713
		None = -1,
		// Token: 0x0400220A RID: 8714
		EyeLeft,
		// Token: 0x0400220B RID: 8715
		EyeRight,
		// Token: 0x0400220C RID: 8716
		EyeCenter,
		// Token: 0x0400220D RID: 8717
		HandLeft,
		// Token: 0x0400220E RID: 8718
		HandRight,
		// Token: 0x0400220F RID: 8719
		TrackerZero,
		// Token: 0x04002210 RID: 8720
		TrackerOne,
		// Token: 0x04002211 RID: 8721
		TrackerTwo,
		// Token: 0x04002212 RID: 8722
		TrackerThree,
		// Token: 0x04002213 RID: 8723
		Head,
		// Token: 0x04002214 RID: 8724
		Count
	}

	// Token: 0x020006A8 RID: 1704
	public enum Controller
	{
		// Token: 0x04002216 RID: 8726
		None,
		// Token: 0x04002217 RID: 8727
		LTouch,
		// Token: 0x04002218 RID: 8728
		RTouch,
		// Token: 0x04002219 RID: 8729
		Touch,
		// Token: 0x0400221A RID: 8730
		Remote,
		// Token: 0x0400221B RID: 8731
		Gamepad = 16,
		// Token: 0x0400221C RID: 8732
		Touchpad = 134217728,
		// Token: 0x0400221D RID: 8733
		Active = -2147483648,
		// Token: 0x0400221E RID: 8734
		All = -1
	}

	// Token: 0x020006A9 RID: 1705
	public enum TrackingOrigin
	{
		// Token: 0x04002220 RID: 8736
		EyeLevel,
		// Token: 0x04002221 RID: 8737
		FloorLevel,
		// Token: 0x04002222 RID: 8738
		Count
	}

	// Token: 0x020006AA RID: 1706
	public enum RecenterFlags
	{
		// Token: 0x04002224 RID: 8740
		Default,
		// Token: 0x04002225 RID: 8741
		IgnoreAll = -2147483648,
		// Token: 0x04002226 RID: 8742
		Count
	}

	// Token: 0x020006AB RID: 1707
	public enum BatteryStatus
	{
		// Token: 0x04002228 RID: 8744
		Charging,
		// Token: 0x04002229 RID: 8745
		Discharging,
		// Token: 0x0400222A RID: 8746
		Full,
		// Token: 0x0400222B RID: 8747
		NotCharging,
		// Token: 0x0400222C RID: 8748
		Unknown
	}

	// Token: 0x020006AC RID: 1708
	public enum PlatformUI
	{
		// Token: 0x0400222E RID: 8750
		None = -1,
		// Token: 0x0400222F RID: 8751
		GlobalMenu,
		// Token: 0x04002230 RID: 8752
		ConfirmQuit,
		// Token: 0x04002231 RID: 8753
		GlobalMenuTutorial
	}

	// Token: 0x020006AD RID: 1709
	public enum SystemRegion
	{
		// Token: 0x04002233 RID: 8755
		Unspecified,
		// Token: 0x04002234 RID: 8756
		Japan,
		// Token: 0x04002235 RID: 8757
		China
	}

	// Token: 0x020006AE RID: 1710
	public enum SystemHeadset
	{
		// Token: 0x04002237 RID: 8759
		None,
		// Token: 0x04002238 RID: 8760
		GearVR_R320,
		// Token: 0x04002239 RID: 8761
		GearVR_R321,
		// Token: 0x0400223A RID: 8762
		GearVR_R322,
		// Token: 0x0400223B RID: 8763
		GearVR_R323,
		// Token: 0x0400223C RID: 8764
		Rift_DK1 = 4096,
		// Token: 0x0400223D RID: 8765
		Rift_DK2,
		// Token: 0x0400223E RID: 8766
		Rift_CV1
	}

	// Token: 0x020006AF RID: 1711
	public enum OverlayShape
	{
		// Token: 0x04002240 RID: 8768
		Quad,
		// Token: 0x04002241 RID: 8769
		Cylinder,
		// Token: 0x04002242 RID: 8770
		Cubemap
	}

	// Token: 0x020006B0 RID: 1712
	private enum OverlayFlag
	{
		// Token: 0x04002244 RID: 8772
		None,
		// Token: 0x04002245 RID: 8773
		OnTop,
		// Token: 0x04002246 RID: 8774
		HeadLocked,
		// Token: 0x04002247 RID: 8775
		ShapeFlag_Quad = 0,
		// Token: 0x04002248 RID: 8776
		ShapeFlag_Cylinder = 16,
		// Token: 0x04002249 RID: 8777
		ShapeFlag_Cubemap = 32,
		// Token: 0x0400224A RID: 8778
		ShapeFlagRangeMask = 240
	}

	// Token: 0x020006B1 RID: 1713
	public struct Vector2i
	{
		// Token: 0x0400224B RID: 8779
		public int x;

		// Token: 0x0400224C RID: 8780
		public int y;
	}

	// Token: 0x020006B2 RID: 1714
	public struct Vector2f
	{
		// Token: 0x0400224D RID: 8781
		public float x;

		// Token: 0x0400224E RID: 8782
		public float y;
	}

	// Token: 0x020006B3 RID: 1715
	public struct Vector3f
	{
		// Token: 0x0400224F RID: 8783
		public float x;

		// Token: 0x04002250 RID: 8784
		public float y;

		// Token: 0x04002251 RID: 8785
		public float z;
	}

	// Token: 0x020006B4 RID: 1716
	public struct Quatf
	{
		// Token: 0x04002252 RID: 8786
		public float x;

		// Token: 0x04002253 RID: 8787
		public float y;

		// Token: 0x04002254 RID: 8788
		public float z;

		// Token: 0x04002255 RID: 8789
		public float w;
	}

	// Token: 0x020006B5 RID: 1717
	public struct Posef
	{
		// Token: 0x04002256 RID: 8790
		public OVRPlugin.Quatf Orientation;

		// Token: 0x04002257 RID: 8791
		public OVRPlugin.Vector3f Position;
	}

	// Token: 0x020006B6 RID: 1718
	public struct ControllerState
	{
		// Token: 0x04002258 RID: 8792
		public uint ConnectedControllers;

		// Token: 0x04002259 RID: 8793
		public uint Buttons;

		// Token: 0x0400225A RID: 8794
		public uint Touches;

		// Token: 0x0400225B RID: 8795
		public uint NearTouches;

		// Token: 0x0400225C RID: 8796
		public float LIndexTrigger;

		// Token: 0x0400225D RID: 8797
		public float RIndexTrigger;

		// Token: 0x0400225E RID: 8798
		public float LHandTrigger;

		// Token: 0x0400225F RID: 8799
		public float RHandTrigger;

		// Token: 0x04002260 RID: 8800
		public OVRPlugin.Vector2f LThumbstick;

		// Token: 0x04002261 RID: 8801
		public OVRPlugin.Vector2f RThumbstick;
	}

	// Token: 0x020006B7 RID: 1719
	public struct HapticsBuffer
	{
		// Token: 0x04002262 RID: 8802
		public IntPtr Samples;

		// Token: 0x04002263 RID: 8803
		public int SamplesCount;
	}

	// Token: 0x020006B8 RID: 1720
	public struct HapticsState
	{
		// Token: 0x04002264 RID: 8804
		public int SamplesAvailable;

		// Token: 0x04002265 RID: 8805
		public int SamplesQueued;
	}

	// Token: 0x020006B9 RID: 1721
	public struct HapticsDesc
	{
		// Token: 0x04002266 RID: 8806
		public int SampleRateHz;

		// Token: 0x04002267 RID: 8807
		public int SampleSizeInBytes;

		// Token: 0x04002268 RID: 8808
		public int MinimumSafeSamplesQueued;

		// Token: 0x04002269 RID: 8809
		public int MinimumBufferSamplesCount;

		// Token: 0x0400226A RID: 8810
		public int OptimalBufferSamplesCount;

		// Token: 0x0400226B RID: 8811
		public int MaximumBufferSamplesCount;
	}

	// Token: 0x020006BA RID: 1722
	public struct AppPerfFrameStats
	{
		// Token: 0x0400226C RID: 8812
		public int HmdVsyncIndex;

		// Token: 0x0400226D RID: 8813
		public int AppFrameIndex;

		// Token: 0x0400226E RID: 8814
		public int AppDroppedFrameCount;

		// Token: 0x0400226F RID: 8815
		public float AppMotionToPhotonLatency;

		// Token: 0x04002270 RID: 8816
		public float AppQueueAheadTime;

		// Token: 0x04002271 RID: 8817
		public float AppCpuElapsedTime;

		// Token: 0x04002272 RID: 8818
		public float AppGpuElapsedTime;

		// Token: 0x04002273 RID: 8819
		public int CompositorFrameIndex;

		// Token: 0x04002274 RID: 8820
		public int CompositorDroppedFrameCount;

		// Token: 0x04002275 RID: 8821
		public float CompositorLatency;

		// Token: 0x04002276 RID: 8822
		public float CompositorCpuElapsedTime;

		// Token: 0x04002277 RID: 8823
		public float CompositorGpuElapsedTime;

		// Token: 0x04002278 RID: 8824
		public float CompositorCpuStartToGpuEndElapsedTime;

		// Token: 0x04002279 RID: 8825
		public float CompositorGpuEndToVsyncElapsedTime;
	}

	// Token: 0x020006BB RID: 1723
	public struct AppPerfStats
	{
		// Token: 0x0400227A RID: 8826
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public OVRPlugin.AppPerfFrameStats[] FrameStats;

		// Token: 0x0400227B RID: 8827
		public int FrameStatsCount;

		// Token: 0x0400227C RID: 8828
		public OVRPlugin.Bool AnyFrameStatsDropped;

		// Token: 0x0400227D RID: 8829
		public float AdaptiveGpuPerformanceScale;
	}

	// Token: 0x020006BC RID: 1724
	public struct Sizei
	{
		// Token: 0x0400227E RID: 8830
		public int w;

		// Token: 0x0400227F RID: 8831
		public int h;
	}

	// Token: 0x020006BD RID: 1725
	public struct Frustumf
	{
		// Token: 0x04002280 RID: 8832
		public float zNear;

		// Token: 0x04002281 RID: 8833
		public float zFar;

		// Token: 0x04002282 RID: 8834
		public float fovX;

		// Token: 0x04002283 RID: 8835
		public float fovY;
	}

	// Token: 0x020006BE RID: 1726
	public enum BoundaryType
	{
		// Token: 0x04002285 RID: 8837
		OuterBoundary = 1,
		// Token: 0x04002286 RID: 8838
		PlayArea = 256
	}

	// Token: 0x020006BF RID: 1727
	public struct BoundaryTestResult
	{
		// Token: 0x04002287 RID: 8839
		public OVRPlugin.Bool IsTriggering;

		// Token: 0x04002288 RID: 8840
		public float ClosestDistance;

		// Token: 0x04002289 RID: 8841
		public OVRPlugin.Vector3f ClosestPoint;

		// Token: 0x0400228A RID: 8842
		public OVRPlugin.Vector3f ClosestPointNormal;
	}

	// Token: 0x020006C0 RID: 1728
	public struct BoundaryLookAndFeel
	{
		// Token: 0x0400228B RID: 8843
		public OVRPlugin.Colorf Color;
	}

	// Token: 0x020006C1 RID: 1729
	public struct BoundaryGeometry
	{
		// Token: 0x0400228C RID: 8844
		public OVRPlugin.BoundaryType BoundaryType;

		// Token: 0x0400228D RID: 8845
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public OVRPlugin.Vector3f[] Points;

		// Token: 0x0400228E RID: 8846
		public int PointsCount;
	}

	// Token: 0x020006C2 RID: 1730
	public struct Colorf
	{
		// Token: 0x0400228F RID: 8847
		public float r;

		// Token: 0x04002290 RID: 8848
		public float g;

		// Token: 0x04002291 RID: 8849
		public float b;

		// Token: 0x04002292 RID: 8850
		public float a;
	}

	// Token: 0x020006C3 RID: 1731
	private static class OVRP_0_1_0
	{
		// Token: 0x0600395B RID: 14683
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Sizei ovrp_GetEyeTextureSize(OVRPlugin.Eye eyeId);

		// Token: 0x04002293 RID: 8851
		public static readonly Version version = new Version(0, 1, 0);
	}

	// Token: 0x020006C4 RID: 1732
	private static class OVRP_0_1_1
	{
		// Token: 0x0600395D RID: 14685
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetOverlayQuad2(OVRPlugin.Bool onTop, OVRPlugin.Bool headLocked, IntPtr texture, IntPtr device, OVRPlugin.Posef pose, OVRPlugin.Vector3f scale);

		// Token: 0x04002294 RID: 8852
		public static readonly Version version = new Version(0, 1, 1);
	}

	// Token: 0x020006C5 RID: 1733
	private static class OVRP_0_1_2
	{
		// Token: 0x0600395F RID: 14687
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodePose(OVRPlugin.Node nodeId);

		// Token: 0x06003960 RID: 14688
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetControllerVibration(uint controllerMask, float frequency, float amplitude);

		// Token: 0x04002295 RID: 8853
		public static readonly Version version = new Version(0, 1, 2);
	}

	// Token: 0x020006C6 RID: 1734
	private static class OVRP_0_1_3
	{
		// Token: 0x06003962 RID: 14690
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodeVelocity(OVRPlugin.Node nodeId);

		// Token: 0x06003963 RID: 14691
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodeAcceleration(OVRPlugin.Node nodeId);

		// Token: 0x04002296 RID: 8854
		public static readonly Version version = new Version(0, 1, 3);
	}

	// Token: 0x020006C7 RID: 1735
	private static class OVRP_0_5_0
	{
		// Token: 0x04002297 RID: 8855
		public static readonly Version version = new Version(0, 5, 0);
	}

	// Token: 0x020006C8 RID: 1736
	private static class OVRP_1_0_0
	{
		// Token: 0x06003966 RID: 14694
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.TrackingOrigin ovrp_GetTrackingOriginType();

		// Token: 0x06003967 RID: 14695
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetTrackingOriginType(OVRPlugin.TrackingOrigin originType);

		// Token: 0x06003968 RID: 14696
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetTrackingCalibratedOrigin();

		// Token: 0x06003969 RID: 14697
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_RecenterTrackingOrigin(uint flags);

		// Token: 0x04002298 RID: 8856
		public static readonly Version version = new Version(1, 0, 0);
	}

	// Token: 0x020006C9 RID: 1737
	private static class OVRP_1_1_0
	{
		// Token: 0x0600396B RID: 14699
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetInitialized();

		// Token: 0x0600396C RID: 14700
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetVersion")]
		private static extern IntPtr _ovrp_GetVersion();

		// Token: 0x0600396D RID: 14701 RVA: 0x00123823 File Offset: 0x00121C23
		public static string ovrp_GetVersion()
		{
			return Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetVersion());
		}

		// Token: 0x0600396E RID: 14702
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetNativeSDKVersion")]
		private static extern IntPtr _ovrp_GetNativeSDKVersion();

		// Token: 0x0600396F RID: 14703 RVA: 0x0012382F File Offset: 0x00121C2F
		public static string ovrp_GetNativeSDKVersion()
		{
			return Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetNativeSDKVersion());
		}

		// Token: 0x06003970 RID: 14704
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrp_GetAudioOutId();

		// Token: 0x06003971 RID: 14705
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrp_GetAudioInId();

		// Token: 0x06003972 RID: 14706
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetEyeTextureScale();

		// Token: 0x06003973 RID: 14707
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetEyeTextureScale(float value);

		// Token: 0x06003974 RID: 14708
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingOrientationSupported();

		// Token: 0x06003975 RID: 14709
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingOrientationEnabled();

		// Token: 0x06003976 RID: 14710
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetTrackingOrientationEnabled(OVRPlugin.Bool value);

		// Token: 0x06003977 RID: 14711
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingPositionSupported();

		// Token: 0x06003978 RID: 14712
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingPositionEnabled();

		// Token: 0x06003979 RID: 14713
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetTrackingPositionEnabled(OVRPlugin.Bool value);

		// Token: 0x0600397A RID: 14714
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetNodePresent(OVRPlugin.Node nodeId);

		// Token: 0x0600397B RID: 14715
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetNodeOrientationTracked(OVRPlugin.Node nodeId);

		// Token: 0x0600397C RID: 14716
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetNodePositionTracked(OVRPlugin.Node nodeId);

		// Token: 0x0600397D RID: 14717
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Frustumf ovrp_GetNodeFrustum(OVRPlugin.Node nodeId);

		// Token: 0x0600397E RID: 14718
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.ControllerState ovrp_GetControllerState(uint controllerMask);

		// Token: 0x0600397F RID: 14719
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemCpuLevel();

		// Token: 0x06003980 RID: 14720
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetSystemCpuLevel(int value);

		// Token: 0x06003981 RID: 14721
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemGpuLevel();

		// Token: 0x06003982 RID: 14722
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetSystemGpuLevel(int value);

		// Token: 0x06003983 RID: 14723
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetSystemPowerSavingMode();

		// Token: 0x06003984 RID: 14724
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemDisplayFrequency();

		// Token: 0x06003985 RID: 14725
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemVSyncCount();

		// Token: 0x06003986 RID: 14726
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemVolume();

		// Token: 0x06003987 RID: 14727
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.BatteryStatus ovrp_GetSystemBatteryStatus();

		// Token: 0x06003988 RID: 14728
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemBatteryLevel();

		// Token: 0x06003989 RID: 14729
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemBatteryTemperature();

		// Token: 0x0600398A RID: 14730
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetSystemProductName")]
		private static extern IntPtr _ovrp_GetSystemProductName();

		// Token: 0x0600398B RID: 14731 RVA: 0x0012383B File Offset: 0x00121C3B
		public static string ovrp_GetSystemProductName()
		{
			return Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetSystemProductName());
		}

		// Token: 0x0600398C RID: 14732
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_ShowSystemUI(OVRPlugin.PlatformUI ui);

		// Token: 0x0600398D RID: 14733
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppMonoscopic();

		// Token: 0x0600398E RID: 14734
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetAppMonoscopic(OVRPlugin.Bool value);

		// Token: 0x0600398F RID: 14735
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppHasVrFocus();

		// Token: 0x06003990 RID: 14736
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppShouldQuit();

		// Token: 0x06003991 RID: 14737
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppShouldRecenter();

		// Token: 0x06003992 RID: 14738
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetAppLatencyTimings")]
		private static extern IntPtr _ovrp_GetAppLatencyTimings();

		// Token: 0x06003993 RID: 14739 RVA: 0x00123847 File Offset: 0x00121C47
		public static string ovrp_GetAppLatencyTimings()
		{
			return Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetAppLatencyTimings());
		}

		// Token: 0x06003994 RID: 14740
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetUserPresent();

		// Token: 0x06003995 RID: 14741
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetUserIPD();

		// Token: 0x06003996 RID: 14742
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetUserIPD(float value);

		// Token: 0x06003997 RID: 14743
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetUserEyeDepth();

		// Token: 0x06003998 RID: 14744
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetUserEyeDepth(float value);

		// Token: 0x06003999 RID: 14745
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetUserEyeHeight();

		// Token: 0x0600399A RID: 14746
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetUserEyeHeight(float value);

		// Token: 0x04002299 RID: 8857
		public static readonly Version version = new Version(1, 1, 0);
	}

	// Token: 0x020006CA RID: 1738
	private static class OVRP_1_2_0
	{
		// Token: 0x0600399C RID: 14748
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetSystemVSyncCount(int vsyncCount);

		// Token: 0x0600399D RID: 14749
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrpi_SetTrackingCalibratedOrigin();

		// Token: 0x0400229A RID: 8858
		public static readonly Version version = new Version(1, 2, 0);
	}

	// Token: 0x020006CB RID: 1739
	private static class OVRP_1_2_1
	{
		// Token: 0x0600399F RID: 14751
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetAppIgnoreVrFocus(OVRPlugin.Bool value);

		// Token: 0x0400229B RID: 8859
		public static readonly Version version = new Version(1, 2, 1);
	}

	// Token: 0x020006CC RID: 1740
	private static class OVRP_1_3_0
	{
		// Token: 0x060039A1 RID: 14753
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetEyeOcclusionMeshEnabled();

		// Token: 0x060039A2 RID: 14754
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetEyeOcclusionMeshEnabled(OVRPlugin.Bool value);

		// Token: 0x060039A3 RID: 14755
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetSystemHeadphonesPresent();

		// Token: 0x0400229C RID: 8860
		public static readonly Version version = new Version(1, 3, 0);
	}

	// Token: 0x020006CD RID: 1741
	private static class OVRP_1_5_0
	{
		// Token: 0x060039A5 RID: 14757
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.SystemRegion ovrp_GetSystemRegion();

		// Token: 0x0400229D RID: 8861
		public static readonly Version version = new Version(1, 5, 0);
	}

	// Token: 0x020006CE RID: 1742
	private static class OVRP_1_6_0
	{
		// Token: 0x060039A7 RID: 14759
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingIPDEnabled();

		// Token: 0x060039A8 RID: 14760
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetTrackingIPDEnabled(OVRPlugin.Bool value);

		// Token: 0x060039A9 RID: 14761
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.HapticsDesc ovrp_GetControllerHapticsDesc(uint controllerMask);

		// Token: 0x060039AA RID: 14762
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.HapticsState ovrp_GetControllerHapticsState(uint controllerMask);

		// Token: 0x060039AB RID: 14763
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetControllerHaptics(uint controllerMask, OVRPlugin.HapticsBuffer hapticsBuffer);

		// Token: 0x060039AC RID: 14764
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetOverlayQuad3(uint flags, IntPtr textureLeft, IntPtr textureRight, IntPtr device, OVRPlugin.Posef pose, OVRPlugin.Vector3f scale, int layerIndex);

		// Token: 0x060039AD RID: 14765
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetEyeRecommendedResolutionScale();

		// Token: 0x060039AE RID: 14766
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetAppCpuStartToGpuEndTime();

		// Token: 0x060039AF RID: 14767
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemRecommendedMSAALevel();

		// Token: 0x0400229E RID: 8862
		public static readonly Version version = new Version(1, 6, 0);
	}

	// Token: 0x020006CF RID: 1743
	private static class OVRP_1_7_0
	{
		// Token: 0x060039B1 RID: 14769
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppChromaticCorrection();

		// Token: 0x060039B2 RID: 14770
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetAppChromaticCorrection(OVRPlugin.Bool value);

		// Token: 0x0400229F RID: 8863
		public static readonly Version version = new Version(1, 7, 0);
	}

	// Token: 0x020006D0 RID: 1744
	private static class OVRP_1_8_0
	{
		// Token: 0x060039B4 RID: 14772
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetBoundaryConfigured();

		// Token: 0x060039B5 RID: 14773
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.BoundaryTestResult ovrp_TestBoundaryNode(OVRPlugin.Node nodeId, OVRPlugin.BoundaryType boundaryType);

		// Token: 0x060039B6 RID: 14774
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.BoundaryTestResult ovrp_TestBoundaryPoint(OVRPlugin.Vector3f point, OVRPlugin.BoundaryType boundaryType);

		// Token: 0x060039B7 RID: 14775
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetBoundaryLookAndFeel(OVRPlugin.BoundaryLookAndFeel lookAndFeel);

		// Token: 0x060039B8 RID: 14776
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_ResetBoundaryLookAndFeel();

		// Token: 0x060039B9 RID: 14777
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.BoundaryGeometry ovrp_GetBoundaryGeometry(OVRPlugin.BoundaryType boundaryType);

		// Token: 0x060039BA RID: 14778
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Vector3f ovrp_GetBoundaryDimensions(OVRPlugin.BoundaryType boundaryType);

		// Token: 0x060039BB RID: 14779
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetBoundaryVisible();

		// Token: 0x060039BC RID: 14780
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetBoundaryVisible(OVRPlugin.Bool value);

		// Token: 0x060039BD RID: 14781
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_Update2(int stateId, int frameIndex, double predictionSeconds);

		// Token: 0x060039BE RID: 14782
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodePose2(int stateId, OVRPlugin.Node nodeId);

		// Token: 0x060039BF RID: 14783
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodeVelocity2(int stateId, OVRPlugin.Node nodeId);

		// Token: 0x060039C0 RID: 14784
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodeAcceleration2(int stateId, OVRPlugin.Node nodeId);

		// Token: 0x040022A0 RID: 8864
		public static readonly Version version = new Version(1, 8, 0);
	}

	// Token: 0x020006D1 RID: 1745
	private static class OVRP_1_9_0
	{
		// Token: 0x060039C2 RID: 14786
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.SystemHeadset ovrp_GetSystemHeadsetType();

		// Token: 0x060039C3 RID: 14787
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Controller ovrp_GetActiveController();

		// Token: 0x060039C4 RID: 14788
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Controller ovrp_GetConnectedControllers();

		// Token: 0x060039C5 RID: 14789
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetBoundaryGeometry2(OVRPlugin.BoundaryType boundaryType, IntPtr points, ref int pointsCount);

		// Token: 0x060039C6 RID: 14790
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.AppPerfStats ovrp_GetAppPerfStats();

		// Token: 0x060039C7 RID: 14791
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_ResetAppPerfStats();

		// Token: 0x040022A1 RID: 8865
		public static readonly Version version = new Version(1, 9, 0);
	}
}
