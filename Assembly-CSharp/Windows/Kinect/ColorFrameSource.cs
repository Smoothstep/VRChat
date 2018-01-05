using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004DE RID: 1246
	public sealed class ColorFrameSource : INativeWrapper
	{
		// Token: 0x06002BE1 RID: 11233 RVA: 0x000DBD09 File Offset: 0x000DA109
		internal ColorFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrameSource.Windows_Kinect_ColorFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002BE2 RID: 11234 RVA: 0x000DBD23 File Offset: 0x000DA123
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000DBD2C File Offset: 0x000DA12C
		~ColorFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06002BE4 RID: 11236
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002BE5 RID: 11237
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000DBD5C File Offset: 0x000DA15C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrameSource>(this._pNative);
			ColorFrameSource.Windows_Kinect_ColorFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002BE7 RID: 11239
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x000DBD9C File Offset: 0x000DA19C
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameSource");
				}
				IntPtr intPtr = ColorFrameSource.Windows_Kinect_ColorFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06002BE9 RID: 11241
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_ColorFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002BEA RID: 11242 RVA: 0x000DBE0F File Offset: 0x000DA20F
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameSource");
				}
				return ColorFrameSource.Windows_Kinect_ColorFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x06002BEB RID: 11243
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002BEC RID: 11244 RVA: 0x000DBE3C File Offset: 0x000DA23C
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameSource");
				}
				IntPtr intPtr = ColorFrameSource.Windows_Kinect_ColorFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000DBEB0 File Offset: 0x000DA2B0
		[AOT.MonoPInvokeCallback(typeof(ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				ColorFrameSource objThis = NativeObjectCache.GetObject<ColorFrameSource>(pNative);
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

		// Token: 0x06002BEE RID: 11246
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameSource_add_FrameCaptured(IntPtr pNative, ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06002BEF RID: 11247 RVA: 0x000DBF7C File Offset: 0x000DA37C
		// (remove) Token: 0x06002BF0 RID: 11248 RVA: 0x000DC00C File Offset: 0x000DA40C
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (ColorFrameSource.f__mg0 == null)
						{
							ColorFrameSource.f__mg0 = new ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_FrameCaptured(pNative, ColorFrameSource.f__mg0, true);
						ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x000DC0BC File Offset: 0x000DA4BC
		[AOT.MonoPInvokeCallback(typeof(ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				ColorFrameSource objThis = NativeObjectCache.GetObject<ColorFrameSource>(pNative);
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

		// Token: 0x06002BF2 RID: 11250
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameSource_add_PropertyChanged(IntPtr pNative, ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06002BF3 RID: 11251 RVA: 0x000DC188 File Offset: 0x000DA588
		// (remove) Token: 0x06002BF4 RID: 11252 RVA: 0x000DC218 File Offset: 0x000DA618
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (ColorFrameSource.f__mg1 == null)
						{
							ColorFrameSource.f__mg1 = new ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_PropertyChanged(pNative, ColorFrameSource.f__mg1, true);
						ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002BF5 RID: 11253
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x06002BF6 RID: 11254 RVA: 0x000DC2C8 File Offset: 0x000DA6C8
		public ColorFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrameSource");
			}
			IntPtr intPtr = ColorFrameSource.Windows_Kinect_ColorFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<ColorFrameReader>(intPtr, (IntPtr n) => new ColorFrameReader(n));
		}

		// Token: 0x06002BF7 RID: 11255
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameSource_CreateFrameDescription(IntPtr pNative, ColorImageFormat format);

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000DC33C File Offset: 0x000DA73C
		public FrameDescription CreateFrameDescription(ColorImageFormat format)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrameSource");
			}
			IntPtr intPtr = ColorFrameSource.Windows_Kinect_ColorFrameSource_CreateFrameDescription(this._pNative, format);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000DC3B0 File Offset: 0x000DA7B0
		private void __EventCleanup()
		{
			ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (ColorFrameSource.f__mg2 == null)
						{
							ColorFrameSource.f__mg2 = new ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						}
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_FrameCaptured(pNative, ColorFrameSource.f__mg2, true);
					}
					ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (ColorFrameSource.f__mg3 == null)
						{
							ColorFrameSource.f__mg3 = new ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_PropertyChanged(pNative2, ColorFrameSource.f__mg3, true);
					}
					ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400177C RID: 6012
		internal IntPtr _pNative;

		// Token: 0x0400177D RID: 6013
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x0400177E RID: 6014
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x0400177F RID: 6015
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04001780 RID: 6016
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x04001783 RID: 6019
		[CompilerGenerated]
		private static ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg0;

		// Token: 0x04001784 RID: 6020
		[CompilerGenerated]
		private static ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x04001787 RID: 6023
		[CompilerGenerated]
		private static ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate f__mg2;

		// Token: 0x04001788 RID: 6024
		[CompilerGenerated]
		private static ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x020004DF RID: 1247
		// (Invoke) Token: 0x06002C00 RID: 11264
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020004E0 RID: 1248
		// (Invoke) Token: 0x06002C04 RID: 11268
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
