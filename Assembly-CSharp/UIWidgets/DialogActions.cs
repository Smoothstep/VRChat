using System;
using System.Collections;
using System.Collections.Generic;

namespace UIWidgets
{
	// Token: 0x02000932 RID: 2354
	public class DialogActions : IDictionary<string, Func<bool>>, ICollection<KeyValuePair<string, Func<bool>>>, IEnumerable<KeyValuePair<string, Func<bool>>>, IEnumerable
	{
		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x060046C5 RID: 18117 RVA: 0x0018002B File Offset: 0x0017E42B
		public int Count
		{
			get
			{
				return this.elements.Count;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x060046C6 RID: 18118 RVA: 0x00180038 File Offset: 0x0017E438
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AAD RID: 2733
		public Func<bool> this[string key]
		{
			get
			{
				int index = this.keys.IndexOf(key);
				return this.elements[index].Value;
			}
			set
			{
				int index = this.keys.IndexOf(key);
				this.elements[index] = new KeyValuePair<string, Func<bool>>(key, value);
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x060046C9 RID: 18121 RVA: 0x00180099 File Offset: 0x0017E499
		public ICollection<string> Keys
		{
			get
			{
				return this.elements.ConvertAll<string>((KeyValuePair<string, Func<bool>> x) => x.Key);
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x060046CA RID: 18122 RVA: 0x001800C3 File Offset: 0x0017E4C3
		public ICollection<Func<bool>> Values
		{
			get
			{
				return this.elements.ConvertAll<Func<bool>>((KeyValuePair<string, Func<bool>> x) => x.Value);
			}
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x001800ED File Offset: 0x0017E4ED
		public void Add(KeyValuePair<string, Func<bool>> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x00180104 File Offset: 0x0017E504
		public void Add(string key, Func<bool> value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("Key is null.");
			}
			if (this.ContainsKey(key))
			{
				throw new ArgumentException(string.Format("An element with the same key ({0}) already exists.", key));
			}
			this.keys.Add(key);
			this.values.Add(value);
			this.elements.Add(new KeyValuePair<string, Func<bool>>(key, value));
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x00180169 File Offset: 0x0017E569
		public void Clear()
		{
			this.keys.Clear();
			this.values.Clear();
			this.elements.Clear();
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x0018018C File Offset: 0x0017E58C
		public bool Contains(KeyValuePair<string, Func<bool>> item)
		{
			return this.elements.Contains(item);
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x0018019A File Offset: 0x0017E59A
		public bool ContainsKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("Key is null.");
			}
			return this.keys.Contains(key);
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x001801B9 File Offset: 0x0017E5B9
		public void CopyTo(KeyValuePair<string, Func<bool>>[] array, int arrayIndex)
		{
			this.elements.CopyTo(array, arrayIndex);
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x001801C8 File Offset: 0x0017E5C8
		public IEnumerator<KeyValuePair<string, Func<bool>>> GetEnumerator()
		{
			return this.elements.GetEnumerator();
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x001801DA File Offset: 0x0017E5DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.elements.GetEnumerator();
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x001801EC File Offset: 0x0017E5EC
		public bool Remove(KeyValuePair<string, Func<bool>> item)
		{
			if (!this.elements.Contains(item))
			{
				return false;
			}
			int index = this.elements.IndexOf(item);
			this.keys.RemoveAt(index);
			this.values.RemoveAt(index);
			this.elements.RemoveAt(index);
			return true;
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x00180240 File Offset: 0x0017E640
		public bool Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("Key is null.");
			}
			if (!this.ContainsKey(key))
			{
				return false;
			}
			int index = this.keys.IndexOf(key);
			this.keys.RemoveAt(index);
			this.values.RemoveAt(index);
			this.elements.RemoveAt(index);
			return true;
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x0018029E File Offset: 0x0017E69E
		public bool TryGetValue(string key, out Func<bool> value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("Key is null.");
			}
			if (!this.ContainsKey(key))
			{
				value = null;
				return false;
			}
			value = this.values[this.keys.IndexOf(key)];
			return true;
		}

		// Token: 0x04003057 RID: 12375
		private List<string> keys = new List<string>();

		// Token: 0x04003058 RID: 12376
		private List<Func<bool>> values = new List<Func<bool>>();

		// Token: 0x04003059 RID: 12377
		private List<KeyValuePair<string, Func<bool>>> elements = new List<KeyValuePair<string, Func<bool>>>();
	}
}
