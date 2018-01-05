using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

// Token: 0x02000C07 RID: 3079
public class SteamVR_Render : MonoBehaviour
{
	// Token: 0x17000D7A RID: 3450
	// (get) Token: 0x06005F4D RID: 24397 RVA: 0x00217A82 File Offset: 0x00215E82
	// (set) Token: 0x06005F4E RID: 24398 RVA: 0x00217A89 File Offset: 0x00215E89
	public static EVREye eye { get; private set; }

	// Token: 0x17000D7B RID: 3451
	// (get) Token: 0x06005F4F RID: 24399 RVA: 0x00217A94 File Offset: 0x00215E94
	public static SteamVR_Render instance
	{
		get
		{
			if (SteamVR_Render._instance == null)
			{
				SteamVR_Render._instance = UnityEngine.Object.FindObjectOfType<SteamVR_Render>();
				if (SteamVR_Render._instance == null)
				{
					SteamVR_Render._instance = new GameObject("[SteamVR]").AddComponent<SteamVR_Render>();
				}
			}
			return SteamVR_Render._instance;
		}
	}

	// Token: 0x06005F50 RID: 24400 RVA: 0x00217AE4 File Offset: 0x00215EE4
	private void OnDestroy()
	{
		SteamVR_Render._instance = null;
	}

	// Token: 0x06005F51 RID: 24401 RVA: 0x00217AEC File Offset: 0x00215EEC
	private void OnApplicationQuit()
	{
		SteamVR_Render.isQuitting = true;
		SteamVR.SafeDispose();
	}

	// Token: 0x06005F52 RID: 24402 RVA: 0x00217AF9 File Offset: 0x00215EF9
	public static void Add(SteamVR_Camera vrcam)
	{
		if (!SteamVR_Render.isQuitting)
		{
			SteamVR_Render.instance.AddInternal(vrcam);
		}
	}

	// Token: 0x06005F53 RID: 24403 RVA: 0x00217B10 File Offset: 0x00215F10
	public static void Remove(SteamVR_Camera vrcam)
	{
		if (!SteamVR_Render.isQuitting && SteamVR_Render._instance != null)
		{
			SteamVR_Render.instance.RemoveInternal(vrcam);
		}
	}

	// Token: 0x06005F54 RID: 24404 RVA: 0x00217B37 File Offset: 0x00215F37
	public static SteamVR_Camera Top()
	{
		if (!SteamVR_Render.isQuitting)
		{
			return SteamVR_Render.instance.TopInternal();
		}
		return null;
	}

