using System;
using BestHTTP.Examples;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

// Token: 0x02000417 RID: 1047
internal class DemoHub : Hub
{
	// Token: 0x060025FE RID: 9726 RVA: 0x000BB64C File Offset: 0x000B9A4C
	public DemoHub() : base("demo")
	{
		base.On("invoke", new OnMethodCallCallbackDelegate(this.Invoke));
		base.On("signal", new OnMethodCallCallbackDelegate(this.Signal));
		base.On("groupAdded", new OnMethodCallCallbackDelegate(this.GroupAdded));
		base.On("fromArbitraryCode", new OnMethodCallCallbackDelegate(this.FromArbitraryCode));
	}

	// Token: 0x060025FF RID: 9727 RVA: 0x000BB788 File Offset: 0x000B9B88
	public void ReportProgress(string arg)
	{
		base.Call("reportProgress", new OnMethodResultDelegate(this.OnLongRunningJob_Done), null, new OnMethodProgressDelegate(this.OnLongRunningJob_Progress), new object[]
		{
			arg
		});
	}

	// Token: 0x06002600 RID: 9728 RVA: 0x000BB7C4 File Offset: 0x000B9BC4
	public void OnLongRunningJob_Progress(Hub hub, ClientMessage originialMessage, ProgressMessage progress)
	{
		this.longRunningJobProgress = (float)progress.Progress;
		this.longRunningJobStatus = progress.Progress.ToString() + "%";
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x000BB802 File Offset: 0x000B9C02
	public void OnLongRunningJob_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.longRunningJobStatus = result.ReturnValue.ToString();
		this.MultipleCalls();
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x000BB81B File Offset: 0x000B9C1B
	public void MultipleCalls()
	{
		base.Call("multipleCalls", new object[0]);
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x000BB82F File Offset: 0x000B9C2F
	public void DynamicTask()
	{
		base.Call("dynamicTask", new OnMethodResultDelegate(this.OnDynamicTask_Done), new OnMethodFailedDelegate(this.OnDynamicTask_Failed), new object[0]);
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x000BB85B File Offset: 0x000B9C5B
	private void OnDynamicTask_Failed(Hub hub, ClientMessage originalMessage, FailureMessage result)
	{
		this.dynamicTaskResult = string.Format("The dynamic task failed :( {0}", result.ErrorMessage);
	}

	// Token: 0x06002605 RID: 9733 RVA: 0x000BB873 File Offset: 0x000B9C73
	private void OnDynamicTask_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.dynamicTaskResult = string.Format("The dynamic task! {0}", result.ReturnValue);
	}

	// Token: 0x06002606 RID: 9734 RVA: 0x000BB88B File Offset: 0x000B9C8B
	public void AddToGroups()
	{
		base.Call("addToGroups", new object[0]);
	}

	// Token: 0x06002607 RID: 9735 RVA: 0x000BB89F File Offset: 0x000B9C9F
	public void GetValue()
	{
		base.Call("getValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.genericTaskResult = string.Format("The value is {0} after 5 seconds", result.ReturnValue);
		}, new object[0]);
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x000BB8BF File Offset: 0x000B9CBF
	public void TaskWithException()
	{
		base.Call("taskWithException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
		{
			this.taskWithExceptionResult = string.Format("Error: {0}", error.ErrorMessage);
		}, new object[0]);
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x000BB8E0 File Offset: 0x000B9CE0
	public void GenericTaskWithException()
	{
		base.Call("genericTaskWithException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
		{
			this.genericTaskWithExceptionResult = string.Format("Error: {0}", error.ErrorMessage);
		}, new object[0]);
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x000BB901 File Offset: 0x000B9D01
	public void SynchronousException()
	{
		base.Call("synchronousException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
		{
			this.synchronousExceptionResult = string.Format("Error: {0}", error.ErrorMessage);
		}, new object[0]);
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x000BB922 File Offset: 0x000B9D22
	public void PassingDynamicComplex(object person)
	{
		base.Call("passingDynamicComplex", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.invokingHubMethodWithDynamicResult = string.Format("The person's age is {0}", result.ReturnValue);
		}, new object[]
		{
			person
		});
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x000BB946 File Offset: 0x000B9D46
	public void SimpleArray(int[] array)
	{
		base.Call("simpleArray", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.simpleArrayResult = "Simple array works!";
		}, new object[]
		{
			array
		});
	}

	// Token: 0x0600260D RID: 9741 RVA: 0x000BB96A File Offset: 0x000B9D6A
	public void ComplexType(object person)
	{
		base.Call("complexType", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.complexTypeResult = string.Format("Complex Type -> {0}", ((IHub)this).Connection.JsonEncoder.Encode(base.State["person"]));
		}, new object[]
		{
			person
		});
	}

	// Token: 0x0600260E RID: 9742 RVA: 0x000BB98E File Offset: 0x000B9D8E
	public void ComplexArray(object[] complexArray)
	{
		base.Call("ComplexArray", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.complexArrayResult = "Complex Array Works!";
		}, new object[]
		{
			complexArray
		});
	}

	// Token: 0x0600260F RID: 9743 RVA: 0x000BB9B2 File Offset: 0x000B9DB2
	public void Overload()
	{
		base.Call("Overload", new OnMethodResultDelegate(this.OnVoidOverload_Done), new object[0]);
	}

	// Token: 0x06002610 RID: 9744 RVA: 0x000BB9D2 File Offset: 0x000B9DD2
	private void OnVoidOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.voidOverloadResult = "Void Overload called";
		this.Overload(101);
	}

	// Token: 0x06002611 RID: 9745 RVA: 0x000BB9E7 File Offset: 0x000B9DE7
	public void Overload(int number)
	{
		base.Call("Overload", new OnMethodResultDelegate(this.OnIntOverload_Done), new object[]
		{
			number
		});
	}

	// Token: 0x06002612 RID: 9746 RVA: 0x000BBA10 File Offset: 0x000B9E10
	private void OnIntOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.intOverloadResult = string.Format("Overload with return value called => {0}", result.ReturnValue.ToString());
	}

