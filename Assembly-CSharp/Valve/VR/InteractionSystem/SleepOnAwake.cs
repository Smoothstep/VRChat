using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BBF RID: 3007
	public class SleepOnAwake : MonoBehaviour
	{
		// Token: 0x06005D07 RID: 23815 RVA: 0x00207D04 File Offset: 0x00206104
		private void Awake()
		{
			Rigidbody component = base.GetComponent<Rigidbody>();
			if (component)
			{
				component.Sleep();
			}
		}
	}
}
