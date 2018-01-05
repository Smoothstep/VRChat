using System;
using System.Collections.Generic;
using PrimitivesPro.GameObjects;
using PrimitivesPro.Primitives;
using UnityEngine;

// Token: 0x02000843 RID: 2115
internal class PrimitivesDemo : MonoBehaviour
{
	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x060041A4 RID: 16804 RVA: 0x0014C236 File Offset: 0x0014A636
	public static PrimitivesDemo Instance
	{
		get
		{
			return PrimitivesDemo.instance;
		}
	}

	// Token: 0x060041A5 RID: 16805 RVA: 0x0014C240 File Offset: 0x0014A640
	private void Start()
	{
		PrimitivesDemo.instance = this;
		this.buttons = new List<GameObject>(16);
		this.animTimeout = 0f;
		this.shapeParamsStart = new float[6];
		this.shapeParamsMax = new float[6];
		Vector3 zero = Vector3.zero;
		zero.z = -0.8f;
		zero.x = -3.1f;
		for (int i = 0; i < 20; i++)
		{
			this.CreateButton(i, zero);
			zero.x += 0.73f;
		}
		this.CreateButton(20, new Vector3(10f, 0f, 5f));
		this.CreateButton(21, new Vector3(10f, 0f, 3.8f));
		this.buttons[20].GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Texture"));
		this.buttons[20].GetComponent<Renderer>().material.mainTexture = (Resources.Load("sphereChecker") as Texture2D);
		this.buttons[21].GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Texture"));
		this.buttons[21].GetComponent<Renderer>().material.mainTexture = (Resources.Load("faceNormalsIcon") as Texture2D);
		this.OnButtonHit(0);
	}

