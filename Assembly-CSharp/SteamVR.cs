using System;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.VR;
using Valve.VR;

// Token: 0x02000BE7 RID: 3047
public class SteamVR : IDisposable
{
	// Token: 0x06005E58 RID: 24152 RVA: 0x002111C8 File Offset: 0x0020F5C8
	private SteamVR()
	{
		this.hmd = OpenVR.System;
		Debug.Log("Connected to " + this.hmd_TrackingSystemName + ":" + this.hmd_SerialNumber);
		this.compositor = OpenVR.Compositor;
		this.overlay = OpenVR.Overlay;
		uint num = 0u;
		uint num2 = 0u;
		this.hmd.GetRecommendedRenderTargetSize(ref num, ref num2);
		this.sceneWidth = num;
		this.sceneHeight = num2;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		this.hmd.GetProjectionRaw(EVREye.Eye_Left, ref num3, ref num4, ref num5, ref num6);
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		this.hmd.GetProjectionRaw(EVREye.Eye_Right, ref num7, ref num8, ref num9, ref num10);
		this.tanHalfFov = new Vector2(Mathf.Max(new float[]
		{
			-num3,
			num4,
			-num7,
			num8
		}), Mathf.Max(new float[]
		{
			-num5,
			num6,
			-num9,
			num10
		}));
		this.textureBounds = new VRTextureBounds_t[2];
		this.textureBounds[0].uMin = 0.5f + 0.5f * num3 / this.tanHalfFov.x;
		this.textureBounds[0].uMax = 0.5f + 0.5f * num4 / this.tanHalfFov.x;
		this.textureBounds[0].vMin = 0.5f - 0.5f * num6 / this.tanHalfFov.y;
		this.textureBounds[0].vMax = 0.5f - 0.5f * num5 / this.tanHalfFov.y;
		this.textureBounds[1].uMin = 0.5f + 0.5f * num7 / this.tanHalfFov.x;
		this.textureBounds[1].uMax = 0.5f + 0.5f * num8 / this.tanHalfFov.x;
		this.textureBounds[1].vMin = 0.5f - 0.5f * num10 / this.tanHalfFov.y;
		this.textureBounds[1].vMax = 0.5f - 0.5f * num9 / this.tanHalfFov.y;
		this.sceneWidth /= Mathf.Max(this.textureBounds[0].uMax - this.textureBounds[0].uMin, this.textureBounds[1].uMax - this.textureBounds[1].uMin);
		this.sceneHeight /= Mathf.Max(this.textureBounds[0].vMax - this.textureBounds[0].vMin, this.textureBounds[1].vMax - this.textureBounds[1].vMin);
		this.aspect = this.tanHalfFov.x / this.tanHalfFov.y;
		this.fieldOfView = 2f * Mathf.Atan(this.tanHalfFov.y) * 57.29578f;
		this.eyes = new SteamVR_Utils.RigidTransform[]
		{
			new SteamVR_Utils.RigidTransform(this.hmd.GetEyeToHeadTransform(EVREye.Eye_Left)),
			new SteamVR_Utils.RigidTransform(this.hmd.GetEyeToHeadTransform(EVREye.Eye_Right))
		};
		GraphicsDeviceType graphicsDeviceType = SystemInfo.graphicsDeviceType;
		switch (graphicsDeviceType)
		{
		case GraphicsDeviceType.OpenGLES2:
		case GraphicsDeviceType.OpenGLES3:
			break;
		default:
			if (graphicsDeviceType != GraphicsDeviceType.OpenGLCore)
			{
				if (graphicsDeviceType != GraphicsDeviceType.Vulkan)
				{
					this.textureType = ETextureType.DirectX;
					goto IL_433;
				}
				this.textureType = ETextureType.Vulkan;
				goto IL_433;
			}
			break;
		}
		this.textureType = ETextureType.OpenGL;
		IL_433:
		SteamVR_Events.Initializing.Listen(new UnityAction<bool>(this.OnInitializing));
		SteamVR_Events.Calibrating.Listen(new UnityAction<bool>(this.OnCalibrating));
		SteamVR_Events.OutOfRange.Listen(new UnityAction<bool>(this.OnOutOfRange));
		SteamVR_Events.DeviceConnected.Listen(new UnityAction<int, bool>(this.OnDeviceConnected));
		SteamVR_Events.NewPoses.Listen(new UnityAction<TrackedDevicePose_t[]>(this.OnNewPoses));
	}

