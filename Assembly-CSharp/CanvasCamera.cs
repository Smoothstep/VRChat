using System;
using UnityEngine;

// Token: 0x02000C1B RID: 3099
public class CanvasCamera : MonoBehaviour
{
	// Token: 0x06005FF3 RID: 24563 RVA: 0x0021BD52 File Offset: 0x0021A152
	private void Start()
	{
		base.GetComponent<Canvas>().worldCamera = VRCVrCamera.GetInstance().screenCamera;
	}
}
