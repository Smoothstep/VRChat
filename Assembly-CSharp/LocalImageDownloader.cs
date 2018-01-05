using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A8B RID: 2699
public class LocalImageDownloader : MonoBehaviour
{
	// Token: 0x06005159 RID: 20825 RVA: 0x001BE0EE File Offset: 0x001BC4EE
	public void DownloadLocalImage(string imagePath, Action<string, Texture2D> onImageDownload)
	{
		base.StartCoroutine(this.DownloadLocalImageCoroutine(imagePath, onImageDownload));
	}

	// Token: 0x0600515A RID: 20826 RVA: 0x001BE100 File Offset: 0x001BC500
	private IEnumerator DownloadLocalImageCoroutine(string imageUrl, Action<string, Texture2D> onImageDownload)
	{
		for (;;)
		{
			WWW www = new WWW(imageUrl);
			yield return www;
			if (onImageDownload != null)
			{
				onImageDownload(imageUrl, www.texture);
			}
			UnityEngine.Object.Destroy(this);
		}
		yield break;
	}

	// Token: 0x0400399F RID: 14751
	public static Dictionary<string, Texture2D> downloadedImages;
}
