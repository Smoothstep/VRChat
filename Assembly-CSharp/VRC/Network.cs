using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using BestHTTP;
using UnityEngine;
using VRC.Core;
using VRCSDK2;

namespace VRC
{
	// Token: 0x02000AB1 RID: 2737
	public class Network
	{
		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x060052ED RID: 21229 RVA: 0x001C76B6 File Offset: 0x001C5AB6
		public static bool IsNetworkSettled
		{
			get
			{
				return VRC_EventLog.IsFinishedInitialLoad && Network.LocalPlayer != null && Network.LocalPlayer.name != null && RoomManager.inRoom;
			}
		}

        public static bool Get_IsNetworkSettled()
        {
            return VRC_EventLog.IsFinishedInitialLoad && Network.LocalPlayer != null && Network.LocalPlayer.name != null && RoomManager.inRoom;
        }

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x060052EE RID: 21230 RVA: 0x001C76E9 File Offset: 0x001C5AE9
		public static bool IsSadAndAlone
		{
			get
			{
				return PhotonNetwork.isMasterClient && PhotonNetwork.room != null && PhotonNetwork.room.PlayerCount == 1 && Network.IsNetworkSettled;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x060052EF RID: 21231 RVA: 0x001C7717 File Offset: 0x001C5B17
		public static IEnumerable<Player> Players
		{
			get
			{
				return PlayerManager.GetAllPlayers();
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x060052F0 RID: 21232 RVA: 0x001C7720 File Offset: 0x001C5B20
		public static Player MasterPlayer
		{
			get
			{
				Player[] allPlayers = PlayerManager.GetAllPlayers();
				if (allPlayers.Length == 0)
				{
					return Network.LocalPlayer;
				}
				if (allPlayers.Length == 1)
				{
					return allPlayers[0];
				}
				return allPlayers.FirstOrDefault((Player p) => p.isMaster);
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x060052F1 RID: 21233 RVA: 0x001C7771 File Offset: 0x001C5B71
		public static bool IsMaster
		{
			get
			{
				return Network.LocalPlayer != null && Network.LocalPlayer.isMaster;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x060052F2 RID: 21234 RVA: 0x001C7790 File Offset: 0x001C5B90
		public static Player LocalPlayer
		{
			get
			{
				return (!(VRCPlayer.Instance == null)) ? VRCPlayer.Instance.player : null;
			}
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x060052F3 RID: 21235 RVA: 0x001C77B2 File Offset: 0x001C5BB2
		public static int LocalInstigatorID
		{
			get
			{
				return Network.GetInstigatorID(Network.LocalPlayer);
			}
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x001C77C0 File Offset: 0x001C5BC0
		public static double SimulationDelay(VRCPlayer player)
		{
			if (player == null)
			{
				return 0.0;
			}
			int num = Mathf.FloorToInt((float)(player.Ping + 50) / 10f);
			int num2 = Mathf.FloorToInt((1f - player.ConnectionQuality) * 10f);
			double num3 = (double)Mathf.Min(2f, (float)(num + num2));
			return Network.SendInterval * num3;
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x001C7828 File Offset: 0x001C5C28
		public static double SimulationDelay(Player player)
		{
			if (player == null)
			{
				return 0.0;
			}
			return Network.SimulationDelay(player.vrcPlayer);
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x060052F6 RID: 21238 RVA: 0x001C784B File Offset: 0x001C5C4B
		public static double ExpectedInterval
		{
			get
			{
				return Network.SendInterval + (double)Time.smoothDeltaTime;
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x060052F7 RID: 21239 RVA: 0x001C7859 File Offset: 0x001C5C59
		public static double SendInterval
		{
			get
			{
				return 1.0 / (double)PhotonNetwork.sendRateOnSerialize;
			}
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x001C786B File Offset: 0x001C5C6B
		public static int GetInstigatorID(PhotonPlayer player)
		{
			return (player == null) ? -1 : player.ID;
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x001C787F File Offset: 0x001C5C7F
		public static int GetInstigatorID(Player player)
		{
			return (!(player != null)) ? -1 : player.GetPhotonPlayerId();
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x001C789C File Offset: 0x001C5C9C
		public static Player GetPlayerByInstigatorID(int instigatorID)
		{
			PhotonPlayer photonPlayer = PhotonPlayer.Find(instigatorID);
			return PlayerManager.GetPlayer(photonPlayer);
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x001C78B6 File Offset: 0x001C5CB6
		public static bool PlayerIsMaster(Player player)
		{
			return !(player == null) && PhotonNetwork.masterClient != null && PhotonNetwork.masterClient.ID == player.GetPhotonPlayerId();
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x001C78E8 File Offset: 0x001C5CE8
		public static bool AllowOwnershipModification(Player player, GameObject obj, Network.OwnershipModificationType modificationType = Network.OwnershipModificationType.Request, bool wholeHierarchy = true)
		{
			if (obj == null)
			{
				UnityEngine.Debug.LogError("Attempted to check ownership modification permissions on a null object.");
				return false;
			}
			if (player == null)
			{
				UnityEngine.Debug.LogError("Attempted to check ownership modification permissions for a null player.");
				return false;
			}
			if (obj.transform.parent != null && wholeHierarchy && !Network.AllowOwnershipModification(player, obj.transform.parent.gameObject, modificationType, wholeHierarchy))
			{
				return false;
			}
			if (modificationType == Network.OwnershipModificationType.Local && player.isLocal)
			{
				return true;
			}
			if (Network.IsOwner(player, obj))
			{
				return true;
			}
			PhotonView[] components = obj.GetComponents<PhotonView>();
			if (components.Length == 0)
			{
				if (obj.isStatic)
				{
					UnityEngine.Debug.LogErrorFormat("Attempt to take ownership of non-sync'd object {0} with type {1}. Cannot modify static objects.", new object[]
					{
						obj.name,
						modificationType
					});
					return false;
				}
				return true;
			}
			else
			{
				if (components.Any((PhotonView view) => view.ownershipTransfer == OwnershipOption.Fixed))
				{
					UnityEngine.Debug.LogError("View does not allow modification.");
					return false;
				}
				Player component = obj.GetComponent<Player>();
				if (component != null && component != player)
				{
					UnityEngine.Debug.LogError("Cannot take ownership of objects that are attached to players or which are players.");
					return false;
				}
				WebPanelInternal[] components2 = obj.GetComponents<WebPanelInternal>();
				if (components2.Any((WebPanelInternal panel) => panel.ReceivesInput && panel.ShouldSync && VRCUiManager.Instance.popups.isPopupActive))
				{
					UnityEngine.Debug.LogError("Web Panel blocks ownership change.");
					return false;
				}
				VRC_Pickup component2 = obj.GetComponent<VRC_Pickup>();
				if (component2 != null && component2.IsHeld && ((component2.DisallowTheft && modificationType == Network.OwnershipModificationType.Pickup) || modificationType == Network.OwnershipModificationType.Collision))
				{
					return false;
				}
				ObjectInternal component3 = obj.GetComponent<ObjectInternal>();
				return !(component3 != null) || component3.AllowCollisionTransfer || modificationType != Network.OwnershipModificationType.Collision;
			}
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x001C7ABF File Offset: 0x001C5EBF
		public static bool IsOwner(Player player, GameObject obj)
		{
			return !(obj == null) && !(player == null) && Network.IsOwnerById(player.GetPhotonPlayerId(), obj);
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x001C7AE7 File Offset: 0x001C5EE7
		public static bool IsOwner(GameObject obj)
		{
			return Network.IsOwnerById(PhotonNetwork.player.ID, obj);
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x001C7AFC File Offset: 0x001C5EFC
		private static bool IsOwnerById(int playerId, GameObject obj)
		{
			int? ownerId = Network.GetOwnerId(obj);
			if (ownerId == null || ownerId.Value <= 0)
			{
				return PhotonNetwork.isMasterClient;
			}
			return ownerId.Value == playerId;
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x001C7B40 File Offset: 0x001C5F40
		public static Player GetOwner(GameObject obj)
		{
			int? ownerId = Network.GetOwnerId(obj);
			if (ownerId == null)
			{
				return null;
			}
			return Network.GetPlayer(PhotonPlayer.Find(ownerId.Value));
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x001C7B74 File Offset: 0x001C5F74
		public static Player GetPlayer(PhotonPlayer player)
		{
			if (player == null)
			{
				return null;
			}
			Player player2 = PlayerManager.GetPlayer(player);
			if (player2 == null)
			{
				IEnumerable<PhotonView> enumerable = from v in AssetManagement.FindObjects<PhotonView>()
				where v.ownerId == player.ID
				select v;
				foreach (PhotonView photonView in enumerable)
				{
					Player component = photonView.GetComponent<Player>();
					if (component != null)
					{
						return component;
					}
				}
			}
			return (!(player2 == null)) ? player2 : null;
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x001C7C44 File Offset: 0x001C6044
		public static int? GetOwnerId(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			PhotonView photonView = obj.GetComponent<PhotonView>();
			if (photonView == null)
			{
				photonView = obj.GetComponentInParent<PhotonView>();
			}
			return new int?((!(photonView == null)) ? photonView.ownerId : ((PhotonNetwork.masterClient != null) ? PhotonNetwork.masterClient.ID : 0));
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x001C7CB8 File Offset: 0x001C60B8
		private static T[] GetComponentsInHierarchy<T>(GameObject obj, bool wholeHierarchy)
		{
			if (wholeHierarchy)
			{
				HashSet<T> hashSet = new HashSet<T>();
				foreach (T item in obj.GetComponentsInParent<T>())
				{
					if (!hashSet.Contains(item))
					{
						hashSet.Add(item);
					}
				}
				foreach (T item2 in obj.GetComponentsInChildren<T>())
				{
					if (!hashSet.Contains(item2))
					{
						hashSet.Add(item2);
					}
				}
				return hashSet.ToArray<T>();
			}
			return obj.GetComponents<T>();
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x001C7D58 File Offset: 0x001C6158
		public static bool SetOwner(Player player, GameObject obj, Network.OwnershipModificationType modificationType = Network.OwnershipModificationType.Request, bool wholeHierarchy = true)
		{
			if (player == null || obj == null)
			{
				UnityEngine.Debug.LogError("Invalid argument for SetOwner, received a null " + ((!(player == null)) ? "game object" : "player"));
				return false;
			}
			if (Network.IsOwner(player, obj))
			{
				return true;
			}
			PhotonView component = player.GetComponent<PhotonView>();
			if (component == null || component.owner == null)
			{
				throw new ArgumentNullException("Player does not have a valid photon view");
			}
			if (Network.AllowOwnershipModification(player, obj, modificationType, wholeHierarchy))
			{
				UnityEngine.Debug.LogFormat("<color=grey>Transferring ownership of {0}, type {1}, wholeHierarchy {2}</color>", new object[]
				{
					obj.name,
					modificationType,
					wholeHierarchy
				});
				PhotonView[] componentsInHierarchy = Network.GetComponentsInHierarchy<PhotonView>(obj, wholeHierarchy);
				foreach (PhotonView view in componentsInHierarchy)
				{
					if (!Network.TransferOwnership(view, player.GetPhotonPlayerId()))
					{
						return false;
					}
				}
				return true;
			}
			UnityEngine.Debug.LogErrorFormat("No ownership modification allowed for {0}, type {1}, wholeHierarchy {2}</color>", new object[]
			{
				obj.name,
				modificationType,
				wholeHierarchy
			});
			return false;
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x001C7E84 File Offset: 0x001C6284
		private static bool TransferOwnership(PhotonView view, int desiredID)
		{
			if (view.ownerId == desiredID)
			{
				return true;
			}
			if (view.ownerId <= 0 || !view.isOwnerActive)
			{
				view.TransferOwnership(desiredID);
				return view.ownerId == desiredID;
			}
			view.TransferOwnership(desiredID);
			return view.ownerId == desiredID;
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x001C7ED8 File Offset: 0x001C62D8
		public static double GetOwnershipTransferTime(GameObject go)
		{
			if (go == null)
			{
				return 0.0;
			}
			PhotonView componentInParent = go.GetComponentInParent<PhotonView>();
			return (!(componentInParent == null)) ? componentInParent.ownershipTransferTime : 0.0;
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x001C7F22 File Offset: 0x001C6322
		public static NetworkMetadata GetMetadata(GameObject obj)
		{
			Network.IsObjectReady(obj);
			return obj.GetComponent<NetworkMetadata>();
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x001C7F34 File Offset: 0x001C6334
		public static bool IsObjectReady(GameObject obj)
		{
			if (obj == null)
			{
				return false;
			}
			NetworkMetadata component = obj.GetComponent<NetworkMetadata>();
			if (component == null)
			{
				VRCFlowManager.Instance.StartCoroutine(Network.CheckReady(obj));
			}
			return component != null && component.isReady;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x001C7F88 File Offset: 0x001C6388
		private static IEnumerator ReadyObjectEnumerator(GameObject root)
		{
			if (root == null)
			{
				yield break;
			}
			Queue<GameObject> queue = new Queue<GameObject>();
			queue.Enqueue(root);
			while (queue.Count > 0)
			{
				GameObject gameObject = queue.Dequeue();
				if (!(gameObject == null))
				{
					NetworkMetadata networkMetadata = gameObject.GetComponent<NetworkMetadata>();
					if (!(networkMetadata != null))
					{
						networkMetadata = gameObject.AddComponent<NetworkMetadata>();
						VRCFlowManager.Instance.StartCoroutine(networkMetadata.Ready());
						for (int i = 0; i < gameObject.transform.childCount; i++)
						{
							Transform child = gameObject.transform.GetChild(i);
							if (child != null && child.gameObject != null)
							{
								queue.Enqueue(child.gameObject);
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x001C7FA3 File Offset: 0x001C63A3
		public static IEnumerator CheckReady(GameObject obj)
		{
			return Network.ReadyObjectEnumerator(obj);
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x001C7FAC File Offset: 0x001C63AC
		public static void SendMessageToChildren(GameObject obj, string message, SendMessageOptions options = SendMessageOptions.RequireReceiver, object value = null)
		{
			VrcSdk2Interface instance = VrcSdk2Interface.Instance;
			instance.StartCoroutine(Network.SendMessageToChildrenInternal(instance, obj, message, options, value));
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x001C7FD0 File Offset: 0x001C63D0
		private static IEnumerator SendMessageToChildrenInternal(VrcSdk2Interface sdkInterface, GameObject obj, string message, SendMessageOptions options = SendMessageOptions.RequireReceiver, object value = null)
		{
			if (obj == null)
			{
				yield break;
			}
			yield return Network.CheckReady(obj);
			if (obj == null)
			{
				yield break;
			}
			List<GameObject> parents = new List<GameObject>
			{
				obj
			};
			List<GameObject> objs = new List<GameObject>();
			while (parents.Count > 0)
			{
				GameObject gameObject = parents[0];
				parents.RemoveAt(0);
				if (!(gameObject == null))
				{
					objs.Add(gameObject);
					int num = 0;
					while (gameObject != null && num < gameObject.transform.childCount)
					{
						if (gameObject.transform.GetChild(num) != null)
						{
							parents.Add(gameObject.transform.GetChild(num).gameObject);
						}
						num++;
					}
				}
			}
			foreach (GameObject gameObject2 in objs)
			{
				if (!(gameObject2 == null))
				{
					try
					{
						gameObject2.SendMessage(message, value, options);
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogException(exception, gameObject2);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x001C8004 File Offset: 0x001C6404
		public static void ProtectFromCleanup(GameObject obj, bool protect = true)
		{
			List<PhotonView> list = new List<PhotonView>();
			list.Add(obj.GetComponent<PhotonView>());
			list.AddRange(obj.GetComponentsInChildren<PhotonView>());
			foreach (PhotonView photonView in from x in list
			where x != null
			select x)
			{
				photonView.isProtectedFromCleanup = protect;
			}
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x001C8098 File Offset: 0x001C6498
		public static void TriggerEvent(VRC_EventHandler.VrcEvent e, VRC_EventHandler.VrcBroadcastType broadcast = VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered, int instagatorId = 0, float fastForward = 0f)
		{
			VRC_EventHandler sceneEventHandler = Network.SceneEventHandler;
			if (sceneEventHandler != null)
			{
				sceneEventHandler.TriggerEvent(e, broadcast, instagatorId, fastForward);
			}
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x001C80C4 File Offset: 0x001C64C4
		public static long? GetCombinedID(GameObject go)
		{
			if (go == null)
			{
				return null;
			}
			NetworkMetadata component = go.GetComponent<NetworkMetadata>();
			if (component != null && component.combinedID != null)
			{
				return component.combinedID;
			}
			int[] array = (from v in go.GetComponents<PhotonView>()
			select v.viewID).ToArray<int>();
			if (array.Any((int id) => id <= 0))
			{
				return null;
			}
			if (array.Length == 0)
			{
				return null;
			}
			int num = array[0];
			long num2 = (long)num << 32;
			return (num2 != 0L) ? new long?(num2) : null;
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x001C81AC File Offset: 0x001C65AC
		public static GameObject FindObjectByCombinedID(long id)
		{
			if (NetworkMetadata.ObjectCache.ContainsKey(id))
			{
				GameObject gameObject = NetworkMetadata.ObjectCache[id];
				if (gameObject == null)
				{
					NetworkMetadata.ObjectCache.Remove(id);
				}
				return gameObject;
			}
			return null;
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x001C81F0 File Offset: 0x001C65F0
		public static int CountSubIDs(GameObject obj, bool children = true)
		{
			return Network.FindSubIDs(obj, children).Length;
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x001C81FC File Offset: 0x001C65FC
		public static int[] FindSubIDs(GameObject obj, bool children = true)
		{
			List<PhotonView> source = new List<PhotonView>();
			Network.GetAllComponents<PhotonView>(obj, ref source, children);
			return (from v in source
			select v.viewID).ToArray<int>();
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x001C8240 File Offset: 0x001C6640
		public static int[] AllocateSubIDs(GameObject prefab, bool children = true)
		{
			if (prefab == null)
			{
				UnityEngine.Debug.LogError("Invalid parameters");
				return null;
			}
			if (PhotonNetwork.player == null || PhotonNetwork.player.ID <= 0)
			{
				UnityEngine.Debug.LogError("Bad network state");
				return null;
			}
			int viewCount = Network.CountSubIDs(prefab, children);
			return Network.AllocateSubIDs(viewCount);
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x001C829C File Offset: 0x001C669C
		public static int[] AllocateSubIDs(int viewCount)
		{
			int[] array = new int[viewCount];
			for (int i = 0; i < viewCount; i++)
			{
				try
				{
					array[i] = PhotonNetwork.AllocateViewID();
				}
				catch (Exception)
				{
					array[i] = -1;
				}
				if (array[i] <= 0)
				{
					UnityEngine.Debug.LogError("Unable to allocate sufficient network IDs");
					for (int j = 0; j < i; j++)
					{
						PhotonNetwork.UnAllocateViewID(array[j]);
					}
					return null;
				}
			}
			return array;
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x001C831C File Offset: 0x001C671C
		public static void UnAllocateSubIDs(int[] ids)
		{
			if (ids == null)
			{
				return;
			}
			foreach (int viewID in ids)
			{
				PhotonView photonView = PhotonView.Find(viewID);
				if (photonView != null)
				{
					PhotonNetwork.networkingPeer.LocalCleanPhotonView(photonView);
					photonView.viewID = 0;
				}
				PhotonNetwork.UnAllocateViewID(viewID);
			}
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x001C8378 File Offset: 0x001C6778
		public static bool AssignSubIDs(GameObject go, int[] ids, bool children = true)
		{
			if (go == null || ids == null)
			{
				UnityEngine.Debug.LogError("Invalid parameters");
				return false;
			}
			bool flag = ids.Any(delegate(int i)
			{
				PhotonView photonView = PhotonView.Find(i);
				return photonView != null && photonView.gameObject != go;
			});
			if (flag)
			{
				UnityEngine.Debug.LogError("Attempted to assign ids which are presently in use.\nThe ids are: " + string.Join(",", (from i in ids
				select i.ToString()).ToArray<string>()), go);
				return false;
			}
			List<PhotonView> list = new List<PhotonView>();
			Network.GetAllComponents<PhotonView>(go, ref list, children);
			if (list.Count != ids.Length)
			{
				UnityEngine.Debug.LogError(string.Format("Incorrect number of network IDs for object, expected {0} and found {1}.", ids.Length, list.Count), go);
				return false;
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].viewID = ids[j];
			}
			return true;
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x001C848C File Offset: 0x001C688C
		public static void RPC(Player targetPlayer, GameObject targetObject, string methodName, params object[] parameters)
		{
			if (targetPlayer == null)
			{
				UnityEngine.Debug.LogError("Target player cannot be null.");
				return;
			}
			if (targetObject == null)
			{
				UnityEngine.Debug.LogError("Target object cannot be null.");
				return;
			}
			if (Network.LocalInstigatorID <= 0)
			{
				UnityEngine.Debug.LogError("Discarding RPC because Local Instigator <= 0, " + methodName + " on " + targetObject.name);
				return;
			}
			if (parameters == null || parameters.Length == 0)
			{
				parameters = new object[]
				{
					targetPlayer.GetPhotonPlayerId()
				};
			}
			else
			{
				parameters = new List<object>(parameters)
				{
					targetPlayer.GetPhotonPlayerId()
				}.ToArray();
			}
			VRC_EventHandler vrc_EventHandler = Network.FindNearestEventHandler(targetObject);
			if (vrc_EventHandler == null)
			{
				UnityEngine.Debug.LogError("Discarding RPC because there was no good event handler, " + methodName + " on " + targetObject.name);
				return;
			}
			byte[] array = VRC_Serialization.ParameterEncoder(parameters);
			if (array == null)
			{
				UnityEngine.Debug.LogError("Discarding RPC because parameters could not encode, " + methodName + " on " + targetObject.name);
				return;
			}
			VRC_EventHandler.VrcEvent e = new VRC_EventHandler.VrcEvent
			{
				EventType = VRC_EventHandler.VrcEventType.SendRPC,
				ParameterObject = targetObject,
				ParameterString = methodName,
				ParameterBytes = array,
				ParameterInt = 9
			};
			vrc_EventHandler.TriggerEvent(e, VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered, Network.LocalInstigatorID, 0f);
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x001C85D8 File Offset: 0x001C69D8
		public static void RPC(VRC_EventHandler.VrcTargetType targetClients, GameObject targetObject, string methodName, params object[] parameters)
		{
			if (parameters == null)
			{
				parameters = new object[0];
			}
			VRC_EventHandler vrc_EventHandler = Network.FindNearestEventHandler(targetObject);
			if (vrc_EventHandler == null)
			{
				UnityEngine.Debug.LogError("Could not find an event handler to process " + methodName + " on " + targetObject.name);
				return;
			}
			byte[] array = VRC_Serialization.ParameterEncoder(parameters);
			if (array == null)
			{
				UnityEngine.Debug.LogError("Could not encode parameters for processing " + methodName + " on " + targetObject.name);
				return;
			}
			VRC_EventHandler.VrcEvent e = new VRC_EventHandler.VrcEvent
			{
				EventType = VRC_EventHandler.VrcEventType.SendRPC,
				ParameterObject = targetObject,
				ParameterString = methodName,
				ParameterBytes = array,
				ParameterInt = (int)targetClients
			};
			int localInstigatorID = Network.LocalInstigatorID;
            if (localInstigatorID <= 0)
			{
				UnityEngine.Debug.LogError("Local instigator was invalid when trying to process " + methodName + " on " + targetObject.name);
				return;
			}
			switch (targetClients)
			{
			case VRC_EventHandler.VrcTargetType.All:
			case VRC_EventHandler.VrcTargetType.Others:
			case VRC_EventHandler.VrcTargetType.Owner:
			case VRC_EventHandler.VrcTargetType.Master:
                    vrc_EventHandler.TriggerEvent(e, VRC_EventHandler.VrcBroadcastType.AlwaysUnbuffered, localInstigatorID, 0f);
                    break;
			case VRC_EventHandler.VrcTargetType.AllBuffered:
			case VRC_EventHandler.VrcTargetType.OthersBuffered:
				vrc_EventHandler.TriggerEvent(e, VRC_EventHandler.VrcBroadcastType.Always, localInstigatorID, 0f);
				break;
			case VRC_EventHandler.VrcTargetType.Local:
				vrc_EventHandler.TriggerEvent(e, VRC_EventHandler.VrcBroadcastType.Local, localInstigatorID, 0f);
				break;
			case VRC_EventHandler.VrcTargetType.AllBufferOne:
			case VRC_EventHandler.VrcTargetType.OthersBufferOne:
				vrc_EventHandler.TriggerEvent(e, VRC_EventHandler.VrcBroadcastType.AlwaysBufferOne, localInstigatorID, 0f);
				break;
			}
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x001C8724 File Offset: 0x001C6B24
		public static void Message(VRC_EventHandler.VrcBroadcastType broadcast, GameObject targetObject, string methodName)
		{
			VRC_EventHandler vrc_EventHandler = Network.FindNearestEventHandler(targetObject);
			if (vrc_EventHandler == null)
			{
				UnityEngine.Debug.LogError("Could not locate scene event handler");
				return;
			}
			VRC_EventHandler.VrcEvent e = new VRC_EventHandler.VrcEvent
			{
				EventType = VRC_EventHandler.VrcEventType.SendMessage,
				ParameterObject = targetObject,
				ParameterString = methodName
			};
			vrc_EventHandler.TriggerEvent(e, broadcast, 0, 0f);
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x0600531A RID: 21274 RVA: 0x001C877C File Offset: 0x001C6B7C
		public static VRC_EventDispatcher Dispatcher
		{
			get
			{
				GameObject gameObject = GameObject.Find("/VRC_OBJECTS/Dispatcher");
				if (gameObject == null)
				{
					return null;
				}
				return gameObject.GetComponent<VRC_EventDispatcher>();
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x0600531B RID: 21275 RVA: 0x001C87A8 File Offset: 0x001C6BA8
		// (set) Token: 0x0600531C RID: 21276 RVA: 0x001C87AF File Offset: 0x001C6BAF
		public static VRC_EventHandler SceneEventHandler { get; set; }

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x0600531D RID: 21277 RVA: 0x001C87B7 File Offset: 0x001C6BB7
		// (set) Token: 0x0600531E RID: 21278 RVA: 0x001C87BE File Offset: 0x001C6BBE
		public static ObjectInstantiator SceneInstantiator { get; set; }

		// Token: 0x0600531F RID: 21279 RVA: 0x001C87C8 File Offset: 0x001C6BC8
		public static void Destroy(GameObject obj)
		{
			if (obj == null)
			{
				return;
			}
			VRC_EventHandler.VrcEvent e = new VRC_EventHandler.VrcEvent
			{
				EventType = VRC_EventHandler.VrcEventType.DestroyObject,
				Name = "Destroy",
				ParameterObject = obj
			};
			Network.TriggerEvent(e, VRC_EventHandler.VrcBroadcastType.Always, 0, 0f);
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x001C8811 File Offset: 0x001C6C11
		public static GameObject Instantiate(VRC_EventHandler.VrcBroadcastType broadcast, string prefabPathOrDynamicPrefabName, Vector3 position, Quaternion rotation)
		{
			return Network.Instantiate(broadcast, prefabPathOrDynamicPrefabName, position, rotation, null);
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x001C8820 File Offset: 0x001C6C20
		public static GameObject Instantiate(VRC_EventHandler.VrcBroadcastType broadcast, string prefabPathOrDynamicPrefabName, Vector3 position, Quaternion rotation, ObjectInstantiator instantiator)
		{
			VRC_SceneDescriptor instance = VRC_SceneDescriptor.Instance;
			if (instance == null)
			{
				UnityEngine.Debug.LogError("Could not locate scene descriptor.");
				return null;
			}
			if (instantiator == null)
			{
				instantiator = Network.SceneInstantiator;
			}
			if (instantiator == null)
			{
				UnityEngine.Debug.LogError("Scene is unable to instantiate objects.");
				return null;
			}
			return instantiator.InstantiateObject(broadcast, prefabPathOrDynamicPrefabName, position, rotation);
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x001C8884 File Offset: 0x001C6C84
		public static bool InstigatorIsModerator(int instigatorID)
		{
			Player playerByInstigatorID = Network.GetPlayerByInstigatorID(instigatorID);
			return !(playerByInstigatorID == null) && playerByInstigatorID.isModerator;
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x001C88B0 File Offset: 0x001C6CB0
		public static VRC_EventHandler FindNearestEventHandler(GameObject target)
		{
			VRC_EventHandler vrc_EventHandler = null;
			if (target != null)
			{
				vrc_EventHandler = target.GetComponent<VRC_EventHandler>();
				if (vrc_EventHandler == null)
				{
					vrc_EventHandler = target.GetComponentInParent<VRC_EventHandler>();
				}
			}
			if (vrc_EventHandler == null)
			{
				vrc_EventHandler = Network.SceneEventHandler;
			}
			return vrc_EventHandler;
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x001C88F8 File Offset: 0x001C6CF8
		public static string GetUniqueName(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			string text = Network.GetGameObjectPath(obj);
			if (text == null)
			{
				return null;
			}
			if (RoomManager.currentRoom != null)
			{
				string id = RoomManager.currentRoom.id;
				string currentInstanceIdWithTags = RoomManager.currentRoom.currentInstanceIdWithTags;
				text = string.Concat(new string[]
				{
					id,
					".",
					currentInstanceIdWithTags,
					":",
					text
				});
			}
			SHA256 sha = SHA256.Create();
			byte[] inArray = sha.ComputeHash(Encoding.Default.GetBytes(text));
			return Uri.EscapeDataString(Convert.ToBase64String(inArray));
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x001C8990 File Offset: 0x001C6D90
		public static string GetGameObjectPath(GameObject go)
		{
			if (Network.Dispatcher == null || go == null)
			{
				return null;
			}
			VRC_EventDispatcherRFC component = Network.Dispatcher.GetComponent<VRC_EventDispatcherRFC>();
			return component.GetGameObjectPath(go);
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x001C89D0 File Offset: 0x001C6DD0
		public static GameObject FindGameObject(string path, bool suppressErrors = false)
		{
			VRC_EventDispatcherRFC instance = VRC_EventDispatcherRFC.Instance;
			if (instance == null)
			{
				return null;
			}
			return instance.FindGameObject(path, suppressErrors);
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x001C89FC File Offset: 0x001C6DFC
		public static IEnumerable<T> GetAllComponents<T>(GameObject obj, bool children = true) where T : class
		{
			List<T> result = new List<T>();
			Network.GetAllComponents<T>(obj, ref result, children);
			return result;
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x001C8A1C File Offset: 0x001C6E1C
		private static void GetAllComponents<T>(GameObject obj, ref List<T> list, bool children = true) where T : class
		{
			if (obj != null)
			{
				Component[] components = obj.GetComponents(typeof(T));
				foreach (Component component in components)
				{
					if (component != null)
					{
						list.Add(component as T);
					}
				}
				if (children)
				{
					for (int j = 0; j < obj.transform.childCount; j++)
					{
						Network.GetAllComponents<T>(obj.transform.GetChild(j).gameObject, ref list, true);
					}
				}
			}
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x001C8AC4 File Offset: 0x001C6EC4
		public static void AssignNetworkIDsToScene()
		{
			List<GameObject> list = AssetManagement.RootGameObjects.ToList<GameObject>();
			list.Sort((GameObject a, GameObject b) => a.name.CompareTo(b.name));
			int num = 10;
			foreach (GameObject obj in list)
			{
				Network.AssignNetworkIDsToObject(obj, true, 0, ref num);
			}
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x001C8B50 File Offset: 0x001C6F50
		public static void AssignNetworkIDsToObject(GameObject obj, bool viewIDs, int ownerId, ref int id)
		{
			if (obj == null)
			{
				return;
			}
			int num = ownerId * 1000 + id;
			INetworkID[] components = obj.GetComponents<INetworkID>();
			PhotonView component = obj.GetComponent<PhotonView>();
			VRCPunBehaviour[] array = obj.GetComponents<VRCPunBehaviour>().ToArray<VRCPunBehaviour>();
			if (component != null || components.Length > 0 || array.Length > 0)
			{
				foreach (VRCPunBehaviour vrcpunBehaviour in array)
				{
					vrcpunBehaviour.ReservedID = num;
				}
				foreach (INetworkID networkID in components)
				{
					networkID.NetworkID = num;
				}
				if (component != null)
				{
					if (PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num))
					{
						PhotonView photonView = PhotonNetwork.networkingPeer.photonViewList[num];
						if (photonView != null)
						{
							photonView.viewID = 0;
						}
						PhotonNetwork.networkingPeer.photonViewList.Remove(num);
					}
					PhotonNetwork.UnAllocateViewID(num);
					component.viewID = num;
				}
				id++;
			}
			List<GameObject> list = new List<GameObject>();
			for (int k = 0; k < obj.transform.childCount; k++)
			{
				if (obj.transform.GetChild(k).gameObject != null)
				{
					list.Add(obj.transform.GetChild(k).gameObject);
				}
			}
			list.Sort((GameObject a, GameObject b) => a.name.CompareTo(b.name));
			foreach (GameObject obj2 in list)
			{
				Network.AssignNetworkIDsToObject(obj2, viewIDs, ownerId, ref id);
			}
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x001C8D40 File Offset: 0x001C7140
		public static void ProcessRPC(VRC_EventHandler.VrcTargetType targetTypeInt, Player instigating_player, GameObject targetObject, string rpcMethodName, byte[] dataParameters)
		{
			if (instigating_player == null || !instigating_player.isLocal)
			{
				PhotonBandwidthGui.RecordRPC(rpcMethodName, (dataParameters != null) ? dataParameters.Length : 0);
			}
			VRC_EventHandler.VrcTargetType targetType = targetTypeInt;
			object[] array = VRC_Serialization.ParameterDecoder(dataParameters, false);
			switch (targetType)
			{
			case VRC_EventHandler.VrcTargetType.Others:
			case VRC_EventHandler.VrcTargetType.OthersBuffered:
			case VRC_EventHandler.VrcTargetType.OthersBufferOne:
				if (instigating_player == null || instigating_player.isLocal)
				{
					return;
				}
				break;
			case VRC_EventHandler.VrcTargetType.Owner:
				if (targetObject != null && !Networking.IsOwner(targetObject))
				{
					return;
				}
				break;
			case VRC_EventHandler.VrcTargetType.Master:
				if (!Networking.IsMaster)
				{
					return;
				}
				break;
			case VRC_EventHandler.VrcTargetType.Local:
				if (instigating_player == null || !instigating_player.isLocal)
				{
					return;
				}
				break;
			case VRC_EventHandler.VrcTargetType.TargetPlayer:
			{
				Player playerByInstigatorID = Network.GetPlayerByInstigatorID((int)array[array.Length - 1]);
				array = array.Take(array.Length - 1).ToArray<object>();
				if (playerByInstigatorID == null || !playerByInstigatorID.isLocal)
				{
					return;
				}
				break;
			}
			}
			if (targetObject == null)
			{
				UnityEngine.Debug.LogError(string.Concat(new string[]
				{
					"RPC ",
					rpcMethodName,
					"/",
					targetType.ToString(),
					" could not find target"
				}));
				return;
			}
			Component[] components = targetObject.GetComponents<Component>();
			if (components == null || components.Length == 0)
			{
				UnityEngine.Debug.LogError("No potential RPC receivers on target object " + targetObject.name);
				return;
			}
			if (array == null)
			{
				return;
			}
			Type[] array2 = new Type[array.Length];
			Type[] array3 = new Type[array2.Length + 1];
			Type[] array4 = new Type[array2.Length + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = ((array[i] != null) ? array[i].GetType() : null);
				array3[i] = array2[i];
				array4[i] = array2[i];
			}
			array3[array2.Length] = typeof(int);
			array4[array2.Length] = typeof(Player);
			Type[][] target_types = new Type[][]
			{
				array2,
				array3,
				array4
			};
			Func<ParameterInfo[], Type[]> func = delegate(ParameterInfo[] paramInfos)
			{
				foreach (Type[] array10 in target_types)
				{
					if (paramInfos.Length == array10.Length)
					{
						int n;
						for (n = 0; n < paramInfos.Length; n++)
						{
							if (paramInfos[n].ParameterType != array10[n] && (array10[n] != null || paramInfos[n].ParameterType.IsValueType))
							{
								break;
							}
						}
						if (n == paramInfos.Length)
						{
							return array10;
						}
					}
				}
				return null;
			};
			Func<MethodInfo, bool> func2 = (MethodInfo method) => PluginManager.IsPluginAssembly(method.DeclaringType.Assembly) || method.GetCustomAttributes(false).Any((object a) => a != null && ((a is VRCSDK2.RPC && (a as VRCSDK2.RPC).allowedTargets.Contains(targetType)) || (a is VRCSDK2.RPC && (a as VRCSDK2.RPC).allowedTargets.Contains(targetType))));
			int num = 0;
			List<MethodInfo> list = new List<MethodInfo>(components.Length * 2);
			List<MethodInfo> list2 = new List<MethodInfo>(components.Length * 2);
 
			foreach (Component component in components)
			{
				if (!(component == null))
				{
					Type type = component.GetType();
					MethodInfo[] array6;
					if (Network.cachedMethodInfo.ContainsKey(type))
					{
						array6 = Network.cachedMethodInfo[type];
					}
					else
					{
						array6 = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						Network.cachedMethodInfo.Add(type, array6);
					}
                    if (array6.Length == 0)
                    {
                        UnityEngine.Debug.Log("Empty method array.");
                    }
					foreach (MethodInfo methodInfo in array6)
					{
						if (!(methodInfo.Name != rpcMethodName))
						{
							ParameterInfo[] parameters = methodInfo.GetParameters();
							Type[] array8 = func(parameters);
							if (array8 == null)
							{
								list.Add(methodInfo);
							}
                            else if (!func2(methodInfo))
                            {
                                list.Add(methodInfo);
                            }
							else
							{
								try
								{
									string format = string.Concat(new string[]
									{
										"<color=cyan>Executing RPC ",
										rpcMethodName,
										" on ",
										component.gameObject.name,
										".",
										type.Name,
										" triggered by ",
										(!(instigating_player != null)) ? "someone" : instigating_player.name.ToString(),
										" for ",
										targetType.ToString(),
										" path ",
										Network.GetGameObjectPath(targetObject),
										"</color>"
									});
									component.gameObject.DebugPrint(format, new object[0]);
									object[] array9;
									if (parameters.Length == array.Length)
									{
										array9 = array;
									}
									else
									{
										array9 = new object[array.Length + 1];
										for (int l = 0; l < array.Length; l++)
										{
											array9[l] = array[l];
										}
										Type type2 = array8[array8.Length - 1];
										if (type2 == typeof(int))
										{
											array9[array.Length] = Network.GetInstigatorID(instigating_player);
										}
										else
										{
											array9[array.Length] = instigating_player;
										}
									}
									object obj = methodInfo.Invoke(component, array9);
									if (obj != null && component is MonoBehaviour && methodInfo.ReturnType == typeof(IEnumerator))
									{
										(component as MonoBehaviour).StartCoroutine((IEnumerator)obj);
									}
								}
								catch (Exception ex)
								{
									UnityEngine.Debug.LogError("User code has thrown an exception, when calling " + rpcMethodName + " on " + component.gameObject.name);
									UnityEngine.Debug.LogError(ex.Message);
									UnityEngine.Debug.LogException((ex.InnerException == null) ? ex : ex.InnerException, component.gameObject);
								}
								num++;
							}
						}
					}
				}
			}
			if (num == 0)
			{
				string str = string.Join(", ", array2.Aggregate(new List<string>(), delegate(List<string> a, Type t)
				{
					a.Add(t.Name);
					return a;
				}).ToArray());
				string text = rpcMethodName + "(" + str + ")";
				if (list.Count > 0)
				{
					string text2 = string.Empty;
					foreach (MethodInfo methodInfo2 in list)
					{
						text2 = text2 + "\n" + methodInfo2.ToString();
					}
					UnityEngine.Debug.LogError(string.Concat(new string[]
					{
						"Could not find any components capable of receiving RPC \"",
						text,
						"\" on object \"",
						targetObject.name,
						"\" with path \"",
						Network.GetGameObjectPath(targetObject),
						"\" and targets \"",
						targetType.ToString(),
						"\" but found close matches with unsuitable parameters: ",
						text2
					}));
				}
				if (list2.Count > 0)
				{
					string text3 = string.Empty;
					foreach (MethodInfo methodInfo3 in list2)
					{
						text3 = text3 + "\n" + methodInfo3.ToString();
						foreach (object obj2 in from a in methodInfo3.GetCustomAttributes(false)
						where a is VRCSDK2.RPC
						select a)
						{
							VRCSDK2.RPC rpc = (VRCSDK2.RPC)obj2;
							text3 = text3 + "\n    [" + string.Join(", ", rpc.allowedTargets.Select((VRC_EventHandler.VrcTargetType tt) => tt.ToString()).ToArray<string>()) + "]";
						}
					}
					UnityEngine.Debug.LogError(string.Concat(new string[]
					{
						"Could not find any components capable of receiving RPC \"",
						text,
						"\" on object \"",
						targetObject.name,
						"\" with path \"",
						Network.GetGameObjectPath(targetObject),
						"\" and targets \"",
						targetType.ToString(),
						"\" but found close matches which need a VRCSDK2.RPC attribute: ",
						text3
					}));
				}
				if (list2.Count == 0 && list.Count == 0)
				{
					UnityEngine.Debug.LogError(string.Concat(new string[]
					{
						"Could not find any components capable of receiving RPC \"",
						text,
						"\" on object \"",
						targetObject.name,
						"\" with path \"",
						Network.GetGameObjectPath(targetObject),
						"\""
					}));
				}
			}
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x001C9614 File Offset: 0x001C7A14
		public static void CloseConnection(Player player)
		{
			if (player != null)
			{
				PhotonNetwork.CloseConnection(PhotonPlayer.Find(player.GetPhotonPlayerId()));
			}
		}

		// Token: 0x0600532D RID: 21293 RVA: 0x001C9633 File Offset: 0x001C7A33
		public static void CloseConnection(PhotonPlayer player)
		{
			if (player != null)
			{
				PhotonNetwork.CloseConnection(player);
			}
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x001C9642 File Offset: 0x001C7A42
		public static void SetMasterClient(Player player)
		{
			if (player != null)
			{
				PhotonNetwork.SetMasterClient(PhotonPlayer.Find(player.GetPhotonPlayerId()));
			}
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x001C9661 File Offset: 0x001C7A61
		public static double GetServerTimeInSeconds()
		{
			return PhotonNetwork.time;
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x001C9668 File Offset: 0x001C7A68
		public static int GetServerTimeInMilliseconds()
		{
			return PhotonNetwork.ServerTimestamp;
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x001C9670 File Offset: 0x001C7A70
		public static double CalculateServerDeltaTime(double timeInSeconds, double previousTimeInSeconds)
		{
			double num = timeInSeconds - previousTimeInSeconds;
			if (num < -2147483.6475)
			{
				num = timeInSeconds + (4294967.295 - previousTimeInSeconds);
			}
			return num;
		}

		// Token: 0x06005332 RID: 21298 RVA: 0x001C96A0 File Offset: 0x001C7AA0
		public static DateTime GetNetworkDateTime()
		{
			object @lock = Network._lock;
			float syncTimeAfterStartup;
			DateTime networkDateTime;
			lock (@lock)
			{
				syncTimeAfterStartup = Network._syncTimeAfterStartup;
				networkDateTime = Network._networkDateTime;
			}
			if (syncTimeAfterStartup < 0f)
			{
				return DateTime.UtcNow;
			}
			return networkDateTime.AddSeconds((double)(Time.realtimeSinceStartup - syncTimeAfterStartup));
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x001C9704 File Offset: 0x001C7B04
		public static void SyncNetworkDateTime()
		{
			string ntpServer = RemoteConfig.GetString("ntpServerUrl");
			if (string.IsNullOrEmpty(ntpServer))
			{
				ntpServer = "pool.ntp.org";
			}
			float syncStartTime = Time.realtimeSinceStartup;
			new Thread(() =>
			{
				Network.SyncNetworkDateTimeThread(ntpServer, syncStartTime);
			})
			{
				Priority = System.Threading.ThreadPriority.BelowNormal,
				IsBackground = true
			}.Start();
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x001C9774 File Offset: 0x001C7B74
		private static void SyncNetworkDateTimeThread(string ntpServer, float syncStartTime)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			byte[] array = new byte[48];
			array[0] = 27;
			DateTime networkDateTime;
			try
			{
				IPAddress[] addressList = Dns.GetHostEntry(ntpServer).AddressList;
				IPEndPoint remoteEP = new IPEndPoint(addressList[0], 123);
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				socket.Connect(remoteEP);
				socket.Send(array);
				socket.Receive(array);
				socket.Close();
				ulong num = (ulong)array[40] << 24 | (ulong)array[41] << 16 | (ulong)array[42] << 8 | (ulong)array[43];
				ulong num2 = (ulong)array[44] << 24 | (ulong)array[45] << 16 | (ulong)array[46] << 8 | (ulong)array[47];
				ulong num3 = num * 1000UL + num2 * 1000UL / 4294967296UL;
				DateTime dateTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				networkDateTime = dateTime.AddMilliseconds((double)num3);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Failed to sync network DateTime: " + ex.ToString());
				return;
			}
			stopwatch.Stop();
			object @lock = Network._lock;
			lock (@lock)
			{
				Network._networkDateTime = networkDateTime;
				Network._syncTimeAfterStartup = (float)((double)syncStartTime + stopwatch.Elapsed.TotalSeconds);
			}
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Current Date and Time is: ",
				networkDateTime.ToString(),
				" ",
				networkDateTime.Kind,
				", sync took ",
				stopwatch.Elapsed.TotalSeconds,
				"s"
			}));
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x001C9940 File Offset: 0x001C7D40
		public static IEnumerator ConfigureEventHandler(VRC_EventHandler evtHandler)
		{
			if (evtHandler == null || evtHandler._registered)
			{
				yield break;
			}
			GameObject dispatcherObject = GameObject.Find("/VRC_OBJECTS/Dispatcher");
			while (evtHandler != null && dispatcherObject == null)
			{
				yield return null;
				dispatcherObject = GameObject.Find("/VRC_OBJECTS/Dispatcher");
			}
			if (evtHandler == null)
			{
				yield break;
			}
			long? newID = Network.GetCombinedID(evtHandler.gameObject);
			evtHandler.CombinedNetworkId = ((newID == null) ? 0L : newID.Value);
			if (evtHandler != null && !evtHandler._registered && dispatcherObject != null)
			{
				evtHandler._dispatcher = dispatcherObject.GetComponent<VRC_EventDispatcherRFC>();
				evtHandler._registered = true;
				evtHandler.SetReady(true);
			}
			yield break;
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x001C995C File Offset: 0x001C7D5C
		public static bool ValidateCompletedHTTPRequest(HTTPRequest request, Action<HTTPResponse> onSuccess = null, Action<string> onError = null)
		{
			if (request == null)
			{
				if (onError != null)
				{
					onError("Request is null.");
				}
				return false;
			}
			string originalString = request.Uri.OriginalString;
			switch (request.State)
			{
			case HTTPRequestStates.Queued:
				if (onError != null)
				{
					onError("Queued request.");
				}
				return false;
			case HTTPRequestStates.Finished:
			{
				if (request.Response.IsSuccess)
				{
					if (onSuccess != null)
					{
						onSuccess(request.Response);
					}
					return true;
				}
				string obj = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", request.Response.StatusCode, request.Response.Message, request.Response.DataAsText);
				if (onError != null)
				{
					onError(obj);
				}
				return false;
			}
			case HTTPRequestStates.Error:
				if (onError != null)
				{
					onError("Request finished with error: " + ((request.Exception == null) ? "(no exception)" : request.Exception.Message));
				}
				return false;
			case HTTPRequestStates.Aborted:
				if (onError != null)
				{
					onError("Download aborted. Server closed the connection.");
				}
				return false;
			}
			if (onError != null)
			{
				onError("Error - request state is: " + request.State);
			}
			return false;
		}

		// Token: 0x04003A88 RID: 14984
		private static Dictionary<Type, MethodInfo[]> cachedMethodInfo = new Dictionary<Type, MethodInfo[]>();

		// Token: 0x04003A89 RID: 14985
		private static DateTime _networkDateTime;

		// Token: 0x04003A8A RID: 14986
		private static float _syncTimeAfterStartup = -1f;

		// Token: 0x04003A8B RID: 14987
		private static object _lock = new object();

		// Token: 0x02000AB2 RID: 2738
		public enum OwnershipModificationType
		{
			// Token: 0x04003A9A RID: 15002
			Local,
			// Token: 0x04003A9B RID: 15003
			Request,
			// Token: 0x04003A9C RID: 15004
			Collision,
			// Token: 0x04003A9D RID: 15005
			Pickup
		}
	}
}
