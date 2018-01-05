using System;
using UnityEngine;

// Token: 0x02000598 RID: 1432
[RequireComponent(typeof(UIPopupList))]
[AddComponentMenu("NGUI/Interaction/Language Selection")]
public class LanguageSelection : MonoBehaviour
{
	// Token: 0x06002FF6 RID: 12278 RVA: 0x000EABC0 File Offset: 0x000E8FC0
	private void Start()
	{
		this.mList = base.GetComponent<UIPopupList>();
		if (Localization.knownLanguages != null)
		{
			this.mList.items.Clear();
			int i = 0;
			int num = Localization.knownLanguages.Length;
			while (i < num)
			{
				this.mList.items.Add(Localization.knownLanguages[i]);
				i++;
			}
			this.mList.value = Localization.language;
		}
		EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x06002FF7 RID: 12279 RVA: 0x000EAC51 File Offset: 0x000E9051
	private void OnChange()
	{
		Localization.language = UIPopupList.current.value;
	}

	// Token: 0x04001A61 RID: 6753
	private UIPopupList mList;
}
