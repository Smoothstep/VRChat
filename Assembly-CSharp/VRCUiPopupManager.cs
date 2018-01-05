using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.UI;

// Token: 0x02000C87 RID: 3207
public class VRCUiPopupManager : MonoBehaviour
{
	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x06006396 RID: 25494 RVA: 0x002361C0 File Offset: 0x002345C0
	public static VRCUiPopupManager Instance
	{
		get
		{
			return VRCUiPopupManager.mInstance;
		}
	}

	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x06006397 RID: 25495 RVA: 0x002361C8 File Offset: 0x002345C8
	public bool isPopupActive
	{
		get
		{
			return this.standardPopup.gameObject.activeInHierarchy || this.alertPopup.gameObject.activeInHierarchy || this.inputPopup.gameObject.activeInHierarchy || this.datePopup.gameObject.activeInHierarchy || this.tableViewPopup.gameObject.activeInHierarchy || this.roomInfoPopup.gameObject.activeInHierarchy || this.roomInstancePopup.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06006398 RID: 25496 RVA: 0x00236266 File Offset: 0x00234666
	private void Awake()
	{
		if (VRCUiPopupManager.mInstance == null)
		{
			VRCUiPopupManager.mInstance = this;
		}
		else
		{
			Debug.LogError("More than one VRCUiPopupManager detected!!!");
		}
	}

	// Token: 0x06006399 RID: 25497 RVA: 0x00236290 File Offset: 0x00234690
	public void ShowStandardPopup(string title, string body, Action<VRCUiPopup> additionalSetup = null)
	{
		this.DisableAllPopups();
		this.standardPopup.gameObject.SetActive(true);
		this.standardPopup.Initialize(title, body);
		if (additionalSetup != null)
		{
			additionalSetup(this.standardPopup);
		}
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/StandardPopup");
	}

	// Token: 0x0600639A RID: 25498 RVA: 0x002362E4 File Offset: 0x002346E4
	public void ShowStandardPopup(string title, string body, string leftButtonText, Action leftButtonAction, string rightButtonText, Action rightButtonAction, Action<VRCUiPopup> additionalSetup = null)
	{
		this.DisableAllPopups();
		this.standardPopup.gameObject.SetActive(true);
		this.standardPopup.Initialize(title, body);
		this.standardPopup.SetupLeftButton(leftButtonText, leftButtonAction);
		this.standardPopup.SetupRightButton(rightButtonText, rightButtonAction);
		if (additionalSetup != null)
		{
			additionalSetup(this.standardPopup);
		}
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/StandardPopup");
	}

	// Token: 0x0600639B RID: 25499 RVA: 0x00236358 File Offset: 0x00234758
	public void ShowStandardPopup(string title, string body, string middleButtonText, Action middleButtonAction, Action<VRCUiPopup> additionalSetup = null)
	{
		this.DisableAllPopups();
		this.standardPopup.gameObject.SetActive(true);
		this.standardPopup.Initialize(title, body);
		this.standardPopup.SetupMiddleButton(middleButtonText, middleButtonAction);
		if (additionalSetup != null)
		{
			additionalSetup(this.standardPopup);
		}
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/StandardPopup");
	}

	// Token: 0x0600639C RID: 25500 RVA: 0x002363BA File Offset: 0x002347BA
	public void ShowNeedAccountPopup()
	{
		this.ShowStandardPopup("Account Required.", "You must be logged in with an account (not anonymous login) to access this feature.", "OK", delegate
		{
			VRCUiManager.Instance.popups.HideCurrentPopup();
		}, null);
	}

	// Token: 0x0600639D RID: 25501 RVA: 0x002363F0 File Offset: 0x002347F0
	public void ShowOldInputPopup(string title, string body, InputField.InputType inputType, string submitButtonText, Action<string, List<KeyCode>, Text> submitButtonAction, string placeholderText = "Enter text....", bool hidePopupOnSubmit = true, Action<VRCUiPopup> additionalSetup = null)
	{
		this.DisableAllPopups();
		this.inputPopup.gameObject.SetActive(true);
		this.inputPopup.Initialize(title, body);
		this.inputPopup.SetupInputAndSubmitButton(placeholderText, inputType, submitButtonText, submitButtonAction, hidePopupOnSubmit);
		if (additionalSetup != null)
		{
			additionalSetup(this.inputPopup);
		}
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/InputPopup");
	}

	// Token: 0x0600639E RID: 25502 RVA: 0x00236458 File Offset: 0x00234858
	public void ShowOldInputPopupWithCancel(string title, string body, InputField.InputType inputType, string submitButtonText, Action<string, List<KeyCode>, Text> submitButtonAction, Action cancelButtonAction, string placeholderText = "Enter text....", bool hidePopupOnSubmit = true, Action<VRCUiPopup> additionalSetup = null)
	{
		if (cancelButtonAction == null)
		{
			cancelButtonAction = delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			};
		}
		this.DisableAllPopups();
		this.inputPopup.gameObject.SetActive(true);
		this.inputPopup.Initialize(title, body);
		this.inputPopup.SetupInputAndSubmitAndCancelButton(placeholderText, inputType, submitButtonText, submitButtonAction, "Cancel", cancelButtonAction, hidePopupOnSubmit);
		if (additionalSetup != null)
		{
			additionalSetup(this.inputPopup);
		}
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/InputPopup");
	}

	// Token: 0x0600639F RID: 25503 RVA: 0x002364F0 File Offset: 0x002348F0
	public void ShowInputPopup(string title, string body, InputField.InputType inputType, string submitButtonText, Action<string, List<KeyCode>, Text> submitButtonAction, string placeholderText = "Enter text....", bool hidePopupOnSubmit = true, Action<VRCUiPopup> additionalSetup = null)
	{
		this.DisableAllPopups();
		this.keyboardPopup.gameObject.SetActive(true);
		this.keyboardPopup.Initialize(title, body);
		this.keyboardPopup.SetupInputAndSubmitButton(placeholderText, inputType, submitButtonText, submitButtonAction, hidePopupOnSubmit);
		if (additionalSetup != null)
		{
			additionalSetup(this.keyboardPopup);
		}
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/JSKeysPopup");
	}

	// Token: 0x060063A0 RID: 25504 RVA: 0x00236558 File Offset: 0x00234958
	public void ShowInputPopupWithCancel(string title, string body, InputField.InputType inputType, string submitButtonText, Action<string, List<KeyCode>, Text> submitButtonAction, Action cancelButtonAction, string placeholderText = "Enter text....", bool hidePopupOnSubmit = true, Action<VRCUiPopup> additionalSetup = null)
	{
		this.DisableAllPopups();
		this.keyboardPopup.gameObject.SetActive(true);
		this.keyboardPopup.Initialize(title, body);
		this.keyboardPopup.SetupInputAndSubmitAndCancelButton(placeholderText, inputType, submitButtonText, submitButtonAction, "Cancel", cancelButtonAction, hidePopupOnSubmit);
		if (additionalSetup != null)
		{
			additionalSetup(this.keyboardPopup);
		}
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/JSKeysPopup");
	}

	// Token: 0x060063A1 RID: 25505 RVA: 0x002365C8 File Offset: 0x002349C8
	public void ShowDatePopupWithCancel(string title, string body, InputField.InputType inputType, string submitButtonText, Action<string, List<KeyCode>, Text> submitButtonAction, Action cancelButtonAction, string placeholderText = "Enter text....", bool hidePopupOnSubmit = true, Action<VRCUiPopup> additionalSetup = null)
	{
		if (cancelButtonAction == null)
		{
			cancelButtonAction = delegate
			{
				VRCUiManager.Instance.popups.HideCurrentPopup();
			};
		}
		this.DisableAllPopups();
		this.datePopup.gameObject.SetActive(true);
		this.datePopup.Initialize(title, body);
		this.datePopup.SetupInputAndSubmitAndCancelButton(placeholderText, inputType, submitButtonText, submitButtonAction, "Cancel", cancelButtonAction, hidePopupOnSubmit);
		if (additionalSetup != null)
		{
			additionalSetup(this.datePopup);
		}
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/DatePopup");
	}

	// Token: 0x060063A2 RID: 25506 RVA: 0x0023665D File Offset: 0x00234A5D
	public void ShowTableViewPopup(string title, IEnumerable<IUIGroupItemDatasource> items, int itemsPerRow, Action<IUIGroupItemDatasource> onItemSelected)
	{
		this.DisableAllPopups();
		this.tableViewPopup.Initialize(title, string.Empty);
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/TableViewPopup");
		this.tableViewPopup.SetupTableView(items, itemsPerRow, onItemSelected);
	}

	// Token: 0x060063A3 RID: 25507 RVA: 0x00236694 File Offset: 0x00234A94
	public void ShowRoomInfoPopup(ApiWorld world)
	{
		this.DisableAllPopups();
		this.roomInfoPopup.gameObject.SetActive(true);
		this.roomInfoPopup.Initialize(string.Empty, string.Empty);
		this.roomInfoPopup.SetupRoomInfo(world);
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/RoomInfoPopup");
	}

	// Token: 0x060063A4 RID: 25508 RVA: 0x002366E8 File Offset: 0x00234AE8
	public void ShowRoomInstancePopup(ApiWorld world, PageWorldInfo pwi)
	{
		this.DisableAllPopups();
		this.roomInstancePopup.gameObject.SetActive(true);
		this.roomInstancePopup.Initialize(string.Empty, string.Empty);
		this.roomInstancePopup.SetupRoomInfo(world, pwi);
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/RoomInstancePopup");
	}

	// Token: 0x060063A5 RID: 25509 RVA: 0x0023673D File Offset: 0x00234B3D
	public void HideCurrentPopup()
	{
		VRCUiManager.Instance.HideScreen("POPUP");
	}

	// Token: 0x060063A6 RID: 25510 RVA: 0x0023674E File Offset: 0x00234B4E
	private void DisableAllPopups()
	{
		VRCUiManager.Instance.HideScreen("POPUP");
	}

	// Token: 0x060063A7 RID: 25511 RVA: 0x00236760 File Offset: 0x00234B60
	public void ShowAlert(string title, string body, float timeout = 10f)
	{
		this.DisableAllPopups();
		this.alertPopup.gameObject.SetActive(true);
		this.alertPopup.SetTimeout(timeout);
		this.alertPopup.Initialize(title, body);
		VRCUiManager.Instance.ShowScreen("UserInterface/MenuContent/Popups/AlertPopup");
	}

	// Token: 0x040048F2 RID: 18674
	private static VRCUiPopupManager mInstance;

	// Token: 0x040048F3 RID: 18675
	public VRCUiPopupStandard standardPopup;

	// Token: 0x040048F4 RID: 18676
	public VRCUiPopupAlert alertPopup;

	// Token: 0x040048F5 RID: 18677
	public VRCUiPopupInput inputPopup;

	// Token: 0x040048F6 RID: 18678
	public VRCUiPopupJsKeyboard keyboardPopup;

	// Token: 0x040048F7 RID: 18679
	public VRCUiPopupDate datePopup;

	// Token: 0x040048F8 RID: 18680
	public VRCUiPopupTableView tableViewPopup;

	// Token: 0x040048F9 RID: 18681
	public PopupRoomInfo roomInfoPopup;

	// Token: 0x040048FA RID: 18682
	public PopupRoomInstance roomInstancePopup;
}
