using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VR;

// Token: 0x0200069A RID: 1690
public class OVRManager : MonoBehaviour
{
	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x060038A6 RID: 14502 RVA: 0x0012151A File Offset: 0x0011F91A
	// (set) Token: 0x060038A7 RID: 14503 RVA: 0x00121521 File Offset: 0x0011F921
	public static OVRManager instance { get; private set; }

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x060038A8 RID: 14504 RVA: 0x00121529 File Offset: 0x0011F929
	// (set) Token: 0x060038A9 RID: 14505 RVA: 0x00121530 File Offset: 0x0011F930
	public static OVRDisplay display { get; private set; }

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x060038AA RID: 14506 RVA: 0x00121538 File Offset: 0x0011F938
	// (set) Token: 0x060038AB RID: 14507 RVA: 0x0012153F File Offset: 0x0011F93F
	public static OVRTracker tracker { get; private set; }

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x060038AC RID: 14508 RVA: 0x00121547 File Offset: 0x0011F947
	// (set) Token: 0x060038AD RID: 14509 RVA: 0x0012154E File Offset: 0x0011F94E
	public static OVRBoundary boundary { get; private set; }

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x060038AE RID: 14510 RVA: 0x00121556 File Offset: 0x0011F956
	public static OVRProfile profile
	{
		get
		{
			if (OVRManager._profile == null)
			{
				OVRManager._profile = new OVRProfile();
			}
			return OVRManager._profile;
		}
	}

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x060038AF RID: 14511 RVA: 0x00121577 File Offset: 0x0011F977
	// (set) Token: 0x060038B0 RID: 14512 RVA: 0x0012157F File Offset: 0x0011F97F
	private bool paused
	{
		get
		{
			return this._isPaused;
		}
		set
		{
			if (value == this._isPaused)
			{
				return;
			}
			this._isPaused = value;
		}
	}

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x060038B1 RID: 14513 RVA: 0x00121598 File Offset: 0x0011F998
	// (remove) Token: 0x060038B2 RID: 14514 RVA: 0x001215CC File Offset: 0x0011F9CC
	public static event Action HMDAcquired;

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x060038B3 RID: 14515 RVA: 0x00121600 File Offset: 0x0011FA00
	// (remove) Token: 0x060038B4 RID: 14516 RVA: 0x00121634 File Offset: 0x0011FA34
	public static event Action HMDLost;

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x060038B5 RID: 14517 RVA: 0x00121668 File Offset: 0x0011FA68
	// (remove) Token: 0x060038B6 RID: 14518 RVA: 0x0012169C File Offset: 0x0011FA9C
	public static event Action HMDMounted;

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x060038B7 RID: 14519 RVA: 0x001216D0 File Offset: 0x0011FAD0
	// (remove) Token: 0x060038B8 RID: 14520 RVA: 0x00121704 File Offset: 0x0011FB04
	public static event Action HMDUnmounted;

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x060038B9 RID: 14521 RVA: 0x00121738 File Offset: 0x0011FB38
	// (remove) Token: 0x060038BA RID: 14522 RVA: 0x0012176C File Offset: 0x0011FB6C
	public static event Action VrFocusAcquired;

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x060038BB RID: 14523 RVA: 0x001217A0 File Offset: 0x0011FBA0
	// (remove) Token: 0x060038BC RID: 14524 RVA: 0x001217D4 File Offset: 0x0011FBD4
	public static event Action VrFocusLost;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x060038BD RID: 14525 RVA: 0x00121808 File Offset: 0x0011FC08
	// (remove) Token: 0x060038BE RID: 14526 RVA: 0x0012183C File Offset: 0x0011FC3C
	public static event Action AudioOutChanged;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x060038BF RID: 14527 RVA: 0x00121870 File Offset: 0x0011FC70
	// (remove) Token: 0x060038C0 RID: 14528 RVA: 0x001218A4 File Offset: 0x0011FCA4
	public static event Action AudioInChanged;

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x060038C1 RID: 14529 RVA: 0x001218D8 File Offset: 0x0011FCD8
	// (remove) Token: 0x060038C2 RID: 14530 RVA: 0x0012190C File Offset: 0x0011FD0C
	public static event Action TrackingAcquired;

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x060038C3 RID: 14531 RVA: 0x00121940 File Offset: 0x0011FD40
	// (remove) Token: 0x060038C4 RID: 14532 RVA: 0x00121974 File Offset: 0x0011FD74
	public static event Action TrackingLost;

