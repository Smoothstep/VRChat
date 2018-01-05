using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000510 RID: 1296
	public sealed class MultiSourceFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002DA2 RID: 11682 RVA: 0x000E0B01 File Offset: 0x000DEF01
		internal MultiSourceFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			MultiSourceFrameArrivedEventArgs.Windows_Kinect_MultiSourceFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002DA3 RID: 11683 RVA: 0x000E0B1B File Offset: 0x000DEF1B
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000E0B24 File Offset: 0x000DEF24
		~MultiSourceFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002DA5 RID: 11685
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002DA6 RID: 11686
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000E0B54 File Offset: 0x000DEF54
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<MultiSourceFrameArrivedEventArgs>(this._pNative);
			MultiSourceFrameArrivedEventArgs.Windows_Kinect_MultiSourceFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002DA8 RID: 11688
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002DA9 RID: 11689 RVA: 0x000E0B94 File Offset: 0x000DEF94
		public MultiSourceFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameArrivedEventArgs");
				}
				IntPtr intPtr = MultiSourceFrameArrivedEventArgs.Windows_Kinect_MultiSourceFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<MultiSourceFrameReference>(intPtr, (IntPtr n) => new MultiSourceFrameReference(n));
			}
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000E0C07 File Offset: 0x000DF007
		private void __EventCleanup()
		{
		}

		// Token: 0x0400183B RID: 6203
		internal IntPtr _pNative;
	}
}
