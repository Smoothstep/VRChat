using System;

namespace Windows.Kinect
{
	// Token: 0x020004D7 RID: 1239
	public struct CameraSpacePoint
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06002B8D RID: 11149 RVA: 0x000DAFF4 File Offset: 0x000D93F4
		// (set) Token: 0x06002B8E RID: 11150 RVA: 0x000DAFFC File Offset: 0x000D93FC
		public float X { get; set; }

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002B8F RID: 11151 RVA: 0x000DB005 File Offset: 0x000D9405
		// (set) Token: 0x06002B90 RID: 11152 RVA: 0x000DB00D File Offset: 0x000D940D
		public float Y { get; set; }

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002B91 RID: 11153 RVA: 0x000DB016 File Offset: 0x000D9416
		// (set) Token: 0x06002B92 RID: 11154 RVA: 0x000DB01E File Offset: 0x000D941E
		public float Z { get; set; }

		// Token: 0x06002B93 RID: 11155 RVA: 0x000DB028 File Offset: 0x000D9428
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000DB073 File Offset: 0x000D9473
		public override bool Equals(object obj)
		{
			return obj is CameraSpacePoint && this.Equals((CameraSpacePoint)obj);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000DB090 File Offset: 0x000D9490
		public bool Equals(CameraSpacePoint obj)
		{
			return this.X.Equals(obj.X) && this.Y.Equals(obj.Y) && this.Z.Equals(obj.Z);
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x000DB0E9 File Offset: 0x000D94E9
		public static bool operator ==(CameraSpacePoint a, CameraSpacePoint b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000DB0F3 File Offset: 0x000D94F3
		public static bool operator !=(CameraSpacePoint a, CameraSpacePoint b)
		{
			return !a.Equals(b);
		}
	}
}
