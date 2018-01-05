using System;

namespace Windows.Kinect
{
	// Token: 0x02000500 RID: 1280
	public struct JointOrientation
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x000DF382 File Offset: 0x000DD782
		// (set) Token: 0x06002D19 RID: 11545 RVA: 0x000DF38A File Offset: 0x000DD78A
		public JointType JointType { get; set; }

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x000DF393 File Offset: 0x000DD793
		// (set) Token: 0x06002D1B RID: 11547 RVA: 0x000DF39B File Offset: 0x000DD79B
		public Vector4 Orientation { get; set; }

		// Token: 0x06002D1C RID: 11548 RVA: 0x000DF3A4 File Offset: 0x000DD7A4
		public override int GetHashCode()
		{
			return this.JointType.GetHashCode() ^ this.Orientation.GetHashCode();
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000DF3DA File Offset: 0x000DD7DA
		public override bool Equals(object obj)
		{
			return obj is JointOrientation && this.Equals((JointOrientation)obj);
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000DF3F8 File Offset: 0x000DD7F8
		public bool Equals(JointOrientation obj)
		{
			return this.JointType.Equals(obj.JointType) && this.Orientation.Equals(obj.Orientation);
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000DF442 File Offset: 0x000DD842
		public static bool operator ==(JointOrientation a, JointOrientation b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000DF44C File Offset: 0x000DD84C
		public static bool operator !=(JointOrientation a, JointOrientation b)
		{
			return !a.Equals(b);
		}
	}
}
