using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C37 RID: 3127
public class ScrollRectEx : ScrollRect
{
	// Token: 0x06006127 RID: 24871 RVA: 0x00224378 File Offset: 0x00222778
	private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
	{
		Transform parent = base.transform.parent;
		while (parent != null)
		{
			foreach (Component component in parent.GetComponents<Component>())
			{
				if (component is T)
				{
					action((T)((object)((IEventSystemHandler)component)));
				}
			}
			parent = parent.parent;
		}
	}

	// Token: 0x06006128 RID: 24872 RVA: 0x002243E4 File Offset: 0x002227E4
	public override void OnInitializePotentialDrag(PointerEventData eventData)
	{
		this.DoForParents<IInitializePotentialDragHandler>(delegate(IInitializePotentialDragHandler parent)
		{
			parent.OnInitializePotentialDrag(eventData);
		});
		base.OnInitializePotentialDrag(eventData);
	}

	// Token: 0x06006129 RID: 24873 RVA: 0x0022441C File Offset: 0x0022281C
	public override void OnDrag(PointerEventData eventData)
	{
		if (this.routeToParent)
		{
			this.DoForParents<IDragHandler>(delegate(IDragHandler parent)
			{
				parent.OnDrag(eventData);
			});
		}
		else
		{
			base.OnDrag(eventData);
		}
	}

	// Token: 0x0600612A RID: 24874 RVA: 0x00224464 File Offset: 0x00222864
	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (!base.horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
		{
			this.routeToParent = true;
		}
		else if (!base.vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
		{
			this.routeToParent = true;
		}
		else
		{
			this.routeToParent = false;
		}
		if (this.routeToParent)
		{
			this.DoForParents<IBeginDragHandler>(delegate(IBeginDragHandler parent)
			{
				parent.OnBeginDrag(eventData);
			});
		}
		else
		{
			base.OnBeginDrag(eventData);
		}
	}

	// Token: 0x0600612B RID: 24875 RVA: 0x0022454C File Offset: 0x0022294C
	public override void OnScroll(PointerEventData eventData)
	{
		Vector2 scrollDelta = eventData.scrollDelta;
		if (scrollDelta.x != 0f)
		{
			eventData.scrollDelta = new Vector2(scrollDelta.x, 0f);
			if (base.horizontal)
			{
				base.OnScroll(eventData);
			}
			else
			{
				this.DoForParents<IScrollHandler>(delegate(IScrollHandler parent)
				{
					parent.OnScroll(eventData);
				});
			}
		}
		if (scrollDelta.y != 0f)
		{
			eventData.scrollDelta = new Vector2(0f, scrollDelta.y);
			if (base.vertical)
			{
				base.OnScroll(eventData);
			}
			else
			{
				this.DoForParents<IScrollHandler>(delegate(IScrollHandler parent)
				{
					parent.OnScroll(eventData);
				});
			}
		}
		eventData.scrollDelta = scrollDelta;
	}

	// Token: 0x0600612C RID: 24876 RVA: 0x00224634 File Offset: 0x00222A34
	public override void OnEndDrag(PointerEventData eventData)
	{
		if (this.routeToParent)
		{
			this.DoForParents<IEndDragHandler>(delegate(IEndDragHandler parent)
			{
				parent.OnEndDrag(eventData);
			});
		}
		else
		{
			base.OnEndDrag(eventData);
		}
		this.routeToParent = false;
	}

	// Token: 0x040046CC RID: 18124
	private bool routeToParent;
}