	// Token: 0x17000D4B RID: 3403
	// (get) Token: 0x06005E59 RID: 24153 RVA: 0x00211676 File Offset: 0x0020FA76
	public static bool active
	{
		get
		{
			return SteamVR._instance != null;
		}
	}

	// Token: 0x17000D4C RID: 3404
	// (get) Token: 0x06005E5A RID: 24154 RVA: 0x00211683 File Offset: 0x0020FA83
	// (set) Token: 0x06005E5B RID: 24155 RVA: 0x0021169A File Offset: 0x0020FA9A
	public static bool enabled
	{
		get
		{
			if (!VRSettings.enabled)
			{
				SteamVR.enabled = false;
			}
			return SteamVR._enabled;
		}
		set
		{
			SteamVR._enabled = value;
			if (!SteamVR._enabled)
			{
				SteamVR.SafeDispose();
			}
		}
	}

	// Token: 0x17000D4D RID: 3405
	// (get) Token: 0x06005E5C RID: 24156 RVA: 0x002116B1 File Offset: 0x0020FAB1
	public static SteamVR instance
	{
		get
		{
			if (!SteamVR.enabled)
			{
				return null;
			}
			if (SteamVR._instance == null)
			{
				SteamVR._instance = SteamVR.CreateInstance();
				if (SteamVR._instance == null)
				{
					SteamVR._enabled = false;
				}
			}
			return SteamVR._instance;
		}
	}

	// Token: 0x17000D4E RID: 3406
	// (get) Token: 0x06005E5D RID: 24157 RVA: 0x002116E8 File Offset: 0x0020FAE8
	public static bool usingNativeSupport
	{
		get
		{
			return VRDevice.GetNativePtr() != IntPtr.Zero;
		}
	}

