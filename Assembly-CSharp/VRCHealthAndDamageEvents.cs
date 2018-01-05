using System;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000B69 RID: 2921
public class VRCHealthAndDamageEvents : MonoBehaviour
{
	// Token: 0x060059EB RID: 23019 RVA: 0x001F3B68 File Offset: 0x001F1F68
	private void Add(int instigatorPhotonId, bool isHealth)
	{
		PhotonView photonView = PhotonView.Find(instigatorPhotonId);
		if (photonView != null && photonView.isMine)
		{
			GameObject gameObject = photonView.gameObject;
			if (gameObject != null)
			{
				PlayerModComponentHealth component = gameObject.GetComponent<PlayerModComponentHealth>();
				if (component != null)
				{
					if (this.healthBase != null)
					{
						component.AddHealth(this.healthBase.healthAmount);
					}
					if (this.damageBase != null)
					{
						component.RemoveHealth(this.damageBase.damageAmount);
					}
				}
			}
		}
	}

	// Token: 0x060059EC RID: 23020 RVA: 0x001F3BFD File Offset: 0x001F1FFD
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	public void AddHealth(int instigatorPhotonId)
	{
		this.Add(instigatorPhotonId, true);
	}

	// Token: 0x060059ED RID: 23021 RVA: 0x001F3C07 File Offset: 0x001F2007
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	public void AddDamage(int instigatorPhotonId)
	{
		this.Add(instigatorPhotonId, false);
	}

	// Token: 0x0400401C RID: 16412
	public VRC_AddDamage damageBase;

	// Token: 0x0400401D RID: 16413
	public VRC_AddHealth healthBase;
}
