using System;
using UnityEngine;

// Token: 0x02000A71 RID: 2673
public class DebugGizmo : MonoBehaviour
{
	// Token: 0x060050BC RID: 20668 RVA: 0x001B97DA File Offset: 0x001B7BDA
	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(base.transform.TransformPoint(Vector3.zero), 0.01f);
	}
}