	// Token: 0x06005E5E RID: 24158 RVA: 0x002116FC File Offset: 0x0020FAFC
	private static SteamVR CreateInstance()
	{
		try
		{
			EVRInitError evrinitError = EVRInitError.None;
			if (!SteamVR.usingNativeSupport)
			{
				Debug.Log("OpenVR initialization failed.  Ensure 'Virtual Reality Supported' is checked in Player Settings, and OpenVR is added to the list of Virtual Reality SDKs.");
				return null;
			}
			OpenVR.GetGenericInterface("IVRCompositor_020", ref evrinitError);
			if (evrinitError != EVRInitError.None)
			{
				SteamVR.ReportError(evrinitError);
				return null;
			}
			OpenVR.GetGenericInterface("IVROverlay_016", ref evrinitError);
			if (evrinitError != EVRInitError.None)
			{
				SteamVR.ReportError(evrinitError);
				return null;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			return null;
		}
		return new SteamVR();
	}

	// Token: 0x06005E5F RID: 24159 RVA: 0x00211790 File Offset: 0x0020FB90
	private static void ReportError(EVRInitError error)
	{
		if (error != EVRInitError.None)
		{
			if (error != EVRInitError.Init_VRClientDLLNotFound)
			{
				if (error != EVRInitError.Driver_RuntimeOutOfDate)
				{
					if (error != EVRInitError.VendorSpecific_UnableToConnectToOculusRuntime)
					{
						Debug.Log(OpenVR.GetStringForHmdError(error));
					}
					else
					{
						Debug.Log("SteamVR Initialization Failed!  Make sure device is on, Oculus runtime is installed, and OVRService_*.exe is running.");
					}
				}
				else
				{
					Debug.Log("SteamVR Initialization Failed!  Make sure device's runtime is up to date.");
				}
			}
			else
			{
				Debug.Log("SteamVR drivers not found!  They can be installed via Steam under Library > Tools.  Visit http://steampowered.com to install Steam.");
			}
		}
	}

	// Token: 0x17000D4F RID: 3407
	// (get) Token: 0x06005E60 RID: 24160 RVA: 0x00211808 File Offset: 0x0020FC08
	// (set) Token: 0x06005E61 RID: 24161 RVA: 0x00211810 File Offset: 0x0020FC10
	public CVRSystem hmd { get; private set; }

	// Token: 0x17000D50 RID: 3408
	// (get) Token: 0x06005E62 RID: 24162 RVA: 0x00211819 File Offset: 0x0020FC19
	// (set) Token: 0x06005E63 RID: 24163 RVA: 0x00211821 File Offset: 0x0020FC21
	public CVRCompositor compositor { get; private set; }

	// Token: 0x17000D51 RID: 3409
	// (get) Token: 0x06005E64 RID: 24164 RVA: 0x0021182A File Offset: 0x0020FC2A
	// (set) Token: 0x06005E65 RID: 24165 RVA: 0x00211832 File Offset: 0x0020FC32
	public CVROverlay overlay { get; private set; }

	// Token: 0x17000D52 RID: 3410
	// (get) Token: 0x06005E66 RID: 24166 RVA: 0x0021183B File Offset: 0x0020FC3B
	// (set) Token: 0x06005E67 RID: 24167 RVA: 0x00211842 File Offset: 0x0020FC42
	public static bool initializing { get; private set; }

	// Token: 0x17000D53 RID: 3411
	// (get) Token: 0x06005E68 RID: 24168 RVA: 0x0021184A File Offset: 0x0020FC4A
	// (set) Token: 0x06005E69 RID: 24169 RVA: 0x00211851 File Offset: 0x0020FC51
	public static bool calibrating { get; private set; }

	// Token: 0x17000D54 RID: 3412
	// (get) Token: 0x06005E6A RID: 24170 RVA: 0x00211859 File Offset: 0x0020FC59
	// (set) Token: 0x06005E6B RID: 24171 RVA: 0x00211860 File Offset: 0x0020FC60
	public static bool outOfRange { get; private set; }

	// Token: 0x17000D55 RID: 3413
	// (get) Token: 0x06005E6C RID: 24172 RVA: 0x00211868 File Offset: 0x0020FC68
	// (set) Token: 0x06005E6D RID: 24173 RVA: 0x00211870 File Offset: 0x0020FC70
	public float sceneWidth { get; private set; }

	// Token: 0x17000D56 RID: 3414
	// (get) Token: 0x06005E6E RID: 24174 RVA: 0x00211879 File Offset: 0x0020FC79
	// (set) Token: 0x06005E6F RID: 24175 RVA: 0x00211881 File Offset: 0x0020FC81
	public float sceneHeight { get; private set; }

	// Token: 0x17000D57 RID: 3415
	// (get) Token: 0x06005E70 RID: 24176 RVA: 0x0021188A File Offset: 0x0020FC8A
	// (set) Token: 0x06005E71 RID: 24177 RVA: 0x00211892 File Offset: 0x0020FC92
	public float aspect { get; private set; }

	// Token: 0x17000D58 RID: 3416
	// (get) Token: 0x06005E72 RID: 24178 RVA: 0x0021189B File Offset: 0x0020FC9B
	// (set) Token: 0x06005E73 RID: 24179 RVA: 0x002118A3 File Offset: 0x0020FCA3
	public float fieldOfView { get; private set; }

	// Token: 0x17000D59 RID: 3417
	// (get) Token: 0x06005E74 RID: 24180 RVA: 0x002118AC File Offset: 0x0020FCAC
	// (set) Token: 0x06005E75 RID: 24181 RVA: 0x002118B4 File Offset: 0x0020FCB4
	public Vector2 tanHalfFov { get; private set; }

	// Token: 0x17000D5A RID: 3418
	// (get) Token: 0x06005E76 RID: 24182 RVA: 0x002118BD File Offset: 0x0020FCBD
	// (set) Token: 0x06005E77 RID: 24183 RVA: 0x002118C5 File Offset: 0x0020FCC5
	public VRTextureBounds_t[] textureBounds { get; private set; }

	// Token: 0x17000D5B RID: 3419
	// (get) Token: 0x06005E78 RID: 24184 RVA: 0x002118CE File Offset: 0x0020FCCE
	// (set) Token: 0x06005E79 RID: 24185 RVA: 0x002118D6 File Offset: 0x0020FCD6
	public SteamVR_Utils.RigidTransform[] eyes { get; private set; }

	// Token: 0x17000D5C RID: 3420
	// (get) Token: 0x06005E7A RID: 24186 RVA: 0x002118DF File Offset: 0x0020FCDF
	public string hmd_TrackingSystemName
	{
		get
		{
			return this.GetStringProperty(ETrackedDeviceProperty.Prop_TrackingSystemName_String, 0u);
		}
	}

	// Token: 0x17000D5D RID: 3421
	// (get) Token: 0x06005E7B RID: 24187 RVA: 0x002118ED File Offset: 0x0020FCED
	public string hmd_ModelNumber
	{
		get
		{
			return this.GetStringProperty(ETrackedDeviceProperty.Prop_ModelNumber_String, 0u);
		}
	}

	// Token: 0x17000D5E RID: 3422
	// (get) Token: 0x06005E7C RID: 24188 RVA: 0x002118FB File Offset: 0x0020FCFB
	public string hmd_SerialNumber
	{
		get
		{
			return this.GetStringProperty(ETrackedDeviceProperty.Prop_SerialNumber_String, 0u);
		}
	}

	// Token: 0x17000D5F RID: 3423
	// (get) Token: 0x06005E7D RID: 24189 RVA: 0x00211909 File Offset: 0x0020FD09
	public float hmd_SecondsFromVsyncToPhotons
	{
		get
		{
			return this.GetFloatProperty(ETrackedDeviceProperty.Prop_SecondsFromVsyncToPhotons_Float, 0u);
		}
	}

	// Token: 0x17000D60 RID: 3424
	// (get) Token: 0x06005E7E RID: 24190 RVA: 0x00211917 File Offset: 0x0020FD17
	public float hmd_DisplayFrequency
	{
		get
		{
			return this.GetFloatProperty(ETrackedDeviceProperty.Prop_DisplayFrequency_Float, 0u);
		}
	}

	// Token: 0x06005E7F RID: 24191 RVA: 0x00211928 File Offset: 0x0020FD28
	public string GetTrackedDeviceString(uint deviceId)
	{
		ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
		uint stringTrackedDeviceProperty = this.hmd.GetStringTrackedDeviceProperty(deviceId, ETrackedDeviceProperty.Prop_AttachedDeviceId_String, null, 0u, ref etrackedPropertyError);
		if (stringTrackedDeviceProperty > 1u)
		{
			StringBuilder stringBuilder = new StringBuilder((int)stringTrackedDeviceProperty);
			this.hmd.GetStringTrackedDeviceProperty(deviceId, ETrackedDeviceProperty.Prop_AttachedDeviceId_String, stringBuilder, stringTrackedDeviceProperty, ref etrackedPropertyError);
			return stringBuilder.ToString();
		}
		return null;
	}

	// Token: 0x06005E80 RID: 24192 RVA: 0x0021197C File Offset: 0x0020FD7C
	public string GetStringProperty(ETrackedDeviceProperty prop, uint deviceId = 0u)
	{
		ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
		uint stringTrackedDeviceProperty = this.hmd.GetStringTrackedDeviceProperty(deviceId, prop, null, 0u, ref etrackedPropertyError);
		if (stringTrackedDeviceProperty > 1u)
		{
			StringBuilder stringBuilder = new StringBuilder((int)stringTrackedDeviceProperty);
			this.hmd.GetStringTrackedDeviceProperty(deviceId, prop, stringBuilder, stringTrackedDeviceProperty, ref etrackedPropertyError);
			return stringBuilder.ToString();
		}
		return (etrackedPropertyError == ETrackedPropertyError.TrackedProp_Success) ? "<unknown>" : etrackedPropertyError.ToString();
	}

	// Token: 0x06005E81 RID: 24193 RVA: 0x002119E4 File Offset: 0x0020FDE4
	public float GetFloatProperty(ETrackedDeviceProperty prop, uint deviceId = 0u)
	{
		ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
		return this.hmd.GetFloatTrackedDeviceProperty(deviceId, prop, ref etrackedPropertyError);
	}

	// Token: 0x06005E82 RID: 24194 RVA: 0x00211A02 File Offset: 0x0020FE02
	private void OnInitializing(bool initializing)
	{
		SteamVR.initializing = initializing;
	}

	// Token: 0x06005E83 RID: 24195 RVA: 0x00211A0A File Offset: 0x0020FE0A
	private void OnCalibrating(bool calibrating)
	{
		SteamVR.calibrating = calibrating;
	}

	// Token: 0x06005E84 RID: 24196 RVA: 0x00211A12 File Offset: 0x0020FE12
	private void OnOutOfRange(bool outOfRange)
	{
		SteamVR.outOfRange = outOfRange;
	}

	// Token: 0x06005E85 RID: 24197 RVA: 0x00211A1A File Offset: 0x0020FE1A
	private void OnDeviceConnected(int i, bool connected)
	{
		SteamVR.connected[i] = connected;
	}

	// Token: 0x06005E86 RID: 24198 RVA: 0x00211A24 File Offset: 0x0020FE24
	private void OnNewPoses(TrackedDevicePose_t[] poses)
	{
		this.eyes[0] = new SteamVR_Utils.RigidTransform(this.hmd.GetEyeToHeadTransform(EVREye.Eye_Left));
		this.eyes[1] = new SteamVR_Utils.RigidTransform(this.hmd.GetEyeToHeadTransform(EVREye.Eye_Right));
		for (int i = 0; i < poses.Length; i++)
		{
			bool bDeviceIsConnected = poses[i].bDeviceIsConnected;
			if (bDeviceIsConnected != SteamVR.connected[i])
			{
				SteamVR_Events.DeviceConnected.Send(i, bDeviceIsConnected);
			}
		}
		if ((long)poses.Length > 0L)
		{
			ETrackingResult eTrackingResult = poses[(int)((UIntPtr)0)].eTrackingResult;
			bool flag = eTrackingResult == ETrackingResult.Uninitialized;
			if (flag != SteamVR.initializing)
			{
				SteamVR_Events.Initializing.Send(flag);
			}
			bool flag2 = eTrackingResult == ETrackingResult.Calibrating_InProgress || eTrackingResult == ETrackingResult.Calibrating_OutOfRange;
			if (flag2 != SteamVR.calibrating)
			{
				SteamVR_Events.Calibrating.Send(flag2);
			}
			bool flag3 = eTrackingResult == ETrackingResult.Running_OutOfRange || eTrackingResult == ETrackingResult.Calibrating_OutOfRange;
			if (flag3 != SteamVR.outOfRange)
			{
				SteamVR_Events.OutOfRange.Send(flag3);
			}
		}
	}

	// Token: 0x06005E87 RID: 24199 RVA: 0x00211B3C File Offset: 0x0020FF3C
	~SteamVR()
	{
		this.Dispose(false);
	}

	// Token: 0x06005E88 RID: 24200 RVA: 0x00211B6C File Offset: 0x0020FF6C
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06005E89 RID: 24201 RVA: 0x00211B7C File Offset: 0x0020FF7C
	private void Dispose(bool disposing)
	{
		SteamVR_Events.Initializing.Remove(new UnityAction<bool>(this.OnInitializing));
		SteamVR_Events.Calibrating.Remove(new UnityAction<bool>(this.OnCalibrating));
		SteamVR_Events.OutOfRange.Remove(new UnityAction<bool>(this.OnOutOfRange));
		SteamVR_Events.DeviceConnected.Remove(new UnityAction<int, bool>(this.OnDeviceConnected));
		SteamVR_Events.NewPoses.Remove(new UnityAction<TrackedDevicePose_t[]>(this.OnNewPoses));
		SteamVR._instance = null;
	}

	// Token: 0x06005E8A RID: 24202 RVA: 0x00211BFD File Offset: 0x0020FFFD
	public static void SafeDispose()
	{
		if (SteamVR._instance != null)
		{
			SteamVR._instance.Dispose();
		}
	}

	// Token: 0x04004435 RID: 17461
	private static bool _enabled = true;

	// Token: 0x04004436 RID: 17462
	private static SteamVR _instance;

	// Token: 0x0400443D RID: 17469
	public static bool[] connected = new bool[16];

	// Token: 0x04004445 RID: 17477
	public ETextureType textureType;
}
