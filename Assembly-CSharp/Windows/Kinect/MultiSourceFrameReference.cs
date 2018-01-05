using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000514 RID: 1300
	public sealed class MultiSourceFrameReference : INativeWrapper
	{
		// Token: 0x06002DD2 RID: 11730 RVA: 0x000E149C File Offset: 0x000DF89C
		internal MultiSourceFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			MultiSourceFrameReference.Windows_Kinect_MultiSourceFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002DD3 RID: 11731 RVA: 0x000E14B6 File Offset: 0x000DF8B6
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000E14C0 File Offset: 0x000DF8C0
		~MultiSourceFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06002DD5 RID: 11733
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002DD6 RID: 11734
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000E14F0 File Offset: 0x000DF8F0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<MultiSourceFrameReference>(this._pNative);
			MultiSourceFrameReference.Windows_Kinect_MultiSourceFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002DD8 RID: 11736
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000E1530 File Offset: 0x000DF930
		public MultiSourceFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("MultiSourceFrameReference");
			}
			IntPtr intPtr = MultiSourceFrameReference.Windows_Kinect_MultiSourceFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<MultiSourceFrame>(intPtr, (IntPtr n) => new MultiSourceFrame(n));
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000E15A3 File Offset: 0x000DF9A3
		private void __EventCleanup()
		{
		}

		// Token: 0x04001848 RID: 6216
		internal IntPtr _pNative;
	}
}
