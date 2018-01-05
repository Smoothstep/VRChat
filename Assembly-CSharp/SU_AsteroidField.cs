using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.Core;

// Token: 0x020008C6 RID: 2246
public class SU_AsteroidField : MonoBehaviour
{
	// Token: 0x060044A5 RID: 17573 RVA: 0x0016EE10 File Offset: 0x0016D210
	private void OnEnable()
	{
		this._cacheTransform = base.transform;
		this._distanceToSpawn = this.range * this.distanceSpawn;
		this._distanceToFade = this.range * this.distanceSpawn * this.distanceFade;
		if (this.fadeAsteroids && this._materialsTransparent.Count == 0)
		{
			this.CreateTransparentMaterial(this.materialVeryCommon, this._materialsTransparent);
			this.CreateTransparentMaterial(this.materialCommon, this._materialsTransparent);
			this.CreateTransparentMaterial(this.materialRare, this._materialsTransparent);
			this.CreateTransparentMaterial(this.materialVeryRare, this._materialsTransparent);
		}
		if (this._materialList.Count == 0)
		{
			if (this.materialVeryRare.Length > 0)
			{
				this._materialList.Add(5, "VeryRare");
			}
			if (this.materialRare.Length > 0)
			{
				this._materialList.Add(20, "Rare");
			}
			if (this.materialCommon.Length > 0)
			{
				this._materialList.Add(50, "Common");
			}
			if (this.materialVeryCommon.Length != 0)
			{
				this._materialList.Add(100, "VeryCommon");
			}
			else
			{
				Debug.LogError("Asteroid Field must have at least one Material in the 'Material Very Common' Array.");
			}
		}
		for (int i = 0; i < this._asteroids.Count; i++)
		{
		}
		this.SpawnAsteroids(false);
	}

	// Token: 0x060044A6 RID: 17574 RVA: 0x0016EF7C File Offset: 0x0016D37C
	private void OnDisable()
	{
		for (int i = 0; i < this._asteroids.Count; i++)
		{
			if (this._asteroids[i] != null)
			{
			}
		}
	}

