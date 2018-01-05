using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B5D RID: 2909
public class PortalInternal : VRCPunBehaviour
{
	// Token: 0x0600592E RID: 22830 RVA: 0x001EF104 File Offset: 0x001ED504
	public static bool CreatePortal(ApiWorld targetWorld, ApiWorld.WorldInstance targetInstance, Vector3 position, Vector3 forward, bool withUIErrors = false)
	{
		if (Time.time - PortalInternal.lastInstantiation <= Time.fixedDeltaTime)
		{
			return false;
		}
		PortalInternal.lastInstantiation = Time.time;
		if (RoomManager.IsLockdown())
		{
			if (withUIErrors)
			{
				VRCUiPopupManager.Instance.ShowAlert("Cannot Create Portal", "Room is in lockdown.", 10f);
			}
			return false;
		}
		if (RoomManager.IsUserPortalForbidden())
		{
			if (withUIErrors)
			{
				VRCUiPopupManager.Instance.ShowAlert("Cannot Create Portal", "User portals are forbidden in this world.", 10f);
			}
			return false;
		}
		if (SpawnManager.Instance.IsCloseToSpawn(position, 3f))
		{
			if (withUIErrors)
			{
				VRCUiPopupManager.Instance.ShowAlert("Cannot Create Portal", "Portal too close to spawn.", 10f);
			}
			return false;
		}
		position = position + forward * 2f + Vector3.up * 1f;
		if (PlayerManager.GetAllPlayersWithinRange(position, 1.75f).Count > 0)
		{
			if (withUIErrors)
			{
				VRCUiPopupManager.Instance.ShowAlert("Cannot Create Portal", "Portal too close to a player.", 10f);
			}
			return false;
		}
		RaycastHit raycastHit;
		if (!Physics.Raycast(position, -Vector3.up, out raycastHit, 2f))
		{
			if (withUIErrors)
			{
				VRCUiPopupManager.Instance.ShowAlert("Cannot Create Portal", "Could not find surface on which to place it.", 10f);
			}
			return false;
		}
		if (ModerationManager.Instance.IsPublicOnlyBannedFromWorld(APIUser.CurrentUser.id, targetWorld.id, targetInstance.idWithTags))
		{
			if (withUIErrors)
			{
				VRCUiPopupManager.Instance.ShowAlert("Cannot Create Portal", ModerationManager.Instance.GetPublicOnlyBannedUserMessage(), 10f);
			}
			return false;
		}
		if (targetWorld.tags.Contains("admin_dont_allow_portals"))
		{
			if (withUIErrors)
			{
				VRCUiPopupManager.Instance.ShowAlert("Cannot Create Portal", "Creating portals to this world is not allowed.", 10f);
			}
			return false;
		}
		GameObject targetObject = VRC.Network.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "PortalInternalDynamic", position + Vector3.down, Quaternion.FromToRotation(Vector3.forward, forward));
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, targetObject, "ConfigurePortal", new object[]
		{
			targetWorld.id,
			targetInstance.idWithTags,
			targetInstance.count
		});
		return true;
	}

	// Token: 0x0600592F RID: 22831 RVA: 0x001EF338 File Offset: 0x001ED738
	public override IEnumerator Start()
	{
        yield return base.Start();
		if (PortalInternal.f__mg0 == null)
		{
			PortalInternal.f__mg0 = new Func<bool>(VRC.Network.Get_IsNetworkSettled);
		}
		yield return new WaitUntil(PortalInternal.f__mg0);
		if (!this.Dynamic)
		{
			this.portalTrigger = base.gameObject.GetComponentInParent<PortalTrigger>();
			this.portalMarker = base.gameObject.GetComponentInParent<VRC_PortalMarker>();
			this.FetchWorld(null);
		}
		yield break;
	}

	// Token: 0x06005930 RID: 22832 RVA: 0x001EF353 File Offset: 0x001ED753
	public void ForceShutdown()
	{
		if (base.Owner.isLocal)
		{
			this.KillPortal();
		}
	}

	// Token: 0x06005931 RID: 22833 RVA: 0x001EF36C File Offset: 0x001ED76C
	public void Enter()
	{
		if (this.isShutdown || (this.apiWorld != null && this.playerCount >= this.apiWorld.capacity))
		{
			return;
		}
		if (!this.isEntering && this.Dynamic && this.isValidWorld && !string.IsNullOrEmpty(this.RoomId) && !ModerationManager.Instance.IsPublicOnlyBannedFromWorld(APIUser.CurrentUser.id, this.RoomId, this.worldInstanceId))
		{
			this.IncrementPortalPlayerCount();
			this.ResetTimeout();
		}
		base.Invoke("EnterWorld", (float)VRC.Network.SendInterval * 2f);
		this.isEntering = true;
	}

	// Token: 0x06005932 RID: 22834 RVA: 0x001EF428 File Offset: 0x001ED828
	private void Update()
	{
		if (this.isShutdown)
		{
			return;
		}
		if (this.Dynamic)
		{
			this.UpdateTimer();
			this.UpdatePlayerCount();
			if (base.Owner != null && base.Owner.isLocal && ((SpawnManager.Instance != null && SpawnManager.Instance.IsCloseToSpawn(base.transform.position, 3f)) || RoomManager.IsLockdown() || this.dynamicTimer > 30f || (this.apiWorld != null && this.playerCount >= this.apiWorld.capacity)))
			{
				this.KillPortal();
			}
		}
		else if (this.portalMarker != null)
		{
			bool flag = false;
			if (!string.IsNullOrEmpty(this.portalMarker.roomId) && this.portalMarker.roomId != this.RoomId)
			{
				this.RoomId = this.portalMarker.roomId;
				flag = true;
			}
			if (this.portalMarker.offset != this.Offset)
			{
				this.Offset = this.portalMarker.offset;
				flag = true;
			}
			if (this.portalMarker.updateFlag)
			{
				this.portalMarker.updateFlag = false;
				flag = true;
			}
			if (flag)
			{
				this.FetchWorld(null);
			}
		}
	}

	// Token: 0x06005933 RID: 22835 RVA: 0x001EF598 File Offset: 0x001ED998
	private void OnDestroy()
	{
		RoomManager.PortalShutdown(this);
		this.isShutdown = true;
	}

	// Token: 0x06005934 RID: 22836 RVA: 0x001EF5A8 File Offset: 0x001ED9A8
	private void UpdateTimer()
	{
		if (base.Owner != null && base.Owner.isLocal)
		{
			this.dynamicTimer += Time.deltaTime;
			if (Time.time - this.lastUpdateTime >= 1f)
			{
				this.lastUpdateTime = Time.time;
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetTimerRPC", new object[]
				{
					this.dynamicTimer
				});
			}
		}
		float num = Mathf.Max(30f - this.dynamicTimer, 0f);
		if (this.timerText != null)
		{
			if (num < 10f)
			{
				this.blinkTime += Time.deltaTime;
				if ((this.blinkState && this.blinkTime > this.blinkRate.x) || (!this.blinkState && this.blinkTime > this.blinkRate.y))
				{
					this.blinkState = !this.blinkState;
					if (this.blinkState)
					{
						this.timerText.text = num.ToString("00");
					}
					else
					{
						this.timerText.text = string.Empty;
					}
					this.blinkTime = 0f;
				}
			}
			else
			{
				this.timerText.text = num.ToString("00");
			}
		}
	}

	// Token: 0x06005935 RID: 22837 RVA: 0x001EF724 File Offset: 0x001EDB24
	private void UpdatePlayerCount()
	{
		if (this.playerCountObj == null || this.playerCountText == null)
		{
			return;
		}
		if (this.apiWorld != null)
		{
			this.playerCountObj.SetActive(true);
			this.playerCountText.text = this.playerCount.ToString() + "/" + this.apiWorld.capacity;
		}
		else
		{
			this.playerCountObj.SetActive(false);
		}
	}

	// Token: 0x06005936 RID: 22838 RVA: 0x001EF7B4 File Offset: 0x001EDBB4
	private void FetchWorldFromApiQuery(Action<ApiWorld, ApiWorld.WorldInstance> onSuccess = null)
	{
		ApiWorld.FetchList(delegate(List<ApiWorld> worlds)
		{
			if (this.isShutdown)
			{
				Debug.Log(this.name + " was shutdown, ignoring query.");
				return;
			}
			worlds = (from w in worlds
			where w != null && w.releaseStatus == "public"
			select w).ToList<ApiWorld>();
			if (worlds.Count > 0)
			{
				ApiWorld.WorldInstance arg = this.ConfigurePortal(worlds[0]);
				if (onSuccess != null)
				{
					onSuccess(worlds[0], arg);
				}
			}
			else
			{
				Debug.LogError("No worlds found in query for " + this.name);
			}
		}, delegate(string msg)
		{
			this.isValidWorld = false;
			Debug.LogWarning(msg);
		}, this.SortHeading, ApiWorld.SortOwnership.Any, this.SortOrder, this.Offset, 1, this.SearchTerm, null, UiWorldList.BuildExcludedTags(this.SortHeading, ApiWorld.SortOwnership.Any), string.Empty, ApiWorld.ReleaseStatus.Public, true);
	}

	// Token: 0x06005937 RID: 22839 RVA: 0x001EF820 File Offset: 0x001EDC20
	private void FetchWorldFromId(Action<ApiWorld, ApiWorld.WorldInstance> onSuccess = null)
	{
		if (string.IsNullOrEmpty(this.RoomId))
		{
			return;
		}
		ApiWorld.Fetch(this.RoomId, delegate(ApiWorld world)
		{
			if (this.isShutdown)
			{
				Debug.Log(this.name + " was shutdown, ignoring query.");
				return;
			}
			if (world == null)
			{
				Debug.LogError(this.name + " received a null query response.");
				return;
			}
			ApiWorld.WorldInstance arg = this.ConfigurePortal(world);
			if (onSuccess != null)
			{
				onSuccess(world, arg);
			}
		}, delegate(string msg)
		{
			this.isValidWorld = false;
			Debug.LogWarning(msg);
		});
	}

	// Token: 0x06005938 RID: 22840 RVA: 0x001EF878 File Offset: 0x001EDC78
	private void SetupPortalName(string name, ApiWorld.WorldInstance winst, ApiWorld world, string creator)
	{
		if (this.portalMarker != null)
		{
			this.portalMarker.roomName = name;
		}
		if (!string.IsNullOrEmpty(name) && winst != null && base.transform != null)
		{
			string text = name;
			if (world.capacity > 1)
			{
				text = text + " #" + winst.idOnly;
			}
			if (!string.IsNullOrEmpty(creator))
			{
				text = text + "\n" + creator;
			}
			ApiWorld.WorldInstance.AccessType accessType = winst.GetAccessType();
			ApiWorld.WorldInstance.AccessDetail accessDetail = ApiWorld.WorldInstance.GetAccessDetail(accessType);
			text = text + "\n" + accessDetail.shortName;
			Transform transform = base.transform.Find("NameTag/TextMesh");
			if (transform != null)
			{
				TextMeshPro component = transform.GetComponent<TextMeshPro>();
				if (component != null)
				{
					component.text = text;
				}
			}
		}
	}

	// Token: 0x06005939 RID: 22841 RVA: 0x001EF958 File Offset: 0x001EDD58
	private void DownloadWorldImage(string url)
	{
		if (this.worldImageUrl == url)
		{
			return;
		}
		this.worldImageUrl = url;
		Downloader.DownloadImage(this.worldImageUrl, delegate(string downloadUrl, Texture2D texture)
		{
			if (this.isShutdown || base.gameObject == null || this.worldImageUrl != downloadUrl)
			{
				return;
			}
			this.portalMesh = base.GetComponentsInChildren<MeshRenderer>().FirstOrDefault((MeshRenderer mr) => mr != null && mr.gameObject != null && mr.gameObject.name == "PortalCore");
			if (this.portalMesh != null)
			{
				this.portalMesh.material.SetTexture("_WorldTex", texture);
			}
		}, string.Empty);
	}

	// Token: 0x0600593A RID: 22842 RVA: 0x001EF98F File Offset: 0x001EDD8F
	private void ResetTimeout()
	{
		if (this.isShutdown)
		{
			return;
		}
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetTimerRPC", new object[]
		{
			0f
		});
	}

	// Token: 0x0600593B RID: 22843 RVA: 0x001EF9C1 File Offset: 0x001EDDC1
	private void IncrementPortalPlayerCount()
	{
		if (this.isShutdown)
		{
			return;
		}
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBuffered, base.gameObject, "IncrementPortalPlayerCountRPC", new object[0]);
	}

	// Token: 0x0600593C RID: 22844 RVA: 0x001EF9E6 File Offset: 0x001EDDE6
	private void KillPortal()
	{
		if (!this.isShutdown)
		{
			this.isShutdown = true;
			base.Invoke("DestroyPortal", (float)VRC.Network.SendInterval * 4f);
		}
	}

	// Token: 0x0600593D RID: 22845 RVA: 0x001EFA11 File Offset: 0x001EDE11
	private void DestroyPortal()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600593E RID: 22846 RVA: 0x001EFA1E File Offset: 0x001EDE1E
	public void FetchWorld(Action<ApiWorld, ApiWorld.WorldInstance> onSuccess = null)
	{
		if (!string.IsNullOrEmpty(this.RoomId))
		{
			this.FetchWorldFromId(onSuccess);
		}
		else if (this.SortHeading != ApiWorld.SortHeading.None)
		{
			this.FetchWorldFromApiQuery(onSuccess);
		}
	}

	// Token: 0x0600593F RID: 22847 RVA: 0x001EFA4F File Offset: 0x001EDE4F
	private void EnterWorld()
	{
		this.FetchWorld(delegate(ApiWorld world, ApiWorld.WorldInstance instance)
		{
			VRCFlowManager.Instance.EnterWorld(world.id, instance.idWithTags, false, delegate(string errorMessage)
			{
				this.isEntering = false;
				VRCUiPopupManager.Instance.ShowAlert("Cannot Join World", errorMessage, 10f);
			});
		});
	}

	// Token: 0x06005940 RID: 22848 RVA: 0x001EFA63 File Offset: 0x001EDE63
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.AllBufferOne
	})]
	private void SetTimerRPC(float t, VRC.Player instigator)
	{
		if (this.isShutdown)
		{
			return;
		}
		this.dynamicTimer = t;
	}

	// Token: 0x06005941 RID: 22849 RVA: 0x001EFA78 File Offset: 0x001EDE78
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.AllBuffered
	})]
	private void IncrementPortalPlayerCountRPC(VRC.Player instigator)
	{
		if (this.isShutdown)
		{
			return;
		}
		this.playerCount++;
		this.dynamicTimer = 0f;
	}

	// Token: 0x06005942 RID: 22850 RVA: 0x001EFAA0 File Offset: 0x001EDEA0
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.AllBufferOne
	})]
	private void ConfigurePortal(string _roomId, string _idWithTags, int _playerCount, VRC.Player instigator)
	{
		this.RoomId = _roomId;
		this.CreatorId = ((!(instigator != null)) ? base.OwnerId : instigator.GetPhotonPlayerId());
		this.worldInstanceId = _idWithTags;
		this.playerCount = _playerCount;
		this.FetchWorld(null);
	}

	// Token: 0x06005943 RID: 22851 RVA: 0x001EFAF0 File Offset: 0x001EDEF0
	private ApiWorld.WorldInstance ConfigurePortal(ApiWorld world)
	{
		this.portalTrigger = base.gameObject.GetOrAddComponent<PortalTrigger>();
		this.portalTrigger.effectPrefabName = "PortalExitEffect";
		this.isValidWorld = true;
		string name = (!(this.portalMarker != null) || string.IsNullOrEmpty(this.portalMarker.customPortalName)) ? world.name : this.portalMarker.customPortalName;
		this.DownloadWorldImage(world.imageUrl);
		BoxCollider orAddComponent = base.gameObject.GetOrAddComponent<BoxCollider>();
		orAddComponent.center = new Vector3(0f, 1f, 0f);
		orAddComponent.size = new Vector3(1f, 2f, 1f);
		orAddComponent.isTrigger = true;
		ApiWorld.WorldInstance worldInstance = (!this.Dynamic) ? world.GetBestInstance(null, ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id)) : new ApiWorld.WorldInstance(this.worldInstanceId, 0);
		this.worldInstanceId = worldInstance.idWithTags;
		VRC.Player player = null;
		if (this.Dynamic)
		{
			player = VRC.Network.GetPlayerByInstigatorID(this.CreatorId);
			if (player != null)
			{
				Vector3 position = player.transform.position;
				position.y = 0f;
				Vector3 position2 = base.transform.position;
				position2.y = 0f;
				Vector3 a = position2 - position;
				base.transform.rotation = Quaternion.LookRotation(-a, Vector3.up);
			}
			if (base.transform.Find("Timer") != null)
			{
				base.transform.Find("Timer").gameObject.SetActive(true);
			}
			if (base.transform.Find("Timer/TextMesh") != null)
			{
				this.timerText = base.transform.Find("Timer/TextMesh").GetComponent<TextMeshPro>();
			}
			if (base.transform.Find("PlayerCount") != null)
			{
				this.playerCountObj = base.transform.Find("PlayerCount").gameObject;
				this.playerCountObj.SetActive(true);
			}
			if (base.transform.Find("PlayerCount/TextMesh"))
			{
				this.playerCountText = base.transform.Find("PlayerCount/TextMesh").GetComponent<TextMeshPro>();
			}
			RoomManager.PortalCreated(this);
		}
		this.SetupPortalName(name, worldInstance, world, (!(player == null)) ? player.name : string.Empty);
		this.apiWorld = world;
		return worldInstance;
	}

	// Token: 0x04003FC7 RID: 16327
	private const float DYNAMIC_TIMEOUT = 30f;

	// Token: 0x04003FC8 RID: 16328
	public int CreatorId;

	// Token: 0x04003FC9 RID: 16329
	public ApiWorld.SortHeading SortHeading;

	// Token: 0x04003FCA RID: 16330
	public ApiWorld.SortOrder SortOrder;

	// Token: 0x04003FCB RID: 16331
	public int Offset;

	// Token: 0x04003FCC RID: 16332
	public string SearchTerm;

	// Token: 0x04003FCD RID: 16333
	public string RoomId;

	// Token: 0x04003FCE RID: 16334
	public bool Dynamic = true;

	// Token: 0x04003FCF RID: 16335
	private PortalTrigger portalTrigger;

	// Token: 0x04003FD0 RID: 16336
	private VRC_PortalMarker portalMarker;

	// Token: 0x04003FD1 RID: 16337
	private ApiWorld apiWorld;

	// Token: 0x04003FD2 RID: 16338
	private MeshRenderer portalMesh;

	// Token: 0x04003FD3 RID: 16339
	private GameObject playerCountObj;

	// Token: 0x04003FD4 RID: 16340
	private TextMeshPro playerCountText;

	// Token: 0x04003FD5 RID: 16341
	private Vector2 blinkRate = new Vector2(0.3f, 0.2f);

	// Token: 0x04003FD6 RID: 16342
	private string worldImageUrl;

	// Token: 0x04003FD7 RID: 16343
	private int playerCount;

	// Token: 0x04003FD8 RID: 16344
	private bool blinkState;

	// Token: 0x04003FD9 RID: 16345
	private float blinkTime;

	// Token: 0x04003FDA RID: 16346
	private float dynamicTimer;

	// Token: 0x04003FDB RID: 16347
	private TextMeshPro timerText;

	// Token: 0x04003FDC RID: 16348
	private bool isShutdown;

	// Token: 0x04003FDD RID: 16349
	private float lastUpdateTime;

	// Token: 0x04003FDE RID: 16350
	private bool isValidWorld;

	// Token: 0x04003FDF RID: 16351
	private string worldInstanceId;

	// Token: 0x04003FE0 RID: 16352
	private bool isEntering;

	// Token: 0x04003FE1 RID: 16353
	private static float lastInstantiation;

	// Token: 0x04003FE2 RID: 16354
	[CompilerGenerated]
	private static Func<bool> f__mg0;
}
