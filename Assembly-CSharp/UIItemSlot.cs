using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000578 RID: 1400
public abstract class UIItemSlot : MonoBehaviour
{
	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x06002F88 RID: 12168
	protected abstract InvGameItem observedItem { get; }

	// Token: 0x06002F89 RID: 12169
	protected abstract InvGameItem Replace(InvGameItem item);

	// Token: 0x06002F8A RID: 12170 RVA: 0x000E7F8C File Offset: 0x000E638C
	private void OnTooltip(bool show)
	{
		InvGameItem invGameItem = (!show) ? null : this.mItem;
		if (invGameItem != null)
		{
			InvBaseItem baseItem = invGameItem.baseItem;
			if (baseItem != null)
			{
				string text = string.Concat(new string[]
				{
					"[",
					NGUIText.EncodeColor(invGameItem.color),
					"]",
					invGameItem.name,
					"[-]\n"
				});
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"[AFAFAF]Level ",
					invGameItem.itemLevel,
					" ",
					baseItem.slot
				});
				List<InvStat> list = invGameItem.CalculateStats();
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					InvStat invStat = list[i];
					if (invStat.amount != 0)
					{
						if (invStat.amount < 0)
						{
							text = text + "\n[FF0000]" + invStat.amount;
						}
						else
						{
							text = text + "\n[00FF00]+" + invStat.amount;
						}
						if (invStat.modifier == InvStat.Modifier.Percent)
						{
							text += "%";
						}
						text = text + " " + invStat.id;
						text += "[-]";
					}
					i++;
				}
				if (!string.IsNullOrEmpty(baseItem.description))
				{
					text = text + "\n[FF9900]" + baseItem.description;
				}
				UITooltip.Show(text);
				return;
			}
		}
		UITooltip.Hide();
	}

	// Token: 0x06002F8B RID: 12171 RVA: 0x000E812C File Offset: 0x000E652C
	private void OnClick()
	{
		if (UIItemSlot.mDraggedItem != null)
		{
			this.OnDrop(null);
		}
		else if (this.mItem != null)
		{
			UIItemSlot.mDraggedItem = this.Replace(null);
			if (UIItemSlot.mDraggedItem != null)
			{
				NGUITools.PlaySound(this.grabSound);
			}
			this.UpdateCursor();
		}
	}

	// Token: 0x06002F8C RID: 12172 RVA: 0x000E8182 File Offset: 0x000E6582
	private void OnDrag(Vector2 delta)
	{
		if (UIItemSlot.mDraggedItem == null && this.mItem != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UIItemSlot.mDraggedItem = this.Replace(null);
			NGUITools.PlaySound(this.grabSound);
			this.UpdateCursor();
		}
	}

	// Token: 0x06002F8D RID: 12173 RVA: 0x000E81C4 File Offset: 0x000E65C4
	private void OnDrop(GameObject go)
	{
		InvGameItem invGameItem = this.Replace(UIItemSlot.mDraggedItem);
		if (UIItemSlot.mDraggedItem == invGameItem)
		{
			NGUITools.PlaySound(this.errorSound);
		}
		else if (invGameItem != null)
		{
			NGUITools.PlaySound(this.grabSound);
		}
		else
		{
			NGUITools.PlaySound(this.placeSound);
		}
		UIItemSlot.mDraggedItem = invGameItem;
		this.UpdateCursor();
	}

	// Token: 0x06002F8E RID: 12174 RVA: 0x000E8228 File Offset: 0x000E6628
	private void UpdateCursor()
	{
		if (UIItemSlot.mDraggedItem != null && UIItemSlot.mDraggedItem.baseItem != null)
		{
			UICursor.Set(UIItemSlot.mDraggedItem.baseItem.iconAtlas, UIItemSlot.mDraggedItem.baseItem.iconName);
		}
		else
		{
			UICursor.Clear();
		}
	}

	// Token: 0x06002F8F RID: 12175 RVA: 0x000E827C File Offset: 0x000E667C
	private void Update()
	{
		InvGameItem observedItem = this.observedItem;
		if (this.mItem != observedItem)
		{
			this.mItem = observedItem;
			InvBaseItem invBaseItem = (observedItem == null) ? null : observedItem.baseItem;
			if (this.label != null)
			{
				string text = (observedItem == null) ? null : observedItem.name;
				if (string.IsNullOrEmpty(this.mText))
				{
					this.mText = this.label.text;
				}
				this.label.text = ((text == null) ? this.mText : text);
			}
			if (this.icon != null)
			{
				if (invBaseItem == null || invBaseItem.iconAtlas == null)
				{
					this.icon.enabled = false;
				}
				else
				{
					this.icon.atlas = invBaseItem.iconAtlas;
					this.icon.spriteName = invBaseItem.iconName;
					this.icon.enabled = true;
					this.icon.MakePixelPerfect();
				}
			}
			if (this.background != null)
			{
				this.background.color = ((observedItem == null) ? Color.white : observedItem.color);
			}
		}
	}

	// Token: 0x040019CE RID: 6606
	public UISprite icon;

	// Token: 0x040019CF RID: 6607
	public UIWidget background;

	// Token: 0x040019D0 RID: 6608
	public UILabel label;

	// Token: 0x040019D1 RID: 6609
	public AudioClip grabSound;

	// Token: 0x040019D2 RID: 6610
	public AudioClip placeSound;

	// Token: 0x040019D3 RID: 6611
	public AudioClip errorSound;

	// Token: 0x040019D4 RID: 6612
	private InvGameItem mItem;

	// Token: 0x040019D5 RID: 6613
	private string mText = string.Empty;

	// Token: 0x040019D6 RID: 6614
	private static InvGameItem mDraggedItem;
}
