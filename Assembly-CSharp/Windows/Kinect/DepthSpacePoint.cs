using System;

namespace Windows.Kinect
{
	// Token: 0x020004ED RID: 1261
	public struct DepthSpacePoint
	{
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002C7C RID: 11388 RVA: 0x000DDAC4 File Offset: 0x000DBEC4
		// (set) Token: 0x06002C7D RID: 11389 RVA: 0x000DDACC File Offset: 0x000DBECC
		public float X { get; set; }

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002C7E RID: 11390 RVA: 0x000DDAD5 File Offset: 0x000DBED5
		// (set) Token: 0x06002C7F RID: 11391 RVA: 0x000DDADD File Offset: 0x000DBEDD
		public float Y { get; set; }

		// Token: 0x06002C80 RID: 11392 RVA: 0x000DDAE8 File Offset: 0x000DBEE8
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x000DDB1E File Offset: 0x000DBF1E
		public override bool Equals(object obj)
		{
			return obj is DepthSpacePoint && this.Equals((DepthSpacePoint)obj);
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x000DDB3C File Offset: 0x000DBF3C
		public bool Equals(DepthSpacePoint obj)
		{
			return this.X.Equals(obj.X) && this.Y.Equals(obj.Y);
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x000DDB7B File Offset: 0x000DBF7B
		public static bool operator ==(DepthSpacePoint a, DepthSpacePoint b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x000DDB85 File Offset: 0x000DBF85
		public static bool operator !=(DepthSpacePoint a, DepthSpacePoint b)
		{
			return !a.Equals(b);
		}
	}
}
