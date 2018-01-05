using System;

namespace Helper
{
	// Token: 0x020004B4 RID: 1204
	public static class NativeWrapper
	{
		// Token: 0x06002A20 RID: 10784 RVA: 0x000D6CA0 File Offset: 0x000D50A0
		public static IntPtr GetNativePtr(object obj)
		{
			if (obj == null)
			{
				return IntPtr.Zero;
			}
			INativeWrapper nativeWrapper = obj as INativeWrapper;
			if (nativeWrapper != null)
			{
				return nativeWrapper.nativePtr;
			}
			throw new ArgumentException("Object must wrap native type");
		}
	}
}
