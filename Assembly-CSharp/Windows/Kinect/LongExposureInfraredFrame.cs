using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004A6 RID: 1190
	public sealed class LongExposureInfraredFrame : IDisposable, INativeWrapper
	{
		// Token: 0x06002998 RID: 10648 RVA: 0x000D46DC File Offset: 0x000D2ADC
		internal LongExposureInfraredFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x06002999 RID: 10649
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x0600299A RID: 10650 RVA: 0x000D46F6 File Offset: 0x000D2AF6
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrame");
			}
			LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size / 2u);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600299B RID: 10651
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrame_LockImageBuffer(IntPtr pNative);

		// Token: 0x0600299C RID: 10652 RVA: 0x000D472C File Offset: 0x000D2B2C
		public KinectBuffer LockImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrame");
			}
			IntPtr intPtr = LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_LockImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600299D RID: 10653 RVA: 0x000D479F File Offset: 0x000D2B9F
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000D47A8 File Offset: 0x000D2BA8
		~LongExposureInfraredFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x0600299F RID: 10655
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060029A0 RID: 10656
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x060029A1 RID: 10657 RVA: 0x000D47D8 File Offset: 0x000D2BD8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrame>(this._pNative);
			if (disposing)
			{
				LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_Dispose(this._pNative);
			}
			LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060029A2 RID: 10658
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060029A3 RID: 10659 RVA: 0x000D4834 File Offset: 0x000D2C34
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrame");
				}
				IntPtr intPtr = LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x060029A4 RID: 10660
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrame_get_LongExposureInfraredFrameSource(IntPtr pNative);

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060029A5 RID: 10661 RVA: 0x000D48A8 File Offset: 0x000D2CA8
		public LongExposureInfraredFrameSource LongExposureInfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrame");
				}
				IntPtr intPtr = LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_get_LongExposureInfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameSource>(intPtr, (IntPtr n) => new LongExposureInfraredFrameSource(n));
			}
		}

		// Token: 0x060029A6 RID: 10662
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_LongExposureInfraredFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060029A7 RID: 10663 RVA: 0x000D491B File Offset: 0x000D2D1B
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrame");
				}
				return TimeSpan.FromMilliseconds((double)LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x060029A8 RID: 10664
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x060029A9 RID: 10665 RVA: 0x000D4950 File Offset: 0x000D2D50
		public void CopyFrameDataToArray(ushort[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrame");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned));
			IntPtr frameData2 = smartGCHandle.AddrOfPinnedObject();
			LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060029AA RID: 10666
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_Dispose(IntPtr pNative);

		// Token: 0x060029AB RID: 10667 RVA: 0x000D49A5 File Offset: 0x000D2DA5
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x000D49CA File Offset: 0x000D2DCA
		private void __EventCleanup()
		{
		}

		// Token: 0x040016C2 RID: 5826
		internal IntPtr _pNative;
	}
}
