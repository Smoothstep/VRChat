using System;
using MoPhoGames.USpeak.Core.Utils;

namespace MoPhoGames.USpeak.Core
{
	// Token: 0x020009D6 RID: 2518
	public class USpeakAudioClipConverter
	{
		// Token: 0x06004CA9 RID: 19625 RVA: 0x0019B53C File Offset: 0x0019993C
		public static short[] AudioDataToShorts(float[] samples, int channels, float gain = 1f)
		{
			short[] @short = USpeakPoolUtils.GetShort(samples.Length * channels);
			for (int i = 0; i < samples.Length; i++)
			{
				float num = samples[i] * gain * 3267f;
				@short[i] = (short)((num >= -3267f) ? ((num <= 3267f) ? num : 3267f) : -3267f);
			}
			return @short;
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x0019B5A4 File Offset: 0x001999A4
		public static float[] ShortsToAudioData(short[] data, int channels, int frequency, bool threedimensional, float gain)
		{
			float[] @float = USpeakPoolUtils.GetFloat(data.Length);
			for (int i = 0; i < @float.Length; i++)
			{
				float num = (float)data[i] / 3267f * gain;
				@float[i] = ((num >= -1f) ? ((num <= 1f) ? num : 1f) : -1f);
			}
			return @float;
		}

		// Token: 0x040034CE RID: 13518
		private const float kScale = 3267f;
	}
}
