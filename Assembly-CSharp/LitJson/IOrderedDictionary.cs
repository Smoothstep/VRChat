using System;
using System.Collections;

namespace LitJson
{
	// Token: 0x020003F2 RID: 1010
	public interface IOrderedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06002413 RID: 9235
		IDictionaryEnumerator GetEnumerator();

		// Token: 0x06002414 RID: 9236
		void Insert(int index, object key, object value);

		// Token: 0x06002415 RID: 9237
		void RemoveAt(int index);

		// Token: 0x17000585 RID: 1413
		object this[int index]
		{
			get;
			set;
		}
	}
}
