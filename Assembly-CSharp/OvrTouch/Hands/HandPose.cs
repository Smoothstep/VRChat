using System;
using UnityEngine;

namespace OvrTouch.Hands
{
	// Token: 0x02000720 RID: 1824
	public class HandPose : MonoBehaviour
	{
		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06003B6B RID: 15211 RVA: 0x0012B245 File Offset: 0x00129645
		public bool AllowPointing
		{
			get
			{
				return this.m_allowPointing;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06003B6C RID: 15212 RVA: 0x0012B24D File Offset: 0x0012964D
		public bool AllowThumbsUp
		{
			get
			{
				return this.m_allowThumbsUp;
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06003B6D RID: 15213 RVA: 0x0012B255 File Offset: 0x00129655
		public HandPoseId PoseId
		{
			get
			{
				return this.m_poseId;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06003B6E RID: 15214 RVA: 0x0012B25D File Offset: 0x0012965D
		public HandPoseAttachType AttachType
		{
			get
			{
				return this.m_attachType;
			}
		}

		// Token: 0x04002431 RID: 9265
		[SerializeField]
		private bool m_allowPointing;

		// Token: 0x04002432 RID: 9266
		[SerializeField]
		private bool m_allowThumbsUp;

		// Token: 0x04002433 RID: 9267
		[SerializeField]
		private HandPoseId m_poseId;

		// Token: 0x04002434 RID: 9268
		[SerializeField]
		private HandPoseAttachType m_attachType;
	}
}
