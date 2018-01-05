using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004E5 RID: 1253
	public sealed class DepthFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002C1B RID: 11291 RVA: 0x000DC74D File Offset: 0x000DAB4D
		internal DepthFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrameArrivedEventArgs.Windows_Kinect_DepthFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002C1C RID: 11292 RVA: 0x000DC767 File Offset: 0x000DAB67
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x000DC770 File Offset: 0x000DAB70
		~DepthFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002C1E RID: 11294
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002C1F RID: 11295
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002C20 RID: 11296 RVA: 0x000DC7A0 File Offset: 0x000DABA0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrameArrivedEventArgs>(this._pNative);
			DepthFrameArrivedEventArgs.Windows_Kinect_DepthFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002C21 RID: 11297
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002C22 RID: 11298 RVA: 0x000DC7E0 File Offset: 0x000DABE0
		public DepthFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameArrivedEventArgs");
				}
				IntPtr intPtr = DepthFrameArrivedEventArgs.Windows_Kinect_DepthFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameReference>(intPtr, (IntPtr n) => new DepthFrameReference(n));
			}
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000DC853 File Offset: 0x000DAC53
		private void __EventCleanup()
		{
		}

		// Token: 0x04001793 RID: 6035
		internal IntPtr _pNative;
	}
}
