using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

// Token: 0x020006DD RID: 1757
public class OVRDebugInfo : MonoBehaviour
{
	// Token: 0x060039F4 RID: 14836 RVA: 0x00124580 File Offset: 0x00122980
	private void Awake()
	{
		this.debugUIManager = new GameObject();
		this.debugUIManager.name = "DebugUIManager";
		this.debugUIManager.transform.parent = GameObject.Find("LeftEyeAnchor").transform;
		RectTransform rectTransform = this.debugUIManager.AddComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(100f, 100f);
		rectTransform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
		rectTransform.localPosition = new Vector3(0.01f, 0.17f, 0.53f);
		rectTransform.localEulerAngles = Vector3.zero;
		Canvas canvas = this.debugUIManager.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.WorldSpace;
		canvas.pixelPerfect = false;
	}

	// Token: 0x060039F5 RID: 14837 RVA: 0x00124644 File Offset: 0x00122A44
	private void Update()
	{
		if (this.initUIComponent && !this.isInited)
		{
			this.InitUIComponents();
		}
		if (Input.GetKeyDown(KeyCode.Space) && this.riftPresentTimeout < 0f)
		{
			this.initUIComponent = true;
			this.showVRVars ^= true;
		}
		this.UpdateDeviceDetection();
		if (this.showVRVars)
		{
			this.debugUIManager.SetActive(true);
			this.UpdateVariable();
			this.UpdateStrings();
		}
		else
		{
			this.debugUIManager.SetActive(false);
		}
	}

	// Token: 0x060039F6 RID: 14838 RVA: 0x001246D8 File Offset: 0x00122AD8
	private void OnDestroy()
	{
		this.isInited = false;
	}

	// Token: 0x060039F7 RID: 14839 RVA: 0x001246E4 File Offset: 0x00122AE4
	private void InitUIComponents()
	{
		float num = 0f;
		int fontSize = 20;
		this.debugUIObject = new GameObject();
		this.debugUIObject.name = "DebugInfo";
		this.debugUIObject.transform.parent = GameObject.Find("DebugUIManager").transform;
		this.debugUIObject.transform.localPosition = new Vector3(0f, 100f, 0f);
		this.debugUIObject.transform.localEulerAngles = Vector3.zero;
		this.debugUIObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (!string.IsNullOrEmpty(this.strFPS))
		{
			this.fps = this.VariableObjectManager(this.fps, "FPS", num -= this.offsetY, this.strFPS, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strIPD))
		{
			this.ipd = this.VariableObjectManager(this.ipd, "IPD", num -= this.offsetY, this.strIPD, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strFOV))
		{
			this.fov = this.VariableObjectManager(this.fov, "FOV", num -= this.offsetY, this.strFOV, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strHeight))
		{
			this.height = this.VariableObjectManager(this.height, "Height", num -= this.offsetY, this.strHeight, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strDepth))
		{
			this.depth = this.VariableObjectManager(this.depth, "Depth", num -= this.offsetY, this.strDepth, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strResolutionEyeTexture))
		{
			this.resolutionEyeTexture = this.VariableObjectManager(this.resolutionEyeTexture, "Resolution", num -= this.offsetY, this.strResolutionEyeTexture, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strLatencies))
		{
			this.latencies = this.VariableObjectManager(this.latencies, "Latency", num - this.offsetY, this.strLatencies, 17);
		}
		this.initUIComponent = false;
		this.isInited = true;
	}

	// Token: 0x060039F8 RID: 14840 RVA: 0x0012492E File Offset: 0x00122D2E
	private void UpdateVariable()
	{
		this.UpdateIPD();
		this.UpdateEyeHeightOffset();
		this.UpdateEyeDepthOffset();
		this.UpdateFOV();
		this.UpdateResolutionEyeTexture();
		this.UpdateLatencyValues();
		this.UpdateFPS();
	}

