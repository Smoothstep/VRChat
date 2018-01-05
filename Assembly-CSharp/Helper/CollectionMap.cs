using System;

namespace Helper
{
	// Token: 0x02000497 RID: 1175
	internal class CollectionMap<TKey, TValue> : ThreadSafeDictionary<TKey, TValue> where TValue : new()
	{
		// Token: 0x0600284F RID: 10319 RVA: 0x000D1758 File Offset: 0x000CFB58
		public bool TryAddDefault(TKey key)
		{
			object impl = this._impl;
			bool result;
			lock (impl)
			{
				if (!this._impl.ContainsKey(key))
				{
					this._impl.Add(key, Activator.CreateInstance<TValue>());
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
	}
}
