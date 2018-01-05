using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BD9 RID: 3033
	[RequireComponent(typeof(Interactable))]
	public class Longbow : MonoBehaviour
	{
		// Token: 0x06005DCD RID: 24013 RVA: 0x0020D235 File Offset: 0x0020B635
		private void OnAttachedToHand(Hand attachedHand)
		{
			this.hand = attachedHand;
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x0020D23E File Offset: 0x0020B63E
		private void Awake()
		{
			this.newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(new UnityAction(this.OnNewPosesApplied));
		}

		// Token: 0x06005DCF RID: 24015 RVA: 0x0020D257 File Offset: 0x0020B657
		private void OnEnable()
		{
			this.newPosesAppliedAction.enabled = true;
		}

		// Token: 0x06005DD0 RID: 24016 RVA: 0x0020D265 File Offset: 0x0020B665
		private void OnDisable()
		{
			this.newPosesAppliedAction.enabled = false;
		}

		// Token: 0x06005DD1 RID: 24017 RVA: 0x0020D273 File Offset: 0x0020B673
		private void LateUpdate()
		{
			if (this.deferNewPoses)
			{
				this.lateUpdatePos = base.transform.position;
				this.lateUpdateRot = base.transform.rotation;
			}
		}

		// Token: 0x06005DD2 RID: 24018 RVA: 0x0020D2A2 File Offset: 0x0020B6A2
		private void OnNewPosesApplied()
		{
			if (this.deferNewPoses)
			{
				base.transform.position = this.lateUpdatePos;
				base.transform.rotation = this.lateUpdateRot;
				this.deferNewPoses = false;
			}
		}

		// Token: 0x06005DD3 RID: 24019 RVA: 0x0020D2D8 File Offset: 0x0020B6D8
		private void HandAttachedUpdate(Hand hand)
		{
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			this.EvaluateHandedness();
			if (this.nocked)
			{
				this.deferNewPoses = true;
				Vector3 lhs = this.arrowHand.arrowNockTransform.parent.position - this.nockRestTransform.position;
				float num = Util.RemapNumberClamped(Time.time, this.nockLerpStartTime, this.nockLerpStartTime + this.lerpDuration, 0f, 1f);
				float d = Util.RemapNumberClamped(lhs.magnitude, 0.05f, 0.5f, 0f, 1f);
				Vector3 normalized = (Player.instance.hmdTransform.position + Vector3.down * 0.05f - this.arrowHand.arrowNockTransform.parent.position).normalized;
				Vector3 a = this.arrowHand.arrowNockTransform.parent.position + normalized * this.drawOffset * d;
				Vector3 normalized2 = (a - this.pivotTransform.position).normalized;
				Vector3 normalized3 = (this.handleTransform.position - this.pivotTransform.position).normalized;
				this.bowLeftVector = -Vector3.Cross(normalized3, normalized2);
				this.pivotTransform.rotation = Quaternion.Lerp(this.nockLerpStartRotation, Quaternion.LookRotation(normalized2, this.bowLeftVector), num);
				if (Vector3.Dot(lhs, -this.nockTransform.forward) > 0f)
				{
					float num2 = lhs.magnitude * num;
					this.nockTransform.localPosition = new Vector3(0f, 0f, Mathf.Clamp(-num2, -0.5f, 0f));
					this.nockDistanceTravelled = -this.nockTransform.localPosition.z;
					this.arrowVelocity = Util.RemapNumber(this.nockDistanceTravelled, 0.05f, 0.5f, this.arrowMinVelocity, this.arrowMaxVelocity);
					this.drawTension = Util.RemapNumberClamped(this.nockDistanceTravelled, 0f, 0.5f, 0f, 1f);
					this.bowDrawLinearMapping.value = this.drawTension;
					if (this.nockDistanceTravelled > 0.05f)
					{
						this.pulled = true;
					}
					else
					{
						this.pulled = false;
					}
					if (this.nockDistanceTravelled > this.lastTickDistance + this.hapticDistanceThreshold || this.nockDistanceTravelled < this.lastTickDistance - this.hapticDistanceThreshold)
					{
						ushort durationMicroSec = (ushort)Util.RemapNumber(this.nockDistanceTravelled, 0f, 0.5f, 100f, 500f);
						hand.controller.TriggerHapticPulse(durationMicroSec, EVRButtonId.k_EButton_Axis0);
						hand.otherHand.controller.TriggerHapticPulse(durationMicroSec, EVRButtonId.k_EButton_Axis0);
						this.drawSound.PlayBowTensionClicks(this.drawTension);
						this.lastTickDistance = this.nockDistanceTravelled;
					}
					if (this.nockDistanceTravelled >= 0.5f && Time.time > this.nextStrainTick)
					{
						hand.controller.TriggerHapticPulse(400, EVRButtonId.k_EButton_Axis0);
						hand.otherHand.controller.TriggerHapticPulse(400, EVRButtonId.k_EButton_Axis0);
						this.drawSound.PlayBowTensionClicks(this.drawTension);
						this.nextStrainTick = Time.time + UnityEngine.Random.Range(this.minStrainTickTime, this.maxStrainTickTime);
					}
				}
				else
				{
					this.nockTransform.localPosition = new Vector3(0f, 0f, 0f);
					this.bowDrawLinearMapping.value = 0f;
				}
			}
			else if (this.lerpBackToZeroRotation)
			{
				float num3 = Util.RemapNumber(Time.time, this.lerpStartTime, this.lerpStartTime + this.lerpDuration, 0f, 1f);
				this.pivotTransform.localRotation = Quaternion.Lerp(this.lerpStartRotation, Quaternion.identity, num3);
				if (num3 >= 1f)
				{
					this.lerpBackToZeroRotation = false;
				}
			}
		}

		// Token: 0x06005DD4 RID: 24020 RVA: 0x0020D714 File Offset: 0x0020BB14
		public void ArrowReleased()
		{
			this.nocked = false;
			this.hand.HoverUnlock(base.GetComponent<Interactable>());
			this.hand.otherHand.HoverUnlock(this.arrowHand.GetComponent<Interactable>());
			if (this.releaseSound != null)
			{
				this.releaseSound.Play();
			}
			base.StartCoroutine(this.ResetDrawAnim());
		}

		// Token: 0x06005DD5 RID: 24021 RVA: 0x0020D780 File Offset: 0x0020BB80
		private IEnumerator ResetDrawAnim()
		{
			float startTime = Time.time;
			float startLerp = this.drawTension;
			while (Time.time < startTime + 0.02f)
			{
				float lerp = Util.RemapNumberClamped(Time.time, startTime, startTime + 0.02f, startLerp, 0f);
				this.bowDrawLinearMapping.value = lerp;
				yield return null;
			}
			this.bowDrawLinearMapping.value = 0f;
			yield break;
		}

		// Token: 0x06005DD6 RID: 24022 RVA: 0x0020D79B File Offset: 0x0020BB9B
		public float GetArrowVelocity()
		{
			return this.arrowVelocity;
		}

		// Token: 0x06005DD7 RID: 24023 RVA: 0x0020D7A3 File Offset: 0x0020BBA3
		public void StartRotationLerp()
		{
			this.lerpStartTime = Time.time;
			this.lerpBackToZeroRotation = true;
			this.lerpStartRotation = this.pivotTransform.localRotation;
			Util.ResetTransform(this.nockTransform, true);
		}

		// Token: 0x06005DD8 RID: 24024 RVA: 0x0020D7D4 File Offset: 0x0020BBD4
		public void StartNock(ArrowHand currentArrowHand)
		{
			this.arrowHand = currentArrowHand;
			this.hand.HoverLock(base.GetComponent<Interactable>());
			this.nocked = true;
			this.nockLerpStartTime = Time.time;
			this.nockLerpStartRotation = this.pivotTransform.rotation;
			this.arrowSlideSound.Play();
			this.DoHandednessCheck();
		}

		// Token: 0x06005DD9 RID: 24025 RVA: 0x0020D830 File Offset: 0x0020BC30
		private void EvaluateHandedness()
		{
			if (this.hand.GuessCurrentHandType() == Hand.HandType.Left)
			{
				if (this.possibleHandSwitch && this.currentHandGuess == Longbow.Handedness.Left)
				{
					this.possibleHandSwitch = false;
				}
				if (!this.possibleHandSwitch && this.currentHandGuess == Longbow.Handedness.Right)
				{
					this.possibleHandSwitch = true;
					this.timeOfPossibleHandSwitch = Time.time;
				}
				if (this.possibleHandSwitch && Time.time > this.timeOfPossibleHandSwitch + this.timeBeforeConfirmingHandSwitch)
				{
					this.currentHandGuess = Longbow.Handedness.Left;
					this.possibleHandSwitch = false;
				}
			}
			else
			{
				if (this.possibleHandSwitch && this.currentHandGuess == Longbow.Handedness.Right)
				{
					this.possibleHandSwitch = false;
				}
				if (!this.possibleHandSwitch && this.currentHandGuess == Longbow.Handedness.Left)
				{
					this.possibleHandSwitch = true;
					this.timeOfPossibleHandSwitch = Time.time;
				}
				if (this.possibleHandSwitch && Time.time > this.timeOfPossibleHandSwitch + this.timeBeforeConfirmingHandSwitch)
				{
					this.currentHandGuess = Longbow.Handedness.Right;
					this.possibleHandSwitch = false;
				}
			}
		}

		// Token: 0x06005DDA RID: 24026 RVA: 0x0020D940 File Offset: 0x0020BD40
		private void DoHandednessCheck()
		{
			if (this.currentHandGuess == Longbow.Handedness.Left)
			{
				this.pivotTransform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				this.pivotTransform.localScale = new Vector3(1f, -1f, 1f);
			}
		}

		// Token: 0x06005DDB RID: 24027 RVA: 0x0020D99B File Offset: 0x0020BD9B
		public void ArrowInPosition()
		{
			this.DoHandednessCheck();
			if (this.nockSound != null)
			{
				this.nockSound.Play();
			}
		}

		// Token: 0x06005DDC RID: 24028 RVA: 0x0020D9BF File Offset: 0x0020BDBF
		public void ReleaseNock()
		{
			this.nocked = false;
			this.hand.HoverUnlock(base.GetComponent<Interactable>());
			base.StartCoroutine(this.ResetDrawAnim());
		}

		// Token: 0x06005DDD RID: 24029 RVA: 0x0020D9E8 File Offset: 0x0020BDE8
		private void ShutDown()
		{
			if (this.hand != null && this.hand.otherHand.currentAttachedObject != null && this.hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>() != null && this.hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>().itemPackage == this.arrowHandItemPackage)
			{
				this.hand.otherHand.DetachObject(this.hand.otherHand.currentAttachedObject, true);
			}
		}

		// Token: 0x06005DDE RID: 24030 RVA: 0x0020DA8C File Offset: 0x0020BE8C
		private void OnHandFocusLost(Hand hand)
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06005DDF RID: 24031 RVA: 0x0020DA9A File Offset: 0x0020BE9A
		private void OnHandFocusAcquired(Hand hand)
		{
			base.gameObject.SetActive(true);
			this.OnAttachedToHand(hand);
		}

		// Token: 0x06005DE0 RID: 24032 RVA: 0x0020DAAF File Offset: 0x0020BEAF
		private void OnDetachedFromHand(Hand hand)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x0020DABC File Offset: 0x0020BEBC
		private void OnDestroy()
		{
			this.ShutDown();
		}

		// Token: 0x0400436B RID: 17259
		public Longbow.Handedness currentHandGuess;

		// Token: 0x0400436C RID: 17260
		private float timeOfPossibleHandSwitch;

		// Token: 0x0400436D RID: 17261
		private float timeBeforeConfirmingHandSwitch = 1.5f;

		// Token: 0x0400436E RID: 17262
		private bool possibleHandSwitch;

		// Token: 0x0400436F RID: 17263
		public Transform pivotTransform;

		// Token: 0x04004370 RID: 17264
		public Transform handleTransform;

		// Token: 0x04004371 RID: 17265
		private Hand hand;

		// Token: 0x04004372 RID: 17266
		private ArrowHand arrowHand;

		// Token: 0x04004373 RID: 17267
		public Transform nockTransform;

		// Token: 0x04004374 RID: 17268
		public Transform nockRestTransform;

		// Token: 0x04004375 RID: 17269
		public bool autoSpawnArrowHand = true;

		// Token: 0x04004376 RID: 17270
		public ItemPackage arrowHandItemPackage;

		// Token: 0x04004377 RID: 17271
		public GameObject arrowHandPrefab;

		// Token: 0x04004378 RID: 17272
		public bool nocked;

		// Token: 0x04004379 RID: 17273
		public bool pulled;

		// Token: 0x0400437A RID: 17274
		private const float minPull = 0.05f;

		// Token: 0x0400437B RID: 17275
		private const float maxPull = 0.5f;

		// Token: 0x0400437C RID: 17276
		private float nockDistanceTravelled;

		// Token: 0x0400437D RID: 17277
		private float hapticDistanceThreshold = 0.01f;

		// Token: 0x0400437E RID: 17278
		private float lastTickDistance;

		// Token: 0x0400437F RID: 17279
		private const float bowPullPulseStrengthLow = 100f;

		// Token: 0x04004380 RID: 17280
		private const float bowPullPulseStrengthHigh = 500f;

		// Token: 0x04004381 RID: 17281
		private Vector3 bowLeftVector;

		// Token: 0x04004382 RID: 17282
		public float arrowMinVelocity = 3f;

		// Token: 0x04004383 RID: 17283
		public float arrowMaxVelocity = 30f;

		// Token: 0x04004384 RID: 17284
		private float arrowVelocity = 30f;

		// Token: 0x04004385 RID: 17285
		private float minStrainTickTime = 0.1f;

		// Token: 0x04004386 RID: 17286
		private float maxStrainTickTime = 0.5f;

		// Token: 0x04004387 RID: 17287
		private float nextStrainTick;

		// Token: 0x04004388 RID: 17288
		private bool lerpBackToZeroRotation;

		// Token: 0x04004389 RID: 17289
		private float lerpStartTime;

		// Token: 0x0400438A RID: 17290
		private float lerpDuration = 0.15f;

		// Token: 0x0400438B RID: 17291
		private Quaternion lerpStartRotation;

		// Token: 0x0400438C RID: 17292
		private float nockLerpStartTime;

		// Token: 0x0400438D RID: 17293
		private Quaternion nockLerpStartRotation;

		// Token: 0x0400438E RID: 17294
		public float drawOffset = 0.06f;

		// Token: 0x0400438F RID: 17295
		public LinearMapping bowDrawLinearMapping;

		// Token: 0x04004390 RID: 17296
		private bool deferNewPoses;

		// Token: 0x04004391 RID: 17297
		private Vector3 lateUpdatePos;

		// Token: 0x04004392 RID: 17298
		private Quaternion lateUpdateRot;

		// Token: 0x04004393 RID: 17299
		public SoundBowClick drawSound;

		// Token: 0x04004394 RID: 17300
		private float drawTension;

		// Token: 0x04004395 RID: 17301
		public SoundPlayOneshot arrowSlideSound;

		// Token: 0x04004396 RID: 17302
		public SoundPlayOneshot releaseSound;

		// Token: 0x04004397 RID: 17303
		public SoundPlayOneshot nockSound;

		// Token: 0x04004398 RID: 17304
		private SteamVR_Events.Action newPosesAppliedAction;

		// Token: 0x02000BDA RID: 3034
		public enum Handedness
		{
			// Token: 0x0400439A RID: 17306
			Left,
			// Token: 0x0400439B RID: 17307
			Right
		}
	}
}
