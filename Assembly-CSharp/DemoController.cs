using System;
using HeathenEngineering.OSK.v2;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000703 RID: 1795
public class DemoController : MonoBehaviour
{
	// Token: 0x06003AD0 RID: 15056 RVA: 0x00128EF4 File Offset: 0x001272F4
	private void Start()
	{
		if (this.keyboard != null)
		{
			this.keyboard.KeyPressed += this.keyboardKeyPressed;
		}
	}

	// Token: 0x06003AD1 RID: 15057 RVA: 0x00128F1E File Offset: 0x0012731E
	public void SaveName()
	{
	}

	// Token: 0x06003AD2 RID: 15058 RVA: 0x00128F20 File Offset: 0x00127320
	private void keyboardKeyPressed(OnScreenKeyboard sender, OnScreenKeyboardArguments args)
	{
		switch (args.KeyPressed.type)
		{
		case KeyClass.String:
		{
			Text text = this.outputText;
			text.text += args.KeyPressed.ToString();
			break;
		}
		case KeyClass.Return:
		{
			Text text2 = this.outputText;
			text2.text += args.KeyPressed.ToString();
			break;
		}
		case KeyClass.Backspace:
			if (this.outputText.text.Length > 0)
			{
				this.outputText.text = this.outputText.text.Substring(0, this.outputText.text.Length - 1);
			}
			break;
		}
	}

	// Token: 0x04002391 RID: 9105
	public Text outputText;

	// Token: 0x04002392 RID: 9106
	public OnScreenKeyboard keyboard;
}
