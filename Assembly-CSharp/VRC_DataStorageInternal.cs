using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using RAIN.Core;
using RAIN.Memory;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000B6A RID: 2922
public class VRC_DataStorageInternal : VRCPunBehaviour
{
	// Token: 0x060059EF RID: 23023 RVA: 0x001F3C1C File Offset: 0x001F201C
	public void Initialize(VRC_DataStorage data)
	{
		this.dataStorage = data;
		this._animator = base.GetComponentInChildren<Animator>();
		AIRig componentInChildren = base.GetComponentInChildren<AIRig>();
		if (componentInChildren != null)
		{
			this._rainMemory = componentInChildren.AI.WorkingMemory;
		}
		if (VRC_DataStorageInternal.f__mg0 == null)
		{
			VRC_DataStorageInternal.f__mg0 = new Func<VRC_DataStorage, string, int>(VRC_DataStorageInternal.GetElementIndex);
		}
		VRC_DataStorage._GetElementIndex = VRC_DataStorageInternal.f__mg0;
	}

	// Token: 0x060059F0 RID: 23024 RVA: 0x001F3C82 File Offset: 0x001F2082
	public override void Awake()
	{
		base.Awake();
		this.BlockReady();
	}

	// Token: 0x060059F1 RID: 23025 RVA: 0x001F3C90 File Offset: 0x001F2090
	public override IEnumerator Start()
	{
        yield return base.Start();
		if (base.photonView != null)
		{
			base.ObserveThis();
			base.photonView.synchronization = ((!(base.GetComponent<Rigidbody>() == null) && !base.GetComponent<Rigidbody>().isKinematic && !(base.GetComponent<SyncPhysics>() == null) && base.GetComponent<SyncPhysics>().enabled) ? ViewSynchronization.Unreliable : ViewSynchronization.ReliableDeltaCompressed);
		}
		yield break;
	}

	// Token: 0x060059F2 RID: 23026 RVA: 0x001F3CAB File Offset: 0x001F20AB
	protected override void OnNetworkReady()
	{
		base.OnNetworkReady();
		this.dataHash = null;
	}

	// Token: 0x060059F3 RID: 23027 RVA: 0x001F3CBC File Offset: 0x001F20BC
	private void LateUpdate()
	{
		if (this.dataStorage == null || this.dataStorage.data == null)
		{
			return;
		}
		if (!base.gameObject.IsReady())
		{
			if (!base.isMine && !VRC.Network.IsSadAndAlone)
			{
				return;
			}
			this.UnblockReady();
		}
		this.CheckConsistency();
		for (int i = 0; i < this.dataStorage.data.Length; i++)
		{
			if (this.dataStorage.data[i] != null)
			{
				if (this.dataStorage.data[i].added)
				{
					try
					{
						this.dataStorage.OnDataElementAdded(i);
						this.dataStorage.data[i].added = false;
					}
					catch (Exception exception)
					{
						Debug.LogException(exception, base.gameObject);
						this.dataHash = null;
					}
				}
				if (this.dataStorage.data[i].modified)
				{
					try
					{
						this.dataStorage.OnDataElementChanged(i);
						this.dataStorage.data[i].modified = false;
					}
					catch (Exception exception2)
					{
						Debug.LogException(exception2, base.gameObject);
						this.dataHash = null;
					}
				}
			}
		}
	}

