using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;

// Token: 0x02000AB4 RID: 2740
public class NotificationManager : MonoBehaviour
{
	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x06005348 RID: 21320 RVA: 0x001CA337 File Offset: 0x001C8737
	public static NotificationManager Instance
	{
		get
		{
			return NotificationManager.instance;
		}
	}

	// Token: 0x14000070 RID: 112
	// (add) Token: 0x06005349 RID: 21321 RVA: 0x001CA340 File Offset: 0x001C8740
	// (remove) Token: 0x0600534A RID: 21322 RVA: 0x001CA378 File Offset: 0x001C8778
	public event Action<List<ApiNotification>> OnRecentNotificationsFetched;

	// Token: 0x0600534B RID: 21323 RVA: 0x001CA3B0 File Offset: 0x001C87B0
	public void FetchNotifications(NotificationManager.HistoryRange historyRange, Action<List<ApiNotification>> onSuccess = null)
	{
		if (!RoomManager.inRoom)
		{
			return;
		}
		string afterString = string.Empty;
		if (historyRange != NotificationManager.HistoryRange.AllTime)
		{
			if (historyRange == NotificationManager.HistoryRange.Recent)
			{
				afterString = "4_minutes_ago";
			}
		}
		this.ClearOldLocalNotifications();
		if (Player.Instance != null && Player.Instance.isInternal)
		{
			this.CheckAndClearHandledHalps();
		}
		ApiNotification.FetchAll(ApiNotification.NotificationType.All, false, afterString, delegate(List<ApiNotification> notifs)
		{
			if (this.notifications.ContainsKey(historyRange))
			{
				this.notifications[historyRange].Clear();
			}
			foreach (ApiNotification n in notifs)
			{
				this.AddNotification(n, historyRange);
			}
			if (onSuccess != null)
			{
				onSuccess(notifs);
			}
			if (this.OnRecentNotificationsFetched != null)
			{
				this.OnRecentNotificationsFetched(notifs);
			}
		}, delegate
		{
			Debug.LogError("Could not fetch Notifications");
		});
	}

	// Token: 0x0600534C RID: 21324 RVA: 0x001CA474 File Offset: 0x001C8874
	private void ClearOldLocalNotifications()
	{
		DateTime now = DateTime.Now;
		List<ApiNotification> notificationsOfType = this.GetNotificationsOfType(ApiNotification.NotificationType.All, NotificationManager.HistoryRange.Local);
		foreach (ApiNotification apiNotification in notificationsOfType)
		{
			if ((now - apiNotification.localCeationTime.Value).TotalMinutes > 4.0)
			{
				this.RemoveNotification(apiNotification, NotificationManager.HistoryRange.Local);
			}
		}
	}

	// Token: 0x0600534D RID: 21325 RVA: 0x001CA504 File Offset: 0x001C8904
	private void CheckAndClearHandledHalps()
	{
		List<ApiNotification> notificationsOfType = this.GetNotificationsOfType(ApiNotification.NotificationType.Halp, NotificationManager.HistoryRange.Recent);
		foreach (ApiNotification apiNotification in notificationsOfType)
		{
			List<ApiNotification> notificationsOfType2 = this.GetNotificationsOfType(ApiNotification.NotificationType.Hidden, NotificationManager.HistoryRange.Recent);
			foreach (ApiNotification apiNotification2 in notificationsOfType2)
			{
				if (apiNotification2.details.ContainsKey("halpId") && apiNotification.details["halpId"].ToString() == apiNotification2.details["halpId"].ToString())
				{
					this.RemoveNotification(apiNotification, NotificationManager.HistoryRange.Recent);
					this.RemoveNotification(apiNotification2, NotificationManager.HistoryRange.Recent);
					ApiNotification.DeleteNotification(apiNotification.id, null, null);
					ApiNotification.DeleteNotification(apiNotification2.id, null, null);
					Debug.Log("<color=purple>HALP was responded to so notify removed.</color>");
				}
			}
		}
	}

