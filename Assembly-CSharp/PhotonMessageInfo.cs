using System;

// Token: 0x02000758 RID: 1880
public struct PhotonMessageInfo
{
	// Token: 0x06003CCB RID: 15563 RVA: 0x001338E7 File Offset: 0x00131CE7
	public PhotonMessageInfo(PhotonPlayer player, int timestamp, PhotonView view)
	{
		this.sender = player;
		this.timeInt = timestamp;
		this.photonView = view;
	}

	// Token: 0x17000984 RID: 2436
	// (get) Token: 0x06003CCC RID: 15564 RVA: 0x001338FE File Offset: 0x00131CFE
	public int timestampInMilliseconds
	{
		get
		{
			return this.timeInt;
		}
	}

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06003CCD RID: 15565 RVA: 0x00133908 File Offset: 0x00131D08
	public double timestamp
	{
		get
		{
			uint num = (uint)this.timeInt;
			double num2 = num;
			return num2 / 1000.0;
		}
	}

	// Token: 0x06003CCE RID: 15566 RVA: 0x0013392B File Offset: 0x00131D2B
	public override string ToString()
	{
		return string.Format("[PhotonMessageInfo: Sender='{1}' Senttime={0}]", this.timestamp, this.sender);
	}

	// Token: 0x04002613 RID: 9747
	private readonly int timeInt;

	// Token: 0x04002614 RID: 9748
	public readonly PhotonPlayer sender;

	// Token: 0x04002615 RID: 9749
	public readonly PhotonView photonView;
}
