using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A39 RID: 2617
public class HandGestureController : MonoBehaviour
{
	// Token: 0x17000BB4 RID: 2996
	// (get) Token: 0x06004EBF RID: 20159 RVA: 0x001A72D9 File Offset: 0x001A56D9
	private float MaxLayerWeight
	{
		get
		{
			if (this._isLocalPlayer)
			{
				return 1f;
			}
			return 2f;
		}
	}

	// Token: 0x06004EC0 RID: 20160 RVA: 0x001A72F4 File Offset: 0x001A56F4
	private void Awake()
	{
		this._inPieLClick = VRCInputManager.FindInput("TouchpadLeftClick");
		this._inPieLX = VRCInputManager.FindInput("TouchpadLeftX");
		this._inPieLY = VRCInputManager.FindInput("TouchpadLeftY");
		this._inPieRClick = VRCInputManager.FindInput("TouchpadRightClick");
		this._inPieRX = VRCInputManager.FindInput("TouchpadRightX");
		this._inPieRY = VRCInputManager.FindInput("TouchpadRightY");
		this._inUseL = VRCInputManager.FindInput("UseLeft");
		this._inUseR = VRCInputManager.FindInput("UseRight");
		this._inDropL = VRCInputManager.FindInput("DropLeft");
		this._inDropR = VRCInputManager.FindInput("DropRight");
		this._inGrabL = VRCInputManager.FindInput("GrabAxisLeft");
		this._inGrabR = VRCInputManager.FindInput("GrabAxisRight");
		this._inTriggerL = VRCInputManager.FindInput("UseAxisLeft");
		this._inTriggerR = VRCInputManager.FindInput("UseAxisRight");
		this._inFaceTouchL = VRCInputManager.FindInput("FaceTouchLeft");
		this._inFaceTouchR = VRCInputManager.FindInput("FaceTouchRight");
		this._inFaceButtonTouchL = VRCInputManager.FindInput("FaceButtonTouchLeft");
		this._inFaceButtonTouchR = VRCInputManager.FindInput("FaceButtonTouchRight");
		this._inTriggerTouchL = VRCInputManager.FindInput("TriggerTouchLeft");
		this._inTriggerTouchR = VRCInputManager.FindInput("TriggerTouchRight");
		this._inThumbRestTouchL = VRCInputManager.FindInput("ThumbRestTouchLeft");
		this._inThumbRestTouchR = VRCInputManager.FindInput("ThumbRestTouchRight");
		this._gestLTimer = 0f;
		this._gestRTimer = 0f;
		this.gestureCollisionLayer = LayerMask.NameToLayer("PickupNoEnvironment");
	}

