using System;

// Token: 0x0200074D RID: 1869
public class AuthenticationValues
{
	// Token: 0x06003BF8 RID: 15352 RVA: 0x0012D538 File Offset: 0x0012B938
	public AuthenticationValues()
	{
	}

	// Token: 0x06003BF9 RID: 15353 RVA: 0x0012D54B File Offset: 0x0012B94B
	public AuthenticationValues(string userId)
	{
		this.UserId = userId;
	}

	// Token: 0x17000968 RID: 2408
	// (get) Token: 0x06003BFA RID: 15354 RVA: 0x0012D565 File Offset: 0x0012B965
	// (set) Token: 0x06003BFB RID: 15355 RVA: 0x0012D56D File Offset: 0x0012B96D
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

	// Token: 0x17000969 RID: 2409
	// (get) Token: 0x06003BFC RID: 15356 RVA: 0x0012D576 File Offset: 0x0012B976
	// (set) Token: 0x06003BFD RID: 15357 RVA: 0x0012D57E File Offset: 0x0012B97E
	public string AuthGetParameters { get; set; }

	// Token: 0x1700096A RID: 2410
	// (get) Token: 0x06003BFE RID: 15358 RVA: 0x0012D587 File Offset: 0x0012B987
	// (set) Token: 0x06003BFF RID: 15359 RVA: 0x0012D58F File Offset: 0x0012B98F
	public object AuthPostData { get; private set; }

	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x06003C00 RID: 15360 RVA: 0x0012D598 File Offset: 0x0012B998
	// (set) Token: 0x06003C01 RID: 15361 RVA: 0x0012D5A0 File Offset: 0x0012B9A0
	public string Token { get; set; }

	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x06003C02 RID: 15362 RVA: 0x0012D5A9 File Offset: 0x0012B9A9
	// (set) Token: 0x06003C03 RID: 15363 RVA: 0x0012D5B1 File Offset: 0x0012B9B1
	public string UserId { get; set; }

	// Token: 0x06003C04 RID: 15364 RVA: 0x0012D5BA File Offset: 0x0012B9BA
	public virtual void SetAuthPostData(string stringData)
	{
		this.AuthPostData = ((!string.IsNullOrEmpty(stringData)) ? stringData : null);
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x0012D5D4 File Offset: 0x0012B9D4
	public virtual void SetAuthPostData(byte[] byteData)
	{
		this.AuthPostData = byteData;
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x0012D5E0 File Offset: 0x0012B9E0
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

	// Token: 0x06003C07 RID: 15367 RVA: 0x0012D642 File Offset: 0x0012BA42
	public override string ToString()
	{
		return string.Format("AuthenticationValues UserId: {0}, GetParameters: {1} Token available: {2}", this.UserId, this.AuthGetParameters, this.Token != null);
	}

	// Token: 0x040025A5 RID: 9637
	private CustomAuthenticationType authType = CustomAuthenticationType.None;
}
