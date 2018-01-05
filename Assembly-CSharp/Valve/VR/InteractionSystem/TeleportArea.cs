using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BE3 RID: 3043
	public class TeleportArea : TeleportMarkerBase
	{
		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06005E34 RID: 24116 RVA: 0x002108E0 File Offset: 0x0020ECE0
		// (set) Token: 0x06005E35 RID: 24117 RVA: 0x002108E8 File Offset: 0x0020ECE8
		public Bounds meshBounds { get; private set; }

		// Token: 0x06005E36 RID: 24118 RVA: 0x002108F1 File Offset: 0x0020ECF1
		public void Awake()
		{
			this.areaMesh = base.GetComponent<MeshRenderer>();
			this.tintColorId = Shader.PropertyToID("_TintColor");
			this.CalculateBounds();
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x00210918 File Offset: 0x0020ED18
		public void Start()
		{
			this.visibleTintColor = Teleport.instance.areaVisibleMaterial.GetColor(this.tintColorId);
			this.highlightedTintColor = Teleport.instance.areaHighlightedMaterial.GetColor(this.tintColorId);
			this.lockedTintColor = Teleport.instance.areaLockedMaterial.GetColor(this.tintColorId);
		}

		// Token: 0x06005E38 RID: 24120 RVA: 0x00210976 File Offset: 0x0020ED76
		public override bool ShouldActivate(Vector3 playerPosition)
		{
			return true;
		}

		// Token: 0x06005E39 RID: 24121 RVA: 0x00210979 File Offset: 0x0020ED79
		public override bool ShouldMovePlayer()
		{
			return true;
		}

		// Token: 0x06005E3A RID: 24122 RVA: 0x0021097C File Offset: 0x0020ED7C
		public override void Highlight(bool highlight)
		{
			if (!this.locked)
			{
				this.highlighted = highlight;
				if (highlight)
				{
					this.areaMesh.material = Teleport.instance.areaHighlightedMaterial;
				}
				else
				{
					this.areaMesh.material = Teleport.instance.areaVisibleMaterial;
				}
			}
		}

		// Token: 0x06005E3B RID: 24123 RVA: 0x002109D0 File Offset: 0x0020EDD0
		public override void SetAlpha(float tintAlpha, float alphaPercent)
		{
			Color tintColor = this.GetTintColor();
			tintColor.a *= alphaPercent;
			this.areaMesh.material.SetColor(this.tintColorId, tintColor);
		}

		// Token: 0x06005E3C RID: 24124 RVA: 0x00210A0A File Offset: 0x0020EE0A
		public override void UpdateVisuals()
		{
			if (this.locked)
			{
				this.areaMesh.material = Teleport.instance.areaLockedMaterial;
			}
			else
			{
				this.areaMesh.material = Teleport.instance.areaVisibleMaterial;
			}
		}

		// Token: 0x06005E3D RID: 24125 RVA: 0x00210A48 File Offset: 0x0020EE48
		public void UpdateVisualsInEditor()
		{
			this.areaMesh = base.GetComponent<MeshRenderer>();
			if (this.locked)
			{
				this.areaMesh.sharedMaterial = Teleport.instance.areaLockedMaterial;
			}
			else
			{
				this.areaMesh.sharedMaterial = Teleport.instance.areaVisibleMaterial;
			}
		}

		// Token: 0x06005E3E RID: 24126 RVA: 0x00210A9C File Offset: 0x0020EE9C
		private bool CalculateBounds()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component == null)
			{
				return false;
			}
			Mesh sharedMesh = component.sharedMesh;
			if (sharedMesh == null)
			{
				return false;
			}
			this.meshBounds = sharedMesh.bounds;
			return true;
		}

		// Token: 0x06005E3F RID: 24127 RVA: 0x00210AE0 File Offset: 0x0020EEE0
		private Color GetTintColor()
		{
			if (this.locked)
			{
				return this.lockedTintColor;
			}
			if (this.highlighted)
			{
				return this.highlightedTintColor;
			}
			return this.visibleTintColor;
		}

		// Token: 0x04004411 RID: 17425
		private MeshRenderer areaMesh;

		// Token: 0x04004412 RID: 17426
		private int tintColorId;

		// Token: 0x04004413 RID: 17427
		private Color visibleTintColor = Color.clear;

		// Token: 0x04004414 RID: 17428
		private Color highlightedTintColor = Color.clear;

		// Token: 0x04004415 RID: 17429
		private Color lockedTintColor = Color.clear;

		// Token: 0x04004416 RID: 17430
		private bool highlighted;
	}
}
