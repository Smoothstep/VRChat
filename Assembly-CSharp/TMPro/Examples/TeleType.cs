using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000902 RID: 2306
	public class TeleType : MonoBehaviour
	{
		// Token: 0x06004593 RID: 17811 RVA: 0x00176756 File Offset: 0x00174B56
		private void Awake()
		{
			this.m_textMeshPro = base.GetComponent<TMP_Text>();
			this.m_textMeshPro.text = this.label01;
			this.m_textMeshPro.enableWordWrapping = true;
			this.m_textMeshPro.alignment = TextAlignmentOptions.Top;
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x00176794 File Offset: 0x00174B94
		private IEnumerator Start()
		{
			this.m_textMeshPro.ForceMeshUpdate();
			int totalVisibleCharacters = this.m_textMeshPro.textInfo.characterCount;
			int counter = 0;
			int visibleCount = 0;
			for (;;)
			{
				visibleCount = counter % (totalVisibleCharacters + 1);
				this.m_textMeshPro.maxVisibleCharacters = visibleCount;
				if (visibleCount >= totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1f);
					this.m_textMeshPro.text = this.label02;
					yield return new WaitForSeconds(1f);
					this.m_textMeshPro.text = this.label01;
					yield return new WaitForSeconds(1f);
				}
				counter++;
				yield return new WaitForSeconds(0.05f);
			}
			yield break;
		}

		// Token: 0x04002FBC RID: 12220
		private string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

		// Token: 0x04002FBD RID: 12221
		private string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

		// Token: 0x04002FBE RID: 12222
		private TMP_Text m_textMeshPro;
	}
}
