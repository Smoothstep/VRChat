using System;
using UnityEngine;

// Token: 0x02000AD8 RID: 2776
public class ProgressBar : MonoBehaviour
{
	// Token: 0x17000C3C RID: 3132
	// (get) Token: 0x06005450 RID: 21584 RVA: 0x001D1F88 File Offset: 0x001D0388
	public UIProgressBar progressBarUI
	{
		get
		{
			return this._uiProgressBar;
		}
	}

	// Token: 0x17000C3D RID: 3133
	// (get) Token: 0x06005451 RID: 21585 RVA: 0x001D1F90 File Offset: 0x001D0390
	public UILabel percentLabel
	{
		get
		{
			return this._percentLabel;
		}
	}

	// Token: 0x06005452 RID: 21586 RVA: 0x001D1F98 File Offset: 0x001D0398
	private void Awake()
	{
		this._uiProgressBar = base.GetComponent<UIProgressBar>();
		this._percentLabel = base.GetComponentInChildren<UILabel>();
	}

	// Token: 0x06005453 RID: 21587 RVA: 0x001D1FB2 File Offset: 0x001D03B2
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005454 RID: 21588 RVA: 0x001D1FC0 File Offset: 0x001D03C0
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x04003B77 RID: 15223
	private UIProgressBar _uiProgressBar;

	// Token: 0x04003B78 RID: 15224
	private UILabel _percentLabel;
}