	// Token: 0x060044A7 RID: 17575 RVA: 0x0016EFBC File Offset: 0x0016D3BC
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position, this.range);
	}

	// Token: 0x060044A8 RID: 17576 RVA: 0x0016EFE0 File Offset: 0x0016D3E0
	private void Update()
	{
		for (int i = 0; i < this._asteroids.Count; i++)
		{
			Transform transform = this._asteroids[i];
			if (transform != null)
			{
				float num = Vector3.Distance(transform.position, this._cacheTransform.position);
				if (num > this.range && this.respawnIfOutOfRange)
				{
					transform.position = UnityEngine.Random.onUnitSphere * this._distanceToSpawn + this._cacheTransform.position;
					float num2 = UnityEngine.Random.Range(this.minAsteroidScale, this.maxAsteroidScale) * this.scaleMultiplier;
					transform.localScale = new Vector3(num2, num2, num2);
					Vector3 eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
					transform.eulerAngles = eulerAngles;
				}
				if (this.fadeAsteroids)
				{
					if (num > this._distanceToFade)
					{
						if (!this._asteroidsFading[i])
						{
							transform.GetComponent<Renderer>().sharedMaterial = (Material)this._materialsTransparent[this._asteroidsMaterials[i]];
							this._asteroidsFading[i] = true;
						}
						Color color = transform.GetComponent<Renderer>().material.color;
						float a = Mathf.Clamp01(1f - (num - this._distanceToFade) / (this._distanceToSpawn - this._distanceToFade));
						transform.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, a);
					}
					else if (this._asteroidsFading[i])
					{
						transform.GetComponent<Renderer>().material = this._asteroidsMaterials[i];
						this._asteroidsFading[i] = false;
					}
				}
			}
			else
			{
				this._asteroids.RemoveAt(i);
				this._asteroidsMaterials.RemoveAt(i);
				this._asteroidsFading.RemoveAt(i);
			}
			if (this.respawnDestroyedAsteroids && this._asteroids.Count < this.maxAsteroids)
			{
				this.SpawnAsteroids(true);
			}
		}
	}

	// Token: 0x060044A9 RID: 17577 RVA: 0x0016F218 File Offset: 0x0016D618
	private void SpawnAsteroids(bool atSpawnDistance)
	{
		while (this._asteroids.Count < this.maxAsteroids)
		{
			Transform asset = this.prefabAsteroids[UnityEngine.Random.Range(0, this.prefabAsteroids.Length)];
			Vector3 pos = Vector3.zero;
			if (atSpawnDistance)
			{
				pos = this._cacheTransform.position + UnityEngine.Random.onUnitSphere * this._distanceToSpawn;
			}
			else
			{
				pos = this._cacheTransform.position + UnityEngine.Random.insideUnitSphere * this._distanceToSpawn;
			}
			Transform transform = (Transform)AssetManagement.Instantiate(asset, pos, this._cacheTransform.rotation);
			string text = SU_AsteroidField.WeightedRandom<string>(this._materialList);
			if (text != null)
			{
				if (!(text == "VeryCommon"))
				{
					if (!(text == "Common"))
					{
						if (!(text == "Rare"))
						{
							if (text == "VeryRare")
							{
								transform.GetComponent<Renderer>().sharedMaterial = this.materialVeryRare[UnityEngine.Random.Range(0, this.materialVeryRare.Length)];
							}
						}
						else
						{
							transform.GetComponent<Renderer>().sharedMaterial = this.materialRare[UnityEngine.Random.Range(0, this.materialRare.Length)];
						}
					}
					else
					{
						transform.GetComponent<Renderer>().sharedMaterial = this.materialCommon[UnityEngine.Random.Range(0, this.materialCommon.Length)];
					}
				}
				else
				{
					transform.GetComponent<Renderer>().sharedMaterial = this.materialVeryCommon[UnityEngine.Random.Range(0, this.materialVeryCommon.Length)];
				}
			}
			this._asteroids.Add(transform);
			this._asteroidsMaterials.Add(transform.GetComponent<Renderer>().sharedMaterial);
			this._asteroidsFading.Add(false);
			if (transform.GetComponent<SU_Asteroid>() != null)
			{
				transform.GetComponent<SU_Asteroid>().SetPolyCount(this.polyCount);
				if (transform.GetComponent<Collider>() != null)
				{
					transform.GetComponent<SU_Asteroid>().SetPolyCount(this.polyCountCollider, true);
				}
			}
			float num = UnityEngine.Random.Range(this.minAsteroidScale, this.maxAsteroidScale) * this.scaleMultiplier;
			transform.localScale = new Vector3(num, num, num);
			transform.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
			if (this.isRigidbody)
			{
				if (transform.GetComponent<Rigidbody>() != null)
				{
					transform.GetComponent<Rigidbody>().mass = this.mass * num;
					transform.GetComponent<Rigidbody>().velocity = transform.transform.forward * UnityEngine.Random.Range(this.minAsteroidVelocity, this.maxAsteroidVelocity) * this.velocityMultiplier;
					transform.GetComponent<Rigidbody>().angularVelocity = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)) * UnityEngine.Random.Range(this.minAsteroidAngularVelocity, this.maxAsteroidAngularVelocity) * this.angularVelocityMultiplier;
				}
				else
				{
					Debug.LogWarning("AsteroidField is set to spawn rigidbody asterodids but one or more asteroid prefabs do not have rigidbody component attached.");
				}
			}
			else
			{
				if (transform.GetComponent<Rigidbody>() != null)
				{
					UnityEngine.Object.Destroy(transform.GetComponent<Rigidbody>());
				}
				if (transform.GetComponent<SU_Asteroid>() != null)
				{
					transform.GetComponent<SU_Asteroid>().rotationSpeed = UnityEngine.Random.Range(this.minAsteroidRotationSpeed, this.maxAsteroidRotationSpeed);
					transform.GetComponent<SU_Asteroid>().rotationalAxis = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
					transform.GetComponent<SU_Asteroid>().driftSpeed = UnityEngine.Random.Range(this.minAsteroidDriftSpeed, this.maxAsteroidDriftSpeed);
					transform.GetComponent<SU_Asteroid>().driftAxis = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
				}
			}
		}
	}

	// Token: 0x060044AA RID: 17578 RVA: 0x0016F634 File Offset: 0x0016DA34
	private void CreateTransparentMaterial(Material[] _sourceMaterials, Hashtable _ht)
	{
		foreach (Material material in _sourceMaterials)
		{
			_ht.Add(material, new Material(Shader.Find("SpaceUnity/Asteroid Transparent")));
			((Material)_ht[material]).SetTexture("_MainTex", material.GetTexture("_MainTex"));
			((Material)_ht[material]).color = material.color;
		}
	}

	// Token: 0x060044AB RID: 17579 RVA: 0x0016F6AC File Offset: 0x0016DAAC
	private static T WeightedRandom<T>(SortedList<int, T> _list)
	{
		int max = _list.Keys[_list.Keys.Count - 1];
		int num = UnityEngine.Random.Range(0, max);
		foreach (int num2 in _list.Keys)
		{
			if (num <= num2)
			{
				return _list[num2];
			}
		}
		return default(T);
	}

	// Token: 0x04002E65 RID: 11877
	public SU_Asteroid.PolyCount polyCount;

	// Token: 0x04002E66 RID: 11878
	public SU_Asteroid.PolyCount polyCountCollider = SU_Asteroid.PolyCount.LOW;

	// Token: 0x04002E67 RID: 11879
	public Transform[] prefabAsteroids;

	// Token: 0x04002E68 RID: 11880
	public Material[] materialVeryCommon;

	// Token: 0x04002E69 RID: 11881
	public Material[] materialCommon;

	// Token: 0x04002E6A RID: 11882
	public Material[] materialRare;

	// Token: 0x04002E6B RID: 11883
	public Material[] materialVeryRare;

	// Token: 0x04002E6C RID: 11884
	public float range = 20000f;

	// Token: 0x04002E6D RID: 11885
	public int maxAsteroids;

	// Token: 0x04002E6E RID: 11886
	public bool respawnDestroyedAsteroids = true;

	// Token: 0x04002E6F RID: 11887
	public bool respawnIfOutOfRange = true;

	// Token: 0x04002E70 RID: 11888
	public float distanceSpawn = 0.95f;

	// Token: 0x04002E71 RID: 11889
	public float minAsteroidScale = 0.1f;

	// Token: 0x04002E72 RID: 11890
	public float maxAsteroidScale = 1f;

	// Token: 0x04002E73 RID: 11891
	public float scaleMultiplier = 1f;

	// Token: 0x04002E74 RID: 11892
	public bool isRigidbody;

	// Token: 0x04002E75 RID: 11893
	public float minAsteroidRotationSpeed;

	// Token: 0x04002E76 RID: 11894
	public float maxAsteroidRotationSpeed = 1f;

	// Token: 0x04002E77 RID: 11895
	public float rotationSpeedMultiplier = 1f;

	// Token: 0x04002E78 RID: 11896
	public float minAsteroidDriftSpeed;

	// Token: 0x04002E79 RID: 11897
	public float maxAsteroidDriftSpeed = 1f;

	// Token: 0x04002E7A RID: 11898
	public float driftSpeedMultiplier = 1f;

	// Token: 0x04002E7B RID: 11899
	public float mass = 1f;

	// Token: 0x04002E7C RID: 11900
	public float minAsteroidAngularVelocity;

	// Token: 0x04002E7D RID: 11901
	public float maxAsteroidAngularVelocity = 1f;

	// Token: 0x04002E7E RID: 11902
	public float angularVelocityMultiplier = 1f;

	// Token: 0x04002E7F RID: 11903
	public float minAsteroidVelocity;

	// Token: 0x04002E80 RID: 11904
	public float maxAsteroidVelocity = 1f;

	// Token: 0x04002E81 RID: 11905
	public float velocityMultiplier = 1f;

	// Token: 0x04002E82 RID: 11906
	public bool fadeAsteroids = true;

	// Token: 0x04002E83 RID: 11907
	public float distanceFade = 0.95f;

	// Token: 0x04002E84 RID: 11908
	private float _distanceToSpawn;

	// Token: 0x04002E85 RID: 11909
	private float _distanceToFade;

	// Token: 0x04002E86 RID: 11910
	private Transform _cacheTransform;

	// Token: 0x04002E87 RID: 11911
	private List<Transform> _asteroids = new List<Transform>();

	// Token: 0x04002E88 RID: 11912
	private List<Material> _asteroidsMaterials = new List<Material>();

	// Token: 0x04002E89 RID: 11913
	private List<bool> _asteroidsFading = new List<bool>();

	// Token: 0x04002E8A RID: 11914
	private Hashtable _materialsTransparent = new Hashtable();

	// Token: 0x04002E8B RID: 11915
	private SortedList<int, string> _materialList = new SortedList<int, string>(4);
}
