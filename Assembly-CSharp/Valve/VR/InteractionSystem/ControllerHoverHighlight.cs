using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000B98 RID: 2968
	public class ControllerHoverHighlight : MonoBehaviour
	{
		// Token: 0x06005C4C RID: 23628 RVA: 0x00204084 File Offset: 0x00202484
		private void Start()
		{
			this.hand = base.GetComponentInParent<Hand>();
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x00204092 File Offset: 0x00202492
		private void Awake()
		{
			this.renderModelLoadedAction = SteamVR_Events.RenderModelLoadedAction(new UnityAction<SteamVR_RenderModel, bool>(this.OnRenderModelLoaded));
		}

		// Token: 0x06005C4E RID: 23630 RVA: 0x002040AB File Offset: 0x002024AB
		private void OnEnable()
		{
			this.renderModelLoadedAction.enabled = true;
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x002040B9 File Offset: 0x002024B9
		private void OnDisable()
		{
			this.renderModelLoadedAction.enabled = false;
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x002040C7 File Offset: 0x002024C7
		private void OnHandInitialized(int deviceIndex)
		{
			this.renderModel = base.gameObject.AddComponent<SteamVR_RenderModel>();
			this.renderModel.SetDeviceIndex(deviceIndex);
			this.renderModel.updateDynamically = false;
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x002040F4 File Offset: 0x002024F4
		private void OnRenderModelLoaded(SteamVR_RenderModel renderModel, bool success)
		{
			if (renderModel != this.renderModel)
			{
				return;
			}
			Transform transform = base.transform.Find("body");
			if (transform != null)
			{
				transform.gameObject.layer = base.gameObject.layer;
				transform.gameObject.tag = base.gameObject.tag;
				this.bodyMeshRenderer = transform.GetComponent<MeshRenderer>();
				this.bodyMeshRenderer.material = this.highLightMaterial;
				this.bodyMeshRenderer.enabled = false;
			}
			Transform transform2 = base.transform.Find("trackhat");
			if (transform2 != null)
			{
				transform2.gameObject.layer = base.gameObject.layer;
				transform2.gameObject.tag = base.gameObject.tag;
				this.trackingHatMeshRenderer = transform2.GetComponent<MeshRenderer>();
				this.trackingHatMeshRenderer.material = this.highLightMaterial;
				this.trackingHatMeshRenderer.enabled = false;
			}
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform3 = (Transform)obj;
					if (transform3.name != "body" && transform3.name != "trackhat")
					{
						UnityEngine.Object.Destroy(transform3.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.renderModelLoaded = true;
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x00204288 File Offset: 0x00202688
		private void OnParentHandHoverBegin(Interactable other)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (other.transform.parent != base.transform.parent)
			{
				this.ShowHighlight();
			}
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x002042BC File Offset: 0x002026BC
		private void OnParentHandHoverEnd(Interactable other)
		{
			this.HideHighlight();
		}

		// Token: 0x06005C54 RID: 23636 RVA: 0x002042C4 File Offset: 0x002026C4
		private void OnParentHandInputFocusAcquired()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.hand.hoveringInteractable && this.hand.hoveringInteractable.transform.parent != base.transform.parent)
			{
				this.ShowHighlight();
			}
		}

		// Token: 0x06005C55 RID: 23637 RVA: 0x00204322 File Offset: 0x00202722
		private void OnParentHandInputFocusLost()
		{
			this.HideHighlight();
		}

		// Token: 0x06005C56 RID: 23638 RVA: 0x0020432C File Offset: 0x0020272C
		public void ShowHighlight()
		{
			if (!this.renderModelLoaded)
			{
				return;
			}
			if (this.fireHapticsOnHightlight)
			{
				this.hand.controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_Axis0);
			}
			if (this.bodyMeshRenderer != null)
			{
				this.bodyMeshRenderer.enabled = true;
			}
			if (this.trackingHatMeshRenderer != null)
			{
				this.trackingHatMeshRenderer.enabled = true;
			}
		}

		// Token: 0x06005C57 RID: 23639 RVA: 0x002043A4 File Offset: 0x002027A4
		public void HideHighlight()
		{
			if (!this.renderModelLoaded)
			{
				return;
			}
			if (this.fireHapticsOnHightlight)
			{
				this.hand.controller.TriggerHapticPulse(300, EVRButtonId.k_EButton_Axis0);
			}
			if (this.bodyMeshRenderer != null)
			{
				this.bodyMeshRenderer.enabled = false;
			}
			if (this.trackingHatMeshRenderer != null)
			{
				this.trackingHatMeshRenderer.enabled = false;
			}
		}

		// Token: 0x040041EB RID: 16875
		public Material highLightMaterial;

		// Token: 0x040041EC RID: 16876
		public bool fireHapticsOnHightlight = true;

		// Token: 0x040041ED RID: 16877
		private Hand hand;

		// Token: 0x040041EE RID: 16878
		private MeshRenderer bodyMeshRenderer;

		// Token: 0x040041EF RID: 16879
		private MeshRenderer trackingHatMeshRenderer;

		// Token: 0x040041F0 RID: 16880
		private SteamVR_RenderModel renderModel;

		// Token: 0x040041F1 RID: 16881
		private bool renderModelLoaded;

		// Token: 0x040041F2 RID: 16882
		private SteamVR_Events.Action renderModelLoadedAction;
	}
}
