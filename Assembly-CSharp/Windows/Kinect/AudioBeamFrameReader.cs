using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004BE RID: 1214
	public sealed class AudioBeamFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x06002A69 RID: 10857 RVA: 0x000D755E File Offset: 0x000D595E
		internal AudioBeamFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x000D7578 File Offset: 0x000D5978
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000D7580 File Offset: 0x000D5980
		~AudioBeamFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06002A6C RID: 10860
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002A6D RID: 10861
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002A6E RID: 10862 RVA: 0x000D75B0 File Offset: 0x000D59B0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrameReader>(this._pNative);
			if (disposing)
			{
				AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_Dispose(this._pNative);
			}
			AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002A6F RID: 10863
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamFrameReader_get_AudioSource(IntPtr pNative);

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x000D760C File Offset: 0x000D5A0C
		public AudioSource AudioSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameReader");
				}
				IntPtr intPtr = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_get_AudioSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioSource>(intPtr, (IntPtr n) => new AudioSource(n));
			}
		}

		// Token: 0x06002A71 RID: 10865
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_AudioBeamFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06002A72 RID: 10866
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002A73 RID: 10867 RVA: 0x000D767F File Offset: 0x000D5A7F
		// (set) Token: 0x06002A74 RID: 10868 RVA: 0x000D76AC File Offset: 0x000D5AAC
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameReader");
				}
				return AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameReader");
				}
				AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000D76E0 File Offset: 0x000D5AE0
		[AOT.MonoPInvokeCallback(typeof(AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<AudioBeamFrameArrivedEventArgs>> list = null;
			AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				AudioBeamFrameReader objThis = NativeObjectCache.GetObject<AudioBeamFrameReader>(pNative);
				AudioBeamFrameArrivedEventArgs args = new AudioBeamFrameArrivedEventArgs(result);
				using (List<EventHandler<AudioBeamFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<AudioBeamFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06002A76 RID: 10870
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_add_FrameArrived(IntPtr pNative, AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06002A77 RID: 10871 RVA: 0x000D77AC File Offset: 0x000D5BAC
		// (remove) Token: 0x06002A78 RID: 10872 RVA: 0x000D783C File Offset: 0x000D5C3C
		public event EventHandler<AudioBeamFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<AudioBeamFrameArrivedEventArgs>> list = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate = new AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate(AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handler);
						AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate);
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_FrameArrived(this._pNative, windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<AudioBeamFrameArrivedEventArgs>> list = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (AudioBeamFrameReader.f__mg0 == null)
						{
							AudioBeamFrameReader.f__mg0 = new AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate(AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handler);
						}
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_FrameArrived(pNative, AudioBeamFrameReader.f__mg0, true);
						AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x000D78EC File Offset: 0x000D5CEC
		[AOT.MonoPInvokeCallback(typeof(AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				AudioBeamFrameReader objThis = NativeObjectCache.GetObject<AudioBeamFrameReader>(pNative);
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

		// Token: 0x06002A7A RID: 10874
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_add_PropertyChanged(IntPtr pNative, AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06002A7B RID: 10875 RVA: 0x000D79B8 File Offset: 0x000D5DB8
		// (remove) Token: 0x06002A7C RID: 10876 RVA: 0x000D7A48 File Offset: 0x000D5E48
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (AudioBeamFrameReader.f__mg1 == null)
						{
							AudioBeamFrameReader.f__mg1 = new AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_PropertyChanged(pNative, AudioBeamFrameReader.f__mg1, true);
						AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002A7D RID: 10877
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrameReader_AcquireLatestBeamFrames_Length(IntPtr pNative);

		// Token: 0x06002A7E RID: 10878
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrameReader_AcquireLatestBeamFrames(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x06002A7F RID: 10879 RVA: 0x000D7AF8 File Offset: 0x000D5EF8
		public IList<AudioBeamFrame> AcquireLatestBeamFrames()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamFrameReader");
			}
			int num = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_AcquireLatestBeamFrames_Length(this._pNative);
			IntPtr[] array = new IntPtr[num];
			AudioBeamFrame[] array2 = new AudioBeamFrame[num];
			num = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_AcquireLatestBeamFrames(this._pNative, array, num);
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

		// Token: 0x06002A80 RID: 10880
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06002A81 RID: 10881 RVA: 0x000D7BA8 File Offset: 0x000D5FA8
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000D7BD0 File Offset: 0x000D5FD0
		private void __EventCleanup()
		{
			AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<AudioBeamFrameArrivedEventArgs>> list = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (AudioBeamFrameReader.f__mg2 == null)
						{
							AudioBeamFrameReader.f__mg2 = new AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate(AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handler);
						}
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_FrameArrived(pNative, AudioBeamFrameReader.f__mg2, true);
					}
					AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (AudioBeamFrameReader.f__mg3 == null)
						{
							AudioBeamFrameReader.f__mg3 = new AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_PropertyChanged(pNative2, AudioBeamFrameReader.f__mg3, true);
					}
					AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04001717 RID: 5911
		internal IntPtr _pNative;

		// Token: 0x04001718 RID: 5912
		private static GCHandle _Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x04001719 RID: 5913
		private static CollectionMap<IntPtr, List<EventHandler<AudioBeamFrameArrivedEventArgs>>> Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<AudioBeamFrameArrivedEventArgs>>>();

		// Token: 0x0400171A RID: 5914
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400171B RID: 5915
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0400171D RID: 5917
		[CompilerGenerated]
		private static AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate f__mg0;

		// Token: 0x0400171E RID: 5918
		[CompilerGenerated]
		private static AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x04001720 RID: 5920
		[CompilerGenerated]
		private static AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate f__mg2;

		// Token: 0x04001721 RID: 5921
		[CompilerGenerated]
		private static AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004BF RID: 1215
		// (Invoke) Token: 0x06002A87 RID: 10887
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004C0 RID: 1216
		// (Invoke) Token: 0x06002A8B RID: 10891
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
