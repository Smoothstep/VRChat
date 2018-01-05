using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

// Token: 0x02000C6E RID: 3182
public class VRCUiManager : MonoBehaviour
{
	// Token: 0x17000DB4 RID: 3508
	// (get) Token: 0x060062C1 RID: 25281 RVA: 0x0023077E File Offset: 0x0022EB7E
	public static VRCUiManager Instance
	{
		get
		{
			return VRCUiManager._instance;
		}
	}

	// Token: 0x060062C2 RID: 25282 RVA: 0x00230788 File Offset: 0x0022EB88
	private void Awake()
	{
		GameObject x = GameObject.Find("_Application");
		if (x == null)
		{
			base.gameObject.SetActive(false);
			AssetManagement.LoadLevel("Application2");
		}
		else
		{
			VRCUiManager._instance = this;
		}
	}

	// Token: 0x17000DB5 RID: 3509
	// (get) Token: 0x060062C3 RID: 25283 RVA: 0x002307CD File Offset: 0x0022EBCD
	public VRCUiPopupManager popups
	{
		get
		{
			return VRCUiPopupManager.Instance;
		}
	}

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x060062C4 RID: 25284 RVA: 0x002307D4 File Offset: 0x0022EBD4
	// (remove) Token: 0x060062C5 RID: 25285 RVA: 0x0023080C File Offset: 0x0022EC0C
	public event Action onUiEnabled;

	// Token: 0x14000088 RID: 136
	// (add) Token: 0x060062C6 RID: 25286 RVA: 0x00230844 File Offset: 0x0022EC44
	// (remove) Token: 0x060062C7 RID: 25287 RVA: 0x0023087C File Offset: 0x0022EC7C
	public event Action onUiDisabled;

	// Token: 0x14000089 RID: 137
	// (add) Token: 0x060062C8 RID: 25288 RVA: 0x002308B4 File Offset: 0x0022ECB4
	// (remove) Token: 0x060062C9 RID: 25289 RVA: 0x002308EC File Offset: 0x0022ECEC
	public event Action<VRCUiPage> onPageShown;

