using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C3D RID: 3133
public class UIDeviceSelector : MonoBehaviour
{
	// Token: 0x0600614E RID: 24910 RVA: 0x00224E4F File Offset: 0x0022324F
	private void OnEnable()
	{
		AudioSettings.OnAudioConfigurationChanged += this.OnAudioConfigurationChanged;
		USpeaker.SetInputDeviceFromPrefs();
		this.RefreshLabel();
	}

	// Token: 0x0600614F RID: 24911 RVA: 0x00224E6D File Offset: 0x0022326D
	private void OnDisable()
	{
		AudioSettings.OnAudioConfigurationChanged -= this.OnAudioConfigurationChanged;
	}

	// Token: 0x06006150 RID: 24912 RVA: 0x00224E80 File Offset: 0x00223280
	private void OnAudioConfigurationChanged(bool devicesChanged)
	{
		if (devicesChanged)
		{
			base.StartCoroutine(this.ConfigUpdated());
		}
	}

	// Token: 0x06006151 RID: 24913 RVA: 0x00224E98 File Offset: 0x00223298
	private IEnumerator ConfigUpdated()
	{
		yield return null;
		this.RefreshLabel();
		yield break;
	}

	// Token: 0x06006152 RID: 24914 RVA: 0x00224EB4 File Offset: 0x002232B4
	private void RefreshLabel()
	{
		bool flag = string.IsNullOrEmpty(VRCInputManager.micDeviceName);
		string friendlyInputDeviceName = USpeaker.GetFriendlyInputDeviceName();
		string text = string.Empty;
		if (flag || string.IsNullOrEmpty(friendlyInputDeviceName))
		{
			text = "Default - " + ((!string.IsNullOrEmpty(friendlyInputDeviceName)) ? friendlyInputDeviceName : "None");
		}
		else
		{
			text = friendlyInputDeviceName;
		}
		this.labelMic.text = text;
	}

	// Token: 0x06006153 RID: 24915 RVA: 0x00224F20 File Offset: 0x00223320
	public void SelectPreviousMic()
	{
		bool flag = string.IsNullOrEmpty(VRCInputManager.micDeviceName);
		int deviceID = (!flag) ? (USpeaker.GetInputDeviceID() - 1) : (USpeaker.GetNumDevices() - 1);
		bool flag2 = USpeaker.TrySetInputDevice(deviceID);
		if (flag2)
		{
			VRCInputManager.micDeviceName = USpeaker.GetInputDeviceName();
		}
		else
		{
			VRCInputManager.micDeviceName = string.Empty;
			USpeaker.SetInputDevice(0);
		}
		this.RefreshLabel();
	}

	// Token: 0x06006154 RID: 24916 RVA: 0x00224F84 File Offset: 0x00223384
	public void SelectNextMic()
	{
		bool flag = string.IsNullOrEmpty(VRCInputManager.micDeviceName);
		int deviceID = (!flag) ? (USpeaker.GetInputDeviceID() + 1) : 0;
		bool flag2 = USpeaker.TrySetInputDevice(deviceID);
		if (flag2)
		{
			VRCInputManager.micDeviceName = USpeaker.GetInputDeviceName();
		}
		else
		{
			VRCInputManager.micDeviceName = string.Empty;
			USpeaker.SetInputDevice(0);
		}
		this.RefreshLabel();
	}

	// Token: 0x040046F0 RID: 18160
	public Button selectPreviousMic;

	// Token: 0x040046F1 RID: 18161
	public Button selectNextMic;

	// Token: 0x040046F2 RID: 18162
	public Text labelMic;
}
