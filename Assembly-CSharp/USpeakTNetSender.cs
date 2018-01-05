using System;
using MoPhoGames.USpeak.Interface;
using Photon;
using UnityEngine;
using VRC;

// Token: 0x020009E7 RID: 2535
public class USpeakTNetSender : Photon.MonoBehaviour, ISpeechDataHandler
{
	// Token: 0x06004D1A RID: 19738 RVA: 0x0019D932 File Offset: 0x0019BD32
	private void Awake()
	{
		this.spk = USpeaker.Get(this);
		Debug.LogError("FIX PREVIOUS LINES");
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06004D1B RID: 19739 RVA: 0x0019D955 File Offset: 0x0019BD55
	private void Start()
	{
		Debug.LogError("FIX PREVIOUS LINES");
	}

	// Token: 0x06004D1C RID: 19740 RVA: 0x0019D961 File Offset: 0x0019BD61
	private void OnDestroy()
	{
		Debug.LogError("FIX PREVIOUS LINES");
	}

	// Token: 0x06004D1D RID: 19741 RVA: 0x0019D96D File Offset: 0x0019BD6D
	private void OnNetworkLeaveChannel()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004D1E RID: 19742 RVA: 0x0019D97A File Offset: 0x0019BD7A
	public void USpeakOnSerializeAudio(byte[] data)
	{
		Debug.LogError("FIX PREVIOUS LINES");
	}

	// Token: 0x06004D1F RID: 19743 RVA: 0x0019D986 File Offset: 0x0019BD86
	public void USpeakInitializeSettings(int data)
	{
		Debug.LogError("FIX PREVIOUS LINES");
	}

	// Token: 0x06004D20 RID: 19744 RVA: 0x0019D992 File Offset: 0x0019BD92
	public void USpeakInitializeSettingsForPlayer(int data, Player p)
	{
		Debug.LogError("FIX PREVIOUS LINES");
	}

	// Token: 0x06004D21 RID: 19745 RVA: 0x0019D99E File Offset: 0x0019BD9E
	public void init(int data)
	{
		this.spk.InitializeSettings(data);
		this.initialized = true;
	}

	// Token: 0x06004D22 RID: 19746 RVA: 0x0019D9B3 File Offset: 0x0019BDB3
	public void receiveVoice(byte[] data, int channel)
	{
		if (!this.initialized)
		{
			return;
		}
		this.spk.ReceiveAudio(data);
		Debug.LogError("FIX PREVIOUS LINES");
	}

	// Token: 0x0400350E RID: 13582
	private USpeaker spk;

	// Token: 0x0400350F RID: 13583
	private bool initialized;
}
