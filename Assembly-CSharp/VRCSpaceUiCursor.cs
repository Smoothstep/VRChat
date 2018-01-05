using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C5E RID: 3166
public class VRCSpaceUiCursor : VRCUiCursor
{
	// Token: 0x06006253 RID: 25171 RVA: 0x00230173 File Offset: 0x0022E573
	protected void Awake()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06006254 RID: 25172 RVA: 0x00230188 File Offset: 0x0022E588
	protected override void Initialize()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
		this.materials = new Material[this.renderers.Length];
		this.baseColors = new Color[this.renderers.Length];
		this.currentColors = new Color[this.renderers.Length];
		this.baseSizes = new float[this.renderers.Length];
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.materials[i] = this.renderers[i].material;
			this.materials[i].renderQueue = 3999;
			this.baseColors[i] = this.materials[i].GetColor("_TintColor");
			this.currentColors[i] = this.baseColors[i];
			if (this.renderers[i] as ParticleSystemRenderer != null)
			{
				this.baseSizes[i] = this.renderers[i].GetComponent<ParticleSystem>().main.startSizeMultiplier;
			}
			else
			{
				this.baseSizes[i] = -1f;
			}
		}
		if (this.fillImage != null)
		{
			this.fillImage.fillAmount = 0f;
		}
		this.initialized = true;
	}

	// Token: 0x06006255 RID: 25173 RVA: 0x002302E8 File Offset: 0x0022E6E8
	private void Update()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		bool flag = false;
		float trackingScale = VRCTrackingManager.GetTrackingScale();
		float num = trackingScale;
		if (num <= 0f)
		{
			num = 1f;
		}
		if (!Mathf.Approximately(this.lastScale, num))
		{
			flag = true;
		}
		for (int i = 0; i < this.materials.Length; i++)
		{
			Color color = this.baseColors[i];
			if (this.over.Contains(VRCUiCursor.CursorOver.UiSelectable))
			{
				color = this.UiColor;
			}
			if (this.over.Contains(VRCUiCursor.CursorOver.Player))
			{
				color = this.UiColor;
			}
			else if (this.over.Contains(VRCUiCursor.CursorOver.Web))
			{
				color = this.UiColor;
			}
			else if (this.over.Contains(VRCUiCursor.CursorOver.Interactable))
			{
				color = this.InteractiveColor;
			}
			else if (this.over.Contains(VRCUiCursor.CursorOver.Pickup))
			{
				color = this.InteractiveColor;
			}
			this.currentColors[i].r = Mathf.MoveTowards(this.currentColors[i].r, color.r, 2f * Time.deltaTime);
			this.currentColors[i].g = Mathf.MoveTowards(this.currentColors[i].g, color.g, 2f * Time.deltaTime);
			this.currentColors[i].b = Mathf.MoveTowards(this.currentColors[i].b, color.b, 2f * Time.deltaTime);
			if (this.baseSizes[i] >= 0f && flag)
			{
				float num2 = num * this.baseSizes[i];
				ParticleSystem component = this.renderers[i].GetComponent<ParticleSystem>();
                // HMM.
				//component.main.startSizeMultiplier = num2;
				int particleCount = component.particleCount;
				ParticleSystem.Particle[] array = new ParticleSystem.Particle[particleCount];
				component.GetParticles(array);
				for (int j = 0; j < particleCount; j++)
				{
					array[j].startSize = num2;
				}
				component.SetParticles(array, particleCount);
			}
			this.materials[i].SetColor("_TintColor", this.currentColors[i]);
		}
		this.lastScale = num;
	}

	// Token: 0x06006256 RID: 25174 RVA: 0x00230554 File Offset: 0x0022E954
	public override void UpdateCursor(VRCUiCursor.CursorRaycast target, bool useForUi = true)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		target.ray = this.CalculateCursorRay();
		target.extraReach = true;
		base.CastCursorRay(target);
		base.SetTargetInfo(target, useForUi);
		this.over = target.over;
		base.transform.position = target.hitInfo.point - target.ray.direction * 0.05f;
	}

	// Token: 0x06006257 RID: 25175 RVA: 0x002305D4 File Offset: 0x0022E9D4
	private Ray CalculateCursorRay()
	{
		Ray result = VRCVrCamera.GetInstance().GetWorldLookRay();
		if (VRCUiManager.Instance != null && VRCUiManager.Instance.IsActive() && this.mouseCursor)
		{
			if (!HMDManager.IsHmdDetected())
			{
				result = VRCVrCamera.GetInstance().ScreenPointToRay(Input.mousePosition);
			}
			else
			{
				Vector2 zero = Vector2.zero;
				zero.x += Input.GetAxis("Mouse X") * this.MouseSensitivity.x;
				zero.y -= Input.GetAxis("Mouse Y") * this.MouseSensitivity.y;
				Ray worldLookRay = VRCVrCamera.GetInstance().GetWorldLookRay();
				Vector3 vector = Vector3.Cross(Vector3.up, worldLookRay.direction);
				Vector3 axis = Vector3.Cross(worldLookRay.direction, vector);
				Quaternion rotation = Quaternion.AngleAxis(zero.x, axis) * Quaternion.AngleAxis(zero.y, vector);
				Vector3 point = base.transform.position - worldLookRay.origin;
				result = new Ray(worldLookRay.origin, rotation * point);
				float num = Vector3.Angle(result.direction, worldLookRay.direction);
				if (num > 50f)
				{
					result.direction = Vector3.RotateTowards(result.direction, worldLookRay.direction, num - 50f, 0f);
				}
			}
		}
		return result;
	}

	// Token: 0x040047C4 RID: 18372
	public Image fillImage;

	// Token: 0x040047C5 RID: 18373
	private Renderer[] renderers;

	// Token: 0x040047C6 RID: 18374
	private Material[] materials;

	// Token: 0x040047C7 RID: 18375
	private Color[] baseColors;

	// Token: 0x040047C8 RID: 18376
	private Color[] currentColors;

	// Token: 0x040047C9 RID: 18377
	private float[] baseSizes;

	// Token: 0x040047CA RID: 18378
	public Vector2 MouseSensitivity = Vector2.one;

	// Token: 0x040047CB RID: 18379
	private const float CursorAngleRange = 50f;

	// Token: 0x040047CC RID: 18380
	public bool mouseCursor;

	// Token: 0x040047CD RID: 18381
	private float lastScale = 1f;
}
