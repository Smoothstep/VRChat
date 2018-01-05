using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004BD RID: 1213
	public sealed class AudioBeamFrameList : IDisposable, INativeWrapper
	{
		// Token: 0x06002A60 RID: 10848 RVA: 0x000D7489 File Offset: 0x000D5889
		internal AudioBeamFrameList(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrameList.Windows_Kinect_AudioBeamFrameList_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002A61 RID: 10849 RVA: 0x000D74A3 File Offset: 0x000D58A3
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000D74AC File Offset: 0x000D58AC
		~AudioBeamFrameList()
		{
			this.Dispose(false);
		}

		// Token: 0x06002A63 RID: 10851
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameList_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002A64 RID: 10852
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameList_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002A65 RID: 10853 RVA: 0x000D74DC File Offset: 0x000D58DC
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrameList>(this._pNative);
			if (disposing)
			{
				AudioBeamFrameList.Windows_Kinect_AudioBeamFrameList_Dispose(this._pNative);
			}
			AudioBeamFrameList.Windows_Kinect_AudioBeamFrameList_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002A66 RID: 10854
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameList_Dispose(IntPtr pNative);

		// Token: 0x06002A67 RID: 10855 RVA: 0x000D7537 File Offset: 0x000D5937
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x000D755C File Offset: 0x000D595C
		private void __EventCleanup()
		{
		}

		// Token: 0x04001716 RID: 5910
		internal IntPtr _pNative;
	}
}
