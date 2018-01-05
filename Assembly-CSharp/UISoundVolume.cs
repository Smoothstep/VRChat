using System;
using UnityEngine;

// Token: 0x020005D3 RID: 1491
[RequireComponent(typeof(UISlider))]
[AddComponentMenu("NGUI/Interaction/Sound Volume")]
public class UISoundVolume : MonoBehaviour
{
	// Token: 0x0600318E RID: 12686 RVA: 0x000F4DF0 File Offset: 0x000F31F0
	private void Awake()
	{
		UISlider component = base.GetComponent<UISlider>();
		component.value = NGUITools.soundVolume;
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x0600318F RID: 12687 RVA: 0x000F4E27 File Offset: 0x000F3227
	private void OnChange()
	{
		NGUITools.soundVolume = UIProgressBar.current.value;
	}
}
