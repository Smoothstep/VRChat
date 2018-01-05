using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020006DE RID: 1758
public class OVRGridCube : MonoBehaviour
{
	// Token: 0x06003A06 RID: 14854 RVA: 0x00125011 File Offset: 0x00123411
	private void Update()
	{
		this.UpdateCubeGrid();
	}

	// Token: 0x06003A07 RID: 14855 RVA: 0x00125019 File Offset: 0x00123419
	public void SetOVRCameraController(ref OVRCameraRig cameraController)
	{
		this.CameraController = cameraController;
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x00125024 File Offset: 0x00123424
	private void UpdateCubeGrid()
	{
		if (Input.GetKeyDown(this.GridKey))
		{
			if (!this.CubeGridOn)
			{
				this.CubeGridOn = true;
				Debug.LogWarning("CubeGrid ON");
				if (this.CubeGrid != null)
				{
					this.CubeGrid.SetActive(true);
				}
				else
				{
					this.CreateCubeGrid();
				}
			}
			else
			{
				this.CubeGridOn = false;
				Debug.LogWarning("CubeGrid OFF");
				if (this.CubeGrid != null)
				{
					this.CubeGrid.SetActive(false);
				}
			}
		}
		if (this.CubeGrid != null)
		{
			this.CubeSwitchColor = !OVRManager.tracker.isPositionTracked;
			if (this.CubeSwitchColor != this.CubeSwitchColorOld)
			{
				this.CubeGridSwitchColor(this.CubeSwitchColor);
			}
			this.CubeSwitchColorOld = this.CubeSwitchColor;
		}
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x00125108 File Offset: 0x00123508
	private void CreateCubeGrid()
	{
		Debug.LogWarning("Create CubeGrid");
		this.CubeGrid = new GameObject("CubeGrid");
		this.CubeGrid.layer = this.CameraController.gameObject.layer;
		for (int i = -this.gridSizeX; i <= this.gridSizeX; i++)
		{
			for (int j = -this.gridSizeY; j <= this.gridSizeY; j++)
			{
				for (int k = -this.gridSizeZ; k <= this.gridSizeZ; k++)
				{
					int num = 0;
					if ((i == 0 && j == 0) || (i == 0 && k == 0) || (j == 0 && k == 0))
					{
						if (i == 0 && j == 0 && k == 0)
						{
							num = 2;
						}
						else
						{
							num = 1;
						}
					}
					GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
					BoxCollider component = gameObject.GetComponent<BoxCollider>();
					component.enabled = false;
					gameObject.layer = this.CameraController.gameObject.layer;
					Renderer component2 = gameObject.GetComponent<Renderer>();
					component2.shadowCastingMode = ShadowCastingMode.Off;
					component2.receiveShadows = false;
					if (num == 0)
					{
						component2.material.color = Color.red;
					}
					else if (num == 1)
					{
						component2.material.color = Color.white;
					}
					else
					{
						component2.material.color = Color.yellow;
					}
					gameObject.transform.position = new Vector3((float)i * this.gridScale, (float)j * this.gridScale, (float)k * this.gridScale);
					float num2 = 0.7f;
					if (num == 1)
					{
						num2 = 1f;
					}
					if (num == 2)
					{
						num2 = 2f;
					}
					gameObject.transform.localScale = new Vector3(this.cubeScale * num2, this.cubeScale * num2, this.cubeScale * num2);
					gameObject.transform.parent = this.CubeGrid.transform;
				}
			}
		}
	}

	// Token: 0x06003A0A RID: 14858 RVA: 0x00125308 File Offset: 0x00123708
	private void CubeGridSwitchColor(bool CubeSwitchColor)
	{
		Color color = Color.red;
		if (CubeSwitchColor)
		{
			color = Color.blue;
		}
		IEnumerator enumerator = this.CubeGrid.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				Material material = transform.GetComponent<Renderer>().material;
				if (material.color == Color.red || material.color == Color.blue)
				{
					material.color = color;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x040022E2 RID: 8930
	public KeyCode GridKey = KeyCode.G;

	// Token: 0x040022E3 RID: 8931
	private GameObject CubeGrid;

	// Token: 0x040022E4 RID: 8932
	private bool CubeGridOn;

	// Token: 0x040022E5 RID: 8933
	private bool CubeSwitchColorOld;

	// Token: 0x040022E6 RID: 8934
	private bool CubeSwitchColor;

	// Token: 0x040022E7 RID: 8935
	private int gridSizeX = 6;

	// Token: 0x040022E8 RID: 8936
	private int gridSizeY = 4;

	// Token: 0x040022E9 RID: 8937
	private int gridSizeZ = 6;

	// Token: 0x040022EA RID: 8938
	private float gridScale = 0.3f;

	// Token: 0x040022EB RID: 8939
	private float cubeScale = 0.03f;

	// Token: 0x040022EC RID: 8940
	private OVRCameraRig CameraController;
}
