using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000856 RID: 2134
	public class Ring : BaseObject
	{
		// Token: 0x06004239 RID: 16953 RVA: 0x00150878 File Offset: 0x0014EC78
		public static Ring Create(float radius0, float radius1, int segments)
		{
			GameObject gameObject = new GameObject("RingPro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Ring ring = gameObject.AddComponent<Ring>();
			ring.GenerateGeometry(radius0, radius1, segments);
			return ring;
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x001508C4 File Offset: 0x0014ECC4
		public void GenerateGeometry(float radius0, float radius1, int segments)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = RingPrimitive.GenerateGeometry(sharedMesh, radius0, radius1, segments);
			this.radius0 = radius0;
			this.radius1 = radius1;
			this.segments = segments;
			this.flipNormals = false;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x00150926 File Offset: 0x0014ED26
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius0, this.radius1, this.segments);
			base.GenerateGeometry();
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x00150948 File Offset: 0x0014ED48
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				RingPrimitive.GenerateGeometry(colliderMesh, this.radius0, this.radius1, this.segments);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x00150994 File Offset: 0x0014ED94
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius0"] = this.radius0;
			dictionary["radius1"] = this.radius1;
			dictionary["segments"] = this.segments;
			return dictionary;
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x001509EC File Offset: 0x0014EDEC
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius0 = (float)dictionary["radius0"];
			this.radius1 = (float)dictionary["radius1"];
			this.segments = (int)dictionary["segments"];
			return dictionary;
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x00150A44 File Offset: 0x0014EE44
		public override void SetWidth(float width0, float length0)
		{
			float num = width0 * 0.5f;
			float num2 = length0 * 0.5f;
			float num3 = Mathf.Max(this.radius0, this.radius1);
			if (Mathf.Abs(num - num3) > Mathf.Abs(num2 - num3))
			{
				num3 = num;
			}
			else
			{
				num3 = num2;
			}
			float num4 = this.radius0 - this.radius1;
			if (this.radius0 > this.radius1)
			{
				this.radius0 = num3;
				this.radius1 = this.radius0 - num4;
			}
			else
			{
				this.radius1 = num3;
				this.radius0 = this.radius1 + num4;
			}
		}

		// Token: 0x04002B07 RID: 11015
		public float radius0;

		// Token: 0x04002B08 RID: 11016
		public float radius1;

		// Token: 0x04002B09 RID: 11017
		public int segments;
	}
}
