using System;

namespace HeathenEngineering.OSK.v2
{
	// Token: 0x02000706 RID: 1798
	public class OnScreenKeyboardArguments
	{
		// Token: 0x06003AD7 RID: 15063 RVA: 0x00128FEE File Offset: 0x001273EE
		public OnScreenKeyboardArguments(OnScreenKeyboardKey KeyPressed)
		{
			this.KeyPressed = KeyPressed;
		}

		// Token: 0x04002398 RID: 9112
		public OnScreenKeyboardKey KeyPressed;
	}
}
