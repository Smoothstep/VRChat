using System;

namespace PrimitivesPro.MeshCutting
{
	// Token: 0x0200085F RID: 2143
	internal class ArrayDictionary<T>
	{
		// Token: 0x06004286 RID: 17030 RVA: 0x00152063 File Offset: 0x00150463
		public ArrayDictionary(int size)
		{
			this.dictionary = new ArrayDictionary<T>.DicItem[size];
			this.Size = size;
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x0015207E File Offset: 0x0015047E
		public bool ContainsKey(int key)
		{
			return key < this.Size && this.dictionary[key].valid;
		}

		// Token: 0x17000A7D RID: 2685
		public T this[int key]
		{
			get
			{
				return this.dictionary[key].data;
			}
			set
			{
				this.dictionary[key].data = value;
			}
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x001520C8 File Offset: 0x001504C8
		public void Clear()
		{
			for (int i = 0; i < this.Size; i++)
			{
				this.dictionary[i].data = default(T);
				this.dictionary[i].valid = false;
			}
			this.Count = 0;
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x0015211F File Offset: 0x0015051F
		public void Add(int key, T data)
		{
			this.dictionary[key].valid = true;
			this.dictionary[key].data = data;
			this.Count++;
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x00152153 File Offset: 0x00150553
		public void Remove(int key)
		{
			this.dictionary[key].valid = false;
			this.Count--;
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x00152178 File Offset: 0x00150578
		public T[] ToArray()
		{
			T[] array = new T[this.Count];
			int num = 0;
			for (int i = 0; i < this.Size; i++)
			{
				if (this.dictionary[i].valid)
				{
					array[num++] = this.dictionary[i].data;
					if (num == this.Count)
					{
						return array;
					}
				}
			}
			return null;
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x001521EC File Offset: 0x001505EC
		public bool TryGetValue(int key, out T value)
		{
			ArrayDictionary<T>.DicItem dicItem = this.dictionary[key];
			if (dicItem.valid)
			{
				value = dicItem.data;
				return true;
			}
			value = default(T);
			return false;
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x00152238 File Offset: 0x00150638
		public T GetFirstValue()
		{
			for (int i = 0; i < this.Size; i++)
			{
				ArrayDictionary<T>.DicItem dicItem = this.dictionary[i];
				if (dicItem.valid)
				{
					return dicItem.data;
				}
			}
			return default(T);
		}

		// Token: 0x04002B31 RID: 11057
		public int Count;

		// Token: 0x04002B32 RID: 11058
		public int Size;

		// Token: 0x04002B33 RID: 11059
		private readonly ArrayDictionary<T>.DicItem[] dictionary;

		// Token: 0x02000860 RID: 2144
		private struct DicItem
		{
			// Token: 0x04002B34 RID: 11060
			public T data;

			// Token: 0x04002B35 RID: 11061
			public bool valid;
		}
	}
}
