using System;
using UnityEngine;

// Token: 0x02000458 RID: 1112
public class ConsoleToggler : MonoBehaviour
{
	// Token: 0x060026F8 RID: 9976 RVA: 0x000BF964 File Offset: 0x000BDD64
	private void Start()
	{
		ConsoleToggler.Instance = this;
		EventManager.OnPrivatePortalEntered += this.ActivateConsole;
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x000BF97D File Offset: 0x000BDD7D
	private void Update()
	{
		if (Input.GetButtonDown("Console"))
		{
			this.ToggleConsole();
		}
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x000BF994 File Offset: 0x000BDD94
	public void ToggleConsole()
	{
		this.consoleEnabled = !this.consoleEnabled;
		if (this.consoleEnabled)
		{
			this.ConsoleOpenAction.Activate();
		}
		else
		{
			this.ConsoleCloseAction.Activate();
		}
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x000BF9CB File Offset: 0x000BDDCB
	public void ActivateConsole()
	{
		this.consoleEnabled = true;
		this.ConsoleOpenAction.Activate();
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x000BF9DF File Offset: 0x000BDDDF
	public void DisableConsole()
	{
		this.consoleEnabled = false;
		this.ConsoleCloseAction.Activate();
		Console_old.Instance.inputMode = InputMode.Command;
	}

	// Token: 0x04001437 RID: 5175
	public static ConsoleToggler Instance;

	// Token: 0x04001438 RID: 5176
	private bool consoleEnabled;

	// Token: 0x04001439 RID: 5177
	public ConsoleAction ConsoleOpenAction;

	// Token: 0x0400143A RID: 5178
	public ConsoleAction ConsoleCloseAction;
}
