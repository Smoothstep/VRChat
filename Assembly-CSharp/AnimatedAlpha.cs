using System;
using UnityEngine;

// Token: 0x02000616 RID: 1558
[ExecuteInEditMode]
public class AnimatedAlpha : MonoBehaviour
{
	// Token: 0x06003430 RID: 13360 RVA: 0x001083A4 File Offset: 0x001067A4
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.mPanel = base.GetComponent<UIPanel>();
		this.LateUpdate();
	}

	// Token: 0x06003431 RID: 13361 RVA: 0x001083C4 File Offset: 0x001067C4
	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.alpha = this.alpha;
		}
		if (this.mPanel != null)
		{
			this.mPanel.alpha = this.alpha;
		}
	}

	// Token: 0x04001DB8 RID: 7608
	[Range(0f, 1f)]
	public float alpha = 1f;

	// Token: 0x04001DB9 RID: 7609
	private UIWidget mWidget;

	// Token: 0x04001DBA RID: 7610
	private UIPanel mPanel;
}
