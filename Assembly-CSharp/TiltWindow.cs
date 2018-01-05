using System;
using UnityEngine;

// Token: 0x02000495 RID: 1173
public class TiltWindow : MonoBehaviour
{
	// Token: 0x06002839 RID: 10297 RVA: 0x000D1295 File Offset: 0x000CF695
	private void Start()
	{
		this.mTrans = base.transform;
		this.mStart = this.mTrans.localRotation;
	}

	// Token: 0x0600283A RID: 10298 RVA: 0x000D12B4 File Offset: 0x000CF6B4
	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		float num = (float)Screen.width * 0.5f;
		float num2 = (float)Screen.height * 0.5f;
		float x = Mathf.Clamp((mousePosition.x - num) / num, -1f, 1f);
		float y = Mathf.Clamp((mousePosition.y - num2) / num2, -1f, 1f);
		this.mRot = Vector2.Lerp(this.mRot, new Vector2(x, y), Time.deltaTime * 5f);
		this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.range.y, this.mRot.x * this.range.x, 0f);
	}

	// Token: 0x0400167C RID: 5756
	public Vector2 range = new Vector2(5f, 3f);

	// Token: 0x0400167D RID: 5757
	private Transform mTrans;

	// Token: 0x0400167E RID: 5758
	private Quaternion mStart;

	// Token: 0x0400167F RID: 5759
	private Vector2 mRot = Vector2.zero;
}
