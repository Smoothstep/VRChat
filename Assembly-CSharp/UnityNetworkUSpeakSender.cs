using System;
using MoPhoGames.USpeak.Interface;
using UnityEngine;
using VRC;

// Token: 0x020009DD RID: 2525
public class UnityNetworkUSpeakSender : MonoBehaviour, ISpeechDataHandler
{
	// Token: 0x06004CCA RID: 19658 RVA: 0x0019BDB4 File Offset: 0x0019A1B4
	private void Start()
	{
		if (!base.GetComponent<NetworkView>().isMine)
		{
			USpeaker.Get(this).SpeakerMode = SpeakerMode.Remote;
		}
	}

	// Token: 0x06004CCB RID: 19659 RVA: 0x0019BDD2 File Offset: 0x0019A1D2
	private void OnDisconnectedFromServer(NetworkDisconnection cause)
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004CCC RID: 19660 RVA: 0x0019BDDF File Offset: 0x0019A1DF
	public void USpeakOnSerializeAudio(byte[] data)
	{
		base.GetComponent<NetworkView>().RPC("vc", RPCMode.All, new object[]
		{
			data
		});
	}

	// Token: 0x06004CCD RID: 19661 RVA: 0x0019BDFC File Offset: 0x0019A1FC
	public void USpeakInitializeSettings(int data)
	{
		base.GetComponent<NetworkView>().RPC("init", RPCMode.AllBuffered, new object[]
		{
			data
		});
	}

	// Token: 0x06004CCE RID: 19662 RVA: 0x0019BE1E File Offset: 0x0019A21E
	public void USpeakInitializeSettingsForPlayer(int data, Player p)
	{
		base.GetComponent<NetworkView>().RPC("init", RPCMode.All, new object[]
		{
			data
		});
	}

	// Token: 0x06004CCF RID: 19663 RVA: 0x0019BE40 File Offset: 0x0019A240
	[PunRPC]
	private void vc(byte[] data)
	{
		USpeaker.Get(this).ReceiveAudio(data);
	}

	// Token: 0x06004CD0 RID: 19664 RVA: 0x0019BE4E File Offset: 0x0019A24E
	[PunRPC]
	private void init(int data)
	{
		USpeaker.Get(this).InitializeSettings(data);
	}
}
