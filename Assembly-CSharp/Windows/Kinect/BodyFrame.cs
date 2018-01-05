using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200049F RID: 1183
	public sealed class BodyFrame : IDisposable, INativeWrapper
	{
		// Token: 0x060028A9 RID: 10409 RVA: 0x000D2387 File Offset: 0x000D0787
		internal BodyFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrame.Windows_Kinect_BodyFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x060028AA RID: 10410
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrame_GetAndRefreshBodyData(IntPtr pNative, [Out] IntPtr[] bodies, int bodiesSize);

		// Token: 0x060028AB RID: 10411 RVA: 0x000D23A4 File Offset: 0x000D07A4
		public void GetAndRefreshBodyData(IList<Body> bodies)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrame");
			}
			int num = 0;
			IntPtr[] array = new IntPtr[bodies.Count];
			for (int i = 0; i < bodies.Count; i++)
			{
				if (bodies[i] == null)
				{
					bodies[i] = new Body();
				}
				array[num] = bodies[i].GetIntPtr();
				num++;
			}
			BodyFrame.Windows_Kinect_BodyFrame_GetAndRefreshBodyData(this._pNative, array, bodies.Count);
			ExceptionHelper.CheckLastError();
			for (int j = 0; j < bodies.Count; j++)
			{
				bodies[j].SetIntPtr(array[j]);
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x000D2463 File Offset: 0x000D0863
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000D246C File Offset: 0x000D086C
		~BodyFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x060028AE RID: 10414
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060028AF RID: 10415
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x060028B0 RID: 10416 RVA: 0x000D249C File Offset: 0x000D089C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrame>(this._pNative);
			if (disposing)
			{
				BodyFrame.Windows_Kinect_BodyFrame_Dispose(this._pNative);
			}
			BodyFrame.Windows_Kinect_BodyFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060028B1 RID: 10417
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_BodyFrame_get_BodyCount(IntPtr pNative);

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x000D24F7 File Offset: 0x000D08F7
		public int BodyCount
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrame");
				}
				return BodyFrame.Windows_Kinect_BodyFrame_get_BodyCount(this._pNative);
			}
		}

		// Token: 0x060028B3 RID: 10419
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrame_get_BodyFrameSource(IntPtr pNative);

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060028B4 RID: 10420 RVA: 0x000D2524 File Offset: 0x000D0924
		public BodyFrameSource BodyFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrame");
				}
				IntPtr intPtr = BodyFrame.Windows_Kinect_BodyFrame_get_BodyFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameSource>(intPtr, (IntPtr n) => new BodyFrameSource(n));
			}
		}

		// Token: 0x060028B5 RID: 10421
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrame_get_FloorClipPlane(IntPtr pNative);

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x000D2598 File Offset: 0x000D0998
		public Vector4 FloorClipPlane
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrame");
				}
				IntPtr intPtr = BodyFrame.Windows_Kinect_BodyFrame_get_FloorClipPlane(this._pNative);
				ExceptionHelper.CheckLastError();
				Vector4 result = (Vector4)Marshal.PtrToStructure(intPtr, typeof(Vector4));
				KinectUnityAddinUtils.FreeMemory(intPtr);
				return result;
			}
		}

		// Token: 0x060028B7 RID: 10423
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_BodyFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060028B8 RID: 10424 RVA: 0x000D25F3 File Offset: 0x000D09F3
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrame");
				}
				return TimeSpan.FromMilliseconds((double)BodyFrame.Windows_Kinect_BodyFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x060028B9 RID: 10425
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrame_Dispose(IntPtr pNative);

		// Token: 0x060028BA RID: 10426 RVA: 0x000D2626 File Offset: 0x000D0A26
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000D264B File Offset: 0x000D0A4B
		private void __EventCleanup()
		{
		}

		// Token: 0x0400169A RID: 5786
		internal IntPtr _pNative;
	}
}
