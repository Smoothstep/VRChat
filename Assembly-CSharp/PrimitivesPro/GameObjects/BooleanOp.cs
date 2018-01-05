using System;
using System.Diagnostics;
using PrimitivesPro.CSG;
using UnityEngine;

namespace PrimitivesPro.GameObjects
{
	// Token: 0x02000848 RID: 2120
	public class BooleanOp : BaseObject
	{
		// Token: 0x060041CF RID: 16847 RVA: 0x0014E5EC File Offset: 0x0014C9EC
		public static BooleanOp Create()
		{
			GameObject gameObject = new GameObject("BooleanOperations");
			return gameObject.AddComponent<BooleanOp>();
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x0014E60C File Offset: 0x0014CA0C
		public void Substract()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			CSG.CSG csg = new CSG.CSG();
			csg.Construct(this.A.GetComponent<MeshFilter>().sharedMesh, this.A.transform, 0);
			CSG.CSG csg2 = new CSG.CSG();
			csg2.Construct(this.B.GetComponent<MeshFilter>().sharedMesh, this.B.transform, 1);
			CSG.CSG csg3 = csg.Substract(csg2);
			Mesh sharedMesh = csg3.ToMesh();
			this.Result = new GameObject("Substract");
			DefaultObject defaultObject = this.Result.AddComponent<DefaultObject>();
			MeshFilter meshFilter = this.Result.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = sharedMesh;
			MeshRenderer meshRenderer = this.Result.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterials = new Material[]
			{
				this.A.GetComponent<MeshRenderer>().sharedMaterial,
				this.B.GetComponent<MeshRenderer>().sharedMaterial
			};
			if (this.DeleteOriginal)
			{
				UnityEngine.Object.DestroyImmediate(this.A);
				UnityEngine.Object.DestroyImmediate(this.B);
			}
			stopwatch.Stop();
			defaultObject.GenerationTimeMS = (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x0014E730 File Offset: 0x0014CB30
		public void Test()
		{
			CSG.CSG csg = new CSG.CSG();
			csg.Construct(this.A.GetComponent<MeshFilter>().sharedMesh, this.A.transform, 0);
			Mesh sharedMesh = csg.Test().ToMesh();
			this.Result = new GameObject("Test");
			this.Result.AddComponent<DefaultObject>();
			MeshFilter meshFilter = this.Result.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = sharedMesh;
			MeshRenderer meshRenderer = this.Result.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterials = new Material[]
			{
				this.A.GetComponent<MeshRenderer>().sharedMaterial,
				this.B.GetComponent<MeshRenderer>().sharedMaterial
			};
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x0014E7E0 File Offset: 0x0014CBE0
		public void Inverse()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			CSG.CSG csg = new CSG.CSG();
			csg.Construct(this.A.GetComponent<MeshFilter>().sharedMesh, this.A.transform, 0);
			CSG.CSG csg2 = csg.Inverse();
			Mesh sharedMesh = csg2.ToMesh();
			this.Result = new GameObject("Inverse");
			DefaultObject defaultObject = this.Result.AddComponent<DefaultObject>();
			MeshFilter meshFilter = this.Result.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = sharedMesh;
			MeshRenderer meshRenderer = this.Result.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(this.A.GetComponent<MeshRenderer>().sharedMaterial);
			if (this.DeleteOriginal)
			{
				UnityEngine.Object.DestroyImmediate(this.A);
			}
			stopwatch.Stop();
			defaultObject.GenerationTimeMS = (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x0014E8B4 File Offset: 0x0014CCB4
		public void Union()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			CSG.CSG csg = new CSG.CSG();
			csg.Construct(this.A.GetComponent<MeshFilter>().sharedMesh, this.A.transform, 0);
			CSG.CSG csg2 = new CSG.CSG();
			csg2.Construct(this.B.GetComponent<MeshFilter>().sharedMesh, this.B.transform, 1);
			CSG.CSG csg3 = csg.Union(csg2);
			Mesh sharedMesh = csg3.ToMesh();
			this.Result = new GameObject("Union");
			DefaultObject defaultObject = this.Result.AddComponent<DefaultObject>();
			MeshFilter meshFilter = this.Result.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = sharedMesh;
			MeshRenderer meshRenderer = this.Result.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterials = new Material[]
			{
				this.A.GetComponent<MeshRenderer>().sharedMaterial,
				this.B.GetComponent<MeshRenderer>().sharedMaterial
			};
			if (this.DeleteOriginal)
			{
				UnityEngine.Object.DestroyImmediate(this.A);
				UnityEngine.Object.DestroyImmediate(this.B);
			}
			stopwatch.Stop();
			defaultObject.GenerationTimeMS = (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x0014E9D8 File Offset: 0x0014CDD8
		public void Intersect()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
            CSG.CSG csg = new CSG.CSG();
			csg.Construct(this.A.GetComponent<MeshFilter>().sharedMesh, this.A.transform, 0);
			CSG.CSG csg2 = new CSG.CSG();
			csg2.Construct(this.B.GetComponent<MeshFilter>().sharedMesh, this.B.transform, 1);
			CSG.CSG csg3 = csg.Intersect(csg2);
			Mesh sharedMesh = csg3.ToMesh();
			this.Result = new GameObject("Intersect");
			DefaultObject defaultObject = this.Result.AddComponent<DefaultObject>();
			MeshFilter meshFilter = this.Result.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = sharedMesh;
			MeshRenderer meshRenderer = this.Result.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterials = new Material[]
			{
				this.A.GetComponent<MeshRenderer>().sharedMaterial,
				this.B.GetComponent<MeshRenderer>().sharedMaterial
			};
			if (this.DeleteOriginal)
			{
				UnityEngine.Object.DestroyImmediate(this.A);
				UnityEngine.Object.DestroyImmediate(this.B);
			}
			stopwatch.Stop();
			defaultObject.GenerationTimeMS = (float)stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x04002ACF RID: 10959
		public GameObject A;

		// Token: 0x04002AD0 RID: 10960
		public GameObject B;

		// Token: 0x04002AD1 RID: 10961
		public GameObject Result;

		// Token: 0x04002AD2 RID: 10962
		public bool DeleteOriginal;
	}
}
