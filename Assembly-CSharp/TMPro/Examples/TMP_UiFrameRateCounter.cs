using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008FE RID: 2302
	public class TMP_UiFrameRateCounter : MonoBehaviour
	{
		// Token: 0x0600458B RID: 17803 RVA: 0x0017611C File Offset: 0x0017451C
		private void Awake()
		{
			if (!base.enabled)
			{
				return;
			}
			Application.targetFrameRate = 120;
			GameObject gameObject = new GameObject("Frame Counter");
			this.m_frameCounter_transform = gameObject.AddComponent<RectTransform>();
			this.m_frameCounter_transform.SetParent(base.transform, false);
			this.m_TextMeshPro = gameObject.AddComponent<TextMeshProUGUI>();
			this.m_TextMeshPro.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			this.m_TextMeshPro.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			this.m_TextMeshPro.enableWordWrapping = false;
			this.m_TextMeshPro.fontSize = 36f;
			this.m_TextMeshPro.isOverlay = true;
			this.Set_FrameCounter_Position(this.AnchorPosition);
			this.last_AnchorPosition = this.AnchorPosition;
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x001761F9 File Offset: 0x001745F9
		private void Start()
		{
			this.m_LastInterval = Time.realtimeSinceStartup;
			this.m_Frames = 0;
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x00176210 File Offset: 0x00174610
		private void Update()
		{
			if (this.AnchorPosition != this.last_AnchorPosition)
			{
				this.Set_FrameCounter_Position(this.AnchorPosition);
			}
			this.last_AnchorPosition = this.AnchorPosition;
			this.m_Frames++;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup > this.m_LastInterval + this.UpdateInterval)
			{
				float num = (float)this.m_Frames / (realtimeSinceStartup - this.m_LastInterval);
				float arg = 1000f / Mathf.Max(num, 1E-05f);
				if (num < 30f)
				{
					this.htmlColorTag = "<color=yellow>";
				}
				else if (num < 10f)
				{
					this.htmlColorTag = "<color=red>";
				}
				else
				{
					this.htmlColorTag = "<color=green>";
				}
				this.m_TextMeshPro.SetText(this.htmlColorTag + "{0:2}</color> FPS \n{1:2} <#8080ff>MS", num, arg);
				this.m_Frames = 0;
				this.m_LastInterval = realtimeSinceStartup;
			}
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x001762FC File Offset: 0x001746FC
		private void Set_FrameCounter_Position(TMP_UiFrameRateCounter.FpsCounterAnchorPositions anchor_position)
		{
			switch (anchor_position)
			{
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.TopLeft:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.TopLeft;
				this.m_frameCounter_transform.pivot = new Vector2(0f, 1f);
				this.m_frameCounter_transform.anchorMin = new Vector2(0.01f, 0.99f);
				this.m_frameCounter_transform.anchorMax = new Vector2(0.01f, 0.99f);
				this.m_frameCounter_transform.anchoredPosition = new Vector2(0f, 1f);
				break;
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.BottomLeft:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.BottomLeft;
				this.m_frameCounter_transform.pivot = new Vector2(0f, 0f);
				this.m_frameCounter_transform.anchorMin = new Vector2(0.01f, 0.01f);
				this.m_frameCounter_transform.anchorMax = new Vector2(0.01f, 0.01f);
				this.m_frameCounter_transform.anchoredPosition = new Vector2(0f, 0f);
				break;
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.TopRight:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.TopRight;
				this.m_frameCounter_transform.pivot = new Vector2(1f, 1f);
				this.m_frameCounter_transform.anchorMin = new Vector2(0.99f, 0.99f);
				this.m_frameCounter_transform.anchorMax = new Vector2(0.99f, 0.99f);
				this.m_frameCounter_transform.anchoredPosition = new Vector2(1f, 1f);
				break;
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.BottomRight:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.BottomRight;
				this.m_frameCounter_transform.pivot = new Vector2(1f, 0f);
				this.m_frameCounter_transform.anchorMin = new Vector2(0.99f, 0.01f);
				this.m_frameCounter_transform.anchorMax = new Vector2(0.99f, 0.01f);
				this.m_frameCounter_transform.anchoredPosition = new Vector2(1f, 0f);
				break;
			}
		}

		// Token: 0x04002FA3 RID: 12195
		public float UpdateInterval = 5f;

		// Token: 0x04002FA4 RID: 12196
		private float m_LastInterval;

		// Token: 0x04002FA5 RID: 12197
		private int m_Frames;

		// Token: 0x04002FA6 RID: 12198
		public TMP_UiFrameRateCounter.FpsCounterAnchorPositions AnchorPosition = TMP_UiFrameRateCounter.FpsCounterAnchorPositions.TopRight;

		// Token: 0x04002FA7 RID: 12199
		private string htmlColorTag;

		// Token: 0x04002FA8 RID: 12200
		private const string fpsLabel = "{0:2}</color> FPS \n{1:2} <#8080ff>MS";

		// Token: 0x04002FA9 RID: 12201
		private TextMeshProUGUI m_TextMeshPro;

		// Token: 0x04002FAA RID: 12202
		private RectTransform m_frameCounter_transform;

		// Token: 0x04002FAB RID: 12203
		private TMP_UiFrameRateCounter.FpsCounterAnchorPositions last_AnchorPosition;

		// Token: 0x020008FF RID: 2303
		public enum FpsCounterAnchorPositions
		{
			// Token: 0x04002FAD RID: 12205
			TopLeft,
			// Token: 0x04002FAE RID: 12206
			BottomLeft,
			// Token: 0x04002FAF RID: 12207
			TopRight,
			// Token: 0x04002FB0 RID: 12208
			BottomRight
		}
	}
}
