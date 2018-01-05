using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace LitJson
{
	// Token: 0x020003F4 RID: 1012
	public class JsonData : IJsonWrapper, IEquatable<JsonData>, IList, IOrderedDictionary, ICollection, IEnumerable, IDictionary
	{
		// Token: 0x0600242D RID: 9261 RVA: 0x000B403A File Offset: 0x000B243A
		public JsonData()
		{
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000B4042 File Offset: 0x000B2442
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000B4058 File Offset: 0x000B2458
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000B406E File Offset: 0x000B246E
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000B4084 File Offset: 0x000B2484
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000B409C File Offset: 0x000B249C
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000B4154 File Offset: 0x000B2554
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002434 RID: 9268 RVA: 0x000B416A File Offset: 0x000B256A
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x000B4177 File Offset: 0x000B2577
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06002436 RID: 9270 RVA: 0x000B4182 File Offset: 0x000B2582
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x000B418D File Offset: 0x000B258D
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x000B4198 File Offset: 0x000B2598
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x000B41A3 File Offset: 0x000B25A3
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x0600243A RID: 9274 RVA: 0x000B41AE File Offset: 0x000B25AE
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x0600243B RID: 9275 RVA: 0x000B41B9 File Offset: 0x000B25B9
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x000B41C4 File Offset: 0x000B25C4
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object.Keys;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x000B41D8 File Offset: 0x000B25D8
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x000B41E0 File Offset: 0x000B25E0
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600243F RID: 9279 RVA: 0x000B41ED File Offset: 0x000B25ED
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06002440 RID: 9280 RVA: 0x000B41FA File Offset: 0x000B25FA
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06002441 RID: 9281 RVA: 0x000B4207 File Offset: 0x000B2607
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06002442 RID: 9282 RVA: 0x000B4214 File Offset: 0x000B2614
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> list = new List<string>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Key);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06002443 RID: 9283 RVA: 0x000B4288 File Offset: 0x000B2688
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> list = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Value);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002444 RID: 9284 RVA: 0x000B42FC File Offset: 0x000B26FC
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06002445 RID: 9285 RVA: 0x000B4304 File Offset: 0x000B2704
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06002446 RID: 9286 RVA: 0x000B430C File Offset: 0x000B270C
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06002447 RID: 9287 RVA: 0x000B4314 File Offset: 0x000B2714
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x000B431C File Offset: 0x000B271C
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002449 RID: 9289 RVA: 0x000B4324 File Offset: 0x000B2724
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x000B432C File Offset: 0x000B272C
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x000B4334 File Offset: 0x000B2734
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x0600244C RID: 9292 RVA: 0x000B4341 File Offset: 0x000B2741
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x1700059D RID: 1437
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData value2 = this.ToJsonData(value);
				this[(string)key] = value2;
			}
		}

		// Token: 0x1700059E RID: 1438
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData value2 = this.ToJsonData(value);
				KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
				this.inst_object[keyValuePair.Key] = value2;
				KeyValuePair<string, JsonData> value3 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value2);
				this.object_list[idx] = value3;
			}
		}

		// Token: 0x1700059F RID: 1439
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData value2 = this.ToJsonData(value);
				this[index] = value2;
			}
		}

		// Token: 0x170005A9 RID: 1449
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = keyValuePair;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(keyValuePair);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x170005AA RID: 1450
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
					KeyValuePair<string, JsonData> value2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
					this.object_list[index] = value2;
					this.inst_object[keyValuePair.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000B45B9 File Offset: 0x000B29B9
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000B45C1 File Offset: 0x000B29C1
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000B45C9 File Offset: 0x000B29C9
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000B45D1 File Offset: 0x000B29D1
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000B45D9 File Offset: 0x000B29D9
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000B45E1 File Offset: 0x000B29E1
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000B4600 File Offset: 0x000B2A00
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000B461F File Offset: 0x000B2A1F
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x000B463E File Offset: 0x000B2A3E
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000B465D File Offset: 0x000B2A5D
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000B467C File Offset: 0x000B2A7C
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000B468C File Offset: 0x000B2A8C
		void IDictionary.Add(object key, object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, value2);
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>((string)key, value2);
			this.object_list.Add(item);
			this.json = null;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000B46CF File Offset: 0x000B2ACF
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000B46EE File Offset: 0x000B2AEE
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000B46FC File Offset: 0x000B2AFC
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000B4704 File Offset: 0x000B2B04
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x000B4775 File Offset: 0x000B2B75
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000B4782 File Offset: 0x000B2B82
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000B47A1 File Offset: 0x000B2BA1
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000B47C0 File Offset: 0x000B2BC0
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000B47DF File Offset: 0x000B2BDF
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000B47FE File Offset: 0x000B2BFE
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000B481D File Offset: 0x000B2C1D
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000B4834 File Offset: 0x000B2C34
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000B484B File Offset: 0x000B2C4B
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000B4862 File Offset: 0x000B2C62
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x000B4879 File Offset: 0x000B2C79
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000B4890 File Offset: 0x000B2C90
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000B4898 File Offset: 0x000B2C98
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000B48A1 File Offset: 0x000B2CA1
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000B48AA File Offset: 0x000B2CAA
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000B48BE File Offset: 0x000B2CBE
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000B48CC File Offset: 0x000B2CCC
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000B48DA File Offset: 0x000B2CDA
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000B48F0 File Offset: 0x000B2CF0
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000B4905 File Offset: 0x000B2D05
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x000B491A File Offset: 0x000B2D1A
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000B4934 File Offset: 0x000B2D34
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData value2 = this.ToJsonData(value);
			this[text] = value2;
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(text, value2);
			this.object_list.Insert(idx, item);
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000B4970 File Offset: 0x000B2D70
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000B49B0 File Offset: 0x000B2DB0
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000B49EC File Offset: 0x000B2DEC
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000B4A50 File Offset: 0x000B2E50
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000B4AA8 File Offset: 0x000B2EA8
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000B4ACC File Offset: 0x000B2ECC
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator = obj.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						JsonData.WriteJson((JsonData)obj2, writer);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				IDictionaryEnumerator enumerator2 = obj.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj3 = enumerator2.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
						writer.WritePropertyName((string)dictionaryEntry.Key);
						JsonData.WriteJson((JsonData)dictionaryEntry.Value, writer);
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000B4C54 File Offset: 0x000B3054
		public int Add(object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(value2);
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000B4C7C File Offset: 0x000B307C
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000B4CA4 File Offset: 0x000B30A4
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type)
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000B4D7F File Offset: 0x000B317F
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000B4D88 File Offset: 0x000B3188
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000B4E4C File Offset: 0x000B324C
		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(stringWriter)
			{
				Validate = false
			});
			this.json = stringWriter.ToString();
			return this.json;
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x000B4E98 File Offset: 0x000B3298
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000B4EC4 File Offset: 0x000B32C4
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x04001215 RID: 4629
		private IList<JsonData> inst_array;

		// Token: 0x04001216 RID: 4630
		private bool inst_boolean;

		// Token: 0x04001217 RID: 4631
		private double inst_double;

		// Token: 0x04001218 RID: 4632
		private int inst_int;

		// Token: 0x04001219 RID: 4633
		private long inst_long;

		// Token: 0x0400121A RID: 4634
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x0400121B RID: 4635
		private string inst_string;

		// Token: 0x0400121C RID: 4636
		private string json;

		// Token: 0x0400121D RID: 4637
		private JsonType type;

		// Token: 0x0400121E RID: 4638
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
