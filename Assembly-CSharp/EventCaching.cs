using System;

// Token: 0x02000744 RID: 1860
public enum EventCaching : byte
{
	// Token: 0x0400256B RID: 9579
	DoNotCache,
	// Token: 0x0400256C RID: 9580
	[Obsolete]
	MergeCache,
	// Token: 0x0400256D RID: 9581
	[Obsolete]
	ReplaceCache,
	// Token: 0x0400256E RID: 9582
	[Obsolete]
	RemoveCache,
	// Token: 0x0400256F RID: 9583
	AddToRoomCache,
	// Token: 0x04002570 RID: 9584
	AddToRoomCacheGlobal,
	// Token: 0x04002571 RID: 9585
	RemoveFromRoomCache,
	// Token: 0x04002572 RID: 9586
	RemoveFromRoomCacheForActorsLeft,
	// Token: 0x04002573 RID: 9587
	SliceIncreaseIndex = 10,
	// Token: 0x04002574 RID: 9588
	SliceSetIndex,
	// Token: 0x04002575 RID: 9589
	SlicePurgeIndex,
	// Token: 0x04002576 RID: 9590
	SlicePurgeUpToIndex
}
