using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tacticsoft.Examples
{
	// Token: 0x020008D6 RID: 2262
	public class DynamicHeightCell : TableViewCell
	{
		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x060044DC RID: 17628 RVA: 0x00170C84 File Offset: 0x0016F084
		// (set) Token: 0x060044DD RID: 17629 RVA: 0x00170C8C File Offset: 0x0016F08C
		public int rowNumber { get; set; }

		// Token: 0x060044DE RID: 17630 RVA: 0x00170C98 File Offset: 0x0016F098
		private void Update()
		{
			this.m_rowNumberText.text = "Row " + this.rowNumber.ToString();
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x00170CCE File Offset: 0x0016F0CE
		public void SliderValueChanged(float value)
		{
			this.onCellHeightChanged.Invoke(this.rowNumber, value);
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x060044E0 RID: 17632 RVA: 0x00170CE2 File Offset: 0x0016F0E2
		// (set) Token: 0x060044E1 RID: 17633 RVA: 0x00170CEF File Offset: 0x0016F0EF
		public float height
		{
			get
			{
				return this.m_cellHeightSlider.value;
			}
			set
			{
				this.m_cellHeightSlider.value = value;
			}
		}

		// Token: 0x04002EE4 RID: 12004
		public Text m_rowNumberText;

		// Token: 0x04002EE5 RID: 12005
		public Slider m_cellHeightSlider;

		// Token: 0x04002EE7 RID: 12007
		public DynamicHeightCell.CellHeightChangedEvent onCellHeightChanged;

		// Token: 0x020008D7 RID: 2263
		[Serializable]
		public class CellHeightChangedEvent : UnityEvent<int, float>
		{
		}
	}
}
