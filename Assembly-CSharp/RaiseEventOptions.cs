using System;

// Token: 0x02000747 RID: 1863
public class RaiseEventOptions
{
	// Token: 0x04002588 RID: 9608
	public static readonly RaiseEventOptions Default = new RaiseEventOptions();

	// Token: 0x04002589 RID: 9609
	public EventCaching CachingOption;

	// Token: 0x0400258A RID: 9610
	public byte InterestGroup;

	// Token: 0x0400258B RID: 9611
	public int[] TargetActors;

	// Token: 0x0400258C RID: 9612
	public ReceiverGroup Receivers;

	// Token: 0x0400258D RID: 9613
	public byte SequenceChannel;

	// Token: 0x0400258E RID: 9614
	public bool ForwardToWebhook;

	// Token: 0x0400258F RID: 9615
	public bool Encrypt;
}
