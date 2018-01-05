using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004D0 RID: 1232
	public sealed class BodyIndexFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x06002B3A RID: 11066 RVA: 0x000D9DE9 File Offset: 0x000D81E9
		internal BodyIndexFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002B3B RID: 11067 RVA: 0x000D9E03 File Offset: 0x000D8203
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000D9E0C File Offset: 0x000D820C
		~BodyIndexFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06002B3D RID: 11069
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002B3E RID: 11070
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002B3F RID: 11071 RVA: 0x000D9E3C File Offset: 0x000D823C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrameReader>(this._pNative);
			if (disposing)
			{
				BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_Dispose(this._pNative);
			}
			BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002B40 RID: 11072
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameReader_get_BodyIndexFrameSource(IntPtr pNative);

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06002B41 RID: 11073 RVA: 0x000D9E98 File Offset: 0x000D8298
		public BodyIndexFrameSource BodyIndexFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameReader");
				}
				IntPtr intPtr = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_get_BodyIndexFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameSource>(intPtr, (IntPtr n) => new BodyIndexFrameSource(n));
			}
		}

		// Token: 0x06002B42 RID: 11074
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_BodyIndexFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06002B43 RID: 11075
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06002B44 RID: 11076 RVA: 0x000D9F0B File Offset: 0x000D830B
		// (set) Token: 0x06002B45 RID: 11077 RVA: 0x000D9F38 File Offset: 0x000D8338
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameReader");
				}
				return BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameReader");
				}
				BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000D9F6C File Offset: 0x000D836C
		[AOT.MonoPInvokeCallback(typeof(BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<BodyIndexFrameArrivedEventArgs>> list = null;
			BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				BodyIndexFrameReader objThis = NativeObjectCache.GetObject<BodyIndexFrameReader>(pNative);
				BodyIndexFrameArrivedEventArgs args = new BodyIndexFrameArrivedEventArgs(result);
				using (List<EventHandler<BodyIndexFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<BodyIndexFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06002B47 RID: 11079
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_add_FrameArrived(IntPtr pNative, BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06002B48 RID: 11080 RVA: 0x000DA038 File Offset: 0x000D8438
		// (remove) Token: 0x06002B49 RID: 11081 RVA: 0x000DA0C8 File Offset: 0x000D84C8
		public event EventHandler<BodyIndexFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<BodyIndexFrameArrivedEventArgs>> list = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate = new BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate(BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handler);
						BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate);
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_FrameArrived(this._pNative, windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<BodyIndexFrameArrivedEventArgs>> list = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (BodyIndexFrameReader.f__mg0 == null)
						{
							BodyIndexFrameReader.f__mg0 = new BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate(BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handler);
						}
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_FrameArrived(pNative, BodyIndexFrameReader.f__mg0, true);
						BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x000DA178 File Offset: 0x000D8578
		[AOT.MonoPInvokeCallback(typeof(BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				BodyIndexFrameReader objThis = NativeObjectCache.GetObject<BodyIndexFrameReader>(pNative);
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

		// Token: 0x06002B4B RID: 11083
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_add_PropertyChanged(IntPtr pNative, BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06002B4C RID: 11084 RVA: 0x000DA244 File Offset: 0x000D8644
		// (remove) Token: 0x06002B4D RID: 11085 RVA: 0x000DA2D4 File Offset: 0x000D86D4
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (BodyIndexFrameReader.f__mg1 == null)
						{
							BodyIndexFrameReader.f__mg1 = new BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_PropertyChanged(pNative, BodyIndexFrameReader.f__mg1, true);
						BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002B4E RID: 11086
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x06002B4F RID: 11087 RVA: 0x000DA384 File Offset: 0x000D8784
		public BodyIndexFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrameReader");
			}
			IntPtr intPtr = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyIndexFrame>(intPtr, (IntPtr n) => new BodyIndexFrame(n));
		}

		// Token: 0x06002B50 RID: 11088
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06002B51 RID: 11089 RVA: 0x000DA3F7 File Offset: 0x000D87F7
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x000DA41C File Offset: 0x000D881C
		private void __EventCleanup()
		{
			BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<BodyIndexFrameArrivedEventArgs>> list = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (BodyIndexFrameReader.f__mg2 == null)
						{
							BodyIndexFrameReader.f__mg2 = new BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate(BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handler);
						}
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_FrameArrived(pNative, BodyIndexFrameReader.f__mg2, true);
					}
					BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (BodyIndexFrameReader.f__mg3 == null)
						{
							BodyIndexFrameReader.f__mg3 = new BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_PropertyChanged(pNative2, BodyIndexFrameReader.f__mg3, true);
					}
					BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04001750 RID: 5968
		internal IntPtr _pNative;

		// Token: 0x04001751 RID: 5969
		private static GCHandle _Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x04001752 RID: 5970
		private static CollectionMap<IntPtr, List<EventHandler<BodyIndexFrameArrivedEventArgs>>> Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<BodyIndexFrameArrivedEventArgs>>>();

		// Token: 0x04001753 RID: 5971
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04001754 RID: 5972
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x04001756 RID: 5974
		[CompilerGenerated]
		private static BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate f__mg0;

		// Token: 0x04001757 RID: 5975
		[CompilerGenerated]
		private static BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x04001759 RID: 5977
		[CompilerGenerated]
		private static BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate f__mg2;

		// Token: 0x0400175A RID: 5978
		[CompilerGenerated]
		private static BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004D1 RID: 1233
		// (Invoke) Token: 0x06002B57 RID: 11095
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004D2 RID: 1234
		// (Invoke) Token: 0x06002B5B RID: 11099
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
