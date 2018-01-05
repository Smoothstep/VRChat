using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

// Token: 0x0200075C RID: 1884
public class WebRpcResponse
{
	// Token: 0x06003CE7 RID: 15591 RVA: 0x00133F10 File Offset: 0x00132310
	public WebRpcResponse(OperationResponse response)
	{
		object obj;
		response.Parameters.TryGetValue(209, out obj);
		this.Name = (obj as string);
		response.Parameters.TryGetValue(207, out obj);
		this.ReturnCode = ((obj == null) ? -1 : ((int)((byte)obj)));
		response.Parameters.TryGetValue(208, out obj);
		this.Parameters = (obj as Dictionary<string, object>);
		response.Parameters.TryGetValue(206, out obj);
		this.DebugMessage = (obj as string);
	}

	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x06003CE8 RID: 15592 RVA: 0x00133FAB File Offset: 0x001323AB
	// (set) Token: 0x06003CE9 RID: 15593 RVA: 0x00133FB3 File Offset: 0x001323B3
	public string Name { get; private set; }

	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x06003CEA RID: 15594 RVA: 0x00133FBC File Offset: 0x001323BC
	// (set) Token: 0x06003CEB RID: 15595 RVA: 0x00133FC4 File Offset: 0x001323C4
	public int ReturnCode { get; private set; }

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x06003CEC RID: 15596 RVA: 0x00133FCD File Offset: 0x001323CD
	// (set) Token: 0x06003CED RID: 15597 RVA: 0x00133FD5 File Offset: 0x001323D5
	public string DebugMessage { get; private set; }

	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x06003CEE RID: 15598 RVA: 0x00133FDE File Offset: 0x001323DE
	// (set) Token: 0x06003CEF RID: 15599 RVA: 0x00133FE6 File Offset: 0x001323E6
	public Dictionary<string, object> Parameters { get; private set; }

	// Token: 0x06003CF0 RID: 15600 RVA: 0x00133FEF File Offset: 0x001323EF
	public string ToStringFull()
	{
		return string.Format("{0}={2}: {1} \"{3}\"", new object[]
		{
			this.Name,
			SupportClass.DictionaryToString(this.Parameters),
			this.ReturnCode,
			this.DebugMessage
		});
	}
}
