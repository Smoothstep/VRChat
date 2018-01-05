using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000B58 RID: 2904
[RequireComponent(typeof(VRC_ObjectSync))]
public class ObjectInstantiatorHandle : VRCPunBehaviour
{
	// Token: 0x060058F1 RID: 22769 RVA: 0x001ECFFA File Offset: 0x001EB3FA
	public override void Awake()
	{
		base.Awake();
		this.TimeCreated = (double)Time.time;
	}

	// Token: 0x060058F2 RID: 22770 RVA: 0x001ED010 File Offset: 0x001EB410
	public override IEnumerator Start()
	{
		foreach (INetworkID networkID in (from c in base.GetComponentsInChildren<Component>()
		where c is INetworkID
		select c).Cast<INetworkID>())
		{
			networkID.NetworkID = this.LocalID.Value;
		}
        yield return base.Start();
		yield break;
	}

	// Token: 0x060058F3 RID: 22771 RVA: 0x001ED02B File Offset: 0x001EB42B
	private void OnDestroy()
	{
		if (this.Instantiator != null)
		{
			this.Instantiator.ReapOneObject(this);
		}
	}

	// Token: 0x060058F4 RID: 22772 RVA: 0x001ED04A File Offset: 0x001EB44A
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	public void ReapObject()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04003FA0 RID: 16288
	public ObjectInstantiator Instantiator;

	// Token: 0x04003FA1 RID: 16289
	public double TimeCreated;

	// Token: 0x04003FA2 RID: 16290
	public int? LocalID;
}
