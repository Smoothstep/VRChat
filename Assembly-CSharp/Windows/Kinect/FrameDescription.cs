using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004F2 RID: 1266
	public sealed class FrameDescription : INativeWrapper
	{
		// Token: 0x06002C92 RID: 11410 RVA: 0x000DDCB2 File Offset: 0x000DC0B2
		internal FrameDescription(IntPtr pNative)
		{
			this._pNative = pNative;
			FrameDescription.Windows_Kinect_FrameDescription_AddRefObject(ref this._pNative);
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002C93 RID: 11411 RVA: 0x000DDCCC File Offset: 0x000DC0CC
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x000DDCD4 File Offset: 0x000DC0D4
		~FrameDescription()
		{
			this.Dispose(false);
		}

		// Token: 0x06002C95 RID: 11413
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_FrameDescription_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002C96 RID: 11414
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_FrameDescription_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002C97 RID: 11415 RVA: 0x000DDD04 File Offset: 0x000DC104
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<FrameDescription>(this._pNative);
			FrameDescription.Windows_Kinect_FrameDescription_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002C98 RID: 11416
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_FrameDescription_get_BytesPerPixel(IntPtr pNative);

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002C99 RID: 11417 RVA: 0x000DDD43 File Offset: 0x000DC143
		public uint BytesPerPixel
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_BytesPerPixel(this._pNative);
			}
		}

		// Token: 0x06002C9A RID: 11418
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_FrameDescription_get_DiagonalFieldOfView(IntPtr pNative);

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002C9B RID: 11419 RVA: 0x000DDD70 File Offset: 0x000DC170
		public float DiagonalFieldOfView
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_DiagonalFieldOfView(this._pNative);
			}
		}

		// Token: 0x06002C9C RID: 11420
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_FrameDescription_get_Height(IntPtr pNative);

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002C9D RID: 11421 RVA: 0x000DDD9D File Offset: 0x000DC19D
		public int Height
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_Height(this._pNative);
			}
		}

		// Token: 0x06002C9E RID: 11422
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_FrameDescription_get_HorizontalFieldOfView(IntPtr pNative);

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002C9F RID: 11423 RVA: 0x000DDDCA File Offset: 0x000DC1CA
		public float HorizontalFieldOfView
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_HorizontalFieldOfView(this._pNative);
			}
		}

		// Token: 0x06002CA0 RID: 11424
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_FrameDescription_get_LengthInPixels(IntPtr pNative);

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002CA1 RID: 11425 RVA: 0x000DDDF7 File Offset: 0x000DC1F7
		public uint LengthInPixels
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_LengthInPixels(this._pNative);
			}
		}

		// Token: 0x06002CA2 RID: 11426
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_FrameDescription_get_VerticalFieldOfView(IntPtr pNative);

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x000DDE24 File Offset: 0x000DC224
		public float VerticalFieldOfView
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_VerticalFieldOfView(this._pNative);
			}
		}

		// Token: 0x06002CA4 RID: 11428
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_FrameDescription_get_Width(IntPtr pNative);

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002CA5 RID: 11429 RVA: 0x000DDE51 File Offset: 0x000DC251
		public int Width
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_Width(this._pNative);
			}
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000DDE7E File Offset: 0x000DC27E
		private void __EventCleanup()
		{
		}

		// Token: 0x040017BD RID: 6077
		internal IntPtr _pNative;
	}
}
