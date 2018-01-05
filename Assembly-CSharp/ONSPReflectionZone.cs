using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000668 RID: 1640
public class ONSPReflectionZone : MonoBehaviour
{
	// Token: 0x06003789 RID: 14217 RVA: 0x0011B828 File Offset: 0x00119C28
	private void Start()
	{
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x0011B82A File Offset: 0x00119C2A
	private void Update()
	{
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x0011B82C File Offset: 0x00119C2C
	private void OnTriggerEnter(Collider other)
	{
		if (this.CheckForAudioListener(other.gameObject))
		{
			this.PushCurrentMixerShapshot();
		}
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x0011B845 File Offset: 0x00119C45
	private void OnTriggerExit(Collider other)
	{
		if (this.CheckForAudioListener(other.gameObject))
		{
			this.PopCurrentMixerSnapshot();
		}
	}

	// Token: 0x0600378D RID: 14221 RVA: 0x0011B860 File Offset: 0x00119C60
	private bool CheckForAudioListener(GameObject gameObject)
	{
		AudioListener componentInChildren = gameObject.GetComponentInChildren<AudioListener>();
		return componentInChildren != null;
	}

	// Token: 0x0600378E RID: 14222 RVA: 0x0011B884 File Offset: 0x00119C84
	private void PushCurrentMixerShapshot()
	{
		ReflectionSnapshot t = ONSPReflectionZone.currentSnapshot;
		ONSPReflectionZone.snapshotList.Push(t);
		this.SetReflectionValues();
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x0011B8A8 File Offset: 0x00119CA8
	private void PopCurrentMixerSnapshot()
	{
		ReflectionSnapshot reflectionSnapshot = ONSPReflectionZone.snapshotList.Pop();
		this.SetReflectionValues(ref reflectionSnapshot);
	}

	// Token: 0x06003790 RID: 14224 RVA: 0x0011B8C8 File Offset: 0x00119CC8
	private void SetReflectionValues()
	{
		if (this.mixerSnapshot != null)
		{
			Debug.Log("Setting off snapshot " + this.mixerSnapshot.name);
			this.mixerSnapshot.TransitionTo(this.fadeTime);
			ONSPReflectionZone.currentSnapshot.mixerSnapshot = this.mixerSnapshot;
			ONSPReflectionZone.currentSnapshot.fadeTime = this.fadeTime;
		}
		else
		{
			Debug.Log("Mixer snapshot not set - Please ensure play area has at least one encompassing snapshot.");
		}
	}

	// Token: 0x06003791 RID: 14225 RVA: 0x0011B940 File Offset: 0x00119D40
	private void SetReflectionValues(ref ReflectionSnapshot mss)
	{
		if (mss.mixerSnapshot != null)
		{
			Debug.Log("Setting off snapshot " + mss.mixerSnapshot.name);
			mss.mixerSnapshot.TransitionTo(mss.fadeTime);
			ONSPReflectionZone.currentSnapshot.mixerSnapshot = mss.mixerSnapshot;
			ONSPReflectionZone.currentSnapshot.fadeTime = mss.fadeTime;
		}
		else
		{
			Debug.Log("Mixer snapshot not set - Please ensure play area has at least one encompassing snapshot.");
		}
	}

	// Token: 0x04002030 RID: 8240
	public AudioMixerSnapshot mixerSnapshot;

	// Token: 0x04002031 RID: 8241
	public float fadeTime;

	// Token: 0x04002032 RID: 8242
	private static Stack<ReflectionSnapshot> snapshotList = new Stack<ReflectionSnapshot>();

	// Token: 0x04002033 RID: 8243
	private static ReflectionSnapshot currentSnapshot = default(ReflectionSnapshot);
}
