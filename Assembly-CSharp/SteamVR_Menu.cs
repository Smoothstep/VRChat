using System;
using UnityEngine;
using Valve.VR;

// Token: 0x02000C02 RID: 3074
public class SteamVR_Menu : MonoBehaviour
{
	// Token: 0x17000D76 RID: 3446
	// (get) Token: 0x06005F30 RID: 24368 RVA: 0x00216133 File Offset: 0x00214533
	public RenderTexture texture
	{
		get
		{
			return (!this.overlay) ? null : (this.overlay.texture as RenderTexture);
		}
	}

	// Token: 0x17000D77 RID: 3447
	// (get) Token: 0x06005F31 RID: 24369 RVA: 0x0021615B File Offset: 0x0021455B
	// (set) Token: 0x06005F32 RID: 24370 RVA: 0x00216163 File Offset: 0x00214563
	public float scale { get; private set; }

	// Token: 0x06005F33 RID: 24371 RVA: 0x0021616C File Offset: 0x0021456C
	private void Awake()
	{
		this.scaleLimitX = string.Format("{0:N1}", this.scaleLimits.x);
		this.scaleLimitY = string.Format("{0:N1}", this.scaleLimits.y);
		this.scaleRateText = string.Format("{0:N1}", this.scaleRate);
		SteamVR_Overlay instance = SteamVR_Overlay.instance;
		if (instance != null)
		{
			this.uvOffset = instance.uvOffset;
			this.distance = instance.distance;
		}
	}

