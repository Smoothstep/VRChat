using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRC.Core
{
	// Token: 0x02000A7D RID: 2685
	public class VRCEventDelegate<T0, T1, T2>
	{
		// Token: 0x060050EA RID: 20714 RVA: 0x001BA5FF File Offset: 0x001B89FF
		public void AddListener(UnityAction<T0, T1, T2> handler)
		{
			if (!this._handlers.Add(handler))
			{
				Debug.LogError("VRCEventDelegate.RemoveListener: handler not registered " + handler.ToString());
			}
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x001BA627 File Offset: 0x001B8A27
		public void RemoveListener(UnityAction<T0, T1, T2> handler)
		{
			if (!this._handlers.Remove(handler))
			{
			}
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x001BA63C File Offset: 0x001B8A3C
		public void Invoke(T0 arg0, T1 arg1, T2 arg2)
		{
			foreach (UnityAction<T0, T1, T2> unityAction in this._handlers)
			{
				try
				{
					unityAction(arg0, arg1, arg2);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x04003963 RID: 14691
		private HashSet<UnityAction<T0, T1, T2>> _handlers = new HashSet<UnityAction<T0, T1, T2>>();
	}
}
