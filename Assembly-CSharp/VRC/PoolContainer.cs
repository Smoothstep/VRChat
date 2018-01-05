using System;
using System.Collections.Generic;

namespace VRC
{
	// Token: 0x02000A64 RID: 2660
	public class PoolContainer<T>
	{
		// Token: 0x0600508C RID: 20620 RVA: 0x001B8688 File Offset: 0x001B6A88
		public T[] Get(int length)
		{
			object obj = this.syncLock;
			lock (obj)
			{
				for (int i = 0; i < PoolContainer<T>.Pool.Count; i++)
				{
					if (PoolContainer<T>.Pool[i].Length == length)
					{
						T[] result = PoolContainer<T>.Pool[i];
						PoolContainer<T>.Pool.RemoveAt(i);
						return result;
					}
				}
			}
			return new T[length];
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x001B8714 File Offset: 0x001B6B14
		public void Return(T[] d)
		{
			object obj = this.syncLock;
			lock (obj)
			{
				PoolContainer<T>.Pool.Add(d);
			}
		}

		// Token: 0x0400392C RID: 14636
		private readonly object syncLock = new object();

		// Token: 0x0400392D RID: 14637
		private static List<T[]> Pool = new List<T[]>();
	}
}
