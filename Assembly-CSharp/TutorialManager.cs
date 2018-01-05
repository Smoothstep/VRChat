using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B03 RID: 2819
public class TutorialManager : MonoBehaviour
{
	// Token: 0x06005543 RID: 21827 RVA: 0x001D67D8 File Offset: 0x001D4BD8
	public static void VRC_Tutorial_ActivateObjectLabel(Transform targetObject, TutorialLabelType type, ControllerHand hand, string text, ControllerActionUI action, string textSecondary, ControllerActionUI actionSecondary, float duration, int priority, AttachMode attachMode, bool showOffscreen)
	{
		if (TutorialManager.Instance != null)
		{
			TutorialManager.Instance.ActivateObjectLabel(targetObject, type, hand, text, action, textSecondary, actionSecondary, duration, priority, attachMode, showOffscreen);
		}
	}

	// Token: 0x06005544 RID: 21828 RVA: 0x001D6811 File Offset: 0x001D4C11
	public static void VRC_Tutorial_DeactivateObjectLabel(Transform targetObject)
	{
		if (TutorialManager.Instance != null)
		{
			TutorialManager.Instance.DeactivateObjectLabel(targetObject);
		}
	}

	// Token: 0x06005545 RID: 21829 RVA: 0x001D682E File Offset: 0x001D4C2E
	public static void VRC_Tutorial_ActivateControllerLabel(ControllerHand hand, ControllerInputUI controllerPart, string text, float duration, int priority)
	{
		if (TutorialManager.Instance != null)
		{
			TutorialManager.Instance.ActivateControllerLabel(hand, controllerPart, text, duration, priority);
		}
	}

	// Token: 0x06005546 RID: 21830 RVA: 0x001D6850 File Offset: 0x001D4C50
	public static void VRC_Tutorial_DeactivateControllerLabel(ControllerHand hand, ControllerInputUI controllerPart)
	{
		if (TutorialManager.Instance != null)
		{
			TutorialManager.Instance.DeactivateControllerLabel(hand, controllerPart);
		}
	}

	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x06005547 RID: 21831 RVA: 0x001D686E File Offset: 0x001D4C6E
	public static TutorialManager Instance
	{
		get
		{
			return TutorialManager._instance;
		}
	}

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x06005548 RID: 21832 RVA: 0x001D6875 File Offset: 0x001D4C75
	public bool AreLabelsEnabled
	{
		get
		{
			return VRCInputManager.showTooltips;
		}
	}

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x06005549 RID: 21833 RVA: 0x001D687C File Offset: 0x001D4C7C
	private float TimeElapsedInCurrentHelpState
	{
		get
		{
			return Time.realtimeSinceStartup - this._timeCurrentHelpStateStarted;
		}
	}

	// Token: 0x0600554A RID: 21834 RVA: 0x001D688C File Offset: 0x001D4C8C
	public void InteractableSelected(VRC_Interactable[] interactable, Component useComponent, bool leftHand)
	{
		string text = (interactable == null || interactable.Length <= 0 || string.IsNullOrEmpty(interactable[0].interactText)) ? "Use" : interactable[0].interactText;
		if (text.CompareTo("Use") == 0 && useComponent.GetComponent<VRC_Station>() != null)
		{
			text = "Sit";
		}
		if (!this.IsShowingOtherLabelOfType(TutorialLabelType.Interactable, useComponent.transform))
		{
			this.ActivateObjectLabel(useComponent.transform, TutorialLabelType.Interactable, (!leftHand) ? ControllerHand.Right : ControllerHand.Left, text, (!VRCInputManager.IsUsingHandController()) ? ControllerActionUI.Use : ControllerActionUI.None, 0.1f, 0, AttachMode.PositionOnly, false);
		}
		if (VRCInputManager.IsUsingHandController())
		{
			ControllerInputUI controllerPart = (VRCInputManager.LastInputMethod != VRCInputManager.InputMethod.Oculus) ? ControllerInputUI.Trigger : ControllerInputUI.Grip;
			this.ActivateControllerLabel((!leftHand) ? ControllerHand.Right : ControllerHand.Left, controllerPart, text, 0.1f, 0);
		}
	}

	// Token: 0x0600554B RID: 21835 RVA: 0x001D6974 File Offset: 0x001D4D74
	public void PickupSelected(VRC_Pickup pickup, bool leftHand)
	{
		if (pickup.currentlyHeldBy != null)
		{
			return;
		}
		if (VRCInputManager.IsUsingHandController())
		{
			ControllerInputUI controllerPart = (VRCInputManager.LastInputMethod != VRCInputManager.InputMethod.Oculus) ? ControllerInputUI.Trigger : ControllerInputUI.Grip;
			this.ActivateControllerLabel((!leftHand) ? ControllerHand.Right : ControllerHand.Left, controllerPart, (!VRCHandGrasper.IsAutoEquipPickup(pickup)) ? "Hold to Grab" : "Equip", 0.1f, 0);
		}
		else if (!this.IsShowingOtherLabelOfType(TutorialLabelType.Pickup, pickup.transform))
		{
			this.ActivateObjectLabel(pickup.transform, TutorialLabelType.Pickup, (!leftHand) ? ControllerHand.Right : ControllerHand.Left, (!VRCInputManager.IsUsingAutoEquipControllerType() || !VRCHandGrasper.IsAutoEquipPickup(pickup)) ? "Hold to Grab" : "Equip", ControllerActionUI.Use, 0.1f, 0, AttachMode.PositionOnly, false);
		}
	}

