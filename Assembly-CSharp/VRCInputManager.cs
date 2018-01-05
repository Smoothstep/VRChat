using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000B11 RID: 2833
public class VRCInputManager : MonoBehaviour
{
	// Token: 0x17000C61 RID: 3169
	// (get) Token: 0x060055EF RID: 21999 RVA: 0x001D9DE8 File Offset: 0x001D81E8
	// (set) Token: 0x060055F0 RID: 22000 RVA: 0x001D9DEF File Offset: 0x001D81EF
	public static bool showTooltips
	{
		get
		{
			return VRCInputManager._showTooltips;
		}
		set
		{
			VRCInputManager._showTooltips = value;
			Storage.Write("VRC_INPUT_SHOW_TOOLTIPS", value);
		}
	}

	// Token: 0x17000C62 RID: 3170
	// (get) Token: 0x060055F1 RID: 22001 RVA: 0x001D9E07 File Offset: 0x001D8207
	// (set) Token: 0x060055F2 RID: 22002 RVA: 0x001D9E0E File Offset: 0x001D820E
	public static bool personalSpace
	{
		get
		{
			return VRCInputManager._personalSpace;
		}
		set
		{
			VRCInputManager._personalSpace = value;
			Storage.Write("VRC_INPUT_PERSONAL_SPACE", value);
		}
	}

	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x060055F3 RID: 22003 RVA: 0x001D9E26 File Offset: 0x001D8226
	// (set) Token: 0x060055F4 RID: 22004 RVA: 0x001D9E2D File Offset: 0x001D822D
	public static bool defaultMute
	{
		get
		{
			return VRCInputManager._defaultMute;
		}
		set
		{
			VRCInputManager._defaultMute = value;
			Storage.Write("VRC_INPUT_DEFAULT_MUTE", value);
		}
	}

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x060055F5 RID: 22005 RVA: 0x001D9E45 File Offset: 0x001D8245
	// (set) Token: 0x060055F6 RID: 22006 RVA: 0x001D9E4C File Offset: 0x001D824C
	public static bool voicePrioritization
	{
		get
		{
			return VRCInputManager._voicePrioritization;
		}
		set
		{
			VRCInputManager._voicePrioritization = value;
			Storage.Write("VRC_INPUT_VOICE_PRIORITIZATION", value);
		}
	}

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x060055F7 RID: 22007 RVA: 0x001D9E64 File Offset: 0x001D8264
	// (set) Token: 0x060055F8 RID: 22008 RVA: 0x001D9E6B File Offset: 0x001D826B
	public static bool gazeWithoutButton
	{
		get
		{
			return VRCInputManager._gazeWithoutButton;
		}
		set
		{
			VRCInputManager._gazeWithoutButton = value;
			Storage.Write("VRC_INPUT_GAZE_WITHOUT_BUTTON", value);
		}
	}

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x060055F9 RID: 22009 RVA: 0x001D9E83 File Offset: 0x001D8283
	// (set) Token: 0x060055FA RID: 22010 RVA: 0x001D9E86 File Offset: 0x001D8286
	public static bool legacyGrasp
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	// Token: 0x17000C67 RID: 3175
	// (get) Token: 0x060055FB RID: 22011 RVA: 0x001D9E88 File Offset: 0x001D8288
	// (set) Token: 0x060055FC RID: 22012 RVA: 0x001D9E8F File Offset: 0x001D828F
	public static bool thirdPersonRotation
	{
		get
		{
			return VRCInputManager._thirdPersonRotation;
		}
		set
		{
			VRCInputManager._thirdPersonRotation = value;
			Storage.Write("VRC_INPUT_THIRD_PERSON_ROTATION", value);
		}
	}

	// Token: 0x17000C68 RID: 3176
	// (get) Token: 0x060055FD RID: 22013 RVA: 0x001D9EA7 File Offset: 0x001D82A7
	// (set) Token: 0x060055FE RID: 22014 RVA: 0x001D9EBA File Offset: 0x001D82BA
	public static bool comfortTurning
	{
		get
		{
			return !VRCInputManager.ShouldForceDisableComfortTurning() && VRCInputManager._comfortTurning;
		}
		set
		{
			VRCInputManager._comfortTurning = value;
			Storage.Write("VRC_INPUT_COMFORT_TURNING", value);
		}
	}

	// Token: 0x17000C69 RID: 3177
	// (get) Token: 0x060055FF RID: 22015 RVA: 0x001D9ED2 File Offset: 0x001D82D2
	// (set) Token: 0x06005600 RID: 22016 RVA: 0x001D9EE5 File Offset: 0x001D82E5
	public static bool headLookWalk
	{
		get
		{
			return VRCInputManager.ShouldForceEnableHeadLookWalk() || VRCInputManager._headLookWalk;
		}
		set
		{
			VRCInputManager._headLookWalk = value;
			Storage.Write("VRC_INPUT_HEAD_LOOK_WALK", value);
		}
	}

