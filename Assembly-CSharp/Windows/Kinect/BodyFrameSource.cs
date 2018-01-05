using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004CC RID: 1228
	public sealed class BodyFrameSource : INativeWrapper
	{
		// Token: 0x06002B0A RID: 11018 RVA: 0x000D9455 File Offset: 0x000D7855
		internal BodyFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrameSource.Windows_Kinect_BodyFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06002B0B RID: 11019 RVA: 0x000D946F File Offset: 0x000D786F
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x000D9478 File Offset: 0x000D7878
		~BodyFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06002B0D RID: 11021
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002B0E RID: 11022
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002B0F RID: 11023 RVA: 0x000D94A8 File Offset: 0x000D78A8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrameSource>(this._pNative);
			BodyFrameSource.Windows_Kinect_BodyFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002B10 RID: 11024
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_BodyFrameSource_get_BodyCount(IntPtr pNative);

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06002B11 RID: 11025 RVA: 0x000D94E7 File Offset: 0x000D78E7
		public int BodyCount
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameSource");
				}
				return BodyFrameSource.Windows_Kinect_BodyFrameSource_get_BodyCount(this._pNative);
			}
		}

		// Token: 0x06002B12 RID: 11026
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_BodyFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002B13 RID: 11027 RVA: 0x000D9514 File Offset: 0x000D7914
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameSource");
				}
				return BodyFrameSource.Windows_Kinect_BodyFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x06002B14 RID: 11028
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x000D9544 File Offset: 0x000D7944
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameSource");
				}
				IntPtr intPtr = BodyFrameSource.Windows_Kinect_BodyFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x000D95B8 File Offset: 0x000D79B8
		[AOT.MonoPInvokeCallback(typeof(BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				BodyFrameSource objThis = NativeObjectCache.GetObject<BodyFrameSource>(pNative);
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

		// Token: 0x06002B17 RID: 11031
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_add_FrameCaptured(IntPtr pNative, BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06002B18 RID: 11032 RVA: 0x000D9684 File Offset: 0x000D7A84
		// (remove) Token: 0x06002B19 RID: 11033 RVA: 0x000D9714 File Offset: 0x000D7B14
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (BodyFrameSource.f__mg0 == null)
						{
							BodyFrameSource.f__mg0 = new BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_FrameCaptured(pNative, BodyFrameSource.f__mg0, true);
						BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000D97C4 File Offset: 0x000D7BC4
		[AOT.MonoPInvokeCallback(typeof(BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				BodyFrameSource objThis = NativeObjectCache.GetObject<BodyFrameSource>(pNative);
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

		// Token: 0x06002B1B RID: 11035
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_add_PropertyChanged(IntPtr pNative, BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06002B1C RID: 11036 RVA: 0x000D9890 File Offset: 0x000D7C90
		// (remove) Token: 0x06002B1D RID: 11037 RVA: 0x000D9920 File Offset: 0x000D7D20
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (BodyFrameSource.f__mg1 == null)
						{
							BodyFrameSource.f__mg1 = new BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_PropertyChanged(pNative, BodyFrameSource.f__mg1, true);
						BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002B1E RID: 11038
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x06002B1F RID: 11039 RVA: 0x000D99D0 File Offset: 0x000D7DD0
		public BodyFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameSource");
			}
			IntPtr intPtr = BodyFrameSource.Windows_Kinect_BodyFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyFrameReader>(intPtr, (IntPtr n) => new BodyFrameReader(n));
		}

		// Token: 0x06002B20 RID: 11040
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_OverrideHandTracking(IntPtr pNative, ulong trackingId);

		// Token: 0x06002B21 RID: 11041 RVA: 0x000D9A43 File Offset: 0x000D7E43
		public void OverrideHandTracking(ulong trackingId)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameSource");
			}
			BodyFrameSource.Windows_Kinect_BodyFrameSource_OverrideHandTracking(this._pNative, trackingId);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002B22 RID: 11042
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_OverrideHandTracking_1(IntPtr pNative, ulong oldTrackingId, ulong newTrackingId);

		// Token: 0x06002B23 RID: 11043 RVA: 0x000D9A76 File Offset: 0x000D7E76
		public void OverrideHandTracking(ulong oldTrackingId, ulong newTrackingId)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameSource");
			}
			BodyFrameSource.Windows_Kinect_BodyFrameSource_OverrideHandTracking_1(this._pNative, oldTrackingId, newTrackingId);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000D9AAC File Offset: 0x000D7EAC
		private void __EventCleanup()
		{
			BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (BodyFrameSource.f__mg2 == null)
						{
							BodyFrameSource.f__mg2 = new BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_FrameCaptured(pNative, BodyFrameSource.f__mg2, true);
					}
					BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (BodyFrameSource.f__mg3 == null)
						{
							BodyFrameSource.f__mg3 = new BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_PropertyChanged(pNative2, BodyFrameSource.f__mg3, true);
					}
					BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04001743 RID: 5955
		internal IntPtr _pNative;

		// Token: 0x04001744 RID: 5956
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x04001745 RID: 5957
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x04001746 RID: 5958
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04001747 RID: 5959
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x04001749 RID: 5961
		[CompilerGenerated]
		private static BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg0;

		// Token: 0x0400174A RID: 5962
		[CompilerGenerated]
		private static BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x0400174C RID: 5964
		[CompilerGenerated]
		private static BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg2;

		// Token: 0x0400174D RID: 5965
		[CompilerGenerated]
		private static BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004CD RID: 1229
		// (Invoke) Token: 0x06002B29 RID: 11049
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004CE RID: 1230
		// (Invoke) Token: 0x06002B2D RID: 11053
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
