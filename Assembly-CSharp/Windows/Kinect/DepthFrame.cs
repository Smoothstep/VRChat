using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004A2 RID: 1186
	public sealed class DepthFrame : IDisposable, INativeWrapper
	{
		// Token: 0x06002912 RID: 10514 RVA: 0x000D3097 File Offset: 0x000D1497
		internal DepthFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrame.Windows_Kinect_DepthFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x06002913 RID: 10515
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_DepthFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x06002914 RID: 10516 RVA: 0x000D30B1 File Offset: 0x000D14B1
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrame");
			}
			DepthFrame.Windows_Kinect_DepthFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size / 2u);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002915 RID: 10517
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrame_LockImageBuffer(IntPtr pNative);

		// Token: 0x06002916 RID: 10518 RVA: 0x000D30E8 File Offset: 0x000D14E8
		public KinectBuffer LockImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrame");
			}
			IntPtr intPtr = DepthFrame.Windows_Kinect_DepthFrame_LockImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002917 RID: 10519 RVA: 0x000D315B File Offset: 0x000D155B
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x000D3164 File Offset: 0x000D1564
		~DepthFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x06002919 RID: 10521
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600291A RID: 10522
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600291B RID: 10523 RVA: 0x000D3194 File Offset: 0x000D1594
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrame>(this._pNative);
			if (disposing)
			{
				DepthFrame.Windows_Kinect_DepthFrame_Dispose(this._pNative);
			}
			DepthFrame.Windows_Kinect_DepthFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600291C RID: 10524
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrame_get_DepthFrameSource(IntPtr pNative);

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600291D RID: 10525 RVA: 0x000D31F0 File Offset: 0x000D15F0
		public DepthFrameSource DepthFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				IntPtr intPtr = DepthFrame.Windows_Kinect_DepthFrame_get_DepthFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameSource>(intPtr, (IntPtr n) => new DepthFrameSource(n));
			}
		}

		// Token: 0x0600291E RID: 10526
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ushort Windows_Kinect_DepthFrame_get_DepthMaxReliableDistance(IntPtr pNative);

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x000D3263 File Offset: 0x000D1663
		public ushort DepthMaxReliableDistance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				return DepthFrame.Windows_Kinect_DepthFrame_get_DepthMaxReliableDistance(this._pNative);
			}
		}

		// Token: 0x06002920 RID: 10528
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ushort Windows_Kinect_DepthFrame_get_DepthMinReliableDistance(IntPtr pNative);

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x000D3290 File Offset: 0x000D1690
		public ushort DepthMinReliableDistance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				return DepthFrame.Windows_Kinect_DepthFrame_get_DepthMinReliableDistance(this._pNative);
			}
		}

		// Token: 0x06002922 RID: 10530
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x000D32C0 File Offset: 0x000D16C0
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				IntPtr intPtr = DepthFrame.Windows_Kinect_DepthFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06002924 RID: 10532
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_DepthFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x000D3333 File Offset: 0x000D1733
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				return TimeSpan.FromMilliseconds((double)DepthFrame.Windows_Kinect_DepthFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002926 RID: 10534
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x06002927 RID: 10535 RVA: 0x000D3368 File Offset: 0x000D1768
		public void CopyFrameDataToArray(ushort[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrame");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned));
			IntPtr frameData2 = smartGCHandle.AddrOfPinnedObject();
			DepthFrame.Windows_Kinect_DepthFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002928 RID: 10536
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_Dispose(IntPtr pNative);

		// Token: 0x06002929 RID: 10537 RVA: 0x000D33BD File Offset: 0x000D17BD
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000D33E2 File Offset: 0x000D17E2
		private void __EventCleanup()
		{
		}

		// Token: 0x040016A3 RID: 5795
		internal IntPtr _pNative;
	}
}
