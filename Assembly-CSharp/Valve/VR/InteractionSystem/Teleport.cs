using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BE1 RID: 3041
	public class Teleport : MonoBehaviour
	{
		// Token: 0x06005E05 RID: 24069 RVA: 0x0020E614 File Offset: 0x0020CA14
		public static SteamVR_Events.Action<float> ChangeSceneAction(UnityAction<float> action)
		{
			return new SteamVR_Events.Action<float>(Teleport.ChangeScene, action);
		}

		// Token: 0x06005E06 RID: 24070 RVA: 0x0020E621 File Offset: 0x0020CA21
		public static SteamVR_Events.Action<TeleportMarkerBase> PlayerAction(UnityAction<TeleportMarkerBase> action)
		{
			return new SteamVR_Events.Action<TeleportMarkerBase>(Teleport.Player, action);
		}

		// Token: 0x06005E07 RID: 24071 RVA: 0x0020E62E File Offset: 0x0020CA2E
		public static SteamVR_Events.Action<TeleportMarkerBase> PlayerPreAction(UnityAction<TeleportMarkerBase> action)
		{
			return new SteamVR_Events.Action<TeleportMarkerBase>(Teleport.PlayerPre, action);
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06005E08 RID: 24072 RVA: 0x0020E63B File Offset: 0x0020CA3B
		public static Teleport instance
		{
			get
			{
				if (Teleport._instance == null)
				{
					Teleport._instance = UnityEngine.Object.FindObjectOfType<Teleport>();
				}
				return Teleport._instance;
			}
		}

		// Token: 0x06005E09 RID: 24073 RVA: 0x0020E65C File Offset: 0x0020CA5C
		private void Awake()
		{
			Teleport._instance = this;
			this.chaperoneInfoInitializedAction = ChaperoneInfo.InitializedAction(new UnityAction(this.OnChaperoneInfoInitialized));
			this.pointerLineRenderer = base.GetComponentInChildren<LineRenderer>();
			this.teleportPointerObject = this.pointerLineRenderer.gameObject;
			int nameID = Shader.PropertyToID("_TintColor");
			this.fullTintAlpha = this.pointVisibleMaterial.GetColor(nameID).a;
			this.teleportArc = base.GetComponent<TeleportArc>();
			this.teleportArc.traceLayerMask = this.traceLayerMask;
			this.loopingAudioMaxVolume = this.loopingAudioSource.volume;
			this.playAreaPreviewCorner.SetActive(false);
			this.playAreaPreviewSide.SetActive(false);
			float x = this.invalidReticleTransform.localScale.x;
			this.invalidReticleMinScale *= x;
			this.invalidReticleMaxScale *= x;
		}

		// Token: 0x06005E0A RID: 24074 RVA: 0x0020E744 File Offset: 0x0020CB44
		private void Start()
		{
			this.teleportMarkers = UnityEngine.Object.FindObjectsOfType<TeleportMarkerBase>();
			this.HidePointer();
			this.player = Valve.VR.InteractionSystem.Player.instance;
			if (this.player == null)
			{
				Debug.LogError("Teleport: No Player instance found in map.");
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			this.CheckForSpawnPoint();
			base.Invoke("ShowTeleportHint", 5f);
		}

		// Token: 0x06005E0B RID: 24075 RVA: 0x0020E7AA File Offset: 0x0020CBAA
		private void OnEnable()
		{
			this.chaperoneInfoInitializedAction.enabled = true;
			this.OnChaperoneInfoInitialized();
		}

		// Token: 0x06005E0C RID: 24076 RVA: 0x0020E7BE File Offset: 0x0020CBBE
		private void OnDisable()
		{
			this.chaperoneInfoInitializedAction.enabled = false;
			this.HidePointer();
		}

		// Token: 0x06005E0D RID: 24077 RVA: 0x0020E7D4 File Offset: 0x0020CBD4
		private void CheckForSpawnPoint()
		{
			foreach (TeleportMarkerBase teleportMarkerBase in this.teleportMarkers)
			{
				TeleportPoint teleportPoint = teleportMarkerBase as TeleportPoint;
				if (teleportPoint && teleportPoint.playerSpawnPoint)
				{
					this.teleportingToMarker = teleportMarkerBase;
					this.TeleportPlayer();
					break;
				}
			}
		}

		// Token: 0x06005E0E RID: 24078 RVA: 0x0020E82F File Offset: 0x0020CC2F
		public void HideTeleportPointer()
		{
			if (this.pointerHand != null)
			{
				this.HidePointer();
			}
		}

		// Token: 0x06005E0F RID: 24079 RVA: 0x0020E848 File Offset: 0x0020CC48
		private void Update()
		{
			Hand oldPointerHand = this.pointerHand;
			Hand hand = null;
			foreach (Hand hand2 in this.player.hands)
			{
				if (this.visible && this.WasTeleportButtonReleased(hand2) && this.pointerHand == hand2)
				{
					this.TryTeleportPlayer();
				}
				if (this.WasTeleportButtonPressed(hand2))
				{
					hand = hand2;
				}
			}
			if (this.allowTeleportWhileAttached && !this.allowTeleportWhileAttached.teleportAllowed)
			{
				this.HidePointer();
			}
			else if (!this.visible && hand != null)
			{
				this.ShowPointer(hand, oldPointerHand);
			}
			else if (this.visible)
			{
				if (hand == null && !this.IsTeleportButtonDown(this.pointerHand))
				{
					this.HidePointer();
				}
				else if (hand != null)
				{
					this.ShowPointer(hand, oldPointerHand);
				}
			}
			if (this.visible)
			{
				this.UpdatePointer();
				if (this.meshFading)
				{
					this.UpdateTeleportColors();
				}
				if (this.onActivateObjectTransform.gameObject.activeSelf && Time.time - this.pointerShowStartTime > this.activateObjectTime)
				{
					this.onActivateObjectTransform.gameObject.SetActive(false);
				}
			}
			else if (this.onDeactivateObjectTransform.gameObject.activeSelf && Time.time - this.pointerHideStartTime > this.deactivateObjectTime)
			{
				this.onDeactivateObjectTransform.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005E10 RID: 24080 RVA: 0x0020E9FC File Offset: 0x0020CDFC
		private void UpdatePointer()
		{
			Vector3 position = this.pointerStartTransform.position;
			Vector3 forward = this.pointerStartTransform.forward;
			bool flag = false;
			bool active = false;
			Vector3 vector = this.player.trackingOriginTransform.position - this.player.feetPositionGuess;
			Vector3 velocity = forward * this.arcDistance;
			TeleportMarkerBase teleportMarkerBase = null;
			float num = Vector3.Dot(forward, Vector3.up);
			float num2 = Vector3.Dot(forward, this.player.hmdTransform.forward);
			bool flag2 = false;
			if ((num2 > 0f && num > 0.75f) || (num2 < 0f && num > 0.5f))
			{
				flag2 = true;
			}
			this.teleportArc.SetArcData(position, velocity, true, flag2);
			RaycastHit raycastHit;
			if (this.teleportArc.DrawArc(out raycastHit))
			{
				flag = true;
				teleportMarkerBase = raycastHit.collider.GetComponentInParent<TeleportMarkerBase>();
			}
			if (flag2)
			{
				teleportMarkerBase = null;
			}
			this.HighlightSelected(teleportMarkerBase);
			Vector3 vector2;
			if (teleportMarkerBase != null)
			{
				if (teleportMarkerBase.locked)
				{
					this.teleportArc.SetColor(this.pointerLockedColor);
					this.pointerLineRenderer.startColor = this.pointerLockedColor;
					this.pointerLineRenderer.endColor = this.pointerLockedColor;
					this.destinationReticleTransform.gameObject.SetActive(false);
				}
				else
				{
					this.teleportArc.SetColor(this.pointerValidColor);
					this.pointerLineRenderer.startColor = this.pointerValidColor;
					this.pointerLineRenderer.endColor = this.pointerValidColor;
					this.destinationReticleTransform.gameObject.SetActive(teleportMarkerBase.showReticle);
				}
				this.offsetReticleTransform.gameObject.SetActive(true);
				this.invalidReticleTransform.gameObject.SetActive(false);
				this.pointedAtTeleportMarker = teleportMarkerBase;
				this.pointedAtPosition = raycastHit.point;
				if (this.showPlayAreaMarker)
				{
					TeleportArea teleportArea = this.pointedAtTeleportMarker as TeleportArea;
					if (teleportArea != null && !teleportArea.locked && this.playAreaPreviewTransform != null)
					{
						Vector3 b = vector;
						if (!this.movedFeetFarEnough)
						{
							float num3 = Vector3.Distance(vector, this.startingFeetOffset);
							if (num3 < 0.1f)
							{
								b = this.startingFeetOffset;
							}
							else if (num3 < 0.4f)
							{
								b = Vector3.Lerp(this.startingFeetOffset, vector, (num3 - 0.1f) / 0.3f);
							}
							else
							{
								this.movedFeetFarEnough = true;
							}
						}
						this.playAreaPreviewTransform.position = this.pointedAtPosition + b;
						active = true;
					}
				}
				vector2 = raycastHit.point;
			}
			else
			{
				this.destinationReticleTransform.gameObject.SetActive(false);
				this.offsetReticleTransform.gameObject.SetActive(false);
				this.teleportArc.SetColor(this.pointerInvalidColor);
				this.pointerLineRenderer.startColor = this.pointerInvalidColor;
				this.pointerLineRenderer.endColor = this.pointerInvalidColor;
				this.invalidReticleTransform.gameObject.SetActive(!flag2);
				Vector3 toDirection = raycastHit.normal;
				float num4 = Vector3.Angle(raycastHit.normal, Vector3.up);
				if (num4 < 15f)
				{
					toDirection = Vector3.up;
				}
				this.invalidReticleTargetRotation = Quaternion.FromToRotation(Vector3.up, toDirection);
				this.invalidReticleTransform.rotation = Quaternion.Slerp(this.invalidReticleTransform.rotation, this.invalidReticleTargetRotation, 0.1f);
				float num5 = Vector3.Distance(raycastHit.point, this.player.hmdTransform.position);
				float num6 = Util.RemapNumberClamped(num5, this.invalidReticleMinScaleDistance, this.invalidReticleMaxScaleDistance, this.invalidReticleMinScale, this.invalidReticleMaxScale);
				this.invalidReticleScale.x = num6;
				this.invalidReticleScale.y = num6;
				this.invalidReticleScale.z = num6;
				this.invalidReticleTransform.transform.localScale = this.invalidReticleScale;
				this.pointedAtTeleportMarker = null;
				if (flag)
				{
					vector2 = raycastHit.point;
				}
				else
				{
					vector2 = this.teleportArc.GetArcPositionAtTime(this.teleportArc.arcDuration);
				}
				if (this.debugFloor)
				{
					this.floorDebugSphere.gameObject.SetActive(false);
					this.floorDebugLine.gameObject.SetActive(false);
				}
			}
			if (this.playAreaPreviewTransform != null)
			{
				this.playAreaPreviewTransform.gameObject.SetActive(active);
			}
			if (!this.showOffsetReticle)
			{
				this.offsetReticleTransform.gameObject.SetActive(false);
			}
			this.destinationReticleTransform.position = this.pointedAtPosition;
			this.invalidReticleTransform.position = vector2;
			this.onActivateObjectTransform.position = vector2;
			this.onDeactivateObjectTransform.position = vector2;
			this.offsetReticleTransform.position = vector2 - vector;
			this.reticleAudioSource.transform.position = this.pointedAtPosition;
			this.pointerLineRenderer.SetPosition(0, position);
			this.pointerLineRenderer.SetPosition(1, vector2);
		}

		// Token: 0x06005E11 RID: 24081 RVA: 0x0020EF14 File Offset: 0x0020D314
		private void FixedUpdate()
		{
			if (!this.visible)
			{
				return;
			}
			if (this.debugFloor)
			{
				TeleportArea x = this.pointedAtTeleportMarker as TeleportArea;
				if (x != null && this.floorFixupMaximumTraceDistance > 0f)
				{
					this.floorDebugSphere.gameObject.SetActive(true);
					this.floorDebugLine.gameObject.SetActive(true);
					Vector3 down = Vector3.down;
					down.x = 0.01f;
					RaycastHit raycastHit;
					if (Physics.Raycast(this.pointedAtPosition + 0.05f * down, down, out raycastHit, this.floorFixupMaximumTraceDistance, this.floorFixupTraceLayerMask))
					{
						this.floorDebugSphere.transform.position = raycastHit.point;
						this.floorDebugSphere.material.color = Color.green;
						this.floorDebugLine.startColor = Color.green;
						this.floorDebugLine.endColor = Color.green;
						this.floorDebugLine.SetPosition(0, this.pointedAtPosition);
						this.floorDebugLine.SetPosition(1, raycastHit.point);
					}
					else
					{
						Vector3 position = this.pointedAtPosition + down * this.floorFixupMaximumTraceDistance;
						this.floorDebugSphere.transform.position = position;
						this.floorDebugSphere.material.color = Color.red;
						this.floorDebugLine.startColor = Color.red;
						this.floorDebugLine.endColor = Color.red;
						this.floorDebugLine.SetPosition(0, this.pointedAtPosition);
						this.floorDebugLine.SetPosition(1, position);
					}
				}
			}
		}

		// Token: 0x06005E12 RID: 24082 RVA: 0x0020F0BC File Offset: 0x0020D4BC
		private void OnChaperoneInfoInitialized()
		{
			ChaperoneInfo instance = ChaperoneInfo.instance;
			if (instance.initialized && instance.roomscale)
			{
				if (this.playAreaPreviewTransform == null)
				{
					this.playAreaPreviewTransform = new GameObject("PlayAreaPreviewTransform").transform;
					this.playAreaPreviewTransform.parent = base.transform;
					Util.ResetTransform(this.playAreaPreviewTransform, true);
					this.playAreaPreviewCorner.SetActive(true);
					this.playAreaPreviewCorners = new Transform[4];
					this.playAreaPreviewCorners[0] = this.playAreaPreviewCorner.transform;
					this.playAreaPreviewCorners[1] = UnityEngine.Object.Instantiate<Transform>(this.playAreaPreviewCorners[0]);
					this.playAreaPreviewCorners[2] = UnityEngine.Object.Instantiate<Transform>(this.playAreaPreviewCorners[0]);
					this.playAreaPreviewCorners[3] = UnityEngine.Object.Instantiate<Transform>(this.playAreaPreviewCorners[0]);
					this.playAreaPreviewCorners[0].transform.parent = this.playAreaPreviewTransform;
					this.playAreaPreviewCorners[1].transform.parent = this.playAreaPreviewTransform;
					this.playAreaPreviewCorners[2].transform.parent = this.playAreaPreviewTransform;
					this.playAreaPreviewCorners[3].transform.parent = this.playAreaPreviewTransform;
					this.playAreaPreviewSide.SetActive(true);
					this.playAreaPreviewSides = new Transform[4];
					this.playAreaPreviewSides[0] = this.playAreaPreviewSide.transform;
					this.playAreaPreviewSides[1] = UnityEngine.Object.Instantiate<Transform>(this.playAreaPreviewSides[0]);
					this.playAreaPreviewSides[2] = UnityEngine.Object.Instantiate<Transform>(this.playAreaPreviewSides[0]);
					this.playAreaPreviewSides[3] = UnityEngine.Object.Instantiate<Transform>(this.playAreaPreviewSides[0]);
					this.playAreaPreviewSides[0].transform.parent = this.playAreaPreviewTransform;
					this.playAreaPreviewSides[1].transform.parent = this.playAreaPreviewTransform;
					this.playAreaPreviewSides[2].transform.parent = this.playAreaPreviewTransform;
					this.playAreaPreviewSides[3].transform.parent = this.playAreaPreviewTransform;
				}
				float playAreaSizeX = instance.playAreaSizeX;
				float playAreaSizeZ = instance.playAreaSizeZ;
				this.playAreaPreviewSides[0].localPosition = new Vector3(0f, 0f, 0.5f * playAreaSizeZ - 0.25f);
				this.playAreaPreviewSides[1].localPosition = new Vector3(0f, 0f, -0.5f * playAreaSizeZ + 0.25f);
				this.playAreaPreviewSides[2].localPosition = new Vector3(0.5f * playAreaSizeX - 0.25f, 0f, 0f);
				this.playAreaPreviewSides[3].localPosition = new Vector3(-0.5f * playAreaSizeX + 0.25f, 0f, 0f);
				this.playAreaPreviewSides[0].localScale = new Vector3(playAreaSizeX - 0.5f, 1f, 1f);
				this.playAreaPreviewSides[1].localScale = new Vector3(playAreaSizeX - 0.5f, 1f, 1f);
				this.playAreaPreviewSides[2].localScale = new Vector3(playAreaSizeZ - 0.5f, 1f, 1f);
				this.playAreaPreviewSides[3].localScale = new Vector3(playAreaSizeZ - 0.5f, 1f, 1f);
				this.playAreaPreviewSides[0].localRotation = Quaternion.Euler(0f, 0f, 0f);
				this.playAreaPreviewSides[1].localRotation = Quaternion.Euler(0f, 180f, 0f);
				this.playAreaPreviewSides[2].localRotation = Quaternion.Euler(0f, 90f, 0f);
				this.playAreaPreviewSides[3].localRotation = Quaternion.Euler(0f, 270f, 0f);
				this.playAreaPreviewCorners[0].localPosition = new Vector3(0.5f * playAreaSizeX - 0.25f, 0f, 0.5f * playAreaSizeZ - 0.25f);
				this.playAreaPreviewCorners[1].localPosition = new Vector3(0.5f * playAreaSizeX - 0.25f, 0f, -0.5f * playAreaSizeZ + 0.25f);
				this.playAreaPreviewCorners[2].localPosition = new Vector3(-0.5f * playAreaSizeX + 0.25f, 0f, -0.5f * playAreaSizeZ + 0.25f);
				this.playAreaPreviewCorners[3].localPosition = new Vector3(-0.5f * playAreaSizeX + 0.25f, 0f, 0.5f * playAreaSizeZ - 0.25f);
				this.playAreaPreviewCorners[0].localRotation = Quaternion.Euler(0f, 0f, 0f);
				this.playAreaPreviewCorners[1].localRotation = Quaternion.Euler(0f, 90f, 0f);
				this.playAreaPreviewCorners[2].localRotation = Quaternion.Euler(0f, 180f, 0f);
				this.playAreaPreviewCorners[3].localRotation = Quaternion.Euler(0f, 270f, 0f);
				this.playAreaPreviewTransform.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005E13 RID: 24083 RVA: 0x0020F5D8 File Offset: 0x0020D9D8
		private void HidePointer()
		{
			if (this.visible)
			{
				this.pointerHideStartTime = Time.time;
			}
			this.visible = false;
			if (this.pointerHand)
			{
				if (this.ShouldOverrideHoverLock())
				{
					if (this.originalHoverLockState)
					{
						this.pointerHand.HoverLock(this.originalHoveringInteractable);
					}
					else
					{
						this.pointerHand.HoverUnlock(null);
					}
				}
				this.loopingAudioSource.Stop();
				this.PlayAudioClip(this.pointerAudioSource, this.pointerStopSound);
			}
			this.teleportPointerObject.SetActive(false);
			this.teleportArc.Hide();
			foreach (TeleportMarkerBase teleportMarkerBase in this.teleportMarkers)
			{
				if (teleportMarkerBase != null && teleportMarkerBase.markerActive && teleportMarkerBase.gameObject != null)
				{
					teleportMarkerBase.gameObject.SetActive(false);
				}
			}
			this.destinationReticleTransform.gameObject.SetActive(false);
			this.invalidReticleTransform.gameObject.SetActive(false);
			this.offsetReticleTransform.gameObject.SetActive(false);
			if (this.playAreaPreviewTransform != null)
			{
				this.playAreaPreviewTransform.gameObject.SetActive(false);
			}
			if (this.onActivateObjectTransform.gameObject.activeSelf)
			{
				this.onActivateObjectTransform.gameObject.SetActive(false);
			}
			this.onDeactivateObjectTransform.gameObject.SetActive(true);
			this.pointerHand = null;
		}

		// Token: 0x06005E14 RID: 24084 RVA: 0x0020F764 File Offset: 0x0020DB64
		private void ShowPointer(Hand newPointerHand, Hand oldPointerHand)
		{
			if (!this.visible)
			{
				this.pointedAtTeleportMarker = null;
				this.pointerShowStartTime = Time.time;
				this.visible = true;
				this.meshFading = true;
				this.teleportPointerObject.SetActive(false);
				this.teleportArc.Show();
				foreach (TeleportMarkerBase teleportMarkerBase in this.teleportMarkers)
				{
					if (teleportMarkerBase.markerActive && teleportMarkerBase.ShouldActivate(this.player.feetPositionGuess))
					{
						teleportMarkerBase.gameObject.SetActive(true);
						teleportMarkerBase.Highlight(false);
					}
				}
				this.startingFeetOffset = this.player.trackingOriginTransform.position - this.player.feetPositionGuess;
				this.movedFeetFarEnough = false;
				if (this.onDeactivateObjectTransform.gameObject.activeSelf)
				{
					this.onDeactivateObjectTransform.gameObject.SetActive(false);
				}
				this.onActivateObjectTransform.gameObject.SetActive(true);
				this.loopingAudioSource.clip = this.pointerLoopSound;
				this.loopingAudioSource.loop = true;
				this.loopingAudioSource.Play();
				this.loopingAudioSource.volume = 0f;
			}
			if (oldPointerHand && this.ShouldOverrideHoverLock())
			{
				if (this.originalHoverLockState)
				{
					oldPointerHand.HoverLock(this.originalHoveringInteractable);
				}
				else
				{
					oldPointerHand.HoverUnlock(null);
				}
			}
			this.pointerHand = newPointerHand;
			if (this.visible && oldPointerHand != this.pointerHand)
			{
				this.PlayAudioClip(this.pointerAudioSource, this.pointerStartSound);
			}
			if (this.pointerHand)
			{
				this.pointerStartTransform = this.GetPointerStartTransform(this.pointerHand);
				if (this.pointerHand.currentAttachedObject != null)
				{
					this.allowTeleportWhileAttached = this.pointerHand.currentAttachedObject.GetComponent<AllowTeleportWhileAttachedToHand>();
				}
				this.originalHoverLockState = this.pointerHand.hoverLocked;
				this.originalHoveringInteractable = this.pointerHand.hoveringInteractable;
				if (this.ShouldOverrideHoverLock())
				{
					this.pointerHand.HoverLock(null);
				}
				this.pointerAudioSource.transform.SetParent(this.pointerStartTransform);
				this.pointerAudioSource.transform.localPosition = Vector3.zero;
				this.loopingAudioSource.transform.SetParent(this.pointerStartTransform);
				this.loopingAudioSource.transform.localPosition = Vector3.zero;
			}
		}

		// Token: 0x06005E15 RID: 24085 RVA: 0x0020F9F0 File Offset: 0x0020DDF0
		private void UpdateTeleportColors()
		{
			float num = Time.time - this.pointerShowStartTime;
			if (num > this.meshFadeTime)
			{
				this.meshAlphaPercent = 1f;
				this.meshFading = false;
			}
			else
			{
				this.meshAlphaPercent = Mathf.Lerp(0f, 1f, num / this.meshFadeTime);
			}
			foreach (TeleportMarkerBase teleportMarkerBase in this.teleportMarkers)
			{
				teleportMarkerBase.SetAlpha(this.fullTintAlpha * this.meshAlphaPercent, this.meshAlphaPercent);
			}
		}

		// Token: 0x06005E16 RID: 24086 RVA: 0x0020FA82 File Offset: 0x0020DE82
		private void PlayAudioClip(AudioSource source, AudioClip clip)
		{
			source.clip = clip;
			source.Play();
		}

		// Token: 0x06005E17 RID: 24087 RVA: 0x0020FA94 File Offset: 0x0020DE94
		private void PlayPointerHaptic(bool validLocation)
		{
			if (this.pointerHand.controller != null)
			{
				if (validLocation)
				{
					this.pointerHand.controller.TriggerHapticPulse(800, EVRButtonId.k_EButton_Axis0);
				}
				else
				{
					this.pointerHand.controller.TriggerHapticPulse(100, EVRButtonId.k_EButton_Axis0);
				}
			}
		}

		// Token: 0x06005E18 RID: 24088 RVA: 0x0020FAE8 File Offset: 0x0020DEE8
		private void TryTeleportPlayer()
		{
			if (this.visible && !this.teleporting && this.pointedAtTeleportMarker != null && !this.pointedAtTeleportMarker.locked)
			{
				this.teleportingToMarker = this.pointedAtTeleportMarker;
				this.InitiateTeleportFade();
				this.CancelTeleportHint();
			}
		}

		// Token: 0x06005E19 RID: 24089 RVA: 0x0020FB44 File Offset: 0x0020DF44
		private void InitiateTeleportFade()
		{
			this.teleporting = true;
			this.currentFadeTime = this.teleportFadeTime;
			TeleportPoint teleportPoint = this.teleportingToMarker as TeleportPoint;
			if (teleportPoint != null && teleportPoint.teleportType == TeleportPoint.TeleportPointType.SwitchToNewScene)
			{
				this.currentFadeTime *= 3f;
				Teleport.ChangeScene.Send(this.currentFadeTime);
			}
			SteamVR_Fade.Start(Color.clear, 0f, false);
			SteamVR_Fade.Start(Color.black, this.currentFadeTime, false);
			this.headAudioSource.transform.SetParent(this.player.hmdTransform);
			this.headAudioSource.transform.localPosition = Vector3.zero;
			this.PlayAudioClip(this.headAudioSource, this.teleportSound);
			base.Invoke("TeleportPlayer", this.currentFadeTime);
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x0020FC20 File Offset: 0x0020E020
		private void TeleportPlayer()
		{
			this.teleporting = false;
			Teleport.PlayerPre.Send(this.pointedAtTeleportMarker);
			SteamVR_Fade.Start(Color.clear, this.currentFadeTime, false);
			TeleportPoint teleportPoint = this.teleportingToMarker as TeleportPoint;
			Vector3 a = this.pointedAtPosition;
			if (teleportPoint != null)
			{
				a = teleportPoint.transform.position;
				if (teleportPoint.teleportType == TeleportPoint.TeleportPointType.SwitchToNewScene)
				{
					teleportPoint.TeleportToScene();
					return;
				}
			}
			TeleportArea x = this.teleportingToMarker as TeleportArea;
			RaycastHit raycastHit;
			if (x != null && this.floorFixupMaximumTraceDistance > 0f && Physics.Raycast(a + 0.05f * Vector3.down, Vector3.down, out raycastHit, this.floorFixupMaximumTraceDistance, this.floorFixupTraceLayerMask))
			{
				a = raycastHit.point;
			}
			if (this.teleportingToMarker.ShouldMovePlayer())
			{
				Vector3 b = this.player.trackingOriginTransform.position - this.player.feetPositionGuess;
				this.player.trackingOriginTransform.position = a + b;
			}
			else
			{
				this.teleportingToMarker.TeleportPlayer(this.pointedAtPosition);
			}
			Teleport.Player.Send(this.pointedAtTeleportMarker);
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x0020FD6C File Offset: 0x0020E16C
		private void HighlightSelected(TeleportMarkerBase hitTeleportMarker)
		{
			if (this.pointedAtTeleportMarker != hitTeleportMarker)
			{
				if (this.pointedAtTeleportMarker != null)
				{
					this.pointedAtTeleportMarker.Highlight(false);
				}
				if (hitTeleportMarker != null)
				{
					hitTeleportMarker.Highlight(true);
					this.prevPointedAtPosition = this.pointedAtPosition;
					this.PlayPointerHaptic(!hitTeleportMarker.locked);
					this.PlayAudioClip(this.reticleAudioSource, this.goodHighlightSound);
					this.loopingAudioSource.volume = this.loopingAudioMaxVolume;
				}
				else if (this.pointedAtTeleportMarker != null)
				{
					this.PlayAudioClip(this.reticleAudioSource, this.badHighlightSound);
					this.loopingAudioSource.volume = 0f;
				}
			}
			else if (hitTeleportMarker != null && Vector3.Distance(this.prevPointedAtPosition, this.pointedAtPosition) > 1f)
			{
				this.prevPointedAtPosition = this.pointedAtPosition;
				this.PlayPointerHaptic(!hitTeleportMarker.locked);
			}
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x0020FE77 File Offset: 0x0020E277
		public void ShowTeleportHint()
		{
			this.CancelTeleportHint();
			this.hintCoroutine = base.StartCoroutine(this.TeleportHintCoroutine());
		}

		// Token: 0x06005E1D RID: 24093 RVA: 0x0020FE94 File Offset: 0x0020E294
		public void CancelTeleportHint()
		{
			if (this.hintCoroutine != null)
			{
				foreach (Hand hand in this.player.hands)
				{
					ControllerButtonHints.HideTextHint(hand, EVRButtonId.k_EButton_Axis0);
				}
				base.StopCoroutine(this.hintCoroutine);
				this.hintCoroutine = null;
			}
			base.CancelInvoke("ShowTeleportHint");
		}

		// Token: 0x06005E1E RID: 24094 RVA: 0x0020FEF8 File Offset: 0x0020E2F8
		private IEnumerator TeleportHintCoroutine()
		{
			float prevBreakTime = Time.time;
			float prevHapticPulseTime = Time.time;
			for (;;)
			{
				bool pulsed = false;
				foreach (Hand hand in this.player.hands)
				{
					bool flag = this.IsEligibleForTeleport(hand);
					bool flag2 = !string.IsNullOrEmpty(ControllerButtonHints.GetActiveHintText(hand, EVRButtonId.k_EButton_Axis0));
					if (flag)
					{
						if (!flag2)
						{
							ControllerButtonHints.ShowTextHint(hand, EVRButtonId.k_EButton_Axis0, "Teleport", true);
							prevBreakTime = Time.time;
							prevHapticPulseTime = Time.time;
						}
						if (Time.time > prevHapticPulseTime + 0.05f)
						{
							pulsed = true;
							hand.controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_Axis0);
						}
					}
					else if (!flag && flag2)
					{
						ControllerButtonHints.HideTextHint(hand, EVRButtonId.k_EButton_Axis0);
					}
				}
				if (Time.time > prevBreakTime + 3f)
				{
					yield return new WaitForSeconds(3f);
					prevBreakTime = Time.time;
				}
				if (pulsed)
				{
					prevHapticPulseTime = Time.time;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005E1F RID: 24095 RVA: 0x0020FF14 File Offset: 0x0020E314
		public bool IsEligibleForTeleport(Hand hand)
		{
			if (hand == null)
			{
				return false;
			}
			if (!hand.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (hand.hoveringInteractable != null)
			{
				return false;
			}
			if (hand.noSteamVRFallbackCamera == null)
			{
				if (hand.controller == null)
				{
					return false;
				}
				if (hand.currentAttachedObject != null)
				{
					AllowTeleportWhileAttachedToHand component = hand.currentAttachedObject.GetComponent<AllowTeleportWhileAttachedToHand>();
					return component != null && component.teleportAllowed;
				}
			}
			return true;
		}

		// Token: 0x06005E20 RID: 24096 RVA: 0x0020FFAB File Offset: 0x0020E3AB
		private bool ShouldOverrideHoverLock()
		{
			return !this.allowTeleportWhileAttached || this.allowTeleportWhileAttached.overrideHoverLock;
		}

		// Token: 0x06005E21 RID: 24097 RVA: 0x0020FFD0 File Offset: 0x0020E3D0
		private bool WasTeleportButtonReleased(Hand hand)
		{
			if (!this.IsEligibleForTeleport(hand))
			{
				return false;
			}
			if (hand.noSteamVRFallbackCamera != null)
			{
				return Input.GetKeyUp(KeyCode.T);
			}
			return hand.controller.GetPressUp(4294967296UL);
		}

		// Token: 0x06005E22 RID: 24098 RVA: 0x0021000D File Offset: 0x0020E40D
		private bool IsTeleportButtonDown(Hand hand)
		{
			if (!this.IsEligibleForTeleport(hand))
			{
				return false;
			}
			if (hand.noSteamVRFallbackCamera != null)
			{
				return Input.GetKey(KeyCode.T);
			}
			return hand.controller.GetPress(4294967296UL);
		}

		// Token: 0x06005E23 RID: 24099 RVA: 0x0021004A File Offset: 0x0020E44A
		private bool WasTeleportButtonPressed(Hand hand)
		{
			if (!this.IsEligibleForTeleport(hand))
			{
				return false;
			}
			if (hand.noSteamVRFallbackCamera != null)
			{
				return Input.GetKeyDown(KeyCode.T);
			}
			return hand.controller.GetPressDown(4294967296UL);
		}

		// Token: 0x06005E24 RID: 24100 RVA: 0x00210087 File Offset: 0x0020E487
		private Transform GetPointerStartTransform(Hand hand)
		{
			if (hand.noSteamVRFallbackCamera != null)
			{
				return hand.noSteamVRFallbackCamera.transform;
			}
			return this.pointerHand.GetAttachmentTransform("Attach_ControllerTip");
		}

		// Token: 0x040043B0 RID: 17328
		public LayerMask traceLayerMask;

		// Token: 0x040043B1 RID: 17329
		public LayerMask floorFixupTraceLayerMask;

		// Token: 0x040043B2 RID: 17330
		public float floorFixupMaximumTraceDistance = 1f;

		// Token: 0x040043B3 RID: 17331
		public Material areaVisibleMaterial;

		// Token: 0x040043B4 RID: 17332
		public Material areaLockedMaterial;

		// Token: 0x040043B5 RID: 17333
		public Material areaHighlightedMaterial;

		// Token: 0x040043B6 RID: 17334
		public Material pointVisibleMaterial;

		// Token: 0x040043B7 RID: 17335
		public Material pointLockedMaterial;

		// Token: 0x040043B8 RID: 17336
		public Material pointHighlightedMaterial;

		// Token: 0x040043B9 RID: 17337
		public Transform destinationReticleTransform;

		// Token: 0x040043BA RID: 17338
		public Transform invalidReticleTransform;

		// Token: 0x040043BB RID: 17339
		public GameObject playAreaPreviewCorner;

		// Token: 0x040043BC RID: 17340
		public GameObject playAreaPreviewSide;

		// Token: 0x040043BD RID: 17341
		public Color pointerValidColor;

		// Token: 0x040043BE RID: 17342
		public Color pointerInvalidColor;

		// Token: 0x040043BF RID: 17343
		public Color pointerLockedColor;

		// Token: 0x040043C0 RID: 17344
		public bool showPlayAreaMarker = true;

		// Token: 0x040043C1 RID: 17345
		public float teleportFadeTime = 0.1f;

		// Token: 0x040043C2 RID: 17346
		public float meshFadeTime = 0.2f;

		// Token: 0x040043C3 RID: 17347
		public float arcDistance = 10f;

		// Token: 0x040043C4 RID: 17348
		[Header("Effects")]
		public Transform onActivateObjectTransform;

		// Token: 0x040043C5 RID: 17349
		public Transform onDeactivateObjectTransform;

		// Token: 0x040043C6 RID: 17350
		public float activateObjectTime = 1f;

		// Token: 0x040043C7 RID: 17351
		public float deactivateObjectTime = 1f;

		// Token: 0x040043C8 RID: 17352
		[Header("Audio Sources")]
		public AudioSource pointerAudioSource;

		// Token: 0x040043C9 RID: 17353
		public AudioSource loopingAudioSource;

		// Token: 0x040043CA RID: 17354
		public AudioSource headAudioSource;

		// Token: 0x040043CB RID: 17355
		public AudioSource reticleAudioSource;

		// Token: 0x040043CC RID: 17356
		[Header("Sounds")]
		public AudioClip teleportSound;

		// Token: 0x040043CD RID: 17357
		public AudioClip pointerStartSound;

		// Token: 0x040043CE RID: 17358
		public AudioClip pointerLoopSound;

		// Token: 0x040043CF RID: 17359
		public AudioClip pointerStopSound;

		// Token: 0x040043D0 RID: 17360
		public AudioClip goodHighlightSound;

		// Token: 0x040043D1 RID: 17361
		public AudioClip badHighlightSound;

		// Token: 0x040043D2 RID: 17362
		[Header("Debug")]
		public bool debugFloor;

		// Token: 0x040043D3 RID: 17363
		public bool showOffsetReticle;

		// Token: 0x040043D4 RID: 17364
		public Transform offsetReticleTransform;

		// Token: 0x040043D5 RID: 17365
		public MeshRenderer floorDebugSphere;

		// Token: 0x040043D6 RID: 17366
		public LineRenderer floorDebugLine;

		// Token: 0x040043D7 RID: 17367
		private LineRenderer pointerLineRenderer;

		// Token: 0x040043D8 RID: 17368
		private GameObject teleportPointerObject;

		// Token: 0x040043D9 RID: 17369
		private Transform pointerStartTransform;

		// Token: 0x040043DA RID: 17370
		private Hand pointerHand;

		// Token: 0x040043DB RID: 17371
		private Player player;

		// Token: 0x040043DC RID: 17372
		private TeleportArc teleportArc;

		// Token: 0x040043DD RID: 17373
		private bool visible;

		// Token: 0x040043DE RID: 17374
		private TeleportMarkerBase[] teleportMarkers;

		// Token: 0x040043DF RID: 17375
		private TeleportMarkerBase pointedAtTeleportMarker;

		// Token: 0x040043E0 RID: 17376
		private TeleportMarkerBase teleportingToMarker;

		// Token: 0x040043E1 RID: 17377
		private Vector3 pointedAtPosition;

		// Token: 0x040043E2 RID: 17378
		private Vector3 prevPointedAtPosition;

		// Token: 0x040043E3 RID: 17379
		private bool teleporting;

		// Token: 0x040043E4 RID: 17380
		private float currentFadeTime;

		// Token: 0x040043E5 RID: 17381
		private float meshAlphaPercent = 1f;

		// Token: 0x040043E6 RID: 17382
		private float pointerShowStartTime;

		// Token: 0x040043E7 RID: 17383
		private float pointerHideStartTime;

		// Token: 0x040043E8 RID: 17384
		private bool meshFading;

		// Token: 0x040043E9 RID: 17385
		private float fullTintAlpha;

		// Token: 0x040043EA RID: 17386
		private float invalidReticleMinScale = 0.2f;

		// Token: 0x040043EB RID: 17387
		private float invalidReticleMaxScale = 1f;

		// Token: 0x040043EC RID: 17388
		private float invalidReticleMinScaleDistance = 0.4f;

		// Token: 0x040043ED RID: 17389
		private float invalidReticleMaxScaleDistance = 2f;

		// Token: 0x040043EE RID: 17390
		private Vector3 invalidReticleScale = Vector3.one;

		// Token: 0x040043EF RID: 17391
		private Quaternion invalidReticleTargetRotation = Quaternion.identity;

		// Token: 0x040043F0 RID: 17392
		private Transform playAreaPreviewTransform;

		// Token: 0x040043F1 RID: 17393
		private Transform[] playAreaPreviewCorners;

		// Token: 0x040043F2 RID: 17394
		private Transform[] playAreaPreviewSides;

		// Token: 0x040043F3 RID: 17395
		private float loopingAudioMaxVolume;

		// Token: 0x040043F4 RID: 17396
		private Coroutine hintCoroutine;

		// Token: 0x040043F5 RID: 17397
		private bool originalHoverLockState;

		// Token: 0x040043F6 RID: 17398
		private Interactable originalHoveringInteractable;

		// Token: 0x040043F7 RID: 17399
		private AllowTeleportWhileAttachedToHand allowTeleportWhileAttached;

		// Token: 0x040043F8 RID: 17400
		private Vector3 startingFeetOffset = Vector3.zero;

		// Token: 0x040043F9 RID: 17401
		private bool movedFeetFarEnough;

		// Token: 0x040043FA RID: 17402
		private SteamVR_Events.Action chaperoneInfoInitializedAction;

		// Token: 0x040043FB RID: 17403
		public static SteamVR_Events.Event<float> ChangeScene = new SteamVR_Events.Event<float>();

		// Token: 0x040043FC RID: 17404
		public static SteamVR_Events.Event<TeleportMarkerBase> Player = new SteamVR_Events.Event<TeleportMarkerBase>();

		// Token: 0x040043FD RID: 17405
		public static SteamVR_Events.Event<TeleportMarkerBase> PlayerPre = new SteamVR_Events.Event<TeleportMarkerBase>();

		// Token: 0x040043FE RID: 17406
		private static Teleport _instance;
	}
}
