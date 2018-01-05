// Decompiled with JetBrains decompiler
// Type: CullArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CullArea : MonoBehaviour
{
    public readonly byte FIRST_GROUP_ID = 1;
    public readonly int[] SUBDIVISION_FIRST_LEVEL_ORDER = new int[4]
    {
    0,
    1,
    1,
    1
    };
    public readonly int[] SUBDIVISION_SECOND_LEVEL_ORDER = new int[8]
    {
    0,
    2,
    1,
    2,
    0,
    2,
    1,
    2
    };
    public readonly int[] SUBDIVISION_THIRD_LEVEL_ORDER = new int[12]
    {
    0,
    3,
    2,
    3,
    1,
    3,
    2,
    3,
    1,
    3,
    2,
    3
    };
    public Vector2 Size = new Vector2(25f, 25f);
    public Vector2[] Subdivisions = new Vector2[3];
    public bool YIsUpAxis = true;
    private const int MAX_NUMBER_OF_ALLOWED_CELLS = 250;
    public const int MAX_NUMBER_OF_SUBDIVISIONS = 3;
    public Vector2 Center;
    public int NumberOfSubdivisions;
    public bool RecreateCellHierarchy;
    private byte idCounter;

    public int CellCount { get; private set; }

    public CellTree CellTree { get; private set; }

    public Dictionary<int, GameObject> Map { get; private set; }

    private void Awake()
    {
        this.idCounter = this.FIRST_GROUP_ID;
        this.CreateCellHierarchy();
    }

    public void OnDrawGizmos()
    {
        this.idCounter = this.FIRST_GROUP_ID;
        if (this.RecreateCellHierarchy)
            this.CreateCellHierarchy();
        this.DrawCells();
    }

    private void CreateCellHierarchy()
    {
        if (!this.IsCellCountAllowed())
        {
            if (Debug.isDebugBuild)
            {
                Debug.LogError((object)("There are too many cells created by your subdivision options. Maximum allowed number of cells is " + (object)(250 - (int)this.FIRST_GROUP_ID) + ". Current number of cells is " + (object)this.CellCount + "."));
                return;
            }
            Application.Quit();
        }
        CellTreeNode cellTreeNode = new CellTreeNode(this.idCounter++, CellTreeNode.ENodeType.Root, (CellTreeNode)null);
        if (this.YIsUpAxis)
        {
            this.Center = new Vector2(this.transform.position.x, this.transform.position.y);
            this.Size = new Vector2(this.transform.localScale.x, this.transform.localScale.y);
            cellTreeNode.Center = new Vector3(this.Center.x, this.Center.y, 0.0f);
            cellTreeNode.Size = new Vector3(this.Size.x, this.Size.y, 0.0f);
            cellTreeNode.TopLeft = new Vector3(this.Center.x - this.Size.x / 2f, this.Center.y - this.Size.y / 2f, 0.0f);
            cellTreeNode.BottomRight = new Vector3(this.Center.x + this.Size.x / 2f, this.Center.y + this.Size.y / 2f, 0.0f);
        }
        else
        {
            this.Center = new Vector2(this.transform.position.x, this.transform.position.z);
            this.Size = new Vector2(this.transform.localScale.x, this.transform.localScale.z);
            cellTreeNode.Center = new Vector3(this.Center.x, 0.0f, this.Center.y);
            cellTreeNode.Size = new Vector3(this.Size.x, 0.0f, this.Size.y);
            cellTreeNode.TopLeft = new Vector3(this.Center.x - this.Size.x / 2f, 0.0f, this.Center.y - this.Size.y / 2f);
            cellTreeNode.BottomRight = new Vector3(this.Center.x + this.Size.x / 2f, 0.0f, this.Center.y + this.Size.y / 2f);
        }
        this.CreateChildCells(cellTreeNode, 1);
        this.CellTree = new CellTree(cellTreeNode);
        this.RecreateCellHierarchy = false;
    }

    private void CreateChildCells(CellTreeNode parent, int cellLevelInHierarchy)
    {
        if (cellLevelInHierarchy > this.NumberOfSubdivisions)
            return;
        int x1 = (int)this.Subdivisions[cellLevelInHierarchy - 1].x;
        int y1 = (int)this.Subdivisions[cellLevelInHierarchy - 1].y;
        float num1 = parent.Center.x - parent.Size.x / 2f;
        float x2 = parent.Size.x / (float)x1;
        for (int index1 = 0; index1 < x1; ++index1)
        {
            for (int index2 = 0; index2 < y1; ++index2)
            {
                float x3 = (float)((double)num1 + (double)index1 * (double)x2 + (double)x2 / 2.0);
                CellTreeNode cellTreeNode = new CellTreeNode(this.idCounter++, this.NumberOfSubdivisions != cellLevelInHierarchy ? CellTreeNode.ENodeType.Node : CellTreeNode.ENodeType.Leaf, parent);
                if (this.YIsUpAxis)
                {
                    float num2 = parent.Center.y - parent.Size.y / 2f;
                    float y2 = parent.Size.y / (float)y1;
                    float y3 = (float)((double)num2 + (double)index2 * (double)y2 + (double)y2 / 2.0);
                    cellTreeNode.Center = new Vector3(x3, y3, 0.0f);
                    cellTreeNode.Size = new Vector3(x2, y2, 0.0f);
                    cellTreeNode.TopLeft = new Vector3(x3 - x2 / 2f, y3 - y2 / 2f, 0.0f);
                    cellTreeNode.BottomRight = new Vector3(x3 + x2 / 2f, y3 + y2 / 2f, 0.0f);
                }
                else
                {
                    float num2 = parent.Center.z - parent.Size.z / 2f;
                    float z1 = parent.Size.z / (float)y1;
                    float z2 = (float)((double)num2 + (double)index2 * (double)z1 + (double)z1 / 2.0);
                    cellTreeNode.Center = new Vector3(x3, 0.0f, z2);
                    cellTreeNode.Size = new Vector3(x2, 0.0f, z1);
                    cellTreeNode.TopLeft = new Vector3(x3 - x2 / 2f, 0.0f, z2 - z1 / 2f);
                    cellTreeNode.BottomRight = new Vector3(x3 + x2 / 2f, 0.0f, z2 + z1 / 2f);
                }
                parent.AddChild(cellTreeNode);
                this.CreateChildCells(cellTreeNode, cellLevelInHierarchy + 1);
            }
        }
    }

    private void DrawCells()
    {
        if (this.CellTree != null && this.CellTree.RootNode != null)
            this.CellTree.RootNode.Draw();
        else
            this.RecreateCellHierarchy = true;
    }

    private bool IsCellCountAllowed()
    {
        int num1 = 1;
        int num2 = 1;
        foreach (Vector2 subdivision in this.Subdivisions)
        {
            num1 *= (int)subdivision.x;
            num2 *= (int)subdivision.y;
        }
        this.CellCount = num1 * num2;
        return this.CellCount <= 250 - (int)this.FIRST_GROUP_ID;
    }

    public List<byte> GetActiveCells(Vector3 position)
    {
        List<byte> activeCells = new List<byte>(0);
        this.CellTree.RootNode.GetActiveCells(activeCells, this.YIsUpAxis, position);
        return activeCells;
    }
}
