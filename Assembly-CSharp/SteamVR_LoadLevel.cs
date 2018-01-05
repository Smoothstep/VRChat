using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

// Token: 0x02000C01 RID: 3073
public class SteamVR_LoadLevel : MonoBehaviour
{
	// Token: 0x17000D73 RID: 3443
	// (get) Token: 0x06005F24 RID: 24356 RVA: 0x00215212 File Offset: 0x00213612
	public static bool loading
	{
		get
		{
			return SteamVR_LoadLevel._active != null;
		}
	}

	// Token: 0x17000D74 RID: 3444
	// (get) Token: 0x06005F25 RID: 24357 RVA: 0x0021521F File Offset: 0x0021361F
	public static float progress
	{
		get
		{
			return (!(SteamVR_LoadLevel._active != null) || SteamVR_LoadLevel._active.async == null) ? 0f : SteamVR_LoadLevel._active.async.progress;
		}
	}

	// Token: 0x17000D75 RID: 3445
	// (get) Token: 0x06005F26 RID: 24358 RVA: 0x00215259 File Offset: 0x00213659
	public static Texture progressTexture
	{
		get
		{
			return (!(SteamVR_LoadLevel._active != null)) ? null : SteamVR_LoadLevel._active.renderTexture;
		}
	}

	// Token: 0x06005F27 RID: 24359 RVA: 0x0021527B File Offset: 0x0021367B
	private void OnEnable()
	{
		if (this.autoTriggerOnEnable)
		{
			this.Trigger();
		}
	}

	// Token: 0x06005F28 RID: 24360 RVA: 0x0021528E File Offset: 0x0021368E
	public void Trigger()
	{
		if (!SteamVR_LoadLevel.loading && !string.IsNullOrEmpty(this.levelName))
		{
			base.StartCoroutine(this.LoadLevel());
		}
	}

	// Token: 0x06005F29 RID: 24361 RVA: 0x002152B8 File Offset: 0x002136B8
	public static void Begin(string levelName, bool showGrid = false, float fadeOutTime = 0.5f, float r = 0f, float g = 0f, float b = 0f, float a = 1f)
	{
		SteamVR_LoadLevel steamVR_LoadLevel = new GameObject("loader").AddComponent<SteamVR_LoadLevel>();
		steamVR_LoadLevel.levelName = levelName;
		steamVR_LoadLevel.showGrid = showGrid;
		steamVR_LoadLevel.fadeOutTime = fadeOutTime;
		steamVR_LoadLevel.backgroundColor = new Color(r, g, b, a);
		steamVR_LoadLevel.Trigger();
	}

	// Token: 0x06005F2A RID: 24362 RVA: 0x00215304 File Offset: 0x00213704
	private void OnGUI()
	{
		if (SteamVR_LoadLevel._active != this)
		{
			return;
		}
		if (this.progressBarEmpty != null && this.progressBarFull != null)
		{
			if (this.progressBarOverlayHandle == 0UL)
			{
				this.progressBarOverlayHandle = this.GetOverlayHandle("progressBar", (!(this.progressBarTransform != null)) ? base.transform : this.progressBarTransform, this.progressBarWidthInMeters);
			}
			if (this.progressBarOverlayHandle != 0UL)
			{
				float num = (this.async == null) ? 0f : this.async.progress;
				int width = this.progressBarFull.width;
				int height = this.progressBarFull.height;
				if (this.renderTexture == null)
				{
					this.renderTexture = new RenderTexture(width, height, 0);
					this.renderTexture.Create();
				}
				RenderTexture active = RenderTexture.active;
				RenderTexture.active = this.renderTexture;
				if (Event.current.type == EventType.Repaint)
				{
					GL.Clear(false, true, Color.clear);
				}
				GUILayout.BeginArea(new Rect(0f, 0f, (float)width, (float)height));
				GUI.DrawTexture(new Rect(0f, 0f, (float)width, (float)height), this.progressBarEmpty);
				GUI.DrawTextureWithTexCoords(new Rect(0f, 0f, num * (float)width, (float)height), this.progressBarFull, new Rect(0f, 0f, num, 1f));
				GUILayout.EndArea();
				RenderTexture.active = active;
				CVROverlay overlay = OpenVR.Overlay;
				if (overlay != null)
				{
					Texture_t texture_t = default(Texture_t);
					texture_t.handle = this.renderTexture.GetNativeTexturePtr();
					texture_t.eType = SteamVR.instance.textureType;
					texture_t.eColorSpace = EColorSpace.Auto;
					overlay.SetOverlayTexture(this.progressBarOverlayHandle, ref texture_t);
				}
			}
		}
	}

