using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRCSDK2;

namespace VRC.Core
{
	// Token: 0x02000A58 RID: 2648
	public class AssetManagement : MonoBehaviour
	{
		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06005016 RID: 20502 RVA: 0x001B611D File Offset: 0x001B451D
		// (set) Token: 0x06005017 RID: 20503 RVA: 0x001B6124 File Offset: 0x001B4524
		public static bool SceneHasLightProbes { get; private set; }

		// Token: 0x06005018 RID: 20504 RVA: 0x001B612C File Offset: 0x001B452C
		private void Awake()
		{
			if (AssetManagement._instance == null)
			{
				AssetManagement._instance = this;
				if (this.FailShader == null)
				{
					this.FailShader = Shader.Find("Standard");
				}
				if (this.ReplaceShader == null)
				{
					this.ReplaceShader = Shader.Find("Standard");
				}
				return;
			}
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x001B619D File Offset: 0x001B459D
		private void AddAction(Action action)
		{
			this.DeferredActions.Enqueue(action);
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x001B61AB File Offset: 0x001B45AB
		public void ClearWorkQueue()
		{
			this.DeferredActions.Clear();
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x001B61B8 File Offset: 0x001B45B8
		private void LateUpdate()
		{
			if (this.DeferredActions.Empty)
			{
				return;
			}
			Action[] array = this.DeferredActions.DumpToArray();
			foreach (Action action in array)
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Debug.LogError("An error occurred while processing Asset actions: \n" + ex.ToString());
				}
			}
		}

		// Token: 0x0600501C RID: 20508 RVA: 0x001B6234 File Offset: 0x001B4634
		private void OnDestroy()
		{
			if (AssetManagement._instance == this)
			{
				AssetManagement._instance = null;
			}
		}

		// Token: 0x0600501D RID: 20509 RVA: 0x001B624C File Offset: 0x001B464C
		public static void FixMaterialsInLevel()
		{
			if (AssetManagement.Instance == null)
			{
				return;
			}
			AssetManagement.Instance.ClearWorkQueue();
			AssetManagement.sceneDescriptors = null;
			Action fixAction = null;
			fixAction = delegate
			{
				if (AssetManagement.AllScenesAreLoaded && AssetManagement.SceneDescriptors.FirstOrDefault<VRC_SceneDescriptor>() != null && Time.timeSinceLevelLoad > 0.5f)
				{
					Debug.Log("Starting Materials Scan");
					Debug.Log("Client Unity Version: \"" + Application.unityVersion + "\"");
					Debug.Log("Scene Unity Version: \"" + AssetManagement.SceneDescriptors.First<VRC_SceneDescriptor>().unityVersion + "\"");
					AssetManagement.Instance.FixMaterials();
				}
				else
				{
					AssetManagement.Instance.AddAction(fixAction);
				}
			};
			AssetManagement.Instance.AddAction(fixAction);
			AssetManagement.SceneHasLightProbes = (UnityEngine.Object.FindObjectOfType(typeof(LightProbeGroup)) != null);
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x0600501E RID: 20510 RVA: 0x001B62C3 File Offset: 0x001B46C3
		public static IEnumerable<VRC_SceneDescriptor> SceneDescriptors
		{
			get
			{
				if (AssetManagement.sceneDescriptors == null)
				{
					AssetManagement.sceneDescriptors = AssetManagement.FindObjects<VRC_SceneDescriptor>().ToArray<VRC_SceneDescriptor>();
				}
				return AssetManagement.sceneDescriptors;
			}
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x001B62E3 File Offset: 0x001B46E3
		public static T Instantiate<T>(UnityEngine.Object asset) where T : class
		{
			return AssetManagement.Instantiate(asset) as T;
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x001B62FA File Offset: 0x001B46FA
		public static T Instantiate<T>(UnityEngine.Object asset, Vector3 pos, Quaternion rot) where T : class
		{
			return AssetManagement.Instantiate(asset, pos, rot) as T;
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x001B6313 File Offset: 0x001B4713
		public new static UnityEngine.Object Instantiate(UnityEngine.Object asset)
		{
			return AssetManagement.Instantiate(asset, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f));
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x001B6348 File Offset: 0x001B4748
		public new static UnityEngine.Object Instantiate(UnityEngine.Object asset, Vector3 pos, Quaternion rot)
		{
			UnityEngine.Object result;
			try
			{
				UnityEngine.Object @object = UnityEngine.Object.Instantiate(asset, pos, rot);
				GameObject go = @object as GameObject;
				if (go != null)
				{
					Action fixAction = null;
					fixAction = delegate
					{
						if (go == null)
						{
							return;
						}
						if (go.GetActive() && AssetManagement.SceneDescriptors.FirstOrDefault<VRC_SceneDescriptor>() != null)
						{
							AssetManagement.Instance.FixComponents(go);
						}
						else
						{
							AssetManagement.Instance.AddAction(fixAction);
						}
					};
					AssetManagement.Instance.AddAction(fixAction);
				}
				result = @object;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				result = null;
			}
			return result;
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x001B63E0 File Offset: 0x001B47E0
		public static void LoadLevel(int levelNumber)
		{
			VRCFlowManager.BlockResetGameFlow = true;
			try
			{
				Resources.UnloadUnusedAssets();
				SceneManager.LoadScene(levelNumber);
			}
			finally
			{
				VRCFlowManager.BlockResetGameFlow = false;
			}
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x001B641C File Offset: 0x001B481C
		public static void LoadLevel(string sceneName)
		{
			VRCFlowManager.BlockResetGameFlow = true;
			try
			{
				Resources.UnloadUnusedAssets();
				SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			}
			finally
			{
				VRCFlowManager.BlockResetGameFlow = false;
			}
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x001B6458 File Offset: 0x001B4858
		public static void LoadLevelAdditive(string sceneName)
		{
			VRCFlowManager.BlockResetGameFlow = true;
			try
			{
				Resources.UnloadUnusedAssets();
				SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
			}
			finally
			{
				VRCFlowManager.BlockResetGameFlow = false;
			}
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x001B6494 File Offset: 0x001B4894
		public static IEnumerator LoadLevelAsync(string sceneName, LoadSceneMode mode, float timeOut = 3.40282347E+38f, Action onSuccess = null)
		{
			float startTime = Time.time;
			if (!Application.isEditor)
			{
				AsyncOperation async = null;
				VRCFlowManager.BlockResetGameFlow = true;
				try
				{
					Resources.UnloadUnusedAssets();
					async = SceneManager.LoadSceneAsync(sceneName, mode);
					async.allowSceneActivation = true;
				}
				finally
				{
					VRCFlowManager.BlockResetGameFlow = false;
				}
				while (!async.isDone && Time.time - startTime < timeOut)
				{
					yield return null;
				}
				if (!async.isDone)
				{
					Analytics.Send(ApiAnalyticEvent.EventType.error, "WorldLoadFailed: LoadLevelAsync timed out: " + sceneName, null, null);
					Debug.LogError("LoadLevelAsync timed out");
					yield break;
				}
			}
			else
			{
				Debug.LogWarning("Loading scene synchronously in editor");
				VRCFlowManager.BlockResetGameFlow = true;
				try
				{
					Resources.UnloadUnusedAssets();
					SceneManager.LoadScene(sceneName, mode);
				}
				finally
				{
					VRCFlowManager.BlockResetGameFlow = false;
				}
				yield return null;
				yield return null;
			}
			Scene scene = SceneManager.GetSceneByName(sceneName);
			if (!scene.isLoaded)
			{
				Debug.LogError("Scene wasn't loaded");
				yield break;
			}
			while (!scene.isLoaded && Time.time - startTime < timeOut)
			{
				yield return null;
			}
			if (!scene.isLoaded)
			{
				Debug.LogError("Loading Scene Manager timed out");
				yield break;
			}
			if (onSuccess != null)
			{
				onSuccess();
			}
			yield break;
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x001B64C4 File Offset: 0x001B48C4
		public static IEnumerable<T> FindObjects<T>() where T : Component
		{
			foreach (GameObject obj in AssetManagement.AllGameObjects)
			{
				foreach (T f in obj.GetComponents<T>())
				{
					yield return f;
				}
			}
			yield break;
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06005028 RID: 20520 RVA: 0x001B64E0 File Offset: 0x001B48E0
		public static IEnumerable<GameObject> AllGameObjects
		{
			get
			{
				Queue<Transform> transforms = new Queue<Transform>(64);
				foreach (GameObject root in AssetManagement.RootGameObjects)
				{
					if (!(root == null))
					{
						transforms.Enqueue(root.transform);
						while (transforms.Count > 0)
						{
							Transform current = transforms.Dequeue();
							if (current != null && current.gameObject != null)
							{
								yield return current.gameObject;
							}
							if (!(current == null))
							{
								for (int i = 0; i < current.childCount; i++)
								{
									Transform child = current.GetChild(i);
									if (!(child == null))
									{
										transforms.Enqueue(child);
									}
								}
							}
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06005029 RID: 20521 RVA: 0x001B64FC File Offset: 0x001B48FC
		public static IEnumerable<GameObject> RootGameObjects
		{
			get
			{
				for (int idx = 0; idx < SceneManager.sceneCount; idx++)
				{
					Scene scene = SceneManager.GetSceneAt(idx);
					if (scene.isLoaded && scene.rootCount > 0)
					{
						GameObject[] roots = scene.GetRootGameObjects();
						foreach (GameObject r in roots)
						{
							if (r != null)
							{
								yield return r;
							}
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x0600502A RID: 20522 RVA: 0x001B6518 File Offset: 0x001B4918
		public static bool AllScenesAreLoaded
		{
			get
			{
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					if (!SceneManager.GetSceneAt(i).isLoaded)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x0600502B RID: 20523 RVA: 0x001B6551 File Offset: 0x001B4951
		public static AssetManagement Instance
		{
			get
			{
				return AssetManagement._instance;
			}
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x001B6558 File Offset: 0x001B4958
		private void FixMaterials()
		{
			this.ClearWorkQueue();
			foreach (GameObject obj in AssetManagement.RootGameObjects)
			{
				this.FixComponents(obj);
			}
			if (RenderSettings.skybox != null)
			{
				this.AddAction(delegate
				{
					RenderSettings.skybox = this.FixMaterial(RenderSettings.skybox, 0);
				});
			}
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x001B65D8 File Offset: 0x001B49D8
		private void FixComponents(UnityEngine.Object obj)
		{
			if (obj == null)
			{
				return;
			}
			if (obj is Transform)
			{
				Transform transform = obj as Transform;
				for (int i = 0; i < transform.childCount; i++)
				{
					this.FixComponents(transform.GetChild(i));
				}
				if (transform.gameObject != null)
				{
					foreach (Component component in transform.gameObject.GetComponents(typeof(Component)))
					{
						if (!(component is Transform))
						{
							this.FixComponents(component);
						}
					}
				}
			}
			else if (obj is GameObject)
			{
				GameObject gameObject = obj as GameObject;
				this.FixComponents(gameObject.transform);
			}
			else if (obj is Renderer)
			{
				Renderer renderer = obj as Renderer;
				this.AddAction(delegate
				{
					this.FixRenderer(renderer);
				});
			}
			else if (obj is Terrain)
			{
				Terrain terr = obj as Terrain;
				if (terr.terrainData != null)
				{
					if (terr.terrainData.treePrototypes != null)
					{
						foreach (TreePrototype treePrototype in terr.terrainData.treePrototypes)
						{
							if (treePrototype != null)
							{
								this.FixComponents(treePrototype.prefab);
							}
						}
					}
					if (terr.terrainData.detailPrototypes != null)
					{
						foreach (DetailPrototype detailPrototype in terr.terrainData.detailPrototypes)
						{
							if (detailPrototype != null)
							{
								this.FixComponents(detailPrototype.prototype);
							}
						}
					}
					this.AddAction(delegate
					{
						terr.terrainData.RefreshPrototypes();
					});
				}
			}
			else if (obj is Skybox)
			{
				this.AddAction(delegate
				{
					(obj as Skybox).material = this.FixMaterial((obj as Skybox).material, 0);
				});
			}
			else if (obj is AudioSource)
			{
				AudioSource audioSource = obj as AudioSource;
				AudioClipWarmer.Warm(audioSource.clip);
			}
			else if (obj is VRC_AudioBank)
			{
				foreach (AudioClip clip in (obj as VRC_AudioBank).Clips)
				{
					AudioClipWarmer.Warm(clip);
				}
			}
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x001B68D4 File Offset: 0x001B4CD4
		private Shader FixShader(Shader shader, int layer)
		{
			if (!shader.name.Contains("InternalErrorShader") && !(shader == null) && !string.IsNullOrEmpty(shader.name))
			{
				Shader shader2 = shader;
				if (!shader.name.Contains("Legacy"))
				{
					if (!AssetManagement.SceneDescriptors.Any((VRC_SceneDescriptor sd) => sd.unityVersion.StartsWith("5.5") || sd.unityVersion.StartsWith("5.4") || sd.unityVersion.StartsWith("5.3")))
					{
						return shader2;
					}
				}
				shader2 = Shader.Find(shader.name);
				if (shader2 == null)
				{
					shader2 = shader;
				}
				else
				{
					shader2.maximumLOD = shader.maximumLOD;
					shader2.hideFlags = shader.hideFlags;
				}
				return shader2;
			}
			if (layer == LayerMask.NameToLayer("PlayerLocal") || RoomManager.isTestRoom)
			{
				return this.FailShader;
			}
			return this.ReplaceShader;
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x001B69B8 File Offset: 0x001B4DB8
		private void FixRenderer(Renderer renderer)
		{
			if (renderer == null)
			{
				return;
			}
			int layer = renderer.gameObject.layer;
			Material[] sharedMaterials = renderer.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				sharedMaterials[i] = this.FixMaterial(sharedMaterials[i], layer);
			}
			renderer.sharedMaterials = sharedMaterials;
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x001B6A10 File Offset: 0x001B4E10
		private Material FixMaterial(Material material, int layer = 0)
		{
			if (material == null)
			{
				return material;
			}
			try
			{
				Shader shader = material.shader;
				material.shader = this.FixShader(material.shader, layer);
				if (this.StandardShaders.Any((Shader s) => s.name == material.shader.name))
				{
					StandardShaderHack.MaterialChanged(material);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Encountered an exception when fixing material " + material.name + ":\n" + ex.ToString());
			}
			return material;
		}

		// Token: 0x040038F2 RID: 14578
		public Texture2D DisabledTexture;

		// Token: 0x040038F3 RID: 14579
		public Shader FailShader;

		// Token: 0x040038F4 RID: 14580
		public Shader ReplaceShader;

		// Token: 0x040038F6 RID: 14582
		private ConcurrentQueue<Action> DeferredActions = new ConcurrentQueue<Action>();

		// Token: 0x040038F7 RID: 14583
		private static VRC_SceneDescriptor[] sceneDescriptors;

		// Token: 0x040038F8 RID: 14584
		private static AssetManagement _instance;

		// Token: 0x040038F9 RID: 14585
		public Shader[] StandardShaders;
	}
}
