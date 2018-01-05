using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004FA RID: 1274
	public sealed class InfraredFrameReference : INativeWrapper
	{
		// Token: 0x06002CD5 RID: 11477 RVA: 0x000DE7F0 File Offset: 0x000DCBF0
		internal InfraredFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrameReference.Windows_Kinect_InfraredFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x000DE80A File Offset: 0x000DCC0A
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000DE814 File Offset: 0x000DCC14
		~InfraredFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06002CD8 RID: 11480
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002CD9 RID: 11481
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002CDA RID: 11482 RVA: 0x000DE844 File Offset: 0x000DCC44
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrameReference>(this._pNative);
			InfraredFrameReference.Windows_Kinect_InfraredFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002CDB RID: 11483
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_InfraredFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002CDC RID: 11484 RVA: 0x000DE883 File Offset: 0x000DCC83
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)InfraredFrameReference.Windows_Kinect_InfraredFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002CDD RID: 11485
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06002CDE RID: 11486 RVA: 0x000DE8B8 File Offset: 0x000DCCB8
		public InfraredFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrameReference");
			}
			IntPtr intPtr = InfraredFrameReference.Windows_Kinect_InfraredFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<InfraredFrame>(intPtr, (IntPtr n) => new InfraredFrame(n));
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000DE92B File Offset: 0x000DCD2B
		private void __EventCleanup()
		{
		}

		// Token: 0x040017E0 RID: 6112
		internal IntPtr _pNative;
	}
}
