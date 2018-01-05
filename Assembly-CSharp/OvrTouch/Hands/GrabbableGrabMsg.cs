using System;

namespace OvrTouch.Hands
{
	// Token: 0x02000717 RID: 1815
	public struct GrabbableGrabMsg
	{
		// Token: 0x040023F0 RID: 9200
		public const string MsgNameGrabBegin = "OnGrabBegin";

		// Token: 0x040023F1 RID: 9201
		public const string MsgNameGrabEnd = "OnGrabEnd";

		// Token: 0x040023F2 RID: 9202
		public Grabbable Sender;
	}
}