	// Token: 0x060041A6 RID: 16806 RVA: 0x0014C3B4 File Offset: 0x0014A7B4
	public void OnButtonHit(int id)
	{
		this.animTimeout = this.animTimeMax;
		BaseObject baseObject = null;
		switch (id)
		{
		case 0:
		{
			baseObject = Triangle.Create(1f, 0);
			float[] array = new float[6];
			array[0] = 4f;
			array[1] = 4f;
			this.shapeParamsMax = array;
			float[] array2 = new float[6];
			array2[0] = 1f;
			array2[1] = 1f;
			this.shapeParamsStart = array2;
			break;
		}
		case 1:
			baseObject = PlaneObject.Create(1f, 1f, 1, 1);
			this.shapeParamsMax = new float[]
			{
				4f,
				4f,
				1f,
				1f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				1f,
				1f,
				1f,
				0f,
				0f
			};
			break;
		case 2:
		{
			baseObject = Circle.Create(1f, 3);
			float[] array3 = new float[6];
			array3[0] = 2.5f;
			array3[1] = 40f;
			this.shapeParamsMax = array3;
			float[] array4 = new float[6];
			array4[0] = 1f;
			array4[1] = 3f;
			this.shapeParamsStart = array4;
			break;
		}
		case 3:
			baseObject = Ellipse.Create(1f, 0.5f, 3);
			this.shapeParamsMax = new float[]
			{
				2.5f,
				1.2f,
				40f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				0.5f,
				3f,
				0f,
				0f,
				0f
			};
			break;
		case 4:
			baseObject = Ring.Create(0.5f, 1f, 3);
			this.shapeParamsMax = new float[]
			{
				1f,
				2.5f,
				40f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				0.5f,
				1f,
				3f,
				0f,
				0f,
				0f
			};
			break;
		case 5:
			baseObject = Box.Create(1f, 1f, 1f, 1, 1, 1, false, null, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				2.5f,
				2.5f,
				2.5f,
				1f,
				1f,
				1f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				1f,
				1f,
				1f,
				1f,
				1f
			};
			break;
		case 6:
			baseObject = Cylinder.Create(1f, 3f, 3, 1, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				1.25f,
				4f,
				40f,
				1f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				3f,
				3f,
				1f,
				0f,
				0f,
				0f
			};
			break;
		case 7:
			baseObject = Cone.Create(1f, 0f, 0f, 2f, 3, 10, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				1.25f,
				0f,
				4f,
				40f,
				10f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				1f,
				2f,
				3f,
				10f,
				0f,
				0f
			};
			break;
		case 8:
		{
			baseObject = Sphere.Create(1f, 4, 0f, 0f, 0f, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			float[] array5 = new float[6];
			array5[0] = 2.25f;
			array5[1] = 40f;
			this.shapeParamsMax = array5;
			float[] array6 = new float[6];
			array6[0] = 1f;
			array6[1] = 4f;
			this.shapeParamsStart = array6;
			break;
		}
		case 9:
			baseObject = Ellipsoid.Create(1f, 1f, 1f, 4, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				1.25f,
				2.45f,
				2.5f,
				40f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				1f,
				1f,
				4f,
				0f,
				0f,
				0f
			};
			break;
		case 10:
			baseObject = Pyramid.Create(1f, 1f, 1f, 1, 1, 1, false, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				2.7f,
				2.7f,
				1.7f,
				1f,
				1f,
				1f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				1f,
				1f,
				1f,
				1f,
				1f,
				0f,
				0f,
				0f
			};
			break;
		case 11:
		{
			baseObject = GeoSphere.Create(1f, 0, GeoSpherePrimitive.BaseType.Icosahedron, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			float[] array7 = new float[6];
			array7[0] = 2.45f;
			array7[1] = 4f;
			this.shapeParamsMax = array7;
			float[] array8 = new float[6];
			array8[0] = 1f;
			this.shapeParamsStart = array8;
			break;
		}
		case 12:
			baseObject = Tube.Create(0.8f, 1f, 1f, 3, 1, 0f, false, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				0.8f,
				1.5f,
				4f,
				40f,
				0f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				0.8f,
				1f,
				1f,
				3f,
				0f,
				0f,
				0f,
				0f
			};
			break;
		case 13:
			baseObject = Capsule.Create(1f, 1f, 4, 1, false, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				1.2f,
				4f,
				40f,
				1f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				1f,
				4f,
				1f,
				0f,
				0f,
				0f
			};
			break;
		case 14:
			baseObject = RoundedCube.Create(1f, 1f, 1f, 1, 0.2f, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				1.6f,
				1.6f,
				1.6f,
				20f,
				0.6f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				1f,
				1f,
				1f,
				0.2f,
				0f,
				0f,
				0f
			};
			break;
		case 15:
			baseObject = Torus.Create(1f, 0.5f, 4, 4, 0f, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				1.6f,
				0.8f,
				40f,
				40f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				0.5f,
				4f,
				4f,
				0f,
				0f,
				0f
			};
			break;
		case 16:
			baseObject = TorusKnot.Create(0.5f, 0.3f, 10, 4, 2, 3, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				1f,
				0.5f,
				120f,
				40f,
				2f,
				3f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				0.5f,
				0.3f,
				10f,
				4f,
				2f,
				3f,
				0f,
				0f,
				0f
			};
			break;
		case 17:
			baseObject = Arc.Create(1f, 1f, 1f, 1f, 10, PivotPosition.Botttom);
			((Arc)baseObject).gizmo.gameObject.transform.localPosition = new Vector3(-1f, -1f, 0f);
			this.shapeParamsMax = new float[]
			{
				4f,
				3f,
				2f,
				1f,
				20f,
				-1f
			};
			this.shapeParamsStart = new float[]
			{
				0.5f,
				0.5f,
				0.1f,
				0.5f,
				0f,
				0f
			};
			break;
		case 18:
			baseObject = SphericalCone.Create(1f, 20, 180f, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				2f,
				40f,
				20f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				1f,
				20f,
				360f,
				0f,
				0f,
				0f
			};
			break;
		case 19:
			baseObject = SuperEllipsoid.Create(1f, 1f, 1f, 20, 0.5f, 1f, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Botttom);
			this.shapeParamsMax = new float[]
			{
				2f,
				2f,
				2f,
				20f,
				0.5f,
				1f,
				0f,
				0f,
				0f
			};
			this.shapeParamsStart = new float[]
			{
				0.5f,
				0.5f,
				0.5f,
				1f,
				0f,
				0f,
				0f,
				0f,
				0f
			};
			break;
		case 20:
			this.textureToggle = !this.textureToggle;
			break;
		case 21:
			this.flatNormals = !this.flatNormals;
			break;
		}
		if (baseObject)
		{
			if (this.shapeOld)
			{
				UnityEngine.Object.Destroy(this.shapeOld.gameObject);
			}
			this.shapeOld = this.shapeMain;
			this.shapeMain = baseObject;
			this.shapeMain.gameObject.GetComponent<Renderer>().material = new Material(this.GetSpecularShader());
			this.shapeMain.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 0.7058824f, 0.7058824f));
			this.shapeMain.gameObject.GetComponent<Renderer>().material.SetColor("_SpecColor", Color.white);
			this.shapeMain.gameObject.transform.position = this.prevPosition.position;
			this.nextShowTimeout = this.nextShowTimeoutMax;
			this.shapeID = id;
		}
		if (this.textureToggle)
		{
			this.shapeMain.GetComponent<MeshRenderer>().sharedMaterial = (Resources.Load("Checker") as Material);
		}
		else
		{
			this.shapeMain.GetComponent<MeshRenderer>().sharedMaterial = new Material(this.GetSpecularShader());
			this.shapeMain.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 0.7058824f, 0.7058824f));
			this.shapeMain.gameObject.GetComponent<Renderer>().material.SetColor("_SpecColor", Color.white);
		}
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x0014CCCC File Offset: 0x0014B0CC
	private Shader GetSpecularShader()
	{
		return Shader.Find("Specular");
	}

	// Token: 0x060041A8 RID: 16808 RVA: 0x0014CCD8 File Offset: 0x0014B0D8
	public void OnButtonHover(int id, bool start)
	{
		Transform transform = this.buttons[id].transform;
		Material sharedMaterial = this.buttons[id].GetComponent<MeshRenderer>().sharedMaterial;
		if (start)
		{
			sharedMaterial.color = new Color(1f, 0f, 0f);
			transform.position -= new Vector3(0f, 0.28f, 0f);
		}
		else
		{
			sharedMaterial.color = new Color(1f, 1f, 1f);
			transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
		}
	}

	// Token: 0x060041A9 RID: 16809 RVA: 0x0014CDA0 File Offset: 0x0014B1A0
	private Texture2D GetTexture(Texture2D tex)
	{
		Texture2D texture2D = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
		texture2D.SetPixels32(tex.GetPixels32());
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060041AA RID: 16810 RVA: 0x0014CDD4 File Offset: 0x0014B1D4
	private void CreateButton(int id, Vector3 position)
	{
		PlaneObject planeObject = PlaneObject.Create(0.67f, 0.67f, 2, 2);
		planeObject.gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(1f, 1f, 1f, 1f);
		BoxCollider boxCollider = planeObject.gameObject.AddComponent<BoxCollider>();
		boxCollider.isTrigger = true;
		planeObject.gameObject.transform.position = position;
		ButtonTrigger buttonTrigger = planeObject.gameObject.AddComponent<ButtonTrigger>();
		buttonTrigger.ID = id;
		this.buttons.Add(planeObject.gameObject);
		switch (id)
		{
		case 0:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/triangle") as Texture2D);
			break;
		case 1:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/plane") as Texture2D);
			break;
		case 2:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/circle") as Texture2D);
			break;
		case 3:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/ellipse") as Texture2D);
			break;
		case 4:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/ring") as Texture2D);
			break;
		case 5:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/box") as Texture2D);
			break;
		case 6:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/cylinder") as Texture2D);
			break;
		case 7:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/cone") as Texture2D);
			break;
		case 8:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/sphere") as Texture2D);
			break;
		case 9:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/ellipsoid") as Texture2D);
			break;
		case 10:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/pyramid") as Texture2D);
			break;
		case 11:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/geosphere") as Texture2D);
			break;
		case 12:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/tube") as Texture2D);
			break;
		case 13:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/capsule") as Texture2D);
			break;
		case 14:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/roundedBox") as Texture2D);
			break;
		case 15:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/torus") as Texture2D);
			break;
		case 16:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/torusKnot") as Texture2D);
			break;
		case 17:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/arc") as Texture2D);
			break;
		case 18:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/sphericalcone") as Texture2D);
			break;
		case 19:
			planeObject.gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Transparent"));
			planeObject.gameObject.GetComponent<Renderer>().material.mainTexture = this.GetTexture(Resources.Load("icons/superellipsoid") as Texture2D);
			break;
		}
		if (planeObject)
		{
			planeObject.gameObject.transform.position = position + new Vector3(0f, 0.1f, 0f);
		}
	}

	// Token: 0x060041AB RID: 16811 RVA: 0x0014D514 File Offset: 0x0014B914
	private void Update()
	{
		if (this.nextShowTimeout > 0f)
		{
			float t = 1f - this.nextShowTimeout / this.nextShowTimeoutMax;
			if (this.shapeOld)
			{
				Vector3 position = Vector3.Lerp(this.shapeOld.gameObject.transform.position, this.nextPosition.position, t);
				this.shapeOld.gameObject.transform.position = position;
			}
			if (this.shapeMain)
			{
				Vector3 position2 = Vector3.Lerp(this.shapeMain.gameObject.transform.position, this.centralPosition.position, t);
				this.shapeMain.gameObject.transform.position = position2;
			}
			this.nextShowTimeout -= Time.deltaTime;
			if (this.shapeOld != null && this.nextShowTimeout <= 0f)
			{
				UnityEngine.Object.Destroy(this.shapeOld.gameObject);
				this.shapeOld = null;
			}
		}
		if (this.shapeMain != null)
		{
			this.animTimeout -= Time.deltaTime;
			this.shapeMain.gameObject.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
			Quaternion rhs = Quaternion.Euler(0f, 360f * this.animTimeout / this.animTimeMax, 0f);
			this.shapeMain.gameObject.transform.rotation *= rhs;
			if (this.animTimeout > 0f)
			{
				float[] array = new float[6];
				float num = 1f - this.animTimeout / this.animTimeMax;
				for (int i = 0; i < 6; i++)
				{
					array[i] = this.shapeParamsStart[i] * (1f - num) + this.shapeParamsMax[i] * num;
				}
				switch (this.shapeID)
				{
				case 0:
					((Triangle)this.shapeMain).GenerateGeometry(array[0], (int)array[1]);
					break;
				case 1:
					((PlaneObject)this.shapeMain).GenerateGeometry(array[0], array[1], 1, 1);
					break;
				case 2:
					((Circle)this.shapeMain).GenerateGeometry(array[0], (int)array[1]);
					break;
				case 3:
					((Ellipse)this.shapeMain).GenerateGeometry(array[0], array[1], (int)array[2]);
					break;
				case 4:
					((Ring)this.shapeMain).GenerateGeometry(array[0], array[1], (int)array[2]);
					break;
				case 5:
					((Box)this.shapeMain).GenerateGeometry(array[0], array[1], array[2], 1, 1, 1, false, null, PivotPosition.Center);
					break;
				case 6:
					((Cylinder)this.shapeMain).GenerateGeometry(array[0], array[1], (int)array[2], 1, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 7:
					((Cone)this.shapeMain).GenerateGeometry(array[0], array[1], 0f, array[2], (int)array[3], (int)array[4], (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 8:
					((Sphere)this.shapeMain).GenerateGeometry(array[0], (int)array[1], 0f, 0f, 0f, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 9:
					((Ellipsoid)this.shapeMain).GenerateGeometry(array[0], array[1], array[2], (int)array[3], (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 10:
					((Pyramid)this.shapeMain).GenerateGeometry(array[0], array[1], array[2], 1, 1, 1, false, PivotPosition.Center);
					break;
				case 11:
					((GeoSphere)this.shapeMain).GenerateGeometry(array[0], (int)array[1], GeoSpherePrimitive.BaseType.Icosahedron, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 12:
					((Tube)this.shapeMain).GenerateGeometry(array[0], array[1], array[2], (int)array[3], (int)array[4], array[4], false, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 13:
					((Capsule)this.shapeMain).GenerateGeometry(array[0], array[1], (int)array[2], (int)array[3], false, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 14:
					((RoundedCube)this.shapeMain).GenerateGeometry(array[0], array[1], array[2], (int)array[3], array[4], (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 15:
					((Torus)this.shapeMain).GenerateGeometry(array[0], array[1], (int)array[2], (int)array[3], 0f, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 16:
					((TorusKnot)this.shapeMain).GenerateGeometry(array[0], array[1], (int)array[2], (int)array[3], 3, 2, (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 17:
					((Arc)this.shapeMain).GenerateGeometry(array[0], array[1], array[2], array[3], (int)array[4], PivotPosition.Center);
					((Arc)this.shapeMain).gizmo.gameObject.transform.localPosition = new Vector3(array[5], array[5], 0f);
					break;
				case 18:
					((SphericalCone)this.shapeMain).GenerateGeometry(array[0], (int)array[1], array[2], (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				case 19:
					((SuperEllipsoid)this.shapeMain).GenerateGeometry(array[0], array[1], array[2], (int)array[3], array[4], array[5], (!this.flatNormals) ? NormalsType.Vertex : NormalsType.Face, PivotPosition.Center);
					break;
				}
			}
		}
	}

	// Token: 0x060041AC RID: 16812 RVA: 0x0014DBA0 File Offset: 0x0014BFA0
	private void OnGUI()
	{
		GUIStyle guistyle = new GUIStyle();
		guistyle.fontSize = 20;
		guistyle.fontStyle = FontStyle.Bold;
	}

	// Token: 0x04002AA7 RID: 10919
	private static PrimitivesDemo instance;

	// Token: 0x04002AA8 RID: 10920
	public float animTimeMax = 5f;

	// Token: 0x04002AA9 RID: 10921
	private List<GameObject> buttons;

	// Token: 0x04002AAA RID: 10922
	private BaseObject shapeMain;

	// Token: 0x04002AAB RID: 10923
	private BaseObject shapeOld;

	// Token: 0x04002AAC RID: 10924
	private float animTimeout;

	// Token: 0x04002AAD RID: 10925
	private float[] shapeParamsStart;

	// Token: 0x04002AAE RID: 10926
	private float[] shapeParamsMax;

	// Token: 0x04002AAF RID: 10927
	private int shapeID;

	// Token: 0x04002AB0 RID: 10928
	public Transform centralPosition;

	// Token: 0x04002AB1 RID: 10929
	public Transform prevPosition;

	// Token: 0x04002AB2 RID: 10930
	public Transform nextPosition;

	// Token: 0x04002AB3 RID: 10931
	private float nextShowTimeout;

	// Token: 0x04002AB4 RID: 10932
	private float nextShowTimeoutMax = 1f;

	// Token: 0x04002AB5 RID: 10933
	private bool textureToggle;

	// Token: 0x04002AB6 RID: 10934
	private bool flatNormals;
}
