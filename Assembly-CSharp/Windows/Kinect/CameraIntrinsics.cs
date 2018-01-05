using System;

namespace Windows.Kinect
{
	// Token: 0x02000496 RID: 1174
	public struct CameraIntrinsics
	{
		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x0600283B RID: 10299 RVA: 0x000D138B File Offset: 0x000CF78B
		// (set) Token: 0x0600283C RID: 10300 RVA: 0x000D1393 File Offset: 0x000CF793
		public float FocalLengthX { get; set; }

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x000D139C File Offset: 0x000CF79C
		// (set) Token: 0x0600283E RID: 10302 RVA: 0x000D13A4 File Offset: 0x000CF7A4
		public float FocalLengthY { get; set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x0600283F RID: 10303 RVA: 0x000D13AD File Offset: 0x000CF7AD
		// (set) Token: 0x06002840 RID: 10304 RVA: 0x000D13B5 File Offset: 0x000CF7B5
		public float PrincipalPointX { get; set; }

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x000D13BE File Offset: 0x000CF7BE
		// (set) Token: 0x06002842 RID: 10306 RVA: 0x000D13C6 File Offset: 0x000CF7C6
		public float PrincipalPointY { get; set; }

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06002843 RID: 10307 RVA: 0x000D13CF File Offset: 0x000CF7CF
		// (set) Token: 0x06002844 RID: 10308 RVA: 0x000D13D7 File Offset: 0x000CF7D7
		public float RadialDistortionSecondOrder { get; set; }

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002845 RID: 10309 RVA: 0x000D13E0 File Offset: 0x000CF7E0
		// (set) Token: 0x06002846 RID: 10310 RVA: 0x000D13E8 File Offset: 0x000CF7E8
		public float RadialDistortionFourthOrder { get; set; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002847 RID: 10311 RVA: 0x000D13F1 File Offset: 0x000CF7F1
		// (set) Token: 0x06002848 RID: 10312 RVA: 0x000D13F9 File Offset: 0x000CF7F9
		public float RadialDistortionSixthOrder { get; set; }

		// Token: 0x06002849 RID: 10313 RVA: 0x000D1404 File Offset: 0x000CF804
		public override int GetHashCode()
		{
			return this.FocalLengthX.GetHashCode() ^ this.FocalLengthY.GetHashCode() ^ this.PrincipalPointX.GetHashCode() ^ this.PrincipalPointY.GetHashCode() ^ this.RadialDistortionSecondOrder.GetHashCode() ^ this.RadialDistortionFourthOrder.GetHashCode() ^ this.RadialDistortionSixthOrder.GetHashCode();
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000D14A6 File Offset: 0x000CF8A6
		public override bool Equals(object obj)
		{
			return obj is CameraIntrinsics && this.Equals((CameraIntrinsics)obj);
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000D14C4 File Offset: 0x000CF8C4
		public bool Equals(CameraIntrinsics obj)
		{
			return this.FocalLengthX.Equals(obj.FocalLengthX) && this.FocalLengthY.Equals(obj.FocalLengthY) && this.PrincipalPointX.Equals(obj.PrincipalPointX) && this.PrincipalPointY.Equals(obj.PrincipalPointY) && this.RadialDistortionSecondOrder.Equals(obj.RadialDistortionSecondOrder) && this.RadialDistortionFourthOrder.Equals(obj.RadialDistortionFourthOrder) && this.RadialDistortionSixthOrder.Equals(obj.RadialDistortionSixthOrder);
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000D1588 File Offset: 0x000CF988
		public static bool operator ==(CameraIntrinsics a, CameraIntrinsics b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000D1592 File Offset: 0x000CF992
		public static bool operator !=(CameraIntrinsics a, CameraIntrinsics b)
		{
			return !a.Equals(b);
		}
	}
}
