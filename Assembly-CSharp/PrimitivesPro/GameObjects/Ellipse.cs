using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000850 RID: 2128
	public class Ellipse : BaseObject
	{
		// Token: 0x0600420B RID: 16907 RVA: 0x0014FC50 File Offset: 0x0014E050
		public static Ellipse Create(float radius0, float radius1, int segments)
		{
			GameObject gameObject = new GameObject("EllipsePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Ellipse ellipse = gameObject.AddComponent<Ellipse>();
			ellipse.GenerateGeometry(radius0, radius1, segments);
			return ellipse;
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x0014FC9C File Offset: 0x0014E09C
		public void GenerateGeometry(float radius0, float radius1, int segments)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = EllipsePrimitive.GenerateGeometry(sharedMesh, radius0, radius1, segments);
			this.radius0 = radius0;
			this.radius1 = radius1;
			this.segments = segments;
			this.flipNormals = false;
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x0014FCFE File Offset: 0x0014E0FE
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius0, this.radius1, this.segments);
			base.GenerateGeometry();
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x0014FD20 File Offset: 0x0014E120
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				EllipsePrimitive.GenerateGeometry(colliderMesh, this.radius0, this.radius1, this.segments);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x0014FD6C File Offset: 0x0014E16C
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius0"] = this.radius0;
			dictionary["radius1"] = this.radius1;
			dictionary["segments"] = this.segments;
			return dictionary;
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x0014FDC4 File Offset: 0x0014E1C4
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius0 = (float)dictionary["radius0"];
			this.radius1 = (float)dictionary["radius1"];
			this.segments = (int)dictionary["depth"];
			return dictionary;
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x0014FE1C File Offset: 0x0014E21C
		public override void SetWidth(float width0, float length0)
		{
			this.radius0 = width0 * 0.5f;
			this.radius1 = length0 * 0.5f;
		}

		// Token: 0x04002AF1 RID: 10993
		public float radius0;

		// Token: 0x04002AF2 RID: 10994
		public float radius1;

		// Token: 0x04002AF3 RID: 10995
		public int segments;
	}
}
