using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004D8 RID: 1240
	public sealed class ColorCameraSettings : INativeWrapper
	{
		// Token: 0x06002B98 RID: 11160 RVA: 0x000DB100 File Offset: 0x000D9500
		internal ColorCameraSettings(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorCameraSettings.Windows_Kinect_ColorCameraSettings_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002B99 RID: 11161 RVA: 0x000DB11A File Offset: 0x000D951A
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x000DB124 File Offset: 0x000D9524
		~ColorCameraSettings()
		{
			this.Dispose(false);
		}

		// Token: 0x06002B9B RID: 11163
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorCameraSettings_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002B9C RID: 11164
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorCameraSettings_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002B9D RID: 11165 RVA: 0x000DB154 File Offset: 0x000D9554
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorCameraSettings>(this._pNative);
			ColorCameraSettings.Windows_Kinect_ColorCameraSettings_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002B9E RID: 11166
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_ColorCameraSettings_get_ExposureTime(IntPtr pNative);

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002B9F RID: 11167 RVA: 0x000DB193 File Offset: 0x000D9593
		public TimeSpan ExposureTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorCameraSettings");
				}
				return TimeSpan.FromMilliseconds((double)ColorCameraSettings.Windows_Kinect_ColorCameraSettings_get_ExposureTime(this._pNative));
			}
		}

		// Token: 0x06002BA0 RID: 11168
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_ColorCameraSettings_get_FrameInterval(IntPtr pNative);

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x000DB1C6 File Offset: 0x000D95C6
		public TimeSpan FrameInterval
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorCameraSettings");
				}
				return TimeSpan.FromMilliseconds((double)ColorCameraSettings.Windows_Kinect_ColorCameraSettings_get_FrameInterval(this._pNative));
			}
		}

		// Token: 0x06002BA2 RID: 11170
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_ColorCameraSettings_get_Gain(IntPtr pNative);

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002BA3 RID: 11171 RVA: 0x000DB1F9 File Offset: 0x000D95F9
		public float Gain
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorCameraSettings");
				}
				return ColorCameraSettings.Windows_Kinect_ColorCameraSettings_get_Gain(this._pNative);
			}
		}

		// Token: 0x06002BA4 RID: 11172
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_ColorCameraSettings_get_Gamma(IntPtr pNative);

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002BA5 RID: 11173 RVA: 0x000DB226 File Offset: 0x000D9626
		public float Gamma
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorCameraSettings");
				}
				return ColorCameraSettings.Windows_Kinect_ColorCameraSettings_get_Gamma(this._pNative);
			}
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000DB253 File Offset: 0x000D9653
		private void __EventCleanup()
		{
		}

		// Token: 0x0400176C RID: 5996
		internal IntPtr _pNative;
	}
}
