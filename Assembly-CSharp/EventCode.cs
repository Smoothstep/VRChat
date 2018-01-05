using System;

// Token: 0x0200073E RID: 1854
public class EventCode
{
	// Token: 0x040024F8 RID: 9464
	public const byte GameList = 230;

	// Token: 0x040024F9 RID: 9465
	public const byte GameListUpdate = 229;

	// Token: 0x040024FA RID: 9466
	public const byte QueueState = 228;

	// Token: 0x040024FB RID: 9467
	public const byte Match = 227;

	// Token: 0x040024FC RID: 9468
	public const byte AppStats = 226;

	// Token: 0x040024FD RID: 9469
	public const byte LobbyStats = 224;

	// Token: 0x040024FE RID: 9470
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureNodeInfo = 210;

	// Token: 0x040024FF RID: 9471
	public const byte Join = 255;

	// Token: 0x04002500 RID: 9472
	public const byte Leave = 254;

	// Token: 0x04002501 RID: 9473
	public const byte PropertiesChanged = 253;

	// Token: 0x04002502 RID: 9474
	[Obsolete("Use PropertiesChanged now.")]
	public const byte SetProperties = 253;

	// Token: 0x04002503 RID: 9475
	public const byte ErrorInfo = 251;

	// Token: 0x04002504 RID: 9476
	public const byte CacheSliceChanged = 250;

	// Token: 0x04002505 RID: 9477
	public const byte AuthEvent = 223;
}
