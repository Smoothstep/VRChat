using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C83 RID: 3203
public class VRCUiPopupAlert : VRCUiPopup
{
	// Token: 0x0600635A RID: 25434 RVA: 0x00234B30 File Offset: 0x00232F30
	public override void Initialize(string title, string body)
	{
		base.Initialize(title, body);
		if (this.timeout <= 0f)
		{
			this.timerText.gameObject.SetActive(false);
		}
		else
		{
			this.timerText.gameObject.SetActive(true);
		}
		this.timer = 0f;
	}

	// Token: 0x0600635B RID: 25435 RVA: 0x00234B87 File Offset: 0x00232F87
	public void SetTimeout(float t)
	{
		this.timeout = t;
	}

	// Token: 0x0600635C RID: 25436 RVA: 0x00234B90 File Offset: 0x00232F90
	public override void Update()
	{
		base.Update();
		if (this.timeout <= 0f)
		{
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timerText.gameObject.activeInHierarchy)
		{
			this.timerText.text = Mathf.Max(0f, this.timeout - this.timer).ToString("0");
		}
		if (this.timer > this.timeout)
		{
			this.Close();
		}
	}

	// Token: 0x040048C2 RID: 18626
	public Text timerText;

	// Token: 0x040048C3 RID: 18627
	public float timeout;

	// Token: 0x040048C4 RID: 18628
	private Text popupButtonText;

	// Token: 0x040048C5 RID: 18629
	private float timer;

	// Token: 0x040048C6 RID: 18630
	private Action buttonPressed;
}
