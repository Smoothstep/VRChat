using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace Tacticsoft
{
	// Token: 0x020008DE RID: 2270
	[RequireComponent(typeof(ScrollRect))]
	public class TableView : MonoBehaviour
	{
		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06004507 RID: 17671 RVA: 0x00171380 File Offset: 0x0016F780
		// (set) Token: 0x06004508 RID: 17672 RVA: 0x00171388 File Offset: 0x0016F788
		public ITableViewDataSource dataSource
		{
			get
			{
				return this.m_dataSource;
			}
			set
			{
				this.m_dataSource = value;
				this.m_requiresReload = true;
			}
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x00171398 File Offset: 0x0016F798
		public TableViewCell GetReusableCell(string reuseIdentifier)
		{
			LinkedList<TableViewCell> linkedList;
			if (!this.m_reusableCells.TryGetValue(reuseIdentifier, out linkedList))
			{
				return null;
			}
			if (linkedList.Count == 0)
			{
				return null;
			}
			TableViewCell value = linkedList.First.Value;
			linkedList.RemoveFirst();
			return value;
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x0600450A RID: 17674 RVA: 0x001713DA File Offset: 0x0016F7DA
		// (set) Token: 0x0600450B RID: 17675 RVA: 0x001713E2 File Offset: 0x0016F7E2
		public bool isEmpty { get; private set; }

		// Token: 0x0600450C RID: 17676 RVA: 0x001713EC File Offset: 0x0016F7EC
		public void ReloadData()
		{
			if (!this.m_isInitialized)
			{
				this.Initialize();
			}
			this.m_rowHeights = new float[this.m_dataSource.GetNumberOfRowsForTableView(this)];
			this.isEmpty = (this.m_rowHeights.Length == 0);
			if (this.isEmpty)
			{
				this.ClearAllRows();
				return;
			}
			this.m_cumulativeRowHeights = new float[this.m_rowHeights.Length];
			this.m_cleanCumulativeIndex = -1;
			for (int i = 0; i < this.m_rowHeights.Length; i++)
			{
				this.m_rowHeights[i] = this.m_dataSource.GetHeightForRowInTableView(this, i);
				if (i > 0)
				{
					this.m_rowHeights[i] += this.m_verticalLayoutGroup.spacing;
				}
			}
			this.m_scrollRect.content.sizeDelta = new Vector2(this.m_scrollRect.content.sizeDelta[0], this.GetCumulativeRowHeight(this.m_rowHeights.Length - 1));
			this.RecalculateVisibleRowsFromScratch();
			this.m_requiresReload = false;
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x001714FC File Offset: 0x0016F8FC
		public TableViewCell GetCellAtRow(int row)
		{
			TableViewCell result = null;
			this.m_visibleCells.TryGetValue(row, out result);
			return result;
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x0600450E RID: 17678 RVA: 0x0017151B File Offset: 0x0016F91B
		public Range visibleRowRange
		{
			get
			{
				return this.m_visibleRowRange;
			}
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x00171524 File Offset: 0x0016F924
		public void NotifyCellDimensionsChanged(int row)
		{
			float num = this.m_rowHeights[row];
			this.m_rowHeights[row] = this.m_dataSource.GetHeightForRowInTableView(this, row);
			this.m_cleanCumulativeIndex = Mathf.Min(this.m_cleanCumulativeIndex, row - 1);
			if (this.m_visibleRowRange.Contains(row))
			{
				TableViewCell cellAtRow = this.GetCellAtRow(row);
				cellAtRow.GetComponent<LayoutElement>().preferredHeight = this.m_rowHeights[row];
				if (row > 0)
				{
					cellAtRow.GetComponent<LayoutElement>().preferredHeight -= this.m_verticalLayoutGroup.spacing;
				}
			}
			float num2 = this.m_rowHeights[row] - num;
			this.m_scrollRect.content.sizeDelta = new Vector2(this.m_scrollRect.content.sizeDelta.x, this.m_scrollRect.content.sizeDelta.y + num2);
			this.m_requiresRefresh = true;
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06004510 RID: 17680 RVA: 0x00171610 File Offset: 0x0016FA10
		public float scrollableHeight
		{
			get
			{
				return this.m_scrollRect.content.rect.height - (base.transform as RectTransform).rect.height;
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06004511 RID: 17681 RVA: 0x0017164E File Offset: 0x0016FA4E
		// (set) Token: 0x06004512 RID: 17682 RVA: 0x00171658 File Offset: 0x0016FA58
		public float scrollY
		{
			get
			{
				return this.m_scrollY;
			}
			set
			{
				if (this.isEmpty)
				{
					return;
				}
				value = Mathf.Clamp(value, 0f, this.GetScrollYForRow(this.m_rowHeights.Length - 1, true));
				if (this.m_scrollY != value)
				{
					this.m_scrollY = value;
					this.m_requiresRefresh = true;
					float num = value / this.scrollableHeight;
					this.m_scrollRect.verticalNormalizedPosition = 1f - num;
				}
			}
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x001716C4 File Offset: 0x0016FAC4
		public float GetScrollYForRow(int row, bool above)
		{
			float num = this.GetCumulativeRowHeight(row);
			if (above)
			{
				num -= this.m_rowHeights[row];
			}
			return num;
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x001716EC File Offset: 0x0016FAEC
		private void ScrollViewValueChanged(Vector2 newScrollValue)
		{
			float num = 1f - newScrollValue.y;
			this.m_scrollY = num * this.scrollableHeight;
			this.m_requiresRefresh = true;
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x0017171C File Offset: 0x0016FB1C
		private void RecalculateVisibleRowsFromScratch()
		{
			this.ClearAllRows();
			this.SetInitialVisibleRows();
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x0017172A File Offset: 0x0016FB2A
		private void ClearAllRows()
		{
			while (this.m_visibleCells.Count > 0)
			{
				this.HideRow(false);
			}
			this.m_visibleRowRange = new Range(0, 0);
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x00171756 File Offset: 0x0016FB56
		private void Awake()
		{
			if (!this.m_isInitialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x0017176C File Offset: 0x0016FB6C
		private void Initialize()
		{
			this.isEmpty = true;
			this.m_scrollRect = base.GetComponent<ScrollRect>();
			this.m_verticalLayoutGroup = base.GetComponentInChildren<VerticalLayoutGroup>();
			this.m_topPadding = this.CreateEmptyPaddingElement("TopPadding");
			this.m_topPadding.transform.SetParent(this.m_scrollRect.content, false);
			this.m_bottomPadding = this.CreateEmptyPaddingElement("Bottom");
			this.m_bottomPadding.transform.SetParent(this.m_scrollRect.content, false);
			this.m_visibleCells = new Dictionary<int, TableViewCell>();
			this.m_reusableCellContainer = new GameObject("ReusableCells", new Type[]
			{
				typeof(RectTransform)
			}).GetComponent<RectTransform>();
			this.m_reusableCellContainer.SetParent(base.transform, false);
			this.m_reusableCellContainer.gameObject.SetActive(false);
			this.m_reusableCells = new Dictionary<string, LinkedList<TableViewCell>>();
			this.m_isInitialized = true;
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x0017185A File Offset: 0x0016FC5A
		private void Update()
		{
			if (this.m_requiresReload)
			{
				this.ReloadData();
			}
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x0017186D File Offset: 0x0016FC6D
		private void LateUpdate()
		{
			if (this.m_requiresRefresh)
			{
				this.RefreshVisibleRows();
			}
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x00171880 File Offset: 0x0016FC80
		private void OnEnable()
		{
			this.m_scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.ScrollViewValueChanged));
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x0017189E File Offset: 0x0016FC9E
		private void OnDisable()
		{
			this.m_scrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.ScrollViewValueChanged));
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x001718BC File Offset: 0x0016FCBC
		private Range CalculateCurrentVisibleRowRange()
		{
			float scrollY = this.m_scrollY;
			float y = this.m_scrollY + (base.transform as RectTransform).rect.height;
			int num = this.FindIndexOfRowAtY(scrollY);
			int num2 = this.FindIndexOfRowAtY(y);
			return new Range(num, num2 - num + 1);
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x00171910 File Offset: 0x0016FD10
		private void SetInitialVisibleRows()
		{
			Range visibleRowRange = this.CalculateCurrentVisibleRowRange();
			for (int i = 0; i < visibleRowRange.count; i++)
			{
				this.AddRow(visibleRowRange.from + i, true);
			}
			this.m_visibleRowRange = visibleRowRange;
			this.UpdatePaddingElements();
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x0017195C File Offset: 0x0016FD5C
		private void AddRow(int row, bool atEnd)
		{
			TableViewCell cellForRowInTableView = this.m_dataSource.GetCellForRowInTableView(this, row);
			cellForRowInTableView.transform.SetParent(this.m_scrollRect.content, false);
			LayoutElement layoutElement = cellForRowInTableView.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = cellForRowInTableView.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.preferredHeight = this.m_rowHeights[row];
			if (row > 0)
			{
				layoutElement.preferredHeight -= this.m_verticalLayoutGroup.spacing;
			}
			this.m_visibleCells[row] = cellForRowInTableView;
			if (atEnd)
			{
				cellForRowInTableView.transform.SetSiblingIndex(this.m_scrollRect.content.childCount - 2);
			}
			else
			{
				cellForRowInTableView.transform.SetSiblingIndex(1);
			}
			this.onCellVisibilityChanged.Invoke(row, true);
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x00171A28 File Offset: 0x0016FE28
		private void RefreshVisibleRows()
		{
			this.m_requiresRefresh = false;
			if (this.isEmpty)
			{
				return;
			}
			Range range = this.CalculateCurrentVisibleRowRange();
			int num = this.m_visibleRowRange.Last();
			int num2 = range.Last();
			if (range.from > num || num2 < this.m_visibleRowRange.from)
			{
				this.RecalculateVisibleRowsFromScratch();
				return;
			}
			for (int i = this.m_visibleRowRange.from; i < range.from; i++)
			{
				this.HideRow(false);
			}
			for (int j = num2; j < num; j++)
			{
				this.HideRow(true);
			}
			for (int k = this.m_visibleRowRange.from - 1; k >= range.from; k--)
			{
				this.AddRow(k, false);
			}
			for (int l = num + 1; l <= num2; l++)
			{
				this.AddRow(l, true);
			}
			this.m_visibleRowRange = range;
			this.UpdatePaddingElements();
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x00171B2C File Offset: 0x0016FF2C
		private void UpdatePaddingElements()
		{
			float num = 0f;
			for (int i = 0; i < this.m_visibleRowRange.from; i++)
			{
				num += this.m_rowHeights[i];
			}
			this.m_topPadding.preferredHeight = num;
			this.m_topPadding.gameObject.SetActive(this.m_topPadding.preferredHeight > 0f);
			for (int j = this.m_visibleRowRange.from; j <= this.m_visibleRowRange.Last(); j++)
			{
				num += this.m_rowHeights[j];
			}
			float num2 = this.m_scrollRect.content.rect.height - num;
			this.m_bottomPadding.preferredHeight = num2 - this.m_verticalLayoutGroup.spacing;
			this.m_bottomPadding.gameObject.SetActive(this.m_bottomPadding.preferredHeight > 0f);
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x00171C1C File Offset: 0x0017001C
		private void HideRow(bool last)
		{
			int num = (!last) ? this.m_visibleRowRange.from : this.m_visibleRowRange.Last();
			TableViewCell cell = this.m_visibleCells[num];
			this.StoreCellForReuse(cell);
			this.m_visibleCells.Remove(num);
			this.m_visibleRowRange.count = this.m_visibleRowRange.count - 1;
			if (!last)
			{
				this.m_visibleRowRange.from = this.m_visibleRowRange.from + 1;
			}
			this.onCellVisibilityChanged.Invoke(num, false);
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x00171CA8 File Offset: 0x001700A8
		private LayoutElement CreateEmptyPaddingElement(string name)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			});
			return gameObject.GetComponent<LayoutElement>();
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x00171CE4 File Offset: 0x001700E4
		private int FindIndexOfRowAtY(float y)
		{
			return this.FindIndexOfRowAtY(y, 0, this.m_cumulativeRowHeights.Length - 1);
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x00171CF8 File Offset: 0x001700F8
		private int FindIndexOfRowAtY(float y, int startIndex, int endIndex)
		{
			if (startIndex >= endIndex)
			{
				return startIndex;
			}
			int num = (startIndex + endIndex) / 2;
			if (this.GetCumulativeRowHeight(num) >= y)
			{
				return this.FindIndexOfRowAtY(y, startIndex, num);
			}
			return this.FindIndexOfRowAtY(y, num + 1, endIndex);
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x00171D38 File Offset: 0x00170138
		private float GetCumulativeRowHeight(int row)
		{
			while (this.m_cleanCumulativeIndex < row)
			{
				this.m_cleanCumulativeIndex++;
				this.m_cumulativeRowHeights[this.m_cleanCumulativeIndex] = this.m_rowHeights[this.m_cleanCumulativeIndex];
				if (this.m_cleanCumulativeIndex > 0)
				{
					this.m_cumulativeRowHeights[this.m_cleanCumulativeIndex] += this.m_cumulativeRowHeights[this.m_cleanCumulativeIndex - 1];
				}
			}
			return this.m_cumulativeRowHeights[row];
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00171DB8 File Offset: 0x001701B8
		private void StoreCellForReuse(TableViewCell cell)
		{
			string reuseIdentifier = cell.reuseIdentifier;
			if (string.IsNullOrEmpty(reuseIdentifier))
			{
				UnityEngine.Object.Destroy(cell.gameObject);
				return;
			}
			if (!this.m_reusableCells.ContainsKey(reuseIdentifier))
			{
				this.m_reusableCells.Add(reuseIdentifier, new LinkedList<TableViewCell>());
			}
			this.m_reusableCells[reuseIdentifier].AddLast(cell);
			cell.transform.SetParent(this.m_reusableCellContainer, false);
		}

		// Token: 0x04002EF6 RID: 12022
		public TableView.CellVisibilityChangeEvent onCellVisibilityChanged;

		// Token: 0x04002EF8 RID: 12024
		private ITableViewDataSource m_dataSource;

		// Token: 0x04002EF9 RID: 12025
		private bool m_requiresReload;

		// Token: 0x04002EFA RID: 12026
		private VerticalLayoutGroup m_verticalLayoutGroup;

		// Token: 0x04002EFB RID: 12027
		private ScrollRect m_scrollRect;

		// Token: 0x04002EFC RID: 12028
		private LayoutElement m_topPadding;

		// Token: 0x04002EFD RID: 12029
		private LayoutElement m_bottomPadding;

		// Token: 0x04002EFE RID: 12030
		private float[] m_rowHeights;

		// Token: 0x04002EFF RID: 12031
		private float[] m_cumulativeRowHeights;

		// Token: 0x04002F00 RID: 12032
		private int m_cleanCumulativeIndex;

		// Token: 0x04002F01 RID: 12033
		private Dictionary<int, TableViewCell> m_visibleCells;

		// Token: 0x04002F02 RID: 12034
		private Range m_visibleRowRange;

		// Token: 0x04002F03 RID: 12035
		private RectTransform m_reusableCellContainer;

		// Token: 0x04002F04 RID: 12036
		private Dictionary<string, LinkedList<TableViewCell>> m_reusableCells;

		// Token: 0x04002F05 RID: 12037
		private float m_scrollY;

		// Token: 0x04002F06 RID: 12038
		private bool m_requiresRefresh;

		// Token: 0x04002F07 RID: 12039
		private bool m_isInitialized;

		// Token: 0x020008DF RID: 2271
		[Serializable]
		public class CellVisibilityChangeEvent : UnityEvent<int, bool>
		{
		}
	}
}
