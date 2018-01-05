using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.Core;

// Token: 0x020008CB RID: 2251
public class SU_SpaceSceneSwitcher : MonoBehaviour
{
	// Token: 0x060044B9 RID: 17593 RVA: 0x0016FD00 File Offset: 0x0016E100
	private void Start()
	{
		SU_SpaceSceneSwitcher.staticMode = this.mode;
		if (this.spaceScenes.Length > 0)
		{
			if (SU_SpaceSceneSwitcher.staticSpaceScenes.Count == 0)
			{
				for (int i = 0; i < this.spaceScenes.Length; i++)
				{
					SU_SpaceSceneSwitcher.staticSpaceScenes.Add(this.spaceScenes[i]);
				}
			}
		}
		else
		{
			Debug.LogError("No Space Scene Prefabs configured for the Space Scene array. Populate array in the inspector with Space Scene prefabs from the Project window. Note! You have to create Prefabs(!) of the space scenes - you cannot assign Unity Scenes to the array.");
		}
		if (SU_SpaceSceneSwitcher.staticMode == SU_SpaceSceneSwitcher.Mode.LOAD_ALL_AT_STARTUP)
		{
			SU_SpaceSceneSwitcher.hSpaceScenes.Clear();
			foreach (GameObject gameObject in this.spaceScenes)
			{
				GameObject gameObject2 = (GameObject)AssetManagement.Instantiate(gameObject, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f));
				SU_SpaceSceneSwitcher.hSpaceScenes.Add(gameObject.name, gameObject2);
				SU_SpaceSceneSwitcher.SetActive(gameObject2, false);
			}
		}
		if (this.sceneIndexLoadFirst >= this.spaceScenes.Length)
		{
			Debug.LogWarning("Scene Index Load First value is greater than the number of Space Scene prefabs in the array. Loading scene with index 0 instead.");
			this.sceneIndexLoadFirst = 0;
			SU_SpaceSceneSwitcher.Switch(this.sceneIndexLoadFirst);
		}
		else
		{
			SU_SpaceSceneSwitcher.Switch(this.sceneIndexLoadFirst);
		}
	}

	// Token: 0x060044BA RID: 17594 RVA: 0x0016FE35 File Offset: 0x0016E235
	public static void Switch(int _arrayIndex)
	{
		if (SU_SpaceSceneSwitcher.staticSpaceScenes.Count > 0)
		{
			SU_SpaceSceneSwitcher.Switch(SU_SpaceSceneSwitcher.staticSpaceScenes[_arrayIndex].name);
		}
	}

	// Token: 0x060044BB RID: 17595 RVA: 0x0016FE5C File Offset: 0x0016E25C
	public static void Switch(string _sceneName)
	{
		for (int i = 0; i < SU_SpaceSceneSwitcher.staticSpaceScenes.Count; i++)
		{
			GameObject gameObject = SU_SpaceSceneSwitcher.staticSpaceScenes[i];
			if (gameObject != null && gameObject.name == _sceneName)
			{
				if (SU_SpaceSceneSwitcher.staticMode != SU_SpaceSceneSwitcher.Mode.LOAD_ALL_AT_STARTUP)
				{
					UnityEngine.Object.Destroy(SU_SpaceSceneSwitcher.currentSpaceScene);
					SU_SpaceSceneSwitcher.currentSpaceScene = (GameObject)AssetManagement.Instantiate(gameObject, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f));
					return;
				}
				if (SU_SpaceSceneSwitcher.hSpaceScenes[_sceneName] != null)
				{
					bool flag = false;
					IDictionaryEnumerator enumerator = SU_SpaceSceneSwitcher.hSpaceScenes.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
							if (dictionaryEntry.Key.ToString() == _sceneName)
							{
								SU_SpaceSceneSwitcher.SetActive((GameObject)dictionaryEntry.Value, true);
								flag = true;
							}
							else
							{
								SU_SpaceSceneSwitcher.SetActive((GameObject)dictionaryEntry.Value, false);
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					if (flag)
					{
						return;
					}
				}
			}
		}
		Debug.LogWarning("Tried to switch to a space scene named " + _sceneName + " but the scene was not found. Ensure that you configured the array on the SpaceSceneSwitcher prefab correctly and that you typed the name of the space scene prefab correctly (case sensitive) for the Switch function call");
	}

	// Token: 0x060044BC RID: 17596 RVA: 0x0016FFCC File Offset: 0x0016E3CC
	public static void SetActive(GameObject _gameObject, bool _bool)
	{
	}

	// Token: 0x04002EA7 RID: 11943
	public SU_SpaceSceneSwitcher.Mode mode = SU_SpaceSceneSwitcher.Mode.LOAD_ON_DEMAND;

	// Token: 0x04002EA8 RID: 11944
	public static SU_SpaceSceneSwitcher.Mode staticMode;

	// Token: 0x04002EA9 RID: 11945
	public GameObject[] spaceScenes;

	// Token: 0x04002EAA RID: 11946
	public static List<GameObject> staticSpaceScenes = new List<GameObject>();

	// Token: 0x04002EAB RID: 11947
	public static Hashtable hSpaceScenes = new Hashtable();

	// Token: 0x04002EAC RID: 11948
	public static GameObject currentSpaceScene;

	// Token: 0x04002EAD RID: 11949
	public int sceneIndexLoadFirst;

	// Token: 0x020008CC RID: 2252
	public enum Mode
	{
		// Token: 0x04002EAF RID: 11951
		LOAD_ALL_AT_STARTUP,
		// Token: 0x04002EB0 RID: 11952
		LOAD_ON_DEMAND
	}
}
