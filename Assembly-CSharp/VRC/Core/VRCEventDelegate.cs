using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRC.Core
{
	// Token: 0x02000A7A RID: 2682
	public class VRCEventDelegate
	{
		// Token: 0x060050DE RID: 20702 RVA: 0x001BA3A0 File Offset: 0x001B87A0
		public void AddListener(UnityAction handler)
		{
			if (!this._handlers.Add(handler))
			{
				Debug.LogError("VRCEventDelegate.AddListener: handler already registered" + handler.ToString());
			}
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x001BA3C8 File Offset: 0x001B87C8
		public void RemoveListener(UnityAction handler)
		{
			if (!this._handlers.Remove(handler))
			{
			}
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x001BA3DC File Offset: 0x001B87DC
		public void Invoke()
		{
			foreach (UnityAction unityAction in this._handlers)
			{
				try
				{
					unityAction();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x04003960 RID: 14688
		private HashSet<UnityAction> _handlers = new HashSet<UnityAction>();
	}
}
