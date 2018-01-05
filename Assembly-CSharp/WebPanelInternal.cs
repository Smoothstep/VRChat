using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Coherent.UI;
using Coherent.UI.Binding;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B76 RID: 2934
[RequireComponent(typeof(VRC_WebPanel))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CoherentUIView))]
[RequireComponent(typeof(VRC_EventHandler))]
public class WebPanelInternal : VRCPunBehaviour
{
	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x06005B35 RID: 23349 RVA: 0x001FD08A File Offset: 0x001FB48A
	public static bool AllWebPanelsInitialized
	{
		get
		{
			return WebPanelInternal._uninitializedWebPanels.Count == 0;
		}
	}

	// Token: 0x17000D20 RID: 3360
	// (get) Token: 0x06005B36 RID: 23350 RVA: 0x001FD099 File Offset: 0x001FB499
	public bool IsLoading
	{
		get
		{
			return this.isLoading;
		}
	}

	// Token: 0x17000D21 RID: 3361
	// (get) Token: 0x06005B37 RID: 23351 RVA: 0x001FD0A1 File Offset: 0x001FB4A1
	// (set) Token: 0x06005B38 RID: 23352 RVA: 0x001FD0A9 File Offset: 0x001FB4A9
	public bool IsVirtualKeyboard
	{
		get
		{
			return this.isVirtualKeyboard;
		}
		set
		{
			this.isVirtualKeyboard = value;
		}
	}

	// Token: 0x17000D22 RID: 3362
	// (get) Token: 0x06005B39 RID: 23353 RVA: 0x001FD0B2 File Offset: 0x001FB4B2
	// (set) Token: 0x06005B3A RID: 23354 RVA: 0x001FD0BA File Offset: 0x001FB4BA
	public WebPanelInternal.CursorTypes CursorState { get; private set; }

	// Token: 0x06005B3B RID: 23355 RVA: 0x001FD0C4 File Offset: 0x001FB4C4
	public void ForceEvent(Event evt)
	{
		if ((this.ShouldSync && !base.isMine) || this.CoherentUI == null || this.CoherentUI.View == null)
		{
			return;
		}
		switch (evt.type)
		{
		case EventType.MouseDown:
		case EventType.MouseUp:
		case EventType.MouseMove:
		case EventType.MouseDrag:
		case EventType.ScrollWheel:
		{
			MouseEventData arg = Coherent.UI.InputManager.ProcessMouseEvent(evt);
			this.CoherentUI.View.MouseEvent(arg);
			break;
		}
		case EventType.KeyDown:
		case EventType.KeyUp:
		{
			KeyEventData arg2 = Coherent.UI.InputManager.ProcessKeyEvent(evt);
			this.CoherentUI.View.KeyEvent(arg2);
			break;
		}
		}
	}

	// Token: 0x06005B3C RID: 23356 RVA: 0x001FD178 File Offset: 0x001FB578
	public bool BindCall(string function, Delegate handler)
	{
		if (this.JavascriptBindings == null)
		{
			this.JavascriptBindings = new Dictionary<string, Delegate>();
		}
		if (this.JavascriptBindings.ContainsKey(function))
		{
			return false;
		}
		this.JavascriptBindings.Add(function, handler);
		if (this.hasPerformedInitialBindings)
		{
			this.CoherentUI.View.BindCall(function, handler);
		}
		return true;
	}

	// Token: 0x06005B3D RID: 23357 RVA: 0x001FD1DC File Offset: 0x001FB5DC
	public void Forward()
	{
		if (!this.ShouldSync || !base.isMine)
		{
			return;
		}
		if (this.HistoryPosition > 0)
		{
			this.HistoryPosition--;
			this.NavigateTo(this.History[this.HistoryPosition], false);
		}
	}

	// Token: 0x06005B3E RID: 23358 RVA: 0x001FD234 File Offset: 0x001FB634
	public void Backward()
	{
		if (!this.ShouldSync || !base.isMine)
		{
			return;
		}
		if (this.HistoryPosition < this.History.Count - 1)
		{
			this.HistoryPosition++;
			this.NavigateTo(this.History[this.HistoryPosition], false);
		}
	}

	// Token: 0x06005B3F RID: 23359 RVA: 0x001FD296 File Offset: 0x001FB696
	public void Reload()
	{
		this.NavigateTo(this.History[this.HistoryPosition], true);
	}

	// Token: 0x06005B40 RID: 23360 RVA: 0x001FD2B0 File Offset: 0x001FB6B0
	public void NavigateTo(string uri, bool reload = false)
	{
		base.StartCoroutine(this.SetURIInternal(uri));
	}

	// Token: 0x06005B41 RID: 23361 RVA: 0x001FD2C0 File Offset: 0x001FB6C0
	private IEnumerator SetURIInternal(string path)
	{
		yield return new WaitUntil(() => !(this.CoherentUI == null) && this.CoherentUI.View != null);
		if (path != this.CoherentUI.View.GetCurentViewPath())
		{
			this.DebugPrint(base.name + " loading " + path, new object[0]);
			Debug.Log(base.name + " loading " + path);
			this.isLoading = true;
			this.CoherentUI.View.Load(path);
		}
		yield break;
	}

	// Token: 0x06005B42 RID: 23362 RVA: 0x001FD2E2 File Offset: 0x001FB6E2
	public void ExecuteScript(string script)
	{
		if (this.CoherentUI == null || this.CoherentUI.View == null)
		{
			return;
		}
		this.CoherentUI.View.ExecuteScript(script);
	}

	// Token: 0x06005B43 RID: 23363 RVA: 0x001FD317 File Offset: 0x001FB717
	public void SetVolume(float volume)
	{
		volume = Mathf.Clamp01(volume);
		base.GetComponent<AudioSource>().volume = volume;
	}

	// Token: 0x06005B44 RID: 23364 RVA: 0x001FD32D File Offset: 0x001FB72D
	private void Haptic(bool right, float duration, float amplitude, float frequency)
	{
		VRCTrackingManager.GenerateHapticEvent((!right) ? VRCTracking.ID.HandTracker_LeftWrist : VRCTracking.ID.HandTracker_RightWrist, duration, amplitude, frequency);
	}

