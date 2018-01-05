using System;
using System.Collections;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BCA RID: 3018
	public class VelocityEstimator : MonoBehaviour
	{
		// Token: 0x06005D69 RID: 23913 RVA: 0x002096E7 File Offset: 0x00207AE7
		public void BeginEstimatingVelocity()
		{
			this.FinishEstimatingVelocity();
			this.routine = base.StartCoroutine(this.EstimateVelocityCoroutine());
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x00209701 File Offset: 0x00207B01
		public void FinishEstimatingVelocity()
		{
			if (this.routine != null)
			{
				base.StopCoroutine(this.routine);
				this.routine = null;
			}
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x00209724 File Offset: 0x00207B24
		public Vector3 GetVelocityEstimate()
		{
			Vector3 vector = Vector3.zero;
			int num = Mathf.Min(this.sampleCount, this.velocitySamples.Length);
			if (num != 0)
			{
				for (int i = 0; i < num; i++)
				{
					vector += this.velocitySamples[i];
				}
				vector *= 1f / (float)num;
			}
			return vector;
		}

		// Token: 0x06005D6C RID: 23916 RVA: 0x0020978C File Offset: 0x00207B8C
		public Vector3 GetAngularVelocityEstimate()
		{
			Vector3 vector = Vector3.zero;
			int num = Mathf.Min(this.sampleCount, this.angularVelocitySamples.Length);
			if (num != 0)
			{
				for (int i = 0; i < num; i++)
				{
					vector += this.angularVelocitySamples[i];
				}
				vector *= 1f / (float)num;
			}
			return vector;
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x002097F4 File Offset: 0x00207BF4
		public Vector3 GetAccelerationEstimate()
		{
			Vector3 a = Vector3.zero;
			for (int i = 2 + this.sampleCount - this.velocitySamples.Length; i < this.sampleCount; i++)
			{
				if (i >= 2)
				{
					int num = i - 2;
					int num2 = i - 1;
					Vector3 b = this.velocitySamples[num % this.velocitySamples.Length];
					Vector3 a2 = this.velocitySamples[num2 % this.velocitySamples.Length];
					a += a2 - b;
				}
			}
			return a * (1f / Time.deltaTime);
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x0020989D File Offset: 0x00207C9D
		private void Awake()
		{
			this.velocitySamples = new Vector3[this.velocityAverageFrames];
			this.angularVelocitySamples = new Vector3[this.angularVelocityAverageFrames];
			if (this.estimateOnAwake)
			{
				this.BeginEstimatingVelocity();
			}
		}

		// Token: 0x06005D6F RID: 23919 RVA: 0x002098D4 File Offset: 0x00207CD4
		private IEnumerator EstimateVelocityCoroutine()
		{
			this.sampleCount = 0;
			Vector3 previousPosition = base.transform.position;
			Quaternion previousRotation = base.transform.rotation;
			for (;;)
			{
				yield return new WaitForEndOfFrame();
				float velocityFactor = 1f / Time.deltaTime;
				int v = this.sampleCount % this.velocitySamples.Length;
				int w = this.sampleCount % this.angularVelocitySamples.Length;
				this.sampleCount++;
				this.velocitySamples[v] = velocityFactor * (base.transform.position - previousPosition);
				Quaternion deltaRotation = base.transform.rotation * Quaternion.Inverse(previousRotation);
				float theta = 2f * Mathf.Acos(Mathf.Clamp(deltaRotation.w, -1f, 1f));
				if (theta > 3.14159274f)
				{
					theta -= 6.28318548f;
				}
				Vector3 angularVelocity = new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z);
				if (angularVelocity.sqrMagnitude > 0f)
				{
					angularVelocity = theta * velocityFactor * angularVelocity.normalized;
				}
				this.angularVelocitySamples[w] = angularVelocity;
				previousPosition = base.transform.position;
				previousRotation = base.transform.rotation;
			}
			yield break;
		}

		// Token: 0x040042D8 RID: 17112
		[Tooltip("How many frames to average over for computing velocity")]
		public int velocityAverageFrames = 5;

		// Token: 0x040042D9 RID: 17113
		[Tooltip("How many frames to average over for computing angular velocity")]
		public int angularVelocityAverageFrames = 11;

		// Token: 0x040042DA RID: 17114
		public bool estimateOnAwake;

		// Token: 0x040042DB RID: 17115
		private Coroutine routine;

		// Token: 0x040042DC RID: 17116
		private int sampleCount;

		// Token: 0x040042DD RID: 17117
		private Vector3[] velocitySamples;

		// Token: 0x040042DE RID: 17118
		private Vector3[] angularVelocitySamples;
	}
}
