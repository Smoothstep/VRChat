using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C47 RID: 3143
public class UiInputLocomotion : MonoBehaviour
{
	// Token: 0x06006180 RID: 24960 RVA: 0x00226944 File Offset: 0x00224D44
	private void OnEnable()
	{
		if (!this.toggle)
		{
			return;
		}
		this._locked = VRCInputManager.ShouldForceGamelikeLocomotion();
		if (this._locked)
		{
			this.toggle.isOn = (this.method == VRCInputManager.LocomotionMethod.Gamelike);
			this.toggle.interactable = false;
			return;
		}
		bool locked = this._locked;
		this._locked = true;
		this.toggle.isOn = (VRCInputManager.locomotionMethod == this.method);
		this._locked = locked;
	}

	// Token: 0x06006181 RID: 24961 RVA: 0x002269C6 File Offset: 0x00224DC6
	public void SetEnable(bool on)
	{
		if (this._locked)
		{
			return;
		}
		if (on)
		{
			VRCInputManager.locomotionMethod = this.method;
		}
	}

	// Token: 0x04004718 RID: 18200
	public VRCInputManager.LocomotionMethod method;

	// Token: 0x04004719 RID: 18201
	public Toggle toggle;

	// Token: 0x0400471A RID: 18202
	public GameObject disable;

	// Token: 0x0400471B RID: 18203
	private bool _locked;
}
