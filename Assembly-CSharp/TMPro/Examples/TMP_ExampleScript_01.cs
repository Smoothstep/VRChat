using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008F1 RID: 2289
	public class TMP_ExampleScript_01 : MonoBehaviour
	{
		// Token: 0x06004556 RID: 17750 RVA: 0x001742C0 File Offset: 0x001726C0
		private void Awake()
		{
			if (this.ObjectType == TMP_ExampleScript_01.objectType.TextMeshPro)
			{
				this.m_text = (base.GetComponent<TextMeshPro>() ?? base.gameObject.AddComponent<TextMeshPro>());
			}
			else
			{
				this.m_text = (base.GetComponent<TextMeshProUGUI>() ?? base.gameObject.AddComponent<TextMeshProUGUI>());
			}
			this.m_text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");
			this.m_text.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");
			this.m_text.fontSize = 120f;
			this.m_text.text = "A <#0080ff>simple</color> line of text.";
			Vector2 preferredValues = this.m_text.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
			this.m_text.rectTransform.sizeDelta = new Vector2(preferredValues.x, preferredValues.y);
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x0017439C File Offset: 0x0017279C
		private void Update()
		{
			if (!this.isStatic)
			{
				this.m_text.SetText("The count is <#0080ff>{0}</color>", (float)(this.count % 1000));
				this.count++;
			}
		}

		// Token: 0x04002F63 RID: 12131
		public TMP_ExampleScript_01.objectType ObjectType;

		// Token: 0x04002F64 RID: 12132
		public bool isStatic;

		// Token: 0x04002F65 RID: 12133
		private TMP_Text m_text;

		// Token: 0x04002F66 RID: 12134
		private const string k_label = "The count is <#0080ff>{0}</color>";

		// Token: 0x04002F67 RID: 12135
		private int count;

		// Token: 0x020008F2 RID: 2290
		public enum objectType
		{
			// Token: 0x04002F69 RID: 12137
			TextMeshPro,
			// Token: 0x04002F6A RID: 12138
			TextMeshProUGUI
		}
	}
}
