using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

// Token: 0x02000C64 RID: 3172
public class VRCUiBlueprint : UiGroupItem
{
	// Token: 0x0600628A RID: 25226 RVA: 0x00232638 File Offset: 0x00230A38
	public override void Setup(IUIGroupItemDatasource item, Action<IUIGroupItemDatasource> onItemSelected)
	{
		base.Setup(item, onItemSelected);
		this.blueprintImage.texture = null;
		this.apiWorld = (item as ApiWorld);
		this.apiAvatar = (item as ApiAvatar);
		string imageUrl = null;
		if (this.apiAvatar != null)
		{
			this.nameText.text = this.apiAvatar.name;
			imageUrl = this.apiAvatar.imageUrl;
		}
		if (this.apiWorld != null)
		{
			this.nameText.text = this.apiWorld.name;
			imageUrl = this.apiWorld.imageUrl;
		}
		Downloader.DownloadImage(imageUrl, delegate(string downloadedUrl, Texture2D image)
		{
			if (imageUrl == downloadedUrl)
			{
				this.blueprintImage.texture = image;
			}
		}, string.Empty);
	}

	// Token: 0x040047F8 RID: 18424
	public ApiWorld apiWorld;

	// Token: 0x040047F9 RID: 18425
	public ApiAvatar apiAvatar;

	// Token: 0x040047FA RID: 18426
	public RawImage blueprintImage;

	// Token: 0x040047FB RID: 18427
	public Text nameText;

	// Token: 0x040047FC RID: 18428
	public Text authorText;
}
