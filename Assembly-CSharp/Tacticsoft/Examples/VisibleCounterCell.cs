using System;
using UnityEngine.UI;

namespace Tacticsoft.Examples
{
	// Token: 0x020008DC RID: 2268
	public class VisibleCounterCell : TableViewCell
	{
		// Token: 0x06004501 RID: 17665 RVA: 0x0017131E File Offset: 0x0016F71E
		public void SetRowNumber(int rowNumber)
		{
			this.m_rowNumberText.text = "Row " + rowNumber.ToString();
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x00171342 File Offset: 0x0016F742
		public void NotifyBecameVisible()
		{
			this.m_numTimesBecameVisible++;
			this.m_visibleCountText.text = "# rows this cell showed : " + this.m_numTimesBecameVisible.ToString();
		}

		// Token: 0x04002EF3 RID: 12019
		public Text m_rowNumberText;

		// Token: 0x04002EF4 RID: 12020
		public Text m_visibleCountText;

		// Token: 0x04002EF5 RID: 12021
		private int m_numTimesBecameVisible;
	}
}
