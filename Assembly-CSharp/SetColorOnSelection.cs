using System;
using UnityEngine;

// Token: 0x02000591 RID: 1425
[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Examples/Set Color on Selection")]
public class SetColorOnSelection : MonoBehaviour
{
	// Token: 0x06002FE0 RID: 12256 RVA: 0x000EA4D8 File Offset: 0x000E88D8
	public void SetSpriteBySelection()
	{
		if (UIPopupList.current == null)
		{
			return;
		}
		if (this.mWidget == null)
		{
			this.mWidget = base.GetComponent<UIWidget>();
		}
		string value = UIPopupList.current.value;
		switch (value)
		{
		case "White":
			this.mWidget.color = Color.white;
			break;
		case "Red":
			this.mWidget.color = Color.red;
			break;
		case "Green":
			this.mWidget.color = Color.green;
			break;
		case "Blue":
			this.mWidget.color = Color.blue;
			break;
		case "Yellow":
			this.mWidget.color = Color.yellow;
			break;
		case "Cyan":
			this.mWidget.color = Color.cyan;
			break;
		case "Magenta":
			this.mWidget.color = Color.magenta;
			break;
		}
	}

	// Token: 0x04001A4B RID: 6731
	private UIWidget mWidget;
}
