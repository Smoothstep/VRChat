using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004CB RID: 1227
	public sealed class BodyFrameReference : INativeWrapper
	{
		// Token: 0x06002AFE RID: 11006 RVA: 0x000D9310 File Offset: 0x000D7710
		internal BodyFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrameReference.Windows_Kinect_BodyFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06002AFF RID: 11007 RVA: 0x000D932A File Offset: 0x000D772A
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000D9334 File Offset: 0x000D7734
		~BodyFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06002B01 RID: 11009
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002B02 RID: 11010
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002B03 RID: 11011 RVA: 0x000D9364 File Offset: 0x000D7764
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrameReference>(this._pNative);
			BodyFrameReference.Windows_Kinect_BodyFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002B04 RID: 11012
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_BodyFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06002B05 RID: 11013 RVA: 0x000D93A3 File Offset: 0x000D77A3
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)BodyFrameReference.Windows_Kinect_BodyFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002B06 RID: 11014
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06002B07 RID: 11015 RVA: 0x000D93D8 File Offset: 0x000D77D8
		public BodyFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameReference");
			}
			IntPtr intPtr = BodyFrameReference.Windows_Kinect_BodyFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyFrame>(intPtr, (IntPtr n) => new BodyFrame(n));
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000D944B File Offset: 0x000D784B
		private void __EventCleanup()
		{
		}

		// Token: 0x04001741 RID: 5953
		internal IntPtr _pNative;
	}
}
