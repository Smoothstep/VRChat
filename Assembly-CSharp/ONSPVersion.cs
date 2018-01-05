using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000C92 RID: 3218
public class ONSPVersion : MonoBehaviour
{
	// Token: 0x060063EB RID: 25579
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern void ONSP_GetVersion(ref int Major, ref int Minor, ref int Patch);

	// Token: 0x060063EC RID: 25580 RVA: 0x00237714 File Offset: 0x00235B14
	private void Awake()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		ONSPVersion.ONSP_GetVersion(ref num, ref num2, ref num3);
		string message = string.Format("ONSP Version: {0:F0}.{1:F0}.{2:F0}", num, num2, num3);
		Debug.Log(message);
	}

	// Token: 0x060063ED RID: 25581 RVA: 0x00237755 File Offset: 0x00235B55
	private void Start()
	{
	}

	// Token: 0x060063EE RID: 25582 RVA: 0x00237757 File Offset: 0x00235B57
	private void Update()
	{
	}

	// Token: 0x04004927 RID: 18727
	public const string strONSPS = "AudioPluginOculusSpatializer";
}
