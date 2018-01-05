using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200085C RID: 2140
	public class TorusKnot : BaseObject
	{
		// Token: 0x0600426E RID: 17006 RVA: 0x00151898 File Offset: 0x0014FC98
		public static TorusKnot Create(float radius0, float radius1, int torusSegments, int coneSegments, int P, int Q, NormalsType normalsType, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("TorusKnotPro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			TorusKnot torusKnot = gameObject.AddComponent<TorusKnot>();
			torusKnot.GenerateGeometry(radius0, radius1, torusSegments, coneSegments, P, Q, normalsType, pivotPosition);
			return torusKnot;
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x001518F0 File Offset: 0x0014FCF0
		public void GenerateGeometry(float radius0, float radius1, int torusSegments, int coneSegments, int P, int Q, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = TorusKnotPrimitive.GenerateGeometry(sharedMesh, radius0, radius1, torusSegments, coneSegments, P, Q, normalsType, pivotPosition);
			this.radius0 = radius0;
			this.radius1 = radius1;
			this.torusSegments = torusSegments;
			this.coneSegments = coneSegments;
			this.P = P;
			this.Q = Q;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x00151984 File Offset: 0x0014FD84
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius0, this.radius1, this.torusSegments, this.coneSegments, this.P, this.Q, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x001519D0 File Offset: 0x0014FDD0
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				TorusKnotPrimitive.GenerateGeometry(colliderMesh, this.radius0, this.radius1, this.torusSegments, this.coneSegments, this.P, this.Q, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x00151A38 File Offset: 0x0014FE38
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius0"] = this.radius0;
			dictionary["radius1"] = this.radius1;
			dictionary["torusSegments"] = this.torusSegments;
			dictionary["coneSegments"] = this.coneSegments;
			dictionary["P"] = this.P;
			dictionary["Q"] = this.Q;
			return dictionary;
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x00151AD4 File Offset: 0x0014FED4
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius0 = (float)dictionary["radius0"];
			this.radius1 = (float)dictionary["radius1"];
			this.torusSegments = (int)dictionary["torusSegments"];
			this.coneSegments = (int)dictionary["coneSegments"];
			this.P = (int)dictionary["P"];
			this.Q = (int)dictionary["Q"];
			return dictionary;
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x00151B6E File Offset: 0x0014FF6E
		public override void SetHeight(float height)
		{
			this.radius0 = height * 0.3f;
		}

		// Token: 0x04002B22 RID: 11042
		public float radius0;

		// Token: 0x04002B23 RID: 11043
		public float radius1;

		// Token: 0x04002B24 RID: 11044
		public int torusSegments;

		// Token: 0x04002B25 RID: 11045
		public int coneSegments;

		// Token: 0x04002B26 RID: 11046
		public int P;

		// Token: 0x04002B27 RID: 11047
		public int Q;
	}
}
