using System;
using UnityEngine;

// Token: 0x0200044C RID: 1100
public class ConsoleCloseAction : ConsoleAction
{
	// Token: 0x060026DA RID: 9946 RVA: 0x000BF73B File Offset: 0x000BDB3B
	public override void Activate()
	{
		Console_old.Instance.inputMode = InputMode.Command;
		this.ConsoleGui.SetActive(false);
	}

	// Token: 0x0400142A RID: 5162
	public GameObject ConsoleGui;
}
