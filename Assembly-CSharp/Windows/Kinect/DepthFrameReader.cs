using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004E6 RID: 1254
	public sealed class DepthFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x06002C25 RID: 11301 RVA: 0x000DC85D File Offset: 0x000DAC5D
		internal DepthFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrameReader.Windows_Kinect_DepthFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06002C26 RID: 11302 RVA: 0x000DC877 File Offset: 0x000DAC77
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000DC880 File Offset: 0x000DAC80
		~DepthFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06002C28 RID: 11304
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002C29 RID: 11305
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002C2A RID: 11306 RVA: 0x000DC8B0 File Offset: 0x000DACB0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrameReader>(this._pNative);
			if (disposing)
			{
				DepthFrameReader.Windows_Kinect_DepthFrameReader_Dispose(this._pNative);
			}
			DepthFrameReader.Windows_Kinect_DepthFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002C2B RID: 11307
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameReader_get_DepthFrameSource(IntPtr pNative);

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002C2C RID: 11308 RVA: 0x000DC90C File Offset: 0x000DAD0C
		public DepthFrameSource DepthFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameReader");
				}
				IntPtr intPtr = DepthFrameReader.Windows_Kinect_DepthFrameReader_get_DepthFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameSource>(intPtr, (IntPtr n) => new DepthFrameSource(n));
			}
		}

		// Token: 0x06002C2D RID: 11309
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_DepthFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06002C2E RID: 11310
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x000DC97F File Offset: 0x000DAD7F
		// (set) Token: 0x06002C30 RID: 11312 RVA: 0x000DC9AC File Offset: 0x000DADAC
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameReader");
				}
				return DepthFrameReader.Windows_Kinect_DepthFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameReader");
				}
				DepthFrameReader.Windows_Kinect_DepthFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000DC9E0 File Offset: 0x000DADE0
		[AOT.MonoPInvokeCallback(typeof(DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<DepthFrameArrivedEventArgs>> list = null;
			DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				DepthFrameReader objThis = NativeObjectCache.GetObject<DepthFrameReader>(pNative);
				DepthFrameArrivedEventArgs args = new DepthFrameArrivedEventArgs(result);
				using (List<EventHandler<DepthFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<DepthFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06002C32 RID: 11314
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_add_FrameArrived(IntPtr pNative, DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06002C33 RID: 11315 RVA: 0x000DCAAC File Offset: 0x000DAEAC
		// (remove) Token: 0x06002C34 RID: 11316 RVA: 0x000DCB3C File Offset: 0x000DAF3C
		public event EventHandler<DepthFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<DepthFrameArrivedEventArgs>> list = DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate windows_Kinect_DepthFrameArrivedEventArgs_Delegate = new DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate(DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handler);
						DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_DepthFrameArrivedEventArgs_Delegate);
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_FrameArrived(this._pNative, windows_Kinect_DepthFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<DepthFrameArrivedEventArgs>> list = DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (DepthFrameReader.f__mg0 == null)
						{
							DepthFrameReader.f__mg0 = new DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate(DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handler);
						}
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_FrameArrived(pNative, DepthFrameReader.f__mg0, true);
						DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x000DCBEC File Offset: 0x000DAFEC
		[AOT.MonoPInvokeCallback(typeof(DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				DepthFrameReader objThis = NativeObjectCache.GetObject<DepthFrameReader>(pNative);
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

		// Token: 0x06002C36 RID: 11318
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_add_PropertyChanged(IntPtr pNative, DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06002C37 RID: 11319 RVA: 0x000DCCB8 File Offset: 0x000DB0B8
		// (remove) Token: 0x06002C38 RID: 11320 RVA: 0x000DCD48 File Offset: 0x000DB148
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (DepthFrameReader.f__mg1 == null)
						{
							DepthFrameReader.f__mg1 = new DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_PropertyChanged(pNative, DepthFrameReader.f__mg1, true);
						DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002C39 RID: 11321
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x06002C3A RID: 11322 RVA: 0x000DCDF8 File Offset: 0x000DB1F8
		public DepthFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrameReader");
			}
			IntPtr intPtr = DepthFrameReader.Windows_Kinect_DepthFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<DepthFrame>(intPtr, (IntPtr n) => new DepthFrame(n));
		}

		// Token: 0x06002C3B RID: 11323
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06002C3C RID: 11324 RVA: 0x000DCE6B File Offset: 0x000DB26B
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000DCE90 File Offset: 0x000DB290
		private void __EventCleanup()
		{
			DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<DepthFrameArrivedEventArgs>> list = DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (DepthFrameReader.f__mg2 == null)
						{
							DepthFrameReader.f__mg2 = new DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate(DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handler);
						}
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_FrameArrived(pNative, DepthFrameReader.f__mg2, true);
					}
					DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (DepthFrameReader.f__mg3 == null)
						{
							DepthFrameReader.f__mg3 = new DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_PropertyChanged(pNative2, DepthFrameReader.f__mg3, true);
					}
					DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04001795 RID: 6037
		internal IntPtr _pNative;

		// Token: 0x04001796 RID: 6038
		private static GCHandle _Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x04001797 RID: 6039
		private static CollectionMap<IntPtr, List<EventHandler<DepthFrameArrivedEventArgs>>> Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<DepthFrameArrivedEventArgs>>>();

		// Token: 0x04001798 RID: 6040
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04001799 RID: 6041
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0400179B RID: 6043
		[CompilerGenerated]
		private static DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate f__mg0;

		// Token: 0x0400179C RID: 6044
		[CompilerGenerated]
		private static DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x0400179E RID: 6046
		[CompilerGenerated]
		private static DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate f__mg2;

		// Token: 0x0400179F RID: 6047
		[CompilerGenerated]
		private static DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004E7 RID: 1255
		// (Invoke) Token: 0x06002C42 RID: 11330
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_DepthFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004E8 RID: 1256
		// (Invoke) Token: 0x06002C46 RID: 11334
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
