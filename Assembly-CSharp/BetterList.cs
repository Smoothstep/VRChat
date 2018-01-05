using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020005E6 RID: 1510
public class BetterList<T>
{
	// Token: 0x060031ED RID: 12781 RVA: 0x000F74E4 File Offset: 0x000F58E4
	public IEnumerator<T> GetEnumerator()
	{
		if (this.buffer != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				yield return this.buffer[i];
			}
		}
		yield break;
	}

	// Token: 0x17000787 RID: 1927
	[DebuggerHidden]
	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x000F751C File Offset: 0x000F591C
	private void AllocateMore()
	{
		T[] array = (this.buffer == null) ? new T[32] : new T[Mathf.Max(this.buffer.Length << 1, 32)];
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(array, 0);
		}
		this.buffer = array;
	}

	// Token: 0x060031F1 RID: 12785 RVA: 0x000F7584 File Offset: 0x000F5984
	private void Trim()
	{
		if (this.size > 0)
		{
			if (this.size < this.buffer.Length)
			{
				T[] array = new T[this.size];
				for (int i = 0; i < this.size; i++)
				{
					array[i] = this.buffer[i];
				}
				this.buffer = array;
			}
		}
		else
		{
			this.buffer = null;
		}
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x000F75F9 File Offset: 0x000F59F9
	public void Clear()
	{
		this.size = 0;
	}

	// Token: 0x060031F3 RID: 12787 RVA: 0x000F7602 File Offset: 0x000F5A02
	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x000F7614 File Offset: 0x000F5A14
	public void Add(T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		this.buffer[this.size++] = item;
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x000F7664 File Offset: 0x000F5A64
	public void Insert(int index, T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		if (index > -1 && index < this.size)
		{
			for (int i = this.size; i > index; i--)
			{
				this.buffer[i] = this.buffer[i - 1];
			}
			this.buffer[index] = item;
			this.size++;
		}
		else
		{
			this.Add(item);
		}
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x000F7700 File Offset: 0x000F5B00
	public bool Contains(T item)
	{
		if (this.buffer == null)
		{
			return false;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x000F7758 File Offset: 0x000F5B58
	public int IndexOf(T item)
	{
		if (this.buffer == null)
		{
			return -1;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x000F77B0 File Offset: 0x000F5BB0
	public bool Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(T);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					this.buffer[this.size] = default(T);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x000F7870 File Offset: 0x000F5C70
	public void RemoveAt(int index)
	{
		if (this.buffer != null && index > -1 && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(T);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
			this.buffer[this.size] = default(T);
		}
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x000F790C File Offset: 0x000F5D0C
	public T Pop()
	{
		if (this.buffer != null && this.size != 0)
		{
			T result = this.buffer[--this.size];
			this.buffer[this.size] = default(T);
			return result;
		}
		return default(T);
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x000F7971 File Offset: 0x000F5D71
	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x000F7980 File Offset: 0x000F5D80
	[DebuggerHidden]
	[DebuggerStepThrough]
	public void Sort(BetterList<T>.CompareFunc comparer)
	{
		int num = 0;
		int num2 = this.size - 1;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = num; i < num2; i++)
			{
				if (comparer(this.buffer[i], this.buffer[i + 1]) > 0)
				{
					T t = this.buffer[i];
					this.buffer[i] = this.buffer[i + 1];
					this.buffer[i + 1] = t;
					flag = true;
				}
				else if (!flag)
				{
					num = ((i != 0) ? (i - 1) : 0);
				}
			}
		}
	}

	// Token: 0x04001C93 RID: 7315
	public T[] buffer;

	// Token: 0x04001C94 RID: 7316
	public int size;

	// Token: 0x020005E7 RID: 1511
	// (Invoke) Token: 0x060031FE RID: 12798
	public delegate int CompareFunc(T left, T right);
}
