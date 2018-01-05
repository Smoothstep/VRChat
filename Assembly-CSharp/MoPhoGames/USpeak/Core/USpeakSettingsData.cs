using System;

namespace MoPhoGames.USpeak.Core
{
	// Token: 0x020009DA RID: 2522
	public class USpeakSettingsData
	{
		// Token: 0x06004CBB RID: 19643 RVA: 0x0019B974 File Offset: 0x00199D74
		public USpeakSettingsData()
		{
			this.bandMode = BandMode.Opus48k;
			this.Bitrate = BitRate.BitRate_24K;
			this.Duration = FrameDuration.FrameDuration_20ms;
			this.Codec = 3;
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x0019B998 File Offset: 0x00199D98
		public USpeakSettingsData(int src)
		{
			this.bandMode = (BandMode)(src & 255);
			this.Bitrate = (BitRate)(src >> 8 & 255);
			this.Duration = (FrameDuration)(src >> 16 & 255);
			this.Codec = (src >> 24 & 255);
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x0019B9E8 File Offset: 0x00199DE8
		public int ToInt()
		{
			int num = 0;
			num |= (int)(this.bandMode & (BandMode)255);
			num |= (int)((int)(this.Bitrate & (BitRate)255) << 8);
			num |= (int)((int)(this.Duration & (FrameDuration)255) << 16);
			return num | (this.Codec & 255) << 24;
		}

		// Token: 0x040034D9 RID: 13529
		public BandMode bandMode;

		// Token: 0x040034DA RID: 13530
		public BitRate Bitrate;

		// Token: 0x040034DB RID: 13531
		public FrameDuration Duration;

		// Token: 0x040034DC RID: 13532
		public int Codec;
	}
}
