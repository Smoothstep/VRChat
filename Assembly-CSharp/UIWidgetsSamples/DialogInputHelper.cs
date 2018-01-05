using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIWidgetsSamples
{
	// Token: 0x0200090E RID: 2318
	public class DialogInputHelper : MonoBehaviour
	{
		// Token: 0x060045CE RID: 17870 RVA: 0x0017A00D File Offset: 0x0017840D
		public void Refresh()
		{
			this.Username.text = string.Empty;
			this.Password.text = string.Empty;
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x0017A030 File Offset: 0x00178430
		public bool Validate()
		{
			bool flag = this.Username.text.Trim().Length > 0;
			bool flag2 = this.Password.text.Length > 0;
			if (!flag)
			{
				this.Username.Select();
			}
			else if (!flag2)
			{
				this.Password.Select();
			}
			return flag && flag2;
		}

		// Token: 0x04002FEE RID: 12270
		[SerializeField]
		public InputField Username;

		// Token: 0x04002FEF RID: 12271
		[SerializeField]
		public InputField Password;
	}
}
