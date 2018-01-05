using System;
using System.Collections;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x02000963 RID: 2403
	[RequireComponent(typeof(RectTransform))]
	public class SlideUp : MonoBehaviour
	{
		// Token: 0x060048DB RID: 18651 RVA: 0x00185B7E File Offset: 0x00183F7E
		private void Awake()
		{
			this.rect = base.GetComponent<RectTransform>();
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x00185B8C File Offset: 0x00183F8C
		public void Run()
		{
			base.StartCoroutine(this.StartAnimation());
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x00185B9C File Offset: 0x00183F9C
		private IEnumerator StartAnimation()
		{
			yield return base.StartCoroutine(this.AnimationCollapse());
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x00185BB7 File Offset: 0x00183FB7
		private void OnDisable()
		{
			Notify.FreeSlide(this.rect);
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x00185BC4 File Offset: 0x00183FC4
		private IEnumerator AnimationCollapse()
		{
			float max_height = this.rect.rect.height;
			float speed = 200f;
			float time = max_height / speed;
			float end_time = Time.time + time;
			while (Time.time <= end_time)
			{
				float height = Mathf.Lerp(max_height, 0f, 1f - (end_time - Time.time) / time);
				this.rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
				yield return null;
			}
			this.rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, max_height);
			yield break;
		}

		// Token: 0x04003170 RID: 12656
		private RectTransform rect;
	}
}
