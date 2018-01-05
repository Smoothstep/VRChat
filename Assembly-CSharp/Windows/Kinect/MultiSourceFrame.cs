using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200050F RID: 1295
	public sealed class MultiSourceFrame : INativeWrapper
	{
		// Token: 0x06002D89 RID: 11657 RVA: 0x000E0784 File Offset: 0x000DEB84
		internal MultiSourceFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			MultiSourceFrame.Windows_Kinect_MultiSourceFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002D8A RID: 11658 RVA: 0x000E079E File Offset: 0x000DEB9E
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000E07A8 File Offset: 0x000DEBA8
		~MultiSourceFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D8C RID: 11660
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002D8D RID: 11661
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002D8E RID: 11662 RVA: 0x000E07D8 File Offset: 0x000DEBD8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<MultiSourceFrame>(this._pNative);
			MultiSourceFrame.Windows_Kinect_MultiSourceFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002D8F RID: 11663
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_BodyFrameReference(IntPtr pNative);

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002D90 RID: 11664 RVA: 0x000E0818 File Offset: 0x000DEC18
		public BodyFrameReference BodyFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_BodyFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameReference>(intPtr, (IntPtr n) => new BodyFrameReference(n));
			}
		}

		// Token: 0x06002D91 RID: 11665
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_BodyIndexFrameReference(IntPtr pNative);

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002D92 RID: 11666 RVA: 0x000E088C File Offset: 0x000DEC8C
		public BodyIndexFrameReference BodyIndexFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_BodyIndexFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameReference>(intPtr, (IntPtr n) => new BodyIndexFrameReference(n));
			}
		}

		// Token: 0x06002D93 RID: 11667
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_ColorFrameReference(IntPtr pNative);

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002D94 RID: 11668 RVA: 0x000E0900 File Offset: 0x000DED00
		public ColorFrameReference ColorFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_ColorFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameReference>(intPtr, (IntPtr n) => new ColorFrameReference(n));
			}
		}

		// Token: 0x06002D95 RID: 11669
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_DepthFrameReference(IntPtr pNative);

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002D96 RID: 11670 RVA: 0x000E0974 File Offset: 0x000DED74
		public DepthFrameReference DepthFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_DepthFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameReference>(intPtr, (IntPtr n) => new DepthFrameReference(n));
			}
		}

		// Token: 0x06002D97 RID: 11671
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_InfraredFrameReference(IntPtr pNative);

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002D98 RID: 11672 RVA: 0x000E09E8 File Offset: 0x000DEDE8
		public InfraredFrameReference InfraredFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_InfraredFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameReference>(intPtr, (IntPtr n) => new InfraredFrameReference(n));
			}
		}

		// Token: 0x06002D99 RID: 11673
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_LongExposureInfraredFrameReference(IntPtr pNative);

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002D9A RID: 11674 RVA: 0x000E0A5C File Offset: 0x000DEE5C
		public LongExposureInfraredFrameReference LongExposureInfraredFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_LongExposureInfraredFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameReference>(intPtr, (IntPtr n) => new LongExposureInfraredFrameReference(n));
			}
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000E0ACF File Offset: 0x000DEECF
		private void __EventCleanup()
		{
		}

		// Token: 0x04001834 RID: 6196
		internal IntPtr _pNative;
	}
}
