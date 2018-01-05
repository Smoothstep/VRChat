using System;
using UnityEngine;
using VRC;

// Token: 0x02000AF9 RID: 2809
public class Spawn : MonoBehaviour
{
	// Token: 0x060054F2 RID: 21746 RVA: 0x001D4ACD File Offset: 0x001D2ECD
	public void Start()
	{
		this.mAudio = base.GetComponent<AudioSource>();
	}

	// Token: 0x060054F3 RID: 21747 RVA: 0x001D4ADC File Offset: 0x001D2EDC
	public VRCPlayer SpawnPlayer()
	{
		VRCPlayer vrcplayer = null;
		if (PhotonNetwork.connected)
		{
			this.prefab = SpawnManager.Instance.playerPrefab;
			Vector3 eulerAngles = base.transform.eulerAngles;
			eulerAngles.x = (eulerAngles.z = 0f);
			GameObject gameObject = PhotonNetwork.Instantiate(this.prefab.name, base.transform.position, Quaternion.Euler(eulerAngles), 0);
			if (gameObject == null)
			{
				return null;
			}
			vrcplayer = gameObject.GetComponent<VRCPlayer>();
			this.mPlayer = gameObject.GetComponent<Player>();
			if (vrcplayer != null)
			{
				if (VRC.Network.IsOwner(gameObject))
				{
					VRCPlayer.Instance = vrcplayer;
				}
				this.mStation = base.GetComponent<VRC_StationInternal>();
				if (this.mStation != null)
				{
					vrcplayer.onAvatarIsReady += this.PutPlayerInStation;
				}
			}
			if (this.mAudio != null)
			{
				this.mAudio.Play();
			}
		}
		else
		{
			Debug.Log("Not connect to server.");
		}
		return vrcplayer;
	}

	// Token: 0x060054F4 RID: 21748 RVA: 0x001D4BE4 File Offset: 0x001D2FE4
	public void RespawnPlayer(VRCPlayer player)
	{
		if (player != null)
		{
			this.mPlayer = player.player;
			VRC_StationInternal.ExitAllStations(this.mPlayer);
			bool isActiveAndEnabled = VRCUiManager.Instance.isActiveAndEnabled;
			if (isActiveAndEnabled)
			{
				VRCUiManager.Instance.CloseUi(true);
			}
			this.mStation = base.GetComponent<VRC_StationInternal>();
			if (this.mStation != null)
			{
				this.PutPlayerInStation();
			}
			else
			{
				Vector3 eulerAngles = base.transform.eulerAngles;
				eulerAngles.x = (eulerAngles.z = 0f);
				this.mPlayer.playerApi.TeleportTo(base.transform.position, Quaternion.Euler(eulerAngles));
			}
		}
	}

	// Token: 0x060054F5 RID: 21749 RVA: 0x001D4C9C File Offset: 0x001D309C
	private void PutPlayerInStation()
	{
		if (this.mStation != null)
		{
			this.mStation.UseStation(this.mPlayer);
		}
	}

	// Token: 0x04003BFE RID: 15358
	[HideInInspector]
	public GameObject prefab;

	// Token: 0x04003BFF RID: 15359
	[HideInInspector]
	public string spawnId = string.Empty;

	// Token: 0x04003C00 RID: 15360
	private Player mPlayer;

	// Token: 0x04003C01 RID: 15361
	private VRC_StationInternal mStation;

	// Token: 0x04003C02 RID: 15362
	private AudioSource mAudio;
}
