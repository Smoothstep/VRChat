using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x02000949 RID: 2377
	[AddComponentMenu("UI/ListViewGameObjects", 255)]
	public class ListViewGameObjects : ListViewBase
	{
		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06004804 RID: 18436 RVA: 0x001830A8 File Offset: 0x001814A8
		// (set) Token: 0x06004805 RID: 18437 RVA: 0x001830B5 File Offset: 0x001814B5
		public List<GameObject> Objects
		{
			get
			{
				return new List<GameObject>(this.objects);
			}
			private set
			{
				this.UpdateItems(value);
			}
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x001830BE File Offset: 0x001814BE
		private void Awake()
		{
			this.Start();
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x001830C8 File Offset: 0x001814C8
		public override void Start()
		{
			if (this.isStartedListViewGameObjects)
			{
				return;
			}
			this.isStartedListViewGameObjects = true;
			base.Start();
			this.DestroyGameObjects = true;
			this.UpdateItems();
			this.OnSelect.AddListener(new UnityAction<int, ListViewItem>(this.OnSelectCallback));
			this.OnDeselect.AddListener(new UnityAction<int, ListViewItem>(this.OnDeselectCallback));
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x00183129 File Offset: 0x00181529
		private void OnSelectCallback(int index, ListViewItem item)
		{
			this.OnSelectObject.Invoke(index, this.objects[index]);
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x00183143 File Offset: 0x00181543
		private void OnDeselectCallback(int index, ListViewItem item)
		{
			this.OnDeselectObject.Invoke(index, this.objects[index]);
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x0018315D File Offset: 0x0018155D
		private void OnPointerEnterCallback(int index)
		{
			this.OnPointerEnterObject.Invoke(index, this.objects[index]);
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x00183177 File Offset: 0x00181577
		private void OnPointerExitCallback(int index)
		{
			this.OnPointerExitObject.Invoke(index, this.objects[index]);
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x00183191 File Offset: 0x00181591
		public override void UpdateItems()
		{
			this.UpdateItems(this.objects);
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x001831A0 File Offset: 0x001815A0
		public virtual int Add(GameObject item)
		{
			List<GameObject> list = this.Objects;
			list.Add(item);
			this.UpdateItems(list);
			return this.objects.FindIndex((GameObject x) => x == item);
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x001831F0 File Offset: 0x001815F0
		public virtual int Remove(GameObject item)
		{
			int num = this.objects.FindIndex((GameObject x) => x == item);
			if (num == -1)
			{
				return num;
			}
			List<GameObject> list = this.Objects;
			list.Remove(item);
			this.UpdateItems(list);
			return num;
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x00183247 File Offset: 0x00181647
		private void RemoveCallbacks()
		{
			base.Items.ForEach(delegate(ListViewItem item, int index)
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
			});
			this.callbacksEnter.Clear();
			this.callbacksExit.Clear();
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x00183276 File Offset: 0x00181676
		private void AddCallbacks()
		{
			base.Items.ForEach(delegate(ListViewItem item, int index)
			{
				this.AddCallback(item, index);
			});
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x00183290 File Offset: 0x00181690
		private void AddCallback(ListViewItem item, int index)
		{
			this.callbacksEnter.Add(delegate(PointerEventData ev)
			{
				this.OnPointerEnterCallback(index);
			});
			this.callbacksExit.Add(delegate(PointerEventData ev)
			{
				this.OnPointerExitCallback(index);
			});
			item.onPointerEnter.AddListener(this.callbacksEnter[index]);
			item.onPointerExit.AddListener(this.callbacksExit[index]);
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x00183318 File Offset: 0x00181718
		private List<GameObject> SortItems(IEnumerable<GameObject> newItems)
		{
			IEnumerable<GameObject> enumerable = newItems;
			if (this.SortFunc != null)
			{
				enumerable = this.SortFunc(enumerable);
			}
			return enumerable.ToList<GameObject>();
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x00183348 File Offset: 0x00181748
		private void UpdateItems(List<GameObject> newItems)
		{
			newItems = this.SortItems(newItems);
			this.RemoveCallbacks();
			List<int> selected_indicies = new List<int>();
			base.SelectedIndicies.ForEach(delegate(int index)
			{
				int num = newItems.FindIndex((GameObject x) => x == this.objects[index]);
				if (num != -1)
				{
					selected_indicies.Add(index);
				}
			});
			List<ListViewItem> base_items = new List<ListViewItem>();
			newItems.ForEach(delegate(GameObject x)
			{
				ListViewItem item = x.GetComponent<ListViewItem>() ?? x.AddComponent<ListViewItem>();
				base_items.Add(item);
			});
			this.objects = newItems;
			base.Items = base_items;
			selected_indicies.ForEach(delegate(int x)
			{
				this.Select(x);
			});
			this.AddCallbacks();
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x001833FA File Offset: 0x001817FA
		protected override void OnDestroy()
		{
			this.OnSelect.RemoveListener(new UnityAction<int, ListViewItem>(this.OnSelectCallback));
			this.OnDeselect.RemoveListener(new UnityAction<int, ListViewItem>(this.OnDeselectCallback));
			this.RemoveCallbacks();
			base.OnDestroy();
		}

		// Token: 0x04003101 RID: 12545
		[SerializeField]
		private List<GameObject> objects = new List<GameObject>();

		// Token: 0x04003102 RID: 12546
		private List<UnityAction<PointerEventData>> callbacksEnter = new List<UnityAction<PointerEventData>>();

		// Token: 0x04003103 RID: 12547
		private List<UnityAction<PointerEventData>> callbacksExit = new List<UnityAction<PointerEventData>>();

		// Token: 0x04003104 RID: 12548
		public Func<IEnumerable<GameObject>, IEnumerable<GameObject>> SortFunc;

		// Token: 0x04003105 RID: 12549
		public ListViewGameObjectsEvent OnSelectObject = new ListViewGameObjectsEvent();

		// Token: 0x04003106 RID: 12550
		public ListViewGameObjectsEvent OnDeselectObject = new ListViewGameObjectsEvent();

		// Token: 0x04003107 RID: 12551
		public ListViewGameObjectsEvent OnPointerEnterObject = new ListViewGameObjectsEvent();

		// Token: 0x04003108 RID: 12552
		public ListViewGameObjectsEvent OnPointerExitObject = new ListViewGameObjectsEvent();

		// Token: 0x04003109 RID: 12553
		[NonSerialized]
		private bool isStartedListViewGameObjects;
	}
}
