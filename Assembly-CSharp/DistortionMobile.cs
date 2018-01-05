using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000896 RID: 2198
public class DistortionMobile : MonoBehaviour
{
	// Token: 0x06004381 RID: 17281 RVA: 0x00164607 File Offset: 0x00162A07
	private void OnEnable()
	{
		this.fpsMove = new WaitForSeconds(1f / (float)this.FPSWhenMoveCamera);
		this.fpsStatic = new WaitForSeconds(1f / (float)this.FPSWhenStaticCamera);
	}

	// Token: 0x06004382 RID: 17282 RVA: 0x0016463C File Offset: 0x00162A3C
	private void Update()
	{
		if (!this.isInitialized)
		{
			this.Initialize();
			base.StartCoroutine(this.RepeatCameraMove());
			base.StartCoroutine(this.RepeatCameraStatic());
		}
		if (Vector3.SqrMagnitude(this.instanceCameraTransform.position - this.oldPosition) <= 1E-05f && this.instanceCameraTransform.rotation == this.oldRotation)
		{
			this.frameCountWhenCameraIsStatic++;
			if (this.frameCountWhenCameraIsStatic >= 50)
			{
				this.isStaticUpdate = true;
			}
		}
		else
		{
			this.frameCountWhenCameraIsStatic = 0;
			this.isStaticUpdate = false;
		}
		this.oldPosition = this.instanceCameraTransform.position;
		this.oldRotation = this.instanceCameraTransform.rotation;
		if (this.canUpdateCamera)
		{
			this.cameraInstance.enabled = true;
			this.canUpdateCamera = false;
		}
		else if (this.cameraInstance.enabled)
		{
			this.cameraInstance.enabled = false;
		}
	}

	// Token: 0x06004383 RID: 17283 RVA: 0x0016474C File Offset: 0x00162B4C
	private IEnumerator RepeatCameraMove()
	{
		for (;;)
		{
			if (!this.isStaticUpdate)
			{
				this.canUpdateCamera = true;
			}
			yield return this.fpsMove;
		}
		yield break;
	}

	// Token: 0x06004384 RID: 17284 RVA: 0x00164768 File Offset: 0x00162B68
	private IEnumerator RepeatCameraStatic()
	{
		for (;;)
		{
			if (this.isStaticUpdate)
			{
				this.canUpdateCamera = true;
			}
			yield return this.fpsStatic;
		}
		yield break;
	}

	// Token: 0x06004385 RID: 17285 RVA: 0x00164783 File Offset: 0x00162B83
	private void OnBecameVisible()
	{
		if (this.goCamera != null)
		{
			this.goCamera.SetActive(true);
		}
	}

	// Token: 0x06004386 RID: 17286 RVA: 0x001647A2 File Offset: 0x00162BA2
	private void OnBecameInvisible()
	{
		if (this.goCamera != null)
		{
			this.goCamera.SetActive(false);
		}
	}

	// Token: 0x06004387 RID: 17287 RVA: 0x001647C4 File Offset: 0x00162BC4
	private void Initialize()
	{
		this.goCamera = new GameObject("RenderTextureCamera");
		this.cameraInstance = this.goCamera.AddComponent<Camera>();
		Camera main = Camera.main;
		this.cameraInstance.CopyFrom(main);
		this.cameraInstance.depth += 1f;
		this.cameraInstance.cullingMask = this.CullingMask;
		this.cameraInstance.renderingPath = this.RenderingPath;
		this.goCamera.transform.parent = main.transform;
		this.renderTexture = new RenderTexture(Mathf.RoundToInt((float)Screen.width * this.TextureScale), Mathf.RoundToInt((float)Screen.height * this.TextureScale), 16, this.RenderTextureFormat);
		this.renderTexture.DiscardContents();
		this.renderTexture.filterMode = this.FilterMode;
		this.cameraInstance.targetTexture = this.renderTexture;
		this.instanceCameraTransform = this.cameraInstance.transform;
		this.oldPosition = this.instanceCameraTransform.position;
		Shader.SetGlobalTexture("_GrabTextureMobile", this.renderTexture);
		this.isInitialized = true;
	}

	// Token: 0x06004388 RID: 17288 RVA: 0x001648F8 File Offset: 0x00162CF8
	private void OnDisable()
	{
		if (this.goCamera)
		{
			UnityEngine.Object.DestroyImmediate(this.goCamera);
			this.goCamera = null;
		}
		if (this.renderTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.renderTexture);
			this.renderTexture = null;
		}
		this.isInitialized = false;
	}

	// Token: 0x04002C23 RID: 11299
	public float TextureScale = 1f;

	// Token: 0x04002C24 RID: 11300
	public RenderTextureFormat RenderTextureFormat;

	// Token: 0x04002C25 RID: 11301
	public FilterMode FilterMode;

	// Token: 0x04002C26 RID: 11302
	public LayerMask CullingMask = -17;

	// Token: 0x04002C27 RID: 11303
	public RenderingPath RenderingPath;

	// Token: 0x04002C28 RID: 11304
	public int FPSWhenMoveCamera = 40;

	// Token: 0x04002C29 RID: 11305
	public int FPSWhenStaticCamera = 20;

	// Token: 0x04002C2A RID: 11306
	private RenderTexture renderTexture;

	// Token: 0x04002C2B RID: 11307
	private Camera cameraInstance;

	// Token: 0x04002C2C RID: 11308
	private GameObject goCamera;

	// Token: 0x04002C2D RID: 11309
	private Vector3 oldPosition;

	// Token: 0x04002C2E RID: 11310
	private Quaternion oldRotation;

	// Token: 0x04002C2F RID: 11311
	private Transform instanceCameraTransform;

	// Token: 0x04002C30 RID: 11312
	private bool canUpdateCamera;

	// Token: 0x04002C31 RID: 11313
	private bool isStaticUpdate;

	// Token: 0x04002C32 RID: 11314
	private WaitForSeconds fpsMove;

	// Token: 0x04002C33 RID: 11315
	private WaitForSeconds fpsStatic;

	// Token: 0x04002C34 RID: 11316
	private const int dropedFrames = 50;

	// Token: 0x04002C35 RID: 11317
	private int frameCountWhenCameraIsStatic;

	// Token: 0x04002C36 RID: 11318
	private bool isInitialized;
}
