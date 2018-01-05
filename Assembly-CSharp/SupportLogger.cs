using System;
using UnityEngine;

// Token: 0x020007A4 RID: 1956
public class SupportLogger : MonoBehaviour
{
	// Token: 0x06003F37 RID: 16183 RVA: 0x0013E728 File Offset: 0x0013CB28
	public void Start()
	{
		GameObject gameObject = GameObject.Find("PunSupportLogger");
		if (gameObject == null)
		{
			gameObject = new GameObject("PunSupportLogger");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			SupportLogging supportLogging = gameObject.AddComponent<SupportLogging>();
			supportLogging.LogTrafficStats = this.LogTrafficStats;
		}
	}

	// Token: 0x0400279E RID: 10142
	public bool LogTrafficStats = true;
}
