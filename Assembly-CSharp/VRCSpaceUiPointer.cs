using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C60 RID: 3168
public class VRCSpaceUiPointer : VRCUiCursor
{
	// Token: 0x0600625E RID: 25182 RVA: 0x0023129B File Offset: 0x0022F69B
	protected void Awake()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x0600625F RID: 25183 RVA: 0x002312B0 File Offset: 0x0022F6B0
	protected override void Initialize()
	{
		VRCUiCursor.CursorHandedness handedness = this.handedness;
		if (handedness != VRCUiCursor.CursorHandedness.Right)
		{
			if (handedness == VRCUiCursor.CursorHandedness.Left)
			{
				this.pointerTracking = VRCTracking.ID.HandTracker_LeftPointer;
				this.touchTracking = VRCTracking.ID.HandTracker_LeftPalm;
			}
		}
		else
		{
			this.pointerTracking = VRCTracking.ID.HandTracker_RightPointer;
			this.touchTracking = VRCTracking.ID.HandTracker_RightPalm;
		}
		this.materials = new Material[this.dimRenderers.Length];
		this.baseColors = new Color[this.dimRenderers.Length];
		this.currentColors = new Color[this.dimRenderers.Length];
		for (int i = 0; i < this.dimRenderers.Length; i++)
		{
			this.materials[i] = this.dimRenderers[i].material;
			this.baseColors[i] = this.materials[i].GetColor("_TintColor");
			this.currentColors[i] = this.baseColors[i];
		}
		this.beam = base.GetComponent<VRCSpaceUiBeam>();
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].material.renderQueue = 3999;
		}
		if (this.fillImage != null)
		{
			this.fillImage.fillAmount = 0f;
		}
		this.initialized = true;
	}

	// Token: 0x06006260 RID: 25184 RVA: 0x0023140C File Offset: 0x0022F80C
	private void Update()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.beam.layerMask = (VRCUiCursorManager.GetCurrentInteractiveLayers() & (-1 ^ 1 << LayerMask.NameToLayer("Walkthrough")));
		for (int i = 0; i < this.materials.Length; i++)
		{
			Color endColor = this.baseColors[i];
			if (this.over.Contains(VRCUiCursor.CursorOver.Ui))
			{
				endColor = this.UiColor;
			}
			else if (this.over.Contains(VRCUiCursor.CursorOver.Player))
			{
				endColor = this.UiColor;
			}
			else if (this.over.Contains(VRCUiCursor.CursorOver.Web))
			{
				endColor = this.UiColor;
			}
			else if (this.over.Contains(VRCUiCursor.CursorOver.Interactable))
			{
				endColor = this.InteractiveColor;
			}
			else if (this.over.Contains(VRCUiCursor.CursorOver.Pickup))
			{
				endColor = this.InteractiveColor;
			}
			this.currentColors[i].r = Mathf.MoveTowards(this.currentColors[i].r, endColor.r, 2f * Time.deltaTime);
			this.currentColors[i].g = Mathf.MoveTowards(this.currentColors[i].g, endColor.g, 2f * Time.deltaTime);
			this.currentColors[i].b = Mathf.MoveTowards(this.currentColors[i].b, endColor.b, 2f * Time.deltaTime);
			this.lineRenderer.startColor = Color.white;
			this.lineRenderer.endColor = endColor;
			this.materials[i].SetColor("_TintColor", this.currentColors[i]);
		}
	}

	// Token: 0x06006261 RID: 25185 RVA: 0x002315F8 File Offset: 0x0022F9F8
	public override void UpdateCursor(VRCUiCursor.CursorRaycast target, bool useForUi = true)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(this.pointerTracking);
		if (trackedTransform != null)
		{
			base.transform.position = trackedTransform.position;
			base.transform.rotation = trackedTransform.rotation;
		}
		if (this.navigationCursor)
		{
			LocomotionInputController.navigationCursorPosition = this.beam.Arccast(1f);
		}
		else
		{
			Transform trackedTransform2 = VRCTrackingManager.GetTrackedTransform(this.touchTracking);
			if (trackedTransform2 != null)
			{
				float trackingScale = VRCTrackingManager.GetTrackingScale();
				target.touch = new BoundingSphere(trackedTransform2.transform.position, trackingScale * this.GetTouchRadius());
				this.over = base.CheckCursorTouch(target);
			}
			if (this.over.Length == 0)
			{
				target.ray = new Ray(base.transform.position, base.transform.forward);
				this.over = base.CastCursorRay(target);
			}
			base.SetTargetInfo(target, useForUi);
			this.over = target.over;
		}
	}

	// Token: 0x06006262 RID: 25186 RVA: 0x0023170D File Offset: 0x0022FB0D
	private float GetTouchRadius()
	{
		if (VRCInputManager.LastInputMethod == VRCInputManager.InputMethod.Oculus)
		{
			return this.touchRadius + 0.05f;
		}
		return this.touchRadius;
	}

	// Token: 0x040047CE RID: 18382
	public Color lowlightColor;

	// Token: 0x040047CF RID: 18383
	public Image fillImage;

	// Token: 0x040047D0 RID: 18384
	public LineRenderer lineRenderer;

	// Token: 0x040047D1 RID: 18385
	public Renderer[] dimRenderers;

	// Token: 0x040047D2 RID: 18386
	private VRCSpaceUiBeam beam;

	// Token: 0x040047D3 RID: 18387
	private Material[] materials;

	// Token: 0x040047D4 RID: 18388
	private Color[] baseColors;

	// Token: 0x040047D5 RID: 18389
	private Color[] currentColors;

	// Token: 0x040047D6 RID: 18390
	private VRCTracking.ID pointerTracking;

	// Token: 0x040047D7 RID: 18391
	private VRCTracking.ID touchTracking;

	// Token: 0x040047D8 RID: 18392
	public bool navigationCursor;

	// Token: 0x040047D9 RID: 18393
	public float touchRadius = 0.4f;
}
