using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C43 RID: 3139
public class UiFeatureList : ScrollRect
{
	// Token: 0x0600616E RID: 24942 RVA: 0x002263F0 File Offset: 0x002247F0
	private void Update()
	{
		if (this.focus != null)
		{
			Vector3 localPosition = base.content.localPosition;
			float num = Mathf.Abs(this.focus.transform.localPosition.x - -localPosition.x);
			localPosition.x = Mathf.MoveTowards(localPosition.x, -this.focus.transform.localPosition.x, num * Time.deltaTime * 5f);
			base.content.localPosition = localPosition;
			if (num < 1f)
			{
				this.focus = null;
			}
		}
	}

	// Token: 0x0600616F RID: 24943 RVA: 0x00226498 File Offset: 0x00224898
	public void CenterOn(VRCUiContentButton content)
	{
		this.focus = content;
	}

	// Token: 0x06006170 RID: 24944 RVA: 0x002264A4 File Offset: 0x002248A4
	public bool isCentered(VRCUiContentButton button)
	{
		float x = button.transform.localPosition.x;
		float num = -base.content.localPosition.x;
		return Mathf.Abs(x - num) < 200f;
	}

	// Token: 0x0400470A RID: 18186
	private VRCUiContentButton focus;
}
