using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EasyLayout;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x02000953 RID: 2387
	public class Notify : MonoBehaviour, ITemplatable
	{
		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x0600483E RID: 18494 RVA: 0x00183A24 File Offset: 0x00181E24
		// (set) Token: 0x0600483F RID: 18495 RVA: 0x00183A59 File Offset: 0x00181E59
		public Button HideButton
		{
			get
			{
				if (this.hideButton != null)
				{
					this.hideButton.onClick.AddListener(new UnityAction(this.Hide));
				}
				return this.hideButton;
			}
			set
			{
				if (this.hideButton != null)
				{
					this.hideButton.onClick.RemoveListener(new UnityAction(this.Hide));
				}
				this.hideButton = value;
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06004840 RID: 18496 RVA: 0x00183A8F File Offset: 0x00181E8F
		// (set) Token: 0x06004841 RID: 18497 RVA: 0x00183A97 File Offset: 0x00181E97
		public Text Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06004842 RID: 18498 RVA: 0x00183AA0 File Offset: 0x00181EA0
		// (set) Token: 0x06004843 RID: 18499 RVA: 0x00183AA8 File Offset: 0x00181EA8
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

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06004844 RID: 18500 RVA: 0x00183AB1 File Offset: 0x00181EB1
		// (set) Token: 0x06004845 RID: 18501 RVA: 0x00183AB9 File Offset: 0x00181EB9
		public string TemplateName { get; set; }

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06004846 RID: 18502 RVA: 0x00183AC2 File Offset: 0x00181EC2
		// (set) Token: 0x06004847 RID: 18503 RVA: 0x00183AFA File Offset: 0x00181EFA
		public static Templates<Notify> Templates
		{
			get
			{
				if (Notify.templates == null)
				{
					if (Notify.f__mg0 == null)
					{
						Notify.f__mg0 = new Action<Notify>(Notify.AddCloseCallback);
					}
					Notify.templates = new Templates<Notify>(Notify.f__mg0);
				}
				return Notify.templates;
			}
			set
			{
				Notify.templates = value;
			}
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x00183B02 File Offset: 0x00181F02
		private void Awake()
		{
			if (this.IsTemplate)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x00183B1B File Offset: 0x00181F1B
		private static void FindTemplates()
		{
			Notify.Templates.FindTemplates();
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x00183B27 File Offset: 0x00181F27
		private void OnDestroy()
		{
			if (!this.IsTemplate)
			{
				Notify.templates = null;
				return;
			}
			if (this.TemplateName != null)
			{
				Notify.DeleteTemplate(this.TemplateName);
			}
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x00183B51 File Offset: 0x00181F51
		public static void ClearCache()
		{
			Notify.Templates.ClearCache();
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x00183B5D File Offset: 0x00181F5D
		public static void ClearCache(string templateName)
		{
			Notify.Templates.ClearCache(templateName);
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x00183B6A File Offset: 0x00181F6A
		public static Notify GetTemplate(string template)
		{
			return Notify.Templates.Get(template);
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x00183B77 File Offset: 0x00181F77
		public static void DeleteTemplate(string template)
		{
			Notify.Templates.Delete(template);
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x00183B84 File Offset: 0x00181F84
		public static void AddTemplate(string template, Notify notifyTemplate, bool replace = true)
		{
			Notify.Templates.Add(template, notifyTemplate, replace);
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x00183B93 File Offset: 0x00181F93
		public static Notify Template(string template)
		{
			return Notify.Templates.Instance(template);
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x00183BA0 File Offset: 0x00181FA0
		private static void AddCloseCallback(Notify notify)
		{
			if (notify.hideButton == null)
			{
				return;
			}
			notify.hideButton.onClick.AddListener(new UnityAction(notify.Hide));
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x00183BD0 File Offset: 0x00181FD0
		public void Show(string message = null, float? customHideDelay = null, Transform container = null, Func<Notify, IEnumerator> showAnimation = null, Func<Notify, IEnumerator> hideAnimation = null, bool? slideUpOnHide = null)
		{
			this.oldShowAnimation = this.ShowAnimation;
			this.oldHideAnimation = this.HideAnimation;
			if (message != null && this.text != null)
			{
				this.text.text = message;
			}
			if (container != null)
			{
				base.transform.SetParent(container, false);
			}
			if (customHideDelay != null)
			{
				this.HideDelay = customHideDelay.Value;
			}
			if (slideUpOnHide != null)
			{
				this.SlideUpOnHide = slideUpOnHide.Value;
			}
			if (showAnimation != null)
			{
				this.ShowAnimation = showAnimation;
			}
			if (hideAnimation != null)
			{
				this.HideAnimation = hideAnimation;
			}
			base.gameObject.SetActive(true);
			base.transform.SetAsLastSibling();
			if (this.ShowAnimation != null)
			{
				this.showCorutine = this.ShowAnimation(this);
				base.StartCoroutine(this.showCorutine);
			}
			if (this.HideDelay > 0f)
			{
				this.hideCorutine = this.HideCorutine();
				base.StartCoroutine(this.hideCorutine);
			}
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x00183CEC File Offset: 0x001820EC
		private IEnumerator HideCorutine()
		{
			yield return new WaitForSeconds(this.HideDelay);
			if (this.HideAnimation != null)
			{
				yield return base.StartCoroutine(this.HideAnimation(this));
			}
			this.Hide();
			yield break;
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x00183D07 File Offset: 0x00182107
		public void Hide()
		{
			if (this.SlideUpOnHide)
			{
				this.SlideUp();
			}
			this.Return();
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x00183D20 File Offset: 0x00182120
		private void Return()
		{
			Notify.Templates.ToCache(this);
			this.ShowAnimation = this.oldShowAnimation;
			this.HideAnimation = this.oldHideAnimation;
			if (this.text != null)
			{
				this.text.text = Notify.Templates.Get(this.TemplateName).text.text;
			}
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x00183D88 File Offset: 0x00182188
		private void SlideUp()
		{
			Notify.slides = new Stack<RectTransform>(from x in Notify.slides
			where x != null
			select x);
			RectTransform rectTransform;
			SlideUp slideUp;
			if (Notify.slides.Count == 0)
			{
				GameObject gameObject = new GameObject("SlideUp");
				gameObject.SetActive(false);
				rectTransform = gameObject.AddComponent<RectTransform>();
				slideUp = gameObject.AddComponent<SlideUp>();
				Image image = gameObject.AddComponent<Image>();
				image.color = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				rectTransform = Notify.slides.Pop();
				slideUp = rectTransform.GetComponent<SlideUp>();
			}
			RectTransform component = base.GetComponent<RectTransform>();
			rectTransform.localRotation = component.localRotation;
			rectTransform.localPosition = component.localPosition;
			rectTransform.localScale = component.localScale;
			rectTransform.anchorMin = component.anchorMin;
			rectTransform.anchorMax = component.anchorMax;
			rectTransform.anchoredPosition = component.anchoredPosition;
			rectTransform.anchoredPosition3D = component.anchoredPosition3D;
			rectTransform.sizeDelta = component.sizeDelta;
			rectTransform.pivot = component.pivot;
			rectTransform.transform.SetParent(base.transform.parent, false);
			rectTransform.transform.SetSiblingIndex(base.transform.GetSiblingIndex());
			rectTransform.gameObject.SetActive(true);
			slideUp.Run();
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x00183EEE File Offset: 0x001822EE
		public static void FreeSlide(RectTransform slide)
		{
			Notify.slides.Push(slide);
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x00183EFC File Offset: 0x001822FC
		public static IEnumerator AnimationRotate(Notify notify)
		{
			RectTransform rect = notify.GetComponent<RectTransform>();
			Vector3 start_rotarion = rect.rotation.eulerAngles;
			float time = 0.5f;
			float end_time = Time.time + time;
			while (Time.time <= end_time)
			{
				float rotation_x = Mathf.Lerp(0f, 90f, 1f - (end_time - Time.time) / time);
				rect.rotation = Quaternion.Euler(rotation_x, start_rotarion.y, start_rotarion.z);
				yield return null;
			}
			rect.rotation = Quaternion.Euler(start_rotarion);
			yield break;
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x00183F18 File Offset: 0x00182318
		public static IEnumerator AnimationCollapse(Notify notify)
		{
			RectTransform rect = notify.GetComponent<RectTransform>();
			EasyLayout.EasyLayout layout = notify.GetComponentInParent<EasyLayout.EasyLayout>();
			float max_height = rect.rect.height;
			float speed = 200f;
			float time = max_height / speed;
			float end_time = Time.time + time;
			while (Time.time <= end_time)
			{
				float height = Mathf.Lerp(max_height, 0f, 1f - (end_time - Time.time) / time);
				rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
				if (layout != null)
				{
					layout.UpdateLayout();
				}
				yield return null;
			}
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, max_height);
			yield break;
		}

		// Token: 0x04003123 RID: 12579
		[SerializeField]
		private Button hideButton;

		// Token: 0x04003124 RID: 12580
		[SerializeField]
		private Text text;

		// Token: 0x04003125 RID: 12581
		[SerializeField]
		private float HideDelay = 10f;

		// Token: 0x04003126 RID: 12582
		private bool isTemplate = true;

		// Token: 0x04003128 RID: 12584
		private static Templates<Notify> templates;

		// Token: 0x04003129 RID: 12585
		public Func<Notify, IEnumerator> ShowAnimation;

		// Token: 0x0400312A RID: 12586
		public Func<Notify, IEnumerator> HideAnimation;

		// Token: 0x0400312B RID: 12587
		private Func<Notify, IEnumerator> oldShowAnimation;

		// Token: 0x0400312C RID: 12588
		private Func<Notify, IEnumerator> oldHideAnimation;

		// Token: 0x0400312D RID: 12589
		private IEnumerator showCorutine;

		// Token: 0x0400312E RID: 12590
		private IEnumerator hideCorutine;

		// Token: 0x0400312F RID: 12591
		public bool SlideUpOnHide = true;

		// Token: 0x04003130 RID: 12592
		private static Stack<RectTransform> slides = new Stack<RectTransform>();

		// Token: 0x04003131 RID: 12593
		[CompilerGenerated]
		private static Action<Notify> f__mg0;
	}
}
