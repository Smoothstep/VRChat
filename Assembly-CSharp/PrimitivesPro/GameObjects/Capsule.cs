using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200084A RID: 2122
	public class Capsule : BaseObject
	{
		// Token: 0x060041DF RID: 16863 RVA: 0x0014EEB8 File Offset: 0x0014D2B8
		public static Capsule Create(float radius, float height, int sides, int heightSegments, bool preserveHeight, NormalsType normals, PivotPosition pivotPosition)
		{
			GameObject gameObject = new GameObject("CapsulePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Capsule capsule = gameObject.AddComponent<Capsule>();
			capsule.GenerateGeometry(radius, height, sides, heightSegments, preserveHeight, normals, pivotPosition);
			return capsule;
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x0014EF0C File Offset: 0x0014D30C
		public void GenerateGeometry(float radius, float height, int sides, int heightSegments, bool preserverHeight, NormalsType normalsType, PivotPosition pivotPosition)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = CapsulePrimitive.GenerateGeometry(sharedMesh, radius, height, sides, heightSegments, preserverHeight, normalsType, pivotPosition);
			this.radius = radius;
			this.height = height;
			this.heightSegments = heightSegments;
			this.sides = sides;
			this.preserveHeight = preserverHeight;
			this.normalsType = normalsType;
			this.flipNormals = false;
			this.pivotPosition = pivotPosition;
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x0014EF96 File Offset: 0x0014D396
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.radius, this.height, this.sides, this.heightSegments, this.preserveHeight, this.normalsType, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x0014EFD0 File Offset: 0x0014D3D0
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				CapsulePrimitive.GenerateGeometry(colliderMesh, this.radius, this.height, this.sides, this.heightSegments, this.preserveHeight, this.normalsType, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x0014F034 File Offset: 0x0014D434
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["radius"] = this.radius;
			dictionary["height"] = this.height;
			dictionary["sides"] = this.sides;
			dictionary["heightSegments"] = this.heightSegments;
			dictionary["preserveHeight"] = this.preserveHeight;
			return dictionary;
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x0014F0B8 File Offset: 0x0014D4B8
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.radius = (float)dictionary["radius"];
			this.height = (float)dictionary["height"];
			this.sides = (int)dictionary["sides"];
			this.heightSegments = (int)dictionary["heightSegments"];
			this.preserveHeight = (bool)dictionary["preserveHeight"];
			return dictionary;
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x0014F13C File Offset: 0x0014D53C
		public override void SetHeight(float height)
		{
			if (this.preserveHeight)
			{
				this.height = height;
			}
			else
			{
				this.height = height - this.radius * 2f;
			}
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x0014F16C File Offset: 0x0014D56C
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

		// Token: 0x04002ADB RID: 10971
		public float radius;

		// Token: 0x04002ADC RID: 10972
		public float height;

		// Token: 0x04002ADD RID: 10973
		public int sides;

		// Token: 0x04002ADE RID: 10974
		public int heightSegments;

		// Token: 0x04002ADF RID: 10975
		public bool preserveHeight;
	}
}
