using System;
using System.Collections.Generic;

// Token: 0x0200044F RID: 1103
public class ConsoleCommandsRepository
{
	// Token: 0x060026E1 RID: 9953 RVA: 0x000BF75E File Offset: 0x000BDB5E
	public ConsoleCommandsRepository()
	{
		this.repository = new Dictionary<string, ConsoleCommandCallback>();
		this.commandManual = new Dictionary<string, string>();
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x060026E2 RID: 9954 RVA: 0x000BF77C File Offset: 0x000BDB7C
	public static ConsoleCommandsRepository Instance
	{
		get
		{
			if (ConsoleCommandsRepository.instance == null)
			{
				ConsoleCommandsRepository.instance = new ConsoleCommandsRepository();
			}
			return ConsoleCommandsRepository.instance;
		}
	}

	// Token: 0x060026E3 RID: 9955 RVA: 0x000BF797 File Offset: 0x000BDB97
	public void RegisterCommand(string command, ConsoleCommandCallback callback, string manual = "")
	{
		this.repository[command] = new ConsoleCommandCallback(callback.Invoke);
		this.commandManual[command] = manual;
	}

	// Token: 0x060026E4 RID: 9956 RVA: 0x000BF7BE File Offset: 0x000BDBBE
	public bool HasCommand(string command)
	{
		return this.repository.ContainsKey(command);
	}

	// Token: 0x060026E5 RID: 9957 RVA: 0x000BF7CC File Offset: 0x000BDBCC
	public string ExecuteCommand(string command, string[] args)
	{
		if (this.HasCommand(command))
		{
			return this.repository[command](args);
		}
		return "Command not found";
	}

	// Token: 0x060026E6 RID: 9958 RVA: 0x000BF7F2 File Offset: 0x000BDBF2
	public Dictionary<string, ConsoleCommandCallback>.KeyCollection CommandList()
	{
		return this.repository.Keys;
	}

	// Token: 0x0400142B RID: 5163
	private static ConsoleCommandsRepository instance;

	// Token: 0x0400142C RID: 5164
	private Dictionary<string, ConsoleCommandCallback> repository;

	// Token: 0x0400142D RID: 5165
	public Dictionary<string, string> commandManual;
}
