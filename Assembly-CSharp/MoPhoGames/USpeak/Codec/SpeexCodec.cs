using System;
using MoPhoGames.USpeak.Core.Utils;
using NSpeex;

namespace MoPhoGames.USpeak.Codec
{
	// Token: 0x020009CB RID: 2507
	public class SpeexCodec : ICodec
	{
		// Token: 0x06004C3E RID: 19518 RVA: 0x00197CC8 File Offset: 0x001960C8
		public SpeexCodec()
		{
			this.m_wide_enc.Quality = 5;
			this.m_narrow_enc.Quality = 5;
			this.m_ultrawide_enc.Quality = 5;
		}

		// Token: 0x06004C3F RID: 19519 RVA: 0x00197D4C File Offset: 0x0019614C
		private byte[] SpeexEncode(short[] input, global::BandMode mode)
		{
			SpeexEncoder speexEncoder = null;
			int num = 320;
			if (mode != global::BandMode.Narrow)
			{
				if (mode != global::BandMode.Wide)
				{
					if (mode == global::BandMode.UltraWide)
					{
						speexEncoder = this.m_ultrawide_enc;
						num = 1280;
					}
				}
				else
				{
					speexEncoder = this.m_wide_enc;
					num = 640;
				}
			}
			else
			{
				speexEncoder = this.m_narrow_enc;
				num = 320;
			}
			byte[] @byte = USpeakPoolUtils.GetByte(num + 4);
			int value = speexEncoder.Encode(input, 0, input.Length, @byte, 4, @byte.Length);
			byte[] bytes = BitConverter.GetBytes(value);
			Array.Copy(bytes, @byte, 4);
			return @byte;
		}

		// Token: 0x06004C40 RID: 19520 RVA: 0x00197DDC File Offset: 0x001961DC
		private short[] SpeexDecode(byte[] input, global::BandMode mode)
		{
			SpeexDecoder speexDecoder = null;
			int length = 320;
			if (mode != global::BandMode.Narrow)
			{
				if (mode != global::BandMode.Wide)
				{
					if (mode == global::BandMode.UltraWide)
					{
						speexDecoder = this.m_ultrawide_dec;
						length = 1280;
					}
				}
				else
				{
					speexDecoder = this.m_wide_dec;
					length = 640;
				}
			}
			else
			{
				speexDecoder = this.m_narrow_dec;
				length = 320;
			}
			byte[] @byte = USpeakPoolUtils.GetByte(4);
			Array.Copy(input, @byte, 4);
			int inCount = BitConverter.ToInt32(@byte, 0);
			USpeakPoolUtils.Return(@byte);
			byte[] byte2 = USpeakPoolUtils.GetByte(input.Length - 4);
			Buffer.BlockCopy(input, 4, byte2, 0, input.Length - 4);
			short[] @short = USpeakPoolUtils.GetShort(length);
			speexDecoder.Decode(byte2, 0, inCount, @short, 0, false);
			USpeakPoolUtils.Return(byte2);
			return @short;
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x00197E98 File Offset: 0x00196298
		public void Initialize(global::BandMode bandMode, BitRate bitrate, FrameDuration duration)
		{
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x00197E9A File Offset: 0x0019629A
		public byte[] Encode(short[] data, global::BandMode mode)
		{
			return this.SpeexEncode(data, mode);
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x00197EA4 File Offset: 0x001962A4
		public short[] Decode(byte[] data, global::BandMode mode)
		{
			return this.SpeexDecode(data, mode);
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x00197EAE File Offset: 0x001962AE
		public int GetSampleSize(int recordingFrequency)
		{
			if (recordingFrequency == 8000)
			{
				return 320;
			}
			if (recordingFrequency == 16000)
			{
				return 640;
			}
			if (recordingFrequency != 32000)
			{
				return 320;
			}
			return 1280;
		}

		// Token: 0x04003429 RID: 13353
		private SpeexDecoder m_ultrawide_dec = new SpeexDecoder(NSpeex.BandMode.UltraWide, true);

		// Token: 0x0400342A RID: 13354
		private SpeexEncoder m_ultrawide_enc = new SpeexEncoder(NSpeex.BandMode.UltraWide);

		// Token: 0x0400342B RID: 13355
		private SpeexDecoder m_wide_dec = new SpeexDecoder(NSpeex.BandMode.Wide, true);

		// Token: 0x0400342C RID: 13356
		private SpeexEncoder m_wide_enc = new SpeexEncoder(NSpeex.BandMode.Wide);

		// Token: 0x0400342D RID: 13357
		private SpeexDecoder m_narrow_dec = new SpeexDecoder(NSpeex.BandMode.Narrow, true);

		// Token: 0x0400342E RID: 13358
		private SpeexEncoder m_narrow_enc = new SpeexEncoder(NSpeex.BandMode.Narrow);
	}
}
