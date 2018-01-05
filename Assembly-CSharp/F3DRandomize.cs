using System;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class F3DRandomize : MonoBehaviour
{
	// Token: 0x060027FE RID: 10238 RVA: 0x000D0379 File Offset: 0x000CE779
	private void Awake()
	{
		this.transform = base.GetComponent<Transform>();
		this.defaultScale = this.transform.localScale;
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x000D0398 File Offset: 0x000CE798
	private void OnEnable()
	{
		if (this.RandomScale)
		{
			this.transform.localScale = this.defaultScale * UnityEngine.Random.Range(this.MinScale, this.MaxScale);
		}
		if (this.RandomRotation)
		{
			this.transform.rotation *= Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(this.MinRotation, this.MaxRotaion));
		}
	}

	// Token: 0x0400163B RID: 5691
	private new Transform transform;

	// Token: 0x0400163C RID: 5692
	private Vector3 defaultScale;

	// Token: 0x0400163D RID: 5693
	public bool RandomScale;

	// Token: 0x0400163E RID: 5694
	public bool RandomRotation;

	// Token: 0x0400163F RID: 5695
	public float MinScale;

	// Token: 0x04001640 RID: 5696
	public float MaxScale;

	// Token: 0x04001641 RID: 5697
	public float MinRotation;

	// Token: 0x04001642 RID: 5698
	public float MaxRotaion;
}