	// Token: 0x06005F55 RID: 24405 RVA: 0x00217B50 File Offset: 0x00215F50
	private void AddInternal(SteamVR_Camera vrcam)
	{
		Camera component = vrcam.GetComponent<Camera>();
		int num = this.cameras.Length;
		SteamVR_Camera[] array = new SteamVR_Camera[num + 1];
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			Camera component2 = this.cameras[i].GetComponent<Camera>();
			if (i == num2 && component2.depth > component.depth)
			{
				array[num2++] = vrcam;
			}
			array[num2++] = this.cameras[i];
		}
		if (num2 == num)
		{
			array[num2] = vrcam;
		}
		this.cameras = array;
	}

	// Token: 0x06005F56 RID: 24406 RVA: 0x00217BE4 File Offset: 0x00215FE4
	private void RemoveInternal(SteamVR_Camera vrcam)
	{
		int num = this.cameras.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			SteamVR_Camera x = this.cameras[i];
			if (x == vrcam)
			{
				num2++;
			}
		}
		if (num2 == 0)
		{
			return;
		}
		SteamVR_Camera[] array = new SteamVR_Camera[num - num2];
		int num3 = 0;
		for (int j = 0; j < num; j++)
		{
			SteamVR_Camera steamVR_Camera = this.cameras[j];
			if (steamVR_Camera != vrcam)
			{
				array[num3++] = steamVR_Camera;
			}
		}
		this.cameras = array;
	}

	// Token: 0x06005F57 RID: 24407 RVA: 0x00217C7D File Offset: 0x0021607D
	private SteamVR_Camera TopInternal()
	{
		if (this.cameras.Length > 0)
		{
			return this.cameras[this.cameras.Length - 1];
		}
		return null;
	}

	// Token: 0x17000D7C RID: 3452
	// (get) Token: 0x06005F58 RID: 24408 RVA: 0x00217CA0 File Offset: 0x002160A0
	// (set) Token: 0x06005F59 RID: 24409 RVA: 0x00217CA8 File Offset: 0x002160A8
	public static bool pauseRendering
	{
		get
		{
			return SteamVR_Render._pauseRendering;
		}
		set
		{
			SteamVR_Render._pauseRendering = value;
			CVRCompositor compositor = OpenVR.Compositor;
			if (compositor != null)
			{
				compositor.SuspendRendering(value);
			}
		}
	}

	// Token: 0x06005F5A RID: 24410 RVA: 0x00217CD0 File Offset: 0x002160D0
	private IEnumerator RenderLoop()
	{
		while (Application.isPlaying)
		{
			yield return this.waitForEndOfFrame;
			if (!SteamVR_Render.pauseRendering)
			{
				CVRCompositor compositor = OpenVR.Compositor;
				if (compositor != null)
				{
					if (!compositor.CanRenderScene())
					{
						continue;
					}
					compositor.SetTrackingSpace(this.trackingSpace);
				}
				SteamVR_Overlay overlay = SteamVR_Overlay.instance;
				if (overlay != null)
				{
					overlay.UpdateOverlay();
				}
				this.RenderExternalCamera();
			}
		}
		yield break;
	}

	// Token: 0x06005F5B RID: 24411 RVA: 0x00217CEC File Offset: 0x002160EC
	private void RenderExternalCamera()
	{
		if (this.externalCamera == null)
		{
			return;
		}
		if (!this.externalCamera.gameObject.activeInHierarchy)
		{
			return;
		}
		int num = (int)Mathf.Max(this.externalCamera.config.frameSkip, 0f);
		if (Time.frameCount % (num + 1) != 0)
		{
			return;
		}
		this.externalCamera.AttachToCamera(this.TopInternal());
		this.externalCamera.RenderNear();
		this.externalCamera.RenderFar();
	}

	// Token: 0x06005F5C RID: 24412 RVA: 0x00217D74 File Offset: 0x00216174
	private void OnInputFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			if (this.pauseGameWhenDashboardIsVisible)
			{
				Time.timeScale = this.timeScale;
			}
			SteamVR_Camera.sceneResolutionScale = this.sceneResolutionScale;
		}
		else
		{
			if (this.pauseGameWhenDashboardIsVisible)
			{
				this.timeScale = Time.timeScale;
				Time.timeScale = 0f;
			}
			this.sceneResolutionScale = SteamVR_Camera.sceneResolutionScale;
			SteamVR_Camera.sceneResolutionScale = 0.5f;
		}
	}

	// Token: 0x06005F5D RID: 24413 RVA: 0x00217DE2 File Offset: 0x002161E2
	private void OnQuit(VREvent_t vrEvent)
	{
		Application.Quit();
	}

	// Token: 0x06005F5E RID: 24414 RVA: 0x00217DEC File Offset: 0x002161EC
	private string GetScreenshotFilename(uint screenshotHandle, EVRScreenshotPropertyFilenames screenshotPropertyFilename)
	{
		EVRScreenshotError evrscreenshotError = EVRScreenshotError.None;
		uint screenshotPropertyFilename2 = OpenVR.Screenshots.GetScreenshotPropertyFilename(screenshotHandle, screenshotPropertyFilename, null, 0u, ref evrscreenshotError);
		if (evrscreenshotError != EVRScreenshotError.None && evrscreenshotError != EVRScreenshotError.BufferTooSmall)
		{
			return null;
		}
		if (screenshotPropertyFilename2 <= 1u)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder((int)screenshotPropertyFilename2);
		OpenVR.Screenshots.GetScreenshotPropertyFilename(screenshotHandle, screenshotPropertyFilename, stringBuilder, screenshotPropertyFilename2, ref evrscreenshotError);
		if (evrscreenshotError != EVRScreenshotError.None)
		{
			return null;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06005F5F RID: 24415 RVA: 0x00217E4C File Offset: 0x0021624C
	private void OnRequestScreenshot(VREvent_t vrEvent)
	{
		uint handle = vrEvent.data.screenshot.handle;
		EVRScreenshotType type = (EVRScreenshotType)vrEvent.data.screenshot.type;
		if (type == EVRScreenshotType.StereoPanorama)
		{
			string screenshotFilename = this.GetScreenshotFilename(handle, EVRScreenshotPropertyFilenames.Preview);
			string screenshotFilename2 = this.GetScreenshotFilename(handle, EVRScreenshotPropertyFilenames.VR);
			if (screenshotFilename == null || screenshotFilename2 == null)
			{
				return;
			}
			SteamVR_Utils.TakeStereoScreenshot(handle, new GameObject("screenshotPosition")
			{
				transform = 
				{
					position = SteamVR_Render.Top().transform.position,
					rotation = SteamVR_Render.Top().transform.rotation,
					localScale = SteamVR_Render.Top().transform.lossyScale
				}
			}, 32, 0.064f, ref screenshotFilename, ref screenshotFilename2);
			OpenVR.Screenshots.SubmitScreenshot(handle, type, screenshotFilename, screenshotFilename2);
		}
	}

	// Token: 0x06005F60 RID: 24416 RVA: 0x00217F24 File Offset: 0x00216324
	private void OnEnable()
	{
		base.StartCoroutine(this.RenderLoop());
		SteamVR_Events.InputFocus.Listen(new UnityAction<bool>(this.OnInputFocus));
		SteamVR_Events.System(EVREventType.VREvent_Quit).Listen(new UnityAction<VREvent_t>(this.OnQuit));
		SteamVR_Events.System(EVREventType.VREvent_RequestScreenshot).Listen(new UnityAction<VREvent_t>(this.OnRequestScreenshot));
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.OnCameraPreCull));
		if (SteamVR.instance == null)
		{
			base.enabled = false;
			return;
		}
		EVRScreenshotType[] pSupportedTypes = new EVRScreenshotType[]
		{
			EVRScreenshotType.StereoPanorama
		};
		OpenVR.Screenshots.HookScreenshot(pSupportedTypes);
	}

	// Token: 0x06005F61 RID: 24417 RVA: 0x00217FD8 File Offset: 0x002163D8
	private void OnDisable()
	{
		base.StopAllCoroutines();
		SteamVR_Events.InputFocus.Remove(new UnityAction<bool>(this.OnInputFocus));
		SteamVR_Events.System(EVREventType.VREvent_Quit).Remove(new UnityAction<VREvent_t>(this.OnQuit));
		SteamVR_Events.System(EVREventType.VREvent_RequestScreenshot).Remove(new UnityAction<VREvent_t>(this.OnRequestScreenshot));
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.OnCameraPreCull));
	}

	// Token: 0x06005F62 RID: 24418 RVA: 0x00218058 File Offset: 0x00216458
	private void Awake()
	{
		if (this.externalCamera == null && File.Exists(this.externalCameraConfigPath))
		{
			GameObject original = Resources.Load<GameObject>("SteamVR_ExternalCamera");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.gameObject.name = "External Camera";
			this.externalCamera = gameObject.transform.GetChild(0).GetComponent<SteamVR_ExternalCamera>();
			this.externalCamera.configPath = this.externalCameraConfigPath;
			this.externalCamera.ReadConfig();
		}
	}

	// Token: 0x06005F63 RID: 24419 RVA: 0x002180DC File Offset: 0x002164DC
	public void UpdatePoses()
	{
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			compositor.GetLastPoses(this.poses, this.gamePoses);
			SteamVR_Events.NewPoses.Send(this.poses);
			SteamVR_Events.NewPosesApplied.Send();
		}
	}

	// Token: 0x06005F64 RID: 24420 RVA: 0x00218122 File Offset: 0x00216522
	private void OnCameraPreCull(Camera cam)
	{
		if (Time.frameCount != SteamVR_Render.lastFrameCount)
		{
			SteamVR_Render.lastFrameCount = Time.frameCount;
			this.UpdatePoses();
		}
	}

	// Token: 0x06005F65 RID: 24421 RVA: 0x00218144 File Offset: 0x00216544
	private void Update()
	{
		SteamVR_Controller.Update();
		CVRSystem system = OpenVR.System;
		if (system != null)
		{
			VREvent_t arg = default(VREvent_t);
			uint uncbVREvent = (uint)Marshal.SizeOf(typeof(VREvent_t));
			for (int i = 0; i < 64; i++)
			{
				if (!system.PollNextEvent(ref arg, uncbVREvent))
				{
					break;
				}
				EVREventType eventType = (EVREventType)arg.eventType;
				if (eventType != EVREventType.VREvent_InputFocusCaptured)
				{
					if (eventType != EVREventType.VREvent_InputFocusReleased)
					{
						if (eventType != EVREventType.VREvent_HideRenderModels)
						{
							if (eventType != EVREventType.VREvent_ShowRenderModels)
							{
								SteamVR_Events.System((EVREventType)arg.eventType).Send(arg);
							}
							else
							{
								SteamVR_Events.HideRenderModels.Send(false);
							}
						}
						else
						{
							SteamVR_Events.HideRenderModels.Send(true);
						}
					}
					else if (arg.data.process.pid == 0u)
					{
						SteamVR_Events.InputFocus.Send(true);
					}
				}
				else if (arg.data.process.oldPid == 0u)
				{
					SteamVR_Events.InputFocus.Send(false);
				}
			}
		}
		Application.targetFrameRate = -1;
		Application.runInBackground = true;
		QualitySettings.maxQueuedFrames = -1;
		QualitySettings.vSyncCount = 0;
		if (this.lockPhysicsUpdateRateToRenderFrequency && Time.timeScale > 0f)
		{
			SteamVR instance = SteamVR.instance;
			if (instance != null)
			{
				Compositor_FrameTiming compositor_FrameTiming = default(Compositor_FrameTiming);
				compositor_FrameTiming.m_nSize = (uint)Marshal.SizeOf(typeof(Compositor_FrameTiming));
				instance.compositor.GetFrameTiming(ref compositor_FrameTiming, 0u);
				Time.fixedDeltaTime = Time.timeScale / instance.hmd_DisplayFrequency;
			}
		}
	}

	// Token: 0x04004518 RID: 17688
	public bool pauseGameWhenDashboardIsVisible = true;

	// Token: 0x04004519 RID: 17689
	public bool lockPhysicsUpdateRateToRenderFrequency = true;

	// Token: 0x0400451A RID: 17690
	public SteamVR_ExternalCamera externalCamera;

	// Token: 0x0400451B RID: 17691
	public string externalCameraConfigPath = "externalcamera.cfg";

	// Token: 0x0400451C RID: 17692
	public ETrackingUniverseOrigin trackingSpace = ETrackingUniverseOrigin.TrackingUniverseStanding;

	// Token: 0x0400451E RID: 17694
	private static SteamVR_Render _instance;

	// Token: 0x0400451F RID: 17695
	private static bool isQuitting;

	// Token: 0x04004520 RID: 17696
	private SteamVR_Camera[] cameras = new SteamVR_Camera[0];

	// Token: 0x04004521 RID: 17697
	public TrackedDevicePose_t[] poses = new TrackedDevicePose_t[16];

	// Token: 0x04004522 RID: 17698
	public TrackedDevicePose_t[] gamePoses = new TrackedDevicePose_t[0];

	// Token: 0x04004523 RID: 17699
	private static bool _pauseRendering;

	// Token: 0x04004524 RID: 17700
	private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

	// Token: 0x04004525 RID: 17701
	private float sceneResolutionScale = 1f;

	// Token: 0x04004526 RID: 17702
	private float timeScale = 1f;

	// Token: 0x04004527 RID: 17703
	private static int lastFrameCount = -1;
}
