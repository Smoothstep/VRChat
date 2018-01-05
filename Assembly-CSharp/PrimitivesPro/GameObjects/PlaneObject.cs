using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000854 RID: 2132
	public class PlaneObject : BaseObject
	{
		// Token: 0x06004228 RID: 16936 RVA: 0x00150354 File Offset: 0x0014E754
		public static PlaneObject Create(float width, float length, int widthSegments, int lengthSegments)
		{
			GameObject gameObject = new GameObject("PlanePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			PlaneObject planeObject = gameObject.AddComponent<PlaneObject>();
			planeObject.GenerateGeometry(width, length, widthSegments, lengthSegments);
			return planeObject;
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x001503A4 File Offset: 0x0014E7A4
		public void GenerateGeometry(float width, float length, int widthSegments, int lengthSegments)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = PlanePrimitive.GenerateGeometry(sharedMesh, width, length, widthSegments, lengthSegments);
			this.width = width;
			this.length = length;
			this.widthSegments = widthSegments;
			this.lengthSegments = lengthSegments;
			this.flipNormals = false;
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x00150410 File Offset: 0x0014E810
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.width, this.length, this.widthSegments, this.lengthSegments);
			base.GenerateGeometry();
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x00150438 File Offset: 0x0014E838
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				PlanePrimitive.GenerateGeometry(colliderMesh, this.width, this.length, this.widthSegments, this.lengthSegments);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x00150488 File Offset: 0x0014E888
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["width"] = this.width;
			dictionary["length"] = this.length;
			dictionary["widthSegments"] = this.widthSegments;
			dictionary["lengthSegments"] = this.lengthSegments;
			return dictionary;
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x001504F8 File Offset: 0x0014E8F8
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.width = (float)dictionary["width"];
			this.length = (float)dictionary["length"];
			this.widthSegments = (int)dictionary["widthSegments"];
			this.lengthSegments = (int)dictionary["lengthSegments"];
			return dictionary;
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x00150566 File Offset: 0x0014E966
		public override void SetWidth(float width0, float length0)
		{
			this.width = width0;
			this.length = length0;
		}

		// Token: 0x04002AFC RID: 11004
		public float width;

		// Token: 0x04002AFD RID: 11005
		public float length;

		// Token: 0x04002AFE RID: 11006
		public int widthSegments;

		// Token: 0x04002AFF RID: 11007
		public int lengthSegments;
	}
}
