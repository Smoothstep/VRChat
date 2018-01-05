using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x02000923 RID: 2339
	[AddComponentMenu("UI/Accordion", 350)]
	public class Accordion : MonoBehaviour
	{
		// Token: 0x0600462D RID: 17965 RVA: 0x0017D847 File Offset: 0x0017BC47
		private void Start()
		{
			this.Items.ForEach(delegate(AccordionItem x)
			{
				if (x.Open)
				{
					this.Open(x, false);
				}
				else
				{
					this.Close(x, false);
				}
				x.ToggleObject.AddComponent<AccordionItemComponent>().OnClick.AddListener(delegate
				{
					this.ToggleItem(x);
				});
				x.ContentObjectRect = x.ContentObject.GetComponent<RectTransform>();
				x.ContentObjectHeight = x.ContentObjectRect.rect.height;
			});
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x0017D860 File Offset: 0x0017BC60
		private void ToggleItem(AccordionItem item)
		{
			if (item.Open)
			{
				if (!this.OnlyOneOpen)
				{
					this.Close(item, this.Animate);
				}
			}
			else
			{
				if (this.OnlyOneOpen)
				{
					(from x in this.Items
					where x.Open
					select x).ForEach(delegate(AccordionItem x)
					{
						this.Close(x, this.Animate);
					});
				}
				this.Open(item, this.Animate);
			}
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x0017D8E8 File Offset: 0x0017BCE8
		private void Open(AccordionItem item, bool animate = false)
		{
			if (item.CurrentCorutine != null)
			{
				base.StopCoroutine(item.CurrentCorutine);
				item.ContentObjectRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.ContentObjectHeight);
				item.ContentObject.SetActive(false);
			}
			if (animate)
			{
				item.CurrentCorutine = base.StartCoroutine(this.OpenCorutine(item));
			}
			else
			{
				item.ContentObject.SetActive(true);
				this.OnToggleItem.Invoke(item);
			}
			item.ContentObject.SetActive(true);
			item.Open = true;
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x0017D974 File Offset: 0x0017BD74
		private void Close(AccordionItem item, bool animate = false)
		{
			if (item.CurrentCorutine != null)
			{
				base.StopCoroutine(item.CurrentCorutine);
				item.ContentObject.SetActive(true);
				item.ContentObjectRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.ContentObjectHeight);
			}
			if (animate)
			{
				item.CurrentCorutine = base.StartCoroutine(this.HideCorutine(item));
			}
			else
			{
				item.ContentObject.SetActive(false);
				this.OnToggleItem.Invoke(item);
			}
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x0017D9EC File Offset: 0x0017BDEC
		private IEnumerator OpenCorutine(AccordionItem item)
		{
			item.ContentObject.SetActive(true);
			item.Open = true;
			yield return base.StartCoroutine(Animations.Open(item.ContentObjectRect, 0.5f));
			this.OnToggleItem.Invoke(item);
			yield break;
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x0017DA10 File Offset: 0x0017BE10
		private IEnumerator HideCorutine(AccordionItem item)
		{
			yield return base.StartCoroutine(Animations.Collapse(item.ContentObjectRect, 0.5f));
			item.Open = false;
			item.ContentObject.SetActive(false);
			this.OnToggleItem.Invoke(item);
			yield break;
		}

		// Token: 0x04003021 RID: 12321
		public List<AccordionItem> Items = new List<AccordionItem>();

		// Token: 0x04003022 RID: 12322
		public AccordionEvent OnToggleItem = new AccordionEvent();

		// Token: 0x04003023 RID: 12323
		public bool OnlyOneOpen = true;

		// Token: 0x04003024 RID: 12324
		public bool Animate = true;
	}
}
