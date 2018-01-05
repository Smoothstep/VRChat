using System;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000B96 RID: 2966
	[RequireComponent(typeof(Interactable))]
	public class ComplexThrowable : MonoBehaviour
	{
		// Token: 0x06005C43 RID: 23619 RVA: 0x00203CF5 File Offset: 0x002020F5
		private void Awake()
		{
			base.GetComponentsInChildren<Rigidbody>(this.rigidBodies);
		}

		// Token: 0x06005C44 RID: 23620 RVA: 0x00203D04 File Offset: 0x00202104
		private void Update()
		{
			for (int i = 0; i < this.holdingHands.Count; i++)
			{
				if (!this.holdingHands[i].GetStandardInteractionButton())
				{
					this.PhysicsDetach(this.holdingHands[i]);
				}
			}
		}

		// Token: 0x06005C45 RID: 23621 RVA: 0x00203D56 File Offset: 0x00202156
		private void OnHandHoverBegin(Hand hand)
		{
			if (this.holdingHands.IndexOf(hand) == -1 && hand.controller != null)
			{
				hand.controller.TriggerHapticPulse(800, EVRButtonId.k_EButton_Axis0);
			}
		}

		// Token: 0x06005C46 RID: 23622 RVA: 0x00203D87 File Offset: 0x00202187
		private void OnHandHoverEnd(Hand hand)
		{
			if (this.holdingHands.IndexOf(hand) == -1 && hand.controller != null)
			{
				hand.controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_Axis0);
			}
		}

		// Token: 0x06005C47 RID: 23623 RVA: 0x00203DB8 File Offset: 0x002021B8
		private void HandHoverUpdate(Hand hand)
		{
			if (hand.GetStandardInteractionButtonDown())
			{
				this.PhysicsAttach(hand);
			}
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x00203DCC File Offset: 0x002021CC
		private void PhysicsAttach(Hand hand)
		{
			this.PhysicsDetach(hand);
			Rigidbody rigidbody = null;
			Vector3 item = Vector3.zero;
			float num = float.MaxValue;
			for (int i = 0; i < this.rigidBodies.Count; i++)
			{
				float num2 = Vector3.Distance(this.rigidBodies[i].worldCenterOfMass, hand.transform.position);
				if (num2 < num)
				{
					rigidbody = this.rigidBodies[i];
					num = num2;
				}
			}
			if (rigidbody == null)
			{
				return;
			}
			if (this.attachMode == ComplexThrowable.AttachMode.FixedJoint)
			{
				Rigidbody rigidbody2 = Util.FindOrAddComponent<Rigidbody>(hand.gameObject);
				rigidbody2.isKinematic = true;
				FixedJoint fixedJoint = hand.gameObject.AddComponent<FixedJoint>();
				fixedJoint.connectedBody = rigidbody;
			}
			hand.HoverLock(null);
			Vector3 b = hand.transform.position - rigidbody.worldCenterOfMass;
			b = Mathf.Min(b.magnitude, 1f) * b.normalized;
			item = rigidbody.transform.InverseTransformPoint(rigidbody.worldCenterOfMass + b);
			hand.AttachObject(base.gameObject, this.attachmentFlags, string.Empty);
			this.holdingHands.Add(hand);
			this.holdingBodies.Add(rigidbody);
			this.holdingPoints.Add(item);
		}

		// Token: 0x06005C49 RID: 23625 RVA: 0x00203F1C File Offset: 0x0020231C
		private bool PhysicsDetach(Hand hand)
		{
			int num = this.holdingHands.IndexOf(hand);
			if (num != -1)
			{
				this.holdingHands[num].DetachObject(base.gameObject, false);
				this.holdingHands[num].HoverUnlock(null);
				if (this.attachMode == ComplexThrowable.AttachMode.FixedJoint)
				{
					UnityEngine.Object.Destroy(this.holdingHands[num].GetComponent<FixedJoint>());
				}
				Util.FastRemove<Hand>(this.holdingHands, num);
				Util.FastRemove<Rigidbody>(this.holdingBodies, num);
				Util.FastRemove<Vector3>(this.holdingPoints, num);
				return true;
			}
			return false;
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x00203FB0 File Offset: 0x002023B0
		private void FixedUpdate()
		{
			if (this.attachMode == ComplexThrowable.AttachMode.Force)
			{
				for (int i = 0; i < this.holdingHands.Count; i++)
				{
					Vector3 vector = this.holdingBodies[i].transform.TransformPoint(this.holdingPoints[i]);
					Vector3 a = this.holdingHands[i].transform.position - vector;
					this.holdingBodies[i].AddForceAtPosition(this.attachForce * a, vector, ForceMode.Acceleration);
					this.holdingBodies[i].AddForceAtPosition(-this.attachForceDamper * this.holdingBodies[i].GetPointVelocity(vector), vector, ForceMode.Acceleration);
				}
			}
		}

		// Token: 0x040041E0 RID: 16864
		public float attachForce = 800f;

		// Token: 0x040041E1 RID: 16865
		public float attachForceDamper = 25f;

		// Token: 0x040041E2 RID: 16866
		public ComplexThrowable.AttachMode attachMode;

		// Token: 0x040041E3 RID: 16867
		[EnumFlags]
		public Hand.AttachmentFlags attachmentFlags;

		// Token: 0x040041E4 RID: 16868
		private List<Hand> holdingHands = new List<Hand>();

		// Token: 0x040041E5 RID: 16869
		private List<Rigidbody> holdingBodies = new List<Rigidbody>();

		// Token: 0x040041E6 RID: 16870
		private List<Vector3> holdingPoints = new List<Vector3>();

		// Token: 0x040041E7 RID: 16871
		private List<Rigidbody> rigidBodies = new List<Rigidbody>();

		// Token: 0x02000B97 RID: 2967
		public enum AttachMode
		{
			// Token: 0x040041E9 RID: 16873
			FixedJoint,
			// Token: 0x040041EA RID: 16874
			Force
		}
	}
}
