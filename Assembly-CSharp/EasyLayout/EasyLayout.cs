using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EasyLayout
{
	// Token: 0x0200093A RID: 2362
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class EasyLayout : LayoutGroup
	{
		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x060046E1 RID: 18145 RVA: 0x001804D6 File Offset: 0x0017E8D6
		// (set) Token: 0x060046E2 RID: 18146 RVA: 0x001804DE File Offset: 0x0017E8DE
		public Vector2 BlockSize
		{
			get
			{
				return this._blockSize;
			}
			protected set
			{
				this._blockSize = value;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060046E3 RID: 18147 RVA: 0x001804E7 File Offset: 0x0017E8E7
		// (set) Token: 0x060046E4 RID: 18148 RVA: 0x001804EF File Offset: 0x0017E8EF
		public Vector2 UISize
		{
			get
			{
				return this._uiSize;
			}
			protected set
			{
				this._uiSize = value;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060046E5 RID: 18149 RVA: 0x001804F8 File Offset: 0x0017E8F8
		public override float minHeight
		{
			get
			{
				return this.BlockSize[1];
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x060046E6 RID: 18150 RVA: 0x00180514 File Offset: 0x0017E914
		public override float minWidth
		{
			get
			{
				return this.BlockSize[0];
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x060046E7 RID: 18151 RVA: 0x00180530 File Offset: 0x0017E930
		public override float preferredHeight
		{
			get
			{
				return this.BlockSize[1];
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060046E8 RID: 18152 RVA: 0x0018054C File Offset: 0x0017E94C
		public override float preferredWidth
		{
			get
			{
				return this.BlockSize[0];
			}
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x00180568 File Offset: 0x0017E968
		protected override void OnEnable()
		{
			this.rebuild = true;
			base.OnEnable();
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x00180577 File Offset: 0x0017E977
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x0018057F File Offset: 0x0017E97F
		protected override void OnDidApplyAnimationProperties()
		{
			this.rebuild = true;
			base.OnDidApplyAnimationProperties();
		}

		// Token: 0x060046EC RID: 18156 RVA: 0x0018058E File Offset: 0x0017E98E
		protected override void OnTransformParentChanged()
		{
			this.rebuild = true;
			base.OnTransformParentChanged();
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x0018059D File Offset: 0x0017E99D
		private void OnRectTransformRemoved()
		{
			this.rebuild = true;
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x001805A6 File Offset: 0x0017E9A6
		protected override void OnCanvasGroupChanged()
		{
			this.rebuild = true;
			base.OnCanvasGroupChanged();
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x001805B5 File Offset: 0x0017E9B5
		protected override void OnRectTransformDimensionsChange()
		{
			this.rebuild = true;
			base.OnRectTransformDimensionsChange();
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x001805C4 File Offset: 0x0017E9C4
		protected override void OnTransformChildrenChanged()
		{
			this.rebuild = true;
			base.OnTransformChildrenChanged();
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x001805D3 File Offset: 0x0017E9D3
		public override void SetLayoutHorizontal()
		{
			this.rebuild = true;
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x001805DC File Offset: 0x0017E9DC
		public override void SetLayoutVertical()
		{
			this.rebuild = true;
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x001805E5 File Offset: 0x0017E9E5
		public override void CalculateLayoutInputHorizontal()
		{
			this.rebuild = true;
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x001805EE File Offset: 0x0017E9EE
		public override void CalculateLayoutInputVertical()
		{
			this.rebuild = true;
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x001805F7 File Offset: 0x0017E9F7
		private void Update()
		{
			if (!this.rebuild)
			{
				return;
			}
			this.rebuild = false;
			this.UpdateLayout();
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x00180614 File Offset: 0x0017EA14
		private float GetLength(RectTransform ui, bool scaled = true)
		{
			if (scaled)
			{
				return (this.Stacking != Stackings.Horizontal) ? EasyLayout.ScaledHeight(ui) : EasyLayout.ScaledWidth(ui);
			}
			return (this.Stacking != Stackings.Horizontal) ? ui.rect.height : ui.rect.width;
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x00180670 File Offset: 0x0017EA70
		private Vector2 CalculateGroupSize(List<List<RectTransform>> group)
		{
			float x;
			if (this.LayoutType == LayoutTypes.Compact)
			{
				x = (from row in @group
				select (from z in row
				select EasyLayout.ScaledWidth(z) + this.Spacing.x).Sum()).Max() - this.Spacing.x;
			}
			else
			{
				float[] maxColumnsWidths = this.GetMaxColumnsWidths(group);
				x = maxColumnsWidths.Sum() + (float)maxColumnsWidths.Length * this.Spacing.x - this.Spacing.x;
			}
			float y = group.Select(delegate(List<RectTransform> row)
			{
				if (EasyLayout.f__mg0 == null)
				{
					EasyLayout.f__mg0 = new Func<RectTransform, float>(EasyLayout.ScaledHeight);
				}
				return row.Select(EasyLayout.f__mg0).Max() + this.Spacing.y;
			}).Sum() - this.Spacing.y;
			return new Vector2(x, y);
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x00180708 File Offset: 0x0017EB08
		public void NeedUpdateLayout()
		{
			this.rebuild = true;
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x00180714 File Offset: 0x0017EB14
		private void UpdateBlockSize()
		{
			if (this.Symmetric)
			{
				this.BlockSize = new Vector2(this.UISize.x + this.Margin.x * 2f, this.UISize.y + this.Margin.y * 2f);
			}
			else
			{
				this.BlockSize = new Vector2(this.UISize.x + this.MarginLeft + this.MarginRight, this.UISize.y + this.MarginLeft + this.MarginRight);
			}
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x001807C0 File Offset: 0x0017EBC0
		public void UpdateLayout()
		{
			List<List<RectTransform>> list = this.GroupUIElements();
			if (list.Count == 0)
			{
				this.UISize = new Vector2(0f, 0f);
				this.UpdateBlockSize();
				return;
			}
			this.UISize = this.CalculateGroupSize(list);
			this.UpdateBlockSize();
			Vector2 vector = EasyLayout.groupPositions[this.GroupPosition];
			Vector2 startPosition = new Vector2(base.rectTransform.rect.width * (vector.x - base.rectTransform.pivot.x), base.rectTransform.rect.height * (vector.y - base.rectTransform.pivot.y));
			startPosition.x -= vector.x * this.UISize.x;
			startPosition.y += (1f - vector.y) * this.UISize.y;
			startPosition.x += this.GetMarginLeft() * ((1f - vector.x) * 2f - 1f);
			startPosition.y += this.GetMarginTop() * ((1f - vector.y) * 2f - 1f);
			this.SetPositions(list, startPosition, this.UISize);
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x00180944 File Offset: 0x0017ED44
		private Vector2 GetUIPosition(RectTransform ui, Vector2 position, Vector2 align)
		{
			float num = EasyLayout.ScaledWidth(ui) * ui.pivot.x;
			float num2 = EasyLayout.ScaledHeight(ui) * ui.pivot.y;
			float x = position.x + num + align.x;
			float y = position.y - EasyLayout.ScaledHeight(ui) + num2 - align.y;
			return new Vector2(x, y);
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x001809B4 File Offset: 0x0017EDB4
		private List<float> GetRowsWidths(IList<List<RectTransform>> group)
		{
			List<float> list = new List<float>();
			foreach (int index in Enumerable.Range(0, group.Count))
			{
				IEnumerable<RectTransform> source = group[index];
				if (EasyLayout.f__mg1 == null)
				{
					EasyLayout.f__mg1 = new Func<RectTransform, float>(EasyLayout.ScaledWidth);
				}
				List<float> list2 = source.Select(EasyLayout.f__mg1).ToList<float>();
				list.Add(list2.Sum() + (float)(list2.Count - 1) * this.Spacing.x);
			}
			return list;
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x00180A64 File Offset: 0x0017EE64
		private float[] GetMaxColumnsWidths(List<List<RectTransform>> group)
		{
			return EasyLayout.Transpose<RectTransform>(group).Select(delegate(List<RectTransform> column)
			{
				if (EasyLayout.f__mg2 == null)
				{
					EasyLayout.f__mg2 = new Func<RectTransform, float>(EasyLayout.ScaledWidth);
				}
				return column.Select(EasyLayout.f__mg2).Max();
			}).ToArray<float>();
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x00180A94 File Offset: 0x0017EE94
		private Vector2 GetMaxCellSize(List<List<RectTransform>> group)
		{
			List<Vector2> source = (from row in @group
			select this.GetMaxCellSize(row)).ToList<Vector2>();
			return new Vector2((from x in source
			select x.x).Max(), (from x in source
			select x.y).Max());
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00180B10 File Offset: 0x0017EF10
		private Vector2 GetMaxCellSize(List<RectTransform> row)
		{
			if (EasyLayout.f__mg3 == null)
			{
				EasyLayout.f__mg3 = new Func<RectTransform, float>(EasyLayout.ScaledWidth);
			}
			float x = row.Select(EasyLayout.f__mg3).Max();
			if (EasyLayout.f__mg4 == null)
			{
				EasyLayout.f__mg4 = new Func<RectTransform, float>(EasyLayout.ScaledHeight);
			}
			return new Vector2(x, row.Select(EasyLayout.f__mg4).Max());
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00180B74 File Offset: 0x0017EF74
		private Vector2 GetAlign(RectTransform ui, float maxWidth, Vector2 cellMaxSize, float emptyWidth)
		{
			if (this.LayoutType == LayoutTypes.Compact)
			{
				return new Vector2(emptyWidth * EasyLayout.rowAligns[this.RowAlign], (cellMaxSize.y - EasyLayout.ScaledHeight(ui)) * EasyLayout.innerAligns[this.InnerAlign]);
			}
			Vector2 vector = EasyLayout.groupPositions[this.CellAlign];
			return new Vector2((maxWidth - EasyLayout.ScaledWidth(ui)) * vector.x, (cellMaxSize.y - EasyLayout.ScaledHeight(ui)) * (1f - vector.y));
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x00180C08 File Offset: 0x0017F008
		private void SetPositions(List<List<RectTransform>> group, Vector2 startPosition, Vector2 groupSize)
		{
			Vector2 position = startPosition;
			List<float> rowsWidths = this.GetRowsWidths(group);
			float[] maxColumnsWidths = this.GetMaxColumnsWidths(group);
			Vector2 align = new Vector2(0f, 0f);
			int num = 0;
			foreach (List<RectTransform> list in group)
			{
				Vector2 maxCellSize = this.GetMaxCellSize(list);
				int num2 = 0;
				foreach (RectTransform rectTransform in list)
				{
					align = this.GetAlign(rectTransform, maxColumnsWidths[num2], maxCellSize, groupSize.x - rowsWidths[num]);
					Vector2 uiposition = this.GetUIPosition(rectTransform, position, align);
					if (rectTransform.localPosition.x != uiposition.x || rectTransform.localPosition.y != uiposition.y)
					{
						rectTransform.localPosition = uiposition;
						rectTransform.localRotation = Quaternion.identity;
					}
					position.x += ((this.LayoutType != LayoutTypes.Compact) ? maxColumnsWidths[num2] : EasyLayout.ScaledWidth(rectTransform)) + this.Spacing.x;
					num2++;
				}
				position.x = startPosition.x;
				position.y -= maxCellSize.y + this.Spacing.y;
				num++;
			}
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x00180DD4 File Offset: 0x0017F1D4
		private List<RectTransform> GetUIElements()
		{
			List<RectTransform> list = new List<RectTransform>();
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					list.Add(transform as RectTransform);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (this.SkipInactive)
			{
				list = (from x in list
				where x.gameObject.activeSelf
				select x).ToList<RectTransform>();
			}
			if (this.Filter != null)
			{
				IEnumerable<GameObject> source = this.Filter(list.ConvertAll<GameObject>((RectTransform x) => x.gameObject));
				list = (from x in source
				select x.transform as RectTransform).ToList<RectTransform>();
			}
			return list.Where(delegate(RectTransform x)
			{
				ILayoutIgnorer component = x.GetComponent<ILayoutIgnorer>();
				return component == null || !component.ignoreLayout;
			}).ToList<RectTransform>();
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x00180F0C File Offset: 0x0017F30C
		private List<List<RectTransform>> CompactGrouping(List<RectTransform> uiElements, float baseLength)
		{
			float num = baseLength;
			float num2 = (this.Stacking != Stackings.Horizontal) ? this.Spacing.y : this.Spacing.x;
			List<List<RectTransform>> list = new List<List<RectTransform>>();
			List<RectTransform> list2 = new List<RectTransform>();
			foreach (RectTransform rectTransform in this.GetUIElements())
			{
				float length = this.GetLength(rectTransform, true);
				if (list2.Count == 0)
				{
					num -= length;
					list2.Add(rectTransform);
				}
				else if (num >= length + num2)
				{
					num -= length + num2;
					list2.Add(rectTransform);
				}
				else
				{
					list.Add(list2);
					num = baseLength;
					num -= length;
					list2 = new List<RectTransform>();
					list2.Add(rectTransform);
				}
			}
			if (list2.Count > 0)
			{
				list.Add(list2);
			}
			return list;
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x00181010 File Offset: 0x0017F410
		private int GetMaxColumnsCount(List<RectTransform> uiElements, float baseLength, int maxColumns)
		{
			float num = baseLength;
			float num2 = (this.Stacking != Stackings.Horizontal) ? this.Spacing.y : this.Spacing.x;
			bool flag = false;
			int num3 = maxColumns;
			int num4 = 0;
			foreach (RectTransform ui in uiElements)
			{
				float length = this.GetLength(ui, true);
				if (num4 == maxColumns)
				{
					flag = true;
					num3 = Math.Min(num3, num4);
					num4 = 1;
					num = baseLength - length;
				}
				else if (num4 == 0)
				{
					num4 = 1;
					num = baseLength - length;
				}
				else if (num >= length + num2)
				{
					num -= length + num2;
					num4++;
				}
				else
				{
					flag = true;
					num3 = Math.Min(num3, num4);
					num4 = 1;
					num = baseLength - length;
				}
			}
			if (!flag)
			{
				num3 = num4;
			}
			return num3;
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x0018110C File Offset: 0x0017F50C
		private List<List<RectTransform>> GridGrouping(List<RectTransform> uiElements, float baseLength, int maxColumns = 0)
		{
			int num = 999999;
			for (;;)
			{
				int maxColumnsCount = this.GetMaxColumnsCount(uiElements, baseLength, num);
				if (num == maxColumnsCount || maxColumnsCount == 1)
				{
					break;
				}
				num = maxColumnsCount;
			}
			List<List<RectTransform>> list = new List<List<RectTransform>>();
			List<RectTransform> list2 = new List<RectTransform>();
			int num2 = 0;
			foreach (RectTransform item in uiElements)
			{
				if (num2 > 0 && num2 % num == 0)
				{
					list.Add(list2);
					list2 = new List<RectTransform>();
				}
				list2.Add(item);
				num2++;
			}
			if (list2.Count > 0)
			{
				list.Add(list2);
			}
			return list;
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x001811D8 File Offset: 0x0017F5D8
		public float GetMarginTop()
		{
			return (!this.Symmetric) ? this.MarginTop : this.Margin.y;
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x001811FB File Offset: 0x0017F5FB
		public float GetMarginBottom()
		{
			return (!this.Symmetric) ? this.MarginBottom : this.Margin.y;
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x0018121E File Offset: 0x0017F61E
		public float GetMarginLeft()
		{
			return (!this.Symmetric) ? this.MarginLeft : this.Margin.x;
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x00181241 File Offset: 0x0017F641
		public float GetMarginRight()
		{
			return (!this.Symmetric) ? this.MarginRight : this.Margin.y;
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x00181264 File Offset: 0x0017F664
		private List<List<RectTransform>> GroupUIElements()
		{
			float num = this.GetLength(base.rectTransform, false);
			num -= ((this.Stacking != Stackings.Horizontal) ? (this.GetMarginTop() + this.GetMarginBottom()) : (this.GetMarginLeft() + this.GetMarginRight()));
			List<RectTransform> uielements = this.GetUIElements();
			List<List<RectTransform>> list = (this.LayoutType != LayoutTypes.Compact) ? this.GridGrouping(uielements, num, 0) : this.CompactGrouping(uielements, num);
			if (this.Stacking == Stackings.Vertical)
			{
				list = EasyLayout.Transpose<RectTransform>(list);
			}
			if (!this.TopToBottom)
			{
				list.Reverse();
			}
			if (this.RightToLeft)
			{
				list.ForEach(delegate(List<RectTransform> x)
				{
					x.Reverse();
				});
			}
			return list;
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x00181328 File Offset: 0x0017F728
		private static List<List<T>> Transpose<T>(List<List<T>> group)
		{
			List<List<T>> list = new List<List<T>>();
			int num = 0;
			foreach (List<T> list2 in group)
			{
				int num2 = 0;
				foreach (T item in list2)
				{
					if (list.Count <= num2)
					{
						list.Add(new List<T>());
					}
					list[num2].Add(item);
					num2++;
				}
				num++;
			}
			return list;
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x001813F8 File Offset: 0x0017F7F8
		private static void Log(IEnumerable<float> values)
		{
			Debug.Log("[" + string.Join("; ", (from x in values
			select x.ToString()).ToArray<string>()) + "]");
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x0018144C File Offset: 0x0017F84C
		private static float ScaledWidth(RectTransform ui)
		{
			return ui.rect.width * ui.localScale.x;
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x00181478 File Offset: 0x0017F878
		private static float ScaledHeight(RectTransform ui)
		{
			return ui.rect.height * ui.localScale.y;
		}

		// Token: 0x04003079 RID: 12409
		[SerializeField]
		public Anchors GroupPosition;

		// Token: 0x0400307A RID: 12410
		[SerializeField]
		public Stackings Stacking;

		// Token: 0x0400307B RID: 12411
		[SerializeField]
		public LayoutTypes LayoutType;

		// Token: 0x0400307C RID: 12412
		[SerializeField]
		public HorizontalAligns RowAlign;

		// Token: 0x0400307D RID: 12413
		[SerializeField]
		public InnerAligns InnerAlign;

		// Token: 0x0400307E RID: 12414
		[SerializeField]
		public Anchors CellAlign;

		// Token: 0x0400307F RID: 12415
		[SerializeField]
		public Vector2 Spacing = new Vector2(5f, 5f);

		// Token: 0x04003080 RID: 12416
		[SerializeField]
		public bool Symmetric = true;

		// Token: 0x04003081 RID: 12417
		[SerializeField]
		public Vector2 Margin = new Vector2(5f, 5f);

		// Token: 0x04003082 RID: 12418
		[SerializeField]
		public float MarginTop = 5f;

		// Token: 0x04003083 RID: 12419
		[SerializeField]
		public float MarginBottom = 5f;

		// Token: 0x04003084 RID: 12420
		[SerializeField]
		public float MarginLeft = 5f;

		// Token: 0x04003085 RID: 12421
		[SerializeField]
		public float MarginRight = 5f;

		// Token: 0x04003086 RID: 12422
		[SerializeField]
		public bool RightToLeft;

		// Token: 0x04003087 RID: 12423
		[SerializeField]
		public bool TopToBottom = true;

		// Token: 0x04003088 RID: 12424
		[SerializeField]
		public bool SkipInactive = true;

		// Token: 0x04003089 RID: 12425
		public Func<IEnumerable<GameObject>, IEnumerable<GameObject>> Filter;

		// Token: 0x0400308A RID: 12426
		private Vector2 _blockSize;

		// Token: 0x0400308B RID: 12427
		private Vector2 _uiSize;

		// Token: 0x0400308C RID: 12428
		private static readonly Dictionary<Anchors, Vector2> groupPositions = new Dictionary<Anchors, Vector2>
		{
			{
				Anchors.LowerLeft,
				new Vector2(0f, 0f)
			},
			{
				Anchors.LowerCenter,
				new Vector2(0.5f, 0f)
			},
			{
				Anchors.LowerRight,
				new Vector2(1f, 0f)
			},
			{
				Anchors.MiddleLeft,
				new Vector2(0f, 0.5f)
			},
			{
				Anchors.MiddleCenter,
				new Vector2(0.5f, 0.5f)
			},
			{
				Anchors.MiddleRight,
				new Vector2(1f, 0.5f)
			},
			{
				Anchors.UpperLeft,
				new Vector2(0f, 1f)
			},
			{
				Anchors.UpperCenter,
				new Vector2(0.5f, 1f)
			},
			{
				Anchors.UpperRight,
				new Vector2(1f, 1f)
			}
		};

		// Token: 0x0400308D RID: 12429
		private static readonly Dictionary<HorizontalAligns, float> rowAligns = new Dictionary<HorizontalAligns, float>
		{
			{
				HorizontalAligns.Left,
				0f
			},
			{
				HorizontalAligns.Center,
				0.5f
			},
			{
				HorizontalAligns.Right,
				1f
			}
		};

		// Token: 0x0400308E RID: 12430
		private static readonly Dictionary<InnerAligns, float> innerAligns = new Dictionary<InnerAligns, float>
		{
			{
				InnerAligns.Top,
				0f
			},
			{
				InnerAligns.Middle,
				0.5f
			},
			{
				InnerAligns.Bottom,
				1f
			}
		};

		// Token: 0x0400308F RID: 12431
		private bool rebuild;

		// Token: 0x04003090 RID: 12432
		[CompilerGenerated]
		private static Func<RectTransform, float> f__mg0;

		// Token: 0x04003091 RID: 12433
		[CompilerGenerated]
		private static Func<RectTransform, float> f__mg1;

		// Token: 0x04003092 RID: 12434
		[CompilerGenerated]
		private static Func<RectTransform, float> f__mg2;

		// Token: 0x04003096 RID: 12438
		[CompilerGenerated]
		private static Func<RectTransform, float> f__mg3;

		// Token: 0x04003097 RID: 12439
		[CompilerGenerated]
		private static Func<RectTransform, float> f__mg4;
	}
}
