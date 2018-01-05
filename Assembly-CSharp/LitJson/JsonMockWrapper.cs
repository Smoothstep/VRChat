using System;
using System.Collections;

namespace LitJson
{
	// Token: 0x02000400 RID: 1024
	public class JsonMockWrapper : IJsonWrapper, IList, IOrderedDictionary, ICollection, IEnumerable, IDictionary
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x000B692D File Offset: 0x000B4D2D
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000B6930 File Offset: 0x000B4D30
		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060024F2 RID: 9458 RVA: 0x000B6933 File Offset: 0x000B4D33
		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x000B6936 File Offset: 0x000B4D36
		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060024F4 RID: 9460 RVA: 0x000B6939 File Offset: 0x000B4D39
		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060024F5 RID: 9461 RVA: 0x000B693C File Offset: 0x000B4D3C
		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x000B693F File Offset: 0x000B4D3F
		public bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000B6942 File Offset: 0x000B4D42
		public bool GetBoolean()
		{
			return false;
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000B6945 File Offset: 0x000B4D45
		public double GetDouble()
		{
			return 0.0;
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000B6950 File Offset: 0x000B4D50
		public int GetInt()
		{
			return 0;
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000B6953 File Offset: 0x000B4D53
		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000B6956 File Offset: 0x000B4D56
		public long GetLong()
		{
			return 0L;
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000B695A File Offset: 0x000B4D5A
		public string GetString()
		{
			return string.Empty;
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000B6961 File Offset: 0x000B4D61
		public void SetBoolean(bool val)
		{
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000B6963 File Offset: 0x000B4D63
		public void SetDouble(double val)
		{
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000B6965 File Offset: 0x000B4D65
		public void SetInt(int val)
		{
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x000B6967 File Offset: 0x000B4D67
		public void SetJsonType(JsonType type)
		{
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000B6969 File Offset: 0x000B4D69
		public void SetLong(long val)
		{
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000B696B File Offset: 0x000B4D6B
		public void SetString(string val)
		{
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x000B696D File Offset: 0x000B4D6D
		public string ToJson()
		{
			return string.Empty;
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x000B6974 File Offset: 0x000B4D74
		public void ToJson(JsonWriter writer)
		{
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002505 RID: 9477 RVA: 0x000B6976 File Offset: 0x000B4D76
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x000B6979 File Offset: 0x000B4D79
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005B7 RID: 1463
		object IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000B6981 File Offset: 0x000B4D81
		int IList.Add(object value)
		{
			return 0;
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x000B6984 File Offset: 0x000B4D84
		void IList.Clear()
		{
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x000B6986 File Offset: 0x000B4D86
		bool IList.Contains(object value)
		{
			return false;
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x000B6989 File Offset: 0x000B4D89
		int IList.IndexOf(object value)
		{
			return -1;
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000B698C File Offset: 0x000B4D8C
		void IList.Insert(int i, object v)
		{
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000B698E File Offset: 0x000B4D8E
		void IList.Remove(object value)
		{
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x000B6990 File Offset: 0x000B4D90
		void IList.RemoveAt(int index)
		{
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x000B6992 File Offset: 0x000B4D92
		int ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x000B6995 File Offset: 0x000B4D95
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002512 RID: 9490 RVA: 0x000B6998 File Offset: 0x000B4D98
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000B699B File Offset: 0x000B4D9B
		void ICollection.CopyTo(Array array, int index)
		{
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000B699D File Offset: 0x000B4D9D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x000B69A0 File Offset: 0x000B4DA0
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002516 RID: 9494 RVA: 0x000B69A3 File Offset: 0x000B4DA3
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002517 RID: 9495 RVA: 0x000B69A6 File Offset: 0x000B4DA6
		ICollection IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06002518 RID: 9496 RVA: 0x000B69A9 File Offset: 0x000B4DA9
		ICollection IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005BF RID: 1471
		object IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000B69B1 File Offset: 0x000B4DB1
		void IDictionary.Add(object k, object v)
		{
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000B69B3 File Offset: 0x000B4DB3
		void IDictionary.Clear()
		{
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000B69B5 File Offset: 0x000B4DB5
		bool IDictionary.Contains(object key)
		{
			return false;
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x000B69B8 File Offset: 0x000B4DB8
		void IDictionary.Remove(object key)
		{
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000B69BA File Offset: 0x000B4DBA
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x170005C0 RID: 1472
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x000B69C2 File Offset: 0x000B4DC2
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x000B69C5 File Offset: 0x000B4DC5
		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x000B69C7 File Offset: 0x000B4DC7
		void IOrderedDictionary.RemoveAt(int i)
		{
		}
	}
}
