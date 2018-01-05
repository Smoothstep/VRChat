using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C45 RID: 3141
public abstract class UiGroupItem : MonoBehaviour
{
	// Token: 0x06006174 RID: 24948 RVA: 0x00226514 File Offset: 0x00224914
	public virtual void Setup(IUIGroupItemDatasource item, Action<IUIGroupItemDatasource> onItemSelected)
	{
		this.dataSource = item;
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(delegate
		{
			if (onItemSelected != null)
			{
				onItemSelected(this.dataSource);
			}
		});
	}

	// Token: 0x0400470B RID: 18187
	public Button button;

	// Token: 0x0400470C RID: 18188
	public IUIGroupItemDatasource dataSource;
}
