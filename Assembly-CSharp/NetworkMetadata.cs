using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VRC;
using VRCSDK2;

// Token: 0x02000AB0 RID: 2736
public class NetworkMetadata : MonoBehaviour
{
	// Token: 0x17000C0D RID: 3085
	// (get) Token: 0x060052E0 RID: 21216 RVA: 0x001C6AC5 File Offset: 0x001C4EC5
	// (set) Token: 0x060052E1 RID: 21217 RVA: 0x001C6AE0 File Offset: 0x001C4EE0
	public bool isReady
	{
		get
		{
			return this.blockReady.Count == 0 && this._isReady;
		}
		set
		{
			this._isReady = value;
		}
	}

	// Token: 0x060052E2 RID: 21218 RVA: 0x001C6AE9 File Offset: 0x001C4EE9
	public bool AddReadyBlock(Component c)
	{
		if (c == null || base.gameObject.IsReady())
		{
			return false;
		}
		if (!this.blockReady.Contains(c))
		{
			this.blockReady.Add(c);
		}
		return true;
	}

	// Token: 0x060052E3 RID: 21219 RVA: 0x001C6B27 File Offset: 0x001C4F27
	public void RemoveReadyBlock(Component c)
	{
		if (c == null)
		{
			return;
		}
		if (this.blockReady.Contains(c))
		{
			this.blockReady.Remove(c);
		}
	}

	// Token: 0x060052E4 RID: 21220 RVA: 0x001C6B54 File Offset: 0x001C4F54
	private void Awake()
	{
		base.StartCoroutine(this.Ready());
	}

