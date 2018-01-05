using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200084E RID: 2126
	public class Cylinder : BaseObject
	{
		// Token: 0x06004201 RID: 16897 RVA: 0x0014F99C File Offset: 0x0014DD9C
		public static Cylinder Create(float radius, float height, int sides, int heightSegments, NormalsType normals, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("CylinderPro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Cylinder cylinder = gameObject.AddComponent<Cylinder>();
			cylinder.GenerateGeometry(radius, height, sides, heightSegments, normals, pivotPosition);
			return cylinder;
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x0014F9F0 File Offset: 0x0014DDF0
		public void GenerateGeometry(float radius, float height, int sides, int heightSegments, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = ConePrimitive.GenerateGeometry(sharedMesh, radius, radius, height, sides, heightSegments, normalsType, pivotPosition);
			this.radius = radius;
			this.height = height;
			this.sides = sides;
			this.heightSegments = heightSegments;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x0014FA71 File Offset: 0x0014DE71
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius, this.height, this.sides, this.heightSegments, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x0014FAA4 File Offset: 0x0014DEA4
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				ConePrimitive.GenerateGeometry(colliderMesh, this.radius, this.radius, this.height, this.sides, this.heightSegments, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x0014FB08 File Offset: 0x0014DF08
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius"] = this.radius;
			dictionary["height"] = this.height;
			dictionary["sides"] = this.sides;
			dictionary["heightSegments"] = this.heightSegments;
			return dictionary;
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x0014FB78 File Offset: 0x0014DF78
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius = (float)dictionary["radius"];
			this.height = (float)dictionary["height"];
			this.sides = (int)dictionary["sides"];
			this.heightSegments = (int)dictionary["heightSegments"];
			return dictionary;
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x0014FBE6 File Offset: 0x0014DFE6
		public override void SetHeight(float height)
		{
			this.height = height;
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x0014FBF0 File Offset: 0x0014DFF0
		public override void SetWidth(float width0, float length0)
		{
			float num = width0 * 0.5f;
			float num2 = length0 * 0.5f;
			if (Mathf.Abs(num - this.radius) > Mathf.Abs(num2 - this.radius))
			{
				this.radius = num;
			}
			else
			{
				this.radius = num2;
			}
		}

		// Token: 0x04002AED RID: 10989
		public float radius;

		// Token: 0x04002AEE RID: 10990
		public float height;

		// Token: 0x04002AEF RID: 10991
		public int sides;

		// Token: 0x04002AF0 RID: 10992
		public int heightSegments;
	}
}
