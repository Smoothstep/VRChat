using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BBA RID: 3002
	[RequireComponent(typeof(Interactable))]
	public class LinearDrive : MonoBehaviour
	{
		// Token: 0x06005CDC RID: 23772 RVA: 0x00206BCA File Offset: 0x00204FCA
		private void Awake()
		{
			this.mappingChangeSamples = new float[this.numMappingChangeSamples];
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x00206BE0 File Offset: 0x00204FE0
		private void Start()
		{
			if (this.linearMapping == null)
			{
				this.linearMapping = base.GetComponent<LinearMapping>();
			}
			if (this.linearMapping == null)
			{
				this.linearMapping = base.gameObject.AddComponent<LinearMapping>();
			}
			this.initialMappingOffset = this.linearMapping.value;
			if (this.repositionGameObject)
			{
				this.UpdateLinearMapping(base.transform);
			}
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x00206C54 File Offset: 0x00205054
		private void HandHoverUpdate(Hand hand)
		{
			if (hand.GetStandardInteractionButtonDown())
			{
				hand.HoverLock(base.GetComponent<Interactable>());
				this.initialMappingOffset = this.linearMapping.value - this.CalculateLinearMapping(hand.transform);
				this.sampleCount = 0;
				this.mappingChangeRate = 0f;
			}
			if (hand.GetStandardInteractionButtonUp())
			{
				hand.HoverUnlock(base.GetComponent<Interactable>());
				this.CalculateMappingChangeRate();
			}
			if (hand.GetStandardInteractionButton())
			{
				this.UpdateLinearMapping(hand.transform);
			}
		}

		// Token: 0x06005CDF RID: 23775 RVA: 0x00206CDC File Offset: 0x002050DC
		private void CalculateMappingChangeRate()
		{
			this.mappingChangeRate = 0f;
			int num = Mathf.Min(this.sampleCount, this.mappingChangeSamples.Length);
			if (num != 0)
			{
				for (int i = 0; i < num; i++)
				{
					this.mappingChangeRate += this.mappingChangeSamples[i];
				}
				this.mappingChangeRate /= (float)num;
			}
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x00206D44 File Offset: 0x00205144
		private void UpdateLinearMapping(Transform tr)
		{
			this.prevMapping = this.linearMapping.value;
			this.linearMapping.value = Mathf.Clamp01(this.initialMappingOffset + this.CalculateLinearMapping(tr));
			this.mappingChangeSamples[this.sampleCount % this.mappingChangeSamples.Length] = 1f / Time.deltaTime * (this.linearMapping.value - this.prevMapping);
			this.sampleCount++;
			if (this.repositionGameObject)
			{
				base.transform.position = Vector3.Lerp(this.startPosition.position, this.endPosition.position, this.linearMapping.value);
			}
		}

		// Token: 0x06005CE1 RID: 23777 RVA: 0x00206E00 File Offset: 0x00205200
		private float CalculateLinearMapping(Transform tr)
		{
			Vector3 rhs = this.endPosition.position - this.startPosition.position;
			float magnitude = rhs.magnitude;
			rhs.Normalize();
			Vector3 lhs = tr.position - this.startPosition.position;
			return Vector3.Dot(lhs, rhs) / magnitude;
		}

		// Token: 0x06005CE2 RID: 23778 RVA: 0x00206E58 File Offset: 0x00205258
		private void Update()
		{
			if (this.maintainMomemntum && this.mappingChangeRate != 0f)
			{
				this.mappingChangeRate = Mathf.Lerp(this.mappingChangeRate, 0f, this.momemtumDampenRate * Time.deltaTime);
				this.linearMapping.value = Mathf.Clamp01(this.linearMapping.value + this.mappingChangeRate * Time.deltaTime);
				if (this.repositionGameObject)
				{
					base.transform.position = Vector3.Lerp(this.startPosition.position, this.endPosition.position, this.linearMapping.value);
				}
			}
		}

		// Token: 0x04004271 RID: 17009
		public Transform startPosition;

		// Token: 0x04004272 RID: 17010
		public Transform endPosition;

		// Token: 0x04004273 RID: 17011
		public LinearMapping linearMapping;

		// Token: 0x04004274 RID: 17012
		public bool repositionGameObject = true;

		// Token: 0x04004275 RID: 17013
		public bool maintainMomemntum = true;

		// Token: 0x04004276 RID: 17014
		public float momemtumDampenRate = 5f;

		// Token: 0x04004277 RID: 17015
		private float initialMappingOffset;

		// Token: 0x04004278 RID: 17016
		private int numMappingChangeSamples = 5;

		// Token: 0x04004279 RID: 17017
		private float[] mappingChangeSamples;

		// Token: 0x0400427A RID: 17018
		private float prevMapping;

		// Token: 0x0400427B RID: 17019
		private float mappingChangeRate;

		// Token: 0x0400427C RID: 17020
		private int sampleCount;
	}
}