	// Token: 0x06004EC1 RID: 20161 RVA: 0x001A7488 File Offset: 0x001A5888
	public void Update()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._modelAnimator == null)
		{
			return;
		}
		if (this.HandLayerRight < 0 || this._modelAnimator.layerCount < this.HandLayerRight)
		{
			return;
		}
		if (this.HandLayerLeft < 0 || this._modelAnimator.layerCount < this.HandLayerLeft)
		{
			return;
		}
		VRCInputManager.InputMethod inputMethod = VRCInputManager.InputMethod.Count;
		if (VRCInputManager.IsSupported(VRCInputManager.InputMethod.Vive) && VRCInputManager.IsEnabled(VRCInputManager.InputMethod.Vive))
		{
			inputMethod = VRCInputManager.InputMethod.Vive;
		}
		if (VRCInputManager.IsSupported(VRCInputManager.InputMethod.Oculus) && VRCInputManager.IsEnabled(VRCInputManager.InputMethod.Oculus) && (inputMethod != VRCInputManager.InputMethod.Vive || VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Oculus))
		{
			inputMethod = VRCInputManager.InputMethod.Oculus;
		}
		if (inputMethod == VRCInputManager.InputMethod.Vive || inputMethod == VRCInputManager.InputMethod.Oculus)
		{
			if (this._isLocalPlayer && this._inited)
			{
				int num;
				if (!this._disable && this.ProcessGestureLeftInput(out num, inputMethod))
				{
					this.HandGestureLeft = num;
					if (num > 0)
					{
						this.LeftLayerWeightGoal = this.MaxLayerWeight;
					}
					else
					{
						this.LeftLayerWeightGoal = 0f;
						if (this.LastHandGestureLeft > 0 && this.LastHandGestureLeft < 101)
						{
							this.LeftLayerWeight = Mathf.Clamp01((float)this.LastHandGestureLeft / 100f);
						}
					}
				}
				else if (this.HandGestureLeft <= 101)
				{
					this.LeftLayerWeightGoal = 0f;
				}
				if (!this._disable && this.ProcessGestureRightInput(out num, inputMethod))
				{
					this.HandGestureRight = num;
					if (num > 0)
					{
						this.RightLayerWeightGoal = this.MaxLayerWeight;
					}
					else
					{
						this.RightLayerWeightGoal = 0f;
						if (this.LastHandGestureRight > 0 && this.LastHandGestureRight <= 101)
						{
							this.RightLayerWeight = Mathf.Clamp01((float)this.LastHandGestureRight / 100f);
						}
					}
				}
				else if (this.HandGestureRight <= 101)
				{
					this.RightLayerWeightGoal = 0f;
				}
				if (this._ik != null)
				{
					if (this._ik.isGrasping(false))
					{
						this._wasHoldingL = true;
					}
					else
					{
						if (this._wasHoldingL)
						{
							this.HandGestureLeft = 101;
							this.LeftLayerWeightGoal = this.MaxLayerWeight;
							this._lettingGoL = true;
						}
						if (this._lettingGoL)
						{
							this._letGoWaitL += Time.deltaTime;
							if (this._letGoWaitL > 1f)
							{
								this._wasHoldingL = false;
								this._lettingGoL = false;
								this._letGoWaitL = 0f;
								this.HandGestureLeft = 0;
							}
						}
					}
					if (this._ik.isGrasping(true))
					{
						this._wasHoldingR = true;
					}
					else
					{
						if (this._wasHoldingR)
						{
							this.HandGestureRight = 101;
							this.RightLayerWeightGoal = this.MaxLayerWeight;
							this._lettingGoR = true;
						}
						if (this._lettingGoR)
						{
							this._letGoWaitR += Time.deltaTime;
							if (this._letGoWaitR > 1f)
							{
								this._wasHoldingR = false;
								this._lettingGoR = false;
								this._letGoWaitR = 0f;
								this.HandGestureRight = 0;
							}
						}
					}
				}
				if (this._inPieLClick.button || this._inPieRClick.button)
				{
					this._gestLTimer = 0f;
					this._gestRTimer = 0f;
					this.HandGestureLeft = 0;
					this.HandGestureRight = 0;
					this.LeftLayerWeightGoal = 0f;
					this.RightLayerWeightGoal = 0f;
				}
				if (this.LeftLayerWeight > 0f || this.RightLayerWeight > 0f)
				{
					PoseRecorder.poseContents |= 16;
				}
			}
		}
		if (this.LeftLayerWeight != this.LeftLayerWeightGoal)
		{
			this.LeftLayerWeight = Mathf.MoveTowards(this.LeftLayerWeight, this.LeftLayerWeightGoal, Time.deltaTime * 5f);
		}
		if (this.RightLayerWeight != this.RightLayerWeightGoal)
		{
			this.RightLayerWeight = Mathf.MoveTowards(this.RightLayerWeight, this.RightLayerWeightGoal, Time.deltaTime * 5f);
		}
		if (this.HandGestureRight != 255)
		{
			if (this.HandGestureRight > 0 && this.HandGestureRight < 101)
			{
				this._modelAnimator.SetInteger("HandGestureRight", 0);
				this._modelAnimator.SetLayerWeight(this.HandLayerRight, Mathf.Clamp01((float)this.HandGestureRight / 100f));
			}
			else
			{
				this._modelAnimator.SetInteger("HandGestureRight", this.HandGestureRight - 100);
				this._modelAnimator.SetLayerWeight(this.HandLayerRight, Mathf.Clamp01(this.RightLayerWeight));
			}
		}
		if (this.HandGestureLeft != 255)
		{
			if (this.HandGestureLeft > 0 && this.HandGestureLeft < 101)
			{
				this._modelAnimator.SetInteger("HandGestureLeft", 0);
				this._modelAnimator.SetLayerWeight(this.HandLayerLeft, Mathf.Clamp01((float)this.HandGestureLeft / 100f));
			}
			else
			{
				this._modelAnimator.SetInteger("HandGestureLeft", this.HandGestureLeft - 100);
				this._modelAnimator.SetLayerWeight(this.HandLayerLeft, Mathf.Clamp01(this.LeftLayerWeight));
			}
		}
		this.LastHandGestureLeft = this.HandGestureLeft;
		this.LastHandGestureRight = this.HandGestureRight;
		this.EnableGestureCollision(0, this.LeftLayerWeight > 0.05f && !this._wasHoldingL && !this._lettingGoL);
		this.EnableGestureCollision(1, this.RightLayerWeight > 0.05f && !this._wasHoldingR && !this._lettingGoR);
		if (this._ik != null)
		{
			if (this._ik.isGrasping(false))
			{
				this._wasHoldingL = true;
			}
			if (this._ik.isGrasping(true))
			{
				this._wasHoldingR = true;
			}
		}
	}

	// Token: 0x06004EC2 RID: 20162 RVA: 0x001A7A90 File Offset: 0x001A5E90
	private bool ProcessGestureRightInput(out int value, VRCInputManager.InputMethod inputMethod)
	{
		value = 0;
		if (inputMethod != VRCInputManager.InputMethod.Vive)
		{
			value = this.SelectTouchGesture(true);
			if (value != 0)
			{
				this._gestRTimer = 0f;
			}
			return true;
		}
		if (this._inDropR.button)
		{
			value = 101;
			this._gestRTimer = 0f;
			return true;
		}
		float axis = this._inGrabR.axis;
		if (axis > 0.01f)
		{
			value = Mathf.FloorToInt(axis * 100f);
			this._gestRTimer = 0f;
			return true;
		}
		Vector2 axis2 = new Vector2(this._inPieRX.axis, this._inPieRY.axis);
		if (axis2.sqrMagnitude > 0f)
		{
			int num = this.DualAxisToPieMenu(true, axis2, 5);
			this._gestRTimer += Time.deltaTime;
			if (this._gestRTimer > 0.1f)
			{
				if (num == 0)
				{
					value = 90;
				}
				else
				{
					value = num - 1 + 102;
				}
				return true;
			}
		}
		else
		{
			this._gestRTimer = 0f;
		}
		return false;
	}

	// Token: 0x06004EC3 RID: 20163 RVA: 0x001A7BA8 File Offset: 0x001A5FA8
	private bool ProcessGestureLeftInput(out int value, VRCInputManager.InputMethod inputMethod)
	{
		value = 0;
		if (inputMethod != VRCInputManager.InputMethod.Vive)
		{
			value = this.SelectTouchGesture(false);
			if (value != 0)
			{
				this._gestLTimer = 0f;
			}
			return true;
		}
		if (this._inDropL.button)
		{
			value = 101;
			this._gestLTimer = 0f;
			return true;
		}
		float axis = this._inGrabL.axis;
		if (axis > 0.01f)
		{
			value = Mathf.FloorToInt(axis * 100f);
			this._gestLTimer = 0f;
			return true;
		}
		Vector2 axis2 = new Vector2(this._inPieLX.axis, this._inPieLY.axis);
		if (axis2.sqrMagnitude > 0f)
		{
			int num = this.DualAxisToPieMenu(false, axis2, 5);
			this._gestLTimer += Time.deltaTime;
			if (this._gestLTimer > 0.1f)
			{
				if (num == 0)
				{
					value = 90;
				}
				else
				{
					value = num - 1 + 102;
				}
				return true;
			}
		}
		else
		{
			this._gestLTimer = 0f;
		}
		return false;
	}

	// Token: 0x06004EC4 RID: 20164 RVA: 0x001A7CC0 File Offset: 0x001A60C0
	private int SelectTouchGesture(bool rightHand)
	{
		bool flag = (!rightHand) ? (this._inGrabL.axis > 0.01f) : (this._inGrabR.axis > 0.01f);
		float num = (!rightHand) ? this._inGrabL.axis : this._inGrabR.axis;
		bool flag2 = (!rightHand) ? (this._inTriggerL.axis > 0.01f) : (this._inTriggerR.axis > 0.01f);
		bool flag3 = (!rightHand) ? this._inFaceTouchL.button : this._inFaceTouchR.button;
		bool flag4 = (!rightHand) ? this._inTriggerTouchL.button : this._inTriggerTouchR.button;
		bool flag5 = (!rightHand) ? this._inThumbRestTouchL.button : this._inThumbRestTouchR.button;
		if (!flag && !flag2 && !flag3 && !flag4)
		{
			return 101;
		}
		if (flag && flag4 && flag3)
		{
			return Mathf.FloorToInt(num * 100f);
		}
		if (flag && flag3 && !flag4)
		{
			return 102;
		}
		if (flag && !flag3 && !flag4)
		{
			return 105;
		}
		if (flag && !flag3 && flag4)
		{
			return 106;
		}
		if (flag5 && !flag && !flag4)
		{
			return 103;
		}
		if (flag5 && !flag && flag2)
		{
			return 104;
		}
		return 0;
	}

	// Token: 0x06004EC5 RID: 20165 RVA: 0x001A7E64 File Offset: 0x001A6264
	private int DualAxisToPieMenu(bool rightHand, Vector2 axis, int slices)
	{
		int result = 0;
		float num = 6.28318548f;
		if (axis.magnitude > 0.5f)
		{
			float num2 = (float)slices;
			float num3 = -1f;
			if (rightHand)
			{
				num3 = 1f;
			}
			float num4 = Mathf.Atan2(-axis.y, axis.x * num3);
			num4 += num / 3f;
			if (num4 < 0f)
			{
				num4 += num;
			}
			if (num4 >= num)
			{
				num4 -= num;
			}
			float num5 = 1f / num2 + Mathf.Floor(num2 * num4 / num) / num2;
			result = Mathf.FloorToInt(num5 * num2);
		}
		return result;
	}

	// Token: 0x06004EC6 RID: 20166 RVA: 0x001A7F05 File Offset: 0x001A6305
	public void GetRemoteHandGestures(out int gestL, out int gestR)
	{
		gestL = 0;
		gestR = 0;
		if (this.LeftLayerWeightGoal > 0f)
		{
			gestL = this.HandGestureLeft;
		}
		if (this.RightLayerWeightGoal > 0f)
		{
			gestR = this.HandGestureRight;
		}
	}

	// Token: 0x06004EC7 RID: 20167 RVA: 0x001A7F40 File Offset: 0x001A6340
	public void SetRemoteHandGestures(int gestL, int gestR)
	{
		if (gestL == 0)
		{
			this.HandGestureLeft = 0;
			this.LeftLayerWeightGoal = 0f;
		}
		else
		{
			this.HandGestureLeft = gestL;
			this.LeftLayerWeightGoal = 1f;
		}
		if (gestR == 0)
		{
			this.HandGestureRight = 0;
			this.RightLayerWeightGoal = 0f;
		}
		else
		{
			this.HandGestureRight = gestR;
			this.RightLayerWeightGoal = 1f;
		}
	}

	// Token: 0x06004EC8 RID: 20168 RVA: 0x001A7FAB File Offset: 0x001A63AB
	public void ClearRemoteHandGestures()
	{
		this.HandGestureRight = 0;
		this.HandGestureLeft = 0;
		this.LeftLayerWeightGoal = 0f;
		this.RightLayerWeightGoal = 0f;
	}

	// Token: 0x06004EC9 RID: 20169 RVA: 0x001A7FD1 File Offset: 0x001A63D1
	public void GestureEnable(bool flag)
	{
		this._disable = !flag;
		if (this._disable)
		{
			this.HandGestureRight = 0;
			this.HandGestureLeft = 0;
			this.LeftLayerWeightGoal = 0f;
			this.RightLayerWeightGoal = 0f;
		}
	}

	// Token: 0x06004ECA RID: 20170 RVA: 0x001A800C File Offset: 0x001A640C
	private void StartingState(bool local)
	{
		this._animationController = base.GetComponent<VRC_AnimationController>();
		if (local)
		{
			this._ik = this._animationController.HeadAndHandsIkController.GetComponent<IkController>();
		}
		else
		{
			this._ik = null;
		}
		this._gestLTimer = (this._gestRTimer = 0f);
		this._letGoWaitL = (this._letGoWaitR = 0f);
		this._wasHoldingL = (this._wasHoldingR = false);
		this._lettingGoL = (this._lettingGoR = false);
		this.HandLayerLeft = -1;
		this.HandLayerRight = -1;
		this._inited = false;
	}

	// Token: 0x06004ECB RID: 20171 RVA: 0x001A80AC File Offset: 0x001A64AC
	public void Initialize(Animator modelAnimator, bool local)
	{
		this._modelAnimator = modelAnimator;
		this._isLocalPlayer = local;
		if (modelAnimator == null || modelAnimator.runtimeAnimatorController == null)
		{
			return;
		}
		this.StartingState(local);
		for (int i = 0; i < this._modelAnimator.layerCount; i++)
		{
			if (this._modelAnimator.GetLayerName(i) == "HandLeft")
			{
				this.HandLayerLeft = i;
			}
			if (this._modelAnimator.GetLayerName(i) == "HandRight")
			{
				this.HandLayerRight = i;
			}
		}
		this._inited = true;
	}

	// Token: 0x06004ECC RID: 20172 RVA: 0x001A8153 File Offset: 0x001A6553
	public void GenerateLocalHandCollision()
	{
	}

	// Token: 0x06004ECD RID: 20173 RVA: 0x001A8155 File Offset: 0x001A6555
	private void GenerateHandCollision()
	{
		this.ClearGeneratedHandCollision();
		this._handGestureCollisionEnabled[0] = false;
		this._handGestureCollisionEnabled[1] = false;
		this.GenerateHandCollision(true);
		this.GenerateHandCollision(false);
	}

	// Token: 0x06004ECE RID: 20174 RVA: 0x001A8180 File Offset: 0x001A6580
	private void GenerateHandCollision(bool left)
	{
		Transform hand = null;
		HumanBodyBones bone = HumanBodyBones.LastBone;
		if (!this.FindHandBone(left, out bone, out hand))
		{
			return;
		}
		Transform x = null;
		HumanBodyBones bone2 = HumanBodyBones.LastBone;
		this.FindWristLengthMeasurementBone(left, out bone2, out x);
		Vector3 tposePosition = this._animationController.GetTPosePosition(bone);
		float num = 0.116999991f;
		if (x != null)
		{
			num = (this._animationController.GetTPosePosition(bone2) - tposePosition).magnitude * 0.9f;
		}
		Vector3 vector;
		Vector3 vector2;
		Vector3 normalized;
		Vector3 vector3;
		if (this._animationController.GetTPosePosition((!left) ? HumanBodyBones.RightIndexProximal : HumanBodyBones.LeftIndexProximal, out vector) && this._animationController.GetTPosePosition((!left) ? HumanBodyBones.RightLittleProximal : HumanBodyBones.LeftLittleProximal, out vector2))
		{
			normalized = ((vector2 + vector) / 2f - tposePosition).normalized;
			Vector3 normalized2 = (vector - vector2).normalized;
			vector3 = Vector3.Cross(normalized, normalized2);
			if (!left)
			{
				vector3 *= -1f;
			}
		}
		else
		{
			Vector3 b;
			if (!this._animationController.GetTPosePosition((!left) ? HumanBodyBones.RightLowerArm : HumanBodyBones.LeftLowerArm, out b))
			{
				return;
			}
			normalized = (tposePosition - b).normalized;
			vector3 = Vector3.up;
		}
		Quaternion tposeRotation = this._animationController.GetTPoseRotation(bone);
		Vector3 localPos = Matrix4x4.TRS(tposePosition, tposeRotation, Vector3.one).inverse.MultiplyPoint3x4(tposePosition + normalized * num / 2f);
		Quaternion localRot = Quaternion.Inverse(tposeRotation) * Quaternion.LookRotation(normalized, vector3);
		this.generatedColliders[(!left) ? 1 : 0].Add(this.AttachBoxCollider(hand, localPos, localRot, num));
		float fingerWidth = num * 0.20825f;
		if (left)
		{
			this.GenerateFingerCollision(left, HumanBodyBones.LeftIndexProximal, HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftIndexDistal, fingerWidth);
			this.GenerateFingerCollision(left, HumanBodyBones.LeftMiddleProximal, HumanBodyBones.LeftMiddleIntermediate, HumanBodyBones.LeftMiddleDistal, fingerWidth);
			this.GenerateFingerCollision(left, HumanBodyBones.LeftRingProximal, HumanBodyBones.LeftRingIntermediate, HumanBodyBones.LeftRingDistal, fingerWidth);
			this.GenerateFingerCollision(left, HumanBodyBones.LeftLittleProximal, HumanBodyBones.LeftLittleIntermediate, HumanBodyBones.LeftLittleDistal, fingerWidth);
			this.GenerateFingerCollision(left, HumanBodyBones.LeftThumbProximal, HumanBodyBones.LeftThumbIntermediate, HumanBodyBones.LeftThumbDistal, fingerWidth);
		}
		else
		{
			this.GenerateFingerCollision(left, HumanBodyBones.RightIndexProximal, HumanBodyBones.RightIndexIntermediate, HumanBodyBones.RightIndexDistal, fingerWidth);
			this.GenerateFingerCollision(left, HumanBodyBones.RightMiddleProximal, HumanBodyBones.RightMiddleIntermediate, HumanBodyBones.RightMiddleDistal, fingerWidth);
			this.GenerateFingerCollision(left, HumanBodyBones.RightRingProximal, HumanBodyBones.RightRingIntermediate, HumanBodyBones.RightRingDistal, fingerWidth);
			this.GenerateFingerCollision(left, HumanBodyBones.RightLittleProximal, HumanBodyBones.RightLittleIntermediate, HumanBodyBones.RightLittleDistal, fingerWidth);
			this.GenerateFingerCollision(left, HumanBodyBones.RightThumbProximal, HumanBodyBones.RightThumbIntermediate, HumanBodyBones.RightThumbDistal, fingerWidth);
		}
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x001A8408 File Offset: 0x001A6808
	private void GenerateFingerCollision(bool left, HumanBodyBones proximal, HumanBodyBones intermediate, HumanBodyBones distal, float fingerWidth)
	{
		HandGestureController.FingerInfo fingerInfo = this.GenerateFingerSegmentCollision(left, proximal, intermediate, fingerWidth, -1f, default(Quaternion));
		if (fingerInfo.boneLength < 0f)
		{
			this.GenerateFingerSegmentCollision(left, proximal, distal, fingerWidth, -1f, default(Quaternion));
		}
		else
		{
			fingerInfo = this.GenerateFingerSegmentCollision(left, intermediate, distal, fingerWidth, -1f, default(Quaternion));
		}
		this.GenerateFingerSegmentCollision(left, distal, HumanBodyBones.LastBone, fingerWidth, (fingerInfo.boneLength <= 0f) ? 0.036f : fingerInfo.boneLength, fingerInfo.localRotation);
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x001A84B4 File Offset: 0x001A68B4
	private HandGestureController.FingerInfo GenerateFingerSegmentCollision(bool left, HumanBodyBones fromBone, HumanBodyBones toBone, float boneWidth, float defaultBoneLength = -1f, Quaternion localRot = default(Quaternion))
	{
		Transform boneTransform = this._modelAnimator.GetBoneTransform(fromBone);
		if (boneTransform == null)
		{
			return new HandGestureController.FingerInfo(Quaternion.identity, -1f);
		}
		Vector3 b;
		if (!this._animationController.GetTPosePosition(fromBone, out b))
		{
			return new HandGestureController.FingerInfo(Quaternion.identity, -1f);
		}
		Vector3 a;
		Quaternion localRot2;
		float num;
		if (!this._animationController.GetTPosePosition(toBone, out a))
		{
			if (defaultBoneLength <= 0f)
			{
				return new HandGestureController.FingerInfo(Quaternion.identity, -1f);
			}
			localRot2 = localRot;
			num = defaultBoneLength;
		}
		else
		{
			Vector3 vector = a - b;
			num = vector.magnitude;
			if (Mathf.Approximately(num, 0f))
			{
				return new HandGestureController.FingerInfo(Quaternion.identity, -1f);
			}
			vector /= num;
			Quaternion tposeRotation = this._animationController.GetTPoseRotation(fromBone);
			localRot2 = Quaternion.Inverse(tposeRotation) * Quaternion.LookRotation(vector);
		}
		this.generatedColliders[(!left) ? 1 : 0].Add(this.AttachCapsuleCollider(boneTransform, localRot2, num, boneWidth));
		return new HandGestureController.FingerInfo(localRot2, num);
	}

	// Token: 0x06004ED1 RID: 20177 RVA: 0x001A85E4 File Offset: 0x001A69E4
	private bool FindHandBone(bool left, out HumanBodyBones bone, out Transform boneTransform)
	{
		Transform boneTransform2 = this._modelAnimator.GetBoneTransform((!left) ? HumanBodyBones.RightHand : HumanBodyBones.LeftHand);
		if (!boneTransform2)
		{
			bone = HumanBodyBones.LastBone;
			boneTransform = null;
			return false;
		}
		bone = ((!left) ? HumanBodyBones.RightHand : HumanBodyBones.LeftHand);
		boneTransform = boneTransform2;
		return true;
	}

	// Token: 0x06004ED2 RID: 20178 RVA: 0x001A8638 File Offset: 0x001A6A38
	private bool FindWristLengthMeasurementBone(bool left, out HumanBodyBones bone, out Transform boneTransform)
	{
		HumanBodyBones humanBodyBones = (!left) ? HumanBodyBones.RightMiddleProximal : HumanBodyBones.LeftMiddleProximal;
		Transform boneTransform2 = this._modelAnimator.GetBoneTransform(humanBodyBones);
		if (boneTransform2 == null)
		{
			humanBodyBones = ((!left) ? HumanBodyBones.RightMiddleIntermediate : HumanBodyBones.LeftMiddleIntermediate);
			boneTransform2 = this._modelAnimator.GetBoneTransform(humanBodyBones);
		}
		if (boneTransform2 == null)
		{
			humanBodyBones = ((!left) ? HumanBodyBones.RightMiddleDistal : HumanBodyBones.LeftMiddleDistal);
			boneTransform2 = this._modelAnimator.GetBoneTransform(humanBodyBones);
		}
		if (boneTransform2 == null)
		{
			humanBodyBones = ((!left) ? HumanBodyBones.RightIndexProximal : HumanBodyBones.LeftIndexProximal);
			boneTransform2 = this._modelAnimator.GetBoneTransform(humanBodyBones);
		}
		if (boneTransform2 == null)
		{
			humanBodyBones = ((!left) ? HumanBodyBones.RightIndexIntermediate : HumanBodyBones.LeftIndexIntermediate);
			boneTransform2 = this._modelAnimator.GetBoneTransform(humanBodyBones);
		}
		if (boneTransform2 == null)
		{
			humanBodyBones = ((!left) ? HumanBodyBones.RightIndexDistal : HumanBodyBones.LeftIndexDistal);
			boneTransform2 = this._modelAnimator.GetBoneTransform(humanBodyBones);
		}
		if (boneTransform2 == null)
		{
			bone = HumanBodyBones.LastBone;
			boneTransform = null;
			return false;
		}
		bone = humanBodyBones;
		boneTransform = boneTransform2;
		return true;
	}

	// Token: 0x06004ED3 RID: 20179 RVA: 0x001A8750 File Offset: 0x001A6B50
	private GameObject AttachBoxCollider(Transform hand, Vector3 localPos, Quaternion localRot, float xzSize)
	{
		GameObject gameObject = new GameObject("HandCollider:" + hand.name);
		gameObject.layer = this.gestureCollisionLayer;
		gameObject.transform.localPosition = localPos;
		gameObject.transform.localRotation = localRot;
		gameObject.transform.localScale = Vector3.one;
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.size = new Vector3(xzSize * 0.833f, xzSize * 0.33f, xzSize);
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.useGravity = false;
		rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		rigidbody.mass = 100f;
		HandCollision handCollision = gameObject.AddComponent<HandCollision>();
		handCollision.FollowTransform = hand;
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x06004ED4 RID: 20180 RVA: 0x001A8804 File Offset: 0x001A6C04
	private GameObject AttachCapsuleCollider(Transform fromBone, Quaternion localRot, float boneLength, float boneWidth)
	{
		GameObject gameObject = new GameObject("FingerCollider:" + fromBone.name);
		gameObject.layer = this.gestureCollisionLayer;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = localRot;
		gameObject.transform.localScale = Vector3.one;
		CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
		capsuleCollider.direction = 2;
		capsuleCollider.center = new Vector3(0f, 0f, boneLength / 2f);
		capsuleCollider.height = boneLength;
		capsuleCollider.radius = boneWidth / 2f;
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.useGravity = false;
		rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		rigidbody.mass = 100f;
		HandCollision handCollision = gameObject.AddComponent<HandCollision>();
		handCollision.FollowTransform = fromBone;
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x06004ED5 RID: 20181 RVA: 0x001A88D4 File Offset: 0x001A6CD4
	private void ClearGeneratedHandCollision()
	{
		for (int i = 0; i < 2; i++)
		{
			foreach (GameObject gameObject in this.generatedColliders[i])
			{
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			this.generatedColliders[i].Clear();
		}
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x001A895C File Offset: 0x001A6D5C
	private void EnableGestureCollision(int index, bool enable)
	{
		if (enable == this._handGestureCollisionEnabled[index])
		{
			return;
		}
		this._handGestureCollisionEnabled[index] = enable;
		foreach (GameObject gameObject in this.generatedColliders[index])
		{
			if (gameObject != null)
			{
				gameObject.SetActive(enable);
			}
		}
	}

	// Token: 0x0400370B RID: 14091
	private Animator _modelAnimator;

	// Token: 0x0400370C RID: 14092
	private VRC_AnimationController _animationController;

	// Token: 0x0400370D RID: 14093
	private int HandLayerLeft = -1;

	// Token: 0x0400370E RID: 14094
	private int HandLayerRight = -1;

	// Token: 0x0400370F RID: 14095
	private float LeftLayerWeight;

	// Token: 0x04003710 RID: 14096
	private int HandGestureLeft = -1;

	// Token: 0x04003711 RID: 14097
	private int LastHandGestureLeft = -1;

	// Token: 0x04003712 RID: 14098
	private float RightLayerWeight;

	// Token: 0x04003713 RID: 14099
	private int HandGestureRight = -1;

	// Token: 0x04003714 RID: 14100
	private int LastHandGestureRight = -1;

	// Token: 0x04003715 RID: 14101
	private float LeftLayerWeightGoal;

	// Token: 0x04003716 RID: 14102
	private float RightLayerWeightGoal;

	// Token: 0x04003717 RID: 14103
	private bool _isLocalPlayer;

	// Token: 0x04003718 RID: 14104
	private bool _inited;

	// Token: 0x04003719 RID: 14105
	private bool _disable;

	// Token: 0x0400371A RID: 14106
	private const float GESTURE_WEIGHT_SPEED = 5f;

	// Token: 0x0400371B RID: 14107
	private const float PAUSE_BEFORE_GESTURE = 0.1f;

	// Token: 0x0400371C RID: 14108
	private const float HOLD_AFTER_LETGO = 1f;

	// Token: 0x0400371D RID: 14109
	private GestureMenu leftMenu;

	// Token: 0x0400371E RID: 14110
	private GestureMenu rightMenu;

	// Token: 0x0400371F RID: 14111
	private IkController _ik;

	// Token: 0x04003720 RID: 14112
	private VRCInput _inPieRClick;

	// Token: 0x04003721 RID: 14113
	private VRCInput _inPieLClick;

	// Token: 0x04003722 RID: 14114
	private VRCInput _inPieRX;

	// Token: 0x04003723 RID: 14115
	private VRCInput _inPieRY;

	// Token: 0x04003724 RID: 14116
	private VRCInput _inPieLX;

	// Token: 0x04003725 RID: 14117
	private VRCInput _inPieLY;

	// Token: 0x04003726 RID: 14118
	private VRCInput _inUseL;

	// Token: 0x04003727 RID: 14119
	private VRCInput _inUseR;

	// Token: 0x04003728 RID: 14120
	private VRCInput _inDropL;

	// Token: 0x04003729 RID: 14121
	private VRCInput _inDropR;

	// Token: 0x0400372A RID: 14122
	private VRCInput _inGrabL;

	// Token: 0x0400372B RID: 14123
	private VRCInput _inGrabR;

	// Token: 0x0400372C RID: 14124
	private VRCInput _inTriggerL;

	// Token: 0x0400372D RID: 14125
	private VRCInput _inTriggerR;

	// Token: 0x0400372E RID: 14126
	private VRCInput _inFaceTouchL;

	// Token: 0x0400372F RID: 14127
	private VRCInput _inFaceTouchR;

	// Token: 0x04003730 RID: 14128
	private VRCInput _inFaceButtonTouchL;

	// Token: 0x04003731 RID: 14129
	private VRCInput _inFaceButtonTouchR;

	// Token: 0x04003732 RID: 14130
	private VRCInput _inTriggerTouchL;

	// Token: 0x04003733 RID: 14131
	private VRCInput _inTriggerTouchR;

	// Token: 0x04003734 RID: 14132
	private VRCInput _inThumbRestTouchL;

	// Token: 0x04003735 RID: 14133
	private VRCInput _inThumbRestTouchR;

	// Token: 0x04003736 RID: 14134
	private float _gestLTimer;

	// Token: 0x04003737 RID: 14135
	private float _gestRTimer;

	// Token: 0x04003738 RID: 14136
	private bool _wasHoldingL;

	// Token: 0x04003739 RID: 14137
	private bool _wasHoldingR;

	// Token: 0x0400373A RID: 14138
	private bool _lettingGoL;

	// Token: 0x0400373B RID: 14139
	private bool _lettingGoR;

	// Token: 0x0400373C RID: 14140
	private float _letGoWaitL;

	// Token: 0x0400373D RID: 14141
	private float _letGoWaitR;

	// Token: 0x0400373E RID: 14142
	private const float DEFAULT_PALM_LENGTH = 0.13f;

	// Token: 0x0400373F RID: 14143
	private const float PALM_LENGTH_SCALE = 0.9f;

	// Token: 0x04003740 RID: 14144
	private const float PALM_THICKNESS_SCALE = 0.33f;

	// Token: 0x04003741 RID: 14145
	private const float PALM_WIDTH_SCALE = 0.833f;

	// Token: 0x04003742 RID: 14146
	private const float FINGER_WIDTH_SCALE = 0.20825f;

	// Token: 0x04003743 RID: 14147
	private const float DEFAULT_FINGER_BONE_LENGTH = 0.036f;

	// Token: 0x04003744 RID: 14148
	private const float HAND_MASS = 100f;

	// Token: 0x04003745 RID: 14149
	private int gestureCollisionLayer;

	// Token: 0x04003746 RID: 14150
	private List<GameObject>[] generatedColliders = new List<GameObject>[]
	{
		new List<GameObject>(),
		new List<GameObject>()
	};

	// Token: 0x04003747 RID: 14151
	private bool[] _handGestureCollisionEnabled = new bool[2];

	// Token: 0x02000A3A RID: 2618
	private enum _Gesture
	{
		// Token: 0x04003749 RID: 14153
		None,
		// Token: 0x0400374A RID: 14154
		Fist,
		// Token: 0x0400374B RID: 14155
		LetGo = 101,
		// Token: 0x0400374C RID: 14156
		Point,
		// Token: 0x0400374D RID: 14157
		Peace,
		// Token: 0x0400374E RID: 14158
		RockNRoll,
		// Token: 0x0400374F RID: 14159
		Gun,
		// Token: 0x04003750 RID: 14160
		ThumbsUp
	}

	// Token: 0x02000A3B RID: 2619
	private struct FingerInfo
	{
		// Token: 0x06004ED7 RID: 20183 RVA: 0x001A89E0 File Offset: 0x001A6DE0
		public FingerInfo(Quaternion localRot, float length)
		{
			this.localRotation = localRot;
			this.boneLength = length;
		}

		// Token: 0x04003751 RID: 14161
		public Quaternion localRotation;

		// Token: 0x04003752 RID: 14162
		public float boneLength;
	}
}
