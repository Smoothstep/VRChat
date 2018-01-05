using System;
using UnityEngine;

// Token: 0x020006EA RID: 1770
[RequireComponent(typeof(AudioSource))]
public class OVRLipSyncTestAudio : MonoBehaviour
{
	// Token: 0x06003A51 RID: 14929 RVA: 0x00126E24 File Offset: 0x00125224
	private void Start()
	{
		if (!this.audioSource)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (!this.audioSource)
		{
			return;
		}
		string text = Application.dataPath;
		text += "/../";
		text += "TestViseme.wav";
		WWW www = new WWW("file:///" + text);
		www.threadPriority = ThreadPriority.Low;
		while (!www.isDone)
		{
			Debug.Log(www.progress);
		}
		if (www.GetAudioClip() != null)
		{
			this.audioSource.clip = www.GetAudioClip();
			this.audioSource.loop = true;
			this.audioSource.mute = false;
			this.audioSource.Play();
		}
	}

	// Token: 0x04002328 RID: 9000
	public AudioSource audioSource;
}
