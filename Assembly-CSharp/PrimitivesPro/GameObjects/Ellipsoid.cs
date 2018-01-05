using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000851 RID: 2129
	public class Ellipsoid : BaseObject
	{
		// Token: 0x06004213 RID: 16915 RVA: 0x0014FE40 File Offset: 0x0014E240
		public static Ellipsoid Create(float width, float height, float length, int segments, NormalsType normals, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("EllipsoidPro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Ellipsoid ellipsoid = gameObject.AddComponent<Ellipsoid>();
			ellipsoid.GenerateGeometry(width, height, length, segments, normals, pivotPosition);
			return ellipsoid;
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x0014FE94 File Offset: 0x0014E294
		public void GenerateGeometry(float width, float height, float length, int segments, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = EllipsoidPrimitive.GenerateGeometry(sharedMesh, width, height, length, segments, normalsType, pivotPosition);
			this.width = width;
			this.height = height;
			this.length = length;
			this.segments = segments;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x0014FF14 File Offset: 0x0014E314
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.width, this.height, this.length, this.segments, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x0014FF48 File Offset: 0x0014E348
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				EllipsoidPrimitive.GenerateGeometry(colliderMesh, this.width, this.height, this.length, this.segments, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x0014FFA4 File Offset: 0x0014E3A4
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["width"] = this.width;
			dictionary["height"] = this.height;
			dictionary["length"] = this.length;
			dictionary["segments"] = this.segments;
			return dictionary;
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x00150014 File Offset: 0x0014E414
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.width = (float)dictionary["width"];
			this.height = (float)dictionary["height"];
			this.length = (float)dictionary["length"];
			this.segments = (int)dictionary["segments"];
			return dictionary;
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x00150082 File Offset: 0x0014E482
		public override void SetHeight(float height)
		{
			this.height = height * 0.5f;
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x00150091 File Offset: 0x0014E491
		public override void SetWidth(float width0, float length0)
		{
			this.width = width0 * 0.5f;
			this.length = length0 * 0.5f;
		}

		// Token: 0x04002AF4 RID: 10996
		public float width;

		// Token: 0x04002AF5 RID: 10997
		public float height;

		// Token: 0x04002AF6 RID: 10998
		public float length;

		// Token: 0x04002AF7 RID: 10999
		public int segments;
	}
}
