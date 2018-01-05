using System;
using UnityEngine;

// Token: 0x02000AE7 RID: 2791
public class RoomLevelPicker : MonoBehaviour
{
	// Token: 0x06005490 RID: 21648 RVA: 0x001D2EFD File Offset: 0x001D12FD
	private void Awake()
	{
		this._popup = base.GetComponent<UIPopupList>();
		NGUITools.SetActive(this.customLevelNameInput, false);
	}

	// Token: 0x06005491 RID: 21649 RVA: 0x001D2F17 File Offset: 0x001D1317
	public void OnLevelChanged()
	{
		if (this._popup.value == "Custom")
		{
			NGUITools.SetActive(this.customLevelNameInput, true);
		}
		else
		{
			NGUITools.SetActive(this.customLevelNameInput, false);
		}
	}

	// Token: 0x04003BAA RID: 15274
	private UIPopupList _popup;

	// Token: 0x04003BAB RID: 15275
	public GameObject customLevelNameInput;
}
