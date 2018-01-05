using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;

// Token: 0x02000C2E RID: 3118
public class QuickMenuSocial : MonoBehaviour
{
	// Token: 0x060060FE RID: 24830 RVA: 0x00223654 File Offset: 0x00221A54
	public Texture GetSocialIcon(QuickMenuSocialElement.IconType iconType)
	{
		Texture result = null;
		if (iconType > QuickMenuSocialElement.IconType.None)
		{
			result = this.socialIcons[(int)iconType];
		}
		return result;
	}

	// Token: 0x060060FF RID: 24831 RVA: 0x00223674 File Offset: 0x00221A74
	private void Awake()
	{
		if (QuickMenuSocial.Instance == null)
		{
			QuickMenuSocial.Instance = this;
		}
		else
		{
			Debug.LogError("Too many QuickMenuSocial Instances");
		}
	}

	// Token: 0x06006100 RID: 24832 RVA: 0x0022369B File Offset: 0x00221A9B
	private void OnEnable()
	{
		this.ClearAllElements();
		this.SetupNotifications();
	}

	// Token: 0x06006101 RID: 24833 RVA: 0x002236AC File Offset: 0x00221AAC
	private void ClearAllElements()
	{
		foreach (QuickMenuSocialElement quickMenuSocialElement in this.elements)
		{
			quickMenuSocialElement.HideElement();
		}
	}

	// Token: 0x06006102 RID: 24834 RVA: 0x002236E0 File Offset: 0x00221AE0
	private void SetupNotifications()
	{
		Action<List<ApiNotification>> SetupNotifs = delegate(List<ApiNotification> notifs)
		{
			List<ApiNotification> notificationsOfType4 = NotificationManager.Instance.GetNotificationsOfType(ApiNotification.NotificationType.VoteToKick, NotificationManager.HistoryRange.Local);
			notifs.AddRange(notificationsOfType4);
			int num = 0;
			for (int i = 0; i < this.elements.Length; i++)
			{
				if (num < notifs.Count)
				{
					ApiNotification apiNotification = notifs[num];
					if (apiNotification.notificationType == ApiNotification.NotificationType.Friendrequest)
					{
						apiNotification.message = apiNotification.senderUsername + " would like to be your friend";
					}
					if (this.elements[i].elementType == QuickMenuSocialElement.ElementType.Empty || this.elements[i].elementType == QuickMenuSocialElement.ElementType.Notification)
					{
						QuickMenuSocialElement.IconType iconTypeForNotificationType = QuickMenuSocialElement.GetIconTypeForNotificationType(apiNotification.notificationType);
						this.elements[i].SetupElement(apiNotification, iconTypeForNotificationType);
						num++;
					}
				}
			}
		};
		List<ApiNotification> priority = NotificationManager.Instance.GetNotificationsOfType(ApiNotification.NotificationType.Halp, NotificationManager.HistoryRange.Recent);
		List<ApiNotification> notificationsOfType = NotificationManager.Instance.GetNotificationsOfType(ApiNotification.NotificationType.Invite, NotificationManager.HistoryRange.Recent);
		List<ApiNotification> notificationsOfType2 = NotificationManager.Instance.GetNotificationsOfType(ApiNotification.NotificationType.Requestinvite, NotificationManager.HistoryRange.Recent);
		List<ApiNotification> notificationsOfType3 = NotificationManager.Instance.GetNotificationsOfType(ApiNotification.NotificationType.Friendrequest, NotificationManager.HistoryRange.Recent);
		priority.AddRange(notificationsOfType);
		priority.AddRange(notificationsOfType2);
		priority.AddRange(notificationsOfType3);
		SetupNotifs(priority);
		if (this.dontFetchNotificationsForOnePeriod)
		{
			this.dontFetchNotificationsForOnePeriod = false;
		}
		else
		{
			NotificationManager.Instance.FetchNotifications(NotificationManager.HistoryRange.Recent, delegate(List<ApiNotification> notifs)
			{
				notifs.RemoveAll((ApiNotification x) => x.notificationType == ApiNotification.NotificationType.Hidden);
				IEnumerable<string> source = (from x in priority
				select x.id).Except(from x in notifs
				select x.id);
				bool flag = source.Count<string>() > 0 || priority.Count != notifs.Count;
				if (flag)
				{
					SetupNotifs(notifs);
				}
			});
		}
	}

	// Token: 0x06006103 RID: 24835 RVA: 0x002237A8 File Offset: 0x00221BA8
	private void SetupNearbyUsers()
	{
		if (RoomManager.inRoom)
		{
			List<Player> players = PlayerManager.GetAllPlayersByDistance();
			players.Remove(PlayerManager.GetCurrentPlayer());
			Action<List<APIUser>> SetupPlayers = delegate(List<APIUser> users)
			{
				if (users == null)
				{
					return;
				}
				int userIndex = 0;
				for (int i = 0; i < this.elements.Length; i++)
				{
					if (userIndex < players.Count && (this.elements[i].elementType == QuickMenuSocialElement.ElementType.Empty || this.elements[i].elementType == QuickMenuSocialElement.ElementType.User) && users.Any((APIUser u) => u.id == players[userIndex].userId))
					{
						this.elements[i].SetupElement(users.Find((APIUser u) => u.id == players[userIndex].userId), QuickMenuSocialElement.IconType.None);
						userIndex++;
					}
				}
			};
			List<APIUser> lastFetchedUserList = PlayerManager.GetLastUserListFetch();
			SetupPlayers(lastFetchedUserList);
			PlayerManager.FetchUsersInWorldInstance(delegate(List<APIUser> fetchedUserList)
			{
				IEnumerable<string> source = (from x in lastFetchedUserList
				select x.id).Except(from x in fetchedUserList
				select x.id);
				bool flag = source.Count<string>() > 0 || lastFetchedUserList.Count != fetchedUserList.Count;
				if (flag)
				{
					SetupPlayers(fetchedUserList);
				}
			}, null);
		}
	}

	// Token: 0x04004692 RID: 18066
	public static QuickMenuSocial Instance;

	// Token: 0x04004693 RID: 18067
	public bool dontFetchNotificationsForOnePeriod;

	// Token: 0x04004694 RID: 18068
	public QuickMenuSocialElement[] elements;

	// Token: 0x04004695 RID: 18069
	public Texture[] socialIcons;

	// Token: 0x02000C2F RID: 3119
	public enum SocialType
	{
		// Token: 0x04004697 RID: 18071
		User,
		// Token: 0x04004698 RID: 18072
		Notification
	}
}
