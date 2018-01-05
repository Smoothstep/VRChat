using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007C2 RID: 1986
[Serializable]
public class FoldoutList
{
	// Token: 0x17000A24 RID: 2596
	public bool this[int index]
	{
		get
		{
			return this.foldouts[index];
		}
		set
		{
			this.foldouts[index] = value;
		}
	}

	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x0600400E RID: 16398 RVA: 0x0014217B File Offset: 0x0014057B
	public int Count
	{
		get
		{
			return this.foldouts.Count;
		}
	}

	// Token: 0x0600400F RID: 16399 RVA: 0x00142188 File Offset: 0x00140588
	public void Add(bool value)
	{
		this.foldouts.Add(value);
	}

	// Token: 0x06004010 RID: 16400 RVA: 0x00142196 File Offset: 0x00140596
	public void RemoveAt(int index)
	{
		this.foldouts.RemoveAt(index);
	}

	// Token: 0x06004011 RID: 16401 RVA: 0x001421A4 File Offset: 0x001405A4
	public void Reset()
	{
		for (int i = 0; i < this.foldouts.Count; i++)
		{
			this.foldouts[i] = false;
		}
	}

	// Token: 0x06004012 RID: 16402 RVA: 0x001421DC File Offset: 0x001405DC
	public void Update(int count, bool defaultValue)
	{
		while (this.foldouts.Count > count)
		{
			this.foldouts.RemoveAt(0);
		}
		for (int i = this.foldouts.Count; i < count; i++)
		{
			this.foldouts.Add(defaultValue);
		}
	}

	// Token: 0x06004013 RID: 16403 RVA: 0x00142234 File Offset: 0x00140634
	public void Isolate(int index)
	{
		for (int i = 0; i < this.foldouts.Count; i++)
		{
			if (i != index)
			{
				this.foldouts[i] = false;
			}
			else
			{
				this.foldouts[i] = true;
			}
		}
	}

	// Token: 0x04002864 RID: 10340
	[SerializeField]
	public List<bool> foldouts = new List<bool>();

	// Token: 0x04002865 RID: 10341
	[SerializeField]
	public bool mainFoldout;

	// Token: 0x04002866 RID: 10342
	[SerializeField]
	public Vector2 scrollPos;
}
