using System;

namespace LitJson
{
	// Token: 0x020003F6 RID: 1014
	public class JsonException : Exception
	{
		// Token: 0x06002492 RID: 9362 RVA: 0x000B5006 File Offset: 0x000B3406
		public JsonException()
		{
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000B500E File Offset: 0x000B340E
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000B5026 File Offset: 0x000B3426
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000B503F File Offset: 0x000B343F
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000B5058 File Offset: 0x000B3458
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000B5072 File Offset: 0x000B3472
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x000B507B File Offset: 0x000B347B
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
