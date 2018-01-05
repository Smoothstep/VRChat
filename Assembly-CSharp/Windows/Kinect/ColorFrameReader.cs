using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004DA RID: 1242
	public sealed class ColorFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x06002BB1 RID: 11185 RVA: 0x000DB365 File Offset: 0x000D9765
		internal ColorFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrameReader.Windows_Kinect_ColorFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x000DB37F File Offset: 0x000D977F
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000DB388 File Offset: 0x000D9788
		~ColorFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06002BB4 RID: 11188
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002BB5 RID: 11189
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000DB3B8 File Offset: 0x000D97B8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrameReader>(this._pNative);
			if (disposing)
			{
				ColorFrameReader.Windows_Kinect_ColorFrameReader_Dispose(this._pNative);
			}
			ColorFrameReader.Windows_Kinect_ColorFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002BB7 RID: 11191
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameReader_get_ColorFrameSource(IntPtr pNative);

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x000DB414 File Offset: 0x000D9814
		public ColorFrameSource ColorFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameReader");
				}
				IntPtr intPtr = ColorFrameReader.Windows_Kinect_ColorFrameReader_get_ColorFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameSource>(intPtr, (IntPtr n) => new ColorFrameSource(n));
			}
		}

		// Token: 0x06002BB9 RID: 11193
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_ColorFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06002BBA RID: 11194
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002BBB RID: 11195 RVA: 0x000DB487 File Offset: 0x000D9887
		// (set) Token: 0x06002BBC RID: 11196 RVA: 0x000DB4B4 File Offset: 0x000D98B4
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameReader");
				}
				return ColorFrameReader.Windows_Kinect_ColorFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameReader");
				}
				ColorFrameReader.Windows_Kinect_ColorFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000DB4E8 File Offset: 0x000D98E8
		[AOT.MonoPInvokeCallback(typeof(ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<ColorFrameArrivedEventArgs>> list = null;
			ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				ColorFrameReader objThis = NativeObjectCache.GetObject<ColorFrameReader>(pNative);
				ColorFrameArrivedEventArgs args = new ColorFrameArrivedEventArgs(result);
				using (List<EventHandler<ColorFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<ColorFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06002BBE RID: 11198
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_add_FrameArrived(IntPtr pNative, ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06002BBF RID: 11199 RVA: 0x000DB5B4 File Offset: 0x000D99B4
		// (remove) Token: 0x06002BC0 RID: 11200 RVA: 0x000DB644 File Offset: 0x000D9A44
		public event EventHandler<ColorFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<ColorFrameArrivedEventArgs>> list = ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate windows_Kinect_ColorFrameArrivedEventArgs_Delegate = new ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate(ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handler);
						ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_ColorFrameArrivedEventArgs_Delegate);
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_FrameArrived(this._pNative, windows_Kinect_ColorFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<ColorFrameArrivedEventArgs>> list = ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (ColorFrameReader.f__mg0 == null)
						{
							ColorFrameReader.f__mg0 = new ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate(ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handler);
						}
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_FrameArrived(pNative, ColorFrameReader.f__mg0, true);
						ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x000DB6F4 File Offset: 0x000D9AF4
		[AOT.MonoPInvokeCallback(typeof(ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				ColorFrameReader objThis = NativeObjectCache.GetObject<ColorFrameReader>(pNative);
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

		// Token: 0x06002BC2 RID: 11202
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_add_PropertyChanged(IntPtr pNative, ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06002BC3 RID: 11203 RVA: 0x000DB7C0 File Offset: 0x000D9BC0
		// (remove) Token: 0x06002BC4 RID: 11204 RVA: 0x000DB850 File Offset: 0x000D9C50
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (ColorFrameReader.f__mg1 == null)
						{
							ColorFrameReader.f__mg1 = new ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_PropertyChanged(pNative, ColorFrameReader.f__mg1, true);
						ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002BC5 RID: 11205
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000DB900 File Offset: 0x000D9D00
		public ColorFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrameReader");
			}
			IntPtr intPtr = ColorFrameReader.Windows_Kinect_ColorFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<ColorFrame>(intPtr, (IntPtr n) => new ColorFrame(n));
		}

		// Token: 0x06002BC7 RID: 11207
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06002BC8 RID: 11208 RVA: 0x000DB973 File Offset: 0x000D9D73
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000DB998 File Offset: 0x000D9D98
		private void __EventCleanup()
		{
			ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<ColorFrameArrivedEventArgs>> list = ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (ColorFrameReader.f__mg2 == null)
						{
							ColorFrameReader.f__mg2 = new ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate(ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handler);
						}
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_FrameArrived(pNative, ColorFrameReader.f__mg2, true);
					}
					ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (ColorFrameReader.f__mg3 == null)
						{
							ColorFrameReader.f__mg3 = new ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_PropertyChanged(pNative2, ColorFrameReader.f__mg3, true);
					}
					ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400176F RID: 5999
		internal IntPtr _pNative;

		// Token: 0x04001770 RID: 6000
		private static GCHandle _Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x04001771 RID: 6001
		private static CollectionMap<IntPtr, List<EventHandler<ColorFrameArrivedEventArgs>>> Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<ColorFrameArrivedEventArgs>>>();

		// Token: 0x04001772 RID: 6002
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04001773 RID: 6003
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x04001775 RID: 6005
		[CompilerGenerated]
		private static ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate f__mg0;

		// Token: 0x04001776 RID: 6006
		[CompilerGenerated]
		private static ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x04001778 RID: 6008
		[CompilerGenerated]
		private static ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate f__mg2;

		// Token: 0x04001779 RID: 6009
		[CompilerGenerated]
		private static ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004DB RID: 1243
		// (Invoke) Token: 0x06002BCE RID: 11214
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_ColorFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004DC RID: 1244
		// (Invoke) Token: 0x06002BD2 RID: 11218
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
