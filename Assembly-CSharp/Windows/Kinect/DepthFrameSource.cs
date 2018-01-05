using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004EA RID: 1258
	public sealed class DepthFrameSource : INativeWrapper
	{
		// Token: 0x06002C55 RID: 11349 RVA: 0x000DD201 File Offset: 0x000DB601
		internal DepthFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrameSource.Windows_Kinect_DepthFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002C56 RID: 11350 RVA: 0x000DD21B File Offset: 0x000DB61B
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x000DD224 File Offset: 0x000DB624
		~DepthFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06002C58 RID: 11352
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002C59 RID: 11353
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002C5A RID: 11354 RVA: 0x000DD254 File Offset: 0x000DB654
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrameSource>(this._pNative);
			DepthFrameSource.Windows_Kinect_DepthFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002C5B RID: 11355
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ushort Windows_Kinect_DepthFrameSource_get_DepthMaxReliableDistance(IntPtr pNative);

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002C5C RID: 11356 RVA: 0x000DD293 File Offset: 0x000DB693
		public ushort DepthMaxReliableDistance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				return DepthFrameSource.Windows_Kinect_DepthFrameSource_get_DepthMaxReliableDistance(this._pNative);
			}
		}

		// Token: 0x06002C5D RID: 11357
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ushort Windows_Kinect_DepthFrameSource_get_DepthMinReliableDistance(IntPtr pNative);

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06002C5E RID: 11358 RVA: 0x000DD2C0 File Offset: 0x000DB6C0
		public ushort DepthMinReliableDistance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				return DepthFrameSource.Windows_Kinect_DepthFrameSource_get_DepthMinReliableDistance(this._pNative);
			}
		}

		// Token: 0x06002C5F RID: 11359
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002C60 RID: 11360 RVA: 0x000DD2F0 File Offset: 0x000DB6F0
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				IntPtr intPtr = DepthFrameSource.Windows_Kinect_DepthFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06002C61 RID: 11361
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_DepthFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x000DD363 File Offset: 0x000DB763
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				return DepthFrameSource.Windows_Kinect_DepthFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x06002C63 RID: 11363
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002C64 RID: 11364 RVA: 0x000DD390 File Offset: 0x000DB790
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				IntPtr intPtr = DepthFrameSource.Windows_Kinect_DepthFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000DD404 File Offset: 0x000DB804
		[AOT.MonoPInvokeCallback(typeof(DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				DepthFrameSource objThis = NativeObjectCache.GetObject<DepthFrameSource>(pNative);
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

		// Token: 0x06002C66 RID: 11366
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameSource_add_FrameCaptured(IntPtr pNative, DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06002C67 RID: 11367 RVA: 0x000DD4D0 File Offset: 0x000DB8D0
		// (remove) Token: 0x06002C68 RID: 11368 RVA: 0x000DD560 File Offset: 0x000DB960
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (DepthFrameSource.f__mg0 == null)
						{
							DepthFrameSource.f__mg0 = new DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_FrameCaptured(pNative, DepthFrameSource.f__mg0, true);
						DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000DD610 File Offset: 0x000DBA10
		[AOT.MonoPInvokeCallback(typeof(DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				DepthFrameSource objThis = NativeObjectCache.GetObject<DepthFrameSource>(pNative);
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

		// Token: 0x06002C6A RID: 11370
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameSource_add_PropertyChanged(IntPtr pNative, DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06002C6B RID: 11371 RVA: 0x000DD6DC File Offset: 0x000DBADC
		// (remove) Token: 0x06002C6C RID: 11372 RVA: 0x000DD76C File Offset: 0x000DBB6C
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (DepthFrameSource.f__mg1 == null)
						{
							DepthFrameSource.f__mg1 = new DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_PropertyChanged(pNative, DepthFrameSource.f__mg1, true);
						DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002C6D RID: 11373
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x06002C6E RID: 11374 RVA: 0x000DD81C File Offset: 0x000DBC1C
		public DepthFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrameSource");
			}
			IntPtr intPtr = DepthFrameSource.Windows_Kinect_DepthFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<DepthFrameReader>(intPtr, (IntPtr n) => new DepthFrameReader(n));
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x000DD890 File Offset: 0x000DBC90
		private void __EventCleanup()
		{
			DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (DepthFrameSource.f__mg2 == null)
						{
							DepthFrameSource.f__mg2 = new DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_FrameCaptured(pNative, DepthFrameSource.f__mg2, true);
					}
					DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (DepthFrameSource.f__mg3 == null)
						{
							DepthFrameSource.f__mg3 = new DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_PropertyChanged(pNative2, DepthFrameSource.f__mg3, true);
					}
					DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x040017A2 RID: 6050
		internal IntPtr _pNative;

		// Token: 0x040017A3 RID: 6051
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x040017A4 RID: 6052
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x040017A5 RID: 6053
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x040017A6 RID: 6054
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x040017A9 RID: 6057
		[CompilerGenerated]
		private static DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg0;

		// Token: 0x040017AA RID: 6058
		[CompilerGenerated]
		private static DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x040017AC RID: 6060
		[CompilerGenerated]
		private static DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg2;

		// Token: 0x040017AD RID: 6061
		[CompilerGenerated]
		private static DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004EB RID: 1259
		// (Invoke) Token: 0x06002C75 RID: 11381
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004EC RID: 1260
		// (Invoke) Token: 0x06002C79 RID: 11385
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
