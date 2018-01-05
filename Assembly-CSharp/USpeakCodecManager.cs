using System;
using MoPhoGames.USpeak.Codec;
using UnityEngine;

// Token: 0x020009D7 RID: 2519
public class USpeakCodecManager : ScriptableObject
{
	// Token: 0x06004CAC RID: 19628 RVA: 0x0019B62C File Offset: 0x00199A2C
	public static USpeakCodecManager CreateNewInstance()
	{
		if (USpeakCodecManager.sDefaultInstance == null)
		{
			USpeakCodecManager.sDefaultInstance = (USpeakCodecManager)Resources.Load("CodecManager");
		}
		USpeakCodecManager uspeakCodecManager = UnityEngine.Object.Instantiate<USpeakCodecManager>(USpeakCodecManager.sDefaultInstance);
		if (uspeakCodecManager == null)
		{
			Debug.LogError("USpeakCodecManager: Could not find resource called 'CodecManager' which is required to load codecs!");
		}
		if (Application.isPlaying)
		{
			uspeakCodecManager.Codecs = new ICodec[uspeakCodecManager.CodecNames.Length];
			for (int i = 0; i < uspeakCodecManager.Codecs.Length; i++)
			{
				uspeakCodecManager.Codecs[i] = (ICodec)Activator.CreateInstance(Type.GetType(uspeakCodecManager.CodecNames[i]));
			}
		}
		return uspeakCodecManager;
	}

	// Token: 0x040034CF RID: 13519
	public static USpeakCodecManager sDefaultInstance;

	// Token: 0x040034D0 RID: 13520
	public ICodec[] Codecs;

	// Token: 0x040034D1 RID: 13521
	public string[] CodecNames = new string[0];

	// Token: 0x040034D2 RID: 13522
	public string[] FriendlyNames = new string[0];
}
