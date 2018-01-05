using System;
using UnityEngine;

// Token: 0x020009FC RID: 2556
public abstract class AbstractButton
{
	// Token: 0x06004DC7 RID: 19911 RVA: 0x001A140E File Offset: 0x0019F80E
	public void Enable(string name, bool pairwithinputmanager, Rect rect)
	{
		this.m_Button = new CrossPlatformInput.VirtualButton(name, pairwithinputmanager);
		this.m_Rect = rect;
	}

	// Token: 0x06004DC8 RID: 19912 RVA: 0x001A1424 File Offset: 0x0019F824
	public void Disable()
	{
		this.m_Button.Remove();
	}

	// Token: 0x06004DC9 RID: 19913
	public abstract void Update();

	// Token: 0x040035BD RID: 13757
	protected CrossPlatformInput.VirtualButton m_Button;

	// Token: 0x040035BE RID: 13758
	protected Rect m_Rect;
}