	// Token: 0x0600554C RID: 21836 RVA: 0x001D6A44 File Offset: 0x001D4E44
	public void QuickMenuOpened(bool opened)
	{
		if (opened)
		{
			string key = (User.CurrentUser == null) ? string.Empty : (User.CurrentUser.id + "_OpenedQuickMenu");
			PlayerPrefs.SetInt(key, Mathf.Min(PlayerPrefs.GetInt(key, 0) + 1, 10));
		}
		this.DeactivateLabel("OpenMenu");
		this.DeactivateLabel("SelectPeople");
		if (this._helpState == TutorialManager.ContextualHelpState.ShowUnmuteHelp || this._helpState == TutorialManager.ContextualHelpState.ShowOpenMenuHelp)
		{
			this.SelectNextContextualHelpState();
		}
	}

	// Token: 0x0600554D RID: 21837 RVA: 0x001D6ACA File Offset: 0x001D4ECA
	public void PlayerSelected()
	{
		this.DeactivateLabel("SelectPeople");
	}

	// Token: 0x0600554E RID: 21838 RVA: 0x001D6AD8 File Offset: 0x001D4ED8
	public void PlayerUnmuted()
	{
		string key = (User.CurrentUser == null) ? string.Empty : (User.CurrentUser.id + "_HasUnmutedSomeone");
		PlayerPrefs.SetInt(key, Mathf.Min(PlayerPrefs.GetInt(key, 0) + 1, 10));
		if (this._helpState == TutorialManager.ContextualHelpState.ShowUnmuteHelp)
		{
			this.DeactivateLabel("UnmuteButton");
			this.SelectNextContextualHelpState();
		}
	}

	// Token: 0x0600554F RID: 21839 RVA: 0x001D6B41 File Offset: 0x001D4F41
	public TutorialManager.ContextualHelpState GetCurrentContextualHelpState()
	{
		return this._helpState;
	}

	// Token: 0x06005550 RID: 21840 RVA: 0x001D6B49 File Offset: 0x001D4F49
	private void InitHelp()
	{
		this._isInTutorialRoom = RoomManager.currentRoom.name.ToLowerInvariant().Contains("tutorial");
		this._helpState = TutorialManager.ContextualHelpState.None;
	}

