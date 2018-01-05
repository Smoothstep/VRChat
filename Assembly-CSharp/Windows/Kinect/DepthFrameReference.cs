using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004E9 RID: 1257
	public sealed class DepthFrameReference : INativeWrapper
	{
		// Token: 0x06002C49 RID: 11337 RVA: 0x000DD0BC File Offset: 0x000DB4BC
		internal DepthFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrameReference.Windows_Kinect_DepthFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002C4A RID: 11338 RVA: 0x000DD0D6 File Offset: 0x000DB4D6
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x000DD0E0 File Offset: 0x000DB4E0
		~DepthFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06002C4C RID: 11340
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002C4D RID: 11341
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002C4E RID: 11342 RVA: 0x000DD110 File Offset: 0x000DB510
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrameReference>(this._pNative);
			DepthFrameReference.Windows_Kinect_DepthFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002C4F RID: 11343
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_DepthFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002C50 RID: 11344 RVA: 0x000DD14F File Offset: 0x000DB54F
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)DepthFrameReference.Windows_Kinect_DepthFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002C51 RID: 11345
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06002C52 RID: 11346 RVA: 0x000DD184 File Offset: 0x000DB584
		public DepthFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrameReference");
			}
			IntPtr intPtr = DepthFrameReference.Windows_Kinect_DepthFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<DepthFrame>(intPtr, (IntPtr n) => new DepthFrame(n));
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x000DD1F7 File Offset: 0x000DB5F7
		private void __EventCleanup()
		{
		}

		// Token: 0x040017A0 RID: 6048
		internal IntPtr _pNative;
	}
}
