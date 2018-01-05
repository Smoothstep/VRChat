using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057E RID: 1406
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Examples/Item Database")]
public class InvDatabase : MonoBehaviour
{
	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x06002F9C RID: 12188 RVA: 0x000E87BF File Offset: 0x000E6BBF
	public static InvDatabase[] list
	{
		get
		{
			if (InvDatabase.mIsDirty)
			{
				InvDatabase.mIsDirty = false;
				InvDatabase.mList = NGUITools.FindActive<InvDatabase>();
			}
			return InvDatabase.mList;
		}
	}

	// Token: 0x06002F9D RID: 12189 RVA: 0x000E87E0 File Offset: 0x000E6BE0
	private void OnEnable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x06002F9E RID: 12190 RVA: 0x000E87E8 File Offset: 0x000E6BE8
	private void OnDisable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x06002F9F RID: 12191 RVA: 0x000E87F0 File Offset: 0x000E6BF0
	private InvBaseItem GetItem(int id16)
	{
		int i = 0;
		int count = this.items.Count;
		while (i < count)
		{
			InvBaseItem invBaseItem = this.items[i];
			if (invBaseItem.id16 == id16)
			{
				return invBaseItem;
			}
			i++;
		}
		return null;
	}

	// Token: 0x06002FA0 RID: 12192 RVA: 0x000E8838 File Offset: 0x000E6C38
	private static InvDatabase GetDatabase(int dbID)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.databaseID == dbID)
			{
				return invDatabase;
			}
			i++;
		}
		return null;
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x000E8878 File Offset: 0x000E6C78
	public static InvBaseItem FindByID(int id32)
	{
		InvDatabase database = InvDatabase.GetDatabase(id32 >> 16);
		return (!(database != null)) ? null : database.GetItem(id32 & 65535);
	}

	// Token: 0x06002FA2 RID: 12194 RVA: 0x000E88B0 File Offset: 0x000E6CB0
	public static InvBaseItem FindByName(string exact)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			int j = 0;
			int count = invDatabase.items.Count;
			while (j < count)
			{
				InvBaseItem invBaseItem = invDatabase.items[j];
				if (invBaseItem.name == exact)
				{
					return invBaseItem;
				}
				j++;
			}
			i++;
		}
		return null;
	}

	// Token: 0x06002FA3 RID: 12195 RVA: 0x000E8924 File Offset: 0x000E6D24
	public static int FindItemID(InvBaseItem item)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.items.Contains(item))
			{
				return invDatabase.databaseID << 16 | item.id16;
			}
			i++;
		}
		return -1;
	}

	// Token: 0x040019F9 RID: 6649
	private static InvDatabase[] mList;

	// Token: 0x040019FA RID: 6650
	private static bool mIsDirty = true;

	// Token: 0x040019FB RID: 6651
	public int databaseID;

	// Token: 0x040019FC RID: 6652
	public List<InvBaseItem> items = new List<InvBaseItem>();

	// Token: 0x040019FD RID: 6653
	public UIAtlas iconAtlas;
}
