using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000B59 RID: 2905
public class ObjectInstantiator : VRCPunBehaviour
{
	// Token: 0x060058F7 RID: 22775 RVA: 0x001ED1B8 File Offset: 0x001EB5B8
	private void LateUpdate()
	{
		this.lastUsedSpawnID = 0;
	}

	// Token: 0x060058F8 RID: 22776 RVA: 0x001ED1C4 File Offset: 0x001EB5C4
	public GameObject InstantiateObject(VRC_EventHandler.VrcBroadcastType broadcast, string prefabName, Vector3 position, Quaternion rotation)
	{
		if (VRC.Network.SceneEventHandler == null)
		{
			Debug.LogError("Could not instantiate " + prefabName + ": Instantiator is not initialized", base.gameObject);
			return null;
		}
		if (!VRC_EventDispatcherRFC.IsValidExecutor(VRC.Network.LocalPlayer, broadcast, base.gameObject))
		{
			Debug.LogError("Could not instantiate " + prefabName + ": Client is not a valid remote for " + broadcast.ToString(), base.gameObject);
			return null;
		}
		VRC_EventHandler.VrcTargetType vrcTargetType;
		switch (broadcast)
		{
		case VRC_EventHandler.VrcBroadcastType.Always:
		case VRC_EventHandler.VrcBroadcastType.Master:
		case VRC_EventHandler.VrcBroadcastType.Owner:
			vrcTargetType = VRC_EventHandler.VrcTargetType.AllBuffered;
			break;
		case VRC_EventHandler.VrcBroadcastType.Local:
			vrcTargetType = VRC_EventHandler.VrcTargetType.Local;
			break;
		default:
			Debug.LogError("Could not instantiate " + prefabName + ": Instantiation cannot be unbuffered.", base.gameObject);
			return null;
		}
		if (broadcast > VRC_EventHandler.VrcBroadcastType.Owner)
		{
			Debug.LogError("Could not instantiate " + prefabName + ": Instantiation cannot be unbuffered.", base.gameObject);
			return null;
		}
		ObjectInstantiator.PrefabInfo prefabInfo = this._FetchPrefab(prefabName);
		if (prefabInfo == null)
		{
			Debug.LogError("Could not instantiate " + prefabName + ": Could not locate prefab", base.gameObject);
			return null;
		}
		int num = Math.Max(1, prefabInfo.viewCount);
		int[] array = VRC.Network.AllocateSubIDs(num);
		if (array == null)
		{
			Debug.LogError("Could not instantiate " + prefabName + ": could not allocate IDs.", base.gameObject);
			return null;
		}
		byte[] array2 = new byte[2 + 4 * num + 28];
		int num2 = 0;
		Protocol.Serialize(position.x, array2, ref num2);
		Protocol.Serialize(position.y, array2, ref num2);
		Protocol.Serialize(position.z, array2, ref num2);
		Protocol.Serialize(rotation.x, array2, ref num2);
		Protocol.Serialize(rotation.y, array2, ref num2);
		Protocol.Serialize(rotation.z, array2, ref num2);
		Protocol.Serialize(rotation.w, array2, ref num2);
		Protocol.Serialize((short)num, array2, ref num2);
		foreach (int value in array)
		{
			Protocol.Serialize(value, array2, ref num2);
		}
		int? num3 = this.MakeID();
		if (num3 == null)
		{
			Debug.LogError("Could not instantiate " + prefabName + ": Ran out of IDs", base.gameObject);
			VRC.Network.UnAllocateSubIDs(array);
			return null;
		}
		this.spawnedObjects.Add(num3.Value, new ObjectInstantiator.ObjectInfo
		{
			data = array2,
			gameObject = null,
			prefabName = prefabName,
			localId = num3.Value
		});
		this.DebugPrint("Instantiating " + prefabName, new object[]
		{
			base.gameObject
		});
		VRC.Network.RPC(vrcTargetType, base.gameObject, "_InstantiateObject", new object[]
		{
			prefabName,
			array2,
			num3.Value
		});
		VRC.Network.RPC((vrcTargetType != VRC_EventHandler.VrcTargetType.Local) ? VRC_EventHandler.VrcTargetType.All : VRC_EventHandler.VrcTargetType.Local, base.gameObject, "_SendOnSpawn", new object[]
		{
			num3.Value
		});
		return this.spawnedObjects[num3.Value].gameObject;
	}

