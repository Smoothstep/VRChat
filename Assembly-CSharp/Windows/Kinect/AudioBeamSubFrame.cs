using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200049D RID: 1181
	public sealed class AudioBeamSubFrame : IDisposable, INativeWrapper
	{
		// Token: 0x06002873 RID: 10355 RVA: 0x000D1CA8 File Offset: 0x000D00A8
		internal AudioBeamSubFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x06002874 RID: 10356
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x06002875 RID: 10357 RVA: 0x000D1CC2 File Offset: 0x000D00C2
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamSubFrame");
			}
			AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002876 RID: 10358
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamSubFrame_LockAudioBuffer(IntPtr pNative);

		// Token: 0x06002877 RID: 10359 RVA: 0x000D1CF8 File Offset: 0x000D00F8
		public KinectBuffer LockAudioBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamSubFrame");
			}
			IntPtr intPtr = AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_LockAudioBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x000D1D6B File Offset: 0x000D016B
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000D1D74 File Offset: 0x000D0174
		~AudioBeamSubFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x0600287A RID: 10362
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600287B RID: 10363
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600287C RID: 10364 RVA: 0x000D1DA4 File Offset: 0x000D01A4
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamSubFrame>(this._pNative);
			if (disposing)
			{
				AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_Dispose(this._pNative);
			}
			AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600287D RID: 10365
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern AudioBeamMode Windows_Kinect_AudioBeamSubFrame_get_AudioBeamMode(IntPtr pNative);

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600287E RID: 10366 RVA: 0x000D1DFF File Offset: 0x000D01FF
		public AudioBeamMode AudioBeamMode
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_AudioBeamMode(this._pNative);
			}
		}

		// Token: 0x0600287F RID: 10367
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamSubFrame_get_AudioBodyCorrelations(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x06002880 RID: 10368
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamSubFrame_get_AudioBodyCorrelations_Length(IntPtr pNative);

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06002881 RID: 10369 RVA: 0x000D1E2C File Offset: 0x000D022C
		public IList<AudioBodyCorrelation> AudioBodyCorrelations
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				int num = AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_AudioBodyCorrelations_Length(this._pNative);
				IntPtr[] array = new IntPtr[num];
				AudioBodyCorrelation[] array2 = new AudioBodyCorrelation[num];
				num = AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_AudioBodyCorrelations(this._pNative, array, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					if (!(array[i] == IntPtr.Zero))
					{
						AudioBodyCorrelation audioBodyCorrelation = NativeObjectCache.CreateOrGetObject<AudioBodyCorrelation>(array[i], (IntPtr n) => new AudioBodyCorrelation(n));
						array2[i] = audioBodyCorrelation;
					}
				}
				return array2;
			}
		}

		// Token: 0x06002882 RID: 10370
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_AudioBeamSubFrame_get_BeamAngle(IntPtr pNative);

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002883 RID: 10371 RVA: 0x000D1EDC File Offset: 0x000D02DC
		public float BeamAngle
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_BeamAngle(this._pNative);
			}
		}

		// Token: 0x06002884 RID: 10372
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_AudioBeamSubFrame_get_BeamAngleConfidence(IntPtr pNative);

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002885 RID: 10373 RVA: 0x000D1F09 File Offset: 0x000D0309
		public float BeamAngleConfidence
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_BeamAngleConfidence(this._pNative);
			}
		}

		// Token: 0x06002886 RID: 10374
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamSubFrame_get_Duration(IntPtr pNative);

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002887 RID: 10375 RVA: 0x000D1F36 File Offset: 0x000D0336
		public TimeSpan Duration
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_Duration(this._pNative));
			}
		}

		// Token: 0x06002888 RID: 10376
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_AudioBeamSubFrame_get_FrameLengthInBytes(IntPtr pNative);

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x000D1F69 File Offset: 0x000D0369
		public uint FrameLengthInBytes
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_FrameLengthInBytes(this._pNative);
			}
		}

		// Token: 0x0600288A RID: 10378
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamSubFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x000D1F96 File Offset: 0x000D0396
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x0600288C RID: 10380
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x0600288D RID: 10381 RVA: 0x000D1FCC File Offset: 0x000D03CC
		public void CopyFrameDataToArray(byte[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamSubFrame");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned));
			IntPtr frameData2 = smartGCHandle.AddrOfPinnedObject();
			AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600288E RID: 10382
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_Dispose(IntPtr pNative);

		// Token: 0x0600288F RID: 10383 RVA: 0x000D2021 File Offset: 0x000D0421
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000D2046 File Offset: 0x000D0446
		private void __EventCleanup()
		{
		}

		// Token: 0x04001693 RID: 5779
		internal IntPtr _pNative;
	}
}
