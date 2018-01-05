using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000666 RID: 1638
public class ONSPProfiler : MonoBehaviour
{
	// Token: 0x06003784 RID: 14212 RVA: 0x0011B798 File Offset: 0x00119B98
	private void Start()
	{
		Application.runInBackground = true;
		if (this.profilerEnabled)
		{
			Debug.Log("Oculus Audio Profiler enabled, IP address = " + Network.player.ipAddress);
		}
	}

	// Token: 0x06003785 RID: 14213 RVA: 0x0011B7D4 File Offset: 0x00119BD4
	private void Update()
	{
		if (this.port < 0 || this.port > 65535)
		{
			this.port = 2121;
		}
		ONSPProfiler.ONSP_SetProfilerPort(this.port);
		ONSPProfiler.ONSP_SetProfilerEnabled(this.profilerEnabled);
	}

	// Token: 0x06003786 RID: 14214
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int ONSP_SetProfilerEnabled(bool enabled);

	// Token: 0x06003787 RID: 14215
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int ONSP_SetProfilerPort(int port);

	// Token: 0x0400202A RID: 8234
	public const string strONSPS = "AudioPluginOculusSpatializer";

	// Token: 0x0400202B RID: 8235
	public bool profilerEnabled;

	// Token: 0x0400202C RID: 8236
	private const int DEFAULT_PORT = 2121;

	// Token: 0x0400202D RID: 8237
	public int port = 2121;
}
