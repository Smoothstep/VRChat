using System;
using UnityEngine;

// Token: 0x0200058E RID: 1422
public class OpenURLOnClick : MonoBehaviour
{
	// Token: 0x06002FD8 RID: 12248 RVA: 0x000EA100 File Offset: 0x000E8500
	private void OnClick()
	{
		UILabel component = base.GetComponent<UILabel>();
		if (component != null)
		{
			string urlAtPosition = component.GetUrlAtPosition(UICamera.lastWorldPosition);
			if (!string.IsNullOrEmpty(urlAtPosition))
			{
				Application.OpenURL(urlAtPosition);
			}
		}
	}
}
