using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000511 RID: 1297
	public sealed class MultiSourceFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x06002DAC RID: 11692 RVA: 0x000E0C11 File Offset: 0x000DF011
		internal MultiSourceFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002DAD RID: 11693 RVA: 0x000E0C2B File Offset: 0x000DF02B
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000E0C34 File Offset: 0x000DF034
		~MultiSourceFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06002DAF RID: 11695
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002DB0 RID: 11696
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000E0C64 File Offset: 0x000DF064
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<MultiSourceFrameReader>(this._pNative);
			if (disposing)
			{
				MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_Dispose(this._pNative);
			}
			MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002DB2 RID: 11698
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern FrameSourceTypes Windows_Kinect_MultiSourceFrameReader_get_FrameSourceTypes(IntPtr pNative);

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002DB3 RID: 11699 RVA: 0x000E0CBF File Offset: 0x000DF0BF
		public FrameSourceTypes FrameSourceTypes
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameReader");
				}
				return MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_get_FrameSourceTypes(this._pNative);
			}
		}

		// Token: 0x06002DB4 RID: 11700
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_MultiSourceFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06002DB5 RID: 11701
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x000E0CEC File Offset: 0x000DF0EC
		// (set) Token: 0x06002DB7 RID: 11703 RVA: 0x000E0D19 File Offset: 0x000DF119
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameReader");
				}
				return MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameReader");
				}
				MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002DB8 RID: 11704
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrameReader_get_KinectSensor(IntPtr pNative);

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x000E0D4C File Offset: 0x000DF14C
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameReader");
				}
				IntPtr intPtr = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000E0DC0 File Offset: 0x000DF1C0
		[AOT.MonoPInvokeCallback(typeof(MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<MultiSourceFrameArrivedEventArgs>> list = null;
			MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				MultiSourceFrameReader objThis = NativeObjectCache.GetObject<MultiSourceFrameReader>(pNative);
				MultiSourceFrameArrivedEventArgs args = new MultiSourceFrameArrivedEventArgs(result);
				using (List<EventHandler<MultiSourceFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<MultiSourceFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06002DBB RID: 11707
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_add_MultiSourceFrameArrived(IntPtr pNative, MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06002DBC RID: 11708 RVA: 0x000E0E8C File Offset: 0x000DF28C
		// (remove) Token: 0x06002DBD RID: 11709 RVA: 0x000E0F1C File Offset: 0x000DF31C
		public event EventHandler<MultiSourceFrameArrivedEventArgs> MultiSourceFrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<MultiSourceFrameArrivedEventArgs>> list = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate = new MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate(MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handler);
						MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate);
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_MultiSourceFrameArrived(this._pNative, windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<MultiSourceFrameArrivedEventArgs>> list = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (MultiSourceFrameReader.f__mg0 == null)
						{
							MultiSourceFrameReader.f__mg0 = new MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate(MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handler);
						}
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_MultiSourceFrameArrived(pNative, MultiSourceFrameReader.f__mg0, true);
						MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000E0FCC File Offset: 0x000DF3CC
		[AOT.MonoPInvokeCallback(typeof(MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				MultiSourceFrameReader objThis = NativeObjectCache.GetObject<MultiSourceFrameReader>(pNative);
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

		// Token: 0x06002DBF RID: 11711
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_add_PropertyChanged(IntPtr pNative, MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06002DC0 RID: 11712 RVA: 0x000E1098 File Offset: 0x000DF498
		// (remove) Token: 0x06002DC1 RID: 11713 RVA: 0x000E1128 File Offset: 0x000DF528
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (MultiSourceFrameReader.f__mg1 == null)
						{
							MultiSourceFrameReader.f__mg1 = new MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_PropertyChanged(pNative, MultiSourceFrameReader.f__mg1, true);
						MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002DC2 RID: 11714
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000E11D8 File Offset: 0x000DF5D8
		public MultiSourceFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("MultiSourceFrameReader");
			}
			IntPtr intPtr = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<MultiSourceFrame>(intPtr, (IntPtr n) => new MultiSourceFrame(n));
		}

		// Token: 0x06002DC4 RID: 11716
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000E124B File Offset: 0x000DF64B
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000E1270 File Offset: 0x000DF670
		private void __EventCleanup()
		{
			MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<MultiSourceFrameArrivedEventArgs>> list = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (MultiSourceFrameReader.f__mg2 == null)
						{
							MultiSourceFrameReader.f__mg2 = new MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate(MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handler);
						}
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_MultiSourceFrameArrived(pNative, MultiSourceFrameReader.f__mg2, true);
					}
					MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (MultiSourceFrameReader.f__mg3 == null)
						{
							MultiSourceFrameReader.f__mg3 = new MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_PropertyChanged(pNative2, MultiSourceFrameReader.f__mg3, true);
					}
					MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400183D RID: 6205
		internal IntPtr _pNative;

		// Token: 0x0400183E RID: 6206
		private static GCHandle _Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x0400183F RID: 6207
		private static CollectionMap<IntPtr, List<EventHandler<MultiSourceFrameArrivedEventArgs>>> Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<MultiSourceFrameArrivedEventArgs>>>();

		// Token: 0x04001840 RID: 6208
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04001841 RID: 6209
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x04001843 RID: 6211
		[CompilerGenerated]
		private static MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate f__mg0;

		// Token: 0x04001844 RID: 6212
		[CompilerGenerated]
		private static MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x04001846 RID: 6214
		[CompilerGenerated]
		private static MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate f__mg2;

		// Token: 0x04001847 RID: 6215
		[CompilerGenerated]
		private static MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x02000512 RID: 1298
		// (Invoke) Token: 0x06002DCB RID: 11723
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000513 RID: 1299
		// (Invoke) Token: 0x06002DCF RID: 11727
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
