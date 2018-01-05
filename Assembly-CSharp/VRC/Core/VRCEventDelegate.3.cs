using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRC.Core
{
	// Token: 0x02000A7C RID: 2684
	public class VRCEventDelegate<T0, T1>
	{
		// Token: 0x060050E6 RID: 20710 RVA: 0x001BA533 File Offset: 0x001B8933
		public void AddListener(UnityAction<T0, T1> handler)
		{
			if (!this._handlers.Add(handler))
			{
				Debug.LogError("VRCEventDelegate.AddListener: handler already registered" + handler.ToString());
			}
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x001BA55B File Offset: 0x001B895B
		public void RemoveListener(UnityAction<T0, T1> handler)
		{
			if (!this._handlers.Remove(handler))
			{
			}
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x001BA570 File Offset: 0x001B8970
		public void Invoke(T0 arg0, T1 arg1)
		{
			foreach (UnityAction<T0, T1> unityAction in this._handlers)
			{
				try
				{
					unityAction(arg0, arg1);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x04003962 RID: 14690
		private HashSet<UnityAction<T0, T1>> _handlers = new HashSet<UnityAction<T0, T1>>();
	}
}
