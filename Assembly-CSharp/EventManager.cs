using System;
using UnityEngine;

// Token: 0x02000A7F RID: 2687
public class EventManager : MonoBehaviour
{
	// Token: 0x1400004D RID: 77
	// (add) Token: 0x060050F2 RID: 20722 RVA: 0x001BA790 File Offset: 0x001B8B90
	// (remove) Token: 0x060050F3 RID: 20723 RVA: 0x001BA7C4 File Offset: 0x001B8BC4
	public static event EventManager.EventAction OnChannelListChanged;

	// Token: 0x060050F4 RID: 20724 RVA: 0x001BA7F8 File Offset: 0x001B8BF8
	public static void onChannelListChanged()
	{
		if (EventManager.OnChannelListChanged != null)
		{
			EventManager.OnChannelListChanged();
		}
	}

	// Token: 0x1400004E RID: 78
	// (add) Token: 0x060050F5 RID: 20725 RVA: 0x001BA810 File Offset: 0x001B8C10
	// (remove) Token: 0x060050F6 RID: 20726 RVA: 0x001BA844 File Offset: 0x001B8C44
	public static event EventManager.EventAction OnServerListChanged;

	// Token: 0x060050F7 RID: 20727 RVA: 0x001BA878 File Offset: 0x001B8C78
	public static void onServerListChanged()
	{
		if (EventManager.OnServerListChanged != null)
		{
			EventManager.OnServerListChanged();
		}
	}

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x060050F8 RID: 20728 RVA: 0x001BA890 File Offset: 0x001B8C90
	// (remove) Token: 0x060050F9 RID: 20729 RVA: 0x001BA8C4 File Offset: 0x001B8CC4
	public static event EventManager.EventAction OnTeleport;

	// Token: 0x060050FA RID: 20730 RVA: 0x001BA8F8 File Offset: 0x001B8CF8
	public static void onTeleport()
	{
		if (EventManager.OnTeleport != null)
		{
			EventManager.OnTeleport();
		}
	}

	// Token: 0x14000050 RID: 80
	// (add) Token: 0x060050FB RID: 20731 RVA: 0x001BA910 File Offset: 0x001B8D10
	// (remove) Token: 0x060050FC RID: 20732 RVA: 0x001BA944 File Offset: 0x001B8D44
	public static event EventManager.EventAction OnPrivatePortalEntered;

	// Token: 0x060050FD RID: 20733 RVA: 0x001BA978 File Offset: 0x001B8D78
	public static void onPrivatePortalEntered()
	{
		if (EventManager.OnPrivatePortalEntered != null)
		{
			EventManager.OnPrivatePortalEntered();
		}
	}

	// Token: 0x14000051 RID: 81
	// (add) Token: 0x060050FE RID: 20734 RVA: 0x001BA990 File Offset: 0x001B8D90
	// (remove) Token: 0x060050FF RID: 20735 RVA: 0x001BA9C4 File Offset: 0x001B8DC4
	public static event EventManager.ChannelAction OnWorldChannelEntered;

	// Token: 0x06005100 RID: 20736 RVA: 0x001BA9F8 File Offset: 0x001B8DF8
	public static void onWorldChannelEntered(GLOBAL.WorldChannel channel)
	{
		if (EventManager.OnWorldChannelEntered != null)
		{
			EventManager.OnWorldChannelEntered(channel);
		}
	}

	// Token: 0x02000A80 RID: 2688
	// (Invoke) Token: 0x06005102 RID: 20738
	public delegate void EventAction();

	// Token: 0x02000A81 RID: 2689
	// (Invoke) Token: 0x06005106 RID: 20742
	public delegate void ChannelAction(GLOBAL.WorldChannel worldChannel);
}
