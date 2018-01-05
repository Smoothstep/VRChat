using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004F7 RID: 1271
	public sealed class InfraredFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x06002CB1 RID: 11441 RVA: 0x000DDF91 File Offset: 0x000DC391
		internal InfraredFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrameReader.Windows_Kinect_InfraredFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002CB2 RID: 11442 RVA: 0x000DDFAB File Offset: 0x000DC3AB
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000DDFB4 File Offset: 0x000DC3B4
		~InfraredFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06002CB4 RID: 11444
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002CB5 RID: 11445
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000DDFE4 File Offset: 0x000DC3E4
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrameReader>(this._pNative);
			if (disposing)
			{
				InfraredFrameReader.Windows_Kinect_InfraredFrameReader_Dispose(this._pNative);
			}
			InfraredFrameReader.Windows_Kinect_InfraredFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002CB7 RID: 11447
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameReader_get_InfraredFrameSource(IntPtr pNative);

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x000DE040 File Offset: 0x000DC440
		public InfraredFrameSource InfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameReader");
				}
				IntPtr intPtr = InfraredFrameReader.Windows_Kinect_InfraredFrameReader_get_InfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameSource>(intPtr, (IntPtr n) => new InfraredFrameSource(n));
			}
		}

		// Token: 0x06002CB9 RID: 11449
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_InfraredFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06002CBA RID: 11450
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002CBB RID: 11451 RVA: 0x000DE0B3 File Offset: 0x000DC4B3
		// (set) Token: 0x06002CBC RID: 11452 RVA: 0x000DE0E0 File Offset: 0x000DC4E0
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameReader");
				}
				return InfraredFrameReader.Windows_Kinect_InfraredFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameReader");
				}
				InfraredFrameReader.Windows_Kinect_InfraredFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x000DE114 File Offset: 0x000DC514
		[AOT.MonoPInvokeCallback(typeof(InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<InfraredFrameArrivedEventArgs>> list = null;
			InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				InfraredFrameReader objThis = NativeObjectCache.GetObject<InfraredFrameReader>(pNative);
				InfraredFrameArrivedEventArgs args = new InfraredFrameArrivedEventArgs(result);
				using (List<EventHandler<InfraredFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<InfraredFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06002CBE RID: 11454
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_add_FrameArrived(IntPtr pNative, InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06002CBF RID: 11455 RVA: 0x000DE1E0 File Offset: 0x000DC5E0
		// (remove) Token: 0x06002CC0 RID: 11456 RVA: 0x000DE270 File Offset: 0x000DC670
		public event EventHandler<InfraredFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<InfraredFrameArrivedEventArgs>> list = InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate windows_Kinect_InfraredFrameArrivedEventArgs_Delegate = new InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate(InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handler);
						InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_InfraredFrameArrivedEventArgs_Delegate);
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_FrameArrived(this._pNative, windows_Kinect_InfraredFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<InfraredFrameArrivedEventArgs>> list = InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (InfraredFrameReader.f__mg0 == null)
						{
							InfraredFrameReader.f__mg0 = new InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate(InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handler);
						}
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_FrameArrived(pNative, InfraredFrameReader.f__mg0, true);
						InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x000DE320 File Offset: 0x000DC720
		[AOT.MonoPInvokeCallback(typeof(InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				InfraredFrameReader objThis = NativeObjectCache.GetObject<InfraredFrameReader>(pNative);
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

		// Token: 0x06002CC2 RID: 11458
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_add_PropertyChanged(IntPtr pNative, InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06002CC3 RID: 11459 RVA: 0x000DE3EC File Offset: 0x000DC7EC
		// (remove) Token: 0x06002CC4 RID: 11460 RVA: 0x000DE47C File Offset: 0x000DC87C
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (InfraredFrameReader.f__mg1 == null)
						{
							InfraredFrameReader.f__mg1 = new InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_PropertyChanged(pNative, InfraredFrameReader.f__mg1, true);
						InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002CC5 RID: 11461
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x06002CC6 RID: 11462 RVA: 0x000DE52C File Offset: 0x000DC92C
		public InfraredFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrameReader");
			}
			IntPtr intPtr = InfraredFrameReader.Windows_Kinect_InfraredFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<InfraredFrame>(intPtr, (IntPtr n) => new InfraredFrame(n));
		}

		// Token: 0x06002CC7 RID: 11463
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06002CC8 RID: 11464 RVA: 0x000DE59F File Offset: 0x000DC99F
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x000DE5C4 File Offset: 0x000DC9C4
		private void __EventCleanup()
		{
			InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<InfraredFrameArrivedEventArgs>> list = InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (InfraredFrameReader.f__mg2 == null)
						{
							InfraredFrameReader.f__mg2 = new InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate(InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handler);
						}
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_FrameArrived(pNative, InfraredFrameReader.f__mg2, true);
					}
					InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (InfraredFrameReader.f__mg3 == null)
						{
							InfraredFrameReader.f__mg3 = new InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_PropertyChanged(pNative2, InfraredFrameReader.f__mg3, true);
					}
					InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x040017D5 RID: 6101
		internal IntPtr _pNative;

		// Token: 0x040017D6 RID: 6102
		private static GCHandle _Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x040017D7 RID: 6103
		private static CollectionMap<IntPtr, List<EventHandler<InfraredFrameArrivedEventArgs>>> Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<InfraredFrameArrivedEventArgs>>>();

		// Token: 0x040017D8 RID: 6104
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x040017D9 RID: 6105
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x040017DB RID: 6107
		[CompilerGenerated]
		private static InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate f__mg0;

		// Token: 0x040017DC RID: 6108
		[CompilerGenerated]
		private static InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x040017DE RID: 6110
		[CompilerGenerated]
		private static InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate f__mg2;

		// Token: 0x040017DF RID: 6111
		[CompilerGenerated]
		private static InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004F8 RID: 1272
		// (Invoke) Token: 0x06002CCE RID: 11470
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004F9 RID: 1273
		// (Invoke) Token: 0x06002CD2 RID: 11474
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
