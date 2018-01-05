using System;
using UnityEngine;

// Token: 0x02000A00 RID: 2560
internal class TouchButton : AbstractButton
{
	// Token: 0x06004DD6 RID: 19926 RVA: 0x001A16EC File Offset: 0x0019FAEC
	public override void Update()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (this.m_Rect.Contains(touch.position))
			{
				if (this.m_Pressed)
				{
					return;
				}
				if (touch.phase == TouchPhase.Began)
				{
					this.m_Button.Pressed();
					this.m_Pressed = true;
					return;
				}
			}
		}
		if (this.m_Pressed)
		{
			this.m_Button.Released();
			this.m_Pressed = false;
		}
	}

	// Token: 0x040035CD RID: 13773
	private bool m_Pressed;
}
