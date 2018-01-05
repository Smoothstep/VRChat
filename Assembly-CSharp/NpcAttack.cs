using System;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000B56 RID: 2902
public class NpcAttack : VRCPunBehaviour
{
	// Token: 0x060058EB RID: 22763 RVA: 0x001ECEA8 File Offset: 0x001EB2A8
	public void GenerateAttack(Transform origin)
	{
		if (base.isMine)
		{
			Vector3 vector = origin.position + origin.forward * this.attackReach;
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "GenerateAttackRPC", new object[]
			{
				vector,
				this.attackRadius,
				this.damage
			});
		}
	}

	// Token: 0x060058EC RID: 22764 RVA: 0x001ECF1C File Offset: 0x001EB31C
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void GenerateAttackRPC(Vector3 pos, float radius, int damage, VRC.Player sender)
	{
		if (sender != base.Owner)
		{
			Debug.LogError("GenerateAttackRPC called by " + sender.name + " who is not the owner", this);
			return;
		}
		Vector3 position = VRCPlayer.Instance.transform.position;
		if (Vector3.Distance(position, pos) < radius)
		{
			PlayerModComponentHealth component = VRCPlayer.Instance.GetComponent<PlayerModComponentHealth>();
			if (component != null)
			{
				component.RemoveHealth((float)damage);
			}
		}
	}

	// Token: 0x04003F9C RID: 16284
	public int damage;

	// Token: 0x04003F9D RID: 16285
	public float attackRadius = 2f;

	// Token: 0x04003F9E RID: 16286
	public float attackReach = 2f;
}
