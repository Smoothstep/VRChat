using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004D9 RID: 1241
	public sealed class ColorFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002BA7 RID: 11175 RVA: 0x000DB255 File Offset: 0x000D9655
		internal ColorFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrameArrivedEventArgs.Windows_Kinect_ColorFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002BA8 RID: 11176 RVA: 0x000DB26F File Offset: 0x000D966F
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000DB278 File Offset: 0x000D9678
		~ColorFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002BAA RID: 11178
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002BAB RID: 11179
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002BAC RID: 11180 RVA: 0x000DB2A8 File Offset: 0x000D96A8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrameArrivedEventArgs>(this._pNative);
			ColorFrameArrivedEventArgs.Windows_Kinect_ColorFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002BAD RID: 11181
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x000DB2E8 File Offset: 0x000D96E8
		public ColorFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameArrivedEventArgs");
				}
				IntPtr intPtr = ColorFrameArrivedEventArgs.Windows_Kinect_ColorFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameReference>(intPtr, (IntPtr n) => new ColorFrameReference(n));
			}
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x000DB35B File Offset: 0x000D975B
		private void __EventCleanup()
		{
		}

		// Token: 0x0400176D RID: 5997
		internal IntPtr _pNative;
	}
}
