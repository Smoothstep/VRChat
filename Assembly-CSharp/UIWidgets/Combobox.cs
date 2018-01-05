using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x0200092D RID: 2349
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Combobox", 220)]
	public class Combobox : MonoBehaviour, ISubmitHandler, IEventSystemHandler
	{
		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06004672 RID: 18034 RVA: 0x0017E9A1 File Offset: 0x0017CDA1
		// (set) Token: 0x06004673 RID: 18035 RVA: 0x0017E9AC File Offset: 0x0017CDAC
		public ListView ListView
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
					this.listView.OnSelectString.RemoveListener(new UnityAction<int, string>(this.SelectItem));
					this.listView.OnFocusOut.RemoveListener(new UnityAction<BaseEventData>(this.onFocusHideList));
					this.listView.onCancel.AddListener(new UnityAction(this.OnListViewCancel));
					this.listView.onItemCancel.AddListener(new UnityAction(this.OnListViewCancel));
					this.RemoveDeselectCallbacks();
				}
				this.listView = value;
				if (this.listView != null)
				{
					this.listParent = this.listView.transform.parent;
					this.listView.OnSelectString.AddListener(new UnityAction<int, string>(this.SelectItem));
					this.listView.OnFocusOut.AddListener(new UnityAction<BaseEventData>(this.onFocusHideList));
					this.listView.onCancel.AddListener(new UnityAction(this.OnListViewCancel));
					this.listView.onItemCancel.AddListener(new UnityAction(this.OnListViewCancel));
					this.AddDeselectCallbacks();
				}
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06004674 RID: 18036 RVA: 0x0017EAEB File Offset: 0x0017CEEB
		// (set) Token: 0x06004675 RID: 18037 RVA: 0x0017EAF4 File Offset: 0x0017CEF4
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

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06004676 RID: 18038 RVA: 0x0017EB62 File Offset: 0x0017CF62
		// (set) Token: 0x06004677 RID: 18039 RVA: 0x0017EB6A File Offset: 0x0017CF6A
		public bool Editable
		{
			get
			{
				return this.editable;
			}
			set
			{
				this.editable = value;
				this.input.interactable = this.editable;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06004678 RID: 18040 RVA: 0x0017EB84 File Offset: 0x0017CF84
		private Transform listCanvas
		{
			get
			{
				if (this._listCanvas == null)
				{
					this._listCanvas = Utilites.FindCanvas(this.listView.transform.parent);
				}
				return this._listCanvas;
			}
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x0017EBB8 File Offset: 0x0017CFB8
		private void Awake()
		{
			this.Start();
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x0017EBC0 File Offset: 0x0017CFC0
		public void Start()
		{
			if (this.isStartedCombobox)
			{
				return;
			}
			this.isStartedCombobox = true;
			this.input = base.GetComponent<InputField>();
			this.input.onEndEdit.AddListener(new UnityAction<string>(this.InputItem));
			this.Editable = this.editable;
			this.ToggleButton = this.toggleButton;
			this.ListView = this.listView;
			if (this.listView != null)
			{
				this.listView.OnSelectString.RemoveListener(new UnityAction<int, string>(this.SelectItem));
				this.listView.OnSelectString.AddListener(delegate(int index, string item)
				{
					this.OnSelect.Invoke(index, item);
				});
				this.listView.gameObject.SetActive(true);
				this.listView.Start();
				if (this.listView.SelectedIndex == -1 && this.listView.Strings.Count > 0)
				{
					this.listView.SelectedIndex = 0;
				}
				if (this.listView.SelectedIndex != -1)
				{
					this.input.text = this.listView.Strings[this.listView.SelectedIndex];
				}
				this.listView.gameObject.SetActive(false);
				this.listView.OnSelectString.AddListener(new UnityAction<int, string>(this.SelectItem));
			}
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x0017ED28 File Offset: 0x0017D128
		public virtual void Clear()
		{
			this.listView.Clear();
			this.input.text = string.Empty;
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x0017ED45 File Offset: 0x0017D145
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

		// Token: 0x0600467D RID: 18045 RVA: 0x0017ED80 File Offset: 0x0017D180
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

		// Token: 0x0600467E RID: 18046 RVA: 0x0017EE50 File Offset: 0x0017D250
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

		// Token: 0x0600467F RID: 18047 RVA: 0x0017EEA8 File Offset: 0x0017D2A8
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

		// Token: 0x06004680 RID: 18048 RVA: 0x0017EF68 File Offset: 0x0017D368
		private void SetChildDeselectListener(GameObject child)
		{
			SelectListener deselectListener = this.GetDeselectListener(child);
			if (!this.childrenDeselect.Contains(deselectListener))
			{
				deselectListener.onDeselect.AddListener(new UnityAction<BaseEventData>(this.onFocusHideList));
				this.childrenDeselect.Add(deselectListener);
			}
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x0017EFB1 File Offset: 0x0017D3B1
		private SelectListener GetDeselectListener(GameObject go)
		{
			return go.GetComponent<SelectListener>() ?? go.AddComponent<SelectListener>();
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x0017EFC8 File Offset: 0x0017D3C8
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

		// Token: 0x06004683 RID: 18051 RVA: 0x0017F02C File Offset: 0x0017D42C
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

		// Token: 0x06004684 RID: 18052 RVA: 0x0017F050 File Offset: 0x0017D450
		public int Set(string item, bool allowDuplicate = true)
		{
			int num;
			if (!allowDuplicate)
			{
				num = this.listView.Strings.FindIndex((string x) => x == item);
				if (num == -1)
				{
					num = this.listView.Add(item);
				}
			}
			else
			{
				num = this.listView.Add(item);
			}
			this.listView.Select(num);
			return num;
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x0017F0CC File Offset: 0x0017D4CC
		private void SelectItem(int index, string text)
		{
			this.input.text = text;
			this.HideList();
			if (EventSystem.current != null && !EventSystem.current.alreadySelecting)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x0017F11C File Offset: 0x0017D51C
		private void InputItem(string item)
		{
			if (!this.editable)
			{
				return;
			}
			if (item == string.Empty)
			{
				return;
			}
			if (!this.listView.Strings.Contains(item))
			{
				int index = this.listView.Add(item);
				this.listView.Select(index);
			}
		}

		// Token: 0x06004687 RID: 18055 RVA: 0x0017F175 File Offset: 0x0017D575
		private void OnDestroy()
		{
			if (this.input != null)
			{
				this.input.onEndEdit.RemoveListener(new UnityAction<string>(this.InputItem));
			}
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x0017F1A4 File Offset: 0x0017D5A4
		private void OnListViewCancel()
		{
			this.HideList();
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x0017F1AC File Offset: 0x0017D5AC
		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			this.ShowList();
		}

		// Token: 0x04003038 RID: 12344
		[SerializeField]
		private ListView listView;

		// Token: 0x04003039 RID: 12345
		[SerializeField]
		private Button toggleButton;

		// Token: 0x0400303A RID: 12346
		[SerializeField]
		private bool editable = true;

		// Token: 0x0400303B RID: 12347
		private InputField input;

		// Token: 0x0400303C RID: 12348
		public ListViewEvent OnSelect = new ListViewEvent();

		// Token: 0x0400303D RID: 12349
		private Transform _listCanvas;

		// Token: 0x0400303E RID: 12350
		private Transform listParent;

		// Token: 0x0400303F RID: 12351
		[NonSerialized]
		private bool isStartedCombobox;

		// Token: 0x04003040 RID: 12352
		private List<SelectListener> childrenDeselect = new List<SelectListener>();
	}
}
