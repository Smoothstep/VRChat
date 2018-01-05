using System;
using MoPhoGames.USpeak.Core.Utils;

namespace MoPhoGames.USpeak.Codec
{
	// Token: 0x020009C5 RID: 2501
	[Serializable]
	public class MuLawCodec : ICodec
	{
		// Token: 0x06004C25 RID: 19493 RVA: 0x0019763D File Offset: 0x00195A3D
		public void Initialize(BandMode bandMode, BitRate bitrate, FrameDuration duration)
		{
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x0019763F File Offset: 0x00195A3F
		public byte[] Encode(short[] data, BandMode mode)
		{
			return MuLawCodec.MuLawEncoder.MuLawEncode(data);
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x00197647 File Offset: 0x00195A47
		public short[] Decode(byte[] data, BandMode mode)
		{
			return MuLawCodec.MuLawDecoder.MuLawDecode(data);
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x0019764F File Offset: 0x00195A4F
		public int GetSampleSize(int recordingFrequency)
		{
			return 0;
		}

		// Token: 0x020009C6 RID: 2502
		private class MuLawEncoder
		{
			// Token: 0x06004C29 RID: 19497 RVA: 0x00197654 File Offset: 0x00195A54
			static MuLawEncoder()
			{
				for (int i = -32768; i <= 32767; i++)
				{
					MuLawCodec.MuLawEncoder.pcmToMuLawMap[i & 65535] = MuLawCodec.MuLawEncoder.encode(i);
				}
			}

			// Token: 0x17000B93 RID: 2963
			// (get) Token: 0x06004C2B RID: 19499 RVA: 0x001976A5 File Offset: 0x00195AA5
			// (set) Token: 0x06004C2C RID: 19500 RVA: 0x001976B8 File Offset: 0x00195AB8
			public static bool ZeroTrap
			{
				get
				{
					return MuLawCodec.MuLawEncoder.pcmToMuLawMap[33000] != 0;
				}
				set
				{
                    byte a = 0;
                    byte b = 2;
					byte c = (!value) ? a : b;
					for (int i = 32768; i <= 33924; i++)
					{
						MuLawCodec.MuLawEncoder.pcmToMuLawMap[i] = c;
					}
				}
			}

			// Token: 0x06004C2D RID: 19501 RVA: 0x001976F6 File Offset: 0x00195AF6
			public static byte MuLawEncode(int pcm)
			{
				return MuLawCodec.MuLawEncoder.pcmToMuLawMap[pcm & 65535];
			}

			// Token: 0x06004C2E RID: 19502 RVA: 0x00197705 File Offset: 0x00195B05
			public static byte MuLawEncode(short pcm)
			{
				return MuLawCodec.MuLawEncoder.pcmToMuLawMap[(int)pcm & 65535];
			}

			// Token: 0x06004C2F RID: 19503 RVA: 0x00197714 File Offset: 0x00195B14
			public static byte[] MuLawEncode(int[] pcm)
			{
				int num = pcm.Length;
				byte[] @byte = USpeakPoolUtils.GetByte(num);
				for (int i = 0; i < num; i++)
				{
					@byte[i] = MuLawCodec.MuLawEncoder.MuLawEncode(pcm[i]);
				}
				return @byte;
			}

			// Token: 0x06004C30 RID: 19504 RVA: 0x0019774C File Offset: 0x00195B4C
			public static byte[] MuLawEncode(short[] pcm)
			{
				int num = pcm.Length;
				byte[] @byte = USpeakPoolUtils.GetByte(num);
				for (int i = 0; i < num; i++)
				{
					@byte[i] = MuLawCodec.MuLawEncoder.MuLawEncode(pcm[i]);
				}
				return @byte;
			}

			// Token: 0x06004C31 RID: 19505 RVA: 0x00197784 File Offset: 0x00195B84
			private static byte encode(int pcm)
			{
				int num = (pcm & 32768) >> 8;
				if (num != 0)
				{
					pcm = -pcm;
				}
				if (pcm > 32635)
				{
					pcm = 32635;
				}
				pcm += 132;
				int num2 = 7;
				int num3 = 16384;
				while ((pcm & num3) == 0)
				{
					num2--;
					num3 >>= 1;
				}
				int num4 = pcm >> num2 + 3 & 15;
				byte b = (byte)(num | num2 << 4 | num4);
				return (byte)~b;
			}

			// Token: 0x04003408 RID: 13320
			public const int BIAS = 132;

			// Token: 0x04003409 RID: 13321
			public const int MAX = 32635;

			// Token: 0x0400340A RID: 13322
			private static byte[] pcmToMuLawMap = new byte[65536];
		}

		// Token: 0x020009C7 RID: 2503
		private class MuLawDecoder
		{
			// Token: 0x06004C32 RID: 19506 RVA: 0x001977F8 File Offset: 0x00195BF8
			static MuLawDecoder()
			{
				for (byte b = 0; b < 255; b += 1)
				{
					MuLawCodec.MuLawDecoder.muLawToPcmMap[(int)b] = MuLawCodec.MuLawDecoder.Decode(b);
				}
			}

			// Token: 0x06004C34 RID: 19508 RVA: 0x00197840 File Offset: 0x00195C40
			public static short[] MuLawDecode(byte[] data)
			{
				int num = data.Length;
				short[] @short = USpeakPoolUtils.GetShort(num);
				for (int i = 0; i < num; i++)
				{
					@short[i] = MuLawCodec.MuLawDecoder.muLawToPcmMap[(int)data[i]];
				}
				return @short;
			}

			// Token: 0x06004C35 RID: 19509 RVA: 0x00197878 File Offset: 0x00195C78
			private static short Decode(byte mulaw)
			{
				mulaw = (byte)~mulaw;
				int num = (int)(mulaw & 128);
				int num2 = (mulaw & 112) >> 4;
				int num3 = (int)(mulaw & 15);
				num3 |= 16;
				num3 <<= 1;
				num3++;
				num3 <<= num2 + 2;
				num3 -= 132;
				return (short)((num != 0) ? (-(short)num3) : num3);
			}

			// Token: 0x0400340B RID: 13323
			private static readonly short[] muLawToPcmMap = new short[256];
		}
	}
}
