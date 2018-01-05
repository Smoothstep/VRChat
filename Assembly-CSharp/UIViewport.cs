using System;
using UnityEngine;

// Token: 0x02000660 RID: 1632
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/UI/Viewport Camera")]
public class UIViewport : MonoBehaviour
{
	// Token: 0x06003708 RID: 14088 RVA: 0x0011A089 File Offset: 0x00118489
	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		if (this.sourceCamera == null)
		{
			this.sourceCamera = Camera.main;
		}
	}

	// Token: 0x06003709 RID: 14089 RVA: 0x0011A0B4 File Offset: 0x001184B4
	private void LateUpdate()
	{
		if (this.topLeft != null && this.bottomRight != null)
		{
			Vector3 vector = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
			Vector3 vector2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
			Rect rect = new Rect(vector.x / (float)Screen.width, vector2.y / (float)Screen.height, (vector2.x - vector.x) / (float)Screen.width, (vector.y - vector2.y) / (float)Screen.height);
			float num = this.fullSize * rect.height;
			if (rect != this.mCam.rect)
			{
				this.mCam.rect = rect;
			}
			if (this.mCam.orthographicSize != num)
			{
				this.mCam.orthographicSize = num;
			}
		}
	}

	// Token: 0x04001FED RID: 8173
	public Camera sourceCamera;

	// Token: 0x04001FEE RID: 8174
	public Transform topLeft;

	// Token: 0x04001FEF RID: 8175
	public Transform bottomRight;

	// Token: 0x04001FF0 RID: 8176
	public float fullSize = 1f;

	// Token: 0x04001FF1 RID: 8177
	private Camera mCam;
}