	// Token: 0x06005B45 RID: 23365 RVA: 0x001FD348 File Offset: 0x001FB748
	public void HandleRayHit(RaycastHit hitInfo)
	{
		if (this.IsLoading || this.web == null || !this.web.interactive || this.CoherentUI == null || this.CoherentUI.View == null)
		{
			return;
		}
		if (this.leftButtonLeftHand == null || this.leftButtonRightHand == null || this.rightButtonLeftHand == null || this.rightButtonRightHand == null)
		{
			return;
		}
		this.ReceivesInput = true;
		if (!this.ReceivesInput)
		{
			return;
		}
		MouseEventData mouseEventData = new MouseEventData
		{
			Button = MouseEventData.MouseButton.ButtonNone,
			Type = MouseEventData.EventType.MouseMove,
			X = (int)(hitInfo.textureCoord.x * (float)this.CoherentUI.Width),
			Y = (int)((1f - hitInfo.textureCoord.y) * (float)this.CoherentUI.Height)
		};
		this.CoherentUI.View.MouseEvent(mouseEventData);
		if (this.isVirtualKeyboard && this.kbDebounceTime > 0f)
		{
			this.kbDebounceTime -= Time.unscaledDeltaTime;
		}
		if (!this.mouseDragL && !this.mouseDragR)
		{
			if (!this.isVirtualKeyboard || this.kbDebounceTime <= 0f)
			{
				bool click = this.leftButtonLeftHand.click;
				bool click2 = this.leftButtonRightHand.click;
				bool flag = this.leftButtonLeftHand.held > 0.5f;
				bool flag2 = this.leftButtonRightHand.held > 0.5f;
				if (click || click2)
				{
					this.kbDebounceTime = 0.1f;
					mouseEventData.Type = MouseEventData.EventType.MouseDown;
					mouseEventData.Button = MouseEventData.MouseButton.ButtonLeft;
					this.CoherentUI.View.MouseEvent(mouseEventData);
					this.OnMouseEvent(mouseEventData);
					mouseEventData.Type = MouseEventData.EventType.MouseUp;
					this.CoherentUI.View.MouseEvent(mouseEventData);
					this.OnMouseEvent(mouseEventData);
					this.Haptic(click2, 0.02f, 0.5f, 1000f);
					if (!this.IsVirtualKeyboard)
					{
						this.ExecuteScript("if (engine!=undefined) engine.call('ReportSelectedElement', document.activeElement.tagName, document.activeElement.type, document.activeElement.name, document.activeElement.value);");
						return;
					}
				}
				else if (flag || flag2)
				{
					mouseEventData.Type = MouseEventData.EventType.MouseDown;
					mouseEventData.Button = MouseEventData.MouseButton.ButtonLeft;
					this.CoherentUI.View.MouseEvent(mouseEventData);
					this.OnMouseEvent(mouseEventData);
					this.mouseDragL = flag;
					this.mouseDragR = flag2;
					this.Haptic(flag2, 0.02f, 0.3f, 1000f);
				}
			}
		}
		else
		{
			bool flag3 = this.leftButtonLeftHand.held > 0.5f;
			bool flag4 = this.leftButtonRightHand.held > 0.5f;
			if ((this.mouseDragL && !flag3) || (this.mouseDragR && !flag4))
			{
				mouseEventData.Type = MouseEventData.EventType.MouseUp;
				mouseEventData.Button = MouseEventData.MouseButton.ButtonLeft;
				this.CoherentUI.View.MouseEvent(mouseEventData);
				this.OnMouseEvent(mouseEventData);
				if (!flag3)
				{
					this.mouseDragL = false;
				}
				if (!flag4)
				{
					this.mouseDragR = false;
				}
				if (!this.IsVirtualKeyboard)
				{
					this.ExecuteScript("if (engine!=undefined) engine.call('ReportSelectedElement', document.activeElement.tagName, document.activeElement.type, document.activeElement.name, document.activeElement.value);");
					return;
				}
			}
			else
			{
				this.hapticTimer += Time.unscaledDeltaTime;
				if (this.hapticTimer > 0.8f)
				{
					this.Haptic(this.mouseDragR, 0.01f, 0.1f, 1000f);
					this.hapticTimer = 0f;
				}
			}
		}
		if ((this.rightButtonLeftHand != null && this.rightButtonLeftHand.click) || (this.rightButtonRightHand != null && this.rightButtonRightHand.click))
		{
			mouseEventData.Type = MouseEventData.EventType.MouseDown;
			mouseEventData.Button = MouseEventData.MouseButton.ButtonRight;
			this.CoherentUI.View.MouseEvent(mouseEventData);
			this.OnMouseEvent(mouseEventData);
			mouseEventData.Type = MouseEventData.EventType.MouseUp;
			this.CoherentUI.View.MouseEvent(mouseEventData);
			this.OnMouseEvent(mouseEventData);
			this.rightButtonLeftHand.Reset();
			this.rightButtonRightHand.Reset();
		}
		if (this.mouseScroll != null && this.mouseScroll.axis != 0f)
		{
			mouseEventData.Type = MouseEventData.EventType.MouseWheel;
			mouseEventData.Button = MouseEventData.MouseButton.ButtonNone;
			mouseEventData.WheelX = 0f;
			mouseEventData.WheelY = this.mouseScroll.axis;
			this.CoherentUI.View.MouseEvent(mouseEventData);
			this.OnMouseEvent(mouseEventData);
		}
		if (!this.isVirtualKeyboard && this.isInputTag)
		{
			this.ReceivesInput = false;
			VRCUiManager.Instance.ShowUi(false, false);
			VRCUiManager.Instance.popups.ShowInputPopupWithCancel((!string.IsNullOrEmpty(this.CurrentInputName)) ? this.CurrentInputName : "Web Input", this.CurrentInputValue, (!(this.CurrentInputType == "password")) ? InputField.InputType.Standard : InputField.InputType.Password, string.Empty, delegate(string typedString, List<KeyCode> keyCodes, Text popupBody)
			{
				typedString = typedString.Replace("\\", "\\\\");
				typedString = typedString.Replace("'", "\\'");
				string script = "{  var el = document.activeElement;      el.value = '" + typedString + "';      var evt = document.createEvent('KeyboardEvent');      for (var charIdx = 0; charIdx < el.value.length; ++charIdx) {        evt.initKeyboardEvent('keyup', true, false, window, false, false, false, false, false, el.value.charCodeAt(charIdx));        el.dispatchEvent(evt);      }      if (el.form != undefined) el.form.submit();      el.blur();}";
				this.ExecuteScript(script);
				VRCUiManager.Instance.CloseUi(true);
				this.ClearCurrentTag();
			}, delegate
			{
				this.ExecuteScript("document.activeElement.blur();");
				VRCUiManager.Instance.CloseUi(true);
				this.ClearCurrentTag();
			}, "Enter text....", true, null);
		}
		this.lastHitInfo = new RaycastHit?(hitInfo);
	}

	// Token: 0x06005B46 RID: 23366 RVA: 0x001FD878 File Offset: 0x001FBC78
	public void SetFocus()
	{
		this.ReceivesInput = true;
		this.mouseDragL = (this.mouseDragR = false);
		if (this.CoherentUI != null && this.CoherentUI.View != null)
		{
			this.CoherentUI.View.SetFocus();
		}
	}

	// Token: 0x06005B47 RID: 23367 RVA: 0x001FD8D0 File Offset: 0x001FBCD0
	public void HandleFocusLoss()
	{
		this.ReceivesInput = false;
		this.mouseDragL = (this.mouseDragR = false);
		if (this.CoherentUI != null && this.CoherentUI.View != null)
		{
			this.CoherentUI.View.KillFocus();
		}
	}

	// Token: 0x17000D23 RID: 3363
	// (get) Token: 0x06005B48 RID: 23368 RVA: 0x001FD925 File Offset: 0x001FBD25
	// (set) Token: 0x06005B49 RID: 23369 RVA: 0x001FD948 File Offset: 0x001FBD48
	public bool ReceivesInput
	{
		get
		{
			return this.CoherentUI != null && this.CoherentUI.ReceivesInput;
		}
		private set
		{
			if (!this.web.interactive)
			{
				this.CoherentUI.ReceivesInput = false;
			}
			else
			{
				if (!this.web.localOnly && value && this.ShouldSync && !base.isMine)
				{
					base.RequestOwnership();
				}
				this.CoherentUI.ReceivesInput = (value && (this.web.localOnly || (base.isMine && this.ShouldSync) || (this.web.interactive && !this.ShouldSync)));
			}
		}
	}

	// Token: 0x17000D24 RID: 3364
	// (get) Token: 0x06005B4A RID: 23370 RVA: 0x001FDA04 File Offset: 0x001FBE04
	public bool ShouldSync
	{
		get
		{
			return this.web != null && base.photonView != null && (this.web.syncInput || this.web.syncDisplayAndAudio || this.web.syncURI);
		}
	}

	// Token: 0x17000D25 RID: 3365
	// (get) Token: 0x06005B4B RID: 23371 RVA: 0x001FDA64 File Offset: 0x001FBE64
	// (set) Token: 0x06005B4C RID: 23372 RVA: 0x001FDA6C File Offset: 0x001FBE6C
	public string CurrentTagName { get; private set; }

	// Token: 0x17000D26 RID: 3366
	// (get) Token: 0x06005B4D RID: 23373 RVA: 0x001FDA75 File Offset: 0x001FBE75
	// (set) Token: 0x06005B4E RID: 23374 RVA: 0x001FDA7D File Offset: 0x001FBE7D
	public string CurrentInputType { get; private set; }

	// Token: 0x17000D27 RID: 3367
	// (get) Token: 0x06005B4F RID: 23375 RVA: 0x001FDA86 File Offset: 0x001FBE86
	// (set) Token: 0x06005B50 RID: 23376 RVA: 0x001FDA8E File Offset: 0x001FBE8E
	public string CurrentInputName { get; private set; }

	// Token: 0x17000D28 RID: 3368
	// (get) Token: 0x06005B51 RID: 23377 RVA: 0x001FDA97 File Offset: 0x001FBE97
	// (set) Token: 0x06005B52 RID: 23378 RVA: 0x001FDA9F File Offset: 0x001FBE9F
	public string CurrentInputValue { get; private set; }

	// Token: 0x06005B53 RID: 23379 RVA: 0x001FDAA8 File Offset: 0x001FBEA8
	public void ClearCurrentTag()
	{
		this.CurrentTagName = string.Empty;
		this.CurrentInputType = string.Empty;
		this.CurrentInputName = string.Empty;
		this.CurrentInputValue = string.Empty;
	}

	// Token: 0x17000D29 RID: 3369
	// (get) Token: 0x06005B54 RID: 23380 RVA: 0x001FDAD8 File Offset: 0x001FBED8
	private bool isInputTag
	{
		get
		{
			if (string.IsNullOrEmpty(this.CurrentTagName))
			{
				return false;
			}
			string a = this.CurrentTagName.ToLower();
			string value = (!string.IsNullOrEmpty(this.CurrentInputType)) ? this.CurrentInputType.ToLower() : string.Empty;
			return a == "textarea" || (a == "input" && !this.nonTextInputTypes.Contains(value));
		}
	}

