using System;
using UnityEngine;

// Token: 0x02000A60 RID: 2656
public static class ComponentExtensions
{
	// Token: 0x0600505D RID: 20573 RVA: 0x001B7E98 File Offset: 0x001B6298
	public static void BlockReady(this Component c)
	{
		if (c == null)
		{
			return;
		}
		NetworkMetadata orAddComponent = c.gameObject.GetOrAddComponent<NetworkMetadata>();
		if (orAddComponent != null)
		{
			orAddComponent.AddReadyBlock(c);
		}
	}

	// Token: 0x0600505E RID: 20574 RVA: 0x001B7ED4 File Offset: 0x001B62D4
	public static void UnblockReady(this Component c)
	{
		if (c == null)
		{
			return;
		}
		NetworkMetadata orAddComponent = c.gameObject.GetOrAddComponent<NetworkMetadata>();
		if (orAddComponent != null)
		{
			orAddComponent.RemoveReadyBlock(c);
		}
	}
}
