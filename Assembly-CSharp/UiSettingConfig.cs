using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C4A RID: 3146
public class UiSettingConfig : MonoBehaviour
{
	// Token: 0x0600618C RID: 24972 RVA: 0x00226BD0 File Offset: 0x00224FD0
	private void OnEnable()
	{
		this.toggle = base.GetComponent<Toggle>();
		this.slider = base.GetComponent<Slider>();
		this._locked = false;
		this.ApplyForcedSettings();
		if (this._locked)
		{
			return;
		}
		if (this.toggle != null)
		{
			this.toggle.interactable = true;
			this.toggle.isOn = this.GetValueBool();
		}
		if (this.slider != null)
		{
			this.slider.value = this.GetValueFloat();
		}
	}

	// Token: 0x0600618D RID: 24973 RVA: 0x00226C60 File Offset: 0x00225060
	private void ApplyForcedSettings()
	{
		if (this.setting == VRCInputManager.InputSetting.ComfortTurning && VRCInputManager.ShouldForceDisableComfortTurning())
		{
			this._locked = true;
			if (this.toggle)
			{
				this.toggle.isOn = false;
				this.toggle.interactable = false;
			}
		}
		if (this.setting == VRCInputManager.InputSetting.HeadLookWalk && VRCInputManager.ShouldForceEnableHeadLookWalk())
		{
			this._locked = true;
			this.toggle.isOn = true;
			this.toggle.interactable = false;
		}
		if (this.setting == VRCInputManager.InputSetting.ToggleTalk && VRCInputManager.ShouldForceTalkToggle())
		{
			this._locked = true;
			this.toggle.isOn = true;
			this.toggle.interactable = false;
		}
	}

	// Token: 0x0600618E RID: 24974 RVA: 0x00226D1C File Offset: 0x0022511C
	private bool GetValueBool()
	{
		if (string.IsNullOrEmpty(this.PlayerPrefsString))
		{
			return VRCInputManager.GetSetting(this.setting);
		}
		return PlayerPrefs.GetInt(this.PlayerPrefsString) != 0;
	}

	// Token: 0x0600618F RID: 24975 RVA: 0x00226D4B File Offset: 0x0022514B
	private float GetValueFloat()
	{
		if (string.IsNullOrEmpty(this.PlayerPrefsString))
		{
			return 0f;
		}
		return PlayerPrefs.GetFloat(this.PlayerPrefsString, 1f);
	}

	// Token: 0x06006190 RID: 24976 RVA: 0x00226D74 File Offset: 0x00225174
	public void SetEnable(bool on)
	{
		if (this._locked)
		{
			return;
		}
		if (this.invertSetting)
		{
			on = !on;
		}
		if (string.IsNullOrEmpty(this.PlayerPrefsString))
		{
			VRCInputManager.SetSetting(this.setting, on);
		}
		else
		{
			PlayerPrefs.SetInt(this.PlayerPrefsString, (!on) ? 0 : 1);
		}
		VRCInputManager.SettingsChanged(this.setting);
	}

	// Token: 0x06006191 RID: 24977 RVA: 0x00226DE2 File Offset: 0x002251E2
	public void SetValue(float val)
	{
		if (this._locked)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.PlayerPrefsString))
		{
			PlayerPrefs.SetFloat(this.PlayerPrefsString, val);
		}
		VRCInputManager.SettingsChanged(this.setting);
	}

	// Token: 0x04004724 RID: 18212
	public string PlayerPrefsString;

	// Token: 0x04004725 RID: 18213
	public VRCInputManager.InputSetting setting;

	// Token: 0x04004726 RID: 18214
	public bool invertSetting;

	// Token: 0x04004727 RID: 18215
	private Toggle toggle;

	// Token: 0x04004728 RID: 18216
	private Slider slider;

	// Token: 0x04004729 RID: 18217
	private bool _locked;
}
