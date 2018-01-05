using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.Core;

// Token: 0x02000AF3 RID: 2803
public static class SimplePool
{
	// Token: 0x060054D5 RID: 21717 RVA: 0x001D4220 File Offset: 0x001D2620
	private static void Init(GameObject prefab = null, int qty = 3)
	{
		if (SimplePool.pools == null)
		{
			SimplePool.pools = new Dictionary<GameObject, SimplePool.Pool>();
		}
		if (prefab != null && !SimplePool.pools.ContainsKey(prefab))
		{
			SimplePool.pools[prefab] = new SimplePool.Pool(prefab, qty);
		}
	}

	// Token: 0x060054D6 RID: 21718 RVA: 0x001D4270 File Offset: 0x001D2670
	public static void Preload(GameObject prefab, int qty = 1)
	{
		SimplePool.Init(prefab, qty);
		GameObject[] array = new GameObject[qty];
		for (int i = 0; i < qty; i++)
		{
			array[i] = SimplePool.Spawn(prefab, Vector3.zero, Quaternion.identity);
		}
		for (int j = 0; j < qty; j++)
		{
			SimplePool.Despawn(array[j]);
		}
	}

	// Token: 0x060054D7 RID: 21719 RVA: 0x001D42CA File Offset: 0x001D26CA
	public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
	{
		SimplePool.Init(prefab, 3);
		return SimplePool.pools[prefab].Spawn(pos, rot);
	}

	// Token: 0x060054D8 RID: 21720 RVA: 0x001D42E8 File Offset: 0x001D26E8
	public static void Despawn(GameObject obj)
	{
		SimplePool.PoolMember component = obj.GetComponent<SimplePool.PoolMember>();
		if (component == null)
		{
			Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
			UnityEngine.Object.Destroy(obj);
		}
		else
		{
			component.myPool.Despawn(obj);
		}
	}

	// Token: 0x04003BE8 RID: 15336
	private const int DEFAULT_POOL_SIZE = 3;

	// Token: 0x04003BE9 RID: 15337
	private static Dictionary<GameObject, SimplePool.Pool> pools;

	// Token: 0x02000AF4 RID: 2804
	private class Pool
	{
		// Token: 0x060054D9 RID: 21721 RVA: 0x001D4339 File Offset: 0x001D2739
		public Pool(GameObject prefab, int initialQty)
		{
			this.prefab = prefab;
			this.inactive = new Stack<GameObject>(initialQty);
		}

		// Token: 0x060054DA RID: 21722 RVA: 0x001D435C File Offset: 0x001D275C
		public GameObject Spawn(Vector3 pos, Quaternion rot)
		{
			GameObject gameObject;
			if (this.inactive.Count == 0)
			{
				gameObject = (GameObject)AssetManagement.Instantiate(this.prefab, pos, rot);
				gameObject.name = string.Concat(new object[]
				{
					this.prefab.name,
					" (",
					this.nextId++,
					")"
				});
				gameObject.AddComponent<SimplePool.PoolMember>().myPool = this;
			}
			else
			{
				gameObject = this.inactive.Pop();
				if (gameObject == null)
				{
					return this.Spawn(pos, rot);
				}
			}
			gameObject.transform.position = pos;
			gameObject.transform.rotation = rot;
			gameObject.SetActive(true);
			return gameObject;
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x001D4426 File Offset: 0x001D2826
		public void Despawn(GameObject obj)
		{
			obj.SetActive(false);
			this.inactive.Push(obj);
		}

		// Token: 0x04003BEA RID: 15338
		private int nextId = 1;

		// Token: 0x04003BEB RID: 15339
		private Stack<GameObject> inactive;

		// Token: 0x04003BEC RID: 15340
		private GameObject prefab;
	}

	// Token: 0x02000AF5 RID: 2805
	private class PoolMember : MonoBehaviour
	{
		// Token: 0x04003BED RID: 15341
		public SimplePool.Pool myPool;
	}
}
