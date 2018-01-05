using System;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000B48 RID: 2888
public class VRCScene : MonoBehaviour
{
	// Token: 0x0600588E RID: 22670 RVA: 0x001EAEC0 File Offset: 0x001E92C0
	private void Start()
	{
		foreach (Transform transform in this.spawnTransforms)
		{
			Spawn spawn = transform.gameObject.AddComponent<Spawn>();
			spawn.spawnId = transform.name;
			spawn.prefab = SpawnManager.Instance.playerPrefab;
			spawn.transform.position = transform.transform.position;
			spawn.transform.rotation = transform.transform.rotation;
			SpawnManager.Instance.AddSpawn(spawn);
		}
		SpawnManager.Instance.spawnOrder = this.spawnOrder;
		SpawnManager.Instance.SpawnPlayerUsingOrder();
	}

	// Token: 0x04003F70 RID: 16240
	public Transform[] spawnTransforms;

	// Token: 0x04003F71 RID: 16241
	public VRC_SceneDescriptor.SpawnOrder spawnOrder = VRC_SceneDescriptor.SpawnOrder.Random;
}
