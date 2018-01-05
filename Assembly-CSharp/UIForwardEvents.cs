using System;
using UnityEngine;

// Token: 0x020005B3 RID: 1459
[AddComponentMenu("NGUI/Interaction/Forward Events (Legacy)")]
public class UIForwardEvents : MonoBehaviour
{
	// Token: 0x0600309B RID: 12443 RVA: 0x000EE8BC File Offset: 0x000ECCBC
	private void OnHover(bool isOver)
	{
		if (this.onHover && this.target != null)
		{
			this.target.SendMessage("OnHover", isOver, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600309C RID: 12444 RVA: 0x000EE8F1 File Offset: 0x000ECCF1
	private void OnPress(bool pressed)
	{
		if (this.onPress && this.target != null)
		{
			this.target.SendMessage("OnPress", pressed, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600309D RID: 12445 RVA: 0x000EE926 File Offset: 0x000ECD26
	private void OnClick()
	{
		if (this.onClick && this.target != null)
		{
			this.target.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600309E RID: 12446 RVA: 0x000EE955 File Offset: 0x000ECD55
	private void OnDoubleClick()
	{
		if (this.onDoubleClick && this.target != null)
		{
			this.target.SendMessage("OnDoubleClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x000EE984 File Offset: 0x000ECD84
	private void OnSelect(bool selected)
	{
		if (this.onSelect && this.target != null)
		{
			this.target.SendMessage("OnSelect", selected, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x000EE9B9 File Offset: 0x000ECDB9
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag && this.target != null)
		{
			this.target.SendMessage("OnDrag", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x000EE9EE File Offset: 0x000ECDEE
	private void OnDrop(GameObject go)
	{
		if (this.onDrop && this.target != null)
		{
			this.target.SendMessage("OnDrop", go, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x000EEA1E File Offset: 0x000ECE1E
	private void OnSubmit()
	{
		if (this.onSubmit && this.target != null)
		{
			this.target.SendMessage("OnSubmit", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x000EEA4D File Offset: 0x000ECE4D
	private void OnScroll(float delta)
	{
		if (this.onScroll && this.target != null)
		{
			this.target.SendMessage("OnScroll", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x04001B18 RID: 6936
	public GameObject target;

	// Token: 0x04001B19 RID: 6937
	public bool onHover;

	// Token: 0x04001B1A RID: 6938
	public bool onPress;

	// Token: 0x04001B1B RID: 6939
	public bool onClick;

	// Token: 0x04001B1C RID: 6940
	public bool onDoubleClick;

	// Token: 0x04001B1D RID: 6941
	public bool onSelect;

	// Token: 0x04001B1E RID: 6942
	public bool onDrag;

	// Token: 0x04001B1F RID: 6943
	public bool onDrop;

	// Token: 0x04001B20 RID: 6944
	public bool onSubmit;

	// Token: 0x04001B21 RID: 6945
	public bool onScroll;
}
