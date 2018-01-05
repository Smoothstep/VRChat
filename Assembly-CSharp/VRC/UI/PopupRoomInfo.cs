using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Core;

namespace VRC.UI
{
	// Token: 0x02000C2B RID: 3115
	public class PopupRoomInfo : VRCUiPopup
	{
		// Token: 0x0600609C RID: 24732 RVA: 0x002205F9 File Offset: 0x0021E9F9
		public override void Initialize(string title, string body)
		{
			this.ClearRoomInfo();
			base.Initialize(title, string.Empty);
		}

		// Token: 0x0600609D RID: 24733 RVA: 0x0022060D File Offset: 0x0021EA0D
		public override void Awake()
		{
			base.Awake();
			this.defaultImageTexture = this.roomImage.texture;
		}

		// Token: 0x0600609E RID: 24734 RVA: 0x00220628 File Offset: 0x0021EA28
		private void ClearRoomInfo()
		{
			this.mWorld = null;
			this.roomName.text = string.Empty;
			this.roomAuthor.text = string.Empty;
			this.roomImage.texture = this.defaultImageTexture;
			this.closeRoomButton.gameObject.SetActive(true);
			this.joinButton.onClick.RemoveAllListeners();
			this.portalButton.onClick.RemoveAllListeners();
			this.closeRoomButton.onClick.RemoveAllListeners();
		}

		// Token: 0x0600609F RID: 24735 RVA: 0x002206B0 File Offset: 0x0021EAB0
		public void SetupRoomInfo(ApiWorld world)
		{
			this.mWorld = world;
			this.roomName.text = world.name;
			this.roomAuthor.text = "By " + world.authorName;
			Downloader.DownloadImage(world.imageUrl, delegate(string downloadedUrl, Texture2D obj)
			{
				this.roomImage.texture = obj;
			}, string.Empty);
			this.joinButton.onClick.AddListener(new UnityAction(this.JoinRoom));
			this.closeRoomButton.gameObject.SetActive(false);
			if (RoomManager.inRoom && !ModerationManager.Instance.IsBannedFromPublicOnly(APIUser.CurrentUser.id))
			{
				this.portalButton.gameObject.SetActive(true);
				this.portalButton.onClick.AddListener(new UnityAction(this.CreatePortal));
			}
			else
			{
				this.portalButton.gameObject.SetActive(false);
			}
		}

		// Token: 0x060060A0 RID: 24736 RVA: 0x0022079F File Offset: 0x0021EB9F
		private void JoinRoom()
		{
			VRCFlowManager.Instance.EnterRoom(this.mWorld.id, null);
		}

		// Token: 0x060060A1 RID: 24737 RVA: 0x002207B8 File Offset: 0x0021EBB8
		private void CreatePortal()
		{
			if (RoomManager.inRoom)
			{
				PortalInternal.CreatePortal(this.mWorld, this.mWorld.GetBestInstance(null, false), VRCPlayer.Instance.transform.position, VRCPlayer.Instance.transform.forward, true);
			}
		}

		// Token: 0x04004640 RID: 17984
		public RawImage roomImage;

		// Token: 0x04004641 RID: 17985
		public Text roomName;

		// Token: 0x04004642 RID: 17986
		public Text roomAuthor;

		// Token: 0x04004643 RID: 17987
		public Button joinButton;

		// Token: 0x04004644 RID: 17988
		public Button portalButton;

		// Token: 0x04004645 RID: 17989
		public Button closeRoomButton;

		// Token: 0x04004646 RID: 17990
		private ApiWorld mWorld;

		// Token: 0x04004647 RID: 17991
		private Texture defaultImageTexture;
	}
}
