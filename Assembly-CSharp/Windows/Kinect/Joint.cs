using System;

namespace Windows.Kinect
{
	// Token: 0x020004FF RID: 1279
	public struct Joint
	{
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002D0D RID: 11533 RVA: 0x000DF25E File Offset: 0x000DD65E
		// (set) Token: 0x06002D0E RID: 11534 RVA: 0x000DF266 File Offset: 0x000DD666
		public JointType JointType { get; set; }

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002D0F RID: 11535 RVA: 0x000DF26F File Offset: 0x000DD66F
		// (set) Token: 0x06002D10 RID: 11536 RVA: 0x000DF277 File Offset: 0x000DD677
		public CameraSpacePoint Position { get; set; }

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002D11 RID: 11537 RVA: 0x000DF280 File Offset: 0x000DD680
		// (set) Token: 0x06002D12 RID: 11538 RVA: 0x000DF288 File Offset: 0x000DD688
		public TrackingState TrackingState { get; set; }

		// Token: 0x06002D13 RID: 11539 RVA: 0x000DF294 File Offset: 0x000DD694
		public override int GetHashCode()
		{
			return this.JointType.GetHashCode() ^ this.Position.GetHashCode() ^ this.TrackingState.GetHashCode();
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000DF2DF File Offset: 0x000DD6DF
		public override bool Equals(object obj)
		{
			return obj is Joint && this.Equals((Joint)obj);
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000DF2FC File Offset: 0x000DD6FC
		public bool Equals(Joint obj)
		{
			return this.JointType.Equals(obj.JointType) && this.Position.Equals(obj.Position) && this.TrackingState.Equals(obj.TrackingState);
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000DF36B File Offset: 0x000DD76B
		public static bool operator ==(Joint a, Joint b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000DF375 File Offset: 0x000DD775
		public static bool operator !=(Joint a, Joint b)
		{
			return !a.Equals(b);
		}
	}
}
