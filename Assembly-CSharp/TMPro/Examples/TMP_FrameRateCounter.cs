using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008F3 RID: 2291
	public class TMP_FrameRateCounter : MonoBehaviour
	{
		// Token: 0x06004559 RID: 17753 RVA: 0x001743F0 File Offset: 0x001727F0
		private void Awake()
		{
			if (!base.enabled)
			{
				return;
			}
			this.m_camera = Camera.main;
			Application.targetFrameRate = -1;
			GameObject gameObject = new GameObject("Frame Counter");
			this.m_TextMeshPro = gameObject.AddComponent<TextMeshPro>();
			this.m_TextMeshPro.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			this.m_TextMeshPro.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			this.m_frameCounter_transform = gameObject.transform;
			this.m_frameCounter_transform.SetParent(this.m_camera.transform);
			this.m_frameCounter_transform.localRotation = Quaternion.identity;
			this.m_TextMeshPro.enableWordWrapping = false;
			this.m_TextMeshPro.fontSize = 24f;
			this.m_TextMeshPro.isOverlay = true;
			this.Set_FrameCounter_Position(this.AnchorPosition);
			this.last_AnchorPosition = this.AnchorPosition;
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x001744EB File Offset: 0x001728EB
		private void Start()
		{
			this.m_LastInterval = Time.realtimeSinceStartup;
			this.m_Frames = 0;
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x00174500 File Offset: 0x00172900
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

		// Token: 0x0600455C RID: 17756 RVA: 0x001745EC File Offset: 0x001729EC
		private void Set_FrameCounter_Position(TMP_FrameRateCounter.FpsCounterAnchorPositions anchor_position)
		{
			this.m_TextMeshPro.margin = new Vector4(1f, 1f, 1f, 1f);
			switch (anchor_position)
			{
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.TopLeft:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.TopLeft;
				this.m_TextMeshPro.rectTransform.pivot = new Vector2(0f, 1f);
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				break;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.BottomLeft:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.BottomLeft;
				this.m_TextMeshPro.rectTransform.pivot = new Vector2(0f, 0f);
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				break;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.TopRight:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.TopRight;
				this.m_TextMeshPro.rectTransform.pivot = new Vector2(1f, 1f);
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				break;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.BottomRight:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.BottomRight;
				this.m_TextMeshPro.rectTransform.pivot = new Vector2(1f, 0f);
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				break;
			}
		}

		// Token: 0x04002F6B RID: 12139
		public float UpdateInterval = 5f;

		// Token: 0x04002F6C RID: 12140
		private float m_LastInterval;

		// Token: 0x04002F6D RID: 12141
		private int m_Frames;

		// Token: 0x04002F6E RID: 12142
		public TMP_FrameRateCounter.FpsCounterAnchorPositions AnchorPosition = TMP_FrameRateCounter.FpsCounterAnchorPositions.TopRight;

		// Token: 0x04002F6F RID: 12143
		private string htmlColorTag;

		// Token: 0x04002F70 RID: 12144
		private const string fpsLabel = "{0:2}</color> FPS \n{1:2} <#8080ff>MS";

		// Token: 0x04002F71 RID: 12145
		private TextMeshPro m_TextMeshPro;

		// Token: 0x04002F72 RID: 12146
		private Transform m_frameCounter_transform;

		// Token: 0x04002F73 RID: 12147
		private Camera m_camera;

		// Token: 0x04002F74 RID: 12148
		private TMP_FrameRateCounter.FpsCounterAnchorPositions last_AnchorPosition;

		// Token: 0x020008F4 RID: 2292
		public enum FpsCounterAnchorPositions
		{
			// Token: 0x04002F76 RID: 12150
			TopLeft,
			// Token: 0x04002F77 RID: 12151
			BottomLeft,
			// Token: 0x04002F78 RID: 12152
			TopRight,
			// Token: 0x04002F79 RID: 12153
			BottomRight
		}
	}
}
