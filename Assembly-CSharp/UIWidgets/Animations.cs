using System;
using System.Collections;
using EasyLayout;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x02000925 RID: 2341
	public static class Animations
	{
		// Token: 0x06004639 RID: 17977 RVA: 0x0017DCF0 File Offset: 0x0017C0F0
		public static IEnumerator Rotate(RectTransform rect, float time = 0.5f)
		{
			if (rect != null)
			{
				Vector3 start_rotarion = rect.rotation.eulerAngles;
				float end_time = Time.time + time;
				while (Time.time <= end_time)
				{
					float rotation_x = Mathf.Lerp(0f, 90f, 1f - (end_time - Time.time) / time);
					rect.rotation = Quaternion.Euler(rotation_x, start_rotarion.y, start_rotarion.z);
					yield return null;
				}
				rect.rotation = Quaternion.Euler(start_rotarion);
			}
			yield break;
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x0017DD14 File Offset: 0x0017C114
		public static IEnumerator Collapse(RectTransform rect, float time = 0.5f)
		{
			if (rect != null)
			{
				EasyLayout.EasyLayout layout = rect.GetComponentInParent<EasyLayout.EasyLayout>();
				float max_height = rect.rect.height;
				float end_time = Time.time + time;
				while (Time.time <= end_time)
				{
					float height = Mathf.Lerp(max_height, 0f, 1f - (end_time - Time.time) / time);
					rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
					if (layout != null)
					{
						layout.NeedUpdateLayout();
					}
					yield return null;
				}
				rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, max_height);
			}
			yield break;
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x0017DD38 File Offset: 0x0017C138
		public static IEnumerator Open(RectTransform rect, float time = 0.5f)
		{
			if (rect != null)
			{
				EasyLayout.EasyLayout layout = rect.GetComponentInParent<EasyLayout.EasyLayout>();
				float max_height = rect.rect.height;
				float end_time = Time.time + time;
				while (Time.time <= end_time)
				{
					float height = Mathf.Lerp(0f, max_height, 1f - (end_time - Time.time) / time);
					rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
					if (layout != null)
					{
						layout.NeedUpdateLayout();
					}
					yield return null;
				}
				rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, max_height);
			}
			yield break;
		}
	}
}
