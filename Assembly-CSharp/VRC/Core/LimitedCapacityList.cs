using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VRC.Core
{
	// Token: 0x02000A63 RID: 2659
	public class LimitedCapacityList<T> : ICollection<T>, IEnumerable<T>, IList<T>, IEnumerable
	{
		// Token: 0x06005078 RID: 20600 RVA: 0x001B8496 File Offset: 0x001B6896
		public LimitedCapacityList(int sz)
		{
			this.data = new ArrayList(sz);
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x001B84B7 File Offset: 0x001B68B7
		public LimitedCapacityList() : this(100)
		{
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x0600507A RID: 20602 RVA: 0x001B84C1 File Offset: 0x001B68C1
		public int Count
		{
			get
			{
				return (this.data != null) ? this.data.Count : 0;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x0600507B RID: 20603 RVA: 0x001B84DF File Offset: 0x001B68DF
		public bool IsReadOnly
		{
			get
			{
				return this.data == null || this.data.IsReadOnly;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		public T this[int index]
		{
			get
			{
				return (T)((object)this.data[index]);
			}
			set
			{
				this.data[index] = value;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x0600507E RID: 20606 RVA: 0x001B8524 File Offset: 0x001B6924
		// (set) Token: 0x0600507F RID: 20607 RVA: 0x001B8531 File Offset: 0x001B6931
		public int Capacity
		{
			get
			{
				return this.data.Capacity;
			}
			set
			{
				if (this.data.Count > value)
				{
					this.data.RemoveRange(0, this.data.Count - value);
				}
				this.data.Capacity = value;
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06005080 RID: 20608 RVA: 0x001B8569 File Offset: 0x001B6969
		public bool Full
		{
			get
			{
				return this.data.Capacity == this.data.Count;
			}
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x001B8583 File Offset: 0x001B6983
		public void Add(T item)
		{
			if (this.Full)
			{
				this.data.RemoveAt(0);
			}
			this.data.Add(item);
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x001B85AE File Offset: 0x001B69AE
		public void Clear()
		{
			this.data.Clear();
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x001B85BB File Offset: 0x001B69BB
		public bool Contains(T item)
		{
			return this.data.Contains(item);
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x001B85CE File Offset: 0x001B69CE
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.data.CopyTo(array, arrayIndex);
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x001B85DD File Offset: 0x001B69DD
		public bool Remove(T item)
		{
			if (this.data.Contains(item))
			{
				this.data.Remove(item);
				return true;
			}
			return false;
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x001B8609 File Offset: 0x001B6A09
		public IEnumerator<T> GetEnumerator()
		{
			return this.data.OfType<T>().GetEnumerator();
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x001B861B File Offset: 0x001B6A1B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x001B8628 File Offset: 0x001B6A28
		public int IndexOf(T item)
		{
			return this.data.IndexOf(item);
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x001B863B File Offset: 0x001B6A3B
		public void Insert(int index, T item)
		{
			if (this.Full)
			{
				this.data.RemoveAt(0);
			}
			this.data.Insert(index, item);
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x001B8666 File Offset: 0x001B6A66
		public void RemoveAt(int index)
		{
			this.data.RemoveAt(index);
		}

		// Token: 0x0400392A RID: 14634
		public const int DEFAULT_SIZE = 100;

		// Token: 0x0400392B RID: 14635
		private ArrayList data = new ArrayList(100);
	}
}
