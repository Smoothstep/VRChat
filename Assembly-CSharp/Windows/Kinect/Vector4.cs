using System;

namespace Windows.Kinect
{
	// Token: 0x02000517 RID: 1303
	public struct Vector4
	{
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002DDC RID: 11740 RVA: 0x000E15AD File Offset: 0x000DF9AD
		// (set) Token: 0x06002DDD RID: 11741 RVA: 0x000E15B5 File Offset: 0x000DF9B5
		public float X { get; set; }

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002DDE RID: 11742 RVA: 0x000E15BE File Offset: 0x000DF9BE
		// (set) Token: 0x06002DDF RID: 11743 RVA: 0x000E15C6 File Offset: 0x000DF9C6
		public float Y { get; set; }

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002DE0 RID: 11744 RVA: 0x000E15CF File Offset: 0x000DF9CF
		// (set) Token: 0x06002DE1 RID: 11745 RVA: 0x000E15D7 File Offset: 0x000DF9D7
		public float Z { get; set; }

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002DE2 RID: 11746 RVA: 0x000E15E0 File Offset: 0x000DF9E0
		// (set) Token: 0x06002DE3 RID: 11747 RVA: 0x000E15E8 File Offset: 0x000DF9E8
		public float W { get; set; }

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000E15F4 File Offset: 0x000DF9F4
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode() ^ this.W.GetHashCode();
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000E1654 File Offset: 0x000DFA54
		public override bool Equals(object obj)
		{
			return obj is Vector4 && this.Equals((Vector4)obj);
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000E1670 File Offset: 0x000DFA70
		public bool Equals(Vector4 obj)
		{
			return this.X.Equals(obj.X) && this.Y.Equals(obj.Y) && this.Z.Equals(obj.Z) && this.W.Equals(obj.W);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000E16E3 File Offset: 0x000DFAE3
		public static bool operator ==(Vector4 a, Vector4 b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000E16ED File Offset: 0x000DFAED
		public static bool operator !=(Vector4 a, Vector4 b)
		{
			return !a.Equals(b);
		}
	}
}
