using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000AA4 RID: 2724
public class InternalSDKPlayer : MonoBehaviour
{
	// Token: 0x060051DA RID: 20954 RVA: 0x001C0850 File Offset: 0x001BEC50
	private void Awake()
	{
		if (InternalSDKPlayer.AllPlayers == null)
		{
			InternalSDKPlayer.AllPlayers = new List<VRC_PlayerApi>();
		}
		InternalSDKPlayer.AllPlayers.Add(base.GetComponent<VRC_PlayerApi>());
	}

	// Token: 0x060051DB RID: 20955 RVA: 0x001C0876 File Offset: 0x001BEC76
	private void OnDestroy()
	{
		InternalSDKPlayer.AllPlayers.Remove(base.GetComponent<VRC_PlayerApi>());
	}

	// Token: 0x060051DC RID: 20956 RVA: 0x001C0889 File Offset: 0x001BEC89
	public static VRC_PlayerApi GetPlayerByGameObject(GameObject playerGameObject)
	{
		return playerGameObject.GetComponent<VRC_PlayerApi>();
	}

	// Token: 0x060051DD RID: 20957 RVA: 0x001C0894 File Offset: 0x001BEC94
	public static VRC_PlayerApi GetPlayerById(int playerId)
	{
		VRC.Player playerByInstigatorID = VRC.Network.GetPlayerByInstigatorID(playerId);
		return (!(playerByInstigatorID == null)) ? playerByInstigatorID.GetComponent<VRC_PlayerApi>() : null;
	}

	// Token: 0x060051DE RID: 20958 RVA: 0x001C08C0 File Offset: 0x001BECC0
	public static int GetPlayerId(VRC_PlayerApi player)
	{
		return (!(player == null)) ? VRC.Network.GetInstigatorID(player.GetComponent<VRC.Player>()) : -1;
	}

	// Token: 0x060051DF RID: 20959 RVA: 0x001C08DF File Offset: 0x001BECDF
	public static bool IsOwner(VRC_PlayerApi player, GameObject obj)
	{
		return VRC.Network.IsOwner(player.GetComponent<VRC.Player>(), obj);
	}

	// Token: 0x060051E0 RID: 20960 RVA: 0x001C08ED File Offset: 0x001BECED
	public static void TakeOwnership(VRC_PlayerApi player, GameObject obj)
	{
		VRC.Network.SetOwner(player.GetComponent<VRC.Player>(), obj, VRC.Network.OwnershipModificationType.Request, true);
	}

