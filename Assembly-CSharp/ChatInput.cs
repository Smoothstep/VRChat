using System;
using UnityEngine;

// Token: 0x02000585 RID: 1413
[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class ChatInput : MonoBehaviour
{
	// Token: 0x06002FBC RID: 12220 RVA: 0x000E9274 File Offset: 0x000E7674
	private void Start()
	{
		this.mInput = base.GetComponent<UIInput>();
		this.mInput.label.maxLineCount = 1;
		if (this.fillWithDummyData && this.textList != null)
		{
			for (int i = 0; i < 30; i++)
			{
				this.textList.Add(string.Concat(new object[]
				{
					(i % 2 != 0) ? "[AAAAAA]" : "[FFFFFF]",
					"This is an example paragraph for the text list, testing line ",
					i,
					"[-]"
				}));
			}
		}
	}

	// Token: 0x06002FBD RID: 12221 RVA: 0x000E9318 File Offset: 0x000E7718
	public void OnSubmit()
	{
		if (this.textList != null)
		{
			string text = NGUIText.StripSymbols(this.mInput.value);
			if (!string.IsNullOrEmpty(text))
			{
				this.textList.Add(text);
				this.mInput.value = string.Empty;
				this.mInput.isSelected = false;
			}
		}
	}

	// Token: 0x04001A24 RID: 6692
	public UITextList textList;

	// Token: 0x04001A25 RID: 6693
	public bool fillWithDummyData;

	// Token: 0x04001A26 RID: 6694
	private UIInput mInput;
}
