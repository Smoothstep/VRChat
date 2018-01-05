using System;
using UnityEngine;

// Token: 0x02000790 RID: 1936
public class OnJoinedInstantiate : MonoBehaviour
{
	// Token: 0x06003ED3 RID: 16083 RVA: 0x0013CEC8 File Offset: 0x0013B2C8
	public void OnJoinedRoom()
	{
		if (this.PrefabsToInstantiate != null)
		{
			foreach (GameObject gameObject in this.PrefabsToInstantiate)
			{
				Debug.Log("Instantiating: " + gameObject.name);
				Vector3 a = Vector3.up;
				if (this.SpawnPosition != null)
				{
					a = this.SpawnPosition.position;
				}
				Vector3 a2 = UnityEngine.Random.insideUnitSphere;
				a2.y = 0f;
				a2 = a2.normalized;
				Vector3 position = a + this.PositionOffset * a2;
				PhotonNetwork.Instantiate(gameObject.name, position, Quaternion.identity, 0);
			}
		}
	}

	// Token: 0x04002772 RID: 10098
	public Transform SpawnPosition;

	// Token: 0x04002773 RID: 10099
	public float PositionOffset = 2f;

	// Token: 0x04002774 RID: 10100
	public GameObject[] PrefabsToInstantiate;
}
