using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x0200092E RID: 2350
	public class ComboboxCustom<TListViewCustom, TComponent, TItem> : MonoBehaviour, ISubmitHandler, IEventSystemHandler where TListViewCustom : ListViewCustom<TComponent, TItem> where TComponent : ListViewItem
	{
		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x0600468D RID: 18061 RVA: 0x0017F21C File Offset: 0x0017D61C
		// (set) Token: 0x0600468E RID: 18062 RVA: 0x0017F224 File Offset: 0x0017D624
		public TListViewCustom ListView
		{
			get
			{
				return this.listView;
			}
			set
			{
				if (this.listView != null)
				{
					this.listParent = null;
					this.listView.OnSelectObject.RemoveListener(new UnityAction<int>(this.SetCurrent));
					this.listView.OnSelectObject.RemoveListener(this.onSelectCallback);
					this.listView.OnFocusOut.RemoveListener(new UnityAction<BaseEventData>(this.onFocusHideList));
					this.listView.onCancel.RemoveListener(new UnityAction(this.OnListViewCancel));
					this.listView.onItemCancel.RemoveListener(new UnityAction(this.OnListViewCancel));
					this.RemoveDeselectCallbacks();
				}
				this.listView = value;
				if (this.listView != null)
				{
					this.listParent = this.listView.transform.parent;
					this.listView.OnSelectObject.AddListener(new UnityAction<int>(this.SetCurrent));
					this.listView.OnSelectObject.AddListener(this.onSelectCallback);
					this.listView.OnFocusOut.AddListener(new UnityAction<BaseEventData>(this.onFocusHideList));
					this.listView.onCancel.AddListener(new UnityAction(this.OnListViewCancel));
					this.listView.onItemCancel.AddListener(new UnityAction(this.OnListViewCancel));
					this.AddDeselectCallbacks();
				}
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x0600468F RID: 18063 RVA: 0x0017F3D1 File Offset: 0x0017D7D1
		// (set) Token: 0x06004690 RID: 18064 RVA: 0x0017F3DC File Offset: 0x0017D7DC
		public Button ToggleButton
		{
			get
			{
				return this.toggleButton;
			}
			set
			{
				if (this.toggleButton != null)
				{
					this.toggleButton.onClick.RemoveListener(new UnityAction(this.ToggleList));
				}
				this.toggleButton = value;
				if (this.toggleButton != null)
				{
					this.toggleButton.onClick.AddListener(new UnityAction(this.ToggleList));
				}
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06004691 RID: 18065 RVA: 0x0017F44A File Offset: 0x0017D84A
		// (set) Token: 0x06004692 RID: 18066 RVA: 0x0017F452 File Offset: 0x0017D852
		public TComponent Current
		{
			get
			{
				return this.current;
			}
			set
			{
				this.current = value;
			}
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x0017F45B File Offset: 0x0017D85B
		private void Awake()
		{
			this.Start();
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x0017F464 File Offset: 0x0017D864
		public virtual void Start()
		{
			if (this.isStartedComboboxCustom)
			{
				return;
			}
			this.isStartedComboboxCustom = true;
			this.onSelectCallback = delegate(int index)
			{
				this.OnSelect.Invoke(index, this.listView.Items[index]);
			};
			this.ToggleButton = this.toggleButton;
			this.ListView = this.listView;
			if (this.listView != null)
			{
				this.listView.OnSelectObject.RemoveListener(new UnityAction<int>(this.SetCurrent));
				this.listCanvas = Utilites.FindCanvas(this.listParent);
				this.listView.gameObject.SetActive(true);
				this.listView.Start();
				if (this.listView.SelectedIndex == -1 && this.listView.Items.Count > 0)
				{
					this.listView.SelectedIndex = 0;
				}
				if (this.listView.SelectedIndex != -1)
				{
					this.UpdateCurrent();
				}
				this.listView.gameObject.SetActive(false);
				this.listView.OnSelectObject.AddListener(new UnityAction<int>(this.SetCurrent));
			}
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x0017F5B8 File Offset: 0x0017D9B8
		public virtual void Clear()
		{
			this.listView.Clear();
			this.UpdateCurrent();
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x0017F5D4 File Offset: 0x0017D9D4
		public void ToggleList()
		{
			if (this.listView == null)
			{
				return;
			}
			if (this.listView.gameObject.activeSelf)
			{
				this.HideList();
			}
			else
			{
				this.ShowList();
			}
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x0017F624 File Offset: 0x0017DA24
		public void ShowList()
		{
			if (this.listView == null)
			{
				return;
			}
			if (this.listCanvas != null)
			{
				this.listParent = this.listView.transform.parent;
				this.listView.transform.SetParent(this.listCanvas);
			}
			this.listView.gameObject.SetActive(true);
			if (this.listView.Layout != null)
			{
				this.listView.Layout.UpdateLayout();
			}
			if (this.listView.SelectComponent())
			{
				this.SetChildDeselectListener(EventSystem.current.currentSelectedGameObject);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(this.listView.gameObject);
			}
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x0017F720 File Offset: 0x0017DB20
		public void HideList()
		{
			if (this.listView == null)
			{
				return;
			}
			this.listView.gameObject.SetActive(false);
			if (this.listCanvas != null)
			{
				this.listView.transform.SetParent(this.listParent);
			}
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x0017F788 File Offset: 0x0017DB88
		private void onFocusHideList(BaseEventData eventData)
		{
			if (eventData.selectedObject == base.gameObject)
			{
				return;
			}
			ListViewItemEventData listViewItemEventData = eventData as ListViewItemEventData;
			if (listViewItemEventData != null)
			{
				if (listViewItemEventData.NewSelectedObject != null)
				{
					this.SetChildDeselectListener(listViewItemEventData.NewSelectedObject);
				}
				return;
			}
			PointerEventData pointerEventData = eventData as PointerEventData;
			if (pointerEventData == null)
			{
				this.HideList();
				return;
			}
			GameObject gameObject = pointerEventData.pointerPressRaycast.gameObject;
			if (gameObject == null)
			{
				this.HideList();
				return;
			}
			if (gameObject.Equals(this.toggleButton.gameObject))
			{
				return;
			}
			if (gameObject.transform.IsChildOf(this.listView.transform))
			{
				this.SetChildDeselectListener(gameObject);
				return;
			}
			this.HideList();
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x0017F854 File Offset: 0x0017DC54
		private void SetChildDeselectListener(GameObject child)
		{
			SelectListener deselectListener = this.GetDeselectListener(child);
			if (!this.childrenDeselect.Contains(deselectListener))
			{
				deselectListener.onDeselect.AddListener(new UnityAction<BaseEventData>(this.onFocusHideList));
				this.childrenDeselect.Add(deselectListener);
			}
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x0017F89D File Offset: 0x0017DC9D
		private SelectListener GetDeselectListener(GameObject go)
		{
			return go.GetComponent<SelectListener>() ?? go.AddComponent<SelectListener>();
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x0017F8B4 File Offset: 0x0017DCB4
		private void AddDeselectCallbacks()
		{
			if (this.listView.ScrollRect != null)
			{
				GameObject gameObject = this.listView.ScrollRect.verticalScrollbar.gameObject;
				SelectListener deselectListener = this.GetDeselectListener(gameObject);
				deselectListener.onDeselect.AddListener(new UnityAction<BaseEventData>(this.onFocusHideList));
				this.childrenDeselect.Add(deselectListener);
			}
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x0017F924 File Offset: 0x0017DD24
		private void RemoveDeselectCallbacks()
		{
			this.childrenDeselect.ForEach(delegate(SelectListener x)
			{
				if (x != null)
				{
					x.onDeselect.RemoveListener(new UnityAction<BaseEventData>(this.onFocusHideList));
				}
			});
			this.childrenDeselect.Clear();
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x0017F948 File Offset: 0x0017DD48
		private void SetCurrent(int index)
		{
			this.UpdateCurrent();
			if (EventSystem.current != null && !EventSystem.current.alreadySelecting)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x0017F97F File Offset: 0x0017DD7F
		private void OnListViewCancel()
		{
			this.HideList();
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x0017F987 File Offset: 0x0017DD87
		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			this.ShowList();
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x0017F98F File Offset: 0x0017DD8F
		protected virtual void UpdateCurrent()
		{
			this.HideList();
		}

		// Token: 0x04003041 RID: 12353
		[SerializeField]
		private TListViewCustom listView;

		// Token: 0x04003042 RID: 12354
		[SerializeField]
		private Button toggleButton;

		// Token: 0x04003043 RID: 12355
		[SerializeField]
		private TComponent current;

		// Token: 0x04003044 RID: 12356
		public ComboboxCustom<TListViewCustom, TComponent, TItem>.ComboboxCustomEvent OnSelect = new ComboboxCustom<TListViewCustom, TComponent, TItem>.ComboboxCustomEvent();

		// Token: 0x04003045 RID: 12357
		private UnityAction<int> onSelectCallback;

		// Token: 0x04003046 RID: 12358
		private Transform listCanvas;

		// Token: 0x04003047 RID: 12359
		private Transform listParent;

		// Token: 0x04003048 RID: 12360
		[NonSerialized]
		private bool isStartedComboboxCustom;

		// Token: 0x04003049 RID: 12361
		private bool isOpeningList;

		// Token: 0x0400304A RID: 12362
		private List<SelectListener> childrenDeselect = new List<SelectListener>();

		// Token: 0x0200092F RID: 2351
		public class ComboboxCustomEvent : UnityEvent<int, TItem>
		{
		}
	}
}
