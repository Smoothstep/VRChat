using System;
using System.Collections;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000AD1 RID: 2769
public class VRC_DestructiblePlayer : VRCPunBehaviour, IVRC_Destructible
{
	// Token: 0x0600541A RID: 21530 RVA: 0x001D0A7C File Offset: 0x001CEE7C
	public float GetMaxHealth()
	{
		return this.maxHealth;
	}

	// Token: 0x0600541B RID: 21531 RVA: 0x001D0A84 File Offset: 0x001CEE84
	public float GetCurrentHealth()
	{
		return this.currentHealth;
	}

	// Token: 0x0600541C RID: 21532 RVA: 0x001D0A8C File Offset: 0x001CEE8C
	public override IEnumerator Start()
	{
		yield return base.Start();
		base.ObserveThis();
		this.currentHealth = this.maxHealth;
		this.theCombatSystem = VRC_CombatSystem.GetInstance();
		this.ragdoll = base.GetComponentInChildren<RagdollController>();
		this.ragdoll.Prepare();
		this.SetupVisualDamage();
		if (this.theCombatSystem.onSetupPlayer != null)
		{
			this.theCombatSystem.onSetupPlayer(base.GetComponent<VRC_PlayerApi>());
		}
		yield break;
	}

	// Token: 0x0600541D RID: 21533 RVA: 0x001D0AA8 File Offset: 0x001CEEA8
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Local
	})]
	private void SetupVisualDamage()
	{
		Transform trackedTransform = InternalSDKPlayer.GetTrackedTransform(Networking.LocalPlayer, VRC_PlayerApi.TrackingDataType.Head);
		GameObject gameObject = this.theCombatSystem.visualDamagePrefab;
		if (gameObject == null)
		{
			gameObject = (Resources.Load("VRC_PlayerVisualDamage") as GameObject);
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject2.transform.SetParent(trackedTransform);
		gameObject2.transform.localPosition = new Vector3(-0.008f, 0.003f, 0.332f);
		gameObject2.transform.localScale = new Vector3(32f, 32f, 32f);
		gameObject2.transform.localRotation = Quaternion.identity;
		this.localVisualDamage = gameObject2.GetComponent<VRC_VisualDamage>();
		this.localVisualDamage.SetDamagePercent(0f);
	}

	// Token: 0x0600541E RID: 21534 RVA: 0x001D0B68 File Offset: 0x001CEF68
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Local
	})]
	public void ApplyDamage(float damage)
	{
		if (this.currentHealth > 0f)
		{
			this.currentHealth -= damage;
			if (this.theCombatSystem.onPlayerDamaged != null)
			{
				this.theCombatSystem.onPlayerDamaged(base.GetComponent<VRC_PlayerApi>());
			}
			if (this.theCombatSystem.onPlayerDamagedTrigger != null)
			{
				VRC_Trigger.TriggerCustom(this.theCombatSystem.onPlayerDamagedTrigger);
			}
			if (this.currentHealth <= 0f)
			{
				this.currentHealth = 0f;
				if (this.DeathCoroutine == null)
				{
					this.DeathCoroutine = base.StartCoroutine(this.Die());
				}
				if (this.theCombatSystem.onPlayerKilledTrigger != null)
				{
					VRC_Trigger.TriggerCustom(this.theCombatSystem.onPlayerKilledTrigger);
				}
			}
			this.localVisualDamage.SetDamagePercent(1f - this.currentHealth / this.maxHealth);
		}
	}

	// Token: 0x0600541F RID: 21535 RVA: 0x001D0C50 File Offset: 0x001CF050
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Local
	})]
	public void ApplyHealing(float healing)
	{
		if (this.currentHealth < this.maxHealth)
		{
			this.currentHealth += healing;
			if (this.theCombatSystem.onPlayerHealed != null)
			{
				this.theCombatSystem.onPlayerHealed(base.GetComponent<VRC_PlayerApi>());
			}
			if (this.theCombatSystem.onPlayerHealedTrigger != null)
			{
				VRC_Trigger.TriggerCustom(this.theCombatSystem.onPlayerHealedTrigger);
			}
			if (this.currentHealth >= this.maxHealth)
			{
				this.currentHealth = this.maxHealth;
			}
		}
		this.localVisualDamage.SetDamagePercent(1f - this.currentHealth / this.maxHealth);
	}

	// Token: 0x06005420 RID: 21536 RVA: 0x001D0CFD File Offset: 0x001CF0FD
	public void ResetHealth()
	{
		if (base.isMine)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ResetHealthRPC", new object[0]);
		}
	}

	// Token: 0x06005421 RID: 21537 RVA: 0x001D0D24 File Offset: 0x001CF124
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void ResetHealthRPC(VRC.Player sender)
	{
		if (base.Owner != sender)
		{
			Debug.LogError("ResetHealthRPC called by " + sender.name + " who is not the owner", this);
			return;
		}
		this.currentHealth = this.maxHealth;
		this.localVisualDamage.SetDamagePercent(0f);
	}

	// Token: 0x06005422 RID: 21538 RVA: 0x001D0D7C File Offset: 0x001CF17C
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	public void ShowDeath(VRC.Player sender)
	{
		if (base.Owner != sender)
		{
			Debug.LogError("ShowDeath called by " + sender.name + " who is not the owner", this);
			return;
		}
		Debug.Log("RagDoll Spawned");
		this.ragdoll.Ragdoll();
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x001D0DCC File Offset: 0x001CF1CC
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	public void EndDeath(VRC.Player sender)
	{
		if (base.Owner != sender)
		{
			Debug.LogError("EndDeath called by " + sender.name + " who is not the owner", this);
			return;
		}
		Debug.Log("RagDoll Ended");
		this.ragdoll.EndRagdoll();
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x001D0E1C File Offset: 0x001CF21C
	private IEnumerator Die()
	{
		if (!base.photonView.isMine)
		{
			yield break;
		}
		VRC_PlayerApi localPlayerApi = base.GetComponent<VRC_PlayerApi>();
		Debug.Log("Local Death");
		InputStateControllerManager inputControllerManager = VRCPlayer.Instance.GetComponent<InputStateControllerManager>();
		inputControllerManager.PushInputController("ImmobileInputController");
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "ShowDeath", new object[0]);
		VRC_Pickup leftPickup = localPlayerApi.GetPickupInHand(VRC_Pickup.PickupHand.Left);
		if (leftPickup != null)
		{
			leftPickup.Drop();
		}
		VRC_Pickup rightPickup = localPlayerApi.GetPickupInHand(VRC_Pickup.PickupHand.Right);
		if (rightPickup != null)
		{
			rightPickup.Drop();
		}
		localPlayerApi.EnablePickups(false);
		if (this.theCombatSystem.onPlayerKilled != null)
		{
			this.theCombatSystem.onPlayerKilled(base.GetComponent<VRC_PlayerApi>());
		}
		if (this.theCombatSystem.respawnOnDeath)
		{
			yield return new WaitForSeconds(this.theCombatSystem.respawnTime);
			this.Respawn();
		}
		else
		{
			yield return null;
		}
		this.DeathCoroutine = null;
		yield break;
	}

	// Token: 0x06005425 RID: 21541 RVA: 0x001D0E38 File Offset: 0x001CF238
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Local
	})]
	private void Respawn()
	{
		InputStateControllerManager component = VRCPlayer.Instance.GetComponent<InputStateControllerManager>();
		component.PopInputController();
		Networking.LocalPlayer.EnablePickups(true);
		Debug.Log("Local Respawn");
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "EndDeath", new object[0]);
		if (this.theCombatSystem.resetHealthOnRespawn)
		{
			this.ResetHealth();
		}
		if (this.theCombatSystem.respawnPoint != null)
		{
			Networking.LocalPlayer.TeleportTo(this.theCombatSystem.respawnPoint.position, this.theCombatSystem.respawnPoint.rotation);
		}
	}

	// Token: 0x06005426 RID: 21542 RVA: 0x001D0ED9 File Offset: 0x001CF2D9
	public object[] GetState()
	{
		return new object[]
		{
			this.currentHealth
		};
	}

	// Token: 0x06005427 RID: 21543 RVA: 0x001D0EEF File Offset: 0x001CF2EF
	public void SetState(object[] state)
	{
		this.currentHealth = (float)state[0];
	}

	// Token: 0x06005428 RID: 21544 RVA: 0x001D0F00 File Offset: 0x001CF300
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			object[] state = this.GetState();
			stream.SendNext(state.Length);
			foreach (object obj in state)
			{
				stream.SendNext(obj);
			}
		}
		else
		{
			int num = (int)stream.ReceiveNext();
			object[] array2 = new object[num];
			for (int j = 0; j < num; j++)
			{
				object obj2 = stream.ReceiveNext();
				if (obj2 == null)
				{
					Debug.LogError("DestructiblePlayer: Error deserializing state, idx=" + j);
				}
				array2[j] = obj2;
			}
			this.SetState(array2);
		}
	}

	// Token: 0x04003B5B RID: 15195
	private VRC_CombatSystem theCombatSystem;

	// Token: 0x04003B5C RID: 15196
	public float maxHealth = 100f;

	// Token: 0x04003B5D RID: 15197
	public float currentHealth = 100f;

	// Token: 0x04003B5E RID: 15198
	private RagdollController ragdoll;

	// Token: 0x04003B5F RID: 15199
	private Coroutine DeathCoroutine;

	// Token: 0x04003B60 RID: 15200
	private VRC_VisualDamage localVisualDamage;
}
