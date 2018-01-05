using System;
using PrimitivesPro;
using PrimitivesPro.MeshCutting;
using PrimitivesPro.Utils;
using UnityEngine;

// Token: 0x02000842 RID: 2114
internal class CutterTest : MonoBehaviour
{
	// Token: 0x0600419E RID: 16798 RVA: 0x0014BBA0 File Offset: 0x00149FA0
	private void Start()
	{
		this.plane = new PrimitivesPro.Utils.Plane(new Vector3(0f, 1f, 0f), Vector3.zero);
		MeshUtils.SetGameObjectActive(this.OriginalObject.gameObject, false);
		UnityEngine.Random.InitState(DateTime.Now.Millisecond);
		this.cutter = new MeshCutter();
	}

	// Token: 0x0600419F RID: 16799 RVA: 0x0014BC00 File Offset: 0x0014A000
	private void OnGUI()
	{
		if (GUI.Button(new Rect((float)(Screen.width - 200), 0f, 200f, 30f), "Test"))
		{
			this.test = true;
		}
		GUI.Label(new Rect((float)(Screen.width - 200), 60f, 200f, 30f), "Tris: " + this.OriginalObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3);
		GUI.Label(new Rect((float)(Screen.width - 200), 80f, 200f, 30f), "Verts: " + this.OriginalObject.GetComponent<MeshFilter>().sharedMesh.vertexCount);
		GUI.Label(new Rect((float)(Screen.width - 200), 100f, 200f, 30f), "Time: " + this.GenerationTimeMS);
		GUI.Label(new Rect((float)(Screen.width - 200), 120f, 200f, 30f), "MinTime: " + this.minTime);
		GUI.Label(new Rect((float)(Screen.width - 200), 140f, 200f, 30f), "MaxTime: " + this.maxTime);
		GUI.Label(new Rect((float)(Screen.width - 200), 160f, 200f, 30f), "AvgTime: " + this.sumTime / (float)this.sumSteps);
		GUI.Label(new Rect((float)(Screen.width - 200), 180f, 200f, 30f), "Success: " + this.success);
		GUI.Label(new Rect((float)(Screen.width - 200), 200f, 200f, 30f), "Failed: " + this.failed);
		GUI.Label(new Rect((float)(Screen.width - 200), 220f, 200f, 30f), "Failure ratio: " + (float)this.failed / (float)this.success * 100f);
		this.triangulate = GUI.Toggle(new Rect(10f, 10f, 100f, 30f), this.triangulate, "Triangulate");
	}

	// Token: 0x060041A0 RID: 16800 RVA: 0x0014BEB5 File Offset: 0x0014A2B5
	private void RandomizePlane()
	{
		this.plane = new PrimitivesPro.Utils.Plane(UnityEngine.Random.onUnitSphere, UnityEngine.Random.insideUnitSphere);
	}

	// Token: 0x060041A1 RID: 16801 RVA: 0x0014BECC File Offset: 0x0014A2CC
	private void Update()
	{
		if (this.test)
		{
			this.RandomizePlane();
			this.Cut();
		}
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x0014BEE8 File Offset: 0x0014A2E8
	private void Cut()
	{
		if (this.cut0)
		{
			UnityEngine.Object.Destroy(this.cut0);
		}
		if (this.cut1)
		{
			UnityEngine.Object.Destroy(this.cut1);
		}
		MeshUtils.SetGameObjectActive(this.OriginalObject.gameObject, false);
		Mesh sharedMesh = this.OriginalObject.GetComponent<MeshFilter>().sharedMesh;
		Mesh mesh = null;
		Mesh mesh2 = null;
		try
		{
			ContourData contourData;
			this.GenerationTimeMS = this.cutter.Cut(sharedMesh, this.OriginalObject.transform, this.plane, this.triangulate, MeshCutter.defaultCrossSection, out mesh, out mesh2, out contourData);
			this.success++;
			if (this.GenerationTimeMS > this.maxTime)
			{
				this.maxTime = this.GenerationTimeMS;
			}
			if (this.GenerationTimeMS < this.minTime)
			{
				this.minTime = this.GenerationTimeMS;
			}
			this.sumTime += this.GenerationTimeMS;
			this.sumSteps++;
		}
		catch (Exception arg)
		{
			this.failed++;
			Debug.Log("Exception!!! " + arg);
			Debug.Break();
			return;
		}
		if (mesh != null)
		{
			this.cut0 = new GameObject(this.OriginalObject.name + "_cut0");
			MeshFilter meshFilter = this.cut0.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = mesh;
			MeshRenderer meshRenderer = this.cut0.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			this.cut0.transform.position = this.OriginalObject.transform.position;
			this.cut0.transform.rotation = this.OriginalObject.transform.rotation;
			this.cut0.transform.localScale = this.OriginalObject.transform.localScale;
			this.cut0.transform.position += this.plane.Normal * 1f;
		}
		if (mesh2 != null)
		{
			this.cut1 = new GameObject(this.OriginalObject.name + "_cut1");
			MeshFilter meshFilter2 = this.cut1.AddComponent<MeshFilter>();
			meshFilter2.sharedMesh = mesh2;
			MeshRenderer meshRenderer2 = this.cut1.AddComponent<MeshRenderer>();
			meshRenderer2.sharedMaterial = new Material(Shader.Find("Diffuse"));
			this.cut1.transform.position = this.OriginalObject.transform.position;
			this.cut1.transform.rotation = this.OriginalObject.transform.rotation;
			this.cut1.transform.localScale = this.OriginalObject.transform.localScale;
			this.cut1.transform.position -= this.plane.Normal * 1f;
		}
	}

	// Token: 0x04002A99 RID: 10905
	public GameObject OriginalObject;

	// Token: 0x04002A9A RID: 10906
	private GameObject cut0;

	// Token: 0x04002A9B RID: 10907
	private GameObject cut1;

	// Token: 0x04002A9C RID: 10908
	private PrimitivesPro.Utils.Plane plane;

	// Token: 0x04002A9D RID: 10909
	private float GenerationTimeMS;

	// Token: 0x04002A9E RID: 10910
	private int success;

	// Token: 0x04002A9F RID: 10911
	private int failed;

	// Token: 0x04002AA0 RID: 10912
	private float minTime = float.MaxValue;

	// Token: 0x04002AA1 RID: 10913
	private float maxTime = float.MinValue;

	// Token: 0x04002AA2 RID: 10914
	private float sumTime;

	// Token: 0x04002AA3 RID: 10915
	private int sumSteps;

	// Token: 0x04002AA4 RID: 10916
	private bool test;

	// Token: 0x04002AA5 RID: 10917
	private bool triangulate = true;

	// Token: 0x04002AA6 RID: 10918
	private MeshCutter cutter;
}
