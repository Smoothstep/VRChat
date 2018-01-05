using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004C7 RID: 1223
	public sealed class BodyFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002AD0 RID: 10960 RVA: 0x000D89A0 File Offset: 0x000D6DA0
		internal BodyFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrameArrivedEventArgs.Windows_Kinect_BodyFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000D89BA File Offset: 0x000D6DBA
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000D89C4 File Offset: 0x000D6DC4
		~BodyFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002AD3 RID: 10963
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002AD4 RID: 10964
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000D89F4 File Offset: 0x000D6DF4
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrameArrivedEventArgs>(this._pNative);
			BodyFrameArrivedEventArgs.Windows_Kinect_BodyFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002AD6 RID: 10966
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x000D8A34 File Offset: 0x000D6E34
		public BodyFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameArrivedEventArgs");
				}
				IntPtr intPtr = BodyFrameArrivedEventArgs.Windows_Kinect_BodyFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameReference>(intPtr, (IntPtr n) => new BodyFrameReference(n));
			}
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x000D8AA7 File Offset: 0x000D6EA7
		private void __EventCleanup()
		{
		}

		// Token: 0x04001734 RID: 5940
		internal IntPtr _pNative;
	}
}
