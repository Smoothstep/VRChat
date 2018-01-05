using System;
using System.Collections.Generic;
using System.Linq;
using EasyLayout;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x02000946 RID: 2374
	public class ListViewCustom<TComponent, TItem> : ListViewBase where TComponent : ListViewItem
	{
		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x060047A6 RID: 18342 RVA: 0x0017AD7F File Offset: 0x0017917F
		// (set) Token: 0x060047A7 RID: 18343 RVA: 0x0017AD8C File Offset: 0x0017918C
		public new List<TItem> Items
		{
			get
			{
				return new List<TItem>(this.customItems);
			}
			set
			{
				this.UpdateItems(new List<TItem>(value));
				if (this.scrollRect != null)
				{
					this.scrollRect.verticalScrollbar.value = 1f;
				}
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x060047A8 RID: 18344 RVA: 0x0017ADC0 File Offset: 0x001791C0
		public TItem SelectedItem
		{
			get
			{
				if (base.SelectedIndex == -1)
				{
					return default(TItem);
				}
				return this.customItems[base.SelectedIndex];
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x060047A9 RID: 18345 RVA: 0x0017ADF4 File Offset: 0x001791F4
		public List<TItem> SelectedItems
		{
			get
			{
				if (base.SelectedIndex == -1)
				{
					return null;
				}
				return base.SelectedIndicies.ConvertAll<TItem>((int x) => this.customItems[x]);
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x060047AA RID: 18346 RVA: 0x0017AE1B File Offset: 0x0017921B
		public TComponent SelectedComponent
		{
			get
			{
				if (base.SelectedIndex == -1)
				{
					return (TComponent)((object)null);
				}
				return this.components[base.SelectedIndex];
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x060047AB RID: 18347 RVA: 0x0017AE41 File Offset: 0x00179241
		public List<TComponent> SelectedComponents
		{
			get
			{
				if (base.SelectedIndex == -1)
				{
					return null;
				}
				return base.SelectedIndicies.ConvertAll<TComponent>((int x) => this.components[x]);
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x060047AC RID: 18348 RVA: 0x0017AE68 File Offset: 0x00179268
		// (set) Token: 0x060047AD RID: 18349 RVA: 0x0017AE70 File Offset: 0x00179270
		public bool Sort
		{
			get
			{
				return this.sort;
			}
			set
			{
				this.sort = value;
				if (this.Sort && this.isStartedListViewCustom)
				{
					this.UpdateItems();
				}
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x060047AE RID: 18350 RVA: 0x0017AE95 File Offset: 0x00179295
		// (set) Token: 0x060047AF RID: 18351 RVA: 0x0017AE9D File Offset: 0x0017929D
		public Func<List<TItem>, List<TItem>> SortFunc
		{
			get
			{
				return this.sortFunc;
			}
			set
			{
				this.sortFunc = value;
				if (this.Sort && this.isStartedListViewCustom)
				{
					this.UpdateItems();
				}
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x060047B0 RID: 18352 RVA: 0x0017AEC2 File Offset: 0x001792C2
		// (set) Token: 0x060047B1 RID: 18353 RVA: 0x0017AECA File Offset: 0x001792CA
		public Color DefaultBackgroundColor
		{
			get
			{
				return this.defaultBackgroundColor;
			}
			set
			{
				this.defaultBackgroundColor = value;
				this.UpdateColors();
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x060047B2 RID: 18354 RVA: 0x0017AED9 File Offset: 0x001792D9
		// (set) Token: 0x060047B3 RID: 18355 RVA: 0x0017AEE1 File Offset: 0x001792E1
		public Color DefaultColor
		{
			get
			{
				return this.defaultColor;
			}
			set
			{
				this.DefaultColor = value;
				this.UpdateColors();
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x060047B4 RID: 18356 RVA: 0x0017AEF0 File Offset: 0x001792F0
		// (set) Token: 0x060047B5 RID: 18357 RVA: 0x0017AEF8 File Offset: 0x001792F8
		public Color SelectedBackgroundColor
		{
			get
			{
				return this.selectedBackgroundColor;
			}
			set
			{
				this.selectedBackgroundColor = value;
				this.UpdateColors();
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x060047B6 RID: 18358 RVA: 0x0017AF07 File Offset: 0x00179307
		// (set) Token: 0x060047B7 RID: 18359 RVA: 0x0017AF0F File Offset: 0x0017930F
		public Color SelectedColor
		{
			get
			{
				return this.selectedColor;
			}
			set
			{
				this.selectedColor = value;
				this.UpdateColors();
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x060047B8 RID: 18360 RVA: 0x0017AF1E File Offset: 0x0017931E
		// (set) Token: 0x060047B9 RID: 18361 RVA: 0x0017AF28 File Offset: 0x00179328
		public ScrollRect ScrollRect
		{
			get
			{
				return this.scrollRect;
			}
			set
			{
				if (this.scrollRect != null)
				{
					this.scrollRect.verticalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.OnScroll));
				}
				this.scrollRect = value;
				if (this.scrollRect != null)
				{
					this.scrollRect.verticalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.OnScroll));
				}
			}
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x0017AFA2 File Offset: 0x001793A2
		protected virtual void Awake()
		{
			this.Start();
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x060047BB RID: 18363 RVA: 0x0017AFAA File Offset: 0x001793AA
		public EasyLayout.EasyLayout Layout
		{
			get
			{
				return this.layout;
			}
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x0017AFB4 File Offset: 0x001793B4
		public override void Start()
		{
			if (this.isStartedListViewCustom)
			{
				return;
			}
			this.isStartedListViewCustom = true;
			base.Start();
			this.DestroyGameObjects = false;
			if (this.DefaultItem == null)
			{
				throw new NullReferenceException(string.Format("DefaultItem is null. Set component of type {0} to DefaultItem.", typeof(TComponent).FullName));
			}
			this.DefaultItem.gameObject.SetActive(true);
			GameObject gameObject = new GameObject("top filler");
			gameObject.transform.SetParent(this.Container);
			this.topFiller = gameObject.AddComponent<RectTransform>();
			this.topFiller.SetAsFirstSibling();
			this.topFiller.localScale = Vector3.one;
			GameObject gameObject2 = new GameObject("bottom filler");
			gameObject2.transform.SetParent(this.Container);
			this.bottomFiller = gameObject2.AddComponent<RectTransform>();
			this.bottomFiller.localScale = Vector3.one;
			if (this.CanOptimize())
			{
				this.ScrollRect = this.scrollRect;
				this.scrollHeight = this.scrollRect.GetComponent<RectTransform>().rect.height;
				if (this.itemHeight == 0f)
				{
					RectTransform component = this.DefaultItem.GetComponent<RectTransform>();
					float preferredSize = LayoutUtility.GetPreferredSize(component, 1);
					this.itemHeight = ((preferredSize > 0f) ? preferredSize : component.rect.height);
				}
				this.layout = this.Container.GetComponent<EasyLayout.EasyLayout>();
				this.CalculateMaxVisibleItems();
				ResizeListener resizeListener = this.scrollRect.gameObject.AddComponent<ResizeListener>();
				resizeListener.OnResize.AddListener(new UnityAction(this.Resize));
			}
			this.customItems = this.SortItems(this.customItems);
			this.UpdateView();
			this.DefaultItem.gameObject.SetActive(false);
			this.OnSelect.AddListener(new UnityAction<int, ListViewItem>(this.OnSelectCallback));
			this.OnDeselect.AddListener(new UnityAction<int, ListViewItem>(this.OnDeselectCallback));
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x0017B1CF File Offset: 0x001795CF
		protected virtual void CalculateMaxVisibleItems()
		{
			this.maxVisibleItems = (int)Math.Ceiling((double)(this.scrollHeight / this.itemHeight)) + 1;
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x0017B1F0 File Offset: 0x001795F0
		protected virtual void Resize()
		{
			this.scrollHeight = this.scrollRect.GetComponent<RectTransform>().rect.height;
			this.CalculateMaxVisibleItems();
			this.UpdateView();
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x0017B227 File Offset: 0x00179627
		protected bool CanOptimize()
		{
			return this.scrollRect != null && (this.layout != null || this.Container.GetComponent<EasyLayout.EasyLayout>() != null);
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x0017B262 File Offset: 0x00179662
		private void OnSelectCallback(int index, ListViewItem item)
		{
			this.OnSelectObject.Invoke(index);
			if (item != null)
			{
				this.SelectColoring(item);
			}
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x0017B283 File Offset: 0x00179683
		private void OnDeselectCallback(int index, ListViewItem item)
		{
			this.OnDeselectObject.Invoke(index);
			if (item != null)
			{
				this.DefaultColoring(item);
			}
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x0017B2A4 File Offset: 0x001796A4
		private void OnPointerEnterCallback(ListViewItem item)
		{
			this.OnPointerEnterObject.Invoke(item.Index);
			if (!base.IsSelected(item.Index))
			{
				this.HighlightColoring(item);
			}
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x0017B2CF File Offset: 0x001796CF
		private void OnPointerExitCallback(ListViewItem item)
		{
			this.OnPointerExitObject.Invoke(item.Index);
			if (!base.IsSelected(item.Index))
			{
				this.DefaultColoring(item);
			}
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x0017B2FA File Offset: 0x001796FA
		public override void UpdateItems()
		{
			this.UpdateItems(this.customItems);
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x0017B308 File Offset: 0x00179708
		public override void Clear()
		{
			this.UpdateItems(new List<TItem>());
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x0017B318 File Offset: 0x00179718
		public virtual int Add(TItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("Item is null.");
			}
			List<TItem> list = this.customItems;
			list.Add(item);
			this.UpdateItems(list);
			return this.customItems.FindIndex((TItem x) => object.ReferenceEquals(x, item));
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x0017B380 File Offset: 0x00179780
		public virtual int Remove(TItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("Item is null.");
			}
			int num = this.customItems.FindIndex((TItem x) => object.ReferenceEquals(x, item));
			if (num == -1)
			{
				return num;
			}
			this.Remove(num);
			return num;
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x0017B3E0 File Offset: 0x001797E0
		public virtual void Remove(int index)
		{
			List<TItem> list = this.customItems;
			list.RemoveAt(index);
			this.UpdateItems(list);
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x0017B404 File Offset: 0x00179804
		private void RemoveCallback(ListViewItem item, int index)
		{
			if (item == null)
			{
				return;
			}
			if (this.callbacksEnter.Count > index)
			{
				item.onPointerEnter.RemoveListener(this.callbacksEnter[index]);
			}
			if (this.callbacksExit.Count > index)
			{
				item.onPointerExit.RemoveListener(this.callbacksExit[index]);
			}
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x0017B470 File Offset: 0x00179870
		protected override void ScrollTo(int index)
		{
			if (!this.CanOptimize())
			{
				return;
			}
			int firstVisibleIndex = this.GetFirstVisibleIndex(true);
			int lastVisibleIndex = this.GetLastVisibleIndex(true);
			if (firstVisibleIndex > index)
			{
				float num = (float)index * (this.itemHeight + this.layout.Spacing.y);
				float num2 = 1f - this.scrollRect.verticalScrollbar.size - num / this.FullHeight();
				float value = num2 / (1f - this.scrollRect.verticalScrollbar.size);
				this.scrollRect.verticalScrollbar.value = value;
			}
			else if (lastVisibleIndex < index)
			{
				float num3 = (float)(index + 1) * (this.itemHeight + this.layout.Spacing.y) - this.layout.Spacing.y + this.layout.GetMarginTop();
				float num4 = (this.FullHeight() - num3) / this.FullHeight();
				float value2 = num4 / (1f - this.scrollRect.verticalScrollbar.size);
				this.scrollRect.verticalScrollbar.value = value2;
			}
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x0017B58D File Offset: 0x0017998D
		private void RemoveCallbacks()
		{
			base.Items.ForEach(new Action<ListViewItem, int>(this.RemoveCallback));
			this.callbacksEnter.Clear();
			this.callbacksExit.Clear();
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x0017B5BC File Offset: 0x001799BC
		private void AddCallbacks()
		{
			base.Items.ForEach(new Action<ListViewItem, int>(this.AddCallback));
		}

		// Token: 0x060047CD RID: 18381 RVA: 0x0017B5D8 File Offset: 0x001799D8
		private void AddCallback(ListViewItem item, int index)
		{
			this.callbacksEnter.Add(delegate(PointerEventData ev)
			{
				this.OnPointerEnterCallback(item);
			});
			this.callbacksExit.Add(delegate(PointerEventData ev)
			{
				this.OnPointerExitCallback(item);
			});
			item.onPointerEnter.AddListener(this.callbacksEnter[this.callbacksEnter.Count - 1]);
			item.onPointerExit.AddListener(this.callbacksExit[this.callbacksExit.Count - 1]);
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x0017B677 File Offset: 0x00179A77
		private List<TItem> SortItems(List<TItem> newItems)
		{
			if (this.Sort && this.SortFunc != null)
			{
				return this.SortFunc(newItems);
			}
			return new List<TItem>(newItems);
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x0017B6A2 File Offset: 0x00179AA2
		protected virtual void SetData(TComponent component, TItem item)
		{
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x0017B6A4 File Offset: 0x00179AA4
		private List<TComponent> GetNewComponents()
		{
			this.componentsCache = (from x in this.componentsCache
			where x != null
			select x).ToList<TComponent>();
			List<TComponent> new_components = new List<TComponent>();
			this.customItems.ForEach(delegate(TItem x, int i)
			{
				if (i >= this.visibleItems)
				{
					return;
				}
				if (this.components.Count > 0)
				{
					new_components.Add(this.components[0]);
					this.components.RemoveAt(0);
				}
				else if (this.componentsCache.Count > 0)
				{
					TComponent tcomponent = this.componentsCache[0];
					tcomponent.gameObject.SetActive(true);
					new_components.Add(this.componentsCache[0]);
					this.componentsCache.RemoveAt(0);
				}
				else
				{
					TComponent tcomponent2 = UnityEngine.Object.Instantiate<TComponent>(this.DefaultItem);
					Utilites.FixInstantiated(this.DefaultItem, tcomponent2);
					tcomponent2.gameObject.SetActive(true);
					new_components.Add(tcomponent2);
				}
			});
			this.components.ForEach(delegate(TComponent x)
			{
				x.gameObject.SetActive(false);
			});
			this.componentsCache.AddRange(this.components);
			this.components.Clear();
			return new_components;
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x0017B760 File Offset: 0x00179B60
		protected float FullHeight()
		{
			return this.layout.BlockSize[1];
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x0017B784 File Offset: 0x00179B84
		protected float GetScrollMargin()
		{
			float num = (1f - this.scrollRect.verticalScrollbar.size) * (1f - this.scrollRect.verticalScrollbar.value);
			return this.FullHeight() * num;
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x0017B7C8 File Offset: 0x00179BC8
		protected virtual int GetLastVisibleIndex(bool strict = false)
		{
			float num = this.GetScrollMargin() + this.scrollHeight;
			int num2 = (!strict) ? ((int)Math.Ceiling((double)(num / (this.itemHeight + this.layout.Spacing.y)))) : ((int)Math.Floor((double)(num / (this.itemHeight + this.layout.Spacing.y))));
			return num2 - 1;
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x0017B834 File Offset: 0x00179C34
		protected virtual int GetFirstVisibleIndex(bool strict = false)
		{
			int num = (!strict) ? ((int)Math.Floor((double)(this.GetScrollMargin() / (this.itemHeight + this.layout.Spacing.y)))) : ((int)Math.Ceiling((double)(this.GetScrollMargin() / (this.itemHeight + this.layout.Spacing.y))));
			if (strict)
			{
				return num;
			}
			return Math.Min(num, Math.Max(0, this.customItems.Count - this.visibleItems));
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x0017B8C0 File Offset: 0x00179CC0
		protected virtual void OnScroll(float value)
		{
			int num = this.topHiddenItems;
			this.topHiddenItems = this.GetFirstVisibleIndex(false);
			this.bottomHiddenItems = Math.Max(0, this.customItems.Count - this.visibleItems - this.topHiddenItems);
			if (num != this.topHiddenItems)
			{
				if (num == this.topHiddenItems + 1)
				{
					TComponent tcomponent = this.components[this.components.Count - 1];
					this.components.RemoveAt(this.components.Count - 1);
					this.components.Insert(0, tcomponent);
					tcomponent.transform.SetAsFirstSibling();
					tcomponent.Index = this.topHiddenItems;
					this.SetData(tcomponent, this.customItems[this.topHiddenItems]);
					this.Coloring(tcomponent);
				}
				else if (num == this.topHiddenItems - 1)
				{
					TComponent tcomponent2 = this.components[0];
					this.components.RemoveAt(0);
					this.components.Add(tcomponent2);
					tcomponent2.transform.SetAsLastSibling();
					tcomponent2.Index = this.topHiddenItems + this.visibleItems - 1;
					this.SetData(tcomponent2, this.customItems[this.topHiddenItems + this.visibleItems - 1]);
					this.Coloring(tcomponent2);
				}
				else
				{
					int[] new_indicies = Enumerable.Range(this.topHiddenItems, this.visibleItems).ToArray<int>();
					this.components.ForEach(delegate(TComponent x, int i)
					{
						x.Index = new_indicies[i];
						this.SetData(x, this.customItems[new_indicies[i]]);
						this.Coloring(x);
					});
				}
			}
			this.SetTopFiller();
			this.SetBottomFiller();
			if (this.layout)
			{
				this.layout.UpdateLayout();
			}
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0017BAA8 File Offset: 0x00179EA8
		protected void UpdateView()
		{
			this.RemoveCallbacks();
			if (this.CanOptimize() && this.customItems.Count > 0)
			{
				this.visibleItems = ((this.maxVisibleItems >= this.customItems.Count) ? this.customItems.Count : this.maxVisibleItems);
			}
			else
			{
				this.visibleItems = this.customItems.Count;
			}
			this.components = this.GetNewComponents();
			List<ListViewItem> base_items = new List<ListViewItem>();
			this.components.ForEach(delegate(TComponent x)
			{
				base_items.Add(x);
			});
			base.Items = base_items;
			this.components.ForEach(delegate(TComponent x, int i)
			{
				x.Index = i;
				this.SetData(x, this.customItems[i]);
				this.Coloring(x);
			});
			this.AddCallbacks();
			this.topHiddenItems = 0;
			this.bottomHiddenItems = this.customItems.Count<TItem>() - this.visibleItems;
			this.SetTopFiller();
			this.SetBottomFiller();
			if (this.layout)
			{
				this.layout.NeedUpdateLayout();
			}
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0017BBCC File Offset: 0x00179FCC
		protected virtual void UpdateItems(List<TItem> newItems)
		{
			newItems = this.SortItems(newItems);
			base.SelectedIndicies.ForEach(delegate(int index)
			{
				int num = newItems.FindIndex((TItem x) => x.Equals(this.customItems[index]));
				if (num == -1)
				{
					this.Deselect(index);
				}
			});
			this.customItems = newItems;
			this.UpdateView();
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x0017BC28 File Offset: 0x0017A028
		protected virtual float CalculateBottomFillerHeight()
		{
			return (float)this.bottomHiddenItems * (this.itemHeight + this.layout.Spacing.y) - this.layout.Spacing.y;
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x0017BC5A File Offset: 0x0017A05A
		protected virtual float CalculateTopFillerHeight()
		{
			return (float)this.topHiddenItems * (this.itemHeight + this.layout.Spacing.y) - this.layout.Spacing.y;
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x0017BC8C File Offset: 0x0017A08C
		private void SetBottomFiller()
		{
			this.bottomFiller.SetAsLastSibling();
			if (this.bottomHiddenItems == 0)
			{
				this.bottomFiller.gameObject.SetActive(false);
			}
			else
			{
				this.bottomFiller.gameObject.SetActive(true);
				this.bottomFiller.sizeDelta = new Vector2(this.bottomFiller.sizeDelta.x, this.CalculateBottomFillerHeight());
			}
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x0017BD00 File Offset: 0x0017A100
		private void SetTopFiller()
		{
			this.topFiller.SetAsFirstSibling();
			if (this.topHiddenItems == 0)
			{
				this.topFiller.gameObject.SetActive(false);
			}
			else
			{
				this.topFiller.gameObject.SetActive(true);
				this.topFiller.sizeDelta = new Vector2(this.bottomFiller.sizeDelta.x, this.CalculateTopFillerHeight());
			}
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x0017BD73 File Offset: 0x0017A173
		public override bool IsValid(int index)
		{
			return index >= 0 && index < this.customItems.Count;
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x0017BD8D File Offset: 0x0017A18D
		protected override void Coloring(ListViewItem component)
		{
			if (component == null)
			{
				return;
			}
			if (base.SelectedIndicies.Contains(component.Index))
			{
				this.SelectColoring(component);
			}
			else
			{
				this.DefaultColoring(component);
			}
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x0017BDC5 File Offset: 0x0017A1C5
		protected override void HighlightColoring(ListViewItem component)
		{
			if (base.IsSelected(component.Index))
			{
				return;
			}
			this.HighlightColoring(component as TComponent);
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x0017BDEA File Offset: 0x0017A1EA
		protected virtual void HighlightColoring(TComponent component)
		{
			component.Background.color = this.HighlightedBackgroundColor;
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x0017BE02 File Offset: 0x0017A202
		protected virtual void SelectColoring(ListViewItem component)
		{
			if (component == null)
			{
				return;
			}
			this.SelectColoring(component as TComponent);
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x0017BE22 File Offset: 0x0017A222
		protected virtual void SelectColoring(TComponent component)
		{
			component.Background.color = this.SelectedBackgroundColor;
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x0017BE3A File Offset: 0x0017A23A
		protected virtual void DefaultColoring(ListViewItem component)
		{
			if (component == null)
			{
				return;
			}
			this.DefaultColoring(component as TComponent);
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x0017BE5A File Offset: 0x0017A25A
		protected virtual void DefaultColoring(TComponent component)
		{
			component.Background.color = this.DefaultBackgroundColor;
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x0017BE72 File Offset: 0x0017A272
		private void UpdateColors()
		{
			this.components.ForEach(delegate(TComponent x)
			{
				this.Coloring(x);
			});
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x0017BE8B File Offset: 0x0017A28B
		protected override void OnDestroy()
		{
			this.OnSelect.RemoveListener(new UnityAction<int, ListViewItem>(this.OnSelectCallback));
			this.OnDeselect.RemoveListener(new UnityAction<int, ListViewItem>(this.OnDeselectCallback));
			this.RemoveCallbacks();
			base.OnDestroy();
		}

		// Token: 0x040030DC RID: 12508
		[SerializeField]
		protected List<TItem> customItems = new List<TItem>();

		// Token: 0x040030DD RID: 12509
		[SerializeField]
		public TComponent DefaultItem;

		// Token: 0x040030DE RID: 12510
		private List<TComponent> components = new List<TComponent>();

		// Token: 0x040030DF RID: 12511
		private List<TComponent> componentsCache = new List<TComponent>();

		// Token: 0x040030E0 RID: 12512
		private List<UnityAction<PointerEventData>> callbacksEnter = new List<UnityAction<PointerEventData>>();

		// Token: 0x040030E1 RID: 12513
		private List<UnityAction<PointerEventData>> callbacksExit = new List<UnityAction<PointerEventData>>();

		// Token: 0x040030E2 RID: 12514
		[SerializeField]
		[FormerlySerializedAs("Sort")]
		private bool sort = true;

		// Token: 0x040030E3 RID: 12515
		private Func<List<TItem>, List<TItem>> sortFunc;

		// Token: 0x040030E4 RID: 12516
		public ListViewCustomEvent OnSelectObject = new ListViewCustomEvent();

		// Token: 0x040030E5 RID: 12517
		public ListViewCustomEvent OnDeselectObject = new ListViewCustomEvent();

		// Token: 0x040030E6 RID: 12518
		public ListViewCustomEvent OnPointerEnterObject = new ListViewCustomEvent();

		// Token: 0x040030E7 RID: 12519
		public ListViewCustomEvent OnPointerExitObject = new ListViewCustomEvent();

		// Token: 0x040030E8 RID: 12520
		[SerializeField]
		private Color defaultBackgroundColor = Color.white;

		// Token: 0x040030E9 RID: 12521
		[SerializeField]
		private Color defaultColor = Color.black;

		// Token: 0x040030EA RID: 12522
		[SerializeField]
		public Color HighlightedBackgroundColor = new Color(203f, 230f, 244f, 255f);

		// Token: 0x040030EB RID: 12523
		[SerializeField]
		public Color HighlightedColor = Color.black;

		// Token: 0x040030EC RID: 12524
		[SerializeField]
		private Color selectedBackgroundColor = new Color(53f, 83f, 227f, 255f);

		// Token: 0x040030ED RID: 12525
		[SerializeField]
		private Color selectedColor = Color.black;

		// Token: 0x040030EE RID: 12526
		[SerializeField]
		protected ScrollRect scrollRect;

		// Token: 0x040030EF RID: 12527
		private RectTransform topFiller;

		// Token: 0x040030F0 RID: 12528
		private RectTransform bottomFiller;

		// Token: 0x040030F1 RID: 12529
		[SerializeField]
		[Tooltip("Minimal height of item")]
		protected float itemHeight;

		// Token: 0x040030F2 RID: 12530
		protected float scrollHeight;

		// Token: 0x040030F3 RID: 12531
		protected int maxVisibleItems;

		// Token: 0x040030F4 RID: 12532
		protected int visibleItems;

		// Token: 0x040030F5 RID: 12533
		protected int topHiddenItems;

		// Token: 0x040030F6 RID: 12534
		protected int bottomHiddenItems;

		// Token: 0x040030F7 RID: 12535
		[NonSerialized]
		private bool isStartedListViewCustom;

		// Token: 0x040030F8 RID: 12536
		protected EasyLayout.EasyLayout layout;
	}
}
