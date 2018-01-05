using System;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

// Token: 0x02000C34 RID: 3124
public class RoomTableViewCell : TableViewCell
{
	// Token: 0x06006112 RID: 24850 RVA: 0x00223F9F File Offset: 0x0022239F
	private void Awake()
	{
		this.defaultImageTexture = this.image.texture;
	}

	// Token: 0x06006113 RID: 24851 RVA: 0x00223FB4 File Offset: 0x002223B4
	public void RefreshCell(ApiWorld world)
	{
		this.mWorld = world;
		this.image.texture = this.defaultImageTexture;
		Downloader.DownloadImage(world.imageUrl, delegate(string downloadedUrl, Texture2D obj)
		{
			if (this.image != null)
			{
				this.image.texture = obj;
			}
		}, string.Empty);
		this.createdAt.text = world.createdAt.ToShortDateString();
		this.name.text = world.name;
		this.author.text = world.authorName;
		this.playerCount.text = "X";
		this.visitCount.text = "X";
		this.likeCount.text = "X";
		this.likeImage.gameObject.SetActive(false);
		this.privateImage.gameObject.SetActive(false);
	}

	// Token: 0x06006114 RID: 24852 RVA: 0x00224082 File Offset: 0x00222482
	public void JoinRoom()
	{
		VRCFlowManager.Instance.EnterRoom(this.mWorld.id, null);
	}

	// Token: 0x06006115 RID: 24853 RVA: 0x0022409A File Offset: 0x0022249A
	public void OpenRoomInfo()
	{
		VRCUiManager.Instance.popups.ShowRoomInfoPopup(this.mWorld);
	}

	// Token: 0x040046B4 RID: 18100
	public RawImage image;

	// Token: 0x040046B5 RID: 18101
	private Texture defaultImageTexture;

	// Token: 0x040046B6 RID: 18102
	public Text createdAt;

	// Token: 0x040046B7 RID: 18103
	public new Text name;

	// Token: 0x040046B8 RID: 18104
	public Text author;

	// Token: 0x040046B9 RID: 18105
	public Text playerCount;

	// Token: 0x040046BA RID: 18106
	public Text visitCount;

	// Token: 0x040046BB RID: 18107
	public Text likeCount;

	// Token: 0x040046BC RID: 18108
	public Image privateImage;

	// Token: 0x040046BD RID: 18109
	public Image likeImage;

	// Token: 0x040046BE RID: 18110
	private ApiWorld mWorld;
}
