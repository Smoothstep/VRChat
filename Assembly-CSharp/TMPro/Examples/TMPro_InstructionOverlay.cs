using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000900 RID: 2304
	public class TMPro_InstructionOverlay : MonoBehaviour
	{
		// Token: 0x06004590 RID: 17808 RVA: 0x00176528 File Offset: 0x00174928
		private void Awake()
		{
			if (!base.enabled)
			{
				return;
			}
			this.m_camera = Camera.main;
			GameObject gameObject = new GameObject("Frame Counter");
			this.m_frameCounter_transform = gameObject.transform;
			this.m_frameCounter_transform.parent = this.m_camera.transform;
			this.m_frameCounter_transform.localRotation = Quaternion.identity;
			this.m_TextMeshPro = gameObject.AddComponent<TextMeshPro>();
			this.m_TextMeshPro.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			this.m_TextMeshPro.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			this.m_TextMeshPro.fontSize = 30f;
			this.m_TextMeshPro.isOverlay = true;
			this.m_textContainer = gameObject.GetComponent<TextContainer>();
			this.Set_FrameCounter_Position(this.AnchorPosition);
			this.m_TextMeshPro.text = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x00176624 File Offset: 0x00174A24
		private void Set_FrameCounter_Position(TMPro_InstructionOverlay.FpsCounterAnchorPositions anchor_position)
		{
			switch (anchor_position)
			{
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.TopLeft:
				this.m_textContainer.anchorPosition = TextContainerAnchors.TopLeft;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				break;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomLeft:
				this.m_textContainer.anchorPosition = TextContainerAnchors.BottomLeft;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				break;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.TopRight:
				this.m_textContainer.anchorPosition = TextContainerAnchors.TopRight;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				break;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomRight:
				this.m_textContainer.anchorPosition = TextContainerAnchors.BottomRight;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				break;
			}
		}

		// Token: 0x04002FB1 RID: 12209
		public TMPro_InstructionOverlay.FpsCounterAnchorPositions AnchorPosition = TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomLeft;

		// Token: 0x04002FB2 RID: 12210
		private const string instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

		// Token: 0x04002FB3 RID: 12211
		private TextMeshPro m_TextMeshPro;

		// Token: 0x04002FB4 RID: 12212
		private TextContainer m_textContainer;

		// Token: 0x04002FB5 RID: 12213
		private Transform m_frameCounter_transform;

		// Token: 0x04002FB6 RID: 12214
		private Camera m_camera;

		// Token: 0x02000901 RID: 2305
		public enum FpsCounterAnchorPositions
		{
			// Token: 0x04002FB8 RID: 12216
			TopLeft,
			// Token: 0x04002FB9 RID: 12217
			BottomLeft,
			// Token: 0x04002FBA RID: 12218
			TopRight,
			// Token: 0x04002FBB RID: 12219
			BottomRight
		}
	}
}
