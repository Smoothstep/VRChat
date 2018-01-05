using System;
using UnityEngine;

// Token: 0x02000C94 RID: 3220
public class AddONSPAudioSourceComponent
{
	// Token: 0x060063F8 RID: 25592 RVA: 0x00237A90 File Offset: 0x00235E90
	public static void ApplyDefaultSpatializationToAudioSources()
	{
		AudioSource[] array = UnityEngine.Object.FindObjectsOfType<AudioSource>();
		foreach (AudioSource audioSrc in array)
		{
			AddONSPAudioSourceComponent.ApplyDefaultSpatializationToAudioSource(audioSrc);
		}
	}

	// Token: 0x060063F9 RID: 25593 RVA: 0x00237AC4 File Offset: 0x00235EC4
	public static bool ApplyDefaultSpatializationToAudioSource(AudioSource audioSrc)
	{
		if (audioSrc == null)
		{
			return false;
		}
		if (!Mathf.Approximately(audioSrc.spatialBlend, 1f))
		{
			return false;
		}
		ONSPAudioSource onspaudioSource = audioSrc.GetComponent<ONSPAudioSource>();
		if (onspaudioSource != null)
		{
			return false;
		}
		onspaudioSource = audioSrc.gameObject.AddComponent<ONSPAudioSource>();
		audioSrc.spatialize = true;
		onspaudioSource.EnableSpatialization = true;
		onspaudioSource.Gain = AddONSPAudioSourceComponent.SpatializeGainFactor;
		onspaudioSource.UseInvSqr = (audioSrc.rolloffMode != AudioRolloffMode.Linear);
		onspaudioSource.Near = ((audioSrc.rolloffMode == AudioRolloffMode.Custom) ? AddONSPAudioSourceComponent.CalculateCustomRolloffNearDistance(audioSrc) : audioSrc.minDistance);
		if (onspaudioSource.UseInvSqr)
		{
			onspaudioSource.Far = audioSrc.maxDistance * AddONSPAudioSourceComponent.InvSqrAttentuationMaxDistanceScale;
		}
		else
		{
			onspaudioSource.Far = audioSrc.maxDistance;
		}
		return true;
	}

	// Token: 0x060063FA RID: 25594 RVA: 0x00237B94 File Offset: 0x00235F94
	private static float CalculateCustomRolloffNearDistance(AudioSource audioSrc)
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

	// Token: 0x0400492E RID: 18734
	private static float SpatializeGainFactor = 10f;

	// Token: 0x0400492F RID: 18735
	private static float InvSqrAttentuationMaxDistanceScale = 4f;
}
