using System;

// Token: 0x0200074A RID: 1866
public class TypedLobbyInfo : TypedLobby
{
	// Token: 0x06003BF7 RID: 15351 RVA: 0x0012D4E8 File Offset: 0x0012B8E8
	public override string ToString()
	{
		return string.Format("TypedLobbyInfo '{0}'[{1}] rooms: {2} players: {3}", new object[]
		{
			this.Name,
			this.Type,
			this.RoomCount,
			this.PlayerCount
		});
	}

	// Token: 0x04002597 RID: 9623
	public int PlayerCount;

	// Token: 0x04002598 RID: 9624
	public int RoomCount;
}
