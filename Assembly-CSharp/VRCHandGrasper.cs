using System;
using System.Linq;
using UnityEngine;
using Valve.VR;
using VRC;
using VRCSDK2;

// Token: 0x02000B36 RID: 2870
public class VRCHandGrasper : MonoBehaviour
{
    public static System.Collections.Generic.List<VRCHandGrasper> All = new System.Collections.Generic.List<VRCHandGrasper>();
	// Token: 0x17000CAD RID: 3245
	// (get) Token: 0x060057A5 RID: 22437 RVA: 0x001E3AD1 File Offset: 0x001E1ED1
	private VRCPlayer player
	{
		get
		{
			return this.mPlayer;
		}
	}

	// Token: 0x060057A6 RID: 22438 RVA: 0x001E3AD9 File Offset: 0x001E1ED9
	private void Awake()
	{
		this.physicsTrackerComp = base.gameObject.AddMissingComponent<PhysicsTrackerComponent>();
	}

	// Token: 0x060057A7 RID: 22439 RVA: 0x001E3AEC File Offset: 0x001E1EEC
	private void Start()
	{
		this.mPlayer = base.GetComponentInParent<VRCPlayer>();
		if (base.GetComponent<Rigidbody>() == null)
		{
			Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
		}
		this.inMoveHoldFB = VRCInputManager.FindInput("MoveHoldFB");
		this.inSpinHoldCwCcw = VRCInputManager.FindInput("SpinHoldCwCcw");
		this.inSpinHoldUD = VRCInputManager.FindInput("SpinHoldUD");
		this.inSpinHoldLR = VRCInputManager.FindInput("SpinHoldLR");
		this.ResetVelocityHistory();
		this.jointObject = new GameObject((!this.RightHand) ? "PickupJointLeftHand" : "PickupJointRightHand");
		this.jointObject.transform.SetParent(base.transform);
		this.jointObject.transform.localPosition = Vector3.zero;
		this.jointObject.transform.localRotation = Quaternion.identity;
		this.jointObject.transform.localScale = Vector3.one;
		this.lastFixedUpdateRealTime = Time.realtimeSinceStartup;
		if (TutorialManager.Instance != null)
		{
			GameObject pickupTetherPrefab = TutorialManager.Instance.PickupTetherPrefab;
			if (pickupTetherPrefab != null)
			{
				this.pickupTether = UnityEngine.Object.Instantiate<GameObject>(pickupTetherPrefab);
				if (this.pickupTether != null)
				{
					this.pickupTether.transform.parent = base.transform;
					this.pickupTether.transform.localPosition = Vector3.zero;
					this.pickupTether.transform.localRotation = Quaternion.identity;
					this.pickupTether.transform.localScale = Vector3.one;
					this.pickupTether.SetActive(false);
				}
			}
		}
		if (this.ShowMarkerObject)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Transform transform = base.transform;
			gameObject.transform.parent = transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			Collider component = gameObject.GetComponent<Collider>();
			if (component)
			{
				component.enabled = false;
			}
			GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Transform trackedTransform = VRCTrackingManager.GetTrackedTransform((!this.RightHand) ? VRCTracking.ID.HandTracker_LeftPalm : VRCTracking.ID.HandTracker_RightPalm);
			gameObject2.transform.parent = trackedTransform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.rotation = Quaternion.identity;
			gameObject2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			Renderer component2 = gameObject2.GetComponent<Renderer>();
			if (component2 != null)
			{
				component2.material.color = Color.green;
			}
			Collider component3 = gameObject2.GetComponent<Collider>();
			if (component3)
			{
				component3.enabled = false;
			}
			GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject3.name = "ClosestPointMarker";
			gameObject3.transform.localPosition = Vector3.zero;
			gameObject3.transform.rotation = Quaternion.identity;
			gameObject3.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			Collider component4 = gameObject3.GetComponent<Collider>();
			if (component4)
			{
				component4.enabled = false;
			}
			this.closestPointMarker = gameObject3;
		}
	}

	// Token: 0x060057A8 RID: 22440 RVA: 0x001E3E4C File Offset: 0x001E224C
	private void Update()
	{
		if (this.DropInput != null)
		{
			if (VRCInputManager.legacyGrasp)
			{
				this.UpdateLegacyGrasp();
			}
			else
			{
				this.UpdateSimplifiedGrasp();
			}
		}
		this.CheckForPickupKnockedOutOfHand();
		this.CheckResetDroppedRigidbodyState();
		if (this.closestPointMarker != null)
		{
			VRCTracking.ID id = (!this.RightHand) ? VRCTracking.ID.HandTracker_LeftPalm : VRCTracking.ID.HandTracker_RightPalm;
			Vector3 position = VRCTrackingManager.GetTrackedTransform(id).position;
			int layerMask = ~(1 << LayerMask.NameToLayer("Player")) | 1 << LayerMask.NameToLayer("PlayerLocal");
			Collider[] array = Physics.OverlapSphere(position, 3f, layerMask, QueryTriggerInteraction.Ignore);
			float num = float.MaxValue;
			Vector3 vector = Vector3.zero;
			Collider x = null;
			foreach (Collider collider in array)
			{
				if (!(collider.GetComponent<VRC_Pickup>() == null))
				{
					bool flag = false;
					vector = PhysicsUtil.ClosestPointOnCollider(collider, position, ref flag);
					if ((vector - position).magnitude < num)
					{
						num = (vector - position).magnitude;
						this.closestPointMarker.transform.position = vector;
						this.closestPointMarker.GetComponent<Renderer>().material.color = ((!flag) ? Color.white : Color.blue);
						this.closestPointMarker.GetComponent<Renderer>().enabled = true;
					}
					x = collider;
				}
			}
			if (x == null)
			{
				this.closestPointMarker.GetComponent<Renderer>().enabled = false;
			}
		}
		this.IsFirstFixedUpdateThisFrame = true;
	}

	// Token: 0x060057A9 RID: 22441 RVA: 0x001E3FE8 File Offset: 0x001E23E8
	private void LateUpdate()
	{
		if (this.graspedPickup != null)
		{
			ObjectInternal component = this.graspedPickup.gameObject.GetComponent<ObjectInternal>();
			if (component != null && component.SyncPhysics && !VRC.Network.IsOwner(this.graspedPickup.gameObject))
			{
				this.Drop("Object null or lost ownership");
			}
		}
	}

	// Token: 0x060057AA RID: 22442 RVA: 0x001E4050 File Offset: 0x001E2450
	private void FixedUpdate()
	{
		if (this.CheckForPlayerTeleport() || this.CheckForPickupTeleportToHand() || this.UpdateFlexibleGrip())
		{
			this.lastPosition = this.graspedRigidbody.transform.position;
			this.lastRotation = this.graspedRigidbody.transform.rotation;
		}
		else if (this.graspedRigidbody != null && this.jointObject != null)
		{
			this.lastPosition = this.graspedRigidbody.transform.position;
			this.lastRotation = this.graspedRigidbody.transform.rotation;
			if (this.graspedRigidbody.isKinematic)
			{
				this.graspedRigidbody.transform.position = this.GetGraspedPickupTargetPosition();
				this.graspedRigidbody.transform.rotation = this.GetGraspedPickupTargetRotation();
			}
		}
		this.gotFixedUpdate = true;
		if (this.IsFirstFixedUpdateThisFrame)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num = realtimeSinceStartup - this.lastFixedUpdateRealTime;
			this.lastFixedUpdateRealTime = realtimeSinceStartup;
			float num2 = Mathf.Min(num, Time.maximumDeltaTime) * Time.timeScale;
			this.numFixedUpdatesThisFrame = Mathf.Max(Mathf.FloorToInt(num2 / Time.fixedDeltaTime - 0.015f), 1);
			this.numFixedUpdatesRemainingThisFrame = this.numFixedUpdatesThisFrame;
			Vector3 position = this.jointObject.transform.position;
			Vector3 vec = (num <= 0f) ? Vector3.zero : ((position - this.lastHandPosition) / num);
			if (!vec.IsBad())
			{
				this.currentHandLinearVelocity = vec;
			}
			this.lastHandPosition = position;
			Quaternion rotation = this.jointObject.transform.rotation;
			Quaternion quaternion = (rotation * Quaternion.Inverse(this.lastHandRotation).Normalize()).Normalize();
			float num3 = 0f;
			Vector3 zero = Vector3.zero;
			quaternion.ToAngleAxis(out num3, out zero);
			if (num3 > 180f)
			{
				num3 -= 360f;
			}
			Vector3 vec2 = (num <= 0f) ? Vector3.zero : (num3 * 0.0174532924f / num * zero);
			if (!vec2.IsBad())
			{
				this.currentHandAngularVelocity = vec2;
			}
			this.lastHandRotation = rotation;
			if (this.graspedRigidbody != null)
			{
				this.currentPickupRotationDelta = (this.GetGraspedPickupTargetRotation() * Quaternion.Inverse(this.graspedRigidbody.transform.rotation).Normalize()).Normalize();
				this.currentPickupPositionDelta = this.GetGraspedPickupTargetPosition() - this.graspedRigidbody.transform.position;
			}
		}
		if (this.graspedRigidbody != null)
		{
			Vector3 vector = Vector3.zero;
			if (this.numFixedUpdatesRemainingThisFrame > 1)
			{
				vector = this.currentPickupPositionDelta / ((float)this.numFixedUpdatesThisFrame * Time.fixedDeltaTime);
			}
			else
			{
				vector = (this.GetGraspedPickupTargetPosition() - this.graspedRigidbody.transform.position) / Time.fixedDeltaTime;
			}
			if (this.MaxPickupLinearVelocity > 0f)
			{
				vector = Vector3.ClampMagnitude(vector, this.MaxPickupLinearVelocity);
			}
			if (this.MaxPickupLinearVelocityDelta > 0f)
			{
				Vector3 vector2 = vector - this.graspedRigidbody.velocity;
				vector2 = Vector3.ClampMagnitude(vector2, this.MaxPickupLinearVelocityDelta / (float)this.numFixedUpdatesThisFrame);
				vector = this.graspedRigidbody.velocity + vector2;
			}
			vector *= 1f - this.PickupVelocityDampPct;
			if (!vector.IsBad())
			{
				this.graspedRigidbody.velocity = vector;
			}
			float num4 = 0f;
			Vector3 zero2 = Vector3.zero;
			this.currentPickupRotationDelta.ToAngleAxis(out num4, out zero2);
			if (num4 > 180f)
			{
				num4 -= 360f;
			}
			Vector3 vector3 = Vector3.zero;
			if (num4 != 0f)
			{
				if (this.numFixedUpdatesRemainingThisFrame > 1)
				{
					vector3 = num4 * 0.0174532924f / ((float)this.numFixedUpdatesThisFrame * Time.fixedDeltaTime) * zero2;
				}
				else
				{
					float num5 = 0f;
					Vector3 zero3 = Vector3.zero;
					(this.jointObject.transform.rotation * Quaternion.Inverse(this.graspedRigidbody.transform.rotation).Normalize()).Normalize().ToAngleAxis(out num5, out zero3);
					if (num5 > 180f)
					{
						num5 -= 360f;
					}
					vector3 = num5 * 0.0174532924f / Time.fixedDeltaTime * zero3;
				}
				vector3 *= 1f - this.PickupVelocityDampPct;
				if (!vector3.IsBad())
				{
					this.graspedRigidbody.angularVelocity = vector3;
				}
			}
		}
		if (this.VelocityHistory != null)
		{
			this.VelocityHistoryStep++;
			if (this.VelocityHistoryStep >= this.VelocityHistory.Length)
			{
				this.VelocityHistoryStep = 0;
			}
			this.VelocityHistory[this.VelocityHistoryStep] = new Vector3?(this.currentHandLinearVelocity);
			this.AngularVelocityHistory[this.VelocityHistoryStep] = new Vector3?(this.currentHandAngularVelocity);
		}
		if (this.pickupJustDropped != null && this.postThrowVelocityFramesLeftToIntegrate > 0)
		{
			this.ApplyCurrentThrowVelocity(this.pickupJustDropped.physicalRoot, this.pickupJustDropped, true);
			this.postThrowVelocityFramesLeftToIntegrate--;
			if (this.postThrowVelocityFramesLeftToIntegrate == 0)
			{
				this.pickupJustDropped = null;
			}
		}
		this.numFixedUpdatesRemainingThisFrame--;
		this.IsFirstFixedUpdateThisFrame = false;
	}

	// Token: 0x060057AB RID: 22443 RVA: 0x001E45FC File Offset: 0x001E29FC
	private Vector3 GetGraspedPickupTargetPosition()
	{
		return Vector3.Lerp(this.graspedRigidbody.transform.position, this.jointObject.transform.position, (this.PickupLerpToHandTime <= Time.fixedDeltaTime) ? 1f : ((Time.fixedTime - this.pickupLerpStartTime) / this.PickupLerpToHandTime));
	}

	// Token: 0x060057AC RID: 22444 RVA: 0x001E465C File Offset: 0x001E2A5C
	private Quaternion GetGraspedPickupTargetRotation()
	{
		return Quaternion.Slerp(this.graspedRigidbody.transform.rotation, this.jointObject.transform.rotation, (this.PickupLerpToHandTime <= Time.fixedDeltaTime) ? 1f : ((Time.fixedTime - this.pickupLerpStartTime) / this.PickupLerpToHandTime));
	}

	// Token: 0x060057AD RID: 22445 RVA: 0x001E46BC File Offset: 0x001E2ABC
	private void CheckToggleThrowMode()
	{
		bool flag = false;
		SteamVR_ControllerManager trackingComponent = VRCTrackingManager.GetTrackingComponent<SteamVR_ControllerManager>();
		int index = (int)trackingComponent.right.GetComponent<SteamVR_TrackedObject>().index;
		if (index >= 0)
		{
			SteamVR_Controller.Device device = SteamVR_Controller.Input(index);
			if (device.GetPressDown(EVRButtonId.k_EButton_Axis0))
			{
				flag = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		this.throwMode = (this.throwMode + 1) % 10;
		switch (this.throwMode)
		{
		case 0:
			this.VelocityHistoryWindowSize = 3;
			this.PostThrowVelocityFramesToIntegrate = 0;
			break;
		case 1:
			this.VelocityHistoryWindowSize = 4;
			this.PostThrowVelocityFramesToIntegrate = 0;
			break;
		case 2:
			this.VelocityHistoryWindowSize = 5;
			this.PostThrowVelocityFramesToIntegrate = 0;
			break;
		case 3:
			this.VelocityHistoryWindowSize = 6;
			this.PostThrowVelocityFramesToIntegrate = 0;
			break;
		case 4:
			this.VelocityHistoryWindowSize = 7;
			this.PostThrowVelocityFramesToIntegrate = 0;
			break;
		case 5:
			this.VelocityHistoryWindowSize = 3;
			this.PostThrowVelocityFramesToIntegrate = 1;
			break;
		case 6:
			this.VelocityHistoryWindowSize = 4;
			this.PostThrowVelocityFramesToIntegrate = 1;
			break;
		case 7:
			this.VelocityHistoryWindowSize = 5;
			this.PostThrowVelocityFramesToIntegrate = 1;
			break;
		case 8:
			this.VelocityHistoryWindowSize = 6;
			this.PostThrowVelocityFramesToIntegrate = 1;
			break;
		case 9:
			this.VelocityHistoryWindowSize = 7;
			this.PostThrowVelocityFramesToIntegrate = 1;
			break;
		}
		this.ResetVelocityHistory();
		string message = string.Concat(new object[]
		{
			"Throw Mode: AVERAGE ",
			this.VelocityHistoryWindowSize,
			" frames ",
			(this.PostThrowVelocityFramesToIntegrate <= 0) ? string.Empty : ("(" + this.PostThrowVelocityFramesToIntegrate + " after throw)")
		});
		DebugDrawHMD.Instance.visible = true;
		DebugDrawHMD.Instance.SetText(message, 0, 3f);
		Debug.Log(message);
	}

	// Token: 0x060057AE RID: 22446 RVA: 0x001E48A8 File Offset: 0x001E2CA8
	private bool UpdateFlexibleGrip()
	{
		if (!(this.graspedPickup != null))
		{
			return false;
		}
		if ((this.graspedPickup.ExactGrip != null || this.graspedPickup.ExactGun != null) && !this.graspedPickup.allowManipulationWhenEquipped)
		{
			return false;
		}
		Collider component = this.graspedPickup.GetComponent<Collider>();
		Vector3 position = component.ClosestPointOnBounds(base.transform.position);
		float z = base.transform.InverseTransformPoint(position).z;
		float move = this.inMoveHoldFB.axis * this.changeGraspMoveSpeed * Time.deltaTime * -1f;
		if ((z < -0.3f && move < 0f) || ((double)z > 0.5 && move > 0f))
		{
			move = 0f;
		}
		if (move.AlmostEquals(0f, 0.01f) && this.inSpinHoldUD.axis.AlmostEquals(0f, 0.01f) && this.inSpinHoldLR.axis.AlmostEquals(0f, 0.01f) && this.inSpinHoldCwCcw.axis.AlmostEquals(0f, 0.01f))
		{
			return false;
		}
		VRCHandGrasper.ApplyMovementToTransform applyMovementToTransform = delegate(Transform rx)
		{
			rx.transform.position += move * this.transform.forward;
			rx.transform.rotation = Quaternion.AngleAxis(-1f * this.inSpinHoldUD.axis * this.changeGraspRotationSpeed * Time.deltaTime, this.transform.parent.right) * rx.transform.rotation;
			rx.transform.rotation = Quaternion.AngleAxis(-1f * this.inSpinHoldLR.axis * this.changeGraspRotationSpeed * Time.deltaTime, this.transform.parent.up) * rx.transform.rotation;
			rx.transform.rotation = Quaternion.AngleAxis(-1f * this.inSpinHoldCwCcw.axis * this.changeGraspRotationSpeed * Time.deltaTime, this.transform.parent.forward) * rx.transform.rotation;
		};
		Transform rx2 = (!(this.graspedRigidbody != null)) ? this.graspedPickup.transform : this.jointObject.transform;
		if (this.graspedRigidbody != null)
		{
			this.graspedRigidbody.transform.rotation = this.GetGraspedPickupTargetRotation();
			this.graspedRigidbody.transform.position = this.GetGraspedPickupTargetPosition();
		}
		applyMovementToTransform(rx2);
		return true;
	}

	// Token: 0x060057AF RID: 22447 RVA: 0x001E4AB0 File Offset: 0x001E2EB0
	private void UpdateLegacyGrasp()
	{
		if (this.graspedPickup == null)
		{
			if (this.DropInput.down)
			{
				this.Pickup();
			}
		}
		else
		{
			if (this.UseInput.button && !this.useActive)
			{
				this.UseHeld(true);
			}
			if (!this.UseInput.button && this.useActive)
			{
				this.UseHeld(false);
			}
			if (this.DropInput.up)
			{
				if (this.useActive)
				{
					this.UseHeld(false);
				}
				this.Drop("legacy grasp Drop");
			}
		}
	}

	// Token: 0x060057B0 RID: 22448 RVA: 0x001E4B5C File Offset: 0x001E2F5C
	private void UpdateSimplifiedGrasp()
	{
		bool flag = VRCInputManager.IsUsingHandController();
		if (this.graspedPickup == null)
		{
			if (this.GrabInput.down)
			{
				this.Pickup();
				if (this.graspedPickup != null)
				{
					this.longHoldMode = false;
					if (VRCInputManager.IsUsingAutoEquipControllerType() && VRCHandGrasper.IsAutoEquipPickup(this.graspedPickup))
					{
						this.longHoldMode = true;
					}
					Debug.Log(string.Concat(new object[]
					{
						"Pickup object: '",
						this.graspedPickup.name,
						"' equipped = ",
						this.longHoldMode,
						", is equippable = ",
						VRCHandGrasper.IsAutoEquipPickup(this.graspedPickup),
						", last input method = ",
						VRCInputManager.LastInputMethod,
						", is auto equip controller = ",
						VRCInputManager.IsUsingAutoEquipControllerType()
					}));
					if (this.longHoldMode)
					{
						return;
					}
				}
			}
		}
		else
		{
			if (this.ControllerToolTipEnabled)
			{
				this.ControllerTooltipResetTimer -= Time.deltaTime;
				if (this.ControllerTooltipResetTimer <= 0f)
				{
					this.ControllerTooltipResetTimer = 30f;
					this.ControllerToolTipEnabled = false;
					if (TutorialManager.Instance != null)
					{
						if (this.longHoldMode)
						{
							if (flag)
							{
								if (!string.IsNullOrEmpty(this.graspedPickup.UseText))
								{
									TutorialManager.Instance.ActivateControllerLabel((!this.RightHand) ? ControllerHand.Left : ControllerHand.Right, ControllerInputUI.Trigger, this.graspedPickup.UseText, 10f, 0);
								}
								TutorialManager.Instance.ActivateControllerLabel((!this.RightHand) ? ControllerHand.Left : ControllerHand.Right, ControllerInputUI.Grip, "Drop", 10f, 0);
							}
							else if (!string.IsNullOrEmpty(this.graspedPickup.UseText))
							{
								TutorialManager.Instance.ActivateObjectLabel(null, TutorialLabelType.Popup, ControllerHand.None, this.graspedPickup.UseText, ControllerActionUI.Use, "Drop", ControllerActionUI.Drop, 10f, 0, AttachMode.PositionOnly, true);
							}
							else
							{
								TutorialManager.Instance.ActivateObjectLabel(null, TutorialLabelType.Popup, ControllerHand.None, "Drop", ControllerActionUI.Drop, string.Empty, ControllerActionUI.None, 10f, 0, AttachMode.PositionOnly, true);
							}
						}
						else
						{
							if (VRCHandGrasper.IsUsablePickup(this.graspedPickup) && !string.IsNullOrEmpty(this.graspedPickup.UseText))
							{
								TutorialManager.Instance.ActivateControllerLabel((!this.RightHand) ? ControllerHand.Left : ControllerHand.Right, ControllerInputUI.Trigger, this.graspedPickup.UseText, 10f, 0);
							}
							if (this.ReleaseToDropShowCount > 0)
							{
								this.ReleaseToDropShowCount--;
								if (flag)
								{
									ControllerInputUI controllerPart = (VRCInputManager.LastInputMethod != VRCInputManager.InputMethod.Oculus) ? ControllerInputUI.Trigger : ControllerInputUI.Grip;
									TutorialManager.Instance.ActivateControllerLabel((!this.RightHand) ? ControllerHand.Left : ControllerHand.Right, controllerPart, "Release to Drop", 10f, 0);
								}
								else
								{
									TutorialManager.Instance.ActivateObjectLabel(null, TutorialLabelType.Popup, ControllerHand.None, "Release to Drop", ControllerActionUI.ReleaseObject, "Hold / Release To Throw", ControllerActionUI.Drop, 10f, 0, AttachMode.PositionOnly, true);
								}
							}
						}
					}
				}
			}
			if ((this.longHoldMode && Time.time - this.timeOfPickup > this.autoHoldUseDelay) || (!this.longHoldMode && VRCHandGrasper.IsUsablePickup(this.graspedPickup)))
			{
				if (this.UseInput.button && !this.useActive)
				{
					this.UseHeld(true);
				}
				if (!this.UseInput.button && this.useActive)
				{
					this.UseHeld(false);
				}
			}
			else if (this.useActive)
			{
				this.UseHeld(false);
			}
			if (!this.longHoldMode && !this.GrabInput.button)
			{
				this.Drop("Grab button released");
			}
			if (this.DropInput.down)
			{
				this.dropButtonDown = true;
				this.throwHoldStartTime = Time.unscaledTime;
			}
			if (this.dropButtonDown && this.DropInput.up)
			{
				this.dropButtonDown = false;
				bool flag2 = !flag;
				if (flag2)
				{
					Rigidbody rigidbody = this.graspedRigidbody;
					this.Drop("Throw on release");
					float num = Time.unscaledTime - this.throwHoldStartTime;
					if (num >= this.MinThrowHoldTime && rigidbody != null)
					{
						float d = Mathf.Lerp(this.MinThrowSpeed, this.MaxThrowSpeed, (num - this.MinThrowHoldTime) / (this.MaxThrowHoldTime - this.MinThrowHoldTime));
						rigidbody.velocity = d * VRCVrCamera.GetInstance().GetWorldLookRay().direction;
					}
					else
					{
						this.Drop("Throw on release 2");
					}
				}
				else
				{
					this.Drop("Clicked drop button");
				}
			}
		}
	}

	// Token: 0x060057B1 RID: 22449 RVA: 0x001E501E File Offset: 0x001E341E
	public static bool IsAutoEquipPickup(VRC_Pickup pickup)
	{
		return pickup.AutoHold == VRC_Pickup.AutoHoldMode.Yes || (pickup.AutoHold == VRC_Pickup.AutoHoldMode.AutoDetect && pickup.orientation != VRC_Pickup.PickupOrientation.Any);
	}

	// Token: 0x060057B2 RID: 22450 RVA: 0x001E5049 File Offset: 0x001E3449
	public static bool IsUsablePickup(VRC_Pickup pickup)
	{
		return VRCHandGrasper.IsAutoEquipPickup(pickup);
	}

	// Token: 0x060057B3 RID: 22451 RVA: 0x001E5054 File Offset: 0x001E3454
	private bool CheckForPlayerTeleport()
	{
		if (this.graspedRigidbody != null && this.jointObject != null && this.player != null && this.player.playerNet != null && this.player.playerNet.WasRecentlyDiscontinuous)
		{
			this.graspedRigidbody.transform.position = this.GetGraspedPickupTargetPosition();
			this.graspedRigidbody.transform.rotation = this.GetGraspedPickupTargetRotation();
			return true;
		}
		return false;
	}

	// Token: 0x060057B4 RID: 22452 RVA: 0x001E50F0 File Offset: 0x001E34F0
	private bool CheckForPickupTeleportToHand()
	{
		if (this.MinTeleportPickupToHandDistance < 0f)
		{
			return false;
		}
		if (this.graspedRigidbody != null)
		{
			float sqrMagnitude = (this.GetGraspedPickupTargetPosition() - this.graspedRigidbody.transform.position).sqrMagnitude;
			if (sqrMagnitude >= this.MinTeleportPickupToHandDistance * this.MinTeleportPickupToHandDistance)
			{
				Bounds compoundColliderAABB = PhysicsUtil.GetCompoundColliderAABB(this.graspedRigidbody.gameObject);
				Vector3 position = Matrix4x4.TRS(this.graspedRigidbody.transform.position, this.graspedRigidbody.transform.rotation, Vector3.one).inverse.MultiplyPoint3x4(compoundColliderAABB.center);
				Vector3 center = this.jointObject.transform.TransformPoint(position);
				Collider[] array = Physics.OverlapBox(center, compoundColliderAABB.extents, this.jointObject.transform.rotation, -1, QueryTriggerInteraction.Ignore);
				if (array == null || array.Length <= 0)
				{
					this.graspedRigidbody.transform.position = this.GetGraspedPickupTargetPosition();
					this.graspedRigidbody.transform.rotation = this.GetGraspedPickupTargetRotation();
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060057B5 RID: 22453 RVA: 0x001E522C File Offset: 0x001E362C
	private void CheckForPickupKnockedOutOfHand()
	{
		if (this.graspedRigidbody != null)
		{
			float num;
			if (this.dropDistanceFromHand > 0f)
			{
				num = this.dropDistanceFromHand;
			}
			else
			{
				num = ((this.MinTeleportPickupToHandDistance <= 0f) ? 1.6f : -1f);
			}
			float num2 = (this.dropAngleFromHand <= 0f) ? -1f : this.dropAngleFromHand;
			if (!VRCTrackingManager.IsPlayerNearTracking())
			{
				num = ((this.MinTeleportPickupToHandDistance <= 0f) ? 1.6f : -1f);
				num2 = -1f;
			}
			if (num > 0f && Vector3.SqrMagnitude(this.GetGraspedPickupTargetPosition() - this.graspedRigidbody.transform.position) > num)
			{
				this.Drop("knocked out of hand");
			}
			else if (num2 > 0f && Quaternion.Angle(this.jointObject.transform.rotation, this.graspedRigidbody.transform.rotation) > num2)
			{
				this.Drop("knocked out of hand");
			}
		}
	}

	// Token: 0x060057B6 RID: 22454 RVA: 0x001E5360 File Offset: 0x001E3760
	private void CheckResetDroppedRigidbodyState()
	{
		if (this.resetRigidbodyStateAfterNextFixedUpdate && this.gotFixedUpdate)
		{
			this.resetRigidbodyStateAfterNextFixedUpdate = false;
			if (this.rigidbodyStateToReset != null)
			{
				this.rigidbodyStateToReset.collisionDetectionMode = this.graspedRigidbodyOriginalCollisionDetectionMode;
			}
			if (this.graspedColliders != null)
			{
				for (int i = 0; i < this.graspedColliders.Length; i++)
				{
					Collider collider = this.graspedColliders[i];
					if (collider != null && collider.material != null)
					{
						collider.material.bounciness = this.previousBounciness[i];
						collider.material.bounceCombine = this.previousBounceCombine[i];
					}
				}
			}
			this.rigidbodyStateToReset = null;
			this.graspedColliders = null;
			this.previousBounciness = null;
			this.previousBounceCombine = null;
		}
	}

	// Token: 0x060057B7 RID: 22455 RVA: 0x001E5438 File Offset: 0x001E3838
	private void TriggerEvent(VRC_Trigger.TriggerType type)
	{
		if (this.graspedPickup == null)
		{
			return;
		}
		string eventName = null;
		VRC_EventHandler.VrcBroadcastType broadcast = VRC_EventHandler.VrcBroadcastType.Always;
		if (type != VRC_Trigger.TriggerType.OnPickupUseDown)
		{
			if (type != VRC_Trigger.TriggerType.OnPickupUseUp)
			{
				if (type != VRC_Trigger.TriggerType.OnPickup)
				{
					if (type == VRC_Trigger.TriggerType.OnDrop)
					{
						eventName = this.graspedPickup.DropEventName;
						broadcast = this.graspedPickup.pickupDropEventBroadcastType;
					}
				}
				else
				{
					eventName = this.graspedPickup.PickupEventName;
					broadcast = this.graspedPickup.pickupDropEventBroadcastType;
				}
			}
			else
			{
				eventName = this.graspedPickup.UseUpEventName;
				broadcast = this.graspedPickup.useEventBroadcastType;
			}
		}
		else
		{
			eventName = this.graspedPickup.UseDownEventName;
			broadcast = this.graspedPickup.useEventBroadcastType;
		}
		if (!string.IsNullOrEmpty(eventName) && this.graspedEvents != null)
		{
			foreach (VRC_EventHandler.VrcEvent e2 in from e in this.graspedEvents.Events
			where e.Name == eventName
			select e)
			{
				this.graspedEvents.TriggerEvent(e2, broadcast, this.mPlayer.gameObject, 0f);
			}
		}
		VRC_Trigger.Trigger(this.graspedPickup.gameObject, type);
	}

	// Token: 0x060057B8 RID: 22456 RVA: 0x001E55C4 File Offset: 0x001E39C4
	private void UseHeld(bool used)
	{
		this.useActive = used;
		if (used)
		{
			this.TriggerEvent(VRC_Trigger.TriggerType.OnPickupUseDown);
		}
		else
		{
			this.TriggerEvent(VRC_Trigger.TriggerType.OnPickupUseUp);
		}
	}

	// Token: 0x060057B9 RID: 22457 RVA: 0x001E55E8 File Offset: 0x001E39E8
	public void Pickup()
	{
		VRC_Pickup pickup = VRCUiCursorManager.GetPickup((!this.RightHand) ? VRCUiCursor.CursorHandedness.Left : VRCUiCursor.CursorHandedness.Right);
        

        if (pickup == null)
		{
            Debug.Log("Invalid pickup");
			return;
		}
		if (pickup.currentlyHeldBy != null && pickup.currentlyHeldBy is VRCHandGrasper)
		{
			(pickup.currentlyHeldBy as VRCHandGrasper).Drop("off hand grab");
		}
		pickup.RevertPhysics();
		ObjectInternal component = pickup.gameObject.GetComponent<ObjectInternal>();
		if (component != null && component.SyncPhysics)
		{
			if (!VRC.Network.IsOwner(pickup.gameObject))
			{
				VRC.Network.SetOwner(this.mPlayer.player, pickup.gameObject, VRC.Network.OwnershipModificationType.Pickup, true);
			}
			if (!VRC.Network.IsOwner(pickup.gameObject))
			{
				Debug.LogFormat("<color=red>Could not pickup {0}</color>", new object[]
				{
					pickup.gameObject.name
				});
				return;
			}
		}
		if (component != null)
		{
			component.StorePickupState();
			component.HeldInHand = ((!this.RightHand) ? VRC_Pickup.PickupHand.Left : VRC_Pickup.PickupHand.Right);
			component.UpdateEnableCollisionWithPlayer();
			component.DiscontinuityHint = true;
		}
		this.graspedRigidbody = pickup.physicalRoot;
		this.graspedRigidbodyOriginalCollisionDetectionMode = this.graspedRigidbody.collisionDetectionMode;
		this.graspedRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		this.graspedColliders = this.graspedRigidbody.GetComponentsInChildren<Collider>();
		this.previousBounciness = new float[this.graspedColliders.Length];
		this.previousBounceCombine = new PhysicMaterialCombine[this.graspedColliders.Length];
		for (int i = 0; i < this.graspedColliders.Length; i++)
		{
			Collider collider = this.graspedColliders[i];
			this.previousBounciness[i] = collider.material.bounciness;
			this.previousBounceCombine[i] = collider.material.bounceCombine;
			collider.material.bounciness = 0f;
			collider.material.bounceCombine = PhysicMaterialCombine.Minimum;
		}
		this.graspedPickup = pickup;
		this.graspedEvents = pickup.GetComponentInParent<VRC_EventHandler>();
		this.graspedPickup.currentlyHeldBy = this;
		this.graspedPickup.currentLocalPlayer = this.mPlayer.apiPlayer;
		this.dropButtonDown = false;
		bool flag = false;
		this.AlignPickupJointToObject(out flag);
		if (flag)
		{
			this.pickupLerpStartTime = Time.fixedTime;
		}
		this.lastPosition = pickup.transform.position;
		this.lastRotation = pickup.transform.rotation;
		this.ControllerTooltipResetTimer = 0f;
		this.ControllerToolTipEnabled = true;
		this.HapticEvent(0.2f, 0.5f, 50f);
		VRCUiCursorManager.BlockCursor(this.RightHand, true);
		this.timeOfPickup = Time.time;
		this.TriggerEvent(VRC_Trigger.TriggerType.OnPickup);
	}

	// Token: 0x060057BA RID: 22458 RVA: 0x001E58C4 File Offset: 0x001E3CC4
	private void AlignPickupJointToObject(out bool shouldLerp)
	{
		shouldLerp = false;
		Vector3 vector = Vector3.zero;
		Quaternion rotation = Quaternion.identity;
		Vector3 vector2 = Vector3.zero;
		Quaternion quaternion = Quaternion.identity;
		VRCTracking.ID id = (!this.RightHand) ? VRCTracking.ID.HandTracker_LeftGun : VRCTracking.ID.HandTracker_RightGun;
		VRCTracking.ID id2 = (!this.RightHand) ? VRCTracking.ID.HandTracker_LeftGrip : VRCTracking.ID.HandTracker_RightGrip;
		VRCTracking.ID id3 = (!this.RightHand) ? VRCTracking.ID.HandTracker_LeftGrip : VRCTracking.ID.HandTracker_RightGrip;
		switch (this.graspedPickup.orientation)
		{
		case VRC_Pickup.PickupOrientation.Any:
		{
			Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(id3);
			vector2 = trackedTransform.position;
			quaternion = trackedTransform.rotation;
			Vector3 zero = Vector3.zero;
			if (this.ShouldLerpPickupToHand(this.graspedPickup, out zero))
			{
				vector = zero;
				shouldLerp = true;
			}
			else
			{
				vector = vector2;
			}
			rotation = quaternion;
			break;
		}
		case VRC_Pickup.PickupOrientation.Grip:
		{
			vector = this.graspedPickup.ExactGrip.position;
			rotation = this.graspedPickup.ExactGrip.rotation;
			Transform trackedTransform2 = VRCTrackingManager.GetTrackedTransform(id2);
			vector2 = trackedTransform2.position;
			quaternion = trackedTransform2.rotation;
			break;
		}
		case VRC_Pickup.PickupOrientation.Gun:
		{
			vector = this.graspedPickup.ExactGun.position;
			rotation = this.graspedPickup.ExactGun.rotation;
			Transform trackedTransform3 = VRCTrackingManager.GetTrackedTransform(id);
			vector2 = trackedTransform3.position;
			quaternion = trackedTransform3.rotation;
			break;
		}
		}
		Quaternion quaternion2 = quaternion * Quaternion.Inverse(rotation);
		Vector3 vector3 = vector - this.graspedRigidbody.transform.position;
		Vector3 b = vector2 - vector - (quaternion2 * vector3 - vector3);
		if (this.graspedRigidbody != null)
		{
			this.jointObject.transform.position = this.graspedRigidbody.transform.position + b;
			this.jointObject.transform.rotation = quaternion2 * this.graspedRigidbody.transform.rotation;
		}
	}

	// Token: 0x060057BB RID: 22459 RVA: 0x001E5ABC File Offset: 0x001E3EBC
	public void ForceDrop()
	{
		Debug.LogException(new Exception("ForceDrop"));
		this.Drop("ForceDrop");
	}

    // Token: 0x060057BC RID: 22460 RVA: 0x001E5AD8 File Offset: 0x001E3ED8
    public void Drop(string why)
	{
		Debug.Log(string.Concat(new object[]
		{
			"Drop object: '",
			(!(this.graspedPickup != null)) ? "(null)" : this.graspedPickup.name,
			", was equipped = ",
			this.longHoldMode,
			"' ",
			why,
			", last input method = ",
			VRCInputManager.LastInputMethod
		}));
		this.longHoldMode = false;
		this.resetRigidbodyStateAfterNextFixedUpdate = true;
		this.gotFixedUpdate = false;
		this.rigidbodyStateToReset = this.graspedRigidbody;
		if (this.graspedPickup == null)
		{
			return;
		}
		this.graspedPickup.RevertPhysics();
		this.graspedPickup.currentlyHeldBy = null;
		this.graspedPickup.currentLocalPlayer = null;
		this.ApplyCurrentThrowVelocity(this.graspedRigidbody, this.graspedPickup, false);
		this.postThrowVelocityFramesLeftToIntegrate = this.PostThrowVelocityFramesToIntegrate;
		this.pickupJustDropped = this.graspedPickup;
		ObjectInternal component = this.graspedPickup.GetComponent<ObjectInternal>();
		if (component != null)
		{
			component.HeldInHand = VRC_Pickup.PickupHand.None;
			component.UpdateEnableCollisionWithPlayer();
		}
		if (this.useActive)
		{
			this.TriggerEvent(VRC_Trigger.TriggerType.OnPickupUseUp);
		}
		this.TriggerEvent(VRC_Trigger.TriggerType.OnDrop);
		this.graspedRigidbody = null;
		this.graspedPickup = null;
		this.graspedEvents = null;
		this.useActive = false;
		this.longHoldMode = false;
		this.pickupLerpStartTime = -1f;
		if (TutorialManager.Instance != null)
		{
			TutorialManager.Instance.DeactivateControllerLabel((!this.RightHand) ? ControllerHand.Left : ControllerHand.Right, ControllerInputUI.Trigger);
			TutorialManager.Instance.DeactivateControllerLabel((!this.RightHand) ? ControllerHand.Left : ControllerHand.Right, ControllerInputUI.Grip);
			TutorialManager.Instance.DeactivateObjectLabel(null);
		}
		this.HapticEvent(0.1f, 0.25f, 50f);
		VRCUiCursorManager.BlockCursor(this.RightHand, false);
	}

	// Token: 0x060057BD RID: 22461 RVA: 0x001E5CC4 File Offset: 0x001E40C4
	private void ApplyCurrentThrowVelocity(Rigidbody pickupRB, VRC_Pickup pickup, bool applyDirectionOnly)
	{
		Vector3 velocity = this.CalculateThrowLinearVelocityFromHistory();
		if (applyDirectionOnly)
		{
			velocity = velocity.normalized * pickupRB.velocity.magnitude;
		}
		pickupRB.velocity = velocity;
		if (!applyDirectionOnly && pickup.ThrowVelocityBoostMinSpeed >= 0f && pickupRB.velocity.sqrMagnitude >= pickup.ThrowVelocityBoostMinSpeed * pickup.ThrowVelocityBoostMinSpeed)
		{
			pickupRB.velocity *= pickup.ThrowVelocityBoostScale;
		}
		Vector3 angularVelocity = this.CalculateThrowAngularVelocityFromHistory();
		if (applyDirectionOnly)
		{
			angularVelocity = angularVelocity.normalized * pickupRB.angularVelocity.magnitude;
		}
		pickupRB.angularVelocity = angularVelocity;
	}

	// Token: 0x060057BE RID: 22462 RVA: 0x001E5D7D File Offset: 0x001E417D
	private void OnDestroy()
	{
		if (this.graspedPickup != null)
		{
			this.Drop("OnDestroy");
		}
	}

	// Token: 0x060057BF RID: 22463 RVA: 0x001E5D9C File Offset: 0x001E419C
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(base.transform.position + base.transform.forward * 0f, 0.1f);
		if (this.jointObject != null)
		{
			Gizmos.DrawWireSphere(this.jointObject.transform.position, 0.15f);
		}
	}

	// Token: 0x060057C0 RID: 22464 RVA: 0x001E5E03 File Offset: 0x001E4203
	public void HapticEvent(float duration, float amplitude, float frequency)
	{
		VRCTrackingManager.GenerateHapticEvent((!this.RightHand) ? VRCTracking.ID.HandTracker_LeftWrist : VRCTracking.ID.HandTracker_RightWrist, duration, amplitude, frequency);
	}

	// Token: 0x060057C1 RID: 22465 RVA: 0x001E5E1F File Offset: 0x001E421F
	public bool IsHoldingObject()
	{
		return this.graspedPickup != null;
	}

	// Token: 0x060057C2 RID: 22466 RVA: 0x001E5E2D File Offset: 0x001E422D
	private Vector3 CalculateThrowLinearVelocityFromHistory()
	{
		if (this.VelocityHistory != null)
		{
			return this.CalculateThrowVelocity(this.VelocityHistory);
		}
		return (this.graspedRigidbody.transform.position - this.lastPosition) / Time.fixedDeltaTime;
	}

	// Token: 0x060057C3 RID: 22467 RVA: 0x001E5E6C File Offset: 0x001E426C
	private Vector3 CalculateThrowAngularVelocityFromHistory()
	{
		if (this.AngularVelocityHistory != null)
		{
			return this.CalculateThrowVelocity(this.AngularVelocityHistory);
		}
		Vector3 zero = Vector3.zero;
		float num = 0f;
		(this.graspedRigidbody.transform.rotation * Quaternion.Inverse(this.lastRotation)).ToAngleAxis(out num, out zero);
		if (num > 180f)
		{
			num -= 360f;
		}
		return zero * (num * 0.0174532924f / Time.fixedDeltaTime);
	}

	// Token: 0x060057C4 RID: 22468 RVA: 0x001E5EF0 File Offset: 0x001E42F0
	private void ResetVelocityHistory()
	{
		this.VelocityHistory = (this.AngularVelocityHistory = null);
		if (this.VelocityHistoryWindowSize > 0)
		{
			this.VelocityHistory = new Vector3?[this.VelocityHistoryWindowSize];
			this.AngularVelocityHistory = new Vector3?[this.VelocityHistoryWindowSize];
		}
		if (this.VelocityHistory != null)
		{
			this.VelocityHistoryStep = 0;
			for (int i = 0; i < this.VelocityHistory.Length; i++)
			{
				this.VelocityHistory[i] = new Vector3?(Vector3.zero);
				this.AngularVelocityHistory[i] = new Vector3?(Vector3.zero);
			}
		}
	}

	// Token: 0x060057C5 RID: 22469 RVA: 0x001E5F9C File Offset: 0x001E439C
	private Vector3 CalculateThrowVelocity(Vector3?[] vels)
	{
		Vector3 a = Vector3.zero;
		int i;
		for (i = 0; i < vels.Length; i++)
		{
			Vector3? vector = vels[i];
			if (vector != null)
			{
				a += vels[i].Value;
			}
		}
		a.Normalize();
		int num = Mathf.Min(Mathf.Max(this.SpeedFramesToIntegrate, 1), this.VelocityHistoryWindowSize);
		i = (this.VelocityHistoryStep + 1) % vels.Length;
		int j = this.VelocityHistoryWindowSize - num;
		while (j > 0)
		{
			j--;
			i = (i + 1) % vels.Length;
		}
		float num2 = 0f;
		for (int k = 0; k < num; k++)
		{
			num2 += vels[i].Value.magnitude;
			i = (i + 1) % vels.Length;
		}
		num2 /= (float)num;
		return num2 * a;
	}

	// Token: 0x060057C6 RID: 22470 RVA: 0x001E6090 File Offset: 0x001E4490
	private bool ShouldLerpPickupToHand(VRC_Pickup pickup, out Vector3 grabPoint)
	{
		Collider component = pickup.GetComponent<Collider>();
		if (component != null)
		{
			VRCTracking.ID id = (!this.RightHand) ? VRCTracking.ID.HandTracker_LeftGrip : VRCTracking.ID.HandTracker_RightGrip;
			Vector3 position = VRCTrackingManager.GetTrackedTransform(id).position;
			bool flag = false;
			grabPoint = PhysicsUtil.ClosestPointOnCollider(component, position, ref flag);
			float magnitude = (grabPoint - position).magnitude;
			if (!flag && magnitude >= Mathf.Max(VRCHandGrasper.LerpToHandMinDistance, 0.001f))
			{
				return true;
			}
		}
		else
		{
			Debug.LogError("Pickup has no Collider! " + this.graspedPickup.gameObject.name);
		}
		grabPoint = Vector3.zero;
		return false;
	}

	// Token: 0x060057C7 RID: 22471 RVA: 0x001E6148 File Offset: 0x001E4548
	public void SetSelectedObject(VRC_Pickup selectedPickup, VRC_Interactable[] selectedInteractable)
	{
		bool active = false;
		if (selectedPickup != null && this.pickupTether != null)
		{
			Vector3 zero = Vector3.zero;
			if (this.ShouldLerpPickupToHand(selectedPickup, out zero))
			{
				this.pickupTether.GetComponent<UiTether>().Target = selectedPickup.transform;
				VRCTracking.ID id = (!this.RightHand) ? VRCTracking.ID.HandTracker_LeftGrip : VRCTracking.ID.HandTracker_RightGrip;
				Vector3 position = VRCTrackingManager.GetTrackedTransform(id).position;
				this.pickupTether.transform.position = position;
				active = true;
			}
		}
		this.pickupTether.SetActive(active);
		int num = (!(selectedPickup != null)) ? 0 : selectedPickup.GetInstanceID();
		if (num == 0)
		{
			num = ((selectedInteractable == null || selectedInteractable.Length <= 0) ? 0 : selectedInteractable[0].GetInstanceID());
		}
		if (num != this.lastSelectedPickupID && num != 0)
		{
			this.HapticEvent(0.1f, 0.1f, 25f);
		}
		this.lastSelectedPickupID = num;
	}

	// Token: 0x060057C8 RID: 22472 RVA: 0x001E6250 File Offset: 0x001E4650
	public static VRC_Pickup.PickupHand GetPickupHand(VRC_Pickup pickup)
	{
		VRCHandGrasper vrchandGrasper = pickup.currentlyHeldBy as VRCHandGrasper;
		VRC_Pickup.PickupHand result = VRC_Pickup.PickupHand.None;
		if (vrchandGrasper != null)
		{
			if (vrchandGrasper.graspedPickup == null)
			{
				result = VRC_Pickup.PickupHand.None;
			}
			else if (vrchandGrasper.RightHand)
			{
				result = VRC_Pickup.PickupHand.Right;
			}
			else
			{
				result = VRC_Pickup.PickupHand.Left;
			}
		}
		else
		{
			ObjectInternal component = pickup.GetComponent<ObjectInternal>();
			if (component != null && !component.isMine)
			{
				result = component.HeldInHand;
			}
		}
		return result;
	}

	// Token: 0x060057C9 RID: 22473 RVA: 0x001E62CE File Offset: 0x001E46CE
	public VRC_Pickup GetGraspedPickup()
	{
		return this.graspedPickup;
	}

	// Token: 0x060057CA RID: 22474 RVA: 0x001E62D8 File Offset: 0x001E46D8
	public static VRC_PlayerApi GetCurrentPlayer(VRC_Pickup pickup)
	{
		if (pickup == null)
		{
			return null;
		}
		VRC_PlayerApi result = null;
		VRCHandGrasper vrchandGrasper = pickup.currentlyHeldBy as VRCHandGrasper;
		if (vrchandGrasper != null && vrchandGrasper.player != null)
		{
			result = vrchandGrasper.player.GetComponent<VRC_PlayerApi>();
		}
		else
		{
			ObjectInternal component = pickup.GetComponent<ObjectInternal>();
			if (component != null && !component.isMine && component.isHeld && component.Owner != null)
			{
				result = component.Owner.playerApi;
			}
		}
		return result;
	}

	// Token: 0x060057CB RID: 22475 RVA: 0x001E6376 File Offset: 0x001E4776
	public void SetIsNearTracking(bool nearTracking)
	{
		if (this._isNearTracking != nearTracking)
		{
			if (nearTracking)
			{
				this.OnResumeTracking();
			}
			this._isNearTracking = nearTracking;
		}
	}

	// Token: 0x060057CC RID: 22476 RVA: 0x001E6398 File Offset: 0x001E4798
	private void OnResumeTracking()
	{
		if (this.graspedPickup != null)
		{
			bool flag = false;
			this.AlignPickupJointToObject(out flag);
		}
	}

	// Token: 0x04003EA6 RID: 16038
	private VRC_Pickup graspedPickup;

	// Token: 0x04003EA7 RID: 16039
	private Rigidbody graspedRigidbody;

	// Token: 0x04003EA8 RID: 16040
	private VRC_EventHandler graspedEvents;

	// Token: 0x04003EA9 RID: 16041
	private CollisionDetectionMode graspedRigidbodyOriginalCollisionDetectionMode;

	// Token: 0x04003EAA RID: 16042
	private Collider[] graspedColliders;

	// Token: 0x04003EAB RID: 16043
	private float[] previousBounciness;

	// Token: 0x04003EAC RID: 16044
	private PhysicMaterialCombine[] previousBounceCombine;

	// Token: 0x04003EAD RID: 16045
	private bool resetRigidbodyStateAfterNextFixedUpdate;

	// Token: 0x04003EAE RID: 16046
	private bool gotFixedUpdate;

	// Token: 0x04003EAF RID: 16047
	private Rigidbody rigidbodyStateToReset;

	// Token: 0x04003EB0 RID: 16048
	private VRCPlayer mPlayer;

	// Token: 0x04003EB1 RID: 16049
	private GameObject jointObject;

	// Token: 0x04003EB2 RID: 16050
	public float handRadius = 0.1f;

	// Token: 0x04003EB3 RID: 16051
	public VRCInput DropInput;

	// Token: 0x04003EB4 RID: 16052
	public VRCInput UseInput;

	// Token: 0x04003EB5 RID: 16053
	public VRCInput GrabInput;

	// Token: 0x04003EB6 RID: 16054
	public float changeGraspMoveSpeed = 1f;

	// Token: 0x04003EB7 RID: 16055
	public float changeGraspRotationSpeed = 100f;

	// Token: 0x04003EB8 RID: 16056
	private VRCInput inMoveHoldFB;

	// Token: 0x04003EB9 RID: 16057
	private VRCInput inSpinHoldCwCcw;

	// Token: 0x04003EBA RID: 16058
	private VRCInput inSpinHoldUD;

	// Token: 0x04003EBB RID: 16059
	private VRCInput inSpinHoldLR;

	// Token: 0x04003EBC RID: 16060
	public bool RightHand;

	// Token: 0x04003EBD RID: 16061
	public float MinThrowSpeed;

	// Token: 0x04003EBE RID: 16062
	public float MaxThrowSpeed = 15f;

	// Token: 0x04003EBF RID: 16063
	public float MinThrowHoldTime = 0.25f;

	// Token: 0x04003EC0 RID: 16064
	public float MaxThrowHoldTime = 2f;

	// Token: 0x04003EC1 RID: 16065
	private float throwHoldStartTime;

	// Token: 0x04003EC2 RID: 16066
	private int ReleaseToDropShowCount = 3;

	// Token: 0x04003EC3 RID: 16067
	public float PickupLerpToHandTime = 0.25f;

	// Token: 0x04003EC4 RID: 16068
	private float pickupLerpStartTime = -1f;

	// Token: 0x04003EC5 RID: 16069
	private bool IsFirstFixedUpdateThisFrame = true;

	// Token: 0x04003EC6 RID: 16070
	private float lastFixedUpdateRealTime;

	// Token: 0x04003EC7 RID: 16071
	private int numFixedUpdatesThisFrame;

	// Token: 0x04003EC8 RID: 16072
	private int numFixedUpdatesRemainingThisFrame;

	// Token: 0x04003EC9 RID: 16073
	private Vector3 lastHandPosition = Vector3.zero;

	// Token: 0x04003ECA RID: 16074
	private Quaternion lastHandRotation = Quaternion.identity;

	// Token: 0x04003ECB RID: 16075
	private Vector3 currentHandLinearVelocity = Vector3.zero;

	// Token: 0x04003ECC RID: 16076
	private Vector3 currentHandAngularVelocity = Vector3.zero;

	// Token: 0x04003ECD RID: 16077
	private Quaternion currentPickupRotationDelta = Quaternion.identity;

	// Token: 0x04003ECE RID: 16078
	private Vector3 currentPickupPositionDelta = Vector3.zero;

	// Token: 0x04003ECF RID: 16079
	public float MaxPickupLinearVelocity = -1f;

	// Token: 0x04003ED0 RID: 16080
	public float MaxPickupLinearVelocityDelta = -1f;

	// Token: 0x04003ED1 RID: 16081
	public float PickupVelocityDampPct = 0.04f;

	// Token: 0x04003ED2 RID: 16082
	private Vector3?[] VelocityHistory;

	// Token: 0x04003ED3 RID: 16083
	private Vector3?[] AngularVelocityHistory;

	// Token: 0x04003ED4 RID: 16084
	private int VelocityHistoryStep;

	// Token: 0x04003ED5 RID: 16085
	public int VelocityHistoryWindowSize = 7;

	// Token: 0x04003ED6 RID: 16086
	public int PostThrowVelocityFramesToIntegrate;

	// Token: 0x04003ED7 RID: 16087
	public int SpeedFramesToIntegrate = 3;

	// Token: 0x04003ED8 RID: 16088
	private int postThrowVelocityFramesLeftToIntegrate;

	// Token: 0x04003ED9 RID: 16089
	private VRC_Pickup pickupJustDropped;

	// Token: 0x04003EDA RID: 16090
	private bool useActive;

	// Token: 0x04003EDB RID: 16091
	private bool longHoldMode;

	// Token: 0x04003EDC RID: 16092
	public bool longReachMode;

	// Token: 0x04003EDD RID: 16093
	private bool dropButtonDown;

	// Token: 0x04003EDE RID: 16094
	private float ControllerTooltipResetTimer;

	// Token: 0x04003EDF RID: 16095
	private bool ControllerToolTipEnabled;

	// Token: 0x04003EE0 RID: 16096
	private const float ControllerTooltipCooldownDuration = 30f;

	// Token: 0x04003EE1 RID: 16097
	private float timeOfPickup;

	// Token: 0x04003EE2 RID: 16098
	private float autoHoldUseDelay = 0.5f;

	// Token: 0x04003EE3 RID: 16099
	private float dropDistanceFromHand = -1f;

	// Token: 0x04003EE4 RID: 16100
	private float dropAngleFromHand = -1f;

	// Token: 0x04003EE5 RID: 16101
	private float MinTeleportPickupToHandDistance = 0.01f;

	// Token: 0x04003EE6 RID: 16102
	public static float LerpToHandMinDistance = 0.1f;

	// Token: 0x04003EE7 RID: 16103
	private bool ShowMarkerObject;

	// Token: 0x04003EE8 RID: 16104
	private GameObject closestPointMarker;

	// Token: 0x04003EE9 RID: 16105
	private GameObject pickupTether;

	// Token: 0x04003EEA RID: 16106
	private Vector3 lastPosition;

	// Token: 0x04003EEB RID: 16107
	private Quaternion lastRotation;

	// Token: 0x04003EEC RID: 16108
	private bool _isNearTracking = true;

	// Token: 0x04003EED RID: 16109
	private int lastSelectedPickupID;

	// Token: 0x04003EEE RID: 16110
	private PhysicsTrackerComponent physicsTrackerComp;

	// Token: 0x04003EEF RID: 16111
	private int throwMode;

	// Token: 0x02000B37 RID: 2871
	// (Invoke) Token: 0x060057CF RID: 22479
	private delegate void ApplyMovementToTransform(Transform rx);
}
