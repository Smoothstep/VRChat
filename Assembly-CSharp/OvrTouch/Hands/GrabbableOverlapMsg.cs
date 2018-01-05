using System;

namespace OvrTouch.Hands
{
	// Token: 0x02000718 RID: 1816
	public struct GrabbableOverlapMsg
	{
		// Token: 0x040023F3 RID: 9203
		public const string MsgNameOverlapBegin = "OnOverlapBegin";

		// Token: 0x040023F4 RID: 9204
		public const string MsgNameOverlapEnd = "OnOverlapEnd";

		// Token: 0x040023F5 RID: 9205
		public Grabbable Sender;

		// Token: 0x040023F6 RID: 9206
		public Hand Hand;
	}
}
