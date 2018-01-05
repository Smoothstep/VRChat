using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004FB RID: 1275
	public sealed class InfraredFrameSource : INativeWrapper
	{
		// Token: 0x06002CE1 RID: 11489 RVA: 0x000DE935 File Offset: 0x000DCD35
		internal InfraredFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrameSource.Windows_Kinect_InfraredFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x000DE94F File Offset: 0x000DCD4F
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000DE958 File Offset: 0x000DCD58
		~InfraredFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06002CE4 RID: 11492
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002CE5 RID: 11493
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000DE988 File Offset: 0x000DCD88
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrameSource>(this._pNative);
			InfraredFrameSource.Windows_Kinect_InfraredFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002CE7 RID: 11495
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002CE8 RID: 11496 RVA: 0x000DE9C8 File Offset: 0x000DCDC8
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameSource");
				}
				IntPtr intPtr = InfraredFrameSource.Windows_Kinect_InfraredFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06002CE9 RID: 11497
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_InfraredFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002CEA RID: 11498 RVA: 0x000DEA3B File Offset: 0x000DCE3B
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameSource");
				}
				return InfraredFrameSource.Windows_Kinect_InfraredFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x06002CEB RID: 11499
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002CEC RID: 11500 RVA: 0x000DEA68 File Offset: 0x000DCE68
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameSource");
				}
				IntPtr intPtr = InfraredFrameSource.Windows_Kinect_InfraredFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000DEADC File Offset: 0x000DCEDC
		[AOT.MonoPInvokeCallback(typeof(InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				InfraredFrameSource objThis = NativeObjectCache.GetObject<InfraredFrameSource>(pNative);
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

		// Token: 0x06002CEE RID: 11502
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameSource_add_FrameCaptured(IntPtr pNative, InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06002CEF RID: 11503 RVA: 0x000DEBA8 File Offset: 0x000DCFA8
		// (remove) Token: 0x06002CF0 RID: 11504 RVA: 0x000DEC38 File Offset: 0x000DD038
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (InfraredFrameSource.f__mg0 == null)
						{
							InfraredFrameSource.f__mg0 = new InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_FrameCaptured(pNative, InfraredFrameSource.f__mg0, true);
						InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x000DECE8 File Offset: 0x000DD0E8
		[AOT.MonoPInvokeCallback(typeof(InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				InfraredFrameSource objThis = NativeObjectCache.GetObject<InfraredFrameSource>(pNative);
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

		// Token: 0x06002CF2 RID: 11506
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameSource_add_PropertyChanged(IntPtr pNative, InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06002CF3 RID: 11507 RVA: 0x000DEDB4 File Offset: 0x000DD1B4
		// (remove) Token: 0x06002CF4 RID: 11508 RVA: 0x000DEE44 File Offset: 0x000DD244
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (InfraredFrameSource.f__mg1 == null)
						{
							InfraredFrameSource.f__mg1 = new InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_PropertyChanged(pNative, InfraredFrameSource.f__mg1, true);
						InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002CF5 RID: 11509
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x06002CF6 RID: 11510 RVA: 0x000DEEF4 File Offset: 0x000DD2F4
		public InfraredFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrameSource");
			}
			IntPtr intPtr = InfraredFrameSource.Windows_Kinect_InfraredFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<InfraredFrameReader>(intPtr, (IntPtr n) => new InfraredFrameReader(n));
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x000DEF68 File Offset: 0x000DD368
		private void __EventCleanup()
		{
			InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (InfraredFrameSource.f__mg2 == null)
						{
							InfraredFrameSource.f__mg2 = new InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_FrameCaptured(pNative, InfraredFrameSource.f__mg2, true);
					}
					InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (InfraredFrameSource.f__mg3 == null)
						{
							InfraredFrameSource.f__mg3 = new InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_PropertyChanged(pNative2, InfraredFrameSource.f__mg3, true);
					}
					InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x040017E2 RID: 6114
		internal IntPtr _pNative;

		// Token: 0x040017E3 RID: 6115
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x040017E4 RID: 6116
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x040017E5 RID: 6117
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x040017E6 RID: 6118
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x040017E9 RID: 6121
		[CompilerGenerated]
		private static InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg0;

		// Token: 0x040017EA RID: 6122
		[CompilerGenerated]
		private static InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x040017EC RID: 6124
		[CompilerGenerated]
		private static InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg2;

		// Token: 0x040017ED RID: 6125
		[CompilerGenerated]
		private static InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004FC RID: 1276
		// (Invoke) Token: 0x06002CFD RID: 11517
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004FD RID: 1277
		// (Invoke) Token: 0x06002D01 RID: 11521
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
