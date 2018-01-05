using System;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public interface IPunPrefabPool
{
	// Token: 0x06003CA9 RID: 15529
	GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation);

	// Token: 0x06003CAA RID: 15530
	void Destroy(GameObject gameObject);
}
