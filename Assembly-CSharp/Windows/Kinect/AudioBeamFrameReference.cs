using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004C1 RID: 1217
	public sealed class AudioBeamFrameReference : INativeWrapper
	{
		// Token: 0x06002A8E RID: 10894 RVA: 0x000D7DFC File Offset: 0x000D61FC
		internal AudioBeamFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002A8F RID: 10895 RVA: 0x000D7E16 File Offset: 0x000D6216
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000D7E20 File Offset: 0x000D6220
		~AudioBeamFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06002A91 RID: 10897
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002A92 RID: 10898
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002A93 RID: 10899 RVA: 0x000D7E50 File Offset: 0x000D6250
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrameReference>(this._pNative);
			AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002A94 RID: 10900
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002A95 RID: 10901 RVA: 0x000D7E8F File Offset: 0x000D628F
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002A96 RID: 10902
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrameReference_AcquireBeamFrames_Length(IntPtr pNative);

		// Token: 0x06002A97 RID: 10903
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrameReference_AcquireBeamFrames(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x06002A98 RID: 10904 RVA: 0x000D7EC4 File Offset: 0x000D62C4
		public IList<AudioBeamFrame> AcquireBeamFrames()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamFrameReference");
			}
			int num = AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_AcquireBeamFrames_Length(this._pNative);
			IntPtr[] array = new IntPtr[num];
			AudioBeamFrame[] array2 = new AudioBeamFrame[num];
			num = AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_AcquireBeamFrames(this._pNative, array, num);
			ExceptionHelper.CheckLastError();
			for (int i = 0; i < num; i++)
			{
				if (!(array[i] == IntPtr.Zero))
				{
					AudioBeamFrame audioBeamFrame = NativeObjectCache.CreateOrGetObject<AudioBeamFrame>(array[i], (IntPtr n) => new AudioBeamFrame(n));
					array2[i] = audioBeamFrame;
				}
			}
			return array2;
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000D7F74 File Offset: 0x000D6374
		private void __EventCleanup()
		{
		}

		// Token: 0x04001722 RID: 5922
		internal IntPtr _pNative;
	}
}
