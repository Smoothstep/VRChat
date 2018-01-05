using System;

namespace Windows.Kinect
{
	// Token: 0x0200049C RID: 1180
	public struct PointF
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x0600286A RID: 10346 RVA: 0x000D1BDF File Offset: 0x000CFFDF
		// (set) Token: 0x0600286B RID: 10347 RVA: 0x000D1BE7 File Offset: 0x000CFFE7
		public float X { get; set; }

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x0600286C RID: 10348 RVA: 0x000D1BF0 File Offset: 0x000CFFF0
		// (set) Token: 0x0600286D RID: 10349 RVA: 0x000D1BF8 File Offset: 0x000CFFF8
		public float Y { get; set; }

		// Token: 0x0600286E RID: 10350 RVA: 0x000D1C04 File Offset: 0x000D0004
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000D1C3A File Offset: 0x000D003A
		public override bool Equals(object obj)
		{
			return obj is PointF && this.Equals((ColorSpacePoint)obj);
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000D1C55 File Offset: 0x000D0055
		public bool Equals(ColorSpacePoint obj)
		{
			return this.X == obj.X && this.Y == obj.Y;
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000D1C7B File Offset: 0x000D007B
		public static bool operator ==(PointF a, PointF b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000D1C90 File Offset: 0x000D0090
		public static bool operator !=(PointF a, PointF b)
		{
			return !a.Equals(b);
		}
	}
}
