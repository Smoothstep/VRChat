using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000568 RID: 1384
[RequireComponent(typeof(Image))]
public class DragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	// Token: 0x06002F50 RID: 12112 RVA: 0x000E5E1C File Offset: 0x000E421C
	public void OnBeginDrag(PointerEventData eventData)
	{
		Canvas canvas = DragMe.FindInParents<Canvas>(base.gameObject);
		if (canvas == null)
		{
			return;
		}
		this.m_DraggingIcon = new GameObject("icon");
		this.m_DraggingIcon.transform.SetParent(canvas.transform, false);
		this.m_DraggingIcon.transform.SetAsLastSibling();
		Image image = this.m_DraggingIcon.AddComponent<Image>();
		this.m_DraggingIcon.AddComponent<IgnoreRaycast>();
		image.sprite = base.GetComponent<Image>().sprite;
		image.SetNativeSize();
		if (this.dragOnSurfaces)
		{
			this.m_DraggingPlane = (base.transform as RectTransform);
		}
		else
		{
			this.m_DraggingPlane = (canvas.transform as RectTransform);
		}
		this.SetDraggedPosition(eventData);
	}

	// Token: 0x06002F51 RID: 12113 RVA: 0x000E5EE1 File Offset: 0x000E42E1
	public void OnDrag(PointerEventData data)
	{
		if (this.m_DraggingIcon != null)
		{
			this.SetDraggedPosition(data);
		}
	}

	// Token: 0x06002F52 RID: 12114 RVA: 0x000E5EFC File Offset: 0x000E42FC
	private void SetDraggedPosition(PointerEventData data)
	{
		if (this.dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
		{
			this.m_DraggingPlane = (data.pointerEnter.transform as RectTransform);
		}
		RectTransform component = this.m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 position;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, data.position, data.pressEventCamera, out position))
		{
			component.position = position;
			component.rotation = this.m_DraggingPlane.rotation;
		}
	}

	// Token: 0x06002F53 RID: 12115 RVA: 0x000E5F98 File Offset: 0x000E4398
	public void OnEndDrag(PointerEventData eventData)
	{
		if (this.m_DraggingIcon != null)
		{
			UnityEngine.Object.Destroy(this.m_DraggingIcon);
		}
	}

	// Token: 0x06002F54 RID: 12116 RVA: 0x000E5FB8 File Offset: 0x000E43B8
	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)((object)null);
		}
		T component = go.GetComponent<T>();
		if (component != null)
		{
			return component;
		}
		Transform parent = go.transform.parent;
		while (parent != null && component == null)
		{
			component = parent.gameObject.GetComponent<T>();
			parent = parent.parent;
		}
		return component;
	}

	// Token: 0x04001991 RID: 6545
	public bool dragOnSurfaces = true;

	// Token: 0x04001992 RID: 6546
	private GameObject m_DraggingIcon;

	// Token: 0x04001993 RID: 6547
	private RectTransform m_DraggingPlane;
}
