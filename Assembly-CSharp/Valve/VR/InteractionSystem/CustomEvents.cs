using System;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000B99 RID: 2969
	public static class CustomEvents
	{
		// Token: 0x02000B9A RID: 2970
		[Serializable]
		public class UnityEventSingleFloat : UnityEvent<float>
		{
		}

		// Token: 0x02000B9B RID: 2971
		[Serializable]
		public class UnityEventHand : UnityEvent<Hand>
		{
		}
	}
}
