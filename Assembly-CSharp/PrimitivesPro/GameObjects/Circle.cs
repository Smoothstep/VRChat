using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200084B RID: 2123
	public class Circle : BaseObject
	{
		// Token: 0x060041E8 RID: 16872 RVA: 0x0014F1C4 File Offset: 0x0014D5C4
		public static Circle Create(float radius, int segments)
		{
			GameObject gameObject = new GameObject("CirclePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Circle circle = gameObject.AddComponent<Circle>();
			circle.GenerateGeometry(radius, segments);
			return circle;
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x0014F210 File Offset: 0x0014D610
		public void GenerateGeometry(float radius, int segments)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = EllipsePrimitive.GenerateGeometry(sharedMesh, radius, radius, segments);
			this.radius = radius;
			this.segments = segments;
			this.flipNormals = false;
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x0014F26B File Offset: 0x0014D66B
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius, this.segments);
			base.GenerateGeometry();
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x0014F288 File Offset: 0x0014D688
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				EllipsePrimitive.GenerateGeometry(colliderMesh, this.radius, this.radius, this.segments);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x0014F2D4 File Offset: 0x0014D6D4
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius"] = this.radius;
			dictionary["segments"] = this.segments;
			return dictionary;
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x0014F318 File Offset: 0x0014D718
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius = (float)dictionary["radius"];
			this.segments = (int)dictionary["segments"];
			return dictionary;
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x0014F35C File Offset: 0x0014D75C
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

		// Token: 0x04002AE0 RID: 10976
		public float radius;

		// Token: 0x04002AE1 RID: 10977
		public int segments;
	}
}
