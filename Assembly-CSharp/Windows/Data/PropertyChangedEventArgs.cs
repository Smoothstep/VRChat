using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Data
{
	// Token: 0x020004B7 RID: 1207
	public sealed class PropertyChangedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002A2E RID: 10798 RVA: 0x000D6D43 File Offset: 0x000D5143
		internal PropertyChangedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			PropertyChangedEventArgs.Windows_Data_PropertyChangedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x000D6D5D File Offset: 0x000D515D
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x000D6D68 File Offset: 0x000D5168
		~PropertyChangedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002A31 RID: 10801
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Data_PropertyChangedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002A32 RID: 10802
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Data_PropertyChangedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002A33 RID: 10803 RVA: 0x000D6D98 File Offset: 0x000D5198
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<PropertyChangedEventArgs>(this._pNative);
			PropertyChangedEventArgs.Windows_Data_PropertyChangedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002A34 RID: 10804
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Data_PropertyChangedEventArgs_get_PropertyName(IntPtr pNative);

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002A35 RID: 10805 RVA: 0x000D6DD8 File Offset: 0x000D51D8
		public string PropertyName
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("PropertyChangedEventArgs");
				}
				IntPtr ptr = PropertyChangedEventArgs.Windows_Data_PropertyChangedEventArgs_get_PropertyName(this._pNative);
				ExceptionHelper.CheckLastError();
				string result = Marshal.PtrToStringUni(ptr);
				Marshal.FreeCoTaskMem(ptr);
				return result;
			}
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x000D6E24 File Offset: 0x000D5224
		private void __EventCleanup()
		{
		}

		// Token: 0x04001705 RID: 5893
		internal IntPtr _pNative;
	}
}
