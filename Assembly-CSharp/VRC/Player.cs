using System;
using UnityEngine;
using VRC.Core;
using VRCSDK2;

namespace VRC
{
	// Token: 0x02000AC2 RID: 2754
	public class Player : MonoBehaviour
	{
		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06005396 RID: 21398 RVA: 0x001CD87E File Offset: 0x001CBC7E
		public PlayerNet playerNet
		{
			get
			{
				return (!(this.vrcPlayer == null)) ? this.vrcPlayer.playerNet : null;
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06005397 RID: 21399 RVA: 0x001CD8A2 File Offset: 0x001CBCA2
		public static Player Instance
		{
			get
			{
				return (!(VRCPlayer.Instance == null)) ? VRCPlayer.Instance.player : null;
			}
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06005398 RID: 21400 RVA: 0x001CD8C4 File Offset: 0x001CBCC4
		public bool isLocal
		{
			get
			{
				return this.vrcPlayer != null && this.vrcPlayer.isLocal;
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06005399 RID: 21401 RVA: 0x001CD8E8 File Offset: 0x001CBCE8
		public bool isModerator
		{
			get
			{
				return this.user != null && this.user.developerType != null && this.user.developerType.Value == APIUser.DeveloperType.Internal;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x0600539A RID: 21402 RVA: 0x001CD934 File Offset: 0x001CBD34
		public bool isVIP
		{
			get
			{
				return this.user != null && this.user.developerType != null && this.user.developerType.Value == APIUser.DeveloperType.Internal;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600539B RID: 21403 RVA: 0x001CD980 File Offset: 0x001CBD80
		public bool isInternal
		{
			get
			{
				return this.user != null && this.user.developerType != null && this.user.developerType.Value == APIUser.DeveloperType.Internal;
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x0600539C RID: 21404 RVA: 0x001CD9CC File Offset: 0x001CBDCC
		public bool isTrusted
		{
			get
			{
				return this.user != null && this.user.developerType != null && this.user.developerType.Value >= APIUser.DeveloperType.Trusted;
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x0600539D RID: 21405 RVA: 0x001CDA18 File Offset: 0x001CBE18
		public bool isMaster
		{
			get
			{
				return Network.PlayerIsMaster(this);
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x0600539E RID: 21406 RVA: 0x001CDA20 File Offset: 0x001CBE20
		public new string name
		{
			get
			{
				return this.mPhotonPlayer.NickName;
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x0600539F RID: 21407 RVA: 0x001CDA2D File Offset: 0x001CBE2D
		public bool isTalking
		{
			get
			{
				return this.mUSpeaker.IsTalking;
			}
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x060053A0 RID: 21408 RVA: 0x001CDA3C File Offset: 0x001CBE3C
		public bool isValidUser
		{
			get
			{
				return this.user != null && this.user.developerType != null;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x060053A1 RID: 21409 RVA: 0x001CDA6A File Offset: 0x001CBE6A
		public bool isRoomAuthor
		{
			get
			{
				return this.user != null && RoomManager.currentRoom != null && RoomManager.currentAuthorId == this.user.id;
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x060053A2 RID: 21410 RVA: 0x001CDA99 File Offset: 0x001CBE99
		public bool isInstanceOwner
		{
			get
			{
				return this.user != null && RoomManager.currentRoom != null && RoomManager.currentOwnerId == this.user.id;
			}
		}

		// Token: 0x060053A3 RID: 21411 RVA: 0x001CDAC8 File Offset: 0x001CBEC8
		public int GetPhotonPlayerId()
		{
			return this.mPhotonPlayer.ID;
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060053A4 RID: 21412 RVA: 0x001CDAD5 File Offset: 0x001CBED5
		public VRC_PlayerApi playerApi
		{
			get
			{
				return this.mPlayerApi;
			}
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x001CDAE0 File Offset: 0x001CBEE0
		private void Awake()
		{
			this.mUSpeaker = base.GetComponentInChildren<USpeaker>();
			this.mPlayerApi = base.GetComponent<VRC_PlayerApi>();
			if (this.vrcPlayer == null)
			{
				this.vrcPlayer = base.GetComponentInParent<VRCPlayer>();
			}
			if (this.vrcPlayer == null)
			{
				this.vrcPlayer = base.GetComponent<VRCPlayer>();
			}
			this.SetupPhotonPlayer();
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x001CDB45 File Offset: 0x001CBF45
		private void Start()
		{
			PlayerManager.MonitorPlayer(this);
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x001CDB4D File Offset: 0x001CBF4D
		private void OnNetworkReady()
		{
			this.UpdateModerations();
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x001CDB58 File Offset: 0x001CBF58
		public void UpdateModerations()
		{
			if (Network.IsOwner(base.gameObject))
			{
				ModerationManager.Instance.SelfCheckAndEnforceModerations();
				return;
			}
			bool muted = !this.vrcPlayer.canSpeak;
			bool blocked = ModerationManager.Instance.IsBlockedEitherWay(this.userId);
			this.ApplyMute(muted);
			this.ApplyBlock(blocked);
			if (this.user != null && this.user.developerType == null)
			{
				Debug.LogError(this.name + " has no developerType. SOME API ENDPOINT ISN'T PROVIDING IT.");
			}
			if (ModerationManager.Instance.IsKicked(this.userId))
			{
				Debug.LogError("Kicked user is trying to join room");
				ModerationManager.Instance.TryKickUser(this.user);
			}
		}

		// Token: 0x060053A9 RID: 21417 RVA: 0x001CDC18 File Offset: 0x001CC018
		private void SetupPhotonPlayer()
		{
			PhotonView component = base.GetComponent<PhotonView>();
			int ownerId = component.ownerId;
			this.mPhotonPlayer = PhotonPlayer.Find(ownerId);
		}

		// Token: 0x060053AA RID: 21418 RVA: 0x001CDC40 File Offset: 0x001CC040
		private void OnDestroy()
		{
			if (VRC_EventLog.Instance == null)
			{
				return;
			}
			string path = Network.GetGameObjectPath(base.gameObject);
			VRC_EventLog.Instance.RemoveEventsIf((VRC_EventLog.EventLogEntry evt) => evt.ObjectPath == path || evt.Event.ParameterObject == this.gameObject);
		}

		// Token: 0x060053AB RID: 21419 RVA: 0x001CDC93 File Offset: 0x001CC093
		public void ApplyMute(bool muted)
		{
			this.mUSpeaker.GetComponent<MuteController>().mute = muted;
		}

		// Token: 0x060053AC RID: 21420 RVA: 0x001CDCA8 File Offset: 0x001CC0A8
		public void ApplyBlock(bool blocked)
		{
			bool flag = !this.vrcPlayer.canSpeak;
			this.ApplyMute(blocked || flag);
			this.ApplyHide(blocked);
			PlayerSelector playerSelector = this.vrcPlayer.playerSelector;
			if (playerSelector != null)
			{
				playerSelector.NotifyPlayerIsBlocked(blocked);
			}
		}

		// Token: 0x060053AD RID: 21421 RVA: 0x001CDD00 File Offset: 0x001CC100
		private void ApplyHide(bool hidden)
		{
			VRCAvatarManager componentInChildren = base.GetComponentInChildren<VRCAvatarManager>();
			if (hidden)
			{
				componentInChildren.SwitchToInvisibleAvatar(1f);
			}
			else
			{
				componentInChildren.SwitchToLastAvatar(1f);
			}
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x001CDD35 File Offset: 0x001CC135
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04003AFC RID: 15100
		public APIUser user;

		// Token: 0x04003AFD RID: 15101
		public VRCPlayer vrcPlayer;

		// Token: 0x04003AFE RID: 15102
		private PhotonPlayer mPhotonPlayer;

		// Token: 0x04003AFF RID: 15103
		private USpeaker mUSpeaker;

		// Token: 0x04003B00 RID: 15104
		public string userId;

		// Token: 0x04003B01 RID: 15105
		private VRC_PlayerApi mPlayerApi;

		// Token: 0x04003B02 RID: 15106
		public VRC_StationInternal currentStation;
	}
}
