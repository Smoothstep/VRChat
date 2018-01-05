using System;
using System.Collections.Generic;
using System.Reflection;
using Photon;
using UnityEngine;

// Token: 0x02000768 RID: 1896
[AddComponentMenu("Photon Networking/Photon View &v")]
public class PhotonView : Photon.MonoBehaviour
{
	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x06003DD1 RID: 15825 RVA: 0x00137A09 File Offset: 0x00135E09
	// (set) Token: 0x06003DD2 RID: 15826 RVA: 0x00137A11 File Offset: 0x00135E11
	public bool OwnerShipWasTransfered
	{
		get
		{
			return this._ownerShipWasTransfered;
		}
		set
		{
			if (this._ownerShipWasTransfered != value)
			{
				if (value)
				{
					this.ownershipTransferTime = (double)Time.time;
				}
				this._ownerShipWasTransfered = value;
			}
		}
	}

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x00137A38 File Offset: 0x00135E38
	// (set) Token: 0x06003DD4 RID: 15828 RVA: 0x00137A66 File Offset: 0x00135E66
	public int prefix
	{
		get
		{
			if (this.prefixBackup == -1 && PhotonNetwork.networkingPeer != null)
			{
				this.prefixBackup = (int)PhotonNetwork.networkingPeer.currentLevelPrefix;
			}
			return this.prefixBackup;
		}
		set
		{
			this.prefixBackup = value;
		}
	}

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x00137A6F File Offset: 0x00135E6F
	// (set) Token: 0x06003DD6 RID: 15830 RVA: 0x00137A98 File Offset: 0x00135E98
	public object[] instantiationData
	{
		get
		{
			if (!this.didAwake)
			{
				this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
			}
			return this.instantiationDataField;
		}
		set
		{
			this.instantiationDataField = value;
		}
	}

	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x00137AA1 File Offset: 0x00135EA1
	// (set) Token: 0x06003DD8 RID: 15832 RVA: 0x00137AAC File Offset: 0x00135EAC
	public int viewID
	{
		get
		{
			return this.viewIdField;
		}
		set
		{
			bool flag = this.didAwake && this.viewIdField == 0;
			this.ownerId = value / PhotonNetwork.MAX_VIEW_IDS;
			this.viewIdField = value;
			if (flag)
			{
				PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			}
		}
	}

	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x00137AF6 File Offset: 0x00135EF6
	public bool isSceneView
	{
		get
		{
			return this.CreatorActorNr == 0;
		}
	}

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x06003DDA RID: 15834 RVA: 0x00137B01 File Offset: 0x00135F01
	public PhotonPlayer owner
	{
		get
		{
			return PhotonPlayer.Find(this.ownerId);
		}
	}

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x06003DDB RID: 15835 RVA: 0x00137B0E File Offset: 0x00135F0E
	public int OwnerActorNr
	{
		get
		{
			return this.ownerId;
		}
	}

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x06003DDC RID: 15836 RVA: 0x00137B16 File Offset: 0x00135F16
	public bool isOwnerActive
	{
		get
		{
			return this.ownerId != 0 && PhotonNetwork.networkingPeer.mActors.ContainsKey(this.ownerId);
		}
	}

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x06003DDD RID: 15837 RVA: 0x00137B3B File Offset: 0x00135F3B
	public int CreatorActorNr
	{
		get
		{
			return this.viewIdField / PhotonNetwork.MAX_VIEW_IDS;
		}
	}

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x06003DDE RID: 15838 RVA: 0x00137B49 File Offset: 0x00135F49
	public bool isMine
	{
		get
		{
			return this.ownerId == PhotonNetwork.player.ID || (!this.isOwnerActive && PhotonNetwork.isMasterClient);
		}
	}

	// Token: 0x06003DDF RID: 15839 RVA: 0x00137B76 File Offset: 0x00135F76
	protected internal void Awake()
	{
		if (this.viewID != 0)
		{
			PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
		}
		this.didAwake = true;
	}