	// Token: 0x06005551 RID: 21841 RVA: 0x001D6B74 File Offset: 0x001D4F74
	private void UpdateContextualHelp()
	{
		if (VRCPlayer.Instance == null || RoomManager.currentRoom == null)
		{
			return;
		}
		if (!this._initializedHelp && this._timeEnteredWorld >= 0f && Time.realtimeSinceStartup - this._timeEnteredWorld > 3f)
		{
			this.InitHelp();
			this._initializedHelp = true;
		}
		if (!this._initializedHelp)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._lastTimeHelpStateChecked > this.RefreshHelpStateInterval)
		{
			this.SelectNextContextualHelpState();
			this._lastTimeHelpStateChecked = Time.realtimeSinceStartup;
		}
		switch (this._helpState)
		{
		case TutorialManager.ContextualHelpState.ShowOpenMenuHelp:
			if (this._openMenuHelpStartTime < 0f)
			{
				this._openMenuHelpStartTime = Time.realtimeSinceStartup;
			}
			if (QuickMenu.Instance != null && !QuickMenu.Instance.IsActive)
			{
				if (Time.realtimeSinceStartup - this._openMenuHelpStartTime < this.FirstTimeHelpDuration)
				{
					if (!this.IsLabelActive("OpenMenu"))
					{
						this.ShowOpenMenuTooltip("OpenMenu");
					}
				}
				else
				{
					this._openMenuHelpComplete = true;
					this.SelectNextContextualHelpState();
				}
			}
			if (QuickMenu.Instance != null && QuickMenu.Instance.IsActive)
			{
				this._openMenuHelpComplete = true;
				this.SelectNextContextualHelpState();
			}
			break;
		case TutorialManager.ContextualHelpState.ShowUnmuteHelp:
			if (!VRCUiManager.Instance.HasQueuedHudMessages())
			{
				if (this._unmuteHelpStartTime < 0f)
				{
					this._unmuteHelpStartTime = Time.realtimeSinceStartup;
				}
				if (QuickMenu.Instance != null && !QuickMenu.Instance.IsActive)
				{
					if (Time.realtimeSinceStartup - this._unmuteHelpStartTime < this.FirstTimeHelpDuration)
					{
						if (!this.IsLabelActive("OpenMenu"))
						{
							this.ShowOpenMenuTooltip("OpenMenu");
						}
					}
					else
					{
						this._unmuteHelpComplete = true;
						this.SelectNextContextualHelpState();
					}
				}
				if (QuickMenu.Instance != null && QuickMenu.Instance.IsActive)
				{
					if (QuickMenu.Instance.SelectedUser == null)
					{
						if (!this.IsLabelActive("SelectPeople"))
						{
							this.ShowUISelectTooltip("SelectPeople", !QuickMenu.Instance.IsOnRightHand);
						}
					}
					else if (ModerationManager.Instance.IsMuted(QuickMenu.Instance.SelectedUser.id) && !this.IsLabelActive("UnmuteButton"))
					{
						this.ShowUnmuteButtonHighlight("UnmuteButton");
					}
				}
			}
			break;
		case TutorialManager.ContextualHelpState.ShowMuteHelp:
			if (!VRCUiManager.Instance.HasQueuedHudMessages())
			{
				if (this._muteHelpStartTime < 0f)
				{
					this._muteHelpStartTime = Time.realtimeSinceStartup;
				}
				if (QuickMenu.Instance != null && !QuickMenu.Instance.IsActive)
				{
					if (Time.realtimeSinceStartup - this._muteHelpStartTime < this.FirstTimeHelpDuration)
					{
						if (!this.IsLabelActive("OpenMenu"))
						{
							this.ShowOpenMenuTooltip("OpenMenu");
						}
					}
					else
					{
						this._muteHelpComplete = true;
						this.SelectNextContextualHelpState();
					}
				}
				if (QuickMenu.Instance != null && QuickMenu.Instance.IsActive)
				{
					this._muteHelpComplete = true;
					this.SelectNextContextualHelpState();
				}
			}
			break;
		}
	}

	// Token: 0x06005552 RID: 21842 RVA: 0x001D6EC8 File Offset: 0x001D52C8
	private void SelectNextContextualHelpState()
	{
		if (!this._initializedHelp)
		{
			return;
		}
		TutorialManager.ContextualHelpState helpState = this._helpState;
		switch (this._helpState)
		{
		case TutorialManager.ContextualHelpState.None:
		case TutorialManager.ContextualHelpState.ShowOpenMenuHelp:
		case TutorialManager.ContextualHelpState.ShowUnmuteHelp:
		case TutorialManager.ContextualHelpState.ShowMuteHelp:
			if (this.ShouldShowUnmuteHelp())
			{
				this._helpState = TutorialManager.ContextualHelpState.ShowUnmuteHelp;
			}
			else if (this.ShouldShowMuteHelp())
			{
				this._helpState = TutorialManager.ContextualHelpState.ShowMuteHelp;
			}
			else if (this.ShouldShowOpenMenuHelp())
			{
				this._helpState = TutorialManager.ContextualHelpState.ShowOpenMenuHelp;
			}
			else
			{
				this._helpState = TutorialManager.ContextualHelpState.None;
			}
			break;
		}
		if (helpState != this._helpState)
		{
			this.ExitContextualHelpState(helpState);
			this.EnterContextualHelpState(this._helpState);
			this._timeCurrentHelpStateStarted = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x06005553 RID: 21843 RVA: 0x001D6F88 File Offset: 0x001D5388
	private void EnterContextualHelpState(TutorialManager.ContextualHelpState state)
	{
		if (state != TutorialManager.ContextualHelpState.ShowUnmuteHelp)
		{
			if (state != TutorialManager.ContextualHelpState.ShowMuteHelp)
			{
				if (state == TutorialManager.ContextualHelpState.ShowOpenMenuHelp)
				{
					this._openMenuHelpStartTime = -1f;
					this._openMenuHelpComplete = false;
				}
			}
			else
			{
				this._muteHelpStartTime = -1f;
				this._muteHelpComplete = false;
				VRCUiManager.Instance.QueueHudMessage("Welcome, " + User.CurrentUser.displayName + ".\nIf you encounter an annoying user, you can mute them. Bring up your menu, aim your pointer at them, select them, and select the sound icon.");
			}
		}
		else
		{
			this._unmuteHelpStartTime = -1f;
			this._unmuteHelpComplete = false;
			VRCUiManager.Instance.QueueHudMessage("Welcome, " + User.CurrentUser.displayName + ".\nTo hear other users, unmute them. Bring up your menu, aim your pointer at them, select them, and select the sound icon.");
		}
	}

	// Token: 0x06005554 RID: 21844 RVA: 0x001D703C File Offset: 0x001D543C
	private void ExitContextualHelpState(TutorialManager.ContextualHelpState state)
	{
		if (state != TutorialManager.ContextualHelpState.ShowUnmuteHelp)
		{
			if (state != TutorialManager.ContextualHelpState.ShowMuteHelp)
			{
				if (state == TutorialManager.ContextualHelpState.ShowOpenMenuHelp)
				{
					this.IncrementOpenMenuHelpShownCount();
					this.DeactivateLabel("OpenMenu");
				}
			}
			else
			{
				this.IncrementMuteHelpShownCount();
				this.IncrementOpenMenuHelpShownCount();
				this.DeactivateLabel("OpenMenu");
			}
		}
		else
		{
			this.IncrementUnmuteHelpShownCount();
			this.IncrementOpenMenuHelpShownCount();
			this.DeactivateLabel("OpenMenu");
			this.DeactivateLabel("SelectPeople");
			this.DeactivateLabel("UnmuteButton");
		}
	}

	// Token: 0x06005555 RID: 21845 RVA: 0x001D70C8 File Offset: 0x001D54C8
	private void IncrementOpenMenuHelpShownCount()
	{
		if (TutorialManager.HasSeenOpenMenuHelpThisSession)
		{
			return;
		}
		TutorialManager.HasSeenOpenMenuHelpThisSession = true;
		string key = (User.CurrentUser == null) ? string.Empty : (User.CurrentUser.id + "_OpenMenuHelpShownCount");
		PlayerPrefs.SetInt(key, Mathf.Min(PlayerPrefs.GetInt(key, 0) + 1, 10));
	}

	// Token: 0x06005556 RID: 21846 RVA: 0x001D7128 File Offset: 0x001D5528
	private int GetOpenMenuHelpShownCount()
	{
		string key = (User.CurrentUser == null) ? string.Empty : (User.CurrentUser.id + "_OpenMenuHelpShownCount");
		return PlayerPrefs.GetInt(key, 0);
	}

	// Token: 0x06005557 RID: 21847 RVA: 0x001D7168 File Offset: 0x001D5568
	private void IncrementMuteHelpShownCount()
	{
		if (TutorialManager.HasSeenMuteHelpThisSession)
		{
			return;
		}
		TutorialManager.HasSeenMuteHelpThisSession = true;
		string key = (User.CurrentUser == null) ? string.Empty : (User.CurrentUser.id + "_MuteHelpShownCount");
		PlayerPrefs.SetInt(key, Mathf.Min(PlayerPrefs.GetInt(key, 0) + 1, 10));
	}

	// Token: 0x06005558 RID: 21848 RVA: 0x001D71C8 File Offset: 0x001D55C8
	private int GetMuteHelpShownCount()
	{
		string key = (User.CurrentUser == null) ? string.Empty : (User.CurrentUser.id + "_MuteHelpShownCount");
		return PlayerPrefs.GetInt(key, 0);
	}

	// Token: 0x06005559 RID: 21849 RVA: 0x001D7208 File Offset: 0x001D5608
	private void IncrementUnmuteHelpShownCount()
	{
		if (TutorialManager.HasSeenUnmuteHelpThisSession)
		{
			return;
		}
		TutorialManager.HasSeenUnmuteHelpThisSession = true;
		string key = (User.CurrentUser == null) ? string.Empty : (User.CurrentUser.id + "_UnmuteHelpShownCount");
		PlayerPrefs.SetInt(key, Mathf.Min(PlayerPrefs.GetInt(key, 0) + 1, 10));
	}

	// Token: 0x0600555A RID: 21850 RVA: 0x001D7268 File Offset: 0x001D5668
	private int GetUnmuteHelpShownCount()
	{
		string key = (User.CurrentUser == null) ? string.Empty : (User.CurrentUser.id + "_UnmuteHelpShownCount");
		return PlayerPrefs.GetInt(key, 0);
	}

	// Token: 0x0600555B RID: 21851 RVA: 0x001D72A8 File Offset: 0x001D56A8
	private bool ShouldShowUnmuteHelp()
	{
		return !this._unmuteHelpComplete && !this._isInTutorialRoom && !TutorialManager.HasSeenUnmuteHelpThisSession && this.GetUnmuteHelpShownCount() < this.FirstTimeHelpShowCount && VRCInputManager.defaultMute && !this.HasUnmutedEnoughPeople() && this.AreMutedUsersInRoom();
	}

	// Token: 0x0600555C RID: 21852 RVA: 0x001D730C File Offset: 0x001D570C
	private bool HasUnmutedEnoughPeople()
	{
		string key = (User.CurrentUser == null) ? string.Empty : (User.CurrentUser.id + "_HasUnmutedSomeone");
		return PlayerPrefs.GetInt(key, 0) >= 1;
	}

	// Token: 0x0600555D RID: 21853 RVA: 0x001D7350 File Offset: 0x001D5750
	private bool AreMutedUsersInRoom()
	{
		VRC.Player[] allPlayers = PlayerManager.GetAllPlayers();
		foreach (VRC.Player player in allPlayers)
		{
			if (player.user != null && !player.isVIP && ModerationManager.Instance.IsMuted(player.userId) && !ModerationManager.Instance.IsBlockedEitherWay(player.userId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600555E RID: 21854 RVA: 0x001D73C0 File Offset: 0x001D57C0
	private bool ShouldShowMuteHelp()
	{
		return !this._muteHelpComplete && !this._isInTutorialRoom && !TutorialManager.HasSeenMuteHelpThisSession && this.GetMuteHelpShownCount() < 1 && !VRCInputManager.defaultMute;
	}

	// Token: 0x0600555F RID: 21855 RVA: 0x001D740C File Offset: 0x001D580C
	private bool ShouldShowOpenMenuHelp()
	{
		return !this._openMenuHelpComplete && !this._isInTutorialRoom && !TutorialManager.HasSeenOpenMenuHelpThisSession && this.GetOpenMenuHelpShownCount() < this.FirstTimeHelpShowCount;
	}

	// Token: 0x06005560 RID: 21856 RVA: 0x001D7448 File Offset: 0x001D5848
	private void ShowOpenMenuTooltip(string id)
	{
		if (VRCInputManager.IsUsingHandController())
		{
			if (VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Vive)
			{
				this.ActivateControllerLabel(id, ControllerHand.Left, ControllerInputUI.MenuButton, "Open Menu", -1f, -1);
			}
			if (VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Oculus)
			{
				this.ActivateControllerLabel(id, ControllerHand.Left, ControllerInputUI.ButtonTwo, "Open Menu", -1f, -1);
			}
		}
		else
		{
			this.ActivateObjectLabel(null, id, TutorialLabelType.PopupAttached, ControllerHand.None, "Open Menu", ControllerActionUI.UIMenu, string.Empty, ControllerActionUI.None, -1f, -1, AttachMode.PositionOnly, true);
		}
	}

	// Token: 0x06005561 RID: 21857 RVA: 0x001D74C4 File Offset: 0x001D58C4
	private void ShowUISelectTooltip(string id, bool rightHand)
	{
		if (VRCInputManager.IsUsingHandController())
		{
			this.ActivateControllerLabel(id, (!rightHand) ? ControllerHand.Left : ControllerHand.Right, ControllerInputUI.Trigger, "Aim At Player And Press To Select", -1f, -1);
		}
		else
		{
			this.ActivateObjectLabel(null, id, TutorialLabelType.PopupAttached, ControllerHand.None, "Aim At Player And Press To Select", ControllerActionUI.UISelect, string.Empty, ControllerActionUI.None, -1f, -1, AttachMode.PositionOnly, true);
		}
	}

	// Token: 0x06005562 RID: 21858 RVA: 0x001D7520 File Offset: 0x001D5920
	private void ShowUnmuteButtonHighlight(string id)
	{
		if (QuickMenu.Instance != null && QuickMenu.Instance.IsActive)
		{
			Transform transform = QuickMenu.Instance.FindUnmuteButtonIfActive();
			if (transform != null)
			{
				this.ActivateObjectLabel(transform, id, TutorialLabelType.UI, ControllerHand.None, "Unmute", (!VRCInputManager.IsUsingHandController()) ? ControllerActionUI.UISelect : ControllerActionUI.None, string.Empty, ControllerActionUI.None, -1f, -1, AttachMode.PositionOnly, true);
			}
		}
	}

	// Token: 0x06005563 RID: 21859 RVA: 0x001D7591 File Offset: 0x001D5991
	private void Awake()
	{
		if (TutorialManager._instance != null)
		{
			Debug.LogError("Duplicate TutorialManager detected");
			return;
		}
		TutorialManager._instance = this;
	}

	// Token: 0x06005564 RID: 21860 RVA: 0x001D75B4 File Offset: 0x001D59B4
	private void Start()
	{
		VRCFlowManager.Instance.onEnteredWorld += this.OnEnteredWorld;
		this.EnableControllerVisibility(ControllerHand.Left, false);
		this.EnableControllerVisibility(ControllerHand.Right, false);
	}

	// Token: 0x06005565 RID: 21861 RVA: 0x001D75DC File Offset: 0x001D59DC
	private void OnDisable()
	{
		if (this._initializedHelp)
		{
			this.ExitContextualHelpState(this._helpState);
		}
	}

	// Token: 0x06005566 RID: 21862 RVA: 0x001D75F5 File Offset: 0x001D59F5
	private void OnDestroy()
	{
		if (VRCFlowManager.Instance != null)
		{
			VRCFlowManager.Instance.onEnteredWorld -= this.OnEnteredWorld;
		}
		TutorialManager._instance = null;
	}

	// Token: 0x06005567 RID: 21863 RVA: 0x001D7623 File Offset: 0x001D5A23
	private void OnEnteredWorld()
	{
		this._timeEnteredWorld = Time.realtimeSinceStartup;
		Debug.Log("TutorialManager: entered world at " + this._timeEnteredWorld);
	}

	// Token: 0x06005568 RID: 21864 RVA: 0x001D764C File Offset: 0x001D5A4C
	private void Update()
	{
		this.UpdateContextualHelp();
		if (this.FloatingLabelTransform == null)
		{
			this.FloatingLabelTransform = new GameObject("_FloatingLabelTransform").transform;
		}
		if (this._xboxControllerIcons == null)
		{
			this._xboxControllerIcons = UnityEngine.Object.Instantiate<ControllerIconSet>(this.XboxControllerIcons);
		}
		if (this._mouseKeyboardIcons == null)
		{
			this._mouseKeyboardIcons = UnityEngine.Object.Instantiate<ControllerIconSet>(this.MouseKeyboardIcons);
		}
		foreach (KeyValuePair<int, TutorialManager.ActiveLabel> keyValuePair in this._activeLabels)
		{
			if (keyValuePair.Value.TimeToLive >= 0f)
			{
				keyValuePair.Value.TimeToLive -= Time.deltaTime;
				if (keyValuePair.Value.TimeToLive < 0f)
				{
					this.DeactivateLabel(keyValuePair.Value);
				}
			}
			if (!keyValuePair.Value.ShowOffscreen && !keyValuePair.Value.Label.CanSeeLabel())
			{
				this.DeactivateLabel(keyValuePair.Value);
			}
			if (!keyValuePair.Value.Label.IsVisible || keyValuePair.Value.TargetObject == null || keyValuePair.Value.TargetObject.gameObject == null || !keyValuePair.Value.TargetObject.gameObject.activeInHierarchy)
			{
				UnityEngine.Object.Destroy(keyValuePair.Value.Label.gameObject);
				this._keysToRemove.Add(keyValuePair.Key);
			}
		}
		foreach (int key in this._keysToRemove)
		{
			this._activeLabels.Remove(key);
		}
		this._keysToRemove.Clear();
	}

	// Token: 0x06005569 RID: 21865 RVA: 0x001D7898 File Offset: 0x001D5C98
	private void EnableControllerVisibility(ControllerHand hand, bool enable)
	{
		ControllerUI controllerUI = VRCTrackingManager.GetControllerUI(hand);
		if (controllerUI != null)
		{
			controllerUI.EnableVisibility(enable);
		}
	}

	// Token: 0x0600556A RID: 21866 RVA: 0x001D78BF File Offset: 0x001D5CBF
	private void RefreshControllerUiVisibility(ControllerHand hand)
	{
		this.EnableControllerVisibility(hand, this.ShouldShowControllerUI(hand));
	}

	// Token: 0x0600556B RID: 21867 RVA: 0x001D78D0 File Offset: 0x001D5CD0
	public void ActivateObjectLabel(Transform targetObject, TutorialLabelType type, ControllerHand hand, string text, float duration = 0.1f, int priority = 0, AttachMode attachMode = AttachMode.PositionOnly, bool showOffscreen = false)
	{
		this.ActivateObjectLabel(targetObject, type, hand, text, ControllerActionUI.None, string.Empty, ControllerActionUI.None, duration, priority, attachMode, showOffscreen);
	}

	// Token: 0x0600556C RID: 21868 RVA: 0x001D78F8 File Offset: 0x001D5CF8
	public void ActivateObjectLabel(Transform targetObject, TutorialLabelType type, ControllerHand hand, string text, ControllerActionUI action, float duration = 0.1f, int priority = 0, AttachMode attachMode = AttachMode.PositionOnly, bool showOffscreen = false)
	{
		this.ActivateObjectLabel(targetObject, type, hand, text, action, string.Empty, ControllerActionUI.None, duration, priority, attachMode, showOffscreen);
	}

	// Token: 0x0600556D RID: 21869 RVA: 0x001D7920 File Offset: 0x001D5D20
	public void ActivateObjectLabel(Transform targetObject, TutorialLabelType type, ControllerHand hand, string text, ControllerActionUI action, string textSecondary, ControllerActionUI actionSecondary, float duration = 0.1f, int priority = 0, AttachMode attachMode = AttachMode.PositionOnly, bool showOffscreen = false)
	{
		this.ActivateObjectLabel(targetObject, "<no id>", type, hand, text, action, textSecondary, actionSecondary, duration, priority, attachMode, showOffscreen);
	}

	// Token: 0x0600556E RID: 21870 RVA: 0x001D794C File Offset: 0x001D5D4C
	public void ActivateObjectLabel(Transform targetObject, string id, TutorialLabelType type, ControllerHand hand, string text, ControllerActionUI action, string textSecondary, ControllerActionUI actionSecondary, float duration = 0.1f, int priority = 0, AttachMode attachMode = AttachMode.PositionOnly, bool showOffscreen = false)
	{
		float scale = (type != TutorialLabelType.AreaMarker) ? 1f : 2f;
		this.ActivateLabel(targetObject, id, type, hand, ControllerInputUI.None, text, action, textSecondary, actionSecondary, duration, priority, null, null, attachMode, showOffscreen, scale);
	}

	// Token: 0x0600556F RID: 21871 RVA: 0x001D79A0 File Offset: 0x001D5DA0
	public void ActivateControllerLabel(ControllerHand hand, ControllerInputUI controllerPart, string text, float duration = 0.1f, int priority = 0)
	{
		this.ActivateControllerLabel("<no id>", hand, controllerPart, text, duration, priority);
	}

	// Token: 0x06005570 RID: 21872 RVA: 0x001D79B4 File Offset: 0x001D5DB4
	public void ActivateControllerLabel(string id, ControllerHand hand, ControllerInputUI controllerPart, string text, float duration = 0.1f, int priority = 0)
	{
		ControllerUI controllerUI = VRCTrackingManager.GetControllerUI(hand);
		if (controllerUI == null)
		{
			return;
		}
		Transform transform;
		Transform transform2;
		if (!controllerUI.GetUIAttachmentPoints(controllerPart, out transform, out transform2))
		{
			return;
		}
		this.ActivateLabel(transform, id, TutorialLabelType.Controller, hand, controllerPart, text, ControllerActionUI.None, string.Empty, ControllerActionUI.None, duration, priority, new Vector3?(transform.position), new Vector3?(transform2.position), AttachMode.PositionAndRotation, true, 0.8f);
		this.RefreshControllerUiVisibility(hand);
	}

	// Token: 0x06005571 RID: 21873 RVA: 0x001D7A24 File Offset: 0x001D5E24
	public bool IsLabelActive(string id)
	{
		TutorialManager.ActiveLabel activeLabel = this.FindActiveLabel(id);
		return activeLabel != null && activeLabel.Label.IsActive;
	}

	// Token: 0x06005572 RID: 21874 RVA: 0x001D7A50 File Offset: 0x001D5E50
	public bool IsLabelActiveOnTransform(Transform targetObject)
	{
		if (targetObject == null)
		{
			targetObject = this.FloatingLabelTransform;
		}
		TutorialManager.ActiveLabel activeLabel = this.FindActiveLabel(targetObject.GetInstanceID());
		return activeLabel != null && activeLabel.Label.IsActive;
	}

	// Token: 0x06005573 RID: 21875 RVA: 0x001D7A94 File Offset: 0x001D5E94
	public void DeactivateObjectLabel(Transform targetObject)
	{
		if (targetObject == null)
		{
			targetObject = this.FloatingLabelTransform;
		}
		TutorialManager.ActiveLabel activeLabel = this.FindActiveLabel(targetObject.GetInstanceID());
		if (activeLabel != null)
		{
			this.DeactivateLabel(activeLabel);
		}
	}

	// Token: 0x06005574 RID: 21876 RVA: 0x001D7AD0 File Offset: 0x001D5ED0
	public void DeactivateLabel(string id)
	{
		TutorialManager.ActiveLabel activeLabel = this.FindActiveLabel(id);
		if (activeLabel != null)
		{
			this.DeactivateLabel(activeLabel);
		}
	}

	// Token: 0x06005575 RID: 21877 RVA: 0x001D7AF4 File Offset: 0x001D5EF4
	public void DeactivateControllerLabel(ControllerHand hand, ControllerInputUI controllerPart)
	{
		foreach (TutorialManager.ActiveLabel activeLabel in this._activeLabels.Values)
		{
			if (activeLabel.Type == TutorialLabelType.Controller && activeLabel.Hand == hand && activeLabel.ControllerPart == controllerPart && activeLabel.Label.IsActive)
			{
				this.DeactivateLabel(activeLabel);
			}
		}
	}

	// Token: 0x06005576 RID: 21878 RVA: 0x001D7B8C File Offset: 0x001D5F8C
	public Material GetIconMaterialForAction(ControllerActionUI action)
	{
		ControllerIconSet controllerIconSet;
		switch (VRCInputManager.LastInputMethod)
		{
		case VRCInputManager.InputMethod.Keyboard:
		case VRCInputManager.InputMethod.Mouse:
			controllerIconSet = this._mouseKeyboardIcons;
			break;
		case VRCInputManager.InputMethod.Controller:
			controllerIconSet = this._xboxControllerIcons;
			break;
		default:
			controllerIconSet = this._mouseKeyboardIcons;
			break;
		}
		if (controllerIconSet != null)
		{
			return controllerIconSet.GetIcon(action);
		}
		return this.ErrorIconMaterial;
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x001D7BF8 File Offset: 0x001D5FF8
	private bool ActivateLabel(Transform targetObj, string id, TutorialLabelType type, ControllerHand hand, ControllerInputUI controllerPart, string text, ControllerActionUI action, string textSecondary, ControllerActionUI actionSecondary, float duration, int priority, Vector3? tetherPos, Vector3? labelPos, AttachMode attachMode, bool showOffscreen, float scale)
	{
		if (!this.AreLabelsEnabled)
		{
			return false;
		}
		if (this.TutorialLabelPrefab == null)
		{
			return false;
		}
		bool flag = targetObj == null || type == TutorialLabelType.Popup || type == TutorialLabelType.PopupAttached;
		if (targetObj == null)
		{
			targetObj = this.FloatingLabelTransform;
		}
		TutorialManager.ActiveLabel activeLabel = this.FindActiveLabel(targetObj.GetInstanceID());
		if (activeLabel == null)
		{
			if (flag)
			{
				Vector3 value = TutorialLabel.CalculateFloatingLabelPosition();
				tetherPos = new Vector3?(value);
				labelPos = new Vector3?(value);
			}
			else if (tetherPos == null || labelPos == null)
			{
				Vector3 value2;
				Vector3 value3;
				TutorialLabel.FindLabelAttachPoints(targetObj, out value2, out value3);
				if (tetherPos == null)
				{
					tetherPos = new Vector3?(value2);
				}
				if (labelPos == null)
				{
					labelPos = new Vector3?(value3);
				}
			}
			if (!showOffscreen && !VRCTrackingManager.IsPointWithinHMDView(labelPos.Value))
			{
				return false;
			}
			activeLabel = new TutorialManager.ActiveLabel();
			activeLabel.TargetObject = targetObj;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TutorialLabelPrefab);
			activeLabel.Label = gameObject.GetComponent<TutorialLabel>();
			activeLabel.Label.Init();
			this._activeLabels.Add(activeLabel.TargetObject.GetInstanceID(), activeLabel);
			activeLabel.Label.SetTargetObject(targetObj, attachMode, tetherPos.Value, labelPos.Value, flag);
		}
		else if (activeLabel.Label.IsActive && activeLabel.Priority > priority)
		{
			VRC.Core.Logger.LogOnceEvery(10f, this, string.Concat(new object[]
			{
				"TutorialManager: Existing label (",
				activeLabel.ID,
				", ",
				activeLabel.TargetObject.name,
				") \"",
				activeLabel.Label.TextString,
				"\" has higher priority ",
				activeLabel.Priority,
				" > ",
				priority,
				", discarding new one"
			}));
			return false;
		}
		activeLabel.ID = id;
		activeLabel.Type = type;
		activeLabel.SetController(hand, controllerPart);
		activeLabel.ShowOffscreen = showOffscreen;
		activeLabel.Priority = priority;
		activeLabel.Label.SetText(text, textSecondary);
		activeLabel.Label.SetIcon(action, actionSecondary);
		activeLabel.Label.SetTextScaleOverride(scale);
		bool invertLabelAlignment = type == TutorialLabelType.Controller && hand == ControllerHand.Right && VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Oculus;
		activeLabel.Label.IsFloatingLabel = flag;
		activeLabel.Label.InvertLabelAlignment = invertLabelAlignment;
		activeLabel.Label.IsAttachedToView = (type == TutorialLabelType.PopupAttached);
		activeLabel.Label.RefreshComponentPositions();
		activeLabel.TimeToLive = duration;
		activeLabel.Label.Activate();
		return true;
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x001D7EB4 File Offset: 0x001D62B4
	public void DeactivateControllerLabels(ControllerHand hand)
	{
		foreach (TutorialManager.ActiveLabel activeLabel in this._activeLabels.Values)
		{
			if (activeLabel.Type == TutorialLabelType.Controller && activeLabel.Hand == hand)
			{
				this.DeactivateLabel(activeLabel);
			}
		}
	}

	// Token: 0x06005579 RID: 21881 RVA: 0x001D7F30 File Offset: 0x001D6330
	private void DeactivateLabel(TutorialManager.ActiveLabel label)
	{
		label.TimeToLive = -1f;
		label.Label.Deactivate();
		ControllerUI controllerUI = VRCTrackingManager.GetControllerUI(label.Hand);
		if (controllerUI != null)
		{
			controllerUI.EnableHighlight(label.ControllerPart, false);
		}
		this.RefreshControllerUiVisibility(label.Hand);
	}

	// Token: 0x0600557A RID: 21882 RVA: 0x001D7F84 File Offset: 0x001D6384
	private TutorialManager.ActiveLabel FindActiveLabel(int instanceid)
	{
		if (this._activeLabels.ContainsKey(instanceid))
		{
			return this._activeLabels[instanceid];
		}
		return null;
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x001D7FA8 File Offset: 0x001D63A8
	private TutorialManager.ActiveLabel FindActiveLabel(string id)
	{
		foreach (TutorialManager.ActiveLabel activeLabel in this._activeLabels.Values)
		{
			if (activeLabel.ID == id)
			{
				return activeLabel;
			}
		}
		return null;
	}

	// Token: 0x0600557C RID: 21884 RVA: 0x001D8020 File Offset: 0x001D6420
	private bool IsShowingOtherLabelOfType(TutorialLabelType type, Transform exclude)
	{
		foreach (TutorialManager.ActiveLabel activeLabel in this._activeLabels.Values)
		{
			if (activeLabel.Type == type && activeLabel.TargetObject != exclude && activeLabel.Label.IsActive)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x001D80B4 File Offset: 0x001D64B4
	private bool ShouldShowControllerUI(ControllerHand hand)
	{
		foreach (TutorialManager.ActiveLabel activeLabel in this._activeLabels.Values)
		{
			if (activeLabel.Type == TutorialLabelType.Controller && activeLabel.Hand == hand && activeLabel.Label.IsActive)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04003C3B RID: 15419
	private static TutorialManager _instance;

	// Token: 0x04003C3C RID: 15420
	public GameObject TutorialLabelPrefab;

	// Token: 0x04003C3D RID: 15421
	public GameObject PickupTetherPrefab;

	// Token: 0x04003C3E RID: 15422
	public ControllerIconSet XboxControllerIcons;

	// Token: 0x04003C3F RID: 15423
	private ControllerIconSet _xboxControllerIcons;

	// Token: 0x04003C40 RID: 15424
	public ControllerIconSet MouseKeyboardIcons;

	// Token: 0x04003C41 RID: 15425
	private ControllerIconSet _mouseKeyboardIcons;

	// Token: 0x04003C42 RID: 15426
	public Material ErrorIconMaterial;

	// Token: 0x04003C43 RID: 15427
	private Transform FloatingLabelTransform;

	// Token: 0x04003C44 RID: 15428
	private const float CONTROLLER_LABEL_SCALE = 0.8f;

	// Token: 0x04003C45 RID: 15429
	private const float AREAMARKER_LABEL_SCALE = 2f;

	// Token: 0x04003C46 RID: 15430
	private Dictionary<int, TutorialManager.ActiveLabel> _activeLabels = new Dictionary<int, TutorialManager.ActiveLabel>();

	// Token: 0x04003C47 RID: 15431
	private List<int> _keysToRemove = new List<int>();

	// Token: 0x04003C48 RID: 15432
	public float RefreshHelpStateInterval = 5f;

	// Token: 0x04003C49 RID: 15433
	private TutorialManager.ContextualHelpState _helpState;

	// Token: 0x04003C4A RID: 15434
	private float _timeCurrentHelpStateStarted;

	// Token: 0x04003C4B RID: 15435
	private bool _initializedHelp;

	// Token: 0x04003C4C RID: 15436
	private float _timeEnteredWorld = -1f;

	// Token: 0x04003C4D RID: 15437
	private bool _isInTutorialRoom;

	// Token: 0x04003C4E RID: 15438
	private float _lastTimeHelpStateChecked = -100f;

	// Token: 0x04003C4F RID: 15439
	private static bool HasSeenOpenMenuHelpThisSession;

	// Token: 0x04003C50 RID: 15440
	public float FirstTimeHelpDuration = 60f;

	// Token: 0x04003C51 RID: 15441
	public int FirstTimeHelpShowCount = 3;

	// Token: 0x04003C52 RID: 15442
	private float _openMenuHelpStartTime = -1f;

	// Token: 0x04003C53 RID: 15443
	private bool _openMenuHelpComplete;

	// Token: 0x04003C54 RID: 15444
	private static bool HasSeenUnmuteHelpThisSession;

	// Token: 0x04003C55 RID: 15445
	private float _unmuteHelpStartTime = -1f;

	// Token: 0x04003C56 RID: 15446
	private bool _unmuteHelpComplete;

	// Token: 0x04003C57 RID: 15447
	private static bool HasSeenMuteHelpThisSession;

	// Token: 0x04003C58 RID: 15448
	private float _muteHelpStartTime = -1f;

	// Token: 0x04003C59 RID: 15449
	private bool _muteHelpComplete;

	// Token: 0x02000B04 RID: 2820
	private class ActiveLabel
	{
		// Token: 0x06005580 RID: 21888 RVA: 0x001D8160 File Offset: 0x001D6560
		public void SetController(ControllerHand hand, ControllerInputUI part)
		{
			ControllerUI controllerUI = VRCTrackingManager.GetControllerUI(this.Hand);
			if (controllerUI != null)
			{
				controllerUI.EnableHighlight(this.ControllerPart, false);
			}
			ControllerUI controllerUI2 = VRCTrackingManager.GetControllerUI(hand);
			if (controllerUI2 != null)
			{
				controllerUI2.EnableHighlight(part, true);
			}
			this.Hand = hand;
			this.ControllerPart = part;
		}

		// Token: 0x04003C5A RID: 15450
		public Component TargetObject;

		// Token: 0x04003C5B RID: 15451
		public TutorialLabelType Type;

		// Token: 0x04003C5C RID: 15452
		public ControllerHand Hand;

		// Token: 0x04003C5D RID: 15453
		public ControllerInputUI ControllerPart;

		// Token: 0x04003C5E RID: 15454
		public TutorialLabel Label;

		// Token: 0x04003C5F RID: 15455
		public bool ShowOffscreen;

		// Token: 0x04003C60 RID: 15456
		public float TimeToLive = 0.1f;

		// Token: 0x04003C61 RID: 15457
		public int Priority;

		// Token: 0x04003C62 RID: 15458
		public string ID = "<no id>";
	}

	// Token: 0x02000B05 RID: 2821
	public enum ContextualHelpState
	{
		// Token: 0x04003C64 RID: 15460
		None,
		// Token: 0x04003C65 RID: 15461
		ShowOpenMenuHelp,
		// Token: 0x04003C66 RID: 15462
		ShowUnmuteHelp,
		// Token: 0x04003C67 RID: 15463
		ShowMuteHelp
	}
}
