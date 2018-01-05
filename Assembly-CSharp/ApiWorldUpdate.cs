using System;
using System.Collections.Generic;
using VRC.Core;

// Token: 0x02000AAF RID: 2735
internal class ApiWorldUpdate : ApiModel
{
	// Token: 0x17000C0C RID: 3084
	// (get) Token: 0x060052DB RID: 21211 RVA: 0x001C6A25 File Offset: 0x001C4E25
	public bool inWorld
	{
		get
		{
			return this.parameters.ContainsKey("worldId");
		}
	}

	// Token: 0x060052DC RID: 21212 RVA: 0x001C6A37 File Offset: 0x001C4E37
	public void Join(string user, string world)
	{
		this.parameters.Add("userId", user);
		this.parameters.Add("worldId", world);
		ApiModel.SendPutRequest("joins", this.parameters, null, null);
	}

	// Token: 0x060052DD RID: 21213 RVA: 0x001C6A6D File Offset: 0x001C4E6D
	public void Leave()
	{
		ApiModel.SendPutRequest("leaves", this.parameters, null, null);
		this.parameters.Clear();
	}

	// Token: 0x060052DE RID: 21214 RVA: 0x001C6A8C File Offset: 0x001C4E8C
	public void Update()
	{
		ApiModel.SendPutRequest("visits", this.parameters, null, null);
	}

	// Token: 0x04003A7D RID: 14973
	private Dictionary<string, string> parameters = new Dictionary<string, string>();
}
