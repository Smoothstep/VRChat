using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004D4 RID: 1236
	public sealed class BodyIndexFrameSource : INativeWrapper
	{
		// Token: 0x06002B6A RID: 11114 RVA: 0x000DA78D File Offset: 0x000D8B8D
		internal BodyIndexFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002B6B RID: 11115 RVA: 0x000DA7A7 File Offset: 0x000D8BA7
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x000DA7B0 File Offset: 0x000D8BB0
		~BodyIndexFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06002B6D RID: 11117
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002B6E RID: 11118
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002B6F RID: 11119 RVA: 0x000DA7E0 File Offset: 0x000D8BE0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrameSource>(this._pNative);
			BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002B70 RID: 11120
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002B71 RID: 11121 RVA: 0x000DA820 File Offset: 0x000D8C20
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameSource");
				}
				IntPtr intPtr = BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06002B72 RID: 11122
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_BodyIndexFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002B73 RID: 11123 RVA: 0x000DA893 File Offset: 0x000D8C93
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameSource");
				}
				return BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x06002B74 RID: 11124
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x000DA8C0 File Offset: 0x000D8CC0
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameSource");
				}
				IntPtr intPtr = BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x000DA934 File Offset: 0x000D8D34
		[AOT.MonoPInvokeCallback(typeof(BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				BodyIndexFrameSource objThis = NativeObjectCache.GetObject<BodyIndexFrameSource>(pNative);
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

		// Token: 0x06002B77 RID: 11127
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameSource_add_FrameCaptured(IntPtr pNative, BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06002B78 RID: 11128 RVA: 0x000DAA00 File Offset: 0x000D8E00
		// (remove) Token: 0x06002B79 RID: 11129 RVA: 0x000DAA90 File Offset: 0x000D8E90
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (BodyIndexFrameSource.f__mg0 == null)
						{
							BodyIndexFrameSource.f__mg0 = new BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_FrameCaptured(pNative, BodyIndexFrameSource.f__mg0, true);
						BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x000DAB40 File Offset: 0x000D8F40
		[AOT.MonoPInvokeCallback(typeof(BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				BodyIndexFrameSource objThis = NativeObjectCache.GetObject<BodyIndexFrameSource>(pNative);
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

		// Token: 0x06002B7B RID: 11131
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameSource_add_PropertyChanged(IntPtr pNative, BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06002B7C RID: 11132 RVA: 0x000DAC0C File Offset: 0x000D900C
		// (remove) Token: 0x06002B7D RID: 11133 RVA: 0x000DAC9C File Offset: 0x000D909C
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (BodyIndexFrameSource.f__mg1 == null)
						{
							BodyIndexFrameSource.f__mg1 = new BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_PropertyChanged(pNative, BodyIndexFrameSource.f__mg1, true);
						BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002B7E RID: 11134
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x06002B7F RID: 11135 RVA: 0x000DAD4C File Offset: 0x000D914C
		public BodyIndexFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrameSource");
			}
			IntPtr intPtr = BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyIndexFrameReader>(intPtr, (IntPtr n) => new BodyIndexFrameReader(n));
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x000DADC0 File Offset: 0x000D91C0
		private void __EventCleanup()
		{
			BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (BodyIndexFrameSource.f__mg2 == null)
						{
							BodyIndexFrameSource.f__mg2 = new BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_FrameCaptured(pNative, BodyIndexFrameSource.f__mg2, true);
					}
					BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (BodyIndexFrameSource.f__mg3 == null)
						{
							BodyIndexFrameSource.f__mg3 = new BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_PropertyChanged(pNative2, BodyIndexFrameSource.f__mg3, true);
					}
					BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400175D RID: 5981
		internal IntPtr _pNative;

		// Token: 0x0400175E RID: 5982
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x0400175F RID: 5983
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x04001760 RID: 5984
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04001761 RID: 5985
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x04001764 RID: 5988
		[CompilerGenerated]
		private static BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg0;

		// Token: 0x04001765 RID: 5989
		[CompilerGenerated]
		private static BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x04001767 RID: 5991
		[CompilerGenerated]
		private static BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg2;

		// Token: 0x04001768 RID: 5992
		[CompilerGenerated]
		private static BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004D5 RID: 1237
		// (Invoke) Token: 0x06002B86 RID: 11142
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004D6 RID: 1238
		// (Invoke) Token: 0x06002B8A RID: 11146
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
