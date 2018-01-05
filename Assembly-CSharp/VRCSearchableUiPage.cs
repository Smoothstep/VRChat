using System;

// Token: 0x02000C5B RID: 3163
public class VRCSearchableUiPage : VRCUiPage
{
	// Token: 0x06006242 RID: 25154 RVA: 0x0022E872 File Offset: 0x0022CC72
	public override void Awake()
	{
		base.Awake();
		this.allLists = base.GetComponentsInChildren<UiVRCList>(true);
	}

	// Token: 0x06006243 RID: 25155 RVA: 0x0022E888 File Offset: 0x0022CC88
	public void HideSearch()
	{
		foreach (UiVRCList uiVRCList in this.allLists)
		{
			if (uiVRCList == this.searchList)
			{
				uiVRCList.gameObject.SetActive(false);
			}
			else
			{
				uiVRCList.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06006244 RID: 25156 RVA: 0x0022E8E4 File Offset: 0x0022CCE4
	public void OnSearch(string searchQuery)
	{
		if (string.IsNullOrEmpty(searchQuery))
		{
			this.searchList.searchQuery = string.Empty;
			this.HideSearch();
		}
		else
		{
			this.searchList.searchQuery = searchQuery;
			foreach (UiVRCList uiVRCList in this.allLists)
			{
				if (uiVRCList == this.searchList)
				{
					if (uiVRCList.gameObject.activeSelf)
					{
						uiVRCList.Refresh();
					}
					else
					{
						uiVRCList.gameObject.SetActive(true);
					}
				}
				else
				{
					uiVRCList.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x040047AC RID: 18348
	private UiVRCList[] allLists;

	// Token: 0x040047AD RID: 18349
	public UiVRCList searchList;
}
