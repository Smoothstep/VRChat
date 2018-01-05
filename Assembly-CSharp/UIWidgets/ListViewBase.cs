using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x02000944 RID: 2372
	public abstract class ListViewBase : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06004779 RID: 18297 RVA: 0x0017A1FE File Offset: 0x001785FE
		// (set) Token: 0x0600477A RID: 18298 RVA: 0x0017A20B File Offset: 0x0017860B
		public List<ListViewItem> Items
		{
			get
			{
				return new List<ListViewItem>(this.items);
			}
			set
			{
				this.UpdateItems(value);
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x0600477B RID: 18299 RVA: 0x0017A214 File Offset: 0x00178614
		// (set) Token: 0x0600477C RID: 18300 RVA: 0x0017A21C File Offset: 0x0017861C
		public int SelectedIndex
		{
			get
			{
				return this.selectedIndex;
			}
			set
			{
				if (value == -1)
				{
					if (this.selectedIndex != -1)
					{
						this.Deselect(this.selectedIndex);
					}
					this.selectedIndex = value;
				}
				else
				{
					this.Select(value);
				}
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x0600477D RID: 18301 RVA: 0x0017A250 File Offset: 0x00178650
		// (set) Token: 0x0600477E RID: 18302 RVA: 0x0017A260 File Offset: 0x00178660
		public List<int> SelectedIndicies
		{
			get
			{
				return new List<int>(this.selectedIndicies);
			}
			set
			{
				List<int> list = (from index in this.selectedIndicies
				where !value.Contains(index)
				select index).ToList<int>();
				List<int> list2 = (from index in value
				where !this.selectedIndicies.Contains(index)
				select index).ToList<int>();
				list.ForEach(delegate(int index)
				{
					this.Deselect(index);
				});
				list2.ForEach(delegate(int index)
				{
					this.Select(index);
				});
			}
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x0017A2DF File Offset: 0x001786DF
		private void Awake()
		{
			this.Start();
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0017A2E8 File Offset: 0x001786E8
		public virtual void Start()
		{
			if (this.isStartedListViewBase)
			{
				return;
			}
			this.isStartedListViewBase = true;
			this.unused = new GameObject("unused base");
			this.unused.SetActive(false);
			this.unused.transform.SetParent(base.transform, false);
			if (this.selectedIndex != -1 && this.selectedIndicies.Count == 0)
			{
				this.selectedIndicies.Add(this.selectedIndex);
			}
			this.selectedIndicies = (from x in this.selectedIndicies
			where this.IsValid(x)
			select x).ToList<int>();
			if (this.selectedIndicies.Count == 0)
			{
				this.selectedIndex = -1;
			}
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x0017A3A1 File Offset: 0x001787A1
		public virtual void UpdateItems()
		{
			this.UpdateItems(this.items);
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x0017A3B0 File Offset: 0x001787B0
		private void RemoveCallback(ListViewItem item, int index)
		{
			if (item == null)
			{
				return;
			}
			if (this.callbacks.Count > index)
			{
				item.onClick.RemoveListener(this.callbacks[index]);
			}
			item.onSubmit.RemoveListener(new UnityAction<ListViewItem>(this.Toggle));
			item.onCancel.RemoveListener(new UnityAction<ListViewItem>(this.OnItemCancel));
			item.onSelect.RemoveListener(new UnityAction<ListViewItem>(this.HighlightColoring));
			item.onDeselect.RemoveListener(new UnityAction<ListViewItem>(this.Coloring));
			item.onMove.RemoveListener(new UnityAction<AxisEventData, ListViewItem>(this.OnItemMove));
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0017A467 File Offset: 0x00178867
		private void OnItemCancel(ListViewItem item)
		{
			if (EventSystem.current.alreadySelecting)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(base.gameObject);
			this.onItemCancel.Invoke();
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0017A494 File Offset: 0x00178894
		private void RemoveCallbacks()
		{
			if (this.callbacks.Count > 0)
			{
				this.items.ForEach(new Action<ListViewItem, int>(this.RemoveCallback));
			}
			this.callbacks.Clear();
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0017A4C9 File Offset: 0x001788C9
		private void AddCallbacks()
		{
			this.items.ForEach(new Action<ListViewItem, int>(this.AddCallback));
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0017A4E4 File Offset: 0x001788E4
		private void AddCallback(ListViewItem item, int index)
		{
			this.callbacks.Insert(index, delegate
			{
				this.Toggle(item);
			});
			item.onClick.AddListener(this.callbacks[index]);
			item.onSubmit.AddListener(new UnityAction<ListViewItem>(this.OnItemSubmit));
			item.onCancel.AddListener(new UnityAction<ListViewItem>(this.OnItemCancel));
			item.onSelect.AddListener(new UnityAction<ListViewItem>(this.HighlightColoring));
			item.onDeselect.AddListener(new UnityAction<ListViewItem>(this.Coloring));
			item.onMove.AddListener(new UnityAction<AxisEventData, ListViewItem>(this.OnItemMove));
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0017A5C7 File Offset: 0x001789C7
		private void OnItemSelect(ListViewItem item)
		{
			this.onItemSelect.Invoke();
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0017A5D4 File Offset: 0x001789D4
		private void OnItemSubmit(ListViewItem item)
		{
			this.Toggle(item);
			if (!this.IsSelected(item.Index))
			{
				this.HighlightColoring(item);
			}
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0017A5F8 File Offset: 0x001789F8
		private void OnItemMove(AxisEventData eventData, ListViewItem item)
		{
			switch (eventData.moveDir)
			{
			case MoveDirection.Up:
				if (item.Index > 0)
				{
					this.SelectComponentByIndex(item.Index - 1);
				}
				break;
			case MoveDirection.Down:
				if (this.IsValid(item.Index + 1))
				{
					this.SelectComponentByIndex(item.Index + 1);
				}
				break;
			}
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0017A676 File Offset: 0x00178A76
		protected virtual void ScrollTo(int index)
		{
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0017A678 File Offset: 0x00178A78
		public virtual int Add(ListViewItem item)
		{
			item.transform.SetParent(this.Container, false);
			this.AddCallback(item, this.items.Count);
			this.items.Add(item);
			item.Index = this.callbacks.Count - 1;
			return this.callbacks.Count - 1;
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x0017A6D5 File Offset: 0x00178AD5
		public virtual void Clear()
		{
			this.items.Clear();
			this.UpdateItems();
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x0017A6E8 File Offset: 0x00178AE8
		protected virtual int Remove(ListViewItem item)
		{
			this.RemoveCallbacks();
			int index = item.Index;
			this.selectedIndicies = (from x in this.selectedIndicies
			where x != index
			select x).Select(delegate(int x)
			{
				int result;
				if (x > index)
				{
					x = (result = x) - 1;
				}
				else
				{
					result = x;
				}
				return result;
			}).ToList<int>();
			if (this.selectedIndex == index)
			{
				this.Deselect(index);
				this.selectedIndex = ((this.selectedIndicies.Count <= 0) ? -1 : this.selectedIndicies.Last<int>());
			}
			else if (this.selectedIndex > index)
			{
				this.selectedIndex--;
			}
			this.items.Remove(item);
			this.Free(item);
			this.AddCallbacks();
			return index;
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x0017A7CC File Offset: 0x00178BCC
		private void Free(Component item)
		{
			if (item == null)
			{
				return;
			}
			if (this.DestroyGameObjects)
			{
				if (item.gameObject == null)
				{
					return;
				}
				UnityEngine.Object.Destroy(item.gameObject);
			}
			else
			{
				if (item.transform == null)
				{
					return;
				}
				item.transform.SetParent(this.unused.transform, false);
			}
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x0017A83C File Offset: 0x00178C3C
		private void UpdateItems(List<ListViewItem> newItems)
		{
			this.RemoveCallbacks();
			(from item in this.items
			where !newItems.Contains(item)
			select item).ForEach(delegate(ListViewItem item)
			{
				this.Free(item);
			});
			newItems.ForEach(delegate(ListViewItem x, int i)
			{
				x.Index = i;
				x.transform.SetParent(this.Container, false);
			});
			this.items = newItems;
			this.AddCallbacks();
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0017A8B4 File Offset: 0x00178CB4
		public virtual bool IsValid(int index)
		{
			return index >= 0 && index < this.items.Count;
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0017A8D0 File Offset: 0x00178CD0
		protected ListViewItem GetItem(int index)
		{
			return this.items.Find((ListViewItem x) => x.Index == index);
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x0017A904 File Offset: 0x00178D04
		public virtual void Select(int index)
		{
			if (index == -1)
			{
				return;
			}
			if (!this.IsValid(index))
			{
				string message = string.Format("Index must be between 0 and Items.Count ({0}). Gameobject {1}.", this.items.Count - 1, base.name);
				throw new IndexOutOfRangeException(message);
			}
			if (this.IsSelected(index) && this.Multiple)
			{
				return;
			}
			if (!this.Multiple)
			{
				if (this.selectedIndex != -1 && this.selectedIndex != index)
				{
					this.Deselect(this.selectedIndex);
				}
				this.selectedIndicies.Clear();
			}
			this.selectedIndicies.Add(index);
			this.selectedIndex = index;
			this.SelectItem(index);
			this.OnSelect.Invoke(index, this.GetItem(index));
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x0017A9D0 File Offset: 0x00178DD0
		public void Deselect(int index)
		{
			if (index == -1)
			{
				return;
			}
			if (!this.IsSelected(index))
			{
				return;
			}
			this.selectedIndicies.Remove(index);
			this.selectedIndex = ((this.selectedIndicies.Count <= 0) ? -1 : this.selectedIndicies.Last<int>());
			this.DeselectItem(index);
			this.OnDeselect.Invoke(index, this.GetItem(index));
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x0017AA41 File Offset: 0x00178E41
		public bool IsSelected(int index)
		{
			return this.selectedIndicies.Contains(index);
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0017AA4F File Offset: 0x00178E4F
		public void Toggle(int index)
		{
			if (this.IsSelected(index) && this.Multiple)
			{
				this.Deselect(index);
			}
			else
			{
				this.Select(index);
			}
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x0017AA7B File Offset: 0x00178E7B
		private void Toggle(ListViewItem item)
		{
			this.Toggle(item.Index);
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x0017AA89 File Offset: 0x00178E89
		protected virtual void SelectItem(int index)
		{
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x0017AA8B File Offset: 0x00178E8B
		protected virtual void DeselectItem(int index)
		{
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x0017AA8D File Offset: 0x00178E8D
		protected virtual void Coloring(ListViewItem component)
		{
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x0017AA8F File Offset: 0x00178E8F
		protected virtual void HighlightColoring(ListViewItem component)
		{
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x0017AA91 File Offset: 0x00178E91
		protected virtual void OnDestroy()
		{
			this.RemoveCallbacks();
			this.items.ForEach(delegate(ListViewItem x)
			{
				this.Free(x);
			});
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x0017AAB0 File Offset: 0x00178EB0
		public virtual bool SelectComponent()
		{
			if (this.items.Count == 0)
			{
				return false;
			}
			int index = (this.SelectedIndex == -1) ? 0 : this.SelectedIndex;
			this.SelectComponentByIndex(index);
			return true;
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x0017AAF0 File Offset: 0x00178EF0
		private void SelectComponentByIndex(int index)
		{
			this.ScrollTo(index);
			ListViewItemEventData listViewItemEventData = new ListViewItemEventData(EventSystem.current)
			{
				NewSelectedObject = this.GetItem(index).gameObject
			};
			ExecuteEvents.Execute<ISelectHandler>(listViewItemEventData.NewSelectedObject, listViewItemEventData, ExecuteEvents.selectHandler);
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x0017AB35 File Offset: 0x00178F35
		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			if (!EventSystem.current.alreadySelecting)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
			this.OnFocusIn.Invoke(eventData);
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x0017AB62 File Offset: 0x00178F62
		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			this.OnFocusOut.Invoke(eventData);
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x0017AB70 File Offset: 0x00178F70
		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			this.SelectComponent();
			this.onSubmit.Invoke();
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x0017AB84 File Offset: 0x00178F84
		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			this.onCancel.Invoke();
		}

		// Token: 0x040030CB RID: 12491
		[SerializeField]
		[HideInInspector]
		private List<ListViewItem> items = new List<ListViewItem>();

		// Token: 0x040030CC RID: 12492
		private List<UnityAction> callbacks = new List<UnityAction>();

		// Token: 0x040030CD RID: 12493
		[SerializeField]
		[HideInInspector]
		public bool DestroyGameObjects = true;

		// Token: 0x040030CE RID: 12494
		[SerializeField]
		public bool Multiple;

		// Token: 0x040030CF RID: 12495
		[SerializeField]
		private int selectedIndex = -1;

		// Token: 0x040030D0 RID: 12496
		[SerializeField]
		private List<int> selectedIndicies = new List<int>();

		// Token: 0x040030D1 RID: 12497
		public ListViewBaseEvent OnSelect = new ListViewBaseEvent();

		// Token: 0x040030D2 RID: 12498
		public ListViewBaseEvent OnDeselect = new ListViewBaseEvent();

		// Token: 0x040030D3 RID: 12499
		public UnityEvent onSubmit = new UnityEvent();

		// Token: 0x040030D4 RID: 12500
		public UnityEvent onCancel = new UnityEvent();

		// Token: 0x040030D5 RID: 12501
		public UnityEvent onItemSelect = new UnityEvent();

		// Token: 0x040030D6 RID: 12502
		public UnityEvent onItemCancel = new UnityEvent();

		// Token: 0x040030D7 RID: 12503
		[SerializeField]
		public Transform Container;

		// Token: 0x040030D8 RID: 12504
		public ListViewFocusEvent OnFocusIn = new ListViewFocusEvent();

		// Token: 0x040030D9 RID: 12505
		public ListViewFocusEvent OnFocusOut = new ListViewFocusEvent();

		// Token: 0x040030DA RID: 12506
		private GameObject unused;

		// Token: 0x040030DB RID: 12507
		[NonSerialized]
		private bool isStartedListViewBase;
	}
}
