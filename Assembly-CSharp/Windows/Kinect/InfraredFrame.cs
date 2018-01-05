using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004A4 RID: 1188
	public sealed class InfraredFrame : IDisposable, INativeWrapper
	{
		// Token: 0x06002946 RID: 10566 RVA: 0x000D3704 File Offset: 0x000D1B04
		internal InfraredFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrame.Windows_Kinect_InfraredFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x06002947 RID: 10567
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_InfraredFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x06002948 RID: 10568 RVA: 0x000D371E File Offset: 0x000D1B1E
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrame");
			}
			InfraredFrame.Windows_Kinect_InfraredFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size / 2u);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002949 RID: 10569
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrame_LockImageBuffer(IntPtr pNative);

		// Token: 0x0600294A RID: 10570 RVA: 0x000D3754 File Offset: 0x000D1B54
		public KinectBuffer LockImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrame");
			}
			IntPtr intPtr = InfraredFrame.Windows_Kinect_InfraredFrame_LockImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600294B RID: 10571 RVA: 0x000D37C7 File Offset: 0x000D1BC7
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000D37D0 File Offset: 0x000D1BD0
		~InfraredFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x0600294D RID: 10573
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600294E RID: 10574
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600294F RID: 10575 RVA: 0x000D3800 File Offset: 0x000D1C00
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrame>(this._pNative);
			if (disposing)
			{
				InfraredFrame.Windows_Kinect_InfraredFrame_Dispose(this._pNative);
			}
			InfraredFrame.Windows_Kinect_InfraredFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002950 RID: 10576
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002951 RID: 10577 RVA: 0x000D385C File Offset: 0x000D1C5C
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrame");
				}
				IntPtr intPtr = InfraredFrame.Windows_Kinect_InfraredFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06002952 RID: 10578
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrame_get_InfraredFrameSource(IntPtr pNative);

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002953 RID: 10579 RVA: 0x000D38D0 File Offset: 0x000D1CD0
		public InfraredFrameSource InfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrame");
				}
				IntPtr intPtr = InfraredFrame.Windows_Kinect_InfraredFrame_get_InfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameSource>(intPtr, (IntPtr n) => new InfraredFrameSource(n));
			}
		}

		// Token: 0x06002954 RID: 10580
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_InfraredFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002955 RID: 10581 RVA: 0x000D3943 File Offset: 0x000D1D43
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrame");
				}
				return TimeSpan.FromMilliseconds((double)InfraredFrame.Windows_Kinect_InfraredFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002956 RID: 10582
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x06002957 RID: 10583 RVA: 0x000D3978 File Offset: 0x000D1D78
		public void CopyFrameDataToArray(ushort[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrame");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned));
			IntPtr frameData2 = smartGCHandle.AddrOfPinnedObject();
			InfraredFrame.Windows_Kinect_InfraredFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002958 RID: 10584
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_Dispose(IntPtr pNative);

		// Token: 0x06002959 RID: 10585 RVA: 0x000D39CD File Offset: 0x000D1DCD
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000D39F2 File Offset: 0x000D1DF2
		private void __EventCleanup()
		{
		}

		// Token: 0x040016AB RID: 5803
		internal IntPtr _pNative;
	}
}
