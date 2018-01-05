using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;

// Token: 0x02000C74 RID: 3188
public class VRCUiPageAvatarPicker : VRCUiPage
{
	// Token: 0x06006304 RID: 25348 RVA: 0x00233910 File Offset: 0x00231D10
	public override void OnEnable()
	{
		this.defaultRect = this.panel.transform.parent.GetComponent<RectTransform>().rect;
		this.pickers.Clear();
		ApiAvatar.FetchList(new Action<List<ApiAvatar>>(this.AvatarsAvailable), delegate(string s)
		{
		}, ApiAvatar.Owner.Public, null, 10, 0, ApiAvatar.SortHeading.None, ApiAvatar.SortOrder.Descending, true);
		ApiAvatar.FetchList(new Action<List<ApiAvatar>>(this.AvatarsAvailable), delegate(string s)
		{
		}, ApiAvatar.Owner.Mine, null, 10, 0, ApiAvatar.SortHeading.None, ApiAvatar.SortOrder.Descending, true);
	}

	// Token: 0x06006305 RID: 25349 RVA: 0x002339B4 File Offset: 0x00231DB4
	private void AvatarsAvailable(List<ApiAvatar> avatars)
	{
		foreach (ApiAvatar b in avatars)
		{
			this.CreatePickerFromBlueprint(b);
		}
		this.panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.spacing * (float)(this.pickers.Count - 1) + this.defaultRect.width);
	}

	// Token: 0x06006306 RID: 25350 RVA: 0x00233A38 File Offset: 0x00231E38
	public override void OnDisable()
	{
		foreach (VRCUiContentButton vrcuiContentButton in this.pickers)
		{
			UnityEngine.Object.Destroy(vrcuiContentButton.gameObject);
		}
	}

	// Token: 0x06006307 RID: 25351 RVA: 0x00233A98 File Offset: 0x00231E98
	private void CreatePickerFromBlueprint(ApiAvatar b)
	{
		GameObject gameObject = AssetManagement.Instantiate(this.pickerPrefab) as GameObject;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(this.panel.transform);
		component.localScale = Vector3.one;
		component.localRotation = Quaternion.identity;
		component.localPosition = new Vector3(this.defaultRect.width / 2f + (float)this.pickers.Count * this.spacing, 0f, 0f);
		VRCUiContentButton component2 = gameObject.GetComponent<VRCUiContentButton>();
		component2.list = this.list;
		component2.Initialize(b.imageUrl, b.name, delegate
		{
			User.CurrentUser.SetCurrentAvatar(b);
			VRCUiManager.Instance.HideScreen("SCREEN");
		}, b.id);
		this.pickers.Add(component2);
	}

	// Token: 0x0400488C RID: 18572
	public Scrollbar scrollbar;

	// Token: 0x0400488D RID: 18573
	public GameObject pickerPrefab;

	// Token: 0x0400488E RID: 18574
	public RectTransform panel;

	// Token: 0x0400488F RID: 18575
	public float spacing = 500f;

	// Token: 0x04004890 RID: 18576
	public UiFeatureList list;

	// Token: 0x04004891 RID: 18577
	private Rect defaultRect;

	// Token: 0x04004892 RID: 18578
	private List<VRCUiContentButton> pickers = new List<VRCUiContentButton>();

	// Token: 0x04004893 RID: 18579
	public int Selected;
}