	// Token: 0x060059F4 RID: 23028 RVA: 0x001F3E1C File Offset: 0x001F221C
	public void CheckConsistency()
	{
		if (this.dataStorage.data.Length == 0 || this.timeOfNextConsistencyCheck - Time.time > 0f)
		{
			return;
		}
		this.timeOfNextConsistencyCheck = Time.time + UnityEngine.Random.Range(1f, 2f);
		if (this.dataHash != null && this.dataHash.Length != this.dataStorage.data.Length)
		{
			this.dataHash = null;
		}
		if (this.dataHash == null)
		{
			this.dataHash = new SHA1[this.dataStorage.data.Length];
		}
		for (int i = 0; i < this.dataHash.Length; i++)
		{
			if (this.dataStorage.data[i].modified)
			{
				this.dataHash[i] = null;
			}
			SHA1 sha = SHA1.Create();
			sha.ComputeHash(VRC_Serialization.ParameterEncoder(new object[]
			{
				this.dataStorage.data[i]
			}));
			bool flag = this.dataHash[i] == null || sha.HashSize != this.dataHash[i].HashSize || sha.Hash.Length != this.dataHash[i].Hash.Length;
			if (!flag)
			{
				for (int j = 0; j < sha.Hash.Length; j++)
				{
					flag = (sha.Hash[j] != this.dataHash[i].Hash[j]);
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				this.dataStorage.data[i].modified = flag;
				this.dataHash[i] = sha;
				base.photonView.lastOnSerializeDataSent = null;
			}
		}
	}

	// Token: 0x060059F5 RID: 23029 RVA: 0x001F3FD7 File Offset: 0x001F23D7
	public int GetElementCount()
	{
		return (!(this.dataStorage == null)) ? this.dataStorage.data.Length : 0;
	}

	// Token: 0x060059F6 RID: 23030 RVA: 0x001F4000 File Offset: 0x001F2400
	public bool Resize(int count)
	{
		if (this.dataStorage == null || count == this.GetElementCount())
		{
			return false;
		}
		int i;
		for (i = count; i < this.dataStorage.data.Length; i++)
		{
			try
			{
				this.dataStorage.OnDataElementRemoved(i);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, base.gameObject);
			}
		}
		VRC_DataStorage.VrcDataElement[] data = this.dataStorage.data;
		this.dataStorage.data = new VRC_DataStorage.VrcDataElement[count];
		i = 0;
		while (i < data.Length && i < count)
		{
			this.dataStorage.data[i] = data[i];
			i++;
		}
		while (i < count)
		{
			this.dataStorage.data[i] = new VRC_DataStorage.VrcDataElement();
			i++;
		}
		SHA1[] array = new SHA1[count];
		i = 0;
		while (i < this.dataHash.Length && i < count)
		{
			array[i] = this.dataHash[i];
			i++;
		}
		while (i < count)
		{
			array[i] = null;
			i++;
		}
		this.dataHash = array;
		return true;
	}

	// Token: 0x060059F7 RID: 23031 RVA: 0x001F4138 File Offset: 0x001F2538
	public VRC_DataStorage.VrcDataElement GetElement(int index)
	{
		return this.dataStorage.data[index];
	}

	// Token: 0x060059F8 RID: 23032 RVA: 0x001F4148 File Offset: 0x001F2548
	public static int GetElementIndex(VRC_DataStorage ds, string name)
	{
		VRC_DataStorageInternal component = ds.gameObject.GetComponent<VRC_DataStorageInternal>();
		return component.GetElementIndex(name);
	}

	// Token: 0x060059F9 RID: 23033 RVA: 0x001F4168 File Offset: 0x001F2568
	public int GetElementIndex(string name)
	{
		if (this.dataLookup == null)
		{
			bool flag = false;
			this.dataLookup = new Dictionary<string, int>();
			for (int i = 0; i < this.dataStorage.data.Length; i++)
			{
				this.dataLookup.Add(this.dataStorage.data[i].name, i);
				flag = true;
			}
			if (flag && this.GetElementCount() > 255)
			{
				Debug.LogError("More than " + byte.MaxValue.ToString() + " objects in Data Storage for " + base.gameObject.name);
			}
		}
		return this.dataLookup[name];
	}

	// Token: 0x060059FA RID: 23034 RVA: 0x001F4220 File Offset: 0x001F2620
	private void VerifyType<T>(VRC_DataStorage.VrcDataElement data)
	{
		if (typeof(T) == typeof(float))
		{
			if (data.type != VRC_DataStorage.VrcDataType.Float)
			{
				Debug.LogError("VRCSDK2.VRC_DataStorage: " + data.name + " is not a float");
			}
		}
		else if (typeof(T) == typeof(string) && data.type != VRC_DataStorage.VrcDataType.String)
		{
			Debug.LogError("VRCSDK2.VRC_DataStorage: " + data.name + " is not a String");
		}
	}