	// Token: 0x060051E1 RID: 20961 RVA: 0x001C0900 File Offset: 0x001BED00
	public static VRC_PlayerApi.TrackingData GetTrackingData(VRC_PlayerApi player, VRC_PlayerApi.TrackingDataType trackedDataType)
	{
		Transform transform = null;
		if (trackedDataType != VRC_PlayerApi.TrackingDataType.Head)
		{
			if (trackedDataType != VRC_PlayerApi.TrackingDataType.LeftHand)
			{
				if (trackedDataType == VRC_PlayerApi.TrackingDataType.RightHand)
				{
					if (player.isLocal)
					{
						transform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.HandTracker_RightPalm);
					}
					else
					{
						transform = InternalSDKPlayer.GetBoneTransform(player, HumanBodyBones.RightHand);
					}
				}
			}
			else if (player.isLocal)
			{
				transform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.HandTracker_LeftPalm);
			}
			else
			{
				transform = InternalSDKPlayer.GetBoneTransform(player, HumanBodyBones.LeftHand);
			}
		}
		else if (player.isLocal)
		{
			transform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.Hmd);
		}
		else
		{
			transform = InternalSDKPlayer.GetBoneTransform(player, HumanBodyBones.Head);
		}
		return (!(transform == null)) ? new VRC_PlayerApi.TrackingData(transform.position, transform.rotation) : new VRC_PlayerApi.TrackingData(Vector3.zero, Quaternion.identity);
	}

	// Token: 0x060051E2 RID: 20962 RVA: 0x001C09CC File Offset: 0x001BEDCC
	public static Transform GetTrackedTransform(VRC_PlayerApi player, VRC_PlayerApi.TrackingDataType tt)
	{
		if (player == null)
		{
			return null;
		}
		Transform result = null;
		if (tt != VRC_PlayerApi.TrackingDataType.Head)
		{
			if (tt != VRC_PlayerApi.TrackingDataType.LeftHand)
			{
				if (tt == VRC_PlayerApi.TrackingDataType.RightHand)
				{
					IkController componentInChildren = player.GetComponentInChildren<IkController>();
					if (componentInChildren != null)
					{
						result = componentInChildren.RightEffector.transform;
					}
				}
			}
			else
			{
				IkController componentInChildren = player.GetComponentInChildren<IkController>();
				if (componentInChildren != null)
				{
					result = componentInChildren.LeftEffector.transform;
				}
			}
		}
		else if (player.isLocal)
		{
			VRCTrackingManager vrctrackingManager = UnityEngine.Object.FindObjectOfType<VRCTrackingManager>();
			if (vrctrackingManager != null)
			{
				Camera componentInChildren2 = vrctrackingManager.GetComponentInChildren<Camera>();
				if (componentInChildren2 != null)
				{
					result = componentInChildren2.transform;
				}
			}
		}
		return result;
	}

	// Token: 0x060051E3 RID: 20963 RVA: 0x001C0A90 File Offset: 0x001BEE90
	public static Transform GetBoneTransform(VRC_PlayerApi player, HumanBodyBones humanBoneId)
	{
		Transform result = null;
		Animator componentInChildren = player.GetComponentInChildren<Animator>();
		if (componentInChildren != null)
		{
			result = componentInChildren.GetBoneTransform(humanBoneId);
		}
		return result;
	}

	// Token: 0x060051E4 RID: 20964 RVA: 0x001C0ABC File Offset: 0x001BEEBC
	private static VRCHandGrasper[] GetGraspers(VRC_PlayerApi player)
	{
		VRCHandGrasper[] array = new VRCHandGrasper[2];
		VRCHandGrasper[] componentsInChildren = player.GetComponentsInChildren<VRCHandGrasper>();
		array = new VRCHandGrasper[2];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].RightHand)
			{
				array[1] = componentsInChildren[i];
			}
			else
			{
				array[0] = componentsInChildren[i];
			}
		}
		return array;
	}

	// Token: 0x060051E5 RID: 20965 RVA: 0x001C0B14 File Offset: 0x001BEF14
	public static VRC_Pickup GetPickupInHand(VRC_PlayerApi player, VRC_Pickup.PickupHand hand)
	{
		VRC_Pickup result = null;
		VRCHandGrasper[] graspers = InternalSDKPlayer.GetGraspers(player);
		if (graspers.Length == 2)
		{
			if (hand != VRC_Pickup.PickupHand.Right)
			{
				if (hand == VRC_Pickup.PickupHand.Left)
				{
					result = graspers[0].GetGraspedPickup();
				}
			}
			else
			{
				result = graspers[1].GetGraspedPickup();
			}
		}
		return result;
	}

	// Token: 0x060051E6 RID: 20966 RVA: 0x001C0B63 File Offset: 0x001BEF63
	public static void SetPickupInHand(VRC_PlayerApi player, VRC_Pickup pickup, VRC_Pickup.PickupHand hand)
	{
	}

	// Token: 0x060051E7 RID: 20967 RVA: 0x001C0B68 File Offset: 0x001BEF68
	public static void PlayHapticEventInHand(VRC_PlayerApi player, VRC_Pickup.PickupHand hand, float duration, float amplitude, float frequency)
	{
		VRCHandGrasper[] graspers = InternalSDKPlayer.GetGraspers(player);
		if (graspers.Length == 2)
		{
			if (hand != VRC_Pickup.PickupHand.Right)
			{
				if (hand == VRC_Pickup.PickupHand.Left)
				{
					if (graspers[0] != null)
					{
						graspers[0].HapticEvent(duration, amplitude, frequency);
					}
				}
			}
			else if (graspers[1] != null)
			{
				graspers[1].HapticEvent(duration, amplitude, frequency);
			}
		}
	}

	// Token: 0x060051E8 RID: 20968 RVA: 0x001C0BD6 File Offset: 0x001BEFD6
	public static void EnablePickups(VRC_PlayerApi player, bool enable)
	{
		player.GetComponent<VRCPlayer>().canPickupObjects = enable;
	}

	// Token: 0x060051E9 RID: 20969 RVA: 0x001C0BE4 File Offset: 0x001BEFE4
	public static void SetNamePlateColor(VRC_PlayerApi player, Color col)
	{
		player.GetComponent<VRCPlayer>().SetNamePlateColor(col);
	}

	// Token: 0x060051EA RID: 20970 RVA: 0x001C0BF2 File Offset: 0x001BEFF2
	public static void RestoreNamePlateColor(VRC_PlayerApi player)
	{
		player.GetComponent<VRCPlayer>().RestoreNamePlateColor();
	}

	// Token: 0x060051EB RID: 20971 RVA: 0x001C0BFF File Offset: 0x001BEFFF
	public static void SetNamePlateVisibility(VRC_PlayerApi player, bool flag)
	{
		player.GetComponent<VRCPlayer>().SetNamePlateVisibility(flag);
	}

	// Token: 0x060051EC RID: 20972 RVA: 0x001C0C0D File Offset: 0x001BF00D
	public static void RestoreNamePlateVisibility(VRC_PlayerApi player)
	{
		player.GetComponent<VRCPlayer>().RestoreNamePlateVisibility();
	}

	// Token: 0x060051ED RID: 20973 RVA: 0x001C0C1A File Offset: 0x001BF01A
	public static void SetPlayerTag(VRC_PlayerApi player, string tagName, string tagValue)
	{
		player.GetComponent<VRCPlayer>().SetPlayerTag(tagName, tagValue);
	}

	// Token: 0x060051EE RID: 20974 RVA: 0x001C0C29 File Offset: 0x001BF029
	public static string GetPlayerTag(VRC_PlayerApi player, string tagName)
	{
		return player.GetComponent<VRCPlayer>().GetPlayerTag(tagName);
	}

	// Token: 0x060051EF RID: 20975 RVA: 0x001C0C37 File Offset: 0x001BF037
	public static List<int> GetPlayersWithTag(string tagName, string tagValue)
	{
		return PlayerManager.GetPlayersWithTag(tagName, tagValue, false);
	}

	// Token: 0x060051F0 RID: 20976 RVA: 0x001C0C41 File Offset: 0x001BF041
	public static void ClearPlayerTags(VRC_PlayerApi player)
	{
		player.GetComponent<VRCPlayer>().ClearPlayerTags();
	}

	// Token: 0x060051F1 RID: 20977 RVA: 0x001C0C4E File Offset: 0x001BF04E
	public static void SetInvisibleToTagged(VRC_PlayerApi player, bool invisible, string tagName, string tagValue)
	{
		player.GetComponent<VRCPlayer>().SetInvisibleToTagged(invisible, tagName, tagValue, false);
	}

	// Token: 0x060051F2 RID: 20978 RVA: 0x001C0C5F File Offset: 0x001BF05F
	public static void SetInvisibleToUntagged(VRC_PlayerApi player, bool invisible, string tagName, string tagValue)
	{
		player.GetComponent<VRCPlayer>().SetInvisibleToTagged(invisible, tagName, tagValue, true);
	}

	// Token: 0x060051F3 RID: 20979 RVA: 0x001C0C70 File Offset: 0x001BF070
	public static void SetSilencedToTagged(VRC_PlayerApi player, int level, string tagName, string tagValue)
	{
		player.GetComponent<VRCPlayer>().SetSilencedToTagged(level, tagName, tagValue, false);
	}

	// Token: 0x060051F4 RID: 20980 RVA: 0x001C0C81 File Offset: 0x001BF081
	public static void SetSilencedToUntagged(VRC_PlayerApi player, int level, string tagName, string tagValue)
	{
		player.GetComponent<VRCPlayer>().SetSilencedToTagged(level, tagName, tagValue, true);
	}

	// Token: 0x060051F5 RID: 20981 RVA: 0x001C0C92 File Offset: 0x001BF092
	public static void ClearInvisible(VRC_PlayerApi player)
	{
		player.GetComponent<VRCPlayer>().ClearInvisible();
	}

	// Token: 0x060051F6 RID: 20982 RVA: 0x001C0C9F File Offset: 0x001BF09F
	public static void ClearSilence(VRC_PlayerApi player)
	{
		player.GetComponent<VRCPlayer>().ClearSilence();
	}

	// Token: 0x060051F7 RID: 20983 RVA: 0x001C0CAC File Offset: 0x001BF0AC
	public static void TeleportTo(VRC_PlayerApi player, Vector3 teleportPos, Quaternion teleportRot)
	{
		InternalSDKPlayer.TeleportToOrientation(player, teleportPos, teleportRot, VRC_SceneDescriptor.SpawnOrientation.Default);
	}

	// Token: 0x060051F8 RID: 20984 RVA: 0x001C0CB8 File Offset: 0x001BF0B8
	public static void TeleportToOrientation(VRC_PlayerApi player, Vector3 teleportPos, Quaternion teleportRot, VRC_SceneDescriptor.SpawnOrientation teleportOrientation)
	{
		if (player != null)
		{
			player.GetComponent<SyncPhysics>().DiscontinuityHint = true;
			LocomotionInputController component = player.GetComponent<LocomotionInputController>();
			if (component != null)
			{
				component.Teleport(teleportPos, teleportRot, teleportOrientation);
			}
			else
			{
				VRC.Network.RPC(player.GetComponent<VRC.Player>(), player.gameObject, "Teleport", new object[]
				{
					teleportPos,
					teleportRot,
					teleportOrientation
				});
			}
		}
		else
		{
			Debug.LogError("Attempted to teleport a null player!");
		}
	}

	// Token: 0x060051F9 RID: 20985 RVA: 0x001C0D44 File Offset: 0x001BF144
	public static bool GoToRoom(string roomId)
	{
		if (VRCPlayer.Instance == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(roomId))
		{
			return false;
		}
		if (!VRCFlowManager.Instance.CanEnterRoom())
		{
			return false;
		}
		VRCFlowManager.Instance.EnterRoom(roomId, null);
		return true;
	}

	// Token: 0x04003A18 RID: 14872
	public static List<VRC_PlayerApi> AllPlayers;
}
