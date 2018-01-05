using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VRC.UI
{
	// Token: 0x02000C3C RID: 3132
	public class ThisRoomPage : VRCUiPage
	{
		// Token: 0x06006147 RID: 24903 RVA: 0x00224D8A File Offset: 0x0022318A
		public override void Awake()
		{
			base.Awake();
			this.respawnButton.onClick.AddListener(new UnityAction(this.Respawn));
			this.likeButton.onClick.AddListener(new UnityAction(this.ToggleLike));
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x00224DCA File Offset: 0x002231CA
		public override void OnEnable()
		{
			base.OnEnable();
			this.RefreshRoomData();
		}

		// Token: 0x06006149 RID: 24905 RVA: 0x00224DD8 File Offset: 0x002231D8
		private void RefreshRoomData()
		{
			this.roomNameText.text = RoomManager.currentRoom.name;
			Downloader.DownloadImage(RoomManager.currentRoom.imageUrl, delegate(string downloadedUrl, Texture2D obj)
			{
				if (this.roomImage != null)
				{
					this.roomImage.texture = obj;
				}
			}, string.Empty);
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x00224E0F File Offset: 0x0022320F
		private void Respawn()
		{
			SpawnManager.Instance.RespawnPlayerUsingOrder(VRCPlayer.Instance);
		}

		// Token: 0x0600614B RID: 24907 RVA: 0x00224E20 File Offset: 0x00223220
		private void ToggleLike()
		{
			this.RefreshRoomData();
		}

		// Token: 0x040046E9 RID: 18153
		public Text roomNameText;

		// Token: 0x040046EA RID: 18154
		public RawImage roomImage;

		// Token: 0x040046EB RID: 18155
		public Text visitCountText;

		// Token: 0x040046EC RID: 18156
		public Text playerCountText;

		// Token: 0x040046ED RID: 18157
		public Text likeCountText;

		// Token: 0x040046EE RID: 18158
		public Button respawnButton;

		// Token: 0x040046EF RID: 18159
		public Button likeButton;
	}
}
