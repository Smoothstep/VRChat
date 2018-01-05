using System;
using UnityEngine;

// Token: 0x0200058F RID: 1423
[AddComponentMenu("NGUI/Examples/Pan With Mouse")]
public class PanWithMouse : MonoBehaviour
{
	// Token: 0x06002FDA RID: 12250 RVA: 0x000EA170 File Offset: 0x000E8570
	private void Start()
	{
		this.mTrans = base.transform;
		this.mStart = this.mTrans.localRotation;
	}

	// Token: 0x06002FDB RID: 12251 RVA: 0x000EA190 File Offset: 0x000E8590
	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		Vector3 mousePosition = Input.mousePosition;
		float num = (float)Screen.width * 0.5f;
		float num2 = (float)Screen.height * 0.5f;
		if (this.range < 0.1f)
		{
			this.range = 0.1f;
		}
		float x = Mathf.Clamp((mousePosition.x - num) / num / this.range, -1f, 1f);
		float y = Mathf.Clamp((mousePosition.y - num2) / num2 / this.range, -1f, 1f);
		this.mRot = Vector2.Lerp(this.mRot, new Vector2(x, y), deltaTime * 5f);
		this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.degrees.y, this.mRot.x * this.degrees.x, 0f);
	}

	// Token: 0x04001A41 RID: 6721
	public Vector2 degrees = new Vector2(5f, 3f);

	// Token: 0x04001A42 RID: 6722
	public float range = 1f;

	// Token: 0x04001A43 RID: 6723
	private Transform mTrans;

	// Token: 0x04001A44 RID: 6724
	private Quaternion mStart;

	// Token: 0x04001A45 RID: 6725
	private Vector2 mRot = Vector2.zero;
}
