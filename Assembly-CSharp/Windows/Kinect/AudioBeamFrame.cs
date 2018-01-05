using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200049E RID: 1182
	public sealed class AudioBeamFrame : IDisposable, INativeWrapper
	{
		// Token: 0x06002893 RID: 10387 RVA: 0x000D2058 File Offset: 0x000D0458
		internal AudioBeamFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrame.Windows_Kinect_AudioBeamFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000D2074 File Offset: 0x000D0474
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			if (this._subFrames != null)
			{
				foreach (AudioBeamSubFrame audioBeamSubFrame in this._subFrames)
				{
					audioBeamSubFrame.Dispose();
				}
				this._subFrames = null;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrame>(this._pNative);
			AudioBeamFrame.Windows_Kinect_AudioBeamFrame_ReleaseObject(ref this._pNative);
			if (disposing)
			{
				AudioBeamFrame.Windows_Kinect_AudioBeamFrame_Dispose(this._pNative);
			}
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002895 RID: 10389
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_AudioBeamFrame_Dispose(IntPtr pNative);

		// Token: 0x06002896 RID: 10390 RVA: 0x000D2106 File Offset: 0x000D0506
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002897 RID: 10391
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrame_get_SubFrames(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x06002898 RID: 10392
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern int Windows_Kinect_AudioBeamFrame_get_SubFrames_Length(IntPtr pNative);

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x000D212C File Offset: 0x000D052C
		public IList<AudioBeamSubFrame> SubFrames
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				if (this._subFrames == null)
				{
					int num = AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_SubFrames_Length(this._pNative);
					IntPtr[] array = new IntPtr[num];
					this._subFrames = new AudioBeamSubFrame[num];
					num = AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_SubFrames(this._pNative, array, num);
					ExceptionHelper.CheckLastError();
					for (int i = 0; i < num; i++)
					{
						if (!(array[i] == IntPtr.Zero))
						{
							AudioBeamSubFrame audioBeamSubFrame = NativeObjectCache.GetObject<AudioBeamSubFrame>(array[i]);
							if (audioBeamSubFrame == null)
							{
								audioBeamSubFrame = new AudioBeamSubFrame(array[i]);
								NativeObjectCache.AddObject<AudioBeamSubFrame>(array[i], audioBeamSubFrame);
							}
							this._subFrames[i] = audioBeamSubFrame;
						}
					}
				}
				return this._subFrames;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x000D21EF File Offset: 0x000D05EF
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000D21F8 File Offset: 0x000D05F8
		~AudioBeamFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x0600289C RID: 10396
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600289D RID: 10397
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600289E RID: 10398
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamFrame_get_AudioBeam(IntPtr pNative);

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x0600289F RID: 10399 RVA: 0x000D2228 File Offset: 0x000D0628
		public AudioBeam AudioBeam
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				IntPtr intPtr = AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_AudioBeam(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioBeam>(intPtr, (IntPtr n) => new AudioBeam(n));
			}
		}

		// Token: 0x060028A0 RID: 10400
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamFrame_get_AudioSource(IntPtr pNative);

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x000D229C File Offset: 0x000D069C
		public AudioSource AudioSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				IntPtr intPtr = AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_AudioSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioSource>(intPtr, (IntPtr n) => new AudioSource(n));
			}
		}

		// Token: 0x060028A2 RID: 10402
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamFrame_get_Duration(IntPtr pNative);

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x000D230F File Offset: 0x000D070F
		public TimeSpan Duration
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_Duration(this._pNative));
			}
		}

		// Token: 0x060028A4 RID: 10404
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamFrame_get_RelativeTimeStart(IntPtr pNative);

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060028A5 RID: 10405 RVA: 0x000D2342 File Offset: 0x000D0742
		public TimeSpan RelativeTimeStart
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_RelativeTimeStart(this._pNative));
			}
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000D2375 File Offset: 0x000D0775
		private void __EventCleanup()
		{
		}

		// Token: 0x04001696 RID: 5782
		private AudioBeamSubFrame[] _subFrames;

		// Token: 0x04001697 RID: 5783
		internal IntPtr _pNative;
	}
}
