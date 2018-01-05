using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000676 RID: 1654
public static class OVRHaptics
{
	// Token: 0x060037EC RID: 14316 RVA: 0x0011D098 File Offset: 0x0011B498
	static OVRHaptics()
	{
		OVRHaptics.Config.Load();
		OVRHaptics.m_outputs = new OVRHaptics.OVRHapticsOutput[]
		{
			new OVRHaptics.OVRHapticsOutput(1u),
			new OVRHaptics.OVRHapticsOutput(2u)
		};
		OVRHaptics.Channels = new OVRHaptics.OVRHapticsChannel[]
		{
			OVRHaptics.LeftChannel = new OVRHaptics.OVRHapticsChannel(0u),
			OVRHaptics.RightChannel = new OVRHaptics.OVRHapticsChannel(1u)
		};
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x0011D0F0 File Offset: 0x0011B4F0
	public static void Process()
	{
		OVRHaptics.Config.Load();
		for (int i = 0; i < OVRHaptics.m_outputs.Length; i++)
		{
			OVRHaptics.m_outputs[i].Process();
		}
	}

	// Token: 0x0400206C RID: 8300
	public static readonly OVRHaptics.OVRHapticsChannel[] Channels;

	// Token: 0x0400206D RID: 8301
	public static readonly OVRHaptics.OVRHapticsChannel LeftChannel;

	// Token: 0x0400206E RID: 8302
	public static readonly OVRHaptics.OVRHapticsChannel RightChannel;

	// Token: 0x0400206F RID: 8303
	private static readonly OVRHaptics.OVRHapticsOutput[] m_outputs;

	// Token: 0x02000677 RID: 1655
	public static class Config
	{
		// Token: 0x060037EE RID: 14318 RVA: 0x0011D126 File Offset: 0x0011B526
		static Config()
		{
			OVRHaptics.Config.Load();
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x060037EF RID: 14319 RVA: 0x0011D12D File Offset: 0x0011B52D
		// (set) Token: 0x060037F0 RID: 14320 RVA: 0x0011D134 File Offset: 0x0011B534
		public static int SampleRateHz { get; private set; }

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x060037F1 RID: 14321 RVA: 0x0011D13C File Offset: 0x0011B53C
		// (set) Token: 0x060037F2 RID: 14322 RVA: 0x0011D143 File Offset: 0x0011B543
		public static int SampleSizeInBytes { get; private set; }

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x060037F3 RID: 14323 RVA: 0x0011D14B File Offset: 0x0011B54B
		// (set) Token: 0x060037F4 RID: 14324 RVA: 0x0011D152 File Offset: 0x0011B552
		public static int MinimumSafeSamplesQueued { get; private set; }

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x0011D15A File Offset: 0x0011B55A
		// (set) Token: 0x060037F6 RID: 14326 RVA: 0x0011D161 File Offset: 0x0011B561
		public static int MinimumBufferSamplesCount { get; private set; }

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x060037F7 RID: 14327 RVA: 0x0011D169 File Offset: 0x0011B569
		// (set) Token: 0x060037F8 RID: 14328 RVA: 0x0011D170 File Offset: 0x0011B570
		public static int OptimalBufferSamplesCount { get; private set; }

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x060037F9 RID: 14329 RVA: 0x0011D178 File Offset: 0x0011B578
		// (set) Token: 0x060037FA RID: 14330 RVA: 0x0011D17F File Offset: 0x0011B57F
		public static int MaximumBufferSamplesCount { get; private set; }

		// Token: 0x060037FB RID: 14331 RVA: 0x0011D188 File Offset: 0x0011B588
		public static void Load()
		{
			OVRPlugin.HapticsDesc controllerHapticsDesc = OVRPlugin.GetControllerHapticsDesc(2u);
			OVRHaptics.Config.SampleRateHz = controllerHapticsDesc.SampleRateHz;
			OVRHaptics.Config.SampleSizeInBytes = controllerHapticsDesc.SampleSizeInBytes;
			OVRHaptics.Config.MinimumSafeSamplesQueued = controllerHapticsDesc.MinimumSafeSamplesQueued;
			OVRHaptics.Config.MinimumBufferSamplesCount = controllerHapticsDesc.MinimumBufferSamplesCount;
			OVRHaptics.Config.OptimalBufferSamplesCount = controllerHapticsDesc.OptimalBufferSamplesCount;
			OVRHaptics.Config.MaximumBufferSamplesCount = controllerHapticsDesc.MaximumBufferSamplesCount;
		}
	}

	// Token: 0x02000678 RID: 1656
	public class OVRHapticsChannel
	{
		// Token: 0x060037FC RID: 14332 RVA: 0x0011D1E4 File Offset: 0x0011B5E4
		public OVRHapticsChannel(uint outputIndex)
		{
			this.m_output = OVRHaptics.m_outputs[(int)((UIntPtr)outputIndex)];
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x0011D1FA File Offset: 0x0011B5FA
		public void Preempt(OVRHapticsClip clip)
		{
			this.m_output.Preempt(clip);
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x0011D208 File Offset: 0x0011B608
		public void Queue(OVRHapticsClip clip)
		{
			this.m_output.Queue(clip);
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x0011D216 File Offset: 0x0011B616
		public void Mix(OVRHapticsClip clip)
		{
			this.m_output.Mix(clip);
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x0011D224 File Offset: 0x0011B624
		public void Clear()
		{
			this.m_output.Clear();
		}

		// Token: 0x04002076 RID: 8310
		private OVRHaptics.OVRHapticsOutput m_output;
	}

	// Token: 0x02000679 RID: 1657
	private class OVRHapticsOutput
	{
		// Token: 0x06003801 RID: 14337 RVA: 0x0011D234 File Offset: 0x0011B634
		public OVRHapticsOutput(uint controller)
		{
			this.m_controller = controller;
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x0011D284 File Offset: 0x0011B684
		public void Process()
		{
			OVRPlugin.HapticsState controllerHapticsState = OVRPlugin.GetControllerHapticsState(this.m_controller);
			float num = Time.realtimeSinceStartup - this.m_prevSamplesQueuedTime;
			if (this.m_prevSamplesQueued > 0)
			{
				int num2 = this.m_prevSamplesQueued - (int)(num * (float)OVRHaptics.Config.SampleRateHz + 0.5f);
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (controllerHapticsState.SamplesQueued - num2 == 0)
				{
					this.m_numPredictionHits++;
				}
				else
				{
					this.m_numPredictionMisses++;
				}
				if (num2 > 0 && controllerHapticsState.SamplesQueued == 0)
				{
					this.m_numUnderruns++;
				}
				this.m_prevSamplesQueued = controllerHapticsState.SamplesQueued;
				this.m_prevSamplesQueuedTime = Time.realtimeSinceStartup;
			}
			int num3 = OVRHaptics.Config.OptimalBufferSamplesCount;
			if (this.m_lowLatencyMode)
			{
				float num4 = 1000f / (float)OVRHaptics.Config.SampleRateHz;
				float num5 = num * 1000f;
				int num6 = (int)Mathf.Ceil(num5 / num4);
				int num7 = OVRHaptics.Config.MinimumSafeSamplesQueued + num6;
				if (num7 < num3)
				{
					num3 = num7;
				}
			}
			if (controllerHapticsState.SamplesQueued > num3)
			{
				return;
			}
			if (num3 > OVRHaptics.Config.MaximumBufferSamplesCount)
			{
				num3 = OVRHaptics.Config.MaximumBufferSamplesCount;
			}
			if (num3 > controllerHapticsState.SamplesAvailable)
			{
				num3 = controllerHapticsState.SamplesAvailable;
			}
			int num8 = 0;
			int num9 = 0;
			while (num8 < num3 && num9 < this.m_pendingClips.Count)
			{
				int num10 = num3 - num8;
				int num11 = this.m_pendingClips[num9].Clip.Count - this.m_pendingClips[num9].ReadCount;
				if (num10 > num11)
				{
					num10 = num11;
				}
				if (num10 > 0)
				{
					int length = num10 * OVRHaptics.Config.SampleSizeInBytes;
					int byteOffset = num8 * OVRHaptics.Config.SampleSizeInBytes;
					int startIndex = this.m_pendingClips[num9].ReadCount * OVRHaptics.Config.SampleSizeInBytes;
					Marshal.Copy(this.m_pendingClips[num9].Clip.Samples, startIndex, this.m_nativeBuffer.GetPointer(byteOffset), length);
					this.m_pendingClips[num9].ReadCount += num10;
					num8 += num10;
				}
				num9++;
			}
			int num12 = this.m_pendingClips.Count - 1;
			while (num12 >= 0 && this.m_pendingClips.Count > 0)
			{
				if (this.m_pendingClips[num12].ReadCount >= this.m_pendingClips[num12].Clip.Count)
				{
					this.m_pendingClips.RemoveAt(num12);
				}
				num12--;
			}
			int num13 = num3 - (controllerHapticsState.SamplesQueued + num8);
			if (num13 < OVRHaptics.Config.MinimumBufferSamplesCount - num8)
			{
				num13 = OVRHaptics.Config.MinimumBufferSamplesCount - num8;
			}
			if (num13 > controllerHapticsState.SamplesAvailable)
			{
				num13 = controllerHapticsState.SamplesAvailable;
			}
			if (num13 > 0)
			{
				int length2 = num13 * OVRHaptics.Config.SampleSizeInBytes;
				int byteOffset2 = num8 * OVRHaptics.Config.SampleSizeInBytes;
				int startIndex2 = 0;
				Marshal.Copy(this.m_paddingClip.Samples, startIndex2, this.m_nativeBuffer.GetPointer(byteOffset2), length2);
				num8 += num13;
			}
			if (num8 > 0)
			{
				OVRPlugin.HapticsBuffer hapticsBuffer;
				hapticsBuffer.Samples = this.m_nativeBuffer.GetPointer(0);
				hapticsBuffer.SamplesCount = num8;
				OVRPlugin.SetControllerHaptics(this.m_controller, hapticsBuffer);
				this.m_prevSamplesQueued = OVRPlugin.GetControllerHapticsState(this.m_controller).SamplesQueued;
				this.m_prevSamplesQueuedTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x0011D5F4 File Offset: 0x0011B9F4
		public void Preempt(OVRHapticsClip clip)
		{
			this.m_pendingClips.Clear();
			this.m_pendingClips.Add(new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip));
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x0011D612 File Offset: 0x0011BA12
		public void Queue(OVRHapticsClip clip)
		{
			this.m_pendingClips.Add(new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip));
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x0011D628 File Offset: 0x0011BA28
		public void Mix(OVRHapticsClip clip)
		{
			int num = 0;
			int num2 = 0;
			int num3 = clip.Count;
			while (num3 > 0 && num < this.m_pendingClips.Count)
			{
				int num4 = this.m_pendingClips[num].Clip.Count - this.m_pendingClips[num].ReadCount;
				num3 -= num4;
				num2 += num4;
				num++;
			}
			if (num3 > 0)
			{
				num2 += num3;
			}
			if (num > 0)
			{
				OVRHapticsClip ovrhapticsClip = new OVRHapticsClip(num2);
				int i = 0;
				for (int j = 0; j < num; j++)
				{
					OVRHapticsClip clip2 = this.m_pendingClips[j].Clip;
					for (int k = this.m_pendingClips[j].ReadCount; k < clip2.Count; k++)
					{
						if (OVRHaptics.Config.SampleSizeInBytes == 1)
						{
							byte sample = 0;
							if (i < clip.Count && k < clip2.Count)
							{
								sample = (byte)Mathf.Clamp((int)(clip.Samples[i] + clip2.Samples[k]), 0, 255);
								i++;
							}
							else if (k < clip2.Count)
							{
								sample = clip2.Samples[k];
							}
							ovrhapticsClip.WriteSample(sample);
						}
					}
				}
				while (i < clip.Count)
				{
					if (OVRHaptics.Config.SampleSizeInBytes == 1)
					{
						ovrhapticsClip.WriteSample(clip.Samples[i]);
					}
					i++;
				}
				this.m_pendingClips[0] = new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(ovrhapticsClip);
				for (int l = 1; l < num; l++)
				{
					this.m_pendingClips.RemoveAt(1);
				}
			}
			else
			{
				this.m_pendingClips.Add(new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip));
			}
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x0011D806 File Offset: 0x0011BC06
		public void Clear()
		{
			this.m_pendingClips.Clear();
		}

		// Token: 0x04002077 RID: 8311
		private bool m_lowLatencyMode = true;

		// Token: 0x04002078 RID: 8312
		private int m_prevSamplesQueued;

		// Token: 0x04002079 RID: 8313
		private float m_prevSamplesQueuedTime;

		// Token: 0x0400207A RID: 8314
		private int m_numPredictionHits;

		// Token: 0x0400207B RID: 8315
		private int m_numPredictionMisses;

		// Token: 0x0400207C RID: 8316
		private int m_numUnderruns;

		// Token: 0x0400207D RID: 8317
		private List<OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker> m_pendingClips = new List<OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker>();

		// Token: 0x0400207E RID: 8318
		private uint m_controller;

		// Token: 0x0400207F RID: 8319
		private OVRNativeBuffer m_nativeBuffer = new OVRNativeBuffer(OVRHaptics.Config.MaximumBufferSamplesCount * OVRHaptics.Config.SampleSizeInBytes);

		// Token: 0x04002080 RID: 8320
		private OVRHapticsClip m_paddingClip = new OVRHapticsClip();

		// Token: 0x0200067A RID: 1658
		private class ClipPlaybackTracker
		{
			// Token: 0x06003807 RID: 14343 RVA: 0x0011D813 File Offset: 0x0011BC13
			public ClipPlaybackTracker(OVRHapticsClip clip)
			{
				this.Clip = clip;
			}

			// Token: 0x170008D4 RID: 2260
			// (get) Token: 0x06003808 RID: 14344 RVA: 0x0011D822 File Offset: 0x0011BC22
			// (set) Token: 0x06003809 RID: 14345 RVA: 0x0011D82A File Offset: 0x0011BC2A
			public int ReadCount { get; set; }

			// Token: 0x170008D5 RID: 2261
			// (get) Token: 0x0600380A RID: 14346 RVA: 0x0011D833 File Offset: 0x0011BC33
			// (set) Token: 0x0600380B RID: 14347 RVA: 0x0011D83B File Offset: 0x0011BC3B
			public OVRHapticsClip Clip { get; set; }
		}
	}
}
