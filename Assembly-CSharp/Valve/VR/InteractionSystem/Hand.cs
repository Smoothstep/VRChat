using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BA4 RID: 2980
	public class Hand : MonoBehaviour
	{
		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06005C70 RID: 23664 RVA: 0x0020496D File Offset: 0x00202D6D
		public ReadOnlyCollection<Hand.AttachedObject> AttachedObjects
		{
			get
			{
				return this.attachedObjects.AsReadOnly();
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06005C71 RID: 23665 RVA: 0x0020497A File Offset: 0x00202D7A
		// (set) Token: 0x06005C72 RID: 23666 RVA: 0x00204982 File Offset: 0x00202D82
		public bool hoverLocked { get; private set; }

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06005C73 RID: 23667 RVA: 0x0020498B File Offset: 0x00202D8B
		// (set) Token: 0x06005C74 RID: 23668 RVA: 0x00204994 File Offset: 0x00202D94
		public Interactable hoveringInteractable
		{
			get
			{
				return this._hoveringInteractable;
			}
			set
			{
				if (this._hoveringInteractable != value)
				{
					if (this._hoveringInteractable != null)
					{
						this.HandDebugLog("HoverEnd " + this._hoveringInteractable.gameObject);
						this._hoveringInteractable.SendMessage("OnHandHoverEnd", this, SendMessageOptions.DontRequireReceiver);
						if (this._hoveringInteractable != null)
						{
							base.BroadcastMessage("OnParentHandHoverEnd", this._hoveringInteractable, SendMessageOptions.DontRequireReceiver);
						}
					}
					this._hoveringInteractable = value;
					if (this._hoveringInteractable != null)
					{
						this.HandDebugLog("HoverBegin " + this._hoveringInteractable.gameObject);
						this._hoveringInteractable.SendMessage("OnHandHoverBegin", this, SendMessageOptions.DontRequireReceiver);
						if (this._hoveringInteractable != null)
						{
							base.BroadcastMessage("OnParentHandHoverBegin", this._hoveringInteractable, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06005C75 RID: 23669 RVA: 0x00204A7C File Offset: 0x00202E7C
		public GameObject currentAttachedObject
		{
			get
			{
				this.CleanUpAttachedObjectStack();
				if (this.attachedObjects.Count > 0)
				{
					return this.attachedObjects[this.attachedObjects.Count - 1].attachedObject;
				}
				return null;
			}
		}

		// Token: 0x06005C76 RID: 23670 RVA: 0x00204AC4 File Offset: 0x00202EC4
		public Transform GetAttachmentTransform(string attachmentPoint = "")
		{
			Transform transform = null;
			if (!string.IsNullOrEmpty(attachmentPoint))
			{
				transform = base.transform.Find(attachmentPoint);
			}
			if (!transform)
			{
				transform = base.transform;
			}
			return transform;
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x00204B00 File Offset: 0x00202F00
		public Hand.HandType GuessCurrentHandType()
		{
			if (this.startingHandType == Hand.HandType.Left || this.startingHandType == Hand.HandType.Right)
			{
				return this.startingHandType;
			}
			if (this.startingHandType == Hand.HandType.Any && this.otherHand != null && this.otherHand.controller == null)
			{
				return Hand.HandType.Right;
			}
			if (this.controller == null || this.otherHand == null || this.otherHand.controller == null)
			{
				return this.startingHandType;
			}
			if ((ulong)this.controller.index == (ulong)((long)SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost, ETrackedDeviceClass.Controller, 0)))
			{
				return Hand.HandType.Left;
			}
			return Hand.HandType.Right;
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x00204BAC File Offset: 0x00202FAC
		public void AttachObject(GameObject objectToAttach, Hand.AttachmentFlags flags = Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand, string attachmentPoint = "")
		{
			if (flags == (Hand.AttachmentFlags)0)
			{
				flags = (Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand);
			}
			this.CleanUpAttachedObjectStack();
			this.DetachObject(objectToAttach, true);
			if ((flags & Hand.AttachmentFlags.DetachFromOtherHand) == Hand.AttachmentFlags.DetachFromOtherHand && this.otherHand)
			{
				this.otherHand.DetachObject(objectToAttach, true);
			}
			if ((flags & Hand.AttachmentFlags.DetachOthers) == Hand.AttachmentFlags.DetachOthers)
			{
				while (this.attachedObjects.Count > 0)
				{
					this.DetachObject(this.attachedObjects[0].attachedObject, true);
				}
			}
			if (this.currentAttachedObject)
			{
				this.currentAttachedObject.SendMessage("OnHandFocusLost", this, SendMessageOptions.DontRequireReceiver);
			}
			Hand.AttachedObject item = default(Hand.AttachedObject);
			item.attachedObject = objectToAttach;
			item.originalParent = ((!(objectToAttach.transform.parent != null)) ? null : objectToAttach.transform.parent.gameObject);
			if ((flags & Hand.AttachmentFlags.ParentToHand) == Hand.AttachmentFlags.ParentToHand)
			{
				objectToAttach.transform.parent = this.GetAttachmentTransform(attachmentPoint);
				item.isParentedToHand = true;
			}
			else
			{
				item.isParentedToHand = false;
			}
			this.attachedObjects.Add(item);
			if ((flags & Hand.AttachmentFlags.SnapOnAttach) == Hand.AttachmentFlags.SnapOnAttach)
			{
				objectToAttach.transform.localPosition = Vector3.zero;
				objectToAttach.transform.localRotation = Quaternion.identity;
			}
			this.HandDebugLog("AttachObject " + objectToAttach);
			objectToAttach.SendMessage("OnAttachedToHand", this, SendMessageOptions.DontRequireReceiver);
			this.UpdateHovering();
		}

		// Token: 0x06005C79 RID: 23673 RVA: 0x00204D20 File Offset: 0x00203120
		public void DetachObject(GameObject objectToDetach, bool restoreOriginalParent = true)
		{
			int num = this.attachedObjects.FindIndex((Hand.AttachedObject l) => l.attachedObject == objectToDetach);
			if (num != -1)
			{
				this.HandDebugLog("DetachObject " + objectToDetach);
				GameObject currentAttachedObject = this.currentAttachedObject;
				Transform parent = null;
				if (this.attachedObjects[num].isParentedToHand)
				{
					if (restoreOriginalParent && this.attachedObjects[num].originalParent != null)
					{
						parent = this.attachedObjects[num].originalParent.transform;
					}
					this.attachedObjects[num].attachedObject.transform.parent = parent;
				}
				this.attachedObjects[num].attachedObject.SetActive(true);
				this.attachedObjects[num].attachedObject.SendMessage("OnDetachedFromHand", this, SendMessageOptions.DontRequireReceiver);
				this.attachedObjects.RemoveAt(num);
				GameObject currentAttachedObject2 = this.currentAttachedObject;
				if (currentAttachedObject2 != null && currentAttachedObject2 != currentAttachedObject)
				{
					currentAttachedObject2.SetActive(true);
					currentAttachedObject2.SendMessage("OnHandFocusAcquired", this, SendMessageOptions.DontRequireReceiver);
				}
			}
			this.CleanUpAttachedObjectStack();
		}

		// Token: 0x06005C7A RID: 23674 RVA: 0x00204E79 File Offset: 0x00203279
		public Vector3 GetTrackedObjectVelocity()
		{
			if (this.controller != null)
			{
				return base.transform.parent.TransformVector(this.controller.velocity);
			}
			return Vector3.zero;
		}

		// Token: 0x06005C7B RID: 23675 RVA: 0x00204EA7 File Offset: 0x002032A7
		public Vector3 GetTrackedObjectAngularVelocity()
		{
			if (this.controller != null)
			{
				return base.transform.parent.TransformVector(this.controller.angularVelocity);
			}
			return Vector3.zero;
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x00204ED5 File Offset: 0x002032D5
		private void CleanUpAttachedObjectStack()
		{
			this.attachedObjects.RemoveAll((Hand.AttachedObject l) => l.attachedObject == null);
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x00204F00 File Offset: 0x00203300
		private void Awake()
		{
			this.inputFocusAction = SteamVR_Events.InputFocusAction(new UnityAction<bool>(this.OnInputFocus));
			if (this.hoverSphereTransform == null)
			{
				this.hoverSphereTransform = base.transform;
			}
			this.applicationLostFocusObject = new GameObject("_application_lost_focus");
			this.applicationLostFocusObject.transform.parent = base.transform;
			this.applicationLostFocusObject.SetActive(false);
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x00204F74 File Offset: 0x00203374
		private IEnumerator Start()
		{
			this.playerInstance = Player.instance;
			if (!this.playerInstance)
			{
				Debug.LogError("No player instance found in Hand Start()");
			}
			this.overlappingColliders = new Collider[16];
			if (this.noSteamVRFallbackCamera)
			{
				yield break;
			}
			for (;;)
			{
				yield return new WaitForSeconds(1f);
				if (this.controller != null)
				{
					break;
				}
				if (this.startingHandType == Hand.HandType.Left || this.startingHandType == Hand.HandType.Right)
				{
					int deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost, ETrackedDeviceClass.Controller, 0);
					int deviceIndex2 = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost, ETrackedDeviceClass.Controller, 0);
					if (deviceIndex != -1 && deviceIndex2 != -1 && deviceIndex != deviceIndex2)
					{
						int index = (this.startingHandType != Hand.HandType.Right) ? deviceIndex : deviceIndex2;
						int index2 = (this.startingHandType != Hand.HandType.Right) ? deviceIndex2 : deviceIndex;
						this.InitController(index);
						if (this.otherHand)
						{
							this.otherHand.InitController(index2);
						}
					}
				}
				else
				{
					SteamVR instance = SteamVR.instance;
					int num = 0;
					while ((long)num < 16L)
					{
						if (instance.hmd.GetTrackedDeviceClass((uint)num) == ETrackedDeviceClass.Controller)
						{
							SteamVR_Controller.Device device = SteamVR_Controller.Input(num);
							if (device.valid)
							{
								if (!(this.otherHand != null) || this.otherHand.controller == null || num != (int)this.otherHand.controller.index)
								{
									this.InitController(num);
								}
							}
						}
						num++;
					}
				}
			}
			yield break;
		}

		// Token: 0x06005C7F RID: 23679 RVA: 0x00204F90 File Offset: 0x00203390
		private void UpdateHovering()
		{
			if (this.noSteamVRFallbackCamera == null && this.controller == null)
			{
				return;
			}
			if (this.hoverLocked)
			{
				return;
			}
			if (this.applicationLostFocusObject.activeSelf)
			{
				return;
			}
			float num = float.MaxValue;
			Interactable hoveringInteractable = null;
			float x = this.playerInstance.transform.lossyScale.x;
			float num2 = this.hoverSphereRadius * x;
			float num3 = Mathf.Abs(base.transform.position.y - this.playerInstance.trackingOriginTransform.position.y);
			float num4 = Util.RemapNumberClamped(num3, 0f, 0.5f * x, 5f, 1f) * x;
			for (int i = 0; i < this.overlappingColliders.Length; i++)
			{
				this.overlappingColliders[i] = null;
			}
			Physics.OverlapBoxNonAlloc(this.hoverSphereTransform.position - new Vector3(0f, num2 * num4 - num2, 0f), new Vector3(num2, num2 * num4 * 2f, num2), this.overlappingColliders, Quaternion.identity, this.hoverLayerMask.value);
			int num5 = 0;
			Collider[] array = this.overlappingColliders;
			for (int j = 0; j < array.Length; j++)
			{
				Collider collider = array[j];
				if (!(collider == null))
				{
					Interactable contacting = collider.GetComponentInParent<Interactable>();
					if (!(contacting == null))
					{
						IgnoreHovering component = collider.GetComponent<IgnoreHovering>();
						if (!(component != null) || (!(component.onlyIgnoreHand == null) && !(component.onlyIgnoreHand == this)))
						{
							if (this.attachedObjects.FindIndex((Hand.AttachedObject l) => l.attachedObject == contacting.gameObject) == -1)
							{
								if (!this.otherHand || !(this.otherHand.hoveringInteractable == contacting))
								{
									float num6 = Vector3.Distance(contacting.transform.position, this.hoverSphereTransform.position);
									if (num6 < num)
									{
										num = num6;
										hoveringInteractable = contacting;
									}
									num5++;
								}
							}
						}
					}
				}
			}
			this.hoveringInteractable = hoveringInteractable;
			if (num5 > 0 && num5 != this.prevOverlappingColliders)
			{
				this.prevOverlappingColliders = num5;
				this.HandDebugLog("Found " + num5 + " overlapping colliders.");
			}
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x00205250 File Offset: 0x00203650
		private void UpdateNoSteamVRFallback()
		{
			if (this.noSteamVRFallbackCamera)
			{
				Ray ray = this.noSteamVRFallbackCamera.ScreenPointToRay(Input.mousePosition);
				if (this.attachedObjects.Count > 0)
				{
					base.transform.position = ray.origin + this.noSteamVRFallbackInteractorDistance * ray.direction;
				}
				else
				{
					Vector3 position = base.transform.position;
					base.transform.position = this.noSteamVRFallbackCamera.transform.forward * -1000f;
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit, this.noSteamVRFallbackMaxDistanceNoItem))
					{
						base.transform.position = raycastHit.point;
						this.noSteamVRFallbackInteractorDistance = Mathf.Min(this.noSteamVRFallbackMaxDistanceNoItem, raycastHit.distance);
					}
					else if (this.noSteamVRFallbackInteractorDistance > 0f)
					{
						base.transform.position = ray.origin + Mathf.Min(this.noSteamVRFallbackMaxDistanceNoItem, this.noSteamVRFallbackInteractorDistance) * ray.direction;
					}
					else
					{
						base.transform.position = position;
					}
				}
			}
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x00205388 File Offset: 0x00203788
		private void UpdateDebugText()
		{
			if (this.showDebugText)
			{
				if (this.debugText == null)
				{
					this.debugText = new GameObject("_debug_text").AddComponent<TextMesh>();
					this.debugText.fontSize = 120;
					this.debugText.characterSize = 0.001f;
					this.debugText.transform.parent = base.transform;
					this.debugText.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
				}
				if (this.GuessCurrentHandType() == Hand.HandType.Right)
				{
					this.debugText.transform.localPosition = new Vector3(-0.05f, 0f, 0f);
					this.debugText.alignment = TextAlignment.Right;
					this.debugText.anchor = TextAnchor.UpperRight;
				}
				else
				{
					this.debugText.transform.localPosition = new Vector3(0.05f, 0f, 0f);
					this.debugText.alignment = TextAlignment.Left;
					this.debugText.anchor = TextAnchor.UpperLeft;
				}
				this.debugText.text = string.Format("Hovering: {0}\nHover Lock: {1}\nAttached: {2}\nTotal Attached: {3}\nType: {4}\n", new object[]
				{
					(!this.hoveringInteractable) ? "null" : this.hoveringInteractable.gameObject.name,
					this.hoverLocked,
					(!this.currentAttachedObject) ? "null" : this.currentAttachedObject.name,
					this.attachedObjects.Count,
					this.GuessCurrentHandType().ToString()
				});
			}
			else if (this.debugText != null)
			{
				UnityEngine.Object.Destroy(this.debugText.gameObject);
			}
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x00205574 File Offset: 0x00203974
		private void OnEnable()
		{
			this.inputFocusAction.enabled = true;
			float time = (!(this.otherHand != null) || this.otherHand.GetInstanceID() >= base.GetInstanceID()) ? 0f : (0.5f * this.hoverUpdateInterval);
			base.InvokeRepeating("UpdateHovering", time, this.hoverUpdateInterval);
			base.InvokeRepeating("UpdateDebugText", time, this.hoverUpdateInterval);
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x002055EF File Offset: 0x002039EF
		private void OnDisable()
		{
			this.inputFocusAction.enabled = false;
			base.CancelInvoke();
		}

		// Token: 0x06005C84 RID: 23684 RVA: 0x00205604 File Offset: 0x00203A04
		private void Update()
		{
			this.UpdateNoSteamVRFallback();
			GameObject currentAttachedObject = this.currentAttachedObject;
			if (currentAttachedObject)
			{
				currentAttachedObject.SendMessage("HandAttachedUpdate", this, SendMessageOptions.DontRequireReceiver);
			}
			if (this.hoveringInteractable)
			{
				this.hoveringInteractable.SendMessage("HandHoverUpdate", this, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x06005C85 RID: 23685 RVA: 0x00205658 File Offset: 0x00203A58
		private void LateUpdate()
		{
			if (this.controllerObject != null && this.attachedObjects.Count == 0)
			{
				this.AttachObject(this.controllerObject, Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand, string.Empty);
			}
		}

		// Token: 0x06005C86 RID: 23686 RVA: 0x00205690 File Offset: 0x00203A90
		private void OnInputFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				this.DetachObject(this.applicationLostFocusObject, true);
				this.applicationLostFocusObject.SetActive(false);
				this.UpdateHandPoses();
				this.UpdateHovering();
				base.BroadcastMessage("OnParentHandInputFocusAcquired", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				this.applicationLostFocusObject.SetActive(true);
				this.AttachObject(this.applicationLostFocusObject, Hand.AttachmentFlags.ParentToHand, string.Empty);
				base.BroadcastMessage("OnParentHandInputFocusLost", SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x06005C87 RID: 23687 RVA: 0x00205703 File Offset: 0x00203B03
		private void FixedUpdate()
		{
			this.UpdateHandPoses();
		}

		// Token: 0x06005C88 RID: 23688 RVA: 0x0020570C File Offset: 0x00203B0C
		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(0.5f, 1f, 0.5f, 0.9f);
			Transform transform = (!this.hoverSphereTransform) ? base.transform : this.hoverSphereTransform;
			Gizmos.DrawWireSphere(transform.position, this.hoverSphereRadius);
		}

		// Token: 0x06005C89 RID: 23689 RVA: 0x0020576A File Offset: 0x00203B6A
		private void HandDebugLog(string msg)
		{
			if (this.spewDebugText)
			{
				Debug.Log("Hand (" + base.name + "): " + msg);
			}
		}

		// Token: 0x06005C8A RID: 23690 RVA: 0x00205794 File Offset: 0x00203B94
		private void UpdateHandPoses()
		{
			if (this.controller != null)
			{
				SteamVR instance = SteamVR.instance;
				if (instance != null)
				{
					TrackedDevicePose_t trackedDevicePose_t = default(TrackedDevicePose_t);
					TrackedDevicePose_t trackedDevicePose_t2 = default(TrackedDevicePose_t);
					if (instance.compositor.GetLastPoseForTrackedDeviceIndex(this.controller.index, ref trackedDevicePose_t, ref trackedDevicePose_t2) == EVRCompositorError.None)
					{
						SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(trackedDevicePose_t2.mDeviceToAbsoluteTracking);
						base.transform.localPosition = rigidTransform.pos;
						base.transform.localRotation = rigidTransform.rot;
					}
				}
			}
		}

		// Token: 0x06005C8B RID: 23691 RVA: 0x0020581B File Offset: 0x00203C1B
		public void HoverLock(Interactable interactable)
		{
			this.HandDebugLog("HoverLock " + interactable);
			this.hoverLocked = true;
			this.hoveringInteractable = interactable;
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x0020583C File Offset: 0x00203C3C
		public void HoverUnlock(Interactable interactable)
		{
			this.HandDebugLog("HoverUnlock " + interactable);
			if (this.hoveringInteractable == interactable)
			{
				this.hoverLocked = false;
			}
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x00205867 File Offset: 0x00203C67
		public bool GetStandardInteractionButtonDown()
		{
			if (this.noSteamVRFallbackCamera)
			{
				return Input.GetMouseButtonDown(0);
			}
			return this.controller != null && this.controller.GetHairTriggerDown();
		}

		// Token: 0x06005C8E RID: 23694 RVA: 0x00205898 File Offset: 0x00203C98
		public bool GetStandardInteractionButtonUp()
		{
			if (this.noSteamVRFallbackCamera)
			{
				return Input.GetMouseButtonUp(0);
			}
			return this.controller != null && this.controller.GetHairTriggerUp();
		}

		// Token: 0x06005C8F RID: 23695 RVA: 0x002058C9 File Offset: 0x00203CC9
		public bool GetStandardInteractionButton()
		{
			if (this.noSteamVRFallbackCamera)
			{
				return Input.GetMouseButton(0);
			}
			return this.controller != null && this.controller.GetHairTrigger();
		}

		// Token: 0x06005C90 RID: 23696 RVA: 0x002058FC File Offset: 0x00203CFC
		private void InitController(int index)
		{
			if (this.controller == null)
			{
				this.controller = SteamVR_Controller.Input(index);
				this.HandDebugLog(string.Concat(new object[]
				{
					"Hand ",
					base.name,
					" connected with device index ",
					this.controller.index
				}));
				this.controllerObject = UnityEngine.Object.Instantiate<GameObject>(this.controllerPrefab);
				this.controllerObject.SetActive(true);
				this.controllerObject.name = this.controllerPrefab.name + "_" + base.name;
				this.controllerObject.layer = base.gameObject.layer;
				this.controllerObject.tag = base.gameObject.tag;
				this.AttachObject(this.controllerObject, Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand, string.Empty);
				this.controller.TriggerHapticPulse(800, EVRButtonId.k_EButton_Axis0);
				this.controllerObject.transform.localScale = this.controllerPrefab.transform.localScale;
				base.BroadcastMessage("OnHandInitialized", index, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x04004202 RID: 16898
		public const Hand.AttachmentFlags defaultAttachmentFlags = Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand;

		// Token: 0x04004203 RID: 16899
		public Hand otherHand;

		// Token: 0x04004204 RID: 16900
		public Hand.HandType startingHandType;

		// Token: 0x04004205 RID: 16901
		public Transform hoverSphereTransform;

		// Token: 0x04004206 RID: 16902
		public float hoverSphereRadius = 0.05f;

		// Token: 0x04004207 RID: 16903
		public LayerMask hoverLayerMask = -1;

		// Token: 0x04004208 RID: 16904
		public float hoverUpdateInterval = 0.1f;

		// Token: 0x04004209 RID: 16905
		public Camera noSteamVRFallbackCamera;

		// Token: 0x0400420A RID: 16906
		public float noSteamVRFallbackMaxDistanceNoItem = 10f;

		// Token: 0x0400420B RID: 16907
		public float noSteamVRFallbackMaxDistanceWithItem = 0.5f;

		// Token: 0x0400420C RID: 16908
		private float noSteamVRFallbackInteractorDistance = -1f;

		// Token: 0x0400420D RID: 16909
		public SteamVR_Controller.Device controller;

		// Token: 0x0400420E RID: 16910
		public GameObject controllerPrefab;

		// Token: 0x0400420F RID: 16911
		private GameObject controllerObject;

		// Token: 0x04004210 RID: 16912
		public bool showDebugText;

		// Token: 0x04004211 RID: 16913
		public bool spewDebugText;

		// Token: 0x04004212 RID: 16914
		private List<Hand.AttachedObject> attachedObjects = new List<Hand.AttachedObject>();

		// Token: 0x04004214 RID: 16916
		private Interactable _hoveringInteractable;

		// Token: 0x04004215 RID: 16917
		private TextMesh debugText;

		// Token: 0x04004216 RID: 16918
		private int prevOverlappingColliders;

		// Token: 0x04004217 RID: 16919
		private const int ColliderArraySize = 16;

		// Token: 0x04004218 RID: 16920
		private Collider[] overlappingColliders;

		// Token: 0x04004219 RID: 16921
		private Player playerInstance;

		// Token: 0x0400421A RID: 16922
		private GameObject applicationLostFocusObject;

		// Token: 0x0400421B RID: 16923
		private SteamVR_Events.Action inputFocusAction;

		// Token: 0x02000BA5 RID: 2981
		public enum HandType
		{
			// Token: 0x0400421E RID: 16926
			Left,
			// Token: 0x0400421F RID: 16927
			Right,
			// Token: 0x04004220 RID: 16928
			Any
		}

		// Token: 0x02000BA6 RID: 2982
		[Flags]
		public enum AttachmentFlags
		{
			// Token: 0x04004222 RID: 16930
			SnapOnAttach = 1,
			// Token: 0x04004223 RID: 16931
			DetachOthers = 2,
			// Token: 0x04004224 RID: 16932
			DetachFromOtherHand = 4,
			// Token: 0x04004225 RID: 16933
			ParentToHand = 8
		}

		// Token: 0x02000BA7 RID: 2983
		public struct AttachedObject
		{
			// Token: 0x04004226 RID: 16934
			public GameObject attachedObject;

			// Token: 0x04004227 RID: 16935
			public GameObject originalParent;

			// Token: 0x04004228 RID: 16936
			public bool isParentedToHand;
		}
	}
}
