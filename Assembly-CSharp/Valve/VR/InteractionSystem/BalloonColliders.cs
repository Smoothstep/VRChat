using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BD4 RID: 3028
	public class BalloonColliders : MonoBehaviour
	{
		// Token: 0x06005DB9 RID: 23993 RVA: 0x0020CAC0 File Offset: 0x0020AEC0
		private void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.colliderLocalPositions = new Vector3[this.colliders.Length];
			this.colliderLocalRotations = new Quaternion[this.colliders.Length];
			for (int i = 0; i < this.colliders.Length; i++)
			{
				this.colliderLocalPositions[i] = this.colliders[i].transform.localPosition;
				this.colliderLocalRotations[i] = this.colliders[i].transform.localRotation;
				this.colliders[i].name = base.gameObject.name + "." + this.colliders[i].name;
			}
		}

		// Token: 0x06005DBA RID: 23994 RVA: 0x0020CB90 File Offset: 0x0020AF90
		private void OnEnable()
		{
			for (int i = 0; i < this.colliders.Length; i++)
			{
				this.colliders[i].transform.SetParent(base.transform);
				this.colliders[i].transform.localPosition = this.colliderLocalPositions[i];
				this.colliders[i].transform.localRotation = this.colliderLocalRotations[i];
				this.colliders[i].transform.SetParent(null);
				FixedJoint fixedJoint = this.colliders[i].AddComponent<FixedJoint>();
				fixedJoint.connectedBody = this.rb;
				fixedJoint.breakForce = float.PositiveInfinity;
				fixedJoint.breakTorque = float.PositiveInfinity;
				fixedJoint.enableCollision = false;
				fixedJoint.enablePreprocessing = true;
				this.colliders[i].SetActive(true);
			}
		}

		// Token: 0x06005DBB RID: 23995 RVA: 0x0020CC74 File Offset: 0x0020B074
		private void OnDisable()
		{
			for (int i = 0; i < this.colliders.Length; i++)
			{
				if (this.colliders[i] != null)
				{
					UnityEngine.Object.Destroy(this.colliders[i].GetComponent<FixedJoint>());
					this.colliders[i].SetActive(false);
				}
			}
		}

		// Token: 0x06005DBC RID: 23996 RVA: 0x0020CCD0 File Offset: 0x0020B0D0
		private void OnDestroy()
		{
			for (int i = 0; i < this.colliders.Length; i++)
			{
				UnityEngine.Object.Destroy(this.colliders[i]);
			}
		}

		// Token: 0x0400434C RID: 17228
		public GameObject[] colliders;

		// Token: 0x0400434D RID: 17229
		private Vector3[] colliderLocalPositions;

		// Token: 0x0400434E RID: 17230
		private Quaternion[] colliderLocalRotations;

		// Token: 0x0400434F RID: 17231
		private Rigidbody rb;
	}
}
