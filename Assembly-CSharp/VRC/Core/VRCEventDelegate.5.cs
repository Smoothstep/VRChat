using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRC.Core
{
	// Token: 0x02000A7E RID: 2686
	public class VRCEventDelegate<T0, T1, T2, T3>
	{
		// Token: 0x060050EE RID: 20718 RVA: 0x001BA6CB File Offset: 0x001B8ACB
		public void AddListener(UnityAction<T0, T1, T2, T3> handler)
		{
			if (!this._handlers.Add(handler))
			{
				Debug.LogError("VRCEventDelegate.AddListener: handler already registered" + handler.ToString());
			}
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x001BA6F3 File Offset: 0x001B8AF3
		public void RemoveListener(UnityAction<T0, T1, T2, T3> handler)
		{
			if (!this._handlers.Remove(handler))
			{
			}
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x001BA708 File Offset: 0x001B8B08
		public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			foreach (UnityAction<T0, T1, T2, T3> unityAction in this._handlers)
			{
				try
				{
					unityAction(arg0, arg1, arg2, arg3);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x04003964 RID: 14692
		private HashSet<UnityAction<T0, T1, T2, T3>> _handlers = new HashSet<UnityAction<T0, T1, T2, T3>>();
	}
}
