using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004F0 RID: 1264
	public sealed class FrameCapturedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002C85 RID: 11397 RVA: 0x000DDB92 File Offset: 0x000DBF92
		internal FrameCapturedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002C86 RID: 11398 RVA: 0x000DDBAC File Offset: 0x000DBFAC
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x000DDBB4 File Offset: 0x000DBFB4
		~FrameCapturedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002C88 RID: 11400
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_FrameCapturedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002C89 RID: 11401
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_FrameCapturedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002C8A RID: 11402 RVA: 0x000DDBE4 File Offset: 0x000DBFE4
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<FrameCapturedEventArgs>(this._pNative);
			FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002C8B RID: 11403
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern FrameCapturedStatus Windows_Kinect_FrameCapturedEventArgs_get_FrameStatus(IntPtr pNative);

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002C8C RID: 11404 RVA: 0x000DDC23 File Offset: 0x000DC023
		public FrameCapturedStatus FrameStatus
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameCapturedEventArgs");
				}
				return FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_get_FrameStatus(this._pNative);
			}
		}

		// Token: 0x06002C8D RID: 11405
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern FrameSourceTypes Windows_Kinect_FrameCapturedEventArgs_get_FrameType(IntPtr pNative);

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x000DDC50 File Offset: 0x000DC050
		public FrameSourceTypes FrameType
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameCapturedEventArgs");
				}
				return FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_get_FrameType(this._pNative);
			}
		}

		// Token: 0x06002C8F RID: 11407
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_FrameCapturedEventArgs_get_RelativeTime(IntPtr pNative);

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002C90 RID: 11408 RVA: 0x000DDC7D File Offset: 0x000DC07D
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameCapturedEventArgs");
				}
				return TimeSpan.FromMilliseconds((double)FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x000DDCB0 File Offset: 0x000DC0B0
		private void __EventCleanup()
		{
		}

		// Token: 0x040017B8 RID: 6072
		internal IntPtr _pNative;
	}
}
