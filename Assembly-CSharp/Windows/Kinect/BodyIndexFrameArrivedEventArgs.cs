using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004CF RID: 1231
	public sealed class BodyIndexFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002B30 RID: 11056 RVA: 0x000D9CD8 File Offset: 0x000D80D8
		internal BodyIndexFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrameArrivedEventArgs.Windows_Kinect_BodyIndexFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06002B31 RID: 11057 RVA: 0x000D9CF2 File Offset: 0x000D80F2
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000D9CFC File Offset: 0x000D80FC
		~BodyIndexFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002B33 RID: 11059
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002B34 RID: 11060
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002B35 RID: 11061 RVA: 0x000D9D2C File Offset: 0x000D812C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrameArrivedEventArgs>(this._pNative);
			BodyIndexFrameArrivedEventArgs.Windows_Kinect_BodyIndexFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002B36 RID: 11062
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06002B37 RID: 11063 RVA: 0x000D9D6C File Offset: 0x000D816C
		public BodyIndexFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameArrivedEventArgs");
				}
				IntPtr intPtr = BodyIndexFrameArrivedEventArgs.Windows_Kinect_BodyIndexFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameReference>(intPtr, (IntPtr n) => new BodyIndexFrameReference(n));
			}
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x000D9DDF File Offset: 0x000D81DF
		private void __EventCleanup()
		{
		}

		// Token: 0x0400174E RID: 5966
		internal IntPtr _pNative;
	}
}