	// Token: 0x17000C6A RID: 3178
	// (get) Token: 0x06005601 RID: 22017 RVA: 0x001D9EFD File Offset: 0x001D82FD
	// (set) Token: 0x06005602 RID: 22018 RVA: 0x001D9F10 File Offset: 0x001D8310
	public static bool talkToggle
	{
		get
		{
			return VRCInputManager.ShouldForceTalkToggle() || VRCInputManager._talkToggle;
		}
		set
		{
			VRCInputManager._talkToggle = value;
			Storage.Write("VRC_INPUT_TALK_TOGGLE", value);
		}
	}

	// Token: 0x17000C6B RID: 3179
	// (get) Token: 0x06005603 RID: 22019 RVA: 0x001D9F28 File Offset: 0x001D8328
	// (set) Token: 0x06005604 RID: 22020 RVA: 0x001D9F2F File Offset: 0x001D832F
	public static bool talkDefaultOn
	{
		get
		{
			return VRCInputManager._talkDefaultOn;
		}
		set
		{
			VRCInputManager._talkDefaultOn = value;
			Storage.Write("VRC_INPUT_TALK_DEFAULT_ON", value);
		}
	}

	// Token: 0x17000C6C RID: 3180
	// (get) Token: 0x06005605 RID: 22021 RVA: 0x001D9F47 File Offset: 0x001D8347
	// (set) Token: 0x06005606 RID: 22022 RVA: 0x001D9F4E File Offset: 0x001D834E
	public static string micDeviceName
	{
		get
		{
			return VRCInputManager._micDeviceName;
		}
		set
		{
			VRCInputManager._micDeviceName = value;
			Storage.Write("VRC_INPUT_MIC_DEVICE_NAME", value);
		}
	}

	// Token: 0x17000C6D RID: 3181
	// (get) Token: 0x06005607 RID: 22023 RVA: 0x001D9F61 File Offset: 0x001D8361
	// (set) Token: 0x06005608 RID: 22024 RVA: 0x001D9F68 File Offset: 0x001D8368
	public static float micLevelVr
	{
		get
		{
			return VRCInputManager._micLevelVr;
		}
		set
		{
			VRCInputManager._micLevelVr = value;
			Storage.Write("VRC_INPUT_MIC_LEVEL_VR", value);
		}
	}

	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x06005609 RID: 22025 RVA: 0x001D9F80 File Offset: 0x001D8380
	// (set) Token: 0x0600560A RID: 22026 RVA: 0x001D9F87 File Offset: 0x001D8387
	public static float micLevelDesk
	{
		get
		{
			return VRCInputManager._micLevelDesk;
		}
		set
		{
			VRCInputManager._micLevelDesk = value;
			Storage.Write("VRC_INPUT_MIC_LEVEL_DESK", value);
		}
	}

	// Token: 0x17000C6F RID: 3183
	// (get) Token: 0x0600560B RID: 22027 RVA: 0x001D9F9F File Offset: 0x001D839F
	// (set) Token: 0x0600560C RID: 22028 RVA: 0x001D9FB2 File Offset: 0x001D83B2
	public static VRCInputManager.LocomotionMethod locomotionMethod
	{
		get
		{
			if (VRCInputManager.ShouldForceGamelikeLocomotion())
			{
				return VRCInputManager.LocomotionMethod.Gamelike;
			}
			return VRCInputManager._locomotionMethod;
		}
		set
		{
			VRCInputManager._locomotionMethod = value;
			Storage.Write("VRC_INPUT_LOCOMOTION_METHOD", (int)value);
			if (VRCPlayer.Instance != null)
			{
				VRCPlayer.Instance.GetComponent<InputStateControllerManager>().Initialize();
			}
		}
	}

	// Token: 0x17000C70 RID: 3184
	// (get) Token: 0x0600560D RID: 22029 RVA: 0x001D9FE9 File Offset: 0x001D83E9
	public static VRCInputManager.InputMethod LastInputMethod
	{
		get
		{
			return VRCInputManager._lastInputMethod;
		}
	}

