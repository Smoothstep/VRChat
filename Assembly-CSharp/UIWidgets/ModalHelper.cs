using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x02000952 RID: 2386
	[RequireComponent(typeof(RectTransform))]
	public class ModalHelper : MonoBehaviour, ITemplatable
	{
		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06004835 RID: 18485 RVA: 0x00183851 File Offset: 0x00181C51
		// (set) Token: 0x06004836 RID: 18486 RVA: 0x00183859 File Offset: 0x00181C59
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

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06004837 RID: 18487 RVA: 0x00183862 File Offset: 0x00181C62
		// (set) Token: 0x06004838 RID: 18488 RVA: 0x0018386A File Offset: 0x00181C6A
		public string TemplateName { get; set; }

		// Token: 0x06004839 RID: 18489 RVA: 0x00183874 File Offset: 0x00181C74
		public static int Open(MonoBehaviour parent, Sprite sprite = null, Color? color = null)
		{
			if (!ModalHelper.Templates.Exists(ModalHelper.key))
			{
				ModalHelper.Templates.FindTemplates();
				ModalHelper.CreateTemplate();
			}
			ModalHelper modalHelper = ModalHelper.Templates.Instance(ModalHelper.key);
			modalHelper.transform.SetParent(Utilites.FindCanvas(parent.transform), false);
			modalHelper.gameObject.SetActive(true);
			modalHelper.transform.SetAsLastSibling();
			RectTransform component = modalHelper.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(0f, 0f);
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 1f);
			component.anchoredPosition = new Vector2(0f, 0f);
			Image component2 = modalHelper.GetComponent<Image>();
			if (sprite != null)
			{
				component2.sprite = sprite;
			}
			if (color != null)
			{
				component2.color = color.Value;
			}
			ModalHelper.used.Add(modalHelper.GetInstanceID(), modalHelper);
			return modalHelper.GetInstanceID();
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x00183988 File Offset: 0x00181D88
		private static void CreateTemplate()
		{
			GameObject gameObject = new GameObject(ModalHelper.key);
			ModalHelper template = gameObject.AddComponent<ModalHelper>();
			gameObject.AddComponent<Image>();
			ModalHelper.Templates.Add(ModalHelper.key, template, true);
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x001839BF File Offset: 0x00181DBF
		public static void Close(int index)
		{
			ModalHelper.Templates.ToCache(ModalHelper.used[index]);
			ModalHelper.used.Remove(index);
		}

		// Token: 0x0400311E RID: 12574
		private bool isTemplate = true;

		// Token: 0x04003120 RID: 12576
		private static Templates<ModalHelper> Templates = new Templates<ModalHelper>(null);

		// Token: 0x04003121 RID: 12577
		private static Dictionary<int, ModalHelper> used = new Dictionary<int, ModalHelper>();

		// Token: 0x04003122 RID: 12578
		private static string key = "ModalTemplate";
	}
}
