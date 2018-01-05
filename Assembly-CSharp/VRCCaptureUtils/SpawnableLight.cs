using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009F3 RID: 2547
	public class SpawnableLight : MonoBehaviour
	{
		// Token: 0x06004D77 RID: 19831 RVA: 0x0019FEC8 File Offset: 0x0019E2C8
		private void Start()
		{
			this.presets = new List<SpawnableLight.Preset>();
			this.AddPreset(LightType.Spot, 50f, 60f, new Color(1f, 1f, 1f, 1f), 0.5f, LightShadows.None);
			this.AddPreset(LightType.Spot, 50f, 60f, new Color(1f, 0.98f, 0.93f, 1f), 0.5f, LightShadows.None);
			this.AddPreset(LightType.Spot, 50f, 60f, new Color(0.91f, 0.97f, 1f, 1f), 0.5f, LightShadows.None);
			this.AddPreset(LightType.Spot, 50f, 60f, new Color(1f, 1f, 1f, 1f), 1f, LightShadows.None);
			this.AddPreset(LightType.Spot, 50f, 60f, new Color(1f, 0.98f, 0.93f, 1f), 1f, LightShadows.None);
			this.AddPreset(LightType.Spot, 50f, 60f, new Color(0.91f, 0.97f, 1f, 1f), 1f, LightShadows.None);
			this.SetPreset(0);
		}

		// Token: 0x06004D78 RID: 19832 RVA: 0x001A0008 File Offset: 0x0019E408
		private void AddPreset(LightType lightType, float range, float angle, Color color, float intensity, LightShadows shadowType)
		{
			SpawnableLight.Preset item = default(SpawnableLight.Preset);
			item.lightType = lightType;
			item.range = range;
			item.angle = angle;
			item.color = color;
			item.intensity = intensity;
			item.shadowType = shadowType;
			this.presets.Add(item);
		}

		// Token: 0x06004D79 RID: 19833 RVA: 0x001A005C File Offset: 0x0019E45C
		private void SetPreset(int num)
		{
			this.myLight.type = this.presets[num].lightType;
			this.myLight.range = this.presets[num].range;
			this.myLight.spotAngle = this.presets[num].angle;
			this.myLight.color = this.presets[num].color;
			this.myLight.intensity = this.presets[num].intensity;
			this.myLight.shadows = this.presets[num].shadowType;
			Renderer component = base.GetComponent<Renderer>();
			component.materials[1].color = this.myLight.color * this.myLight.intensity;
			component.materials[1].SetColor("_EmissionColor", this.myLight.color * this.myLight.intensity);
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x001A0184 File Offset: 0x0019E584
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.Local
		})]
		public void Pickup(int instigator)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "EnableMeshRPC", new object[]
			{
				true
			});
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x001A01A6 File Offset: 0x0019E5A6
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.Local
		})]
		public void Drop(int instigator)
		{
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "EnableMeshRPC", new object[]
			{
				false
			});
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x001A01C8 File Offset: 0x0019E5C8
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.AllBufferOne
		})]
		private void EnableMeshRPC(bool flag, VRC.Player sender)
		{
			if (!VRC.Network.IsOwner(sender, base.gameObject))
			{
				return;
			}
			Renderer component = base.GetComponent<Renderer>();
			component.enabled = flag;
			Rigidbody component2 = base.GetComponent<Rigidbody>();
			component2.isKinematic = !flag;
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x001A0208 File Offset: 0x0019E608
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.Local
		})]
		public void Use(int instigator)
		{
			this.currentPreset++;
			if (this.currentPreset >= this.presets.Count)
			{
				this.currentPreset = 0;
			}
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetPresetRPC", new object[]
			{
				this.currentPreset
			});
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x001A0265 File Offset: 0x0019E665
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.AllBufferOne
		})]
		private void SetPresetRPC(int preset, VRC.Player sender)
		{
			if (!VRC.Network.IsOwner(sender, base.gameObject))
			{
				return;
			}
			this.currentPreset = preset;
			this.SetPreset(preset);
		}

		// Token: 0x04003589 RID: 13705
		public Light myLight;

		// Token: 0x0400358A RID: 13706
		public List<SpawnableLight.Preset> presets;

		// Token: 0x0400358B RID: 13707
		private int currentPreset;

		// Token: 0x020009F4 RID: 2548
		public struct Preset
		{
			// Token: 0x0400358C RID: 13708
			public LightType lightType;

			// Token: 0x0400358D RID: 13709
			public float range;

			// Token: 0x0400358E RID: 13710
			public float angle;

			// Token: 0x0400358F RID: 13711
			public Color color;

			// Token: 0x04003590 RID: 13712
			public float intensity;

			// Token: 0x04003591 RID: 13713
			public LightShadows shadowType;
		}
	}
}
