using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004DD RID: 1245
	public sealed class ColorFrameReference : INativeWrapper
	{
		// Token: 0x06002BD5 RID: 11221 RVA: 0x000DBBC4 File Offset: 0x000D9FC4
		internal ColorFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrameReference.Windows_Kinect_ColorFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x000DBBDE File Offset: 0x000D9FDE
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000DBBE8 File Offset: 0x000D9FE8
		~ColorFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06002BD8 RID: 11224
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002BD9 RID: 11225
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002BDA RID: 11226 RVA: 0x000DBC18 File Offset: 0x000DA018
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrameReference>(this._pNative);
			ColorFrameReference.Windows_Kinect_ColorFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002BDB RID: 11227
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_ColorFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002BDC RID: 11228 RVA: 0x000DBC57 File Offset: 0x000DA057
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)ColorFrameReference.Windows_Kinect_ColorFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06002BDD RID: 11229
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06002BDE RID: 11230 RVA: 0x000DBC8C File Offset: 0x000DA08C
		public ColorFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrameReference");
			}
			IntPtr intPtr = ColorFrameReference.Windows_Kinect_ColorFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<ColorFrame>(intPtr, (IntPtr n) => new ColorFrame(n));
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000DBCFF File Offset: 0x000DA0FF
		private void __EventCleanup()
		{
		}

		// Token: 0x0400177A RID: 6010
		internal IntPtr _pNative;
	}
}