	// Token: 0x060059FB RID: 23035 RVA: 0x001F42B0 File Offset: 0x001F26B0
	public T Get<T>(string name)
	{
		int elementIndex = this.GetElementIndex(name);
		return this.Get<T>(elementIndex);
	}

	// Token: 0x060059FC RID: 23036 RVA: 0x001F42CC File Offset: 0x001F26CC
	public T Get<T>(int index)
	{
		VRC_DataStorage.VrcDataElement vrcDataElement = this.dataStorage.data[index];
		this.VerifyType<T>(vrcDataElement);
		if (typeof(T) == typeof(bool))
		{
			switch (vrcDataElement.mirror)
			{
			case VRC_DataStorage.VrcDataMirror.None:
				break;
			case VRC_DataStorage.VrcDataMirror.Rain:
				vrcDataElement.valueBool = (bool)this._rainMemory.GetItem(vrcDataElement.name);
				break;
			case VRC_DataStorage.VrcDataMirror.Animator:
				vrcDataElement.valueBool = this._animator.GetBool(vrcDataElement.name);
				break;
			default:
				Debug.LogError("Add Data Mirroring.");
				break;
			}
			return (T)((object)vrcDataElement.valueBool);
		}
		if (typeof(T) == typeof(int))
		{
			switch (vrcDataElement.mirror)
			{
			case VRC_DataStorage.VrcDataMirror.None:
				break;
			case VRC_DataStorage.VrcDataMirror.Rain:
				vrcDataElement.valueInt = (int)this._rainMemory.GetItem(vrcDataElement.name);
				break;
			case VRC_DataStorage.VrcDataMirror.Animator:
				vrcDataElement.valueInt = this._animator.GetInteger(vrcDataElement.name);
				break;
			default:
				Debug.LogError("Add Data Mirroring.");
				break;
			}
			return (T)((object)vrcDataElement.valueInt);
		}
		if (typeof(T) == typeof(float))
		{
			switch (vrcDataElement.mirror)
			{
			case VRC_DataStorage.VrcDataMirror.None:
				break;
			case VRC_DataStorage.VrcDataMirror.Rain:
				vrcDataElement.valueFloat = (float)this._rainMemory.GetItem(vrcDataElement.name);
				break;
			case VRC_DataStorage.VrcDataMirror.Animator:
				vrcDataElement.valueFloat = this._animator.GetFloat(vrcDataElement.name);
				break;
			default:
				Debug.LogError("Add Data Mirroring.");
				break;
			}
			return (T)((object)vrcDataElement.valueFloat);
		}
		if (typeof(T) == typeof(string))
		{
			switch (vrcDataElement.mirror)
			{
			case VRC_DataStorage.VrcDataMirror.None:
				break;
			case VRC_DataStorage.VrcDataMirror.Rain:
				vrcDataElement.valueString = (string)this._rainMemory.GetItem(vrcDataElement.name);
				break;
			case VRC_DataStorage.VrcDataMirror.Animator:
				Debug.LogError("Cant get animator string");
				break;
			default:
				Debug.LogError("Add Data Mirroring.");
				break;
			}
			return (T)((object)vrcDataElement.valueString);
		}
		if (typeof(T) == typeof(Array) || typeof(T) == typeof(byte[]))
		{
			VRC_DataStorage.VrcDataMirror mirror = vrcDataElement.mirror;
			if (mirror != VRC_DataStorage.VrcDataMirror.None)
			{
				if (mirror != VRC_DataStorage.VrcDataMirror.SerializeComponent)
				{
					Debug.LogError("Add Data Mirroring.");
				}
				else
				{
					byte[] bytes = vrcDataElement.serializeComponent.GetBytes();
					vrcDataElement.valueSerializedBytes = bytes;
				}
			}
			return (T)((object)vrcDataElement.valueSerializedBytes);
		}
		T result;
		vrcDataElement.Deserialize<T>(out result);
		return result;
	}

