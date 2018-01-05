using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200085A RID: 2138
	public class SuperEllipsoid : BaseObject
	{
		// Token: 0x0600425C RID: 16988 RVA: 0x001512F4 File Offset: 0x0014F6F4
		public static SuperEllipsoid Create(float width, float height, float length, int segments, float n1, float n2, NormalsType normals, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("SuperEllipsoidPro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			SuperEllipsoid superEllipsoid = gameObject.AddComponent<SuperEllipsoid>();
			superEllipsoid.GenerateGeometry(width, height, length, segments, n1, n2, normals, pivotPosition);
			return superEllipsoid;
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x0015134C File Offset: 0x0014F74C
		public void GenerateGeometry(float width, float height, float length, int segments, float n1, float n2, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = SuperEllipsoidPrimitive.GenerateGeometry(sharedMesh, width, height, length, segments, n1, n2, normalsType, pivotPosition);
			this.width = width;
			this.height = height;
			this.length = length;
			this.segments = segments;
			this.n1 = n1;
			this.n2 = n2;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x001513E0 File Offset: 0x0014F7E0
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.width, this.height, this.length, this.segments, this.n1, this.n2, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x0015142C File Offset: 0x0014F82C
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				SuperEllipsoidPrimitive.GenerateGeometry(colliderMesh, this.width, this.height, this.length, this.segments, this.n1, this.n2, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x00151494 File Offset: 0x0014F894
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["width"] = this.width;
			dictionary["height"] = this.height;
			dictionary["length"] = this.length;
			dictionary["segments"] = this.segments;
			dictionary["n1"] = this.n1;
			dictionary["n2"] = this.n2;
			return dictionary;
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x00151530 File Offset: 0x0014F930
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.width = (float)dictionary["width"];
			this.height = (float)dictionary["height"];
			this.length = (float)dictionary["length"];
			this.segments = (int)dictionary["segments"];
			this.n1 = (float)dictionary["n1"];
			this.n2 = (float)dictionary["n2"];
			return dictionary;
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x001515CA File Offset: 0x0014F9CA
		public override void SetHeight(float height)
		{
			this.height = height * 0.5f;
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x001515D9 File Offset: 0x0014F9D9
		public override void SetWidth(float width0, float length0)
		{
			this.width = width0 * 0.5f;
			this.length = length0 * 0.5f;
		}

		// Token: 0x04002B17 RID: 11031
		public float width;

		// Token: 0x04002B18 RID: 11032
		public float height;

		// Token: 0x04002B19 RID: 11033
		public float length;

		// Token: 0x04002B1A RID: 11034
		public int segments;

		// Token: 0x04002B1B RID: 11035
		public float n1;

		// Token: 0x04002B1C RID: 11036
		public float n2;
	}
}
