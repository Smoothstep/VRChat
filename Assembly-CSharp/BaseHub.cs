using System;
using System.Collections.Generic;
using BestHTTP.Examples;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

// Token: 0x02000411 RID: 1041
internal class BaseHub : Hub
{
	// Token: 0x060025D1 RID: 9681 RVA: 0x000BA7A4 File Offset: 0x000B8BA4
	public BaseHub(string name, string title) : base(name)
	{
		this.Title = title;
		base.On("joined", new OnMethodCallCallbackDelegate(this.Joined));
		base.On("rejoined", new OnMethodCallCallbackDelegate(this.Rejoined));
		base.On("left", new OnMethodCallCallbackDelegate(this.Left));
		base.On("invoked", new OnMethodCallCallbackDelegate(this.Invoked));
	}

	// Token: 0x060025D2 RID: 9682 RVA: 0x000BA828 File Offset: 0x000B8C28
	private void Joined(Hub hub, MethodCallMessage methodCall)
	{
		Dictionary<string, object> dictionary = methodCall.Arguments[2] as Dictionary<string, object>;
		this.messages.Add(string.Format("{0} joined at {1}\n\tIsAuthenticated: {2} IsAdmin: {3} UserName: {4}", new object[]
		{
			methodCall.Arguments[0],
			methodCall.Arguments[1],
			dictionary["IsAuthenticated"],
			dictionary["IsAdmin"],
			dictionary["UserName"]
		}));
	}

	// Token: 0x060025D3 RID: 9683 RVA: 0x000BA89E File Offset: 0x000B8C9E
	private void Rejoined(Hub hub, MethodCallMessage methodCall)
	{
		this.messages.Add(string.Format("{0} reconnected at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
	}

	// Token: 0x060025D4 RID: 9684 RVA: 0x000BA8C5 File Offset: 0x000B8CC5
	private void Left(Hub hub, MethodCallMessage methodCall)
	{
		this.messages.Add(string.Format("{0} left at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
	}

	// Token: 0x060025D5 RID: 9685 RVA: 0x000BA8EC File Offset: 0x000B8CEC
	private void Invoked(Hub hub, MethodCallMessage methodCall)
	{
		this.messages.Add(string.Format("{0} invoked hub method at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
	}

	// Token: 0x060025D6 RID: 9686 RVA: 0x000BA913 File Offset: 0x000B8D13
	public void InvokedFromClient()
	{
		base.Call("invokedFromClient", new OnMethodResultDelegate(this.OnInvoked), new OnMethodFailedDelegate(this.OnInvokeFailed), new object[0]);
	}

	// Token: 0x060025D7 RID: 9687 RVA: 0x000BA93F File Offset: 0x000B8D3F
	private void OnInvoked(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		Debug.Log(hub.Name + " invokedFromClient success!");
	}

	// Token: 0x060025D8 RID: 9688 RVA: 0x000BA956 File Offset: 0x000B8D56
	private void OnInvokeFailed(Hub hub, ClientMessage originalMessage, FailureMessage result)
	{
		Debug.LogWarning(hub.Name + " " + result.ErrorMessage);
	}

	// Token: 0x060025D9 RID: 9689 RVA: 0x000BA974 File Offset: 0x000B8D74
	public void Draw()
	{
		GUILayout.Label(this.Title, new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		this.messages.Draw((float)(Screen.width - 20), 100f);
		GUILayout.EndHorizontal();
	}

	// Token: 0x040012EC RID: 4844
	private string Title;

	// Token: 0x040012ED RID: 4845
	private GUIMessageList messages = new GUIMessageList();
}
