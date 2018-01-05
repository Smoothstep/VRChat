using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008EE RID: 2286
	public class SimpleScript : MonoBehaviour
	{
		// Token: 0x0600454C RID: 17740 RVA: 0x00173948 File Offset: 0x00171D48
		private void Start()
		{
			this.m_textMeshPro = base.gameObject.AddComponent<TextMeshPro>();
			this.m_textMeshPro.autoSizeTextContainer = true;
			this.m_textMeshPro.fontSize = 48f;
			this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
			this.m_textMeshPro.enableWordWrapping = false;
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x0017399E File Offset: 0x00171D9E
		private void Update()
		{
			this.m_textMeshPro.SetText("The <#0050FF>count is: </color>{0:2}", this.m_frame % 1000f);
			this.m_frame += 1f * Time.deltaTime;
		}

		// Token: 0x04002F5C RID: 12124
		private TextMeshPro m_textMeshPro;

		// Token: 0x04002F5D RID: 12125
		private const string label = "The <#0050FF>count is: </color>{0:2}";

		// Token: 0x04002F5E RID: 12126
		private float m_frame;
	}
}
