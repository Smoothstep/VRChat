using System;
using UnityEngine;
using Windows.Kinect;

// Token: 0x020004AE RID: 1198
public class DepthSourceView : MonoBehaviour
{
	// Token: 0x060029FF RID: 10751 RVA: 0x000D5E58 File Offset: 0x000D4258
	private void Start()
	{
		this._Sensor = KinectSensor.GetDefault();
		if (this._Sensor != null)
		{
			this._Mapper = this._Sensor.CoordinateMapper;
			FrameDescription frameDescription = this._Sensor.DepthFrameSource.FrameDescription;
			this.CreateMesh(frameDescription.Width / 4, frameDescription.Height / 4);
			if (!this._Sensor.IsOpen)
			{
				this._Sensor.Open();
			}
		}
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x000D5ED0 File Offset: 0x000D42D0
	private void CreateMesh(int width, int height)
	{
		this._Mesh = new Mesh();
		base.GetComponent<MeshFilter>().mesh = this._Mesh;
		this._Vertices = new Vector3[width * height];
		this._UV = new Vector2[width * height];
		this._Triangles = new int[6 * ((width - 1) * (height - 1))];
		int num = 0;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				int num2 = i * width + j;
				this._Vertices[num2] = new Vector3((float)j, (float)(-(float)i), 0f);
				this._UV[num2] = new Vector2((float)j / (float)width, (float)i / (float)height);
				if (j != width - 1 && i != height - 1)
				{
					int num3 = num2;
					int num4 = num3 + 1;
					int num5 = num3 + width;
					int num6 = num5 + 1;
					this._Triangles[num++] = num3;
					this._Triangles[num++] = num4;
					this._Triangles[num++] = num5;
					this._Triangles[num++] = num5;
					this._Triangles[num++] = num4;
					this._Triangles[num++] = num6;
				}
			}
		}
		this._Mesh.vertices = this._Vertices;
		this._Mesh.uv = this._UV;
		this._Mesh.triangles = this._Triangles;
		this._Mesh.RecalculateNormals();
	}

	// Token: 0x06002A01 RID: 10753 RVA: 0x000D6050 File Offset: 0x000D4450
	private void OnGUI()
	{
		GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		GUI.TextField(new Rect((float)(Screen.width - 250), 10f, 250f, 20f), "DepthMode: " + this.ViewMode.ToString());
		GUI.EndGroup();
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x000D60C4 File Offset: 0x000D44C4
	private void Update()
	{
		if (this._Sensor == null)
		{
			return;
		}
		if (Input.GetButtonDown("Fire1"))
		{
			if (this.ViewMode == DepthViewMode.MultiSourceReader)
			{
				this.ViewMode = DepthViewMode.SeparateSourceReaders;
			}
			else
			{
				this.ViewMode = DepthViewMode.MultiSourceReader;
			}
		}
		float axis = Input.GetAxis("Horizontal");
		float num = -Input.GetAxis("Vertical");
		base.transform.Rotate(num * Time.deltaTime * 50f, axis * Time.deltaTime * 50f, 0f, Space.Self);
		if (this.ViewMode == DepthViewMode.SeparateSourceReaders)
		{
			if (this.ColorSourceManager == null)
			{
				return;
			}
			this._ColorManager = this.ColorSourceManager.GetComponent<ColorSourceManager>();
			if (this._ColorManager == null)
			{
				return;
			}
			if (this.DepthSourceManager == null)
			{
				return;
			}
			this._DepthManager = this.DepthSourceManager.GetComponent<DepthSourceManager>();
			if (this._DepthManager == null)
			{
				return;
			}
			base.gameObject.GetComponent<Renderer>().material.mainTexture = this._ColorManager.GetColorTexture();
			this.RefreshData(this._DepthManager.GetData(), this._ColorManager.ColorWidth, this._ColorManager.ColorHeight);
		}
		else
		{
			if (this.MultiSourceManager == null)
			{
				return;
			}
			this._MultiManager = this.MultiSourceManager.GetComponent<MultiSourceManager>();
			if (this._MultiManager == null)
			{
				return;
			}
			base.gameObject.GetComponent<Renderer>().material.mainTexture = this._MultiManager.GetColorTexture();
			this.RefreshData(this._MultiManager.GetDepthData(), this._MultiManager.ColorWidth, this._MultiManager.ColorHeight);
		}
	}

	// Token: 0x06002A03 RID: 10755 RVA: 0x000D628C File Offset: 0x000D468C
	private void RefreshData(ushort[] depthData, int colorWidth, int colorHeight)
	{
		FrameDescription frameDescription = this._Sensor.DepthFrameSource.FrameDescription;
		ColorSpacePoint[] array = new ColorSpacePoint[depthData.Length];
		this._Mapper.MapDepthFrameToColorSpace(depthData, array);
		for (int i = 0; i < frameDescription.Height; i += 4)
		{
			for (int j = 0; j < frameDescription.Width; j += 4)
			{
				int num = j / 4;
				int num2 = i / 4;
				int num3 = num2 * (frameDescription.Width / 4) + num;
				double num4 = this.GetAvg(depthData, j, i, frameDescription.Width, frameDescription.Height);
				num4 *= 0.10000000149011612;
				this._Vertices[num3].z = (float)num4;
				ColorSpacePoint colorSpacePoint = array[i * frameDescription.Width + j];
				this._UV[num3] = new Vector2(colorSpacePoint.X / (float)colorWidth, colorSpacePoint.Y / (float)colorHeight);
			}
		}
		this._Mesh.vertices = this._Vertices;
		this._Mesh.uv = this._UV;
		this._Mesh.triangles = this._Triangles;
		this._Mesh.RecalculateNormals();
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x000D63C4 File Offset: 0x000D47C4
	private double GetAvg(ushort[] depthData, int x, int y, int width, int height)
	{
		double num = 0.0;
		for (int i = y; i < y + 4; i++)
		{
			for (int j = x; j < x + 4; j++)
			{
				int num2 = i * width + j;
				if (depthData[num2] == 0)
				{
					num += 4500.0;
				}
				else
				{
					num += (double)depthData[num2];
				}
			}
		}
		return num / 16.0;
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x000D6438 File Offset: 0x000D4838
	private void OnApplicationQuit()
	{
		if (this._Mapper != null)
		{
			this._Mapper = null;
		}
		if (this._Sensor != null)
		{
			if (this._Sensor.IsOpen)
			{
				this._Sensor.Close();
			}
			this._Sensor = null;
		}
	}

	// Token: 0x040016E3 RID: 5859
	public DepthViewMode ViewMode;

	// Token: 0x040016E4 RID: 5860
	public GameObject ColorSourceManager;

	// Token: 0x040016E5 RID: 5861
	public GameObject DepthSourceManager;

	// Token: 0x040016E6 RID: 5862
	public GameObject MultiSourceManager;

	// Token: 0x040016E7 RID: 5863
	private KinectSensor _Sensor;

	// Token: 0x040016E8 RID: 5864
	private CoordinateMapper _Mapper;

	// Token: 0x040016E9 RID: 5865
	private Mesh _Mesh;

	// Token: 0x040016EA RID: 5866
	private Vector3[] _Vertices;

	// Token: 0x040016EB RID: 5867
	private Vector2[] _UV;

	// Token: 0x040016EC RID: 5868
	private int[] _Triangles;

	// Token: 0x040016ED RID: 5869
	private const int _DownsampleSize = 4;

	// Token: 0x040016EE RID: 5870
	private const double _DepthScale = 0.10000000149011612;

	// Token: 0x040016EF RID: 5871
	private const int _Speed = 50;

	// Token: 0x040016F0 RID: 5872
	private MultiSourceManager _MultiManager;

	// Token: 0x040016F1 RID: 5873
	private ColorSourceManager _ColorManager;

	// Token: 0x040016F2 RID: 5874
	private DepthSourceManager _DepthManager;
}
