using System;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B5E RID: 2910
public class PortalTrigger : MonoBehaviour
{
	// Token: 0x0600594B RID: 22859 RVA: 0x001F0162 File Offset: 0x001EE562
	private void Awake()
	{
		this.pi = base.gameObject.GetComponentInChildren<PortalInternal>();
	}

	// Token: 0x0600594C RID: 22860 RVA: 0x001F0178 File Offset: 0x001EE578
	private void OnTriggerEnter(Collider other)
	{
		VRCPlayer componentInParent = other.gameObject.GetComponentInParent<VRCPlayer>();
		if (componentInParent != null && componentInParent.isLocal && this.IsObserving(componentInParent) && this.IsMovingTowards(componentInParent))
		{
			if (Time.time - this.LastEffectTime > 2f)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "PlayEffect", new object[0]);
			}
			this.pi.Enter();
		}
	}

	// Token: 0x0600594D RID: 22861 RVA: 0x001F01F8 File Offset: 0x001EE5F8
	private bool IsObserving(VRCPlayer playerObject)
	{
		return Mathf.Abs(Mathf.Repeat(playerObject.transform.rotation.eulerAngles.y, 360f) - Mathf.Repeat(Quaternion.LookRotation(base.transform.position - playerObject.transform.position, Vector3.up).eulerAngles.y, 360f)) <= 45f;
	}

	// Token: 0x0600594E RID: 22862 RVA: 0x001F027C File Offset: 0x001EE67C
	private bool IsMovingTowards(VRCPlayer playerObject)
	{
		SyncPhysics component = playerObject.GetComponent<SyncPhysics>();
		Vector3 observedVelocity = component.ObservedVelocity;
		Vector3 rhs = base.transform.position - playerObject.transform.position;
		return Vector3.Dot(observedVelocity, rhs) > 0f;
	}

	// Token: 0x0600594F RID: 22863 RVA: 0x001F02C4 File Offset: 0x001EE6C4
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void PlayEffect(VRC.Player instigator)
	{
		if (instigator == null)
		{
			return;
		}
		this.LastEffectTime = Time.time;
		if (this.effectPrefabName != null && this.effectPrefabName != string.Empty)
		{
			if (this.effectPrefab == null)
			{
				this.effectPrefab = (GameObject)Resources.Load(this.effectPrefabName);
			}
			if (this.effectPrefab)
			{
				AssetManagement.Instantiate(this.effectPrefab, instigator.transform.position, instigator.transform.rotation);
			}
		}
	}

	// Token: 0x04003FE4 RID: 16356
	public string effectPrefabName;

	// Token: 0x04003FE5 RID: 16357
	public GameObject effectPrefab;

	// Token: 0x04003FE6 RID: 16358
	private PortalInternal pi;

	// Token: 0x04003FE7 RID: 16359
	private float LastEffectTime;

	// Token: 0x04003FE8 RID: 16360
	private const float EffectTimeLimit = 2f;
}
