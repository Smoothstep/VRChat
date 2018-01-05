using System;
using PrimitivesPro.MeshCutting;
using PrimitivesPro.Primitives;
using PrimitivesPro.Utils;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x0200084D RID: 2125
	public class CuttingPlane : BaseObject
	{
		// Token: 0x060041F9 RID: 16889 RVA: 0x0014F7A8 File Offset: 0x0014DBA8
		public static CuttingPlane Create(float size)
		{
			GameObject gameObject = new GameObject("CuttingPlanePro");
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			CuttingPlane cuttingPlane = gameObject.AddComponent<CuttingPlane>();
			cuttingPlane.GenerateGeometry(size);
			cuttingPlane.triangulateHoles = true;
			cuttingPlane.deleteOriginal = true;
			cuttingPlane.crossSection = new Vector4(0f, 0f, 1f, 1f);
			return cuttingPlane;
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x0014F81F File Offset: 0x0014DC1F
		private void Awake()
		{
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x0014F824 File Offset: 0x0014DC24
		public void GenerateGeometry(float size)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component.sharedMesh == null)
			{
				component.sharedMesh = new Mesh();
			}
			Mesh sharedMesh = component.sharedMesh;
			base.GenerationTimeMS = PlanePrimitive.GenerateGeometry(sharedMesh, size, size, 1, 1);
			this.size = size;
			this.flipNormals = false;
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x0014F87C File Offset: 0x0014DC7C
		public PrimitivesPro.Utils.Plane ComputePlane()
		{
			return new PrimitivesPro.Utils.Plane(base.transform.position, base.transform.position + base.transform.right, base.transform.position + base.transform.forward);
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x0014F8D0 File Offset: 0x0014DCD0
		public void Cut(GameObject primitive, bool fillHoles, bool deleteOriginal)
		{
			MeshCutter meshCutter = new MeshCutter();
			PrimitivesPro.Utils.Plane plane = this.ComputePlane();
			GameObject gameObject;
			GameObject gameObject2;
			ContourData contourData;
			base.GenerationTimeMS = meshCutter.Cut(primitive, plane, fillHoles, deleteOriginal, this.crossSection, out gameObject, out gameObject2, out contourData);
			if (gameObject != null)
			{
				gameObject.AddComponent<DefaultObject>();
				gameObject2.AddComponent<DefaultObject>();
			}
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x0014F924 File Offset: 0x0014DD24
		public void Cut()
		{
			MeshCutter meshCutter = new MeshCutter();
			PrimitivesPro.Utils.Plane plane = this.ComputePlane();
			GameObject gameObject;
			GameObject gameObject2;
			ContourData contourData;
			base.GenerationTimeMS = meshCutter.Cut(this.cuttingObject, plane, this.triangulateHoles, this.deleteOriginal, this.crossSection, out gameObject, out gameObject2, out contourData);
			if (gameObject != null)
			{
				gameObject.AddComponent<DefaultObject>();
				gameObject2.AddComponent<DefaultObject>();
			}
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x0014F984 File Offset: 0x0014DD84
		public override void GenerateGeometry()
		{
			this.GenerateGeometry(this.size);
		}

		// Token: 0x04002AE8 RID: 10984
		public float size;

		// Token: 0x04002AE9 RID: 10985
		public bool triangulateHoles;

		// Token: 0x04002AEA RID: 10986
		public bool deleteOriginal;

		// Token: 0x04002AEB RID: 10987
		public Vector4 crossSection;

		// Token: 0x04002AEC RID: 10988
		public GameObject cuttingObject;
	}
}
