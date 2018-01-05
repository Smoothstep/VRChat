using System;
using UnityEngine;

// Token: 0x02000AA1 RID: 2721
public class UIRaycast : MonoBehaviour
{
	// Token: 0x060051CA RID: 20938 RVA: 0x001C0586 File Offset: 0x001BE986
	private void Start()
	{
		Debug.LogError("SHouldn't be part of project");
	}

	// Token: 0x060051CB RID: 20939 RVA: 0x001C0594 File Offset: 0x001BE994
	public Vector3 RenderTextureRaycastConversion(Vector3 screenPoint)
	{
		Ray ray = this.ingameCamera.ScreenPointToRay(screenPoint);
		Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
		Plane plane = new Plane(this.quad.transform.forward, this.quad.transform.position);
		float d;
		if (!plane.Raycast(ray, out d))
		{
			return new Vector3(float.NaN, float.NaN, float.NaN);
		}
		Vector3 vector = ray.origin + ray.direction * d;
		Debug.DrawLine(Vector3.zero, vector, Color.red);
		Vector3 a = this.quad.transform.InverseTransformPoint(vector);
		return a + new Vector3(0.5f, 0.5f, 0f);
	}

	// Token: 0x04003A12 RID: 14866
	public Transform quad;

	// Token: 0x04003A13 RID: 14867
	public Camera ingameCamera;
}
