using System;

namespace TMPro
{
	// Token: 0x020008F0 RID: 2288
	[Serializable]
	public class TMP_DigitValidator : TMP_InputValidator
	{
		// Token: 0x06004554 RID: 17748 RVA: 0x0017429D File Offset: 0x0017269D
		public override char Validate(ref string text, ref int pos, char ch)
		{
			if (ch >= '0' && ch <= '9')
			{
				pos++;
				return ch;
			}
			return '\0';
		}
	}
}
