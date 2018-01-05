using System;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000845 RID: 2117
	public class ArcGizmo : MonoBehaviour
	{
		// Token: 0x060041B7 RID: 16823 RVA: 0x0014E5C4 File Offset: 0x0014C9C4
		public static ArcGizmo Create()
		{
			GameObject gameObject = new GameObject("ArcGizmoPro");
			return gameObject.AddComponent<ArcGizmo>();
		}
	}
}