	// Token: 0x0600560E RID: 22030 RVA: 0x001D9FF0 File Offset: 0x001D83F0
	private void Awake()
	{
		VRCInputManager.inputProcessors.Add(VRCInputManager.InputMethod.Keyboard, base.GetComponent<VRCInputProcessorKeyboard>());
		VRCInputManager.inputProcessors.Add(VRCInputManager.InputMethod.Mouse, base.GetComponent<VRCInputProcessorMouse>());
		VRCInputManager.inputProcessors.Add(VRCInputManager.InputMethod.Controller, base.GetComponent<VRCInputProcessorController>());
		VRCInputManager.inputProcessors.Add(VRCInputManager.InputMethod.Gaze, base.GetComponent<VRCInputProcessorGaze>());
		VRCInputManager.inputProcessors.Add(VRCInputManager.InputMethod.Hydra, base.GetComponent<VRCInputProcessorHydra>());
		VRCInputManager.inputProcessors.Add(VRCInputManager.InputMethod.Vive, base.GetComponent<VRCInputProcessorVive>());
		VRCInputManager.inputProcessors.Add(VRCInputManager.InputMethod.Oculus, base.GetComponent<VRCInputProcessorTouch>());
		this.inputs[8].SetAxisButtons(this.inputs[1], this.inputs[0]);
		this.inputs[9].SetAxisButtons(this.inputs[2], this.inputs[3]);
		this.inputs[10].SetAxisButtons(this.inputs[4], this.inputs[5]);
		this.inputs[11].SetAxisButtons(this.inputs[6], this.inputs[7]);
		VRCInputManager.inputList = new Dictionary<string, VRCInput>();
		foreach (VRCInput vrcinput in this.inputs)
		{
			VRCInputManager.inputList.Add(vrcinput.name, vrcinput);
		}
		foreach (VRCInputProcessor vrcinputProcessor in VRCInputManager.inputProcessors.Values)
		{
			vrcinputProcessor.ConnectInputs(VRCInputManager.inputList);
		}
		this.ProcessInitialSettings();
		VRCInputManager._comfortTurning = (bool)Storage.Read("VRC_INPUT_COMFORT_TURNING", typeof(bool), null);
		VRCInputManager._gazeWithoutButton = (bool)Storage.Read("VRC_INPUT_GAZE_WITHOUT_BUTTON", typeof(bool), null);
		VRCInputManager._headLookWalk = (bool)Storage.Read("VRC_INPUT_HEAD_LOOK_WALK", typeof(bool), null);
		VRCInputManager._talkToggle = (bool)Storage.Read("VRC_INPUT_TALK_TOGGLE", typeof(bool), null);
		VRCInputManager._talkDefaultOn = (bool)Storage.Read("VRC_INPUT_TALK_DEFAULT_ON", typeof(bool), null);
		VRCInputManager._micDeviceName = (string)Storage.Read("VRC_INPUT_MIC_DEVICE_NAME", typeof(string), null);
		VRCInputManager._micLevelVr = (float)Storage.Read("VRC_INPUT_MIC_LEVEL_VR", typeof(float), null);
		VRCInputManager._micLevelDesk = (float)Storage.Read("VRC_INPUT_MIC_LEVEL_DESK", typeof(float), null);
		VRCInputManager._locomotionMethod = (VRCInputManager.LocomotionMethod)Storage.Read("VRC_INPUT_LOCOMOTION_METHOD", typeof(int), null);
		VRCInputManager._thirdPersonRotation = (bool)Storage.Read("VRC_INPUT_THIRD_PERSON_ROTATION", typeof(bool), null);
		VRCInputManager._showTooltips = (bool)Storage.Read("VRC_INPUT_SHOW_TOOLTIPS", typeof(bool), null);
		VRCInputManager._personalSpace = (bool)Storage.Read("VRC_INPUT_PERSONAL_SPACE", typeof(bool), null);
		VRCInputManager._defaultMute = (bool)Storage.Read("VRC_INPUT_DEFAULT_MUTE", typeof(bool), null);
		VRCInputManager._voicePrioritization = (bool)Storage.Read("VRC_INPUT_VOICE_PRIORITIZATION", typeof(bool), null);
	}

