using System;
using UnityEngine;

namespace OvrTouch.Hands
{
	// Token: 0x02000721 RID: 1825
	public class VelocityTracker : MonoBehaviour
	{
		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06003B70 RID: 15216 RVA: 0x0012B2D8 File Offset: 0x001296D8
		public Vector3 FrameAngularVelocity
		{
			get
			{
				return this.m_frameAngularVelocity;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06003B71 RID: 15217 RVA: 0x0012B2E0 File Offset: 0x001296E0
		public Vector3 FrameLinearVelocity
		{
			get
			{
				return this.m_frameLinearVelocity;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06003B72 RID: 15218 RVA: 0x0012B2E8 File Offset: 0x001296E8
		public Vector3 TrackedAngularVelocity
		{
			get
			{
				return this.m_trackedAngularVelocity;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06003B73 RID: 15219 RVA: 0x0012B2F0 File Offset: 0x001296F0
		public Vector3 TrackedLinearVelocity
		{
			get
			{
				return this.m_trackedLinearVelocity;
			}
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x0012B2F8 File Offset: 0x001296F8
		private void Awake()
		{
			this.m_position = base.transform.position;
			this.m_rotation = base.transform.rotation;
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0012B31C File Offset: 0x0012971C
		private void FixedUpdate()
		{
			Vector3 position = base.transform.position;
			Vector3 deltaPosition = position - this.m_position;
			this.m_position = position;
			Quaternion rotation = base.transform.rotation;
			Vector3 deltaRotation = this.DeltaRotation(rotation, this.m_rotation) * 0.0174532924f;
			this.m_rotation = rotation;
			this.AddSample(deltaPosition, deltaRotation);
			this.m_frameLinearVelocity = this.m_samples[this.m_index].LinearVelocity;
			this.m_frameAngularVelocity = this.m_samples[this.m_index].AngularVelocity;
			this.m_trackedLinearVelocity = this.ComputeAverageLinearVelocity().normalized * this.ComputeMaxLinearSpeed();
			this.m_trackedAngularVelocity = this.ComputeAverageAngularVelocity();
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0012B3E0 File Offset: 0x001297E0
		private void OnDrawGizmos()
		{
			if (!this.m_showGizmos)
			{
				return;
			}
			Gizmos.color = Color.red;
			Gizmos.DrawRay(base.transform.position, this.TrackedLinearVelocity);
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0012B410 File Offset: 0x00129810
		private Vector3 DeltaRotation(Quaternion final, Quaternion initial)
		{
			Vector3 eulerAngles = final.eulerAngles;
			Vector3 eulerAngles2 = initial.eulerAngles;
			Vector3 result = new Vector3(Mathf.DeltaAngle(eulerAngles2.x, eulerAngles.x), Mathf.DeltaAngle(eulerAngles2.y, eulerAngles.y), Mathf.DeltaAngle(eulerAngles2.z, eulerAngles.z));
			return result;
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0012B470 File Offset: 0x00129870
		private void AddSample(Vector3 deltaPosition, Vector3 deltaRotation)
		{
			this.m_index = (this.m_index + 1) % this.m_samples.Length;
			this.m_count = Mathf.Min(this.m_count + 1, this.m_samples.Length);
			float time = Time.time;
			Vector3 linearVelocity = deltaPosition / Time.deltaTime;
			Vector3 angularVelocity = deltaRotation / Time.deltaTime;
			this.m_samples[this.m_index] = new VelocityTracker.Sample
			{
				Time = time,
				LinearVelocity = linearVelocity,
				AngularVelocity = angularVelocity
			};
			this.m_samples[this.m_index].LinearSpeed = this.ComputeAverageLinearVelocity().magnitude;
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x0012B529 File Offset: 0x00129929
		private int Count()
		{
			return Mathf.Min(this.m_count, this.m_samples.Length);
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0012B53E File Offset: 0x0012993E
		private int IndexPrev(int index)
		{
			return (index != 0) ? (index - 1) : (this.m_count - 1);
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x0012B558 File Offset: 0x00129958
		private bool IsSampleValid(int index, float windowSize)
		{
			float num = Time.time - this.m_samples[index].Time;
			return windowSize - num >= 0.0001f || index == this.m_index;
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x0012B598 File Offset: 0x00129998
		private Vector3 ComputeAverageAngularVelocity()
		{
			int num = this.m_index;
			int num2 = this.Count();
			int num3 = 0;
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < num2; i++)
			{
				if (!this.IsSampleValid(num, 0.0222222228f))
				{
					break;
				}
				num3++;
				vector += this.m_samples[num].AngularVelocity;
				num = this.IndexPrev(num);
			}
			if (num3 > 1)
			{
				vector /= (float)num3;
			}
			return vector;
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x0012B61C File Offset: 0x00129A1C
		private Vector3 ComputeAverageLinearVelocity()
		{
			int num = this.m_index;
			int num2 = this.Count();
			int num3 = 0;
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < num2; i++)
			{
				if (!this.IsSampleValid(num, 0.0444444455f))
				{
					break;
				}
				num3++;
				vector += this.m_samples[num].LinearVelocity;
				num = this.IndexPrev(num);
			}
			if (num3 > 1)
			{
				vector /= (float)num3;
			}
			return vector;
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0012B6A0 File Offset: 0x00129AA0
		private float ComputeMaxLinearSpeed()
		{
			int num = this.m_index;
			int num2 = this.Count();
			float num3 = 0f;
			for (int i = 0; i < num2; i++)
			{
				if (!this.IsSampleValid(num, 0.08888889f))
				{
					break;
				}
				num3 = Mathf.Max(num3, this.m_samples[num].LinearSpeed);
				num = this.IndexPrev(num);
			}
			return num3;
		}

		// Token: 0x04002435 RID: 9269
		[SerializeField]
		private bool m_showGizmos = true;

		// Token: 0x04002436 RID: 9270
		private int m_index = -1;

		// Token: 0x04002437 RID: 9271
		private int m_count;

		// Token: 0x04002438 RID: 9272
		private VelocityTracker.Sample[] m_samples = new VelocityTracker.Sample[45];

		// Token: 0x04002439 RID: 9273
		private Vector3 m_position = Vector3.zero;

		// Token: 0x0400243A RID: 9274
		private Quaternion m_rotation = Quaternion.identity;

		// Token: 0x0400243B RID: 9275
		private Vector3 m_frameLinearVelocity = Vector3.zero;

		// Token: 0x0400243C RID: 9276
		private Vector3 m_frameAngularVelocity = Vector3.zero;

		// Token: 0x0400243D RID: 9277
		private Vector3 m_trackedLinearVelocity = Vector3.zero;

		// Token: 0x0400243E RID: 9278
		private Vector3 m_trackedAngularVelocity = Vector3.zero;

		// Token: 0x02000722 RID: 1826
		private static class Const
		{
			// Token: 0x0400243F RID: 9279
			public const float WindowTime = 0.0111111114f;

			// Token: 0x04002440 RID: 9280
			public const float WindowEpsilon = 0.0001f;

			// Token: 0x04002441 RID: 9281
			public const float LinearSpeedWindow = 0.08888889f;

			// Token: 0x04002442 RID: 9282
			public const float LinearVelocityWindow = 0.0444444455f;

			// Token: 0x04002443 RID: 9283
			public const float AngularVelocityWindow = 0.0222222228f;

			// Token: 0x04002444 RID: 9284
			public const int MaxSamples = 45;
		}

		// Token: 0x02000723 RID: 1827
		private struct Sample
		{
			// Token: 0x04002445 RID: 9285
			public float Time;

			// Token: 0x04002446 RID: 9286
			public float LinearSpeed;

			// Token: 0x04002447 RID: 9287
			public Vector3 LinearVelocity;

			// Token: 0x04002448 RID: 9288
			public Vector3 AngularVelocity;
		}
	}
}