	// Token: 0x060062CA RID: 25290 RVA: 0x00230922 File Offset: 0x0022ED22
	public void Start()
	{
		this.menuContentTransform = base.transform.Find("MenuContent");
		this.SetupForDesktopOrHMD();
		VRCFlowManager.Instance.onEnteredWorld += this.OnEnteredWorld;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060062CB RID: 25291 RVA: 0x00230961 File Offset: 0x0022ED61
	private void OnDestroy()
	{
		if (VRCFlowManager.Instance != null)
		{
			VRCFlowManager.Instance.onEnteredWorld -= this.OnEnteredWorld;
		}
	}

	// Token: 0x060062CC RID: 25292 RVA: 0x00230989 File Offset: 0x0022ED89
	private void SetupForDesktopOrHMD()
	{
		if (!HMDManager.IsHmdDetected())
		{
			this.menuContentTransform.position = new Vector3(-0.007f, 0.005f, 0.78f);
			this.menuContentTransform.rotation = Quaternion.identity;
		}
	}

	// Token: 0x060062CD RID: 25293 RVA: 0x002309C4 File Offset: 0x0022EDC4
	public VRCUiPage GetPage(string screenPath)
	{
		GameObject gameObject = GameObject.Find(screenPath);
		VRCUiPage vrcuiPage = null;
		if (gameObject != null)
		{
			vrcuiPage = gameObject.GetComponent<VRCUiPage>();
			if (vrcuiPage == null)
			{
				Debug.LogError("Screen Not Found - " + screenPath);
			}
		}
		else
		{
			Debug.LogWarning("Screen Not Found - " + screenPath);
		}
		return vrcuiPage;
	}

	// Token: 0x060062CE RID: 25294 RVA: 0x00230A1F File Offset: 0x0022EE1F
	public VRCUiPage GetActiveScreen(string screenType)
	{
		return (!this.ActiveScreens.ContainsKey(screenType)) ? null : this.ActiveScreens[screenType];
	}

	// Token: 0x060062CF RID: 25295 RVA: 0x00230A44 File Offset: 0x0022EE44
	public void ShowScreen(string screen)
	{
		VRCUiPage page = this.GetPage(screen);
		this.ShowScreen(page);
	}

	// Token: 0x060062D0 RID: 25296 RVA: 0x00230A60 File Offset: 0x0022EE60
	public void ShowScreen(VRCUiPage page)
	{
		if (page == null)
		{
			Debug.LogError("Screen Not Found - " + page);
		}
		this.CheckIfSubPage(page);
		this.HideScreen(page.screenType);
		page.SetShown(true);
		this.ActiveScreens.Add(page.screenType, page);
		this.CheckUiActive();
		if (page.onPageActivated != null)
		{
			page.onPageActivated();
		}
		VRCUiCursorManager.SetUiActive(true);
		if (this.onPageShown != null)
		{
			this.onPageShown(page);
		}
	}

	// Token: 0x060062D1 RID: 25297 RVA: 0x00230AF0 File Offset: 0x0022EEF0
	public void HideScreen(string screenType)
	{
		if (!this.ActiveScreens.ContainsKey(screenType))
		{
			return;
		}
		VRCUiPage vrcuiPage = this.ActiveScreens[screenType];
		vrcuiPage.SetShown(false);
		if (vrcuiPage.onPageDeactivated != null)
		{
			vrcuiPage.onPageDeactivated();
		}
		this.ActiveScreens.Remove(screenType);
		this.CheckUiActive();
	}

	// Token: 0x060062D2 RID: 25298 RVA: 0x00230B4C File Offset: 0x0022EF4C
	public virtual void HideAll()
	{
		foreach (KeyValuePair<string, VRCUiPage> keyValuePair in this.ActiveScreens)
		{
			keyValuePair.Value.SetShown(false);
		}
		this.ActiveScreens.Clear();
		this.CheckUiActive();
		VRCTrackingManager.SetControllerVisibility(false);
	}

	// Token: 0x060062D3 RID: 25299 RVA: 0x00230BC8 File Offset: 0x0022EFC8
	public void ShowUi(bool showDefaultScreen = true, bool showBackdrop = true)
	{
		VRCUiManager.Instance.FadeTo("SpaceFade", 0.4f, null);
		if (showBackdrop)
		{
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Backdrop");
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Header");
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Backdrop/Footer");
		}
		if (showDefaultScreen)
		{
			VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Screens/Worlds");
		}
		InputStateControllerManager.localInstance.PushInputController("UIInputController");
		VRCTrackingManager.SetControllerVisibility(true);
		if (VRCUiManager.Instance.onUiEnabled != null)
		{
			VRCUiManager.Instance.onUiEnabled();
		}
	}

	// Token: 0x060062D4 RID: 25300 RVA: 0x00230C68 File Offset: 0x0022F068
	public void CloseUi(bool withFade = false)
	{
		if (this.UiActive)
		{
			if (withFade)
			{
				VRCUiManager.Instance.FadeTo("SpaceFade", 0f, null);
			}
			VRCUiManager.Instance.HideAll();
			if (InputStateControllerManager.localInstance != null)
			{
				InputStateControllerManager.localInstance.PopInputController();
			}
			if (VRCUiManager.Instance.onUiDisabled != null)
			{
				VRCUiManager.Instance.onUiDisabled();
			}
			VRCTrackingManager.SetControllerVisibility(false);
		}
	}

	// Token: 0x060062D5 RID: 25301 RVA: 0x00230CE4 File Offset: 0x0022F0E4
	public void PlaceUi()
	{
		if (VRCTrackingManager.IsInitialized())
		{
			VRCPlayer vrcplayer = (!APIUser.IsLoggedIn) ? null : VRCPlayer.Instance;
			float num = VRCTrackingManager.GetTrackingScale();
			if (num <= 0f)
			{
				num = 1f;
			}
			Vector3 position = VRCVrCamera.GetInstance().GetWorldCameraPos();
			if (vrcplayer != null)
			{
				Vector3 position2 = vrcplayer.transform.InverseTransformPoint(position);
				if (position2.y < 0.8f * num)
				{
					position2.y = 0.8f * num;
				}
				position = vrcplayer.transform.TransformPoint(position2);
			}
			this.playerTrackingDisplay.position = position;
			Vector3 euler = Vector3.zero;
			if (VRCTrackingManager.IsInVRMode())
			{
				euler = VRCTrackingManager.GetWorldTrackingOrientation().eulerAngles;
			}
			else if (vrcplayer != null)
			{
				euler = vrcplayer.transform.rotation.eulerAngles;
			}
			else
			{
				euler = VRCVrCamera.GetInstance().GetWorldCameraRot().eulerAngles;
			}
			euler.x = (euler.z = 0f);
			this.playerTrackingDisplay.rotation = Quaternion.Euler(euler);
			if (num >= 0f)
			{
				this.playerTrackingDisplay.localScale = num * Vector3.one;
			}
			else
			{
				this.playerTrackingDisplay.localScale = Vector3.one;
			}
			if (num > 1.401298E-45f)
			{
				this.unscaledUIRoot.localScale = 1f / num * Vector3.one;
			}
			else
			{
				this.unscaledUIRoot.localScale = Vector3.one;
			}
		}
	}

	// Token: 0x060062D6 RID: 25302 RVA: 0x00230E84 File Offset: 0x0022F284
	private void CheckUiActive()
	{
		bool flag = this.ActiveScreens.Count > 0;
		if (flag != this.UiActive)
		{
			if (flag)
			{
				this.PlaceUi();
			}
			this.UiActive = flag;
			VRCUiCursorManager.SetUiActive(flag);
			USpeaker.CacheDevices();
			this.menuContentTransform.gameObject.GetComponent<Collider>().enabled = flag;
			if (flag && QuickMenu.Instance != null && QuickMenu.Instance && QuickMenu.Instance.IsActive)
			{
				QuickMenu.Instance.CloseMenu();
			}
		}
	}

	// Token: 0x060062D7 RID: 25303 RVA: 0x00230F1E File Offset: 0x0022F31E
	public bool IsScreenActive(string screen)
	{
		return this.ActiveScreens.ContainsKey(screen);
	}

	// Token: 0x060062D8 RID: 25304 RVA: 0x00230F2C File Offset: 0x0022F32C
	public bool IsActive()
	{
		return this.UiActive;
	}

	// Token: 0x060062D9 RID: 25305 RVA: 0x00230F34 File Offset: 0x0022F334
	public void FadeTo(string fadeName, float fade, Action action)
	{
		foreach (VRCUiBackgroundFade vrcuiBackgroundFade in this.backgrounds)
		{
			if (vrcuiBackgroundFade.name == fadeName)
			{
				vrcuiBackgroundFade.FadeTo(fade, action);
			}
		}
	}

	// Token: 0x060062DA RID: 25306 RVA: 0x00230F7C File Offset: 0x0022F37C
	private void CheckIfSubPage(VRCUiPage page)
	{
		if (page.GetType().IsSubclassOf(typeof(VRCUiSubPage)))
		{
			VRCUiSubPageContainer[] componentsInParent = page.GetComponentsInParent<VRCUiSubPageContainer>(true);
			if (componentsInParent.Length > 0)
			{
				VRCUiSubPageContainer vrcuiSubPageContainer = componentsInParent[0];
				if (!vrcuiSubPageContainer.gameObject.activeSelf)
				{
					this.ShowScreen(vrcuiSubPageContainer.transform.GetHierarchyPath());
				}
			}
		}
	}

	// Token: 0x060062DB RID: 25307 RVA: 0x00230FDC File Offset: 0x0022F3DC
	private void Update()
	{
		if (this._delayBeforeHudMessages > 0f)
		{
			this._delayBeforeHudMessages -= Time.deltaTime;
			if (this._delayBeforeHudMessages < 0f)
			{
				this._delayBeforeHudMessages = -1f;
			}
		}
		if (this.hudMessageDisplayTime > 0f || this.hudMessageQueue.Count > 0)
		{
			float a = this.hudMessageDisplayCurve.Evaluate(1f - this.hudMessageDisplayTime / this.hudMessageDisplayDuration);
			Color color = this.hudMessageText.color;
			color.a = a;
			this.hudMessageText.color = color;
			this.hudMessageDisplayTime -= Time.deltaTime;
			if (this.hudMessageDisplayTime <= 0f)
			{
				this.hudMessageDisplayTime = 0f;
				if (this._hudMessagesEnabled && this._delayBeforeHudMessages <= 0f && this.hudMessageQueue.Count > 0)
				{
					this.hudMessageText.text = this.hudMessageQueue[0];
					this.hudMessageDisplayDuration = 0.11f * (float)this.hudMessageQueue[0].Length + 1f;
					this.hudMessageDisplayTime = this.hudMessageDisplayDuration;
					this.hudMessageQueue.RemoveAt(0);
				}
			}
		}
		if (VRCPlayer.Instance != null)
		{
			this.HandleHUDHiding();
		}
	}

	// Token: 0x060062DC RID: 25308 RVA: 0x00231148 File Offset: 0x0022F548
	private void LateUpdate()
	{
		if (VRCPlayer.Instance != null)
		{
			this.hudWorldRoot.rotation = VRCVrCamera.GetInstance().GetWorldCameraRot();
			this.hudWorldRoot.position = VRCVrCamera.GetInstance().GetWorldCameraPos();
		}
	}

	// Token: 0x060062DD RID: 25309 RVA: 0x00231184 File Offset: 0x0022F584
	private void HandleHUDHiding()
	{
		if (Input.GetKeyDown(KeyCode.H) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
		{
			this.hudMountingPoint.gameObject.SetActive(!this.hudMountingPoint.gameObject.activeSelf);
		}
	}

	// Token: 0x060062DE RID: 25310 RVA: 0x002311DE File Offset: 0x0022F5DE
	public void QueueHudMessage(string message)
	{
		this.hudMessageQueue.Add(message);
	}

	// Token: 0x060062DF RID: 25311 RVA: 0x002311EC File Offset: 0x0022F5EC
	public bool HasQueuedHudMessages()
	{
		return this.hudMessageDisplayTime > 0f || this.hudMessageQueue.Count > 0;
	}

	// Token: 0x060062E0 RID: 25312 RVA: 0x0023120F File Offset: 0x0022F60F
	public void EnableHudMessages(bool enable)
	{
		this._hudMessagesEnabled = enable;
	}

	// Token: 0x060062E1 RID: 25313 RVA: 0x00231218 File Offset: 0x0022F618
	private void OnSceneWasLoaded()
	{
		this.hudMessageQueue.Clear();
		this.hudMessageDisplayTime = 0f;
		this.QueueHudMessage(string.Empty);
	}

	// Token: 0x060062E2 RID: 25314 RVA: 0x0023123B File Offset: 0x0022F63B
	private void OnEnteredWorld()
	{
		this.EnableHudMessages(true);
		this._delayBeforeHudMessages = 5f;
	}

	// Token: 0x04004855 RID: 18517
	protected static VRCUiManager _instance;

	// Token: 0x04004856 RID: 18518
	public Transform playerTrackingDisplay;

	// Token: 0x04004857 RID: 18519
	public Transform unscaledUIRoot;

	// Token: 0x04004858 RID: 18520
	protected Dictionary<string, VRCUiPage> ActiveScreens = new Dictionary<string, VRCUiPage>();

	// Token: 0x04004859 RID: 18521
	protected bool UiActive;

	// Token: 0x0400485A RID: 18522
	public VRCUiPopup currentPopup;

	// Token: 0x0400485B RID: 18523
	public VRCUiBackgroundFade[] backgrounds;

	// Token: 0x0400485E RID: 18526
	public Transform hudMountingPoint;

	// Token: 0x0400485F RID: 18527
	public Transform hudWorldRoot;

	// Token: 0x04004860 RID: 18528
	private Transform menuContentTransform;

	// Token: 0x04004862 RID: 18530
	public AnimationCurve hudMessageDisplayCurve;

	// Token: 0x04004863 RID: 18531
	private const float MESSAGE_DISPLAY_TIME_PER_CHAR = 0.11f;

	// Token: 0x04004864 RID: 18532
	private float hudMessageDisplayTime;

	// Token: 0x04004865 RID: 18533
	private float hudMessageDisplayDuration = 1f;

	// Token: 0x04004866 RID: 18534
	private List<string> hudMessageQueue = new List<string>();

	// Token: 0x04004867 RID: 18535
	public Text hudMessageText;

	// Token: 0x04004868 RID: 18536
	private bool _hudMessagesEnabled;

	// Token: 0x04004869 RID: 18537
	private float _delayBeforeHudMessages = -1f;
}
