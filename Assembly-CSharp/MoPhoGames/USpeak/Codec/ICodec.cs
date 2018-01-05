using System;

namespace MoPhoGames.USpeak.Codec
{
	// Token: 0x020009C4 RID: 2500
	public interface ICodec
	{
		// Token: 0x06004C20 RID: 19488
		void Initialize(BandMode bandMode, BitRate bitrate, FrameDuration duration);

		// Token: 0x06004C21 RID: 19489
		byte[] Encode(short[] data, BandMode bandMode);

		// Token: 0x06004C22 RID: 19490
		short[] Decode(byte[] data, BandMode bandMode);

		// Token: 0x06004C23 RID: 19491
		int GetSampleSize(int recordingFrequency);
	}
}
