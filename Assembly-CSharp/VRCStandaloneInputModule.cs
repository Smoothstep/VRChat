using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000C61 RID: 3169
public class VRCStandaloneInputModule : PointerInputModule
{
	// Token: 0x06006263 RID: 25187 RVA: 0x00231730 File Offset: 0x0022FB30
	protected VRCStandaloneInputModule()
	{
	}

	// Token: 0x17000DAD RID: 3501
	// (get) Token: 0x06006264 RID: 25188 RVA: 0x0023179D File Offset: 0x0022FB9D
	[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
	public VRCStandaloneInputModule.InputMode inputMode
	{
		get
		{
			return VRCStandaloneInputModule.InputMode.Mouse;
		}
	}

	// Token: 0x17000DAE RID: 3502
	// (get) Token: 0x06006265 RID: 25189 RVA: 0x002317A0 File Offset: 0x0022FBA0
	// (set) Token: 0x06006266 RID: 25190 RVA: 0x002317A8 File Offset: 0x0022FBA8
	[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
	public bool allowActivationOnMobileDevice
	{
		get
		{
			return this.m_ForceModuleActive;
		}
		set
		{
			this.m_ForceModuleActive = value;
		}
	}

	// Token: 0x17000DAF RID: 3503
	// (get) Token: 0x06006267 RID: 25191 RVA: 0x002317B1 File Offset: 0x0022FBB1
	// (set) Token: 0x06006268 RID: 25192 RVA: 0x002317B9 File Offset: 0x0022FBB9
	public bool forceModuleActive
	{
		get
		{
			return this.m_ForceModuleActive;
		}
		set
		{
			this.m_ForceModuleActive = value;
		}
	}

	// Token: 0x17000DB0 RID: 3504
	// (get) Token: 0x06006269 RID: 25193 RVA: 0x002317C2 File Offset: 0x0022FBC2
	// (set) Token: 0x0600626A RID: 25194 RVA: 0x002317CA File Offset: 0x0022FBCA
	public float inputActionsPerSecond
	{
		get
		{
			return this.m_InputActionsPerSecond;
		}
		set
		{
			this.m_InputActionsPerSecond = value;
		}
	}

	// Token: 0x17000DB1 RID: 3505
	// (get) Token: 0x0600626B RID: 25195 RVA: 0x002317D3 File Offset: 0x0022FBD3
	// (set) Token: 0x0600626C RID: 25196 RVA: 0x002317DB File Offset: 0x0022FBDB
	public float repeatDelay
	{
		get
		{
			return this.m_RepeatDelay;
		}
		set
		{
			this.m_RepeatDelay = value;
		}
	}

	// Token: 0x17000DB2 RID: 3506
	// (get) Token: 0x0600626D RID: 25197 RVA: 0x002317E4 File Offset: 0x0022FBE4
	// (set) Token: 0x0600626E RID: 25198 RVA: 0x002317EC File Offset: 0x0022FBEC
	public string horizontalAxis
	{
		get
		{
			return this.m_HorizontalAxis;
		}
		set
		{
			this.m_HorizontalAxis = value;
		}
	}

	// Token: 0x17000DB3 RID: 3507
	// (get) Token: 0x0600626F RID: 25199 RVA: 0x002317F5 File Offset: 0x0022FBF5
	// (set) Token: 0x06006270 RID: 25200 RVA: 0x002317FD File Offset: 0x0022FBFD
	public string verticalAxis
	{
		get
		{
			return this.m_VerticalAxis;
		}
		set
		{
			this.m_VerticalAxis = value;
		}
	}

	// Token: 0x06006271 RID: 25201 RVA: 0x00231806 File Offset: 0x0022FC06
	public override bool IsModuleSupported()
	{
		return true;
	}

	// Token: 0x06006272 RID: 25202 RVA: 0x0023180C File Offset: 0x0022FC0C
	public override bool ShouldActivateModule()
	{
		if (!base.ShouldActivateModule())
		{
			return false;
		}
		bool flag = this.m_ForceModuleActive;
		flag |= this.buttonDown[0];
		flag |= this.buttonDown[1];
		flag |= !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0f);
		flag |= !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0f);
		return flag | (this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f;
	}

	// Token: 0x06006273 RID: 25203 RVA: 0x002318A0 File Offset: 0x0022FCA0
	public override void ActivateModule()
	{
		base.ActivateModule();
		this.m_MousePosition = this.m_CursorPosition;
		this.m_LastMousePosition = this.m_CursorPosition;
		GameObject gameObject = base.eventSystem.currentSelectedGameObject;
		if (gameObject == null)
		{
			gameObject = base.eventSystem.firstSelectedGameObject;
		}
		base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
	}

	// Token: 0x06006274 RID: 25204 RVA: 0x00231901 File Offset: 0x0022FD01
	public override void DeactivateModule()
	{
		base.DeactivateModule();
		base.ClearSelection();
	}

	// Token: 0x06006275 RID: 25205 RVA: 0x00231910 File Offset: 0x0022FD10
	public override void Process()
	{
		bool flag = this.SendUpdateEventToSelectedObject();
		if (base.eventSystem.sendNavigationEvents)
		{
			if (!flag)
			{
				flag |= this.SendMoveEventToSelectedObject();
			}
			if (!flag)
			{
				this.SendSubmitEventToSelectedObject();
			}
		}
		this.ProcessMouseEvent();
	}

	// Token: 0x06006276 RID: 25206 RVA: 0x00231958 File Offset: 0x0022FD58
	protected bool SendSubmitEventToSelectedObject()
	{
		if (base.eventSystem.currentSelectedGameObject == null)
		{
			return false;
		}
		BaseEventData baseEventData = this.GetBaseEventData();
		return baseEventData.used;
	}

	// Token: 0x06006277 RID: 25207 RVA: 0x0023198C File Offset: 0x0022FD8C
	private Vector2 GetRawMoveVector()
	{
		Vector2 zero = Vector2.zero;
		zero.x = Input.GetAxisRaw(this.m_HorizontalAxis);
		zero.y = Input.GetAxisRaw(this.m_VerticalAxis);
		if (Input.GetButtonDown(this.m_HorizontalAxis))
		{
			if (zero.x < 0f)
			{
				zero.x = -1f;
			}
			if (zero.x > 0f)
			{
				zero.x = 1f;
			}
		}
		if (Input.GetButtonDown(this.m_VerticalAxis))
		{
			if (zero.y < 0f)
			{
				zero.y = -1f;
			}
			if (zero.y > 0f)
			{
				zero.y = 1f;
			}
		}
		return zero;
	}

	// Token: 0x06006278 RID: 25208 RVA: 0x00231A58 File Offset: 0x0022FE58
	protected bool SendMoveEventToSelectedObject()
	{
		float unscaledTime = Time.unscaledTime;
		Vector2 rawMoveVector = this.GetRawMoveVector();
		if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
		{
			this.m_ConsecutiveMoveCount = 0;
			return false;
		}
		bool flag = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
		bool flag2 = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
		if (!flag)
		{
			if (flag2 && this.m_ConsecutiveMoveCount == 1)
			{
				flag = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
			}
			else
			{
				flag = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
			}
		}
		if (!flag)
		{
			return false;
		}
		AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
		ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
		if (!flag2)
		{
			this.m_ConsecutiveMoveCount = 0;
		}
		this.m_ConsecutiveMoveCount++;
		this.m_PrevActionTime = unscaledTime;
		this.m_LastMoveVector = rawMoveVector;
		return axisEventData.used;
	}

	// Token: 0x06006279 RID: 25209 RVA: 0x00231B8C File Offset: 0x0022FF8C
	protected static RaycastResult VRCFindFirstRaycast(List<RaycastResult> candidates)
	{
		RaycastResult result = default(RaycastResult);
		result.distance = 1000000f;
		for (int i = 0; i < candidates.Count; i++)
		{
			if (!(candidates[i].gameObject == null))
			{
				if (candidates[i].distance < result.distance - 0.001f)
				{
					result = candidates[i];
				}
			}
		}
		return result;
	}

	// Token: 0x0600627A RID: 25210 RVA: 0x00231C10 File Offset: 0x00230010
	protected virtual PointerInputModule.MouseState VRCGetMousePointerEventData()
	{
		PointerEventData pointerEventData;
		bool pointerData = base.GetPointerData(-1, out pointerEventData, true);
		pointerEventData.Reset();
		if (pointerData)
		{
			pointerEventData.position = this.m_CursorPosition;
		}
		Vector2 cursorPosition = this.m_CursorPosition;
		pointerEventData.delta = cursorPosition - pointerEventData.position;
		pointerEventData.position = cursorPosition;
		Vector2 scrollDelta;
		scrollDelta.x = Input.GetAxis(this.m_HorizontalAxis) * 1500f * Time.deltaTime;
		scrollDelta.y = Input.GetAxis(this.m_VerticalAxis) * 1500f * Time.deltaTime;
		if (Mathf.Abs(scrollDelta.x) > Mathf.Abs(scrollDelta.y))
		{
			scrollDelta.y = 0f;
		}
		else
		{
			scrollDelta.x = 0f;
		}
		pointerEventData.scrollDelta = scrollDelta;
		pointerEventData.button = PointerEventData.InputButton.Left;
		base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
		RaycastResult pointerCurrentRaycast = VRCStandaloneInputModule.VRCFindFirstRaycast(this.m_RaycastResultCache);
		pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
		this.m_RaycastResultCache.Clear();
		PointerEventData pointerEventData2;
		base.GetPointerData(-2, out pointerEventData2, true);
		base.CopyFromTo(pointerEventData, pointerEventData2);
		pointerEventData2.button = PointerEventData.InputButton.Right;
		this.m_VRCMouseState.SetButtonState(PointerEventData.InputButton.Left, this.VRCStateForMouseButton(0), pointerEventData);
		this.m_VRCMouseState.SetButtonState(PointerEventData.InputButton.Right, this.VRCStateForMouseButton(1), pointerEventData2);
		return this.m_VRCMouseState;
	}

	// Token: 0x0600627B RID: 25211 RVA: 0x00231D64 File Offset: 0x00230164
	protected PointerEventData.FramePressState VRCStateForMouseButton(int buttonId)
	{
		if (buttonId >= 2)
		{
			return PointerEventData.FramePressState.NotChanged;
		}
		bool flag = this.buttonDown[buttonId];
		bool flag2 = this.buttonUp[buttonId];
		if (flag && flag2)
		{
			return PointerEventData.FramePressState.PressedAndReleased;
		}
		if (flag)
		{
			return PointerEventData.FramePressState.Pressed;
		}
		if (flag2)
		{
			return PointerEventData.FramePressState.Released;
		}
		return PointerEventData.FramePressState.NotChanged;
	}

	// Token: 0x0600627C RID: 25212 RVA: 0x00231DAB File Offset: 0x002301AB
	private static bool UseMouse(bool pressed, bool released, PointerEventData pointerData)
	{
		return pressed || released || pointerData.IsPointerMoving() || pointerData.IsScrolling();
	}

	// Token: 0x0600627D RID: 25213 RVA: 0x00231DD4 File Offset: 0x002301D4
	protected void ProcessMouseEvent()
	{
		PointerInputModule.MouseState mouseState = this.VRCGetMousePointerEventData();
		PointerInputModule.MouseButtonEventData eventData = mouseState.GetButtonState(PointerEventData.InputButton.Left).eventData;
		this.overSelection = null;
		if (eventData.buttonData.pointerCurrentRaycast.gameObject != null)
		{
			this.overSelection = eventData.buttonData.pointerCurrentRaycast.gameObject.GetComponentInParent<Selectable>();
		}
		this.ProcessMousePress(eventData, true);
		this.ProcessMove(eventData.buttonData);
		this.ProcessDrag(eventData.buttonData);
		PointerInputModule.MouseButtonEventData eventData2 = mouseState.GetButtonState(PointerEventData.InputButton.Right).eventData;
		eventData2.buttonData.button = PointerEventData.InputButton.Left;
		this.ProcessMousePress(eventData2, false);
		this.ProcessMove(eventData2.buttonData);
		this.ProcessDrag(eventData2.buttonData);
		if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
		{
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
			ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, eventData.buttonData, ExecuteEvents.scrollHandler);
		}
	}

