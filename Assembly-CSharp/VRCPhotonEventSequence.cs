using System;

// Token: 0x02000AAC RID: 2732
public class VRCPhotonEventSequence
{
	// Token: 0x06005267 RID: 21095 RVA: 0x001C4BEC File Offset: 0x001C2FEC
	public static byte GetNextVoiceChannel()
	{
		byte result = VRCPhotonEventSequence.sNextVoiceChannel;
		VRCPhotonEventSequence.sNextVoiceChannel += 1;
		if (VRCPhotonEventSequence.sNextVoiceChannel > 50)
		{
			VRCPhotonEventSequence.sNextVoiceChannel = 1;
		}
		return result;
	}

	// Token: 0x04003A51 RID: 14929
	public const byte Reserved = 0;

	// Token: 0x04003A52 RID: 14930
	public const byte VoiceDataChannelStart = 1;

	// Token: 0x04003A53 RID: 14931
	public const byte VoiceDataChannelEnd = 50;

	// Token: 0x04003A54 RID: 14932
	public const byte ChannelCount = 51;

	// Token: 0x04003A55 RID: 14933
	private static byte sNextVoiceChannel = 1;
}
