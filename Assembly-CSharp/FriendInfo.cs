using System;

// Token: 0x02000734 RID: 1844
public class FriendInfo
{
	// Token: 0x17000953 RID: 2387
	// (get) Token: 0x06003BA9 RID: 15273 RVA: 0x0012C295 File Offset: 0x0012A695
	// (set) Token: 0x06003BAA RID: 15274 RVA: 0x0012C29D File Offset: 0x0012A69D
	public string Name { get; protected internal set; }

	// Token: 0x17000954 RID: 2388
	// (get) Token: 0x06003BAB RID: 15275 RVA: 0x0012C2A6 File Offset: 0x0012A6A6
	// (set) Token: 0x06003BAC RID: 15276 RVA: 0x0012C2AE File Offset: 0x0012A6AE
	public bool IsOnline { get; protected internal set; }

	// Token: 0x17000955 RID: 2389
	// (get) Token: 0x06003BAD RID: 15277 RVA: 0x0012C2B7 File Offset: 0x0012A6B7
	// (set) Token: 0x06003BAE RID: 15278 RVA: 0x0012C2BF File Offset: 0x0012A6BF
	public string Room { get; protected internal set; }

	// Token: 0x17000956 RID: 2390
	// (get) Token: 0x06003BAF RID: 15279 RVA: 0x0012C2C8 File Offset: 0x0012A6C8
	public bool IsInRoom
	{
		get
		{
			return this.IsOnline && !string.IsNullOrEmpty(this.Room);
		}
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x0012C2E8 File Offset: 0x0012A6E8
	public override string ToString()
	{
		return string.Format("{0}\t is: {1}", this.Name, this.IsOnline ? ((!this.IsInRoom) ? "on master" : "playing") : "offline");
	}
}