	// Token: 0x060039F9 RID: 14841 RVA: 0x0012495C File Offset: 0x00122D5C
	private void UpdateStrings()
	{
		if (this.debugUIObject == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.strFPS))
		{
			this.fps.GetComponentInChildren<Text>().text = this.strFPS;
		}
		if (!string.IsNullOrEmpty(this.strIPD))
		{
			this.ipd.GetComponentInChildren<Text>().text = this.strIPD;
		}
		if (!string.IsNullOrEmpty(this.strFOV))
		{
			this.fov.GetComponentInChildren<Text>().text = this.strFOV;
		}
		if (!string.IsNullOrEmpty(this.strResolutionEyeTexture))
		{
			this.resolutionEyeTexture.GetComponentInChildren<Text>().text = this.strResolutionEyeTexture;
		}
		if (!string.IsNullOrEmpty(this.strLatencies))
		{
			this.latencies.GetComponentInChildren<Text>().text = this.strLatencies;
			this.latencies.GetComponentInChildren<Text>().fontSize = 14;
		}
		if (!string.IsNullOrEmpty(this.strHeight))
		{
			this.height.GetComponentInChildren<Text>().text = this.strHeight;
		}
		if (!string.IsNullOrEmpty(this.strDepth))
		{
			this.depth.GetComponentInChildren<Text>().text = this.strDepth;
		}
	}

	// Token: 0x060039FA RID: 14842 RVA: 0x00124A98 File Offset: 0x00122E98
	private void RiftPresentGUI(GameObject guiMainOBj)
	{
		this.riftPresent = this.ComponentComposition(this.riftPresent);
		this.riftPresent.transform.SetParent(guiMainOBj.transform);
		this.riftPresent.name = "RiftPresent";
		RectTransform component = this.riftPresent.GetComponent<RectTransform>();
		component.localPosition = new Vector3(0f, 0f, 0f);
		component.localScale = new Vector3(1f, 1f, 1f);
		component.localEulerAngles = Vector3.zero;
		Text componentInChildren = this.riftPresent.GetComponentInChildren<Text>();
		componentInChildren.text = this.strRiftPresent;
		componentInChildren.fontSize = 20;
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x00124B48 File Offset: 0x00122F48
	private void UpdateDeviceDetection()
	{
		if (this.riftPresentTimeout >= 0f)
		{
			this.riftPresentTimeout -= Time.deltaTime;
		}
	}

	// Token: 0x060039FC RID: 14844 RVA: 0x00124B6C File Offset: 0x00122F6C
	private GameObject VariableObjectManager(GameObject gameObject, string name, float posY, string str, int fontSize)
	{
		gameObject = this.ComponentComposition(gameObject);
		gameObject.name = name;
		gameObject.transform.SetParent(this.debugUIObject.transform);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.localPosition = new Vector3(0f, posY -= this.offsetY, 0f);
		Text componentInChildren = gameObject.GetComponentInChildren<Text>();
		componentInChildren.text = str;
		componentInChildren.fontSize = fontSize;
		gameObject.transform.localEulerAngles = Vector3.zero;
		component.localScale = new Vector3(1f, 1f, 1f);
		return gameObject;
	}

	// Token: 0x060039FD RID: 14845 RVA: 0x00124C08 File Offset: 0x00123008
	private GameObject ComponentComposition(GameObject GO)
	{
		GO = new GameObject();
		GO.AddComponent<RectTransform>();
		GO.AddComponent<CanvasRenderer>();
		GO.AddComponent<Image>();
		GO.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
		GO.GetComponent<Image>().color = new Color(0.02745098f, 0.1764706f, 0.2784314f, 0.784313738f);
		this.texts = new GameObject();
		this.texts.AddComponent<RectTransform>();
		this.texts.AddComponent<CanvasRenderer>();
		this.texts.AddComponent<Text>();
		this.texts.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
		this.texts.GetComponent<Text>().font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
		this.texts.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
		this.texts.transform.SetParent(GO.transform);
		this.texts.name = "TextBox";
		return GO;
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x00124D1E File Offset: 0x0012311E
	private void UpdateIPD()
	{
		this.strIPD = string.Format("IPD (mm): {0:F4}", OVRManager.profile.ipd * 1000f);
	}

	// Token: 0x060039FF RID: 14847 RVA: 0x00124D48 File Offset: 0x00123148
	private void UpdateEyeHeightOffset()
	{
		float eyeHeight = OVRManager.profile.eyeHeight;
		this.strHeight = string.Format("Eye Height (m): {0:F3}", eyeHeight);
	}

	// Token: 0x06003A00 RID: 14848 RVA: 0x00124D78 File Offset: 0x00123178
	private void UpdateEyeDepthOffset()
	{
		float eyeDepth = OVRManager.profile.eyeDepth;
		this.strDepth = string.Format("Eye Depth (m): {0:F3}", eyeDepth);
	}

	// Token: 0x06003A01 RID: 14849 RVA: 0x00124DA8 File Offset: 0x001231A8
	private void UpdateFOV()
	{
		this.strFOV = string.Format("FOV (deg): {0:F3}", OVRManager.display.GetEyeRenderDesc(VRNode.LeftEye).fov.y);
	}

	// Token: 0x06003A02 RID: 14850 RVA: 0x00124DE4 File Offset: 0x001231E4
	private void UpdateResolutionEyeTexture()
	{
		OVRDisplay.EyeRenderDesc eyeRenderDesc = OVRManager.display.GetEyeRenderDesc(VRNode.LeftEye);
		OVRDisplay.EyeRenderDesc eyeRenderDesc2 = OVRManager.display.GetEyeRenderDesc(VRNode.RightEye);
		float renderScale = VRSettings.renderScale;
		float num = (float)((int)(renderScale * (eyeRenderDesc.resolution.x + eyeRenderDesc2.resolution.x)));
		float num2 = (float)((int)(renderScale * Mathf.Max(eyeRenderDesc.resolution.y, eyeRenderDesc2.resolution.y)));
		this.strResolutionEyeTexture = string.Format("Resolution : {0} x {1}", num, num2);
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x00124E70 File Offset: 0x00123270
	private void UpdateLatencyValues()
	{
		OVRDisplay.LatencyData latency = OVRManager.display.latency;
		if (latency.render < 1E-06f && latency.timeWarp < 1E-06f && latency.postPresent < 1E-06f)
		{
			this.strLatencies = string.Format("Latency values are not available.", new object[0]);
		}
		else
		{
			this.strLatencies = string.Format("Render: {0:F3} TimeWarp: {1:F3} Post-Present: {2:F3}\nRender Error: {3:F3} TimeWarp Error: {4:F3}", new object[]
			{
				latency.render,
				latency.timeWarp,
				latency.postPresent,
				latency.renderError,
				latency.timeWarpError
			});
		}
	}

	// Token: 0x06003A04 RID: 14852 RVA: 0x00124F38 File Offset: 0x00123338
	private void UpdateFPS()
	{
		this.timeLeft -= Time.unscaledDeltaTime;
		this.accum += Time.unscaledDeltaTime;
		this.frames++;
		if ((double)this.timeLeft <= 0.0)
		{
			float num = (float)this.frames / this.accum;
			this.strFPS = string.Format("FPS: {0:F2}", num);
			this.timeLeft += this.updateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
	}

	// Token: 0x040022C6 RID: 8902
	private GameObject debugUIManager;

	// Token: 0x040022C7 RID: 8903
	private GameObject debugUIObject;

	// Token: 0x040022C8 RID: 8904
	private GameObject riftPresent;

	// Token: 0x040022C9 RID: 8905
	private GameObject fps;

	// Token: 0x040022CA RID: 8906
	private GameObject ipd;

	// Token: 0x040022CB RID: 8907
	private GameObject fov;

	// Token: 0x040022CC RID: 8908
	private GameObject height;

	// Token: 0x040022CD RID: 8909
	private GameObject depth;

	// Token: 0x040022CE RID: 8910
	private GameObject resolutionEyeTexture;

	// Token: 0x040022CF RID: 8911
	private GameObject latencies;

	// Token: 0x040022D0 RID: 8912
	private GameObject texts;

	// Token: 0x040022D1 RID: 8913
	private string strRiftPresent;

	// Token: 0x040022D2 RID: 8914
	private string strFPS;

	// Token: 0x040022D3 RID: 8915
	private string strIPD;

	// Token: 0x040022D4 RID: 8916
	private string strFOV;

	// Token: 0x040022D5 RID: 8917
	private string strHeight;

	// Token: 0x040022D6 RID: 8918
	private string strDepth;

	// Token: 0x040022D7 RID: 8919
	private string strResolutionEyeTexture;

	// Token: 0x040022D8 RID: 8920
	private string strLatencies;

	// Token: 0x040022D9 RID: 8921
	private float updateInterval = 0.5f;

	// Token: 0x040022DA RID: 8922
	private float accum;

	// Token: 0x040022DB RID: 8923
	private int frames;

	// Token: 0x040022DC RID: 8924
	private float timeLeft;

	// Token: 0x040022DD RID: 8925
	private bool initUIComponent;

	// Token: 0x040022DE RID: 8926
	private bool isInited;

	// Token: 0x040022DF RID: 8927
	private float offsetY = 55f;

	// Token: 0x040022E0 RID: 8928
	private float riftPresentTimeout;

	// Token: 0x040022E1 RID: 8929
	private bool showVRVars;
}
