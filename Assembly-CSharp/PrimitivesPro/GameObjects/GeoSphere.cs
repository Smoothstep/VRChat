using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000852 RID: 2130
	public class GeoSphere : BaseObject
	{
		// Token: 0x0600421C RID: 16924 RVA: 0x001500B8 File Offset: 0x0014E4B8
		public static GeoSphere Create(float radius, int subdivision, GeoSpherePrimitive.BaseType baseType, NormalsType normals, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("GeoSpherePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			GeoSphere geoSphere = gameObject.AddComponent<GeoSphere>();
			geoSphere.GenerateGeometry(radius, subdivision, baseType, normals, pivotPosition);
			return geoSphere;
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x00150108 File Offset: 0x0014E508
		public void GenerateGeometry(float radius, int subdivision, GeoSpherePrimitive.BaseType baseType, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = GeoSpherePrimitive.GenerateGeometry(sharedMesh, radius, subdivision, baseType, normalsType, pivotPosition);
			this.radius = radius;
			this.subdivision = subdivision;
			this.baseType = baseType;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x0015017E File Offset: 0x0014E57E
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius, this.subdivision, this.baseType, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x001501AC File Offset: 0x0014E5AC
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				GeoSpherePrimitive.GenerateGeometry(colliderMesh, this.radius, this.subdivision, this.baseType, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x00150204 File Offset: 0x0014E604
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius"] = this.radius;
			dictionary["subdivision"] = this.subdivision;
			return dictionary;
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x00150248 File Offset: 0x0014E648
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius = (float)dictionary["radius"];
			this.subdivision = (int)dictionary["subdivision"];
			return dictionary;
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x0015028A File Offset: 0x0014E68A
		public override void SetHeight(float height)
		{
			this.radius = height / 2f;
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x0015029C File Offset: 0x0014E69C
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

		// Token: 0x04002AF8 RID: 11000
		public float radius;

		// Token: 0x04002AF9 RID: 11001
		public int subdivision;

		// Token: 0x04002AFA RID: 11002
		public GeoSpherePrimitive.BaseType baseType;
	}
}
