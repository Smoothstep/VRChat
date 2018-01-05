using System;

// Token: 0x02000C8C RID: 3212
public class VRCUiSubPage : VRCUiPage
{
	// Token: 0x060063C1 RID: 25537 RVA: 0x00233BBC File Offset: 0x00231FBC
	public override void OnEnable()
	{
		base.OnEnable();
		if (this.screenType == "SUBSCREEN")
		{
			if (this.subPageContainer == null)
			{
				this.subPageContainer = base.GetComponentInParent<VRCUiSubPageContainer>();
			}
			if (this.shouldHaveSubPageTab && !this.subPageContainer.isAnimatingToTab && this.subPageContainer.currentTabName != this.displayName)
			{
				this.subPageContainer.ScrollToTabImmediate(this.displayName);
			}
		}
	}

	// Token: 0x04004912 RID: 18706
	public bool shouldHaveSubPageTab = true;

	// Token: 0x04004913 RID: 18707
	private VRCUiSubPageContainer subPageContainer;
}
