using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200085D RID: 2141
	public class Triangle : BaseObject
	{
		// Token: 0x06004276 RID: 17014 RVA: 0x00151B88 File Offset: 0x0014FF88
		public static Triangle Create(float side, int subdivision)
		{
			GameObject gameObject = new GameObject("TrianglePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Triangle triangle = gameObject.AddComponent<Triangle>();
			triangle.GenerateGeometry(side, subdivision);
			return triangle;
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x00151BD4 File Offset: 0x0014FFD4
		public void GenerateGeometry(float side, int subdivision)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = TrianglePrimitive.GenerateGeometry(sharedMesh, side, subdivision);
			this.subdivision = subdivision;
			this.side = side;
			this.flipNormals = false;
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x00151C2E File Offset: 0x0015002E
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.side, this.subdivision);
			base.GenerateGeometry();
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x00151C48 File Offset: 0x00150048
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				TrianglePrimitive.GenerateGeometry(colliderMesh, this.side, this.subdivision);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x00151C8C File Offset: 0x0015008C
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["side"] = this.side;
			dictionary["subdivision"] = this.subdivision;
			return dictionary;
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x00151CD0 File Offset: 0x001500D0
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.side = (float)dictionary["side"];
			this.subdivision = (int)dictionary["subdivision"];
			return dictionary;
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x00151D12 File Offset: 0x00150112
		public override void SetWidth(float width0, float length0)
		{
			this.side = length0;
		}

		// Token: 0x04002B28 RID: 11048
		public float side;

		// Token: 0x04002B29 RID: 11049
		public int subdivision;
	}
}
