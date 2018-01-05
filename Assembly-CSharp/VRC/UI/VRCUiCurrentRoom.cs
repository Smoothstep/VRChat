using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Core;

namespace VRC.UI
{
	// Token: 0x02000C67 RID: 3175
	public class VRCUiCurrentRoom : MonoBehaviour
	{
		// Token: 0x0600629C RID: 25244 RVA: 0x00232AA8 File Offset: 0x00230EA8
		private void Awake()
		{
			if (this.respawnButton)
			{
				this.respawnButton.onClick.AddListener(new UnityAction(this.Respawn));
			}
			if (this.likeButton)
			{
				this.likeButton.onClick.AddListener(new UnityAction(this.ToggleLike));
			}
		}

		// Token: 0x0600629D RID: 25245 RVA: 0x00232B0D File Offset: 0x00230F0D
		private void OnEnable()
		{
			this.RefreshRoomData();
		}

		// Token: 0x0600629E RID: 25246 RVA: 0x00232B18 File Offset: 0x00230F18
		private void RefreshRoomData()
		{
			string text = "You Are In: ";
			if (RoomManager.currentRoom != null)
			{
				ApiWorld.WorldInstance.AccessDetail accessDetail = ApiWorld.WorldInstance.GetAccessDetail(RoomManager.currentRoom.currentInstanceAccess);
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					RoomManager.currentRoom.name,
					" #",
					RoomManager.currentRoom.currentInstanceIdOnly,
					" ",
					accessDetail.shortName
				});
			}
			this.roomNameText.text = text;
		}

		// Token: 0x0600629F RID: 25247 RVA: 0x00232B96 File Offset: 0x00230F96
		private void Respawn()
		{
			SpawnManager.Instance.RespawnPlayerUsingOrder(VRCPlayer.Instance);
		}

		// Token: 0x060062A0 RID: 25248 RVA: 0x00232BA7 File Offset: 0x00230FA7
		private void ToggleLike()
		{
			this.RefreshRoomData();
		}

		// Token: 0x0400480C RID: 18444
		public Text roomNameText;

		// Token: 0x0400480D RID: 18445
		public Button respawnButton;

		// Token: 0x0400480E RID: 18446
		public Button likeButton;
	}
}
