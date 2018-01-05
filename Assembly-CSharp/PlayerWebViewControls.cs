using System;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000AD4 RID: 2772
public class PlayerWebViewControls : MonoBehaviour
{
	// Token: 0x06005443 RID: 21571 RVA: 0x001D1B30 File Offset: 0x001CFF30
	private void InitializeWebPanels()
	{
		this.cUIViews = Resources.FindObjectsOfTypeAll<CoherentUIView>();
		VRC_WebPanel[] array = Resources.FindObjectsOfTypeAll<VRC_WebPanel>();
		if (array != null && array.Length > 0 && array[0] != null)
		{
			AudioSource componentInChildren = array[0].GetComponentInChildren<AudioSource>();
			this.localVolume = (double)((!(componentInChildren != null)) ? 0f : componentInChildren.volume);
			this.UpdateViewVolumes();
		}
	}

	// Token: 0x06005444 RID: 21572 RVA: 0x001D1BA0 File Offset: 0x001CFFA0
	private void IncreaseVolume()
	{
		this.localVolume += 0.05;
		if (this.localVolume <= 1.0)
		{
			this.UpdateViewVolumes();
		}
		else
		{
			this.localVolume = 0.0;
		}
	}

	// Token: 0x06005445 RID: 21573 RVA: 0x001D1BF4 File Offset: 0x001CFFF4
	private void DecreaseVolume()
	{
		this.localVolume -= 0.05;
		if (this.localVolume >= 0.0)
		{
			this.UpdateViewVolumes();
		}
		else
		{
			this.localVolume = 0.0;
		}
	}

	// Token: 0x06005446 RID: 21574 RVA: 0x001D1C48 File Offset: 0x001D0048
	private void UpdateViewVolumes()
	{
		foreach (CoherentUIView coherentUIView in this.cUIViews)
		{
			if (coherentUIView != null && coherentUIView.View != null)
			{
				coherentUIView.View.SetMasterVolume(this.localVolume);
			}
		}
	}

	// Token: 0x04003B72 RID: 15218
	private CoherentUIView[] cUIViews;

	// Token: 0x04003B73 RID: 15219
	private double localVolume;
}
