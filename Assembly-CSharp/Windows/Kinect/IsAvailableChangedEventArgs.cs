using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004FE RID: 1278
	public sealed class IsAvailableChangedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002D04 RID: 11524 RVA: 0x000DF19C File Offset: 0x000DD59C
		internal IsAvailableChangedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			IsAvailableChangedEventArgs.Windows_Kinect_IsAvailableChangedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002D05 RID: 11525 RVA: 0x000DF1B6 File Offset: 0x000DD5B6
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000DF1C0 File Offset: 0x000DD5C0
		~IsAvailableChangedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D07 RID: 11527
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_IsAvailableChangedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002D08 RID: 11528
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_IsAvailableChangedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002D09 RID: 11529 RVA: 0x000DF1F0 File Offset: 0x000DD5F0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<IsAvailableChangedEventArgs>(this._pNative);
			IsAvailableChangedEventArgs.Windows_Kinect_IsAvailableChangedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002D0A RID: 11530
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_IsAvailableChangedEventArgs_get_IsAvailable(IntPtr pNative);

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x000DF22F File Offset: 0x000DD62F
		public bool IsAvailable
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("IsAvailableChangedEventArgs");
				}
				return IsAvailableChangedEventArgs.Windows_Kinect_IsAvailableChangedEventArgs_get_IsAvailable(this._pNative);
			}
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000DF25C File Offset: 0x000DD65C
		private void __EventCleanup()
		{
		}

		// Token: 0x040017EE RID: 6126
		internal IntPtr _pNative;
	}
}
