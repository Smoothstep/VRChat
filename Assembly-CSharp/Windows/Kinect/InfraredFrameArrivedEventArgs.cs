using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004F6 RID: 1270
	public sealed class InfraredFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002CA7 RID: 11431 RVA: 0x000DDE80 File Offset: 0x000DC280
		internal InfraredFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrameArrivedEventArgs.Windows_Kinect_InfraredFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002CA8 RID: 11432 RVA: 0x000DDE9A File Offset: 0x000DC29A
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000DDEA4 File Offset: 0x000DC2A4
		~InfraredFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002CAA RID: 11434
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002CAB RID: 11435
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002CAC RID: 11436 RVA: 0x000DDED4 File Offset: 0x000DC2D4
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrameArrivedEventArgs>(this._pNative);
			InfraredFrameArrivedEventArgs.Windows_Kinect_InfraredFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002CAD RID: 11437
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002CAE RID: 11438 RVA: 0x000DDF14 File Offset: 0x000DC314
		public InfraredFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameArrivedEventArgs");
				}
				IntPtr intPtr = InfraredFrameArrivedEventArgs.Windows_Kinect_InfraredFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameReference>(intPtr, (IntPtr n) => new InfraredFrameReference(n));
			}
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000DDF87 File Offset: 0x000DC387
		private void __EventCleanup()
		{
		}

		// Token: 0x040017D3 RID: 6099
		internal IntPtr _pNative;
	}
}
