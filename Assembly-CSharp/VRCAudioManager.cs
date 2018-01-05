using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using VRC;

// Token: 0x02000B30 RID: 2864
public class VRCAudioManager : MonoBehaviour
{
	// Token: 0x17000CA8 RID: 3240
	// (get) Token: 0x06005755 RID: 22357 RVA: 0x001E1642 File Offset: 0x001DFA42
	public static VRCAudioManager Instance
	{
		get
		{
			return VRCAudioManager.instance;
		}
	}

	// Token: 0x06005756 RID: 22358 RVA: 0x001E1649 File Offset: 0x001DFA49
	private void Start()
	{
		if (VRCAudioManager.instance != null)
		{
			Debug.LogError("Multiple instances of VRCAudioManager = Bad");
		}
		VRCAudioManager.instance = this;
		VRCAudioManager.SettingsChanged();
	}

	// Token: 0x06005757 RID: 22359 RVA: 0x001E1670 File Offset: 0x001DFA70
	public static void SettingsChanged()
	{
		VRCAudioManager.instance.SetVolume("AUDIO_MASTER");
		VRCAudioManager.instance.SetVolume("AUDIO_UI");
		VRCAudioManager.instance.SetVolume("AUDIO_UI_AMBIENCE");
		VRCAudioManager.instance.SetVolume("AUDIO_UI_MUSIC");
		VRCAudioManager.instance.SetVolume("AUDIO_UI_SFX");
		VRCAudioManager.instance.SetVolume("AUDIO_GAME");
		VRCAudioManager.instance.SetVolume("AUDIO_GAME_SFX");
		VRCAudioManager.instance.SetVolume("AUDIO_GAME_VOICE");
	}

	// Token: 0x06005758 RID: 22360 RVA: 0x001E16F8 File Offset: 0x001DFAF8
	private void SetVolume(string name)
	{
		float num = 1f;
		if (PlayerPrefs.HasKey(name))
		{
			num = PlayerPrefs.GetFloat(name, 0.8f);
		}
		else
		{
			PlayerPrefs.SetFloat(name, num);
		}
		num = this.volumeCurve.Evaluate(num);
		this.mixer.SetFloat(name, num);
	}

	// Token: 0x06005759 RID: 22361 RVA: 0x001E1749 File Offset: 0x001DFB49
	public static AudioMixerGroup GetGameGroup()
	{
		return VRCAudioManager.instance.gameGroup;
	}

	// Token: 0x0600575A RID: 22362 RVA: 0x001E1755 File Offset: 0x001DFB55
	public static AudioMixerGroup GetUiGroup()
	{
		return VRCAudioManager.instance.uiGroup;
	}

	// Token: 0x0600575B RID: 22363 RVA: 0x001E1764 File Offset: 0x001DFB64
	public void ApplyDefaultSpatializationToAudioSources()
	{
		AudioSource[] array = Tools.FindSceneObjectsOfTypeAll<AudioSource>();
		foreach (AudioSource audioSource in array)
		{
			if (!(audioSource == null))
			{
				if (audioSource.spatialBlend.AlmostEquals(1f, 0.01f))
				{
					ONSPAudioSource onspaudioSource = audioSource.GetComponent<ONSPAudioSource>();
					if (!(onspaudioSource != null))
					{
						onspaudioSource = audioSource.gameObject.AddComponent<ONSPAudioSource>();
						audioSource.spatialize = true;
						onspaudioSource.EnableSpatialization = true;
						onspaudioSource.Gain = this.SpatializeGainFactor;
						onspaudioSource.UseInvSqr = (audioSource.rolloffMode != AudioRolloffMode.Linear);
						onspaudioSource.Near = ((audioSource.rolloffMode == AudioRolloffMode.Custom) ? this.CalculateCustomRolloffNearDistance(audioSource) : audioSource.minDistance);
						if (onspaudioSource.UseInvSqr)
						{
							onspaudioSource.Far = audioSource.maxDistance * this.InvSqrAttentuationMaxDistanceScale;
						}
						else
						{
							onspaudioSource.Far = audioSource.maxDistance;
						}
					}
				}
			}
		}
	}

	// Token: 0x0600575C RID: 22364 RVA: 0x001E1870 File Offset: 0x001DFC70
	private float CalculateCustomRolloffNearDistance(AudioSource audioSrc)
	{
		if (audioSrc.rolloffMode != AudioRolloffMode.Custom)
		{
			return audioSrc.minDistance;
		}
		AnimationCurve customCurve = audioSrc.GetCustomCurve(AudioSourceCurveType.CustomRolloff);
		if (customCurve == null)
		{
			return 1f;
		}
		Keyframe[] keys = customCurve.keys;
		if (keys.Length == 0)
		{
			return 1f;
		}
		Keyframe keyframe = keys[0];
		for (int i = 1; i < keys.Length; i++)
		{
			if (keys[i].value < keyframe.value - 0.001f)
			{
				break;
			}
			keyframe = keys[i];
		}
		return keyframe.time * audioSrc.maxDistance;
	}

	// Token: 0x0600575D RID: 22365 RVA: 0x001E1918 File Offset: 0x001DFD18
	public static void ApplyGameAudioMixerSettings(AudioSource audioSource)
	{
		if (audioSource == null)
		{
			return;
		}
		if (audioSource.transform.root.gameObject.CompareTag("VRCGlobalRoot") || audioSource.GetComponent<USpeaker>() != null)
		{
			return;
		}
		try
		{
			audioSource.outputAudioMixerGroup = VRCAudioManager.GetGameGroup();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, audioSource.gameObject);
		}
	}

	// Token: 0x0600575E RID: 22366 RVA: 0x001E1998 File Offset: 0x001DFD98
	public static void DisableAllExtraAudioListeners()
	{
		AudioListener[] source = Tools.FindSceneObjectsOfTypeAll<AudioListener>();
		foreach (AudioListener audioListener in from al in source
		where al != null && !al.transform.root.gameObject.CompareTag("VRCGlobalRoot")
		select al)
		{
			audioListener.enabled = false;
		}
	}

	// Token: 0x0600575F RID: 22367 RVA: 0x001E1A14 File Offset: 0x001DFE14
	public static void EnableAllAudio(bool enable)
	{
		AudioListener.pause = !enable;
	}

	// Token: 0x04003E48 RID: 15944
	public AudioMixer mixer;

	// Token: 0x04003E49 RID: 15945
	public AudioMixerGroup gameGroup;

	// Token: 0x04003E4A RID: 15946
	public AudioMixerGroup uiGroup;

	// Token: 0x04003E4B RID: 15947
	public AnimationCurve volumeCurve;

	// Token: 0x04003E4C RID: 15948
	public float SpatializeGainFactor = 10f;

	// Token: 0x04003E4D RID: 15949
	public float InvSqrAttentuationMaxDistanceScale = 4f;

	// Token: 0x04003E4E RID: 15950
	private static VRCAudioManager instance;
}
