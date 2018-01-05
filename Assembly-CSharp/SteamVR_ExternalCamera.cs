using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR;

// Token: 0x02000BFB RID: 3067
public class SteamVR_ExternalCamera : MonoBehaviour
{
	// Token: 0x06005F08 RID: 24328 RVA: 0x00213724 File Offset: 0x00211B24
	public void ReadConfig()
	{
		try
		{
			HmdMatrix34_t pose = default(HmdMatrix34_t);
			bool flag = false;
			object obj = this.config;
			string[] array = File.ReadAllLines(this.configPath);
			foreach (string text in array)
			{
				string[] array3 = text.Split(new char[]
				{
					'='
				});
				if (array3.Length == 2)
				{
					string text2 = array3[0];
					if (text2 == "m")
					{
						string[] array4 = array3[1].Split(new char[]
						{
							','
						});
						if (array4.Length == 12)
						{
							pose.m0 = float.Parse(array4[0]);
							pose.m1 = float.Parse(array4[1]);
							pose.m2 = float.Parse(array4[2]);
							pose.m3 = float.Parse(array4[3]);
							pose.m4 = float.Parse(array4[4]);
							pose.m5 = float.Parse(array4[5]);
							pose.m6 = float.Parse(array4[6]);
							pose.m7 = float.Parse(array4[7]);
							pose.m8 = float.Parse(array4[8]);
							pose.m9 = float.Parse(array4[9]);
							pose.m10 = float.Parse(array4[10]);
							pose.m11 = float.Parse(array4[11]);
							flag = true;
						}
					}
					else if (text2 == "disableStandardAssets")
					{
						FieldInfo field = obj.GetType().GetField(text2);
						if (field != null)
						{
							field.SetValue(obj, bool.Parse(array3[1]));
						}
					}
					else
					{
						FieldInfo field2 = obj.GetType().GetField(text2);
						if (field2 != null)
						{
							field2.SetValue(obj, float.Parse(array3[1]));
						}
					}
				}
			}
			this.config = (SteamVR_ExternalCamera.Config)obj;
			if (flag)
			{
				SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(pose);
				this.config.x = rigidTransform.pos.x;
				this.config.y = rigidTransform.pos.y;
				this.config.z = rigidTransform.pos.z;
				Vector3 eulerAngles = rigidTransform.rot.eulerAngles;
				this.config.rx = eulerAngles.x;
				this.config.ry = eulerAngles.y;
				this.config.rz = eulerAngles.z;
			}
		}
		catch
		{
		}
		this.target = null;
		if (this.watcher == null)
		{
			FileInfo fileInfo = new FileInfo(this.configPath);
			this.watcher = new FileSystemWatcher(fileInfo.DirectoryName, fileInfo.Name);
			this.watcher.NotifyFilter = NotifyFilters.LastWrite;
			this.watcher.Changed += this.OnChanged;
			this.watcher.EnableRaisingEvents = true;
		}
	}

	// Token: 0x06005F09 RID: 24329 RVA: 0x00213A38 File Offset: 0x00211E38
	private void OnChanged(object source, FileSystemEventArgs e)
	{
		this.ReadConfig();
	}

