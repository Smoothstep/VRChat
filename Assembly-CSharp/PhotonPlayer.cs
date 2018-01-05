using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000761 RID: 1889
public class PhotonPlayer : IComparable<PhotonPlayer>, IComparable<int>, IEquatable<PhotonPlayer>, IEquatable<int>
{
	// Token: 0x06003D9B RID: 15771 RVA: 0x00136BDF File Offset: 0x00134FDF
	public PhotonPlayer(bool isLocal, int actorID, string name)
	{
		this.CustomProperties = new Hashtable();
		this.IsLocal = isLocal;
		this.actorID = actorID;
		this.nameField = name;
	}

	// Token: 0x06003D9C RID: 15772 RVA: 0x00136C19 File Offset: 0x00135019
	protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
	{
		this.CustomProperties = new Hashtable();
		this.IsLocal = isLocal;
		this.actorID = actorID;
		this.InternalCacheProperties(properties);
	}

	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x06003D9D RID: 15773 RVA: 0x00136C53 File Offset: 0x00135053
	public int ID
	{
		get
		{
			return this.actorID;
		}
	}

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x06003D9E RID: 15774 RVA: 0x00136C5B File Offset: 0x0013505B
	// (set) Token: 0x06003D9F RID: 15775 RVA: 0x00136C64 File Offset: 0x00135064
	public string NickName
	{
		get
		{
			return this.nameField;
		}
		set
		{
			if (!this.IsLocal)
			{
				Debug.LogError("Error: Cannot change the name of a remote player!");
				return;
			}
			if (string.IsNullOrEmpty(value) || value.Equals(this.nameField))
			{
				return;
			}
			this.nameField = value;
			PhotonNetwork.playerName = value;
		}
	}

	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x00136CB1 File Offset: 0x001350B1
	// (set) Token: 0x06003DA1 RID: 15777 RVA: 0x00136CB9 File Offset: 0x001350B9
	public string UserId { get; internal set; }

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x00136CC2 File Offset: 0x001350C2
	public bool IsMasterClient
	{
		get
		{
			return PhotonNetwork.networkingPeer.mMasterClientId == this.ID;
		}
	}

	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x06003DA3 RID: 15779 RVA: 0x00136CD6 File Offset: 0x001350D6
	// (set) Token: 0x06003DA4 RID: 15780 RVA: 0x00136CDE File Offset: 0x001350DE
	public bool IsInactive { get; set; }

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x06003DA5 RID: 15781 RVA: 0x00136CE7 File Offset: 0x001350E7
	// (set) Token: 0x06003DA6 RID: 15782 RVA: 0x00136CEF File Offset: 0x001350EF
	public Hashtable CustomProperties { get; internal set; }

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x06003DA7 RID: 15783 RVA: 0x00136CF8 File Offset: 0x001350F8
	public Hashtable AllProperties
	{
		get
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Merge(this.CustomProperties);
			hashtable[byte.MaxValue] = this.NickName;
			return hashtable;
		}
	}

	// Token: 0x06003DA8 RID: 15784 RVA: 0x00136D30 File Offset: 0x00135130
	public override bool Equals(object p)
	{
		PhotonPlayer photonPlayer = p as PhotonPlayer;
		return photonPlayer != null && this.GetHashCode() == photonPlayer.GetHashCode();
	}

	// Token: 0x06003DA9 RID: 15785 RVA: 0x00136D5B File Offset: 0x0013515B
	public override int GetHashCode()
	{
		return this.ID;
	}

	// Token: 0x06003DAA RID: 15786 RVA: 0x00136D63 File Offset: 0x00135163
	internal void InternalChangeLocalID(int newID)
	{
		if (!this.IsLocal)
		{
			Debug.LogError("ERROR You should never change PhotonPlayer IDs!");
			return;
		}
		this.actorID = newID;
	}

	// Token: 0x06003DAB RID: 15787 RVA: 0x00136D84 File Offset: 0x00135184
	internal void InternalCacheProperties(Hashtable properties)
	{
		if (properties == null || properties.Count == 0 || this.CustomProperties.Equals(properties))
		{
			return;
		}
		if (properties.ContainsKey(255))
		{
			this.nameField = (string)properties[byte.MaxValue];
		}
		if (properties.ContainsKey(253))
		{
			this.UserId = (string)properties[253];
		}
		if (properties.ContainsKey(254))
		{
			this.IsInactive = (bool)properties[254];
		}
		this.CustomProperties.MergeStringKeys(properties);
		this.CustomProperties.StripKeysWithNullValues();
	}

	// Token: 0x06003DAC RID: 15788 RVA: 0x00136E5C File Offset: 0x0013525C
	public void SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedValues = null, bool webForward = false)
	{
		if (propertiesToSet == null)
		{
			return;
		}
		Hashtable hashtable = propertiesToSet.StripToStringKeys();
		Hashtable hashtable2 = expectedValues.StripToStringKeys();
		bool flag = hashtable2 == null || hashtable2.Count == 0;
		bool flag2 = this.actorID > 0 && !PhotonNetwork.offlineMode;
		if (flag)
		{
			this.CustomProperties.Merge(hashtable);
			this.CustomProperties.StripKeysWithNullValues();
		}
		if (flag2)
		{
			PhotonNetwork.networkingPeer.OpSetPropertiesOfActor(this.actorID, hashtable, hashtable2, webForward);
		}
		if (!flag2 || flag)
		{
			this.InternalCacheProperties(hashtable);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[]
			{
				this,
				hashtable
			});
		}
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00136F08 File Offset: 0x00135308
	public static PhotonPlayer Find(int ID)
	{
		if (PhotonNetwork.networkingPeer != null)
		{
			return PhotonNetwork.networkingPeer.GetPlayerWithId(ID);
		}
		return null;
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00136F21 File Offset: 0x00135321
	public PhotonPlayer Get(int id)
	{
		return PhotonPlayer.Find(id);
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00136F29 File Offset: 0x00135329
	public PhotonPlayer GetNext()
	{
		return this.GetNextFor(this.ID);
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00136F37 File Offset: 0x00135337
	public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
	{
		if (currentPlayer == null)
		{
			return null;
		}
		return this.GetNextFor(currentPlayer.ID);
	}

	// Token: 0x06003DB1 RID: 15793 RVA: 0x00136F50 File Offset: 0x00135350
	public PhotonPlayer GetNextFor(int currentPlayerId)
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.networkingPeer.mActors == null || PhotonNetwork.networkingPeer.mActors.Count < 2)
		{
			return null;
		}
		Dictionary<int, PhotonPlayer> mActors = PhotonNetwork.networkingPeer.mActors;
		int num = int.MaxValue;
		int num2 = currentPlayerId;
		foreach (int num3 in mActors.Keys)
		{
			if (num3 < num2)
			{
				num2 = num3;
			}
			else if (num3 > currentPlayerId && num3 < num)
			{
				num = num3;
			}
		}
		return (num == int.MaxValue) ? mActors[num2] : mActors[num];
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x00137028 File Offset: 0x00135428
	public int CompareTo(PhotonPlayer other)
	{
		if (other == null)
		{
			return 0;
		}
		return this.GetHashCode().CompareTo(other.GetHashCode());
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x00137054 File Offset: 0x00135454
	public int CompareTo(int other)
	{
		return this.GetHashCode().CompareTo(other);
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x00137070 File Offset: 0x00135470
	public bool Equals(PhotonPlayer other)
	{
		return other != null && this.GetHashCode().Equals(other.GetHashCode());
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x0013709C File Offset: 0x0013549C
	public bool Equals(int other)
	{
		return this.GetHashCode().Equals(other);
	}

	// Token: 0x06003DB6 RID: 15798 RVA: 0x001370B8 File Offset: 0x001354B8
	public override string ToString()
	{
		if (string.IsNullOrEmpty(this.NickName))
		{
			return string.Format("#{0:00}{1}{2}", this.ID, (!this.IsInactive) ? " " : " (inactive)", (!this.IsMasterClient) ? string.Empty : "(master)");
		}
		return string.Format("'{0}'{1}{2}", this.NickName, (!this.IsInactive) ? " " : " (inactive)", (!this.IsMasterClient) ? string.Empty : "(master)");
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x00137164 File Offset: 0x00135564
	public string ToStringFull()
	{
		return string.Format("#{0:00} '{1}'{2} {3}", new object[]
		{
			this.ID,
			this.NickName,
			(!this.IsInactive) ? string.Empty : " (inactive)",
			this.CustomProperties.ToStringFull()
		});
	}

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x001371C3 File Offset: 0x001355C3
	// (set) Token: 0x06003DB9 RID: 15801 RVA: 0x001371CB File Offset: 0x001355CB
	[Obsolete("Please use NickName (updated case for naming).")]
	public string name
	{
		get
		{
			return this.NickName;
		}
		set
		{
			this.NickName = value;
		}
	}

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x06003DBA RID: 15802 RVA: 0x001371D4 File Offset: 0x001355D4
	// (set) Token: 0x06003DBB RID: 15803 RVA: 0x001371DC File Offset: 0x001355DC
	[Obsolete("Please use UserId (updated case for naming).")]
	public string userId
	{
		get
		{
			return this.UserId;
		}
		internal set
		{
			this.UserId = value;
		}
	}

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x06003DBC RID: 15804 RVA: 0x001371E5 File Offset: 0x001355E5
	[Obsolete("Please use IsLocal (updated case for naming).")]
	public bool isLocal
	{
		get
		{
			return this.IsLocal;
		}
	}

	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x06003DBD RID: 15805 RVA: 0x001371ED File Offset: 0x001355ED
	[Obsolete("Please use IsMasterClient (updated case for naming).")]
	public bool isMasterClient
	{
		get
		{
			return this.IsMasterClient;
		}
	}

	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x06003DBE RID: 15806 RVA: 0x001371F5 File Offset: 0x001355F5
	// (set) Token: 0x06003DBF RID: 15807 RVA: 0x001371FD File Offset: 0x001355FD
	[Obsolete("Please use IsInactive (updated case for naming).")]
	public bool isInactive
	{
		get
		{
			return this.IsInactive;
		}
		set
		{
			this.IsInactive = value;
		}
	}

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x00137206 File Offset: 0x00135606
	// (set) Token: 0x06003DC1 RID: 15809 RVA: 0x0013720E File Offset: 0x0013560E
	[Obsolete("Please use CustomProperties (updated case for naming).")]
	public Hashtable customProperties
	{
		get
		{
			return this.CustomProperties;
		}
		internal set
		{
			this.CustomProperties = value;
		}
	}

	// Token: 0x170009CC RID: 2508
	// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x00137217 File Offset: 0x00135617
	[Obsolete("Please use AllProperties (updated case for naming).")]
	public Hashtable allProperties
	{
		get
		{
			return this.AllProperties;
		}
	}

	// Token: 0x0400265E RID: 9822
	private int actorID = -1;

	// Token: 0x0400265F RID: 9823
	private string nameField = string.Empty;

	// Token: 0x04002661 RID: 9825
	public readonly bool IsLocal;

	// Token: 0x04002664 RID: 9828
	public object TagObject;
}
