using System;
using System.Collections.Generic;
using PrimitivesPro.Primitives;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000844 RID: 2116
	public class Arc : BaseObject
	{
		// Token: 0x060041AE RID: 16814 RVA: 0x0014E264 File Offset: 0x0014C664
		public static Arc Create(float width, float height1, float height2, float depth, int arcSegments, PivotPosition pivot)
		{
			GameObject gameObject = new GameObject("ArcPro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			Arc arc = gameObject.AddComponent<Arc>();
			arc.gizmo = ArcGizmo.Create();
			arc.gizmo.transform.parent = gameObject.transform;
			arc.GenerateGeometry(width, height1, height2, depth, arcSegments, pivot);
			return arc;
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x0014E2D8 File Offset: 0x0014C6D8
		public void GenerateGeometry(float width, float height1, float height2, float depth, int arcSegments, PivotPosition pivot)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			sharedMesh.Clear();
			base.GenerationTimeMS = ArcPrimitive.GenerateGeometry(sharedMesh, width, height1, height2, depth, arcSegments, this.gizmo.transform.localPosition, pivot);
			this.width = width;
			this.height1 = height1;
			this.height2 = height2;
			this.depth = depth;
			this.arcSegments = arcSegments;
			this.flipNormals = false;
			this.pivotPosition = pivot;
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x0014E36E File Offset: 0x0014C76E
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.width, this.height1, this.height2, this.depth, this.arcSegments, this.pivotPosition);
			base.GenerateGeometry();
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x0014E3A0 File Offset: 0x0014C7A0
		public override void GenerateColliderGeometry()
		{
			Mesh colliderMesh = base.GetColliderMesh();
			if (colliderMesh)
			{
				colliderMesh.Clear();
				ArcPrimitive.GenerateGeometry(colliderMesh, this.width, this.height1, this.height2, this.depth, this.arcSegments, this.gizmo.transform.position, this.pivotPosition);
				base.RefreshMeshCollider();
			}
			base.GenerateColliderGeometry();
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x0014E40C File Offset: 0x0014C80C
		public override Dictionary<string, object> SaveState(bool collision)
		{
			Dictionary<string, object> dictionary = base.SaveState(collision);
			dictionary["width"] = this.width;
			dictionary["height1"] = this.height1;
			dictionary["height2"] = this.height2;
			dictionary["depth"] = this.depth;
			dictionary["arcSegments"] = this.arcSegments;
			dictionary["controlPoint"] = this.gizmo.transform.position;
			return dictionary;
		}

		// Token: 0x060041B3 RID: 16819 RVA: 0x0014E4B0 File Offset: 0x0014C8B0
		public override Dictionary<string, object> LoadState(bool collision)
		{
			Dictionary<string, object> dictionary = base.LoadState(collision);
			this.width = (float)dictionary["width"];
			this.height1 = (float)dictionary["height1"];
			this.height2 = (float)dictionary["height2"];
			this.depth = (float)dictionary["depth"];
			this.arcSegments = (int)dictionary["arcSegments"];
			this.gizmo.transform.position = (Vector3)dictionary["controlPoint"];
			return dictionary;
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x0014E554 File Offset: 0x0014C954
		public override void SetHeight(float height)
		{
			float num = this.height1 - this.height2;
			if (num > 0f)
			{
				this.height1 = height;
				this.height2 = this.height1 - num;
			}
			else
			{
				this.height2 = height;
				this.height1 = this.height2 + num;
			}
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x0014E5A9 File Offset: 0x0014C9A9
		public override void SetWidth(float width0, float length0)
		{
			this.width = width0;
			this.depth = length0;
		}

		// Token: 0x04002AB7 RID: 10935
		public float width;

		// Token: 0x04002AB8 RID: 10936
		public float height1;

		// Token: 0x04002AB9 RID: 10937
		public float height2;

		// Token: 0x04002ABA RID: 10938
		public float depth;

		// Token: 0x04002ABB RID: 10939
		public int arcSegments;

		// Token: 0x04002ABC RID: 10940
		public ArcGizmo gizmo;
	}
}
