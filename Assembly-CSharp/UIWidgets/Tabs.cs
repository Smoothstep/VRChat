using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x0200096B RID: 2411
	[AddComponentMenu("UI/Tabs", 290)]
	public class Tabs : MonoBehaviour
	{
		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06004920 RID: 18720 RVA: 0x00186C84 File Offset: 0x00185084
		// (set) Token: 0x06004921 RID: 18721 RVA: 0x00186C8C File Offset: 0x0018508C
		public Tab[] TabObjects
		{
			get
			{
				return this.tabObjects;
			}
			set
			{
				this.tabObjects = value;
				this.UpdateButtons();
			}
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x00186C9C File Offset: 0x0018509C
		private void Start()
		{
			if (this.Container == null)
			{
				throw new NullReferenceException("Container is null. Set object of type GameObject to Container.");
			}
			if (this.DefaultTabButton == null)
			{
				throw new NullReferenceException("DefaultTabButton is null. Set object of type GameObject to DefaultTabButton.");
			}
			if (this.ActiveTabButton == null)
			{
				throw new NullReferenceException("ActiveTabButton is null. Set object of type GameObject to ActiveTabButton.");
			}
			this.DefaultTabButton.gameObject.SetActive(false);
			this.ActiveTabButton.gameObject.SetActive(false);
			this.UpdateButtons();
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x00186D28 File Offset: 0x00185128
		private void UpdateButtons()
		{
			if (this.tabObjects.Length == 0)
			{
				throw new ArgumentException("TabObjects array is empty. Fill it.");
			}
			this.defaultButtons.ForEach(delegate(Button x, int index)
			{
				x.onClick.RemoveListener(this.callbacks[index]);
			});
			this.callbacks.Clear();
			this.CreateButtons();
			this.tabObjects.ToList<Tab>().ForEach(delegate(Tab x, int index)
			{
				string tabName = x.Name;
				UnityAction item = delegate
				{
					this.SelectTab(tabName);
				};
				this.callbacks.Add(item);
				this.defaultButtons[index].onClick.AddListener(this.callbacks[index]);
			});
			this.SelectTab(this.tabObjects[0].Name);
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x00186DA4 File Offset: 0x001851A4
		public void SelectTab(string tabName)
		{
			int num = Array.FindIndex<Tab>(this.tabObjects, (Tab x) => x.Name == tabName);
			if (num == -1)
			{
				throw new ArgumentException(string.Format("Tab with name \"{0}\" not found.", tabName));
			}
			if (this.KeepTabsActive)
			{
				this.tabObjects[num].TabObject.transform.SetAsLastSibling();
			}
			else
			{
				this.tabObjects.ForEach(delegate(Tab x)
				{
					x.TabObject.SetActive(false);
				});
				this.tabObjects[num].TabObject.SetActive(true);
			}
			this.defaultButtons.ForEach(delegate(Button x)
			{
				x.gameObject.SetActive(true);
			});
			this.defaultButtons[num].gameObject.SetActive(false);
			this.activeButtons.ForEach(delegate(Button x)
			{
				x.gameObject.SetActive(false);
			});
			this.activeButtons[num].gameObject.SetActive(true);
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x00186ED4 File Offset: 0x001852D4
		private void CreateButtons()
		{
			if (this.tabObjects.Length > this.defaultButtons.Count)
			{
				for (int i = this.defaultButtons.Count; i < this.tabObjects.Length; i++)
				{
					Button button = UnityEngine.Object.Instantiate<Button>(this.DefaultTabButton);
					button.transform.SetParent(this.Container, false);
					Utilites.FixInstantiated(this.DefaultTabButton, button);
					this.defaultButtons.Add(button);
					Button button2 = UnityEngine.Object.Instantiate<Button>(this.ActiveTabButton);
					button2.transform.SetParent(this.Container, false);
					Utilites.FixInstantiated(this.ActiveTabButton, button2);
					this.activeButtons.Add(button2);
				}
			}
			if (this.tabObjects.Length < this.defaultButtons.Count)
			{
				for (int j = this.defaultButtons.Count; j > this.tabObjects.Length; j--)
				{
					UnityEngine.Object.Destroy(this.defaultButtons[j]);
					UnityEngine.Object.Destroy(this.activeButtons[j]);
					this.defaultButtons.RemoveAt(j);
					this.activeButtons.RemoveAt(j);
				}
			}
			this.defaultButtons.ForEach(new Action<Button, int>(this.SetButtonName));
			this.activeButtons.ForEach(new Action<Button, int>(this.SetButtonName));
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x0018702C File Offset: 0x0018542C
		private void SetButtonName(Button button, int index)
		{
			button.gameObject.SetActive(true);
			Text componentInChildren = button.GetComponentInChildren<Text>();
			if (componentInChildren)
			{
				componentInChildren.text = this.tabObjects[index].Name;
			}
		}

		// Token: 0x0400318F RID: 12687
		[SerializeField]
		public Transform Container;

		// Token: 0x04003190 RID: 12688
		[SerializeField]
		public Button DefaultTabButton;

		// Token: 0x04003191 RID: 12689
		[SerializeField]
		public Button ActiveTabButton;

		// Token: 0x04003192 RID: 12690
		[SerializeField]
		private Tab[] tabObjects = new Tab[0];

		// Token: 0x04003193 RID: 12691
		[SerializeField]
		[Tooltip("If true does not deactivate hidden tabs.")]
		public bool KeepTabsActive;

		// Token: 0x04003194 RID: 12692
		private List<Button> defaultButtons = new List<Button>();

		// Token: 0x04003195 RID: 12693
		private List<Button> activeButtons = new List<Button>();

		// Token: 0x04003196 RID: 12694
		private List<UnityAction> callbacks = new List<UnityAction>();
	}
}
