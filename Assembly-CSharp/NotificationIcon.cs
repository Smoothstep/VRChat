using System;
using UnityEngine;
using VRC.Core;

// Token: 0x02000AB3 RID: 2739
public class NotificationIcon : MonoBehaviour
{
	// Token: 0x06005346 RID: 21318 RVA: 0x001CA204 File Offset: 0x001C8604
	private void Update()
	{
		this.voteKickIcon.SetActive(false);
		this.FriendRequestIcon.SetActive(false);
		this.InviteIcon.SetActive(false);
		this.InviteRequestIcon.SetActive(false);
		this.notificationIcon.SetActive(false);
		if (NotificationManager.Instance.HasNotificationsOfType(ApiNotification.NotificationType.All, NotificationManager.HistoryRange.Recent) || NotificationManager.Instance.HasNotificationsOfType(ApiNotification.NotificationType.All, NotificationManager.HistoryRange.Local))
		{
			ApiNotification apiNotification;
			if (NotificationManager.Instance.HasNotificationsOfType(ApiNotification.NotificationType.All, NotificationManager.HistoryRange.Recent))
			{
				apiNotification = NotificationManager.Instance.GetNotificationsOfType(ApiNotification.NotificationType.All, NotificationManager.HistoryRange.Recent)[0];
			}
			else
			{
				apiNotification = NotificationManager.Instance.GetNotificationsOfType(ApiNotification.NotificationType.All, NotificationManager.HistoryRange.Local)[0];
			}
			switch (apiNotification.notificationType)
			{
			case ApiNotification.NotificationType.Friendrequest:
				this.FriendRequestIcon.SetActive(true);
				break;
			case ApiNotification.NotificationType.Invite:
				this.InviteIcon.SetActive(true);
				break;
			case ApiNotification.NotificationType.Requestinvite:
				this.InviteRequestIcon.SetActive(true);
				break;
			case ApiNotification.NotificationType.VoteToKick:
				this.voteKickIcon.SetActive(true);
				break;
			default:
				this.notificationIcon.SetActive(true);
				break;
			}
		}
	}

	// Token: 0x04003A9E RID: 15006
	public GameObject notificationIcon;

	// Token: 0x04003A9F RID: 15007
	public GameObject voteKickIcon;

	// Token: 0x04003AA0 RID: 15008
	public GameObject FriendRequestIcon;

	// Token: 0x04003AA1 RID: 15009
	public GameObject InviteIcon;

	// Token: 0x04003AA2 RID: 15010
	public GameObject InviteRequestIcon;
}
