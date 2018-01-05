using System;
using UnityEngine;

// Token: 0x0200057B RID: 1403
[AddComponentMenu("NGUI/Examples/Item Attachment Point")]
public class InvAttachmentPoint : MonoBehaviour
{
	// Token: 0x06002F99 RID: 12185 RVA: 0x000E86C4 File Offset: 0x000E6AC4
	public GameObject Attach(GameObject prefab)
	{
		if (this.mPrefab != prefab)
		{
			this.mPrefab = prefab;
			if (this.mChild != null)
			{
				UnityEngine.Object.Destroy(this.mChild);
			}
			if (this.mPrefab != null)
			{
				Transform transform = base.transform;
				this.mChild = UnityEngine.Object.Instantiate<GameObject>(this.mPrefab, transform.position, transform.rotation);
				Transform transform2 = this.mChild.transform;
				transform2.parent = transform;
				transform2.localPosition = Vector3.zero;
				transform2.localRotation = Quaternion.identity;
				transform2.localScale = Vector3.one;
			}
		}
		return this.mChild;
	}

	// Token: 0x040019E1 RID: 6625
	public InvBaseItem.Slot slot;

	// Token: 0x040019E2 RID: 6626
	private GameObject mPrefab;

	// Token: 0x040019E3 RID: 6627
	private GameObject mChild;
}
