using System;
using MoPhoGames.USpeak.Core.Utils;

namespace MoPhoGames.USpeak.Codec
{
	// Token: 0x020009C3 RID: 2499
	[Serializable]
	internal class ADPCMCodec : ICodec
	{
		// Token: 0x06004C18 RID: 19480 RVA: 0x00197279 File Offset: 0x00195679
		private void Init()
		{
			this.predictedSample = 0;
			this.stepsize = 7;
			this.index = 0;
			this.newSample = 0;
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x00197298 File Offset: 0x00195698
		private short ADPCM_Decode(byte originalSample)
		{
			int num = this.stepsize * (int)originalSample / 4 + this.stepsize / 8;
			if ((originalSample & 4) == 4)
			{
				num += this.stepsize;
			}
			if ((originalSample & 2) == 2)
			{
				num += this.stepsize >> 1;
			}
			if ((originalSample & 1) == 1)
			{
				num += this.stepsize >> 2;
			}
			num += this.stepsize >> 3;
			if ((originalSample & 8) == 8)
			{
				num = -num;
			}
			this.newSample = num;
			if (this.newSample > 32767)
			{
				this.newSample = 32767;
			}
			else if (this.newSample < -32768)
			{
				this.newSample = -32768;
			}
			this.index += ADPCMCodec.indexTable[(int)originalSample];
			if (this.index < 0)
			{
				this.index = 0;
			}
			if (this.index > 88)
			{
				this.index = 88;
			}
			this.stepsize = ADPCMCodec.stepsizeTable[this.index];
			return (short)this.newSample;
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x001973A4 File Offset: 0x001957A4
		private byte ADPCM_Encode(short originalSample)
		{
			int num = (int)originalSample - this.predictedSample;
			if (num >= 0)
			{
				this.newSample = 0;
			}
			else
			{
				this.newSample = 8;
				num = -num;
			}
			byte b = 4;
			int num2 = this.stepsize;
			for (int i = 0; i < 3; i++)
			{
				if (num >= num2)
				{
					this.newSample |= (int)b;
					num -= num2;
				}
				num2 >>= 1;
				b = (byte)(b >> 1);
			}
			num = this.stepsize >> 3;
			if ((this.newSample & 4) != 0)
			{
				num += this.stepsize;
			}
			if ((this.newSample & 2) != 0)
			{
				num += this.stepsize >> 1;
			}
			if ((this.newSample & 1) != 0)
			{
				num += this.stepsize >> 2;
			}
			if ((this.newSample & 8) != 0)
			{
				num = -num;
			}
			this.predictedSample += num;
			if (this.predictedSample > 32767)
			{
				this.predictedSample = 32767;
			}
			if (this.predictedSample < -32768)
			{
				this.predictedSample = -32768;
			}
			this.index += ADPCMCodec.indexTable[this.newSample];
			if (this.index < 0)
			{
				this.index = 0;
			}
			else if (this.index > 88)
			{
				this.index = 88;
			}
			this.stepsize = ADPCMCodec.stepsizeTable[this.index];
			return (byte)this.newSample;
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x00197516 File Offset: 0x00195916
		public void Initialize(BandMode bandMode, BitRate bitrate, FrameDuration duration)
		{
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x00197518 File Offset: 0x00195918
		public byte[] Encode(short[] data, BandMode mode)
		{
			this.Init();
			int num = data.Length / 2;
			if (num % 2 != 0)
			{
				num++;
			}
			byte[] @byte = USpeakPoolUtils.GetByte(num);
			for (int i = 0; i < @byte.Length; i++)
			{
				if (i * 2 >= data.Length)
				{
					break;
				}
				byte b = this.ADPCM_Encode(data[i * 2]);
				byte b2 = 0;
				if (i * 2 + 1 < data.Length)
				{
					b2 = this.ADPCM_Encode(data[i * 2 + 1]);
				}
				byte b3 = (byte)((int)b2 << 4 | (int)b);
				@byte[i] = b3;
			}
			return @byte;
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x001975A4 File Offset: 0x001959A4
		public short[] Decode(byte[] data, BandMode mode)
		{
			this.Init();
			short[] @short = USpeakPoolUtils.GetShort(data.Length * 2);
			for (int i = 0; i < data.Length; i++)
			{
				byte b = data[i];
                byte m = 15;
				byte originalSample = (byte)(b & m);
				byte originalSample2 = (byte)(b >> 4);
				@short[i * 2] = this.ADPCM_Decode(originalSample);
				@short[i * 2 + 1] = this.ADPCM_Decode(originalSample2);
			}
			return @short;
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x00197602 File Offset: 0x00195A02
		public int GetSampleSize(int recordingFrequency)
		{
			return 0;
		}

		// Token: 0x04003402 RID: 13314
		private static int[] indexTable = new int[]
		{
			-1,
			-1,
			-1,
			-1,
			2,
			4,
			6,
			8,
			-1,
			-1,
			-1,
			-1,
			2,
			4,
			6,
			8
		};

		// Token: 0x04003403 RID: 13315
		private static int[] stepsizeTable = new int[]
		{
			7,
			8,
			9,
			10,
			11,
			12,
			14,
			16,
			17,
			19,
			21,
			23,
			25,
			28,
			31,
			34,
			37,
			41,
			45,
			50,
			55,
			60,
			66,
			73,
			80,
			88,
			97,
			107,
			118,
			130,
			143,
			157,
			173,
			190,
			209,
			230,
			253,
			279,
			307,
			337,
			371,
			408,
			449,
			494,
			544,
			598,
			658,
			724,
			796,
			876,
			963,
			1060,
			1166,
			1282,
			1411,
			1522,
			1707,
			1876,
			2066,
			2272,
			2499,
			2749,
			3024,
			3327,
			3660,
			4026,
			4428,
			4871,
			5358,
			5894,
			6484,
			7132,
			7845,
			8630,
			9493,
			10442,
			11487,
			12635,
			13899,
			15289,
			16818,
			18500,
			203500,
			22385,
			24623,
			27086,
			29794,
			32767
		};

		// Token: 0x04003404 RID: 13316
		private int predictedSample;

		// Token: 0x04003405 RID: 13317
		private int stepsize = 7;

		// Token: 0x04003406 RID: 13318
		private int index;

		// Token: 0x04003407 RID: 13319
		private int newSample;
	}
}
