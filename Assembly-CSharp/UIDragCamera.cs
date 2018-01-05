using System;
using UnityEngine;

// Token: 0x020005A8 RID: 1448
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Drag Camera")]
public class UIDragCamera : MonoBehaviour
{
	// Token: 0x06003055 RID: 12373 RVA: 0x000ED2E9 File Offset: 0x000EB6E9
	private void Awake()
	{
		if (this.draggableCamera == null)
		{
			this.draggableCamera = NGUITools.FindInParents<UIDraggableCamera>(base.gameObject);
		}
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x000ED30D File Offset: 0x000EB70D
	private void OnPress(bool isPressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Press(isPressed);
		}
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x000ED347 File Offset: 0x000EB747
	private void OnDrag(Vector2 delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Drag(delta);
		}
	}

	// Token: 0x06003058 RID: 12376 RVA: 0x000ED381 File Offset: 0x000EB781
	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Scroll(delta);
		}
	}

	// Token: 0x04001ABC RID: 6844
	public UIDraggableCamera draggableCamera;
}
