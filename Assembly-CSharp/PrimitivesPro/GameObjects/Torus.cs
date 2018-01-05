using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200085B RID: 2139
	public class Torus : BaseObject
	{
		// Token: 0x06004265 RID: 16997 RVA: 0x00151600 File Offset: 0x0014FA00
		public static Torus Create(float radius0, float radius1, int torusSegments, int coneSegments, float slice, NormalsType normalsType, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("TorusPro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Torus torus = gameObject.AddComponent<Torus>();
			torus.GenerateGeometry(radius0, radius1, torusSegments, coneSegments, slice, normalsType, pivotPosition);
			return torus;
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x00151654 File Offset: 0x0014FA54
		public void GenerateGeometry(float radius0, float radius1, int torusSegments, int coneSegments, float slice, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = TorusPrimitive.GenerateGeometry(sharedMesh, radius0, radius1, torusSegments, coneSegments, slice, normalsType, pivotPosition);
			this.radius0 = radius0;
			this.radius1 = radius1;
			this.torusSegments = torusSegments;
			this.coneSegments = coneSegments;
			this.normalsType = normalsType;
			this.slice = slice;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x001516DE File Offset: 0x0014FADE
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius0, this.radius1, this.torusSegments, this.coneSegments, this.slice, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x00151718 File Offset: 0x0014FB18
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				TorusPrimitive.GenerateGeometry(colliderMesh, this.radius0, this.radius1, this.torusSegments, this.coneSegments, this.slice, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x0015177C File Offset: 0x0014FB7C
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius0"] = this.radius0;
			dictionary["radius1"] = this.radius1;
			dictionary["torusSegments"] = this.torusSegments;
			dictionary["coneSegments"] = this.coneSegments;
			dictionary["slice"] = this.slice;
			return dictionary;
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x00151800 File Offset: 0x0014FC00
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius0 = (float)dictionary["radius0"];
			this.radius1 = (float)dictionary["radius1"];
			this.torusSegments = (int)dictionary["torusSegments"];
			this.coneSegments = (int)dictionary["coneSegments"];
			this.slice = (float)((int)dictionary["slice"]);
			return dictionary;
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x00151885 File Offset: 0x0014FC85
		public override void SetHeight(float height)
		{
			this.radius0 = height;
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x0015188E File Offset: 0x0014FC8E
		public override void SetWidth(float width0, float length0)
		{
		}

		// Token: 0x04002B1D RID: 11037
		public float radius0;

		// Token: 0x04002B1E RID: 11038
		public float radius1;

		// Token: 0x04002B1F RID: 11039
		public int torusSegments;

		// Token: 0x04002B20 RID: 11040
		public int coneSegments;

		// Token: 0x04002B21 RID: 11041
		public float slice;
	}
}
