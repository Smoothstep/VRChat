using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x020004A3 RID: 1187
	public sealed class BodyIndexFrame : IDisposable, INativeWrapper
	{
		// Token: 0x0600292E RID: 10542 RVA: 0x000D33FC File Offset: 0x000D17FC
		internal BodyIndexFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrame.Windows_Kinect_BodyIndexFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600292F RID: 10543
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_BodyIndexFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x06002930 RID: 10544 RVA: 0x000D3416 File Offset: 0x000D1816
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrame");
			}
			BodyIndexFrame.Windows_Kinect_BodyIndexFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002931 RID: 10545
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrame_LockImageBuffer(IntPtr pNative);

		// Token: 0x06002932 RID: 10546 RVA: 0x000D344C File Offset: 0x000D184C
		public KinectBuffer LockImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrame");
			}
			IntPtr intPtr = BodyIndexFrame.Windows_Kinect_BodyIndexFrame_LockImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002933 RID: 10547 RVA: 0x000D34BF File Offset: 0x000D18BF
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000D34C8 File Offset: 0x000D18C8
		~BodyIndexFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x06002935 RID: 10549
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06002936 RID: 10550
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x06002937 RID: 10551 RVA: 0x000D34F8 File Offset: 0x000D18F8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrame>(this._pNative);
			if (disposing)
			{
				BodyIndexFrame.Windows_Kinect_BodyIndexFrame_Dispose(this._pNative);
			}
			BodyIndexFrame.Windows_Kinect_BodyIndexFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002938 RID: 10552
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrame_get_BodyIndexFrameSource(IntPtr pNative);

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002939 RID: 10553 RVA: 0x000D3554 File Offset: 0x000D1954
		public BodyIndexFrameSource BodyIndexFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrame");
				}
				IntPtr intPtr = BodyIndexFrame.Windows_Kinect_BodyIndexFrame_get_BodyIndexFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameSource>(intPtr, (IntPtr n) => new BodyIndexFrameSource(n));
			}
		}

		// Token: 0x0600293A RID: 10554
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x0600293B RID: 10555 RVA: 0x000D35C8 File Offset: 0x000D19C8
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrame");
				}
				IntPtr intPtr = BodyIndexFrame.Windows_Kinect_BodyIndexFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x0600293C RID: 10556
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_BodyIndexFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x000D363B File Offset: 0x000D1A3B
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrame");
				}
				return TimeSpan.FromMilliseconds((double)BodyIndexFrame.Windows_Kinect_BodyIndexFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x0600293E RID: 10558
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x0600293F RID: 10559 RVA: 0x000D3670 File Offset: 0x000D1A70
		public void CopyFrameDataToArray(byte[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrame");
			}
			SmartGCHandle smartGCHandle = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned));
			IntPtr frameData2 = smartGCHandle.AddrOfPinnedObject();
			BodyIndexFrame.Windows_Kinect_BodyIndexFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06002940 RID: 10560
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_Dispose(IntPtr pNative);

		// Token: 0x06002941 RID: 10561 RVA: 0x000D36C5 File Offset: 0x000D1AC5
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x000D36EA File Offset: 0x000D1AEA
		private void __EventCleanup()
		{
		}

		// Token: 0x040016A7 RID: 5799
		internal IntPtr _pNative;
	}
}
