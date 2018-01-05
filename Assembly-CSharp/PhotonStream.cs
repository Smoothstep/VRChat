using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200075A RID: 1882
public class PhotonStream
{
	// Token: 0x06003CD0 RID: 15568 RVA: 0x00133950 File Offset: 0x00131D50
	public PhotonStream(bool write, object[] incomingData)
	{
		this.write = write;
		if (incomingData == null)
		{
			this.writeData = new Queue<object>(10);
		}
		else
		{
			this.readData = incomingData;
		}
	}

	// Token: 0x06003CD1 RID: 15569 RVA: 0x0013397E File Offset: 0x00131D7E
	public void SetReadStream(object[] incomingData, byte pos = 0)
	{
		this.readData = incomingData;
		this.currentItem = pos;
		this.write = false;
	}

	// Token: 0x06003CD2 RID: 15570 RVA: 0x00133995 File Offset: 0x00131D95
	internal void ResetWriteStream()
	{
		this.writeData.Clear();
	}

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06003CD3 RID: 15571 RVA: 0x001339A2 File Offset: 0x00131DA2
	public bool isWriting
	{
		get
		{
			return this.write;
		}
	}

	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x06003CD4 RID: 15572 RVA: 0x001339AA File Offset: 0x00131DAA
	public bool isReading
	{
		get
		{
			return !this.write;
		}
	}

	// Token: 0x17000988 RID: 2440
	// (get) Token: 0x06003CD5 RID: 15573 RVA: 0x001339B5 File Offset: 0x00131DB5
	public int Count
	{
		get
		{
			return (!this.isWriting) ? this.readData.Length : this.writeData.Count;
		}
	}

	// Token: 0x06003CD6 RID: 15574 RVA: 0x001339DC File Offset: 0x00131DDC
	public object ReceiveNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		if ((int)this.currentItem >= this.Count)
		{
			Debug.LogError("Error: read beyond stream length!");
			return null;
		}
		object result = this.readData[(int)this.currentItem];
		this.currentItem += 1;
		return result;
	}

	// Token: 0x06003CD7 RID: 15575 RVA: 0x00133A3C File Offset: 0x00131E3C
	public object PeekNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		return this.readData[(int)this.currentItem];
	}

	// Token: 0x06003CD8 RID: 15576 RVA: 0x00133A6F File Offset: 0x00131E6F
	public void SendNext(object obj)
	{
		if (!this.write)
		{
			Debug.LogError("Error: you cannot write/send to this stream that you are reading!");
			return;
		}
		this.writeData.Enqueue(obj);
	}

	// Token: 0x06003CD9 RID: 15577 RVA: 0x00133A93 File Offset: 0x00131E93
	public object[] ToArray()
	{
		return (!this.isWriting) ? this.readData : this.writeData.ToArray();
	}

	// Token: 0x06003CDA RID: 15578 RVA: 0x00133AB8 File Offset: 0x00131EB8
	public void Serialize(ref bool myBool)
	{
		if (this.write)
		{
			this.writeData.Enqueue(myBool);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			myBool = (bool)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x00133B20 File Offset: 0x00131F20
	public void Serialize(ref int myInt)
	{
		if (this.write)
		{
			this.writeData.Enqueue(myInt);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			myInt = (int)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x00133B88 File Offset: 0x00131F88
	public void Serialize(ref string value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			value = (string)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x00133BE8 File Offset: 0x00131FE8
	public void Serialize(ref char value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			value = (char)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x00133C50 File Offset: 0x00132050
	public void Serialize(ref short value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			value = (short)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x00133CB8 File Offset: 0x001320B8
	public void Serialize(ref float obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (float)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x00133D20 File Offset: 0x00132120
	public void Serialize(ref PhotonPlayer obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (PhotonPlayer)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x00133D80 File Offset: 0x00132180
	public void Serialize(ref Vector3 obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (Vector3)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x00133DF0 File Offset: 0x001321F0
	public void Serialize(ref Vector2 obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (Vector2)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x00133E60 File Offset: 0x00132260
	public void Serialize(ref Quaternion obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (Quaternion)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x04002622 RID: 9762
	private bool write;

	// Token: 0x04002623 RID: 9763
	private Queue<object> writeData;

	// Token: 0x04002624 RID: 9764
	private object[] readData;

	// Token: 0x04002625 RID: 9765
	internal byte currentItem;
}