	// Token: 0x06005F34 RID: 24372 RVA: 0x00216200 File Offset: 0x00214600
	private void OnGUI()
	{
		if (this.overlay == null)
		{
			return;
		}
		RenderTexture renderTexture = this.overlay.texture as RenderTexture;
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		if (Event.current.type == EventType.Repaint)
		{
			GL.Clear(false, true, Color.clear);
		}
		Rect screenRect = new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height);
		if (Screen.width < renderTexture.width)
		{
			screenRect.width = (float)Screen.width;
			this.overlay.uvOffset.x = -(float)(renderTexture.width - Screen.width) / (float)(2 * renderTexture.width);
		}
		if (Screen.height < renderTexture.height)
		{
			screenRect.height = (float)Screen.height;
			this.overlay.uvOffset.y = (float)(renderTexture.height - Screen.height) / (float)(2 * renderTexture.height);
		}
		GUILayout.BeginArea(screenRect);
		if (this.background != null)
		{
			GUI.DrawTexture(new Rect((screenRect.width - (float)this.background.width) / 2f, (screenRect.height - (float)this.background.height) / 2f, (float)this.background.width, (float)this.background.height), this.background);
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		if (this.logo != null)
		{
			GUILayout.Space(screenRect.height / 2f - this.logoHeight);
			GUILayout.Box(this.logo, new GUILayoutOption[0]);
		}
		GUILayout.Space(this.menuOffset);
		bool flag = GUILayout.Button("[Esc] - Close menu", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(string.Format("Scale: {0:N4}", this.scale), new GUILayoutOption[0]);
		float num = GUILayout.HorizontalSlider(this.scale, this.scaleLimits.x, this.scaleLimits.y, new GUILayoutOption[0]);
		if (num != this.scale)
		{
			this.SetScale(num);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(string.Format("Scale limits:", new object[0]), new GUILayoutOption[0]);
		string text = GUILayout.TextField(this.scaleLimitX, new GUILayoutOption[0]);
		if (text != this.scaleLimitX && float.TryParse(text, out this.scaleLimits.x))
		{
			this.scaleLimitX = text;
		}
		string text2 = GUILayout.TextField(this.scaleLimitY, new GUILayoutOption[0]);
		if (text2 != this.scaleLimitY && float.TryParse(text2, out this.scaleLimits.y))
		{
			this.scaleLimitY = text2;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(string.Format("Scale rate:", new object[0]), new GUILayoutOption[0]);
		string text3 = GUILayout.TextField(this.scaleRateText, new GUILayoutOption[0]);
		if (text3 != this.scaleRateText && float.TryParse(text3, out this.scaleRate))
		{
			this.scaleRateText = text3;
		}
		GUILayout.EndHorizontal();
		if (SteamVR.active)
		{
			SteamVR instance = SteamVR.instance;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			float sceneResolutionScale = SteamVR_Camera.sceneResolutionScale;
			int num2 = (int)(instance.sceneWidth * sceneResolutionScale);
			int num3 = (int)(instance.sceneHeight * sceneResolutionScale);
			int num4 = (int)(100f * sceneResolutionScale);
			GUILayout.Label(string.Format("Scene quality: {0}x{1} ({2}%)", num2, num3, num4), new GUILayoutOption[0]);
			int num5 = Mathf.RoundToInt(GUILayout.HorizontalSlider((float)num4, 50f, 200f, new GUILayoutOption[0]));
			if (num5 != num4)
			{
				SteamVR_Camera.sceneResolutionScale = (float)num5 / 100f;
			}
			GUILayout.EndHorizontal();
		}
		this.overlay.highquality = GUILayout.Toggle(this.overlay.highquality, "High quality", new GUILayoutOption[0]);
		if (this.overlay.highquality)
		{
			this.overlay.curved = GUILayout.Toggle(this.overlay.curved, "Curved overlay", new GUILayoutOption[0]);
			this.overlay.antialias = GUILayout.Toggle(this.overlay.antialias, "Overlay RGSS(2x2)", new GUILayoutOption[0]);
		}
		else
		{
			this.overlay.curved = false;
			this.overlay.antialias = false;
		}
		SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
		if (steamVR_Camera != null)
		{
			steamVR_Camera.wireframe = GUILayout.Toggle(steamVR_Camera.wireframe, "Wireframe", new GUILayoutOption[0]);
			SteamVR_Render instance2 = SteamVR_Render.instance;
			if (instance2.trackingSpace == ETrackingUniverseOrigin.TrackingUniverseSeated)
			{
				if (GUILayout.Button("Switch to Standing", new GUILayoutOption[0]))
				{
					instance2.trackingSpace = ETrackingUniverseOrigin.TrackingUniverseStanding;
				}
				if (GUILayout.Button("Center View", new GUILayoutOption[0]))
				{
					CVRSystem system = OpenVR.System;
					if (system != null)
					{
						system.ResetSeatedZeroPose();
					}
				}
			}
			else if (GUILayout.Button("Switch to Seated", new GUILayoutOption[0]))
			{
				instance2.trackingSpace = ETrackingUniverseOrigin.TrackingUniverseSeated;
			}
		}
		if (GUILayout.Button("Exit", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		GUILayout.Space(this.menuOffset);
		string environmentVariable = Environment.GetEnvironmentVariable("VR_OVERRIDE");
		if (environmentVariable != null)
		{
			GUILayout.Label("VR_OVERRIDE=" + environmentVariable, new GUILayoutOption[0]);
		}
		GUILayout.Label("Graphics device: " + SystemInfo.graphicsDeviceVersion, new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		if (this.cursor != null)
		{
			float x = Input.mousePosition.x;
			float y = (float)Screen.height - Input.mousePosition.y;
			float width = (float)this.cursor.width;
			float height = (float)this.cursor.height;
			GUI.DrawTexture(new Rect(x, y, width, height), this.cursor);
		}
		RenderTexture.active = active;
		if (flag)
		{
			this.HideMenu();
		}
	}

	// Token: 0x06005F35 RID: 24373 RVA: 0x00216868 File Offset: 0x00214C68
	public void ShowMenu()
	{
		SteamVR_Overlay instance = SteamVR_Overlay.instance;
		if (instance == null)
		{
			return;
		}
		RenderTexture renderTexture = instance.texture as RenderTexture;
		if (renderTexture == null)
		{
			Debug.LogError("Menu requires overlay texture to be a render texture.");
			return;
		}
		this.SaveCursorState();
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		this.overlay = instance;
		this.uvOffset = instance.uvOffset;
		this.distance = instance.distance;
		Camera[] array = UnityEngine.Object.FindObjectsOfType(typeof(Camera)) as Camera[];
		foreach (Camera camera in array)
		{
			if (camera.enabled && camera.targetTexture == renderTexture)
			{
				this.overlayCam = camera;
				this.overlayCam.enabled = false;
				break;
			}
		}
		SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
		if (steamVR_Camera != null)
		{
			this.scale = steamVR_Camera.origin.localScale.x;
		}
	}

	// Token: 0x06005F36 RID: 24374 RVA: 0x00216978 File Offset: 0x00214D78
	public void HideMenu()
	{
		this.RestoreCursorState();
		if (this.overlayCam != null)
		{
			this.overlayCam.enabled = true;
		}
		if (this.overlay != null)
		{
			this.overlay.uvOffset = this.uvOffset;
			this.overlay.distance = this.distance;
			this.overlay = null;
		}
	}

	// Token: 0x06005F37 RID: 24375 RVA: 0x002169E4 File Offset: 0x00214DE4
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
		{
			if (this.overlay == null)
			{
				this.ShowMenu();
			}
			else
			{
				this.HideMenu();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Home))
		{
			this.SetScale(1f);
		}
		else if (Input.GetKey(KeyCode.PageUp))
		{
			this.SetScale(Mathf.Clamp(this.scale + this.scaleRate * Time.deltaTime, this.scaleLimits.x, this.scaleLimits.y));
		}
		else if (Input.GetKey(KeyCode.PageDown))
		{
			this.SetScale(Mathf.Clamp(this.scale - this.scaleRate * Time.deltaTime, this.scaleLimits.x, this.scaleLimits.y));
		}
	}

	// Token: 0x06005F38 RID: 24376 RVA: 0x00216AE0 File Offset: 0x00214EE0
	private void SetScale(float scale)
	{
		this.scale = scale;
		SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
		if (steamVR_Camera != null)
		{
			steamVR_Camera.origin.localScale = new Vector3(scale, scale, scale);
		}
	}

	// Token: 0x06005F39 RID: 24377 RVA: 0x00216B19 File Offset: 0x00214F19
	private void SaveCursorState()
	{
		this.savedCursorVisible = Cursor.visible;
		this.savedCursorLockState = Cursor.lockState;
	}

	// Token: 0x06005F3A RID: 24378 RVA: 0x00216B31 File Offset: 0x00214F31
	private void RestoreCursorState()
	{
		Cursor.visible = this.savedCursorVisible;
		Cursor.lockState = this.savedCursorLockState;
	}

	// Token: 0x040044EA RID: 17642
	public Texture cursor;

	// Token: 0x040044EB RID: 17643
	public Texture background;

	// Token: 0x040044EC RID: 17644
	public Texture logo;

	// Token: 0x040044ED RID: 17645
	public float logoHeight;

	// Token: 0x040044EE RID: 17646
	public float menuOffset;

	// Token: 0x040044EF RID: 17647
	public Vector2 scaleLimits = new Vector2(0.1f, 5f);

	// Token: 0x040044F0 RID: 17648
	public float scaleRate = 0.5f;

	// Token: 0x040044F1 RID: 17649
	private SteamVR_Overlay overlay;

	// Token: 0x040044F2 RID: 17650
	private Camera overlayCam;

	// Token: 0x040044F3 RID: 17651
	private Vector4 uvOffset;

	// Token: 0x040044F4 RID: 17652
	private float distance;

	// Token: 0x040044F6 RID: 17654
	private string scaleLimitX;

	// Token: 0x040044F7 RID: 17655
	private string scaleLimitY;

	// Token: 0x040044F8 RID: 17656
	private string scaleRateText;

	// Token: 0x040044F9 RID: 17657
	private CursorLockMode savedCursorLockState;

	// Token: 0x040044FA RID: 17658
	private bool savedCursorVisible;
}