	// Token: 0x06005F0A RID: 24330 RVA: 0x00213A40 File Offset: 0x00211E40
	public void AttachToCamera(SteamVR_Camera vrcam)
	{
		if (this.target == vrcam.head)
		{
			return;
		}
		this.target = vrcam.head;
		Transform parent = base.transform.parent;
		Transform parent2 = vrcam.head.parent;
		parent.parent = parent2;
		parent.localPosition = Vector3.zero;
		parent.localRotation = Quaternion.identity;
		parent.localScale = Vector3.one;
		vrcam.enabled = false;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(vrcam.gameObject);
		vrcam.enabled = true;
		gameObject.name = "camera";
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<SteamVR_Camera>());
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<SteamVR_Fade>());
		this.cam = gameObject.GetComponent<Camera>();
		this.cam.stereoTargetEye = StereoTargetEyeMask.None;
		this.cam.fieldOfView = this.config.fov;
		this.cam.useOcclusionCulling = false;
		this.cam.enabled = false;
		this.colorMat = new Material(Shader.Find("Custom/SteamVR_ColorOut"));
		this.alphaMat = new Material(Shader.Find("Custom/SteamVR_AlphaOut"));
		this.clipMaterial = new Material(Shader.Find("Custom/SteamVR_ClearAll"));
		Transform transform = gameObject.transform;
		transform.parent = base.transform;
		transform.localPosition = new Vector3(this.config.x, this.config.y, this.config.z);
		transform.localRotation = Quaternion.Euler(this.config.rx, this.config.ry, this.config.rz);
		transform.localScale = Vector3.one;
		while (transform.childCount > 0)
		{
			UnityEngine.Object.DestroyImmediate(transform.GetChild(0).gameObject);
		}
		this.clipQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		this.clipQuad.name = "ClipQuad";
		UnityEngine.Object.DestroyImmediate(this.clipQuad.GetComponent<MeshCollider>());
		MeshRenderer component = this.clipQuad.GetComponent<MeshRenderer>();
		component.material = this.clipMaterial;
		component.shadowCastingMode = ShadowCastingMode.Off;
		component.receiveShadows = false;
		component.lightProbeUsage = LightProbeUsage.Off;
		component.reflectionProbeUsage = ReflectionProbeUsage.Off;
		Transform transform2 = this.clipQuad.transform;
		transform2.parent = transform;
		transform2.localScale = new Vector3(1000f, 1000f, 1f);
		transform2.localRotation = Quaternion.identity;
		this.clipQuad.SetActive(false);
	}

	// Token: 0x06005F0B RID: 24331 RVA: 0x00213CB8 File Offset: 0x002120B8
	public float GetTargetDistance()
	{
		if (this.target == null)
		{
			return this.config.near + 0.01f;
		}
		Transform transform = this.cam.transform;
		Vector3 vector = new Vector3(transform.forward.x, 0f, transform.forward.z);
		Vector3 normalized = vector.normalized;
		Vector3 position = this.target.position;
		Vector3 vector2 = new Vector3(this.target.forward.x, 0f, this.target.forward.z);
		Vector3 inPoint = position + vector2.normalized * this.config.hmdOffset;
		Plane plane = new Plane(normalized, inPoint);
		float value = -plane.GetDistanceToPoint(transform.position);
		return Mathf.Clamp(value, this.config.near + 0.01f, this.config.far - 0.01f);
	}

	// Token: 0x06005F0C RID: 24332 RVA: 0x00213DC8 File Offset: 0x002121C8
	public void RenderNear()
	{
		int num = Screen.width / 2;
		int num2 = Screen.height / 2;
		if (this.cam.targetTexture == null || this.cam.targetTexture.width != num || this.cam.targetTexture.height != num2)
		{
			RenderTexture renderTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32);
			renderTexture.antiAliasing = ((QualitySettings.antiAliasing != 0) ? QualitySettings.antiAliasing : 1);
			this.cam.targetTexture = renderTexture;
		}
		this.cam.nearClipPlane = this.config.near;
		this.cam.farClipPlane = this.config.far;
		CameraClearFlags clearFlags = this.cam.clearFlags;
		Color backgroundColor = this.cam.backgroundColor;
		this.cam.clearFlags = CameraClearFlags.Color;
		this.cam.backgroundColor = Color.clear;
		this.clipMaterial.color = new Color(this.config.r, this.config.g, this.config.b, this.config.a);
		float d = Mathf.Clamp(this.GetTargetDistance() + this.config.nearOffset, this.config.near, this.config.far);
		Transform parent = this.clipQuad.transform.parent;
		this.clipQuad.transform.position = parent.position + parent.forward * d;
		MonoBehaviour[] array = null;
		bool[] array2 = null;
		if (this.config.disableStandardAssets)
		{
			array = this.cam.gameObject.GetComponents<MonoBehaviour>();
			array2 = new bool[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				MonoBehaviour monoBehaviour = array[i];
				if (monoBehaviour.enabled && monoBehaviour.GetType().ToString().StartsWith("UnityStandardAssets."))
				{
					monoBehaviour.enabled = false;
					array2[i] = true;
				}
			}
		}
		this.clipQuad.SetActive(true);
		this.cam.Render();
		Graphics.DrawTexture(new Rect(0f, 0f, (float)num, (float)num2), this.cam.targetTexture, this.colorMat);
		MonoBehaviour monoBehaviour2 = this.cam.gameObject.GetComponent("PostProcessingBehaviour") as MonoBehaviour;
		if (monoBehaviour2 != null && monoBehaviour2.enabled)
		{
			monoBehaviour2.enabled = false;
			this.cam.Render();
			monoBehaviour2.enabled = true;
		}
		Graphics.DrawTexture(new Rect((float)num, 0f, (float)num, (float)num2), this.cam.targetTexture, this.alphaMat);
		this.clipQuad.SetActive(false);
		if (array != null)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array2[j])
				{
					array[j].enabled = true;
				}
			}
		}
		this.cam.clearFlags = clearFlags;
		this.cam.backgroundColor = backgroundColor;
	}

	// Token: 0x06005F0D RID: 24333 RVA: 0x002140FC File Offset: 0x002124FC
	public void RenderFar()
	{
		this.cam.nearClipPlane = this.config.near;
		this.cam.farClipPlane = this.config.far;
		this.cam.Render();
		int num = Screen.width / 2;
		int num2 = Screen.height / 2;
		Graphics.DrawTexture(new Rect(0f, (float)num2, (float)num, (float)num2), this.cam.targetTexture, this.colorMat);
	}

	// Token: 0x06005F0E RID: 24334 RVA: 0x00214176 File Offset: 0x00212576
	private void OnGUI()
	{
	}

	// Token: 0x06005F0F RID: 24335 RVA: 0x00214178 File Offset: 0x00212578
	private void OnEnable()
	{
		this.cameras = UnityEngine.Object.FindObjectsOfType<Camera>();
		if (this.cameras != null)
		{
			int num = this.cameras.Length;
			this.cameraRects = new Rect[num];
			for (int i = 0; i < num; i++)
			{
				Camera camera = this.cameras[i];
				this.cameraRects[i] = camera.rect;
				if (!(camera == this.cam))
				{
					if (!(camera.targetTexture != null))
					{
						if (!(camera.GetComponent<SteamVR_Camera>() != null))
						{
							camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
						}
					}
				}
			}
		}
		if (this.config.sceneResolutionScale > 0f)
		{
			this.sceneResolutionScale = SteamVR_Camera.sceneResolutionScale;
			SteamVR_Camera.sceneResolutionScale = this.config.sceneResolutionScale;
		}
	}

	// Token: 0x06005F10 RID: 24336 RVA: 0x00214274 File Offset: 0x00212674
	private void OnDisable()
	{
		if (this.cameras != null)
		{
			int num = this.cameras.Length;
			for (int i = 0; i < num; i++)
			{
				Camera camera = this.cameras[i];
				if (camera != null)
				{
					camera.rect = this.cameraRects[i];
				}
			}
			this.cameras = null;
			this.cameraRects = null;
		}
		if (this.config.sceneResolutionScale > 0f)
		{
			SteamVR_Camera.sceneResolutionScale = this.sceneResolutionScale;
		}
	}

	// Token: 0x04004492 RID: 17554
	public SteamVR_ExternalCamera.Config config;

	// Token: 0x04004493 RID: 17555
	public string configPath;

	// Token: 0x04004494 RID: 17556
	private FileSystemWatcher watcher;

	// Token: 0x04004495 RID: 17557
	private Camera cam;

	// Token: 0x04004496 RID: 17558
	private Transform target;

	// Token: 0x04004497 RID: 17559
	private GameObject clipQuad;

	// Token: 0x04004498 RID: 17560
	private Material clipMaterial;

	// Token: 0x04004499 RID: 17561
	private Material colorMat;

	// Token: 0x0400449A RID: 17562
	private Material alphaMat;

	// Token: 0x0400449B RID: 17563
	private Camera[] cameras;

	// Token: 0x0400449C RID: 17564
	private Rect[] cameraRects;

	// Token: 0x0400449D RID: 17565
	private float sceneResolutionScale;

	// Token: 0x02000BFC RID: 3068
	[Serializable]
	public struct Config
	{
		// Token: 0x0400449E RID: 17566
		public float x;

		// Token: 0x0400449F RID: 17567
		public float y;

		// Token: 0x040044A0 RID: 17568
		public float z;

		// Token: 0x040044A1 RID: 17569
		public float rx;

		// Token: 0x040044A2 RID: 17570
		public float ry;

		// Token: 0x040044A3 RID: 17571
		public float rz;

		// Token: 0x040044A4 RID: 17572
		public float fov;

		// Token: 0x040044A5 RID: 17573
		public float near;

		// Token: 0x040044A6 RID: 17574
		public float far;

		// Token: 0x040044A7 RID: 17575
		public float sceneResolutionScale;

		// Token: 0x040044A8 RID: 17576
		public float frameSkip;

		// Token: 0x040044A9 RID: 17577
		public float nearOffset;

		// Token: 0x040044AA RID: 17578
		public float farOffset;

		// Token: 0x040044AB RID: 17579
		public float hmdOffset;

		// Token: 0x040044AC RID: 17580
		public float r;

		// Token: 0x040044AD RID: 17581
		public float g;

		// Token: 0x040044AE RID: 17582
		public float b;

		// Token: 0x040044AF RID: 17583
		public float a;

		// Token: 0x040044B0 RID: 17584
		public bool disableStandardAssets;
	}
}
