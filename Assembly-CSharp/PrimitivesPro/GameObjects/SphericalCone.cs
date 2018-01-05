using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000859 RID: 2137
	public class SphericalCone : BaseObject
	{
		// Token: 0x06004253 RID: 16979 RVA: 0x00151090 File Offset: 0x0014F490
		public static SphericalCone Create(float radius, int segments, float coneAngle, NormalsType normals, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("SphericalConePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			SphericalCone sphericalCone = gameObject.AddComponent<SphericalCone>();
			sphericalCone.GenerateGeometry(radius, segments, coneAngle, normals, pivotPosition);
			return sphericalCone;
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x001510E0 File Offset: 0x0014F4E0
		public void GenerateGeometry(float radius, int segments, float coneAngle, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = SphericalConePrimitive.GenerateGeometry(sharedMesh, radius, segments, coneAngle, normalsType, pivotPosition);
			this.radius = radius;
			this.segments = segments;
			this.coneAngle = coneAngle;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x00151156 File Offset: 0x0014F556
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius, this.segments, this.coneAngle, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x00151184 File Offset: 0x0014F584
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				SphericalConePrimitive.GenerateGeometry(colliderMesh, this.radius, this.segments, this.coneAngle, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x001511DC File Offset: 0x0014F5DC
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius"] = this.radius;
			dictionary["segments"] = this.segments;
			dictionary["coneAngle"] = this.coneAngle;
			return dictionary;
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x00151234 File Offset: 0x0014F634
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius = (float)dictionary["radius"];
			this.segments = (int)dictionary["segments"];
			this.coneAngle = (float)dictionary["coneAngle"];
			return dictionary;
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x0015128C File Offset: 0x0014F68C
		public override void SetHeight(float height)
		{
			this.radius = height / 2f;
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x0015129C File Offset: 0x0014F69C
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

		// Token: 0x04002B14 RID: 11028
		public float radius;

		// Token: 0x04002B15 RID: 11029
		public int segments;

		// Token: 0x04002B16 RID: 11030
		public float coneAngle;
	}
}
