using System;
using System.Collections;
using MoPhoGames.USpeak.Interface;
using UnityEngine;
using VRC;

// Token: 0x020009DB RID: 2523
public class LocalUSpeakSender : MonoBehaviour, ISpeechDataHandler, IUSpeakTalkController
{
	// Token: 0x06004CBF RID: 19647 RVA: 0x0019BA4B File Offset: 0x00199E4B
	public void OnInspectorGUI()
	{
	}

	// Token: 0x06004CC0 RID: 19648 RVA: 0x0019BA4D File Offset: 0x00199E4D
	public bool ShouldSend()
	{
		return this.recording;
	}

	// Token: 0x06004CC1 RID: 19649 RVA: 0x0019BA55 File Offset: 0x00199E55
	public void USpeakOnSerializeAudio(byte[] data)
	{
		base.StartCoroutine(this.fakeSendPacket(data));
	}

	// Token: 0x06004CC2 RID: 19650 RVA: 0x0019BA65 File Offset: 0x00199E65
	public void USpeakInitializeSettings(int data)
	{
		USpeaker.Get(this).InitializeSettings(data);
	}

	// Token: 0x06004CC3 RID: 19651 RVA: 0x0019BA73 File Offset: 0x00199E73
	public void USpeakInitializeSettingsForPlayer(int data, Player p)
	{
		USpeaker.Get(this).InitializeSettings(data);
	}

	// Token: 0x06004CC4 RID: 19652 RVA: 0x0019BA84 File Offset: 0x00199E84
	private IEnumerator fakeSendPacket(byte[] data)
	{
		float rand = UnityEngine.Random.Range(0f, 100f);
		if (rand < this.packetLoss)
		{
			yield break;
		}
		float delay = UnityEngine.Random.Range(this.ping, this.ping + this.jitter) / 1000f;
		yield return new WaitForSeconds(delay);
		USpeaker.Get(this).ReceiveAudio(data);
		yield break;
	}

	// Token: 0x040034DD RID: 13533
	public bool recording = true;

	// Token: 0x040034DE RID: 13534
	public float jitter;

	// Token: 0x040034DF RID: 13535
	public float ping;

	// Token: 0x040034E0 RID: 13536
	public float packetLoss;
}
