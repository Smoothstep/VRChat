using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

// Token: 0x02000C30 RID: 3120
public class QuickMenuSocialElement : MonoBehaviour
{
	// Token: 0x17000D9F RID: 3487
	// (get) Token: 0x06006105 RID: 24837 RVA: 0x00223BBC File Offset: 0x00221FBC
	public bool isNotification
	{
		get
		{
			return this.notification != null;
		}
	}

	// Token: 0x06006106 RID: 24838 RVA: 0x00223BCC File Offset: 0x00221FCC
	public void ClearElement()
	{
		this.elementType = QuickMenuSocialElement.ElementType.Empty;
		this.user = null;
		this.notification = null;
		this.userName.text = string.Empty;
		this.image.texture = this.defaultImage;
		this.SetIcon(QuickMenuSocialElement.IconType.None, string.Empty);
	}

	// Token: 0x06006107 RID: 24839 RVA: 0x00223C1C File Offset: 0x0022201C
	public void SetupElement(APIUser u, QuickMenuSocialElement.IconType icon = QuickMenuSocialElement.IconType.None)
	{
		base.gameObject.SetActive(true);
		this.ClearElement();
		this.user = u;
		this.userName.text = this.user.displayName;
		this.SetIcon(icon, u.id);
		if (!string.IsNullOrEmpty(this.user.currentAvatarThumbnailImageUrl))
		{
			Downloader.DownloadImage(u.currentAvatarThumbnailImageUrl, delegate(string downloadedUrl, Texture2D obj)
			{
				if (u.currentAvatarThumbnailImageUrl == downloadedUrl)
				{
					this.image.texture = obj;
				}
			}, string.Empty);
		}
		this.elementType = QuickMenuSocialElement.ElementType.User;
	}

	// Token: 0x06006108 RID: 24840 RVA: 0x00223CC0 File Offset: 0x002220C0
	public void SetupElement(ApiNotification n, QuickMenuSocialElement.IconType icon = QuickMenuSocialElement.IconType.None)
	{
		APIUser cachedUser = APIUser.GetCachedUser(n.senderUserId);
		if (cachedUser != null)
		{
			this.SetupElement(cachedUser, QuickMenuSocialElement.IconType.None);
			this.SetIcon(icon, cachedUser.id);
			this.notification = n;
			this.elementType = QuickMenuSocialElement.ElementType.Notification;
		}
		else
		{
			APIUser.Fetch(n.senderUserId, delegate(APIUser user)
			{
				this.SetupElement(user, QuickMenuSocialElement.IconType.None);
				this.SetIcon(icon, user.id);
				this.notification = n;
				this.elementType = QuickMenuSocialElement.ElementType.Notification;
			}, delegate(string obj)
			{
				Debug.LogError(obj);
			});
		}
	}

	// Token: 0x06006109 RID: 24841 RVA: 0x00223D6C File Offset: 0x0022216C
	public void SetupElement(QuickMenuSocialElement e)
	{
		base.gameObject.SetActive(true);
		this.ClearElement();
		this.user = e.user;
		this.userName.text = e.user.displayName;
		this.image.texture = e.image.texture;
		this.SetIcon(e.currentIcon, this.user.id);
		this.elementType = e.elementType;
	}

	// Token: 0x0600610A RID: 24842 RVA: 0x00223DE6 File Offset: 0x002221E6
	public void HideElement()
	{
		this.ClearElement();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600610B RID: 24843 RVA: 0x00223DFC File Offset: 0x002221FC
	private void SetIcon(QuickMenuSocialElement.IconType icon, string userId)
	{
		this.currentIcon = icon;
		if (icon == QuickMenuSocialElement.IconType.None)
		{
			this.iconImage.gameObject.SetActive(false);
		}
		else
		{
			this.iconImage.gameObject.SetActive(true);
			Texture socialIcon = QuickMenuSocial.Instance.GetSocialIcon(icon);
			this.iconImage.texture = socialIcon;
		}
	}

	// Token: 0x0600610C RID: 24844 RVA: 0x00223E56 File Offset: 0x00222256
	public static QuickMenuSocialElement.IconType GetIconTypeForNotificationType(ApiNotification.NotificationType nType)
	{
		switch (nType)
		{
		case ApiNotification.NotificationType.Friendrequest:
			return QuickMenuSocialElement.IconType.FriendRequest;
		case ApiNotification.NotificationType.Invite:
			return QuickMenuSocialElement.IconType.Invite;
		case ApiNotification.NotificationType.Requestinvite:
			return QuickMenuSocialElement.IconType.RequestInvite;
		case ApiNotification.NotificationType.VoteToKick:
			return QuickMenuSocialElement.IconType.VoteToKick;
		case ApiNotification.NotificationType.Halp:
			return QuickMenuSocialElement.IconType.HelpRequest;
		default:
			return QuickMenuSocialElement.IconType.GenericNotification;
		}
	}

	// Token: 0x0600610D RID: 24845 RVA: 0x00223E84 File Offset: 0x00222284
	public void RefreshIcon()
	{
		this.SetIcon(this.currentIcon, this.user.id);
	}

	// Token: 0x04004699 RID: 18073
	public QuickMenuSocial quickMenuSocial;

	// Token: 0x0400469A RID: 18074
	public QuickMenuSocialElement.ElementType elementType;

	// Token: 0x0400469B RID: 18075
	public Texture defaultImage;

	// Token: 0x0400469C RID: 18076
	public RawImage image;

	// Token: 0x0400469D RID: 18077
	public Text userName;

	// Token: 0x0400469E RID: 18078
	public APIUser user;

	// Token: 0x0400469F RID: 18079
	public ApiNotification notification;

	// Token: 0x040046A0 RID: 18080
	public RawImage iconImage;

	// Token: 0x040046A1 RID: 18081
	private QuickMenuSocialElement.IconType currentIcon;

	// Token: 0x02000C31 RID: 3121
	public enum IconType
	{
		// Token: 0x040046A4 RID: 18084
		None = -1,
		// Token: 0x040046A5 RID: 18085
		GenericNotification,
		// Token: 0x040046A6 RID: 18086
		Friend,
		// Token: 0x040046A7 RID: 18087
		FriendRequest,
		// Token: 0x040046A8 RID: 18088
		Invite,
		// Token: 0x040046A9 RID: 18089
		Blocked,
		// Token: 0x040046AA RID: 18090
		HelpRequest,
		// Token: 0x040046AB RID: 18091
		Muted,
		// Token: 0x040046AC RID: 18092
		RequestInvite,
		// Token: 0x040046AD RID: 18093
		VoteToKick
	}

	// Token: 0x02000C32 RID: 3122
	public enum ElementType
	{
		// Token: 0x040046AF RID: 18095
		Empty,
		// Token: 0x040046B0 RID: 18096
		Notification,
		// Token: 0x040046B1 RID: 18097
		User
	}
}
