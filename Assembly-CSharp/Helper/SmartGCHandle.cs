using System;
using System.Runtime.InteropServices;

namespace Helper
{
	// Token: 0x020004B5 RID: 1205
	public class SmartGCHandle : IDisposable
	{
		// Token: 0x06002A21 RID: 10785 RVA: 0x000D6CD7 File Offset: 0x000D50D7
		public SmartGCHandle(GCHandle handle)
		{
			this.handle = handle;
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000D6CE8 File Offset: 0x000D50E8
		~SmartGCHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x000D6D18 File Offset: 0x000D5118
		public IntPtr AddrOfPinnedObject()
		{
			return this.handle.AddrOfPinnedObject();
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000D6D25 File Offset: 0x000D5125
		public virtual void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x000D6D2E File Offset: 0x000D512E
		protected virtual void Dispose(bool disposing)
		{
			this.handle.Free();
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000D6D3B File Offset: 0x000D513B
		public static implicit operator GCHandle(SmartGCHandle other)
		{
			return other.handle;
		}

		// Token: 0x04001703 RID: 5891
		private GCHandle handle;
	}
}
