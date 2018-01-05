using System;
using Tacticsoft;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VRC.UI
{
	// Token: 0x02000C29 RID: 3113
	public class PlayerTableViewCell : TableViewCell
	{
		// Token: 0x0600608D RID: 24717 RVA: 0x0021FF7C File Offset: 0x0021E37C
		public void RefreshCell(Player player)
		{
			this.mPlayer = player;
			this.name.text = player.name;
			float num = (!(PlayerManager.GetCurrentPlayer() != null)) ? -1f : PlayerManager.GetDistanceBetween(player, PlayerManager.GetCurrentPlayer());
			this.distance.text = num.ToString("0.0") + "m";
			if (ModerationManager.Instance.IsMuted(player.user.id))
			{
				this.muteButtonText.text = "Unmute";
			}
			else
			{
				this.muteButtonText.text = "Mute";
			}
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x00220027 File Offset: 0x0021E427
		private void Awake()
		{
			this.muteButtonText = this.muteButton.GetComponentInChildren<Text>();
			this.muteButton.onClick.AddListener(new UnityAction(this.ToggleMute));
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x00220056 File Offset: 0x0021E456
		private void Update()
		{
			this.ChangeNameColorIfTalking();
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x00220060 File Offset: 0x0021E460
		private void ToggleMute()
		{
			if (ModerationManager.Instance.IsMuted(this.mPlayer.userId))
			{
				this.mPlayer.ApplyMute(false);
				ModerationManager.Instance.UnmuteUser(this.mPlayer.userId);
			}
			else
			{
				this.mPlayer.ApplyMute(true);
				ModerationManager.Instance.MuteUser(this.mPlayer.userId);
			}
			this.RefreshCell(this.mPlayer);
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x002200DC File Offset: 0x0021E4DC
		private void ChangeNameColorIfTalking()
		{
			if (this.mPlayer.isTalking)
			{
				this.name.color = Color.red;
			}
			else if (this.name.color != Color.white)
			{
				this.name.color = Color.white;
			}
		}

		// Token: 0x04004637 RID: 17975
		public new Text name;

		// Token: 0x04004638 RID: 17976
		public Text distance;

		// Token: 0x04004639 RID: 17977
		public Button muteButton;

		// Token: 0x0400463A RID: 17978
		private Text muteButtonText;

		// Token: 0x0400463B RID: 17979
		private Player mPlayer;
	}
}