	// Token: 0x06003DE0 RID: 15840 RVA: 0x00137BAB File Offset: 0x00135FAB
	public void RequestOwnership()
	{
		PhotonNetwork.networkingPeer.RequestOwnership(this.viewID, this.ownerId);
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x00137BC3 File Offset: 0x00135FC3
	public void TransferOwnership(PhotonPlayer newOwner)
	{
		this.TransferOwnership(newOwner.ID);
	}

	// Token: 0x06003DE2 RID: 15842 RVA: 0x00137BD1 File Offset: 0x00135FD1
	public void TransferOwnership(int newOwnerId)
	{
		if (newOwnerId == this.ownerId)
		{
			return;
		}
		PhotonNetwork.networkingPeer.TransferOwnership(this.viewID, newOwnerId);
		this.ownerId = newOwnerId;
		this.OwnerShipWasTransfered = true;
	}

	// Token: 0x06003DE3 RID: 15843 RVA: 0x00137C00 File Offset: 0x00136000
	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (this.CreatorActorNr == 0 && !this.OwnerShipWasTransfered && (this.currentMasterID == -1 || this.ownerId == this.currentMasterID))
		{
			this.ownerId = newMasterClient.ID;
		}
		this.currentMasterID = newMasterClient.ID;
	}

	// Token: 0x06003DE4 RID: 15844 RVA: 0x00137C58 File Offset: 0x00136058
	protected internal void OnDestroy()
	{
		if (!this.removedFromLocalViewList)
		{
			bool flag = PhotonNetwork.networkingPeer.LocalCleanPhotonView(this);
			bool flag2 = false;
			if (flag && !flag2 && this.instantiationId > 0 && !PhotonHandler.AppQuits && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log("PUN-instantiated '" + base.gameObject.name + "' got destroyed by engine. This is OK when loading levels. Otherwise use: PhotonNetwork.Destroy().");
			}
		}
	}

