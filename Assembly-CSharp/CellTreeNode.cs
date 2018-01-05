using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000787 RID: 1927
public class CellTreeNode
{
	// Token: 0x06003EA8 RID: 16040 RVA: 0x0013BCDD File Offset: 0x0013A0DD
	public CellTreeNode()
	{
	}

	// Token: 0x06003EA9 RID: 16041 RVA: 0x0013BCE5 File Offset: 0x0013A0E5
	public CellTreeNode(byte id, CellTreeNode.ENodeType nodeType, CellTreeNode parent)
	{
		this.Id = id;
		this.NodeType = nodeType;
		this.Parent = parent;
	}

	// Token: 0x06003EAA RID: 16042 RVA: 0x0013BD02 File Offset: 0x0013A102
	public void AddChild(CellTreeNode child)
	{
		if (this.Childs == null)
		{
			this.Childs = new List<CellTreeNode>(1);
		}
		this.Childs.Add(child);
	}

	// Token: 0x06003EAB RID: 16043 RVA: 0x0013BD27 File Offset: 0x0013A127
	public void Draw()
	{
	}

	// Token: 0x06003EAC RID: 16044 RVA: 0x0013BD2C File Offset: 0x0013A12C
	public void GetActiveCells(List<byte> activeCells, bool yIsUpAxis, Vector3 position)
	{
		if (this.NodeType != CellTreeNode.ENodeType.Leaf)
		{
			foreach (CellTreeNode cellTreeNode in this.Childs)
			{
				cellTreeNode.GetActiveCells(activeCells, yIsUpAxis, position);
			}
		}
		else if (this.IsPointNearCell(yIsUpAxis, position))
		{
			if (this.IsPointInsideCell(yIsUpAxis, position))
			{
				activeCells.Insert(0, this.Id);
				for (CellTreeNode parent = this.Parent; parent != null; parent = parent.Parent)
				{
					activeCells.Insert(0, parent.Id);
				}
			}
			else
			{
				activeCells.Add(this.Id);
			}
		}
	}

	// Token: 0x06003EAD RID: 16045 RVA: 0x0013BDFC File Offset: 0x0013A1FC
	public bool IsPointInsideCell(bool yIsUpAxis, Vector3 point)
	{
		if (point.x < this.TopLeft.x || point.x > this.BottomRight.x)
		{
			return false;
		}
		if (yIsUpAxis)
		{
			if (point.y >= this.TopLeft.y && point.y <= this.BottomRight.y)
			{
				return true;
			}
		}
		else if (point.z >= this.TopLeft.z && point.z <= this.BottomRight.z)
		{
			return true;
		}
		return false;
	}

	// Token: 0x06003EAE RID: 16046 RVA: 0x0013BEA8 File Offset: 0x0013A2A8
	public bool IsPointNearCell(bool yIsUpAxis, Vector3 point)
	{
		if (this.maxDistance == 0f)
		{
			this.maxDistance = (this.Size.x + this.Size.y + this.Size.z) / 2f;
		}
		return (point - this.Center).sqrMagnitude <= this.maxDistance * this.maxDistance;
	}

	// Token: 0x04002742 RID: 10050
	public byte Id;

	// Token: 0x04002743 RID: 10051
	public Vector3 Center;

	// Token: 0x04002744 RID: 10052
	public Vector3 Size;

	// Token: 0x04002745 RID: 10053
	public Vector3 TopLeft;

	// Token: 0x04002746 RID: 10054
	public Vector3 BottomRight;

	// Token: 0x04002747 RID: 10055
	public CellTreeNode.ENodeType NodeType;

	// Token: 0x04002748 RID: 10056
	public CellTreeNode Parent;

	// Token: 0x04002749 RID: 10057
	public List<CellTreeNode> Childs;

	// Token: 0x0400274A RID: 10058
	private float maxDistance;

	// Token: 0x02000788 RID: 1928
	public enum ENodeType
	{
		// Token: 0x0400274C RID: 10060
		Root,
		// Token: 0x0400274D RID: 10061
		Node,
		// Token: 0x0400274E RID: 10062
		Leaf
	}
}
