using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000857 RID: 2135
	public class RoundedCube : BaseObject
	{
		// Token: 0x06004241 RID: 16961 RVA: 0x00150AE8 File Offset: 0x0014EEE8
		public static RoundedCube Create(float width, float height, float length, int segments, float roundness, NormalsType normals, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("RoundedCubePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			RoundedCube roundedCube = gameObject.AddComponent<RoundedCube>();
			roundedCube.GenerateGeometry(width, height, length, segments, roundness, normals, pivotPosition);
			return roundedCube;
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x00150B3C File Offset: 0x0014EF3C
		public void GenerateGeometry(float width, float height, float length, int segments, float roundness, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = SuperEllipsoidPrimitive.GenerateGeometry(sharedMesh, width, height, length, segments, roundness, roundness, normalsType, pivotPosition);
			this.width = width;
			this.height = height;
			this.length = length;
			this.segments = segments;
			this.roundness = roundness;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x00150BC8 File Offset: 0x0014EFC8
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.width, this.height, this.length, this.segments, this.roundness, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x00150C00 File Offset: 0x0014F000
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				SuperEllipsoidPrimitive.GenerateGeometry(colliderMesh, this.width, this.height, this.length, this.segments, this.roundness, this.roundness, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x00150C68 File Offset: 0x0014F068
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["width"] = this.width;
			dictionary["height"] = this.height;
			dictionary["length"] = this.length;
			dictionary["segments"] = this.segments;
			dictionary["roundness"] = this.roundness;
			return dictionary;
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x00150CEC File Offset: 0x0014F0EC
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.width = (float)dictionary["width"];
			this.height = (float)dictionary["height"];
			this.length = (float)dictionary["length"];
			this.segments = (int)dictionary["segments"];
			this.roundness = (float)dictionary["roundness"];
			return dictionary;
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x00150D70 File Offset: 0x0014F170
		public override void SetHeight(float height)
		{
			this.height = height * 0.5f;
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x00150D7F File Offset: 0x0014F17F
		public override void SetWidth(float width0, float length0)
		{
			this.width = width0 * 0.5f;
			this.length = length0 * 0.5f;
		}

		// Token: 0x04002B0A RID: 11018
		public float width;

		// Token: 0x04002B0B RID: 11019
		public float height;

		// Token: 0x04002B0C RID: 11020
		public float length;

		// Token: 0x04002B0D RID: 11021
		public int segments;

		// Token: 0x04002B0E RID: 11022
		public float roundness;
	}
}
