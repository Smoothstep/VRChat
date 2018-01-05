using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000AC8 RID: 2760
public class PlayerModComponentHealth : VRCPunBehaviour, IPlayerModComponent
{
	// Token: 0x060053E2 RID: 21474 RVA: 0x001CF474 File Offset: 0x001CD874
	public override IEnumerator Start()
	{
		yield return base.Start();
		this.mHealth = this.mTotalHealth;
		this.InitializeHud();
		this.ragdoll = base.GetComponentInChildren<RagdollController>();
		this.ragdoll.Prepare();
		yield break;
	}

	// Token: 0x060053E3 RID: 21475 RVA: 0x001CF490 File Offset: 0x001CD890
	private void InitializeHud()
	{
		GameObject gameObject = base.transform.Find("Profile/Health Bar").gameObject;
		if (base.isMine)
		{
			Transform transform = VRCVrCamera.GetInstance().transform.Find("HUD/Health Bar");
			if (transform != null)
			{
				gameObject = transform.gameObject;
			}
		}
		if (gameObject != null)
		{
			gameObject.SetActive(true);
			this.mHealthBar = gameObject.GetComponent<HealthBar>();
		}
	}

	// Token: 0x060053E4 RID: 21476 RVA: 0x001CF508 File Offset: 0x001CD908
	private void Update()
	{
		if (this._dead)
		{
			return;
		}
		if (this.mHealthBar == null)
		{
			this.InitializeHud();
		}
		else
		{
			this.mHealthBar.SetValue(this.mHealth / this.mTotalHealth);
		}
	}

	// Token: 0x060053E5 RID: 21477 RVA: 0x001CF558 File Offset: 0x001CD958
	public void SetProperties(List<VRCPlayerModProperty> properties)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (VRCPlayerModProperty vrcplayerModProperty in properties)
		{
			dictionary[vrcplayerModProperty.name] = vrcplayerModProperty.value();
		}
		this.mTotalHealth = (float)Tools.GetOrDefaultFromDictionary(dictionary, "totalHealth", 100f);
		this.mOnDeathAction = (VRCPlayerModFactory.HealthOnDeathAction)Tools.GetOrDefaultFromDictionary(dictionary, "onDeathAction", VRCPlayerModFactory.HealthOnDeathAction.Respawn);
	}

	// Token: 0x060053E6 RID: 21478 RVA: 0x001CF5FC File Offset: 0x001CD9FC
	public void AddHealth(float amount)
	{
		if (base.isMine)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "AddHealthRPC", new object[]
			{
				amount
			});
		}
	}

	// Token: 0x060053E7 RID: 21479 RVA: 0x001CF62C File Offset: 0x001CDA2C
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	public void AddHealthRPC(float amount, VRC.Player instigator)
	{
		if (base.Owner != instigator)
		{
			return;
		}
		if (this._dead)
		{
			return;
		}
		this.mHealth += amount;
		if (this.mHealth > this.mTotalHealth)
		{
			this.mHealth = this.mTotalHealth;
		}
	}

	// Token: 0x060053E8 RID: 21480 RVA: 0x001CF682 File Offset: 0x001CDA82
	public void RemoveHealth(float amount)
	{
		if (base.isMine)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "RemoveHealthRPC", new object[]
			{
				amount
			});
		}
	}

	// Token: 0x060053E9 RID: 21481 RVA: 0x001CF6B0 File Offset: 0x001CDAB0
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	public void RemoveHealthRPC(float amount, VRC.Player instigator)
	{
		if (base.Owner != instigator)
		{
			return;
		}
		if (this._dead)
		{
			return;
		}
		this.mHealth -= amount;
		if (this.mHealth < 0f)
		{
			this.mHealth = 0f;
		}
		if (this.mHealth <= 0f && this.DeathCoroutine == null)
		{
			this.DeathCoroutine = base.StartCoroutine(this.Die());
		}
	}

	// Token: 0x060053EA RID: 21482 RVA: 0x001CF731 File Offset: 0x001CDB31
	public void ResetHealth()
	{
		if (base.isMine)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ResetHealthRPC", new object[0]);
		}
	}

	// Token: 0x060053EB RID: 21483 RVA: 0x001CF755 File Offset: 0x001CDB55
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	public void ResetHealthRPC(VRC.Player instigator)
	{
		if (base.Owner != instigator)
		{
			return;
		}
		this._dead = false;
		this.mHealth = this.mTotalHealth;
	}

	// Token: 0x060053EC RID: 21484 RVA: 0x001CF77C File Offset: 0x001CDB7C
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	public void ShowDeath(VRC.Player instigator)
	{
		if (base.Owner != instigator)
		{
			return;
		}
		Debug.Log("RagDoll Spawned");
		this.ragdoll.Ragdoll();
	}

	// Token: 0x060053ED RID: 21485 RVA: 0x001CF7A8 File Offset: 0x001CDBA8
	private IEnumerator Die()
	{
		this._dead = true;
		if (!base.isMine)
		{
			yield break;
		}
		Debug.Log("Local Death");
		InputStateControllerManager inputControllerManager = VRCPlayer.Instance.GetComponent<InputStateControllerManager>();
		inputControllerManager.PushInputController("ImmobileInputController");
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ShowDeath", new object[0]);
		yield return new WaitForSeconds(RagdollController.DeathTime);
		inputControllerManager.PopInputController();
		VRCPlayerModFactory.HealthOnDeathAction healthOnDeathAction = this.mOnDeathAction;
		if (healthOnDeathAction != VRCPlayerModFactory.HealthOnDeathAction.Respawn)
		{
			if (healthOnDeathAction != VRCPlayerModFactory.HealthOnDeathAction.Kick)
			{
			}
			Debug.Log("Local Nothing");
			this.ResetHealth();
			SpawnManager.Instance.RespawnPlayerUsingOrder(base.Owner.vrcPlayer);
		}
		else
		{
			Debug.Log("Local Respawn");
			this.ResetHealth();
			SpawnManager.Instance.RespawnPlayerUsingOrder(base.Owner.vrcPlayer);
		}
		Debug.Log("Local Complete");
		this.DeathCoroutine = null;
		yield break;
	}

	// Token: 0x04003B2D RID: 15149
	private float mTotalHealth;

	// Token: 0x04003B2E RID: 15150
	private float mHealth;

	// Token: 0x04003B2F RID: 15151
	private HealthBar mHealthBar;

	// Token: 0x04003B30 RID: 15152
	private VRCPlayerModFactory.HealthOnDeathAction mOnDeathAction;

	// Token: 0x04003B31 RID: 15153
	private bool _dead;

	// Token: 0x04003B32 RID: 15154
	private RagdollController ragdoll;

	// Token: 0x04003B33 RID: 15155
	private Coroutine DeathCoroutine;
}
