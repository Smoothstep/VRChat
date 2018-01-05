using System;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public sealed class SampleDescriptor
{
	// Token: 0x06002592 RID: 9618 RVA: 0x000B9533 File Offset: 0x000B7933
	public SampleDescriptor(Type type, string displayName, string description, string codeBlock)
	{
		this.Type = type;
		this.DisplayName = displayName;
		this.Description = description;
		this.CodeBlock = codeBlock;
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06002593 RID: 9619 RVA: 0x000B9558 File Offset: 0x000B7958
	// (set) Token: 0x06002594 RID: 9620 RVA: 0x000B9560 File Offset: 0x000B7960
	public bool IsLabel { get; set; }

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06002595 RID: 9621 RVA: 0x000B9569 File Offset: 0x000B7969
	// (set) Token: 0x06002596 RID: 9622 RVA: 0x000B9571 File Offset: 0x000B7971
	public Type Type { get; set; }

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06002597 RID: 9623 RVA: 0x000B957A File Offset: 0x000B797A
	// (set) Token: 0x06002598 RID: 9624 RVA: 0x000B9582 File Offset: 0x000B7982
	public string DisplayName { get; set; }

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06002599 RID: 9625 RVA: 0x000B958B File Offset: 0x000B798B
	// (set) Token: 0x0600259A RID: 9626 RVA: 0x000B9593 File Offset: 0x000B7993
	public string Description { get; set; }

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x0600259B RID: 9627 RVA: 0x000B959C File Offset: 0x000B799C
	// (set) Token: 0x0600259C RID: 9628 RVA: 0x000B95A4 File Offset: 0x000B79A4
	public string CodeBlock { get; set; }

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x0600259D RID: 9629 RVA: 0x000B95AD File Offset: 0x000B79AD
	// (set) Token: 0x0600259E RID: 9630 RVA: 0x000B95B5 File Offset: 0x000B79B5
	public bool IsSelected { get; set; }

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x0600259F RID: 9631 RVA: 0x000B95BE File Offset: 0x000B79BE
	// (set) Token: 0x060025A0 RID: 9632 RVA: 0x000B95C6 File Offset: 0x000B79C6
	public GameObject UnityObject { get; set; }

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x060025A1 RID: 9633 RVA: 0x000B95CF File Offset: 0x000B79CF
	public bool IsRunning
	{
		get
		{
			return this.UnityObject != null;
		}
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x000B95DD File Offset: 0x000B79DD
	public void CreateUnityObject()
	{
		if (this.UnityObject != null)
		{
			return;
		}
		this.UnityObject = new GameObject(this.DisplayName);
		this.UnityObject.AddComponent(this.Type);
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x000B9614 File Offset: 0x000B7A14
	public void DestroyUnityObject()
	{
		if (this.UnityObject != null)
		{
			UnityEngine.Object.Destroy(this.UnityObject);
			this.UnityObject = null;
		}
	}
}
