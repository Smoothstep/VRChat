using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000858 RID: 2136
	public class Sphere : BaseObject
	{
		// Token: 0x0600424A RID: 16970 RVA: 0x00150DA4 File Offset: 0x0014F1A4
		public static Sphere Create(float radius, int segments, float hemisphere, float innerRadius, float slice, NormalsType normals, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("SpherePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Sphere sphere = gameObject.AddComponent<Sphere>();
			sphere.GenerateGeometry(radius, segments, hemisphere, innerRadius, slice, normals, pivotPosition);
			return sphere;
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x00150DF8 File Offset: 0x0014F1F8
		public void GenerateGeometry(float radius, int segments, float hemisphere, float innerRadius, float slice, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = SpherePrimitive.GenerateGeometry(sharedMesh, radius, segments, hemisphere, innerRadius, slice, normalsType, pivotPosition);
			this.radius = radius;
			this.segments = segments;
			this.hemisphere = hemisphere;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.innerRadius = innerRadius;
			this.slice = slice;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x00150E82 File Offset: 0x0014F282
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius, this.segments, this.hemisphere, this.innerRadius, this.slice, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x00150EBC File Offset: 0x0014F2BC
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				SpherePrimitive.GenerateGeometry(colliderMesh, this.radius, this.segments, this.hemisphere, this.innerRadius, this.slice, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x00150F20 File Offset: 0x0014F320
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius"] = this.radius;
			dictionary["segments"] = this.segments;
			dictionary["innerRadius"] = this.innerRadius;
			dictionary["slice"] = this.slice;
			dictionary["hemisphere"] = this.hemisphere;
			return dictionary;
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x00150FA4 File Offset: 0x0014F3A4
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius = (float)dictionary["radius"];
			this.innerRadius = (float)dictionary["innerRadius"];
			this.slice = (float)dictionary["slice"];
			this.segments = (int)dictionary["segments"];
			this.hemisphere = (float)dictionary["hemisphere"];
			return dictionary;
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x00151028 File Offset: 0x0014F428
		public override void SetHeight(float height)
		{
			this.radius = height / 2f;
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x00151038 File Offset: 0x0014F438
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

		// Token: 0x04002B0F RID: 11023
		public float radius;

		// Token: 0x04002B10 RID: 11024
		public int segments;

		// Token: 0x04002B11 RID: 11025
		public float hemisphere;

		// Token: 0x04002B12 RID: 11026
		public float innerRadius;

		// Token: 0x04002B13 RID: 11027
		public float slice;
	}
}
