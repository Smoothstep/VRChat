using System;

namespace Windows.Kinect
{
	// Token: 0x020004E2 RID: 1250
	public struct ColorSpacePoint
	{
		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002C07 RID: 11271 RVA: 0x000DC5EC File Offset: 0x000DA9EC
		// (set) Token: 0x06002C08 RID: 11272 RVA: 0x000DC5F4 File Offset: 0x000DA9F4
		public float X { get; set; }

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06002C09 RID: 11273 RVA: 0x000DC5FD File Offset: 0x000DA9FD
		// (set) Token: 0x06002C0A RID: 11274 RVA: 0x000DC605 File Offset: 0x000DAA05
		public float Y { get; set; }

		// Token: 0x06002C0B RID: 11275 RVA: 0x000DC610 File Offset: 0x000DAA10
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x000DC646 File Offset: 0x000DAA46
		public override bool Equals(object obj)
		{
			return obj is ColorSpacePoint && this.Equals((ColorSpacePoint)obj);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000DC664 File Offset: 0x000DAA64
		public bool Equals(ColorSpacePoint obj)
		{
			return this.X.Equals(obj.X) && this.Y.Equals(obj.Y);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000DC6A3 File Offset: 0x000DAAA3
		public static bool operator ==(ColorSpacePoint a, ColorSpacePoint b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000DC6AD File Offset: 0x000DAAAD
		public static bool operator !=(ColorSpacePoint a, ColorSpacePoint b)
		{
			return !a.Equals(b);
		}
	}
}
