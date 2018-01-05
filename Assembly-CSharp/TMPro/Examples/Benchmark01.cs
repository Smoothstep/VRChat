using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008E2 RID: 2274
	public class Benchmark01 : MonoBehaviour
	{
		// Token: 0x0600452E RID: 17710 RVA: 0x00171E88 File Offset: 0x00170288
		private IEnumerator Start()
		{
			if (this.BenchmarkType == 0)
			{
				this.m_textMeshPro = base.gameObject.AddComponent<TextMeshPro>();
				this.m_textMeshPro.autoSizeTextContainer = true;
				if (this.TMProFont != null)
				{
					this.m_textMeshPro.font = this.TMProFont;
				}
				this.m_textMeshPro.fontSize = 48f;
				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.extraPadding = true;
				this.m_textMeshPro.enableWordWrapping = false;
				this.m_material01 = this.m_textMeshPro.font.material;
				this.m_material02 = (Resources.Load("Fonts & Materials/LiberationSans SDF - Drop Shadow", typeof(Material)) as Material);
			}
			else if (this.BenchmarkType == 1)
			{
				this.m_textMesh = base.gameObject.AddComponent<TextMesh>();
				if (this.TextMeshFont != null)
				{
					this.m_textMesh.font = this.TextMeshFont;
					this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				}
				else
				{
					this.m_textMesh.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
					this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				}
				this.m_textMesh.fontSize = 48;
				this.m_textMesh.anchor = TextAnchor.MiddleCenter;
			}
			for (int i = 0; i <= 1000000; i++)
			{
				if (this.BenchmarkType == 0)
				{
					this.m_textMeshPro.SetText("The <#0050FF>count is: </color>{0}", (float)(i % 1000));
					if (i % 1000 == 999)
					{
						TMP_Text textMeshPro = this.m_textMeshPro;
						Material fontSharedMaterial;
						if (this.m_textMeshPro.fontSharedMaterial == this.m_material01)
						{
							Material material = this.m_material02;
							this.m_textMeshPro.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						else
						{
							Material material = this.m_material01;
							this.m_textMeshPro.fontSharedMaterial = material;
							fontSharedMaterial = material;
						}
						textMeshPro.fontSharedMaterial = fontSharedMaterial;
					}
				}
				else if (this.BenchmarkType == 1)
				{
					this.m_textMesh.text = "The <color=#0050FF>count is: </color>" + (i % 1000).ToString();
				}
				yield return null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x04002F08 RID: 12040
		public int BenchmarkType;

		// Token: 0x04002F09 RID: 12041
		public TMP_FontAsset TMProFont;

		// Token: 0x04002F0A RID: 12042
		public Font TextMeshFont;

		// Token: 0x04002F0B RID: 12043
		private TextMeshPro m_textMeshPro;

		// Token: 0x04002F0C RID: 12044
		private TextContainer m_textContainer;

		// Token: 0x04002F0D RID: 12045
		private TextMesh m_textMesh;

		// Token: 0x04002F0E RID: 12046
		private const string label01 = "The <#0050FF>count is: </color>{0}";

		// Token: 0x04002F0F RID: 12047
		private const string label02 = "The <color=#0050FF>count is: </color>";

		// Token: 0x04002F10 RID: 12048
		private Material m_material01;

		// Token: 0x04002F11 RID: 12049
		private Material m_material02;
	}
}
