using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrimitivesPro.MeshEditor
{
	// Token: 0x0200086C RID: 2156
	internal class UndoSystem
	{
		// Token: 0x060042E0 RID: 17120 RVA: 0x00156F16 File Offset: 0x00155316
		public UndoSystem()
		{
			this.restorePoints = new LinkedList<UndoSystem.RestorePoint>();
			this.changedVertex = new Dictionary<int, Vector3>();
			this.current = this.restorePoints.AddLast(new UndoSystem.RestorePoint(this.changedVertex));
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x00156F50 File Offset: 0x00155350
		public void CreateRestorePoint()
		{
			if (this.changedVertex.Count > 0)
			{
				if (this.current != this.restorePoints.First && this.current != this.restorePoints.Last)
				{
					while (this.current != this.restorePoints.Last)
					{
						this.restorePoints.RemoveLast();
					}
				}
				this.current = this.restorePoints.AddLast(new UndoSystem.RestorePoint(this.changedVertex));
				this.changedVertex.Clear();
			}
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x00156FE8 File Offset: 0x001553E8
		public void OnVertexChanged(int index, Vector3 delta)
		{
			if (delta.sqrMagnitude > Mathf.Epsilon)
			{
				if (!this.changedVertex.ContainsKey(index))
				{
					this.changedVertex[index] = Vector3.zero;
				}
				Dictionary<int, Vector3> dictionary;
				(dictionary = this.changedVertex)[index] = dictionary[index] + delta;
			}
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x00157045 File Offset: 0x00155445
		public Dictionary<int, Vector3> RedoVertex()
		{
			if (this.current.Next != null)
			{
				this.current = this.current.Next;
				return this.current.Value.Data;
			}
			return null;
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x0015707C File Offset: 0x0015547C
		public Dictionary<int, Vector3> UndoVertex()
		{
			if (this.current != this.restorePoints.First)
			{
				LinkedListNode<UndoSystem.RestorePoint> linkedListNode = this.current;
				this.current = this.current.Previous;
				return linkedListNode.Value.Data;
			}
			return null;
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x001570C4 File Offset: 0x001554C4
		public bool CanUndo()
		{
			return this.current != this.restorePoints.First;
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x001570DC File Offset: 0x001554DC
		public bool CanRedo()
		{
			return this.current.Next != null;
		}

		// Token: 0x04002B6F RID: 11119
		private readonly LinkedList<UndoSystem.RestorePoint> restorePoints;

		// Token: 0x04002B70 RID: 11120
		private readonly Dictionary<int, Vector3> changedVertex;

		// Token: 0x04002B71 RID: 11121
		private LinkedListNode<UndoSystem.RestorePoint> current;

		// Token: 0x0200086D RID: 2157
		private class RestorePoint
		{
			// Token: 0x060042E7 RID: 17127 RVA: 0x001570EF File Offset: 0x001554EF
			public RestorePoint(Dictionary<int, Vector3> changedData)
			{
				this.Data = new Dictionary<int, Vector3>(changedData);
			}

			// Token: 0x04002B72 RID: 11122
			public readonly Dictionary<int, Vector3> Data;
		}
	}
}
