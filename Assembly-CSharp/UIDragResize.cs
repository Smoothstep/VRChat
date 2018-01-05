using System;
using UnityEngine;

// Token: 0x020005AF RID: 1455
[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour
{
	// Token: 0x0600307B RID: 12411 RVA: 0x000EDCC4 File Offset: 0x000EC0C4
	private void OnDragStart()
	{
		if (this.target != null)
		{
			Vector3[] worldCorners = this.target.worldCorners;
			this.mPlane = new Plane(worldCorners[0], worldCorners[1], worldCorners[3]);
			Ray currentRay = UICamera.currentRay;
			float distance;
			if (this.mPlane.Raycast(currentRay, out distance))
			{
				this.mRayPos = currentRay.GetPoint(distance);
				this.mLocalPos = this.target.cachedTransform.localPosition;
				this.mWidth = this.target.width;
				this.mHeight = this.target.height;
				this.mDragging = true;
			}
		}
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x000EDD84 File Offset: 0x000EC184
	private void OnDrag(Vector2 delta)
	{
		if (this.mDragging && this.target != null)
		{
			Ray currentRay = UICamera.currentRay;
			float distance;
			if (this.mPlane.Raycast(currentRay, out distance))
			{
				Transform cachedTransform = this.target.cachedTransform;
				cachedTransform.localPosition = this.mLocalPos;
				this.target.width = this.mWidth;
				this.target.height = this.mHeight;
				Vector3 b = currentRay.GetPoint(distance) - this.mRayPos;
				cachedTransform.position += b;
				Vector3 vector = Quaternion.Inverse(cachedTransform.localRotation) * (cachedTransform.localPosition - this.mLocalPos);
				cachedTransform.localPosition = this.mLocalPos;
				NGUIMath.ResizeWidget(this.target, this.pivot, vector.x, vector.y, this.minWidth, this.minHeight, this.maxWidth, this.maxHeight);
			}
		}
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x000EDE8C File Offset: 0x000EC28C
	private void OnDragEnd()
	{
		this.mDragging = false;
	}

	// Token: 0x04001AEA RID: 6890
	public UIWidget target;

	// Token: 0x04001AEB RID: 6891
	public UIWidget.Pivot pivot = UIWidget.Pivot.BottomRight;

	// Token: 0x04001AEC RID: 6892
	public int minWidth = 100;

	// Token: 0x04001AED RID: 6893
	public int minHeight = 100;

	// Token: 0x04001AEE RID: 6894
	public int maxWidth = 100000;

	// Token: 0x04001AEF RID: 6895
	public int maxHeight = 100000;

	// Token: 0x04001AF0 RID: 6896
	private Plane mPlane;

	// Token: 0x04001AF1 RID: 6897
	private Vector3 mRayPos;

	// Token: 0x04001AF2 RID: 6898
	private Vector3 mLocalPos;

	// Token: 0x04001AF3 RID: 6899
	private int mWidth;

	// Token: 0x04001AF4 RID: 6900
	private int mHeight;

	// Token: 0x04001AF5 RID: 6901
	private bool mDragging;
}
