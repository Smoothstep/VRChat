using System;
using UnityEngine;

// Token: 0x02000A5D RID: 2653
public class ComboBox
{
	// Token: 0x06005051 RID: 20561 RVA: 0x001B7AD6 File Offset: 0x001B5ED6
	public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle)
	{
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		this.buttonStyle = "button";
		this.boxStyle = "box";
		this.listStyle = listStyle;
	}

	// Token: 0x06005052 RID: 20562 RVA: 0x001B7B11 File Offset: 0x001B5F11
	public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, string buttonStyle, string boxStyle, GUIStyle listStyle)
	{
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		this.buttonStyle = buttonStyle;
		this.boxStyle = boxStyle;
		this.listStyle = listStyle;
	}

	// Token: 0x06005053 RID: 20563 RVA: 0x001B7B46 File Offset: 0x001B5F46
	public void SetRect(Rect pRect)
	{
		this.rect = pRect;
	}

	// Token: 0x06005054 RID: 20564 RVA: 0x001B7B50 File Offset: 0x001B5F50
	public int Show()
	{
		if (ComboBox.forceToUnShow)
		{
			ComboBox.forceToUnShow = false;
			this.isClickedComboButton = false;
		}
		bool flag = false;
		int controlID = GUIUtility.GetControlID(FocusType.Passive);
		EventType typeForControl = Event.current.GetTypeForControl(controlID);
		if (typeForControl == EventType.MouseUp)
		{
			if (this.isClickedComboButton)
			{
				flag = true;
			}
		}
		if (GUI.Button(this.rect, this.buttonContent, this.buttonStyle))
		{
			if (ComboBox.useControlID == -1)
			{
				ComboBox.useControlID = controlID;
				this.isClickedComboButton = false;
			}
			if (ComboBox.useControlID != controlID)
			{
				ComboBox.forceToUnShow = true;
				ComboBox.useControlID = controlID;
			}
			this.isClickedComboButton = true;
		}
		if (this.isClickedComboButton)
		{
			Rect position = new Rect(this.rect.x, this.rect.y + this.listStyle.CalcHeight(this.listContent[0], 1f), this.rect.width, this.listStyle.CalcHeight(this.listContent[0], 1f) * (float)this.listContent.Length);
			GUI.Box(position, string.Empty, this.boxStyle);
			int num = GUI.SelectionGrid(position, this.selectedItemIndex, this.listContent, 1, this.listStyle);
			if (num != this.selectedItemIndex)
			{
				this.selectedItemIndex = num;
				this.buttonContent = this.listContent[this.selectedItemIndex];
			}
		}
		if (flag)
		{
			this.isClickedComboButton = false;
		}
		return this.selectedItemIndex;
	}

	// Token: 0x17000BEC RID: 3052
	// (get) Token: 0x06005055 RID: 20565 RVA: 0x001B7CD8 File Offset: 0x001B60D8
	// (set) Token: 0x06005056 RID: 20566 RVA: 0x001B7CE0 File Offset: 0x001B60E0
	public int SelectedItemIndex
	{
		get
		{
			return this.selectedItemIndex;
		}
		set
		{
			this.selectedItemIndex = value;
		}
	}

	// Token: 0x0400391A RID: 14618
	private static bool forceToUnShow;

	// Token: 0x0400391B RID: 14619
	private static int useControlID = -1;

	// Token: 0x0400391C RID: 14620
	private bool isClickedComboButton;

	// Token: 0x0400391D RID: 14621
	private int selectedItemIndex;

	// Token: 0x0400391E RID: 14622
	private Rect rect;

	// Token: 0x0400391F RID: 14623
	private GUIContent buttonContent;

	// Token: 0x04003920 RID: 14624
	private GUIContent[] listContent;

	// Token: 0x04003921 RID: 14625
	private string buttonStyle;

	// Token: 0x04003922 RID: 14626
	private string boxStyle;

	// Token: 0x04003923 RID: 14627
	private GUIStyle listStyle;
}
