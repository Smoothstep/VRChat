using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004E4 RID: 1252
	public sealed class CoordinateMappingChangedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002C14 RID: 11284 RVA: 0x000DC6BA File Offset: 0x000DAABA
		internal CoordinateMappingChangedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			CoordinateMappingChangedEventArgs.Windows_Kinect_CoordinateMappingChangedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002C15 RID: 11285 RVA: 0x000DC6D4 File Offset: 0x000DAAD4
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000DC6DC File Offset: 0x000DAADC
		~CoordinateMappingChangedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002C17 RID: 11287
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMappingChangedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002C18 RID: 11288
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMappingChangedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002C19 RID: 11289 RVA: 0x000DC70C File Offset: 0x000DAB0C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<CoordinateMappingChangedEventArgs>(this._pNative);
			CoordinateMappingChangedEventArgs.Windows_Kinect_CoordinateMappingChangedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000DC74B File Offset: 0x000DAB4B
		private void __EventCleanup()
		{
		}

		// Token: 0x04001792 RID: 6034
		internal IntPtr _pNative;
	}
}
