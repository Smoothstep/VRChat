using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004D3 RID: 1235
	public sealed class BodyIndexFrameReference : INativeWrapper
	{
		// Token: 0x06002B5E RID: 11102 RVA: 0x000DA648 File Offset: 0x000D8A48
		internal BodyIndexFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrameReference.Windows_Kinect_BodyIndexFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002B5F RID: 11103 RVA: 0x000DA662 File Offset: 0x000D8A62
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000DA66C File Offset: 0x000D8A6C
		~BodyIndexFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06002B61 RID: 11105
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002B62 RID: 11106
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002B63 RID: 11107 RVA: 0x000DA69C File Offset: 0x000D8A9C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrameReference>(this._pNative);
			BodyIndexFrameReference.Windows_Kinect_BodyIndexFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002B64 RID: 11108
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_BodyIndexFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002B65 RID: 11109 RVA: 0x000DA6DB File Offset: 0x000D8ADB
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)BodyIndexFrameReference.Windows_Kinect_BodyIndexFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002B66 RID: 11110
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06002B67 RID: 11111 RVA: 0x000DA710 File Offset: 0x000D8B10
		public BodyIndexFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrameReference");
			}
			IntPtr intPtr = BodyIndexFrameReference.Windows_Kinect_BodyIndexFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyIndexFrame>(intPtr, (IntPtr n) => new BodyIndexFrame(n));
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x000DA783 File Offset: 0x000D8B83
		private void __EventCleanup()
		{
		}

		// Token: 0x0400175B RID: 5979
		internal IntPtr _pNative;
	}
}
