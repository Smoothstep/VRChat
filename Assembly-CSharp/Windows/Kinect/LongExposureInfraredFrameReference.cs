using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200050B RID: 1291
	public sealed class LongExposureInfraredFrameReference : INativeWrapper
	{
		// Token: 0x06002D5A RID: 11610 RVA: 0x000DFDD8 File Offset: 0x000DE1D8
		internal LongExposureInfraredFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrameReference.Windows_Kinect_LongExposureInfraredFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002D5B RID: 11611 RVA: 0x000DFDF2 File Offset: 0x000DE1F2
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000DFDFC File Offset: 0x000DE1FC
		~LongExposureInfraredFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D5D RID: 11613
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002D5E RID: 11614
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002D5F RID: 11615 RVA: 0x000DFE2C File Offset: 0x000DE22C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrameReference>(this._pNative);
			LongExposureInfraredFrameReference.Windows_Kinect_LongExposureInfraredFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002D60 RID: 11616
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_LongExposureInfraredFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002D61 RID: 11617 RVA: 0x000DFE6B File Offset: 0x000DE26B
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)LongExposureInfraredFrameReference.Windows_Kinect_LongExposureInfraredFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002D62 RID: 11618
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06002D63 RID: 11619 RVA: 0x000DFEA0 File Offset: 0x000DE2A0
		public LongExposureInfraredFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrameReference");
			}
			IntPtr intPtr = LongExposureInfraredFrameReference.Windows_Kinect_LongExposureInfraredFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrame>(intPtr, (IntPtr n) => new LongExposureInfraredFrame(n));
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000DFF13 File Offset: 0x000DE313
		private void __EventCleanup()
		{
		}

		// Token: 0x04001826 RID: 6182
		internal IntPtr _pNative;
	}
}
