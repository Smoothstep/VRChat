using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRC.Core
{
	// Token: 0x02000A7B RID: 2683
	public class VRCEventDelegate<T>
	{
		// Token: 0x060050E2 RID: 20706 RVA: 0x001BA467 File Offset: 0x001B8867
		public void AddListener(UnityAction<T> handler)
		{
			if (!this._handlers.Add(handler))
			{
				Debug.LogError("VRCEventDelegate.AddListener: handler already registered" + handler.ToString());
			}
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x001BA48F File Offset: 0x001B888F
		public void RemoveListener(UnityAction<T> handler)
		{
			if (!this._handlers.Remove(handler))
			{
			}
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x001BA4A4 File Offset: 0x001B88A4
		public void Invoke(T arg)
		{
			foreach (UnityAction<T> unityAction in this._handlers)
			{
				try
				{
					unityAction(arg);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x04003961 RID: 14689
		private HashSet<UnityAction<T>> _handlers = new HashSet<UnityAction<T>>();
	}
}
