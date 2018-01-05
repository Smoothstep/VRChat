using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009EF RID: 2543
	public class PlayerCamViewer : VRCPunBehaviour
	{
		// Token: 0x06004D51 RID: 19793 RVA: 0x0019E88A File Offset: 0x0019CC8A
		public override void Awake()
		{
			base.Awake();
			this.DisableButtons();
			this.pickup = base.GetComponent<VRC_Pickup>();
			this.uiParent.SetActive(false);
			this.headOffset = this.headOffsetFront;
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x0019E8BC File Offset: 0x0019CCBC
		protected override void OnNetworkReady()
		{
			if (VRC.Network.IsOwner(base.gameObject))
			{
				this.currentPlayer = VRC.Network.LocalPlayer;
				if (this.cam != null)
				{
					this.cam.cullingMask = this.localMask;
				}
				if (this.desktopCam != null)
				{
					this.desktopCam.cullingMask = this.localMask;
				}
			}
			this.followPlayerNameText.text = this.followPrefix + this.currentPlayer.name;
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x0019E954 File Offset: 0x0019CD54
		private void OnPlayerLeft(VRC_PlayerApi player)
		{
			if (base.isMine)
			{
				this.UpdateUI();
				if (player.playerId == this.currentPlayer.playerApi.playerId)
				{
					VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "SetNewPlayer", new object[]
					{
						VRC.Network.LocalPlayer.playerApi.playerId
					});
				}
			}
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0019E9BC File Offset: 0x0019CDBC
		private void OnPlayerJoin(VRC_PlayerApi player)
		{
			if (base.isMine)
			{
				this.UpdateUI();
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "SetNewPlayer", new object[]
				{
					this.currentPlayer.playerApi.playerId
				});
			}
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x0019EA0C File Offset: 0x0019CE0C
		public void UpdateUI()
		{
			Debug.Log("UpdateUI");
			this.DisableButtons();
			for (int i = 0; i < VRC_PlayerApi.AllPlayers.Count; i++)
			{
				this.SetUpPlayerButton(i, VRC_PlayerApi.AllPlayers[i].playerId, VRC_PlayerApi.AllPlayers[i].name);
			}
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x0019EA6C File Offset: 0x0019CE6C
		private void SetUpPlayerButton(int number, int playerId, string playerName)
		{
			Debug.Log("Setting up: " + playerName);
			this.playerButtons[number].button.onClick.AddListener(new UnityAction(this.playerButtons[number].ButtonClicked));
			this.playerButtons[number].playerId = playerId;
			this.playerButtons[number].playerNameText.text = playerName;
			this.playerButtons[number].gameObject.SetActive(true);
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x0019EAE8 File Offset: 0x0019CEE8
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.All
		})]
		private void SetNewPlayer(int playerId, VRC.Player instigator)
		{
			if (base.Owner != instigator)
			{
				return;
			}
			this.currentPlayer = VRC_PlayerApi.GetPlayerById(playerId).GetComponent<VRC.Player>();
			if (this.currentPlayer == VRC.Network.LocalPlayer)
			{
				if (this.cam != null)
				{
					this.cam.cullingMask = this.localMask;
				}
				if (this.desktopCam != null)
				{
					this.desktopCam.cullingMask = this.localMask;
				}
				VRC.Network.SetOwner(VRC.Network.LocalPlayer, this.cam.gameObject, VRC.Network.OwnershipModificationType.Request, true);
			}
			else
			{
				if (this.cam != null)
				{
					this.cam.cullingMask = this.remoteMask;
				}
				if (this.desktopCam != null)
				{
					this.desktopCam.cullingMask = this.remoteMask;
				}
			}
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x0019EBE8 File Offset: 0x0019CFE8
		public void OnPlayerViewerPickup()
		{
			this.pickup.pickupable = false;
			if (this.pickup.currentLocalPlayer != null && this.pickup.currentLocalPlayer.playerId == VRC.Network.LocalPlayer.playerApi.playerId)
			{
				this.holder = this.pickup.currentLocalPlayer;
				this.uiButton.SetActive(true);
				this.pickup.pickupable = true;
				if (this.cam != null)
				{
					this.desktopCam = this.cam.transform.GetChild(0).GetComponent<Camera>();
					if (this.desktopCam != null)
					{
						this.desktopCam.enabled = true;
					}
				}
			}
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x0019ECB0 File Offset: 0x0019D0B0
		public void OnPlayerViewerDrop()
		{
			this.pickup.pickupable = true;
			this.uiParent.SetActive(false);
			if (this.holder != null && this.desktopCam != null)
			{
				this.desktopCam.enabled = false;
			}
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x0019ED04 File Offset: 0x0019D104
		private void DisableButtons()
		{
			Debug.Log("Disabling buttons");
			for (int i = 0; i < this.playerButtons.Length; i++)
			{
				if (this.playerButtons[i].gameObject.activeSelf)
				{
					this.playerButtons[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x0019ED60 File Offset: 0x0019D160
		public void UIButtonPressed(PlayerCamViewer.buttons button)
		{
			if (!base.isMine)
			{
				return;
			}
			if (button == PlayerCamViewer.buttons.uiButton)
			{
				this.UpdateUI();
				this.uiButton.SetActive(false);
				this.playerOptionsWindow.SetActive(false);
				this.uiParent.SetActive(true);
				this.scrollView.SetActive(true);
				Debug.Log(string.Concat(new object[]
				{
					this.uiButton.activeSelf,
					" : ",
					this.playerOptionsWindow.activeSelf,
					" : ",
					this.uiParent.activeSelf,
					" : ",
					this.scrollView.activeSelf
				}));
			}
			else if (button == PlayerCamViewer.buttons.back)
			{
				VRC.Network.LocalPlayer.playerApi.SetVelocity(Vector3.up * 100f);
				if (this.inSubmenu)
				{
					this.scrollView.SetActive(true);
					this.playerOptionsWindow.SetActive(false);
					this.inSubmenu = false;
				}
				else
				{
					this.uiParent.SetActive(false);
					this.uiButton.SetActive(true);
				}
			}
			else if (button == PlayerCamViewer.buttons.playerOptions)
			{
				this.scrollView.SetActive(false);
				this.playerOptionsWindow.SetActive(true);
				this.inSubmenu = true;
			}
			else if (button == PlayerCamViewer.buttons.teleportMe)
			{
				this.TeleportMeToTarget();
			}
			else if (button == PlayerCamViewer.buttons.frontView)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetHeadOffsetRPC", new object[]
				{
					0
				});
			}
			else if (button == PlayerCamViewer.buttons.rightRearView)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetHeadOffsetRPC", new object[]
				{
					1
				});
			}
			else if (button == PlayerCamViewer.buttons.leftRearView)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetHeadOffsetRPC", new object[]
				{
					2
				});
			}
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x0019EF5C File Offset: 0x0019D35C
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.AllBufferOne
		})]
		private void SetHeadOffsetRPC(int view, VRC.Player instigator)
		{
			if (base.Owner != instigator)
			{
				return;
			}
			if (view == 1)
			{
				this.followPrefix = "Follow(R): ";
				this.headOffset = this.headOffsetRightRear;
			}
			else if (view == 2)
			{
				this.followPrefix = "Follow(L): ";
				this.headOffset = this.headOffsetLeftRear;
			}
			else
			{
				this.followPrefix = "View of: ";
				this.headOffset = this.headOffsetFront;
			}
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x0019EFD8 File Offset: 0x0019D3D8
		public void SetRemoteCamera(GameObject remotecam)
		{
			if (!base.isMine)
			{
				return;
			}
			VRC.Network.RPC(VRC_EventHandler.VrcTargetType.AllBufferOne, base.gameObject, "SetRemoteCamRPC", new object[]
			{
				remotecam
			});
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x0019F004 File Offset: 0x0019D404
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.AllBufferOne
		})]
		private void SetRemoteCamRPC(GameObject remotecam, VRC.Player instigator)
		{
			if (base.Owner != instigator)
			{
				return;
			}
			this.cam = remotecam.GetComponent<Camera>();
			RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
			this.cam.targetTexture = renderTexture;
			this.previewMesh.material.mainTexture = renderTexture;
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x0019F060 File Offset: 0x0019D460
		private void Update()
		{
			if (this.currentPlayer != null)
			{
				this.followPlayerNameText.text = this.followPrefix + this.currentPlayer.name;
				if (this.currentPlayer.playerApi.playerId == VRC.Network.LocalPlayer.playerApi.playerId)
				{
					Vector3 vector = this.currentPlayer.playerApi.GetTrackingData(VRC_PlayerApi.TrackingDataType.Head).position;
					Quaternion rotation = this.currentPlayer.playerApi.GetTrackingData(VRC_PlayerApi.TrackingDataType.Head).rotation;
					vector += this.currentPlayer.transform.TransformVector(this.headOffset);
					if (this.cam != null)
					{
						this.cam.transform.position = Vector3.Lerp(this.cam.transform.position, vector, Time.deltaTime * this.damping);
						this.cam.transform.rotation = Quaternion.Slerp(this.cam.transform.rotation, rotation, Time.deltaTime * this.rotationDamping);
					}
				}
			}
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x0019F18C File Offset: 0x0019D58C
		public void TeleportMeToTarget()
		{
			if (this.currentPlayer != null)
			{
				VRC.Network.LocalPlayer.playerApi.TeleportTo(this.currentPlayer.transform.position, this.currentPlayer.transform.rotation);
			}
		}

		// Token: 0x04003546 RID: 13638
		public float damping = 1f;

		// Token: 0x04003547 RID: 13639
		public float rotationDamping = 1f;

		// Token: 0x04003548 RID: 13640
		public MeshRenderer previewMesh;

		// Token: 0x04003549 RID: 13641
		public Text followPlayerNameText;

		// Token: 0x0400354A RID: 13642
		public LayerMask localMask;

		// Token: 0x0400354B RID: 13643
		public LayerMask remoteMask;

		// Token: 0x0400354C RID: 13644
		public PlayerButton[] playerButtons;

		// Token: 0x0400354D RID: 13645
		public GameObject uiButton;

		// Token: 0x0400354E RID: 13646
		public GameObject uiParent;

		// Token: 0x0400354F RID: 13647
		public GameObject scrollView;

		// Token: 0x04003550 RID: 13648
		public GameObject playerOptionsWindow;

		// Token: 0x04003551 RID: 13649
		private Camera cam;

		// Token: 0x04003552 RID: 13650
		private Camera desktopCam;

		// Token: 0x04003553 RID: 13651
		public Vector3 headOffsetFront = new Vector3(0f, 0f, 0.5f);

		// Token: 0x04003554 RID: 13652
		public Vector3 headOffsetRightRear = new Vector3(0.6f, 0.1f, -0.6f);

		// Token: 0x04003555 RID: 13653
		public Vector3 headOffsetLeftRear = new Vector3(-0.6f, 0.1f, -0.6f);

		// Token: 0x04003556 RID: 13654
		private Vector3 headOffset;

		// Token: 0x04003557 RID: 13655
		private string followPrefix = "View of: ";

		// Token: 0x04003558 RID: 13656
		private VRC_PlayerApi holder;

		// Token: 0x04003559 RID: 13657
		private bool inSubmenu;

		// Token: 0x0400355A RID: 13658
		private VRC_Pickup pickup;

		// Token: 0x0400355B RID: 13659
		private int currentSelection;

		// Token: 0x0400355C RID: 13660
		private VRC.Player currentPlayer;

		// Token: 0x020009F0 RID: 2544
		public enum buttons
		{
			// Token: 0x0400355E RID: 13662
			uiButton,
			// Token: 0x0400355F RID: 13663
			back,
			// Token: 0x04003560 RID: 13664
			playerOptions,
			// Token: 0x04003561 RID: 13665
			teleportMe,
			// Token: 0x04003562 RID: 13666
			teleportPlayer,
			// Token: 0x04003563 RID: 13667
			frontView,
			// Token: 0x04003564 RID: 13668
			rightRearView,
			// Token: 0x04003565 RID: 13669
			leftRearView
		}
	}
}
