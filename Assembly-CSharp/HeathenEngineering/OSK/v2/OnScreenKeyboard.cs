using System;
using System.Collections.Generic;
using UnityEngine;

namespace HeathenEngineering.OSK.v2
{
	// Token: 0x02000707 RID: 1799
	[AddComponentMenu("Heathen/OSK/v2/On Screen Keyboard (v2.x)")]
	public class OnScreenKeyboard : MonoBehaviour
	{
		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06003AD9 RID: 15065 RVA: 0x0012900C File Offset: 0x0012740C
		// (remove) Token: 0x06003ADA RID: 15066 RVA: 0x00129044 File Offset: 0x00127444
		public event KeyboardEventHandler KeyPressed;

		// Token: 0x06003ADB RID: 15067 RVA: 0x0012907C File Offset: 0x0012747C
		private void Start()
		{
			try
			{
				this.UpdateStructure();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x001290C0 File Offset: 0x001274C0
		public void UpdateStructure()
		{
			this.Keys = new List<OnScreenKeyboardKey>(base.gameObject.GetComponentsInChildren<OnScreenKeyboardKey>());
			if (this.Keys.Count < 1)
			{
				Debug.LogWarning("Heathen On Screen Keyboard was unable to locate an OnScreeKeboardKey component in any of its children.\nPlease add at least 1 key or indicate a key on the OnScreenKeyboard behaviour by setting the ActiveKey value.", this);
			}
			if (this.Keys.Count > 0 && (this.ActiveKey == null || !this.Keys.Contains(this.ActiveKey)))
			{
				this.ActiveKey = this.Keys[0];
			}
			foreach (OnScreenKeyboardKey onScreenKeyboardKey in this.Keys)
			{
				onScreenKeyboardKey.Keyboard = this;
			}
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x00129198 File Offset: 0x00127598
		public void SetCase(bool ToUpper)
		{
			this.IsLowerCase = !ToUpper;
			foreach (OnScreenKeyboardKey onScreenKeyboardKey in this.Keys)
			{
				onScreenKeyboardKey.SetCase(ToUpper);
			}
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x00129200 File Offset: 0x00127600
		public string ActivateKey()
		{
			if (this.KeyPressed != null)
			{
				this.KeyPressed(this, new OnScreenKeyboardArguments(this.ActiveKey));
			}
			switch (this.ActiveKey.type)
			{
			case KeyClass.String:
				if (this.IsLowerCase)
				{
					return this.ActiveKey.LowerCaseValue;
				}
				return this.ActiveKey.UpperCaseValue;
			case KeyClass.Shift:
				this.SetCase(this.IsLowerCase);
				return string.Empty;
			case KeyClass.Return:
				return "\n";
			case KeyClass.Backspace:
				return string.Empty;
			default:
				return string.Empty;
			}
		}

		// Token: 0x04002399 RID: 9113
		public OnScreenKeyboardKey KeyTemplate;

		// Token: 0x0400239A RID: 9114
		public OnScreenKeyboardKey ActiveKey;

		// Token: 0x0400239C RID: 9116
		public bool IsLowerCase = true;

		// Token: 0x0400239D RID: 9117
		public List<OnScreenKeyboardKey> Keys;
	}
}