	// Token: 0x060052E5 RID: 21221 RVA: 0x001C6B64 File Offset: 0x001C4F64
	public IEnumerator Ready()
	{
		if (!NetworkMetadata.AllMetadata.Contains(this))
		{
			NetworkMetadata.AllMetadata.Add(this);
		}
		if (this.isReady)
		{
			yield break;
		}
		this.isReady = false;
		this.combinedID = null;
		this.waitingFor = "photon view IDs";
		PhotonView[] views = base.gameObject.GetComponents<PhotonView>();
		while (this != null)
		{
			if (!views.Any((PhotonView v) => v != null && v.viewID <= 0))
			{
				break;
			}
			INetworkID netID = base.gameObject.GetComponent<INetworkID>();
			VRCPunBehaviour vrc = base.gameObject.GetComponent<VRCPunBehaviour>();
			foreach (PhotonView photonView in views)
			{
				if (netID != null && netID.NetworkID > 0)
				{
					photonView.viewID = netID.NetworkID;
				}
				else if (vrc != null && vrc.ReservedID > 0)
				{
					photonView.viewID = vrc.ReservedID;
				}
			}
			yield return null;
		}
		if (this == null)
		{
			yield break;
		}
		this.waitingFor = "network to settle";
		while (this != null && (NetworkManager.Instance == null || !VRC.Network.IsNetworkSettled))
		{
			yield return null;
		}
		if (this == null)
		{
			yield break;
		}
		this.waitingFor = "event handlers";
		VRC_EventHandler evtHandler = base.gameObject.GetComponent<VRC_EventHandler>();
		yield return VRC.Network.ConfigureEventHandler(evtHandler);
		this.waitingFor = "children to ready";
		bool childrenNotReady = true;
		while (this != null && childrenNotReady)
		{
			childrenNotReady = false;
			for (int idx = 0; idx < base.transform.childCount; idx++)
			{
				Transform t = base.transform.GetChild(idx);
				if (!(t == null))
				{
					GameObject child = base.transform.GetChild(idx).gameObject;
					if (child != null && child.activeInHierarchy && !VRC.Network.IsObjectReady(child))
					{
						childrenNotReady = true;
						this.waitingFor = child.name + " to ready";
						yield return null;
						break;
					}
				}
			}
		}
		if (this == null)
		{
			yield break;
		}
		this.waitingFor = "blockReady to be lifted by " + string.Join(", ", (from b in this.blockReady
		select b.GetType().Name).ToArray<string>());
		while (this != null && this.blockReady.Count > 0)
		{
			yield return null;
		}
		if (this == null)
		{
			yield break;
		}
		this.waitingFor = "local player to ready";
		while (this != null && VRC.Network.LocalPlayer == null)
		{
			yield return null;
		}
		if (this == null)
		{
			yield break;
		}
		if (!base.gameObject.IsPlayer())
		{
			VRC.Player localPlayer = VRC.Network.LocalPlayer;
			while (this != null && !base.gameObject.IsPlayer() && !localPlayer.gameObject.IsReady())
			{
				yield return null;
			}
			if (this == null)
			{
				yield break;
			}
		}
		this.waitingFor = "configuration";
		this.combinedID = VRC.Network.GetCombinedID(base.gameObject);
		this.isInstantiated = (base.GetComponent<ObjectInstantiatorHandle>() != null);
		this.sendCallbacks = (base.GetComponent<NetworkManager>() == null);
		if (this.combinedID != null)
		{
			if (NetworkMetadata.ObjectCache.ContainsKey(this.combinedID.Value))
			{
				NetworkMetadata.ObjectCache.Remove(this.combinedID.Value);
			}
			NetworkMetadata.ObjectCache.Add(this.combinedID.Value, base.gameObject);
		}
		this.isReady = true;
		this.waitingFor = "callbacks to finish";
		if (this.sendCallbacks)
		{
			try
			{
				base.SendMessage("OnNetworkReady", SendMessageOptions.DontRequireReceiver);
			}
			catch (MissingMethodException)
			{
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, base.gameObject);
			}
		}
		this.waitingFor = "nothing";
		yield break;
	}

	// Token: 0x060052E6 RID: 21222 RVA: 0x001C6B80 File Offset: 0x001C4F80
	private void HandleOnPlayerJoined(VRC.Player p)
	{
		try
		{
			if (p != null && this.sendCallbacks)
			{
				base.SendMessage("OnPlayerJoined", p.playerApi, SendMessageOptions.DontRequireReceiver);
			}
		}
		catch (MissingMethodException)
		{
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, base.gameObject);
		}
	}

	// Token: 0x060052E7 RID: 21223 RVA: 0x001C6BF0 File Offset: 0x001C4FF0
	private void HandleOnPlayerLeft(VRC.Player p)
	{
		try
		{
			if (p != null && this.sendCallbacks)
			{
				base.SendMessage("OnPlayerLeft", p.playerApi, SendMessageOptions.DontRequireReceiver);
			}
		}
		catch (MissingMethodException)
		{
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, base.gameObject);
		}
	}

	// Token: 0x060052E8 RID: 21224 RVA: 0x001C6C60 File Offset: 0x001C5060
	private void OnDestroy()
	{
		if (this.sendCallbacks && NetworkManager.Instance != null)
		{
			NetworkManager.Instance.OnPlayerJoinedEvent.RemoveListener(new UnityAction<VRC.Player>(this.HandleOnPlayerJoined));
			NetworkManager.Instance.OnPlayerLeftEvent.RemoveListener(new UnityAction<VRC.Player>(this.HandleOnPlayerLeft));
		}
		if (this.combinedID != null && NetworkMetadata.ObjectCache.ContainsKey(this.combinedID.Value))
		{
			NetworkMetadata.ObjectCache.Remove(this.combinedID.Value);
		}
		if (NetworkMetadata.AllMetadata.Contains(this))
		{
			NetworkMetadata.AllMetadata.Remove(this);
		}
		if (VRC_EventLog.Instance != null)
		{
			string path = VRC.Network.GetGameObjectPath(base.gameObject);
			VRC_EventLog.Instance.RemoveEventsIf((VRC_EventLog.EventLogEntry e) => (e.Event.EventType != VRC_EventHandler.VrcEventType.DestroyObject || this.isInstantiated) && (e.Event.ParameterObject == this.gameObject || e.ObjectPath == path || (e.Event.ParameterObjects != null && e.Event.ParameterObjects.Contains(this.gameObject)) || (e.Event.EventType == VRC_EventHandler.VrcEventType.SendRPC && e.rpcParameters.Contains(this.gameObject))));
		}
		base.StopAllCoroutines();
	}

	// Token: 0x060052E9 RID: 21225 RVA: 0x001C6D68 File Offset: 0x001C5168
	private void OnDisable()
	{
		if (this.sendCallbacks && NetworkManager.Instance != null)
		{
			NetworkManager.Instance.OnPlayerJoinedEvent.RemoveListener(new UnityAction<VRC.Player>(this.HandleOnPlayerJoined));
			NetworkManager.Instance.OnPlayerLeftEvent.RemoveListener(new UnityAction<VRC.Player>(this.HandleOnPlayerLeft));
		}
	}

	// Token: 0x060052EA RID: 21226 RVA: 0x001C6DC8 File Offset: 0x001C51C8
	private void OnEnable()
	{
		if (this.sendCallbacks && NetworkManager.Instance != null)
		{
			NetworkManager.Instance.OnPlayerJoinedEvent.AddListener(new UnityAction<VRC.Player>(this.HandleOnPlayerJoined));
			NetworkManager.Instance.OnPlayerLeftEvent.AddListener(new UnityAction<VRC.Player>(this.HandleOnPlayerLeft));
		}
		base.StartCoroutine(this.Ready());
	}

	// Token: 0x04003A7E RID: 14974
	public static Dictionary<long, GameObject> ObjectCache = new Dictionary<long, GameObject>();

	// Token: 0x04003A7F RID: 14975
	public static HashSet<NetworkMetadata> AllMetadata = new HashSet<NetworkMetadata>();

	// Token: 0x04003A80 RID: 14976
	private List<Component> blockReady = new List<Component>();

	// Token: 0x04003A81 RID: 14977
	private bool _isReady;

	// Token: 0x04003A82 RID: 14978
	public long? combinedID;

	// Token: 0x04003A83 RID: 14979
	public string waitingFor = "ready coroutine to be called";

	// Token: 0x04003A84 RID: 14980
	private bool sendCallbacks = true;

	// Token: 0x04003A85 RID: 14981
	private bool isInstantiated;
}