	// Token: 0x0600627E RID: 25214 RVA: 0x00231EE8 File Offset: 0x002302E8
	protected bool SendUpdateEventToSelectedObject()
	{
		if (base.eventSystem.currentSelectedGameObject == null)
		{
			return false;
		}
		BaseEventData baseEventData = this.GetBaseEventData();
		ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
		return baseEventData.used;
	}

	// Token: 0x0600627F RID: 25215 RVA: 0x00231F34 File Offset: 0x00230334
	protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data, bool sendEvents)
	{
		PointerEventData buttonData = data.buttonData;
		GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
		if (data.PressedThisFrame())
		{
			buttonData.eligibleForClick = true;
			buttonData.delta = Vector2.zero;
			buttonData.dragging = false;
			buttonData.useDragThreshold = true;
			buttonData.pressPosition = this.m_CursorPosition;
			this.pressWorldPos[buttonData] = this.m_CursorWorldPos;
			buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
			base.DeselectIfSelectionChanged(gameObject, buttonData);
			GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
			if (gameObject2 == null)
			{
				gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
			}
			float unscaledTime = Time.unscaledTime;
			if (gameObject2 == buttonData.lastPress)
			{
				float num = unscaledTime - buttonData.clickTime;
				if (num < 0.3f)
				{
					buttonData.clickCount++;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.clickTime = unscaledTime;
			}
			else
			{
				buttonData.clickCount = 1;
			}
			buttonData.pointerPress = gameObject2;
			buttonData.rawPointerPress = gameObject;
			buttonData.clickTime = unscaledTime;
			buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
			if (buttonData.pointerDrag != null)
			{
				ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
			}
		}
		if (data.ReleasedThisFrame())
		{
			ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
			if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
			{
				if (sendEvents)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
			}
			else if (buttonData.pointerDrag != null && buttonData.dragging)
			{
				ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
			}
			buttonData.eligibleForClick = false;
			buttonData.pointerPress = null;
			buttonData.rawPointerPress = null;
			if (buttonData.pointerDrag != null && buttonData.dragging)
			{
				ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
			}
			buttonData.dragging = false;
			buttonData.pointerDrag = null;
			if (gameObject != buttonData.pointerEnter)
			{
				base.HandlePointerExitAndEnter(buttonData, null);
				base.HandlePointerExitAndEnter(buttonData, gameObject);
			}
		}
	}

	// Token: 0x06006280 RID: 25216 RVA: 0x00232170 File Offset: 0x00230570
	public override void UpdateModule()
	{
		this.m_LastMousePosition = this.m_MousePosition;
		this.m_MousePosition = this.m_CursorPosition;
	}

	// Token: 0x06006281 RID: 25217 RVA: 0x0023218A File Offset: 0x0023058A
	public void SetInputState(Vector2 screenPos, Vector3 worldPos, bool btn1down, bool btn1up, bool btn2down, bool btn2up)
	{
		this.m_CursorPosition = screenPos;
		this.m_CursorWorldPos = worldPos;
		this.buttonDown[0] = btn1down;
		this.buttonDown[1] = btn2down;
		this.buttonUp[0] = btn1up;
		this.buttonUp[1] = btn2up;
	}

	// Token: 0x06006282 RID: 25218 RVA: 0x002321C1 File Offset: 0x002305C1
	public bool IsOverSelection()
	{
		return this.overSelection != null;
	}

	// Token: 0x06006283 RID: 25219 RVA: 0x002321D0 File Offset: 0x002305D0
	private static bool ShouldStartDrag(Vector3 pressPos, Vector3 currentPos, float threshold, bool useDragThreshold)
	{
		return !useDragThreshold || (pressPos - currentPos).sqrMagnitude >= 0.002f;
	}

	// Token: 0x06006284 RID: 25220 RVA: 0x00232200 File Offset: 0x00230600
	protected override void ProcessDrag(PointerEventData pointerEvent)
	{
		bool flag = pointerEvent.IsPointerMoving();
		if (flag && pointerEvent.pointerDrag != null && !pointerEvent.dragging && VRCStandaloneInputModule.ShouldStartDrag(this.pressWorldPos[pointerEvent], this.m_CursorWorldPos, (float)base.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
		{
			ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
			pointerEvent.dragging = true;
		}
		if (pointerEvent.dragging && flag && pointerEvent.pointerDrag != null)
		{
			if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
			}
			ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
		}
	}

	// Token: 0x040047DA RID: 18394
	private float m_PrevActionTime;

	// Token: 0x040047DB RID: 18395
	private Vector2 m_LastMoveVector;

	// Token: 0x040047DC RID: 18396
	private int m_ConsecutiveMoveCount;

	// Token: 0x040047DD RID: 18397
	private Vector2 m_LastMousePosition;

	// Token: 0x040047DE RID: 18398
	private Vector2 m_MousePosition;

	// Token: 0x040047DF RID: 18399
	[SerializeField]
	private string m_HorizontalAxis = "Horizontal";

	// Token: 0x040047E0 RID: 18400
	[SerializeField]
	private string m_VerticalAxis = "Vertical";

	// Token: 0x040047E1 RID: 18401
	[SerializeField]
	private float m_InputActionsPerSecond = 10f;

	// Token: 0x040047E2 RID: 18402
	[SerializeField]
	private float m_RepeatDelay = 0.5f;

	// Token: 0x040047E3 RID: 18403
	[SerializeField]
	[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
	private bool m_ForceModuleActive;

	// Token: 0x040047E4 RID: 18404
	private readonly PointerInputModule.MouseState m_VRCMouseState = new PointerInputModule.MouseState();

	// Token: 0x040047E5 RID: 18405
	public Selectable overSelection;

	// Token: 0x040047E6 RID: 18406
	public Vector2 m_CursorPosition;

	// Token: 0x040047E7 RID: 18407
	public Vector3 m_CursorWorldPos;

	// Token: 0x040047E8 RID: 18408
	public bool[] buttonDown = new bool[2];

	// Token: 0x040047E9 RID: 18409
	public bool[] buttonUp = new bool[2];

	// Token: 0x040047EA RID: 18410
	private Dictionary<PointerEventData, Vector3> pressWorldPos = new Dictionary<PointerEventData, Vector3>();

	// Token: 0x02000C62 RID: 3170
	[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
	public enum InputMode
	{
		// Token: 0x040047EC RID: 18412
		Mouse,
		// Token: 0x040047ED RID: 18413
		Buttons
	}
}
