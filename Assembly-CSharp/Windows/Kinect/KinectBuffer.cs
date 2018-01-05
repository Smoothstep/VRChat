using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200049B RID: 1179
	public class KinectBuffer : INativeWrapper, IDisposable
	{
		// Token: 0x0600285A RID: 10330 RVA: 0x000D1A10 File Offset: 0x000CFE10
		internal KinectBuffer(IntPtr pNative)
		{
			this._pNative = pNative;
			KinectBuffer.Windows_Storage_Streams_IBuffer_AddRefObject(ref this._pNative);
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x0600285B RID: 10331 RVA: 0x000D1A2A File Offset: 0x000CFE2A
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x000D1A34 File Offset: 0x000CFE34
		~KinectBuffer()
		{
			this.Dispose(false);
		}

		// Token: 0x0600285D RID: 10333
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Windows_Storage_Streams_IBuffer_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600285E RID: 10334
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Windows_Storage_Streams_IBuffer_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600285F RID: 10335 RVA: 0x000D1A64 File Offset: 0x000CFE64
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			NativeObjectCache.RemoveObject<KinectBuffer>(this._pNative);
			if (disposing)
			{
				KinectBuffer.Windows_Storage_Streams_IBuffer_Dispose(this._pNative);
			}
			KinectBuffer.Windows_Storage_Streams_IBuffer_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06002860 RID: 10336
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Storage_Streams_IBuffer_get_Capacity(IntPtr pNative);

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002861 RID: 10337 RVA: 0x000D1ABC File Offset: 0x000CFEBC
		public uint Capacity
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectBuffer");
				}
				uint result = KinectBuffer.Windows_Storage_Streams_IBuffer_get_Capacity(this._pNative);
				ExceptionHelper.CheckLastError();
				return result;
			}
		}

		// Token: 0x06002862 RID: 10338
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Storage_Streams_IBuffer_get_Length(IntPtr pNative);

		// Token: 0x06002863 RID: 10339
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Storage_Streams_IBuffer_put_Length(IntPtr pNative, uint value);

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06002864 RID: 10340 RVA: 0x000D1AFC File Offset: 0x000CFEFC
		// (set) Token: 0x06002865 RID: 10341 RVA: 0x000D1B3B File Offset: 0x000CFF3B
		public uint Length
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectBuffer");
				}
				uint result = KinectBuffer.Windows_Storage_Streams_IBuffer_get_Length(this._pNative);
				ExceptionHelper.CheckLastError();
				return result;
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectBuffer");
				}
				KinectBuffer.Windows_Storage_Streams_IBuffer_put_Length(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06002866 RID: 10342
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Windows_Storage_Streams_IBuffer_Dispose(IntPtr pNative);

		// Token: 0x06002867 RID: 10343 RVA: 0x000D1B6E File Offset: 0x000CFF6E
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("KinectBuffer");
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002868 RID: 10344
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Storage_Streams_IBuffer_get_UnderlyingBuffer(IntPtr pNative);

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06002869 RID: 10345 RVA: 0x000D1BA0 File Offset: 0x000CFFA0
		public IntPtr UnderlyingBuffer
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectBuffer");
				}
				IntPtr result = KinectBuffer.Windows_Storage_Streams_IBuffer_get_UnderlyingBuffer(this._pNative);
				ExceptionHelper.CheckLastError();
				return result;
			}
		}

		// Token: 0x04001690 RID: 5776
		internal IntPtr _pNative;
	}
}
