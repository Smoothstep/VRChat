using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.UI;

// Token: 0x02000C52 RID: 3154
public class UiWorldList : UiVRCList
{
	// Token: 0x060061CF RID: 25039 RVA: 0x002281E3 File Offset: 0x002265E3
	private void Update()
	{
		if (!this.isSetup)
		{
			return;
		}
		this.timeUntilUpdate -= Time.deltaTime;
		if (this.timeUntilUpdate <= 0f)
		{
			this.FetchAndRenderElements(this.currentPage);
		}
	}

	// Token: 0x060061D0 RID: 25040 RVA: 0x00228220 File Offset: 0x00226620
	protected override void FetchAndRenderElements(int page)
	{
		this.timeUntilUpdate = float.PositiveInfinity;
		ApiWorld.ReleaseStatus releaseStatus = ApiWorld.ReleaseStatus.Public;
		if (this.sortOwnership == ApiWorld.SortOwnership.Mine)
		{
			releaseStatus = ApiWorld.ReleaseStatus.All;
		}
		ApiWorld.FetchList(delegate(List<ApiWorld> worlds)
		{
			this.timeUntilUpdate = UnityEngine.Random.Range(15f, 20f);
			this.paginatedLists[page] = worlds.Cast<ApiModel>().ToList<ApiModel>();
			List<ApiWorld> list = this.paginatedLists.SelectMany((KeyValuePair<int, List<ApiModel>> w) => w.Value).Cast<ApiWorld>().ToList<ApiWorld>();
			this.RenderElements<ApiWorld>(list, page);
		}, delegate(string err)
		{
			this.timeUntilUpdate = UnityEngine.Random.Range(5f, 10f);
		}, this.sortHeading, this.sortOwnership, this.sortOrder, this.numElementsPerPage * page, this.numElementsPerPage, this.searchQuery, this.tags.ToArray(), UiWorldList.BuildExcludedTags(this.sortHeading, this.sortOwnership), this.ownerId, releaseStatus, true);
	}

	// Token: 0x060061D1 RID: 25041 RVA: 0x002282C8 File Offset: 0x002266C8
	public static string[] BuildExcludedTags(ApiWorld.SortHeading sortHeading, ApiWorld.SortOwnership sortOwnership)
	{
		if (sortHeading == ApiWorld.SortHeading.Active)
		{
			return new string[]
			{
				"admin_hide_active"
			};
		}
		if (sortHeading == ApiWorld.SortHeading.Trending)
		{
			return new string[]
			{
				"admin_hide_popular"
			};
		}
		if (sortHeading == ApiWorld.SortHeading.Created)
		{
			return new string[]
			{
				"admin_hide_new"
			};
		}
		if (sortHeading == ApiWorld.SortHeading.Updated && sortOwnership != ApiWorld.SortOwnership.Mine)
		{
			return new string[]
			{
				"admin_hide_new"
			};
		}
		return null;
	}

	// Token: 0x060061D2 RID: 25042 RVA: 0x00228338 File Offset: 0x00226738
	protected override void SetPickerContentFromApiModel(VRCUiContentButton content, object am)
	{
		ApiWorld w = (ApiWorld)am;
		content.SetDetailText(0, (w.occupants <= 0) ? string.Empty : w.occupants.ToString());
		if (content.contentId != w.id)
		{
			content.Initialize(w.thumbnailImageUrl, w.name, delegate
			{
				this.ShowRoomDetails(w);
			}, w.id);
		}
	}

	// Token: 0x060061D3 RID: 25043 RVA: 0x002283E8 File Offset: 0x002267E8
	public void ShowRoomDetails(ApiWorld w)
	{
		VRCUiPage page = VRCUiManager.Instance.GetPage("UserInterface/MenuContent/Screens/WorldInfo");
		VRCUiManager.Instance.ShowScreen(page);
		PageWorldInfo info = page.GetComponent<PageWorldInfo>();
		ApiWorld.WorldInstance winst = null;
		if (RoomManager.currentRoom != null && RoomManager.currentRoom.id == w.id)
		{
			int count = PlayerManager.GetAllPlayers().Length;
			winst = new ApiWorld.WorldInstance(RoomManager.currentRoom.currentInstanceIdWithTags, count);
		}
		info.SetupWorldInfo(w, winst, false, false);
		ApiWorld.Fetch(w.id, delegate(ApiWorld world)
		{
			info.SetupWorldInfo(world, winst, false, false);
		}, delegate(string error)
		{
			Debug.LogWarning("Could not join room: " + w.name);
		});
	}

	// Token: 0x0400476F RID: 18287
	public ApiWorld.SortHeading sortHeading;

	// Token: 0x04004770 RID: 18288
	public ApiWorld.SortOwnership sortOwnership;

	// Token: 0x04004771 RID: 18289
	public ApiWorld.SortOrder sortOrder;

	// Token: 0x04004772 RID: 18290
	public List<string> tags;

	// Token: 0x04004773 RID: 18291
	public string ownerId;

	// Token: 0x04004774 RID: 18292
	private float timeUntilUpdate;
}
