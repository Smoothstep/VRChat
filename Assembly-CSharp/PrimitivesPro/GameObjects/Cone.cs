using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200084C RID: 2124
	public class Cone : BaseObject
	{
		// Token: 0x060041F0 RID: 16880 RVA: 0x0014F3B4 File Offset: 0x0014D7B4
		public static Cone Create(float radius0, float radius1, float thickness, float height, int sides, int heightSegments, NormalsType normalsType, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("ConePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Cone cone = gameObject.AddComponent<Cone>();
			cone.GenerateGeometry(radius0, radius1, thickness, height, sides, heightSegments, normalsType, pivotPosition);
			return cone;
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x0014F40C File Offset: 0x0014D80C
		public void GenerateGeometry(float radius0, float radius1, float thickness, float height, int sides, int heightSegments, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			if (thickness >= 0f)
			{
				base.GenerationTimeMS = HollowConePrimitive.GenerateGeometry(sharedMesh, radius0, radius1, thickness, height, sides, heightSegments, normalsType, pivotPosition);
			}
			else
			{
				base.GenerationTimeMS = ConePrimitive.GenerateGeometry(sharedMesh, radius0, radius1, height, sides, heightSegments, normalsType, pivotPosition);
			}
			this.radius0 = radius0;
			this.radius1 = radius1;
			this.height = height;
			this.thickness = thickness;
			this.sides = sides;
			this.heightSegments = heightSegments;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x0014F4C8 File Offset: 0x0014D8C8
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius0, this.radius1, this.thickness, this.height, this.sides, this.heightSegments, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x0014F514 File Offset: 0x0014D914
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				if (this.thickness >= 0f)
				{
					HollowConePrimitive.GenerateGeometry(colliderMesh, this.radius0, this.radius1, this.thickness, this.height, this.sides, this.heightSegments, this.normalsType, this.pivotPosition);
				}
				else
				{
					ConePrimitive.GenerateGeometry(colliderMesh, this.radius0, this.radius1, this.height, this.sides, this.heightSegments, this.normalsType, this.pivotPosition);
				}
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x0014F5C4 File Offset: 0x0014D9C4
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius0"] = this.radius0;
			dictionary["radius1"] = this.radius1;
			dictionary["thickness"] = this.thickness;
			dictionary["height"] = this.height;
			dictionary["sides"] = this.sides;
			dictionary["heightSegments"] = this.heightSegments;
			return dictionary;
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x0014F660 File Offset: 0x0014DA60
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius0 = (float)dictionary["radius0"];
			this.height = (float)dictionary["height"];
			this.thickness = (float)dictionary["thickness"];
			this.radius1 = (float)dictionary["radius1"];
			this.sides = (int)dictionary["sides"];
			this.heightSegments = (int)dictionary["heightSegments"];
			return dictionary;
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x0014F6FA File Offset: 0x0014DAFA
		public override void SetHeight(float height)
		{
			this.height = height;
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x0014F704 File Offset: 0x0014DB04
		public override void SetWidth(float width0, float length0)
		{
			float num = width0 * 0.5f;
			float num2 = length0 * 0.5f;
			float num3 = Mathf.Max(this.radius0, this.radius1);
			if (Mathf.Abs(num - num3) > Mathf.Abs(num2 - num3))
			{
				num3 = num;
			}
			else
			{
				num3 = num2;
			}
			float num4 = this.radius0 - this.radius1;
			if (this.radius0 > this.radius1)
			{
				this.radius0 = num3;
				this.radius1 = this.radius0 - num4;
			}
			else
			{
				this.radius1 = num3;
				this.radius0 = this.radius1 + num4;
			}
		}

		// Token: 0x04002AE2 RID: 10978
		public float radius0;

		// Token: 0x04002AE3 RID: 10979
		public float radius1;

		// Token: 0x04002AE4 RID: 10980
		public float thickness;

		// Token: 0x04002AE5 RID: 10981
		public float height;

		// Token: 0x04002AE6 RID: 10982
		public int sides;

		// Token: 0x04002AE7 RID: 10983
		public int heightSegments;
	}
}
