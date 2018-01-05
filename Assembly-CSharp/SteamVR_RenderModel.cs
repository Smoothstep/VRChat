using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Valve.VR;

// Token: 0x02000C08 RID: 3080
[ExecuteInEditMode]
public class SteamVR_RenderModel : MonoBehaviour
{
	// Token: 0x06005F67 RID: 24423 RVA: 0x0021840C File Offset: 0x0021680C
	private SteamVR_RenderModel()
	{
		this.deviceConnectedAction = SteamVR_Events.DeviceConnectedAction(new UnityAction<int, bool>(this.OnDeviceConnected));
		this.hideRenderModelsAction = SteamVR_Events.HideRenderModelsAction(new UnityAction<bool>(this.OnHideRenderModels));
		this.modelSkinSettingsHaveChangedAction = SteamVR_Events.SystemAction(EVREventType.VREvent_ModelSkinSettingsHaveChanged, new UnityAction<VREvent_t>(this.OnModelSkinSettingsHaveChanged));
	}

	// Token: 0x17000D7D RID: 3453
	// (get) Token: 0x06005F68 RID: 24424 RVA: 0x0021847E File Offset: 0x0021687E
	// (set) Token: 0x06005F69 RID: 24425 RVA: 0x00218486 File Offset: 0x00216886
	public string renderModelName { get; private set; }

	// Token: 0x06005F6A RID: 24426 RVA: 0x0021848F File Offset: 0x0021688F
	private void OnModelSkinSettingsHaveChanged(VREvent_t vrEvent)
	{
		if (!string.IsNullOrEmpty(this.renderModelName))
		{
			this.renderModelName = string.Empty;
			this.UpdateModel();
		}
	}