	// Token: 0x06003DE5 RID: 15845 RVA: 0x00137CCC File Offset: 0x001360CC
	public void SerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
		{
			for (int i = 0; i < this.ObservedComponents.Count; i++)
			{
				this.SerializeComponent(this.ObservedComponents[i], stream, info);
			}
		}
	}

	// Token: 0x06003DE6 RID: 15846 RVA: 0x00137D28 File Offset: 0x00136128
	public void DeserializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
		{
			for (int i = 0; i < this.ObservedComponents.Count; i++)
			{
				this.DeserializeComponent(this.ObservedComponents[i], stream, info);
			}
		}
	}

	// Token: 0x06003DE7 RID: 15847 RVA: 0x00137D84 File Offset: 0x00136184
	protected internal void DeserializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		if (component == null)
		{
			return;
		}
		if (component is UnityEngine.MonoBehaviour)
		{
			this.ExecuteComponentOnSerialize(component, stream, info);
		}
		else if (component is Transform)
		{
			Transform transform = (Transform)component;
			switch (this.onSerializeTransformOption)
			{
			case OnSerializeTransform.OnlyPosition:
				transform.localPosition = (Vector3)stream.ReceiveNext();
				break;
			case OnSerializeTransform.OnlyRotation:
				transform.localRotation = (Quaternion)stream.ReceiveNext();
				break;
			case OnSerializeTransform.OnlyScale:
				transform.localScale = (Vector3)stream.ReceiveNext();
				break;
			case OnSerializeTransform.PositionAndRotation:
				transform.localPosition = (Vector3)stream.ReceiveNext();
				transform.localRotation = (Quaternion)stream.ReceiveNext();
				break;
			case OnSerializeTransform.All:
				transform.localPosition = (Vector3)stream.ReceiveNext();
				transform.localRotation = (Quaternion)stream.ReceiveNext();
				transform.localScale = (Vector3)stream.ReceiveNext();
				break;
			}
		}
		else if (component is Rigidbody)
		{
			Rigidbody rigidbody = (Rigidbody)component;
			OnSerializeRigidBody onSerializeRigidBody = this.onSerializeRigidBodyOption;
			if (onSerializeRigidBody != OnSerializeRigidBody.All)
			{
				if (onSerializeRigidBody != OnSerializeRigidBody.OnlyAngularVelocity)
				{
					if (onSerializeRigidBody == OnSerializeRigidBody.OnlyVelocity)
					{
						rigidbody.velocity = (Vector3)stream.ReceiveNext();
					}
				}
				else
				{
					rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
				}
			}
			else
			{
				rigidbody.velocity = (Vector3)stream.ReceiveNext();
				rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
			}
		}
		else if (component is Rigidbody2D)
		{
			Rigidbody2D rigidbody2D = (Rigidbody2D)component;
			OnSerializeRigidBody onSerializeRigidBody2 = this.onSerializeRigidBodyOption;
			if (onSerializeRigidBody2 != OnSerializeRigidBody.All)
			{
				if (onSerializeRigidBody2 != OnSerializeRigidBody.OnlyAngularVelocity)
				{
					if (onSerializeRigidBody2 == OnSerializeRigidBody.OnlyVelocity)
					{
						rigidbody2D.velocity = (Vector2)stream.ReceiveNext();
					}
				}
				else
				{
					rigidbody2D.angularVelocity = (float)stream.ReceiveNext();
				}
			}
			else
			{
				rigidbody2D.velocity = (Vector2)stream.ReceiveNext();
				rigidbody2D.angularVelocity = (float)stream.ReceiveNext();
			}
		}
		else
		{
			Debug.LogError("Type of observed is unknown when receiving.");
		}
	}

	// Token: 0x06003DE8 RID: 15848 RVA: 0x00137FBC File Offset: 0x001363BC
	protected internal void SerializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		if (component == null)
		{
			return;
		}
		if (component is UnityEngine.MonoBehaviour)
		{
			this.ExecuteComponentOnSerialize(component, stream, info);
		}
		else if (component is Transform)
		{
			Transform transform = (Transform)component;
			switch (this.onSerializeTransformOption)
			{
			case OnSerializeTransform.OnlyPosition:
				stream.SendNext(transform.localPosition);
				break;
			case OnSerializeTransform.OnlyRotation:
				stream.SendNext(transform.localRotation);
				break;
			case OnSerializeTransform.OnlyScale:
				stream.SendNext(transform.localScale);
				break;
			case OnSerializeTransform.PositionAndRotation:
				stream.SendNext(transform.localPosition);
				stream.SendNext(transform.localRotation);
				break;
			case OnSerializeTransform.All:
				stream.SendNext(transform.localPosition);
				stream.SendNext(transform.localRotation);
				stream.SendNext(transform.localScale);
				break;
			}
		}
		else if (component is Rigidbody)
		{
			Rigidbody rigidbody = (Rigidbody)component;
			OnSerializeRigidBody onSerializeRigidBody = this.onSerializeRigidBodyOption;
			if (onSerializeRigidBody != OnSerializeRigidBody.All)
			{
				if (onSerializeRigidBody != OnSerializeRigidBody.OnlyAngularVelocity)
				{
					if (onSerializeRigidBody == OnSerializeRigidBody.OnlyVelocity)
					{
						stream.SendNext(rigidbody.velocity);
					}
				}
				else
				{
					stream.SendNext(rigidbody.angularVelocity);
				}
			}
			else
			{
				stream.SendNext(rigidbody.velocity);
				stream.SendNext(rigidbody.angularVelocity);
			}
		}
		else if (component is Rigidbody2D)
		{
			Rigidbody2D rigidbody2D = (Rigidbody2D)component;
			OnSerializeRigidBody onSerializeRigidBody2 = this.onSerializeRigidBodyOption;
			if (onSerializeRigidBody2 != OnSerializeRigidBody.All)
			{
				if (onSerializeRigidBody2 != OnSerializeRigidBody.OnlyAngularVelocity)
				{
					if (onSerializeRigidBody2 == OnSerializeRigidBody.OnlyVelocity)
					{
						stream.SendNext(rigidbody2D.velocity);
					}
				}
				else
				{
					stream.SendNext(rigidbody2D.angularVelocity);
				}
			}
			else
			{
				stream.SendNext(rigidbody2D.velocity);
				stream.SendNext(rigidbody2D.angularVelocity);
			}
		}
		else
		{
			Debug.LogError("Observed type is not serializable: " + component.GetType());
		}
	}

	// Token: 0x06003DE9 RID: 15849 RVA: 0x00138200 File Offset: 0x00136600
	protected internal void ExecuteComponentOnSerialize(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		IPunObservable punObservable = component as IPunObservable;
		if (punObservable != null)
		{
			punObservable.OnPhotonSerializeView(stream, info);
		}
		else if (component != null)
		{
			MethodInfo methodInfo = null;
			if (!this.m_OnSerializeMethodInfos.TryGetValue(component, out methodInfo))
			{
				if (!NetworkingPeer.GetMethod(component as UnityEngine.MonoBehaviour, PhotonNetworkingMessage.OnPhotonSerializeView.ToString(), out methodInfo))
				{
					Debug.LogError("The observed monobehaviour (" + component.name + ") of this PhotonView does not implement OnPhotonSerializeView()!");
					methodInfo = null;
				}
				this.m_OnSerializeMethodInfos.Add(component, methodInfo);
			}
			if (methodInfo != null)
			{
				methodInfo.Invoke(component, new object[]
				{
					stream,
					info
				});
			}
		}
	}

	// Token: 0x06003DEA RID: 15850 RVA: 0x001382B9 File Offset: 0x001366B9
	public void RefreshRpcMonoBehaviourCache()
	{
		this.RpcMonoBehaviours = base.GetComponents<UnityEngine.MonoBehaviour>();
	}

	// Token: 0x06003DEB RID: 15851 RVA: 0x001382C7 File Offset: 0x001366C7
	public void RPC(string methodName, PhotonTargets target, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, target, false, parameters);
	}

	// Token: 0x06003DEC RID: 15852 RVA: 0x001382D3 File Offset: 0x001366D3
	public void RpcSecure(string methodName, PhotonTargets target, bool encrypt, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, target, encrypt, parameters);
	}

	// Token: 0x06003DED RID: 15853 RVA: 0x001382E0 File Offset: 0x001366E0
	public void RPC(string methodName, PhotonPlayer targetPlayer, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, false, parameters);
	}

	// Token: 0x06003DEE RID: 15854 RVA: 0x001382EC File Offset: 0x001366EC
	public void RpcSecure(string methodName, PhotonPlayer targetPlayer, bool encrypt, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, encrypt, parameters);
	}

	// Token: 0x06003DEF RID: 15855 RVA: 0x001382F9 File Offset: 0x001366F9
	public static PhotonView Get(Component component)
	{
		return component.GetComponent<PhotonView>();
	}

	// Token: 0x06003DF0 RID: 15856 RVA: 0x00138301 File Offset: 0x00136701
	public static PhotonView Get(GameObject gameObj)
	{
		return gameObj.GetComponent<PhotonView>();
	}

	// Token: 0x06003DF1 RID: 15857 RVA: 0x00138309 File Offset: 0x00136709
	public static PhotonView Find(int viewID)
	{
		return PhotonNetwork.networkingPeer.GetPhotonView(viewID);
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x00138318 File Offset: 0x00136718
	public override string ToString()
	{
		return string.Format("View ({3}){0} on {1} {2}", new object[]
		{
			this.viewID,
			(!(base.gameObject != null)) ? "GO==null" : base.gameObject.name,
			(!this.isSceneView) ? string.Empty : "(scene)",
			this.prefix
		});
	}

	// Token: 0x04002687 RID: 9863
	public int ownerId;

	// Token: 0x04002688 RID: 9864
	public byte group;

	// Token: 0x04002689 RID: 9865
	protected internal bool mixedModeIsReliable;

	// Token: 0x0400268A RID: 9866
	private bool _ownerShipWasTransfered;

	// Token: 0x0400268B RID: 9867
	public int prefixBackup = -1;

	// Token: 0x0400268C RID: 9868
	internal object[] instantiationDataField;

	// Token: 0x0400268D RID: 9869
	protected internal object[] lastOnSerializeDataSent;

	// Token: 0x0400268E RID: 9870
	protected internal object[] lastOnSerializeDataReceived;

	// Token: 0x0400268F RID: 9871
	public ViewSynchronization synchronization;

	// Token: 0x04002690 RID: 9872
	public OnSerializeTransform onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;

	// Token: 0x04002691 RID: 9873
	public OnSerializeRigidBody onSerializeRigidBodyOption = OnSerializeRigidBody.All;

	// Token: 0x04002692 RID: 9874
	public OwnershipOption ownershipTransfer;

	// Token: 0x04002693 RID: 9875
	public List<Component> ObservedComponents;

	// Token: 0x04002694 RID: 9876
	private Dictionary<Component, MethodInfo> m_OnSerializeMethodInfos = new Dictionary<Component, MethodInfo>(3);

	// Token: 0x04002695 RID: 9877
	[SerializeField]
	private int viewIdField;

	// Token: 0x04002696 RID: 9878
	public int instantiationId;

	// Token: 0x04002697 RID: 9879
	public bool isProtectedFromCleanup;

	// Token: 0x04002698 RID: 9880
	public double ownershipTransferTime;

	// Token: 0x04002699 RID: 9881
	public int currentMasterID = -1;

	// Token: 0x0400269A RID: 9882
	protected internal bool didAwake;

	// Token: 0x0400269B RID: 9883
	[SerializeField]
	protected internal bool isRuntimeInstantiated;

	// Token: 0x0400269C RID: 9884
	protected internal bool removedFromLocalViewList;

	// Token: 0x0400269D RID: 9885
	internal UnityEngine.MonoBehaviour[] RpcMonoBehaviours;

	// Token: 0x0400269E RID: 9886
	private MethodInfo OnSerializeMethodInfo;

	// Token: 0x0400269F RID: 9887
	private bool failedToFindOnSerialize;
}
