using System;
using UnityEngine;

// Token: 0x02000617 RID: 1559
[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
public class AnimatedColor : MonoBehaviour
{
	// Token: 0x06003433 RID: 13363 RVA: 0x00108428 File Offset: 0x00106828
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.LateUpdate();
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x0010843C File Offset: 0x0010683C
	private void LateUpdate()
	{
		this.mWidget.color = this.color;
	}

	// Token: 0x04001DBB RID: 7611
	public Color color = Color.white;

	// Token: 0x04001DBC RID: 7612
	private UIWidget mWidget;
}
