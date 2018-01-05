using System;

// Token: 0x02000456 RID: 1110
public class ConsolePromptInput : ConsoleInput_old
{
	// Token: 0x060026F3 RID: 9971 RVA: 0x000BF8D8 File Offset: 0x000BDCD8
	public override void Interpret(ref string response)
	{
		ConsoleLog.Instance.Log(response);
	}

	// Token: 0x04001432 RID: 5170
	public ConsoleGUI consoleGUI;
}
