using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x020003F5 RID: 1013
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x0600248B RID: 9355 RVA: 0x000B4F61 File Offset: 0x000B3361
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x0600248C RID: 9356 RVA: 0x000B4F70 File Offset: 0x000B3370
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x000B4F80 File Offset: 0x000B3380
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x0600248E RID: 9358 RVA: 0x000B4FAC File Offset: 0x000B33AC
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600248F RID: 9359 RVA: 0x000B4FCC File Offset: 0x000B33CC
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000B4FEC File Offset: 0x000B33EC
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x000B4FF9 File Offset: 0x000B33F9
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x0400121F RID: 4639
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
