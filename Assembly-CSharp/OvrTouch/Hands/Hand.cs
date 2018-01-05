using System;
using System.Collections.Generic;
using System.Linq;
using OvrTouch.Controllers;
using UnityEngine;

namespace OvrTouch.Hands
{
	// Token: 0x0200071A RID: 1818
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(VelocityTracker))]
	public class Hand : MonoBehaviour
	{
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06003B46 RID: 15174 RVA: 0x0012A6BF File Offset: 0x00128ABF
		public HandednessId Handedness
		{
			get
			{
				return this.m_handedness;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06003B47 RID: 15175 RVA: 0x0012A6C7 File Offset: 0x00128AC7
		public bool IsGrabbingGrabbable
		{
			get
			{
				return this.m_grabbedGrabbable != null;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06003B48 RID: 15176 RVA: 0x0012A6D5 File Offset: 0x00128AD5
		public Rigidbody Rigidbody
		{
			get
			{
				return this.m_rigidbody;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06003B49 RID: 15177 RVA: 0x0012A6DD File Offset: 0x00128ADD
		public Vector3 LinearVelocity
		{
			get
			{
				return this.m_velocityTracker.TrackedLinearVelocity;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06003B4A RID: 15178 RVA: 0x0012A6EA File Offset: 0x00128AEA
		public Vector3 AngularVelocity
		{
			get
			{
				return this.m_velocityTracker.TrackedAngularVelocity;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06003B4B RID: 15179 RVA: 0x0012A6F7 File Offset: 0x00128AF7
		public TrackedController TrackedController
		{
			get
			{
				return this.m_trackedController;
			}
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x0012A6FF File Offset: 0x00128AFF
		public void SetVisible(bool visible)
		{
			this.m_meshRoot.gameObject.SetActive(visible);
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x0012A714 File Offset: 0x00128B14
		private void Start()
		{
			this.m_colliders = (from childCollider in base.GetComponentsInChildren<Collider>()
			where !childCollider.isTrigger
			select childCollider).ToArray<Collider>();
			this.CollisionEnable(false);
			this.m_rigidbody = base.GetComponent<Rigidbody>();
			this.m_velocityTracker = base.GetComponent<VelocityTracker>();
			this.m_animLayerIndexPoint = this.m_animator.GetLayerIndex("Point Layer");
			this.m_animLayerIndexThumb = this.m_animator.GetLayerIndex("Thumb Layer");
			this.m_animParamIndexFlex = Animator.StringToHash("Flex");
			this.m_animParamIndexPose = Animator.StringToHash("Pose");
			HandParticles handParticles = UnityEngine.Object.Instantiate<HandParticles>(this.m_handParticlesPf);
			handParticles.transform.parent = base.transform;
			handParticles.transform.position = base.transform.position;
			handParticles.transform.rotation = base.transform.rotation;
			handParticles.SetHand(this);
			this.m_trackedController = TrackedController.FindOrCreate(this.m_handedness);
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x0012A820 File Offset: 0x00128C20
		private void FixedUpdate()
		{
			float flex = this.m_flex;
			this.m_flex = this.InputFlex();
			this.m_point = this.InputValueRateChange(this.InputPoint(), this.m_point);
			this.m_thumbsUp = this.InputValueRateChange(this.InputThumbsUp(), this.m_thumbsUp);
			this.GrabVolumeAdvance();
			this.GrabAdvance(flex);
			this.CollisionAdvance();
			this.AnimationAdvance();
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x0012A88C File Offset: 0x00128C8C
		private void LateUpdate()
		{
			Vector3 vector = base.transform.position;
			Quaternion quaternion = base.transform.rotation;
			Hand.RegistrationTransform registrationTransform = this.HandRegistration();
			vector = this.m_trackedController.transform.position + this.m_trackedController.transform.rotation * registrationTransform.Translation;
			quaternion = this.m_trackedController.transform.rotation * registrationTransform.Rotation;
			this.GrabbableAdvance(vector, quaternion);
			base.transform.position = vector;
			base.transform.rotation = quaternion;
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x0012A928 File Offset: 0x00128D28
		private void OnTriggerEnter(Collider otherCollider)
		{
			GrabTrigger component = otherCollider.GetComponent<GrabTrigger>();
			if (component == null)
			{
				return;
			}
			Grabbable grabbable = component.Grabbable;
			int num = 0;
			this.m_grabCandidates.TryGetValue(grabbable, out num);
			this.m_grabCandidates[grabbable] = num + 1;
			if (num == 0)
			{
				grabbable.OverlapBegin(this);
				if (this.m_wasGrabVolumeEnabled == this.m_grabVolumeEnabled)
				{
					this.m_trackedController.PlayHapticEvent(320f, 0.25f, 0.05f);
				}
			}
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x0012A9A8 File Offset: 0x00128DA8
		private void OnTriggerExit(Collider otherCollider)
		{
			GrabTrigger component = otherCollider.GetComponent<GrabTrigger>();
			if (component == null)
			{
				return;
			}
			Grabbable grabbable = component.Grabbable;
			int num = 0;
			if (!this.m_grabCandidates.TryGetValue(grabbable, out num))
			{
				return;
			}
			if (num > 1)
			{
				this.m_grabCandidates[grabbable] = num - 1;
			}
			else
			{
				grabbable.OverlapEnd(this);
				this.m_grabCandidates.Remove(grabbable);
			}
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x0012AA17 File Offset: 0x00128E17
		private float InputFlex()
		{
			return this.m_trackedController.GripTrigger;
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x0012AA24 File Offset: 0x00128E24
		private bool InputPoint()
		{
			return this.m_trackedController.IsPoint;
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x0012AA31 File Offset: 0x00128E31
		private bool InputThumbsUp()
		{
			return this.m_trackedController.IsThumbsUp;
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x0012AA40 File Offset: 0x00128E40
		private float InputValueRateChange(bool isDown, float value)
		{
			float num = Time.deltaTime * 20f;
			float num2 = (!isDown) ? -1f : 1f;
			return Mathf.Clamp01(value + num * num2);
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x0012AA7C File Offset: 0x00128E7C
		private void AnimationAdvance()
		{
			HandPoseId value = (!(this.m_grabbedHandPose != null)) ? HandPoseId.Default : this.m_grabbedHandPose.PoseId;
			this.m_animator.SetInteger(this.m_animParamIndexPose, (int)value);
			this.m_animator.SetFloat(this.m_animParamIndexFlex, this.m_flex);
			bool flag = !this.IsGrabbingGrabbable || (this.m_grabbedHandPose != null && this.m_grabbedHandPose.AllowPointing);
			float weight = (!flag) ? 0f : this.m_point;
			this.m_animator.SetLayerWeight(this.m_animLayerIndexPoint, weight);
			bool flag2 = !this.IsGrabbingGrabbable || (this.m_grabbedHandPose != null && this.m_grabbedHandPose.AllowThumbsUp);
			float weight2 = (!flag2) ? 0f : this.m_thumbsUp;
			this.m_animator.SetLayerWeight(this.m_animLayerIndexThumb, weight2);
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x0012AB88 File Offset: 0x00128F88
		private void CollisionAdvance()
		{
			bool enabled = this.IsGrabbingGrabbable || this.m_flex >= 0.96f;
			this.CollisionEnable(enabled);
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x0012ABBC File Offset: 0x00128FBC
		private void CollisionEnable(bool enabled)
		{
			if (this.m_collisionEnabled == enabled)
			{
				return;
			}
			this.m_collisionEnabled = enabled;
			foreach (Collider collider in this.m_colliders)
			{
				collider.enabled = this.m_collisionEnabled;
			}
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x0012AC08 File Offset: 0x00129008
		private void GrabAdvance(float prevFlex)
		{
			if (this.m_flex >= 0.55f && prevFlex < 0.55f)
			{
				this.GrabBegin();
			}
			else if (this.m_flex <= 0.35f && prevFlex > 0.35f)
			{
				this.GrabEnd();
			}
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x0012AC5C File Offset: 0x0012905C
		private void GrabBegin()
		{
			float num = float.MaxValue;
			Grabbable grabbable = null;
			GrabPoint grabPoint = null;
			foreach (Grabbable grabbable2 in this.m_grabCandidates.Keys)
			{
				if (!grabbable2.IsGrabbed || grabbable2.AllowOffhandGrab)
				{
					foreach (GrabPoint grabPoint2 in grabbable2.GrabPoints)
					{
						Vector3 b = grabPoint2.GrabCollider.ClosestPointOnBounds(this.m_gripTransform.position);
						float sqrMagnitude = (this.m_gripTransform.position - b).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							grabbable = grabbable2;
							grabPoint = grabPoint2;
						}
					}
				}
			}
			this.GrabVolumeEnable(false);
			if (grabbable != null)
			{
				if (grabbable.IsGrabbed)
				{
					grabbable.GrabbedHand.OffhandGrabbed(grabbable);
				}
				this.GrabbableGrab(grabbable, grabPoint);
			}
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x0012AD8C File Offset: 0x0012918C
		private void GrabEnd()
		{
			if (this.IsGrabbingGrabbable)
			{
				bool flag = this.m_velocityTracker.TrackedLinearVelocity.magnitude >= 1f;
				Vector3 linearVelocity = Vector3.zero;
				Vector3 angularVelocity = Vector3.zero;
				if (flag)
				{
					linearVelocity = this.m_velocityTracker.TrackedLinearVelocity;
					angularVelocity = this.m_velocityTracker.TrackedAngularVelocity;
				}
				else
				{
					linearVelocity = this.m_velocityTracker.FrameLinearVelocity;
					angularVelocity = this.m_velocityTracker.FrameAngularVelocity;
				}
				this.GrabbableRelease(linearVelocity, angularVelocity);
			}
			this.GrabVolumeEnable(true);
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0012AE18 File Offset: 0x00129218
		private void GrabbableAdvance(Vector3 finalPosition, Quaternion finalRotation)
		{
			if (!this.IsGrabbingGrabbable)
			{
				return;
			}
			bool flag = this.m_grabbedHandPose != null && (this.m_grabbedHandPose.AttachType == HandPoseAttachType.Snap || this.m_grabbedHandPose.AttachType == HandPoseAttachType.SnapPosition);
			bool flag2 = this.m_grabbedHandPose != null && this.m_grabbedHandPose.AttachType == HandPoseAttachType.Snap;
			Transform grabTransform = this.m_grabbedGrabbable.GrabTransform;
			Quaternion quaternion = finalRotation * Quaternion.Inverse(base.transform.rotation);
			Vector3 position = (!flag) ? (finalPosition + quaternion * (grabTransform.position - base.transform.position)) : this.m_gripTransform.position;
			Quaternion rotation = (!flag2) ? (quaternion * grabTransform.rotation) : this.m_gripTransform.rotation;
			grabTransform.position = position;
			grabTransform.rotation = rotation;
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x0012AF1C File Offset: 0x0012931C
		private void GrabbableGrab(Grabbable grabbable, GrabPoint grabPoint)
		{
			this.m_grabbedGrabbable = grabbable;
			this.m_grabbedGrabbable.GrabBegin(this, grabPoint);
			this.m_grabbedHandPose = this.m_grabbedGrabbable.HandPose;
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x0012AF43 File Offset: 0x00129343
		private void GrabbableRelease(Vector3 linearVelocity, Vector3 angularVelocity)
		{
			this.m_grabbedGrabbable.GrabEnd(linearVelocity, angularVelocity);
			this.m_grabbedHandPose = null;
			this.m_grabbedGrabbable = null;
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x0012AF60 File Offset: 0x00129360
		private void GrabVolumeAdvance()
		{
			this.m_wasGrabVolumeEnabled = this.m_grabVolumeEnabled;
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x0012AF70 File Offset: 0x00129370
		private void GrabVolumeEnable(bool enabled)
		{
			if (this.m_grabVolumeEnabled == enabled)
			{
				return;
			}
			this.m_grabVolumeEnabled = enabled;
			foreach (Collider collider in this.m_grabVolumes)
			{
				collider.enabled = this.m_grabVolumeEnabled;
			}
			if (!this.m_grabVolumeEnabled)
			{
				foreach (Grabbable grabbable in this.m_grabCandidates.Keys)
				{
					grabbable.OverlapEnd(this);
				}
				this.m_grabCandidates.Clear();
			}
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x0012B028 File Offset: 0x00129428
		private void OffhandGrabbed(Grabbable grabbable)
		{
			if (this.m_grabbedGrabbable == grabbable)
			{
				this.GrabbableRelease(Vector3.zero, Vector3.zero);
			}
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x0012B04C File Offset: 0x0012944C
		private Hand.RegistrationTransform HandRegistration()
		{
			return (this.m_handedness != HandednessId.Left) ? Hand.Const.RegistrationRight : Hand.Const.RegistrationLeft;
		}

		// Token: 0x040023FC RID: 9212
		[SerializeField]
		private HandednessId m_handedness;

		// Token: 0x040023FD RID: 9213
		[SerializeField]
		private Transform m_gripTransform;

		// Token: 0x040023FE RID: 9214
		[SerializeField]
		private Animator m_animator;

		// Token: 0x040023FF RID: 9215
		[SerializeField]
		private Transform m_meshRoot;

		// Token: 0x04002400 RID: 9216
		[SerializeField]
		private HandParticles m_handParticlesPf;

		// Token: 0x04002401 RID: 9217
		[SerializeField]
		private Collider[] m_grabVolumes;

		// Token: 0x04002402 RID: 9218
		private TrackedController m_trackedController;

		// Token: 0x04002403 RID: 9219
		private VelocityTracker m_velocityTracker;

		// Token: 0x04002404 RID: 9220
		private Rigidbody m_rigidbody;

		// Token: 0x04002405 RID: 9221
		private Collider[] m_colliders;

		// Token: 0x04002406 RID: 9222
		private bool m_collisionEnabled = true;

		// Token: 0x04002407 RID: 9223
		private bool m_grabVolumeEnabled = true;

		// Token: 0x04002408 RID: 9224
		private bool m_wasGrabVolumeEnabled = true;

		// Token: 0x04002409 RID: 9225
		private int m_animLayerIndexThumb = -1;

		// Token: 0x0400240A RID: 9226
		private int m_animLayerIndexPoint = -1;

		// Token: 0x0400240B RID: 9227
		private int m_animParamIndexFlex = -1;

		// Token: 0x0400240C RID: 9228
		private int m_animParamIndexPose = -1;

		// Token: 0x0400240D RID: 9229
		private float m_flex;

		// Token: 0x0400240E RID: 9230
		private float m_point;

		// Token: 0x0400240F RID: 9231
		private float m_thumbsUp;

		// Token: 0x04002410 RID: 9232
		private HandPose m_grabbedHandPose;

		// Token: 0x04002411 RID: 9233
		private Grabbable m_grabbedGrabbable;

		// Token: 0x04002412 RID: 9234
		private Dictionary<Grabbable, int> m_grabCandidates = new Dictionary<Grabbable, int>();

		// Token: 0x0200071B RID: 1819
		private static class Const
		{
			// Token: 0x04002414 RID: 9236
			public const string AnimLayerNamePoint = "Point Layer";

			// Token: 0x04002415 RID: 9237
			public const string AnimLayerNameThumb = "Thumb Layer";

			// Token: 0x04002416 RID: 9238
			public const string AnimParamNameFlex = "Flex";

			// Token: 0x04002417 RID: 9239
			public const string AnimParamNamePose = "Pose";

			// Token: 0x04002418 RID: 9240
			public const float HapticOverlapAmplitude = 0.25f;

			// Token: 0x04002419 RID: 9241
			public const float HapticOverlapFrequency = 320f;

			// Token: 0x0400241A RID: 9242
			public const float HapticOverlapDuration = 0.05f;

			// Token: 0x0400241B RID: 9243
			public const float InputRateChange = 20f;

			// Token: 0x0400241C RID: 9244
			public static readonly Hand.RegistrationTransform RegistrationLeft = new Hand.RegistrationTransform(new Vector3(-0.02f, -0.04f, -0.03f), Quaternion.Euler(21.4075623f, -0.319427f, 77.7745361f));

			// Token: 0x0400241D RID: 9245
			public static readonly Hand.RegistrationTransform RegistrationRight = new Hand.RegistrationTransform(new Vector3(0.02f, -0.04f, -0.03f), Quaternion.Euler(21.4075623f, -0.319427f, -77.7745361f));

			// Token: 0x0400241E RID: 9246
			public const float ThreshCollisionFlex = 0.96f;

			// Token: 0x0400241F RID: 9247
			public const float ThreshGrabBegin = 0.55f;

			// Token: 0x04002420 RID: 9248
			public const float ThreshGrabEnd = 0.35f;

			// Token: 0x04002421 RID: 9249
			public const float ThreshThrowSpeed = 1f;
		}

		// Token: 0x0200071C RID: 1820
		private struct RegistrationTransform
		{
			// Token: 0x06003B65 RID: 15205 RVA: 0x0012B0F1 File Offset: 0x001294F1
			public RegistrationTransform(Vector3 translation, Quaternion rotation)
			{
				this.Translation = translation;
				this.Rotation = rotation;
			}

			// Token: 0x04002422 RID: 9250
			public Vector3 Translation;

			// Token: 0x04002423 RID: 9251
			public Quaternion Rotation;
		}
	}
}