	// Token: 0x060059FD RID: 23037 RVA: 0x001F45D8 File Offset: 0x001F29D8
	public void Set<T>(string name, T value)
	{
		int elementIndex = this.GetElementIndex(name);
		this.Set<T>(elementIndex, value);
	}

	// Token: 0x060059FE RID: 23038 RVA: 0x001F45F8 File Offset: 0x001F29F8
	public void Set<T>(int index, T value)
	{
		VRC_DataStorage.VrcDataElement vrcDataElement = this.dataStorage.data[index];
		this.VerifyType<T>(vrcDataElement);
		if (this.dataHash != null && this.dataHash.Length > index)
		{
			this.dataHash[index] = null;
		}
		vrcDataElement.added = (vrcDataElement.type == VRC_DataStorage.VrcDataType.None);
		if (typeof(T) == typeof(bool))
		{
			vrcDataElement.valueBool = (bool)((object)value);
			switch (vrcDataElement.mirror)
			{
			case VRC_DataStorage.VrcDataMirror.None:
				break;
			case VRC_DataStorage.VrcDataMirror.Rain:
				this._rainMemory.SetItem<T>(vrcDataElement.name, value);
				break;
			case VRC_DataStorage.VrcDataMirror.Animator:
				if ((bool)((object)value))
				{
					Debug.Log("True");
				}
				this._animator.SetBool(vrcDataElement.name, (bool)((object)value));
				break;
			default:
				Debug.LogError("Add Data Mirroring.");
				break;
			}
		}
		else if (typeof(T) == typeof(int))
		{
			vrcDataElement.valueInt = (int)((object)value);
			switch (vrcDataElement.mirror)
			{
			case VRC_DataStorage.VrcDataMirror.None:
				break;
			case VRC_DataStorage.VrcDataMirror.Rain:
				this._rainMemory.SetItem<T>(vrcDataElement.name, value);
				break;
			case VRC_DataStorage.VrcDataMirror.Animator:
				this._animator.SetInteger(vrcDataElement.name, (int)((object)value));
				break;
			default:
				Debug.LogError("Add Data Mirroring.");
				break;
			}
		}
		else if (typeof(T) == typeof(float))
		{
			vrcDataElement.valueFloat = (float)((object)value);
			switch (vrcDataElement.mirror)
			{
			case VRC_DataStorage.VrcDataMirror.None:
				break;
			case VRC_DataStorage.VrcDataMirror.Rain:
				this._rainMemory.SetItem<T>(vrcDataElement.name, value);
				break;
			case VRC_DataStorage.VrcDataMirror.Animator:
				this._animator.SetFloat(vrcDataElement.name, (float)((object)value));
				break;
			default:
				Debug.LogError("Add Data Mirroring.");
				break;
			}
		}
		else if (typeof(T) == typeof(string))
		{
			vrcDataElement.valueString = (string)((object)value);
			switch (vrcDataElement.mirror)
			{
			case VRC_DataStorage.VrcDataMirror.None:
				break;
			case VRC_DataStorage.VrcDataMirror.Rain:
				this._rainMemory.SetItem<T>(vrcDataElement.name, value);
				break;
			case VRC_DataStorage.VrcDataMirror.Animator:
				Debug.LogError("Cant set animator string");
				break;
			default:
				Debug.LogError("Add Data Mirroring.");
				break;
			}
		}
		else if (typeof(T) == typeof(Array) || typeof(T) == typeof(byte[]))
		{
			vrcDataElement.valueSerializedBytes = (byte[])((object)value);
			VRC_DataStorage.VrcDataMirror mirror = vrcDataElement.mirror;
			if (mirror != VRC_DataStorage.VrcDataMirror.None)
			{
				if (mirror != VRC_DataStorage.VrcDataMirror.SerializeComponent)
				{
					Debug.LogError("Add Data Mirroring.");
				}
				else
				{
					vrcDataElement.serializeComponent.SetBytes(vrcDataElement.valueSerializedBytes);
				}
			}
		}
		else
		{
			vrcDataElement.Serialize<T>(value);
		}
	}

