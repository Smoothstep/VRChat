using System;

// Token: 0x02000740 RID: 1856
public class OperationCode
{
	// Token: 0x04002549 RID: 9545
	[Obsolete("Exchanging encrpytion keys is done internally in the lib now. Don't expect this operation-result.")]
	public const byte ExchangeKeysForEncryption = 250;

	// Token: 0x0400254A RID: 9546
	public const byte Join = 255;

	// Token: 0x0400254B RID: 9547
	public const byte AuthenticateOnce = 231;

	// Token: 0x0400254C RID: 9548
	public const byte Authenticate = 230;

	// Token: 0x0400254D RID: 9549
	public const byte JoinLobby = 229;

	// Token: 0x0400254E RID: 9550
	public const byte LeaveLobby = 228;

	// Token: 0x0400254F RID: 9551
	public const byte CreateGame = 227;

	// Token: 0x04002550 RID: 9552
	public const byte JoinGame = 226;

	// Token: 0x04002551 RID: 9553
	public const byte JoinRandomGame = 225;

	// Token: 0x04002552 RID: 9554
	public const byte Leave = 254;

	// Token: 0x04002553 RID: 9555
	public const byte RaiseEvent = 253;

	// Token: 0x04002554 RID: 9556
	public const byte SetProperties = 252;

	// Token: 0x04002555 RID: 9557
	public const byte GetProperties = 251;

	// Token: 0x04002556 RID: 9558
	public const byte ChangeGroups = 248;

	// Token: 0x04002557 RID: 9559
	public const byte FindFriends = 222;

	// Token: 0x04002558 RID: 9560
	public const byte GetLobbyStats = 221;

	// Token: 0x04002559 RID: 9561
	public const byte GetRegions = 220;

	// Token: 0x0400255A RID: 9562
	public const byte WebRpc = 219;

	// Token: 0x0400255B RID: 9563
	public const byte ServerSettings = 218;

	// Token: 0x0400255C RID: 9564
	public const byte GetGameList = 217;
}
