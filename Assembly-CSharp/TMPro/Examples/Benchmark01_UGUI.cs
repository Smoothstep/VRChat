using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro.Examples
{
	// Token: 0x020008E3 RID: 2275
	public class Benchmark01_UGUI : MonoBehaviour
	{
		// Token: 0x06004530 RID: 17712 RVA: 0x00172284 File Offset: 0x00170684
		private IEnumerator Start()
		{
			if (this.BenchmarkType == 0)
			{
				this.m_textMeshPro = base.gameObject.AddComponent<TextMeshProUGUI>();
				if (this.TMProFont != null)
				{
					this.m_textMeshPro.font = this.TMProFont;
				}
				this.m_textMeshPro.fontSize = 48f;
				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.extraPadding = true;
				this.m_material01 = this.m_textMeshPro.font.material;
				this.m_material02 = (Resources.Load("Fonts & Materials/LiberationSans SDF - BEVEL", typeof(Material)) as Material);
			}
			else if (this.BenchmarkType == 1)
			{
				this.m_textMesh = base.gameObject.AddComponent<Text>();
				if (this.TextMeshFont != null)
				{
					this.m_textMesh.font = this.TextMeshFont;
				}
				this.m_textMesh.fontSize = 48;
				this.m_textMesh.alignment = TextAnchor.MiddleCenter;
			}
			for (int i = 0; i <= 1000000; i++)
			{
				if (this.BenchmarkType == 0)
				{
					this.m_textMeshPro.text = "The <#0050FF>count is: </color>" + i % 1000;
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

		// Token: 0x04002F12 RID: 12050
		public int BenchmarkType;

		// Token: 0x04002F13 RID: 12051
		public Canvas canvas;

		// Token: 0x04002F14 RID: 12052
		public TMP_FontAsset TMProFont;

		// Token: 0x04002F15 RID: 12053
		public Font TextMeshFont;

		// Token: 0x04002F16 RID: 12054
		private TextMeshProUGUI m_textMeshPro;

		// Token: 0x04002F17 RID: 12055
		private Text m_textMesh;

		// Token: 0x04002F18 RID: 12056
		private const string label01 = "The <#0050FF>count is: </color>";

		// Token: 0x04002F19 RID: 12057
		private const string label02 = "The <color=#0050FF>count is: </color>";

		// Token: 0x04002F1A RID: 12058
		private Material m_material01;

		// Token: 0x04002F1B RID: 12059
		private Material m_material02;
	}
}