	// Token: 0x060059FF RID: 23039 RVA: 0x001F4950 File Offset: 0x001F2D50
	public static bool Serialize(VRC_DataStorage.VrcDataElement ds, object objectToStore)
	{
		ds.added = (ds.type == VRC_DataStorage.VrcDataType.None);
		ds.modified = true;
		if (objectToStore is bool)
		{
			ds.valueBool = (bool)objectToStore;
			ds.type = VRC_DataStorage.VrcDataType.Bool;
			return true;
		}
		if (objectToStore is int)
		{
			ds.valueInt = (int)objectToStore;
			ds.type = VRC_DataStorage.VrcDataType.Int;
			return true;
		}
		if (objectToStore is float)
		{
			ds.valueFloat = (float)objectToStore;
			ds.type = VRC_DataStorage.VrcDataType.Float;
			return true;
		}
		if (objectToStore is string)
		{
			ds.valueString = (string)objectToStore;
			ds.type = VRC_DataStorage.VrcDataType.String;
			return true;
		}
		if (objectToStore is byte[])
		{
			ds.valueSerializedBytes = (byte[])objectToStore;
			ds.type = VRC_DataStorage.VrcDataType.SerializeBytes;
			return true;
		}
		if (objectToStore is VRC_SerializableBehaviour)
		{
			ds.serializeComponent = (VRC_SerializableBehaviour)objectToStore;
			ds.valueSerializedBytes = ((VRC_SerializableBehaviour)objectToStore).GetBytes();
			ds.type = VRC_DataStorage.VrcDataType.SerializeObject;
			return true;
		}
		ds.valueSerializedBytes = VRC_Serialization.ParameterEncoder(new object[]
		{
			objectToStore
		});
		ds.type = VRC_DataStorage.VrcDataType.Other;
		return true;
	}

	// Token: 0x06005A00 RID: 23040 RVA: 0x001F4A64 File Offset: 0x001F2E64
	public static bool Deserialize(VRC_DataStorage.VrcDataElement ds, out object obj)
	{
		obj = null;
		switch (ds.type)
		{
		case VRC_DataStorage.VrcDataType.None:
			obj = null;
			return true;
		case VRC_DataStorage.VrcDataType.Bool:
			obj = ds.valueBool;
			return true;
		case VRC_DataStorage.VrcDataType.Int:
			obj = ds.valueInt;
			return true;
		case VRC_DataStorage.VrcDataType.Float:
			obj = ds.valueFloat;
			return true;
		case VRC_DataStorage.VrcDataType.String:
			obj = ds.valueString;
			return true;
		case VRC_DataStorage.VrcDataType.SerializeBytes:
			obj = ds.valueSerializedBytes;
			return true;
		case VRC_DataStorage.VrcDataType.SerializeObject:
			if (ds.serializeComponent == null)
			{
				return false;
			}
			ds.serializeComponent.SetBytes(ds.valueSerializedBytes);
			return true;
		case VRC_DataStorage.VrcDataType.Other:
		{
			object[] array = VRC_Serialization.ParameterDecoder(ds.valueSerializedBytes, false);
			obj = ((array != null && array.Length != 0) ? array[0] : null);
			return true;
		}
		default:
			Debug.LogError("Unable to handle " + ds.type.ToString());
			return false;
		}
	}

