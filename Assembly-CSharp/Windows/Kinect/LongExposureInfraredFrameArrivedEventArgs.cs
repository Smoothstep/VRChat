using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000507 RID: 1287
	public sealed class LongExposureInfraredFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002D2C RID: 11564 RVA: 0x000DF469 File Offset: 0x000DD869
		internal LongExposureInfraredFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrameArrivedEventArgs.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002D2D RID: 11565 RVA: 0x000DF483 File Offset: 0x000DD883
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000DF48C File Offset: 0x000DD88C
		~LongExposureInfraredFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D2F RID: 11567
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002D30 RID: 11568
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002D31 RID: 11569 RVA: 0x000DF4BC File Offset: 0x000DD8BC
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrameArrivedEventArgs>(this._pNative);
			LongExposureInfraredFrameArrivedEventArgs.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002D32 RID: 11570
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x000DF4FC File Offset: 0x000DD8FC
		public LongExposureInfraredFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameArrivedEventArgs");
				}
				IntPtr intPtr = LongExposureInfraredFrameArrivedEventArgs.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameReference>(intPtr, (IntPtr n) => new LongExposureInfraredFrameReference(n));
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000DF56F File Offset: 0x000DD96F
		private void __EventCleanup()
		{
		}

		// Token: 0x04001819 RID: 6169
		internal IntPtr _pNative;
	}
}
