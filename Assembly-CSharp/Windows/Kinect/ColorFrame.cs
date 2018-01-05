using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004A1 RID: 1185
	public sealed class ColorFrame : IDisposable, INativeWrapper
	{
		// Token: 0x060028EE RID: 10478 RVA: 0x000D2BDA File Offset: 0x000D0FDA
		internal ColorFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrame.Windows_Kinect_ColorFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x060028EF RID: 10479
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_ColorFrame_CopyRawFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_CopyRawFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x060028F0 RID: 10480 RVA: 0x000D2BF4 File Offset: 0x000D0FF4
		public void CopyRawFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			ColorFrame.Windows_Kinect_ColorFrame_CopyRawFrameDataToIntPtr(this._pNative, frameData, size);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060028F1 RID: 10481
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_ColorFrame_CopyConvertedFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_CopyConvertedFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize, ColorImageFormat colorFormat);

		// Token: 0x060028F2 RID: 10482 RVA: 0x000D2C28 File Offset: 0x000D1028
		public void CopyConvertedFrameDataToIntPtr(IntPtr frameData, uint size, ColorImageFormat colorFormat)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			ColorFrame.Windows_Kinect_ColorFrame_CopyConvertedFrameDataToIntPtr(this._pNative, frameData, size, colorFormat);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060028F3 RID: 10483
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_LockRawImageBuffer(IntPtr pNative);

		// Token: 0x060028F4 RID: 10484 RVA: 0x000D2C60 File Offset: 0x000D1060
		public KinectBuffer LockRawImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_LockRawImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060028F5 RID: 10485 RVA: 0x000D2CD3 File Offset: 0x000D10D3
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x000D2CDC File Offset: 0x000D10DC
		~ColorFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x060028F7 RID: 10487
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060028F8 RID: 10488
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x060028F9 RID: 10489 RVA: 0x000D2D0C File Offset: 0x000D110C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrame>(this._pNative);
			if (disposing)
			{
				ColorFrame.Windows_Kinect_ColorFrame_Dispose(this._pNative);
			}
			ColorFrame.Windows_Kinect_ColorFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060028FA RID: 10490
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_get_ColorCameraSettings(IntPtr pNative);

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x060028FB RID: 10491 RVA: 0x000D2D68 File Offset: 0x000D1168
		public ColorCameraSettings ColorCameraSettings
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_get_ColorCameraSettings(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorCameraSettings>(intPtr, (IntPtr n) => new ColorCameraSettings(n));
			}
		}

		// Token: 0x060028FC RID: 10492
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_get_ColorFrameSource(IntPtr pNative);

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060028FD RID: 10493 RVA: 0x000D2DDC File Offset: 0x000D11DC
		public ColorFrameSource ColorFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_get_ColorFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameSource>(intPtr, (IntPtr n) => new ColorFrameSource(n));
			}
		}

		// Token: 0x060028FE RID: 10494
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x060028FF RID: 10495 RVA: 0x000D2E50 File Offset: 0x000D1250
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06002900 RID: 10496
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ColorImageFormat Windows_Kinect_ColorFrame_get_RawColorImageFormat(IntPtr pNative);

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002901 RID: 10497 RVA: 0x000D2EC3 File Offset: 0x000D12C3
		public ColorImageFormat RawColorImageFormat
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				return ColorFrame.Windows_Kinect_ColorFrame_get_RawColorImageFormat(this._pNative);
			}
		}

		// Token: 0x06002902 RID: 10498
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_ColorFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002903 RID: 10499 RVA: 0x000D2EF0 File Offset: 0x000D12F0
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				return TimeSpan.FromMilliseconds((double)ColorFrame.Windows_Kinect_ColorFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002904 RID: 10500
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_CopyRawFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x06002905 RID: 10501 RVA: 0x000D2F24 File Offset: 0x000D1324
		public void CopyRawFrameDataToArray(byte[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned));
			IntPtr frameData2 = smartGCHandle.AddrOfPinnedObject();
			ColorFrame.Windows_Kinect_ColorFrame_CopyRawFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002906 RID: 10502
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_CopyConvertedFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize, ColorImageFormat colorFormat);

		// Token: 0x06002907 RID: 10503 RVA: 0x000D2F7C File Offset: 0x000D137C
		public void CopyConvertedFrameDataToArray(byte[] frameData, ColorImageFormat colorFormat)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned));
			IntPtr frameData2 = smartGCHandle.AddrOfPinnedObject();
			ColorFrame.Windows_Kinect_ColorFrame_CopyConvertedFrameDataToArray(this._pNative, frameData2, frameData.Length, colorFormat);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002908 RID: 10504
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_CreateFrameDescription(IntPtr pNative, ColorImageFormat format);

		// Token: 0x06002909 RID: 10505 RVA: 0x000D2FD4 File Offset: 0x000D13D4
		public FrameDescription CreateFrameDescription(ColorImageFormat format)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_CreateFrameDescription(this._pNative, format);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
		}

		// Token: 0x0600290A RID: 10506
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_Dispose(IntPtr pNative);

		// Token: 0x0600290B RID: 10507 RVA: 0x000D3048 File Offset: 0x000D1448
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000D306D File Offset: 0x000D146D
		private void __EventCleanup()
		{
		}

		// Token: 0x0400169D RID: 5789
		internal IntPtr _pNative;
	}
}
