using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000763 RID: 1891
public class PhotonStreamQueue
{
	// Token: 0x06003DC8 RID: 15816 RVA: 0x00137677 File Offset: 0x00135A77
	public PhotonStreamQueue(int sampleRate)
	{
		this.m_SampleRate = sampleRate;
	}

	// Token: 0x06003DC9 RID: 15817 RVA: 0x001376B4 File Offset: 0x00135AB4
	private void BeginWritePackage()
	{
		if (Time.realtimeSinceStartup < this.m_LastSampleTime + 1f / (float)this.m_SampleRate)
		{
			this.m_IsWriting = false;
			return;
		}
		if (this.m_SampleCount == 1)
		{
			this.m_ObjectsPerSample = this.m_Objects.Count;
		}
		else if (this.m_SampleCount > 1 && this.m_Objects.Count / this.m_SampleCount != this.m_ObjectsPerSample)
		{
			Debug.LogWarning("The number of objects sent via a PhotonStreamQueue has to be the same each frame");
			Debug.LogWarning(string.Concat(new object[]
			{
				"Objects in List: ",
				this.m_Objects.Count,
				" / Sample Count: ",
				this.m_SampleCount,
				" = ",
				this.m_Objects.Count / this.m_SampleCount,
				" != ",
				this.m_ObjectsPerSample
			}));
		}
		this.m_IsWriting = true;
		this.m_SampleCount++;
		this.m_LastSampleTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x001377D5 File Offset: 0x00135BD5
	public void Reset()
	{
		this.m_SampleCount = 0;
		this.m_ObjectsPerSample = -1;
		this.m_LastSampleTime = float.NegativeInfinity;
		this.m_LastFrameCount = -1;
		this.m_Objects.Clear();
	}

	// Token: 0x06003DCB RID: 15819 RVA: 0x00137802 File Offset: 0x00135C02
	public void SendNext(object obj)
	{
		if (Time.frameCount != this.m_LastFrameCount)
		{
			this.BeginWritePackage();
		}
		this.m_LastFrameCount = Time.frameCount;
		if (!this.m_IsWriting)
		{
			return;
		}
		this.m_Objects.Add(obj);
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x0013783D File Offset: 0x00135C3D
	public bool HasQueuedObjects()
	{
		return this.m_NextObjectIndex != -1;
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x0013784C File Offset: 0x00135C4C
	public object ReceiveNext()
	{
		if (this.m_NextObjectIndex == -1)
		{
			return null;
		}
		if (this.m_NextObjectIndex >= this.m_Objects.Count)
		{
			this.m_NextObjectIndex -= this.m_ObjectsPerSample;
		}
		return this.m_Objects[this.m_NextObjectIndex++];
	}

	// Token: 0x06003DCE RID: 15822 RVA: 0x001378AC File Offset: 0x00135CAC
	public void Serialize(PhotonStream stream)
	{
		if (this.m_Objects.Count > 0 && this.m_ObjectsPerSample < 0)
		{
			this.m_ObjectsPerSample = this.m_Objects.Count;
		}
		stream.SendNext(this.m_SampleCount);
		stream.SendNext(this.m_ObjectsPerSample);
		for (int i = 0; i < this.m_Objects.Count; i++)
		{
			stream.SendNext(this.m_Objects[i]);
		}
		this.m_Objects.Clear();
		this.m_SampleCount = 0;
	}

	// Token: 0x06003DCF RID: 15823 RVA: 0x0013794C File Offset: 0x00135D4C
	public void Deserialize(PhotonStream stream)
	{
		this.m_Objects.Clear();
		this.m_SampleCount = (int)stream.ReceiveNext();
		this.m_ObjectsPerSample = (int)stream.ReceiveNext();
		for (int i = 0; i < this.m_SampleCount * this.m_ObjectsPerSample; i++)
		{
			this.m_Objects.Add(stream.ReceiveNext());
		}
		if (this.m_Objects.Count > 0)
		{
			this.m_NextObjectIndex = 0;
		}
		else
		{
			this.m_NextObjectIndex = -1;
		}
	}

	// Token: 0x0400266C RID: 9836
	private int m_SampleRate;

	// Token: 0x0400266D RID: 9837
	private int m_SampleCount;

	// Token: 0x0400266E RID: 9838
	private int m_ObjectsPerSample = -1;

	// Token: 0x0400266F RID: 9839
	private float m_LastSampleTime = float.NegativeInfinity;

	// Token: 0x04002670 RID: 9840
	private int m_LastFrameCount = -1;

	// Token: 0x04002671 RID: 9841
	private int m_NextObjectIndex = -1;

	// Token: 0x04002672 RID: 9842
	private List<object> m_Objects = new List<object>();

	// Token: 0x04002673 RID: 9843
	private bool m_IsWriting;
}
