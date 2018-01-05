using System;
using UnityEngine;

// Token: 0x0200056A RID: 1386
public class IgnoreRaycast : MonoBehaviour, ICanvasRaycastFilter
{
	// Token: 0x06002F5C RID: 12124 RVA: 0x000E616D File Offset: 0x000E456D
	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		return false;
	}
}
