using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004C3 RID: 1219
	public sealed class AudioBodyCorrelation : INativeWrapper
	{
		// Token: 0x06002A9B RID: 10907 RVA: 0x000D7F7E File Offset: 0x000D637E
		internal AudioBodyCorrelation(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBodyCorrelation.Windows_Kinect_AudioBodyCorrelation_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002A9C RID: 10908 RVA: 0x000D7F98 File Offset: 0x000D6398
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000D7FA0 File Offset: 0x000D63A0
		~AudioBodyCorrelation()
		{
			this.Dispose(false);
		}

		// Token: 0x06002A9E RID: 10910
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBodyCorrelation_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002A9F RID: 10911
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBodyCorrelation_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000D7FD0 File Offset: 0x000D63D0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBodyCorrelation>(this._pNative);
			AudioBodyCorrelation.Windows_Kinect_AudioBodyCorrelation_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002AA1 RID: 10913
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ulong Windows_Kinect_AudioBodyCorrelation_get_BodyTrackingId(IntPtr pNative);

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002AA2 RID: 10914 RVA: 0x000D800F File Offset: 0x000D640F
		public ulong BodyTrackingId
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBodyCorrelation");
				}
				return AudioBodyCorrelation.Windows_Kinect_AudioBodyCorrelation_get_BodyTrackingId(this._pNative);
			}
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x000D803C File Offset: 0x000D643C
		private void __EventCleanup()
		{
		}

		// Token: 0x04001727 RID: 5927
		internal IntPtr _pNative;
	}
}
