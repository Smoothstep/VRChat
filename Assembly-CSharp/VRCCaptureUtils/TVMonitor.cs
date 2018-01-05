using System;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009F7 RID: 2551
	public class TVMonitor : MonoBehaviour
	{
		// Token: 0x06004D95 RID: 19861 RVA: 0x001A0853 File Offset: 0x0019EC53
		private void Awake()
		{
			this.dataStorage = base.GetComponent<VRC_DataStorage>();
			this.cam.transform.parent = null;
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x001A0874 File Offset: 0x0019EC74
		private void OnNetworkReady()
		{
			if (VRC.Network.IsMaster)
			{
				this.currentPlayer = VRC.Network.LocalPlayer.playerApi.playerId;
				this.cam.cullingMask = this.localMask;
				this.dataStorage.data[this.NAME].valueString = VRC.Network.LocalPlayer.name;
			}
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x001A08D8 File Offset: 0x0019ECD8
		private void OnPlayerLeft(VRC_PlayerApi player)
		{
			if (VRC.Network.IsMaster && player.name == this.dataStorage.data[this.NAME].valueString)
			{
				int num = UnityEngine.Random.Range(0, VRC_PlayerApi.AllPlayers.Count - 1);
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "SetNewPlayer", new object[]
				{
					num
				});
				this.timer = 0f;
				this.timeUntilSwap = UnityEngine.Random.Range(this.minTime, this.maxTime);
			}
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x001A096B File Offset: 0x0019ED6B
		private void OnPlayerJoin(VRC_PlayerApi player)
		{
			if (VRC.Network.IsMaster)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "SetNewPlayer", new object[]
				{
					VRC_PlayerApi.AllPlayers[this.currentSelection].playerId
				});
			}
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x001A09AC File Offset: 0x0019EDAC
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.All
		})]
		private void SetNewPlayer(int playerId, VRC.Player instigator)
		{
			if (!instigator.isMaster)
			{
				return;
			}
			this.currentPlayer = playerId;
			if (playerId == VRC.Network.LocalPlayer.playerApi.playerId)
			{
				this.cam.cullingMask = this.localMask;
				VRC.Network.SetOwner(VRC.Network.LocalPlayer, this.cam.gameObject, VRC.Network.OwnershipModificationType.Request, true);
			}
			else
			{
				this.cam.cullingMask = this.remoteMask;
			}
			if (VRC.Network.IsMaster)
			{
				this.dataStorage.data[this.NAME].valueString = VRC_PlayerApi.GetPlayerById(playerId).name;
			}
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x001A0A58 File Offset: 0x0019EE58
		private void Update()
		{
			if (this.currentPlayer == VRC.Network.LocalPlayer.playerApi.playerId)
			{
				Vector3 position = VRC.Network.LocalPlayer.playerApi.GetTrackingData(VRC_PlayerApi.TrackingDataType.Head).position;
				Quaternion rotation = VRC.Network.LocalPlayer.playerApi.GetTrackingData(VRC_PlayerApi.TrackingDataType.Head).rotation;
				this.cam.transform.position = Vector3.Lerp(this.cam.transform.position, position, Time.deltaTime * this.damping);
				this.cam.transform.rotation = Quaternion.Slerp(this.cam.transform.rotation, rotation, Time.deltaTime * this.rotationDamping);
			}
			if (this.dataStorage != null)
			{
				this.followPlayerNameText.text = "Following: " + this.dataStorage.data[this.NAME].valueString;
				if (VRC.Network.IsMaster)
				{
					this.timer += Time.deltaTime;
					if (this.timer > this.timeUntilSwap)
					{
						if (VRC_PlayerApi.AllPlayers.Count > 1)
						{
							int num = 0;
							for (int i = 0; i < 5; i++)
							{
								num = UnityEngine.Random.Range(0, VRC_PlayerApi.AllPlayers.Count);
								if (VRC_PlayerApi.AllPlayers[num].playerId != this.currentPlayer)
								{
									break;
								}
							}
							Debug.Log("Setting new player: " + num);
							VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "SetNewPlayer", new object[]
							{
								VRC_PlayerApi.AllPlayers[num].playerId
							});
						}
						this.timer = 0f;
						this.timeUntilSwap = UnityEngine.Random.Range(this.minTime, this.maxTime);
					}
				}
			}
		}

		// Token: 0x0400359C RID: 13724
		public float damping = 15f;

		// Token: 0x0400359D RID: 13725
		public float rotationDamping = 7f;

		// Token: 0x0400359E RID: 13726
		public Camera cam;

		// Token: 0x0400359F RID: 13727
		public Text followPlayerNameText;

		// Token: 0x040035A0 RID: 13728
		public LayerMask localMask;

		// Token: 0x040035A1 RID: 13729
		public LayerMask remoteMask;

		// Token: 0x040035A2 RID: 13730
		public float minTime = 5f;

		// Token: 0x040035A3 RID: 13731
		public float maxTime = 30f;

		// Token: 0x040035A4 RID: 13732
		private float timeUntilSwap = 15f;

		// Token: 0x040035A5 RID: 13733
		private float timer;

		// Token: 0x040035A6 RID: 13734
		private int currentSelection;

		// Token: 0x040035A7 RID: 13735
		private int currentPlayer;

		// Token: 0x040035A8 RID: 13736
		private VRC_DataStorage dataStorage;

		// Token: 0x040035A9 RID: 13737
		private int NAME;
	}
}
