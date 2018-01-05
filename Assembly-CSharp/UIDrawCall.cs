using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000601 RID: 1537
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Draw Call")]
public class UIDrawCall : MonoBehaviour
{
	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x0600334F RID: 13135 RVA: 0x001069DA File Offset: 0x00104DDA
	[Obsolete("Use UIDrawCall.activeList")]
	public static BetterList<UIDrawCall> list
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x06003350 RID: 13136 RVA: 0x001069E1 File Offset: 0x00104DE1
	public static BetterList<UIDrawCall> activeList
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x06003351 RID: 13137 RVA: 0x001069E8 File Offset: 0x00104DE8
	public static BetterList<UIDrawCall> inactiveList
	{
		get
		{
			return UIDrawCall.mInactiveList;
		}
	}

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x06003352 RID: 13138 RVA: 0x001069EF File Offset: 0x00104DEF
	// (set) Token: 0x06003353 RID: 13139 RVA: 0x001069F7 File Offset: 0x00104DF7
	public int renderQueue
	{
		get
		{
			return this.mRenderQueue;
		}
		set
		{
			if (this.mRenderQueue != value)
			{
				this.mRenderQueue = value;
				if (this.mDynamicMat != null)
				{
					this.mDynamicMat.renderQueue = value;
				}
			}
		}
	}

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06003354 RID: 13140 RVA: 0x00106A29 File Offset: 0x00104E29
	// (set) Token: 0x06003355 RID: 13141 RVA: 0x00106A4D File Offset: 0x00104E4D
	public int sortingOrder
	{
		get
		{
			return (!(this.mRenderer != null)) ? 0 : this.mRenderer.sortingOrder;
		}
		set
		{
			if (this.mRenderer != null && this.mRenderer.sortingOrder != value)
			{
				this.mRenderer.sortingOrder = value;
			}
		}
	}

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x06003356 RID: 13142 RVA: 0x00106A7D File Offset: 0x00104E7D
	public int finalRenderQueue
	{
		get
		{
			return (!(this.mDynamicMat != null)) ? this.mRenderQueue : this.mDynamicMat.renderQueue;
		}
	}

	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x06003357 RID: 13143 RVA: 0x00106AA6 File Offset: 0x00104EA6
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x06003358 RID: 13144 RVA: 0x00106ACB File Offset: 0x00104ECB
	// (set) Token: 0x06003359 RID: 13145 RVA: 0x00106AD3 File Offset: 0x00104ED3
	public Material baseMaterial
	{
		get
		{
			return this.mMaterial;
		}
		set
		{
			if (this.mMaterial != value)
			{
				this.mMaterial = value;
				this.mRebuildMat = true;
			}
		}
	}

	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x0600335A RID: 13146 RVA: 0x00106AF4 File Offset: 0x00104EF4
	public Material dynamicMaterial
	{
		get
		{
			return this.mDynamicMat;
		}
	}

	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x0600335B RID: 13147 RVA: 0x00106AFC File Offset: 0x00104EFC
	// (set) Token: 0x0600335C RID: 13148 RVA: 0x00106B04 File Offset: 0x00104F04
	public Texture mainTexture
	{
		get
		{
			return this.mTexture;
		}
		set
		{
			this.mTexture = value;
			if (this.mDynamicMat != null)
			{
				this.mDynamicMat.mainTexture = value;
			}
		}
	}

	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x0600335D RID: 13149 RVA: 0x00106B2A File Offset: 0x00104F2A
	// (set) Token: 0x0600335E RID: 13150 RVA: 0x00106B32 File Offset: 0x00104F32
	public Shader shader
	{
		get
		{
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				this.mShader = value;
				this.mRebuildMat = true;
			}
		}
	}

	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x0600335F RID: 13151 RVA: 0x00106B53 File Offset: 0x00104F53
	public int triangles
	{
		get
		{
			return (!(this.mMesh != null)) ? 0 : this.mTriangles;
		}
	}

	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x06003360 RID: 13152 RVA: 0x00106B72 File Offset: 0x00104F72
	public bool isClipped
	{
		get
		{
			return this.mClipCount != 0;
		}
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x00106B80 File Offset: 0x00104F80
	private void CreateMaterial()
	{
		this.mTextureClip = false;
		this.mLegacyShader = false;
		this.mClipCount = this.panel.clipCount;
		string text = (!(this.mShader != null)) ? ((!(this.mMaterial != null)) ? "Unlit/Transparent Colored" : this.mMaterial.shader.name) : this.mShader.name;
		text = text.Replace("GUI/Text Shader", "Unlit/Text");
		if (text.Length > 2 && text[text.Length - 2] == ' ')
		{
			int num = (int)text[text.Length - 1];
			if (num > 48 && num <= 57)
			{
				text = text.Substring(0, text.Length - 2);
			}
		}
		if (text.StartsWith("Hidden/"))
		{
			text = text.Substring(7);
		}
		text = text.Replace(" (SoftClip)", string.Empty);
		text = text.Replace(" (TextureClip)", string.Empty);
		if (this.panel.clipping == UIDrawCall.Clipping.TextureMask)
		{
			this.mTextureClip = true;
			this.shader = Shader.Find("Hidden/" + text + " (TextureClip)");
		}
		else if (this.mClipCount != 0)
		{
			this.shader = Shader.Find(string.Concat(new object[]
			{
				"Hidden/",
				text,
				" ",
				this.mClipCount
			}));
			if (this.shader == null)
			{
				this.shader = Shader.Find(text + " " + this.mClipCount);
			}
			if (this.shader == null && this.mClipCount == 1)
			{
				this.mLegacyShader = true;
				this.shader = Shader.Find(text + " (SoftClip)");
			}
		}
		else
		{
			this.shader = Shader.Find(text);
		}
		if (this.mMaterial != null)
		{
			this.mDynamicMat = new Material(this.mMaterial);
			this.mDynamicMat.name = "[NGUI] " + this.mMaterial.name;
			this.mDynamicMat.hideFlags = (HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
			this.mDynamicMat.CopyPropertiesFromMaterial(this.mMaterial);
			string[] shaderKeywords = this.mMaterial.shaderKeywords;
			for (int i = 0; i < shaderKeywords.Length; i++)
			{
				this.mDynamicMat.EnableKeyword(shaderKeywords[i]);
			}
			if (this.shader != null)
			{
				this.mDynamicMat.shader = this.shader;
			}
			else if (this.mClipCount != 0)
			{
				Debug.LogError(string.Concat(new object[]
				{
					text,
					" shader doesn't have a clipped shader version for ",
					this.mClipCount,
					" clip regions"
				}));
			}
		}
		else
		{
			this.mDynamicMat = new Material(this.shader);
			this.mDynamicMat.name = "[NGUI] " + this.shader.name;
			this.mDynamicMat.hideFlags = (HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
		}
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x00106EC4 File Offset: 0x001052C4
	private Material RebuildMaterial()
	{
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.CreateMaterial();
		this.mDynamicMat.renderQueue = this.mRenderQueue;
		if (this.mTexture != null)
		{
			this.mDynamicMat.mainTexture = this.mTexture;
		}
		if (this.mRenderer != null)
		{
			this.mRenderer.sharedMaterials = new Material[]
			{
				this.mDynamicMat
			};
		}
		return this.mDynamicMat;
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x00106F48 File Offset: 0x00105348
	private void UpdateMaterials()
	{
		if (this.mRebuildMat || this.mDynamicMat == null || this.mClipCount != this.panel.clipCount || this.mTextureClip != (this.panel.clipping == UIDrawCall.Clipping.TextureMask))
		{
			this.RebuildMaterial();
			this.mRebuildMat = false;
		}
		else if (this.mRenderer.sharedMaterial != this.mDynamicMat)
		{
			this.mRenderer.sharedMaterials = new Material[]
			{
				this.mDynamicMat
			};
		}
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x00106FE8 File Offset: 0x001053E8
	public void UpdateGeometry(int widgetCount)
	{
		this.widgetCount = widgetCount;
		int size = this.verts.size;
		if (size > 0 && size == this.uvs.size && size == this.cols.size && size % 4 == 0)
		{
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.GetComponent<MeshFilter>();
			}
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (this.verts.size < 65000)
			{
				int num = (size >> 1) * 3;
				bool flag = this.mIndices == null || this.mIndices.Length != num;
				if (this.mMesh == null)
				{
					this.mMesh = new Mesh();
					this.mMesh.hideFlags = HideFlags.DontSave;
					this.mMesh.name = ((!(this.mMaterial != null)) ? "[NGUI] Mesh" : ("[NGUI] " + this.mMaterial.name));
					this.mMesh.MarkDynamic();
					flag = true;
				}
				bool flag2 = this.uvs.buffer.Length != this.verts.buffer.Length || this.cols.buffer.Length != this.verts.buffer.Length || (this.norms.buffer != null && this.norms.buffer.Length != this.verts.buffer.Length) || (this.tans.buffer != null && this.tans.buffer.Length != this.verts.buffer.Length);
				if (!flag2 && this.panel.renderQueue != UIPanel.RenderQueue.Automatic)
				{
					flag2 = (this.mMesh == null || this.mMesh.vertexCount != this.verts.buffer.Length);
				}
				if (!flag2 && this.verts.size << 1 < this.verts.buffer.Length)
				{
					flag2 = true;
				}
				this.mTriangles = this.verts.size >> 1;
				if (flag2 || this.verts.buffer.Length > 65000)
				{
					if (flag2 || this.mMesh.vertexCount != this.verts.size)
					{
						this.mMesh.Clear();
						flag = true;
					}
					this.mMesh.vertices = this.verts.ToArray();
					this.mMesh.uv = this.uvs.ToArray();
					this.mMesh.colors32 = this.cols.ToArray();
					if (this.norms != null)
					{
						this.mMesh.normals = this.norms.ToArray();
					}
					if (this.tans != null)
					{
						this.mMesh.tangents = this.tans.ToArray();
					}
				}
				else
				{
					if (this.mMesh.vertexCount != this.verts.buffer.Length)
					{
						this.mMesh.Clear();
						flag = true;
					}
					this.mMesh.vertices = this.verts.buffer;
					this.mMesh.uv = this.uvs.buffer;
					this.mMesh.colors32 = this.cols.buffer;
					if (this.norms != null)
					{
						this.mMesh.normals = this.norms.buffer;
					}
					if (this.tans != null)
					{
						this.mMesh.tangents = this.tans.buffer;
					}
				}
				if (flag)
				{
					this.mIndices = this.GenerateCachedIndexBuffer(size, num);
					this.mMesh.triangles = this.mIndices;
				}
				if (flag2 || !this.alwaysOnScreen)
				{
					this.mMesh.RecalculateBounds();
				}
				this.mFilter.mesh = this.mMesh;
			}
			else
			{
				this.mTriangles = 0;
				if (this.mFilter.mesh != null)
				{
					this.mFilter.mesh.Clear();
				}
				Debug.LogError("Too many vertices on one panel: " + this.verts.size);
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.GetComponent<MeshRenderer>();
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.AddComponent<MeshRenderer>();
			}
			this.UpdateMaterials();
		}
		else
		{
			if (this.mFilter.mesh != null)
			{
				this.mFilter.mesh.Clear();
			}
			Debug.LogError("UIWidgets must fill the buffer with 4 vertices per quad. Found " + size);
		}
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.norms.Clear();
		this.tans.Clear();
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x00107534 File Offset: 0x00105934
	private int[] GenerateCachedIndexBuffer(int vertexCount, int indexCount)
	{
		int i = 0;
		int count = UIDrawCall.mCache.Count;
		while (i < count)
		{
			int[] array = UIDrawCall.mCache[i];
			if (array != null && array.Length == indexCount)
			{
				return array;
			}
			i++;
		}
		int[] array2 = new int[indexCount];
		int num = 0;
		for (int j = 0; j < vertexCount; j += 4)
		{
			array2[num++] = j;
			array2[num++] = j + 1;
			array2[num++] = j + 2;
			array2[num++] = j + 2;
			array2[num++] = j + 3;
			array2[num++] = j;
		}
		if (UIDrawCall.mCache.Count > 10)
		{
			UIDrawCall.mCache.RemoveAt(0);
		}
		UIDrawCall.mCache.Add(array2);
		return array2;
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x00107610 File Offset: 0x00105A10
	private void OnWillRenderObject()
	{
		this.UpdateMaterials();
		if (this.onRender != null)
		{
			this.onRender(this.mDynamicMat ?? this.mMaterial);
		}
		if (this.mDynamicMat == null || this.mClipCount == 0)
		{
			return;
		}
		if (this.mTextureClip)
		{
			Vector4 drawCallClipRange = this.panel.drawCallClipRange;
			Vector2 clipSoftness = this.panel.clipSoftness;
			Vector2 vector = new Vector2(1000f, 1000f);
			if (clipSoftness.x > 0f)
			{
				vector.x = drawCallClipRange.z / clipSoftness.x;
			}
			if (clipSoftness.y > 0f)
			{
				vector.y = drawCallClipRange.w / clipSoftness.y;
			}
			this.mDynamicMat.SetVector(UIDrawCall.ClipRange[0], new Vector4(-drawCallClipRange.x / drawCallClipRange.z, -drawCallClipRange.y / drawCallClipRange.w, 1f / drawCallClipRange.z, 1f / drawCallClipRange.w));
			this.mDynamicMat.SetTexture("_ClipTex", this.clipTexture);
		}
		else if (!this.mLegacyShader)
		{
			UIPanel parentPanel = this.panel;
			int num = 0;
			while (parentPanel != null)
			{
				if (parentPanel.hasClipping)
				{
					float angle = 0f;
					Vector4 drawCallClipRange2 = parentPanel.drawCallClipRange;
					if (parentPanel != this.panel)
					{
						Vector3 vector2 = parentPanel.cachedTransform.InverseTransformPoint(this.panel.cachedTransform.position);
						drawCallClipRange2.x -= vector2.x;
						drawCallClipRange2.y -= vector2.y;
						Vector3 eulerAngles = this.panel.cachedTransform.rotation.eulerAngles;
						Vector3 eulerAngles2 = parentPanel.cachedTransform.rotation.eulerAngles;
						Vector3 vector3 = eulerAngles2 - eulerAngles;
						vector3.x = NGUIMath.WrapAngle(vector3.x);
						vector3.y = NGUIMath.WrapAngle(vector3.y);
						vector3.z = NGUIMath.WrapAngle(vector3.z);
						if (Mathf.Abs(vector3.x) > 0.001f || Mathf.Abs(vector3.y) > 0.001f)
						{
							Debug.LogWarning("Panel can only be clipped properly if X and Y rotation is left at 0", this.panel);
						}
						angle = vector3.z;
					}
					this.SetClipping(num++, drawCallClipRange2, parentPanel.clipSoftness, angle);
				}
				parentPanel = parentPanel.parentPanel;
			}
		}
		else
		{
			Vector2 clipSoftness2 = this.panel.clipSoftness;
			Vector4 drawCallClipRange3 = this.panel.drawCallClipRange;
			Vector2 mainTextureOffset = new Vector2(-drawCallClipRange3.x / drawCallClipRange3.z, -drawCallClipRange3.y / drawCallClipRange3.w);
			Vector2 mainTextureScale = new Vector2(1f / drawCallClipRange3.z, 1f / drawCallClipRange3.w);
			Vector2 v = new Vector2(1000f, 1000f);
			if (clipSoftness2.x > 0f)
			{
				v.x = drawCallClipRange3.z / clipSoftness2.x;
			}
			if (clipSoftness2.y > 0f)
			{
				v.y = drawCallClipRange3.w / clipSoftness2.y;
			}
			this.mDynamicMat.mainTextureOffset = mainTextureOffset;
			this.mDynamicMat.mainTextureScale = mainTextureScale;
			this.mDynamicMat.SetVector("_ClipSharpness", v);
		}
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x001079C0 File Offset: 0x00105DC0
	private void SetClipping(int index, Vector4 cr, Vector2 soft, float angle)
	{
		angle *= -0.0174532924f;
		Vector2 vector = new Vector2(1000f, 1000f);
		if (soft.x > 0f)
		{
			vector.x = cr.z / soft.x;
		}
		if (soft.y > 0f)
		{
			vector.y = cr.w / soft.y;
		}
		if (index < UIDrawCall.ClipRange.Length)
		{
			this.mDynamicMat.SetVector(UIDrawCall.ClipRange[index], new Vector4(-cr.x / cr.z, -cr.y / cr.w, 1f / cr.z, 1f / cr.w));
			this.mDynamicMat.SetVector(UIDrawCall.ClipArgs[index], new Vector4(vector.x, vector.y, Mathf.Sin(angle), Mathf.Cos(angle)));
		}
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x00107AC8 File Offset: 0x00105EC8
	private void Awake()
	{
		if (UIDrawCall.ClipRange == null)
		{
			UIDrawCall.ClipRange = new int[]
			{
				Shader.PropertyToID("_ClipRange0"),
				Shader.PropertyToID("_ClipRange1"),
				Shader.PropertyToID("_ClipRange2"),
				Shader.PropertyToID("_ClipRange4")
			};
		}
		if (UIDrawCall.ClipArgs == null)
		{
			UIDrawCall.ClipArgs = new int[]
			{
				Shader.PropertyToID("_ClipArgs0"),
				Shader.PropertyToID("_ClipArgs1"),
				Shader.PropertyToID("_ClipArgs2"),
				Shader.PropertyToID("_ClipArgs3")
			};
		}
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x00107B67 File Offset: 0x00105F67
	private void OnEnable()
	{
		this.mRebuildMat = true;
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x00107B70 File Offset: 0x00105F70
	private void OnDisable()
	{
		this.depthStart = int.MaxValue;
		this.depthEnd = int.MinValue;
		this.panel = null;
		this.manager = null;
		this.mMaterial = null;
		this.mTexture = null;
		this.clipTexture = null;
		if (this.mRenderer != null)
		{
			this.mRenderer.sharedMaterials = new Material[0];
		}
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.mDynamicMat = null;
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x00107BEA File Offset: 0x00105FEA
	private void OnDestroy()
	{
		NGUITools.DestroyImmediate(this.mMesh);
		this.mMesh = null;
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x00107BFE File Offset: 0x00105FFE
	public static UIDrawCall Create(UIPanel panel, Material mat, Texture tex, Shader shader)
	{
		return UIDrawCall.Create(null, panel, mat, tex, shader);
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x00107C0C File Offset: 0x0010600C
	private static UIDrawCall Create(string name, UIPanel pan, Material mat, Texture tex, Shader shader)
	{
		UIDrawCall uidrawCall = UIDrawCall.Create(name);
		uidrawCall.gameObject.layer = pan.cachedGameObject.layer;
		uidrawCall.baseMaterial = mat;
		uidrawCall.mainTexture = tex;
		uidrawCall.shader = shader;
		uidrawCall.renderQueue = pan.startingRenderQueue;
		uidrawCall.sortingOrder = pan.sortingOrder;
		uidrawCall.manager = pan;
		return uidrawCall;
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x00107C6C File Offset: 0x0010606C
	private static UIDrawCall Create(string name)
	{
		if (UIDrawCall.mInactiveList.size > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mInactiveList.Pop();
			UIDrawCall.mActiveList.Add(uidrawCall);
			if (name != null)
			{
				uidrawCall.name = name;
			}
			NGUITools.SetActive(uidrawCall.gameObject, true);
			return uidrawCall;
		}
		GameObject gameObject = new GameObject(name);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		UIDrawCall uidrawCall2 = gameObject.AddComponent<UIDrawCall>();
		UIDrawCall.mActiveList.Add(uidrawCall2);
		return uidrawCall2;
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x00107CDC File Offset: 0x001060DC
	public static void ClearAll()
	{
		bool isPlaying = Application.isPlaying;
		int i = UIDrawCall.mActiveList.size;
		while (i > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mActiveList[--i];
			if (uidrawCall)
			{
				if (isPlaying)
				{
					NGUITools.SetActive(uidrawCall.gameObject, false);
				}
				else
				{
					NGUITools.DestroyImmediate(uidrawCall.gameObject);
				}
			}
		}
		UIDrawCall.mActiveList.Clear();
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x00107D4D File Offset: 0x0010614D
	public static void ReleaseAll()
	{
		UIDrawCall.ClearAll();
		UIDrawCall.ReleaseInactive();
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x00107D5C File Offset: 0x0010615C
	public static void ReleaseInactive()
	{
		int i = UIDrawCall.mInactiveList.size;
		while (i > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mInactiveList[--i];
			if (uidrawCall)
			{
				NGUITools.DestroyImmediate(uidrawCall.gameObject);
			}
		}
		UIDrawCall.mInactiveList.Clear();
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x00107DB0 File Offset: 0x001061B0
	public static int Count(UIPanel panel)
	{
		int num = 0;
		for (int i = 0; i < UIDrawCall.mActiveList.size; i++)
		{
			if (UIDrawCall.mActiveList[i].manager == panel)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x00107DFC File Offset: 0x001061FC
	public static void Destroy(UIDrawCall dc)
	{
		if (dc)
		{
			dc.onRender = null;
			if (Application.isPlaying)
			{
				if (UIDrawCall.mActiveList.Remove(dc))
				{
					NGUITools.SetActive(dc.gameObject, false);
					UIDrawCall.mInactiveList.Add(dc);
				}
			}
			else
			{
				UIDrawCall.mActiveList.Remove(dc);
				NGUITools.DestroyImmediate(dc.gameObject);
			}
		}
	}

	// Token: 0x04001D2B RID: 7467
	private static BetterList<UIDrawCall> mActiveList = new BetterList<UIDrawCall>();

	// Token: 0x04001D2C RID: 7468
	private static BetterList<UIDrawCall> mInactiveList = new BetterList<UIDrawCall>();

	// Token: 0x04001D2D RID: 7469
	[HideInInspector]
	[NonSerialized]
	public int widgetCount;

	// Token: 0x04001D2E RID: 7470
	[HideInInspector]
	[NonSerialized]
	public int depthStart = int.MaxValue;

	// Token: 0x04001D2F RID: 7471
	[HideInInspector]
	[NonSerialized]
	public int depthEnd = int.MinValue;

	// Token: 0x04001D30 RID: 7472
	[HideInInspector]
	[NonSerialized]
	public UIPanel manager;

	// Token: 0x04001D31 RID: 7473
	[HideInInspector]
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x04001D32 RID: 7474
	[HideInInspector]
	[NonSerialized]
	public Texture2D clipTexture;

	// Token: 0x04001D33 RID: 7475
	[HideInInspector]
	[NonSerialized]
	public bool alwaysOnScreen;

	// Token: 0x04001D34 RID: 7476
	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector3> verts = new BetterList<Vector3>();

	// Token: 0x04001D35 RID: 7477
	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector3> norms = new BetterList<Vector3>();

	// Token: 0x04001D36 RID: 7478
	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector4> tans = new BetterList<Vector4>();

	// Token: 0x04001D37 RID: 7479
	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	// Token: 0x04001D38 RID: 7480
	[HideInInspector]
	[NonSerialized]
	public BetterList<Color32> cols = new BetterList<Color32>();

	// Token: 0x04001D39 RID: 7481
	private Material mMaterial;

	// Token: 0x04001D3A RID: 7482
	private Texture mTexture;

	// Token: 0x04001D3B RID: 7483
	private Shader mShader;

	// Token: 0x04001D3C RID: 7484
	private int mClipCount;

	// Token: 0x04001D3D RID: 7485
	private Transform mTrans;

	// Token: 0x04001D3E RID: 7486
	private Mesh mMesh;

	// Token: 0x04001D3F RID: 7487
	private MeshFilter mFilter;

	// Token: 0x04001D40 RID: 7488
	private MeshRenderer mRenderer;

	// Token: 0x04001D41 RID: 7489
	private Material mDynamicMat;

	// Token: 0x04001D42 RID: 7490
	private int[] mIndices;

	// Token: 0x04001D43 RID: 7491
	private bool mRebuildMat = true;

	// Token: 0x04001D44 RID: 7492
	private bool mLegacyShader;

	// Token: 0x04001D45 RID: 7493
	private int mRenderQueue = 3000;

	// Token: 0x04001D46 RID: 7494
	private int mTriangles;

	// Token: 0x04001D47 RID: 7495
	[NonSerialized]
	public bool isDirty;

	// Token: 0x04001D48 RID: 7496
	[NonSerialized]
	private bool mTextureClip;

	// Token: 0x04001D49 RID: 7497
	public UIDrawCall.OnRenderCallback onRender;

	// Token: 0x04001D4A RID: 7498
	private const int maxIndexBufferCache = 10;

	// Token: 0x04001D4B RID: 7499
	private static List<int[]> mCache = new List<int[]>(10);

	// Token: 0x04001D4C RID: 7500
	private static int[] ClipRange = null;

	// Token: 0x04001D4D RID: 7501
	private static int[] ClipArgs = null;

	// Token: 0x02000602 RID: 1538
	public enum Clipping
	{
		// Token: 0x04001D4F RID: 7503
		None,
		// Token: 0x04001D50 RID: 7504
		TextureMask,
		// Token: 0x04001D51 RID: 7505
		SoftClip = 3,
		// Token: 0x04001D52 RID: 7506
		ConstrainButDontClip
	}

	// Token: 0x02000603 RID: 1539
	// (Invoke) Token: 0x06003376 RID: 13174
	public delegate void OnRenderCallback(Material mat);
}
