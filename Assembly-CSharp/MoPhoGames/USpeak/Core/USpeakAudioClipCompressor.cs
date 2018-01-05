using System;
using MoPhoGames.USpeak.Codec;
using MoPhoGames.USpeak.Core.Utils;
using UnityEngine;

namespace MoPhoGames.USpeak.Core
{
	// Token: 0x020009D5 RID: 2517
	public class USpeakAudioClipCompressor : MonoBehaviour
	{
		// Token: 0x06004CA6 RID: 19622 RVA: 0x0019B47C File Offset: 0x0019987C
		public static byte[] CompressAudioData(float[] samples, int channels, BandMode mode, ICodec Codec, float gain = 1f)
		{
			short[] array = USpeakAudioClipConverter.AudioDataToShorts(samples, channels, gain);
			byte[] array2 = Codec.Encode(array, mode);
			USpeakPoolUtils.Return(array);
			byte[] array3 = new byte[array2.Length];
			Array.Copy(array2, array3, array2.Length);
			USpeakPoolUtils.Return(array2);
			return array3;
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x0019B4BC File Offset: 0x001998BC
		public static float[] DecompressAudio(byte[] data, int samples, int channels, bool threeD, BandMode mode, ICodec Codec, float gain)
		{
			int frequency = 4000;
			if (mode == BandMode.Narrow)
			{
				frequency = 8000;
			}
			else if (mode == BandMode.Wide)
			{
				frequency = 16000;
			}
			else if (mode == BandMode.UltraWide)
			{
				frequency = 32000;
			}
			else if (mode == BandMode.Opus48k)
			{
				frequency = 48000;
			}
			short[] array = Codec.Decode(data, mode);
			float[] result = USpeakAudioClipConverter.ShortsToAudioData(array, channels, frequency, threeD, gain);
			USpeakPoolUtils.Return(array);
			return result;
		}
	}
}
