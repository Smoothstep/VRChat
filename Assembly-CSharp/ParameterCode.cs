using System;

// Token: 0x0200073F RID: 1855
public class ParameterCode
{
	// Token: 0x04002506 RID: 9478
	public const byte SuppressRoomEvents = 237;

	// Token: 0x04002507 RID: 9479
	public const byte EmptyRoomTTL = 236;

	// Token: 0x04002508 RID: 9480
	public const byte PlayerTTL = 235;

	// Token: 0x04002509 RID: 9481
	public const byte EventForward = 234;

	// Token: 0x0400250A RID: 9482
	[Obsolete("Use: IsInactive")]
	public const byte IsComingBack = 233;

	// Token: 0x0400250B RID: 9483
	public const byte IsInactive = 233;

	// Token: 0x0400250C RID: 9484
	public const byte CheckUserOnJoin = 232;

	// Token: 0x0400250D RID: 9485
	public const byte ExpectedValues = 231;

	// Token: 0x0400250E RID: 9486
	public const byte Address = 230;

	// Token: 0x0400250F RID: 9487
	public const byte PeerCount = 229;

	// Token: 0x04002510 RID: 9488
	public const byte GameCount = 228;

	// Token: 0x04002511 RID: 9489
	public const byte MasterPeerCount = 227;

	// Token: 0x04002512 RID: 9490
	public const byte UserId = 225;

	// Token: 0x04002513 RID: 9491
	public const byte ApplicationId = 224;

	// Token: 0x04002514 RID: 9492
	public const byte Position = 223;

	// Token: 0x04002515 RID: 9493
	public const byte MatchMakingType = 223;

	// Token: 0x04002516 RID: 9494
	public const byte GameList = 222;

	// Token: 0x04002517 RID: 9495
	public const byte Secret = 221;

	// Token: 0x04002518 RID: 9496
	public const byte AppVersion = 220;

	// Token: 0x04002519 RID: 9497
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureNodeInfo = 210;

	// Token: 0x0400251A RID: 9498
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureLocalNodeId = 209;

	// Token: 0x0400251B RID: 9499
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureMasterNodeId = 208;

	// Token: 0x0400251C RID: 9500
	public const byte RoomName = 255;

	// Token: 0x0400251D RID: 9501
	public const byte Broadcast = 250;

	// Token: 0x0400251E RID: 9502
	public const byte ActorList = 252;

	// Token: 0x0400251F RID: 9503
	public const byte ActorNr = 254;

	// Token: 0x04002520 RID: 9504
	public const byte PlayerProperties = 249;

	// Token: 0x04002521 RID: 9505
	public const byte CustomEventContent = 245;

	// Token: 0x04002522 RID: 9506
	public const byte Data = 245;

	// Token: 0x04002523 RID: 9507
	public const byte Code = 244;

	// Token: 0x04002524 RID: 9508
	public const byte GameProperties = 248;

	// Token: 0x04002525 RID: 9509
	public const byte Properties = 251;

	// Token: 0x04002526 RID: 9510
	public const byte TargetActorNr = 253;

	// Token: 0x04002527 RID: 9511
	public const byte ReceiverGroup = 246;

	// Token: 0x04002528 RID: 9512
	public const byte Cache = 247;

	// Token: 0x04002529 RID: 9513
	public const byte CleanupCacheOnLeave = 241;

	// Token: 0x0400252A RID: 9514
	public const byte Group = 240;

	// Token: 0x0400252B RID: 9515
	public const byte Remove = 239;

	// Token: 0x0400252C RID: 9516
	public const byte PublishUserId = 239;

	// Token: 0x0400252D RID: 9517
	public const byte Add = 238;

	// Token: 0x0400252E RID: 9518
	public const byte Info = 218;

	// Token: 0x0400252F RID: 9519
	public const byte ClientAuthenticationType = 217;

	// Token: 0x04002530 RID: 9520
	public const byte ClientAuthenticationParams = 216;

	// Token: 0x04002531 RID: 9521
	public const byte JoinMode = 215;

	// Token: 0x04002532 RID: 9522
	public const byte ClientAuthenticationData = 214;

	// Token: 0x04002533 RID: 9523
	public const byte MasterClientId = 203;

	// Token: 0x04002534 RID: 9524
	public const byte FindFriendsRequestList = 1;

	// Token: 0x04002535 RID: 9525
	public const byte FindFriendsResponseOnlineList = 1;

	// Token: 0x04002536 RID: 9526
	public const byte FindFriendsResponseRoomIdList = 2;

	// Token: 0x04002537 RID: 9527
	public const byte LobbyName = 213;

	// Token: 0x04002538 RID: 9528
	public const byte LobbyType = 212;

	// Token: 0x04002539 RID: 9529
	public const byte LobbyStats = 211;

	// Token: 0x0400253A RID: 9530
	public const byte Region = 210;

	// Token: 0x0400253B RID: 9531
	public const byte UriPath = 209;

	// Token: 0x0400253C RID: 9532
	public const byte WebRpcParameters = 208;

	// Token: 0x0400253D RID: 9533
	public const byte WebRpcReturnCode = 207;

	// Token: 0x0400253E RID: 9534
	public const byte WebRpcReturnMessage = 206;

	// Token: 0x0400253F RID: 9535
	public const byte CacheSliceIndex = 205;

	// Token: 0x04002540 RID: 9536
	public const byte Plugins = 204;

	// Token: 0x04002541 RID: 9537
	public const byte NickName = 202;

	// Token: 0x04002542 RID: 9538
	public const byte PluginName = 201;

	// Token: 0x04002543 RID: 9539
	public const byte PluginVersion = 200;

	// Token: 0x04002544 RID: 9540
	public const byte ExpectedProtocol = 195;

	// Token: 0x04002545 RID: 9541
	public const byte CustomInitData = 194;

	// Token: 0x04002546 RID: 9542
	public const byte EncryptionMode = 193;

	// Token: 0x04002547 RID: 9543
	public const byte EncryptionData = 192;

	// Token: 0x04002548 RID: 9544
	public const byte RoomOptionFlags = 191;
}
