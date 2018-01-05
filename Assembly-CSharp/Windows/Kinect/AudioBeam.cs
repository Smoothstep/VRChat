using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004BA RID: 1210
	public sealed class AudioBeam : INativeWrapper
	{
		// Token: 0x06002A37 RID: 10807 RVA: 0x000D6E26 File Offset: 0x000D5226
		internal AudioBeam(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeam.Windows_Kinect_AudioBeam_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002A38 RID: 10808 RVA: 0x000D6E40 File Offset: 0x000D5240
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x000D6E48 File Offset: 0x000D5248
		~AudioBeam()
		{
			this.Dispose(false);
		}

		// Token: 0x06002A3A RID: 10810
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002A3B RID: 10811
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002A3C RID: 10812 RVA: 0x000D6E78 File Offset: 0x000D5278
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeam>(this._pNative);
			AudioBeam.Windows_Kinect_AudioBeam_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002A3D RID: 10813
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern AudioBeamMode Windows_Kinect_AudioBeam_get_AudioBeamMode(IntPtr pNative);

		// Token: 0x06002A3E RID: 10814
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_put_AudioBeamMode(IntPtr pNative, AudioBeamMode audioBeamMode);

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002A3F RID: 10815 RVA: 0x000D6EB7 File Offset: 0x000D52B7
		// (set) Token: 0x06002A40 RID: 10816 RVA: 0x000D6EE4 File Offset: 0x000D52E4
		public AudioBeamMode AudioBeamMode
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				return AudioBeam.Windows_Kinect_AudioBeam_get_AudioBeamMode(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				AudioBeam.Windows_Kinect_AudioBeam_put_AudioBeamMode(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002A41 RID: 10817
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeam_get_AudioSource(IntPtr pNative);

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x000D6F18 File Offset: 0x000D5318
		public AudioSource AudioSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				IntPtr intPtr = AudioBeam.Windows_Kinect_AudioBeam_get_AudioSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioSource>(intPtr, (IntPtr n) => new AudioSource(n));
			}
		}

		// Token: 0x06002A43 RID: 10819
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_AudioBeam_get_BeamAngle(IntPtr pNative);

		// Token: 0x06002A44 RID: 10820
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_put_BeamAngle(IntPtr pNative, float beamAngle);

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002A45 RID: 10821 RVA: 0x000D6F8B File Offset: 0x000D538B
		// (set) Token: 0x06002A46 RID: 10822 RVA: 0x000D6FB8 File Offset: 0x000D53B8
		public float BeamAngle
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				return AudioBeam.Windows_Kinect_AudioBeam_get_BeamAngle(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				AudioBeam.Windows_Kinect_AudioBeam_put_BeamAngle(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002A47 RID: 10823
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_AudioBeam_get_BeamAngleConfidence(IntPtr pNative);

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000D6FEB File Offset: 0x000D53EB
		public float BeamAngleConfidence
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				return AudioBeam.Windows_Kinect_AudioBeam_get_BeamAngleConfidence(this._pNative);
			}
		}

		// Token: 0x06002A49 RID: 10825
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeam_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x000D7018 File Offset: 0x000D5418
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeam.Windows_Kinect_AudioBeam_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000D704C File Offset: 0x000D544C
		[AOT.MonoPInvokeCallback(typeof(AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				AudioBeam objThis = NativeObjectCache.GetObject<AudioBeam>(pNative);
				PropertyChangedEventArgs args = new PropertyChangedEventArgs(result);
				using (List<EventHandler<PropertyChangedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<PropertyChangedEventArgs> func = enumerator.Current;
						EventPump.Instance.Enqueue(delegate
						{
							try
							{
								func(objThis, args);
							}
							catch
							{
							}
						});
					}
				}
			}
		}

		// Token: 0x06002A4C RID: 10828
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_add_PropertyChanged(IntPtr pNative, AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06002A4D RID: 10829 RVA: 0x000D7118 File Offset: 0x000D5518
		// (remove) Token: 0x06002A4E RID: 10830 RVA: 0x000D71A8 File Offset: 0x000D55A8
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						AudioBeam.Windows_Kinect_AudioBeam_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (AudioBeam.f__mg0 == null)
						{
							AudioBeam.f__mg0 = new AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						AudioBeam.Windows_Kinect_AudioBeam_add_PropertyChanged(pNative, AudioBeam.f__mg0, true);
						AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000D7258 File Offset: 0x000D5658
		private void __EventCleanup()
		{
			AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list = AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (AudioBeam.f__mg1 == null)
						{
							AudioBeam.f__mg1 = new AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						AudioBeam.Windows_Kinect_AudioBeam_add_PropertyChanged(pNative, AudioBeam.f__mg1, true);
					}
					AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400170E RID: 5902
		internal IntPtr _pNative;

		// Token: 0x0400170F RID: 5903
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04001710 RID: 5904
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x04001712 RID: 5906
		[CompilerGenerated]
		private static AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate f__mg0;

		// Token: 0x04001713 RID: 5907
		[CompilerGenerated]
		private static AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x020004BB RID: 1211
		// (Invoke) Token: 0x06002A53 RID: 10835
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
