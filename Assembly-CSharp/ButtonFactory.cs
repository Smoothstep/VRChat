using System;

// Token: 0x020009FF RID: 2559
public class ButtonFactory
{
	// Token: 0x06004DD4 RID: 19924 RVA: 0x001A16DB File Offset: 0x0019FADB
	public static AbstractButton GetPlatformSpecificButtonImplementation()
	{
		return new ClickButton();
	}
}
