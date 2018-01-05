using System;
using System.Runtime.InteropServices;

namespace Helper
{
	// Token: 0x02000499 RID: 1177
	public static class ExceptionHelper
	{
		// Token: 0x06002858 RID: 10328 RVA: 0x000D196C File Offset: 0x000CFD6C
		public static void CheckLastError()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == -2147483638 || lastWin32Error == -2147467259)
			{
				return;
			}
			if (lastWin32Error >= 0)
			{
				return;
			}
			Exception exceptionForHR = Marshal.GetExceptionForHR(lastWin32Error);
			string message = string.Format("This API has returned an exception from an HRESULT: 0x{0:X}", lastWin32Error);
			switch (lastWin32Error + 2147467263)
			{
			case 0:
				throw new NotImplementedException(message, exceptionForHR);
			default:
				if (lastWin32Error == -2147024882)
				{
					throw new OutOfMemoryException(message, exceptionForHR);
				}
				if (lastWin32Error != -2147024809)
				{
					throw new InvalidOperationException(message, exceptionForHR);
				}
				throw new ArgumentException(message, exceptionForHR);
			case 2:
				throw new ArgumentNullException(message, exceptionForHR);
			}
		}

		// Token: 0x0400168A RID: 5770
		private const int E_NOTIMPL = -2147467263;

		// Token: 0x0400168B RID: 5771
		private const int E_OUTOFMEMORY = -2147024882;

		// Token: 0x0400168C RID: 5772
		private const int E_INVALIDARG = -2147024809;

		// Token: 0x0400168D RID: 5773
		private const int E_POINTER = -2147467261;

		// Token: 0x0400168E RID: 5774
		private const int E_PENDING = -2147483638;

		// Token: 0x0400168F RID: 5775
		private const int E_FAIL = -2147467259;
	}
}
