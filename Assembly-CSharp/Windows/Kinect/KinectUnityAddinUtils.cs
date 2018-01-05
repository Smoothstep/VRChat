using System;
using System.Runtime.InteropServices;

namespace Windows.Kinect
{
	// Token: 0x02000506 RID: 1286
	public sealed class KinectUnityAddinUtils
	{
		// Token: 0x06002D2A RID: 11562
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void KinectUnityAddin_FreeMemory(IntPtr pToDealloc);

		// Token: 0x06002D2B RID: 11563 RVA: 0x000DF461 File Offset: 0x000DD861
		public static void FreeMemory(IntPtr pToDealloc)
		{
			KinectUnityAddinUtils.KinectUnityAddin_FreeMemory(pToDealloc);
		}
	}
}
