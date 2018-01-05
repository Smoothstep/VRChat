using System;
using UnityEngine;
using VRC.Core;

// Token: 0x02000C6F RID: 3183
public class VRCUiOpenURL : MonoBehaviour
{
	// Token: 0x060062E4 RID: 25316 RVA: 0x00233384 File Offset: 0x00231784
	public static string GetAPIURL(string url)
	{
		string api_URL = ApiModel.API_URL;
		Uri uri = new Uri(api_URL);
		string leftPart = uri.GetLeftPart(UriPartial.Authority);
		return leftPart + "/" + url.TrimStart(new char[]
		{
			'/'
		});
	}

	// Token: 0x060062E5 RID: 25317 RVA: 0x002333C4 File Offset: 0x002317C4
	public void OpenURL()
	{
		if (!string.IsNullOrEmpty(this.URL))
		{
			string url = (this.UrlType != VRCUiOpenURL.URLType.Absolute) ? VRCUiOpenURL.GetAPIURL(this.URL) : this.URL;
			Application.OpenURL(url);
		}
	}

	// Token: 0x0400486A RID: 18538
	public VRCUiOpenURL.URLType UrlType;

	// Token: 0x0400486B RID: 18539
	public string URL = string.Empty;

	// Token: 0x02000C70 RID: 3184
	public enum URLType
	{
		// Token: 0x0400486D RID: 18541
		Absolute,
		// Token: 0x0400486E RID: 18542
		Api
	}
}
