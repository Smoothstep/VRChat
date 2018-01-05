using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace VRC.UI
{
	// Token: 0x02000C53 RID: 3155
	[RequireComponent(typeof(Text))]
	public class UsernameDisplay : MonoBehaviour
	{
		// Token: 0x060061D5 RID: 25045 RVA: 0x002285D4 File Offset: 0x002269D4
		private void Start()
		{
			this.userNameText = base.GetComponent<Text>();
			this.userNameText.text = string.Empty;
		}

		// Token: 0x060061D6 RID: 25046 RVA: 0x002285F4 File Offset: 0x002269F4
		private void Update()
		{
			if (APIUser.IsLoggedIn && this.lastDisplayName != User.CurrentUser.displayName)
			{
				this.lastDisplayName = User.CurrentUser.displayName;
				this.userNameText.text = "Hi, " + User.CurrentUser.displayName;
			}
			if (!string.IsNullOrEmpty(this.userNameText.text) && !APIUser.IsLoggedIn)
			{
				this.userNameText.text = string.Empty;
				this.lastDisplayName = string.Empty;
			}
		}

		// Token: 0x04004775 RID: 18293
		private Text userNameText;

		// Token: 0x04004776 RID: 18294
		private string lastDisplayName;
	}
}
