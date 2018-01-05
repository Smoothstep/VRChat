using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000481 RID: 1153
public class F3DPool : MonoBehaviour
{
	// Token: 0x060027E7 RID: 10215 RVA: 0x000CF9F4 File Offset: 0x000CDDF4
	private void Start()
	{
		F3DPool.instance = this;
		if (this.poolItems.Length > 0)
		{
			this.pool = new Dictionary<Transform, Transform[]>();
			for (int i = 0; i < this.poolItems.Length; i++)
			{
				Transform[] array = new Transform[this.poolLength[i]];
				for (int j = 0; j < this.poolLength[i]; j++)
				{
					Transform transform = UnityEngine.Object.Instantiate<Transform>(this.poolItems[i], Vector3.zero, Quaternion.identity);
					transform.gameObject.SetActive(false);
					transform.parent = base.transform;
					array[j] = transform;
				}
				this.pool.Add(this.poolItems[i], array);
			}
		}
		if (this.audioPoolItems.Length > 0)
		{
			this.audioPool = new Dictionary<AudioClip, AudioSource[]>();
			for (int k = 0; k < this.audioPoolItems.Length; k++)
			{
				AudioSource[] array2 = new AudioSource[this.audioPoolLength[k]];
				for (int l = 0; l < this.audioPoolLength[k]; l++)
				{
					AudioSource component = UnityEngine.Object.Instantiate<Transform>(this.audioSourcePrefab, Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
					component.clip = this.audioPoolItems[k];
					component.gameObject.SetActive(false);
					component.transform.parent = base.transform;
					array2[l] = component;
				}
				this.audioPool.Add(this.audioPoolItems[k], array2);
			}
		}
	}

	// Token: 0x060027E8 RID: 10216 RVA: 0x000CFB74 File Offset: 0x000CDF74
	public Transform Spawn(Transform obj, Vector3 pos, Quaternion rot, Transform parent)
	{
		for (int i = 0; i < this.pool[obj].Length; i++)
		{
			if (!this.pool[obj][i].gameObject.activeSelf)
			{
				Transform transform = this.pool[obj][i];
				transform.parent = parent;
				transform.position = pos;
				transform.rotation = rot;
				transform.gameObject.SetActive(true);
				transform.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
				return transform;
			}
		}
		return null;
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x000CFC00 File Offset: 0x000CE000
	public AudioSource SpawnAudio(AudioClip clip, Vector3 pos, Transform parent)
	{
		for (int i = 0; i < this.audioPool[clip].Length; i++)
		{
			if (!this.audioPool[clip][i].gameObject.activeSelf)
			{
				AudioSource audioSource = this.audioPool[clip][i];
				audioSource.transform.parent = parent;
				audioSource.transform.position = pos;
				audioSource.gameObject.SetActive(true);
				audioSource.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
				return audioSource;
			}
		}
		return null;
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x000CFC8B File Offset: 0x000CE08B
	public void Despawn(Transform obj)
	{
		obj.BroadcastMessage("OnDespawned", SendMessageOptions.DontRequireReceiver);
		obj.gameObject.SetActive(false);
	}

	// Token: 0x04001618 RID: 5656
	public static F3DPool instance;

	// Token: 0x04001619 RID: 5657
	[Header("VFX Pool")]
	public Transform[] poolItems;

	// Token: 0x0400161A RID: 5658
	public int[] poolLength;

	// Token: 0x0400161B RID: 5659
	[Header("Audio Pool")]
	public Transform audioSourcePrefab;

	// Token: 0x0400161C RID: 5660
	public AudioClip[] audioPoolItems;

	// Token: 0x0400161D RID: 5661
	public int[] audioPoolLength;

	// Token: 0x0400161E RID: 5662
	private Dictionary<Transform, Transform[]> pool;

	// Token: 0x0400161F RID: 5663
	private Dictionary<AudioClip, AudioSource[]> audioPool;
}
