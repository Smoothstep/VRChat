using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000586 RID: 1414
[RequireComponent(typeof(UITexture))]
public class DownloadTexture : MonoBehaviour
{
	// Token: 0x06002FBF RID: 12223 RVA: 0x000E9394 File Offset: 0x000E7794
	private IEnumerator Start()
	{
		WWW www = new WWW(this.url);
		yield return www;
		this.mTex = www.texture;
		if (this.mTex != null)
		{
			UITexture component = base.GetComponent<UITexture>();
			component.mainTexture = this.mTex;
			if (this.pixelPerfect)
			{
				component.MakePixelPerfect();
			}
		}
		www.Dispose();
		yield break;
	}

	// Token: 0x06002FC0 RID: 12224 RVA: 0x000E93AF File Offset: 0x000E77AF
	private void OnDestroy()
	{
		if (this.mTex != null)
		{
			UnityEngine.Object.Destroy(this.mTex);
		}
	}

	// Token: 0x04001A27 RID: 6695
	public string url = "http://www.yourwebsite.com/logo.png";

	// Token: 0x04001A28 RID: 6696
	public bool pixelPerfect = true;

	// Token: 0x04001A29 RID: 6697
	private Texture2D mTex;
}
