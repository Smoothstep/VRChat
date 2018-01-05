using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B3E RID: 2878
public class VRCWeb : MonoBehaviour
{
	// Token: 0x17000CCA RID: 3274
	// (get) Token: 0x06005853 RID: 22611 RVA: 0x001E9903 File Offset: 0x001E7D03
	public static VRCWeb Instance
	{
		get
		{
			if (VRCWeb._instance == null)
			{
				VRCWeb._instance = UnityEngine.Object.FindObjectOfType<VRCWeb>();
			}
			return VRCWeb._instance;
		}
	}

	// Token: 0x06005854 RID: 22612 RVA: 0x001E9924 File Offset: 0x001E7D24
	private IEnumerator MOTDCoroutine(VRCWeb.WebTextDelegate callback)
	{
		WWW www = new WWW(this.motdUrl);
		www.threadPriority = ThreadPriority.Low;
		yield return www;
		callback(www.text);
		yield break;
	}

	// Token: 0x06005855 RID: 22613 RVA: 0x001E9948 File Offset: 0x001E7D48
	private IEnumerator PatchMOTDCoroutine(VRCWeb.WebTextDelegate callback)
	{
		WWW www = new WWW(this.patchMOTDUrl);
		www.threadPriority = ThreadPriority.Low;
		yield return www;
		callback(www.text);
		yield break;
	}

	// Token: 0x06005856 RID: 22614 RVA: 0x001E996C File Offset: 0x001E7D6C
	private IEnumerator VersionCoroutine(VRCWeb.WebTextDelegate callback)
	{
		WWW www = new WWW(this.versionUrl);
		www.threadPriority = ThreadPriority.Low;
		yield return www;
		callback(www.text);
		yield break;
	}

	// Token: 0x06005857 RID: 22615 RVA: 0x001E998E File Offset: 0x001E7D8E
	public void RequestMOTD(VRCWeb.WebTextDelegate callback)
	{
		base.StartCoroutine(this.MOTDCoroutine(callback));
	}

	// Token: 0x06005858 RID: 22616 RVA: 0x001E999E File Offset: 0x001E7D9E
	public void RequestPatchMOTD(VRCWeb.WebTextDelegate callback)
	{
		base.StartCoroutine(this.PatchMOTDCoroutine(callback));
	}

	// Token: 0x06005859 RID: 22617 RVA: 0x001E99AE File Offset: 0x001E7DAE
	public void RequestVersion(VRCWeb.WebTextDelegate callback)
	{
		base.StartCoroutine(this.VersionCoroutine(callback));
	}

	// Token: 0x04003F52 RID: 16210
	private static VRCWeb _instance;

	// Token: 0x04003F53 RID: 16211
	public string motdUrl = "http://vrchat.net/motd.txt";

	// Token: 0x04003F54 RID: 16212
	public string patchMOTDUrl = "http://vrchat.net/patch_motd.txt";

	// Token: 0x04003F55 RID: 16213
	public string versionUrl = "http://vrchat.net/version.txt";

	// Token: 0x02000B3F RID: 2879
	// (Invoke) Token: 0x0600585B RID: 22619
	public delegate void WebTextDelegate(string text);
}
