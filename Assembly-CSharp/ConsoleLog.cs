using System;

// Token: 0x02000452 RID: 1106
public class ConsoleLog
{
	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x060026EB RID: 9963 RVA: 0x000BF81A File Offset: 0x000BDC1A
	public static ConsoleLog Instance
	{
		get
		{
			if (ConsoleLog.instance == null)
			{
				ConsoleLog.instance = new ConsoleLog();
			}
			return ConsoleLog.instance;
		}
	}

	// Token: 0x060026EC RID: 9964 RVA: 0x000BF835 File Offset: 0x000BDC35
	public void Log(string message)
	{
		this.log = this.log + message + "\n";
	}

	// Token: 0x0400142E RID: 5166
	private static ConsoleLog instance;

	// Token: 0x0400142F RID: 5167
	public string lastInput;

	// Token: 0x04001430 RID: 5168
	public string log = string.Empty;
}
