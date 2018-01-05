using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x020007B2 RID: 1970
	public class AuthenticationValues
	{
		// Token: 0x06003FAB RID: 16299 RVA: 0x0014023D File Offset: 0x0013E63D
		public AuthenticationValues()
		{
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x00140250 File Offset: 0x0013E650
		public AuthenticationValues(string userId)
		{
			this.UserId = userId;
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06003FAD RID: 16301 RVA: 0x0014026A File Offset: 0x0013E66A
		// (set) Token: 0x06003FAE RID: 16302 RVA: 0x00140272 File Offset: 0x0013E672
		public CustomAuthenticationType AuthType
		{
			get
			{
				return this.authType;
			}
			set
			{
				this.authType = value;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06003FAF RID: 16303 RVA: 0x0014027B File Offset: 0x0013E67B
		// (set) Token: 0x06003FB0 RID: 16304 RVA: 0x00140283 File Offset: 0x0013E683
		public string AuthGetParameters { get; set; }

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06003FB1 RID: 16305 RVA: 0x0014028C File Offset: 0x0013E68C
		// (set) Token: 0x06003FB2 RID: 16306 RVA: 0x00140294 File Offset: 0x0013E694
		public object AuthPostData { get; private set; }

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06003FB3 RID: 16307 RVA: 0x0014029D File Offset: 0x0013E69D
		// (set) Token: 0x06003FB4 RID: 16308 RVA: 0x001402A5 File Offset: 0x0013E6A5
		public string Token { get; set; }

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06003FB5 RID: 16309 RVA: 0x001402AE File Offset: 0x0013E6AE
		// (set) Token: 0x06003FB6 RID: 16310 RVA: 0x001402B6 File Offset: 0x0013E6B6
		public string UserId { get; set; }

		// Token: 0x06003FB7 RID: 16311 RVA: 0x001402BF File Offset: 0x0013E6BF
		public virtual void SetAuthPostData(string stringData)
		{
			this.AuthPostData = ((!string.IsNullOrEmpty(stringData)) ? stringData : null);
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x001402D9 File Offset: 0x0013E6D9
		public virtual void SetAuthPostData(byte[] byteData)
		{
			this.AuthPostData = byteData;
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x001402E4 File Offset: 0x0013E6E4
		public virtual void AddAuthParameter(string key, string value)
		{
			string text = (!string.IsNullOrEmpty(this.AuthGetParameters)) ? "&" : string.Empty;
			this.AuthGetParameters = string.Format("{0}{1}{2}={3}", new object[]
			{
				this.AuthGetParameters,
				text,
				Uri.EscapeDataString(key),
				Uri.EscapeDataString(value)
			});
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x00140346 File Offset: 0x0013E746
		public override string ToString()
		{
			return string.Format("AuthenticationValues UserId: {0}, GetParameters: {1} Token available: {2}", this.UserId, this.AuthGetParameters, this.Token != null);
		}

		// Token: 0x040027FF RID: 10239
		private CustomAuthenticationType authType = CustomAuthenticationType.None;
	}
}
