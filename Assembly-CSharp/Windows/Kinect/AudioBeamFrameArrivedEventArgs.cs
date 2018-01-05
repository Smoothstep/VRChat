using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004BC RID: 1212
	public sealed class AudioBeamFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x06002A56 RID: 10838 RVA: 0x000D7378 File Offset: 0x000D5778
		internal AudioBeamFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrameArrivedEventArgs.Windows_Kinect_AudioBeamFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002A57 RID: 10839 RVA: 0x000D7392 File Offset: 0x000D5792
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000D739C File Offset: 0x000D579C
		~AudioBeamFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06002A59 RID: 10841
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002A5A RID: 10842
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002A5B RID: 10843 RVA: 0x000D73CC File Offset: 0x000D57CC
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrameArrivedEventArgs>(this._pNative);
			AudioBeamFrameArrivedEventArgs.Windows_Kinect_AudioBeamFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002A5C RID: 10844
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06002A5D RID: 10845 RVA: 0x000D740C File Offset: 0x000D580C
		public AudioBeamFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameArrivedEventArgs");
				}
				IntPtr intPtr = AudioBeamFrameArrivedEventArgs.Windows_Kinect_AudioBeamFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioBeamFrameReference>(intPtr, (IntPtr n) => new AudioBeamFrameReference(n));
			}
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000D747F File Offset: 0x000D587F
		private void __EventCleanup()
		{
		}

		// Token: 0x04001714 RID: 5908
		internal IntPtr _pNative;
	}
}
