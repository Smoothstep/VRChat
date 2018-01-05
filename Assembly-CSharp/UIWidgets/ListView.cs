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
	// Token: 0x02000941 RID: 2369
	[AddComponentMenu("UI/ListView", 250)]
	public class ListView : ListViewBase
	{
		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x0600472B RID: 18219 RVA: 0x00181882 File Offset: 0x0017FC82
		// (set) Token: 0x0600472C RID: 18220 RVA: 0x0018188F File Offset: 0x0017FC8F
		public List<string> Strings
		{
			get
			{
				return new List<string>(this.strings);
			}
			set
			{
				this.UpdateItems(new List<string>(value));
				if (this.scrollRect != null)
				{
					this.scrollRect.verticalScrollbar.value = 1f;
				}
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x0600472D RID: 18221 RVA: 0x001818C3 File Offset: 0x0017FCC3
		// (set) Token: 0x0600472E RID: 18222 RVA: 0x001818D0 File Offset: 0x0017FCD0
		public new List<string> Items
		{
			get
			{
				return new List<string>(this.strings);
			}
			set
			{
				this.UpdateItems(new List<string>(value));
				if (this.scrollRect != null)
				{
					this.scrollRect.verticalScrollbar.value = 1f;
				}
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x0600472F RID: 18223 RVA: 0x00181904 File Offset: 0x0017FD04
		// (set) Token: 0x06004730 RID: 18224 RVA: 0x0018190C File Offset: 0x0017FD0C
		public TextAsset File
		{
			get
			{
				return this.file;
			}
			set
			{
				this.file = value;
				if (this.file != null)
				{
					this.UpdateItems(this.file);
				}
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06004731 RID: 18225 RVA: 0x00181932 File Offset: 0x0017FD32
		// (set) Token: 0x06004732 RID: 18226 RVA: 0x0018193A File Offset: 0x0017FD3A
		public Color BackgroundColor
		{
			get
			{
				return this.backgroundColor;
			}
			set
			{
				this.backgroundColor = value;
				this.UpdateColors();
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06004733 RID: 18227 RVA: 0x00181949 File Offset: 0x0017FD49
		// (set) Token: 0x06004734 RID: 18228 RVA: 0x00181951 File Offset: 0x0017FD51
		public Color TextColor
		{
			get
			{
				return this.textColor;
			}
			set
			{
				this.textColor = value;
				this.UpdateColors();
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06004735 RID: 18229 RVA: 0x00181960 File Offset: 0x0017FD60
		// (set) Token: 0x06004736 RID: 18230 RVA: 0x00181968 File Offset: 0x0017FD68
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

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06004737 RID: 18231 RVA: 0x00181977 File Offset: 0x0017FD77
		// (set) Token: 0x06004738 RID: 18232 RVA: 0x0018197F File Offset: 0x0017FD7F
		public Color SelectedTextColor
		{
			get
			{
				return this.selectedTextColor;
			}
			set
			{
				this.selectedTextColor = value;
				this.UpdateColors();
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06004739 RID: 18233 RVA: 0x0018198E File Offset: 0x0017FD8E
		// (set) Token: 0x0600473A RID: 18234 RVA: 0x00181996 File Offset: 0x0017FD96
		public bool Sort
		{
			get
			{
				return this.sort;
			}
			set
			{
				this.sort = value;
				if (this.Sort && this.isStartedListView)
				{
					this.UpdateItems();
				}
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x0600473B RID: 18235 RVA: 0x001819BB File Offset: 0x0017FDBB
		// (set) Token: 0x0600473C RID: 18236 RVA: 0x001819C3 File Offset: 0x0017FDC3
		public Func<IEnumerable<string>, IEnumerable<string>> SortFunc
		{
			get
			{
				return this.sortFunc;
			}
			set
			{
				this.sortFunc = value;
				if (this.Sort && this.isStartedListView)
				{
					this.UpdateItems();
				}
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x0600473D RID: 18237 RVA: 0x001819E8 File Offset: 0x0017FDE8
		// (set) Token: 0x0600473E RID: 18238 RVA: 0x001819F0 File Offset: 0x0017FDF0
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

		// Token: 0x0600473F RID: 18239 RVA: 0x00181A68 File Offset: 0x0017FE68
		private void Awake()
		{
			this.Start();
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06004740 RID: 18240 RVA: 0x00181A70 File Offset: 0x0017FE70
		public EasyLayout.EasyLayout Layout
		{
			get
			{
				return this.layout;
			}
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x00181A78 File Offset: 0x0017FE78
		public override void Start()
		{
			if (this.isStartedListView)
			{
				return;
			}
			this.isStartedListView = true;
			base.Start();
			this.DestroyGameObjects = false;
			if (this.DefaultItem == null)
			{
				throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
			}
			this.DefaultItem.gameObject.SetActive(true);
			if (this.DefaultItem.GetComponentInChildren<Text>() == null)
			{
				throw new MissingComponentException("DefaultItem don't have child with 'Text' component. Add child with 'Text' component to DefaultItem.");
			}
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
				this.itemHeight = this.DefaultItem.GetComponent<RectTransform>().rect.height;
				this.maxVisibleItems = (int)Math.Ceiling((double)(this.scrollHeight / this.itemHeight)) + 1;
				this.layout = this.Container.GetComponent<EasyLayout.EasyLayout>();
				ResizeListener resizeListener = this.scrollRect.gameObject.AddComponent<ResizeListener>();
				resizeListener.OnResize.AddListener(new UnityAction(this.Resize));
			}
			this.UpdateItems();
			this.DefaultItem.gameObject.SetActive(false);
			this.OnSelect.AddListener(new UnityAction<int, ListViewItem>(this.OnSelectCallback));
			this.OnDeselect.AddListener(new UnityAction<int, ListViewItem>(this.OnDeselectCallback));
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x00181C60 File Offset: 0x00180060
		private void Resize()
		{
			this.scrollHeight = this.scrollRect.GetComponent<RectTransform>().rect.height;
			this.maxVisibleItems = (int)Math.Ceiling((double)(this.scrollHeight / this.itemHeight)) + 1;
			this.UpdateView();
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x00181CAD File Offset: 0x001800AD
		private bool CanOptimize()
		{
			return this.scrollRect != null && (this.layout != null || this.Container.GetComponent<EasyLayout.EasyLayout>() != null);
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x00181CE8 File Offset: 0x001800E8
		private void OnSelectCallback(int index, ListViewItem item)
		{
			this.OnSelectString.Invoke(index, this.strings[index]);
			if (item != null)
			{
				this.SelectColoring(item as ListViewStringComponent);
			}
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x00181D1A File Offset: 0x0018011A
		private void OnDeselectCallback(int index, ListViewItem item)
		{
			this.OnDeselectString.Invoke(index, this.strings[index]);
			if (item != null)
			{
				this.DefaultColoring(item as ListViewStringComponent);
			}
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x00181D4C File Offset: 0x0018014C
		public override void UpdateItems()
		{
			if (this.Source == ListViewSources.List)
			{
				this.UpdateItems(this.strings);
			}
			else
			{
				this.UpdateItems(this.File);
			}
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x00181D76 File Offset: 0x00180176
		public override void Clear()
		{
			this.UpdateItems(new List<string>());
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x00181D84 File Offset: 0x00180184
		private void UpdateItems(TextAsset newFile)
		{
			if (this.file == null)
			{
				return;
			}
			List<string> newItems = new List<string>(newFile.text.Split(new string[]
			{
				"\r\n",
				"\r",
				"\n"
			}, StringSplitOptions.None));
			this.UpdateItems(newItems);
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x00181DDC File Offset: 0x001801DC
		public virtual List<int> FindIndicies(string item)
		{
			return (from i in Enumerable.Range(0, this.strings.Count)
			where this.strings[i] == item
			select i).ToList<int>();
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x00181E24 File Offset: 0x00180224
		public virtual int FindIndex(string item)
		{
			return this.strings.FindIndex((string x) => x == item);
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x00181E58 File Offset: 0x00180258
		public virtual int Add(string item)
		{
			List<string> items = this.Items;
			if (!this.Sort || this.SortFunc == null)
			{
				items.Add(item);
				this.UpdateItems(items);
				return this.Items.Count - 1;
			}
			List<int> second = this.FindIndicies(item);
			items.Add(item);
			this.UpdateItems(items);
			List<int> list = this.FindIndicies(item);
			List<int> list2 = list.Except(second).ToList<int>();
			return (list2.Count <= 0) ? list.FirstOrDefault<int>() : list2[0];
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x00181EEC File Offset: 0x001802EC
		public virtual int Remove(string item)
		{
			int num = this.FindIndex(item);
			if (num == -1)
			{
				return num;
			}
			List<string> list = this.Strings;
			list.Remove(item);
			this.UpdateItems(list);
			return num;
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x00181F21 File Offset: 0x00180321
		private void RemoveCallbacks()
		{
			this.components.ForEach(delegate(ListViewStringComponent x, int index)
			{
				if (x == null)
				{
					return;
				}
				if (this.callbacksEnter.Count > index)
				{
					x.onPointerEnter.RemoveListener(this.callbacksEnter[index]);
				}
				if (this.callbacksExit.Count > index)
				{
					x.onPointerExit.RemoveListener(this.callbacksExit[index]);
				}
			});
			this.callbacksEnter.Clear();
			this.callbacksExit.Clear();
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x00181F50 File Offset: 0x00180350
		private void AddCallbacks()
		{
			this.components.ForEach(new Action<ListViewStringComponent, int>(this.AddCallback));
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x00181F6C File Offset: 0x0018036C
		private void AddCallback(ListViewStringComponent component, int index)
		{
			this.callbacksEnter.Add(delegate(PointerEventData ev)
			{
				this.OnPointerEnterCallback(component);
			});
			this.callbacksExit.Add(delegate(PointerEventData ev)
			{
				this.OnPointerExitCallback(component);
			});
			component.onPointerEnter.AddListener(this.callbacksEnter[index]);
			component.onPointerExit.AddListener(this.callbacksExit[index]);
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x00181FF3 File Offset: 0x001803F3
		public override bool IsValid(int index)
		{
			return index >= 0 && index < this.strings.Count;
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x00182010 File Offset: 0x00180410
		private void OnPointerEnterCallback(ListViewStringComponent component)
		{
			if (!this.IsValid(component.Index))
			{
				string message = string.Format("Index must be between 0 and Items.Count ({0})", this.strings.Count);
				throw new IndexOutOfRangeException(message);
			}
			if (base.IsSelected(component.Index))
			{
				return;
			}
			this.HighlightColoring(component);
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x0018206C File Offset: 0x0018046C
		private void OnPointerExitCallback(ListViewStringComponent component)
		{
			if (!this.IsValid(component.Index))
			{
				string message = string.Format("Index must be between 0 and Items.Count ({0})", this.strings.Count);
				throw new IndexOutOfRangeException(message);
			}
			if (base.IsSelected(component.Index))
			{
				return;
			}
			this.DefaultColoring(component);
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x001820C8 File Offset: 0x001804C8
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

		// Token: 0x06004754 RID: 18260 RVA: 0x001821E8 File Offset: 0x001805E8
		private float FullHeight()
		{
			return this.layout.BlockSize[1];
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x0018220C File Offset: 0x0018060C
		private float GetScrollMargin()
		{
			float num = (1f - this.scrollRect.verticalScrollbar.size) * (1f - this.scrollRect.verticalScrollbar.value);
			return this.FullHeight() * num;
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x00182250 File Offset: 0x00180650
		private int GetLastVisibleIndex(bool strict = false)
		{
			float num = this.GetScrollMargin() + this.scrollHeight;
			int num2 = (!strict) ? ((int)Math.Ceiling((double)(num / (this.itemHeight + this.layout.Spacing.y)))) : ((int)Math.Floor((double)(num / (this.itemHeight + this.layout.Spacing.y))));
			return num2 - 1;
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x001822BC File Offset: 0x001806BC
		private int GetFirstVisibleIndex(bool strict = false)
		{
			int num = (!strict) ? ((int)Math.Floor((double)(this.GetScrollMargin() / (this.itemHeight + this.layout.Spacing.y)))) : ((int)Math.Ceiling((double)(this.GetScrollMargin() / (this.itemHeight + this.layout.Spacing.y))));
			if (strict)
			{
				return num;
			}
			return Math.Min(num, Math.Max(0, this.strings.Count - this.visibleItems));
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x00182348 File Offset: 0x00180748
		private List<string> FilterItems(List<string> newItems)
		{
			IEnumerable<string> enumerable = newItems;
			if (this.Source == ListViewSources.File)
			{
				enumerable = from x in enumerable
				select x.Trim();
				if (this.Unique)
				{
					enumerable = enumerable.Distinct<string>();
				}
				if (!this.AllowEmptyItems)
				{
					enumerable = from x in enumerable
					where x != string.Empty
					select x;
				}
				if (this.CommentsStartWith.Count > 0)
				{
					enumerable = from line in enumerable
					where !this.CommentsStartWith.Any((string comment) => line.StartsWith(comment, StringComparison.InvariantCulture))
					select line;
				}
			}
			if (this.Sort && this.SortFunc != null)
			{
				enumerable = this.SortFunc(enumerable);
			}
			return enumerable.ToList<string>();
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x00182418 File Offset: 0x00180818
		private ListViewStringComponent ComponentTopToBottom()
		{
			int index = this.components.Count - 1;
			ListViewStringComponent listViewStringComponent = this.components[index];
			this.components.RemoveAt(index);
			this.components.Insert(0, listViewStringComponent);
			listViewStringComponent.transform.SetAsFirstSibling();
			return listViewStringComponent;
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x00182468 File Offset: 0x00180868
		private ListViewStringComponent ComponentBottomToTop()
		{
			ListViewStringComponent listViewStringComponent = this.components[0];
			this.components.RemoveAt(0);
			this.components.Add(listViewStringComponent);
			listViewStringComponent.transform.SetAsLastSibling();
			return listViewStringComponent;
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x001824A8 File Offset: 0x001808A8
		private void OnScroll(float value)
		{
			int num = this.topHiddenItems;
			this.topHiddenItems = this.GetFirstVisibleIndex(false);
			this.bottomHiddenItems = Math.Max(0, this.strings.Count - this.visibleItems - this.topHiddenItems);
			if (num != this.topHiddenItems)
			{
				if (num == this.topHiddenItems + 1)
				{
					ListViewStringComponent listViewStringComponent = this.ComponentTopToBottom();
					listViewStringComponent.Index = this.topHiddenItems;
					listViewStringComponent.Text.text = this.strings[this.topHiddenItems];
					this.Coloring(listViewStringComponent);
				}
				else if (num == this.topHiddenItems - 1)
				{
					ListViewStringComponent listViewStringComponent2 = this.ComponentBottomToTop();
					int index = this.topHiddenItems + this.visibleItems - 1;
					listViewStringComponent2.Index = index;
					listViewStringComponent2.Text.text = this.strings[index];
					this.Coloring(listViewStringComponent2);
				}
				else
				{
					int[] new_indicies = Enumerable.Range(this.topHiddenItems, this.visibleItems).ToArray<int>();
					this.components.ForEach(delegate(ListViewStringComponent x, int i)
					{
						x.Index = new_indicies[i];
						x.Text.text = this.strings[new_indicies[i]];
						this.Coloring(x);
					});
				}
			}
			this.UpdateTopFiller();
			this.UpdateBottomFiller();
			if (this.layout)
			{
				this.layout.NeedUpdateLayout();
			}
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x00182604 File Offset: 0x00180A04
		private List<ListViewStringComponent> GetNewComponents()
		{
			this.componentsCache = (from x in this.componentsCache
			where x != null
			select x).ToList<ListViewStringComponent>();
			List<ListViewStringComponent> new_components = new List<ListViewStringComponent>();
			this.strings.ForEach(delegate(string x, int i)
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
					this.componentsCache[0].gameObject.SetActive(true);
					new_components.Add(this.componentsCache[0]);
					this.componentsCache.RemoveAt(0);
				}
				else
				{
					ImageAdvanced imageAdvanced = UnityEngine.Object.Instantiate<ImageAdvanced>(this.DefaultItem);
					imageAdvanced.gameObject.SetActive(true);
					ListViewStringComponent listViewStringComponent = imageAdvanced.GetComponent<ListViewStringComponent>();
					if (listViewStringComponent == null)
					{
						listViewStringComponent = imageAdvanced.gameObject.AddComponent<ListViewStringComponent>();
						listViewStringComponent.Background = imageAdvanced;
						listViewStringComponent.Text = imageAdvanced.GetComponentInChildren<Text>();
					}
					Utilites.FixInstantiated(this.DefaultItem, imageAdvanced);
					listViewStringComponent.gameObject.SetActive(true);
					new_components.Add(listViewStringComponent);
				}
			});
			this.components.ForEach(delegate(ListViewStringComponent x)
			{
				x.gameObject.SetActive(false);
			});
			this.componentsCache.AddRange(this.components);
			this.components.Clear();
			return new_components;
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x001826C0 File Offset: 0x00180AC0
		private void UpdateView()
		{
			this.RemoveCallbacks();
			if (this.CanOptimize() && this.strings.Count > 0)
			{
				this.visibleItems = ((this.maxVisibleItems >= this.strings.Count) ? this.strings.Count : this.maxVisibleItems);
			}
			else
			{
				this.visibleItems = this.strings.Count;
			}
			this.components = this.GetNewComponents();
			List<ListViewItem> base_items = new List<ListViewItem>();
			this.components.ForEach(delegate(ListViewStringComponent x)
			{
				base_items.Add(x);
			});
			base.Items = base_items;
			this.components.ForEach(delegate(ListViewStringComponent x, int i)
			{
				x.Index = i;
				x.Text.text = this.strings[i];
				this.Coloring(x);
			});
			this.AddCallbacks();
			this.topHiddenItems = 0;
			this.bottomHiddenItems = this.strings.Count<string>() - this.visibleItems;
			this.UpdateTopFiller();
			this.UpdateBottomFiller();
			if (this.layout)
			{
				this.layout.UpdateLayout();
			}
			if (this.scrollRect != null)
			{
				RectTransform component = this.scrollRect.GetComponent<RectTransform>();
				component.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, component.rect.width);
			}
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x00182814 File Offset: 0x00180C14
		private void UpdateItems(List<string> newItems)
		{
			newItems = this.FilterItems(newItems);
			List<int> new_selected_indicies = this.NewSelectedIndicies(newItems);
			base.SelectedIndicies.ForEach(delegate(int x)
			{
				if (!new_selected_indicies.Contains(x))
				{
					this.Deselect(x);
				}
			});
			this.strings = newItems;
			this.UpdateView();
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x00182868 File Offset: 0x00180C68
		private void UpdateBottomFiller()
		{
			this.bottomFiller.SetAsLastSibling();
			if (this.bottomHiddenItems == 0)
			{
				this.bottomFiller.gameObject.SetActive(false);
			}
			else
			{
				this.bottomFiller.gameObject.SetActive(true);
				this.bottomFiller.sizeDelta = new Vector2(this.bottomFiller.sizeDelta.x, (float)this.bottomHiddenItems * (this.itemHeight + this.layout.Spacing.y) - this.layout.Spacing.y);
			}
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x00182908 File Offset: 0x00180D08
		private void UpdateTopFiller()
		{
			this.topFiller.SetAsFirstSibling();
			if (this.topHiddenItems == 0)
			{
				this.topFiller.gameObject.SetActive(false);
			}
			else
			{
				this.topFiller.gameObject.SetActive(true);
				this.topFiller.sizeDelta = new Vector2(this.bottomFiller.sizeDelta.x, (float)this.topHiddenItems * (this.itemHeight + this.layout.Spacing.y) - this.layout.Spacing.y);
			}
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x001829A8 File Offset: 0x00180DA8
		private List<int> NewSelectedIndicies(IList<string> newItems)
		{
			List<int> selected_indicies = new List<int>();
			if (newItems.Count == 0)
			{
				return selected_indicies;
			}
			List<string> new_items_copy = new List<string>(newItems);
			List<string> selected_items = (from x in base.SelectedIndicies
			select this.strings[x]).ToList<string>();
			selected_items = selected_items.Where(delegate(string x)
			{
				bool flag = newItems.Contains(x);
				if (flag)
				{
					new_items_copy.Remove(x);
				}
				return flag;
			}).ToList<string>();
			newItems.ForEach(delegate(string item, int index)
			{
				if (selected_items.Contains(item))
				{
					selected_items.Remove(item);
					selected_indicies.Add(index);
				}
			});
			return selected_indicies;
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x00182A5D File Offset: 0x00180E5D
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

		// Token: 0x06004763 RID: 18275 RVA: 0x00182A95 File Offset: 0x00180E95
		private void UpdateColors()
		{
			this.components.ForEach(new Action<ListViewStringComponent>(this.Coloring));
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x00182AB0 File Offset: 0x00180EB0
		private ListViewStringComponent GetComponent(int index)
		{
			return this.components.Find((ListViewStringComponent x) => x.Index == index);
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x00182AE1 File Offset: 0x00180EE1
		protected override void SelectItem(int index)
		{
			this.SelectColoring(this.GetComponent(index));
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x00182AF0 File Offset: 0x00180EF0
		protected override void DeselectItem(int index)
		{
			this.DefaultColoring(this.GetComponent(index));
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x00182AFF File Offset: 0x00180EFF
		protected override void HighlightColoring(ListViewItem component)
		{
			if (base.IsSelected(component.Index))
			{
				return;
			}
			this.HighlightColoring(component as ListViewStringComponent);
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x00182B1F File Offset: 0x00180F1F
		protected virtual void HighlightColoring(ListViewStringComponent component)
		{
			if (component == null)
			{
				return;
			}
			component.Background.color = this.HighlightedBackgroundColor;
			component.Text.color = this.HighlightedTextColor;
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x00182B50 File Offset: 0x00180F50
		protected virtual void SelectColoring(ListViewItem component)
		{
			if (component == null)
			{
				return;
			}
			this.SelectColoring(component as ListViewStringComponent);
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x00182B6B File Offset: 0x00180F6B
		protected virtual void SelectColoring(ListViewStringComponent component)
		{
			if (component == null)
			{
				return;
			}
			component.Background.color = this.selectedBackgroundColor;
			component.Text.color = this.selectedTextColor;
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x00182B9C File Offset: 0x00180F9C
		protected virtual void DefaultColoring(ListViewItem component)
		{
			if (component == null)
			{
				return;
			}
			this.DefaultColoring(component as ListViewStringComponent);
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x00182BB7 File Offset: 0x00180FB7
		protected virtual void DefaultColoring(ListViewStringComponent component)
		{
			if (component == null)
			{
				return;
			}
			component.Background.color = this.backgroundColor;
			component.Text.color = this.textColor;
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x00182BE8 File Offset: 0x00180FE8
		protected override void OnDestroy()
		{
			this.OnSelect.RemoveListener(new UnityAction<int, ListViewItem>(this.OnSelectCallback));
			this.OnDeselect.RemoveListener(new UnityAction<int, ListViewItem>(this.OnDeselectCallback));
			this.RemoveCallbacks();
			base.OnDestroy();
		}

		// Token: 0x040030A5 RID: 12453
		[SerializeField]
		private List<string> strings = new List<string>();

		// Token: 0x040030A6 RID: 12454
		[SerializeField]
		private TextAsset file;

		// Token: 0x040030A7 RID: 12455
		[SerializeField]
		public List<string> CommentsStartWith = new List<string>
		{
			"#",
			"//"
		};

		// Token: 0x040030A8 RID: 12456
		[SerializeField]
		public ListViewSources Source;

		// Token: 0x040030A9 RID: 12457
		[SerializeField]
		public bool Unique = true;

		// Token: 0x040030AA RID: 12458
		[SerializeField]
		public bool AllowEmptyItems;

		// Token: 0x040030AB RID: 12459
		[SerializeField]
		private Color backgroundColor = Color.white;

		// Token: 0x040030AC RID: 12460
		[SerializeField]
		private Color textColor = Color.black;

		// Token: 0x040030AD RID: 12461
		[SerializeField]
		public Color HighlightedBackgroundColor = new Color(203f, 230f, 244f, 255f);

		// Token: 0x040030AE RID: 12462
		[SerializeField]
		public Color HighlightedTextColor = Color.black;

		// Token: 0x040030AF RID: 12463
		[SerializeField]
		private Color selectedBackgroundColor = new Color(53f, 83f, 227f, 255f);

		// Token: 0x040030B0 RID: 12464
		[SerializeField]
		private Color selectedTextColor = Color.black;

		// Token: 0x040030B1 RID: 12465
		[SerializeField]
		public ImageAdvanced DefaultItem;

		// Token: 0x040030B2 RID: 12466
		private List<ListViewStringComponent> components = new List<ListViewStringComponent>();

		// Token: 0x040030B3 RID: 12467
		private List<UnityAction<PointerEventData>> callbacksEnter = new List<UnityAction<PointerEventData>>();

		// Token: 0x040030B4 RID: 12468
		private List<UnityAction<PointerEventData>> callbacksExit = new List<UnityAction<PointerEventData>>();

		// Token: 0x040030B5 RID: 12469
		[SerializeField]
		[FormerlySerializedAs("Sort")]
		private bool sort = true;

		// Token: 0x040030B6 RID: 12470
		private Func<IEnumerable<string>, IEnumerable<string>> sortFunc = (IEnumerable<string> items) => from x in items
		orderby x
		select x;

		// Token: 0x040030B7 RID: 12471
		public ListViewEvent OnSelectString = new ListViewEvent();

		// Token: 0x040030B8 RID: 12472
		public ListViewEvent OnDeselectString = new ListViewEvent();

		// Token: 0x040030B9 RID: 12473
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x040030BA RID: 12474
		private RectTransform topFiller;

		// Token: 0x040030BB RID: 12475
		private RectTransform bottomFiller;

		// Token: 0x040030BC RID: 12476
		private float itemHeight;

		// Token: 0x040030BD RID: 12477
		private float scrollHeight;

		// Token: 0x040030BE RID: 12478
		private int maxVisibleItems;

		// Token: 0x040030BF RID: 12479
		private int visibleItems;

		// Token: 0x040030C0 RID: 12480
		private int topHiddenItems;

		// Token: 0x040030C1 RID: 12481
		private int bottomHiddenItems;

		// Token: 0x040030C2 RID: 12482
		[NonSerialized]
		private bool isStartedListView;

		// Token: 0x040030C3 RID: 12483
		private EasyLayout.EasyLayout layout;

		// Token: 0x040030C4 RID: 12484
		private List<ListViewStringComponent> componentsCache = new List<ListViewStringComponent>();
	}
}
