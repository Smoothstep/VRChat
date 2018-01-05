using System;
using UnityEngine;
using UnityEngine.Events;

namespace Klak.Wiring
{
	// Token: 0x0200055A RID: 1370
	public class NodeBase : MonoBehaviour
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002F0E RID: 12046 RVA: 0x000E30D4 File Offset: 0x000E14D4
		public static Vector2 uninitializedNodePosition
		{
			get
			{
				return new Vector2(-1000f, -1000f);
			}
		}

		// Token: 0x0400195E RID: 6494
		[SerializeField]
		[HideInInspector]
		private Vector2 _wiringNodePosition = NodeBase.uninitializedNodePosition;

		// Token: 0x0200055B RID: 1371
		[Serializable]
		public class VoidEvent : UnityEvent
		{
		}

		// Token: 0x0200055C RID: 1372
		[Serializable]
		public class FloatEvent : UnityEvent<float>
		{
		}

		// Token: 0x0200055D RID: 1373
		[Serializable]
		public class Vector3Event : UnityEvent<Vector3>
		{
		}

		// Token: 0x0200055E RID: 1374
		[Serializable]
		public class QuaternionEvent : UnityEvent<Quaternion>
		{
		}

		// Token: 0x0200055F RID: 1375
		[Serializable]
		public class ColorEvent : UnityEvent<Color>
		{
		}
	}
}
