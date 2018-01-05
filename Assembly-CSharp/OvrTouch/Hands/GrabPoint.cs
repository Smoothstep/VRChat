using System;
using UnityEngine;

namespace OvrTouch.Hands
{
	// Token: 0x02000715 RID: 1813
	[Serializable]
	public class GrabPoint
	{
		// Token: 0x06003B2F RID: 15151 RVA: 0x0012A322 File Offset: 0x00128722
		public GrabPoint(Collider collider)
		{
			this.m_grabCollider = collider;
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003B30 RID: 15152 RVA: 0x0012A331 File Offset: 0x00128731
		public HandPose HandPose
		{
			get
			{
				return this.m_handPose;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003B31 RID: 15153 RVA: 0x0012A339 File Offset: 0x00128739
		public Collider GrabCollider
		{
			get
			{
				return this.m_grabCollider;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003B32 RID: 15154 RVA: 0x0012A341 File Offset: 0x00128741
		public Transform GrabTransform
		{
			get
			{
				return this.m_grabTransform;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003B33 RID: 15155 RVA: 0x0012A349 File Offset: 0x00128749
		public Rigidbody Rigidbody
		{
			get
			{
				return this.m_rigidbody;
			}
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x0012A354 File Offset: 0x00128754
		public void Initialize()
		{
			if (this.m_grabCollider == null)
			{
				throw new ArgumentException("GrabPoint: Grab points require a grab collider -- please set a collider.");
			}
			this.m_grabTransform = ((!(this.m_grabOverride != null)) ? this.m_grabCollider.transform : this.m_grabOverride);
			this.m_rigidbody = this.m_grabCollider.attachedRigidbody;
		}

		// Token: 0x040023EA RID: 9194
		[SerializeField]
		private HandPose m_handPose;

		// Token: 0x040023EB RID: 9195
		[SerializeField]
		private Collider m_grabCollider;

		// Token: 0x040023EC RID: 9196
		[SerializeField]
		private Transform m_grabOverride;

		// Token: 0x040023ED RID: 9197
		private Transform m_grabTransform;

		// Token: 0x040023EE RID: 9198
		private Rigidbody m_rigidbody;
	}
}