	// Token: 0x06002613 RID: 9747 RVA: 0x000BBA2D File Offset: 0x000B9E2D
	public void ReadStateValue()
	{
		base.Call("readStateValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.readStateResult = string.Format("Read some state! => {0}", result.ReturnValue);
		}, new object[0]);
	}

	// Token: 0x06002614 RID: 9748 RVA: 0x000BBA4D File Offset: 0x000B9E4D
	public void PlainTask()
	{
		base.Call("plainTask", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.plainTaskResult = "Plain Task Result";
		}, new object[0]);
	}

	// Token: 0x06002615 RID: 9749 RVA: 0x000BBA6D File Offset: 0x000B9E6D
	public void GenericTaskWithContinueWith()
	{
		base.Call("genericTaskWithContinueWith", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.genericTaskWithContinueWithResult = result.ReturnValue.ToString();
		}, new object[0]);
	}

	// Token: 0x06002616 RID: 9750 RVA: 0x000BBA8D File Offset: 0x000B9E8D
	private void FromArbitraryCode(Hub hub, MethodCallMessage methodCall)
	{
		this.fromArbitraryCodeResult = (methodCall.Arguments[0] as string);
	}

	// Token: 0x06002617 RID: 9751 RVA: 0x000BBAA2 File Offset: 0x000B9EA2
	private void GroupAdded(Hub hub, MethodCallMessage methodCall)
	{
		if (!string.IsNullOrEmpty(this.groupAddedResult))
		{
			this.groupAddedResult = "Group Already Added!";
		}
		else
		{
			this.groupAddedResult = "Group Added!";
		}
	}

	// Token: 0x06002618 RID: 9752 RVA: 0x000BBACF File Offset: 0x000B9ECF
	private void Signal(Hub hub, MethodCallMessage methodCall)
	{
		this.dynamicTaskResult = string.Format("The dynamic task! {0}", methodCall.Arguments[0]);
	}

	// Token: 0x06002619 RID: 9753 RVA: 0x000BBAE9 File Offset: 0x000B9EE9
	private void Invoke(Hub hub, MethodCallMessage methodCall)
	{
		this.invokeResults.Add(string.Format("{0} client state index -> {1}", methodCall.Arguments[0], base.State["index"]));
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x000BBB18 File Offset: 0x000B9F18
	public void Draw()
	{
		GUILayout.Label("Arbitrary Code", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(string.Format("Sending {0} from arbitrary code without the hub itself!", this.fromArbitraryCodeResult), new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Group Added", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.groupAddedResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Dynamic Task", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.dynamicTaskResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Report Progress", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label(this.longRunningJobStatus, new GUILayoutOption[0]);
		GUILayout.HorizontalSlider(this.longRunningJobProgress, 0f, 100f, new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Generic Task", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.genericTaskResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Task With Exception", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.taskWithExceptionResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Generic Task With Exception", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.genericTaskWithExceptionResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Synchronous Exception", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.synchronousExceptionResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Invoking hub method with dynamic", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.invokingHubMethodWithDynamicResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Simple Array", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.simpleArrayResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Complex Type", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.complexTypeResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Complex Array", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.complexArrayResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Overloads", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label(this.voidOverloadResult, new GUILayoutOption[0]);
		GUILayout.Label(this.intOverloadResult, new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Read State Value", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.readStateResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Plain Task", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.plainTaskResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Generic Task With ContinueWith", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		GUILayout.Label(this.genericTaskWithContinueWithResult, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Message Pump", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(20f);
		this.invokeResults.Draw((float)(Screen.width - 40), 270f);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
	}

	// Token: 0x0400130A RID: 4874
	private float longRunningJobProgress;

	// Token: 0x0400130B RID: 4875
	private string longRunningJobStatus = "Not Started!";

	// Token: 0x0400130C RID: 4876
	private string fromArbitraryCodeResult = string.Empty;

	// Token: 0x0400130D RID: 4877
	private string groupAddedResult = string.Empty;

	// Token: 0x0400130E RID: 4878
	private string dynamicTaskResult = string.Empty;

	// Token: 0x0400130F RID: 4879
	private string genericTaskResult = string.Empty;

	// Token: 0x04001310 RID: 4880
	private string taskWithExceptionResult = string.Empty;

	// Token: 0x04001311 RID: 4881
	private string genericTaskWithExceptionResult = string.Empty;

	// Token: 0x04001312 RID: 4882
	private string synchronousExceptionResult = string.Empty;

	// Token: 0x04001313 RID: 4883
	private string invokingHubMethodWithDynamicResult = string.Empty;

	// Token: 0x04001314 RID: 4884
	private string simpleArrayResult = string.Empty;

	// Token: 0x04001315 RID: 4885
	private string complexTypeResult = string.Empty;

	// Token: 0x04001316 RID: 4886
	private string complexArrayResult = string.Empty;

	// Token: 0x04001317 RID: 4887
	private string voidOverloadResult = string.Empty;

	// Token: 0x04001318 RID: 4888
	private string intOverloadResult = string.Empty;

	// Token: 0x04001319 RID: 4889
	private string readStateResult = string.Empty;

	// Token: 0x0400131A RID: 4890
	private string plainTaskResult = string.Empty;

	// Token: 0x0400131B RID: 4891
	private string genericTaskWithContinueWithResult = string.Empty;

	// Token: 0x0400131C RID: 4892
	private GUIMessageList invokeResults = new GUIMessageList();
}
