using System;
using System.Collections.Generic;

namespace Helper
{
	// Token: 0x020004B6 RID: 1206
	public class ThreadSafeDictionary<TKey, TValue>
	{
		// Token: 0x17000655 RID: 1621
		public TValue this[TKey key]
		{
			get
			{
				object impl = this._impl;
				TValue result;
				lock (impl)
				{
					result = this._impl[key];
				}
				return result;
			}
			set
			{
				object impl = this._impl;
				lock (impl)
				{
					this._impl[key] = value;
				}
			}
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x000D163C File Offset: 0x000CFA3C
		public void Add(TKey key, TValue value)
		{
			object impl = this._impl;
			lock (impl)
			{
				this._impl.Add(key, value);
			}
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x000D1680 File Offset: 0x000CFA80
		public bool TryGetValue(TKey key, out TValue value)
		{
			object impl = this._impl;
			bool result;
			lock (impl)
			{
				result = this._impl.TryGetValue(key, out value);
			}
			return result;
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x000D16C8 File Offset: 0x000CFAC8
		public bool Remove(TKey key)
		{
			object impl = this._impl;
			bool result;
			lock (impl)
			{
				result = this._impl.Remove(key);
			}
			return result;
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x000D170C File Offset: 0x000CFB0C
		public void Clear()
		{
			object impl = this._impl;
			lock (impl)
			{
				this._impl.Clear();
			}
		}

		// Token: 0x04001704 RID: 5892
		protected readonly Dictionary<TKey, TValue> _impl = new Dictionary<TKey, TValue>();
	}
}
