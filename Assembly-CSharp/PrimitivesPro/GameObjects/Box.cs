using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000849 RID: 2121
	public class Box : BaseObject
	{
		// Token: 0x060041D6 RID: 16854 RVA: 0x0014EB04 File Offset: 0x0014CF04
		public static Box Create(float width, float height, float depth, int widthSegments, int heightSegments, int depthSegments, bool cubeMap, float[] edgeOffset, PivotPosition pivot)
		{
			GameObject gameObject = new GameObject("BoxPro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			if (edgeOffset == null || edgeOffset.Length != 4)
			{
				edgeOffset = new float[4];
			}
			Box box = gameObject.AddComponent<Box>();
			box.GenerateGeometry(width, height, depth, widthSegments, heightSegments, depthSegments, cubeMap, edgeOffset, pivot);
			return box;
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x0014EB74 File Offset: 0x0014CF74
		public void GenerateGeometry(float width, float height, float depth, int widthSegments, int heightSegments, int depthSegments, bool cubeMap, float[] edgeOffset, PivotPosition pivot)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			sharedMesh.Clear();
			base.GenerationTimeMS = BoxPrimitive.GenerateGeometry(sharedMesh, width, height, depth, widthSegments, heightSegments, depthSegments, cubeMap, edgeOffset, this.flipUVMapping, pivot);
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.widthSegments = widthSegments;
			this.heightSegments = heightSegments;
			this.depthSegments = depthSegments;
			this.cubeMap = cubeMap;
			this.flipNormals = false;
			this.pivotPosition = pivot;
			this.edgeOffsets = edgeOffset;
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x0014EC20 File Offset: 0x0014D020
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.width, this.height, this.depth, this.widthSegments, this.heightSegments, this.depthSegments, this.cubeMap, this.edgeOffsets, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x0014EC70 File Offset: 0x0014D070
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				BoxPrimitive.GenerateGeometry(colliderMesh, this.width, this.height, this.depth, this.widthSegments, this.heightSegments, this.depthSegments, this.cubeMap, this.edgeOffsets, this.flipUVMapping, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x0014ECE4 File Offset: 0x0014D0E4
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["width"] = this.width;
			dictionary["height"] = this.height;
			dictionary["depth"] = this.depth;
			dictionary["widthSegments"] = this.widthSegments;
			dictionary["heightSegments"] = this.heightSegments;
			dictionary["depthSegments"] = this.depthSegments;
			dictionary["edgeOffsets"] = this.edgeOffsets;
			return dictionary;
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x0014ED90 File Offset: 0x0014D190
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.width = (float)dictionary["width"];
			this.height = (float)dictionary["height"];
			this.depth = (float)dictionary["depth"];
			this.widthSegments = (int)dictionary["widthSegments"];
			this.heightSegments = (int)dictionary["heightSegments"];
			this.depthSegments = (int)dictionary["depthSegments"];
			this.edgeOffsets = (float[])dictionary["edgeOffsets"];
			return dictionary;
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x0014EE40 File Offset: 0x0014D240
		public override void SetHeight(float height)
		{
			this.height = height;
		}

		// Token: 0x060041DD RID: 16861 RVA: 0x0014EE4C File Offset: 0x0014D24C
		public override void SetWidth(float width0, float length0)
		{
			this.width = width0;
			this.depth = length0;
			float num = -Mathf.Min(this.edgeOffsets[0], this.edgeOffsets[1]);
			float num2 = Mathf.Max(this.edgeOffsets[2], this.edgeOffsets[3]);
			this.width -= num;
			this.width -= num2;
		}

		// Token: 0x04002AD3 RID: 10963
		public float width;

		// Token: 0x04002AD4 RID: 10964
		public float height;

		// Token: 0x04002AD5 RID: 10965
		public float depth;

		// Token: 0x04002AD6 RID: 10966
		public int widthSegments;

		// Token: 0x04002AD7 RID: 10967
		public int heightSegments;

		// Token: 0x04002AD8 RID: 10968
		public int depthSegments;

		// Token: 0x04002AD9 RID: 10969
		public bool cubeMap;

		// Token: 0x04002ADA RID: 10970
		public float[] edgeOffsets;
	}
}
