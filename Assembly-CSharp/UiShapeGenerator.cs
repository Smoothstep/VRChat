using System;
using System.Collections.Generic;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000C4B RID: 3147
public class UiShapeGenerator : VRC_UiShape
{
	// Token: 0x06006193 RID: 24979 RVA: 0x00226E54 File Offset: 0x00225254
	private void Start()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		MeshRenderer component2 = base.GetComponent<MeshRenderer>();
		this.resolution = new Vector2((float)component2.material.mainTexture.width, (float)component2.material.mainTexture.height);
		component2.material.renderQueue = this.renderQueue;
		List<Vector3> list = new List<Vector3>();
		List<Vector2> list2 = new List<Vector2>();
		List<int> list3 = new List<int>();
		for (int i = 0; i <= this.xDivs; i++)
		{
			float num = (float)i / (float)this.xDivs;
			float f = num * this.angleCoverage - this.angleCoverage / 2f;
			float x = this.radius * Mathf.Sin(f);
			float z = this.radius * Mathf.Cos(f);
			for (int j = 0; j <= this.yDivs; j++)
			{
				float num2 = (float)j / (float)this.yDivs;
				float y = num2 - 0.5f;
				list.Add(new Vector3(x, y, z));
				list2.Add(new Vector2(num, num2));
				if (i != 0 && j != 0)
				{
					list3.Add(j + (this.yDivs + 1) * i);
					list3.Add(j - 1 + (this.yDivs + 1) * i);
					list3.Add(j + (this.yDivs + 1) * (i - 1));
					list3.Add(j - 1 + (this.yDivs + 1) * i);
					list3.Add(j - 1 + (this.yDivs + 1) * (i - 1));
					list3.Add(j + (this.yDivs + 1) * (i - 1));
				}
			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = list.ToArray();
		mesh.uv = list2.ToArray();
		mesh.triangles = list3.ToArray();
		component.mesh = mesh;
		MeshCollider component3 = base.GetComponent<MeshCollider>();
		if (component3 != null)
		{
			component3.sharedMesh = mesh;
		}
	}

	// Token: 0x06006194 RID: 24980 RVA: 0x0022705F File Offset: 0x0022545F
	public Vector2 GetPointerPosition(Vector2 v)
	{
		return new Vector2(v.x * this.resolution.x, v.y * this.resolution.y);
	}

	// Token: 0x0400472A RID: 18218
	public int xDivs = 30;

	// Token: 0x0400472B RID: 18219
	public int yDivs = 5;

	// Token: 0x0400472C RID: 18220
	public float angleCoverage = 180f;

	// Token: 0x0400472D RID: 18221
	public float radius = 1f;

	// Token: 0x0400472E RID: 18222
	public int renderQueue = 3998;

	// Token: 0x0400472F RID: 18223
	public Vector2 resolution;
}
