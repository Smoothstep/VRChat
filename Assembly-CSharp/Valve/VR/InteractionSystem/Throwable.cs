using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BC5 RID: 3013
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(VelocityEstimator))]
	public class Throwable : MonoBehaviour
	{
		// Token: 0x06005D21 RID: 23841 RVA: 0x00208330 File Offset: 0x00206730
		private void Awake()
		{
			this.velocityEstimator = base.GetComponent<VelocityEstimator>();
			if (this.attachEaseIn)
			{
				this.attachmentFlags &= ~Hand.AttachmentFlags.SnapOnAttach;
			}
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.maxAngularVelocity = 50f;
		}

		// Token: 0x06005D22 RID: 23842 RVA: 0x00208378 File Offset: 0x00206778
		private void OnHandHoverBegin(Hand hand)
		{
			bool flag = false;
			if (!this.attached && hand.GetStandardInteractionButton())
			{
				Rigidbody component = base.GetComponent<Rigidbody>();
				if (component.velocity.magnitude >= this.catchSpeedThreshold)
				{
					hand.AttachObject(base.gameObject, this.attachmentFlags, this.attachmentPoint);
					flag = false;
				}
			}
			if (flag)
			{
				ControllerButtonHints.ShowButtonHint(hand, new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Axis1
				});
			}
		}

		// Token: 0x06005D23 RID: 23843 RVA: 0x002083EE File Offset: 0x002067EE
		private void OnHandHoverEnd(Hand hand)
		{
			ControllerButtonHints.HideButtonHint(hand, new EVRButtonId[]
			{
				EVRButtonId.k_EButton_Axis1
			});
		}

		// Token: 0x06005D24 RID: 23844 RVA: 0x00208401 File Offset: 0x00206801
		private void HandHoverUpdate(Hand hand)
		{
			if (hand.GetStandardInteractionButtonDown())
			{
				hand.AttachObject(base.gameObject, this.attachmentFlags, this.attachmentPoint);
				ControllerButtonHints.HideButtonHint(hand, new EVRButtonId[]
				{
					EVRButtonId.k_EButton_Axis1
				});
			}
		}

		// Token: 0x06005D25 RID: 23845 RVA: 0x00208438 File Offset: 0x00206838
		private void OnAttachedToHand(Hand hand)
		{
			this.attached = true;
			this.onPickUp.Invoke();
			hand.HoverLock(null);
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.isKinematic = true;
			component.interpolation = RigidbodyInterpolation.None;
			if (hand.controller == null)
			{
				this.velocityEstimator.BeginEstimatingVelocity();
			}
			this.attachTime = Time.time;
			this.attachPosition = base.transform.position;
			this.attachRotation = base.transform.rotation;
			if (this.attachEaseIn)
			{
				this.attachEaseInTransform = hand.transform;
				if (!Util.IsNullOrEmpty<string>(this.attachEaseInAttachmentNames))
				{
					float num = float.MaxValue;
					for (int i = 0; i < this.attachEaseInAttachmentNames.Length; i++)
					{
						Transform attachmentTransform = hand.GetAttachmentTransform(this.attachEaseInAttachmentNames[i]);
						float num2 = Quaternion.Angle(attachmentTransform.rotation, this.attachRotation);
						if (num2 < num)
						{
							this.attachEaseInTransform = attachmentTransform;
							num = num2;
						}
					}
				}
			}
			this.snapAttachEaseInCompleted = false;
		}

		// Token: 0x06005D26 RID: 23846 RVA: 0x00208538 File Offset: 0x00206938
		private void OnDetachedFromHand(Hand hand)
		{
			this.attached = false;
			this.onDetachFromHand.Invoke();
			hand.HoverUnlock(null);
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.isKinematic = false;
			component.interpolation = RigidbodyInterpolation.Interpolate;
			Vector3 b = Vector3.zero;
			Vector3 a = Vector3.zero;
			Vector3 vector = Vector3.zero;
			if (hand.controller == null)
			{
				this.velocityEstimator.FinishEstimatingVelocity();
				a = this.velocityEstimator.GetVelocityEstimate();
				vector = this.velocityEstimator.GetAngularVelocityEstimate();
				b = this.velocityEstimator.transform.position;
			}
			else
			{
				a = Player.instance.trackingOriginTransform.TransformVector(hand.controller.velocity);
				vector = Player.instance.trackingOriginTransform.TransformVector(hand.controller.angularVelocity);
				b = hand.transform.position;
			}
			Vector3 rhs = base.transform.TransformPoint(component.centerOfMass) - b;
			component.velocity = a + Vector3.Cross(vector, rhs);
			component.angularVelocity = vector;
			float num = Time.fixedDeltaTime + Time.fixedTime - Time.time;
			base.transform.position += num * a;
			float num2 = 57.29578f * vector.magnitude;
			Vector3 normalized = vector.normalized;
			base.transform.rotation *= Quaternion.AngleAxis(num2 * num, normalized);
		}

		// Token: 0x06005D27 RID: 23847 RVA: 0x002086AC File Offset: 0x00206AAC
		private void HandAttachedUpdate(Hand hand)
		{
			if (!hand.GetStandardInteractionButton())
			{
				base.StartCoroutine(this.LateDetach(hand));
			}
			if (this.attachEaseIn)
			{
				float num = Util.RemapNumberClamped(Time.time, this.attachTime, this.attachTime + this.snapAttachEaseInTime, 0f, 1f);
				if (num < 1f)
				{
					num = this.snapAttachEaseInCurve.Evaluate(num);
					base.transform.position = Vector3.Lerp(this.attachPosition, this.attachEaseInTransform.position, num);
					base.transform.rotation = Quaternion.Lerp(this.attachRotation, this.attachEaseInTransform.rotation, num);
				}
				else if (!this.snapAttachEaseInCompleted)
				{
					base.gameObject.SendMessage("OnThrowableAttachEaseInCompleted", hand, SendMessageOptions.DontRequireReceiver);
					this.snapAttachEaseInCompleted = true;
				}
			}
		}

		// Token: 0x06005D28 RID: 23848 RVA: 0x0020878C File Offset: 0x00206B8C
		private IEnumerator LateDetach(Hand hand)
		{
			yield return new WaitForEndOfFrame();
			hand.DetachObject(base.gameObject, this.restoreOriginalParent);
			yield break;
		}

		// Token: 0x06005D29 RID: 23849 RVA: 0x002087AE File Offset: 0x00206BAE
		private void OnHandFocusAcquired(Hand hand)
		{
			base.gameObject.SetActive(true);
			this.velocityEstimator.BeginEstimatingVelocity();
		}

		// Token: 0x06005D2A RID: 23850 RVA: 0x002087C7 File Offset: 0x00206BC7
		private void OnHandFocusLost(Hand hand)
		{
			base.gameObject.SetActive(false);
			this.velocityEstimator.FinishEstimatingVelocity();
		}

		// Token: 0x040042B6 RID: 17078
		[EnumFlags]
		[Tooltip("The flags used to attach this object to the hand.")]
		public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.ParentToHand;

		// Token: 0x040042B7 RID: 17079
		[Tooltip("Name of the attachment transform under in the hand's hierarchy which the object should should snap to.")]
		public string attachmentPoint;

		// Token: 0x040042B8 RID: 17080
		[Tooltip("How fast must this object be moving to attach due to a trigger hold instead of a trigger press?")]
		public float catchSpeedThreshold;

		// Token: 0x040042B9 RID: 17081
		[Tooltip("When detaching the object, should it return to its original parent?")]
		public bool restoreOriginalParent;

		// Token: 0x040042BA RID: 17082
		public bool attachEaseIn;

		// Token: 0x040042BB RID: 17083
		public AnimationCurve snapAttachEaseInCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040042BC RID: 17084
		public float snapAttachEaseInTime = 0.15f;

		// Token: 0x040042BD RID: 17085
		public string[] attachEaseInAttachmentNames;

		// Token: 0x040042BE RID: 17086
		private VelocityEstimator velocityEstimator;

		// Token: 0x040042BF RID: 17087
		private bool attached;

		// Token: 0x040042C0 RID: 17088
		private float attachTime;

		// Token: 0x040042C1 RID: 17089
		private Vector3 attachPosition;

		// Token: 0x040042C2 RID: 17090
		private Quaternion attachRotation;

		// Token: 0x040042C3 RID: 17091
		private Transform attachEaseInTransform;

		// Token: 0x040042C4 RID: 17092
		public UnityEvent onPickUp;

		// Token: 0x040042C5 RID: 17093
		public UnityEvent onDetachFromHand;

		// Token: 0x040042C6 RID: 17094
		public bool snapAttachEaseInCompleted;
	}
}
