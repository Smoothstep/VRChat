using System;

// Token: 0x02000786 RID: 1926
public class CellTree
{
	// Token: 0x06003EA4 RID: 16036 RVA: 0x0013BCB5 File Offset: 0x0013A0B5
	public CellTree()
	{
	}

	// Token: 0x06003EA5 RID: 16037 RVA: 0x0013BCBD File Offset: 0x0013A0BD
	public CellTree(CellTreeNode root)
	{
		this.RootNode = root;
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x06003EA6 RID: 16038 RVA: 0x0013BCCC File Offset: 0x0013A0CC
	// (set) Token: 0x06003EA7 RID: 16039 RVA: 0x0013BCD4 File Offset: 0x0013A0D4
	public CellTreeNode RootNode { get; private set; }
}
