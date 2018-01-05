using System;
using UnityEngine;

// Token: 0x02000618 RID: 1560
[ExecuteInEditMode]
public class AnimatedWidget : MonoBehaviour
{
	// Token: 0x06003436 RID: 13366 RVA: 0x0010846D File Offset: 0x0010686D
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.LateUpdate();
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x00108481 File Offset: 0x00106881
	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.width = Mathf.RoundToInt(this.width);
			this.mWidget.height = Mathf.RoundToInt(this.height);
		}
	}

	// Token: 0x04001DBD RID: 7613
	public float width = 1f;

	// Token: 0x04001DBE RID: 7614
	public float height = 1f;

	// Token: 0x04001DBF RID: 7615
	private UIWidget mWidget;
}
