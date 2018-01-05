using System;
using System.Collections.Generic;
using System.Linq;
using VRC;
using VRC.Core;
using VRC.UI;

// Token: 0x02000C51 RID: 3153
public class UiWorldInstanceList : UiVRCList
{
	// Token: 0x060061C7 RID: 25031 RVA: 0x00227E15 File Offset: 0x00226215
	public new void Refresh()
	{
		this.FetchAndRenderElements(0);
	}

	// Token: 0x060061C8 RID: 25032 RVA: 0x00227E20 File Offset: 0x00226220
	protected override void FetchAndRenderElements(int page)
	{
		if (this.world != null)
		{
			int num = 0;
			List<ApiWorld.WorldInstance> list = new List<ApiWorld.WorldInstance>();
			PageWorldInfo info = base.gameObject.GetComponentInParent<PageWorldInfo>();
			List<ApiWorld.WorldInstance> collection = new List<ApiWorld.WorldInstance>(this.world.worldInstances);
			list.AddRange(collection);
			list.RemoveAll((ApiWorld.WorldInstance i) => i.GetAccessType() == ApiWorld.WorldInstance.AccessType.FriendsOfGuests && i.GetInstanceCreator() != APIUser.CurrentUser.id);
			if (RoomManager.inRoom && RoomManager.currentRoom.id == this.world.id && RoomManager.currentOwnerId != APIUser.CurrentUser.id)
			{
				int count = PlayerManager.GetAllPlayers().Length;
				ApiWorld.WorldInstance worldInstance = new ApiWorld.WorldInstance(RoomManager.currentRoom.currentInstanceIdWithTags, count);
				ApiWorld.WorldInstance.AccessType accessType = worldInstance.GetAccessType();
				if (accessType == ApiWorld.WorldInstance.AccessType.InviteOnly || accessType == ApiWorld.WorldInstance.AccessType.FriendsOfGuests)
				{
					list.Add(worldInstance);
				}
			}
			num += list.Sum((ApiWorld.WorldInstance wInst) => wInst.count);
			if (info != null)
			{
				list.RemoveAll((ApiWorld.WorldInstance i) => i.idWithTags == info.worldInstance.idWithTags);
			}
			int num2 = this.world.occupants - num;
			if (num2 > 0)
			{
				list.RemoveAll((ApiWorld.WorldInstance i) => i.GetAccessType() == ApiWorld.WorldInstance.AccessType.PrivatePopCounter);
				ApiWorld.WorldInstance item = new ApiWorld.WorldInstance("0" + ApiWorld.WorldInstance.BuildAccessTags(ApiWorld.WorldInstance.AccessType.PrivatePopCounter, string.Empty), num2);
				list.Add(item);
			}
			base.RenderElements<ApiWorld.WorldInstance>(list, 0);
		}
	}

	// Token: 0x060061C9 RID: 25033 RVA: 0x00227FC8 File Offset: 0x002263C8
	protected override void SetPickerContentFromApiModel(VRCUiContentButton content, object am)
	{
		ApiWorld.WorldInstance wi = (ApiWorld.WorldInstance)am;
		string detail = (wi.count <= 0) ? string.Empty : wi.count.ToString();
		ApiWorld.WorldInstance.AccessType accessType = wi.GetAccessType();
		ApiWorld.WorldInstance.AccessDetail accessDetail = ApiWorld.WorldInstance.GetAccessDetail(accessType);
		Action action = null;
		if (accessType == ApiWorld.WorldInstance.AccessType.PrivatePopCounter)
		{
			content.EnableDetail(0, false);
			content.SetDetailShouldShowImage(1, false);
			content.EnableDetail(2, false);
			content.EnableDetail(4, true);
			content.SetDetailText(5, detail);
		}
		else
		{
			content.EnableDetail(0, true);
			content.SetDetailText(0, detail);
			content.SetDetailShouldShowImage(1, true);
			if (wi.count >= this.world.capacity)
			{
			}
			content.EnableDetail(2, true);
			content.SetDetailText(3, "#" + wi.idOnly + " " + accessDetail.shortName);
			content.EnableDetail(4, false);
			action = delegate
			{
				this.ShowWorldInstanceDetails(this.world, wi);
			};
		}
		content.Initialize(this.world.thumbnailImageUrl, this.world.name, action, this.world.id);
	}

	// Token: 0x060061CA RID: 25034 RVA: 0x00228114 File Offset: 0x00226514
	public void ShowWorldInstanceDetails(ApiWorld w, ApiWorld.WorldInstance instance)
	{
		VRCUiPage page = VRCUiManager.Instance.GetPage("UserInterface/MenuContent/Screens/WorldInfo");
		VRCUiManager.Instance.ShowScreen(page);
		PageWorldInfo component = page.GetComponent<PageWorldInfo>();
		component.SetupWorldInfo(this.world, instance, false, component.openedFromPortal);
	}

	// Token: 0x0400476B RID: 18283
	public ApiWorld world;
}
