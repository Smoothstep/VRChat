using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C41 RID: 3137
public class UiControllerInputModule : BaseInputModule
{
	// Token: 0x0600615D RID: 24925 RVA: 0x00225F9F File Offset: 0x0022439F
	protected UiControllerInputModule()
	{
	}

	// Token: 0x0600615E RID: 24926 RVA: 0x00225FB4 File Offset: 0x002243B4
	protected override void Start()
	{
		base.Start();
		this.inAxisHorizontal = VRCInputManager.FindInput("Horizontal");
		this.inAxisVertical = VRCInputManager.FindInput("Vertical");
		this.inSubmit = VRCInputManager.FindInput("Select");
		this.inCancel = VRCInputManager.FindInput("Back");
	}

	// Token: 0x17000DA1 RID: 3489
	// (get) Token: 0x0600615F RID: 24927 RVA: 0x00226007 File Offset: 0x00224407
	// (set) Token: 0x06006160 RID: 24928 RVA: 0x0022600F File Offset: 0x0022440F
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

	// Token: 0x06006161 RID: 24929 RVA: 0x00226018 File Offset: 0x00224418
	public override bool IsModuleSupported()
	{
		return VRCInputManager.IsSupported(VRCInputManager.InputMethod.Controller) || VRCInputManager.IsSupported(VRCInputManager.InputMethod.Keyboard);
	}

	// Token: 0x06006162 RID: 24930 RVA: 0x00226030 File Offset: 0x00224430
	public override bool ShouldActivateModule()
	{
		if (this.disableControllerNavigation)
		{
			return false;
		}
		if (!base.ShouldActivateModule())
		{
			return false;
		}
		if (!VRCInputManager.IsEnabled(VRCInputManager.InputMethod.Controller) && !VRCInputManager.IsEnabled(VRCInputManager.InputMethod.Keyboard))
		{
			return false;
		}
		if (!VRCUiManager.Instance.IsActive())
		{
			return false;
		}
		bool flag = this.inSubmit.button;
		flag |= this.inCancel.button;
		return flag | !Mathf.Approximately(this.inAxisHorizontal.axis, 0f);
	}

	// Token: 0x06006163 RID: 24931 RVA: 0x002260B6 File Offset: 0x002244B6
	public override void ActivateModule()
	{
		base.ActivateModule();
		VRCUiCursorManager.SetActiveCursor(VRCUiCursorManager.CursorType.None);
	}

	// Token: 0x06006164 RID: 24932 RVA: 0x002260C4 File Offset: 0x002244C4
	public override void Process()
	{
		if (base.eventSystem.currentSelectedGameObject == null || !base.eventSystem.currentSelectedGameObject.activeInHierarchy || base.eventSystem.currentSelectedGameObject.GetComponent<Selectable>() == null)
		{
			Selectable[] componentsInChildren = base.transform.parent.gameObject.GetComponentsInChildren<Selectable>();
			foreach (Selectable selectable in componentsInChildren)
			{
				if (selectable.navigation.mode != Navigation.Mode.None)
				{
					base.eventSystem.SetSelectedGameObject(selectable.gameObject);
					break;
				}
			}
		}
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
	}

	// Token: 0x06006165 RID: 24933 RVA: 0x002261AC File Offset: 0x002245AC
	private bool SendSubmitEventToSelectedObject()
	{
		if (base.eventSystem.currentSelectedGameObject == null)
		{
			return false;
		}
		BaseEventData baseEventData = this.GetBaseEventData();
		if (this.inSubmit.down)
		{
			ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
		}
		if (this.inCancel.down)
		{
			ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
		}
		return baseEventData.used;
	}

	// Token: 0x06006166 RID: 24934 RVA: 0x0022622C File Offset: 0x0022462C
	private bool AllowMoveEventProcessing(float time)
	{
		return time > this.m_NextAction;
	}

	// Token: 0x06006167 RID: 24935 RVA: 0x00226244 File Offset: 0x00224644
	private Vector2 GetRawMoveVector()
	{
		Vector2 zero = Vector2.zero;
		zero.x = this.inAxisHorizontal.axis;
		zero.y = this.inAxisVertical.axis;
		return zero;
	}

	// Token: 0x06006168 RID: 24936 RVA: 0x0022627C File Offset: 0x0022467C
	private bool SendMoveEventToSelectedObject()
	{
		float unscaledTime = Time.unscaledTime;
		if (!this.AllowMoveEventProcessing(unscaledTime))
		{
			return false;
		}
		Vector2 rawMoveVector = this.GetRawMoveVector();
		AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
		if (!Mathf.Approximately(axisEventData.moveVector.x, 0f) || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
		{
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
		}
		this.m_NextAction = unscaledTime + 1f / this.m_InputActionsPerSecond;
		return axisEventData.used;
	}

	// Token: 0x06006169 RID: 24937 RVA: 0x0022632C File Offset: 0x0022472C
	private bool SendUpdateEventToSelectedObject()
	{
		if (base.eventSystem.currentSelectedGameObject == null)
		{
			return false;
		}
		BaseEventData baseEventData = this.GetBaseEventData();
		ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
		return baseEventData.used;
	}

	// Token: 0x04004702 RID: 18178
	public bool disableControllerNavigation;

	// Token: 0x04004703 RID: 18179
	private float m_NextAction;

	// Token: 0x04004704 RID: 18180
	private VRCInput inAxisHorizontal;

	// Token: 0x04004705 RID: 18181
	private VRCInput inAxisVertical;

	// Token: 0x04004706 RID: 18182
	private VRCInput inSubmit;

	// Token: 0x04004707 RID: 18183
	private VRCInput inCancel;

	// Token: 0x04004708 RID: 18184
	[SerializeField]
	private float m_InputActionsPerSecond = 10f;
}
