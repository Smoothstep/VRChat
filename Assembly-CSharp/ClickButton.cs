using System;
using UnityEngine;

// Token: 0x02000A01 RID: 2561
internal class ClickButton : AbstractButton
{
	// Token: 0x06004DD8 RID: 19928 RVA: 0x001A1784 File Offset: 0x0019FB84
	public override void Update()
	{
		if (this.m_Rect.Contains(Input.mousePosition) && Input.GetMouseButtonDown(0) && !this.pressed)
		{
			this.pressed = true;
			this.m_Button.Pressed();
			return;
		}
		if (Input.GetMouseButtonUp(0) && this.pressed)
		{
			this.pressed = false;
			this.m_Button.Released();
		}
	}

	// Token: 0x040035CE RID: 13774
	private bool pressed;
}
