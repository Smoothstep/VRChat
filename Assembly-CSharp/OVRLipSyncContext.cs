using System;
using UnityEngine;
using VRC;

// Token: 0x020006FA RID: 1786
[RequireComponent(typeof(AudioSource))]
public class OVRLipSyncContext : MonoBehaviour
{
	// Token: 0x06003A95 RID: 14997 RVA: 0x00127710 File Offset: 0x00125B10
	public void Initialize()
	{
		if (!this.audioSource)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (!this.audioSource)
		{
			return;
		}
		lock (this)
		{
			if (this.context == 0u && OVRLipSync.CreateContext(ref this.context, this.provider) != 0)
			{
				Debug.Log("OVRPhonemeContext.Start ERROR: Could not create Phoneme context.");
			}
		}
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x001277A0 File Offset: 0x00125BA0
	private void Start()
	{
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x001277A2 File Offset: 0x00125BA2
	private void Update()
	{
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x001277A4 File Offset: 0x00125BA4
	private void OnDestroy()
	{
		lock (this)
		{
			if (this.context != 0u && OVRLipSync.DestroyContext(this.context) != 0)
			{
				Debug.Log("OVRPhonemeContext.OnDestroy ERROR: Could not delete Phoneme context.");
			}
		}
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x001277FC File Offset: 0x00125BFC
	private void OnAudioFilterRead(float[] data, int channels)
	{
		if (OVRLipSync.IsInitialized() != 0 || this.audioSource == null)
		{
			return;
		}
		float[] array = this.floatArrayPool.Get(data.Length);
		data.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] *= this.gain;
		}
		lock (this)
		{
			if (this.context != 0u)
			{
				OVRLipSync.ovrLipSyncFlag ovrLipSyncFlag = OVRLipSync.ovrLipSyncFlag.None;
				if (this.delayCompensate)
				{
					ovrLipSyncFlag |= OVRLipSync.ovrLipSyncFlag.DelayCompensateAudio;
				}
				OVRLipSync.ProcessFrameInterleaved(this.context, array, ovrLipSyncFlag, ref this.frame);
			}
		}
		this.floatArrayPool.Return(array);
		if (this.audioMute)
		{
			for (int j = 0; j < data.Length; j++)
			{
				data[j] *= 0f;
			}
		}
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x001278F0 File Offset: 0x00125CF0
	public int GetCurrentPhonemeFrame(ref OVRLipSync.ovrLipSyncFrame inFrame)
	{
		if (OVRLipSync.IsInitialized() != 0)
		{
			return -2200;
		}
		lock (this)
		{
			inFrame.frameNumber = this.frame.frameNumber;
			inFrame.frameDelay = this.frame.frameDelay;
			for (int i = 0; i < inFrame.Visemes.Length; i++)
			{
				inFrame.Visemes[i] = this.frame.Visemes[i];
			}
		}
		return 0;
	}

	// Token: 0x06003A9B RID: 15003 RVA: 0x00127984 File Offset: 0x00125D84
	public int ResetContext()
	{
		if (OVRLipSync.IsInitialized() != 0)
		{
			return -2200;
		}
		return OVRLipSync.ResetContext(this.context);
	}

	// Token: 0x06003A9C RID: 15004 RVA: 0x001279A1 File Offset: 0x00125DA1
	public int SendSignal(OVRLipSync.ovrLipSyncSignals signal, int arg1, int arg2)
	{
		if (OVRLipSync.IsInitialized() != 0)
		{
			return -2200;
		}
		return OVRLipSync.SendSignal(this.context, signal, arg1, arg2);
	}

	// Token: 0x04002358 RID: 9048
	public AudioSource audioSource;

	// Token: 0x04002359 RID: 9049
	public float gain = 1f;

	// Token: 0x0400235A RID: 9050
	public bool audioMute;

	// Token: 0x0400235B RID: 9051
	public OVRLipSync.ovrLipSyncContextProvider provider;

	// Token: 0x0400235C RID: 9052
	public bool delayCompensate;

	// Token: 0x0400235D RID: 9053
	private OVRLipSync.ovrLipSyncFrame frame = new OVRLipSync.ovrLipSyncFrame(0);

	// Token: 0x0400235E RID: 9054
	private uint context;

	// Token: 0x0400235F RID: 9055
	private PoolContainer<float> floatArrayPool = new PoolContainer<float>();
}
