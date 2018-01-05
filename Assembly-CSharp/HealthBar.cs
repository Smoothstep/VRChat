using System;
using UnityEngine;

// Token: 0x02000A96 RID: 2710
public class HealthBar : MonoBehaviour
{
	// Token: 0x17000C00 RID: 3072
	// (get) Token: 0x06005187 RID: 20871 RVA: 0x001BED8A File Offset: 0x001BD18A
	public UIProgressBar healthBarUI
	{
		get
		{
			return this.mHealthBarUI;
		}
	}

	// Token: 0x06005188 RID: 20872 RVA: 0x001BED92 File Offset: 0x001BD192
	private void Awake()
	{
		this.mHealthBarUI = base.GetComponent<UIProgressBar>();
	}

	// Token: 0x06005189 RID: 20873 RVA: 0x001BEDA0 File Offset: 0x001BD1A0
	public void SetValue(float value)
	{
		this.mHealthBarUI.value = value;
	}

	// Token: 0x0600518A RID: 20874 RVA: 0x001BEDAE File Offset: 0x001BD1AE
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600518B RID: 20875 RVA: 0x001BEDBC File Offset: 0x001BD1BC
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x040039E2 RID: 14818
	private UIProgressBar mHealthBarUI;
}
