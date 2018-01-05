using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x0200050C RID: 1292
	public sealed class LongExposureInfraredFrameSource : INativeWrapper
	{
		// Token: 0x06002D66 RID: 11622 RVA: 0x000DFF1D File Offset: 0x000DE31D
		internal LongExposureInfraredFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002D67 RID: 11623 RVA: 0x000DFF37 File Offset: 0x000DE337
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000DFF40 File Offset: 0x000DE340
		~LongExposureInfraredFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D69 RID: 11625
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002D6A RID: 11626
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002D6B RID: 11627 RVA: 0x000DFF70 File Offset: 0x000DE370
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrameSource>(this._pNative);
			LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002D6C RID: 11628
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002D6D RID: 11629 RVA: 0x000DFFB0 File Offset: 0x000DE3B0
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameSource");
				}
				IntPtr intPtr = LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06002D6E RID: 11630
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_LongExposureInfraredFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002D6F RID: 11631 RVA: 0x000E0023 File Offset: 0x000DE423
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameSource");
				}
				return LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x06002D70 RID: 11632
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002D71 RID: 11633 RVA: 0x000E0050 File Offset: 0x000DE450
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameSource");
				}
				IntPtr intPtr = LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000E00C4 File Offset: 0x000DE4C4
		[AOT.MonoPInvokeCallback(typeof(LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				LongExposureInfraredFrameSource objThis = NativeObjectCache.GetObject<LongExposureInfraredFrameSource>(pNative);
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

		// Token: 0x06002D73 RID: 11635
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameSource_add_FrameCaptured(IntPtr pNative, LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06002D74 RID: 11636 RVA: 0x000E0190 File Offset: 0x000DE590
		// (remove) Token: 0x06002D75 RID: 11637 RVA: 0x000E0220 File Offset: 0x000DE620
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (LongExposureInfraredFrameSource.f__mg0 == null)
						{
							LongExposureInfraredFrameSource.f__mg0 = new LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_FrameCaptured(pNative, LongExposureInfraredFrameSource.f__mg0, true);
						LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000E02D0 File Offset: 0x000DE6D0
		[AOT.MonoPInvokeCallback(typeof(LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				LongExposureInfraredFrameSource objThis = NativeObjectCache.GetObject<LongExposureInfraredFrameSource>(pNative);
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

		// Token: 0x06002D77 RID: 11639
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameSource_add_PropertyChanged(IntPtr pNative, LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06002D78 RID: 11640 RVA: 0x000E039C File Offset: 0x000DE79C
		// (remove) Token: 0x06002D79 RID: 11641 RVA: 0x000E042C File Offset: 0x000DE82C
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (LongExposureInfraredFrameSource.f__mg1 == null)
						{
							LongExposureInfraredFrameSource.f__mg1 = new LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_PropertyChanged(pNative, LongExposureInfraredFrameSource.f__mg1, true);
						LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002D7A RID: 11642
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x06002D7B RID: 11643 RVA: 0x000E04DC File Offset: 0x000DE8DC
		public LongExposureInfraredFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrameSource");
			}
			IntPtr intPtr = LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameReader>(intPtr, (IntPtr n) => new LongExposureInfraredFrameReader(n));
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000E0550 File Offset: 0x000DE950
		private void __EventCleanup()
		{
			LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (LongExposureInfraredFrameSource.f__mg2 == null)
						{
							LongExposureInfraredFrameSource.f__mg2 = new LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_FrameCaptured(pNative, LongExposureInfraredFrameSource.f__mg2, true);
					}
					LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (LongExposureInfraredFrameSource.f__mg3 == null)
						{
							LongExposureInfraredFrameSource.f__mg3 = new LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_PropertyChanged(pNative2, LongExposureInfraredFrameSource.f__mg3, true);
					}
					LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04001828 RID: 6184
		internal IntPtr _pNative;

		// Token: 0x04001829 RID: 6185
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x0400182A RID: 6186
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x0400182B RID: 6187
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400182C RID: 6188
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0400182F RID: 6191
		[CompilerGenerated]
		private static LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg0;

		// Token: 0x04001830 RID: 6192
		[CompilerGenerated]
		private static LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x04001832 RID: 6194
		[CompilerGenerated]
		private static LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg2;

		// Token: 0x04001833 RID: 6195
		[CompilerGenerated]
		private static LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x0200050D RID: 1293
		// (Invoke) Token: 0x06002D82 RID: 11650
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x0200050E RID: 1294
		// (Invoke) Token: 0x06002D86 RID: 11654
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
