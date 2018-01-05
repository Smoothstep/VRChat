using System;

// Token: 0x0200045A RID: 1114
public class Console_old
{
	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x060026FE RID: 9982 RVA: 0x000BFA11 File Offset: 0x000BDE11
	public static Console_old Instance
	{
		get
		{
			if (Console_old.instance == null)
			{
				Console_old.instance = new Console_old();
			}
			return Console_old.instance;
		}
	}

	// Token: 0x060026FF RID: 9983 RVA: 0x000BFA2C File Offset: 0x000BDE2C
	public void Prompt(string prompt, Console_old.OnConsoleSubmit callback)
	{
		this.onConsoleSubmit = callback;
		ConsoleLog.Instance.Log("> " + prompt);
		this.inputMode = InputMode.Prompt;
	}

	// Token: 0x0400143E RID: 5182
	private static Console_old instance;

	// Token: 0x0400143F RID: 5183
	private ConsoleLog consoleLog;

	// Token: 0x04001440 RID: 5184
	public InputMode inputMode;

	// Token: 0x04001441 RID: 5185
	public string promptResponse = string.Empty;

	// Token: 0x04001442 RID: 5186
	public Console_old.OnConsoleSubmit onConsoleSubmit;

	// Token: 0x0200045B RID: 1115
	// (Invoke) Token: 0x06002701 RID: 9985
	public delegate void OnConsoleSubmit(string response);
}