	// Token: 0x06005B55 RID: 23381 RVA: 0x001FDB60 File Offset: 0x001FBF60
	public override void Awake()
	{
		base.Awake();
		if (this.web == null)
		{
			this.web = base.gameObject.GetComponent<VRC_WebPanel>();
		}
		if (this.web == null)
		{
			Debug.LogError("Could not locate the associated web panel.");
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.web._NavigateTo = delegate(string uri)
		{
			this.NavigateTo(uri, false);
		};
		WebPanelInternal._uninitializedWebPanels.Add(this);
		this.web._WebPanelForward = new Action(this.Forward);
		this.web._WebPanelBackward = new Action(this.Backward);
		this.web._WebPanelReload = new Action(this.Reload);
		this.web._ExecuteScript = new Action<string>(this.ExecuteScript);
		if (CoherentUISystem.FileHandlerFactoryFunc().GetType() != typeof(WebPanelInternal.CoherentFileHandler))
		{
			CoherentUISystem.FileHandlerFactoryFunc = (() => new WebPanelInternal.CoherentFileHandler());
		}
		WebPanelInternal.CoherentFileHandler.RegisterPanel(this.web);
		WebPanelInternal.InputEvent.RegisterForSerialization();
		if (this.CoherentUI == null)
		{
			this.CoherentUI = this.web.gameObject.GetComponent<CoherentUIView>();
			if (this.CoherentUI == null)
			{
				this.CoherentUI = this.web.gameObject.AddComponent<CoherentUIView>();
			}
		}
		this.CoherentUI.OnViewCreated += this.HandleOnViewCreated;
		this.CoherentUI.OnMouseEvent += this.OnMouseEvent;
		this.CoherentUI.OnKeyEvent += this.OnKeyEvent;
		this.CoherentUI.Page = ((!string.IsNullOrEmpty(this.web.defaultUrl)) ? this.web.defaultUrl : "http://api.vrchat.cloud/public/blank.html");
		this.CoherentUI.FlipY = true;
		this.CoherentUI.Context.AllowCookies = this.web.cookiesEnabled;
		this.CoherentUI.Context.DisableWebSecurity = true;
		this.CoherentUI.UseCameraDimensions = true;
		this.CoherentUI.ScaleToFit = true;
		this.CoherentUI.IsTransparent = this.web.transparent;
		this.CoherentUI.EnableBindingAttribute = false;
		this.CoherentUI.EnableWebGLSupport = false;
		this.CoherentUI.EnableIME = false;
		this.CoherentUI.ClickToFocus = false;
		this.CoherentUI.CorrectGamma = true;
		this.CoherentUI.Filtering = CoherentUISystem.CoherentFilteringModes.LinearFiltering;
		this.CoherentUI.ForceSoftwareRendering = false;
		if (this.web.interactive)
		{
			this.CoherentUI.EnableIME = true;
			foreach (Collider collider in base.gameObject.GetComponents<Collider>())
			{
				if (collider.GetType() != typeof(MeshCollider))
				{
					UnityEngine.Object.Destroy(collider);
				}
			}
			base.gameObject.GetOrAddComponent<MeshCollider>();
		}
		this.listener = this.CoherentUI.Listener;
		this.listener.BindingsReleased += this.CoherentOnBindingsReleased;
		this.listener.Callback += this.CoherentOnCallback;
		this.listener.CaretRectChanged += this.CoherentOnCaretRectChanged;
		this.listener.CertificateError += this.CoherentOnCertificateError;
		this.listener.CursorChanged += this.CoherentOnCursorChanged;
		this.listener.Draw += this.CoherentOnDraw;
		this.listener.Error += this.CoherentOnError;
		this.listener.FailLoad += this.CoherentOnFailLoad;
		this.listener.FileSelectRequest += this.CoherentOnFileSelectRequest;
		this.listener.FinishLoad += this.CoherentOnFinishLoad;
		this.listener.GetAuthCredentials += this.CoherentOnGetAuthCredentials;
		this.listener.HistoryObtained += this.CoherentOnHistoryObtained;
		this.listener.IMEShouldCancelComposition += this.CoherentOnIMEShouldCancelComposition;
		this.listener.JavaScriptMessage += this.CoherentOnJavaScriptMessage;
		this.listener.NavigateTo += this.CoherentOnNavigateTo;
		this.listener.ReadyForBindings += this.CoherentOnReadyForBindings;
		this.listener.RequestMediaStream += this.CoherentOnRequestMediaStream;
		this.listener.ScriptMessage += this.CoherentOnScriptMessage;
		this.listener.StartLoading += this.CoherentOnStartLoading;
		this.listener.StopLoading += this.CoherentOnStopLoading;
		this.listener.TextInputTypeChanged += this.CoherentOnTextInputTypeChanged;
		this.listener.URLRequest += this.CoherentOnURLRequest;
		this.listener.ReleaseEvent += this.CoherentOnRelease;
		if (base.gameObject.GetComponent<CoherentUIClientAudio>() == null)
		{
			base.gameObject.AddComponent<CoherentUIClientAudio>();
		}
		this.eventsReceived = new List<WebPanelInternal.InputEvent>();
		this.ReceivesInput = false;
	}

	// Token: 0x06005B56 RID: 23382 RVA: 0x001FE0C8 File Offset: 0x001FC4C8
	private void SetupShader(MeshRenderer rend)
	{
		if (rend != null)
		{
			Material material = rend.material;
			if (this.web.transparent)
			{
				material.shader = Shader.Find("UI/Unlit/WebPanelTransparent");
			}
			else
			{
				material.shader = Shader.Find("Unlit/Texture");
			}
			this.shaderSetup = true;
		}
	}

	// Token: 0x06005B57 RID: 23383 RVA: 0x001FE124 File Offset: 0x001FC524
	public override IEnumerator Start()
	{
		MeshRenderer rend = base.GetComponent<MeshRenderer>();
		if (rend != null)
		{
			if (!this.shaderSetup)
			{
				this.SetupShader(rend);
			}
			Material material = rend.material;
			if (material != null)
			{
				material.SetTextureScale("_MainTex", this.web.displayRegion.size);
				material.SetTextureOffset("_MainTex", this.web.displayRegion.min);
			}
		}
		if (this.web.cursor != null)
		{
			this.Cursor = UnityEngine.Object.Instantiate<GameObject>(this.web.cursor);
			this.Cursor.transform.parent = base.gameObject.transform;
			this.Cursor.layer = 2;
			if (this.Cursor.GetComponent<Collider>() != null)
			{
				UnityEngine.Object.Destroy(this.Cursor.GetComponent<Collider>());
			}
			if (this.Cursor.GetComponentInChildren<Collider>() != null)
			{
				UnityEngine.Object.Destroy(this.Cursor.GetComponentInChildren<Collider>());
			}
		}
		this.SetViewSettings();
		this.leftButtonLeftHand = VRCInputManager.FindInput("UseLeft");
		this.leftButtonRightHand = VRCInputManager.FindInput("UseRight");
		this.rightButtonLeftHand = VRCInputManager.FindInput("DropLeft");
		this.rightButtonRightHand = VRCInputManager.FindInput("DropRight");
		this.mouseScroll = VRCInputManager.FindInput("MouseZ");
		this.NavigateTo(this.web.defaultUrl, true);
		this.History.Add(this.web.defaultUrl);
		this.eventReplicator = base.gameObject.GetComponent<WebPanelInternal.EventReplicator>();
		if (this.eventReplicator == null)
		{
			this.eventReplicator = base.gameObject.AddComponent<WebPanelInternal.EventReplicator>();
		}
		WebPanelInternal.EventReplicator eventReplicator = this.eventReplicator;
		eventReplicator.OnNewEvent = (EventReplicator<WebPanelInternal.InputEvent, WebPanelInternal.InputEvent.EqualityComparer>.NewEventHandler)Delegate.Combine(eventReplicator.OnNewEvent, new EventReplicator<WebPanelInternal.InputEvent, WebPanelInternal.InputEvent.EqualityComparer>.NewEventHandler(this.OnNewEvent));
        yield return base.Start();
		yield break;
	}

	// Token: 0x06005B58 RID: 23384 RVA: 0x001FE140 File Offset: 0x001FC540
	protected override void OnNetworkReady()
	{
		base.OnNetworkReady();
		if (!this.web.localOnly && base.photonView != null)
		{
			base.photonView.ownershipTransfer = OwnershipOption.Request;
			base.photonView.synchronization = ((!this.ShouldSync) ? ViewSynchronization.Off : ViewSynchronization.Unreliable);
			base.ObserveThis();
		}
	}

	// Token: 0x06005B59 RID: 23385 RVA: 0x001FE1A4 File Offset: 0x001FC5A4
	private void LateUpdate()
	{
		if (base.photonView != null && PhotonNetwork.masterClient != null && !base.hasOwner && PhotonNetwork.isMasterClient && this.ShouldSync)
		{
			base.RequestOwnership();
		}
		if (this.eventReplicator != null && this.ShouldSync)
		{
			if (!this.sentNag && !this.eventReplicator.IsReceivingEvents && VRC.Network.IsNetworkSettled)
			{
				this.sentNag = true;
				this.eventReplicator.RequestPastEvents();
			}
			this.ReceivesInput = (this.ReceivesInput && base.isMine);
			this.ProcessNewEventQueue();
		}
		this.DuplicateMaterial();
		this.PositionCursor();
		this.SetViewSettings();
		if (this.failedLoad)
		{
			this.failedLoad = false;
			this.Reload();
		}
	}

	// Token: 0x06005B5A RID: 23386 RVA: 0x001FE290 File Offset: 0x001FC690
	private void OnDestroy()
	{
		if (this.CoherentUI != null)
		{
			this.CoherentUI.DestroyView();
			UnityEngine.Object.Destroy(this.CoherentUI);
			this.CoherentUI = null;
		}
		WebPanelInternal.CoherentFileHandler.UnregisterPanel(this.web);
	}

	// Token: 0x06005B5B RID: 23387 RVA: 0x001FE2CC File Offset: 0x001FC6CC
	private void OnNewEvent(WebPanelInternal.InputEvent entry, VRC.Player sender)
	{
		if (!this.web.localOnly && this.web.syncInput && this.eventReplicator != null && base.Owner == sender && !base.isMine)
		{
			object obj = this.eventsReceived;
			lock (obj)
			{
				this.eventsReceived.Add(entry);
			}
		}
	}

	// Token: 0x06005B5C RID: 23388 RVA: 0x001FE35C File Offset: 0x001FC75C
	private void AddNewInputEvent(WebPanelInternal.InputEvent eventData)
	{
		if (this.web.localOnly || !base.isMine)
		{
			return;
		}
		if (eventData.mouseEvent != null)
		{
			this.mousePosition[0] = eventData.mouseEvent.X;
			this.mousePosition[1] = eventData.mouseEvent.Y;
		}
		if (this.web.syncInput && this.ReceivesInput && this.eventReplicator != null && (eventData.mouseEvent == null || eventData.mouseEvent.Type != MouseEventData.EventType.MouseMove))
		{
			this.eventReplicator.ProcessEvent(eventData, PhotonNetwork.player);
		}
	}

	// Token: 0x06005B5D RID: 23389 RVA: 0x001FE410 File Offset: 0x001FC810
	private void ProcessNewEventQueue()
	{
		if (this.isLoading || !this.eventReplicator.IsReceivingEvents || !VRC.Network.IsObjectReady(base.gameObject) || this.CoherentUI.View == null)
		{
			return;
		}
		object obj = this.eventsReceived;
		lock (obj)
		{
			if (this.ReceivesInput)
			{
				this.lastMouseEvent = null;
			}
			else
			{
				foreach (WebPanelInternal.InputEvent inputEvent in this.eventsReceived)
				{
					this.CoherentUI.ReceivesInput = true;
					if (inputEvent.keyEvent != null)
					{
						this.CoherentUI.View.KeyEvent(inputEvent.keyEvent);
					}
					if (inputEvent.mouseEvent != null)
					{
						MouseEventData mouseEventData = this.lastMouseEvent;
						if (mouseEventData != null)
						{
							mouseEventData.Type = MouseEventData.EventType.MouseMove;
							mouseEventData.X = inputEvent.mouseEvent.X;
							mouseEventData.Y = inputEvent.mouseEvent.Y;
							mouseEventData.WheelX = inputEvent.mouseEvent.WheelX;
							mouseEventData.WheelY = inputEvent.mouseEvent.WheelY;
							this.CoherentUI.View.MouseEvent(mouseEventData);
						}
						this.CoherentUI.View.MouseEvent(inputEvent.mouseEvent);
						this.lastMouseEvent = inputEvent.mouseEvent;
					}
					this.CoherentUI.ReceivesInput = false;
				}
			}
			this.eventsReceived.Clear();
		}
	}

	// Token: 0x06005B5E RID: 23390 RVA: 0x001FE5D0 File Offset: 0x001FC9D0
	private VRC_EventHandler.VrcEvent CreateURIChangeEvent(string uri)
	{
		return new VRC_EventHandler.VrcEvent
		{
			EventType = VRC_EventHandler.VrcEventType.SetWebPanelURI,
			ParameterString = uri,
			ParameterObject = base.gameObject
		};
	}

	// Token: 0x06005B5F RID: 23391 RVA: 0x001FE600 File Offset: 0x001FCA00
	private void ReplicateURI(string newURI)
	{
		if (this.web.syncURI && base.isMine)
		{
			if (base.EventHandler == null)
			{
				Debug.LogError("Sync enabled for Web Panel but no Event Handler was present.");
				return;
			}
			Debug.Log("<color=red>Web Panel changing URI:</color> " + newURI);
			this.eventReplicator.ClearRecordedEvents();
			VRC_EventHandler.VrcEvent e = this.CreateURIChangeEvent(newURI);
			base.EventHandler.TriggerEvent(e, VRC_EventHandler.VrcBroadcastType.AlwaysBufferOne, VRC.Network.LocalInstigatorID, 0f);
		}
	}

	// Token: 0x06005B60 RID: 23392 RVA: 0x001FE680 File Offset: 0x001FCA80
	private void PositionCursor()
	{
		if (!this.web.interactive)
		{
			return;
		}
		if (this.Cursor != null)
		{
			if (!this.Cursor.GetActive())
			{
				this.Cursor.SetActive(true);
			}
			if (base.photonView != null && !base.photonView.isMine)
			{
				this.Cursor.transform.position = this.cursorPosition;
				if (this.lastHitInfo != null)
				{
					this.Cursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, this.lastHitInfo.Value.normal);
				}
			}
			else if (this.lastHitInfo != null)
			{
				Vector3 point = this.lastHitInfo.Value.point;
				this.Cursor.transform.position = point;
				this.cursorPosition = point;
				this.Cursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, this.lastHitInfo.Value.normal);
			}
		}
	}

	// Token: 0x06005B61 RID: 23393 RVA: 0x001FE7AD File Offset: 0x001FCBAD
	private void OnKeyEvent(KeyEventData eventData)
	{
	}

	// Token: 0x06005B62 RID: 23394 RVA: 0x001FE7AF File Offset: 0x001FCBAF
	private void OnMouseEvent(MouseEventData eventData)
	{
		if (this.web.localOnly)
		{
			return;
		}
		this.AddNewInputEvent(new WebPanelInternal.InputEvent(eventData));
	}

	// Token: 0x06005B63 RID: 23395 RVA: 0x001FE7D0 File Offset: 0x001FCBD0
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			byte b = (!this.web.syncInput) ? (byte)0 : (byte)1;
			stream.SendNext(b);
			if (this.web.syncInput)
			{
				stream.SendNext(this.mousePosition[0]);
				stream.SendNext(this.mousePosition[1]);
				Serialization.SerializeVectorAsShorts(stream, this.cursorPosition, true);
			}
		}
		else
		{
			byte b2 = (byte)stream.ReceiveNext();
			this.web.syncInput = ((b2 & 1) > 0);
			if (this.web.syncInput)
			{
				this.mousePosition[0] = (int)stream.ReceiveNext();
				this.mousePosition[1] = (int)stream.ReceiveNext();
				this.cursorPosition = Serialization.DeserializeVectorFromShorts(stream, true);
				if (!this.isLoading && this.eventReplicator.IsReceivingEvents && VRC.Network.IsObjectReady(base.gameObject) && this.CoherentUI.View != null)
				{
					MouseEventData arg = new MouseEventData
					{
						Type = MouseEventData.EventType.MouseMove,
						X = this.mousePosition[0],
						Y = this.mousePosition[1]
					};
					this.CoherentUI.View.MouseEvent(arg);
				}
			}
		}
	}

	// Token: 0x06005B64 RID: 23396 RVA: 0x001FE92C File Offset: 0x001FCD2C
	private void DuplicateMaterial()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component != null)
		{
			Material material = component.material;
			if (material != null)
			{
				material.SetTextureScale("_MainTex", this.web.displayRegion.size);
				material.SetTextureOffset("_MainTex", this.web.displayRegion.min);
			}
			if (this.web != null && this.web.extraVideoScreens != null)
			{
				foreach (Material material2 in this.web.extraVideoScreens)
				{
					if (!(material2 == null))
					{
						material2.mainTexture = material.mainTexture;
						material2.SetTextureScale("_MainTex", this.web.displayRegion.size);
						material2.SetTextureOffset("_MainTex", this.web.displayRegion.min);
					}
				}
			}
		}
	}

	// Token: 0x06005B65 RID: 23397 RVA: 0x001FEA34 File Offset: 0x001FCE34
	private void HandleOnViewCreated(View view)
	{
		Debug.Log("View created");
		view.SetMasterVolume(1.0);
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component != null)
		{
			this.SetupShader(component);
		}
		this.DuplicateMaterial();
		WebPanelInternal._uninitializedWebPanels.Remove(this);
		if (WebPanelInternal.AllWebPanelsInitialized)
		{
			try
			{
				if (WebPanelInternal.OnAllWebPanelsInitialized != null)
				{
					WebPanelInternal.OnAllWebPanelsInitialized();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, base.gameObject);
			}
		}
		view.Resize((uint)this.web.resolutionWidth, (uint)this.web.resolutionHeight);
		view.InterceptURLRequests(true);
	}

	// Token: 0x06005B66 RID: 23398 RVA: 0x001FEAF0 File Offset: 0x001FCEF0
	private void SetViewSettings()
	{
		if (this.CoherentUI != null)
		{
			this.CoherentUI.Width = this.web.resolutionWidth;
			this.CoherentUI.Height = this.web.resolutionHeight;
		}
	}

	// Token: 0x06005B67 RID: 23399 RVA: 0x001FEB2F File Offset: 0x001FCF2F
	private void CoherentOnCursorChanged(Coherent.UI.CursorTypes cursor)
	{
		this.CursorState = (WebPanelInternal.CursorTypes)cursor;
	}

	// Token: 0x06005B68 RID: 23400 RVA: 0x001FEB38 File Offset: 0x001FCF38
	private void CoherentOnFinishLoad(int frameId, string validatedPath, bool isMainFrame, int statusCode, HTTPHeader[] headers)
	{
		if (isMainFrame && this.History != null && this.History.Count > 0)
		{
			if (this.History[this.HistoryPosition] != validatedPath)
			{
				this.History = this.History.Skip(this.HistoryPosition).ToList<string>();
				this.History.Insert(0, validatedPath);
				this.HistoryPosition = 0;
			}
			string text = string.Concat(new object[]
			{
				Application.dataPath,
				Path.DirectorySeparatorChar,
				"StreamingAssets",
				Path.DirectorySeparatorChar,
				"vrcwebroot",
				Path.DirectorySeparatorChar,
				"coherent.js"
			});
			if (File.Exists(text))
			{
				string script = File.ReadAllText(text);
				this.ExecuteScript(script);
			}
			else
			{
				Debug.LogError("Could not find Coherent bindings at " + text);
			}
			this.ReplicateURI(validatedPath);
		}
		if (!this.isVirtualKeyboard)
		{
			this.ExecuteScript("var allvids = document.getElementsByTagName('VIDEO'); for (var i=0; i<allvids.length; i++) { allvids[i].remove(); } var allfrms = document.getElementsByTagName('IFRAME'); for (var i=0; i<allfrms.length; i++) { allfrms[i].remove(); }");
		}
		this.mouseDragL = (this.mouseDragR = false);
		if (this.OnWebPageLoaded != null)
		{
			this.OnWebPageLoaded();
		}
	}

	// Token: 0x06005B69 RID: 23401 RVA: 0x001FEC77 File Offset: 0x001FD077
	private void CoherentOnNavigateTo(string path)
	{
	}

	// Token: 0x06005B6A RID: 23402 RVA: 0x001FEC7C File Offset: 0x001FD07C
	private void CoherentOnURLRequest(URLRequest request)
	{
		string url = request.GetURL();
		if (url.ToLower().StartsWith("file"))
		{
			request.SetURL("coui" + url.Substring("file".Length));
		}
	}

	// Token: 0x06005B6B RID: 23403 RVA: 0x001FECC8 File Offset: 0x001FD0C8
	private void CoherentOnFailLoad(int frameId, string validatedPath, bool isMainFrame, string error)
	{
		if (isMainFrame)
		{
			if (error != "net::ERR_ABORTED" && error != "net::ERR_FILE_NOT_FOUND")
			{
				Debug.LogError(string.Concat(new string[]
				{
					base.name,
					" failed loading ",
					validatedPath,
					"\n",
					error
				}), base.gameObject);
			}
			this.failedLoad = true;
		}
	}

	// Token: 0x06005B6C RID: 23404 RVA: 0x001FED3C File Offset: 0x001FD13C
	private void CoherentOnStartLoading()
	{
		Debug.Log(base.name + " started loading " + this.CoherentUI.View.GetCurentViewPath());
		this.isLoading = true;
		this.hasPerformedInitialBindings = false;
	}

	// Token: 0x06005B6D RID: 23405 RVA: 0x001FED71 File Offset: 0x001FD171
	private void CoherentOnStopLoading()
	{
		Debug.Log(base.name + " finished loading " + this.CoherentUI.View.GetCurentViewPath());
		this.isLoading = false;
	}

	// Token: 0x06005B6E RID: 23406 RVA: 0x001FED9F File Offset: 0x001FD19F
	private void CoherentOnBindingsReleased(int frameId, string path, bool isMainFrame)
	{
	}

	// Token: 0x06005B6F RID: 23407 RVA: 0x001FEDA1 File Offset: 0x001FD1A1
	private void CoherentOnCallback(string eventName, CallbackArguments arguments)
	{
	}

	// Token: 0x06005B70 RID: 23408 RVA: 0x001FEDA3 File Offset: 0x001FD1A3
	private void CoherentOnCaretRectChanged(uint x, uint y, uint width, uint height)
	{
	}

	// Token: 0x06005B71 RID: 23409 RVA: 0x001FEDA5 File Offset: 0x001FD1A5
	private void CoherentOnCertificateError(string url, CertificateStatus status, Certificate certificate, CertificateErrorResponse response)
	{
	}

	// Token: 0x06005B72 RID: 23410 RVA: 0x001FEDA7 File Offset: 0x001FD1A7
	private void CoherentOnDraw(CoherentHandle handle, bool usesSharedMemory, int width, int height)
	{
	}

	// Token: 0x06005B73 RID: 23411 RVA: 0x001FEDA9 File Offset: 0x001FD1A9
	private void CoherentOnError(ViewError error)
	{
		Debug.Log(base.name + " error: " + error.Error);
	}

	// Token: 0x06005B74 RID: 23412 RVA: 0x001FEDC6 File Offset: 0x001FD1C6
	private void CoherentOnFileSelectRequest(FileSelectRequest request)
	{
	}

	// Token: 0x06005B75 RID: 23413 RVA: 0x001FEDC8 File Offset: 0x001FD1C8
	private void CoherentOnGetAuthCredentials(bool isProxy, string host, uint port, string realm, string scheme)
	{
	}

	// Token: 0x06005B76 RID: 23414 RVA: 0x001FEDCA File Offset: 0x001FD1CA
	private void CoherentOnHistoryObtained(string[] previous, string[] next)
	{
	}

	// Token: 0x06005B77 RID: 23415 RVA: 0x001FEDCC File Offset: 0x001FD1CC
	private void CoherentOnIMEShouldCancelComposition()
	{
	}

	// Token: 0x06005B78 RID: 23416 RVA: 0x001FEDCE File Offset: 0x001FD1CE
	private void CoherentOnJavaScriptMessage(string message, string defaultPrompt, string frameUrl, int messageType)
	{
		Debug.Log(string.Concat(new string[]
		{
			base.name,
			" message: [",
			messageType.ToString(),
			"]",
			message
		}));
	}

	// Token: 0x06005B79 RID: 23417 RVA: 0x001FEE0D File Offset: 0x001FD20D
	private void CoherentOnRequestMediaStream(MediaStreamRequest request)
	{
	}

	// Token: 0x06005B7A RID: 23418 RVA: 0x001FEE10 File Offset: 0x001FD210
	private void CoherentOnScriptMessage(ViewListenerBase.MessageLevel level, string message, string sourceId, int line)
	{
		switch (level)
		{
		case ViewListenerBase.MessageLevel.ML_TIP:
		case ViewListenerBase.MessageLevel.ML_INFO:
			Debug.Log(string.Concat(new string[]
			{
				base.name,
				" [",
				sourceId,
				"#",
				line.ToString(),
				"]: ",
				message
			}));
			break;
		case ViewListenerBase.MessageLevel.ML_WARNING:
			Debug.LogWarning(string.Concat(new string[]
			{
				base.name,
				" [",
				sourceId,
				"#",
				line.ToString(),
				"]: ",
				message
			}));
			break;
		case ViewListenerBase.MessageLevel.ML_ERROR:
			Debug.LogError(string.Concat(new string[]
			{
				base.name,
				" [",
				sourceId,
				"#",
				line.ToString(),
				"]: ",
				message
			}));
			break;
		}
	}

	// Token: 0x06005B7B RID: 23419 RVA: 0x001FEF22 File Offset: 0x001FD322
	private void CoherentOnTextInputTypeChanged(TextInputControlType type, bool canComposeInline)
	{
	}

	// Token: 0x06005B7C RID: 23420 RVA: 0x001FEF24 File Offset: 0x001FD324
	private void CoherentOnRelease()
	{
	}

	// Token: 0x06005B7D RID: 23421 RVA: 0x001FEF28 File Offset: 0x001FD328
	private void CoherentOnReadyForBindings(int frameId, string path, bool isMainFrame)
	{
		if (isMainFrame)
		{
			this.BaseBindings();
			this.hasPerformedInitialBindings = true;
			foreach (string text in this.JavascriptBindings.Keys)
			{
				this.CoherentUI.View.BindCall(text, this.JavascriptBindings[text]);
			}
			this.ExecuteScript("{var evt = document.createEvent('Event'); evt.initEvent('onBindingsReady', true, true); document.dispatchEvent(evt);}");
		}
	}

	// Token: 0x06005B7E RID: 23422 RVA: 0x001FEFC0 File Offset: 0x001FD3C0
	private void BaseBindings()
	{
		this.BindCall("FullScreen", new Action(this.MakeFullScreen));
		this.BindCall("ReportSelectedElement", new Action<string, string, string, string>(this.ReportSelectedElement));
		this.BindCall("ListBindings", new Func<string[]>(this.ListBindings));
		this.BindStaticMethods();
	}

	// Token: 0x06005B7F RID: 23423 RVA: 0x001FF01C File Offset: 0x001FD41C
	private void BindStaticMethods()
	{
		List<Assembly> list = PluginManager.PluginAssemblies.ToList<Assembly>();
		Assembly assembly = typeof(Networking).Assembly;
		list.Add(assembly);
		foreach (Assembly assembly2 in list)
		{
			foreach (Type type in from t in assembly2.GetTypes()
			where t.IsPublic && t.IsClass
			select t)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
				{
					string text = string.Concat(new string[]
					{
						type.Namespace,
						".",
						type.Name,
						".",
						methodInfo.Name
					});
					try
					{
						if (methodInfo.ReturnType == typeof(void))
						{
							Debug.Log(base.name + " binding " + text + " as action.");
							Type actionType = Expression.GetActionType(methodInfo.GetParameters().Select((ParameterInfo p) => p.ParameterType).ToArray<Type>());
							this.BindCall(text, Delegate.CreateDelegate(actionType, methodInfo));
						}
						else
						{
							List<Type> list2 = methodInfo.GetParameters().Select((ParameterInfo p) => p.ParameterType).ToList<Type>();
							list2.Add(methodInfo.ReturnType);
							Debug.Log(string.Concat(new string[]
							{
								base.name,
								" binding ",
								text,
								" as func, returning ",
								methodInfo.ReturnType.Name,
								"."
							}));
							Type funcType = Expression.GetFuncType(list2.ToArray());
							this.BindCall(text, Delegate.CreateDelegate(funcType, methodInfo));
						}
					}
					catch (ArgumentException ex)
					{
						Debug.LogWarning("Could not bind " + text + ": " + ex.Message);
					}
				}
			}
		}
	}

	// Token: 0x06005B80 RID: 23424 RVA: 0x001FF2D8 File Offset: 0x001FD6D8
	public void MakeFullScreen()
	{
		Debug.Log("Making full screen");
		this.CoherentUI.Resize(this.CoherentUI.Width + 1, this.CoherentUI.Height + 1);
	}

	// Token: 0x06005B81 RID: 23425 RVA: 0x001FF309 File Offset: 0x001FD709
	public void ReportSelectedElement(string tagName, string elType, string elName, string elValue)
	{
		this.CurrentTagName = tagName;
		this.CurrentInputType = elType;
		this.CurrentInputName = elName;
		this.CurrentInputValue = elValue;
	}

	// Token: 0x06005B82 RID: 23426 RVA: 0x001FF328 File Offset: 0x001FD728
	public string[] ListBindings()
	{
		return this.JavascriptBindings.Keys.ToArray<string>();
	}

	// Token: 0x040040F2 RID: 16626
	public VRC_WebPanel web;

	// Token: 0x040040F3 RID: 16627
	public GameObject Cursor;

	// Token: 0x040040F4 RID: 16628
	public Dictionary<string, Delegate> JavascriptBindings = new Dictionary<string, Delegate>();

	// Token: 0x040040F5 RID: 16629
	private static HashSet<WebPanelInternal> _uninitializedWebPanels = new HashSet<WebPanelInternal>();

	// Token: 0x040040F6 RID: 16630
	public static Action OnAllWebPanelsInitialized;

	// Token: 0x040040F7 RID: 16631
	public Action OnWebPageLoaded;

	// Token: 0x040040F9 RID: 16633
	private float hapticTimer;

	// Token: 0x040040FA RID: 16634
	public CoherentUIView CoherentUI;

	// Token: 0x040040FB RID: 16635
	private MouseEventData lastMouseEvent;

	// Token: 0x040040FC RID: 16636
	private UnityViewListener listener;

	// Token: 0x040040FD RID: 16637
	private List<WebPanelInternal.InputEvent> eventsReceived = new List<WebPanelInternal.InputEvent>();

	// Token: 0x040040FE RID: 16638
	private WebPanelInternal.EventReplicator eventReplicator;

	// Token: 0x040040FF RID: 16639
	private bool isLoading;

	// Token: 0x04004100 RID: 16640
	private bool failedLoad;

	// Token: 0x04004101 RID: 16641
	private bool hasPerformedInitialBindings;

	// Token: 0x04004102 RID: 16642
	private RaycastHit? lastHitInfo;

	// Token: 0x04004103 RID: 16643
	private int[] mousePosition = new int[2];

	// Token: 0x04004104 RID: 16644
	private Vector3 cursorPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x04004105 RID: 16645
	private bool mouseDragL;

	// Token: 0x04004106 RID: 16646
	private bool mouseDragR;

	// Token: 0x04004107 RID: 16647
	private const float KEYBOARD_DEBOUNCE = 0.1f;

	// Token: 0x04004108 RID: 16648
	private float kbDebounceTime;

	// Token: 0x04004109 RID: 16649
	private bool sentNag;

	// Token: 0x0400410A RID: 16650
	public List<string> History = new List<string>();

	// Token: 0x0400410B RID: 16651
	private int HistoryPosition;

	// Token: 0x0400410C RID: 16652
	private VRCInput leftButtonLeftHand;

	// Token: 0x0400410D RID: 16653
	private VRCInput leftButtonRightHand;

	// Token: 0x0400410E RID: 16654
	private VRCInput rightButtonLeftHand;

	// Token: 0x0400410F RID: 16655
	private VRCInput rightButtonRightHand;

	// Token: 0x04004110 RID: 16656
	private VRCInput mouseScroll;

	// Token: 0x04004111 RID: 16657
	private const int audioBufferSize = 4096;

	// Token: 0x04004112 RID: 16658
	private bool isVirtualKeyboard;

	// Token: 0x04004113 RID: 16659
	private bool shaderSetup;

	// Token: 0x04004114 RID: 16660
	private const string VRCWebRoot = "vrcwebroot";

	// Token: 0x04004119 RID: 16665
	private string[] nonTextInputTypes = new string[]
	{
		"submit",
		"reset",
		"radio",
		"checkbox",
		"button"
	};

	// Token: 0x02000B77 RID: 2935
	public enum CursorTypes
	{
		// Token: 0x0400411F RID: 16671
		Pointer,
		// Token: 0x04004120 RID: 16672
		Cross,
		// Token: 0x04004121 RID: 16673
		Hand,
		// Token: 0x04004122 RID: 16674
		IBeam,
		// Token: 0x04004123 RID: 16675
		Wait,
		// Token: 0x04004124 RID: 16676
		Help,
		// Token: 0x04004125 RID: 16677
		EastResize,
		// Token: 0x04004126 RID: 16678
		NorthResize,
		// Token: 0x04004127 RID: 16679
		NorthEastResize,
		// Token: 0x04004128 RID: 16680
		NorthWestResize,
		// Token: 0x04004129 RID: 16681
		SouthResize,
		// Token: 0x0400412A RID: 16682
		SouthEastResize,
		// Token: 0x0400412B RID: 16683
		SouthWestResize,
		// Token: 0x0400412C RID: 16684
		WestResize,
		// Token: 0x0400412D RID: 16685
		NorthSouthResize,
		// Token: 0x0400412E RID: 16686
		EastWestResize,
		// Token: 0x0400412F RID: 16687
		NorthEastSouthWestResize,
		// Token: 0x04004130 RID: 16688
		NorthWestSouthEastResize,
		// Token: 0x04004131 RID: 16689
		ColumnResize,
		// Token: 0x04004132 RID: 16690
		RowResize,
		// Token: 0x04004133 RID: 16691
		MiddlePanning,
		// Token: 0x04004134 RID: 16692
		EastPanning,
		// Token: 0x04004135 RID: 16693
		NorthPanning,
		// Token: 0x04004136 RID: 16694
		NorthEastPanning,
		// Token: 0x04004137 RID: 16695
		NorthWestPanning,
		// Token: 0x04004138 RID: 16696
		SouthPanning,
		// Token: 0x04004139 RID: 16697
		SouthEastPanning,
		// Token: 0x0400413A RID: 16698
		SouthWestPanning,
		// Token: 0x0400413B RID: 16699
		WestPanning,
		// Token: 0x0400413C RID: 16700
		Move,
		// Token: 0x0400413D RID: 16701
		VerticalText,
		// Token: 0x0400413E RID: 16702
		Cell,
		// Token: 0x0400413F RID: 16703
		ContextMenu,
		// Token: 0x04004140 RID: 16704
		Alias,
		// Token: 0x04004141 RID: 16705
		Progress,
		// Token: 0x04004142 RID: 16706
		NoDrop,
		// Token: 0x04004143 RID: 16707
		Copy,
		// Token: 0x04004144 RID: 16708
		None,
		// Token: 0x04004145 RID: 16709
		NotAllowed,
		// Token: 0x04004146 RID: 16710
		ZoomIn,
		// Token: 0x04004147 RID: 16711
		ZoomOut,
		// Token: 0x04004148 RID: 16712
		Grab,
		// Token: 0x04004149 RID: 16713
		Grabbing,
		// Token: 0x0400414A RID: 16714
		Custom
	}

	// Token: 0x02000B78 RID: 2936
	private class CoherentFileHandler : FileHandler
	{
		// Token: 0x06005B8C RID: 23436 RVA: 0x001FF400 File Offset: 0x001FD800
		public CoherentFileHandler()
		{
			object obj = WebPanelInternal.CoherentFileHandler.panelSync;
			lock (obj)
			{
				WebPanelInternal.CoherentFileHandler.panels.RemoveWhere((VRC_WebPanel p) => p == null);
			}
		}

		// Token: 0x06005B8D RID: 23437 RVA: 0x001FF464 File Offset: 0x001FD864
		public static void RegisterPanel(VRC_WebPanel web)
		{
			object obj = WebPanelInternal.CoherentFileHandler.panelSync;
			lock (obj)
			{
				WebPanelInternal.CoherentFileHandler.panels.Add(web);
			}
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x001FF4A8 File Offset: 0x001FD8A8
		public static void UnregisterPanel(VRC_WebPanel web)
		{
			object obj = WebPanelInternal.CoherentFileHandler.panelSync;
			lock (obj)
			{
				WebPanelInternal.CoherentFileHandler.panels.Remove(web);
			}
		}

		// Token: 0x06005B8F RID: 23439 RVA: 0x001FF4EC File Offset: 0x001FD8EC
		private void RespondWithBytes(byte[] bytes, ResourceResponse response)
		{
			IntPtr buffer = response.GetBuffer((uint)bytes.Length);
			if (buffer == IntPtr.Zero)
			{
				response.SignalFailure();
				return;
			}
			Marshal.Copy(bytes, 0, buffer, bytes.Length);
			response.SignalSuccess();
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x001FF52C File Offset: 0x001FD92C
		public override void ReadFile(string url, URLRequestBase request, ResourceResponse response)
		{
			string text = url.Substring("coui://".Length);
			object obj = WebPanelInternal.CoherentFileHandler.panelSync;
			lock (obj)
			{
				if (text.StartsWith("vrcwebroot".ToLower()))
				{
					text = string.Concat(new object[]
					{
						Application.dataPath,
						Path.DirectorySeparatorChar,
						"StreamingAssets",
						Path.DirectorySeparatorChar,
						text.Replace('/', Path.DirectorySeparatorChar)
					});
					if (File.Exists(text))
					{
						this.RespondWithBytes(File.ReadAllBytes(text), response);
						return;
					}
				}
				foreach (VRC_WebPanel vrc_WebPanel in from p in WebPanelInternal.CoherentFileHandler.panels
				where p != null && !string.IsNullOrEmpty(p.webRoot)
				select p)
				{
					foreach (VRC_WebPanel.WebFile webFile in vrc_WebPanel.webData)
					{
						if (text.ToLower() == webFile.path.ToLower())
						{
							this.RespondWithBytes(webFile.data, response);
							return;
						}
					}
				}
			}
			Debug.LogError("Failed to intercept " + text);
		}

		// Token: 0x06005B91 RID: 23441 RVA: 0x001FF6F8 File Offset: 0x001FDAF8
		public override void WriteFile(string url, ResourceData resource)
		{
		}

		// Token: 0x0400414B RID: 16715
		private static HashSet<VRC_WebPanel> panels = new HashSet<VRC_WebPanel>();

		// Token: 0x0400414C RID: 16716
		private static object panelSync = new object();
	}

	// Token: 0x02000B79 RID: 2937
	public class EventReplicator : EventReplicator<WebPanelInternal.InputEvent, WebPanelInternal.InputEvent.EqualityComparer>
	{
		// Token: 0x06005B96 RID: 23446 RVA: 0x001FF740 File Offset: 0x001FDB40
		[PunRPC]
		protected override void SendPastEvents(PhotonPlayer sender)
		{
			base.SendPastEvents(sender);
		}

		// Token: 0x06005B97 RID: 23447 RVA: 0x001FF749 File Offset: 0x001FDB49
		[PunRPC]
		protected override void SyncEvents(WebPanelInternal.InputEvent[] eventLog, PhotonPlayer sender)
		{
			base.SyncEvents(eventLog, sender);
		}

		// Token: 0x06005B98 RID: 23448 RVA: 0x001FF753 File Offset: 0x001FDB53
		[PunRPC]
		protected override void InitialSyncFinished(PhotonPlayer sender)
		{
			base.InitialSyncFinished(sender);
		}

		// Token: 0x06005B99 RID: 23449 RVA: 0x001FF75C File Offset: 0x001FDB5C
		[PunRPC]
		public override void ProcessEvent(WebPanelInternal.InputEvent entry, PhotonPlayer sender)
		{
			base.ProcessEvent(entry, sender);
		}
	}

	// Token: 0x02000B7A RID: 2938
	public class InputEvent : IEvent
	{
		// Token: 0x06005B9A RID: 23450 RVA: 0x001FF766 File Offset: 0x001FDB66
		public InputEvent()
		{
			this.keyEvent = null;
			this.mouseEvent = null;
			this.time = 0.0;
		}

		// Token: 0x06005B9B RID: 23451 RVA: 0x001FF78B File Offset: 0x001FDB8B
		public InputEvent(KeyEventData eventData)
		{
			this.keyEvent = eventData;
			this.mouseEvent = null;
			this.time = PhotonNetwork.time;
		}

		// Token: 0x06005B9C RID: 23452 RVA: 0x001FF7AC File Offset: 0x001FDBAC
		public InputEvent(MouseEventData eventData)
		{
			this.keyEvent = null;
			this.mouseEvent = eventData;
			this.time = PhotonNetwork.time;
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06005B9D RID: 23453 RVA: 0x001FF7CD File Offset: 0x001FDBCD
		public bool Store
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06005B9E RID: 23454 RVA: 0x001FF7D0 File Offset: 0x001FDBD0
		public double Time
		{
			get
			{
				return this.time;
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06005B9F RID: 23455 RVA: 0x001FF7D8 File Offset: 0x001FDBD8
		// (set) Token: 0x06005BA0 RID: 23456 RVA: 0x001FF7E0 File Offset: 0x001FDBE0
		public int Instigator { get; set; }

		// Token: 0x06005BA1 RID: 23457 RVA: 0x001FF7EC File Offset: 0x001FDBEC
		private static byte[] SerializeForPhoton(object customobject)
		{
			WebPanelInternal.InputEvent inputEvent = (WebPanelInternal.InputEvent)customobject;
			if (inputEvent._cachedSerialization != null)
			{
				return inputEvent._cachedSerialization;
			}
			byte[] array = new byte[20];
			int num = 0;
			if (inputEvent.keyEvent != null)
			{
				int num2 = int.MinValue;
				if (inputEvent.keyEvent.IsAutoRepeat)
				{
					num2 |= 1073741824;
				}
				if (inputEvent.keyEvent.IsNumPad)
				{
					num2 |= 536870912;
				}
				if (inputEvent.keyEvent.Modifiers.IsAltDown)
				{
					num2 |= 268435456;
				}
				if (inputEvent.keyEvent.Modifiers.IsCapsOn)
				{
					num2 |= 134217728;
				}
				if (inputEvent.keyEvent.Modifiers.IsCtrlDown)
				{
					num2 |= 67108864;
				}
				if (inputEvent.keyEvent.Modifiers.IsMetaDown)
				{
					num2 |= 33554432;
				}
				if (inputEvent.keyEvent.Modifiers.IsNumLockOn)
				{
					num2 |= 16777216;
				}
				if (inputEvent.keyEvent.Modifiers.IsShiftDown)
				{
					num2 |= 8388608;
				}
				num2 |= (int)((int)(KeyEventData.EventType.Char & inputEvent.keyEvent.Type) << 21);
				Protocol.Serialize(num2, array, ref num);
				Protocol.Serialize(inputEvent.keyEvent.KeyCode, array, ref num);
			}
			else
			{
				int num3 = 0;
				num3 |= (int)((int)((MouseEventData.MouseButton)3 & inputEvent.mouseEvent.Button + 1) << 29);
				if (inputEvent.mouseEvent.Modifiers.IsAltDown)
				{
					num3 |= 268435456;
				}
				if (inputEvent.mouseEvent.Modifiers.IsCapsOn)
				{
					num3 |= 134217728;
				}
				if (inputEvent.mouseEvent.Modifiers.IsCtrlDown)
				{
					num3 |= 67108864;
				}
				if (inputEvent.mouseEvent.Modifiers.IsMetaDown)
				{
					num3 |= 33554432;
				}
				if (inputEvent.mouseEvent.Modifiers.IsNumLockOn)
				{
					num3 |= 16777216;
				}
				if (inputEvent.mouseEvent.Modifiers.IsShiftDown)
				{
					num3 |= 8388608;
				}
				if (inputEvent.mouseEvent.MouseModifiers.IsLeftButtonDown)
				{
					num3 |= 4194304;
				}
				if (inputEvent.mouseEvent.MouseModifiers.IsMiddleButtonDown)
				{
					num3 |= 2097152;
				}
				if (inputEvent.mouseEvent.MouseModifiers.IsRightButtonDown)
				{
					num3 |= 1048576;
				}
				num3 |= (int)((int)(MouseEventData.EventType.MouseWheel & inputEvent.mouseEvent.Type) << 18);
				Protocol.Serialize(num3, array, ref num);
				Protocol.Serialize(inputEvent.mouseEvent.WheelX, array, ref num);
				Protocol.Serialize(inputEvent.mouseEvent.WheelY, array, ref num);
				Protocol.Serialize(inputEvent.mouseEvent.X, array, ref num);
				Protocol.Serialize(inputEvent.mouseEvent.Y, array, ref num);
			}
			inputEvent._cachedSerialization = array;
			return array;
		}

		// Token: 0x06005BA2 RID: 23458 RVA: 0x001FFAE4 File Offset: 0x001FDEE4
		private static object DeserializeForPhoton(byte[] bytes)
		{
			WebPanelInternal.InputEvent inputEvent = new WebPanelInternal.InputEvent();
			int num = 0;
			inputEvent.time = double.MaxValue;
			int num2 = 0;
			Protocol.Deserialize(out num2, bytes, ref num);
			if ((num2 & -2147483648) > 0)
			{
				inputEvent.keyEvent = new KeyEventData();
				inputEvent.keyEvent.IsAutoRepeat = ((num2 & 1073741824) > 0);
				inputEvent.keyEvent.IsNumPad = ((num2 & 536870912) > 0);
				inputEvent.keyEvent.Modifiers.IsAltDown = ((num2 & 268435456) > 0);
				inputEvent.keyEvent.Modifiers.IsCapsOn = ((num2 & 134217728) > 0);
				inputEvent.keyEvent.Modifiers.IsCtrlDown = ((num2 & 67108864) > 0);
				inputEvent.keyEvent.Modifiers.IsMetaDown = ((num2 & 33554432) > 0);
				inputEvent.keyEvent.Modifiers.IsNumLockOn = ((num2 & 16777216) > 0);
				inputEvent.keyEvent.Modifiers.IsShiftDown = ((num2 & 8388608) > 0);
				inputEvent.keyEvent.Type = (KeyEventData.EventType)Enum.ToObject(typeof(KeyEventData.EventType), 3 & num2 >> 21);
				int keyCode = 0;
				Protocol.Deserialize(out keyCode, bytes, ref num);
				inputEvent.keyEvent.KeyCode = keyCode;
			}
			else
			{
				inputEvent.mouseEvent = new MouseEventData();
				inputEvent.mouseEvent.Button = (MouseEventData.MouseButton)Enum.ToObject(typeof(MouseEventData.MouseButton), (3 & num2 >> 29) - 1);
				inputEvent.mouseEvent.Modifiers.IsAltDown = ((num2 & 268435456) > 0);
				inputEvent.mouseEvent.Modifiers.IsCapsOn = ((num2 & 134217728) > 0);
				inputEvent.mouseEvent.Modifiers.IsCtrlDown = ((num2 & 67108864) > 0);
				inputEvent.mouseEvent.Modifiers.IsMetaDown = ((num2 & 33554432) > 0);
				inputEvent.mouseEvent.Modifiers.IsNumLockOn = ((num2 & 16777216) > 0);
				inputEvent.mouseEvent.Modifiers.IsShiftDown = ((num2 & 8388608) > 0);
				inputEvent.mouseEvent.MouseModifiers.IsLeftButtonDown = ((num2 & 4194304) > 0);
				inputEvent.mouseEvent.MouseModifiers.IsMiddleButtonDown = ((num2 & 2097152) > 0);
				inputEvent.mouseEvent.MouseModifiers.IsRightButtonDown = ((num2 & 1048576) > 0);
				inputEvent.mouseEvent.Type = (MouseEventData.EventType)Enum.ToObject(typeof(MouseEventData.EventType), 3 & num2 >> 18);
				float wheelX;
				Protocol.Deserialize(out wheelX, bytes, ref num);
				inputEvent.mouseEvent.WheelX = wheelX;
				float wheelY;
				Protocol.Deserialize(out wheelY, bytes, ref num);
				inputEvent.mouseEvent.WheelY = wheelY;
				int x;
				Protocol.Deserialize(out x, bytes, ref num);
				inputEvent.mouseEvent.X = x;
				int y;
				Protocol.Deserialize(out y, bytes, ref num);
				inputEvent.mouseEvent.Y = y;
			}
			return inputEvent;
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x001FFDD4 File Offset: 0x001FE1D4
		public static void RegisterForSerialization()
		{
			Type typeFromHandle = typeof(WebPanelInternal.InputEvent);
			byte code = 127;
			if (WebPanelInternal.InputEvent.f__mg0 == null)
			{
				WebPanelInternal.InputEvent.f__mg0 = new SerializeMethod(WebPanelInternal.InputEvent.SerializeForPhoton);
			}
			SerializeMethod serializeMethod = WebPanelInternal.InputEvent.f__mg0;
			if (WebPanelInternal.InputEvent.f__mg1 == null)
			{
				WebPanelInternal.InputEvent.f__mg1 = new DeserializeMethod(WebPanelInternal.InputEvent.DeserializeForPhoton);
			}
			PhotonPeer.RegisterType(typeFromHandle, code, serializeMethod, WebPanelInternal.InputEvent.f__mg1);
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x001FFE2D File Offset: 0x001FE22D
		public byte[] toBytes()
		{
			return WebPanelInternal.InputEvent.SerializeForPhoton(this);
		}

		// Token: 0x0400414F RID: 16719
		public KeyEventData keyEvent;

		// Token: 0x04004150 RID: 16720
		public MouseEventData mouseEvent;

		// Token: 0x04004151 RID: 16721
		public double time;

		// Token: 0x04004152 RID: 16722
		private byte[] _cachedSerialization;

		// Token: 0x04004154 RID: 16724
		[CompilerGenerated]
		private static SerializeMethod f__mg0;

		// Token: 0x04004155 RID: 16725
		[CompilerGenerated]
		private static DeserializeMethod f__mg1;

		// Token: 0x02000B7B RID: 2939
		public class EqualityComparer : IEqualityComparer<WebPanelInternal.InputEvent>
		{
			// Token: 0x06005BA6 RID: 23462 RVA: 0x001FFE3D File Offset: 0x001FE23D
			public bool Equals(WebPanelInternal.InputEvent x, WebPanelInternal.InputEvent y)
			{
				return false;
			}

			// Token: 0x06005BA7 RID: 23463 RVA: 0x001FFE40 File Offset: 0x001FE240
			public int GetHashCode(WebPanelInternal.InputEvent obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