	// Token: 0x06005F6B RID: 24427 RVA: 0x002184B4 File Offset: 0x002168B4
	private void OnHideRenderModels(bool hidden)
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component != null)
		{
			component.enabled = !hidden;
		}
		foreach (MeshRenderer meshRenderer in base.transform.GetComponentsInChildren<MeshRenderer>())
		{
			meshRenderer.enabled = !hidden;
		}
	}

	// Token: 0x06005F6C RID: 24428 RVA: 0x0021850C File Offset: 0x0021690C
	private void OnDeviceConnected(int i, bool connected)
	{
		if (i != (int)this.index)
		{
			return;
		}
		if (connected)
		{
			this.UpdateModel();
		}
	}

	// Token: 0x06005F6D RID: 24429 RVA: 0x00218528 File Offset: 0x00216928
	public void UpdateModel()
	{
		CVRSystem system = OpenVR.System;
		if (system == null)
		{
			return;
		}
		ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
		uint stringTrackedDeviceProperty = system.GetStringTrackedDeviceProperty((uint)this.index, ETrackedDeviceProperty.Prop_RenderModelName_String, null, 0u, ref etrackedPropertyError);
		if (stringTrackedDeviceProperty <= 1u)
		{
			Debug.LogError("Failed to get render model name for tracked object " + this.index);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder((int)stringTrackedDeviceProperty);
		system.GetStringTrackedDeviceProperty((uint)this.index, ETrackedDeviceProperty.Prop_RenderModelName_String, stringBuilder, stringTrackedDeviceProperty, ref etrackedPropertyError);
		string text = stringBuilder.ToString();
		if (this.renderModelName != text)
		{
			this.renderModelName = text;
			base.StartCoroutine(this.SetModelAsync(text));
		}
	}

	// Token: 0x06005F6E RID: 24430 RVA: 0x002185CC File Offset: 0x002169CC
	private IEnumerator SetModelAsync(string renderModelName)
	{
		if (string.IsNullOrEmpty(renderModelName))
		{
			yield break;
		}
		using (SteamVR_RenderModel.RenderModelInterfaceHolder holder = new SteamVR_RenderModel.RenderModelInterfaceHolder())
		{
			CVRRenderModels renderModels = holder.instance;
			if (renderModels == null)
			{
				yield break;
			}
			uint count = renderModels.GetComponentCount(renderModelName);
			string[] renderModelNames;
			if (count > 0u)
			{
				renderModelNames = new string[count];
				int num = 0;
				while ((long)num < (long)((ulong)count))
				{
					uint num2 = renderModels.GetComponentName(renderModelName, (uint)num, null, 0u);
					if (num2 != 0u)
					{
						StringBuilder stringBuilder = new StringBuilder((int)num2);
						if (renderModels.GetComponentName(renderModelName, (uint)num, stringBuilder, num2) != 0u)
						{
							num2 = renderModels.GetComponentRenderModelName(renderModelName, stringBuilder.ToString(), null, 0u);
							if (num2 != 0u)
							{
								StringBuilder stringBuilder2 = new StringBuilder((int)num2);
								if (renderModels.GetComponentRenderModelName(renderModelName, stringBuilder.ToString(), stringBuilder2, num2) != 0u)
								{
									string text = stringBuilder2.ToString();
									SteamVR_RenderModel.RenderModel renderModel = SteamVR_RenderModel.models[text] as SteamVR_RenderModel.RenderModel;
									if (renderModel == null || renderModel.mesh == null)
									{
										renderModelNames[num] = text;
									}
								}
							}
						}
					}
					num++;
				}
			}
			else
			{
				SteamVR_RenderModel.RenderModel renderModel2 = SteamVR_RenderModel.models[renderModelName] as SteamVR_RenderModel.RenderModel;
				if (renderModel2 == null || renderModel2.mesh == null)
				{
					renderModelNames = new string[]
					{
						renderModelName
					};
				}
				else
				{
					renderModelNames = new string[0];
				}
			}
			for (;;)
			{
				bool loading = false;
				foreach (string text2 in renderModelNames)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						IntPtr zero = IntPtr.Zero;
						EVRRenderModelError evrrenderModelError = renderModels.LoadRenderModel_Async(text2, ref zero);
						if (evrrenderModelError == EVRRenderModelError.Loading)
						{
							loading = true;
						}
						else if (evrrenderModelError == EVRRenderModelError.None)
						{
							RenderModel_t renderModel_t = this.MarshalRenderModel(zero);
							Material material = SteamVR_RenderModel.materials[renderModel_t.diffuseTextureId] as Material;
							if (material == null || material.mainTexture == null)
							{
								IntPtr zero2 = IntPtr.Zero;
								evrrenderModelError = renderModels.LoadTexture_Async(renderModel_t.diffuseTextureId, ref zero2);
								if (evrrenderModelError == EVRRenderModelError.Loading)
								{
									loading = true;
								}
							}
						}
					}
				}
				if (!loading)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
		}
		bool success = this.SetModel(renderModelName);
		SteamVR_Events.RenderModelLoaded.Send(this, success);
		yield break;
	}

	// Token: 0x06005F6F RID: 24431 RVA: 0x002185F0 File Offset: 0x002169F0
	private bool SetModel(string renderModelName)
	{
		this.StripMesh(base.gameObject);
		using (SteamVR_RenderModel.RenderModelInterfaceHolder renderModelInterfaceHolder = new SteamVR_RenderModel.RenderModelInterfaceHolder())
		{
			if (this.createComponents)
			{
				if (this.LoadComponents(renderModelInterfaceHolder, renderModelName))
				{
					this.UpdateComponents(renderModelInterfaceHolder.instance);
					return true;
				}
				Debug.Log("[" + base.gameObject.name + "] Render model does not support components, falling back to single mesh.");
			}
			if (!string.IsNullOrEmpty(renderModelName))
			{
				SteamVR_RenderModel.RenderModel renderModel = SteamVR_RenderModel.models[renderModelName] as SteamVR_RenderModel.RenderModel;
				if (renderModel == null || renderModel.mesh == null)
				{
					CVRRenderModels instance = renderModelInterfaceHolder.instance;
					if (instance == null)
					{
						return false;
					}
					if (this.verbose)
					{
						Debug.Log("Loading render model " + renderModelName);
					}
					renderModel = this.LoadRenderModel(instance, renderModelName, renderModelName);
					if (renderModel == null)
					{
						return false;
					}
					SteamVR_RenderModel.models[renderModelName] = renderModel;
				}
				base.gameObject.AddComponent<MeshFilter>().mesh = renderModel.mesh;
				base.gameObject.AddComponent<MeshRenderer>().sharedMaterial = renderModel.material;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005F70 RID: 24432 RVA: 0x00218744 File Offset: 0x00216B44
	private SteamVR_RenderModel.RenderModel LoadRenderModel(CVRRenderModels renderModels, string renderModelName, string baseName)
	{
		IntPtr zero = IntPtr.Zero;
		EVRRenderModelError evrrenderModelError;
		for (;;)
		{
			evrrenderModelError = renderModels.LoadRenderModel_Async(renderModelName, ref zero);
			if (evrrenderModelError != EVRRenderModelError.Loading)
			{
				break;
			}
			SteamVR_RenderModel.Sleep();
		}
		if (evrrenderModelError != EVRRenderModelError.None)
		{
			Debug.LogError(string.Format("Failed to load render model {0} - {1}", renderModelName, evrrenderModelError.ToString()));
			return null;
		}
		RenderModel_t renderModel_t = this.MarshalRenderModel(zero);
		Vector3[] array = new Vector3[renderModel_t.unVertexCount];
		Vector3[] array2 = new Vector3[renderModel_t.unVertexCount];
		Vector2[] array3 = new Vector2[renderModel_t.unVertexCount];
		Type typeFromHandle = typeof(RenderModel_Vertex_t);
		int num = 0;
		while ((long)num < (long)((ulong)renderModel_t.unVertexCount))
		{
			IntPtr ptr = new IntPtr(renderModel_t.rVertexData.ToInt64() + (long)(num * Marshal.SizeOf(typeFromHandle)));
			RenderModel_Vertex_t renderModel_Vertex_t = (RenderModel_Vertex_t)Marshal.PtrToStructure(ptr, typeFromHandle);
			array[num] = new Vector3(renderModel_Vertex_t.vPosition.v0, renderModel_Vertex_t.vPosition.v1, -renderModel_Vertex_t.vPosition.v2);
			array2[num] = new Vector3(renderModel_Vertex_t.vNormal.v0, renderModel_Vertex_t.vNormal.v1, -renderModel_Vertex_t.vNormal.v2);
			array3[num] = new Vector2(renderModel_Vertex_t.rfTextureCoord0, renderModel_Vertex_t.rfTextureCoord1);
			num++;
		}
		int num2 = (int)(renderModel_t.unTriangleCount * 3u);
		short[] array4 = new short[num2];
		Marshal.Copy(renderModel_t.rIndexData, array4, 0, array4.Length);
		int[] array5 = new int[num2];
		int num3 = 0;
		while ((long)num3 < (long)((ulong)renderModel_t.unTriangleCount))
		{
			array5[num3 * 3] = (int)array4[num3 * 3 + 2];
			array5[num3 * 3 + 1] = (int)array4[num3 * 3 + 1];
			array5[num3 * 3 + 2] = (int)array4[num3 * 3];
			num3++;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.uv = array3;
		mesh.triangles = array5;
		Material material = SteamVR_RenderModel.materials[renderModel_t.diffuseTextureId] as Material;
		if (material == null || material.mainTexture == null)
		{
			IntPtr zero2 = IntPtr.Zero;
			for (;;)
			{
				evrrenderModelError = renderModels.LoadTexture_Async(renderModel_t.diffuseTextureId, ref zero2);
				if (evrrenderModelError != EVRRenderModelError.Loading)
				{
					break;
				}
				SteamVR_RenderModel.Sleep();
			}
			if (evrrenderModelError == EVRRenderModelError.None)
			{
				RenderModel_TextureMap_t renderModel_TextureMap_t = this.MarshalRenderModel_TextureMap(zero2);
				Texture2D texture2D = new Texture2D((int)renderModel_TextureMap_t.unWidth, (int)renderModel_TextureMap_t.unHeight, TextureFormat.ARGB32, false);
				if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11)
				{
					texture2D.Apply();
					for (;;)
					{
						evrrenderModelError = renderModels.LoadIntoTextureD3D11_Async(renderModel_t.diffuseTextureId, texture2D.GetNativeTexturePtr());
						if (evrrenderModelError != EVRRenderModelError.Loading)
						{
							break;
						}
						SteamVR_RenderModel.Sleep();
					}
				}
				else
				{
					byte[] array6 = new byte[(int)(renderModel_TextureMap_t.unWidth * renderModel_TextureMap_t.unHeight * '\u0004')];
					Marshal.Copy(renderModel_TextureMap_t.rubTextureMapData, array6, 0, array6.Length);
					Color32[] array7 = new Color32[(int)(renderModel_TextureMap_t.unWidth * renderModel_TextureMap_t.unHeight)];
					int num4 = 0;
					for (int i = 0; i < (int)renderModel_TextureMap_t.unHeight; i++)
					{
						for (int j = 0; j < (int)renderModel_TextureMap_t.unWidth; j++)
						{
							byte r = array6[num4++];
							byte g = array6[num4++];
							byte b = array6[num4++];
							byte a = array6[num4++];
							array7[i * (int)renderModel_TextureMap_t.unWidth + j] = new Color32(r, g, b, a);
						}
					}
					texture2D.SetPixels32(array7);
					texture2D.Apply();
				}
				material = new Material((!(this.shader != null)) ? Shader.Find("Standard") : this.shader);
				material.mainTexture = texture2D;
				SteamVR_RenderModel.materials[renderModel_t.diffuseTextureId] = material;
				renderModels.FreeTexture(zero2);
			}
			else
			{
				Debug.Log("Failed to load render model texture for render model " + renderModelName);
			}
		}
		base.StartCoroutine(this.FreeRenderModel(zero));
		return new SteamVR_RenderModel.RenderModel(mesh, material);
	}

	// Token: 0x06005F71 RID: 24433 RVA: 0x00218B9C File Offset: 0x00216F9C
	private IEnumerator FreeRenderModel(IntPtr pRenderModel)
	{
		yield return new WaitForSeconds(1f);
		using (SteamVR_RenderModel.RenderModelInterfaceHolder renderModelInterfaceHolder = new SteamVR_RenderModel.RenderModelInterfaceHolder())
		{
			CVRRenderModels instance = renderModelInterfaceHolder.instance;
			instance.FreeRenderModel(pRenderModel);
		}
		yield break;
	}

	// Token: 0x06005F72 RID: 24434 RVA: 0x00218BB8 File Offset: 0x00216FB8
	public Transform FindComponent(string componentName)
	{
		Transform transform = base.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.name == componentName)
			{
				return child;
			}
		}
		return null;
	}

	// Token: 0x06005F73 RID: 24435 RVA: 0x00218C00 File Offset: 0x00217000
	private void StripMesh(GameObject go)
	{
		MeshRenderer component = go.GetComponent<MeshRenderer>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(component);
		}
		MeshFilter component2 = go.GetComponent<MeshFilter>();
		if (component2 != null)
		{
			UnityEngine.Object.DestroyImmediate(component2);
		}
	}

	// Token: 0x06005F74 RID: 24436 RVA: 0x00218C40 File Offset: 0x00217040
	private bool LoadComponents(SteamVR_RenderModel.RenderModelInterfaceHolder holder, string renderModelName)
	{
		Transform transform = base.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			child.gameObject.SetActive(false);
			this.StripMesh(child.gameObject);
		}
		if (string.IsNullOrEmpty(renderModelName))
		{
			return true;
		}
		CVRRenderModels instance = holder.instance;
		if (instance == null)
		{
			return false;
		}
		uint componentCount = instance.GetComponentCount(renderModelName);
		if (componentCount == 0u)
		{
			return false;
		}
		int num = 0;
		while ((long)num < (long)((ulong)componentCount))
		{
			uint num2 = instance.GetComponentName(renderModelName, (uint)num, null, 0u);
			if (num2 != 0u)
			{
				StringBuilder stringBuilder = new StringBuilder((int)num2);
				if (instance.GetComponentName(renderModelName, (uint)num, stringBuilder, num2) != 0u)
				{
					transform = this.FindComponent(stringBuilder.ToString());
					if (transform != null)
					{
						transform.gameObject.SetActive(true);
					}
					else
					{
						transform = new GameObject(stringBuilder.ToString()).transform;
						transform.parent = base.transform;
						transform.gameObject.layer = base.gameObject.layer;
						Transform transform2 = new GameObject("attach").transform;
						transform2.parent = transform;
						transform2.localPosition = Vector3.zero;
						transform2.localRotation = Quaternion.identity;
						transform2.localScale = Vector3.one;
						transform2.gameObject.layer = base.gameObject.layer;
					}
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;
					transform.localScale = Vector3.one;
					num2 = instance.GetComponentRenderModelName(renderModelName, stringBuilder.ToString(), null, 0u);
					if (num2 != 0u)
					{
						StringBuilder stringBuilder2 = new StringBuilder((int)num2);
						if (instance.GetComponentRenderModelName(renderModelName, stringBuilder.ToString(), stringBuilder2, num2) != 0u)
						{
							SteamVR_RenderModel.RenderModel renderModel = SteamVR_RenderModel.models[stringBuilder2] as SteamVR_RenderModel.RenderModel;
							if (renderModel == null || renderModel.mesh == null)
							{
								if (this.verbose)
								{
									Debug.Log("Loading render model " + stringBuilder2);
								}
								renderModel = this.LoadRenderModel(instance, stringBuilder2.ToString(), renderModelName);
								if (renderModel == null)
								{
									goto IL_265;
								}
								SteamVR_RenderModel.models[stringBuilder2] = renderModel;
							}
							transform.gameObject.AddComponent<MeshFilter>().mesh = renderModel.mesh;
							transform.gameObject.AddComponent<MeshRenderer>().sharedMaterial = renderModel.material;
						}
					}
				}
			}
			IL_265:
			num++;
		}
		return true;
	}

	// Token: 0x06005F75 RID: 24437 RVA: 0x00218EC4 File Offset: 0x002172C4
	private void OnEnable()
	{
		if (!string.IsNullOrEmpty(this.modelOverride))
		{
			Debug.Log("Model override is really only meant to be used in the scene view for lining things up; using it at runtime is discouraged.  Use tracked device index instead to ensure the correct model is displayed for all users.");
			base.enabled = false;
			return;
		}
		CVRSystem system = OpenVR.System;
		if (system != null && system.IsTrackedDeviceConnected((uint)this.index))
		{
			this.UpdateModel();
		}
		this.deviceConnectedAction.enabled = true;
		this.hideRenderModelsAction.enabled = true;
		this.modelSkinSettingsHaveChangedAction.enabled = true;
	}

	// Token: 0x06005F76 RID: 24438 RVA: 0x00218F3A File Offset: 0x0021733A
	private void OnDisable()
	{
		this.deviceConnectedAction.enabled = false;
		this.hideRenderModelsAction.enabled = false;
		this.modelSkinSettingsHaveChangedAction.enabled = false;
	}

	// Token: 0x06005F77 RID: 24439 RVA: 0x00218F60 File Offset: 0x00217360
	private void Update()
	{
		if (this.updateDynamically)
		{
			this.UpdateComponents(OpenVR.RenderModels);
		}
	}

	// Token: 0x06005F78 RID: 24440 RVA: 0x00218F78 File Offset: 0x00217378
	public void UpdateComponents(CVRRenderModels renderModels)
	{
		if (renderModels == null)
		{
			return;
		}
		Transform transform = base.transform;
		if (transform.childCount == 0)
		{
			return;
		}
		VRControllerState_t vrcontrollerState_t = (this.index == SteamVR_TrackedObject.EIndex.None) ? default(VRControllerState_t) : SteamVR_Controller.Input((int)this.index).GetState();
		if (this.nameCache == null)
		{
			this.nameCache = new Dictionary<int, string>();
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			string name;
			if (!this.nameCache.TryGetValue(child.GetInstanceID(), out name))
			{
				name = child.name;
				this.nameCache.Add(child.GetInstanceID(), name);
			}
			RenderModel_ComponentState_t renderModel_ComponentState_t = default(RenderModel_ComponentState_t);
			if (renderModels.GetComponentState(this.renderModelName, name, ref vrcontrollerState_t, ref this.controllerModeState, ref renderModel_ComponentState_t))
			{
				SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(renderModel_ComponentState_t.mTrackingToComponentRenderModel);
				child.localPosition = rigidTransform.pos;
				child.localRotation = rigidTransform.rot;
				Transform transform2 = child.Find("attach");
				if (transform2 != null)
				{
					SteamVR_Utils.RigidTransform rigidTransform2 = new SteamVR_Utils.RigidTransform(renderModel_ComponentState_t.mTrackingToComponentLocal);
					transform2.position = transform.TransformPoint(rigidTransform2.pos);
					transform2.rotation = transform.rotation * rigidTransform2.rot;
				}
				bool flag = (renderModel_ComponentState_t.uProperties & 2u) != 0u;
				if (flag != child.gameObject.activeSelf)
				{
					child.gameObject.SetActive(flag);
				}
			}
		}
	}

	// Token: 0x06005F79 RID: 24441 RVA: 0x00219112 File Offset: 0x00217512
	public void SetDeviceIndex(int index)
	{
		this.index = (SteamVR_TrackedObject.EIndex)index;
		this.modelOverride = string.Empty;
		if (base.enabled)
		{
			this.UpdateModel();
		}
	}

	// Token: 0x06005F7A RID: 24442 RVA: 0x00219137 File Offset: 0x00217537
	private static void Sleep()
	{
		Thread.Sleep(1);
	}

	// Token: 0x06005F7B RID: 24443 RVA: 0x00219140 File Offset: 0x00217540
	private RenderModel_t MarshalRenderModel(IntPtr pRenderModel)
	{
		if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
		{
			RenderModel_t_Packed renderModel_t_Packed = (RenderModel_t_Packed)Marshal.PtrToStructure(pRenderModel, typeof(RenderModel_t_Packed));
			RenderModel_t result = default(RenderModel_t);
			renderModel_t_Packed.Unpack(ref result);
			return result;
		}
		return (RenderModel_t)Marshal.PtrToStructure(pRenderModel, typeof(RenderModel_t));
	}

	// Token: 0x06005F7C RID: 24444 RVA: 0x002191AC File Offset: 0x002175AC
	private RenderModel_TextureMap_t MarshalRenderModel_TextureMap(IntPtr pRenderModel)
	{
		if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
		{
			RenderModel_TextureMap_t_Packed renderModel_TextureMap_t_Packed = (RenderModel_TextureMap_t_Packed)Marshal.PtrToStructure(pRenderModel, typeof(RenderModel_TextureMap_t_Packed));
			RenderModel_TextureMap_t result = default(RenderModel_TextureMap_t);
			renderModel_TextureMap_t_Packed.Unpack(ref result);
			return result;
		}
		return (RenderModel_TextureMap_t)Marshal.PtrToStructure(pRenderModel, typeof(RenderModel_TextureMap_t));
	}

	// Token: 0x04004528 RID: 17704
	public SteamVR_TrackedObject.EIndex index = SteamVR_TrackedObject.EIndex.None;

	// Token: 0x04004529 RID: 17705
	public const string modelOverrideWarning = "Model override is really only meant to be used in the scene view for lining things up; using it at runtime is discouraged.  Use tracked device index instead to ensure the correct model is displayed for all users.";

	// Token: 0x0400452A RID: 17706
	[Tooltip("Model override is really only meant to be used in the scene view for lining things up; using it at runtime is discouraged.  Use tracked device index instead to ensure the correct model is displayed for all users.")]
	public string modelOverride;

	// Token: 0x0400452B RID: 17707
	[Tooltip("Shader to apply to model.")]
	public Shader shader;

	// Token: 0x0400452C RID: 17708
	[Tooltip("Enable to print out when render models are loaded.")]
	public bool verbose;

	// Token: 0x0400452D RID: 17709
	[Tooltip("If available, break down into separate components instead of loading as a single mesh.")]
	public bool createComponents = true;

	// Token: 0x0400452E RID: 17710
	[Tooltip("Update transforms of components at runtime to reflect user action.")]
	public bool updateDynamically = true;

	// Token: 0x0400452F RID: 17711
	public RenderModel_ControllerMode_State_t controllerModeState;

	// Token: 0x04004530 RID: 17712
	public const string k_localTransformName = "attach";

	// Token: 0x04004532 RID: 17714
	public static Hashtable models = new Hashtable();

	// Token: 0x04004533 RID: 17715
	public static Hashtable materials = new Hashtable();

	// Token: 0x04004534 RID: 17716
	private SteamVR_Events.Action deviceConnectedAction;

	// Token: 0x04004535 RID: 17717
	private SteamVR_Events.Action hideRenderModelsAction;

	// Token: 0x04004536 RID: 17718
	private SteamVR_Events.Action modelSkinSettingsHaveChangedAction;

	// Token: 0x04004537 RID: 17719
	private Dictionary<int, string> nameCache;

	// Token: 0x02000C09 RID: 3081
	public class RenderModel
	{
		// Token: 0x06005F7E RID: 24446 RVA: 0x0021922D File Offset: 0x0021762D
		public RenderModel(Mesh mesh, Material material)
		{
			this.mesh = mesh;
			this.material = material;
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06005F7F RID: 24447 RVA: 0x00219243 File Offset: 0x00217643
		// (set) Token: 0x06005F80 RID: 24448 RVA: 0x0021924B File Offset: 0x0021764B
		public Mesh mesh { get; private set; }

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06005F81 RID: 24449 RVA: 0x00219254 File Offset: 0x00217654
		// (set) Token: 0x06005F82 RID: 24450 RVA: 0x0021925C File Offset: 0x0021765C
		public Material material { get; private set; }
	}

	// Token: 0x02000C0A RID: 3082
	public sealed class RenderModelInterfaceHolder : IDisposable
	{
		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06005F84 RID: 24452 RVA: 0x00219270 File Offset: 0x00217670
		public CVRRenderModels instance
		{
			get
			{
				if (this._instance == null && !this.failedLoadInterface)
				{
					if (!SteamVR.active && !SteamVR.usingNativeSupport)
					{
						EVRInitError evrinitError = EVRInitError.None;
						OpenVR.Init(ref evrinitError, EVRApplicationType.VRApplication_Utility);
						this.needsShutdown = true;
					}
					this._instance = OpenVR.RenderModels;
					if (this._instance == null)
					{
						Debug.LogError("Failed to load IVRRenderModels interface version IVRRenderModels_005");
						this.failedLoadInterface = true;
					}
				}
				return this._instance;
			}
		}

		// Token: 0x06005F85 RID: 24453 RVA: 0x002192E6 File Offset: 0x002176E6
		public void Dispose()
		{
			if (this.needsShutdown)
			{
				OpenVR.Shutdown();
			}
		}

		// Token: 0x0400453A RID: 17722
		private bool needsShutdown;

		// Token: 0x0400453B RID: 17723
		private bool failedLoadInterface;

		// Token: 0x0400453C RID: 17724
		private CVRRenderModels _instance;
	}
}