	// Token: 0x06005F2B RID: 24363 RVA: 0x002154F0 File Offset: 0x002138F0
	private void Update()
	{
		if (SteamVR_LoadLevel._active != this)
		{
			return;
		}
		this.alpha = Mathf.Clamp01(this.alpha + this.fadeRate * Time.deltaTime);
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay != null)
		{
			if (this.loadingScreenOverlayHandle != 0UL)
			{
				overlay.SetOverlayAlpha(this.loadingScreenOverlayHandle, this.alpha);
			}
			if (this.progressBarOverlayHandle != 0UL)
			{
				overlay.SetOverlayAlpha(this.progressBarOverlayHandle, this.alpha);
			}
		}
	}

	// Token: 0x06005F2C RID: 24364 RVA: 0x00215578 File Offset: 0x00213978
	private IEnumerator LoadLevel()
	{
		if (this.loadingScreen != null && this.loadingScreenDistance > 0f)
		{
			SteamVR_Controller.Device hmd = SteamVR_Controller.Input(0);
			while (!hmd.hasTracking)
			{
				yield return null;
			}
			SteamVR_Utils.RigidTransform tloading = hmd.transform;
			tloading.rot = Quaternion.Euler(0f, tloading.rot.eulerAngles.y, 0f);
			tloading.pos += tloading.rot * new Vector3(0f, 0f, this.loadingScreenDistance);
			Transform t = (!(this.loadingScreenTransform != null)) ? base.transform : this.loadingScreenTransform;
			t.position = tloading.pos;
			t.rotation = tloading.rot;
		}
		SteamVR_LoadLevel._active = this;
		SteamVR_Events.Loading.Send(true);
		if (this.loadingScreenFadeInTime > 0f)
		{
			this.fadeRate = 1f / this.loadingScreenFadeInTime;
		}
		else
		{
			this.alpha = 1f;
		}
		CVROverlay overlay = OpenVR.Overlay;
		if (this.loadingScreen != null && overlay != null)
		{
			this.loadingScreenOverlayHandle = this.GetOverlayHandle("loadingScreen", (!(this.loadingScreenTransform != null)) ? base.transform : this.loadingScreenTransform, this.loadingScreenWidthInMeters);
			if (this.loadingScreenOverlayHandle != 0UL)
			{
				Texture_t texture_t = default(Texture_t);
				texture_t.handle = this.loadingScreen.GetNativeTexturePtr();
				texture_t.eType = SteamVR.instance.textureType;
				texture_t.eColorSpace = EColorSpace.Auto;
				overlay.SetOverlayTexture(this.loadingScreenOverlayHandle, ref texture_t);
			}
		}
		bool fadedForeground = false;
		SteamVR_Events.LoadingFadeOut.Send(this.fadeOutTime);
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			if (this.front != null)
			{
				SteamVR_Skybox.SetOverride(this.front, this.back, this.left, this.right, this.top, this.bottom);
				compositor.FadeGrid(this.fadeOutTime, true);
				yield return new WaitForSeconds(this.fadeOutTime);
			}
			else if (this.backgroundColor != Color.clear)
			{
				if (this.showGrid)
				{
					compositor.FadeToColor(0f, this.backgroundColor.r, this.backgroundColor.g, this.backgroundColor.b, this.backgroundColor.a, true);
					compositor.FadeGrid(this.fadeOutTime, true);
					yield return new WaitForSeconds(this.fadeOutTime);
				}
				else
				{
					compositor.FadeToColor(this.fadeOutTime, this.backgroundColor.r, this.backgroundColor.g, this.backgroundColor.b, this.backgroundColor.a, false);
					yield return new WaitForSeconds(this.fadeOutTime + 0.1f);
					compositor.FadeGrid(0f, true);
					fadedForeground = true;
				}
			}
		}
		SteamVR_Render.pauseRendering = true;
		while (this.alpha < 1f)
		{
			yield return null;
		}
		base.transform.parent = null;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!string.IsNullOrEmpty(this.internalProcessPath))
		{
			UnityEngine.Debug.Log("Launching external application...");
			CVRApplications applications = OpenVR.Applications;
			if (applications == null)
			{
				UnityEngine.Debug.Log("Failed to get OpenVR.Applications interface!");
			}
			else
			{
				string currentDirectory = Directory.GetCurrentDirectory();
				string text = Path.Combine(currentDirectory, this.internalProcessPath);
				UnityEngine.Debug.Log("LaunchingInternalProcess");
				UnityEngine.Debug.Log("ExternalAppPath = " + this.internalProcessPath);
				UnityEngine.Debug.Log("FullPath = " + text);
				UnityEngine.Debug.Log("ExternalAppArgs = " + this.internalProcessArgs);
				UnityEngine.Debug.Log("WorkingDirectory = " + currentDirectory);
				EVRApplicationError evrapplicationError = applications.LaunchInternalProcess(text, this.internalProcessArgs, currentDirectory);
				UnityEngine.Debug.Log("LaunchInternalProcessError: " + evrapplicationError);
				Process.GetCurrentProcess().Kill();
			}
		}
		else
		{
			LoadSceneMode mode = (!this.loadAdditive) ? LoadSceneMode.Single : LoadSceneMode.Additive;
			if (this.loadAsync)
			{
				Application.backgroundLoadingPriority = ThreadPriority.Low;
				this.async = SceneManager.LoadSceneAsync(this.levelName, mode);
				while (!this.async.isDone)
				{
					yield return null;
				}
			}
			else
			{
				SceneManager.LoadScene(this.levelName, mode);
			}
		}
		yield return null;
		GC.Collect();
		yield return null;
		Shader.WarmupAllShaders();
		yield return new WaitForSeconds(this.postLoadSettleTime);
		SteamVR_Render.pauseRendering = false;
		if (this.loadingScreenFadeOutTime > 0f)
		{
			this.fadeRate = -1f / this.loadingScreenFadeOutTime;
		}
		else
		{
			this.alpha = 0f;
		}
		SteamVR_Events.LoadingFadeIn.Send(this.fadeInTime);
		if (compositor != null)
		{
			if (fadedForeground)
			{
				compositor.FadeGrid(0f, false);
				compositor.FadeToColor(this.fadeInTime, 0f, 0f, 0f, 0f, false);
				yield return new WaitForSeconds(this.fadeInTime);
			}
			else
			{
				compositor.FadeGrid(this.fadeInTime, false);
				yield return new WaitForSeconds(this.fadeInTime);
				if (this.front != null)
				{
					SteamVR_Skybox.ClearOverride();
				}
			}
		}
		while (this.alpha > 0f)
		{
			yield return null;
		}
		if (overlay != null)
		{
			if (this.progressBarOverlayHandle != 0UL)
			{
				overlay.HideOverlay(this.progressBarOverlayHandle);
			}
			if (this.loadingScreenOverlayHandle != 0UL)
			{
				overlay.HideOverlay(this.loadingScreenOverlayHandle);
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
		SteamVR_LoadLevel._active = null;
		SteamVR_Events.Loading.Send(false);
		yield break;
	}

	// Token: 0x06005F2D RID: 24365 RVA: 0x00215594 File Offset: 0x00213994
	private ulong GetOverlayHandle(string overlayName, Transform transform, float widthInMeters = 1f)
	{
		ulong num = 0UL;
		CVROverlay overlay = OpenVR.Overlay;
		if (overlay == null)
		{
			return num;
		}
		string pchOverlayKey = SteamVR_Overlay.key + "." + overlayName;
		EVROverlayError evroverlayError = overlay.FindOverlay(pchOverlayKey, ref num);
		if (evroverlayError != EVROverlayError.None)
		{
			evroverlayError = overlay.CreateOverlay(pchOverlayKey, overlayName, ref num);
		}
		if (evroverlayError == EVROverlayError.None)
		{
			overlay.ShowOverlay(num);
			overlay.SetOverlayAlpha(num, this.alpha);
			overlay.SetOverlayWidthInMeters(num, widthInMeters);
			if (SteamVR.instance.textureType == ETextureType.DirectX)
			{
				VRTextureBounds_t vrtextureBounds_t = default(VRTextureBounds_t);
				vrtextureBounds_t.uMin = 0f;
				vrtextureBounds_t.vMin = 1f;
				vrtextureBounds_t.uMax = 1f;
				vrtextureBounds_t.vMax = 0f;
				overlay.SetOverlayTextureBounds(num, ref vrtextureBounds_t);
			}
			SteamVR_Camera steamVR_Camera = (this.loadingScreenDistance != 0f) ? null : SteamVR_Render.Top();
			if (steamVR_Camera != null && steamVR_Camera.origin != null)
			{
				SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(steamVR_Camera.origin, transform);
				rigidTransform.pos.x = rigidTransform.pos.x / steamVR_Camera.origin.localScale.x;
				rigidTransform.pos.y = rigidTransform.pos.y / steamVR_Camera.origin.localScale.y;
				rigidTransform.pos.z = rigidTransform.pos.z / steamVR_Camera.origin.localScale.z;
				HmdMatrix34_t hmdMatrix34_t = rigidTransform.ToHmdMatrix34();
				overlay.SetOverlayTransformAbsolute(num, SteamVR_Render.instance.trackingSpace, ref hmdMatrix34_t);
			}
			else
			{
				SteamVR_Utils.RigidTransform rigidTransform2 = new SteamVR_Utils.RigidTransform(transform);
				HmdMatrix34_t hmdMatrix34_t2 = rigidTransform2.ToHmdMatrix34();
				overlay.SetOverlayTransformAbsolute(num, SteamVR_Render.instance.trackingSpace, ref hmdMatrix34_t2);
			}
		}
		return num;
	}

	// Token: 0x040044C8 RID: 17608
	private static SteamVR_LoadLevel _active;

	// Token: 0x040044C9 RID: 17609
	public string levelName;

	// Token: 0x040044CA RID: 17610
	public string internalProcessPath;

	// Token: 0x040044CB RID: 17611
	public string internalProcessArgs;

	// Token: 0x040044CC RID: 17612
	public bool loadAdditive;

	// Token: 0x040044CD RID: 17613
	public bool loadAsync = true;

	// Token: 0x040044CE RID: 17614
	public Texture loadingScreen;

	// Token: 0x040044CF RID: 17615
	public Texture progressBarEmpty;

	// Token: 0x040044D0 RID: 17616
	public Texture progressBarFull;

	// Token: 0x040044D1 RID: 17617
	public float loadingScreenWidthInMeters = 6f;

	// Token: 0x040044D2 RID: 17618
	public float progressBarWidthInMeters = 3f;

	// Token: 0x040044D3 RID: 17619
	public float loadingScreenDistance;

	// Token: 0x040044D4 RID: 17620
	public Transform loadingScreenTransform;

	// Token: 0x040044D5 RID: 17621
	public Transform progressBarTransform;

	// Token: 0x040044D6 RID: 17622
	public Texture front;

	// Token: 0x040044D7 RID: 17623
	public Texture back;

	// Token: 0x040044D8 RID: 17624
	public Texture left;

	// Token: 0x040044D9 RID: 17625
	public Texture right;

	// Token: 0x040044DA RID: 17626
	public Texture top;

	// Token: 0x040044DB RID: 17627
	public Texture bottom;

	// Token: 0x040044DC RID: 17628
	public Color backgroundColor = Color.black;

	// Token: 0x040044DD RID: 17629
	public bool showGrid;

	// Token: 0x040044DE RID: 17630
	public float fadeOutTime = 0.5f;

	// Token: 0x040044DF RID: 17631
	public float fadeInTime = 0.5f;

	// Token: 0x040044E0 RID: 17632
	public float postLoadSettleTime;

	// Token: 0x040044E1 RID: 17633
	public float loadingScreenFadeInTime = 1f;

	// Token: 0x040044E2 RID: 17634
	public float loadingScreenFadeOutTime = 0.25f;

	// Token: 0x040044E3 RID: 17635
	private float fadeRate = 1f;

	// Token: 0x040044E4 RID: 17636
	private float alpha;

	// Token: 0x040044E5 RID: 17637
	private AsyncOperation async;

	// Token: 0x040044E6 RID: 17638
	private RenderTexture renderTexture;

	// Token: 0x040044E7 RID: 17639
	private ulong loadingScreenOverlayHandle;

	// Token: 0x040044E8 RID: 17640
	private ulong progressBarOverlayHandle;

	// Token: 0x040044E9 RID: 17641
	public bool autoTriggerOnEnable;
}
