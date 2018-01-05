using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020008FB RID: 2299
	[ExecuteInEditMode]
	public class TMP_TextInfoDebugTool : MonoBehaviour
	{
		// Token: 0x04002F86 RID: 12166
		public bool ShowCharacters;

		// Token: 0x04002F87 RID: 12167
		public bool ShowWords;

		// Token: 0x04002F88 RID: 12168
		public bool ShowLinks;

		// Token: 0x04002F89 RID: 12169
		public bool ShowLines;

		// Token: 0x04002F8A RID: 12170
		public bool ShowMeshBounds;

		// Token: 0x04002F8B RID: 12171
		public bool ShowTextBounds;

		// Token: 0x04002F8C RID: 12172
		[Space(10f)]
		[TextArea(2, 2)]
		public string ObjectStats;

		// Token: 0x04002F8D RID: 12173
		private TMP_Text m_TextComponent;

		// Token: 0x04002F8E RID: 12174
		private Transform m_Transform;
	}
}
