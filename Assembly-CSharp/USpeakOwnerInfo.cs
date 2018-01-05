using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009E1 RID: 2529
public class USpeakOwnerInfo : MonoBehaviour
{
	// Token: 0x17000B9A RID: 2970
	// (get) Token: 0x06004CE0 RID: 19680 RVA: 0x0019BF9C File Offset: 0x0019A39C
	public USpeaker Speaker
	{
		get
		{
			if (this.m_speaker == null)
			{
				this.m_speaker = USpeaker.Get(this);
			}
			return this.m_speaker;
		}
	}

	// Token: 0x17000B9B RID: 2971
	// (get) Token: 0x06004CE1 RID: 19681 RVA: 0x0019BFC1 File Offset: 0x0019A3C1
	public USpeakPlayer Owner
	{
		get
		{
			return this.m_Owner;
		}
	}

	// Token: 0x06004CE2 RID: 19682 RVA: 0x0019BFC9 File Offset: 0x0019A3C9
	public static USpeakOwnerInfo FindPlayerByID(string PlayerID)
	{
		return USpeakOwnerInfo.USpeakPlayerMap[PlayerID];
	}

	// Token: 0x06004CE3 RID: 19683 RVA: 0x0019BFD6 File Offset: 0x0019A3D6
	public void Init(USpeakPlayer owner)
	{
		this.m_Owner = owner;
		USpeakOwnerInfo.USpeakPlayerMap.Add(owner.PlayerID, this);
		USpeakOwnerInfo.USpeakerMap.Add(this, base.GetComponent<USpeaker>());
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06004CE4 RID: 19684 RVA: 0x0019C00C File Offset: 0x0019A40C
	public void DeInit()
	{
		USpeakOwnerInfo.USpeakPlayerMap.Remove(this.m_Owner.PlayerID);
		USpeakOwnerInfo.USpeakerMap.Remove(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040034E9 RID: 13545
	public static Dictionary<USpeakOwnerInfo, USpeaker> USpeakerMap = new Dictionary<USpeakOwnerInfo, USpeaker>();

	// Token: 0x040034EA RID: 13546
	public static Dictionary<string, USpeakOwnerInfo> USpeakPlayerMap = new Dictionary<string, USpeakOwnerInfo>();

	// Token: 0x040034EB RID: 13547
	private USpeaker m_speaker;

	// Token: 0x040034EC RID: 13548
	private USpeakPlayer m_Owner;
}
