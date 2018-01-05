using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000855 RID: 2133
	public class Pyramid : BaseObject
	{
		// Token: 0x06004230 RID: 16944 RVA: 0x00150580 File Offset: 0x0014E980
		public static Pyramid Create(float width, float height, float depth, int widthSegments, int heightSegments, int depthSegments, bool pyramidMap, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("PyramidPro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Pyramid pyramid = gameObject.AddComponent<Pyramid>();
			pyramid.GenerateGeometry(width, height, depth, widthSegments, heightSegments, depthSegments, pyramidMap, pivotPosition);
			return pyramid;
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x001505D8 File Offset: 0x0014E9D8
		public void GenerateGeometry(float width, float height, float depth, int widthSegments, int heightSegments, int depthSegments, bool pyramidMap, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = PyramidPrimitive.GenerateGeometry(sharedMesh, width, height, depth, widthSegments, heightSegments, depthSegments, pyramidMap, pivotPosition);
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.widthSegments = widthSegments;
			this.heightSegments = heightSegments;
			this.depthSegments = depthSegments;
			this.flipNormals = false;
			this.pyramidMap = pyramidMap;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x0015066C File Offset: 0x0014EA6C
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.width, this.height, this.depth, this.widthSegments, this.heightSegments, this.depthSegments, this.pyramidMap, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x001506B8 File Offset: 0x0014EAB8
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				PyramidPrimitive.GenerateGeometry(colliderMesh, this.width, this.height, this.depth, this.widthSegments, this.heightSegments, this.depthSegments, this.pyramidMap, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x00150720 File Offset: 0x0014EB20
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["width"] = this.width;
			dictionary["height"] = this.height;
			dictionary["depth"] = this.depth;
			dictionary["widthSegments"] = this.widthSegments;
			dictionary["heightSegments"] = this.heightSegments;
			dictionary["depthSegments"] = this.depthSegments;
			return dictionary;
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x001507BC File Offset: 0x0014EBBC
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.width = (float)dictionary["width"];
			this.height = (float)dictionary["height"];
			this.depth = (float)dictionary["depth"];
			this.widthSegments = (int)dictionary["widthSegments"];
			this.heightSegments = (int)dictionary["heightSegments"];
			this.depthSegments = (int)dictionary["depthSegments"];
			return dictionary;
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x00150856 File Offset: 0x0014EC56
		public override void SetHeight(float height)
		{
			this.height = height;
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x0015085F File Offset: 0x0014EC5F
		public override void SetWidth(float width0, float length0)
		{
			this.width = length0;
			this.depth = width0;
		}

		// Token: 0x04002B00 RID: 11008
		public float width;

		// Token: 0x04002B01 RID: 11009
		public float height;

		// Token: 0x04002B02 RID: 11010
		public float depth;

		// Token: 0x04002B03 RID: 11011
		public int widthSegments;

		// Token: 0x04002B04 RID: 11012
		public int heightSegments;

		// Token: 0x04002B05 RID: 11013
		public int depthSegments;

		// Token: 0x04002B06 RID: 11014
		public bool pyramidMap;
	}
}
