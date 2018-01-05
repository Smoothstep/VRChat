using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C44 RID: 3140
public class UiFeaturedButton : Button, ISelectHandler, IEventSystemHandler
{
	// Token: 0x06006172 RID: 24946 RVA: 0x002264F7 File Offset: 0x002248F7
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		base.GetComponent<VRCUiContentButton>().OnSelect(eventData);
	}
}
