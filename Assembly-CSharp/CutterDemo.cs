using System;
using PrimitivesPro;
using PrimitivesPro.MeshCutting;
using PrimitivesPro.Utils;
using UnityEngine;

// Token: 0x02000841 RID: 2113
internal class CutterDemo : MonoBehaviour
{
	// Token: 0x06004194 RID: 16788 RVA: 0x0014B57C File Offset: 0x0014997C
	private void Start()
	{
		foreach (GameObject gameObject in this.OriginalObjects)
		{
			MeshUtils.SetGameObjectActive(gameObject.gameObject, false);
		}
		this.OriginalObject = this.OriginalObjects[this.objectIdx];
		this.plane = new PrimitivesPro.Utils.Plane(new Vector3(0f, 1f, 0f), Vector3.zero);
		UnityEngine.Random.InitState(DateTime.Now.Millisecond);
		this.cutter = new MeshCutter();
	}

	// Token: 0x06004195 RID: 16789 RVA: 0x0014B608 File Offset: 0x00149A08
	private void OnGUI()
	{
		GUI.Window(0, new Rect(10f, 10f, 300f, 150f), new GUI.WindowFunction(this.wnd), "Statistics");
		if (GUI.Button(new Rect((float)(Screen.width - 130), 20f, 60f, 30f), "Next"))
		{
			this.NextObject();
		}
		if (GUI.Button(new Rect((float)(Screen.width - 200), 20f, 60f, 30f), "Prev"))
		{
			this.PrevObject();
		}
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x0014B6B0 File Offset: 0x00149AB0
	private void wnd(int id)
	{
		GUI.Label(new Rect(10f, 20f, 200f, 30f), "Triangles: " + this.OriginalObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3);
		GUI.Label(new Rect(10f, 40f, 200f, 30f), "Vertices: " + this.OriginalObject.GetComponent<MeshFilter>().sharedMesh.vertexCount);
		GUI.Label(new Rect(10f, 60f, 300f, 30f), "Cutting time: " + this.GenerationTimeMS + " [ms]");
		GUI.Label(new Rect(10f, 80f, 300f, 30f), "Avg cutting time: " + this.sumTime / (float)this.sumSteps + " [ms]");
		this.triangulate = GUI.Toggle(new Rect(10f, 110f, 300f, 30f), this.triangulate, "Triangulate holes");
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x0014B7EC File Offset: 0x00149BEC
	private void Reset()
	{
		UnityEngine.Object.Destroy(this.cut0);
		UnityEngine.Object.Destroy(this.cut1);
		this.cutTimeout = 0f;
		this.sumTime = 0f;
		this.sumSteps = 0;
	}

	// Token: 0x06004198 RID: 16792 RVA: 0x0014B824 File Offset: 0x00149C24
	private void NextObject()
	{
		this.objectIdx++;
		if (this.objectIdx >= this.OriginalObjects.Length)
		{
			this.objectIdx = 0;
		}
		MeshUtils.SetGameObjectActive(this.OriginalObject.gameObject, false);
		this.OriginalObject = this.OriginalObjects[this.objectIdx];
		MeshUtils.SetGameObjectActive(this.OriginalObject.gameObject, true);
		this.Reset();
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x0014B894 File Offset: 0x00149C94
	private void PrevObject()
	{
		this.objectIdx--;
		if (this.objectIdx < 0)
		{
			this.objectIdx = this.OriginalObjects.Length - 1;
		}
		MeshUtils.SetGameObjectActive(this.OriginalObject.gameObject, false);
		this.OriginalObject = this.OriginalObjects[this.objectIdx];
		MeshUtils.SetGameObjectActive(this.OriginalObject.gameObject, true);
		this.Reset();
	}

	// Token: 0x0600419A RID: 16794 RVA: 0x0014B906 File Offset: 0x00149D06
	private void RandomizePlane()
	{
		this.plane = new PrimitivesPro.Utils.Plane(UnityEngine.Random.insideUnitCircle, Vector3.zero);
	}

	// Token: 0x0600419B RID: 16795 RVA: 0x0014B924 File Offset: 0x00149D24
	private void Update()
	{
		this.cutTimeout -= Time.deltaTime;
		if (this.cutTimeout < 0f)
		{
			this.RandomizePlane();
			this.Cut();
		}
		else if (this.targetTweenTimeout >= 0f)
		{
			if (this.cut0 != null)
			{
				this.cut0.gameObject.transform.Translate(this.tweenAmount[0]);
				this.cut1.gameObject.transform.Translate(this.tweenAmount[1]);
			}
			this.targetTweenTimeout -= Time.deltaTime;
		}
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x0014B9E4 File Offset: 0x00149DE4
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
		try
		{
			ContourData contourData;
			this.GenerationTimeMS = this.cutter.Cut(this.OriginalObject.gameObject, this.plane, this.triangulate, false, MeshCutter.defaultCrossSection, out this.cut0, out this.cut1, out contourData);
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
		catch (Exception)
		{
			Debug.Log("Cutter exception!");
			return;
		}
		if (this.cut0 != null)
		{
			this.tweenAmount[0] = this.plane.Normal * 0.02f;
			this.tweenAmount[1] = this.plane.Normal * -0.02f;
			this.targetTweenTimeout = 0.5f;
			this.cutTimeout = 0.5f;
		}
	}

	// Token: 0x04002A88 RID: 10888
	public GameObject[] OriginalObjects;

	// Token: 0x04002A89 RID: 10889
	private GameObject OriginalObject;

	// Token: 0x04002A8A RID: 10890
	private int objectIdx;

	// Token: 0x04002A8B RID: 10891
	private GameObject cut0;

	// Token: 0x04002A8C RID: 10892
	private GameObject cut1;

	// Token: 0x04002A8D RID: 10893
	private PrimitivesPro.Utils.Plane plane;

	// Token: 0x04002A8E RID: 10894
	private float GenerationTimeMS;

	// Token: 0x04002A8F RID: 10895
	private int success;

	// Token: 0x04002A90 RID: 10896
	private float minTime = float.MaxValue;

	// Token: 0x04002A91 RID: 10897
	private float maxTime = float.MinValue;

	// Token: 0x04002A92 RID: 10898
	private float sumTime;

	// Token: 0x04002A93 RID: 10899
	private int sumSteps;

	// Token: 0x04002A94 RID: 10900
	private float cutTimeout;

	// Token: 0x04002A95 RID: 10901
	private bool triangulate = true;

	// Token: 0x04002A96 RID: 10902
	private Vector3[] tweenAmount = new Vector3[2];

	// Token: 0x04002A97 RID: 10903
	private float targetTweenTimeout;

	// Token: 0x04002A98 RID: 10904
	private MeshCutter cutter;
}