	// Token: 0x0600560F RID: 22031 RVA: 0x001DA338 File Offset: 0x001D8738
	private void ProcessInitialSettings()
	{
		if (!PlayerPrefs.HasKey("VRC_INPUT_MIC_DEVICE_NAME"))
		{
			Storage.Write("VRC_INPUT_MIC_DEVICE_NAME", string.Empty);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_MIC_LEVEL_VR"))
		{
			Storage.Write("VRC_INPUT_MIC_LEVEL_VR", 0.8f);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_MIC_LEVEL_DESK"))
		{
			Storage.Write("VRC_INPUT_MIC_LEVEL_DESK", 0.4f);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_COMFORT_TURNING"))
		{
			Storage.Write("VRC_INPUT_COMFORT_TURNING", true);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_GAZE_WITHOUT_BUTTON"))
		{
			Storage.Write("VRC_INPUT_GAZE_WITHOUT_BUTTON", false);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_HEAD_LOOK_WALK"))
		{
			Storage.Write("VRC_INPUT_HEAD_LOOK_WALK", true);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_TALK_TOGGLE"))
		{
			Storage.Write("VRC_INPUT_TALK_TOGGLE", true);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_TALK_DEFAULT_ON"))
		{
			Storage.Write("VRC_INPUT_TALK_DEFAULT_ON", true);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_LOCOMOTION_METHOD"))
		{
			Storage.Write("VRC_INPUT_LOCOMOTION_METHOD", 0);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_LEGACY_GRASP"))
		{
			Storage.Write("VRC_INPUT_LEGACY_GRASP", false);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_THIRD_PERSON_ROTATION"))
		{
			Storage.Write("VRC_INPUT_THIRD_PERSON_ROTATION", false);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_SHOW_TOOLTIPS"))
		{
			Storage.Write("VRC_INPUT_SHOW_TOOLTIPS", true);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_PERSONAL_SPACE"))
		{
			Storage.Write("VRC_INPUT_PERSONAL_SPACE", true);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_DEFAULT_MUTE"))
		{
			Storage.Write("VRC_INPUT_DEFAULT_MUTE", false);
		}
		if (!PlayerPrefs.HasKey("VRC_INPUT_VOICE_PRIORITIZATION"))
		{
			Storage.Write("VRC_INPUT_VOICE_PRIORITIZATION", true);
		}
	}

	// Token: 0x06005610 RID: 22032 RVA: 0x001DA520 File Offset: 0x001D8920
	public void OnVRSDKInitialized()
	{
		foreach (VRCInputProcessor vrcinputProcessor in VRCInputManager.inputProcessors.Values)
		{
			vrcinputProcessor.enabled = true;
		}
	}

	// Token: 0x06005611 RID: 22033 RVA: 0x001DA580 File Offset: 0x001D8980
	public static void UseKeyboardOnlyForText(bool on)
	{
		VRCInputManager._useKeyboardOnlyForText = on;
	}

	// Token: 0x06005612 RID: 22034 RVA: 0x001DA588 File Offset: 0x001D8988
	private void Update()
	{
		if (!VRCApplicationSetup.IsVRSDKInitialized)
		{
			return;
		}
		bool flag = VRCInputManager.IsUsingHandController();
		foreach (VRCInput vrcinput in this.inputs)
		{
			vrcinput.Reset();
		}
		foreach (KeyValuePair<VRCInputManager.InputMethod, VRCInputProcessor> keyValuePair in VRCInputManager.inputProcessors)
		{
			if (keyValuePair.Key != VRCInputManager.InputMethod.Keyboard || !VRCInputManager._useKeyboardOnlyForText)
			{
				if (keyValuePair.Value.enabled && keyValuePair.Value.inputEnabled)
				{
					keyValuePair.Value.Apply();
				}
			}
		}
		foreach (KeyValuePair<VRCInputManager.InputMethod, VRCInputProcessor> keyValuePair2 in VRCInputManager.inputProcessors)
		{
			if (keyValuePair2.Value.AnyKey())
			{
				VRCInputManager._lastInputMethod = keyValuePair2.Key;
			}
		}
		if (VRCInputManager.IsUsingHandController() != flag)
		{
			this.OnPlayerUsingHandControllersChanged(VRCInputManager.IsUsingHandController());
		}
	}

	// Token: 0x06005613 RID: 22035 RVA: 0x001DA6D8 File Offset: 0x001D8AD8
	private void OnPlayerUsingHandControllersChanged(bool isUsingHandControllers)
	{
		VRCInputManager.SettingsChanged(VRCInputManager.InputSetting.Unknown);
	}

	// Token: 0x06005614 RID: 22036 RVA: 0x001DA6E1 File Offset: 0x001D8AE1
	public static bool IsPresent(VRCInputManager.InputMethod input)
	{
		return VRCInputManager.inputProcessors.ContainsKey(input) && VRCInputManager.inputProcessors[input].present;
	}

	// Token: 0x06005615 RID: 22037 RVA: 0x001DA706 File Offset: 0x001D8B06
	public static bool IsSupported(VRCInputManager.InputMethod input)
	{
		return VRCInputManager.inputProcessors.ContainsKey(input) && VRCInputManager.inputProcessors[input].supported;
	}

	// Token: 0x06005616 RID: 22038 RVA: 0x001DA72B File Offset: 0x001D8B2B
	public static bool IsRequired(VRCInputManager.InputMethod input)
	{
		return VRCInputManager.inputProcessors.ContainsKey(input) && VRCInputManager.inputProcessors[input].required;
	}

	// Token: 0x06005617 RID: 22039 RVA: 0x001DA750 File Offset: 0x001D8B50
	public static bool IsEnabled(VRCInputManager.InputMethod input)
	{
		return VRCInputManager.inputProcessors.ContainsKey(input) && VRCInputManager.inputProcessors[input].inputEnabled;
	}

	// Token: 0x06005618 RID: 22040 RVA: 0x001DA775 File Offset: 0x001D8B75
	public static bool AnyKey(VRCInputManager.InputMethod input)
	{
		return VRCInputManager.inputProcessors.ContainsKey(input) && VRCInputManager.inputProcessors[input].AnyKey();
	}

	// Token: 0x06005619 RID: 22041 RVA: 0x001DA79A File Offset: 0x001D8B9A
	public static void SetEnabled(VRCInputManager.InputMethod input, bool val)
	{
		if (VRCInputManager.inputProcessors.ContainsKey(input))
		{
			VRCInputManager.inputProcessors[input].inputEnabled = val;
		}
	}

	// Token: 0x0600561A RID: 22042 RVA: 0x001DA7C0 File Offset: 0x001D8BC0
	public static void SetSetting(VRCInputManager.InputSetting setting, bool val)
	{
		switch (setting)
		{
		case VRCInputManager.InputSetting.GazeWithoutButton:
			VRCInputManager.gazeWithoutButton = val;
			break;
		case VRCInputManager.InputSetting.ComfortTurning:
			VRCInputManager.comfortTurning = val;
			break;
		case VRCInputManager.InputSetting.HeadLookWalk:
			VRCInputManager.headLookWalk = val;
			break;
		case VRCInputManager.InputSetting.ToggleTalk:
			VRCInputManager.talkToggle = val;
			break;
		case VRCInputManager.InputSetting.TalkDefaultOn:
			VRCInputManager.talkDefaultOn = val;
			break;
		case VRCInputManager.InputSetting.LegacyGrasp:
			VRCInputManager.legacyGrasp = val;
			break;
		case VRCInputManager.InputSetting.ThirdPersonRotation:
			VRCInputManager.thirdPersonRotation = val;
			break;
		case VRCInputManager.InputSetting.ShowTooltips:
			VRCInputManager.showTooltips = val;
			break;
		case VRCInputManager.InputSetting.PersonalSpace:
			VRCInputManager.personalSpace = val;
			break;
		case VRCInputManager.InputSetting.DefaultMute:
			VRCInputManager.defaultMute = val;
			break;
		case VRCInputManager.InputSetting.VoicePrioritization:
			VRCInputManager.voicePrioritization = val;
			break;
		}
	}

	// Token: 0x0600561B RID: 22043 RVA: 0x001DA880 File Offset: 0x001D8C80
	public static bool GetSetting(VRCInputManager.InputSetting setting)
	{
		switch (setting)
		{
		case VRCInputManager.InputSetting.GazeWithoutButton:
			return VRCInputManager.gazeWithoutButton;
		case VRCInputManager.InputSetting.ComfortTurning:
			return VRCInputManager.comfortTurning;
		case VRCInputManager.InputSetting.HeadLookWalk:
			return VRCInputManager.headLookWalk;
		case VRCInputManager.InputSetting.ToggleTalk:
			return VRCInputManager.talkToggle;
		case VRCInputManager.InputSetting.TalkDefaultOn:
			return VRCInputManager.talkDefaultOn;
		case VRCInputManager.InputSetting.LegacyGrasp:
			return VRCInputManager.legacyGrasp;
		case VRCInputManager.InputSetting.ThirdPersonRotation:
			return VRCInputManager.thirdPersonRotation;
		case VRCInputManager.InputSetting.ShowTooltips:
			return VRCInputManager.showTooltips;
		case VRCInputManager.InputSetting.PersonalSpace:
			return VRCInputManager.personalSpace;
		case VRCInputManager.InputSetting.DefaultMute:
			return VRCInputManager.defaultMute;
		case VRCInputManager.InputSetting.VoicePrioritization:
			return VRCInputManager.voicePrioritization;
		default:
			return false;
		}
	}

	// Token: 0x0600561C RID: 22044 RVA: 0x001DA908 File Offset: 0x001D8D08
	public static VRCInput FindInput(string name)
	{
		VRCInput result = null;
		VRCInputManager.inputList.TryGetValue(name, out result);
		return result;
	}

	// Token: 0x0600561D RID: 22045 RVA: 0x001DA926 File Offset: 0x001D8D26
	public static bool ShouldForceGamelikeLocomotion()
	{
		return !VRCApplicationSetup.IsEditor() && !HMDManager.IsHmdDetected();
	}

	// Token: 0x0600561E RID: 22046 RVA: 0x001DA93C File Offset: 0x001D8D3C
	public static bool ShouldForceDisableComfortTurning()
	{
		return !VRCApplicationSetup.IsEditor() && !HMDManager.IsHmdDetected();
	}

	// Token: 0x0600561F RID: 22047 RVA: 0x001DA952 File Offset: 0x001D8D52
	public static bool ShouldForceEnableHeadLookWalk()
	{
		return !VRCApplicationSetup.IsEditor() && (VRCInputManager.IsRequired(VRCInputManager.InputMethod.Vive) || VRCInputManager.IsRequired(VRCInputManager.InputMethod.Oculus));
	}

	// Token: 0x06005620 RID: 22048 RVA: 0x001DA974 File Offset: 0x001D8D74
	public static bool ShouldForceTalkToggle()
	{
		return !VRCApplicationSetup.IsEditor() && VRCInputManager.IsUsingHandController();
	}

	// Token: 0x06005621 RID: 22049 RVA: 0x001DA987 File Offset: 0x001D8D87
	public static void SettingsChanged(VRCInputManager.InputSetting whichSetting = VRCInputManager.InputSetting.Unknown)
	{
		VRCAudioManager.SettingsChanged();
		VRCPlayer.SettingsChanged();
		if (whichSetting == VRCInputManager.InputSetting.DefaultMute)
		{
			User.SetNetworkProperties();
			ModerationManager.Instance.Updated();
		}
	}

	// Token: 0x06005622 RID: 22050 RVA: 0x001DA9AA File Offset: 0x001D8DAA
	public static bool IsUsingHandController()
	{
		return VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Vive || VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Hydra || VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Oculus;
	}

	// Token: 0x06005623 RID: 22051 RVA: 0x001DA9CD File Offset: 0x001D8DCD
	public static VRCInputMethod GetLastUsedInputMethod()
	{
		return (VRCInputMethod)VRCInputManager.LastInputMethod;
	}

	// Token: 0x06005624 RID: 22052 RVA: 0x001DA9D4 File Offset: 0x001D8DD4
	public static bool IsUsingAutoEquipControllerType()
	{
		return VRCInputManager.LastInputMethod != VRCInputManager.InputMethod.Oculus;
	}

	// Token: 0x06005625 RID: 22053 RVA: 0x001DA9E4 File Offset: 0x001D8DE4
	public static bool GetInputSetting(VRCInputSetting setting)
	{
		if (setting == VRCInputSetting.Locomotion3P)
		{
			return VRCInputManager.locomotionMethod == VRCInputManager.LocomotionMethod.ThirdPerson;
		}
		VRCInputManager.InputSetting setting2 = VRCInputManager.InputSetting.ComfortTurning;
		bool flag = false;
		IEnumerator enumerator = Enum.GetValues(typeof(VRCInputManager.InputSetting)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				VRCInputManager.InputSetting inputSetting = (VRCInputManager.InputSetting)obj;
				if (inputSetting.ToString().CompareTo(setting.ToString()) == 0)
				{
					flag = true;
					setting2 = inputSetting;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (flag)
		{
			return VRCInputManager.GetSetting(setting2);
		}
		Debug.LogError("GetInputSetting: could not find setting: " + setting.ToString());
		return false;
	}

	// Token: 0x06005626 RID: 22054 RVA: 0x001DAAB4 File Offset: 0x001D8EB4
	public static void SetInputSetting(VRCInputSetting setting, bool enable)
	{
		if (setting == VRCInputSetting.Locomotion3P)
		{
			VRCInputManager.locomotionMethod = ((!enable) ? VRCInputManager.LocomotionMethod.Gamelike : VRCInputManager.LocomotionMethod.ThirdPerson);
			VRCInputManager.SettingsChanged(VRCInputManager.InputSetting.Unknown);
			return;
		}
		VRCInputManager.InputSetting inputSetting = VRCInputManager.InputSetting.ComfortTurning;
		bool flag = false;
		IEnumerator enumerator = Enum.GetValues(typeof(VRCInputManager.InputSetting)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				VRCInputManager.InputSetting inputSetting2 = (VRCInputManager.InputSetting)obj;
				if (inputSetting2.ToString().CompareTo(setting.ToString()) == 0)
				{
					flag = true;
					inputSetting = inputSetting2;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (flag)
		{
			VRCInputManager.SetSetting(inputSetting, enable);
			VRCInputManager.SettingsChanged(inputSetting);
		}
		else
		{
			Debug.LogError("SetInputSetting: could not find setting: " + setting.ToString());
		}
	}

	// Token: 0x06005627 RID: 22055 RVA: 0x001DABA0 File Offset: 0x001D8FA0
	public static string[] GetGamepadNames()
	{
		foreach (string str in Input.GetJoystickNames())
		{
			Debug.Log("joystick:" + str);
		}
		return Input.GetJoystickNames();
	}

	// Token: 0x04003CA1 RID: 15521
	private VRCInput[] inputs = new VRCInput[]
	{
		new VRCInput("MoveForward", 1f, 0f, 1f),
		new VRCInput("MoveBackward", -1f, 0f, 1f),
		new VRCInput("MoveLeft", -1f, 0f, 1f),
		new VRCInput("MoveRight", 1f, 0f, 1f),
		new VRCInput("LookLeft", -1f, 0f, 1f),
		new VRCInput("LookRight", 1f, 0f, 1f),
		new VRCInput("LookDown", -1f, 0f, 1f),
		new VRCInput("LookUp", 1f, 0f, 1f),
		new VRCInput("Vertical", 1f, 0f, 1f),
		new VRCInput("Horizontal", 1f, 0f, 1f),
		new VRCInput("LookHorizontal", 1f, 0f, 1f),
		new VRCInput("LookVertical", 1f, 0f, 1f),
		new VRCInput("ComfortLeft", -1f, 0f, 1f),
		new VRCInput("ComfortRight", 1f, 0f, 1f),
		new VRCInput("Voice", 1f, 0f, 1f),
		new VRCInput("Select", 1f, 0f, 1f),
		new VRCInput("Jump", 1f, 0f, 1f),
		new VRCInput("Run", 1f, 0f, 1f),
		new VRCInput("Back", 1f, 0f, 1f),
		new VRCInput("Menu", 1f, 0f, 1f),
		new VRCInput("Menu2", 1f, 0f, 1f),
		new VRCInput("Reset Orientation", 1f, 0f, 1f),
		new VRCInput("ToggleSitStand", 1f, 0f, 1f),
		new VRCInput("CapturePanorama", 1f, 0f, 1f),
		new VRCInput("DropRight", 1f, 0f, 1f),
		new VRCInput("UseRight", 1f, 0f, 1f),
		new VRCInput("GrabRight", 1f, 0f, 1f),
		new VRCInput("DropLeft", 1f, 0f, 1f),
		new VRCInput("UseLeft", 1f, 0f, 1f),
		new VRCInput("GrabLeft", 1f, 0f, 1f),
		new VRCInput("GrabToggleLeft", 1f, 0f, 1f),
		new VRCInput("GrabToggleRight", 1f, 0f, 1f),
		new VRCInput("MouseX", 1f, 0f, 1f),
		new VRCInput("MouseY", 1f, 0f, 1f),
		new VRCInput("MouseZ", 1f, 0f, 1f),
		new VRCInput("MoveHoldFB", 1f, 0f, 1f),
		new VRCInput("SpinHoldCwCcw", 1f, 0f, 1f),
		new VRCInput("SpinHoldUD", 1f, 0f, 1f),
		new VRCInput("SpinHoldLR", 1f, 0f, 1f),
		new VRCInput("TouchpadLeftClick", 1f, 0f, 1f),
		new VRCInput("TouchpadLeftX", 1f, 0f, 1f),
		new VRCInput("TouchpadLeftY", 1f, 0f, 1f),
		new VRCInput("TouchpadRightClick", 1f, 0f, 1f),
		new VRCInput("TouchpadRightX", 1f, 0f, 1f),
		new VRCInput("TouchpadRightY", 1f, 0f, 1f),
		new VRCInput("UseAxisLeft", 1f, 0f, 1f),
		new VRCInput("UseAxisRight", 1f, 0f, 1f),
		new VRCInput("GrabAxisLeft", 1f, 0f, 1f),
		new VRCInput("GrabAxisRight", 1f, 0f, 1f),
		new VRCInput("FaceTouchLeft", 1f, 0f, 1f),
		new VRCInput("FaceTouchRight", 1f, 0f, 1f),
		new VRCInput("FaceButtonTouchLeft", 1f, 0f, 1f),
		new VRCInput("FaceButtonTouchRight", 1f, 0f, 1f),
		new VRCInput("TriggerTouchLeft", 1f, 0f, 1f),
		new VRCInput("TriggerTouchRight", 1f, 0f, 1f),
		new VRCInput("ThumbRestTouchLeft", 1f, 0f, 1f),
		new VRCInput("ThumbRestTouchRight", 1f, 0f, 1f)
	};

	// Token: 0x04003CA2 RID: 15522
	private static Dictionary<string, VRCInput> inputList;

	// Token: 0x04003CA3 RID: 15523
	private static Dictionary<VRCInputManager.InputMethod, VRCInputProcessor> inputProcessors = new Dictionary<VRCInputManager.InputMethod, VRCInputProcessor>();

	// Token: 0x04003CA4 RID: 15524
	private static bool _useKeyboardOnlyForText = false;

	// Token: 0x04003CA5 RID: 15525
	public const string TagShowTooltips = "VRC_INPUT_SHOW_TOOLTIPS";

	// Token: 0x04003CA6 RID: 15526
	private static bool _showTooltips;

	// Token: 0x04003CA7 RID: 15527
	public const string TagPersonalSpace = "VRC_INPUT_PERSONAL_SPACE";

	// Token: 0x04003CA8 RID: 15528
	private static bool _personalSpace;

	// Token: 0x04003CA9 RID: 15529
	public const string TagDefaultMute = "VRC_INPUT_DEFAULT_MUTE";

	// Token: 0x04003CAA RID: 15530
	private static bool _defaultMute;

	// Token: 0x04003CAB RID: 15531
	public const string TagVoicePrioritization = "VRC_INPUT_VOICE_PRIORITIZATION";

	// Token: 0x04003CAC RID: 15532
	private static bool _voicePrioritization;

	// Token: 0x04003CAD RID: 15533
	public const string TagGazeWithoutButton = "VRC_INPUT_GAZE_WITHOUT_BUTTON";

	// Token: 0x04003CAE RID: 15534
	private static bool _gazeWithoutButton;

	// Token: 0x04003CAF RID: 15535
	public const string TagLegacyGrasp = "VRC_INPUT_LEGACY_GRASP";

	// Token: 0x04003CB0 RID: 15536
	public const string TagThirdPersonRotation = "VRC_INPUT_THIRD_PERSON_ROTATION";

	// Token: 0x04003CB1 RID: 15537
	private static bool _thirdPersonRotation;

	// Token: 0x04003CB2 RID: 15538
	public const string TagComfortTurning = "VRC_INPUT_COMFORT_TURNING";

	// Token: 0x04003CB3 RID: 15539
	private static bool _comfortTurning;

	// Token: 0x04003CB4 RID: 15540
	public const string TagHeadLookWalk = "VRC_INPUT_HEAD_LOOK_WALK";

	// Token: 0x04003CB5 RID: 15541
	private static bool _headLookWalk;

	// Token: 0x04003CB6 RID: 15542
	public const string TagTalkToggle = "VRC_INPUT_TALK_TOGGLE";

	// Token: 0x04003CB7 RID: 15543
	private static bool _talkToggle;

	// Token: 0x04003CB8 RID: 15544
	public const string TagTalkDefaultOn = "VRC_INPUT_TALK_DEFAULT_ON";

	// Token: 0x04003CB9 RID: 15545
	private static bool _talkDefaultOn;

	// Token: 0x04003CBA RID: 15546
	public const string TagMicDeviceName = "VRC_INPUT_MIC_DEVICE_NAME";

	// Token: 0x04003CBB RID: 15547
	private static string _micDeviceName;

	// Token: 0x04003CBC RID: 15548
	public const string TagMicLevelVr = "VRC_INPUT_MIC_LEVEL_VR";

	// Token: 0x04003CBD RID: 15549
	private static float _micLevelVr;

	// Token: 0x04003CBE RID: 15550
	public const string TagMicLevelDesk = "VRC_INPUT_MIC_LEVEL_DESK";

	// Token: 0x04003CBF RID: 15551
	private static float _micLevelDesk;

	// Token: 0x04003CC0 RID: 15552
	public const string TagLocomotionMethod = "VRC_INPUT_LOCOMOTION_METHOD";

	// Token: 0x04003CC1 RID: 15553
	private static VRCInputManager.LocomotionMethod _locomotionMethod;

	// Token: 0x04003CC2 RID: 15554
	private static VRCInputManager.InputMethod _lastInputMethod = VRCInputManager.InputMethod.Count;

	// Token: 0x02000B12 RID: 2834
	public enum LocomotionMethod
	{
		// Token: 0x04003CC4 RID: 15556
		Gamelike,
		// Token: 0x04003CC5 RID: 15557
		Blink,
		// Token: 0x04003CC6 RID: 15558
		ThirdPerson
	}

	// Token: 0x02000B13 RID: 2835
	public enum InputMethod
	{
		// Token: 0x04003CC8 RID: 15560
		Keyboard,
		// Token: 0x04003CC9 RID: 15561
		Mouse,
		// Token: 0x04003CCA RID: 15562
		Controller,
		// Token: 0x04003CCB RID: 15563
		Gaze,
		// Token: 0x04003CCC RID: 15564
		Hydra,
		// Token: 0x04003CCD RID: 15565
		Vive,
		// Token: 0x04003CCE RID: 15566
		Oculus,
		// Token: 0x04003CCF RID: 15567
		Count
	}

	// Token: 0x02000B14 RID: 2836
	public enum InputSetting
	{
		// Token: 0x04003CD1 RID: 15569
		GazeWithoutButton,
		// Token: 0x04003CD2 RID: 15570
		ComfortTurning,
		// Token: 0x04003CD3 RID: 15571
		HeadLookWalk,
		// Token: 0x04003CD4 RID: 15572
		ToggleTalk,
		// Token: 0x04003CD5 RID: 15573
		TalkDefaultOn,
		// Token: 0x04003CD6 RID: 15574
		LegacyGrasp,
		// Token: 0x04003CD7 RID: 15575
		ThirdPersonRotation,
		// Token: 0x04003CD8 RID: 15576
		ShowTooltips,
		// Token: 0x04003CD9 RID: 15577
		PersonalSpace,
		// Token: 0x04003CDA RID: 15578
		DefaultMute,
		// Token: 0x04003CDB RID: 15579
		VoicePrioritization,
		// Token: 0x04003CDC RID: 15580
		Unknown
	}
}
