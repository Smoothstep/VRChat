using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000772 RID: 1906
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]
[AddComponentMenu("Photon Networking/Photon Animator View")]
public class PhotonAnimatorView : MonoBehaviour, IPunObservable
{
	// Token: 0x06003E56 RID: 15958 RVA: 0x00139C36 File Offset: 0x00138036
	private void Awake()
	{
		this.m_PhotonView = base.GetComponent<PhotonView>();
		this.m_StreamQueue = new PhotonStreamQueue(120);
		this.m_Animator = base.GetComponent<Animator>();
	}

	// Token: 0x06003E57 RID: 15959 RVA: 0x00139C60 File Offset: 0x00138060
	private void Update()
	{
		if (this.m_Animator.applyRootMotion && !this.m_PhotonView.isMine && PhotonNetwork.connected)
		{
			this.m_Animator.applyRootMotion = false;
		}
		if (!PhotonNetwork.inRoom || PhotonNetwork.room.PlayerCount <= 1)
		{
			this.m_StreamQueue.Reset();
			return;
		}
		if (this.m_PhotonView.isMine)
		{
			this.SerializeDataContinuously();
			this.CacheDiscreteTriggers();
		}
		else
		{
			this.DeserializeDataContinuously();
		}
	}

	// Token: 0x06003E58 RID: 15960 RVA: 0x00139CF0 File Offset: 0x001380F0
	public void CacheDiscreteTriggers()
	{
		for (int i = 0; i < this.m_SynchronizeParameters.Count; i++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[i];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete && synchronizedParameter.Type == PhotonAnimatorView.ParameterType.Trigger && this.m_Animator.GetBool(synchronizedParameter.Name) && synchronizedParameter.Type == PhotonAnimatorView.ParameterType.Trigger)
			{
				this.m_raisedDiscreteTriggersCache.Add(synchronizedParameter.Name);
				break;
			}
		}
	}

	// Token: 0x06003E59 RID: 15961 RVA: 0x00139D78 File Offset: 0x00138178
	public bool DoesLayerSynchronizeTypeExist(int layerIndex)
	{
		return this.m_SynchronizeLayers.FindIndex((PhotonAnimatorView.SynchronizedLayer item) => item.LayerIndex == layerIndex) != -1;
	}

	// Token: 0x06003E5A RID: 15962 RVA: 0x00139DB0 File Offset: 0x001381B0
	public bool DoesParameterSynchronizeTypeExist(string name)
	{
		return this.m_SynchronizeParameters.FindIndex((PhotonAnimatorView.SynchronizedParameter item) => item.Name == name) != -1;
	}

	// Token: 0x06003E5B RID: 15963 RVA: 0x00139DE7 File Offset: 0x001381E7
	public List<PhotonAnimatorView.SynchronizedLayer> GetSynchronizedLayers()
	{
		return this.m_SynchronizeLayers;
	}

	// Token: 0x06003E5C RID: 15964 RVA: 0x00139DEF File Offset: 0x001381EF
	public List<PhotonAnimatorView.SynchronizedParameter> GetSynchronizedParameters()
	{
		return this.m_SynchronizeParameters;
	}

	// Token: 0x06003E5D RID: 15965 RVA: 0x00139DF8 File Offset: 0x001381F8
	public PhotonAnimatorView.SynchronizeType GetLayerSynchronizeType(int layerIndex)
	{
		int num = this.m_SynchronizeLayers.FindIndex((PhotonAnimatorView.SynchronizedLayer item) => item.LayerIndex == layerIndex);
		if (num == -1)
		{
			return PhotonAnimatorView.SynchronizeType.Disabled;
		}
		return this.m_SynchronizeLayers[num].SynchronizeType;
	}

	// Token: 0x06003E5E RID: 15966 RVA: 0x00139E44 File Offset: 0x00138244
	public PhotonAnimatorView.SynchronizeType GetParameterSynchronizeType(string name)
	{
		int num = this.m_SynchronizeParameters.FindIndex((PhotonAnimatorView.SynchronizedParameter item) => item.Name == name);
		if (num == -1)
		{
			return PhotonAnimatorView.SynchronizeType.Disabled;
		}
		return this.m_SynchronizeParameters[num].SynchronizeType;
	}

	// Token: 0x06003E5F RID: 15967 RVA: 0x00139E90 File Offset: 0x00138290
	public void SetLayerSynchronized(int layerIndex, PhotonAnimatorView.SynchronizeType synchronizeType)
	{
		if (Application.isPlaying)
		{
			this.m_WasSynchronizeTypeChanged = true;
		}
		int num = this.m_SynchronizeLayers.FindIndex((PhotonAnimatorView.SynchronizedLayer item) => item.LayerIndex == layerIndex);
		if (num == -1)
		{
			this.m_SynchronizeLayers.Add(new PhotonAnimatorView.SynchronizedLayer
			{
				LayerIndex = layerIndex,
				SynchronizeType = synchronizeType
			});
		}
		else
		{
			this.m_SynchronizeLayers[num].SynchronizeType = synchronizeType;
		}
	}

	// Token: 0x06003E60 RID: 15968 RVA: 0x00139F18 File Offset: 0x00138318
	public void SetParameterSynchronized(string name, PhotonAnimatorView.ParameterType type, PhotonAnimatorView.SynchronizeType synchronizeType)
	{
		if (Application.isPlaying)
		{
			this.m_WasSynchronizeTypeChanged = true;
		}
		int num = this.m_SynchronizeParameters.FindIndex((PhotonAnimatorView.SynchronizedParameter item) => item.Name == name);
		if (num == -1)
		{
			this.m_SynchronizeParameters.Add(new PhotonAnimatorView.SynchronizedParameter
			{
				Name = name,
				Type = type,
				SynchronizeType = synchronizeType
			});
		}
		else
		{
			this.m_SynchronizeParameters[num].SynchronizeType = synchronizeType;
		}
	}

	// Token: 0x06003E61 RID: 15969 RVA: 0x00139FA8 File Offset: 0x001383A8
	private void SerializeDataContinuously()
	{
		if (this.m_Animator == null)
		{
			return;
		}
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				this.m_StreamQueue.SendNext(this.m_Animator.GetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex));
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[j];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				PhotonAnimatorView.ParameterType type = synchronizedParameter.Type;
				switch (type)
				{
				case PhotonAnimatorView.ParameterType.Float:
					this.m_StreamQueue.SendNext(this.m_Animator.GetFloat(synchronizedParameter.Name));
					break;
				default:
					if (type == PhotonAnimatorView.ParameterType.Trigger)
					{
						this.m_StreamQueue.SendNext(this.m_Animator.GetBool(synchronizedParameter.Name));
					}
					break;
				case PhotonAnimatorView.ParameterType.Int:
					this.m_StreamQueue.SendNext(this.m_Animator.GetInteger(synchronizedParameter.Name));
					break;
				case PhotonAnimatorView.ParameterType.Bool:
					this.m_StreamQueue.SendNext(this.m_Animator.GetBool(synchronizedParameter.Name));
					break;
				}
			}
		}
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x0013A120 File Offset: 0x00138520
	private void DeserializeDataContinuously()
	{
		if (!this.m_StreamQueue.HasQueuedObjects())
		{
			return;
		}
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				this.m_Animator.SetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex, (float)this.m_StreamQueue.ReceiveNext());
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[j];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				PhotonAnimatorView.ParameterType type = synchronizedParameter.Type;
				switch (type)
				{
				case PhotonAnimatorView.ParameterType.Float:
					this.m_Animator.SetFloat(synchronizedParameter.Name, (float)this.m_StreamQueue.ReceiveNext());
					break;
				default:
					if (type == PhotonAnimatorView.ParameterType.Trigger)
					{
						this.m_Animator.SetBool(synchronizedParameter.Name, (bool)this.m_StreamQueue.ReceiveNext());
					}
					break;
				case PhotonAnimatorView.ParameterType.Int:
					this.m_Animator.SetInteger(synchronizedParameter.Name, (int)this.m_StreamQueue.ReceiveNext());
					break;
				case PhotonAnimatorView.ParameterType.Bool:
					this.m_Animator.SetBool(synchronizedParameter.Name, (bool)this.m_StreamQueue.ReceiveNext());
					break;
				}
			}
		}
	}

	// Token: 0x06003E63 RID: 15971 RVA: 0x0013A298 File Offset: 0x00138698
	private void SerializeDataDiscretly(PhotonStream stream)
	{
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				stream.SendNext(this.m_Animator.GetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex));
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[j];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				PhotonAnimatorView.ParameterType type = synchronizedParameter.Type;
				switch (type)
				{
				case PhotonAnimatorView.ParameterType.Float:
					stream.SendNext(this.m_Animator.GetFloat(synchronizedParameter.Name));
					break;
				default:
					if (type == PhotonAnimatorView.ParameterType.Trigger)
					{
						stream.SendNext(this.m_raisedDiscreteTriggersCache.Contains(synchronizedParameter.Name));
					}
					break;
				case PhotonAnimatorView.ParameterType.Int:
					stream.SendNext(this.m_Animator.GetInteger(synchronizedParameter.Name));
					break;
				case PhotonAnimatorView.ParameterType.Bool:
					stream.SendNext(this.m_Animator.GetBool(synchronizedParameter.Name));
					break;
				}
			}
		}
		this.m_raisedDiscreteTriggersCache.Clear();
	}

	// Token: 0x06003E64 RID: 15972 RVA: 0x0013A3F0 File Offset: 0x001387F0
	private void DeserializeDataDiscretly(PhotonStream stream)
	{
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				this.m_Animator.SetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex, (float)stream.ReceiveNext());
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = this.m_SynchronizeParameters[j];
			if (synchronizedParameter.SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				PhotonAnimatorView.ParameterType type = synchronizedParameter.Type;
				switch (type)
				{
				case PhotonAnimatorView.ParameterType.Float:
					if (!(stream.PeekNext() is float))
					{
						return;
					}
					this.m_Animator.SetFloat(synchronizedParameter.Name, (float)stream.ReceiveNext());
					break;
				default:
					if (type == PhotonAnimatorView.ParameterType.Trigger)
					{
						if (!(stream.PeekNext() is bool))
						{
							return;
						}
						if ((bool)stream.ReceiveNext())
						{
							this.m_Animator.SetTrigger(synchronizedParameter.Name);
						}
					}
					break;
				case PhotonAnimatorView.ParameterType.Int:
					if (!(stream.PeekNext() is int))
					{
						return;
					}
					this.m_Animator.SetInteger(synchronizedParameter.Name, (int)stream.ReceiveNext());
					break;
				case PhotonAnimatorView.ParameterType.Bool:
					if (!(stream.PeekNext() is bool))
					{
						return;
					}
					this.m_Animator.SetBool(synchronizedParameter.Name, (bool)stream.ReceiveNext());
					break;
				}
			}
		}
	}

	// Token: 0x06003E65 RID: 15973 RVA: 0x0013A588 File Offset: 0x00138988
	private void SerializeSynchronizationTypeState(PhotonStream stream)
	{
		byte[] array = new byte[this.m_SynchronizeLayers.Count + this.m_SynchronizeParameters.Count];
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			array[i] = (byte)this.m_SynchronizeLayers[i].SynchronizeType;
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			array[this.m_SynchronizeLayers.Count + j] = (byte)this.m_SynchronizeParameters[j].SynchronizeType;
		}
		stream.SendNext(array);
	}

	// Token: 0x06003E66 RID: 15974 RVA: 0x0013A628 File Offset: 0x00138A28
	private void DeserializeSynchronizationTypeState(PhotonStream stream)
	{
		byte[] array = (byte[])stream.ReceiveNext();
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			this.m_SynchronizeLayers[i].SynchronizeType = (PhotonAnimatorView.SynchronizeType)array[i];
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			this.m_SynchronizeParameters[j].SynchronizeType = (PhotonAnimatorView.SynchronizeType)array[this.m_SynchronizeLayers.Count + j];
		}
	}

	// Token: 0x06003E67 RID: 15975 RVA: 0x0013A6B0 File Offset: 0x00138AB0
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.m_Animator == null)
		{
			return;
		}
		if (stream.isWriting)
		{
			if (this.m_WasSynchronizeTypeChanged)
			{
				this.m_StreamQueue.Reset();
				this.SerializeSynchronizationTypeState(stream);
				this.m_WasSynchronizeTypeChanged = false;
			}
			this.m_StreamQueue.Serialize(stream);
			this.SerializeDataDiscretly(stream);
		}
		else
		{
			if (stream.PeekNext() is byte[])
			{
				this.DeserializeSynchronizationTypeState(stream);
			}
			this.m_StreamQueue.Deserialize(stream);
			this.DeserializeDataDiscretly(stream);
		}
	}

	// Token: 0x040026D1 RID: 9937
	private Animator m_Animator;

	// Token: 0x040026D2 RID: 9938
	private PhotonStreamQueue m_StreamQueue;

	// Token: 0x040026D3 RID: 9939
	[HideInInspector]
	[SerializeField]
	private bool ShowLayerWeightsInspector = true;

	// Token: 0x040026D4 RID: 9940
	[HideInInspector]
	[SerializeField]
	private bool ShowParameterInspector = true;

	// Token: 0x040026D5 RID: 9941
	[HideInInspector]
	[SerializeField]
	private List<PhotonAnimatorView.SynchronizedParameter> m_SynchronizeParameters = new List<PhotonAnimatorView.SynchronizedParameter>();

	// Token: 0x040026D6 RID: 9942
	[HideInInspector]
	[SerializeField]
	private List<PhotonAnimatorView.SynchronizedLayer> m_SynchronizeLayers = new List<PhotonAnimatorView.SynchronizedLayer>();

	// Token: 0x040026D7 RID: 9943
	private Vector3 m_ReceiverPosition;

	// Token: 0x040026D8 RID: 9944
	private float m_LastDeserializeTime;

	// Token: 0x040026D9 RID: 9945
	private bool m_WasSynchronizeTypeChanged = true;

	// Token: 0x040026DA RID: 9946
	private PhotonView m_PhotonView;

	// Token: 0x040026DB RID: 9947
	private List<string> m_raisedDiscreteTriggersCache = new List<string>();

	// Token: 0x02000773 RID: 1907
	public enum ParameterType
	{
		// Token: 0x040026DD RID: 9949
		Float = 1,
		// Token: 0x040026DE RID: 9950
		Int = 3,
		// Token: 0x040026DF RID: 9951
		Bool,
		// Token: 0x040026E0 RID: 9952
		Trigger = 9
	}

	// Token: 0x02000774 RID: 1908
	public enum SynchronizeType
	{
		// Token: 0x040026E2 RID: 9954
		Disabled,
		// Token: 0x040026E3 RID: 9955
		Discrete,
		// Token: 0x040026E4 RID: 9956
		Continuous
	}

	// Token: 0x02000775 RID: 1909
	[Serializable]
	public class SynchronizedParameter
	{
		// Token: 0x040026E5 RID: 9957
		public PhotonAnimatorView.ParameterType Type;

		// Token: 0x040026E6 RID: 9958
		public PhotonAnimatorView.SynchronizeType SynchronizeType;

		// Token: 0x040026E7 RID: 9959
		public string Name;
	}

	// Token: 0x02000776 RID: 1910
	[Serializable]
	public class SynchronizedLayer
	{
		// Token: 0x040026E8 RID: 9960
		public PhotonAnimatorView.SynchronizeType SynchronizeType;

		// Token: 0x040026E9 RID: 9961
		public int LayerIndex;
	}
}
