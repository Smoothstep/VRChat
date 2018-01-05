using System;

// Token: 0x02000B82 RID: 2946
public enum LoadErrorReason
{
	// Token: 0x04004169 RID: 16745
	Unknown,
	// Token: 0x0400416A RID: 16746
	ConnectionError,
	// Token: 0x0400416B RID: 16747
	ServerReturnedError,
	// Token: 0x0400416C RID: 16748
	AssetBundleCorrupt,
	// Token: 0x0400416D RID: 16749
	AssetBundleInvalidOrNull,
	// Token: 0x0400416E RID: 16750
	DuplicateLoadFailed,
	// Token: 0x0400416F RID: 16751
	InvalidURL,
	// Token: 0x04004170 RID: 16752
	PluginLoadFailed,
	// Token: 0x04004171 RID: 16753
	Cancelled
}
