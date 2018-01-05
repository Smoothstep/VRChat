using System;
using RAIN.Memory;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000B57 RID: 2903
public class NpcMortality : MonoBehaviour
{
	// Token: 0x060058EE RID: 22766 RVA: 0x001ECF9C File Offset: 0x001EB39C
	public void ApplyDamage(float damage)
	{
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ApplyDamageRPC", new object[]
		{
			damage
		});
	}

	// Token: 0x060058EF RID: 22767 RVA: 0x001ECFC0 File Offset: 0x001EB3C0
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void ApplyDamageRPC(float damage)
	{
		int item = this.memory.GetItem<int>("health");
		this.memory.SetItem<int>("health", item - (int)damage);
	}

	// Token: 0x04003F9F RID: 16287
	public RAINMemory memory;
}
