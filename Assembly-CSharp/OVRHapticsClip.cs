using System;
using UnityEngine;

// Token: 0x0200067B RID: 1659
public class OVRHapticsClip
{
	// Token: 0x0600380C RID: 14348 RVA: 0x0011D844 File Offset: 0x0011BC44
	public OVRHapticsClip()
	{
		this.Capacity = OVRHaptics.Config.MaximumBufferSamplesCount;
		this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x0011D86E File Offset: 0x0011BC6E
	public OVRHapticsClip(int capacity)
	{
		this.Capacity = ((capacity < 0) ? 0 : capacity);
		this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
	}

	// Token: 0x0600380E RID: 14350 RVA: 0x0011D8A1 File Offset: 0x0011BCA1
	public OVRHapticsClip(byte[] samples, int samplesCount)
	{
		this.Samples = samples;
		this.Capacity = this.Samples.Length / OVRHaptics.Config.SampleSizeInBytes;
		this.Count = ((samplesCount < 0) ? 0 : samplesCount);
	}

	// Token: 0x0600380F RID: 14351 RVA: 0x0011D8D8 File Offset: 0x0011BCD8
	public OVRHapticsClip(OVRHapticsClip a, OVRHapticsClip b)
	{
		int count = a.Count;
		if (b.Count > count)
		{
			count = b.Count;
		}
		this.Capacity = count;
		this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
		int num = 0;
		while (num < a.Count || num < b.Count)
		{
			if (OVRHaptics.Config.SampleSizeInBytes == 1)
			{
				byte sample = 0;
				if (num < a.Count && num < b.Count)
				{
					sample = (byte)Mathf.Clamp((int)(a.Samples[num] + b.Samples[num]), 0, 255);
				}
				else if (num < a.Count)
				{
					sample = a.Samples[num];
				}
				else if (num < b.Count)
				{
					sample = b.Samples[num];
				}
				this.WriteSample(sample);
			}
			num++;
		}
	}

	// Token: 0x06003810 RID: 14352 RVA: 0x0011D9C4 File Offset: 0x0011BDC4
	public OVRHapticsClip(AudioClip audioClip, int channel = 0)
	{
		float[] array = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData(array, 0);
		this.InitializeFromAudioFloatTrack(array, (double)audioClip.frequency, audioClip.channels, channel);
	}

	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x06003811 RID: 14353 RVA: 0x0011DA08 File Offset: 0x0011BE08
	// (set) Token: 0x06003812 RID: 14354 RVA: 0x0011DA10 File Offset: 0x0011BE10
	public int Count { get; private set; }

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x06003813 RID: 14355 RVA: 0x0011DA19 File Offset: 0x0011BE19
	// (set) Token: 0x06003814 RID: 14356 RVA: 0x0011DA21 File Offset: 0x0011BE21
	public int Capacity { get; private set; }

	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x06003815 RID: 14357 RVA: 0x0011DA2A File Offset: 0x0011BE2A
	// (set) Token: 0x06003816 RID: 14358 RVA: 0x0011DA32 File Offset: 0x0011BE32
	public byte[] Samples { get; private set; }

	// Token: 0x06003817 RID: 14359 RVA: 0x0011DA3C File Offset: 0x0011BE3C
	public void WriteSample(byte sample)
	{
		if (this.Count >= this.Capacity)
		{
			return;
		}
		if (OVRHaptics.Config.SampleSizeInBytes == 1)
		{
			this.Samples[this.Count * OVRHaptics.Config.SampleSizeInBytes] = sample;
		}
		this.Count++;
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x0011DA88 File Offset: 0x0011BE88
	public void Reset()
	{
		this.Count = 0;
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x0011DA94 File Offset: 0x0011BE94
	private void InitializeFromAudioFloatTrack(float[] sourceData, double sourceFrequency, int sourceChannelCount, int sourceChannel)
	{
		double num = sourceFrequency / (double)OVRHaptics.Config.SampleRateHz;
		int num2 = (int)num;
		double num3 = num - (double)num2;
		double num4 = 0.0;
		int num5 = sourceData.Length;
		this.Count = 0;
		this.Capacity = num5 / sourceChannelCount / num2 + 1;
		this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
		int i = sourceChannel % sourceChannelCount;
		while (i < num5)
		{
			if (OVRHaptics.Config.SampleSizeInBytes == 1)
			{
				this.WriteSample((byte)(Mathf.Clamp01(Mathf.Abs(sourceData[i])) * 255f));
			}
			i += num2 * sourceChannelCount;
			num4 += num3;
			if ((int)num4 > 0)
			{
				i += (int)num4 * sourceChannelCount;
				num4 -= (double)((int)num4);
			}
		}
	}
}
