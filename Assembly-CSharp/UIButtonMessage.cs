using System;
using UnityEngine;

// Token: 0x020005A0 RID: 1440
[AddComponentMenu("NGUI/Interaction/Button Message (Legacy)")]
public class UIButtonMessage : MonoBehaviour
{
	// Token: 0x06003027 RID: 12327 RVA: 0x000EC411 File Offset: 0x000EA811
	private void Start()
	{
		this.mStarted = true;
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x000EC41A File Offset: 0x000EA81A
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x000EC438 File Offset: 0x000EA838
	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOver) || (!isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOut)))
		{
			this.Send();
		}
	}

	// Token: 0x0600302A RID: 12330 RVA: 0x000EC46F File Offset: 0x000EA86F
	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && this.trigger == UIButtonMessage.Trigger.OnPress) || (!isPressed && this.trigger == UIButtonMessage.Trigger.OnRelease)))
		{
			this.Send();
		}
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x000EC4A6 File Offset: 0x000EA8A6
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x000EC4CB File Offset: 0x000EA8CB
	private void OnClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnClick)
		{
			this.Send();
		}
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x000EC4E9 File Offset: 0x000EA8E9
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnDoubleClick)
		{
			this.Send();
		}
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x000EC508 File Offset: 0x000EA908
	private void Send()
	{
		if (string.IsNullOrEmpty(this.functionName))
		{
			return;
		}
		if (this.target == null)
		{
			this.target = base.gameObject;
		}
		if (this.includeChildren)
		{
			Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				Transform transform = componentsInChildren[i];
				transform.gameObject.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
				i++;
			}
		}
		else
		{
			this.target.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x04001A98 RID: 6808
	public GameObject target;

	// Token: 0x04001A99 RID: 6809
	public string functionName;

	// Token: 0x04001A9A RID: 6810
	public UIButtonMessage.Trigger trigger;

	// Token: 0x04001A9B RID: 6811
	public bool includeChildren;

	// Token: 0x04001A9C RID: 6812
	private bool mStarted;

	// Token: 0x020005A1 RID: 1441
	public enum Trigger
	{
		// Token: 0x04001A9E RID: 6814
		OnClick,
		// Token: 0x04001A9F RID: 6815
		OnMouseOver,
		// Token: 0x04001AA0 RID: 6816
		OnMouseOut,
		// Token: 0x04001AA1 RID: 6817
		OnPress,
		// Token: 0x04001AA2 RID: 6818
		OnRelease,
		// Token: 0x04001AA3 RID: 6819
		OnDoubleClick
	}
}
