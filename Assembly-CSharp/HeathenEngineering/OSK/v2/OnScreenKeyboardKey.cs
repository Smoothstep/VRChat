using System;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.OSK.v2
{
	// Token: 0x02000708 RID: 1800
	[AddComponentMenu("Heathen/OSK/v2/On Screen Keyboard Key (v2.x)")]
	[RequireComponent(typeof(Button))]
	public class OnScreenKeyboardKey : MonoBehaviour
	{
		// Token: 0x06003AE0 RID: 15072 RVA: 0x001292C2 File Offset: 0x001276C2
		private void Start()
		{
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x001292C4 File Offset: 0x001276C4
		private void Update()
		{
			if (!this.IsCommandOrControlPressed())
			{
				if (this.type == KeyClass.Shift)
				{
					if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
					{
						this.PressKey();
					}
				}
				else if (this.type == KeyClass.String)
				{
					if (Input.GetKeyDown(this.keyCode))
					{
						this.PressKey();
					}
					else if (Input.GetKey(this.keyCode))
					{
						this.PressKeyIfGreaterThanTimer();
					}
					else if (Input.GetKeyUp(this.keyCode))
					{
						this.keyRepeatRateTimer = 0f;
						this.keyRepeatDelayTimer = 0f;
					}
				}
				else if (this.type == KeyClass.Backspace)
				{
					if (Input.GetKeyDown(this.keyCode))
					{
						this.PressKey();
					}
					else if (Input.GetKey(this.keyCode))
					{
						this.PressKeyIfGreaterThanTimer();
					}
					else if (Input.GetKeyUp(this.keyCode))
					{
						this.keyRepeatRateTimer = 0f;
						this.keyRepeatDelayTimer = 0f;
					}
				}
			}
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x00129404 File Offset: 0x00127804
		private void PressKeyIfGreaterThanTimer()
		{
			this.keyRepeatDelayTimer += Time.deltaTime;
			if (this.keyRepeatDelayTimer > 0.5f)
			{
				this.keyRepeatRateTimer += Time.deltaTime;
				if (this.keyRepeatRateTimer > 0.05f)
				{
					this.PressKey();
					this.keyRepeatRateTimer = 0f;
				}
			}
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x00129468 File Offset: 0x00127868
		public override string ToString()
		{
			if (!(this.Keyboard != null))
			{
				Debug.LogError("To String was called on an OnScreenKeyboardKey that has no valid keyboard", this);
				return this.LowerCaseValue;
			}
			if (this.type == KeyClass.Return)
			{
				return "\n";
			}
			return (!this.Keyboard.IsLowerCase) ? this.UpperCaseValue : this.LowerCaseValue;
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x001294CB File Offset: 0x001278CB
		public void PressKey()
		{
			if (this.Keyboard != null)
			{
				this.Keyboard.ActiveKey = this;
				this.Keyboard.ActivateKey();
			}
			else
			{
				Debug.LogError("An OnScreenKeyboardKey was pressed but does not have an owning keyboard; insure the key is a child of an OnScreenKeyboard", this);
			}
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x00129508 File Offset: 0x00127908
		public void SetCase(bool ToUpper)
		{
			if (this.type == KeyClass.String && string.IsNullOrEmpty(this.UpperCaseValue.Trim()))
			{
				this.Text.text = "_";
			}
			else if (ToUpper)
			{
				this.Text.text = this.UpperCaseValue;
			}
			else
			{
				this.Text.text = this.LowerCaseValue;
			}
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x00129578 File Offset: 0x00127978
		public bool IsCommandOrControlPressed()
		{
			bool result = false;
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0400239E RID: 9118
		[HideInInspector]
		public OnScreenKeyboard Keyboard;

		// Token: 0x0400239F RID: 9119
		public KeyClass type;

		// Token: 0x040023A0 RID: 9120
		public KeyCode keyCode = KeyCode.A;

		// Token: 0x040023A1 RID: 9121
		public string LowerCaseValue = "a";

		// Token: 0x040023A2 RID: 9122
		public string UpperCaseValue = "A";

		// Token: 0x040023A3 RID: 9123
		public Text Text;

		// Token: 0x040023A4 RID: 9124
		private const float KEY_REPEAT_RATE = 0.05f;

		// Token: 0x040023A5 RID: 9125
		private const float KEY_REPEAT_DELAY = 0.5f;

		// Token: 0x040023A6 RID: 9126
		private float keyRepeatRateTimer;

		// Token: 0x040023A7 RID: 9127
		private float keyRepeatDelayTimer;
	}
}
