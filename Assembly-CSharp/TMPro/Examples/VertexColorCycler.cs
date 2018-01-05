using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000906 RID: 2310
	public class VertexColorCycler : MonoBehaviour
	{
		// Token: 0x060045A6 RID: 17830 RVA: 0x00177841 File Offset: 0x00175C41
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x0017784F File Offset: 0x00175C4F
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x00177860 File Offset: 0x00175C60
		private IEnumerator AnimateVertexColors()
		{
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			int currentCharacter = 0;
			Color32 c0 = this.m_TextComponent.color;
			for (;;)
			{
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
					Color32[] newVertexColors = textInfo.meshInfo[materialIndex].colors32;
					int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;
					if (textInfo.characterInfo[currentCharacter].isVisible)
					{
						c0 = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
						newVertexColors[vertexIndex] = c0;
						newVertexColors[vertexIndex + 1] = c0;
						newVertexColors[vertexIndex + 2] = c0;
						newVertexColors[vertexIndex + 3] = c0;
						this.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
					}
					currentCharacter = (currentCharacter + 1) % characterCount;
					yield return new WaitForSeconds(0.05f);
				}
			}
			yield break;
		}

		// Token: 0x04002FCF RID: 12239
		private TMP_Text m_TextComponent;
	}
}
