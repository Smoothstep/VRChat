using System;
using System.Linq;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000A62 RID: 2658
public static class VRCGameObjectExtensions
{
	// Token: 0x0600506C RID: 20588 RVA: 0x001B8268 File Offset: 0x001B6668
	public static bool IsVisible(this GameObject go)
	{
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
		return componentsInChildren.Any((Renderer r) => r != null && r.enabled && r.isVisible);
	}

	// Token: 0x0600506D RID: 20589 RVA: 0x001B829F File Offset: 0x001B669F
	public static bool IsVisibleLocally(this GameObject go)
	{
		return !(VRCVrCamera.GetInstance() == null) && go.IsVisibleTo(VRCVrCamera.GetInstance().screenCamera);
	}

	// Token: 0x0600506E RID: 20590 RVA: 0x001B82C8 File Offset: 0x001B66C8
	public static bool IsVisibleTo(this GameObject go, Camera camera)
	{
		if (camera == null)
		{
			return false;
		}
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in from r in componentsInChildren
		where r != null && r.enabled
		select r)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600506F RID: 20591 RVA: 0x001B8370 File Offset: 0x001B6770
	public static bool IsPickup(this GameObject go)
	{
		return go != null && go.GetComponentInParent<VRC_Pickup>() != null;
	}

	// Token: 0x06005070 RID: 20592 RVA: 0x001B8390 File Offset: 0x001B6790
	public static bool IsHeld(this GameObject go)
	{
		if (go == null)
		{
			return false;
		}
		VRC_Pickup componentInParent = go.GetComponentInParent<VRC_Pickup>();
		return componentInParent != null && componentInParent.IsHeld;
	}

	// Token: 0x06005071 RID: 20593 RVA: 0x001B83C7 File Offset: 0x001B67C7
	public static bool IsPlayer(this GameObject go)
	{
		return go != null && go.GetComponentInParent<VRCPlayer>() != null;
	}

	// Token: 0x06005072 RID: 20594 RVA: 0x001B83E4 File Offset: 0x001B67E4
	public static bool IsMine(this GameObject go)
	{
		return VRC.Network.IsOwner(go);
	}

	// Token: 0x06005073 RID: 20595 RVA: 0x001B83EC File Offset: 0x001B67EC
	public static bool IsReady(this GameObject go)
	{
		return VRC.Network.IsObjectReady(go);
	}

	// Token: 0x06005074 RID: 20596 RVA: 0x001B83F4 File Offset: 0x001B67F4
	public static VRC.Player Owner(this GameObject go)
	{
		return VRC.Network.GetOwner(go);
	}

	// Token: 0x06005075 RID: 20597 RVA: 0x001B83FC File Offset: 0x001B67FC
	public static string WhyNotReady(this GameObject go)
	{
		if (go == null)
		{
			return "object is null";
		}
		NetworkMetadata component = go.GetComponent<NetworkMetadata>();
		return (!(component == null)) ? ((!component.isReady) ? ("waiting for " + component.waitingFor) : "ready") : "no metadata";
	}
}
