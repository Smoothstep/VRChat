using System;
using UnityEngine;

// Token: 0x02000B8C RID: 2956
public class SteamVR_Teleporter : MonoBehaviour
{
	// Token: 0x17000D2F RID: 3375
	// (get) Token: 0x06005BFC RID: 23548 RVA: 0x00201DC8 File Offset: 0x002001C8
	private Transform reference
	{
		get
		{
			SteamVR_Camera steamVR_Camera = SteamVR_Render.Top();
			return (!(steamVR_Camera != null)) ? null : steamVR_Camera.origin;
		}
	}

	// Token: 0x06005BFD RID: 23549 RVA: 0x00201DF4 File Offset: 0x002001F4
	private void Start()
	{
		SteamVR_TrackedController steamVR_TrackedController = base.GetComponent<SteamVR_TrackedController>();
		if (steamVR_TrackedController == null)
		{
			steamVR_TrackedController = base.gameObject.AddComponent<SteamVR_TrackedController>();
		}
		steamVR_TrackedController.TriggerClicked += this.DoClick;
		if (this.teleportType == SteamVR_Teleporter.TeleportType.TeleportTypeUseTerrain)
		{
			Transform reference = this.reference;
			if (reference != null)
			{
				reference.position = new Vector3(reference.position.x, Terrain.activeTerrain.SampleHeight(reference.position), reference.position.z);
			}
		}
	}

	// Token: 0x06005BFE RID: 23550 RVA: 0x00201E88 File Offset: 0x00200288
	private void DoClick(object sender, ClickedEventArgs e)
	{
		if (this.teleportOnClick)
		{
			Transform reference = this.reference;
			if (reference == null)
			{
				return;
			}
			float y = reference.position.y;
			Plane plane = new Plane(Vector3.up, -y);
			Ray ray = new Ray(base.transform.position, base.transform.forward);
			float d = 0f;
			bool flag;
			if (this.teleportType == SteamVR_Teleporter.TeleportType.TeleportTypeUseTerrain)
			{
				TerrainCollider component = Terrain.activeTerrain.GetComponent<TerrainCollider>();
				RaycastHit raycastHit;
				flag = component.Raycast(ray, out raycastHit, 1000f);
				d = raycastHit.distance;
			}
			else if (this.teleportType == SteamVR_Teleporter.TeleportType.TeleportTypeUseCollider)
			{
				RaycastHit raycastHit2;
				flag = Physics.Raycast(ray, out raycastHit2);
				d = raycastHit2.distance;
			}
			else
			{
				flag = plane.Raycast(ray, out d);
			}
			if (flag)
			{
				Vector3 b = new Vector3(SteamVR_Render.Top().head.position.x, y, SteamVR_Render.Top().head.position.z);
				reference.position = reference.position + (ray.origin + ray.direction * d) - b;
			}
		}
	}

	// Token: 0x0400418F RID: 16783
	public bool teleportOnClick;

	// Token: 0x04004190 RID: 16784
	public SteamVR_Teleporter.TeleportType teleportType = SteamVR_Teleporter.TeleportType.TeleportTypeUseZeroY;

	// Token: 0x02000B8D RID: 2957
	public enum TeleportType
	{
		// Token: 0x04004192 RID: 16786
		TeleportTypeUseTerrain,
		// Token: 0x04004193 RID: 16787
		TeleportTypeUseCollider,
		// Token: 0x04004194 RID: 16788
		TeleportTypeUseZeroY
	}
}
