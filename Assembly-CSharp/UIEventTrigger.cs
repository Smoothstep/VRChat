using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005B2 RID: 1458
[AddComponentMenu("NGUI/Interaction/Event Trigger")]
public class UIEventTrigger : MonoBehaviour
{
	// Token: 0x06003090 RID: 12432 RVA: 0x000EE6CE File Offset: 0x000ECACE
	private void OnHover(bool isOver)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (isOver)
		{
			EventDelegate.Execute(this.onHoverOver);
		}
		else
		{
			EventDelegate.Execute(this.onHoverOut);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x000EE70E File Offset: 0x000ECB0E
	private void OnPress(bool pressed)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (pressed)
		{
			EventDelegate.Execute(this.onPress);
		}
		else
		{
			EventDelegate.Execute(this.onRelease);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x000EE74E File Offset: 0x000ECB4E
	private void OnSelect(bool selected)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (selected)
		{
			EventDelegate.Execute(this.onSelect);
		}
		else
		{
			EventDelegate.Execute(this.onDeselect);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x000EE78E File Offset: 0x000ECB8E
	private void OnClick()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onClick);
		UIEventTrigger.current = null;
	}

	// Token: 0x06003094 RID: 12436 RVA: 0x000EE7B8 File Offset: 0x000ECBB8
	private void OnDoubleClick()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDoubleClick);
		UIEventTrigger.current = null;
	}

	// Token: 0x06003095 RID: 12437 RVA: 0x000EE7E2 File Offset: 0x000ECBE2
	private void OnDragStart()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragStart);
		UIEventTrigger.current = null;
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x000EE80C File Offset: 0x000ECC0C
	private void OnDragEnd()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragEnd);
		UIEventTrigger.current = null;
	}

	// Token: 0x06003097 RID: 12439 RVA: 0x000EE836 File Offset: 0x000ECC36
	private void OnDragOver(GameObject go)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOver);
		UIEventTrigger.current = null;
	}

	// Token: 0x06003098 RID: 12440 RVA: 0x000EE860 File Offset: 0x000ECC60
	private void OnDragOut(GameObject go)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOut);
		UIEventTrigger.current = null;
	}

	// Token: 0x06003099 RID: 12441 RVA: 0x000EE88A File Offset: 0x000ECC8A
	private void OnDrag(Vector2 delta)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOut);
		UIEventTrigger.current = null;
	}

	// Token: 0x04001B0A RID: 6922
	public static UIEventTrigger current;

	// Token: 0x04001B0B RID: 6923
	public List<EventDelegate> onHoverOver = new List<EventDelegate>();

	// Token: 0x04001B0C RID: 6924
	public List<EventDelegate> onHoverOut = new List<EventDelegate>();

	// Token: 0x04001B0D RID: 6925
	public List<EventDelegate> onPress = new List<EventDelegate>();

	// Token: 0x04001B0E RID: 6926
	public List<EventDelegate> onRelease = new List<EventDelegate>();

	// Token: 0x04001B0F RID: 6927
	public List<EventDelegate> onSelect = new List<EventDelegate>();

	// Token: 0x04001B10 RID: 6928
	public List<EventDelegate> onDeselect = new List<EventDelegate>();

	// Token: 0x04001B11 RID: 6929
	public List<EventDelegate> onClick = new List<EventDelegate>();

	// Token: 0x04001B12 RID: 6930
	public List<EventDelegate> onDoubleClick = new List<EventDelegate>();

	// Token: 0x04001B13 RID: 6931
	public List<EventDelegate> onDragStart = new List<EventDelegate>();

	// Token: 0x04001B14 RID: 6932
	public List<EventDelegate> onDragEnd = new List<EventDelegate>();

	// Token: 0x04001B15 RID: 6933
	public List<EventDelegate> onDragOver = new List<EventDelegate>();

	// Token: 0x04001B16 RID: 6934
	public List<EventDelegate> onDragOut = new List<EventDelegate>();

	// Token: 0x04001B17 RID: 6935
	public List<EventDelegate> onDrag = new List<EventDelegate>();
}
