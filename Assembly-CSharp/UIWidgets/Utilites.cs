using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x0200096E RID: 2414
	public static class Utilites
	{
		// Token: 0x06004945 RID: 18757 RVA: 0x001877E1 File Offset: 0x00185BE1
		public static void FixInstantiated(Component source, Component instance)
		{
			Utilites.FixInstantiated(source.gameObject, instance.gameObject);
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x001877F4 File Offset: 0x00185BF4
		public static void FixInstantiated(GameObject source, GameObject instance)
		{
			RectTransform component = source.GetComponent<RectTransform>();
			RectTransform component2 = instance.GetComponent<RectTransform>();
			component2.localPosition = component.localPosition;
			component2.position = component.position;
			component2.rotation = component.rotation;
			component2.localScale = component.localScale;
			component2.anchoredPosition = component.anchoredPosition;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x0018784C File Offset: 0x00185C4C
		public static Transform FindCanvas(Transform currentObject)
		{
			Canvas componentInParent = currentObject.GetComponentInParent<Canvas>();
			if (componentInParent == null)
			{
				return null;
			}
			return componentInParent.transform;
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x00187874 File Offset: 0x00185C74
		public static Vector3 CalculateDragPosition(Vector3 screenPosition, Canvas canvas, RectTransform canvasRect)
		{
			Vector2 sizeDelta = canvasRect.sizeDelta;
			Vector2 b = Vector2.zero;
			Vector2 a = sizeDelta;
			bool flag = canvas.renderMode == RenderMode.ScreenSpaceOverlay;
			bool flag2 = canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null;
			Vector3 result;
			if (flag || flag2)
			{
				result = screenPosition;
			}
			else
			{
				Ray ray = canvas.worldCamera.ScreenPointToRay(screenPosition);
				Plane plane = new Plane(canvasRect.forward, canvasRect.position);
				float d;
				plane.Raycast(ray, out d);
				result = canvasRect.InverseTransformPoint(ray.origin + ray.direction * d);
				b = -Vector2.Scale(a, canvasRect.pivot);
				a = sizeDelta - b;
			}
			result.x = Mathf.Clamp(result.x, b.x, a.y);
			result.y = Mathf.Clamp(result.y, b.x, a.y);
			return result;
		}
	}
}