	// Token: 0x14000046 RID: 70
	// (add) Token: 0x060038C5 RID: 14533 RVA: 0x001219A8 File Offset: 0x0011FDA8
	// (remove) Token: 0x060038C6 RID: 14534 RVA: 0x001219DC File Offset: 0x0011FDDC
	[Obsolete]
	public static event Action HSWDismissed;

	// Token: 0x170008E0 RID: 2272
	// (get) Token: 0x060038C7 RID: 14535 RVA: 0x00121A10 File Offset: 0x0011FE10
	// (set) Token: 0x060038C8 RID: 14536 RVA: 0x00121A31 File Offset: 0x0011FE31
	public static bool isHmdPresent
	{
		get
		{
			if (!OVRManager._isHmdPresentCached)
			{
				OVRManager._isHmdPresentCached = true;
				OVRManager._isHmdPresent = OVRPlugin.hmdPresent;
			}
			return OVRManager._isHmdPresent;
		}
		private set
		{
			OVRManager._isHmdPresentCached = true;
			OVRManager._isHmdPresent = value;
		}
	}

	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x060038C9 RID: 14537 RVA: 0x00121A3F File Offset: 0x0011FE3F
	public static string audioOutId
	{
		get
		{
			return OVRPlugin.audioOutId;
		}
	}

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x060038CA RID: 14538 RVA: 0x00121A46 File Offset: 0x0011FE46
	public static string audioInId
	{
		get
		{
			return OVRPlugin.audioInId;
		}
	}

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x060038CB RID: 14539 RVA: 0x00121A4D File Offset: 0x0011FE4D
	// (set) Token: 0x060038CC RID: 14540 RVA: 0x00121A6E File Offset: 0x0011FE6E
	public static bool hasVrFocus
	{
		get
		{
			if (!OVRManager._hasVrFocusCached)
			{
				OVRManager._hasVrFocusCached = true;
				OVRManager._hasVrFocus = OVRPlugin.hasVrFocus;
			}
			return OVRManager._hasVrFocus;
		}
		private set
		{
			OVRManager._hasVrFocusCached = true;
			OVRManager._hasVrFocus = value;
		}
	}

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x060038CD RID: 14541 RVA: 0x00121A7C File Offset: 0x0011FE7C
	[Obsolete]
	public static bool isHSWDisplayed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060038CE RID: 14542 RVA: 0x00121A7F File Offset: 0x0011FE7F
	[Obsolete]
	public static void DismissHSWDisplay()
	{
	}

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x060038CF RID: 14543 RVA: 0x00121A81 File Offset: 0x0011FE81
	// (set) Token: 0x060038D0 RID: 14544 RVA: 0x00121A94 File Offset: 0x0011FE94
	public bool chromatic
	{
		get
		{
			return OVRManager.isHmdPresent && OVRPlugin.chromatic;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.chromatic = value;
		}
	}

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x060038D1 RID: 14545 RVA: 0x00121AA7 File Offset: 0x0011FEA7
	// (set) Token: 0x060038D2 RID: 14546 RVA: 0x00121ABA File Offset: 0x0011FEBA
	public bool monoscopic
	{
		get
		{
			return !OVRManager.isHmdPresent || OVRPlugin.monoscopic;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.monoscopic = value;
		}
	}

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x060038D3 RID: 14547 RVA: 0x00121ACD File Offset: 0x0011FECD
	// (set) Token: 0x060038D4 RID: 14548 RVA: 0x00121AE0 File Offset: 0x0011FEE0
	public int vsyncCount
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 1;
			}
			return OVRPlugin.vsyncCount;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.vsyncCount = value;
		}
	}

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x060038D5 RID: 14549 RVA: 0x00121AF3 File Offset: 0x0011FEF3
	public static float batteryLevel
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 1f;
			}
			return OVRPlugin.batteryLevel;
		}
	}

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x060038D6 RID: 14550 RVA: 0x00121B0A File Offset: 0x0011FF0A
	public static float batteryTemperature
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 0f;
			}
			return OVRPlugin.batteryTemperature;
		}
	}

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x060038D7 RID: 14551 RVA: 0x00121B21 File Offset: 0x0011FF21
	public static int batteryStatus
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return -1;
			}
			return (int)OVRPlugin.batteryStatus;
		}
	}

	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x060038D8 RID: 14552 RVA: 0x00121B34 File Offset: 0x0011FF34
	public static float volumeLevel
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 0f;
			}
			return OVRPlugin.systemVolume;
		}
	}

	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x060038D9 RID: 14553 RVA: 0x00121B4B File Offset: 0x0011FF4B
	// (set) Token: 0x060038DA RID: 14554 RVA: 0x00121B5E File Offset: 0x0011FF5E
	public static int cpuLevel
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 2;
			}
			return OVRPlugin.cpuLevel;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.cpuLevel = value;
		}
	}

	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x060038DB RID: 14555 RVA: 0x00121B71 File Offset: 0x0011FF71
	// (set) Token: 0x060038DC RID: 14556 RVA: 0x00121B84 File Offset: 0x0011FF84
	public static int gpuLevel
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 2;
			}
			return OVRPlugin.gpuLevel;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.gpuLevel = value;
		}
	}

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x060038DD RID: 14557 RVA: 0x00121B97 File Offset: 0x0011FF97
	public static bool isPowerSavingActive
	{
		get
		{
			return OVRManager.isHmdPresent && OVRPlugin.powerSaving;
		}
	}

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x060038DE RID: 14558 RVA: 0x00121BAA File Offset: 0x0011FFAA
	// (set) Token: 0x060038DF RID: 14559 RVA: 0x00121BC2 File Offset: 0x0011FFC2
	public OVRManager.TrackingOrigin trackingOriginType
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return this._trackingOriginType;
			}
			return (OVRManager.TrackingOrigin)OVRPlugin.GetTrackingOriginType();
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			if (OVRPlugin.SetTrackingOriginType((OVRPlugin.TrackingOrigin)value))
			{
				this._trackingOriginType = value;
			}
		}
	}

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x060038E0 RID: 14560 RVA: 0x00121BE1 File Offset: 0x0011FFE1
	// (set) Token: 0x060038E1 RID: 14561 RVA: 0x00121BE9 File Offset: 0x0011FFE9
	public bool isSupportedPlatform { get; private set; }

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x060038E2 RID: 14562 RVA: 0x00121BF2 File Offset: 0x0011FFF2
	// (set) Token: 0x060038E3 RID: 14563 RVA: 0x00121C13 File Offset: 0x00120013
	public bool isUserPresent
	{
		get
		{
			if (!OVRManager._isUserPresentCached)
			{
				OVRManager._isUserPresentCached = true;
				OVRManager._isUserPresent = OVRPlugin.userPresent;
			}
			return OVRManager._isUserPresent;
		}
		private set
		{
			OVRManager._isUserPresentCached = true;
			OVRManager._isUserPresent = value;
		}
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x00121C24 File Offset: 0x00120024
	private void Awake()
	{
		if (OVRManager.instance != null)
		{
			base.enabled = false;
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		OVRManager.instance = this;
		Debug.Log(string.Concat(new object[]
		{
			"Unity v",
			Application.unityVersion,
			", Oculus Utilities v",
			OVRPlugin.wrapperVersion,
			", OVRPlugin v",
			OVRPlugin.version,
			", SDK v",
			OVRPlugin.nativeSDKVersion,
			"."
		}));
		if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D11)
		{
			Debug.LogWarning("VR rendering requires Direct3D11. Your graphics device: " + SystemInfo.graphicsDeviceType);
		}
		RuntimePlatform platform = Application.platform;
		this.isSupportedPlatform |= (platform == RuntimePlatform.Android);
		this.isSupportedPlatform |= (platform == RuntimePlatform.OSXEditor);
		this.isSupportedPlatform |= (platform == RuntimePlatform.OSXPlayer);
		this.isSupportedPlatform |= (platform == RuntimePlatform.WindowsEditor);
		this.isSupportedPlatform |= (platform == RuntimePlatform.WindowsPlayer);
		if (!this.isSupportedPlatform)
		{
			Debug.LogWarning("This platform is unsupported");
			return;
		}
		if (OVRManager.display == null)
		{
			OVRManager.display = new OVRDisplay();
		}
		if (OVRManager.tracker == null)
		{
			OVRManager.tracker = new OVRTracker();
		}
		if (OVRManager.boundary == null)
		{
			OVRManager.boundary = new OVRBoundary();
		}
		if (this.resetTrackerOnLoad)
		{
			OVRManager.display.RecenterPose();
		}
		OVRPlugin.occlusionMesh = false;
		OVRPlugin.ignoreVrFocus = OVRManager.runInBackground;
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x00121DA8 File Offset: 0x001201A8
	private void Update()
	{
		this.paused = !OVRPlugin.hasVrFocus;
		if (OVRPlugin.shouldQuit)
		{
			Application.Quit();
		}
		if (OVRPlugin.shouldRecenter)
		{
			OVRManager.display.RecenterPose();
		}
		if (this.trackingOriginType != this._trackingOriginType)
		{
			this.trackingOriginType = this._trackingOriginType;
		}
		OVRManager.tracker.isEnabled = this.usePositionTracking;
		OVRPlugin.useIPDInPositionTracking = this.useIPDInPositionTracking;
		OVRManager.isHmdPresent = OVRPlugin.hmdPresent;
		if (this.useRecommendedMSAALevel && QualitySettings.antiAliasing != OVRManager.display.recommendedMSAALevel)
		{
			Debug.Log(string.Concat(new object[]
			{
				"The current MSAA level is ",
				QualitySettings.antiAliasing,
				", but the recommended MSAA level is ",
				OVRManager.display.recommendedMSAALevel,
				". Switching to the recommended level."
			}));
			QualitySettings.antiAliasing = OVRManager.display.recommendedMSAALevel;
		}
		if (OVRManager._wasHmdPresent && !OVRManager.isHmdPresent)
		{
			try
			{
				if (OVRManager.HMDLost != null)
				{
					OVRManager.HMDLost();
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Caught Exception: " + arg);
			}
		}
		if (!OVRManager._wasHmdPresent && OVRManager.isHmdPresent)
		{
			try
			{
				if (OVRManager.HMDAcquired != null)
				{
					OVRManager.HMDAcquired();
				}
			}
			catch (Exception arg2)
			{
				Debug.LogError("Caught Exception: " + arg2);
			}
		}
		OVRManager._wasHmdPresent = OVRManager.isHmdPresent;
		this.isUserPresent = OVRPlugin.userPresent;
		if (OVRManager._wasUserPresent && !this.isUserPresent)
		{
			try
			{
				if (OVRManager.HMDUnmounted != null)
				{
					OVRManager.HMDUnmounted();
				}
			}
			catch (Exception arg3)
			{
				Debug.LogError("Caught Exception: " + arg3);
			}
		}
		if (!OVRManager._wasUserPresent && this.isUserPresent)
		{
			try
			{
				if (OVRManager.HMDMounted != null)
				{
					OVRManager.HMDMounted();
				}
			}
			catch (Exception arg4)
			{
				Debug.LogError("Caught Exception: " + arg4);
			}
		}
		OVRManager._wasUserPresent = this.isUserPresent;
		OVRManager.hasVrFocus = OVRPlugin.hasVrFocus;
		if (OVRManager._hadVrFocus && !OVRManager.hasVrFocus)
		{
			try
			{
				if (OVRManager.VrFocusLost != null)
				{
					OVRManager.VrFocusLost();
				}
			}
			catch (Exception arg5)
			{
				Debug.LogError("Caught Exception: " + arg5);
			}
		}
		if (!OVRManager._hadVrFocus && OVRManager.hasVrFocus)
		{
			try
			{
				if (OVRManager.VrFocusAcquired != null)
				{
					OVRManager.VrFocusAcquired();
				}
			}
			catch (Exception arg6)
			{
				Debug.LogError("Caught Exception: " + arg6);
			}
		}
		OVRManager._hadVrFocus = OVRManager.hasVrFocus;
		if (this.enableAdaptiveResolution)
		{
			if (VRSettings.renderScale < this.maxRenderScale)
			{
				VRSettings.renderScale = this.maxRenderScale;
			}
			else
			{
				this.maxRenderScale = Mathf.Max(this.maxRenderScale, VRSettings.renderScale);
			}
			float min = this.minRenderScale / VRSettings.renderScale;
			float num = OVRPlugin.GetEyeRecommendedResolutionScale() / VRSettings.renderScale;
			num = Mathf.Clamp(num, min, 1f);
			VRSettings.renderViewportScale = num;
		}
		string audioOutId = OVRPlugin.audioOutId;
		if (!OVRManager.prevAudioOutIdIsCached)
		{
			OVRManager.prevAudioOutId = audioOutId;
			OVRManager.prevAudioOutIdIsCached = true;
		}
		else if (audioOutId != OVRManager.prevAudioOutId)
		{
			try
			{
				if (OVRManager.AudioOutChanged != null)
				{
					OVRManager.AudioOutChanged();
				}
			}
			catch (Exception arg7)
			{
				Debug.LogError("Caught Exception: " + arg7);
			}
			OVRManager.prevAudioOutId = audioOutId;
		}
		string audioInId = OVRPlugin.audioInId;
		if (!OVRManager.prevAudioInIdIsCached)
		{
			OVRManager.prevAudioInId = audioInId;
			OVRManager.prevAudioInIdIsCached = true;
		}
		else if (audioInId != OVRManager.prevAudioInId)
		{
			try
			{
				if (OVRManager.AudioInChanged != null)
				{
					OVRManager.AudioInChanged();
				}
			}
			catch (Exception arg8)
			{
				Debug.LogError("Caught Exception: " + arg8);
			}
			OVRManager.prevAudioInId = audioInId;
		}
		if (OVRManager.wasPositionTracked && !OVRManager.tracker.isPositionTracked)
		{
			try
			{
				if (OVRManager.TrackingLost != null)
				{
					OVRManager.TrackingLost();
				}
			}
			catch (Exception arg9)
			{
				Debug.LogError("Caught Exception: " + arg9);
			}
		}
		if (!OVRManager.wasPositionTracked && OVRManager.tracker.isPositionTracked)
		{
			try
			{
				if (OVRManager.TrackingAcquired != null)
				{
					OVRManager.TrackingAcquired();
				}
			}
			catch (Exception arg10)
			{
				Debug.LogError("Caught Exception: " + arg10);
			}
		}
		OVRManager.wasPositionTracked = OVRManager.tracker.isPositionTracked;
		OVRManager.display.Update();
		OVRInput.Update();
	}

	// Token: 0x060038E6 RID: 14566 RVA: 0x001222E4 File Offset: 0x001206E4
	private void LateUpdate()
	{
		OVRHaptics.Process();
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x001222EB File Offset: 0x001206EB
	private void FixedUpdate()
	{
		OVRInput.FixedUpdate();
	}

	// Token: 0x060038E8 RID: 14568 RVA: 0x001222F2 File Offset: 0x001206F2
	public void ReturnToLauncher()
	{
		OVRManager.PlatformUIConfirmQuit();
	}

	// Token: 0x060038E9 RID: 14569 RVA: 0x001222F9 File Offset: 0x001206F9
	public static void PlatformUIConfirmQuit()
	{
		if (!OVRManager.isHmdPresent)
		{
			return;
		}
		OVRPlugin.ShowUI(OVRPlugin.PlatformUI.ConfirmQuit);
	}

	// Token: 0x060038EA RID: 14570 RVA: 0x0012230D File Offset: 0x0012070D
	public static void PlatformUIGlobalMenu()
	{
		if (!OVRManager.isHmdPresent)
		{
			return;
		}
		OVRPlugin.ShowUI(OVRPlugin.PlatformUI.GlobalMenu);
	}

	// Token: 0x04002190 RID: 8592
	private static OVRProfile _profile;

	// Token: 0x04002191 RID: 8593
	private bool _isPaused;

	// Token: 0x04002192 RID: 8594
	private IEnumerable<Camera> disabledCameras;

	// Token: 0x04002193 RID: 8595
	private float prevTimeScale;

	// Token: 0x0400219F RID: 8607
	private static bool _isHmdPresentCached = false;

	// Token: 0x040021A0 RID: 8608
	private static bool _isHmdPresent = false;

	// Token: 0x040021A1 RID: 8609
	private static bool _wasHmdPresent = false;

	// Token: 0x040021A2 RID: 8610
	private static bool _hasVrFocusCached = false;

	// Token: 0x040021A3 RID: 8611
	private static bool _hasVrFocus = false;

	// Token: 0x040021A4 RID: 8612
	private static bool _hadVrFocus = false;

	// Token: 0x040021A5 RID: 8613
	public bool queueAhead = true;

	// Token: 0x040021A6 RID: 8614
	public bool useRecommendedMSAALevel;

	// Token: 0x040021A7 RID: 8615
	public bool enableAdaptiveResolution;

	// Token: 0x040021A8 RID: 8616
	[Range(0.5f, 2f)]
	public float maxRenderScale = 1f;

	// Token: 0x040021A9 RID: 8617
	[Range(0.5f, 2f)]
	public float minRenderScale = 0.7f;

	// Token: 0x040021AA RID: 8618
	[SerializeField]
	private OVRManager.TrackingOrigin _trackingOriginType;

	// Token: 0x040021AB RID: 8619
	public bool usePositionTracking = true;

	// Token: 0x040021AC RID: 8620
	public bool useIPDInPositionTracking = true;

	// Token: 0x040021AD RID: 8621
	public bool resetTrackerOnLoad;

	// Token: 0x040021AF RID: 8623
	private static bool _isUserPresentCached = false;

	// Token: 0x040021B0 RID: 8624
	private static bool _isUserPresent = false;

	// Token: 0x040021B1 RID: 8625
	private static bool _wasUserPresent = false;

	// Token: 0x040021B2 RID: 8626
	private static bool prevAudioOutIdIsCached = false;

	// Token: 0x040021B3 RID: 8627
	private static bool prevAudioInIdIsCached = false;

	// Token: 0x040021B4 RID: 8628
	private static string prevAudioOutId = string.Empty;

	// Token: 0x040021B5 RID: 8629
	private static string prevAudioInId = string.Empty;

	// Token: 0x040021B6 RID: 8630
	private static bool wasPositionTracked = false;

	// Token: 0x040021B7 RID: 8631
	[SerializeField]
	[HideInInspector]
	internal static bool runInBackground = false;

	// Token: 0x0200069B RID: 1691
	public enum TrackingOrigin
	{
		// Token: 0x040021B9 RID: 8633
		EyeLevel,
		// Token: 0x040021BA RID: 8634
		FloorLevel
	}
}
