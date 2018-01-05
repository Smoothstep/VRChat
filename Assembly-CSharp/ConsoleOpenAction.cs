using System;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class ConsoleOpenAction : ConsoleAction
{
	// Token: 0x060026F1 RID: 9969 RVA: 0x000BF8BD File Offset: 0x000BDCBD
	public override void Activate()
	{
		this.ConsoleGui.SetActive(true);
		Input.ResetInputAxes();
	}

	// Token: 0x04001431 RID: 5169
	public GameObject ConsoleGui;
}
