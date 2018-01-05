using System;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

// Token: 0x02000416 RID: 1046
internal class TypedDemoHub : Hub
{
	// Token: 0x060025F9 RID: 9721 RVA: 0x000BB548 File Offset: 0x000B9948
	public TypedDemoHub() : base("typeddemohub")
	{
		base.On("Echo", new OnMethodCallCallbackDelegate(this.Echo));
	}

	// Token: 0x060025FA RID: 9722 RVA: 0x000BB582 File Offset: 0x000B9982
	private void Echo(Hub hub, MethodCallMessage methodCall)
	{
		this.typedEchoClientResult = string.Format("{0} #{1} triggered!", methodCall.Arguments[0], methodCall.Arguments[1]);
	}

	// Token: 0x060025FB RID: 9723 RVA: 0x000BB5A4 File Offset: 0x000B99A4
	public void Echo(string msg)
	{
		base.Call("echo", new OnMethodResultDelegate(this.OnEcho_Done), new object[]
		{
			msg
		});
	}

	// Token: 0x060025FC RID: 9724 RVA: 0x000BB5C8 File Offset: 0x000B99C8
	private void OnEcho_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.typedEchoResult = "TypedDemoHub.Echo(string message) invoked!";
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x000BB5D8 File Offset: 0x000B99D8
	public void Draw()
	{
		GUILayout.Label("Typed callback", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label(this.typedEchoResult, new GUILayoutOption[0]);
		GUILayout.Label(this.typedEchoClientResult, new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
	}

	// Token: 0x04001308 RID: 4872
	private string typedEchoResult = string.Empty;

	// Token: 0x04001309 RID: 4873
	private string typedEchoClientResult = string.Empty;
}
