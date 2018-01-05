using System;

// Token: 0x02000749 RID: 1865
public class TypedLobby
{
	// Token: 0x06003BF1 RID: 15345 RVA: 0x0012D46A File Offset: 0x0012B86A
	public TypedLobby()
	{
		this.Name = string.Empty;
		this.Type = LobbyType.Default;
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x0012D484 File Offset: 0x0012B884
	public TypedLobby(string name, LobbyType type)
	{
		this.Name = name;
		this.Type = type;
	}

	// Token: 0x17000967 RID: 2407
	// (get) Token: 0x06003BF3 RID: 15347 RVA: 0x0012D49A File Offset: 0x0012B89A
	public bool IsDefault
	{
		get
		{
			return this.Type == LobbyType.Default && string.IsNullOrEmpty(this.Name);
		}
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x0012D4B5 File Offset: 0x0012B8B5
	public override string ToString()
	{
		return string.Format("lobby '{0}'[{1}]", this.Name, this.Type);
	}

	// Token: 0x04002594 RID: 9620
	public string Name;

	// Token: 0x04002595 RID: 9621
	public LobbyType Type;

	// Token: 0x04002596 RID: 9622
	public static readonly TypedLobby Default = new TypedLobby();
}