	// Token: 0x0600534E RID: 21326 RVA: 0x001CA628 File Offset: 0x001C8A28
	public void AddNotification(ApiNotification n, NotificationManager.HistoryRange historyRange)
	{
		if (!this.notifications.ContainsKey(historyRange))
		{
			this.notifications[historyRange] = new Dictionary<ApiNotification.NotificationType, List<ApiNotification>>();
		}
		if (!this.notifications[historyRange].ContainsKey(n.notificationType))
		{
			this.notifications[historyRange][n.notificationType] = new List<ApiNotification>();
		}
		if (!this.notifications[historyRange][n.notificationType].Contains(n))
		{
			this.notifications[historyRange][n.notificationType].Add(n);
		}
	}

	// Token: 0x0600534F RID: 21327 RVA: 0x001CA6D0 File Offset: 0x001C8AD0
	public void RemoveNotification(ApiNotification n, NotificationManager.HistoryRange historyRange)
	{
		if (this.notifications.ContainsKey(historyRange) && this.notifications[historyRange].ContainsKey(n.notificationType))
		{
			this.notifications[historyRange][n.notificationType].Remove(this.notifications[historyRange][n.notificationType].First((ApiNotification no) => no.id == n.id));
		}
	}

	// Token: 0x06005350 RID: 21328 RVA: 0x001CA76C File Offset: 0x001C8B6C
	public List<ApiNotification> GetNotificationsOfType(ApiNotification.NotificationType t, NotificationManager.HistoryRange historyRange)
	{
		List<ApiNotification> list = new List<ApiNotification>();
		if (this.notifications.ContainsKey(historyRange))
		{
			if (t == ApiNotification.NotificationType.All)
			{
				foreach (KeyValuePair<ApiNotification.NotificationType, List<ApiNotification>> keyValuePair in this.notifications[historyRange])
				{
					if (keyValuePair.Key != ApiNotification.NotificationType.Hidden)
					{
						list.AddRange(keyValuePair.Value);
					}
				}
			}
			else if (this.notifications[historyRange].ContainsKey(t))
			{
				list = new List<ApiNotification>(this.notifications[historyRange][t]);
			}
		}
		return list;
	}

	// Token: 0x06005351 RID: 21329 RVA: 0x001CA834 File Offset: 0x001C8C34
	public bool HasNotificationsOfType(ApiNotification.NotificationType t, NotificationManager.HistoryRange historyRange)
	{
		return this.GetNotificationsOfType(t, historyRange).Count > 0;
	}

	// Token: 0x06005352 RID: 21330 RVA: 0x001CA846 File Offset: 0x001C8C46
	private void Awake()
	{
		if (NotificationManager.instance == null)
		{
			NotificationManager.instance = this;
		}
		else
		{
			Debug.LogError("Too many instances of NotificationManage. There can only be 1.");
		}
		this.notifications = new Dictionary<NotificationManager.HistoryRange, Dictionary<ApiNotification.NotificationType, List<ApiNotification>>>();
		this.timer = this.notificationFetchPeriod;
	}

	// Token: 0x06005353 RID: 21331 RVA: 0x001CA884 File Offset: 0x001C8C84
	private void Update()
	{
		if (!RoomManager.inRoom)
		{
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer <= 0f && Player.Instance != null)
		{
			this.FetchNotifications(NotificationManager.HistoryRange.Recent, null);
			this.timer = this.notificationFetchPeriod;
		}
	}

	// Token: 0x04003AA3 RID: 15011
	private static NotificationManager instance;

	// Token: 0x04003AA5 RID: 15013
	private Dictionary<NotificationManager.HistoryRange, Dictionary<ApiNotification.NotificationType, List<ApiNotification>>> notifications;

	// Token: 0x04003AA6 RID: 15014
	public float notificationFetchPeriod = 20f;

	// Token: 0x04003AA7 RID: 15015
	private float timer;

	// Token: 0x02000AB5 RID: 2741
	public enum HistoryRange
	{
		// Token: 0x04003AAA RID: 15018
		AllTime,
		// Token: 0x04003AAB RID: 15019
		Recent,
		// Token: 0x04003AAC RID: 15020
		Local
	}
}
