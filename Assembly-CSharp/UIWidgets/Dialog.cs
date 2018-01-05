using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x02000931 RID: 2353
	public class Dialog : MonoBehaviour, ITemplatable
	{
		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x060046AA RID: 18090 RVA: 0x0017FA62 File Offset: 0x0017DE62
		// (set) Token: 0x060046AB RID: 18091 RVA: 0x0017FA6A File Offset: 0x0017DE6A
		public Button DefaultButton
		{
			get
			{
				return this.defaultButton;
			}
			set
			{
				this.defaultButton = value;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x060046AC RID: 18092 RVA: 0x0017FA73 File Offset: 0x0017DE73
		// (set) Token: 0x060046AD RID: 18093 RVA: 0x0017FA7B File Offset: 0x0017DE7B
		public Text TitleText
		{
			get
			{
				return this.titleText;
			}
			set
			{
				this.titleText = value;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060046AE RID: 18094 RVA: 0x0017FA84 File Offset: 0x0017DE84
		// (set) Token: 0x060046AF RID: 18095 RVA: 0x0017FA8C File Offset: 0x0017DE8C
		public Text ContentText
		{
			get
			{
				return this.contentText;
			}
			set
			{
				this.contentText = value;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x060046B0 RID: 18096 RVA: 0x0017FA95 File Offset: 0x0017DE95
		// (set) Token: 0x060046B1 RID: 18097 RVA: 0x0017FA9D File Offset: 0x0017DE9D
		public Image Icon
		{
			get
			{
				return this.dialogIcon;
			}
			set
			{
				this.dialogIcon = value;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x0017FAA6 File Offset: 0x0017DEA6
		// (set) Token: 0x060046B3 RID: 18099 RVA: 0x0017FAAE File Offset: 0x0017DEAE
		public bool IsTemplate
		{
			get
			{
				return this.isTemplate;
			}
			set
			{
				this.isTemplate = value;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x060046B4 RID: 18100 RVA: 0x0017FAB7 File Offset: 0x0017DEB7
		// (set) Token: 0x060046B5 RID: 18101 RVA: 0x0017FABF File Offset: 0x0017DEBF
		public string TemplateName { get; set; }

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060046B6 RID: 18102 RVA: 0x0017FAC8 File Offset: 0x0017DEC8
		// (set) Token: 0x060046B7 RID: 18103 RVA: 0x0017FAE4 File Offset: 0x0017DEE4
		public static Templates<Dialog> Templates
		{
			get
			{
				if (Dialog.templates == null)
				{
					Dialog.templates = new Templates<Dialog>(null);
				}
				return Dialog.templates;
			}
			set
			{
				Dialog.templates = value;
			}
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x0017FAEC File Offset: 0x0017DEEC
		private void Awake()
		{
			if (this.IsTemplate)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x0017FB05 File Offset: 0x0017DF05
		private void OnDestroy()
		{
			if (!this.IsTemplate)
			{
				Dialog.templates = null;
				return;
			}
			if (this.TemplateName != null)
			{
				Dialog.Templates.Delete(this.TemplateName);
			}
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x0017FB34 File Offset: 0x0017DF34
		public static Dialog Template(string template)
		{
			return Dialog.Templates.Instance(template);
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x0017FB44 File Offset: 0x0017DF44
		public void Show(string title = null, string message = null, DialogActions buttons = null, string focusButton = null, Vector3? position = null, Sprite icon = null, bool modal = false, Sprite modalSprite = null, Color? modalColor = null, Canvas canvas = null)
		{
			if (position == null)
			{
				position = new Vector3?(new Vector3(0f, 0f, 0f));
			}
			if (title != null && this.TitleText != null)
			{
				this.TitleText.text = title;
			}
			if (message != null && this.ContentText != null)
			{
				this.contentText.text = message;
			}
			if (icon != null && this.Icon != null)
			{
				this.Icon.sprite = icon;
			}
			Transform parent = (!(canvas != null)) ? Utilites.FindCanvas(base.gameObject.transform) : canvas.transform;
			base.transform.SetParent(parent, false);
			if (modal)
			{
				this.modalKey = new int?(ModalHelper.Open(this, modalSprite, modalColor));
			}
			else
			{
				this.modalKey = null;
			}
			base.transform.SetAsLastSibling();
			base.transform.localPosition = position.Value;
			base.gameObject.SetActive(true);
			this.CreateButtons(buttons, focusButton);
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x0017FC88 File Offset: 0x0017E088
		public void Hide()
		{
			int? num = this.modalKey;
			if (num != null)
			{
				int? num2 = this.modalKey;
				ModalHelper.Close(num2.Value);
			}
			this.Return();
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x0017FCC4 File Offset: 0x0017E0C4
		private void CreateButtons(DialogActions buttons, string focusButton)
		{
			this.defaultButton.gameObject.SetActive(false);
			if (buttons == null)
			{
				return;
			}
			buttons.ForEach(delegate(KeyValuePair<string, Func<bool>> x)
			{
				Button button = this.GetButton();
				UnityAction value = delegate
				{
					if (x.Value())
					{
						this.Hide();
					}
				};
				this.buttonsInUse.Add(x.Key, button);
				this.buttonsActions.Add(x.Key, value);
				button.gameObject.SetActive(true);
				button.transform.SetAsLastSibling();
				Text componentInChildren = button.GetComponentInChildren<Text>();
				if (componentInChildren)
				{
					componentInChildren.text = x.Key;
				}
				button.onClick.AddListener(this.buttonsActions[x.Key]);
				if (x.Key == focusButton)
				{
					button.Select();
				}
			});
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x0017FD10 File Offset: 0x0017E110
		private Button GetButton()
		{
			if (this.buttonsCache.Count > 0)
			{
				return this.buttonsCache.Pop();
			}
			Button button = UnityEngine.Object.Instantiate<Button>(this.DefaultButton);
			Utilites.FixInstantiated(this.DefaultButton, button);
			button.transform.SetParent(this.DefaultButton.transform.parent, false);
			return button;
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x0017FD6F File Offset: 0x0017E16F
		private void Return()
		{
			Dialog.Templates.ToCache(this);
			this.DeactivateButtons();
			this.ResetParametres();
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x0017FD88 File Offset: 0x0017E188
		private void DeactivateButtons()
		{
			this.buttonsInUse.ForEach(delegate(KeyValuePair<string, Button> x)
			{
				x.Value.gameObject.SetActive(false);
				x.Value.onClick.RemoveListener(this.buttonsActions[x.Key]);
				this.buttonsCache.Push(x.Value);
			});
			this.buttonsInUse.Clear();
			this.buttonsActions.Clear();
		}

		// Token: 0x060046C1 RID: 18113 RVA: 0x0017FDB8 File Offset: 0x0017E1B8
		private void ResetParametres()
		{
			Dialog dialog = Dialog.Templates.Get(this.TemplateName);
			if (this.TitleText != null && dialog.TitleText != null)
			{
				this.TitleText.text = dialog.TitleText.text;
			}
			if (this.ContentText != null && dialog.ContentText != null)
			{
				this.ContentText.text = dialog.ContentText.text;
			}
			if (this.Icon != null && dialog.Icon != null)
			{
				this.Icon.sprite = dialog.Icon.sprite;
			}
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x0017FE7E File Offset: 0x0017E27E
		public static bool Close()
		{
			return true;
		}

		// Token: 0x0400304C RID: 12364
		[SerializeField]
		private Button defaultButton;

		// Token: 0x0400304D RID: 12365
		[SerializeField]
		private Text titleText;

		// Token: 0x0400304E RID: 12366
		[SerializeField]
		private Text contentText;

		// Token: 0x0400304F RID: 12367
		[SerializeField]
		private Image dialogIcon;

		// Token: 0x04003050 RID: 12368
		private bool isTemplate = true;

		// Token: 0x04003052 RID: 12370
		private static Templates<Dialog> templates;

		// Token: 0x04003053 RID: 12371
		private int? modalKey;

		// Token: 0x04003054 RID: 12372
		private Stack<Button> buttonsCache = new Stack<Button>();

		// Token: 0x04003055 RID: 12373
		private Dictionary<string, Button> buttonsInUse = new Dictionary<string, Button>();

		// Token: 0x04003056 RID: 12374
		private Dictionary<string, UnityAction> buttonsActions = new Dictionary<string, UnityAction>();
	}
}
