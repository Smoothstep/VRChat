using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200085E RID: 2142
	public class Tube : BaseObject
	{
		// Token: 0x0600427E RID: 17022 RVA: 0x00151D24 File Offset: 0x00150124
		public static Tube Create(float radius0, float radius1, float height, int sides, int heightSegments, float slice, bool radialMapping, NormalsType normalsType, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("TubePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Tube tube = gameObject.AddComponent<Tube>();
			tube.GenerateGeometry(radius0, radius1, height, sides, heightSegments, slice, radialMapping, normalsType, pivotPosition);
			return tube;
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x00151D7C File Offset: 0x0015017C
		public void GenerateGeometry(float radius0, float radius1, float height, int sides, int heightSegments, float slice, bool radialMapping, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = TubePrimitive.GenerateGeometry(sharedMesh, radius0, radius1, height, sides, heightSegments, slice, radialMapping, normalsType, pivotPosition);
			this.radius0 = radius0;
			this.radius1 = radius1;
			this.height = height;
			this.sides = sides;
			this.heightSegments = heightSegments;
			this.slice = slice;
			this.radialMapping = radialMapping;
			this.normalsType = normalsType;
			this.flipNormals = false;
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x00151E14 File Offset: 0x00150214
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius0, this.radius1, this.height, this.sides, this.heightSegments, this.slice, this.radialMapping, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x00151E64 File Offset: 0x00150264
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				TubePrimitive.GenerateGeometry(colliderMesh, this.radius0, this.radius1, this.height, this.sides, this.heightSegments, this.slice, this.radialMapping, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x00151ED4 File Offset: 0x001502D4
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius0"] = this.radius0;
			dictionary["radius1"] = this.radius1;
			dictionary["height"] = this.height;
			dictionary["sides"] = this.sides;
			dictionary["heightSegments"] = this.heightSegments;
			dictionary["slice"] = this.slice;
			return dictionary;
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x00151F70 File Offset: 0x00150370
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius0 = (float)dictionary["radius0"];
			this.radius1 = (float)dictionary["radius1"];
			this.height = (float)dictionary["height"];
			this.sides = (int)dictionary["sides"];
			this.heightSegments = (int)dictionary["heightSegments"];
			this.slice = (float)dictionary["slice"];
			return dictionary;
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x0015200A File Offset: 0x0015040A
		public override void SetHeight(float height)
		{
			this.height = height;
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x00152014 File Offset: 0x00150414
		public override void SetWidth(float width0, float length0)
		{
			float num = width0 * 0.5f;
			float num2 = length0 * 0.5f;
			if (Mathf.Abs(num - this.radius1) > Mathf.Abs(num2 - this.radius1))
			{
				this.radius1 = num;
			}
			else
			{
				this.radius1 = num2;
			}
		}

		// Token: 0x04002B2A RID: 11050
		public float radius0;

		// Token: 0x04002B2B RID: 11051
		public float radius1;

		// Token: 0x04002B2C RID: 11052
		public float height;

		// Token: 0x04002B2D RID: 11053
		public int sides;

		// Token: 0x04002B2E RID: 11054
		public int heightSegments;

		// Token: 0x04002B2F RID: 11055
		public float slice;

		// Token: 0x04002B30 RID: 11056
		public bool radialMapping;
	}
}
