using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004A7 RID: 1191
	public sealed class CoordinateMapper : INativeWrapper
	{
		// Token: 0x060029B0 RID: 10672 RVA: 0x000D49E4 File Offset: 0x000D2DE4
		internal CoordinateMapper(IntPtr pNative)
		{
			this._pNative = pNative;
			CoordinateMapper.Windows_Kinect_CoordinateMapper_AddRefObject(ref this._pNative);
		}

		// Token: 0x060029B1 RID: 10673
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_GetDepthCameraIntrinsics(IntPtr pNative);

		// Token: 0x060029B2 RID: 10674 RVA: 0x000D4A00 File Offset: 0x000D2E00
		public CameraIntrinsics GetDepthCameraIntrinsics()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_GetDepthCameraIntrinsics(this._pNative);
			ExceptionHelper.CheckLastError();
			CameraIntrinsics result = (CameraIntrinsics)Marshal.PtrToStructure(intPtr, typeof(CameraIntrinsics));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x060029B3 RID: 10675
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_CoordinateMapper_GetDepthFrameToCameraSpaceTable(IntPtr pNative, IntPtr outCollection, uint outCollectionSize);

		// Token: 0x060029B4 RID: 10676 RVA: 0x000D4A5C File Offset: 0x000D2E5C
		public PointF[] GetDepthFrameToCameraSpaceTable()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			if (this._DepthFrameToCameraSpaceTable == null)
			{
				FrameDescription frameDescription = KinectSensor.GetDefault().DepthFrameSource.FrameDescription;
				this._DepthFrameToCameraSpaceTable = new PointF[frameDescription.Width * frameDescription.Height];
				SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(this._DepthFrameToCameraSpaceTable, GCHandleType.Pinned));
				IntPtr outCollection = smartGCHandle.AddrOfPinnedObject();
				CoordinateMapper.Windows_Kinect_CoordinateMapper_GetDepthFrameToCameraSpaceTable(this._pNative, outCollection, (uint)this._DepthFrameToCameraSpaceTable.Length);
				ExceptionHelper.CheckLastError();
			}
			return this._DepthFrameToCameraSpaceTable;
		}

		// Token: 0x060029B5 RID: 10677
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(IntPtr pNative, IntPtr depthFrameData, uint depthFrameDataSize, IntPtr depthSpacePoints, uint depthSpacePointsSize);

		// Token: 0x060029B6 RID: 10678 RVA: 0x000D4AF8 File Offset: 0x000D2EF8
		public void MapColorFrameToDepthSpaceUsingIntPtr(IntPtr depthFrameData, uint depthFrameSize, IntPtr depthSpacePoints, uint depthSpacePointsSize)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			uint depthFrameDataSize = depthFrameSize / 2u;
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(this._pNative, depthFrameData, depthFrameDataSize, depthSpacePoints, depthSpacePointsSize);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029B7 RID: 10679
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(IntPtr pNative, IntPtr depthFrameData, uint depthFrameDataSize, IntPtr cameraSpacePoints, uint cameraSpacePointsSize);

		// Token: 0x060029B8 RID: 10680 RVA: 0x000D4B40 File Offset: 0x000D2F40
		public void MapColorFrameToCameraSpaceUsingIntPtr(IntPtr depthFrameData, int depthFrameSize, IntPtr cameraSpacePoints, uint cameraSpacePointsSize)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			uint depthFrameDataSize = (uint)(depthFrameSize / 2);
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(this._pNative, depthFrameData, depthFrameDataSize, cameraSpacePoints, cameraSpacePointsSize);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029B9 RID: 10681
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(IntPtr pNative, IntPtr depthFrameData, uint depthFrameDataSize, IntPtr colorSpacePoints, uint colorSpacePointsSize);

		// Token: 0x060029BA RID: 10682 RVA: 0x000D4B88 File Offset: 0x000D2F88
		public void MapDepthFrameToColorSpaceUsingIntPtr(IntPtr depthFrameData, int depthFrameSize, IntPtr colorSpacePoints, uint colorSpacePointsSize)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			uint depthFrameDataSize = (uint)(depthFrameSize / 2);
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(this._pNative, depthFrameData, depthFrameDataSize, colorSpacePoints, colorSpacePointsSize);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029BB RID: 10683
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(IntPtr pNative, IntPtr depthFrameData, uint depthFrameDataSize, IntPtr cameraSpacePoints, uint cameraSpacePointsSize);

		// Token: 0x060029BC RID: 10684 RVA: 0x000D4BD0 File Offset: 0x000D2FD0
		public void MapDepthFrameToCameraSpaceUsingIntPtr(IntPtr depthFrameData, int depthFrameSize, IntPtr cameraSpacePoints, uint cameraSpacePointsSize)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			uint depthFrameDataSize = (uint)(depthFrameSize / 2);
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(this._pNative, depthFrameData, depthFrameDataSize, cameraSpacePoints, cameraSpacePointsSize);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060029BD RID: 10685 RVA: 0x000D4C16 File Offset: 0x000D3016
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000D4C20 File Offset: 0x000D3020
		~CoordinateMapper()
		{
			this.Dispose(false);
		}

		// Token: 0x060029BF RID: 10687
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060029C0 RID: 10688
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_AddRefObject(ref IntPtr pNative);

		// Token: 0x060029C1 RID: 10689 RVA: 0x000D4C50 File Offset: 0x000D3050
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<CoordinateMapper>(this._pNative);
			CoordinateMapper.Windows_Kinect_CoordinateMapper_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000D4C90 File Offset: 0x000D3090
		[AOT.MonoPInvokeCallback(typeof(CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate))]
		private static void Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<CoordinateMappingChangedEventArgs>> list = null;
			CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			object obj = list;
			lock (obj)
			{
				CoordinateMapper objThis = NativeObjectCache.GetObject<CoordinateMapper>(pNative);
				CoordinateMappingChangedEventArgs args = new CoordinateMappingChangedEventArgs(result);
				using (List<EventHandler<CoordinateMappingChangedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<CoordinateMappingChangedEventArgs> func = enumerator.Current;
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

		// Token: 0x060029C3 RID: 10691
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_add_CoordinateMappingChanged(IntPtr pNative, CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060029C4 RID: 10692 RVA: 0x000D4D5C File Offset: 0x000D315C
		// (remove) Token: 0x060029C5 RID: 10693 RVA: 0x000D4DEC File Offset: 0x000D31EC
		public event EventHandler<CoordinateMappingChangedEventArgs> CoordinateMappingChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<CoordinateMappingChangedEventArgs>> list = CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate windows_Kinect_CoordinateMappingChangedEventArgs_Delegate = new CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate(CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handler);
						CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_CoordinateMappingChangedEventArgs_Delegate);
						CoordinateMapper.Windows_Kinect_CoordinateMapper_add_CoordinateMappingChanged(this._pNative, windows_Kinect_CoordinateMappingChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<CoordinateMappingChangedEventArgs>> list = CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks[this._pNative];
				object obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						IntPtr pNative = this._pNative;
						if (CoordinateMapper.f__mg0 == null)
						{
							CoordinateMapper.f__mg0 = new CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate(CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handler);
						}
						CoordinateMapper.Windows_Kinect_CoordinateMapper_add_CoordinateMappingChanged(pNative, CoordinateMapper.f__mg0, true);
						CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060029C6 RID: 10694
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_MapCameraPointToDepthSpace(IntPtr pNative, CameraSpacePoint cameraPoint);

		// Token: 0x060029C7 RID: 10695 RVA: 0x000D4E9C File Offset: 0x000D329C
		public DepthSpacePoint MapCameraPointToDepthSpace(CameraSpacePoint cameraPoint)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_MapCameraPointToDepthSpace(this._pNative, cameraPoint);
			ExceptionHelper.CheckLastError();
			DepthSpacePoint result = (DepthSpacePoint)Marshal.PtrToStructure(intPtr, typeof(DepthSpacePoint));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x060029C8 RID: 10696
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_MapCameraPointToColorSpace(IntPtr pNative, CameraSpacePoint cameraPoint);

		// Token: 0x060029C9 RID: 10697 RVA: 0x000D4EF8 File Offset: 0x000D32F8
		public ColorSpacePoint MapCameraPointToColorSpace(CameraSpacePoint cameraPoint)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_MapCameraPointToColorSpace(this._pNative, cameraPoint);
			ExceptionHelper.CheckLastError();
			ColorSpacePoint result = (ColorSpacePoint)Marshal.PtrToStructure(intPtr, typeof(ColorSpacePoint));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x060029CA RID: 10698
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_MapDepthPointToCameraSpace(IntPtr pNative, DepthSpacePoint depthPoint, ushort depth);

		// Token: 0x060029CB RID: 10699 RVA: 0x000D4F54 File Offset: 0x000D3354
		public CameraSpacePoint MapDepthPointToCameraSpace(DepthSpacePoint depthPoint, ushort depth)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthPointToCameraSpace(this._pNative, depthPoint, depth);
			ExceptionHelper.CheckLastError();
			CameraSpacePoint result = (CameraSpacePoint)Marshal.PtrToStructure(intPtr, typeof(CameraSpacePoint));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x060029CC RID: 10700
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_MapDepthPointToColorSpace(IntPtr pNative, DepthSpacePoint depthPoint, ushort depth);

		// Token: 0x060029CD RID: 10701 RVA: 0x000D4FB4 File Offset: 0x000D33B4
		public ColorSpacePoint MapDepthPointToColorSpace(DepthSpacePoint depthPoint, ushort depth)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthPointToColorSpace(this._pNative, depthPoint, depth);
			ExceptionHelper.CheckLastError();
			ColorSpacePoint result = (ColorSpacePoint)Marshal.PtrToStructure(intPtr, typeof(ColorSpacePoint));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x060029CE RID: 10702
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapCameraPointsToDepthSpace(IntPtr pNative, IntPtr cameraPoints, int cameraPointsSize, IntPtr depthPoints, int depthPointsSize);

		// Token: 0x060029CF RID: 10703 RVA: 0x000D5014 File Offset: 0x000D3414
		public void MapCameraPointsToDepthSpace(CameraSpacePoint[] cameraPoints, DepthSpacePoint[] depthPoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(cameraPoints, GCHandleType.Pinned));
			IntPtr cameraPoints2 = smartGCHandle.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle2 = new SmartGCHandle(GCHandle.Alloc(depthPoints, GCHandleType.Pinned));
			IntPtr depthPoints2 = smartGCHandle2.AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapCameraPointsToDepthSpace(this._pNative, cameraPoints2, cameraPoints.Length, depthPoints2, depthPoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029D0 RID: 10704
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapCameraPointsToColorSpace(IntPtr pNative, IntPtr cameraPoints, int cameraPointsSize, IntPtr colorPoints, int colorPointsSize);

		// Token: 0x060029D1 RID: 10705 RVA: 0x000D5084 File Offset: 0x000D3484
		public void MapCameraPointsToColorSpace(CameraSpacePoint[] cameraPoints, ColorSpacePoint[] colorPoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(cameraPoints, GCHandleType.Pinned));
			IntPtr cameraPoints2 = smartGCHandle.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle2 = new SmartGCHandle(GCHandle.Alloc(colorPoints, GCHandleType.Pinned));
			IntPtr colorPoints2 = smartGCHandle2.AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapCameraPointsToColorSpace(this._pNative, cameraPoints2, cameraPoints.Length, colorPoints2, colorPoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029D2 RID: 10706
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthPointsToCameraSpace(IntPtr pNative, IntPtr depthPoints, int depthPointsSize, IntPtr depths, int depthsSize, IntPtr cameraPoints, int cameraPointsSize);

		// Token: 0x060029D3 RID: 10707 RVA: 0x000D50F4 File Offset: 0x000D34F4
		public void MapDepthPointsToCameraSpace(DepthSpacePoint[] depthPoints, ushort[] depths, CameraSpacePoint[] cameraPoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(depthPoints, GCHandleType.Pinned));
			IntPtr depthPoints2 = smartGCHandle.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle2 = new SmartGCHandle(GCHandle.Alloc(depths, GCHandleType.Pinned));
			IntPtr depths2 = smartGCHandle2.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle3 = new SmartGCHandle(GCHandle.Alloc(cameraPoints, GCHandleType.Pinned));
			IntPtr cameraPoints2 = smartGCHandle3.AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthPointsToCameraSpace(this._pNative, depthPoints2, depthPoints.Length, depths2, depths.Length, cameraPoints2, cameraPoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029D4 RID: 10708
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthPointsToColorSpace(IntPtr pNative, IntPtr depthPoints, int depthPointsSize, IntPtr depths, int depthsSize, IntPtr colorPoints, int colorPointsSize);

		// Token: 0x060029D5 RID: 10709 RVA: 0x000D5180 File Offset: 0x000D3580
		public void MapDepthPointsToColorSpace(DepthSpacePoint[] depthPoints, ushort[] depths, ColorSpacePoint[] colorPoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(depthPoints, GCHandleType.Pinned));
			IntPtr depthPoints2 = smartGCHandle.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle2 = new SmartGCHandle(GCHandle.Alloc(depths, GCHandleType.Pinned));
			IntPtr depths2 = smartGCHandle2.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle3 = new SmartGCHandle(GCHandle.Alloc(colorPoints, GCHandleType.Pinned));
			IntPtr colorPoints2 = smartGCHandle3.AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthPointsToColorSpace(this._pNative, depthPoints2, depthPoints.Length, depths2, depths.Length, colorPoints2, colorPoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029D6 RID: 10710
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(IntPtr pNative, IntPtr depthFrameData, int depthFrameDataSize, IntPtr cameraSpacePoints, int cameraSpacePointsSize);

		// Token: 0x060029D7 RID: 10711 RVA: 0x000D520C File Offset: 0x000D360C
		public void MapDepthFrameToCameraSpace(ushort[] depthFrameData, CameraSpacePoint[] cameraSpacePoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(depthFrameData, GCHandleType.Pinned));
			IntPtr depthFrameData2 = smartGCHandle.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle2 = new SmartGCHandle(GCHandle.Alloc(cameraSpacePoints, GCHandleType.Pinned));
			IntPtr cameraSpacePoints2 = smartGCHandle2.AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(this._pNative, depthFrameData2, depthFrameData.Length, cameraSpacePoints2, cameraSpacePoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029D8 RID: 10712
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(IntPtr pNative, IntPtr depthFrameData, int depthFrameDataSize, IntPtr colorSpacePoints, int colorSpacePointsSize);

		// Token: 0x060029D9 RID: 10713 RVA: 0x000D527C File Offset: 0x000D367C
		public void MapDepthFrameToColorSpace(ushort[] depthFrameData, ColorSpacePoint[] colorSpacePoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(depthFrameData, GCHandleType.Pinned));
			IntPtr depthFrameData2 = smartGCHandle.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle2 = new SmartGCHandle(GCHandle.Alloc(colorSpacePoints, GCHandleType.Pinned));
			IntPtr colorSpacePoints2 = smartGCHandle2.AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(this._pNative, depthFrameData2, depthFrameData.Length, colorSpacePoints2, colorSpacePoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029DA RID: 10714
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(IntPtr pNative, IntPtr depthFrameData, int depthFrameDataSize, IntPtr depthSpacePoints, int depthSpacePointsSize);

		// Token: 0x060029DB RID: 10715 RVA: 0x000D52EC File Offset: 0x000D36EC
		public void MapColorFrameToDepthSpace(ushort[] depthFrameData, DepthSpacePoint[] depthSpacePoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(depthFrameData, GCHandleType.Pinned));
			IntPtr depthFrameData2 = smartGCHandle.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle2 = new SmartGCHandle(GCHandle.Alloc(depthSpacePoints, GCHandleType.Pinned));
			IntPtr depthSpacePoints2 = smartGCHandle2.AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(this._pNative, depthFrameData2, depthFrameData.Length, depthSpacePoints2, depthSpacePoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029DC RID: 10716
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(IntPtr pNative, IntPtr depthFrameData, int depthFrameDataSize, IntPtr cameraSpacePoints, int cameraSpacePointsSize);

		// Token: 0x060029DD RID: 10717 RVA: 0x000D535C File Offset: 0x000D375C
		public void MapColorFrameToCameraSpace(ushort[] depthFrameData, CameraSpacePoint[] cameraSpacePoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(depthFrameData, GCHandleType.Pinned));
			IntPtr depthFrameData2 = smartGCHandle.AddrOfPinnedObject();
			SmartGCHandle smartGCHandle2 = new SmartGCHandle(GCHandle.Alloc(cameraSpacePoints, GCHandleType.Pinned));
			IntPtr cameraSpacePoints2 = smartGCHandle2.AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(this._pNative, depthFrameData2, depthFrameData.Length, cameraSpacePoints2, cameraSpacePoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x000D53CC File Offset: 0x000D37CC
		private void __EventCleanup()
		{
			CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<CoordinateMappingChangedEventArgs>> list = CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks[this._pNative];
			object obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						IntPtr pNative = this._pNative;
						if (CoordinateMapper.f__mg1 == null)
						{
							CoordinateMapper.f__mg1 = new CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate(CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handler);
						}
						CoordinateMapper.Windows_Kinect_CoordinateMapper_add_CoordinateMappingChanged(pNative, CoordinateMapper.f__mg1, true);
					}
					CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x040016C6 RID: 5830
		private PointF[] _DepthFrameToCameraSpaceTable;

		// Token: 0x040016C7 RID: 5831
		internal IntPtr _pNative;

		// Token: 0x040016C8 RID: 5832
		private static GCHandle _Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handle;

		// Token: 0x040016C9 RID: 5833
		private static CollectionMap<IntPtr, List<EventHandler<CoordinateMappingChangedEventArgs>>> Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<CoordinateMappingChangedEventArgs>>>();

		// Token: 0x040016CA RID: 5834
		[CompilerGenerated]
		private static CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate f__mg0;

		// Token: 0x040016CB RID: 5835
		[CompilerGenerated]
		private static CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate f__mg1;

		// Token: 0x020004E3 RID: 1251
		// (Invoke) Token: 0x06002C11 RID: 11281
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
