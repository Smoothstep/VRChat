using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004C4 RID: 1220
	public sealed class AudioSource : INativeWrapper
	{
		// Token: 0x06002AA4 RID: 10916 RVA: 0x000D803E File Offset: 0x000D643E
		internal AudioSource(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioSource.Windows_Kinect_AudioSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002AA5 RID: 10917 RVA: 0x000D8058 File Offset: 0x000D6458
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x000D8060 File Offset: 0x000D6460
		~AudioSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06002AA7 RID: 10919
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002AA8 RID: 10920
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002AA9 RID: 10921 RVA: 0x000D8090 File Offset: 0x000D6490
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioSource>(this._pNative);
			AudioSource.Windows_Kinect_AudioSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002AAA RID: 10922
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioSource_get_AudioBeams(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x06002AAB RID: 10923
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioSource_get_AudioBeams_Length(IntPtr pNative);

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002AAC RID: 10924 RVA: 0x000D80D0 File Offset: 0x000D64D0
		public IList<AudioBeam> AudioBeams
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				int num = AudioSource.Windows_Kinect_AudioSource_get_AudioBeams_Length(this._pNative);
				IntPtr[] array = new IntPtr[num];
				AudioBeam[] array2 = new AudioBeam[num];
				num = AudioSource.Windows_Kinect_AudioSource_get_AudioBeams(this._pNative, array, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					if (!(array[i] == IntPtr.Zero))
					{
						AudioBeam audioBeam = NativeObjectCache.CreateOrGetObject<AudioBeam>(array[i], (IntPtr n) => new AudioBeam(n));
						array2[i] = audioBeam;
					}
				}
				return array2;
			}
		}

		// Token: 0x06002AAD RID: 10925
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_AudioSource_get_IsActive(IntPtr pNative);

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002AAE RID: 10926 RVA: 0x000D8180 File Offset: 0x000D6580
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return AudioSource.Windows_Kinect_AudioSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x06002AAF RID: 10927
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002AB0 RID: 10928 RVA: 0x000D81B0 File Offset: 0x000D65B0
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				IntPtr intPtr = AudioSource.Windows_Kinect_AudioSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06002AB1 RID: 10929
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_AudioSource_get_MaxSubFrameCount(IntPtr pNative);

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002AB2 RID: 10930 RVA: 0x000D8223 File Offset: 0x000D6623
		public uint MaxSubFrameCount
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return AudioSource.Windows_Kinect_AudioSource_get_MaxSubFrameCount(this._pNative);
			}
		}

		// Token: 0x06002AB3 RID: 10931
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioSource_get_SubFrameDuration(IntPtr pNative);

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002AB4 RID: 10932 RVA: 0x000D8250 File Offset: 0x000D6650
		public TimeSpan SubFrameDuration
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return TimeSpan.FromMilliseconds((double)AudioSource.Windows_Kinect_AudioSource_get_SubFrameDuration(this._pNative));
			}
		}

		// Token: 0x06002AB5 RID: 10933
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_AudioSource_get_SubFrameLengthInBytes(IntPtr pNative);

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x000D8283 File Offset: 0x000D6683
		public uint SubFrameLengthInBytes
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return AudioSource.Windows_Kinect_AudioSource_get_SubFrameLengthInBytes(this._pNative);
			}
		}

		// Token: 0x06002AB7 RID: 10935
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern KinectAudioCalibrationState Windows_Kinect_AudioSource_get_AudioCalibrationState(IntPtr pNative);

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002AB8 RID: 10936 RVA: 0x000D82B0 File Offset: 0x000D66B0
		public KinectAudioCalibrationState AudioCalibrationState
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return AudioSource.Windows_Kinect_AudioSource_get_AudioCalibrationState(this._pNative);
			}
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x000D82E0 File Offset: 0x000D66E0
		[AOT.MonoPInvokeCallback(typeof(AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				AudioSource objThis = NativeObjectCache.GetObject<AudioSource>(pNative);
				FrameCapturedEventArgs args = new FrameCapturedEventArgs(result);
				using (List<EventHandler<FrameCapturedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<FrameCapturedEventArgs> func = enumerator.Current;
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

		// Token: 0x06002ABA RID: 10938
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioSource_add_FrameCaptured(IntPtr pNative, AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06002ABB RID: 10939 RVA: 0x000D83AC File Offset: 0x000D67AC
		// (remove) Token: 0x06002ABC RID: 10940 RVA: 0x000D843C File Offset: 0x000D683C
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						AudioSource.Windows_Kinect_AudioSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (AudioSource.f__mg0 == null)
						{
							AudioSource.f__mg0 = new AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						AudioSource.Windows_Kinect_AudioSource_add_FrameCaptured(pNative, AudioSource.f__mg0, true);
						AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000D84EC File Offset: 0x000D68EC
		[AOT.MonoPInvokeCallback(typeof(AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				AudioSource objThis = NativeObjectCache.GetObject<AudioSource>(pNative);
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

		// Token: 0x06002ABE RID: 10942
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioSource_add_PropertyChanged(IntPtr pNative, AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06002ABF RID: 10943 RVA: 0x000D85B8 File Offset: 0x000D69B8
		// (remove) Token: 0x06002AC0 RID: 10944 RVA: 0x000D8648 File Offset: 0x000D6A48
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate(AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						AudioSource.Windows_Kinect_AudioSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (AudioSource.f__mg1 == null)
						{
							AudioSource.f__mg1 = new AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate(AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						AudioSource.Windows_Kinect_AudioSource_add_PropertyChanged(pNative, AudioSource.f__mg1, true);
						AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002AC1 RID: 10945
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioSource_OpenReader(IntPtr pNative);

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000D86F8 File Offset: 0x000D6AF8
		public AudioBeamFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioSource");
			}
			IntPtr intPtr = AudioSource.Windows_Kinect_AudioSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<AudioBeamFrameReader>(intPtr, (IntPtr n) => new AudioBeamFrameReader(n));
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000D876C File Offset: 0x000D6B6C
		private void __EventCleanup()
		{
			AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (AudioSource.f__mg2 == null)
						{
							AudioSource.f__mg2 = new AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						AudioSource.Windows_Kinect_AudioSource_add_FrameCaptured(pNative, AudioSource.f__mg2, true);
					}
					AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (AudioSource.f__mg3 == null)
						{
							AudioSource.f__mg3 = new AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate(AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						AudioSource.Windows_Kinect_AudioSource_add_PropertyChanged(pNative2, AudioSource.f__mg3, true);
					}
					AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04001728 RID: 5928
		internal IntPtr _pNative;

		// Token: 0x04001729 RID: 5929
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x0400172A RID: 5930
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x0400172B RID: 5931
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400172C RID: 5932
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0400172F RID: 5935
		[CompilerGenerated]
		private static AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg0;

		// Token: 0x04001730 RID: 5936
		[CompilerGenerated]
		private static AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x04001732 RID: 5938
		[CompilerGenerated]
		private static AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg2;

		// Token: 0x04001733 RID: 5939
		[CompilerGenerated]
		private static AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004C5 RID: 1221
		// (Invoke) Token: 0x06002AC9 RID: 10953
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004C6 RID: 1222
		// (Invoke) Token: 0x06002ACD RID: 10957
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
