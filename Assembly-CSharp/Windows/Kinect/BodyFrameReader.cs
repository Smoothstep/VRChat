using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004C8 RID: 1224
	public sealed class BodyFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x06002ADA RID: 10970 RVA: 0x000D8AB1 File Offset: 0x000D6EB1
		internal BodyFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrameReader.Windows_Kinect_BodyFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002ADB RID: 10971 RVA: 0x000D8ACB File Offset: 0x000D6ECB
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000D8AD4 File Offset: 0x000D6ED4
		~BodyFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06002ADD RID: 10973
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002ADE RID: 10974
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002ADF RID: 10975 RVA: 0x000D8B04 File Offset: 0x000D6F04
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrameReader>(this._pNative);
			if (disposing)
			{
				BodyFrameReader.Windows_Kinect_BodyFrameReader_Dispose(this._pNative);
			}
			BodyFrameReader.Windows_Kinect_BodyFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002AE0 RID: 10976
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameReader_get_BodyFrameSource(IntPtr pNative);

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x000D8B60 File Offset: 0x000D6F60
		public BodyFrameSource BodyFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameReader");
				}
				IntPtr intPtr = BodyFrameReader.Windows_Kinect_BodyFrameReader_get_BodyFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameSource>(intPtr, (IntPtr n) => new BodyFrameSource(n));
			}
		}

		// Token: 0x06002AE2 RID: 10978
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_BodyFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06002AE3 RID: 10979
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x000D8BD3 File Offset: 0x000D6FD3
		// (set) Token: 0x06002AE5 RID: 10981 RVA: 0x000D8C00 File Offset: 0x000D7000
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameReader");
				}
				return BodyFrameReader.Windows_Kinect_BodyFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameReader");
				}
				BodyFrameReader.Windows_Kinect_BodyFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000D8C34 File Offset: 0x000D7034
		[AOT.MonoPInvokeCallback(typeof(BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<BodyFrameArrivedEventArgs>> list = null;
			BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				BodyFrameReader objThis = NativeObjectCache.GetObject<BodyFrameReader>(pNative);
				BodyFrameArrivedEventArgs args = new BodyFrameArrivedEventArgs(result);
				using (List<EventHandler<BodyFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<BodyFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06002AE7 RID: 10983
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_add_FrameArrived(IntPtr pNative, BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06002AE8 RID: 10984 RVA: 0x000D8D00 File Offset: 0x000D7100
		// (remove) Token: 0x06002AE9 RID: 10985 RVA: 0x000D8D90 File Offset: 0x000D7190
		public event EventHandler<BodyFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<BodyFrameArrivedEventArgs>> list = BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate windows_Kinect_BodyFrameArrivedEventArgs_Delegate = new BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate(BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handler);
						BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_BodyFrameArrivedEventArgs_Delegate);
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_FrameArrived(this._pNative, windows_Kinect_BodyFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<BodyFrameArrivedEventArgs>> list = BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (BodyFrameReader.f__mg0 == null)
						{
							BodyFrameReader.f__mg0 = new BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate(BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handler);
						}
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_FrameArrived(pNative, BodyFrameReader.f__mg0, true);
						BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000D8E40 File Offset: 0x000D7240
		[AOT.MonoPInvokeCallback(typeof(BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				BodyFrameReader objThis = NativeObjectCache.GetObject<BodyFrameReader>(pNative);
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

		// Token: 0x06002AEB RID: 10987
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_add_PropertyChanged(IntPtr pNative, BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06002AEC RID: 10988 RVA: 0x000D8F0C File Offset: 0x000D730C
		// (remove) Token: 0x06002AED RID: 10989 RVA: 0x000D8F9C File Offset: 0x000D739C
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (BodyFrameReader.f__mg1 == null)
						{
							BodyFrameReader.f__mg1 = new BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_PropertyChanged(pNative, BodyFrameReader.f__mg1, true);
						BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002AEE RID: 10990
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x06002AEF RID: 10991 RVA: 0x000D904C File Offset: 0x000D744C
		public BodyFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameReader");
			}
			IntPtr intPtr = BodyFrameReader.Windows_Kinect_BodyFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyFrame>(intPtr, (IntPtr n) => new BodyFrame(n));
		}

		// Token: 0x06002AF0 RID: 10992
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06002AF1 RID: 10993 RVA: 0x000D90BF File Offset: 0x000D74BF
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000D90E4 File Offset: 0x000D74E4
		private void __EventCleanup()
		{
			BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<BodyFrameArrivedEventArgs>> list = BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (BodyFrameReader.f__mg2 == null)
						{
							BodyFrameReader.f__mg2 = new BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate(BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handler);
						}
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_FrameArrived(pNative, BodyFrameReader.f__mg2, true);
					}
					BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (BodyFrameReader.f__mg3 == null)
						{
							BodyFrameReader.f__mg3 = new BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_PropertyChanged(pNative2, BodyFrameReader.f__mg3, true);
					}
					BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04001736 RID: 5942
		internal IntPtr _pNative;

		// Token: 0x04001737 RID: 5943
		private static GCHandle _Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x04001738 RID: 5944
		private static CollectionMap<IntPtr, List<EventHandler<BodyFrameArrivedEventArgs>>> Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<BodyFrameArrivedEventArgs>>>();

		// Token: 0x04001739 RID: 5945
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400173A RID: 5946
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0400173C RID: 5948
		[CompilerGenerated]
		private static BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate f__mg0;

		// Token: 0x0400173D RID: 5949
		[CompilerGenerated]
		private static BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x0400173F RID: 5951
		[CompilerGenerated]
		private static BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate f__mg2;

		// Token: 0x04001740 RID: 5952
		[CompilerGenerated]
		private static BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004C9 RID: 1225
		// (Invoke) Token: 0x06002AF7 RID: 10999
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_BodyFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004CA RID: 1226
		// (Invoke) Token: 0x06002AFB RID: 11003
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
