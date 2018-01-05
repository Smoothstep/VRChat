using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VRC.UI
{
	// Token: 0x02000C24 RID: 3108
	public class PageConsole : VRCUiPage
	{
		// Token: 0x06006030 RID: 24624 RVA: 0x0021DAAB File Offset: 0x0021BEAB
		public override void Awake()
		{
			base.Awake();
			this.consoleInput.onEndEdit.AddListener(new UnityAction<string>(this.OnSubmit));
		}

		// Token: 0x06006031 RID: 24625 RVA: 0x0021DAD0 File Offset: 0x0021BED0
		private void OnSubmit(string inputText)
		{
			Text text = this.consoleLog;
			text.text = text.text + inputText + "\n";
			string str = Console.ProcessConsoleCommand(inputText);
			Text text2 = this.consoleLog;
			text2.text = text2.text + str + "\n";
			this.consoleInput.text = string.Empty;
		}

		// Token: 0x06006032 RID: 24626 RVA: 0x0021DB2C File Offset: 0x0021BF2C
		public override void Update()
		{
			base.Update();
			if (Input.GetKeyDown(KeyCode.Return) && VRCUiManager.Instance.currentPopup == null)
			{
				this.consoleInput.PressEdit();
			}
		}

		// Token: 0x040045E8 RID: 17896
		public Text consoleLog;

		// Token: 0x040045E9 RID: 17897
		public UiInputField consoleInput;
	}
}
