using System;
using VRC;

namespace MoPhoGames.USpeak.Interface
{
	// Token: 0x020009DF RID: 2527
	public interface ISpeechDataHandler
	{
		// Token: 0x06004CDA RID: 19674
		void USpeakOnSerializeAudio(byte[] data);

		// Token: 0x06004CDB RID: 19675
		void USpeakInitializeSettings(int data);

		// Token: 0x06004CDC RID: 19676
		void USpeakInitializeSettingsForPlayer(int data, Player p);
	}
}
