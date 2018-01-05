using System;
using UnityEngine;
using VRC;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009F2 RID: 2546
	[RequireComponent(typeof(VRC_Pickup), typeof(VRC_ObjectSync))]
	public class RemoteVideoCamera : MonoBehaviour
	{
		// Token: 0x06004D6F RID: 19823 RVA: 0x0019F8B4 File Offset: 0x0019DCB4
		public void CamPickup(int instigator)
		{
			this.desktopCam.GetComponent<Camera>().enabled = true;
			this.isHeld = true;
			VRC.Network.SetOwner(VRC.Network.GetPlayerByInstigatorID(instigator), this.ingameCam.gameObject, VRC.Network.OwnershipModificationType.Request, true);
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.isKinematic = false;
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x0019F900 File Offset: 0x0019DD00
		public void CamDrop(int instigator)
		{
			this.isHeld = false;
			this.camMode = 0;
			this.desktopCam.parent = base.transform;
			this.desktopCam.position = this.standardCamPos.position;
			this.desktopCam.rotation = this.standardCamPos.rotation;
			if (this.ingameCam != null)
			{
				this.ingameCam.position = this.standardCamPos.position;
				this.ingameCam.rotation = this.standardCamPos.rotation;
			}
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.isKinematic = true;
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x0019F9A4 File Offset: 0x0019DDA4
		public void CamUseDown(int instigator)
		{
			if (this.isHeld)
			{
				this.camMode++;
				if (this.camMode > 3)
				{
					this.camMode = 0;
				}
				if (this.camMode == 0)
				{
					this.desktopCam.parent = base.transform;
					this.desktopCam.position = this.standardCamPos.position;
					this.desktopCam.rotation = this.standardCamPos.rotation;
					VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetCameraLocator", new object[]
					{
						false,
						true
					});
				}
				else if (this.camMode == 3)
				{
					this.desktopCam.parent = null;
					this.desktopCam.position = VRC.Network.LocalPlayer.playerApi.GetTrackingData(VRC_PlayerApi.TrackingDataType.Head).position;
					VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetCameraLocator", new object[]
					{
						false,
						false
					});
				}
				else
				{
					this.desktopCam.parent = null;
					VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetCameraLocator", new object[]
					{
						true,
						true
					});
				}
			}
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x0019FAF4 File Offset: 0x0019DEF4
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.AllBufferOne
		})]
		public void SetCameraLocator(bool state, bool mirrorState, VRC.Player instigator)
		{
			if (!VRC.Network.IsOwner(instigator, base.gameObject))
			{
				return;
			}
			this.camLocator.SetActive(state);
			if (mirrorState)
			{
				this.ingameCam.GetComponent<Camera>().cullingMask = this.mirrorMask;
				this.desktopCam.GetComponent<Camera>().cullingMask = this.mirrorMask;
			}
			else
			{
				if (this.isHeld)
				{
					return;
				}
				this.ingameCam.GetComponent<Camera>().cullingMask = this.noMirrorMask;
				this.desktopCam.GetComponent<Camera>().cullingMask = this.noMirrorMask;
			}
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x0019FBA2 File Offset: 0x0019DFA2
		public void SetInGameCamera(GameObject inGameCam)
		{
			if (VRC.Network.IsOwner(base.gameObject))
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetInGameCamRPC", new object[]
				{
					inGameCam
				});
			}
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x0019FBD0 File Offset: 0x0019DFD0
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.AllBufferOne
		})]
		private void SetInGameCamRPC(GameObject igc, VRC.Player instigator)
		{
			if (!VRC.Network.IsOwner(instigator, base.gameObject))
			{
				return;
			}
			this.ingameCam = igc.transform;
			this.camLocator = this.ingameCam.Find("CameraLocator").gameObject;
			this.camLocator.SetActive(false);
			Camera component = this.ingameCam.GetComponent<Camera>();
			this.renderTexture = new RenderTexture(1920, 1080, 24);
			component.targetTexture = this.renderTexture;
			for (int i = 0; i < this.previewMeshes.Length; i++)
			{
				this.previewMeshes[i].material.mainTexture = this.renderTexture;
			}
			Transform parent = base.transform.Find("InGameMount");
			this.ingameCam.SetParent(parent);
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x0019FCA0 File Offset: 0x0019E0A0
		private void Update()
		{
			if (this.isHeld)
			{
				Vector3 position = VRC.Network.LocalPlayer.playerApi.GetTrackingData(VRC_PlayerApi.TrackingDataType.Head).position;
				Quaternion rotation = VRC.Network.LocalPlayer.playerApi.GetTrackingData(VRC_PlayerApi.TrackingDataType.Head).rotation;
				if (this.camMode == 1)
				{
					Vector3 b = position + Quaternion.Euler(0f, rotation.eulerAngles.y, 0f) * new Vector3(0f, this.height, -this.distance);
					this.desktopCam.position = Vector3.Lerp(this.desktopCam.position, b, Time.deltaTime * this.damping);
				}
				else if (this.camMode == 2)
				{
					Vector3 b2 = position + Quaternion.Euler(0f, rotation.eulerAngles.y, 0f) * new Vector3(0f, this.height, this.distance);
					this.desktopCam.position = Vector3.Lerp(this.desktopCam.position, b2, Time.deltaTime * this.damping);
				}
				else if (this.camMode == 3)
				{
					this.desktopCam.position = position;
				}
				if (this.camMode == 1 || this.camMode == 2)
				{
					Quaternion b3 = Quaternion.LookRotation(position - this.desktopCam.position, Vector3.up);
					this.desktopCam.rotation = Quaternion.Slerp(this.desktopCam.rotation, b3, Time.deltaTime * this.rotationDamping);
				}
				else if (this.camMode == 3)
				{
					this.desktopCam.rotation = Quaternion.Slerp(this.desktopCam.rotation, rotation, Time.deltaTime * this.rotationDamping);
				}
				this.ingameCam.position = this.desktopCam.position;
				this.ingameCam.rotation = this.desktopCam.rotation;
			}
		}

		// Token: 0x0400357B RID: 13691
		public float distance = 3f;

		// Token: 0x0400357C RID: 13692
		public float height = 2f;

		// Token: 0x0400357D RID: 13693
		public float damping = 1f;

		// Token: 0x0400357E RID: 13694
		public float rotationDamping = 1f;

		// Token: 0x0400357F RID: 13695
		public Transform standardCamPos;

		// Token: 0x04003580 RID: 13696
		public MeshRenderer[] previewMeshes;

		// Token: 0x04003581 RID: 13697
		public Transform desktopCam;

		// Token: 0x04003582 RID: 13698
		private Transform ingameCam;

		// Token: 0x04003583 RID: 13699
		private GameObject camLocator;

		// Token: 0x04003584 RID: 13700
		public LayerMask noMirrorMask;

		// Token: 0x04003585 RID: 13701
		public LayerMask mirrorMask;

		// Token: 0x04003586 RID: 13702
		private bool isHeld;

		// Token: 0x04003587 RID: 13703
		private int camMode;

		// Token: 0x04003588 RID: 13704
		private RenderTexture renderTexture;
	}
}
