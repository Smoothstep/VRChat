using System;

// Token: 0x02000457 RID: 1111
public class ConsoleSubmitAction : ConsoleAction
{
	// Token: 0x060026F5 RID: 9973 RVA: 0x000BF8EE File Offset: 0x000BDCEE
	private void Start()
	{
		this.console = Console_old.Instance;
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x000BF8FC File Offset: 0x000BDCFC
	public override void Activate()
	{
		string empty = string.Empty;
		if (this.console.inputMode == InputMode.Prompt)
		{
			this.promptInput.Interpret(ref empty);
			this.console.inputMode = InputMode.Command;
			this.console.onConsoleSubmit(empty);
		}
		else
		{
			this.commandInput.Interpret(ref empty);
		}
	}

	// Token: 0x04001433 RID: 5171
	private Console_old console;

	// Token: 0x04001434 RID: 5172
	public ConsoleGUI consoleGUI;

	// Token: 0x04001435 RID: 5173
	public ConsoleInput_old commandInput;

	// Token: 0x04001436 RID: 5174
	public ConsoleInput_old promptInput;
}
