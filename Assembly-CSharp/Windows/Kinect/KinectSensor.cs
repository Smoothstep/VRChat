using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x020004A5 RID: 1189
	public sealed class KinectSensor : INativeWrapper
	{
		// Token: 0x0600295E RID: 10590 RVA: 0x000D3A0C File Offset: 0x000D1E0C
		internal KinectSensor(IntPtr pNative)
		{
			this._pNative = pNative;
			KinectSensor.Windows_Kinect_KinectSensor_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x000D3A28 File Offset: 0x000D1E28
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			if (this.IsOpen)
			{
				this.Close();
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<KinectSensor>(this._pNative);
			KinectSensor.Windows_Kinect_KinectSensor_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002960 RID: 10592 RVA: 0x000D3A83 File Offset: 0x000D1E83
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000D3A8C File Offset: 0x000D1E8C
		~KinectSensor()
		{
			this.Dispose(false);
		}

		// Token: 0x06002962 RID: 10594
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002963 RID: 10595
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002964 RID: 10596
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_AudioSource(IntPtr pNative);

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002965 RID: 10597 RVA: 0x000D3ABC File Offset: 0x000D1EBC
		public AudioSource AudioSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_AudioSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioSource>(intPtr, (IntPtr n) => new AudioSource(n));
			}
		}

		// Token: 0x06002966 RID: 10598
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_BodyFrameSource(IntPtr pNative);

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002967 RID: 10599 RVA: 0x000D3B30 File Offset: 0x000D1F30
		public BodyFrameSource BodyFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_BodyFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameSource>(intPtr, (IntPtr n) => new BodyFrameSource(n));
			}
		}

		// Token: 0x06002968 RID: 10600
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_BodyIndexFrameSource(IntPtr pNative);

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06002969 RID: 10601 RVA: 0x000D3BA4 File Offset: 0x000D1FA4
		public BodyIndexFrameSource BodyIndexFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_BodyIndexFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameSource>(intPtr, (IntPtr n) => new BodyIndexFrameSource(n));
			}
		}

		// Token: 0x0600296A RID: 10602
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_ColorFrameSource(IntPtr pNative);

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600296B RID: 10603 RVA: 0x000D3C18 File Offset: 0x000D2018
		public ColorFrameSource ColorFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_ColorFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameSource>(intPtr, (IntPtr n) => new ColorFrameSource(n));
			}
		}

		// Token: 0x0600296C RID: 10604
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_CoordinateMapper(IntPtr pNative);

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x0600296D RID: 10605 RVA: 0x000D3C8C File Offset: 0x000D208C
		public CoordinateMapper CoordinateMapper
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_CoordinateMapper(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<CoordinateMapper>(intPtr, (IntPtr n) => new CoordinateMapper(n));
			}
		}

		// Token: 0x0600296E RID: 10606
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_DepthFrameSource(IntPtr pNative);

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x0600296F RID: 10607 RVA: 0x000D3D00 File Offset: 0x000D2100
		public DepthFrameSource DepthFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_DepthFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameSource>(intPtr, (IntPtr n) => new DepthFrameSource(n));
			}
		}

		// Token: 0x06002970 RID: 10608
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_InfraredFrameSource(IntPtr pNative);

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002971 RID: 10609 RVA: 0x000D3D74 File Offset: 0x000D2174
		public InfraredFrameSource InfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_InfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameSource>(intPtr, (IntPtr n) => new InfraredFrameSource(n));
			}
		}

		// Token: 0x06002972 RID: 10610
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_KinectSensor_get_IsAvailable(IntPtr pNative);

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06002973 RID: 10611 RVA: 0x000D3DE7 File Offset: 0x000D21E7
		public bool IsAvailable
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				return KinectSensor.Windows_Kinect_KinectSensor_get_IsAvailable(this._pNative);
			}
		}

		// Token: 0x06002974 RID: 10612
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_KinectSensor_get_IsOpen(IntPtr pNative);

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002975 RID: 10613 RVA: 0x000D3E14 File Offset: 0x000D2214
		public bool IsOpen
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				return KinectSensor.Windows_Kinect_KinectSensor_get_IsOpen(this._pNative);
			}
		}

		// Token: 0x06002976 RID: 10614
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern KinectCapabilities Windows_Kinect_KinectSensor_get_KinectCapabilities(IntPtr pNative);

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002977 RID: 10615 RVA: 0x000D3E41 File Offset: 0x000D2241
		public KinectCapabilities KinectCapabilities
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				return KinectSensor.Windows_Kinect_KinectSensor_get_KinectCapabilities(this._pNative);
			}
		}

		// Token: 0x06002978 RID: 10616
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_LongExposureInfraredFrameSource(IntPtr pNative);

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002979 RID: 10617 RVA: 0x000D3E70 File Offset: 0x000D2270
		public LongExposureInfraredFrameSource LongExposureInfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_LongExposureInfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameSource>(intPtr, (IntPtr n) => new LongExposureInfraredFrameSource(n));
			}
		}

		// Token: 0x0600297A RID: 10618
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_UniqueKinectId(IntPtr pNative);

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x0600297B RID: 10619 RVA: 0x000D3EE4 File Offset: 0x000D22E4
		public string UniqueKinectId
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr ptr = KinectSensor.Windows_Kinect_KinectSensor_get_UniqueKinectId(this._pNative);
				ExceptionHelper.CheckLastError();
				string result = Marshal.PtrToStringUni(ptr);
				Marshal.FreeCoTaskMem(ptr);
				return result;
			}
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000D3F30 File Offset: 0x000D2330
		[AOT.MonoPInvokeCallback(typeof(KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate))]
		private static void Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<IsAvailableChangedEventArgs>> list = null;
			KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				KinectSensor objThis = NativeObjectCache.GetObject<KinectSensor>(pNative);
				IsAvailableChangedEventArgs args = new IsAvailableChangedEventArgs(result);
				using (List<EventHandler<IsAvailableChangedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<IsAvailableChangedEventArgs> func = enumerator.Current;
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

		// Token: 0x0600297D RID: 10621
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_add_IsAvailableChanged(IntPtr pNative, KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600297E RID: 10622 RVA: 0x000D3FFC File Offset: 0x000D23FC
		// (remove) Token: 0x0600297F RID: 10623 RVA: 0x000D408C File Offset: 0x000D248C
		public event EventHandler<IsAvailableChangedEventArgs> IsAvailableChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<IsAvailableChangedEventArgs>> list = KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate windows_Kinect_IsAvailableChangedEventArgs_Delegate = new KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate(KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handler);
						KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_IsAvailableChangedEventArgs_Delegate);
						KinectSensor.Windows_Kinect_KinectSensor_add_IsAvailableChanged(this._pNative, windows_Kinect_IsAvailableChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<IsAvailableChangedEventArgs>> list = KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (KinectSensor.f__mg0 == null)
						{
							KinectSensor.f__mg0 = new KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate(KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handler);
						}
						KinectSensor.Windows_Kinect_KinectSensor_add_IsAvailableChanged(pNative, KinectSensor.f__mg0, true);
						KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000D413C File Offset: 0x000D253C
		[AOT.MonoPInvokeCallback(typeof(KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				KinectSensor objThis = NativeObjectCache.GetObject<KinectSensor>(pNative);
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

		// Token: 0x06002981 RID: 10625
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_add_PropertyChanged(IntPtr pNative, KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06002982 RID: 10626 RVA: 0x000D4208 File Offset: 0x000D2608
		// (remove) Token: 0x06002983 RID: 10627 RVA: 0x000D4298 File Offset: 0x000D2698
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate(KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						KinectSensor.Windows_Kinect_KinectSensor_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (KinectSensor.f__mg1 == null)
						{
							KinectSensor.f__mg1 = new KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate(KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						KinectSensor.Windows_Kinect_KinectSensor_add_PropertyChanged(pNative, KinectSensor.f__mg1, true);
						KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06002984 RID: 10628
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_GetDefault();

		// Token: 0x06002985 RID: 10629 RVA: 0x000D4348 File Offset: 0x000D2748
		public static KinectSensor GetDefault()
		{
			IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_GetDefault();
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
		}

		// Token: 0x06002986 RID: 10630
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_Open(IntPtr pNative);

		// Token: 0x06002987 RID: 10631 RVA: 0x000D4395 File Offset: 0x000D2795
		public void Open()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("KinectSensor");
			}
			KinectSensor.Windows_Kinect_KinectSensor_Open(this._pNative);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002988 RID: 10632
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_Close(IntPtr pNative);

		// Token: 0x06002989 RID: 10633 RVA: 0x000D43C7 File Offset: 0x000D27C7
		public void Close()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("KinectSensor");
			}
			KinectSensor.Windows_Kinect_KinectSensor_Close(this._pNative);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600298A RID: 10634
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_OpenMultiSourceFrameReader(IntPtr pNative, FrameSourceTypes enabledFrameSourceTypes);

		// Token: 0x0600298B RID: 10635 RVA: 0x000D43FC File Offset: 0x000D27FC
		public MultiSourceFrameReader OpenMultiSourceFrameReader(FrameSourceTypes enabledFrameSourceTypes)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("KinectSensor");
			}
			IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_OpenMultiSourceFrameReader(this._pNative, enabledFrameSourceTypes);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<MultiSourceFrameReader>(intPtr, (IntPtr n) => new MultiSourceFrameReader(n));
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000D4470 File Offset: 0x000D2870
		private void __EventCleanup()
		{
			KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<IsAvailableChangedEventArgs>> list = KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (KinectSensor.f__mg2 == null)
						{
							KinectSensor.f__mg2 = new KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate(KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handler);
						}
						KinectSensor.Windows_Kinect_KinectSensor_add_IsAvailableChanged(pNative, KinectSensor.f__mg2, true);
					}
					KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handle.Free();
				}
			}
			KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative2 = this._pNative;
						if (KinectSensor.f__mg3 == null)
						{
							KinectSensor.f__mg3 = new KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate(KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						}
						KinectSensor.Windows_Kinect_KinectSensor_add_PropertyChanged(pNative2, KinectSensor.f__mg3, true);
					}
					KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x040016AF RID: 5807
		internal IntPtr _pNative;

		// Token: 0x040016B0 RID: 5808
		private static GCHandle _Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handle;

		// Token: 0x040016B1 RID: 5809
		private static CollectionMap<IntPtr, List<EventHandler<IsAvailableChangedEventArgs>>> Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<IsAvailableChangedEventArgs>>>();

		// Token: 0x040016B2 RID: 5810
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x040016B3 RID: 5811
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x040016BC RID: 5820
		[CompilerGenerated]
		private static KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate f__mg0;

		// Token: 0x040016BD RID: 5821
		[CompilerGenerated]
		private static KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate f__mg1;

		// Token: 0x040016C0 RID: 5824
		[CompilerGenerated]
		private static KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate f__mg2;

		// Token: 0x040016C1 RID: 5825
		[CompilerGenerated]
		private static KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate f__mg3;

		// Token: 0x02000504 RID: 1284
		// (Invoke) Token: 0x06002D22 RID: 11554
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_IsAvailableChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000505 RID: 1285
		// (Invoke) Token: 0x06002D26 RID: 11558
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
