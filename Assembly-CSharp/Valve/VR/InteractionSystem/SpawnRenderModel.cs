using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC4 RID: 3012
	public class SpawnRenderModel : MonoBehaviour
	{
		// Token: 0x06005D16 RID: 23830 RVA: 0x00207F9C File Offset: 0x0020639C
		private void Awake()
		{
			this.renderModels = new SteamVR_RenderModel[this.materials.Length];
			this.renderModelLoadedAction = SteamVR_Events.RenderModelLoadedAction(new UnityAction<SteamVR_RenderModel, bool>(this.OnRenderModelLoaded));
		}

		// Token: 0x06005D17 RID: 23831 RVA: 0x00207FC8 File Offset: 0x002063C8
		private void OnEnable()
		{
			this.ShowController();
			this.renderModelLoadedAction.enabled = true;
			SpawnRenderModel.spawnRenderModels.Add(this);
		}

		// Token: 0x06005D18 RID: 23832 RVA: 0x00207FE7 File Offset: 0x002063E7
		private void OnDisable()
		{
			this.HideController();
			this.renderModelLoadedAction.enabled = false;
			SpawnRenderModel.spawnRenderModels.Remove(this);
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x00208007 File Offset: 0x00206407
		private void OnAttachedToHand(Hand hand)
		{
			this.hand = hand;
			this.ShowController();
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x00208016 File Offset: 0x00206416
		private void OnDetachedFromHand(Hand hand)
		{
			this.hand = null;
			this.HideController();
		}

		// Token: 0x06005D1B RID: 23835 RVA: 0x00208028 File Offset: 0x00206428
		private void Update()
		{
			if (SpawnRenderModel.lastFrameUpdated == Time.renderedFrameCount)
			{
				return;
			}
			SpawnRenderModel.lastFrameUpdated = Time.renderedFrameCount;
			if (SpawnRenderModel.spawnRenderModelUpdateIndex >= SpawnRenderModel.spawnRenderModels.Count)
			{
				SpawnRenderModel.spawnRenderModelUpdateIndex = 0;
			}
			if (SpawnRenderModel.spawnRenderModelUpdateIndex < SpawnRenderModel.spawnRenderModels.Count)
			{
				SteamVR_RenderModel steamVR_RenderModel = SpawnRenderModel.spawnRenderModels[SpawnRenderModel.spawnRenderModelUpdateIndex].renderModels[0];
				if (steamVR_RenderModel != null)
				{
					steamVR_RenderModel.UpdateComponents(OpenVR.RenderModels);
				}
			}
			SpawnRenderModel.spawnRenderModelUpdateIndex++;
		}

		// Token: 0x06005D1C RID: 23836 RVA: 0x002080B8 File Offset: 0x002064B8
		private void ShowController()
		{
			if (this.hand == null || this.hand.controller == null)
			{
				return;
			}
			for (int i = 0; i < this.renderModels.Length; i++)
			{
				if (this.renderModels[i] == null)
				{
					this.renderModels[i] = new GameObject("SteamVR_RenderModel").AddComponent<SteamVR_RenderModel>();
					this.renderModels[i].updateDynamically = false;
					this.renderModels[i].transform.parent = base.transform;
					Util.ResetTransform(this.renderModels[i].transform, true);
				}
				this.renderModels[i].gameObject.SetActive(true);
				this.renderModels[i].SetDeviceIndex((int)this.hand.controller.index);
			}
		}

		// Token: 0x06005D1D RID: 23837 RVA: 0x00208194 File Offset: 0x00206594
		private void HideController()
		{
			for (int i = 0; i < this.renderModels.Length; i++)
			{
				if (this.renderModels[i] != null)
				{
					this.renderModels[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06005D1E RID: 23838 RVA: 0x002081E0 File Offset: 0x002065E0
		private void OnRenderModelLoaded(SteamVR_RenderModel renderModel, bool success)
		{
			for (int i = 0; i < this.renderModels.Length; i++)
			{
				if (renderModel == this.renderModels[i] && this.materials[i] != null)
				{
					this.renderers.Clear();
					this.renderModels[i].GetComponentsInChildren<MeshRenderer>(this.renderers);
					for (int j = 0; j < this.renderers.Count; j++)
					{
						Texture mainTexture = this.renderers[j].material.mainTexture;
						this.renderers[j].sharedMaterial = this.materials[i];
						this.renderers[j].material.mainTexture = mainTexture;
						this.renderers[j].gameObject.layer = base.gameObject.layer;
						this.renderers[j].tag = base.gameObject.tag;
					}
				}
			}
		}

		// Token: 0x040042AE RID: 17070
		public Material[] materials;

		// Token: 0x040042AF RID: 17071
		private SteamVR_RenderModel[] renderModels;

		// Token: 0x040042B0 RID: 17072
		private Hand hand;

		// Token: 0x040042B1 RID: 17073
		private List<MeshRenderer> renderers = new List<MeshRenderer>();

		// Token: 0x040042B2 RID: 17074
		private static List<SpawnRenderModel> spawnRenderModels = new List<SpawnRenderModel>();

		// Token: 0x040042B3 RID: 17075
		private static int lastFrameUpdated;

		// Token: 0x040042B4 RID: 17076
		private static int spawnRenderModelUpdateIndex;

		// Token: 0x040042B5 RID: 17077
		private SteamVR_Events.Action renderModelLoadedAction;
	}
}
