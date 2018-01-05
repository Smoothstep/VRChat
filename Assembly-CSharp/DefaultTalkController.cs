using System;
using MoPhoGames.USpeak.Interface;
using UnityEngine;

// Token: 0x020009DE RID: 2526
[AddComponentMenu("USpeak/Default Talk Controller")]
public class DefaultTalkController : MonoBehaviour, IUSpeakTalkController
{
	// Token: 0x06004CD2 RID: 19666 RVA: 0x0019BE64 File Offset: 0x0019A264
	public void OnInspectorGUI()
	{
	}

	// Token: 0x06004CD3 RID: 19667 RVA: 0x0019BE68 File Offset: 0x0019A268
	private void Start()
	{
		if (DefaultTalkController.Instance == null)
		{
			DefaultTalkController.Instance = this;
		}
		else
		{
			Debug.LogError("More than one DefaultTalkControllers exist! Destroying this one.");
			UnityEngine.Object.Destroy(this);
		}
		this.ToggleMode = ((!VRCInputManager.talkToggle) ? 0 : 1);
		this.inVoice = VRCInputManager.FindInput("Voice");
	}

	// Token: 0x06004CD4 RID: 19668 RVA: 0x0019BEC8 File Offset: 0x0019A2C8
	private void Update()
	{
		if (PlayerPrefs.GetInt("SETTING_DISABLE_MIC_BUTTON", 0) == 0)
		{
			if (this.ToggleMode == 0)
			{
				DefaultTalkController.voiceMuted = this.inVoice.button;
			}
			else if (this.inVoice.down)
			{
				Debug.Log("Toggling voice active");
				DefaultTalkController.voiceMuted = !DefaultTalkController.voiceMuted;
			}
		}
	}

	// Token: 0x06004CD5 RID: 19669 RVA: 0x0019BF2C File Offset: 0x0019A32C
	public bool ShouldSend()
	{
		return DefaultTalkController.IsLive();
	}

	// Token: 0x06004CD6 RID: 19670 RVA: 0x0019BF34 File Offset: 0x0019A334
	public static bool IsLive()
	{
		bool talkDefaultOn = VRCInputManager.talkDefaultOn;
		return (!talkDefaultOn) ? DefaultTalkController.voiceMuted : (!DefaultTalkController.voiceMuted);
	}

	// Token: 0x06004CD7 RID: 19671 RVA: 0x0019BF5F File Offset: 0x0019A35F
	public static void ToggleMute()
	{
		DefaultTalkController.voiceMuted = !DefaultTalkController.voiceMuted;
	}

	// Token: 0x06004CD8 RID: 19672 RVA: 0x0019BF6E File Offset: 0x0019A36E
	public static void Mute()
	{
		Debug.Log("Attmepting to to mute mic");
		VRCInputManager.talkToggle = false;
		VRCInputManager.talkDefaultOn = false;
		VRCInputManager.SettingsChanged(VRCInputManager.InputSetting.ToggleTalk);
		DefaultTalkController.voiceMuted = false;
	}

	// Token: 0x040034E5 RID: 13541
	[HideInInspector]
	[SerializeField]
	public int ToggleMode;

	// Token: 0x040034E6 RID: 13542
	private static bool voiceMuted;

	// Token: 0x040034E7 RID: 13543
	public static DefaultTalkController Instance;

	// Token: 0x040034E8 RID: 13544
	private VRCInput inVoice;
}
