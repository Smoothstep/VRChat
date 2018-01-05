using System;
using UnityEngine;

// Token: 0x0200064E RID: 1614
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/UI/Orthographic Camera")]
public class UIOrthoCamera : MonoBehaviour
{
	// Token: 0x06003642 RID: 13890 RVA: 0x001143F1 File Offset: 0x001127F1
	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		this.mTrans = base.transform;
		this.mCam.orthographic = true;
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x00114418 File Offset: 0x00112818
	private void Update()
	{
		float num = this.mCam.rect.yMin * (float)Screen.height;
		float num2 = this.mCam.rect.yMax * (float)Screen.height;
		float num3 = (num2 - num) * 0.5f * this.mTrans.lossyScale.y;
		if (!Mathf.Approximately(this.mCam.orthographicSize, num3))
		{
			this.mCam.orthographicSize = num3;
		}
	}

	// Token: 0x04001F50 RID: 8016
	private Camera mCam;

	// Token: 0x04001F51 RID: 8017
	private Transform mTrans;
}
