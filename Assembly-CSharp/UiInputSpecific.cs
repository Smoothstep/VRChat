using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C48 RID: 3144
public class UiInputSpecific : MonoBehaviour
{
	// Token: 0x06006183 RID: 24963 RVA: 0x002269F0 File Offset: 0x00224DF0
	private void OnEnable()
	{
		if (this.toggle)
		{
			if (VRCInputManager.IsRequired(this.method))
			{
				this.toggle.interactable = false;
				this.toggle.isOn = true;
			}
			else if (!VRCInputManager.IsSupported(this.method))
			{
				this.toggle.interactable = false;
				this.toggle.isOn = false;
			}
			else
			{
				this.toggle.interactable = true;
				this.toggle.isOn = VRCInputManager.IsEnabled(this.method);
			}
		}
		if (this.disable)
		{
			bool active = VRCInputManager.IsEnabled(this.method) && VRCInputManager.IsSupported(this.method);
			foreach (VRCInputManager.InputMethod input in this.disableExclusiveOf)
			{
				if (VRCInputManager.IsSupported(input) && VRCInputManager.IsEnabled(input))
				{
					active = false;
				}
			}
			this.disable.SetActive(active);
		}
	}

	// Token: 0x06006184 RID: 24964 RVA: 0x00226AFB File Offset: 0x00224EFB
	public void SetEnable(bool on)
	{
		VRCInputManager.SetEnabled(this.method, on);
	}

	// Token: 0x0400471C RID: 18204
	public VRCInputManager.InputMethod method;

	// Token: 0x0400471D RID: 18205
	public VRCInputManager.InputMethod[] disableExclusiveOf;

	// Token: 0x0400471E RID: 18206
	public Toggle toggle;

	// Token: 0x0400471F RID: 18207
	public GameObject disable;
}
