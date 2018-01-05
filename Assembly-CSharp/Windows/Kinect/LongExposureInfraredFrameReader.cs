using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000508 RID: 1288
	public sealed class LongExposureInfraredFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x06002D36 RID: 11574 RVA: 0x000DF579 File Offset: 0x000DD979
		internal LongExposureInfraredFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x000DF593 File Offset: 0x000DD993
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000DF59C File Offset: 0x000DD99C
		~LongExposureInfraredFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D39 RID: 11577
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002D3A RID: 11578
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002D3B RID: 11579 RVA: 0x000DF5CC File Offset: 0x000DD9CC
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrameReader>(this._pNative);
			if (disposing)
			{
				LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_Dispose(this._pNative);
			}
			LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002D3C RID: 11580
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_LongExposureInfraredFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06002D3D RID: 11581
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002D3E RID: 11582 RVA: 0x000DF627 File Offset: 0x000DDA27
		// (set) Token: 0x06002D3F RID: 11583 RVA: 0x000DF654 File Offset: 0x000DDA54
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameReader");
				}
				return LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameReader");
				}
				LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002D40 RID: 11584
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameReader_get_LongExposureInfraredFrameSource(IntPtr pNative);

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x000DF688 File Offset: 0x000DDA88
		public LongExposureInfraredFrameSource LongExposureInfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameReader");
				}
				IntPtr intPtr = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_get_LongExposureInfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameSource>(intPtr, (IntPtr n) => new LongExposureInfraredFrameSource(n));
			}
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000DF6FC File Offset: 0x000DDAFC
		[AOT.MonoPInvokeCallback(typeof(LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> list = null;
			LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				LongExposureInfraredFrameReader objThis = NativeObjectCache.GetObject<LongExposureInfraredFrameReader>(pNative);
				LongExposureInfraredFrameArrivedEventArgs args = new LongExposureInfraredFrameArrivedEventArgs(result);
				using (List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<LongExposureInfraredFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06002D43 RID: 11587
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_add_FrameArrived(IntPtr pNative, LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06002D44 RID: 11588 RVA: 0x000DF7C8 File Offset: 0x000DDBC8
		// (remove) Token: 0x06002D45 RID: 11589 RVA: 0x000DF858 File Offset: 0x000DDC58
		public event EventHandler<LongExposureInfraredFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate = new LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handler);
						LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate);
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_FrameArrived(this._pNative, windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (LongExposureInfraredFrameReader.f__mg0 == null)
						{
							LongExposureInfraredFrameReader.f__mg0 = new LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handler);
						}
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_FrameArrived(pNative, LongExposureInfraredFrameReader.f__mg0, true);
						LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000DF908 File Offset: 0x000DDD08
		[AOT.MonoPInvokeCallback(typeof(LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				LongExposureInfraredFrameReader objThis = NativeObjectCache.GetObject<LongExposureInfraredFrameReader>(pNative);
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

		// Token: 0x06002D47 RID: 11591
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_add_PropertyChanged(IntPtr pNative, LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06002D48 RID: 11592 RVA: 0x000DF9D4 File Offset: 0x000DDDD4
		// (remove) Token: 0x06002D49 RID: 11593 RVA: 0x000DFA64 File Offset: 0x000DDE64
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (LongExposureInfraredFrameReader.f__mg1 == null)
						{
							LongExposureInfraredFrameReader.f__mg1 = new LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_PropertyChanged(pNative, LongExposureInfraredFrameReader.f__mg1, true);
						LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002D4A RID: 11594
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x06002D4B RID: 11595 RVA: 0x000DFB14 File Offset: 0x000DDF14
		public LongExposureInfraredFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrameReader");
			}
			IntPtr intPtr = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrame>(intPtr, (IntPtr n) => new LongExposureInfraredFrame(n));
		}

		// Token: 0x06002D4C RID: 11596
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06002D4D RID: 11597 RVA: 0x000DFB87 File Offset: 0x000DDF87
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000DFBAC File Offset: 0x000DDFAC
		private void __EventCleanup()
		{
			LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (LongExposureInfraredFrameReader.f__mg2 == null)
						{
							LongExposureInfraredFrameReader.f__mg2 = new LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handler);
						}
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_FrameArrived(pNative, LongExposureInfraredFrameReader.f__mg2, true);
					}
					LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (LongExposureInfraredFrameReader.f__mg3 == null)
						{
							LongExposureInfraredFrameReader.f__mg3 = new LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_PropertyChanged(pNative2, LongExposureInfraredFrameReader.f__mg3, true);
					}
					LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400181B RID: 6171
		internal IntPtr _pNative;

		// Token: 0x0400181C RID: 6172
		private static GCHandle _Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x0400181D RID: 6173
		private static CollectionMap<IntPtr, List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>>> Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>>>();

		// Token: 0x0400181E RID: 6174
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400181F RID: 6175
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x04001821 RID: 6177
		[CompilerGenerated]
		private static LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate f__mg0;

		// Token: 0x04001822 RID: 6178
		[CompilerGenerated]
		private static LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x04001824 RID: 6180
		[CompilerGenerated]
		private static LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate f__mg2;

		// Token: 0x04001825 RID: 6181
		[CompilerGenerated]
		private static LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x02000509 RID: 1289
		// (Invoke) Token: 0x06002D53 RID: 11603
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x0200050A RID: 1290
		// (Invoke) Token: 0x06002D57 RID: 11607
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