	// Token: 0x06005A01 RID: 23041 RVA: 0x001F4B5B File Offset: 0x001F2F5B
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			VRC_DataStorageInternal.SerializeDataStorage(this, stream);
		}
		else
		{
			VRC_DataStorageInternal.DeserializeDataStorage(this, stream);
		}
		this.UnblockReady();
	}

	// Token: 0x06005A02 RID: 23042 RVA: 0x001F4B84 File Offset: 0x001F2F84
	private static void SerializeDataStorage(VRC_DataStorageInternal dataStorage, PhotonStream stream)
	{
		if (dataStorage == null)
		{
			stream.SendNext(0);
		}
		else
		{
			stream.SendNext(dataStorage.GetElementCount());
			for (int i = 0; i < dataStorage.GetElementCount(); i++)
			{
				VRC_DataStorage.VrcDataElement element = dataStorage.GetElement(i);
				stream.SendNext(element.type);
				stream.SendNext(element.name);
				stream.SendNext(element.mirror);
				switch (element.type)
				{
				case VRC_DataStorage.VrcDataType.Bool:
					stream.SendNext(dataStorage.Get<bool>(i));
					break;
				case VRC_DataStorage.VrcDataType.Int:
					stream.SendNext(dataStorage.Get<int>(i));
					break;
				case VRC_DataStorage.VrcDataType.Float:
					stream.SendNext(dataStorage.Get<float>(i));
					break;
				case VRC_DataStorage.VrcDataType.String:
					stream.SendNext(dataStorage.Get<string>(i));
					break;
				case VRC_DataStorage.VrcDataType.SerializeBytes:
				case VRC_DataStorage.VrcDataType.SerializeObject:
				case VRC_DataStorage.VrcDataType.Other:
				{
					byte[] obj = dataStorage.Get<byte[]>(i);
					stream.SendNext(obj);
					break;
				}
				}
			}
		}
	}

	// Token: 0x06005A03 RID: 23043 RVA: 0x001F4CA8 File Offset: 0x001F30A8
	private static void DeserializeDataStorage(VRC_DataStorageInternal dataStorage, PhotonStream stream)
	{
		int count = (int)stream.ReceiveNext();
		if (dataStorage != null)
		{
			dataStorage.Resize(count);
		}
		for (int i = 0; i < dataStorage.GetElementCount(); i++)
		{
			VRC_DataStorage.VrcDataElement element = dataStorage.GetElement(i);
			element.type = (VRC_DataStorage.VrcDataType)stream.ReceiveNext();
			element.name = (string)stream.ReceiveNext();
			element.mirror = (VRC_DataStorage.VrcDataMirror)stream.ReceiveNext();
			switch (element.type)
			{
			case VRC_DataStorage.VrcDataType.Bool:
				dataStorage.Set<bool>(i, (bool)stream.ReceiveNext());
				break;
			case VRC_DataStorage.VrcDataType.Int:
				dataStorage.Set<int>(i, (int)stream.ReceiveNext());
				break;
			case VRC_DataStorage.VrcDataType.Float:
				dataStorage.Set<float>(i, (float)stream.ReceiveNext());
				break;
			case VRC_DataStorage.VrcDataType.String:
				dataStorage.Set<string>(i, (string)stream.ReceiveNext());
				break;
			case VRC_DataStorage.VrcDataType.SerializeBytes:
			case VRC_DataStorage.VrcDataType.SerializeObject:
			case VRC_DataStorage.VrcDataType.Other:
			{
				byte[] value = (byte[])stream.ReceiveNext();
				dataStorage.Set<byte[]>(i, value);
				break;
			}
			}
		}
	}

	// Token: 0x0400401E RID: 16414
	private SHA1[] dataHash;

	// Token: 0x0400401F RID: 16415
	private VRC_DataStorage dataStorage;

	// Token: 0x04004020 RID: 16416
	private Dictionary<string, int> dataLookup;

	// Token: 0x04004021 RID: 16417
	private Animator _animator;

	// Token: 0x04004022 RID: 16418
	private RAINMemory _rainMemory;

	// Token: 0x04004023 RID: 16419
	private float timeOfNextConsistencyCheck;

	// Token: 0x04004024 RID: 16420
	[CompilerGenerated]
	private static Func<VRC_DataStorage, string, int> f__mg0;
}