	// Token: 0x060058F9 RID: 22777 RVA: 0x001ED4DC File Offset: 0x001EB8DC
	private void RemoveEvents(int[] localIDs)
	{
		if (VRC_EventLog.Instance == null)
		{
			return;
		}
		string path = VRC.Network.GetGameObjectPath(base.gameObject);
		VRC_EventLog.Instance.RemoveEventsIf((VRC_EventLog.EventLogEntry evt) => evt.Event.EventType == VRC_EventHandler.VrcEventType.SendRPC && evt.Event.ParameterString == "_InstantiateObject" && localIDs.Any((int id) => evt.rpcParameters.Contains(id)) && (evt.Event.ParameterObject == this.gameObject || evt.ObjectPath == path));
	}

	// Token: 0x060058FA RID: 22778 RVA: 0x001ED538 File Offset: 0x001EB938
	public void ReapObjects()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, ObjectInstantiator.ObjectInfo> keyValuePair in this.spawnedObjects)
		{
			list.Add(keyValuePair.Key);
		}
		foreach (int num in list)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "_DestroyObject", new object[]
			{
				num
			});
		}
	}

	// Token: 0x060058FB RID: 22779 RVA: 0x001ED600 File Offset: 0x001EBA00
	public void ReapOneObject(ObjectInstantiatorHandle obj)
	{
		if (obj == null || obj.LocalID == null)
		{
			return;
		}
		VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "_DestroyObject", new object[]
		{
			obj.LocalID.Value
		});
	}

	// Token: 0x060058FC RID: 22780 RVA: 0x001ED654 File Offset: 0x001EBA54
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.Local,
		VRC_EventHandler.VrcTargetType.AllBuffered
	})]
	private void _InstantiateObject(string prefabName, byte[] data, int localID, VRC.Player instantiator)
	{
		int num = 0;
		float x;
		Protocol.Deserialize(out x, data, ref num);
		float y;
		Protocol.Deserialize(out y, data, ref num);
		float z;
		Protocol.Deserialize(out z, data, ref num);
		Vector3 pos = new Vector3(x, y, z);
		Protocol.Deserialize(out x, data, ref num);
		Protocol.Deserialize(out y, data, ref num);
		Protocol.Deserialize(out z, data, ref num);
		float w;
		Protocol.Deserialize(out w, data, ref num);
		Quaternion rot = new Quaternion(x, y, z, w);
		short num2;
		Protocol.Deserialize(out num2, data, ref num);
		int[] array = new int[(int)num2];
		for (short num3 = 0; num3 < num2; num3 += 1)
		{
			int num4;
			Protocol.Deserialize(out num4, data, ref num);
			array[(int)num3] = num4;
		}
		ObjectInstantiator.PrefabInfo prefabInfo = this._FetchPrefab(prefabName);
		if (prefabInfo == null)
		{
			Debug.LogError("Could not instantiate " + prefabName + ": Could not find prefab", base.gameObject);
			VRC.Network.UnAllocateSubIDs(array);
			return;
		}
		GameObject gameObject = AssetManagement.Instantiate<GameObject>(prefabInfo.obj, pos, rot);
		if (gameObject == null)
		{
			Debug.LogError("Could not instantiate " + prefabName + ": Could not instantiate prefab", base.gameObject);
			VRC.Network.UnAllocateSubIDs(array);
			return;
		}
		gameObject.SetActive(true);
		ObjectInstantiatorHandle orAddComponent = gameObject.GetOrAddComponent<ObjectInstantiatorHandle>();
		orAddComponent.Instantiator = this;
		orAddComponent.LocalID = new int?(localID);
		PhotonView orAddComponent2 = gameObject.GetOrAddComponent<PhotonView>();
		orAddComponent2.synchronization = ViewSynchronization.Off;
		orAddComponent2.ownershipTransfer = OwnershipOption.Takeover;
		if (!VRC.Network.AssignSubIDs(gameObject, array, true))
		{
			Debug.LogError("Could not instantiate " + prefabName + ": Could not assign IDs", base.gameObject);
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		gameObject.name = string.Concat(new object[]
		{
			prefabInfo.name,
			" (Dynamic Clone ",
			localID,
			":",
			orAddComponent2.viewID,
			")"
		});
		Debug.LogFormat("Instantiated a {0} with Local ID {1} and View ID {2}", new object[]
		{
			prefabInfo.name,
			localID,
			orAddComponent2.viewID
		});
		if (!this.spawnedObjects.ContainsKey(localID))
		{
			this.spawnedObjects.Add(localID, new ObjectInstantiator.ObjectInfo
			{
				data = data,
				gameObject = null,
				prefabName = prefabName
			});
		}
		VRC.Network.ProtectFromCleanup(gameObject, true);
		this.spawnedObjects[localID].objectPath = VRC.Network.GetGameObjectPath(gameObject);
		this.spawnedObjects[localID].gameObject = gameObject;
		VRC.Network.SetOwner(instantiator, gameObject, VRC.Network.OwnershipModificationType.Request, true);
	}

	// Token: 0x060058FD RID: 22781 RVA: 0x001ED8DC File Offset: 0x001EBCDC
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void _SendOnSpawn(int localID, VRC.Player instigator)
	{
		ObjectInstantiator.ObjectInfo objectInfo = null;
		if (this.spawnedObjects.TryGetValue(localID, out objectInfo) && objectInfo.gameObject != null)
		{
			if (VRC.Network.GetOwner(objectInfo.gameObject) != instigator)
			{
				return;
			}
			VRC.Network.SendMessageToChildren(objectInfo.gameObject, "OnSpawn", SendMessageOptions.DontRequireReceiver, null);
		}
	}

	// Token: 0x060058FE RID: 22782 RVA: 0x001ED938 File Offset: 0x001EBD38
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{
		VRC_EventHandler.VrcTargetType.All
	})]
	private void _DestroyObject(int localID)
	{
		if (!this.spawnedObjects.ContainsKey(localID))
		{
			return;
		}
		this.RemoveEvents(new int[]
		{
			localID
		});
		ObjectInstantiator.ObjectInfo objectInfo = this.spawnedObjects[localID];
		this.spawnedObjects.Remove(localID);
		if (objectInfo.gameObject != null)
		{
			UnityEngine.Object.Destroy(objectInfo.gameObject);
		}
	}

	// Token: 0x060058FF RID: 22783 RVA: 0x001ED9A0 File Offset: 0x001EBDA0
	private ObjectInstantiator.PrefabInfo _FetchPrefab(string path)
	{
		if (this.prefabCache.ContainsKey(path))
		{
			return this.prefabCache[path];
		}
		GameObject gameObject = VRC_SceneDescriptor.GetPrefab(path);
		if (gameObject == null)
		{
			gameObject = (Resources.Load(path) as GameObject);
		}
		if (gameObject != null)
		{
			return this._AddPrefab(path, gameObject);
		}
		return null;
	}

	// Token: 0x06005900 RID: 22784 RVA: 0x001EDA00 File Offset: 0x001EBE00
	private ObjectInstantiator.PrefabInfo _AddPrefab(string path, GameObject prefab)
	{
		GameObject gameObject = AssetManagement.Instantiate<GameObject>(prefab);
		if (gameObject == null)
		{
			Debug.LogError("Could not instantiate prefab " + path, base.gameObject);
			return null;
		}
		int viewCount = VRC.Network.CountSubIDs(gameObject, true);
		int idCount = (from c in VRC.Network.GetAllComponents<Component>(gameObject, true)
		where c is INetworkID
		select c).Count<Component>();
		UnityEngine.Object.Destroy(gameObject);
		prefab.SetActive(false);
		ObjectInstantiator.PrefabInfo prefabInfo = new ObjectInstantiator.PrefabInfo
		{
			obj = prefab,
			name = path,
			viewCount = viewCount,
			idCount = idCount
		};
		this.prefabCache.Add(path, prefabInfo);
		return prefabInfo;
	}

	// Token: 0x06005901 RID: 22785 RVA: 0x001EDAB4 File Offset: 0x001EBEB4
	private int? MakeID()
	{
		int num = Time.renderedFrameCount << 8;
		this.lastUsedSpawnID++;
		while (this.lastUsedSpawnID < 255 && this.spawnedObjects.ContainsKey(this.lastUsedSpawnID | num))
		{
			this.lastUsedSpawnID++;
		}
		if (this.lastUsedSpawnID >= 255)
		{
			Debug.LogError("No more available spawn ids for this instantiator, this frame.");
			return null;
		}
		return new int?(this.lastUsedSpawnID | num);
	}

	// Token: 0x06005902 RID: 22786 RVA: 0x001EDB44 File Offset: 0x001EBF44
	public override void Awake()
	{
		base.Awake();
		this.prefabCache.Clear();
		VRC_ObjectSpawn component = base.GetComponent<VRC_ObjectSpawn>();
		if (component != null && component.ObjectPrefab != null)
		{
			this._AddPrefab("VRC_ObjectSpawnPrefab", component.ObjectPrefab);
		}
		component = base.GetComponent<VRC_ObjectSpawn>();
		if (component != null)
		{
			component.Instantiate = delegate(Vector3 position, Quaternion rotation)
			{
				this.InstantiateObject(VRC_EventHandler.VrcBroadcastType.Master, "VRC_ObjectSpawnPrefab", position, rotation);
			};
			component.ReapObjects = delegate
			{
				this.ReapObjects();
			};
		}
	}

	// Token: 0x06005903 RID: 22787 RVA: 0x001EDBD0 File Offset: 0x001EBFD0
	private void OnDestroy()
	{
		this.ReapObjects();
		string path = VRC.Network.GetGameObjectPath(base.gameObject);
		if (VRC_EventLog.Instance != null)
		{
			VRC_EventLog.Instance.RemoveEventsIf((VRC_EventLog.EventLogEntry evt) => evt.Event.ParameterObject == this.gameObject || evt.ObjectPath == path);
		}
	}

	// Token: 0x04003FA3 RID: 16291
	private Dictionary<string, ObjectInstantiator.PrefabInfo> prefabCache = new Dictionary<string, ObjectInstantiator.PrefabInfo>();

	// Token: 0x04003FA4 RID: 16292
	private Dictionary<int, ObjectInstantiator.ObjectInfo> spawnedObjects = new Dictionary<int, ObjectInstantiator.ObjectInfo>();

	// Token: 0x04003FA5 RID: 16293
	private const int maxSpawnCountPerFrame = 255;

	// Token: 0x04003FA6 RID: 16294
	private int lastUsedSpawnID;

	// Token: 0x02000B5A RID: 2906
	private class PrefabInfo
	{
		// Token: 0x04003FA8 RID: 16296
		public string name;

		// Token: 0x04003FA9 RID: 16297
		public GameObject obj;

		// Token: 0x04003FAA RID: 16298
		public int viewCount;

		// Token: 0x04003FAB RID: 16299
		public int idCount;
	}

	// Token: 0x02000B5B RID: 2907
	private class ObjectInfo
	{
		// Token: 0x04003FAC RID: 16300
		public string prefabName;

		// Token: 0x04003FAD RID: 16301
		public byte[] data;

		// Token: 0x04003FAE RID: 16302
		public GameObject gameObject;

		// Token: 0x04003FAF RID: 16303
		public string objectPath;

		// Token: 0x04003FB0 RID: 16304
		public int localId;
	}
}
