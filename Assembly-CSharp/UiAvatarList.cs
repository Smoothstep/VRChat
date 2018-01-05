using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.UI;

// Token: 0x02000C3F RID: 3135
public class UiAvatarList : UiVRCList
{
	// Token: 0x06006157 RID: 24919 RVA: 0x00225CF5 File Offset: 0x002240F5
	private void Start()
	{
		this.defaultRect = this.content.transform.parent.GetComponent<RectTransform>().rect;
		if (this.category == UiAvatarList.Category.Public)
		{
			base.Expand(true);
		}
	}

	// Token: 0x06006158 RID: 24920 RVA: 0x00225D2A File Offset: 0x0022412A
	private void Update()
	{
		this.timeUntilUpdate -= Time.deltaTime;
		if (this.timeUntilUpdate <= 0f)
		{
			this.FetchAndRenderElements(this.currentPage);
		}
	}

	// Token: 0x06006159 RID: 24921 RVA: 0x00225D5C File Offset: 0x0022415C
	protected override void FetchAndRenderElements(int page)
	{
		this.timeUntilUpdate = 60f;
		if (this.category == UiAvatarList.Category.Public)
		{
			ApiAvatar.FetchList(new Action<List<ApiAvatar>>(this.AvatarsAvailable), new Action<string>(this.AvatarError), ApiAvatar.Owner.Public, null, this.numElementsPerPage, page * this.numElementsPerPage, this.heading, this.order, true);
		}
		else if (this.category == UiAvatarList.Category.Mine)
		{
			ApiAvatar.FetchList(new Action<List<ApiAvatar>>(this.AvatarsAvailable), new Action<string>(this.AvatarError), ApiAvatar.Owner.Mine, null, this.numElementsPerPage, page * this.numElementsPerPage, this.heading, this.order, true);
		}
		else if (this.category == UiAvatarList.Category.Internal && Player.Instance.isInternal)
		{
			ApiAvatar.FetchList(new Action<List<ApiAvatar>>(this.AvatarsAvailable), new Action<string>(this.AvatarError), ApiAvatar.Owner.Developer, null, this.numElementsPerPage, page * this.numElementsPerPage, this.heading, this.order, true);
		}
	}

	// Token: 0x0600615A RID: 24922 RVA: 0x00225E5E File Offset: 0x0022425E
	private void AvatarError(string err)
	{
		Debug.LogWarning(err);
	}

	// Token: 0x0600615B RID: 24923 RVA: 0x00225E68 File Offset: 0x00224268
	private void AvatarsAvailable(List<ApiAvatar> list)
	{
		Debug.Log(string.Concat(new object[]
		{
			"AvatarsAvailable, page: ",
			this.currentPage,
			" numAvatars: ",
			list.Count
		}));
		base.RenderElements<ApiAvatar>(list, this.currentPage);
	}

	// Token: 0x0600615C RID: 24924 RVA: 0x00225EC0 File Offset: 0x002242C0
	protected override void SetPickerContentFromApiModel(VRCUiContentButton content, object am)
	{
		ApiAvatar av = am as ApiAvatar;
		if (content.contentId != av.id)
		{
			content.Initialize(av.thumbnailImageUrl, av.name, delegate
			{
				Debug.Log("Picked new avatar: " + av.name);
				this.avatarPedestal.Refresh(av);
				if (this.myPage != null)
				{
					this.myPage.ChangedPreviewAvatar();
				}
			}, av.id);
		}
	}

	// Token: 0x040046F6 RID: 18166
	public SimpleAvatarPedestal avatarPedestal;

	// Token: 0x040046F7 RID: 18167
	public Dictionary<int, List<ApiModel>> devPaginatedLists;

	// Token: 0x040046F8 RID: 18168
	private const float UPDATE_PERIOD = 60f;

	// Token: 0x040046F9 RID: 18169
	private float timeUntilUpdate;

	// Token: 0x040046FA RID: 18170
	public UiAvatarList.Category category;

	// Token: 0x040046FB RID: 18171
	public ApiAvatar.SortHeading heading;

	// Token: 0x040046FC RID: 18172
	public ApiAvatar.SortOrder order;

	// Token: 0x040046FD RID: 18173
	public PageAvatar myPage;

	// Token: 0x02000C40 RID: 3136
	public enum Category
	{
		// Token: 0x040046FF RID: 18175
		Internal,
		// Token: 0x04004700 RID: 18176
		Public,
		// Token: 0x04004701 RID: 18177
		Mine
	}
}
